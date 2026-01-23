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
/// Interaction logic for MethodFamiliesCreateControl.xaml
/// A user control that provides functionality for creating new Method Families.
/// Implements INotifyPropertyChanged to support data binding with the XAML view.
/// </summary>
public partial class MethodFamiliesCreateControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<MethodFamiliesCreateControl> _logger;
	private readonly ILoggedInUserModel _loggedInUser;
	private readonly IApiHelper _apiHelper;
	private readonly IMethodFamiliesEndpoint _endpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="MethodFamiliesCreateControl"/> class.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="loggedInUser">The currently logged-in user model.</param>
	/// <param name="apiHelper">The API helper for making HTTP requests.</param>
	/// <param name="endpoint">The endpoint for Method Families API operations.</param>
	public MethodFamiliesCreateControl(ILogger<MethodFamiliesCreateControl> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, IMethodFamiliesEndpoint endpoint)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_endpoint = endpoint;

		InitializeComponent();
		DataContext = this;
		PropertyChanged += MethodFamiliesCreateControl_PropertyChanged;
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Occurs when a child control event is raised, typically for navigation or state changes.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<MethodFamiliesCreateControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the Method Family view model being created.
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
	/// This property is bound to the UI and triggers validation when changed.
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
			_logger.LogDebug("{UserControl} {Method} with {PropertyName}", nameof(MethodFamiliesCreateControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void MethodFamiliesCreateControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesCreateControl), nameof(MethodFamiliesCreateControl_PropertyChanged), sender, e);
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

	private void CreateButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesCreateControl), nameof(CreateButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;

			string descriptionRtf = DescriptionRichTextBox.GetRtfText();
			string descriptionText = DescriptionRichTextBox.GetPlainText();
			MethodFamily = new MethodFamilyViewModel
			{
				MethodFamilyName = MethodFamilyName,
				DescriptionRtf = descriptionRtf,
				DescriptionText = descriptionText
			};

			MethodFamilyFullModel model = MethodFamily.ToFullModel();
			MethodFamilyFullModel? result = _endpoint.CreateAsync(model).Result;

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} returned {Result}", nameof(_endpoint.CreateAsync), result);
			}

			if (result is not null)
			{
				MethodFamily.Id = result.Id;
				MethodFamily.CreatedDate = result.CreatedDate;
				MethodFamily.LastUpdatedDate = result.LastUpdatedDate;
				MethodFamily.Archived = result.Archived;
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<MethodFamiliesCreateControl>("create", null));
			}
		}
		catch (HttpIOException ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(MethodFamiliesCreateControl), nameof(CreateButton_Click));
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
						_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(MethodFamiliesCreateControl), nameof(CreateButton_Click));
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
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesCreateControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<MethodFamiliesCreateControl>("index", null));
	}

	private void BoldButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(MethodFamiliesCreateControl), nameof(BoldButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleFontWeight();
	}

	private void ItalicButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(MethodFamiliesCreateControl), nameof(ItalicButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleFontStyle();
	}

	private void UnderlineButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(MethodFamiliesCreateControl), nameof(UnderlineButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleUnderline();
	}

	private void SubscriptButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(MethodFamiliesCreateControl), nameof(SubscriptButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleBaselineAlignment(BaselineAlignment.Subscript);
	}

	private void SuperscriptButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(MethodFamiliesCreateControl), nameof(SuperscriptButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleBaselineAlignment(BaselineAlignment.Superscript);
	}

	private void BulletsButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(MethodFamiliesCreateControl), nameof(BulletsButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleList(TextMarkerStyle.Disc);
	}

	private void NumberingButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}",
				nameof(MethodFamiliesCreateControl), nameof(NumberingButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleList(TextMarkerStyle.Decimal);
	}
}
