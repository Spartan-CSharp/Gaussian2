using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

using GaussianCommonLibrary.Models;

using GaussianWPF.Models;
using GaussianWPF.WPFHelpers;

using GaussianWPFLibrary.DataAccess;
using GaussianWPFLibrary.EventModels;
using GaussianWPFLibrary.Models;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls.BaseMethods;

/// <summary>
/// Interaction logic for BaseMethodsDeleteControl.xaml
/// Provides a user interface for viewing and deleting Base Methods with confirmation.
/// </summary>
public partial class BaseMethodsDeleteControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<BaseMethodsDeleteControl> _logger;
	private readonly ILoggedInUserModel _loggedInUser;
	private readonly IApiHelper _apiHelper;
	private readonly IBaseMethodsEndpoint _baseMethodsEndpoint;
	private readonly IMethodFamiliesEndpoint _methodFamiliesEndpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodsDeleteControl"/> class.
	/// </summary>
	/// <param name="logger">Logger for diagnostic and trace information.</param>
	/// <param name="loggedInUser">The currently logged-in user model.</param>
	/// <param name="apiHelper">Helper for API interactions.</param>
	/// <param name="baseMethodsEndpoint">Endpoint for Base Methods data access operations.</param>
	/// <param name="methodFamiliesEndpoint">Endpoint for Method Families data access operations.</param>
	public BaseMethodsDeleteControl(ILogger<BaseMethodsDeleteControl> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, IBaseMethodsEndpoint baseMethodsEndpoint, IMethodFamiliesEndpoint methodFamiliesEndpoint)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_baseMethodsEndpoint = baseMethodsEndpoint;
		_methodFamiliesEndpoint = methodFamiliesEndpoint;
		InitializeComponent();
		DataContext = this;
		PropertyChanged += BaseMethodsDeleteControl_PropertyChanged;
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Occurs when a child control event is raised, such as navigation or completion events.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<BaseMethodsDeleteControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the ID of the Base Method to be deleted.
	/// When set, triggers loading of the Base Method details.
	/// </summary>
	public int BaseMethodId
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(BaseMethodId));
		}
	}

	/// <summary>
	/// Gets or sets the Base Method view model containing the data to be displayed and deleted.
	/// </summary>
	public BaseMethodViewModel? BaseMethod
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(BaseMethod));
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether the Base Method model is not null.
	/// Used for controlling UI element visibility.
	/// </summary>
	public bool ModelIsNotNull
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(ModelIsNotNull));
			OnPropertyChanged(nameof(ModelIsNull));
		}
	}

	/// <summary>
	/// Gets a value indicating whether the Base Method model is null.
	/// Computed property based on <see cref="ModelIsNotNull"/>.
	/// </summary>
	public bool ModelIsNull
	{
		get { return !ModelIsNotNull; }
	}

	/// <summary>
	/// Gets or sets the error message to display when an operation fails.
	/// </summary>
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
	/// Automatically updated when <see cref="ErrorMessage"/> changes.
	/// </summary>
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
	/// Raises the <see cref="PropertyChanged"/> event.
	/// </summary>
	/// <param name="propertyName">Name of the property that changed. Automatically populated by the compiler.</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} with {PropertyName}", nameof(BaseMethodsDeleteControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void BaseMethodsDeleteControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsDeleteControl), nameof(BaseMethodsDeleteControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(BaseMethodId))
		{
			try
			{
				if (BaseMethodId != 0)
				{
					BaseMethodFullModel? results = _baseMethodsEndpoint.GetByIdAsync(BaseMethodId).Result;

					if (_logger.IsEnabled(LogLevel.Trace))
					{
						_logger.LogTrace("{Method} returned {Results}", nameof(_baseMethodsEndpoint.GetByIdAsync), results);
					}

					List<MethodFamilyRecord>? methodFamilies = _methodFamiliesEndpoint.GetListAsync().Result;

					if (results is not null && methodFamilies is not null && methodFamilies.Count > 0)
					{
						BaseMethod = new BaseMethodViewModel(results, methodFamilies);
						ModelIsNotNull = true;
					}
				}
				else
				{
					BaseMethod = null;
					ModelIsNotNull = false;
				}
			}
			catch (HttpIOException ex)
			{
				if (_logger.IsEnabled(LogLevel.Error))
				{
					_logger.LogError(ex, "{UserControl} {Method} had an error", nameof(BaseMethodsEditControl), nameof(OnInitialized));
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
							_logger.LogError(ex, "{UserControl} {Method} had an error", nameof(BaseMethodsEditControl), nameof(OnInitialized));
						}

						ErrorMessage = ex.Message;
						return true;
					}

					// Return false for any other exception types to rethrow them in a new AggregateException
					return false;
				});
			}
		}

		if (e.PropertyName is nameof(BaseMethod))
		{
			if (BaseMethod is not null)
			{
				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(BaseMethod.DescriptionRtf);
			}
			else
			{
				DescriptionRichTextBox.Document.Blocks.Clear();
			}
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}
	}

	private void DeleteButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsDeleteControl), nameof(DeleteButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;

			_baseMethodsEndpoint.DeleteAsync(BaseMethodId).Wait();
			BaseMethod!.LastUpdatedDate = DateTime.Now;
			BaseMethod!.Archived = true;
			ChildControlEvent?.Invoke(this, new ChildControlEventArgs<BaseMethodsDeleteControl>("delete", null));
		}
		catch (HttpIOException ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(BaseMethodsDeleteControl), nameof(DeleteButton_Click));
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
						_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(BaseMethodsDeleteControl), nameof(DeleteButton_Click));
					}

					ErrorMessage = ex.Message;
					return true;
				}

				// Return false for any other exception types to rethrow them in a new AggregateException
				return false;
			});
		}
	}

	private void BackToIndexButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsDeleteControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<BaseMethodsDeleteControl>("index", null));
	}
}
