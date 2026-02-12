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

namespace GaussianWPF.Controls.SpinStatesElectronicStatesMethodFamilies;

/// <summary>
/// Interaction logic for SpinStatesElectronicStatesMethodFamiliesCreateControl.xaml
/// </summary>
public partial class SpinStatesElectronicStatesMethodFamiliesCreateControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<SpinStatesElectronicStatesMethodFamiliesCreateControl> _logger;
	private readonly ISpinStatesElectronicStatesMethodFamiliesEndpoint _spinStatesElectronicStatesMethodFamiliesEndpoint;
	private readonly IElectronicStatesMethodFamiliesEndpoint _electronicStatesMethodFamiliesEndpoint;
	private readonly ISpinStatesEndpoint _spinStatesEndpoint;
	private SolidColorBrush _currentFontColor = new(Colors.Black);
	private SolidColorBrush _currentBackgroundColor = new(Colors.Yellow);
	private readonly List<SolidColorBrush> _recentFontColors = [];
	private readonly List<SolidColorBrush> _recentBackgroundColors = [];

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStatesElectronicStatesMethodFamiliesCreateControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="spinStatesElectronicStatesMethodFamiliesEndpoint">The endpoint for Spin State/Electronic State/Method Family Combination API operations.</param>
	/// <param name="electronicStatesMethodFamiliesEndpoint">The endpoint for Electronic State/Method Family Combination API operations.</param>
	/// <param name="spinStatesEndpoint">The endpoint for Spin State API operations.</param>
	public SpinStatesElectronicStatesMethodFamiliesCreateControl(ILogger<SpinStatesElectronicStatesMethodFamiliesCreateControl> logger, ISpinStatesElectronicStatesMethodFamiliesEndpoint spinStatesElectronicStatesMethodFamiliesEndpoint, IElectronicStatesMethodFamiliesEndpoint electronicStatesMethodFamiliesEndpoint, ISpinStatesEndpoint spinStatesEndpoint)
	{
		_logger = logger;
		_spinStatesElectronicStatesMethodFamiliesEndpoint = spinStatesElectronicStatesMethodFamiliesEndpoint;
		_electronicStatesMethodFamiliesEndpoint = electronicStatesMethodFamiliesEndpoint;
		_spinStatesEndpoint = spinStatesEndpoint;

		InitializeComponent();
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Occurs when a child control event is raised to request navigation to another view.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesCreateControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the Spin State/Electronic State/Method Family Combination view model being created.
	/// </summary>
	public SpinStateElectronicStateMethodFamilyViewModel SpinStateElectronicStateMethodFamily
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(SpinStateElectronicStateMethodFamily));
		}
	} = new();

	/// <summary>
	/// Gets or sets the name of the Spin State/Electronic State/Method Family Combination.
	/// </summary>
	public string SpinStateElectronicStateMethodFamilyName
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(SpinStateElectronicStateMethodFamilyName));
		}
	} = string.Empty;

	/// <summary>
	/// Gets or sets the keyword that identifies the Spin State/Electronic State/Method Family Combination.
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
	/// Gets or sets the selected Electronic State/Method Family Combination for the Spin State/Electronic State/Method Family Combination.
	/// </summary>
	public ElectronicStateMethodFamilyRecord? SelectedElectronicStateMethodFamily
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(SelectedElectronicStateMethodFamily));
		}
	}

	/// <summary>
	/// Gets or sets the observable collection of Electronic State/Method Family Combinations available for selection.
	/// </summary>
	/// <remarks>
	/// This collection supports data binding for ComboBox controls in WPF.
	/// </remarks>
	public ObservableCollection<ElectronicStateMethodFamilyRecord> ElectronicStateMethodFamilyList
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(ElectronicStateMethodFamilyList));
		}
	} = [];

	/// <summary>
	/// Gets or sets the selected Spin State for the Spin State/Electronic State/Method Family Combination.
	/// </summary>
	public SpinStateRecord? SelectedSpinState
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(SelectedSpinState));
		}
	}

	/// <summary>
	/// Gets or sets the observable collection of Spin States available for selection.
	/// </summary>
	/// <remarks>
	/// This collection supports data binding for ComboBox controls in WPF.
	/// </remarks>
	public ObservableCollection<SpinStateRecord> SpinStateList
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(SpinStateList));
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
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(OnPropertyChanged));
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += SpinStatesElectronicStatesMethodFamiliesCreateControl_PropertyChanged;
		FontFamilyComboBox.ItemsSource = Fonts.SystemFontFamilies.Where(ff => ff.Source is "Segoe UI" or "Arial" or "Georgia" or "Impact" or "Tahoma" or "Times New Roman" or "Verdana").OrderByDescending(ff => ff.Source.Equals("Segoe UI", StringComparison.OrdinalIgnoreCase)).ThenBy(ff => ff.Source);
		FontFamilyComboBox.SelectedItem = new FontFamily("Segoe UI");
		FontSizeComboBox.ItemsSource = new List<double>() { 32.0 / 3.0, 40.0 / 3.0, 16.0, 56.0 / 3.0, 24.0, 32.0, 48.0 };
		FontSizeComboBox.SelectedItem = 16.0;

		// Initialize color pickers
		InitializeColorPickers();
		List<ElectronicStateMethodFamilyRecord>? electronicStatesMethodFamilies = _electronicStatesMethodFamiliesEndpoint.GetListAsync().Result;
		List<SpinStateRecord>? spinStates = _spinStatesEndpoint.GetListAsync().Result;

		if (electronicStatesMethodFamilies is not null && electronicStatesMethodFamilies.Count > 0)
		{
			foreach (ElectronicStateMethodFamilyRecord item in electronicStatesMethodFamilies)
			{
				SpinStateElectronicStateMethodFamily.ElectronicStateMethodFamilyList.Add(item);
				ElectronicStateMethodFamilyList.Add(item);
			}
		}

		if (spinStates is not null && spinStates.Count > 0)
		{
			foreach (SpinStateRecord item in spinStates)
			{
				SpinStateElectronicStateMethodFamily.SpinStateList.Add(item);
				SpinStateList.Add(item);
			}
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(OnInitialized));
		}
	}

	private void SpinStatesElectronicStatesMethodFamiliesCreateControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(SpinStateElectronicStateMethodFamily))
		{
			if (SpinStateElectronicStateMethodFamily is not null)
			{
				SpinStateElectronicStateMethodFamilyName = SpinStateElectronicStateMethodFamily.Name ?? string.Empty;
				Keyword = SpinStateElectronicStateMethodFamily.Keyword ?? string.Empty;
				SelectedElectronicStateMethodFamily = ElectronicStateMethodFamilyList.First(x => x.Id == SpinStateElectronicStateMethodFamily.ElectronicStateMethodFamily?.Id);
				SelectedSpinState = SpinStateList.FirstOrDefault(x => x.Id == SpinStateElectronicStateMethodFamily.SpinState?.Id);

				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(SpinStateElectronicStateMethodFamily.DescriptionRtf);
				CanSave = (SpinStateElectronicStateMethodFamily.Name?.Length is > 0 and <= 200 || SpinStateElectronicStateMethodFamily.Keyword?.Length is > 0 and <= 50) && SpinStateElectronicStateMethodFamily.ElectronicStateMethodFamily is not null && (SpinStateElectronicStateMethodFamily.DescriptionText?.Length is <= 4000 || string.IsNullOrEmpty(SpinStateElectronicStateMethodFamily.DescriptionText));
			}
			else
			{
				SpinStateElectronicStateMethodFamilyName = string.Empty;
				Keyword = string.Empty;
				SelectedElectronicStateMethodFamily = null;
				SelectedSpinState = null;
				DescriptionRichTextBox.Document.Blocks.Clear();
				CanSave = false;
			}
		}

		if (e.PropertyName is nameof(SpinStateElectronicStateMethodFamilyName) or nameof(Keyword) or nameof(SelectedElectronicStateMethodFamily) or nameof(SelectedSpinState))
		{
			CanSave = (SpinStateElectronicStateMethodFamilyName?.Length is > 0 and <= 200 || Keyword?.Length is > 0 and <= 50) && SelectedElectronicStateMethodFamily is not null && (DescriptionRichTextBox.GetPlainText()?.Length is <= 4000 || string.IsNullOrEmpty(DescriptionRichTextBox.GetPlainText()));
		}

		if (e.PropertyName is nameof(ElectronicStateMethodFamilyList))
		{
			_ = (SpinStateElectronicStateMethodFamily?.ElectronicStateMethodFamily = null);
			_ = (SpinStateElectronicStateMethodFamily?.ElectronicStateMethodFamilyList = ElectronicStateMethodFamilyList);
			SelectedElectronicStateMethodFamily = null;
		}

		if (e.PropertyName is nameof(SpinStateList))
		{
			_ = (SpinStateElectronicStateMethodFamily?.SpinState = null);
			_ = (SpinStateElectronicStateMethodFamily?.SpinStateList = SpinStateList);
			SelectedSpinState = null;
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}

		// Nothing to do for change events of IsErrorVisible and CanSave.

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl_PropertyChanged));
		}
	}

	private void SuperscriptToggleButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(SuperscriptToggleButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleSuperscript(SuperscriptToggleButton, SubscriptToggleButton);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(SuperscriptToggleButton_Click));
		}
	}

	private void SubscriptToggleButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(SubscriptToggleButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleSubscript(SuperscriptToggleButton, SubscriptToggleButton);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(SubscriptToggleButton_Click));
		}
	}

	private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(FontFamilyComboBox_SelectionChanged), sender, e);
		}

		if (FontFamilyComboBox.SelectedItem != null)
		{
			DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamilyComboBox.SelectedItem);
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(FontFamilyComboBox_SelectionChanged));
		}
	}

	private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(FontSizeComboBox_SelectionChanged), sender, e);
		}

		if (FontSizeComboBox.SelectedItem != null)
		{
			DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, FontSizeComboBox.SelectedItem);
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(FontSizeComboBox_SelectionChanged));
		}
	}

	private void FontColorButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(FontColorButton_Click), sender, e);
		}

		ApplyFontColor(_currentFontColor);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(FontColorButton_Click));
		}
	}

	private void FontColorPickerButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(FontColorPickerButton_Click), sender, e);
		}

		FontColorPopup.IsOpen = !FontColorPopup.IsOpen;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(FontColorPickerButton_Click));
		}
	}

	private void BackgroundColorButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(BackgroundColorButton_Click), sender, e);
		}

		ApplyBackgroundColor(_currentBackgroundColor);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(BackgroundColorButton_Click));
		}
	}

	private void BackgroundColorPickerButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(BackgroundColorPickerButton_Click), sender, e);
		}

		BackgroundColorPopup.IsOpen = !BackgroundColorPopup.IsOpen;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(BackgroundColorPickerButton_Click));
		}
	}

	private void DescriptionRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(DescriptionRichTextBox_SelectionChanged), sender, e);
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
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(DescriptionRichTextBox_SelectionChanged));
		}
	}

	private void DescriptionRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(DescriptionRichTextBox_TextChanged), sender, e);
		}

		CanSave = (SpinStateElectronicStateMethodFamilyName?.Length is > 0 and <= 200 || Keyword?.Length is > 0 and <= 50) && SelectedElectronicStateMethodFamily is not null && (DescriptionRichTextBox.GetPlainText()?.Length is <= 4000 || string.IsNullOrEmpty(DescriptionRichTextBox.GetPlainText()));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(DescriptionRichTextBox_TextChanged));
		}
	}

	private void CreateButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(CreateButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;
			SpinStateElectronicStateMethodFamily.Name = SpinStateElectronicStateMethodFamilyName;
			SpinStateElectronicStateMethodFamily.Keyword = Keyword;
			SpinStateElectronicStateMethodFamily.ElectronicStateMethodFamily = SelectedElectronicStateMethodFamily;
			SpinStateElectronicStateMethodFamily.SpinState = SelectedSpinState;
			SpinStateElectronicStateMethodFamily.DescriptionRtf = DescriptionRichTextBox.GetRtfText();
			SpinStateElectronicStateMethodFamily.DescriptionText = DescriptionRichTextBox.GetPlainText();
			SpinStateElectronicStateMethodFamilySimpleModel model = SpinStateElectronicStateMethodFamily.ToSimpleModel();
			SpinStateElectronicStateMethodFamilyFullModel? result = _spinStatesElectronicStatesMethodFamiliesEndpoint.CreateAsync(model).Result;

			if (result is not null)
			{
				SpinStateElectronicStateMethodFamily.Id = result.Id;
				SpinStateElectronicStateMethodFamily.CreatedDate = result.CreatedDate;
				SpinStateElectronicStateMethodFamily.LastUpdatedDate = result.LastUpdatedDate;
				SpinStateElectronicStateMethodFamily.Archived = result.Archived;
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesCreateControl>("create", SpinStateElectronicStateMethodFamily.Id));
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(CreateButton_Click));
			}
		}
		catch (HttpIOException ex)
		{
			ErrorMessage = ex.Message;

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(CreateButton_Click), sender, e);
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
						_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(CreateButton_Click), sender, e);
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesCreateControl>("index", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(BackToIndexButton_Click));
		}
	}

	private void InitializeColorPickers()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(InitializeColorPickers));
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
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(InitializeColorPickers));
		}
	}

	private Button CreateColorButton(Color color, bool isFontColor)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with Color = {Color} and IsFontColor = {IsFontColor}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(CreateColorButton), color, isFontColor);
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
			_logger.LogDebug("{UserControl} {Method} returning Button {Button}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(InitializeColorPickers), button);
		}

		return button;
	}

	private void ApplyFontColor(SolidColorBrush color)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with Color = {Color}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(ApplyFontColor), color);
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
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(ApplyFontColor));
		}
	}

	private void ApplyBackgroundColor(SolidColorBrush color)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with Color = {Color}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(ApplyBackgroundColor), color);
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
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(ApplyBackgroundColor));
		}
	}

	private void AddToRecentColors(SolidColorBrush color, bool isFontColor)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with Color = {Color} and IsFontColor = {IsFontColor}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(AddToRecentColors), color, isFontColor);
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
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(AddToRecentColors));
		}
	}

	private void UpdateRecentColors(bool isFontColor)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with IsFontColor = {IsFontColor}.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(UpdateRecentColors), isFontColor);
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
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCreateControl), nameof(UpdateRecentColors));
		}
	}
}
