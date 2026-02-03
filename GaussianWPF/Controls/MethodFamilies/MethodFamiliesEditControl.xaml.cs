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

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls.MethodFamilies;

/// <summary>
/// Interaction logic for MethodFamiliesEditControl.xaml
/// </summary>
public partial class MethodFamiliesEditControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<MethodFamiliesEditControl> _logger;
	private readonly IMethodFamiliesEndpoint _endpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="MethodFamiliesEditControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="endpoint">The endpoint for method family API operations.</param>
	public MethodFamiliesEditControl(ILogger<MethodFamiliesEditControl> logger, IMethodFamiliesEndpoint endpoint)
	{
		_logger = logger;
		_endpoint = endpoint;

		InitializeComponent();
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Occurs when a child control event is raised to request navigation to another view.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<MethodFamiliesEditControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the method family view model being edited.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically populates the name, RTF content, and updates validation state.
	/// </remarks>
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
	/// Gets or sets the identifier of the method family to edit.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically loads the method family data from the API.
	/// </remarks>
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
	/// Gets or sets the name of the method family.
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
	/// Gets or sets a value indicating whether a valid method family model is loaded.
	/// </summary>
	/// <remarks>
	/// Setting this property also updates the <see cref="ModelIsNull"/> property.
	/// </remarks>
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
	/// Gets a value indicating whether no method family model is loaded.
	/// </summary>
	/// <remarks>
	/// This property returns the inverse of <see cref="ModelIsNotNull"/>.
	/// </remarks>
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
	/// </summary>
	/// <remarks>
	/// This property is automatically set based on whether <see cref="ErrorMessage"/> has a value.
	/// </remarks>
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
	/// </summary>
	/// <remarks>
	/// This property is automatically set to <see langword="true"/> when the name and description are valid.
	/// </remarks>
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
	/// <param name="propertyName">The name of the property that changed. This parameter is optional and can be automatically populated by the caller member name.</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(MethodFamiliesEditControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(MethodFamiliesEditControl), nameof(OnPropertyChanged));
		}
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the OnInitialzied event and sets up data binding and property change notifications.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(MethodFamiliesEditControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += MethodFamiliesEditControl_PropertyChanged;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(MethodFamiliesEditControl), nameof(OnInitialized));
		}
	}

	private void MethodFamiliesEditControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MethodFamiliesEditControl), nameof(MethodFamiliesEditControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(MethodFamily))
		{
			if (MethodFamily is not null)
			{
				MethodFamilyName = MethodFamily.Name;
				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(MethodFamily.DescriptionRtf);
				ModelIsNotNull = true;
				CanSave = MethodFamily.Name?.Length is > 0 and <= 200 && (MethodFamily.DescriptionText?.Length is <= 2000 || string.IsNullOrEmpty(MethodFamily.DescriptionText));
			}
			else
			{
				MethodFamilyName = string.Empty;
				DescriptionRichTextBox.Document.Blocks.Clear();
				ModelIsNotNull = false;
				CanSave = false;
			}
		}

		if (e.PropertyName is nameof(MethodFamilyId))
		{
			try
			{
				if (MethodFamilyId != 0 && (MethodFamily is null || MethodFamily.Id != MethodFamilyId))
				{
					MethodFamilyFullModel? results = _endpoint.GetByIdAsync(MethodFamilyId).Result;

					if (results is not null)
					{
						MethodFamily = new MethodFamilyViewModel(results);
						MethodFamilyName = MethodFamily.Name;
						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(MethodFamily.DescriptionRtf);
						ModelIsNotNull = true;
						CanSave = MethodFamily.Name?.Length is > 0 and <= 200 && (MethodFamily.DescriptionText?.Length is <= 2000 || string.IsNullOrEmpty(MethodFamily.DescriptionText));
					}
					else
					{
						MethodFamily = null;
						MethodFamilyName = string.Empty;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
						CanSave = false;
					}
				}
				else if (MethodFamilyId == 0)
				{
					MethodFamily = null;
					MethodFamilyName = string.Empty;
					DescriptionRichTextBox.Document.Blocks.Clear();
					ModelIsNotNull = false;
					CanSave = false;
				}
			}
			catch (HttpIOException ex)
			{
				ErrorMessage = ex.Message;

				if (_logger.IsEnabled(LogLevel.Error))
				{
					_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(MethodFamiliesEditControl), nameof(MethodFamiliesEditControl_PropertyChanged), sender, e);
				}
			}
			catch (AggregateException ae)
			{
				ae.Handle(ex =>
				{
					if (ex is HttpIOException)
					{
						ErrorMessage = ex.Message;

						if (_logger.IsEnabled(LogLevel.Error))
						{
							_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(MethodFamiliesEditControl), nameof(MethodFamiliesEditControl_PropertyChanged), sender, e);
						}

						return true;
					}

					// Return false for any other exception types to rethrow them in a new AggregateException
					return false;
				});
			}
		}

		if (e.PropertyName is nameof(MethodFamilyName))
		{
			CanSave = MethodFamilyName?.Length is > 0 and <= 200;
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}

		// Nothing to do for change events of ModelIsNull, ModelIsNotNull, IsErrorVisible and CanSave.

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(MethodFamiliesEditControl), nameof(MethodFamiliesEditControl_PropertyChanged));
		}
	}

	private void SaveButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MethodFamiliesEditControl), nameof(SaveButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;
			MethodFamily!.Name = MethodFamilyName;
			MethodFamily!.DescriptionRtf = DescriptionRichTextBox.GetRtfText();
			MethodFamily!.DescriptionText = DescriptionRichTextBox.GetPlainText();
			MethodFamilyFullModel model = MethodFamily!.ToFullModel();
			MethodFamilyFullModel? result = _endpoint.UpdateAsync(MethodFamilyId, model).Result;

			if (result is not null)
			{
				MethodFamily.LastUpdatedDate = result.LastUpdatedDate;
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<MethodFamiliesEditControl>("save", null));
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(MethodFamiliesEditControl), nameof(SaveButton_Click));
			}
		}
		catch (HttpIOException ex)
		{
			ErrorMessage = ex.Message;

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(MethodFamiliesEditControl), nameof(SaveButton_Click), sender, e);
			}
		}
		catch (AggregateException ae)
		{
			ae.Handle(ex =>
			{
				if (ex is HttpIOException)
				{
					ErrorMessage = ex.Message;

					if (_logger.IsEnabled(LogLevel.Error))
					{
						_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(MethodFamiliesEditControl), nameof(SaveButton_Click), sender, e);
					}

					return true;
				}

				// Return false for any other exception types to rethrow them in a new AggregateException
				return false;
			});
		}
	}

	private void BackToIndexButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MethodFamiliesEditControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<MethodFamiliesEditControl>("index", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(MethodFamiliesEditControl), nameof(BackToIndexButton_Click));
		}
	}

	private void BoldButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(MethodFamiliesEditControl), nameof(BoldButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleFontWeight();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(MethodFamiliesEditControl), nameof(BoldButton_Click));
		}
	}

	private void ItalicButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(MethodFamiliesEditControl), nameof(ItalicButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleFontStyle();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(MethodFamiliesEditControl), nameof(ItalicButton_Click));
		}
	}

	private void UnderlineButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(MethodFamiliesEditControl), nameof(UnderlineButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleUnderline();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(MethodFamiliesEditControl), nameof(UnderlineButton_Click));
		}
	}

	private void SubscriptButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(MethodFamiliesEditControl), nameof(SubscriptButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleBaselineAlignment(BaselineAlignment.Subscript);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(MethodFamiliesEditControl), nameof(SubscriptButton_Click));
		}
	}

	private void SuperscriptButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(MethodFamiliesEditControl), nameof(SuperscriptButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleBaselineAlignment(BaselineAlignment.Superscript);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(MethodFamiliesEditControl), nameof(SuperscriptButton_Click));
		}
	}

	private void BulletsButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(MethodFamiliesEditControl), nameof(BulletsButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleList(TextMarkerStyle.Disc);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(MethodFamiliesEditControl), nameof(BulletsButton_Click));
		}
	}

	private void NumberingButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(MethodFamiliesEditControl), nameof(NumberingButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleList(TextMarkerStyle.Decimal);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(MethodFamiliesEditControl), nameof(NumberingButton_Click));
		}
	}
}
