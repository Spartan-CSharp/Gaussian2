using System.ComponentModel;
using System.Globalization;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

using GaussianCommonLibrary.Models;

using GaussianWPF.Models;
using GaussianWPF.WPFHelpers;

using GaussianWPFLibrary.DataAccess;
using GaussianWPFLibrary.EventModels;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls.ElectronicStates;

/// <summary>
/// Interaction logic for ElectronicStatesEditControl.xaml
/// </summary>
public partial class ElectronicStatesEditControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<ElectronicStatesEditControl> _logger;
	private readonly IElectronicStatesEndpoint _endpoint;
	private SolidColorBrush _currentFontColor = new(Colors.Black);
	private SolidColorBrush _currentBackgroundColor = new(Colors.Yellow);
	private readonly List<SolidColorBrush> _recentFontColors = [];
	private readonly List<SolidColorBrush> _recentBackgroundColors = [];

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStatesEditControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="endpoint">The endpoint for Electronic State API operations.</param>
	public ElectronicStatesEditControl(ILogger<ElectronicStatesEditControl> logger, IElectronicStatesEndpoint endpoint)
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
	public event EventHandler<ChildControlEventArgs<ElectronicStatesEditControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the Electronic State view model being edited.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically populates the name, keyword, RTF content, and updates validation state.
	/// </remarks>
	public ElectronicStateViewModel? ElectronicState
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(ElectronicState));
		}
	}

	/// <summary>
	/// Gets or sets the identifier of the Electronic State to edit.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically loads the Electronic State data from the API.
	/// </remarks>
	public int ElectronicStateId
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(ElectronicStateId));
		}
	}

	/// <summary>
	/// Gets or sets the name of the Electronic State.
	/// </summary>
	public string ElectronicStateName
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(ElectronicStateName));
		}
	} = string.Empty;

	/// <summary>
	/// Gets or sets the keyword that identifies the Electronic State.
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
	/// Gets or sets a value indicating whether a valid Electronic State model is loaded.
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
	/// Gets a value indicating whether no Electronic State model is loaded.
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
	/// This property is automatically set to <see langword="true"/> when the name, keyword, and description are valid.
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
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(ElectronicStatesEditControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesEditControl), nameof(OnPropertyChanged));
		}
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the OnInitialized event and sets up data binding and property change notifications.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += ElectronicStatesEditControl_PropertyChanged;
		FontFamilyComboBox.ItemsSource = Fonts.SystemFontFamilies.Where(ff => ff.Source is "Segoe UI" or "Arial" or "Consolas" or "Georgia" or "Impact" or "Tahoma" or "Times New Roman" or "Verdana").OrderByDescending(ff => ff.Source.Equals("Segoe UI", StringComparison.OrdinalIgnoreCase)).ThenBy(ff => ff.Source);
		FontFamilyComboBox.SelectedItem = new FontFamily("Segoe UI");
		FontSizeComboBox.ItemsSource = new List<double>() { 32.0 / 3.0, 40.0 / 3.0, 16.0, 56.0 / 3.0, 24.0, 32.0, 48.0 };
		FontSizeComboBox.SelectedItem = 16.0;

		// Initialize color pickers
		InitializeColorPickers();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(OnInitialized));
		}
	}

	private void ElectronicStatesEditControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(ElectronicStatesEditControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(ElectronicState))
		{
			if (ElectronicState is not null)
			{
				ElectronicStateName = ElectronicState.Name ?? string.Empty;
				Keyword = ElectronicState.Keyword ?? string.Empty;
				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(ElectronicState.DescriptionRtf);
				ModelIsNotNull = true;
				CanSave = (ElectronicState.Name?.Length is > 0 and <= 200 || ElectronicState.Keyword?.Length is > 0 and <= 50) && (ElectronicState.DescriptionText?.Length is <= 4000 || string.IsNullOrEmpty(ElectronicState.DescriptionText));
			}
			else
			{
				ElectronicStateName = string.Empty;
				Keyword = string.Empty;
				DescriptionRichTextBox.Document.Blocks.Clear();
				ModelIsNotNull = false;
				CanSave = false;
			}
		}

		if (e.PropertyName is nameof(ElectronicStateId))
		{
			try
			{
				if (ElectronicStateId != 0 && (ElectronicState is null || ElectronicState.Id != ElectronicStateId))
				{
					ElectronicStateFullModel? results = _endpoint.GetByIdAsync(ElectronicStateId).Result;

					if (results is not null)
					{
						ElectronicState = new ElectronicStateViewModel(results);
						ElectronicStateName = ElectronicState.Name ?? string.Empty;
						Keyword = ElectronicState.Keyword ?? string.Empty;

						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(ElectronicState.DescriptionRtf);
						ModelIsNotNull = true;
						CanSave = (ElectronicState.Name?.Length is > 0 and <= 200 || ElectronicState.Keyword?.Length is > 0 and <= 50) && (ElectronicState.DescriptionText?.Length is <= 4000 || string.IsNullOrEmpty(ElectronicState.DescriptionText));
					}
					else
					{
						ElectronicState = null;
						ElectronicStateName = string.Empty;
						Keyword = string.Empty;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
						CanSave = false;
					}
				}
				else if (ElectronicStateId == 0)
				{
					ElectronicState = null;
					ElectronicStateName = string.Empty;
					Keyword = string.Empty;
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
					_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(ElectronicStatesEditControl), nameof(ElectronicStatesEditControl_PropertyChanged), sender, e);
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
							_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(ElectronicStatesEditControl), nameof(ElectronicStatesEditControl_PropertyChanged), sender, e);
						}

						return true;
					}

					// Return false for any other exception types to rethrow them in a new AggregateException
					return false;
				});
			}
		}

		if (e.PropertyName is (nameof(ElectronicStateName)) or (nameof(Keyword)))
		{
			CanSave = ElectronicStateName?.Length is > 0 and <= 200 || Keyword?.Length is > 0 and <= 50;
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}

		// Nothing to do for change events of ModelIsNull, ModelIsNotNull, IsErrorVisible and CanSave.

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(ElectronicStatesEditControl_PropertyChanged));
		}
	}

	private void SuperscriptToggleButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(SuperscriptToggleButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleSuperscript(SuperscriptToggleButton, SubscriptToggleButton);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(SuperscriptToggleButton_Click));
		}
	}

	private void SubscriptToggleButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(SubscriptToggleButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleSubscript(SuperscriptToggleButton, SubscriptToggleButton);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(SubscriptToggleButton_Click));
		}
	}

	private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(FontFamilyComboBox_SelectionChanged), sender, e);
		}

		if (FontFamilyComboBox.SelectedItem != null)
		{
			DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamilyComboBox.SelectedItem);
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(FontFamilyComboBox_SelectionChanged));
		}
	}

	private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(FontSizeComboBox_SelectionChanged), sender, e);
		}

		if (FontSizeComboBox.SelectedItem != null)
		{
			DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, FontSizeComboBox.SelectedItem);
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(FontSizeComboBox_SelectionChanged));
		}
	}

	private void FontColorButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(FontColorButton_Click), sender, e);
		}

		ApplyFontColor(_currentFontColor);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(FontColorButton_Click));
		}
	}

	private void FontColorPickerButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(FontColorPickerButton_Click), sender, e);
		}

		FontColorPopup.IsOpen = !FontColorPopup.IsOpen;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(FontColorPickerButton_Click));
		}
	}

	private void BackgroundColorButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(BackgroundColorButton_Click), sender, e);
		}

		ApplyBackgroundColor(_currentBackgroundColor);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(BackgroundColorButton_Click));
		}
	}

	private void BackgroundColorPickerButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(BackgroundColorPickerButton_Click), sender, e);
		}

		BackgroundColorPopup.IsOpen = !BackgroundColorPopup.IsOpen;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(BackgroundColorPickerButton_Click));
		}
	}

	private void DescriptionRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(DescriptionRichTextBox_SelectionChanged), sender, e);
		}

		object propertyToCheck = DescriptionRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
		BoldToggleButton.IsChecked = propertyToCheck != DependencyProperty.UnsetValue && propertyToCheck.Equals(FontWeights.Bold);
		propertyToCheck = DescriptionRichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
		ItalicToggleButton.IsChecked = propertyToCheck != DependencyProperty.UnsetValue && propertyToCheck.Equals(FontStyles.Italic);
		propertyToCheck = DescriptionRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
		UnderlineToggleButton.IsChecked = propertyToCheck != DependencyProperty.UnsetValue && propertyToCheck.Equals(TextDecorations.Underline);
		object alignmentProperty = DescriptionRichTextBox.Selection.GetPropertyValue(Inline.BaselineAlignmentProperty);
		SuperscriptToggleButton.IsChecked = alignmentProperty != DependencyProperty.UnsetValue && alignmentProperty.Equals(BaselineAlignment.Superscript);
		SubscriptToggleButton.IsChecked = alignmentProperty != DependencyProperty.UnsetValue && alignmentProperty.Equals(BaselineAlignment.Subscript);
		propertyToCheck = DescriptionRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
		FontFamilyComboBox.SelectedItem = propertyToCheck;
		propertyToCheck = DescriptionRichTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty);

		if (propertyToCheck != DependencyProperty.UnsetValue)
		{
			double fontSize = (double)propertyToCheck;

			// If superscript or subscript, show the "base" size (before 0.7x reduction)
			bool isSuperOrSubscript = alignmentProperty != DependencyProperty.UnsetValue && (alignmentProperty.Equals(BaselineAlignment.Superscript) || alignmentProperty.Equals(BaselineAlignment.Subscript));

			if (isSuperOrSubscript)
			{
				fontSize /= RTBHelpers._scriptFontSizeFactor; // Reverse the scaling to show base size
			}

			double roundedSize = Math.Round(fontSize);

			// Try to select the matching item in the ComboBox
			if (FontSizeComboBox.Items.Contains(roundedSize))
			{
				FontSizeComboBox.SelectedItem = roundedSize;
			}
			else
			{
				FontSizeComboBox.Text = roundedSize.ToString(CultureInfo.InvariantCulture);
			}
		}
		else
		{
			FontSizeComboBox.SelectedItem = null;
			FontSizeComboBox.Text = string.Empty;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(DescriptionRichTextBox_SelectionChanged));
		}
	}

	private void SaveButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(SaveButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;
			ElectronicState!.Name = ElectronicStateName;
			ElectronicState!.Keyword = Keyword;
			ElectronicState!.DescriptionRtf = DescriptionRichTextBox.GetRtfText();
			ElectronicState!.DescriptionText = DescriptionRichTextBox.GetPlainText();
			ElectronicStateFullModel model = ElectronicState!.ToFullModel();
			ElectronicStateFullModel? result = _endpoint.UpdateAsync(ElectronicStateId, model).Result;

			if (result is not null)
			{
				ElectronicState.LastUpdatedDate = result.LastUpdatedDate;
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesEditControl>("save", ElectronicStateId));
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(SaveButton_Click));
			}
		}
		catch (HttpIOException ex)
		{
			ErrorMessage = ex.Message;

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(ElectronicStatesEditControl), nameof(SaveButton_Click), sender, e);
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
						_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(ElectronicStatesEditControl), nameof(SaveButton_Click), sender, e);
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesEditControl>("index", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(BackToIndexButton_Click));
		}
	}

	private void InitializeColorPickers()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called.", nameof(ElectronicStatesEditControl), nameof(InitializeColorPickers));
		}

		// Initialize font color picker
		foreach (Color color in Constants.FontColors)
		{
			Button colorButton = CreateColorButton(color, true);
			_ = FontColorGrid.Children.Add(colorButton);
		}

		// Initialize background color picker
		foreach (Color color in Constants.BackgroundColors)
		{
			Button colorButton = CreateColorButton(color, false);
			_ = BackgroundColorGrid.Children.Add(colorButton);
		}

		// Initialize recent colors sections (empty initially)
		UpdateRecentColors(true);
		UpdateRecentColors(false);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesEditControl), nameof(InitializeColorPickers));
		}
	}

	private Button CreateColorButton(Color color, bool isFontColor)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with Color = {Color} and IsFontColor = {IsFontColor}.", nameof(ElectronicStatesEditControl), nameof(CreateColorButton), color, isFontColor);
		}

		Button button = new()
		{
			Width = 20,
			Height = 20,
			Margin = new Thickness(2),
			Background = new SolidColorBrush(color),
			BorderBrush = new SolidColorBrush(Colors.Gray),
			BorderThickness = new Thickness(1),
			ToolTip = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}"
		};

		button.Click += (sender, e) =>
		{
			if (isFontColor)
			{
				ApplyFontColor(new SolidColorBrush(color));
				FontColorPopup.IsOpen = false;
			}
			else
			{
				ApplyBackgroundColor(new SolidColorBrush(color));
				BackgroundColorPopup.IsOpen = false;
			}
		};

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning Button {Button}.", nameof(ElectronicStatesEditControl), nameof(InitializeColorPickers), button);
		}

		return button;
	}

	private void ApplyFontColor(SolidColorBrush color)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with Color = {Color}.", nameof(ElectronicStatesEditControl), nameof(ApplyFontColor), color);
		}

		if (!DescriptionRichTextBox.Selection.IsEmpty)
		{
			DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.ForegroundProperty, color);
		}

		_currentFontColor = color;
		FontColorRectangle.Fill = color;

		// Add to recent colors
		AddToRecentColors(color, true);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesEditControl), nameof(ApplyFontColor));
		}
	}

	private void ApplyBackgroundColor(SolidColorBrush color)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with Color = {Color}.", nameof(ElectronicStatesEditControl), nameof(ApplyBackgroundColor), color);
		}

		if (!DescriptionRichTextBox.Selection.IsEmpty)
		{
			DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.BackgroundProperty, color);
		}

		_currentBackgroundColor = color;
		BackgroundColorRectangle.Fill = color;

		// Add to recent colors
		AddToRecentColors(color, false);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesEditControl), nameof(ApplyBackgroundColor));
		}
	}

	private void AddToRecentColors(SolidColorBrush color, bool isFontColor)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with Color = {Color} and IsFontColor = {IsFontColor}.", nameof(ElectronicStatesEditControl), nameof(AddToRecentColors), color, isFontColor);
		}

		List<SolidColorBrush> recentList = isFontColor ? _recentFontColors : _recentBackgroundColors;

		// Remove if already exists
		SolidColorBrush? existing = recentList.FirstOrDefault(c => c.Color == color.Color);
		if (existing != null)
		{
			_ = recentList.Remove(existing);
		}

		// Add to front
		recentList.Insert(0, color);

		// Keep only 10 recent colors
		if (recentList.Count > 10)
		{
			recentList.RemoveAt(10);
		}

		// Update UI
		UpdateRecentColors(isFontColor);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesEditControl), nameof(AddToRecentColors));
		}
	}

	private void UpdateRecentColors(bool isFontColor)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with IsFontColor = {IsFontColor}.", nameof(ElectronicStatesEditControl), nameof(UpdateRecentColors), isFontColor);
		}

		UniformGrid grid = isFontColor ? FontColorRecentGrid : BackgroundColorRecentGrid;
		List<SolidColorBrush> recentList = isFontColor ? _recentFontColors : _recentBackgroundColors;

		grid.Children.Clear();

		foreach (SolidColorBrush colorBrush in recentList)
		{
			Button colorButton = CreateColorButton(colorBrush.Color, isFontColor);
			_ = grid.Children.Add(colorButton);
		}

		// Fill remaining slots with empty placeholders
		for (int i = recentList.Count; i < 10; i++)
		{
			Border placeholder = new()
			{
				Width = 20,
				Height = 20,
				Margin = new Thickness(2),
				Background = new SolidColorBrush(Colors.Transparent),
				BorderBrush = new SolidColorBrush(Colors.LightGray),
				BorderThickness = new Thickness(1)
			};

			_ = grid.Children.Add(placeholder);
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesEditControl), nameof(UpdateRecentColors));
		}
	}
}
