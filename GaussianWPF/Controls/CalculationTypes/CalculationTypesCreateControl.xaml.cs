using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
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
/// Interaction logic for CalculationTypesCreateControl.xaml
/// A user control that provides functionality for creating new calculation types.
/// Implements INotifyPropertyChanged to support data binding with the XAML view.
/// </summary>
public partial class CalculationTypesCreateControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<CalculationTypesCreateControl> _logger;
	private readonly ILoggedInUserModel _loggedInUser;
	private readonly IApiHelper _apiHelper;
	private readonly ICalculationTypesEndpoint _endpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="CalculationTypesCreateControl"/> class.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="loggedInUser">The currently logged-in user model.</param>
	/// <param name="apiHelper">The API helper for making HTTP requests.</param>
	/// <param name="endpoint">The endpoint for calculation types API operations.</param>
	public CalculationTypesCreateControl(ILogger<CalculationTypesCreateControl> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, ICalculationTypesEndpoint endpoint)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_endpoint = endpoint;

		InitializeComponent();
		DataContext = this;
		PropertyChanged += CalculationTypesCreateControl_PropertyChanged;
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Occurs when a child control event is raised, typically for navigation or state changes.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<CalculationTypesCreateControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the calculation type view model being created.
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
	/// Gets or sets the name of the calculation type.
	/// This property is bound to the UI and triggers validation when changed.
	/// </summary>
	public string CalculationTypeName
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(CalculationTypeName));
		}
	} = string.Empty;

	/// <summary>
	/// Gets or sets the keyword associated with the calculation type.
	/// This property is bound to the UI and triggers validation when changed.
	/// </summary>
	public string Keyword
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(Keyword));
		}
	} = string.Empty;

	/// <summary>
	/// Gets or sets the error message to display to the user.
	/// Setting this property will automatically update the <see cref="IsErrorVisible"/> property.
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
	/// Gets or sets a value indicating whether error messages should be visible in the UI.
	/// This property is automatically updated when <see cref="ErrorMessage"/> changes.
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
	/// Gets or sets a value indicating whether the save operation can be performed.
	/// This property is automatically updated based on validation of required fields.
	/// </summary>
	public bool CanSave
	{
		get;

		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(CanSave));
			}
		}
	}

	/// <summary>
	/// Raises the <see cref="PropertyChanged"/> event for the specified property.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed. This is automatically provided by the compiler.</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} with {PropertyName}", nameof(CalculationTypesCreateControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void CalculationTypesCreateControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesCreateControl), nameof(CalculationTypesCreateControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is (nameof(CalculationTypeName)) or (nameof(Keyword)))
		{
			CanSave = CalculationTypeName?.Length > 0 && Keyword?.Length > 0;
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}
	}

	private void CreateButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesCreateControl), nameof(CreateButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;

			string descriptionRtf = GetRtfText(DescriptionRichTextBox);
			string descriptionText = GetPlainText(DescriptionRichTextBox);
			CalculationType = new CalculationTypeViewModel
			{
				Name = CalculationTypeName,
				Keyword = Keyword,
				DescriptionRtf = descriptionRtf,
				DescriptionText = descriptionText
			};

			CalculationTypeFullModel model = CalculationType.ToFullModel();
			CalculationTypeFullModel? result = _endpoint.CreateAsync(model).Result;

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} returned {Result}", nameof(_endpoint.CreateAsync), result);
			}

			if (result is not null)
			{
				CalculationType.Id = result.Id;
				CalculationType.CreatedDate = result.CreatedDate;
				CalculationType.LastUpdatedDate = result.LastUpdatedDate;
				CalculationType.Archived = result.Archived;
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<CalculationTypesCreateControl>("create", null));
			}
		}
		catch (HttpIOException ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(CalculationTypesCreateControl), nameof(CreateButton_Click));
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
						_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(CalculationTypesCreateControl), nameof(CreateButton_Click));
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
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesCreateControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<CalculationTypesCreateControl>("index", null));
	}

	private static string GetPlainText(RichTextBox rtb)
	{
		TextRange range = new(rtb.Document.ContentStart, rtb.Document.ContentEnd);
		string output = range.Text;
		return output;
	}

	private static string GetRtfText(RichTextBox rtb)
	{
		TextRange range = new(rtb.Document.ContentStart, rtb.Document.ContentEnd);
		using MemoryStream stream = new();
		range.Save(stream, DataFormats.Rtf);
		stream.Position = 0;
		using StreamReader reader = new(stream);
		string output = reader.ReadToEnd();
		return output;
	}
}
