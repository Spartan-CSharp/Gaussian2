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

namespace GaussianWPF.Controls.CalculationTypes;

/// <summary>
/// Interaction logic for CalculationTypesIndexControl.xaml
/// A WPF UserControl that displays a list of calculation types and provides CRUD operations.
/// </summary>
/// <remarks>
/// This control implements the INotifyPropertyChanged interface to support data binding
/// and raises events to notify parent controls of user actions.
/// </remarks>
public partial class CalculationTypesIndexControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<CalculationTypesIndexControl> _logger;
	private readonly ILoggedInUserModel _loggedInUser;
	private readonly IApiHelper _apiHelper;
	private readonly ICalculationTypesEndpoint _endpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="CalculationTypesIndexControl"/> class.
	/// </summary>
	/// <param name="logger">Logger instance for diagnostic logging.</param>
	/// <param name="loggedInUser">Model representing the currently logged-in user.</param>
	/// <param name="apiHelper">Helper for API communication.</param>
	/// <param name="endpoint">Endpoint for accessing calculation types data.</param>
	public CalculationTypesIndexControl(ILogger<CalculationTypesIndexControl> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, ICalculationTypesEndpoint endpoint)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_endpoint = endpoint;
		CalculationTypesList = [];

		InitializeComponent();
		DataContext = this;
		PropertyChanged += CalculationTypesIndexControl_PropertyChanged;
	}

	/// <summary>
	/// Event raised when a child control action is triggered.
	/// </summary>
	/// <remarks>
	/// This event notifies parent controls when the user initiates create, edit, details, or delete actions.
	/// The event data includes the action type and the ID of the affected item (if applicable).
	/// </remarks>
	public event EventHandler<ChildControlEventArgs<CalculationTypesIndexControl>>? ChildControlEvent;

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Gets or sets the collection of calculation types to display.
	/// </summary>
	/// <value>
	/// An observable collection of <see cref="CalculationTypeViewModel"/> instances.
	/// </value>
	public Collection<CalculationTypeViewModel> CalculationTypesList
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(CalculationTypesList));
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
	/// Raises the <see cref="OnInitialized"/> event and loads calculation types from the API.
	/// </summary>
	/// <param name="e">The event data.</param>
	/// <remarks>
	/// This method fetches all calculation types from the endpoint and populates the <see cref="CalculationTypesList"/>.
	/// If an error occurs during data retrieval, it sets the <see cref="ErrorMessage"/> property.
	/// </remarks>
	/// <exception cref="HttpIOException">Thrown when an HTTP communication error occurs.</exception>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} with {EventArgs}", nameof(CalculationTypesIndexControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);

		try
		{
			List<CalculationTypeFullModel>? results = _endpoint.GetAllAsync().Result;

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} returned {ResultsCount}", nameof(_endpoint.GetAllAsync), results?.Count);
			}

			if (results is not null)
			{
				foreach (CalculationTypeFullModel item in results)
				{
					CalculationTypeViewModel model = new(item);
					CalculationTypesList.Add(model);
				}
			}
		}
		catch (HttpIOException ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {Method} had an error", nameof(CalculationTypesIndexControl), nameof(OnInitialized));
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
						_logger.LogError(ex, "{UserControl} {Method} had an error", nameof(CalculationTypesIndexControl), nameof(OnInitialized));
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
			_logger.LogDebug("{UserControl} {Method} with {PropertyName}", nameof(CalculationTypesIndexControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void CreateNewButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesIndexControl), nameof(CreateNewButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<CalculationTypesIndexControl>("create", null));
	}

	private void EditButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesIndexControl), nameof(EditButton_Click), sender, e);
		}

		Button? button = sender as Button;
		if (button is not null)
		{
			CalculationTypeViewModel? itemToEdit = button.DataContext as CalculationTypeViewModel;
			if (itemToEdit is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<CalculationTypesIndexControl>("edit", itemToEdit.Id));
			}
		}
	}

	private void DetailsButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesIndexControl), nameof(DetailsButton_Click), sender, e);
		}

		Button? button = sender as Button;
		if (button is not null)
		{
			CalculationTypeViewModel? itemToView = button.DataContext as CalculationTypeViewModel;
			if (itemToView is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<CalculationTypesIndexControl>("details", itemToView.Id));
			}
		}
	}

	private void DeleteButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesIndexControl), nameof(DeleteButton_Click), sender, e);
		}

		Button? button = sender as Button;
		if (button is not null)
		{
			CalculationTypeViewModel? itemToDelete = button.DataContext as CalculationTypeViewModel;
			if (itemToDelete is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<CalculationTypesIndexControl>("delete", itemToDelete.Id));
			}
		}
	}

	private void CalculationTypesIndexControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesIndexControl), nameof(CalculationTypesIndexControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}
	}
}
