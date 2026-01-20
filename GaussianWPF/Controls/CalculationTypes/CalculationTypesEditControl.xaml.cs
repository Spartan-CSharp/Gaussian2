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
/// A WPF UserControl that provides an interface for editing calculation type records.
/// Implements INotifyPropertyChanged for data binding and includes comprehensive error handling and logging.
/// </summary>
public partial class CalculationTypesEditControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<CalculationTypesEditControl> _logger;
	private readonly ILoggedInUserModel _loggedInUser;
	private readonly IApiHelper _apiHelper;
	private readonly ICalculationTypesEndpoint _endpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="CalculationTypesEditControl"/> class.
	/// </summary>
	/// <param name="logger">Logger instance for diagnostic and trace information.</param>
	/// <param name="loggedInUser">The currently logged-in user model.</param>
	/// <param name="apiHelper">Helper for API operations.</param>
	/// <param name="endpoint">Endpoint for calculation type data access operations.</param>
	public CalculationTypesEditControl(ILogger<CalculationTypesEditControl> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, ICalculationTypesEndpoint endpoint)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_endpoint = endpoint;

		InitializeComponent();
		DataContext = this;
		PropertyChanged += CalculationTypesEditControl_PropertyChanged;
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Occurs when a child control event needs to be communicated to the parent control.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<CalculationTypesEditControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the ID of the calculation type being edited.
	/// Setting this property triggers loading of the calculation type data.
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
	/// Gets or sets the calculation type view model being edited.
	/// When set, updates all related properties for data binding.
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
	/// This property is bound to the UI and affects the <see cref="CanSave"/> state.
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
	/// This property is bound to the UI and affects the <see cref="CanSave"/> state.
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
	/// Gets or sets a value indicating whether the calculation type model is not null.
	/// Used for conditional UI rendering.
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
	/// Computed from <see cref="ModelIsNotNull"/>.
	/// </summary>
	public bool ModelIsNull
	{
		get { return !ModelIsNotNull; }
	}

	/// <summary>
	/// Gets or sets the current error message to be displayed to the user.
	/// Setting this property updates the <see cref="IsErrorVisible"/> state.
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
	/// Gets or sets a value indicating whether an error message should be visible in the UI.
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
	/// Determined by the validity of <see cref="CalculationTypeName"/> and <see cref="Keyword"/>.
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
	/// <param name="propertyName">The name of the property that changed. This is automatically populated by the CallerMemberName attribute.</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} with {PropertyName}", nameof(CalculationTypesEditControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void CalculationTypesEditControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesEditControl), nameof(CalculationTypesEditControl_PropertyChanged), sender, e);
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
					_logger.LogError(ex, "{UserControl} {Method} had an error", nameof(CalculationTypesEditControl), nameof(OnInitialized));
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
							_logger.LogError(ex, "{UserControl} {Method} had an error", nameof(CalculationTypesEditControl), nameof(OnInitialized));
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
				CalculationTypeName = CalculationType.Name;
				Keyword = CalculationType.Keyword;
				// Populate the RichTextBox with RTF
				SetRtfText(DescriptionRichTextBox, CalculationType.DescriptionRtf);
			}
			else
			{
				CalculationTypeName = string.Empty;
				Keyword = string.Empty;
				DescriptionRichTextBox.Document.Blocks.Clear();
			}
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

	private void SaveButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesEditControl), nameof(SaveButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;

			string descriptionRtf = GetRtfText(DescriptionRichTextBox);
			string descriptionText = GetPlainText(DescriptionRichTextBox);
			CalculationType!.Name = CalculationTypeName;
			CalculationType!.Keyword = Keyword;
			CalculationType!.DescriptionRtf = descriptionRtf;
			CalculationType!.DescriptionText = descriptionText;

			CalculationTypeFullModel model = CalculationType!.ToFullModel();
			CalculationTypeFullModel? result = _endpoint.UpdateAsync(CalculationTypeId, model).Result;

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} returned {Result}", nameof(_endpoint.UpdateAsync), result);
			}

			if (result is not null)
			{
				CalculationType.LastUpdatedDate = result.LastUpdatedDate;
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<CalculationTypesEditControl>("save", null));
			}
		}
		catch (HttpIOException ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(CalculationTypesEditControl), nameof(SaveButton_Click));
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
						_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(CalculationTypesEditControl), nameof(SaveButton_Click));
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
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesEditControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<CalculationTypesEditControl>("index", null));
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
