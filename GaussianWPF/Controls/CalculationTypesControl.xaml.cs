using System.Windows.Controls;

using GaussianCommonLibrary.ErrorModels;

using GaussianWPF.Controls.CalculationTypes;
using GaussianWPF.FactoryHelpers;

using GaussianWPFLibrary.EventModels;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls;

/// <summary>
/// Interaction logic for CalculationTypesControl.xaml
/// </summary>
public partial class CalculationTypesControl : UserControl
{
	private readonly ILogger<CalculationTypesControl> _logger;
	private readonly IAbstractFactory<CalculationTypesIndexControl> _calculationTypesIndexFactory;
	private readonly IAbstractFactory<CalculationTypesDetailsControl> _calculationTypesDetailsFactory;
	private readonly IAbstractFactory<CalculationTypesCreateControl> _calculationTypesCreateFactory;
	private readonly IAbstractFactory<CalculationTypesEditControl> _calculationTypesEditFactory;
	private readonly IAbstractFactory<CalculationTypesDeleteControl> _calculationTypesDeleteFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="CalculationTypesControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="calculationTypesIndexFactory">The factory for creating calculation types index controls.</param>
	/// <param name="calculationTypesDetailsFactory">The factory for creating calculation types details controls.</param>
	/// <param name="calculationTypesCreateFactory">The factory for creating calculation types create controls.</param>
	/// <param name="calculationTypesEditFactory">The factory for creating calculation types edit controls.</param>
	/// <param name="calculationTypesDeleteFactory">The factory for creating calculation types delete controls.</param>
	public CalculationTypesControl(ILogger<CalculationTypesControl> logger, IAbstractFactory<CalculationTypesIndexControl> calculationTypesIndexFactory, IAbstractFactory<CalculationTypesDetailsControl> calculationTypesDetailsFactory, IAbstractFactory<CalculationTypesCreateControl> calculationTypesCreateFactory, IAbstractFactory<CalculationTypesEditControl> calculationTypesEditFactory, IAbstractFactory<CalculationTypesDeleteControl> calculationTypesDeleteFactory)
	{
		_logger = logger;
		_calculationTypesIndexFactory = calculationTypesIndexFactory;
		_calculationTypesDetailsFactory = calculationTypesDetailsFactory;
		_calculationTypesCreateFactory = calculationTypesCreateFactory;
		_calculationTypesEditFactory = calculationTypesEditFactory;
		_calculationTypesDeleteFactory = calculationTypesDeleteFactory;

		InitializeComponent();
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the OnInitialized event and displays the calculation types index control as the initial content.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} with {EventArgs}.", nameof(CalculationTypesControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		CalculationTypesIndexControl indexControl = _calculationTypesIndexFactory.Create();
		indexControl.ChildControlEvent += CalculationTypes_Index_ChildControlEvent;
		CalculationTypesContent.Content = indexControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(CalculationTypesControl), nameof(OnInitialized));
		}
	}

	private void CalculationTypes_Index_ChildControlEvent(object? sender, ChildControlEventArgs<CalculationTypesIndexControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(CalculationTypesControl), nameof(CalculationTypes_Index_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
				CalculationTypesCreateControl createControl = _calculationTypesCreateFactory.Create();
				createControl.ChildControlEvent += CalculationTypes_Create_ChildControlEvent;
				CalculationTypesContent.Content = createControl;
				break;
			case "edit":
				CalculationTypesEditControl editControl = _calculationTypesEditFactory.Create();
				editControl.CalculationTypeId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				editControl.ChildControlEvent += CalculationTypes_Edit_ChildControlEvent;
				CalculationTypesContent.Content = editControl;
				break;
			case "details":
				CalculationTypesDetailsControl detailsControl = _calculationTypesDetailsFactory.Create();
				detailsControl.CalculationTypeId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += CalculationTypes_Details_ChildControlEvent;
				CalculationTypesContent.Content = detailsControl;
				break;
			case "delete":
				CalculationTypesDeleteControl deleteControl = _calculationTypesDeleteFactory.Create();
				deleteControl.CalculationTypeId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				deleteControl.ChildControlEvent += CalculationTypes_Delete_ChildControlEvent;
				CalculationTypesContent.Content = deleteControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(CalculationTypesControl), nameof(CalculationTypes_Index_ChildControlEvent));
		}
	}

	private void CalculationTypes_Details_ChildControlEvent(object? sender, ChildControlEventArgs<CalculationTypesDetailsControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(CalculationTypesControl), nameof(CalculationTypes_Details_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "edit":
				CalculationTypesEditControl editControl = _calculationTypesEditFactory.Create();
				editControl.CalculationTypeId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				editControl.ChildControlEvent += CalculationTypes_Edit_ChildControlEvent;
				CalculationTypesContent.Content = editControl;
				break;
			case "index":
				CalculationTypesIndexControl indexControl = _calculationTypesIndexFactory.Create();
				indexControl.ChildControlEvent += CalculationTypes_Index_ChildControlEvent;
				CalculationTypesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(CalculationTypesControl), nameof(CalculationTypes_Details_ChildControlEvent));
		}
	}

	private void CalculationTypes_Create_ChildControlEvent(object? sender, ChildControlEventArgs<CalculationTypesCreateControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(CalculationTypesControl), nameof(CalculationTypes_Create_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
				CalculationTypesDetailsControl detailsControl = _calculationTypesDetailsFactory.Create();
				detailsControl.CalculationTypeId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += CalculationTypes_Details_ChildControlEvent;
				CalculationTypesContent.Content = detailsControl;
				break;
			case "index":
				CalculationTypesIndexControl indexControl = _calculationTypesIndexFactory.Create();
				indexControl.ChildControlEvent += CalculationTypes_Index_ChildControlEvent;
				CalculationTypesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(CalculationTypesControl), nameof(CalculationTypes_Create_ChildControlEvent));
		}
	}

	private void CalculationTypes_Edit_ChildControlEvent(object? sender, ChildControlEventArgs<CalculationTypesEditControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(CalculationTypesControl), nameof(CalculationTypes_Edit_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "save":
				CalculationTypesDetailsControl detailsControl = _calculationTypesDetailsFactory.Create();
				detailsControl.CalculationTypeId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += CalculationTypes_Details_ChildControlEvent;
				CalculationTypesContent.Content = detailsControl;
				break;
			case "index":
				CalculationTypesIndexControl indexControl = _calculationTypesIndexFactory.Create();
				indexControl.ChildControlEvent += CalculationTypes_Index_ChildControlEvent;
				CalculationTypesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(CalculationTypesControl), nameof(CalculationTypes_Edit_ChildControlEvent));
		}
	}
	private void CalculationTypes_Delete_ChildControlEvent(object? sender, ChildControlEventArgs<CalculationTypesDeleteControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(CalculationTypesControl), nameof(CalculationTypes_Delete_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "delete":
			case "index":
				CalculationTypesIndexControl indexControl = _calculationTypesIndexFactory.Create();
				indexControl.ChildControlEvent += CalculationTypes_Index_ChildControlEvent;
				CalculationTypesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(CalculationTypesControl), nameof(CalculationTypes_Delete_ChildControlEvent));
		}
	}
}
