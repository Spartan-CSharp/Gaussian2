using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using GaussianCommonLibrary.Models;

using GaussianWPF.Models;

using GaussianWPFLibrary.DataAccess;
using GaussianWPFLibrary.EventModels;
using GaussianWPFLibrary.Models;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls.CalculationTypes;

/// <summary>
/// User control for displaying detailed information about a specific calculation type.
/// Implements INotifyPropertyChanged to support WPF data binding and provides functionality
/// for viewing calculation type details, including RTF-formatted descriptions.
/// </summary>
public partial class CalculationTypesDetailsControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<CalculationTypesIndexControl> _logger;
	private readonly ILoggedInUserModel _loggedInUser;
	private readonly IApiHelper _apiHelper;
	private readonly ICalculationTypesEndpoint _endpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="CalculationTypesDetailsControl"/> class.
	/// </summary>
	/// <param name="logger">The logger instance for logging control events and errors.</param>
	/// <param name="loggedInUser">The model representing the currently logged-in user.</param>
	/// <param name="apiHelper">The API helper for making HTTP requests.</param>
	/// <param name="endpoint">The endpoint for accessing calculation types data.</param>
	public CalculationTypesDetailsControl(ILogger<CalculationTypesIndexControl> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, ICalculationTypesEndpoint endpoint)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_endpoint = endpoint;

		InitializeComponent();
		DataContext = this;
		PropertyChanged += CalculationTypesDetailsControl_PropertyChanged;
	}

	/// <summary>
	/// Event raised when the control needs to communicate with its parent or container.
	/// Used for navigation and control interaction events.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<CalculationTypesDetailsControl>>? ChildControlEvent;

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Gets or sets the ID of the calculation type to display.
	/// When set, triggers retrieval of the calculation type details from the endpoint.
	/// </summary>
	public int CalculationTypeId
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(CalculationTypeId));
		}
	}

	/// <summary>
	/// Gets or sets the view model for the calculation type being displayed.
	/// When set, triggers updates to the UI, including the RTF description.
	/// </summary>
	public CalculationTypeViewModel? CalculationType
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(CalculationType));
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether the calculation type model is not null.
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
	/// Gets a value indicating whether the calculation type model is null.
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
			_logger.LogDebug("{UserControl} {Method} with {PropertyName}", nameof(CalculationTypesDetailsControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void CalculationTypesDetailsControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesDetailsControl), nameof(CalculationTypesDetailsControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(CalculationTypeId))
		{
			try
			{
				if (CalculationTypeId != 0)
				{
					CalculationTypeFullModel? results = _endpoint.GetByIdAsync(CalculationTypeId).Result;

					if (_logger.IsEnabled(LogLevel.Trace))
					{
						_logger.LogTrace("{Method} returned {Results}", nameof(_endpoint.GetByIdAsync), results);
					}

					if (results is not null)
					{
						CalculationType = new CalculationTypeViewModel(results);
						ModelIsNotNull = true;
					}
				}
				else
				{
					CalculationType = null;
					ModelIsNotNull = false;
				}
			}
			catch (HttpIOException ex)
			{
				if (_logger.IsEnabled(LogLevel.Error))
				{
					_logger.LogError(ex, "{UserControl} {Method} had an error", nameof(CalculationTypesDetailsControl), nameof(OnInitialized));
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
							_logger.LogError(ex, "{UserControl} {Method} had an error", nameof(CalculationTypesDetailsControl), nameof(OnInitialized));
						}

						ErrorMessage = ex.Message;
						return true;
					}

					// Return false for any other exception types to rethrow them in a new AggregateException
					return false;
				});
			}
		}

		if (e.PropertyName is nameof(CalculationType))
		{
			if (CalculationType is not null)
			{
				// Populate the RichTextBox with RTF
				SetRtfText(DescriptionRichTextBox, CalculationType.DescriptionRtf);
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
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesDetailsControl), nameof(EditButton_Click), sender, e);
		}

		if (ModelIsNotNull)
		{
			ChildControlEvent?.Invoke(this, new ChildControlEventArgs<CalculationTypesDetailsControl>("edit", CalculationType!.Id));
		}
	}

	private void BackToIndexButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesDetailsControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<CalculationTypesDetailsControl>("index", null));
	}

	private static void SetRtfText(RichTextBox rtb, string? rtfText)
	{
		if (string.IsNullOrEmpty(rtfText))
		{
			rtb.Document.Blocks.Clear();
		}
		else
		{
			TextRange range = new(rtb.Document.ContentStart, rtb.Document.ContentEnd);
			using MemoryStream stream = new(Encoding.UTF8.GetBytes(rtfText));
			range.Load(stream, DataFormats.Rtf);
		}
	}
}
