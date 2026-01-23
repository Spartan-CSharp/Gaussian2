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

namespace GaussianWPF.Controls.MethodFamilies;

/// <summary>
/// User control for displaying detailed information about a specific Method Family.
/// Implements INotifyPropertyChanged to support WPF data binding and provides functionality
/// for viewing Method Family details, including RTF-formatted descriptions.
/// </summary>
public partial class MethodFamiliesDetailsControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<MethodFamiliesIndexControl> _logger;
	private readonly ILoggedInUserModel _loggedInUser;
	private readonly IApiHelper _apiHelper;
	private readonly IMethodFamiliesEndpoint _endpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="MethodFamiliesDetailsControl"/> class.
	/// </summary>
	/// <param name="logger">The logger instance for logging control events and errors.</param>
	/// <param name="loggedInUser">The model representing the currently logged-in user.</param>
	/// <param name="apiHelper">The API helper for making HTTP requests.</param>
	/// <param name="endpoint">The endpoint for accessing Method Families data.</param>
	public MethodFamiliesDetailsControl(ILogger<MethodFamiliesIndexControl> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, IMethodFamiliesEndpoint endpoint)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_endpoint = endpoint;

		InitializeComponent();
		DataContext = this;
		PropertyChanged += MethodFamiliesDetailsControl_PropertyChanged;
	}

	/// <summary>
	/// Event raised when the control needs to communicate with its parent or container.
	/// Used for navigation and control interaction events.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<MethodFamiliesDetailsControl>>? ChildControlEvent;

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Gets or sets the ID of the Method Family to display.
	/// When set, triggers retrieval of the Method Family details from the endpoint.
	/// </summary>
	public int MethodFamilyId
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(MethodFamilyId));
		}
	}

	/// <summary>
	/// Gets or sets the view model for the Method Family being displayed.
	/// When set, triggers updates to the UI, including the RTF description.
	/// </summary>
	public MethodFamilyViewModel? MethodFamily
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(MethodFamily));
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether the Method Family model is not null.
	/// Used for controlling the visibility of UI elements based on whether data is loaded.
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
	/// Gets a value indicating whether the Method Family model is null.
	/// This is the inverse of <see cref="ModelIsNotNull"/>.
	/// </summary>
	public bool ModelIsNull
	{
		get { return !ModelIsNotNull; }
	}

	/// <summary>
	/// Gets or sets the error message to display when an error occurs.
	/// Typically populated when data retrieval fails.
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
	/// Used to control the visibility of error UI elements.
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
	/// Raises the <see cref="PropertyChanged"/> event for the specified property.
	/// Logs debug information when enabled.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed. This parameter is optional and can be automatically populated by the compiler.</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} with {PropertyName}", nameof(MethodFamiliesDetailsControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void MethodFamiliesDetailsControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesDetailsControl), nameof(MethodFamiliesDetailsControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(MethodFamilyId))
		{
			try
			{
				if (MethodFamilyId != 0)
				{
					MethodFamilyFullModel? results = _endpoint.GetByIdAsync(MethodFamilyId).Result;

					if (_logger.IsEnabled(LogLevel.Trace))
					{
						_logger.LogTrace("{Method} returned {Results}", nameof(_endpoint.GetByIdAsync), results);
					}

					if (results is not null)
					{
						MethodFamily = new MethodFamilyViewModel(results);
						ModelIsNotNull = true;
					}
				}
				else
				{
					MethodFamily = null;
					ModelIsNotNull = false;
				}
			}
			catch (HttpIOException ex)
			{
				if (_logger.IsEnabled(LogLevel.Error))
				{
					_logger.LogError(ex, "{UserControl} {Method} had an error", nameof(MethodFamiliesDetailsControl), nameof(OnInitialized));
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
							_logger.LogError(ex, "{UserControl} {Method} had an error", nameof(MethodFamiliesDetailsControl), nameof(OnInitialized));
						}

						ErrorMessage = ex.Message;
						return true;
					}

					// Return false for any other exception types to rethrow them in a new AggregateException
					return false;
				});
			}
		}

		if (e.PropertyName is nameof(MethodFamily))
		{
			if (MethodFamily is not null)
			{
				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(MethodFamily.DescriptionRtf);
			}
			else
			{
				DescriptionRichTextBox.Document.Blocks.Clear();
			}
		}
	}

	private void EditButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesDetailsControl), nameof(EditButton_Click), sender, e);
		}

		if (ModelIsNotNull)
		{
			ChildControlEvent?.Invoke(this, new ChildControlEventArgs<MethodFamiliesDetailsControl>("edit", MethodFamily!.Id));
		}
	}

	private void BackToIndexButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesDetailsControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<MethodFamiliesDetailsControl>("index", null));
	}
}
