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
/// A WPF UserControl that provides an interface for editing Method Family records.
/// Implements INotifyPropertyChanged for data binding and includes comprehensive error handling and logging.
/// </summary>
public partial class MethodFamiliesEditControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<MethodFamiliesEditControl> _logger;
	private readonly ILoggedInUserModel _loggedInUser;
	private readonly IApiHelper _apiHelper;
	private readonly IMethodFamiliesEndpoint _endpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="MethodFamiliesEditControl"/> class.
	/// </summary>
	/// <param name="logger">Logger instance for diagnostic and trace information.</param>
	/// <param name="loggedInUser">The currently logged-in user model.</param>
	/// <param name="apiHelper">Helper for API operations.</param>
	/// <param name="endpoint">Endpoint for Method Family data access operations.</param>
	public MethodFamiliesEditControl(ILogger<MethodFamiliesEditControl> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, IMethodFamiliesEndpoint endpoint)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_endpoint = endpoint;

		InitializeComponent();
		DataContext = this;
		PropertyChanged += MethodFamiliesEditControl_PropertyChanged;
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Occurs when a child control event needs to be communicated to the parent control.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<MethodFamiliesEditControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the ID of the Method Family being edited.
	/// Setting this property triggers loading of the Method Family data.
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
	/// Gets or sets the Method Family view model being edited.
	/// When set, updates all related properties for data binding.
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
	/// Gets or sets the keyword associated with the Method Family.
	/// This property is bound to the UI and affects the <see cref="CanSave"/> state.
	/// </summary>
	public string MethodFamilyName
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(MethodFamilyName));
		}
	} = string.Empty;

	/// <summary>
	/// Gets or sets a value indicating whether the Method Family model is not null.
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
	/// Gets a value indicating whether the Method Family model is null.
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
	/// Determined by the validity of <see cref="MethodFamilyName"/>.
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
			_logger.LogDebug("{UserControl} {Method} with {PropertyName}", nameof(MethodFamiliesEditControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void MethodFamiliesEditControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesEditControl), nameof(MethodFamiliesEditControl_PropertyChanged), sender, e);
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
					_logger.LogError(ex, "{UserControl} {Method} had an error", nameof(MethodFamiliesEditControl), nameof(OnInitialized));
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
							_logger.LogError(ex, "{UserControl} {Method} had an error", nameof(MethodFamiliesEditControl), nameof(OnInitialized));
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
				MethodFamilyName = MethodFamily.Name;
				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(MethodFamily.DescriptionRtf);
			}
			else
			{
				MethodFamilyName = string.Empty;
				DescriptionRichTextBox.Document.Blocks.Clear();
			}
		}

		if (e.PropertyName is nameof(MethodFamilyName))
		{
			CanSave = MethodFamilyName?.Length > 0;
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
				_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesEditControl), nameof(SaveButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;

			string descriptionRtf = DescriptionRichTextBox.GetRtfText();
			string descriptionText = DescriptionRichTextBox.GetPlainText();
			MethodFamily!.Name = MethodFamilyName;
			MethodFamily!.DescriptionRtf = descriptionRtf;
			MethodFamily!.DescriptionText = descriptionText;

			MethodFamilyFullModel model = MethodFamily!.ToFullModel();
			MethodFamilyFullModel? result = _endpoint.UpdateAsync(MethodFamilyId, model).Result;

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} returned {Result}", nameof(_endpoint.UpdateAsync), result);
			}

			if (result is not null)
			{
				MethodFamily.LastUpdatedDate = result.LastUpdatedDate;
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<MethodFamiliesEditControl>("save", null));
			}
		}
		catch (HttpIOException ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(MethodFamiliesEditControl), nameof(SaveButton_Click));
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
						_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(MethodFamiliesEditControl), nameof(SaveButton_Click));
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
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesEditControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<MethodFamiliesEditControl>("index", null));
	}

	private void BoldButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(MethodFamiliesEditControl), nameof(BoldButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleFontWeight();
	}

	private void ItalicButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(MethodFamiliesEditControl), nameof(ItalicButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleFontStyle();
	}

	private void UnderlineButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(MethodFamiliesEditControl), nameof(UnderlineButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleUnderline();
	}

	private void SubscriptButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(MethodFamiliesEditControl), nameof(SubscriptButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleBaselineAlignment(BaselineAlignment.Subscript);
	}

	private void SuperscriptButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(MethodFamiliesEditControl), nameof(SuperscriptButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleBaselineAlignment(BaselineAlignment.Superscript);
	}

	private void BulletsButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(MethodFamiliesEditControl), nameof(BulletsButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleList(TextMarkerStyle.Disc);
	}

	private void NumberingButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(MethodFamiliesEditControl), nameof(NumberingButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleList(TextMarkerStyle.Decimal);
	}
}
