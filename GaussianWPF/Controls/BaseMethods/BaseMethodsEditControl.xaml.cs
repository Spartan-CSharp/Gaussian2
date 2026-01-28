using System.Collections.ObjectModel;
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
/// A WPF UserControl that provides an interface for editing Base Method records.
/// Implements INotifyPropertyChanged for data binding and includes comprehensive error handling and logging.
/// </summary>
public partial class BaseMethodsEditControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<BaseMethodsEditControl> _logger;
	private readonly ILoggedInUserModel _loggedInUser;
	private readonly IApiHelper _apiHelper;
	private readonly IBaseMethodsEndpoint _baseMethodsEndpoint;
	private readonly IMethodFamiliesEndpoint _methodFamiliesEndpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodsEditControl"/> class.
	/// </summary>
	/// <param name="logger">Logger instance for diagnostic and trace information.</param>
	/// <param name="loggedInUser">The currently logged-in user model.</param>
	/// <param name="apiHelper">Helper for API operations.</param>
	/// <param name="baseMethodsEndpoint">Endpoint for Base Method data access operations.</param>
	/// <param name="methodFamiliesEndpoint">Endpoint for Method Family data access operations.</param>
	public BaseMethodsEditControl(ILogger<BaseMethodsEditControl> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, IBaseMethodsEndpoint baseMethodsEndpoint, IMethodFamiliesEndpoint methodFamiliesEndpoint)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_baseMethodsEndpoint = baseMethodsEndpoint;
		_methodFamiliesEndpoint = methodFamiliesEndpoint;

		InitializeComponent();
		DataContext = this;
		PropertyChanged += BaseMethodsEditControl_PropertyChanged;
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Occurs when a child control event needs to be communicated to the parent control.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<BaseMethodsEditControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the ID of the Base Method being edited.
	/// Setting this property triggers loading of the Base Method data.
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
	/// Gets or sets the Base Method view model being edited.
	/// When set, updates all related properties for data binding.
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
	/// Gets or sets the keyword associated with the Base Method.
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
	/// Gets or sets the currently selected Method Family.
	/// This property is bound to the UI and affects the <see cref="CanSave"/> state.
	/// </summary>
	public MethodFamilyRecord? SelectedMethodFamily
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(SelectedMethodFamily));
		}
	}

	/// <summary>
	/// Gets or sets the collection of available Method Families for selection.
	/// This collection is populated when a Base Method is loaded for editing.
	/// </summary>
	public ObservableCollection<MethodFamilyRecord> MethodFamilyList
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(MethodFamilyList));
		}
	} = [];

	/// <summary>
	/// Gets or sets a value indicating whether the Base Method model is not null.
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
	/// Gets a value indicating whether the Base Method model is null.
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
	/// This is true when both <see cref="SelectedMethodFamily"/> is not null and <see cref="Keyword"/> has a length greater than zero.
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
			_logger.LogDebug("{UserControl} {Method} with {PropertyName}", nameof(BaseMethodsEditControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	/// <summary>
	/// Handles property changes for the control, managing dependent property updates and data loading.
	/// Loads Base Method data when <see cref="BaseMethodId"/> changes, updates UI bindings when <see cref="BaseMethod"/> changes,
	/// validates save conditions when <see cref="SelectedMethodFamily"/> or <see cref="Keyword"/> change,
	/// and controls error visibility when <see cref="ErrorMessage"/> changes.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="PropertyChangedEventArgs"/> containing the property name that changed.</param>
	private void BaseMethodsEditControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsEditControl), nameof(BaseMethodsEditControl_PropertyChanged), sender, e);
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
						foreach (MethodFamilyRecord item in methodFamilies)
						{
							MethodFamilyList.Add(item);
						}

						SelectedMethodFamily = MethodFamilyList.FirstOrDefault(mf => mf.Id == results.MethodFamily.Id);
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
				Keyword = BaseMethod.Keyword;
				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(BaseMethod.DescriptionRtf);
			}
			else
			{
				Keyword = string.Empty;
				DescriptionRichTextBox.Document.Blocks.Clear();
			}
		}

		if (e.PropertyName is (nameof(SelectedMethodFamily)) or (nameof(Keyword)))
		{
			CanSave = SelectedMethodFamily is not null && Keyword?.Length > 0;
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}
	}

	/// <summary>
	/// Handles the Save button click event, persisting changes to the Base Method.
	/// Retrieves RTF and plain text from the description editor, updates the view model,
	/// converts to a simple model, and calls the update endpoint. On success, updates the
	/// last modified timestamp and raises a "save" child control event.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
	private void SaveButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsEditControl), nameof(SaveButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;

			string descriptionRtf = DescriptionRichTextBox.GetRtfText();
			string descriptionText = DescriptionRichTextBox.GetPlainText();
			BaseMethod!.Keyword = Keyword;
			BaseMethod.MethodFamily = SelectedMethodFamily;
			BaseMethod!.DescriptionRtf = descriptionRtf;
			BaseMethod!.DescriptionText = descriptionText;

			BaseMethodSimpleModel model = BaseMethod.ToSimpleModel();
			BaseMethodFullModel? result = _baseMethodsEndpoint.UpdateAsync(BaseMethodId, model).Result;

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} returned {Result}", nameof(_baseMethodsEndpoint.UpdateAsync), result);
			}

			if (result is not null)
			{
				BaseMethod.LastUpdatedDate = result.LastUpdatedDate;
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<BaseMethodsEditControl>("save", null));
			}
		}
		catch (HttpIOException ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(BaseMethodsEditControl), nameof(SaveButton_Click));
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
						_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(BaseMethodsEditControl), nameof(SaveButton_Click));
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
	/// Handles the Back to Index button click event, navigating the user back to the index view.
	/// Raises an "index" child control event to signal the parent to switch views.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
	private void BackToIndexButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsEditControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<BaseMethodsEditControl>("index", null));
	}

	/// <summary>
	/// Handles the Bold button click event, toggling bold formatting on the current selection in the description editor.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
	private void BoldButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(BaseMethodsEditControl), nameof(BoldButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleFontWeight();
	}

	/// <summary>
	/// Handles the Italic button click event, toggling italic formatting on the current selection in the description editor.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
	private void ItalicButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(BaseMethodsEditControl), nameof(ItalicButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleFontStyle();
	}

	/// <summary>
	/// Handles the Underline button click event, toggling underline formatting on the current selection in the description editor.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
	private void UnderlineButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(BaseMethodsEditControl), nameof(UnderlineButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleUnderline();
	}

	/// <summary>
	/// Handles the Subscript button click event, toggling subscript formatting on the current selection in the description editor.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
	private void SubscriptButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(BaseMethodsEditControl), nameof(SubscriptButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleBaselineAlignment(BaselineAlignment.Subscript);
	}

	/// <summary>
	/// Handles the Superscript button click event, toggling superscript formatting on the current selection in the description editor.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
	private void SuperscriptButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(BaseMethodsEditControl), nameof(SuperscriptButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleBaselineAlignment(BaselineAlignment.Superscript);
	}

	/// <summary>
	/// Handles the Bullets button click event, toggling bulleted list formatting on the current selection in the description editor.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
	private void BulletsButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(BaseMethodsEditControl), nameof(BulletsButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleList(TextMarkerStyle.Disc);
	}

	/// <summary>
	/// Handles the Numbering button click event, toggling numbered list formatting on the current selection in the description editor.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
	private void NumberingButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(BaseMethodsEditControl), nameof(NumberingButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleList(TextMarkerStyle.Decimal);
	}
}
