using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

using GaussianCommonLibrary.Models;

using GaussianWPF.Models;

using GaussianWPFLibrary.DataAccess;
using GaussianWPFLibrary.EventModels;
using GaussianWPFLibrary.Models;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls.BaseMethods;

/// <summary>
/// Interaction logic for BaseMethodsIndexControl.xaml
/// A WPF UserControl that displays a list of Base Methods and provides CRUD operations.
/// </summary>
/// <remarks>
/// This control implements the INotifyPropertyChanged interface to support data binding
/// and raises events to notify parent controls of user actions.
/// </remarks>
public partial class BaseMethodsIndexControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<BaseMethodsIndexControl> _logger;
	private readonly ILoggedInUserModel _loggedInUser;
	private readonly IApiHelper _apiHelper;
	private readonly IBaseMethodsEndpoint _baseMethodsEndpoint;
	private readonly IMethodFamiliesEndpoint _methodFamiliesEndpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodsIndexControl"/> class.
	/// </summary>
	/// <param name="logger">Logger instance for diagnostic logging.</param>
	/// <param name="loggedInUser">Model representing the currently logged-in user.</param>
	/// <param name="apiHelper">Helper for API communication.</param>
	/// <param name="baseMethodsEndpoint">Endpoint for accessing Base Methods data.</param>
	/// <param name="methodFamiliesEndpoint">Endpoint for accessing Method Families data.</param>
	public BaseMethodsIndexControl(ILogger<BaseMethodsIndexControl> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, IBaseMethodsEndpoint baseMethodsEndpoint, IMethodFamiliesEndpoint methodFamiliesEndpoint)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_baseMethodsEndpoint = baseMethodsEndpoint;
		_methodFamiliesEndpoint = methodFamiliesEndpoint;
		BaseMethodsList = [];

		InitializeComponent();
		DataContext = this;
		PropertyChanged += BaseMethodsIndexControl_PropertyChanged;
	}

	/// <summary>
	/// Event raised when a child control action is triggered.
	/// </summary>
	/// <remarks>
	/// This event notifies parent controls when the user initiates create, edit, details, or delete actions.
	/// The event data includes the action type and the ID of the affected item (if applicable).
	/// </remarks>
	public event EventHandler<ChildControlEventArgs<BaseMethodsIndexControl>>? ChildControlEvent;

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Gets or sets the collection of Base Methods to display.
	/// </summary>
	/// <value>
	/// An observable collection of <see cref="BaseMethodViewModel"/> instances.
	/// </value>
	public Collection<BaseMethodViewModel> BaseMethodsList
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(BaseMethodsList));
			}
		}
	}

	/// <summary>
	/// Gets or sets the error message to display to the user.
	/// </summary>
	/// <value>
	/// A string containing the error message, or <c>null</c> if there is no error.
	/// </value>
	public string? ErrorMessage
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(ErrorMessage));
			}
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether the error message should be visible.
	/// </summary>
	/// <value>
	/// <c>true</c> if the error message should be visible; otherwise, <c>false</c>.
	/// </value>
	public bool IsErrorVisible
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(IsErrorVisible));
			}
		}
	}

	/// <summary>
	/// Raises the <see cref="OnInitialized"/> event and loads Base Methods from the API.
	/// </summary>
	/// <param name="e">The event data.</param>
	/// <remarks>
	/// This method fetches all Base Methods from the baseMethodsEndpoint and populates the <see cref="BaseMethodsList"/>.
	/// If an error occurs during data retrieval, it sets the <see cref="ErrorMessage"/> property.
	/// </remarks>
	/// <exception cref="HttpIOException">Thrown when an HTTP communication error occurs.</exception>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} with {EventArgs}", nameof(BaseMethodsIndexControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);

		try
		{
			List<BaseMethodSimpleModel>? results = _baseMethodsEndpoint.GetAllSimpleAsync().Result;

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} returned {ResultsCount}", nameof(_baseMethodsEndpoint.GetAllSimpleAsync), results?.Count);
			}

			List<MethodFamilyRecord>? methodFamilies = _methodFamiliesEndpoint.GetListAsync().Result;

			if (results is not null && methodFamilies is not null && methodFamilies.Count > 0)
			{
				foreach (BaseMethodSimpleModel item in results)
				{
					MethodFamilyRecord methodFamily = methodFamilies.First(mf => mf.Id == item.MethodFamilyId);
					BaseMethodViewModel model = new(item, methodFamily, methodFamilies);
					BaseMethodsList.Add(model);
				}
			}
		}
		catch (HttpIOException ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {Method} had an error", nameof(BaseMethodsIndexControl), nameof(OnInitialized));
			}

			ErrorMessage = ex.Message;
		}
		catch (AggregateException ae)
		{
			ae.Handle(ex =>
			{
				if (ex is HttpIOException)
				{
					if (_logger.IsEnabled(LogLevel.Error))
					{
						_logger.LogError(ex, "{UserControl} {Method} had an error", nameof(BaseMethodsIndexControl), nameof(OnInitialized));
					}

					ErrorMessage = ex.Message;
					return true;
				}

				// Return false for any other exception types to rethrow them in a new AggregateException
				return false;
			});
		}
	}

	/// <summary>
	/// Raises the <see cref="PropertyChanged"/> event.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed. This parameter is automatically populated by the compiler.</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} with {PropertyName}", nameof(BaseMethodsIndexControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void CreateNewButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsIndexControl), nameof(CreateNewButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<BaseMethodsIndexControl>("create", null));
	}

	private void EditButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsIndexControl), nameof(EditButton_Click), sender, e);
		}

		Button? button = sender as Button;
		if (button is not null)
		{
			BaseMethodViewModel? itemToEdit = button.DataContext as BaseMethodViewModel;
			if (itemToEdit is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<BaseMethodsIndexControl>("edit", itemToEdit.Id));
			}
		}
	}

	private void DetailsButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsIndexControl), nameof(DetailsButton_Click), sender, e);
		}

		Button? button = sender as Button;
		if (button is not null)
		{
			BaseMethodViewModel? itemToView = button.DataContext as BaseMethodViewModel;
			if (itemToView is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<BaseMethodsIndexControl>("details", itemToView.Id));
			}
		}
	}

	private void DeleteButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsIndexControl), nameof(DeleteButton_Click), sender, e);
		}

		Button? button = sender as Button;
		if (button is not null)
		{
			BaseMethodViewModel? itemToDelete = button.DataContext as BaseMethodViewModel;
			if (itemToDelete is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<BaseMethodsIndexControl>("delete", itemToDelete.Id));
			}
		}
	}

	private void BaseMethodsIndexControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsIndexControl), nameof(BaseMethodsIndexControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}
	}
}
