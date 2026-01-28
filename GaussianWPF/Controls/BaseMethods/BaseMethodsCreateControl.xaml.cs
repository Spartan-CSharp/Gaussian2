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
/// User control for creating new base method records.
/// Provides functionality for entering method details, selecting method families,
/// formatting rich text descriptions, and saving new base methods via API endpoints.
/// </summary>
public partial class BaseMethodsCreateControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<BaseMethodsCreateControl> _logger;
	private readonly ILoggedInUserModel _loggedInUser;
	private readonly IApiHelper _apiHelper;
	private readonly IBaseMethodsEndpoint _baseMethodsEndpoint;
	private readonly IMethodFamiliesEndpoint _methodFamiliesEndpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodsCreateControl"/> class.
	/// </summary>
	/// <param name="logger">Logger instance for tracking control events and errors.</param>
	/// <param name="loggedInUser">Current logged-in user information.</param>
	/// <param name="apiHelper">Helper for API authentication and communication.</param>
	/// <param name="baseMethodsEndpoint">Endpoint for base method CRUD operations.</param>
	/// <param name="methodFamiliesEndpoint">Endpoint for retrieving method family data.</param>
	public BaseMethodsCreateControl(ILogger<BaseMethodsCreateControl> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, IBaseMethodsEndpoint baseMethodsEndpoint, IMethodFamiliesEndpoint methodFamiliesEndpoint)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_baseMethodsEndpoint = baseMethodsEndpoint;
		_methodFamiliesEndpoint = methodFamiliesEndpoint;

		InitializeComponent();
		DataContext = this;
		PropertyChanged += BaseMethodsCreateControl_PropertyChanged;
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Occurs when the control needs to communicate with its parent, such as navigation requests.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<BaseMethodsCreateControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the view model for the base method being created.
	/// Contains all the data and validation logic for the new base method.
	/// </summary>
	public BaseMethodViewModel BaseMethod
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(BaseMethod));
		}
	} = new();

	/// <summary>
	/// Gets or sets the keyword identifier for the base method.
	/// Used to validate if the save operation can be performed.
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
	/// Gets or sets the currently selected method family from the dropdown.
	/// Required field for creating a new base method.
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
	/// Gets or sets the collection of available method families for selection.
	/// Populated during control initialization from the API endpoint.
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
	/// Gets or sets the error message to display when an operation fails.
	/// Setting this value automatically updates the <see cref="IsErrorVisible"/> property.
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
	/// Automatically set based on whether <see cref="ErrorMessage"/> has content.
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
	/// Gets or sets a value indicating whether the save button should be enabled.
	/// Determined by the presence of both a keyword and a selected method family.
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
	/// Populates the method family list from the API endpoint for user selection.
	/// </summary>
	/// <param name="e">Event data.</param>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} with {EventArgs}", nameof(BaseMethodsCreateControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		List<MethodFamilyRecord>? methodFamilies = _methodFamiliesEndpoint.GetListAsync().Result;

		if (methodFamilies is not null && methodFamilies.Count > 0)
		{
			foreach (MethodFamilyRecord item in methodFamilies)
			{
				BaseMethod.MethodFamilyList.Add(item);
				MethodFamilyList.Add(item);
			}
		}
	}

	/// <summary>
	/// Raises the <see cref="PropertyChanged"/> event to notify listeners of property value changes.
	/// Includes debug logging when enabled.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed. Automatically populated by the caller member name.</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} with {PropertyName}", nameof(BaseMethodsCreateControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	/// <summary>
	/// Handles property changed events to update dependent properties.
	/// Updates <see cref="CanSave"/> when keyword or method family changes,
	/// and updates <see cref="IsErrorVisible"/> when error message changes.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">Event data containing the property name that changed.</param>
	private void BaseMethodsCreateControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsCreateControl), nameof(BaseMethodsCreateControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is (nameof(Keyword)) or (nameof(SelectedMethodFamily)))
		{
			CanSave = SelectedMethodFamily is not null && Keyword?.Length > 0;
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}
	}

	/// <summary>
	/// Handles the create button click event to save a new base method.
	/// Extracts rich text and plain text from the description editor,
	/// sends the data to the API endpoint, and raises a child control event on success.
	/// Catches and logs HTTP and aggregate exceptions, displaying error messages to the user.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">Event data.</param>
	private void CreateButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsCreateControl), nameof(CreateButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;

			string descriptionRtf = DescriptionRichTextBox.GetRtfText();
			string descriptionText = DescriptionRichTextBox.GetPlainText();
			BaseMethod.Keyword = Keyword;
			BaseMethod.MethodFamily = SelectedMethodFamily;
			BaseMethod.DescriptionRtf = descriptionRtf;
			BaseMethod.DescriptionText = descriptionText;
			BaseMethodSimpleModel model = BaseMethod.ToSimpleModel();
			BaseMethodFullModel? result = _baseMethodsEndpoint.CreateAsync(model).Result;

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} returned {Result}", nameof(_baseMethodsEndpoint.CreateAsync), result);
			}

			if (result is not null)
			{
				BaseMethod.Id = result.Id;
				BaseMethod.CreatedDate = result.CreatedDate;
				BaseMethod.LastUpdatedDate = result.LastUpdatedDate;
				BaseMethod.Archived = result.Archived;
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<BaseMethodsCreateControl>("create", null));
			}
		}
		catch (HttpIOException ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(BaseMethodsCreateControl), nameof(CreateButton_Click));
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
						_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(BaseMethodsCreateControl), nameof(CreateButton_Click));
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
	/// Handles the back to index button click event.
	/// Raises a child control event to navigate back to the index view.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">Event data.</param>
	private void BackToIndexButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsCreateControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<BaseMethodsCreateControl>("index", null));
	}

	/// <summary>
	/// Handles the bold button click event.
	/// Toggles bold font weight for the selected text in the description rich text box.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">Event data.</param>
	private void BoldButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(BaseMethodsCreateControl), nameof(BoldButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleFontWeight();
	}

	/// <summary>
	/// Handles the italic button click event.
	/// Toggles italic font style for the selected text in the description rich text box.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">Event data.</param>
	private void ItalicButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(BaseMethodsCreateControl), nameof(ItalicButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleFontStyle();
	}

	/// <summary>
	/// Handles the underline button click event.
	/// Toggles underline decoration for the selected text in the description rich text box.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">Event data.</param>
	private void UnderlineButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(BaseMethodsCreateControl), nameof(UnderlineButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleUnderline();
	}

	/// <summary>
	/// Handles the subscript button click event.
	/// Toggles subscript baseline alignment for the selected text in the description rich text box.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">Event data.</param>
	private void SubscriptButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(BaseMethodsCreateControl), nameof(SubscriptButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleBaselineAlignment(BaselineAlignment.Subscript);
	}

	/// <summary>
	/// Handles the superscript button click event.
	/// Toggles superscript baseline alignment for the selected text in the description rich text box.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">Event data.</param>
	private void SuperscriptButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(BaseMethodsCreateControl), nameof(SuperscriptButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleBaselineAlignment(BaselineAlignment.Superscript);
	}

	/// <summary>
	/// Handles the bullets button click event.
	/// Toggles bulleted list formatting with disc markers for the selected paragraph in the description rich text box.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">Event data.</param>
	private void BulletsButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(BaseMethodsCreateControl), nameof(BulletsButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleList(TextMarkerStyle.Disc);
	}

	/// <summary>
	/// Handles the numbering button click event.
	/// Toggles numbered list formatting with decimal markers for the selected paragraph in the description rich text box.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">Event data.</param>
	private void NumberingButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(BaseMethodsCreateControl), nameof(NumberingButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleList(TextMarkerStyle.Decimal);
	}
}
