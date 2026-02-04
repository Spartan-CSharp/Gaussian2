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

namespace GaussianWPF.Controls.MethodFamilies;

/// <summary>
/// Interaction logic for MethodFamiliesCreateControl.xaml
/// </summary>
public partial class MethodFamiliesCreateControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<MethodFamiliesCreateControl> _logger;
	private readonly IMethodFamiliesEndpoint _endpoint;
	private SolidColorBrush _currentFontColor = new(Colors.Black);
	private SolidColorBrush _currentBackgroundColor = new(Colors.Yellow);
	private readonly List<SolidColorBrush> _recentFontColors = [];
	private readonly List<SolidColorBrush> _recentBackgroundColors = [];

	/// <summary>
	/// Initializes a new instance of the <see cref="MethodFamiliesCreateControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="endpoint">The endpoint for method family API operations.</param>
	public MethodFamiliesCreateControl(ILogger<MethodFamiliesCreateControl> logger, IMethodFamiliesEndpoint endpoint)
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
	public event EventHandler<ChildControlEventArgs<MethodFamiliesCreateControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the method family view model being created.
	/// </summary>
	public MethodFamilyViewModel MethodFamily
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(MethodFamily));
		}
	} = new();

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
	/// Gets or sets a value indicating whether the create button should be enabled.
	/// </summary>
	/// <remarks>
	/// This property is automatically set to <see langword="true"/> when the name is valid.
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
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(MethodFamiliesCreateControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(MethodFamiliesCreateControl), nameof(OnPropertyChanged));
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(MethodFamiliesCreateControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += MethodFamiliesCreateControl_PropertyChanged;
		FontFamilyComboBox.ItemsSource = Fonts.SystemFontFamilies.Where(ff => ff.Source is "Segoe UI" or "Arial" or "Consolas" or "Georgia" or "Impact" or "Tahoma" or "Times New Roman" or "Verdana").OrderByDescending(ff => ff.Source.Equals("Segoe UI", StringComparison.OrdinalIgnoreCase)).ThenBy(ff => ff.Source);
		FontFamilyComboBox.SelectedItem = new FontFamily("Segoe UI");
		FontSizeComboBox.ItemsSource = new List<double>() { 32.0 / 3.0, 40.0 / 3.0, 16.0, 56.0 / 3.0, 24.0, 32.0, 48.0 };
		FontSizeComboBox.SelectedItem = 16.0;
		// Initialize color pickers
		InitializeColorPickers();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(MethodFamiliesCreateControl), nameof(OnInitialized));
		}
	}

	private void MethodFamiliesCreateControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MethodFamiliesCreateControl), nameof(MethodFamiliesCreateControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(MethodFamily))
		{
			if (MethodFamily is not null)
			{
				MethodFamilyName = MethodFamily.Name;
				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(MethodFamily.DescriptionRtf);
				CanSave = MethodFamily.Name?.Length is > 0 and <= 200 && (MethodFamily.DescriptionText?.Length is <= 2000 || string.IsNullOrEmpty(MethodFamily.DescriptionText));
			}
			else
			{
				MethodFamilyName = string.Empty;
				DescriptionRichTextBox.Document.Blocks.Clear();
				CanSave = false;
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

		// Nothing to do for change events of IsErrorVisible and CanSave.

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(MethodFamiliesCreateControl), nameof(MethodFamiliesCreateControl_PropertyChanged));
		}
	}

	private void SuperscriptToggleButton_Click(object sender, RoutedEventArgs e)
	{
		TextSelection selection = DescriptionRichTextBox.Selection;
		if (selection.IsEmpty)
		{
			return;
		}

		// Get current baseline alignment
		object currentAlignment = selection.GetPropertyValue(Inline.BaselineAlignmentProperty);

		// Toggle superscript
		BaselineAlignment newAlignment = (currentAlignment != DependencyProperty.UnsetValue &&
										 currentAlignment.Equals(BaselineAlignment.Superscript))
			? BaselineAlignment.Baseline
			: BaselineAlignment.Superscript;

		selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, newAlignment);

		// Optionally reduce font size for better appearance
		if (newAlignment == BaselineAlignment.Superscript)
		{
			object currentSize = selection.GetPropertyValue(Inline.FontSizeProperty);
			if (currentSize != DependencyProperty.UnsetValue)
			{
				double fontSize = (double)currentSize;
				selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSize * 0.7);
			}
		}
		else
		{
			// Reset font size when removing superscript
			object currentSize = selection.GetPropertyValue(Inline.FontSizeProperty);
			if (currentSize != DependencyProperty.UnsetValue)
			{
				double fontSize = (double)currentSize;
				selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSize / 0.7);
			}
		}

		// Update button state
		SuperscriptToggleButton.IsChecked = newAlignment == BaselineAlignment.Superscript;
		SubscriptToggleButton.IsChecked = false; // Can't be both
	}

	private void SubscriptToggleButton_Click(object sender, RoutedEventArgs e)
	{
		TextSelection selection = DescriptionRichTextBox.Selection;
		if (selection.IsEmpty)
		{
			return;
		}

		// Get current baseline alignment
		object currentAlignment = selection.GetPropertyValue(Inline.BaselineAlignmentProperty);

		// Toggle subscript
		BaselineAlignment newAlignment = (currentAlignment != DependencyProperty.UnsetValue &&
										 currentAlignment.Equals(BaselineAlignment.Subscript))
			? BaselineAlignment.Baseline
			: BaselineAlignment.Subscript;

		selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, newAlignment);

		// Optionally reduce font size for better appearance
		if (newAlignment == BaselineAlignment.Subscript)
		{
			object currentSize = selection.GetPropertyValue(Inline.FontSizeProperty);
			if (currentSize != DependencyProperty.UnsetValue)
			{
				double fontSize = (double)currentSize;
				selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSize * 0.7);
			}
		}
		else
		{
			// Reset font size when removing subscript
			object currentSize = selection.GetPropertyValue(Inline.FontSizeProperty);
			if (currentSize != DependencyProperty.UnsetValue)
			{
				double fontSize = (double)currentSize;
				selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSize / 0.7);
			}
		}

		// Update button state
		SubscriptToggleButton.IsChecked = newAlignment == BaselineAlignment.Subscript;
		SuperscriptToggleButton.IsChecked = false; // Can't be both
	}

	private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (FontFamilyComboBox.SelectedItem != null)
		{
			DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamilyComboBox.SelectedItem);
		}
	}

	private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (FontSizeComboBox.SelectedItem != null)
		{
			DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, FontSizeComboBox.SelectedItem);
		}
	}

	private void FontColorButton_Click(object sender, RoutedEventArgs e)
	{
		ApplyFontColor(_currentFontColor);
	}

	private void FontColorPickerButton_Click(object sender, RoutedEventArgs e)
	{
		FontColorPopup.IsOpen = !FontColorPopup.IsOpen;
	}

	private void BackgroundColorButton_Click(object sender, RoutedEventArgs e)
	{
		ApplyBackgroundColor(_currentBackgroundColor);
	}

	private void BackgroundColorPickerButton_Click(object sender, RoutedEventArgs e)
	{
		BackgroundColorPopup.IsOpen = !BackgroundColorPopup.IsOpen;
	}

	private void DescriptionRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
	{
		object temp = DescriptionRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
		BoldToggleButton.IsChecked = temp != DependencyProperty.UnsetValue && temp.Equals(FontWeights.Bold);
		temp = DescriptionRichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
		ItalicToggleButton.IsChecked = temp != DependencyProperty.UnsetValue && temp.Equals(FontStyles.Italic);
		temp = DescriptionRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
		UnderlineToggleButton.IsChecked = temp != DependencyProperty.UnsetValue && temp.Equals(TextDecorations.Underline);

		object alignmentTemp = DescriptionRichTextBox.Selection.GetPropertyValue(Inline.BaselineAlignmentProperty);
		SuperscriptToggleButton.IsChecked = alignmentTemp != DependencyProperty.UnsetValue &&
			alignmentTemp.Equals(BaselineAlignment.Superscript);
		SubscriptToggleButton.IsChecked = alignmentTemp != DependencyProperty.UnsetValue &&
			alignmentTemp.Equals(BaselineAlignment.Subscript);

		temp = DescriptionRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
		FontFamilyComboBox.SelectedItem = temp;

		temp = DescriptionRichTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty);
		if (temp != DependencyProperty.UnsetValue)
		{
			double fontSize = (double)temp;

			// If superscript or subscript, show the "base" size (before 0.7x reduction)
			bool isSuperOrSubscript = alignmentTemp != DependencyProperty.UnsetValue &&
				(alignmentTemp.Equals(BaselineAlignment.Superscript) ||
				 alignmentTemp.Equals(BaselineAlignment.Subscript));

			if (isSuperOrSubscript)
			{
				fontSize /= 0.7; // Reverse the scaling to show base size
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

		//// Update color previews based on current selection
		//object foreground = DescriptionRichTextBox.Selection.GetPropertyValue(Inline.ForegroundProperty);
		//if (foreground is SolidColorBrush foregroundBrush)
		//{
		//	FontColorRectangle.Fill = foregroundBrush;
		//}

		//object background = DescriptionRichTextBox.Selection.GetPropertyValue(Inline.BackgroundProperty);
		//if (background is SolidColorBrush backgroundBrush)
		//{
		//	BackgroundColorRectangle.Fill = backgroundBrush;
		//}
	}

	private void CreateButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MethodFamiliesCreateControl), nameof(CreateButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;
			MethodFamily.Name = MethodFamilyName;
			MethodFamily.DescriptionRtf = DescriptionRichTextBox.GetRtfText();
			MethodFamily.DescriptionText = DescriptionRichTextBox.GetPlainText();
			MethodFamilyFullModel model = MethodFamily.ToFullModel();
			MethodFamilyFullModel? result = _endpoint.CreateAsync(model).Result;

			if (result is not null)
			{
				MethodFamily.Id = result.Id;
				MethodFamily.CreatedDate = result.CreatedDate;
				MethodFamily.LastUpdatedDate = result.LastUpdatedDate;
				MethodFamily.Archived = result.Archived;
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<MethodFamiliesCreateControl>("create", MethodFamily.Id));
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(MethodFamiliesCreateControl), nameof(CreateButton_Click));
			}
		}
		catch (HttpIOException ex)
		{
			ErrorMessage = ex.Message;

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(MethodFamiliesCreateControl), nameof(CreateButton_Click), sender, e);
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
						_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(MethodFamiliesCreateControl), nameof(CreateButton_Click), sender, e);
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MethodFamiliesCreateControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<MethodFamiliesCreateControl>("index", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(MethodFamiliesCreateControl), nameof(BackToIndexButton_Click));
		}
	}

	private void InitializeColorPickers()
	{
		// Initialize font color picker
		foreach (Color color in Constants.FontColors)
		{
			Button colorButton = CreateColorButton(color, true);
			FontColorGrid.Children.Add(colorButton);
		}

		// Initialize background color picker
		foreach (Color color in Constants.BackgroundColors)
		{
			Button colorButton = CreateColorButton(color, false);
			BackgroundColorGrid.Children.Add(colorButton);
		}

		// Initialize recent colors sections (empty initially)
		UpdateRecentColors(true);
		UpdateRecentColors(false);
	}

	private Button CreateColorButton(Color color, bool isFontColor)
	{
		Button button = new()
		{
			Width = 20,
			Height = 20,
			Margin = new Thickness(2),
			Background = new SolidColorBrush(color),
			BorderBrush = new SolidColorBrush(Colors.Gray),
			BorderThickness = new Thickness(1),
			ToolTip = $"#{color.R:X2}{color.G:X2}{color.B:X2}"
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

		return button;
	}

	private void ApplyFontColor(SolidColorBrush color)
	{
		if (!DescriptionRichTextBox.Selection.IsEmpty)
		{
			DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.ForegroundProperty, color);
		}

		_currentFontColor = color;
		FontColorRectangle.Fill = color;

		// Add to recent colors
		AddToRecentColors(color, true);
	}

	private void ApplyBackgroundColor(SolidColorBrush color)
	{
		if (!DescriptionRichTextBox.Selection.IsEmpty)
		{
			DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.BackgroundProperty, color);
		}

		_currentBackgroundColor = color;
		BackgroundColorRectangle.Fill = color;

		// Add to recent colors
		AddToRecentColors(color, false);
	}

	private void AddToRecentColors(SolidColorBrush color, bool isFontColor)
	{
		List<SolidColorBrush> recentList = isFontColor ? _recentFontColors : _recentBackgroundColors;

		// Remove if already exists
		SolidColorBrush? existing = recentList.FirstOrDefault(c => c.Color == color.Color);
		if (existing != null)
		{
			recentList.Remove(existing);
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
	}

	private void UpdateRecentColors(bool isFontColor)
	{
		UniformGrid grid = isFontColor ? FontColorRecentGrid : BackgroundColorRecentGrid;
		List<SolidColorBrush> recentList = isFontColor ? _recentFontColors : _recentBackgroundColors;

		grid.Children.Clear();

		foreach (SolidColorBrush colorBrush in recentList)
		{
			Button colorButton = CreateColorButton(colorBrush.Color, isFontColor);
			grid.Children.Add(colorButton);
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
			grid.Children.Add(placeholder);
		}
	}
}
