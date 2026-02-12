using System.Collections.ObjectModel;
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

namespace GaussianWPF.Controls.ElectronicStatesMethodFamilies;

/// <summary>
/// Interaction logic for ElectronicStatesMethodFamiliesCreateControl.xaml
/// </summary>
public partial class ElectronicStatesMethodFamiliesCreateControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<ElectronicStatesMethodFamiliesCreateControl> _logger;
	private readonly IElectronicStatesMethodFamiliesEndpoint _electronicStatesMethodFamiliesEndpoint;
	private readonly IElectronicStatesEndpoint _electronicStatesEndpoint;
	private readonly IMethodFamiliesEndpoint _methodFamiliesEndpoint;
	private SolidColorBrush _currentFontColor = new(Colors.Black);
	private SolidColorBrush _currentBackgroundColor = new(Colors.Yellow);
	private readonly List<SolidColorBrush> _recentFontColors = [];
	private readonly List<SolidColorBrush> _recentBackgroundColors = [];

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStatesMethodFamiliesCreateControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="electronicStatesMethodFamiliesEndpoint">The endpoint for Electronic State/Method Family Combination API operations.</param>
	/// <param name="electronicStatesEndpoint">The endpoint for Electronic State API operations.</param>
	/// <param name="methodFamiliesEndpoint">The endpoint for Method Family API operations.</param>
	public ElectronicStatesMethodFamiliesCreateControl(ILogger<ElectronicStatesMethodFamiliesCreateControl> logger, IElectronicStatesMethodFamiliesEndpoint electronicStatesMethodFamiliesEndpoint, IElectronicStatesEndpoint electronicStatesEndpoint, IMethodFamiliesEndpoint methodFamiliesEndpoint)
	{
		_logger = logger;
		_electronicStatesMethodFamiliesEndpoint = electronicStatesMethodFamiliesEndpoint;
		_electronicStatesEndpoint = electronicStatesEndpoint;
		_methodFamiliesEndpoint = methodFamiliesEndpoint;

		InitializeComponent();
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Occurs when a child control event is raised to request navigation to another view.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<ElectronicStatesMethodFamiliesCreateControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the Electronic State/Method Family Combination view model being created.
	/// </summary>
	public ElectronicStateMethodFamilyViewModel ElectronicStateMethodFamily
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(ElectronicStateMethodFamily));
		}
	} = new();

	/// <summary>
	/// Gets or sets the name of the Electronic State/Method Family Combination.
	/// </summary>
	public string ElectronicStateMethodFamilyName
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(ElectronicStateMethodFamilyName));
		}
	} = string.Empty;

	/// <summary>
	/// Gets or sets the keyword that identifies the Electronic State/Method Family Combination.
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
	/// Gets or sets the selected Electronic State for the Electronic State/Method Family Combination.
	/// </summary>
	public ElectronicStateRecord? SelectedElectronicState
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(SelectedElectronicState));
		}
	}

	/// <summary>
	/// Gets or sets the observable collection of Electronic States available for selection.
	/// </summary>
	/// <remarks>
	/// This collection supports data binding for ComboBox controls in WPF.
	/// </remarks>
	public ObservableCollection<ElectronicStateRecord> ElectronicStateList
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(ElectronicStateList));
		}
	} = [];

	/// <summary>
	/// Gets or sets the selected Method Family for the Electronic State/Method Family Combination.
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
	/// Gets or sets the observable collection of Method Families available for selection.
	/// </summary>
	/// <remarks>
	/// This collection supports data binding for ComboBox controls in WPF.
	/// </remarks>
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
	/// This property is automatically set to <see langword="true"/> when the keyword and selected Method Family are valid.
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
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(OnPropertyChanged));
		}
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the OnInitialized event, sets up data binding, and loads the list of Method Families from the API.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += ElectronicStatesMethodFamiliesCreateControl_PropertyChanged;
		FontFamilyComboBox.ItemsSource = Fonts.SystemFontFamilies.Where(ff => ff.Source is "Segoe UI" or "Arial" or "Georgia" or "Impact" or "Tahoma" or "Times New Roman" or "Verdana").OrderByDescending(ff => ff.Source.Equals("Segoe UI", StringComparison.OrdinalIgnoreCase)).ThenBy(ff => ff.Source);
		FontFamilyComboBox.SelectedItem = new FontFamily("Segoe UI");
		FontSizeComboBox.ItemsSource = new List<double>() { 32.0 / 3.0, 40.0 / 3.0, 16.0, 56.0 / 3.0, 24.0, 32.0, 48.0 };
		FontSizeComboBox.SelectedItem = 16.0;

		// Initialize color pickers
		InitializeColorPickers();
		List<ElectronicStateRecord>? electronicStates = _electronicStatesEndpoint.GetListAsync().Result;
		List<MethodFamilyRecord>? methodFamilies = _methodFamiliesEndpoint.GetListAsync().Result;

		if (electronicStates is not null && electronicStates.Count > 0)
		{
			foreach (ElectronicStateRecord item in electronicStates)
			{
				ElectronicStateMethodFamily.ElectronicStateList.Add(item);
				ElectronicStateList.Add(item);
			}
		}

		if (methodFamilies is not null && methodFamilies.Count > 0)
		{
			foreach (MethodFamilyRecord item in methodFamilies)
			{
				ElectronicStateMethodFamily.MethodFamilyList.Add(item);
				MethodFamilyList.Add(item);
			}
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(OnInitialized));
		}
	}

	private void ElectronicStatesMethodFamiliesCreateControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(ElectronicStatesMethodFamiliesCreateControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(ElectronicStateMethodFamily))
		{
			if (ElectronicStateMethodFamily is not null)
			{
				ElectronicStateMethodFamilyName = ElectronicStateMethodFamily.Name ?? string.Empty;
				Keyword = ElectronicStateMethodFamily.Keyword ?? string.Empty;
				SelectedElectronicState = ElectronicStateList.First(x => x.Id == ElectronicStateMethodFamily.ElectronicState?.Id);
				SelectedMethodFamily = MethodFamilyList.FirstOrDefault(x => x.Id == ElectronicStateMethodFamily.MethodFamily?.Id);

				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(ElectronicStateMethodFamily.DescriptionRtf);
				CanSave = (ElectronicStateMethodFamily.Name?.Length is > 0 and <= 200 || ElectronicStateMethodFamily.Keyword?.Length is > 0 and <= 50) && ElectronicStateMethodFamily.ElectronicState is not null && (ElectronicStateMethodFamily.DescriptionText?.Length is <= 4000 || string.IsNullOrEmpty(ElectronicStateMethodFamily.DescriptionText));
			}
			else
			{
				ElectronicStateMethodFamilyName = string.Empty;
				Keyword = string.Empty;
				SelectedElectronicState = null;
				SelectedMethodFamily = null;
				DescriptionRichTextBox.Document.Blocks.Clear();
				CanSave = false;
			}
		}

		if (e.PropertyName is nameof(ElectronicStateMethodFamilyName) or nameof(Keyword) or nameof(SelectedElectronicState) or nameof(SelectedMethodFamily))
		{
			CanSave = (ElectronicStateMethodFamilyName?.Length is > 0 and <= 200 || Keyword?.Length is > 0 and <= 50) && SelectedElectronicState is not null && (DescriptionRichTextBox.GetPlainText()?.Length is <= 4000 || string.IsNullOrEmpty(DescriptionRichTextBox.GetPlainText()));
		}

		if (e.PropertyName is nameof(ElectronicStateList))
		{
			_ = (ElectronicStateMethodFamily?.ElectronicState = null);
			_ = (ElectronicStateMethodFamily?.ElectronicStateList = ElectronicStateList);
			SelectedElectronicState = null;
		}

		if (e.PropertyName is nameof(MethodFamilyList))
		{
			_ = (ElectronicStateMethodFamily?.MethodFamily = null);
			_ = (ElectronicStateMethodFamily?.MethodFamilyList = MethodFamilyList);
			SelectedMethodFamily = null;
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}

		// Nothing to do for change events of IsErrorVisible and CanSave.

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(ElectronicStatesMethodFamiliesCreateControl_PropertyChanged));
		}
	}

	private void SuperscriptToggleButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(SuperscriptToggleButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleSuperscript(SuperscriptToggleButton, SubscriptToggleButton);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(SuperscriptToggleButton_Click));
		}
	}

	private void SubscriptToggleButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(SubscriptToggleButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleSubscript(SuperscriptToggleButton, SubscriptToggleButton);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(SubscriptToggleButton_Click));
		}
	}

	private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(FontFamilyComboBox_SelectionChanged), sender, e);
		}

		if (FontFamilyComboBox.SelectedItem != null)
		{
			DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamilyComboBox.SelectedItem);
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(FontFamilyComboBox_SelectionChanged));
		}
	}

	private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(FontSizeComboBox_SelectionChanged), sender, e);
		}

		if (FontSizeComboBox.SelectedItem != null)
		{
			DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, FontSizeComboBox.SelectedItem);
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(FontSizeComboBox_SelectionChanged));
		}
	}

	private void FontColorButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(FontColorButton_Click), sender, e);
		}

		ApplyFontColor(_currentFontColor);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(FontColorButton_Click));
		}
	}

	private void FontColorPickerButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(FontColorPickerButton_Click), sender, e);
		}

		FontColorPopup.IsOpen = !FontColorPopup.IsOpen;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(FontColorPickerButton_Click));
		}
	}

	private void BackgroundColorButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(BackgroundColorButton_Click), sender, e);
		}

		ApplyBackgroundColor(_currentBackgroundColor);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(BackgroundColorButton_Click));
		}
	}

	private void BackgroundColorPickerButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(BackgroundColorPickerButton_Click), sender, e);
		}

		BackgroundColorPopup.IsOpen = !BackgroundColorPopup.IsOpen;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(BackgroundColorPickerButton_Click));
		}
	}

	private void DescriptionRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(DescriptionRichTextBox_SelectionChanged), sender, e);
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
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(DescriptionRichTextBox_SelectionChanged));
		}
	}

	private void DescriptionRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(DescriptionRichTextBox_TextChanged), sender, e);
		}

		CanSave = (ElectronicStateMethodFamilyName?.Length is > 0 and <= 200 || Keyword?.Length is > 0 and <= 50) && SelectedElectronicState is not null && (DescriptionRichTextBox.GetPlainText()?.Length is <= 4000 || string.IsNullOrEmpty(DescriptionRichTextBox.GetPlainText()));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(DescriptionRichTextBox_TextChanged));
		}
	}

	private void CreateButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(CreateButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;
			ElectronicStateMethodFamily.Name = ElectronicStateMethodFamilyName;
			ElectronicStateMethodFamily.Keyword = Keyword;
			ElectronicStateMethodFamily.ElectronicState = SelectedElectronicState;
			ElectronicStateMethodFamily.MethodFamily = SelectedMethodFamily;
			ElectronicStateMethodFamily.DescriptionRtf = DescriptionRichTextBox.GetRtfText();
			ElectronicStateMethodFamily.DescriptionText = DescriptionRichTextBox.GetPlainText();
			ElectronicStateMethodFamilySimpleModel model = ElectronicStateMethodFamily.ToSimpleModel();
			ElectronicStateMethodFamilyFullModel? result = _electronicStatesMethodFamiliesEndpoint.CreateAsync(model).Result;

			if (result is not null)
			{
				ElectronicStateMethodFamily.Id = result.Id;
				ElectronicStateMethodFamily.CreatedDate = result.CreatedDate;
				ElectronicStateMethodFamily.LastUpdatedDate = result.LastUpdatedDate;
				ElectronicStateMethodFamily.Archived = result.Archived;
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesMethodFamiliesCreateControl>("create", ElectronicStateMethodFamily.Id));
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(CreateButton_Click));
			}
		}
		catch (HttpIOException ex)
		{
			ErrorMessage = ex.Message;

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(CreateButton_Click), sender, e);
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
						_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(CreateButton_Click), sender, e);
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesMethodFamiliesCreateControl>("index", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(BackToIndexButton_Click));
		}
	}

	private void InitializeColorPickers()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(InitializeColorPickers));
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
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(InitializeColorPickers));
		}
	}

	private Button CreateColorButton(Color color, bool isFontColor)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with Color = {Color} and IsFontColor = {IsFontColor}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(CreateColorButton), color, isFontColor);
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
			_logger.LogDebug("{UserControl} {Method} returning Button {Button}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(InitializeColorPickers), button);
		}

		return button;
	}

	private void ApplyFontColor(SolidColorBrush color)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with Color = {Color}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(ApplyFontColor), color);
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
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(ApplyFontColor));
		}
	}

	private void ApplyBackgroundColor(SolidColorBrush color)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with Color = {Color}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(ApplyBackgroundColor), color);
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
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(ApplyBackgroundColor));
		}
	}

	private void AddToRecentColors(SolidColorBrush color, bool isFontColor)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with Color = {Color} and IsFontColor = {IsFontColor}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(AddToRecentColors), color, isFontColor);
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
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(AddToRecentColors));
		}
	}

	private void UpdateRecentColors(bool isFontColor)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with IsFontColor = {IsFontColor}.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(UpdateRecentColors), isFontColor);
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
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesMethodFamiliesCreateControl), nameof(UpdateRecentColors));
		}
	}
}
