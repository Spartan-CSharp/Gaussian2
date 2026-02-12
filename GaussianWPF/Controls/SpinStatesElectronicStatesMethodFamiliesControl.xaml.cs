using System.Windows.Controls;

using GaussianCommonLibrary.ErrorModels;

using GaussianWPF.Controls.SpinStatesElectronicStatesMethodFamilies;
using GaussianWPF.FactoryHelpers;

using GaussianWPFLibrary.EventModels;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls;

/// <summary>
/// Interaction logic for SpinStatesElectronicStatesMethodFamiliesControl.xaml
/// </summary>
public partial class SpinStatesElectronicStatesMethodFamiliesControl : UserControl
{
	private readonly ILogger<SpinStatesElectronicStatesMethodFamiliesControl> _logger;
	private readonly IAbstractFactory<SpinStatesElectronicStatesMethodFamiliesIndexControl> _spinStatesElectronicStatesMethodFamiliesIndexFactory;
	private readonly IAbstractFactory<SpinStatesElectronicStatesMethodFamiliesDetailsControl> _spinStatesElectronicStatesMethodFamiliesDetailsFactory;
	private readonly IAbstractFactory<SpinStatesElectronicStatesMethodFamiliesCreateControl> _spinStatesElectronicStatesMethodFamiliesCreateFactory;
	private readonly IAbstractFactory<SpinStatesElectronicStatesMethodFamiliesEditControl> _spinStatesElectronicStatesMethodFamiliesEditFactory;
	private readonly IAbstractFactory<SpinStatesElectronicStatesMethodFamiliesDeleteControl> _spinStatesElectronicStatesMethodFamiliesDeleteFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStatesElectronicStatesMethodFamiliesControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="spinStatesElectronicStatesMethodFamiliesIndexFactory">The factory for creating Spin State/Electronic State/Method Family Combinations index controls.</param>
	/// <param name="spinStatesElectronicStatesMethodFamiliesDetailsFactory">The factory for creating Spin State/Electronic State/Method Family Combinations details controls.</param>
	/// <param name="spinStatesElectronicStatesMethodFamiliesCreateFactory">The factory for creating Spin State/Electronic State/Method Family Combinations create controls.</param>
	/// <param name="spinStatesElectronicStatesMethodFamiliesEditFactory">The factory for creating Spin State/Electronic State/Method Family Combinations edit controls.</param>
	/// <param name="spinStatesElectronicStatesMethodFamiliesDeleteFactory">The factory for creating Spin State/Electronic State/Method Family Combinations delete controls.</param>
	public SpinStatesElectronicStatesMethodFamiliesControl(ILogger<SpinStatesElectronicStatesMethodFamiliesControl> logger, IAbstractFactory<SpinStatesElectronicStatesMethodFamiliesIndexControl> spinStatesElectronicStatesMethodFamiliesIndexFactory, IAbstractFactory<SpinStatesElectronicStatesMethodFamiliesDetailsControl> spinStatesElectronicStatesMethodFamiliesDetailsFactory, IAbstractFactory<SpinStatesElectronicStatesMethodFamiliesCreateControl> spinStatesElectronicStatesMethodFamiliesCreateFactory, IAbstractFactory<SpinStatesElectronicStatesMethodFamiliesEditControl> spinStatesElectronicStatesMethodFamiliesEditFactory, IAbstractFactory<SpinStatesElectronicStatesMethodFamiliesDeleteControl> spinStatesElectronicStatesMethodFamiliesDeleteFactory)
	{
		_logger = logger;
		_spinStatesElectronicStatesMethodFamiliesIndexFactory = spinStatesElectronicStatesMethodFamiliesIndexFactory;
		_spinStatesElectronicStatesMethodFamiliesDetailsFactory = spinStatesElectronicStatesMethodFamiliesDetailsFactory;
		_spinStatesElectronicStatesMethodFamiliesCreateFactory = spinStatesElectronicStatesMethodFamiliesCreateFactory;
		_spinStatesElectronicStatesMethodFamiliesEditFactory = spinStatesElectronicStatesMethodFamiliesEditFactory;
		_spinStatesElectronicStatesMethodFamiliesDeleteFactory = spinStatesElectronicStatesMethodFamiliesDeleteFactory;

		InitializeComponent();
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the OnInitialized event and displays the Spin State/Electronic State/Method Family Combinations index control as the initial content.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} with {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		SpinStatesElectronicStatesMethodFamiliesIndexControl indexControl = _spinStatesElectronicStatesMethodFamiliesIndexFactory.Create();
		indexControl.ChildControlEvent += SpinStatesElectronicStatesMethodFamilies_Index_ChildControlEvent;
		SpinStatesElectronicStatesMethodFamiliesContent.Content = indexControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesControl), nameof(OnInitialized));
		}
	}

	private void SpinStatesElectronicStatesMethodFamilies_Index_ChildControlEvent(object? sender, ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesIndexControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesControl), nameof(SpinStatesElectronicStatesMethodFamilies_Index_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
				SpinStatesElectronicStatesMethodFamiliesCreateControl createControl = _spinStatesElectronicStatesMethodFamiliesCreateFactory.Create();
				createControl.ChildControlEvent += SpinStatesElectronicStatesMethodFamilies_Create_ChildControlEvent;
				SpinStatesElectronicStatesMethodFamiliesContent.Content = createControl;
				break;
			case "edit":
				SpinStatesElectronicStatesMethodFamiliesEditControl editControl = _spinStatesElectronicStatesMethodFamiliesEditFactory.Create();
				editControl.SpinStateElectronicStateMethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				editControl.ChildControlEvent += SpinStatesElectronicStatesMethodFamilies_Edit_ChildControlEvent;
				SpinStatesElectronicStatesMethodFamiliesContent.Content = editControl;
				break;
			case "details":
				SpinStatesElectronicStatesMethodFamiliesDetailsControl detailsControl = _spinStatesElectronicStatesMethodFamiliesDetailsFactory.Create();
				detailsControl.SpinStateElectronicStateMethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += SpinStatesElectronicStatesMethodFamilies_Details_ChildControlEvent;
				SpinStatesElectronicStatesMethodFamiliesContent.Content = detailsControl;
				break;
			case "delete":
				SpinStatesElectronicStatesMethodFamiliesDeleteControl deleteControl = _spinStatesElectronicStatesMethodFamiliesDeleteFactory.Create();
				deleteControl.SpinStateElectronicStateMethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				deleteControl.ChildControlEvent += SpinStatesElectronicStatesMethodFamilies_Delete_ChildControlEvent;
				SpinStatesElectronicStatesMethodFamiliesContent.Content = deleteControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesControl), nameof(SpinStatesElectronicStatesMethodFamilies_Index_ChildControlEvent));
		}
	}

	private void SpinStatesElectronicStatesMethodFamilies_Details_ChildControlEvent(object? sender, ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesDetailsControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesControl), nameof(SpinStatesElectronicStatesMethodFamilies_Details_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "edit":
				SpinStatesElectronicStatesMethodFamiliesEditControl editControl = _spinStatesElectronicStatesMethodFamiliesEditFactory.Create();
				editControl.SpinStateElectronicStateMethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				editControl.ChildControlEvent += SpinStatesElectronicStatesMethodFamilies_Edit_ChildControlEvent;
				SpinStatesElectronicStatesMethodFamiliesContent.Content = editControl;
				break;
			case "index":
				SpinStatesElectronicStatesMethodFamiliesIndexControl indexControl = _spinStatesElectronicStatesMethodFamiliesIndexFactory.Create();
				indexControl.ChildControlEvent += SpinStatesElectronicStatesMethodFamilies_Index_ChildControlEvent;
				SpinStatesElectronicStatesMethodFamiliesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesControl), nameof(SpinStatesElectronicStatesMethodFamilies_Details_ChildControlEvent));
		}
	}

	private void SpinStatesElectronicStatesMethodFamilies_Create_ChildControlEvent(object? sender, ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesCreateControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesControl), nameof(SpinStatesElectronicStatesMethodFamilies_Create_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
				SpinStatesElectronicStatesMethodFamiliesDetailsControl detailsControl = _spinStatesElectronicStatesMethodFamiliesDetailsFactory.Create();
				detailsControl.SpinStateElectronicStateMethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += SpinStatesElectronicStatesMethodFamilies_Details_ChildControlEvent;
				SpinStatesElectronicStatesMethodFamiliesContent.Content = detailsControl;
				break;
			case "index":
				SpinStatesElectronicStatesMethodFamiliesIndexControl indexControl = _spinStatesElectronicStatesMethodFamiliesIndexFactory.Create();
				indexControl.ChildControlEvent += SpinStatesElectronicStatesMethodFamilies_Index_ChildControlEvent;
				SpinStatesElectronicStatesMethodFamiliesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesControl), nameof(SpinStatesElectronicStatesMethodFamilies_Create_ChildControlEvent));
		}
	}

	private void SpinStatesElectronicStatesMethodFamilies_Edit_ChildControlEvent(object? sender, ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesEditControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesControl), nameof(SpinStatesElectronicStatesMethodFamilies_Edit_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "save":
				SpinStatesElectronicStatesMethodFamiliesDetailsControl detailsControl = _spinStatesElectronicStatesMethodFamiliesDetailsFactory.Create();
				detailsControl.SpinStateElectronicStateMethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += SpinStatesElectronicStatesMethodFamilies_Details_ChildControlEvent;
				SpinStatesElectronicStatesMethodFamiliesContent.Content = detailsControl;
				break;
			case "index":
				SpinStatesElectronicStatesMethodFamiliesIndexControl indexControl = _spinStatesElectronicStatesMethodFamiliesIndexFactory.Create();
				indexControl.ChildControlEvent += SpinStatesElectronicStatesMethodFamilies_Index_ChildControlEvent;
				SpinStatesElectronicStatesMethodFamiliesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesControl), nameof(SpinStatesElectronicStatesMethodFamilies_Edit_ChildControlEvent));
		}
	}
	private void SpinStatesElectronicStatesMethodFamilies_Delete_ChildControlEvent(object? sender, ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesDeleteControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesControl), nameof(SpinStatesElectronicStatesMethodFamilies_Delete_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "delete":
			case "index":
				SpinStatesElectronicStatesMethodFamiliesIndexControl indexControl = _spinStatesElectronicStatesMethodFamiliesIndexFactory.Create();
				indexControl.ChildControlEvent += SpinStatesElectronicStatesMethodFamilies_Index_ChildControlEvent;
				SpinStatesElectronicStatesMethodFamiliesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesControl), nameof(SpinStatesElectronicStatesMethodFamilies_Delete_ChildControlEvent));
		}
	}
}
