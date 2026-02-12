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

namespace GaussianWPF.Controls.SpinStatesElectronicStatesMethodFamilies;

/// <summary>
/// Interaction logic for SpinStatesElectronicStatesMethodFamiliesDetailsControl.xaml
/// </summary>
public partial class SpinStatesElectronicStatesMethodFamiliesDetailsControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<SpinStatesElectronicStatesMethodFamiliesIndexControl> _logger;
	private readonly ISpinStatesElectronicStatesMethodFamiliesEndpoint _spinStatesElectronicStatesMethodFamiliesEndpoint;
	private readonly IElectronicStatesMethodFamiliesEndpoint _electronicStatesMethodFamiliesEndpoint;
	private readonly ISpinStatesEndpoint _spinStatesEndpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStatesElectronicStatesMethodFamiliesDetailsControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="spinStatesElectronicStatesMethodFamiliesEndpoint">The endpoint for Spin State/Electronic State/Method Family Combination API operations.</param>
	/// <param name="electronicStatesMethodFamiliesEndpoint">The endpoint for Electronic State/Method Family Combination API operations.</param>
	/// <param name="spinStatesEndpoint">The endpoint for Spin State API operations.</param>
	public SpinStatesElectronicStatesMethodFamiliesDetailsControl(ILogger<SpinStatesElectronicStatesMethodFamiliesIndexControl> logger, ISpinStatesElectronicStatesMethodFamiliesEndpoint spinStatesElectronicStatesMethodFamiliesEndpoint, IElectronicStatesMethodFamiliesEndpoint electronicStatesMethodFamiliesEndpoint, ISpinStatesEndpoint spinStatesEndpoint)
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
	public event EventHandler<ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesDetailsControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the Spin State/Electronic State/Method Family Combination view model being displayed.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically populates the RTF content and updates the <see cref="ModelIsNotNull"/> property.
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
	/// Gets or sets the identifier of the Spin State/Electronic State/Method Family Combination to display.
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
	/// Raises the <see cref="PropertyChanged"/> event for the specified property.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed. This parameter is optional and can be automatically populated by the caller member name.</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(SpinStatesElectronicStatesMethodFamiliesDetailsControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesDetailsControl), nameof(OnPropertyChanged));
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesDetailsControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += SpinStatesElectronicStatesMethodFamiliesDetailsControl_PropertyChanged;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesDetailsControl), nameof(OnInitialized));
		}
	}

	private void SpinStatesElectronicStatesMethodFamiliesDetailsControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesDetailsControl), nameof(SpinStatesElectronicStatesMethodFamiliesDetailsControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(SpinStateElectronicStateMethodFamily))
		{
			if (SpinStateElectronicStateMethodFamily is not null)
			{
				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(SpinStateElectronicStateMethodFamily.DescriptionRtf);
				ModelIsNotNull = true;
			}
			else
			{
				DescriptionRichTextBox.Document.Blocks.Clear();
				ModelIsNotNull = false;
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
						SpinStateElectronicStateMethodFamily = new SpinStateElectronicStateMethodFamilyViewModel(results, electronicStatesMethodFamilies, spinStates);
						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(SpinStateElectronicStateMethodFamily.DescriptionRtf);
						ModelIsNotNull = true;
					}
					else if (results is not null && electronicStatesMethodFamilies is not null && electronicStatesMethodFamilies.Count > 0)
					{
						SpinStateElectronicStateMethodFamily = new SpinStateElectronicStateMethodFamilyViewModel()
						{
							Id = results.Id,
							Name = results.Name,
							Keyword = results.Keyword,
							ElectronicStateMethodFamily = electronicStatesMethodFamilies.First(x => x.Id == results.ElectronicStateMethodFamily.Id),
							ElectronicStateMethodFamilyList = [.. electronicStatesMethodFamilies],
							SpinState = results.SpinState?.ToRecord(),
							DescriptionRtf = results.DescriptionRtf,
							DescriptionText = results.DescriptionText,
							CreatedDate = results.CreatedDate,
							LastUpdatedDate = results.LastUpdatedDate,
							Archived = results.Archived
						};

						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(SpinStateElectronicStateMethodFamily.DescriptionRtf);
						ModelIsNotNull = true;
					}
					else if (results is not null && spinStates is not null && spinStates.Count > 0)
					{
						SpinStateElectronicStateMethodFamily = new SpinStateElectronicStateMethodFamilyViewModel()
						{
							Id = results.Id,
							Name = results.Name,
							Keyword = results.Keyword,
							ElectronicStateMethodFamily = results.ElectronicStateMethodFamily.ToRecord(),
							SpinState = spinStates.FirstOrDefault(x => x.Id == results.SpinState?.Id),
							SpinStateList = [.. spinStates],
							DescriptionRtf = results.DescriptionRtf,
							DescriptionText = results.DescriptionText,
							CreatedDate = results.CreatedDate,
							LastUpdatedDate = results.LastUpdatedDate,
							Archived = results.Archived
						};

						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(SpinStateElectronicStateMethodFamily.DescriptionRtf);
						ModelIsNotNull = true;
					}
					else if (electronicStatesMethodFamilies is not null && electronicStatesMethodFamilies.Count > 0 && spinStates is not null && spinStates.Count > 0)
					{
						SpinStateElectronicStateMethodFamily = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
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

						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(SpinStateElectronicStateMethodFamily.DescriptionRtf);
						ModelIsNotNull = true;
					}
					else if (electronicStatesMethodFamilies is not null && electronicStatesMethodFamilies.Count > 0)
					{
						SpinStateElectronicStateMethodFamily = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
					}
					else if (spinStates is not null && spinStates.Count > 0)
					{
						SpinStateElectronicStateMethodFamily = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
					}
					else
					{
						SpinStateElectronicStateMethodFamily = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
					}
				}
				else if (SpinStateElectronicStateMethodFamilyId == 0)
				{
					SpinStateElectronicStateMethodFamily = null;
					DescriptionRichTextBox.Document.Blocks.Clear();
					ModelIsNotNull = false;
				}
			}
			catch (HttpIOException ex)
			{
				ErrorMessage = ex.Message;

				if (_logger.IsEnabled(LogLevel.Error))
				{
					_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(SpinStatesElectronicStatesMethodFamiliesDeleteControl), nameof(SpinStatesElectronicStatesMethodFamiliesDetailsControl_PropertyChanged), sender, e);
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
							_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(SpinStatesElectronicStatesMethodFamiliesDeleteControl), nameof(SpinStatesElectronicStatesMethodFamiliesDetailsControl_PropertyChanged), sender, e);
						}

						return true;
					}

					// Return false for any other exception types to rethrow them in a new AggregateException
					return false;
				});
			}
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}

		// Nothing to do for change events of ModelIsNull, ModelIsNotNull, IsErrorVisible and CanSave.

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesDetailsControl), nameof(SpinStatesElectronicStatesMethodFamiliesDetailsControl_PropertyChanged));
		}
	}

	private void EditButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesDetailsControl), nameof(EditButton_Click), sender, e);
		}

		if (ModelIsNotNull)
		{
			ChildControlEvent?.Invoke(this, new ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesDetailsControl>("edit", SpinStateElectronicStateMethodFamily!.Id));
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesDetailsControl), nameof(EditButton_Click));
		}
	}

	private void BackToIndexButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesDetailsControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesDetailsControl>("index", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesDetailsControl), nameof(BackToIndexButton_Click));
		}
	}
}
