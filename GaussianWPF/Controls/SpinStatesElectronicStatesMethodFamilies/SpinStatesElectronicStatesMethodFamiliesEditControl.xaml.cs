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
/// Interaction logic for SpinStatesElectronicStatesMethodFamiliesEditControl.xaml
/// </summary>
public partial class SpinStatesElectronicStatesMethodFamiliesEditControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<SpinStatesElectronicStatesMethodFamiliesEditControl> _logger;
	private readonly ISpinStatesElectronicStatesMethodFamiliesEndpoint _spinStatesElectronicStatesMethodFamiliesEndpoint;
	private readonly IElectronicStatesMethodFamiliesEndpoint _electronicStatesMethodFamiliesEndpoint;
	private readonly ISpinStatesEndpoint _spinStatesEndpoint;
	private SolidColorBrush _currentFontColor = new(Colors.Black);
	private SolidColorBrush _currentBackgroundColor = new(Colors.Yellow);
	private readonly List<SolidColorBrush> _recentFontColors = [];
	private readonly List<SolidColorBrush> _recentBackgroundColors = [];

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStatesElectronicStatesMethodFamiliesEditControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="spinStatesElectronicStatesMethodFamiliesEndpoint">The endpoint for Spin State/Electronic State/Method Family Combination API operations.</param>
	/// <param name="electronicStatesMethodFamiliesEndpoint">The endpoint for Electronic State/Method Family Combination API operations.</param>
	/// <param name="spinStatesEndpoint">The endpoint for Spin State API operations.</param>
	public SpinStatesElectronicStatesMethodFamiliesEditControl(ILogger<SpinStatesElectronicStatesMethodFamiliesEditControl> logger, ISpinStatesElectronicStatesMethodFamiliesEndpoint spinStatesElectronicStatesMethodFamiliesEndpoint, IElectronicStatesMethodFamiliesEndpoint electronicStatesMethodFamiliesEndpoint, ISpinStatesEndpoint spinStatesEndpoint)
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
	public event EventHandler<ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesEditControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the Spin State/Electronic State/Method Family Combination view model being edited.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically populates the keyword, RTF content, and updates validation state.
	/// </remarks>
	public SpinStateElectronicStateMethodFamilyViewModel? SpinStateElectronicStateMethodFamily
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(SpinStateElectronicStateMethodFamily));
		}
	}

	/// <summary>
	/// Gets or sets the identifier of the Spin State/Electronic State/Method Family Combination to edit.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically loads the Spin State/Electronic State/Method Family Combination data from the API.
	/// </remarks>
	public int SpinStateElectronicStateMethodFamilyId
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(SpinStateElectronicStateMethodFamilyId));
		}
	}

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
	/// Gets or sets a value indicating whether a valid Spin State/Electronic State/Method Family Combination model is loaded.
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
	/// Gets a value indicating whether no Spin State/Electronic State/Method Family Combination model is loaded.
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
	/// This property is automatically set to <see langword="true"/> when the keyword, selected Method Family, and description are valid.
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
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(OnPropertyChanged));
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += SpinStatesElectronicStatesMethodFamiliesEditControl_PropertyChanged;
		FontFamilyComboBox.ItemsSource = Fonts.SystemFontFamilies.Where(ff => ff.Source is "Segoe UI" or "Arial" or "Georgia" or "Impact" or "Tahoma" or "Times New Roman" or "Verdana").OrderByDescending(ff => ff.Source.Equals("Segoe UI", StringComparison.OrdinalIgnoreCase)).ThenBy(ff => ff.Source);
		FontFamilyComboBox.SelectedItem = new FontFamily("Segoe UI");
		FontSizeComboBox.ItemsSource = new List<double>() { 32.0 / 3.0, 40.0 / 3.0, 16.0, 56.0 / 3.0, 24.0, 32.0, 48.0 };
		FontSizeComboBox.SelectedItem = 16.0;

		// Initialize color pickers
		InitializeColorPickers();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(OnInitialized));
		}
	}

	private void SpinStatesElectronicStatesMethodFamiliesEditControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(SpinStatesElectronicStatesMethodFamiliesEditControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(SpinStateElectronicStateMethodFamily))
		{
			if (SpinStateElectronicStateMethodFamily is not null)
			{
				SpinStateElectronicStateMethodFamilyName = SpinStateElectronicStateMethodFamily.Name ?? string.Empty;
				Keyword = SpinStateElectronicStateMethodFamily.Keyword ?? string.Empty;
				SelectedElectronicStateMethodFamily = SpinStateElectronicStateMethodFamily.ElectronicStateMethodFamilyList.First(x => x.Id == SpinStateElectronicStateMethodFamily.ElectronicStateMethodFamily?.Id);
				SelectedSpinState = SpinStateElectronicStateMethodFamily.SpinStateList.FirstOrDefault(x => x.Id == SpinStateElectronicStateMethodFamily.SpinState?.Id);

				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(SpinStateElectronicStateMethodFamily.DescriptionRtf);
				ModelIsNotNull = true;
				CanSave = (SpinStateElectronicStateMethodFamily.Name?.Length is > 0 and <= 200 || SpinStateElectronicStateMethodFamily.Keyword?.Length is > 0 and <= 50) && SpinStateElectronicStateMethodFamily.ElectronicStateMethodFamily is not null && (SpinStateElectronicStateMethodFamily.DescriptionText?.Length is <= 4000 || string.IsNullOrEmpty(SpinStateElectronicStateMethodFamily.DescriptionText));
			}
			else
			{
				SpinStateElectronicStateMethodFamilyName = string.Empty;
				Keyword = string.Empty;
				SelectedElectronicStateMethodFamily = null;
				SelectedSpinState = null;
				DescriptionRichTextBox.Document.Blocks.Clear();
				ModelIsNotNull = false;
				CanSave = false;
			}
		}

		if (e.PropertyName is nameof(SpinStateElectronicStateMethodFamilyId))
		{
			try
			{
				if (SpinStateElectronicStateMethodFamilyId != 0 && (SpinStateElectronicStateMethodFamily is null || SpinStateElectronicStateMethodFamily.Id != SpinStateElectronicStateMethodFamilyId))
				{
					SpinStateElectronicStateMethodFamilyFullModel? results = _spinStatesElectronicStatesMethodFamiliesEndpoint.GetByIdAsync(SpinStateElectronicStateMethodFamilyId).Result;
					List<ElectronicStateMethodFamilyRecord>? electronicStatesMethodFamilies = _electronicStatesMethodFamiliesEndpoint.GetListAsync().Result;
					List<SpinStateRecord>? spinStates = _spinStatesEndpoint.GetListAsync().Result;

					if (results is not null && electronicStatesMethodFamilies is not null && electronicStatesMethodFamilies.Count > 0 && spinStates is not null && spinStates.Count > 0)
					{
						ElectronicStateMethodFamilyList.Clear();
						SpinStateList.Clear();

						foreach (ElectronicStateMethodFamilyRecord item in electronicStatesMethodFamilies)
						{
							ElectronicStateMethodFamilyList.Add(item);
						}

						foreach (SpinStateRecord item in spinStates)
						{
							SpinStateList.Add(item);
						}

						SpinStateElectronicStateMethodFamily = new SpinStateElectronicStateMethodFamilyViewModel(results, electronicStatesMethodFamilies, spinStates);
						SpinStateElectronicStateMethodFamilyName = SpinStateElectronicStateMethodFamily.Name ?? string.Empty;
						Keyword = SpinStateElectronicStateMethodFamily.Keyword ?? string.Empty;
						SelectedElectronicStateMethodFamily = ElectronicStateMethodFamilyList.First(x => x.Id == results.ElectronicStateMethodFamily?.Id);
						SelectedSpinState = SpinStateList.FirstOrDefault(mf => mf.Id == results.SpinState?.Id);

						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(SpinStateElectronicStateMethodFamily.DescriptionRtf);
						ModelIsNotNull = true;
						CanSave = (SpinStateElectronicStateMethodFamily.Name?.Length is > 0 and <= 200 || SpinStateElectronicStateMethodFamily.Keyword?.Length is > 0 and <= 50) && SpinStateElectronicStateMethodFamily.ElectronicStateMethodFamily is not null && (SpinStateElectronicStateMethodFamily.DescriptionText?.Length is <= 4000 || string.IsNullOrEmpty(SpinStateElectronicStateMethodFamily.DescriptionText));
					}
					else if (results is not null && electronicStatesMethodFamilies is not null && electronicStatesMethodFamilies.Count > 0)
					{
						ElectronicStateMethodFamilyList.Clear();

						foreach (ElectronicStateMethodFamilyRecord item in electronicStatesMethodFamilies)
						{
							ElectronicStateMethodFamilyList.Add(item);
						}

						SpinStateElectronicStateMethodFamily = new SpinStateElectronicStateMethodFamilyViewModel()
						{
							Id = results.Id,
							Name = results.Name,
							Keyword = results.Keyword,
							ElectronicStateMethodFamily = results.ElectronicStateMethodFamily.ToRecord(),
							ElectronicStateMethodFamilyList = [..ElectronicStateMethodFamilyList],
							SpinState = results.SpinState?.ToRecord(),
							DescriptionRtf = results.DescriptionRtf,
							DescriptionText = results.DescriptionText,
							CreatedDate = results.CreatedDate,
							LastUpdatedDate = results.LastUpdatedDate,
							Archived = results.Archived
						};

						SpinStateElectronicStateMethodFamilyName = SpinStateElectronicStateMethodFamily.Name ?? string.Empty;
						Keyword = SpinStateElectronicStateMethodFamily.Keyword ?? string.Empty;
						SelectedElectronicStateMethodFamily = ElectronicStateMethodFamilyList.First(x => x.Id == results.ElectronicStateMethodFamily?.Id);
						SelectedSpinState = SpinStateElectronicStateMethodFamily.SpinState;

						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(SpinStateElectronicStateMethodFamily.DescriptionRtf);
						ModelIsNotNull = true;
						CanSave = (SpinStateElectronicStateMethodFamily.Name?.Length is > 0 and <= 200 || SpinStateElectronicStateMethodFamily.Keyword?.Length is > 0 and <= 50) && SpinStateElectronicStateMethodFamily.ElectronicStateMethodFamily is not null && (SpinStateElectronicStateMethodFamily.DescriptionText?.Length is <= 4000 || string.IsNullOrEmpty(SpinStateElectronicStateMethodFamily.DescriptionText));
					}
					else if (results is not null && spinStates is not null && spinStates.Count > 0)
					{
						SpinStateList.Clear();

						foreach (SpinStateRecord item in spinStates)
						{
							SpinStateList.Add(item);
						}

						SpinStateElectronicStateMethodFamily = new SpinStateElectronicStateMethodFamilyViewModel()
						{
							Id = results.Id,
							Name = results.Name,
							Keyword = results.Keyword,
							ElectronicStateMethodFamily = results.ElectronicStateMethodFamily.ToRecord(),
							SpinState = results.SpinState?.ToRecord(),
							SpinStateList = [..SpinStateList],
							DescriptionRtf = results.DescriptionRtf,
							DescriptionText = results.DescriptionText,
							CreatedDate = results.CreatedDate,
							LastUpdatedDate = results.LastUpdatedDate,
							Archived = results.Archived
						};

						SpinStateElectronicStateMethodFamilyName = SpinStateElectronicStateMethodFamily.Name ?? string.Empty;
						Keyword = SpinStateElectronicStateMethodFamily.Keyword ?? string.Empty;
						SelectedElectronicStateMethodFamily = SpinStateElectronicStateMethodFamily.ElectronicStateMethodFamily;
						SelectedSpinState = SpinStateList.FirstOrDefault(mf => mf.Id == results.SpinState?.Id);

						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(SpinStateElectronicStateMethodFamily.DescriptionRtf);
						ModelIsNotNull = true;
						CanSave = (SpinStateElectronicStateMethodFamily.Name?.Length is > 0 and <= 200 || SpinStateElectronicStateMethodFamily.Keyword?.Length is > 0 and <= 50) && SpinStateElectronicStateMethodFamily.ElectronicStateMethodFamily is not null && (SpinStateElectronicStateMethodFamily.DescriptionText?.Length is <= 4000 || string.IsNullOrEmpty(SpinStateElectronicStateMethodFamily.DescriptionText));
					}

					else if (electronicStatesMethodFamilies is not null && electronicStatesMethodFamilies.Count > 0 && spinStates is not null && spinStates.Count > 0)
					{
						ElectronicStateMethodFamilyList.Clear();
						SpinStateList.Clear();

						foreach (ElectronicStateMethodFamilyRecord item in electronicStatesMethodFamilies)
						{
							ElectronicStateMethodFamilyList.Add(item);
						}

						foreach (SpinStateRecord item in spinStates)
						{
							SpinStateList.Add(item);
						}

						SpinStateElectronicStateMethodFamily = null;
						SpinStateElectronicStateMethodFamilyName = string.Empty;
						Keyword = string.Empty;
						SelectedElectronicStateMethodFamily = null;
						SelectedSpinState = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
						CanSave = false;
					}
					else if (results is not null)
					{
						SpinStateElectronicStateMethodFamily = new SpinStateElectronicStateMethodFamilyViewModel()
						{
							Id = results.Id,
							Name = results.Name,
							Keyword = results.Keyword,
							ElectronicStateMethodFamily = results.ElectronicStateMethodFamily.ToRecord(),
							SpinState = results.SpinState?.ToRecord(),
							DescriptionRtf = results.DescriptionRtf,
							DescriptionText = results.DescriptionText,
							CreatedDate = results.CreatedDate,
							LastUpdatedDate = results.LastUpdatedDate,
							Archived = results.Archived
						};

						SpinStateElectronicStateMethodFamilyName = SpinStateElectronicStateMethodFamily.Name ?? string.Empty;
						Keyword = SpinStateElectronicStateMethodFamily.Keyword ?? string.Empty;
						SelectedElectronicStateMethodFamily = SpinStateElectronicStateMethodFamily.ElectronicStateMethodFamily;
						SelectedSpinState = SpinStateElectronicStateMethodFamily.SpinState;

						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(SpinStateElectronicStateMethodFamily.DescriptionRtf);
						ModelIsNotNull = true;
						CanSave = (SpinStateElectronicStateMethodFamily.Name?.Length is > 0 and <= 200 || SpinStateElectronicStateMethodFamily.Keyword?.Length is > 0 and <= 50) && SpinStateElectronicStateMethodFamily.ElectronicStateMethodFamily is not null && (SpinStateElectronicStateMethodFamily.DescriptionText?.Length is <= 4000 || string.IsNullOrEmpty(SpinStateElectronicStateMethodFamily.DescriptionText));
					}
					else if (electronicStatesMethodFamilies is not null && electronicStatesMethodFamilies.Count > 0)
					{
						ElectronicStateMethodFamilyList.Clear();

						foreach (ElectronicStateMethodFamilyRecord item in electronicStatesMethodFamilies)
						{
							ElectronicStateMethodFamilyList.Add(item);
						}

						SpinStateElectronicStateMethodFamily = null;
						SpinStateElectronicStateMethodFamilyName = string.Empty;
						Keyword = string.Empty;
						SelectedElectronicStateMethodFamily = null;
						SelectedSpinState = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
						CanSave = false;
					}
					else if (spinStates is not null && spinStates.Count > 0)
					{
						SpinStateList.Clear();

						foreach (SpinStateRecord item in spinStates)
						{
							SpinStateList.Add(item);
						}

						SpinStateElectronicStateMethodFamily = null;
						SpinStateElectronicStateMethodFamilyName = string.Empty;
						Keyword = string.Empty;
						SelectedElectronicStateMethodFamily = null;
						SelectedSpinState = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
						CanSave = false;
					}
					else
					{
						SpinStateElectronicStateMethodFamily = null;
						SpinStateElectronicStateMethodFamilyName = string.Empty;
						Keyword = string.Empty;
						SelectedElectronicStateMethodFamily = null;
						SelectedSpinState = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
						CanSave = false;
					}
				}
				else if (SpinStateElectronicStateMethodFamilyId == 0)
				{
					SpinStateElectronicStateMethodFamily = null;
					SpinStateElectronicStateMethodFamilyName = string.Empty;
					Keyword = string.Empty;
					SelectedElectronicStateMethodFamily = null;
					SelectedSpinState = null;
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
					_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(SpinStatesElectronicStatesMethodFamiliesEditControl_PropertyChanged), sender, e);
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
							_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(SpinStatesElectronicStatesMethodFamiliesEditControl_PropertyChanged), sender, e);
						}

						return true;
					}

					// Return false for any other exception types to rethrow them in a new AggregateException
					return false;
				});
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

		// Nothing to do for change events of ModelIsNull, ModelIsNotNull, IsErrorVisible and CanSave.

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(SpinStatesElectronicStatesMethodFamiliesEditControl_PropertyChanged));
		}
	}

	private void SuperscriptToggleButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(SuperscriptToggleButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleSuperscript(SuperscriptToggleButton, SubscriptToggleButton);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(SuperscriptToggleButton_Click));
		}
	}

	private void SubscriptToggleButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(SubscriptToggleButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleSubscript(SuperscriptToggleButton, SubscriptToggleButton);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(SubscriptToggleButton_Click));
		}
	}

	private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(FontFamilyComboBox_SelectionChanged), sender, e);
		}

		if (FontFamilyComboBox.SelectedItem != null)
		{
			DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamilyComboBox.SelectedItem);
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(FontFamilyComboBox_SelectionChanged));
		}
	}

	private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(FontSizeComboBox_SelectionChanged), sender, e);
		}

		if (FontSizeComboBox.SelectedItem != null)
		{
			DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, FontSizeComboBox.SelectedItem);
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(FontSizeComboBox_SelectionChanged));
		}
	}

	private void FontColorButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(FontColorButton_Click), sender, e);
		}

		ApplyFontColor(_currentFontColor);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(FontColorButton_Click));
		}
	}

	private void FontColorPickerButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(FontColorPickerButton_Click), sender, e);
		}

		FontColorPopup.IsOpen = !FontColorPopup.IsOpen;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(FontColorPickerButton_Click));
		}
	}

	private void BackgroundColorButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(BackgroundColorButton_Click), sender, e);
		}

		ApplyBackgroundColor(_currentBackgroundColor);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(BackgroundColorButton_Click));
		}
	}

	private void BackgroundColorPickerButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(BackgroundColorPickerButton_Click), sender, e);
		}

		BackgroundColorPopup.IsOpen = !BackgroundColorPopup.IsOpen;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(BackgroundColorPickerButton_Click));
		}
	}

	private void DescriptionRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(DescriptionRichTextBox_SelectionChanged), sender, e);
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
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(DescriptionRichTextBox_SelectionChanged));
		}
	}

	private void DescriptionRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(DescriptionRichTextBox_TextChanged), sender, e);
		}

		CanSave = (SpinStateElectronicStateMethodFamilyName?.Length is > 0 and <= 200 || Keyword?.Length is > 0 and <= 50) && SelectedElectronicStateMethodFamily is not null && (DescriptionRichTextBox.GetPlainText()?.Length is <= 4000 || string.IsNullOrEmpty(DescriptionRichTextBox.GetPlainText()));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(DescriptionRichTextBox_TextChanged));
		}
	}

	private void SaveButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(SaveButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;
			SpinStateElectronicStateMethodFamily!.Name = SpinStateElectronicStateMethodFamilyName;
			SpinStateElectronicStateMethodFamily!.Keyword = Keyword;
			SpinStateElectronicStateMethodFamily!.ElectronicStateMethodFamily = SelectedElectronicStateMethodFamily;
			SpinStateElectronicStateMethodFamily!.SpinState = SelectedSpinState;
 			SpinStateElectronicStateMethodFamily!.DescriptionRtf = DescriptionRichTextBox.GetRtfText();
			SpinStateElectronicStateMethodFamily!.DescriptionText = DescriptionRichTextBox.GetPlainText();
			SpinStateElectronicStateMethodFamilySimpleModel model = SpinStateElectronicStateMethodFamily!.ToSimpleModel();
			SpinStateElectronicStateMethodFamilyFullModel? result = _spinStatesElectronicStatesMethodFamiliesEndpoint.UpdateAsync(SpinStateElectronicStateMethodFamilyId, model).Result;

			if (result is not null)
			{
				SpinStateElectronicStateMethodFamily.LastUpdatedDate = result.LastUpdatedDate;
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesEditControl>("save", SpinStateElectronicStateMethodFamilyId));
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(SaveButton_Click));
			}
		}
		catch (HttpIOException ex)
		{
			ErrorMessage = ex.Message;

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(SaveButton_Click), sender, e);
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
						_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(SaveButton_Click), sender, e);
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesEditControl>("index", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(BackToIndexButton_Click));
		}
	}

	private void InitializeColorPickers()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(InitializeColorPickers));
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
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(InitializeColorPickers));
		}
	}

	private Button CreateColorButton(Color color, bool isFontColor)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with Color = {Color} and IsFontColor = {IsFontColor}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(CreateColorButton), color, isFontColor);
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
			_logger.LogDebug("{UserControl} {Method} returning Button {Button}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(InitializeColorPickers), button);
		}

		return button;
	}

	private void ApplyFontColor(SolidColorBrush color)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with Color = {Color}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(ApplyFontColor), color);
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
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(ApplyFontColor));
		}
	}

	private void ApplyBackgroundColor(SolidColorBrush color)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with Color = {Color}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(ApplyBackgroundColor), color);
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
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(ApplyBackgroundColor));
		}
	}

	private void AddToRecentColors(SolidColorBrush color, bool isFontColor)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with Color = {Color} and IsFontColor = {IsFontColor}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(AddToRecentColors), color, isFontColor);
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
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(AddToRecentColors));
		}
	}

	private void UpdateRecentColors(bool isFontColor)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with IsFontColor = {IsFontColor}.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(UpdateRecentColors), isFontColor);
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
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEditControl), nameof(UpdateRecentColors));
		}
	}
}
