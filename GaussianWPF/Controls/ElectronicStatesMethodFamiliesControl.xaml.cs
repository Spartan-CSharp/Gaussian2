using System.Windows.Controls;

using GaussianCommonLibrary.ErrorModels;

using GaussianWPF.Controls.ElectronicStatesMethodFamilies;
using GaussianWPF.FactoryHelpers;

using GaussianWPFLibrary.EventModels;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls;

/// <summary>
/// Interaction logic for ElectronicStatesMethodFamiliesControl.xaml
/// </summary>
public partial class ElectronicStatesMethodFamiliesControl : UserControl
{
	private readonly ILogger<ElectronicStatesMethodFamiliesControl> _logger;
	private readonly IAbstractFactory<ElectronicStatesMethodFamiliesIndexControl> _electronicStatesMethodFamiliesIndexFactory;
	private readonly IAbstractFactory<ElectronicStatesMethodFamiliesDetailsControl> _electronicStatesMethodFamiliesDetailsFactory;
	private readonly IAbstractFactory<ElectronicStatesMethodFamiliesCreateControl> _electronicStatesMethodFamiliesCreateFactory;
	private readonly IAbstractFactory<ElectronicStatesMethodFamiliesEditControl> _electronicStatesMethodFamiliesEditFactory;
	private readonly IAbstractFactory<ElectronicStatesMethodFamiliesDeleteControl> _electronicStatesMethodFamiliesDeleteFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStatesMethodFamiliesControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="electronicStatesMethodFamiliesIndexFactory">The factory for creating Electronic State/Method Family Combinations index controls.</param>
	/// <param name="electronicStatesMethodFamiliesDetailsFactory">The factory for creating Electronic State/Method Family Combinations details controls.</param>
	/// <param name="electronicStatesMethodFamiliesCreateFactory">The factory for creating Electronic State/Method Family Combinations create controls.</param>
	/// <param name="electronicStatesMethodFamiliesEditFactory">The factory for creating Electronic State/Method Family Combinations edit controls.</param>
	/// <param name="electronicStatesMethodFamiliesDeleteFactory">The factory for creating Electronic State/Method Family Combinations delete controls.</param>
	public ElectronicStatesMethodFamiliesControl(ILogger<ElectronicStatesMethodFamiliesControl> logger, IAbstractFactory<ElectronicStatesMethodFamiliesIndexControl> electronicStatesMethodFamiliesIndexFactory, IAbstractFactory<ElectronicStatesMethodFamiliesDetailsControl> electronicStatesMethodFamiliesDetailsFactory, IAbstractFactory<ElectronicStatesMethodFamiliesCreateControl> electronicStatesMethodFamiliesCreateFactory, IAbstractFactory<ElectronicStatesMethodFamiliesEditControl> electronicStatesMethodFamiliesEditFactory, IAbstractFactory<ElectronicStatesMethodFamiliesDeleteControl> electronicStatesMethodFamiliesDeleteFactory)
	{
		_logger = logger;
		_electronicStatesMethodFamiliesIndexFactory = electronicStatesMethodFamiliesIndexFactory;
		_electronicStatesMethodFamiliesDetailsFactory = electronicStatesMethodFamiliesDetailsFactory;
		_electronicStatesMethodFamiliesCreateFactory = electronicStatesMethodFamiliesCreateFactory;
		_electronicStatesMethodFamiliesEditFactory = electronicStatesMethodFamiliesEditFactory;
		_electronicStatesMethodFamiliesDeleteFactory = electronicStatesMethodFamiliesDeleteFactory;

		InitializeComponent();
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the OnInitialized event and displays the Electronic State/Method Family Combinations index control as the initial content.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} with {EventArgs}.", nameof(ElectronicStatesMethodFamiliesControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		ElectronicStatesMethodFamiliesIndexControl indexControl = _electronicStatesMethodFamiliesIndexFactory.Create();
		indexControl.ChildControlEvent += ElectronicStatesMethodFamilies_Index_ChildControlEvent;
		ElectronicStatesMethodFamiliesContent.Content = indexControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesControl), nameof(OnInitialized));
		}
	}

	private void ElectronicStatesMethodFamilies_Index_ChildControlEvent(object? sender, ChildControlEventArgs<ElectronicStatesMethodFamiliesIndexControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesControl), nameof(ElectronicStatesMethodFamilies_Index_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
				ElectronicStatesMethodFamiliesCreateControl createControl = _electronicStatesMethodFamiliesCreateFactory.Create();
				createControl.ChildControlEvent += ElectronicStatesMethodFamilies_Create_ChildControlEvent;
				ElectronicStatesMethodFamiliesContent.Content = createControl;
				break;
			case "edit":
				ElectronicStatesMethodFamiliesEditControl editControl = _electronicStatesMethodFamiliesEditFactory.Create();
				editControl.ElectronicStateMethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				editControl.ChildControlEvent += ElectronicStatesMethodFamilies_Edit_ChildControlEvent;
				ElectronicStatesMethodFamiliesContent.Content = editControl;
				break;
			case "details":
				ElectronicStatesMethodFamiliesDetailsControl detailsControl = _electronicStatesMethodFamiliesDetailsFactory.Create();
				detailsControl.ElectronicStateMethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += ElectronicStatesMethodFamilies_Details_ChildControlEvent;
				ElectronicStatesMethodFamiliesContent.Content = detailsControl;
				break;
			case "delete":
				ElectronicStatesMethodFamiliesDeleteControl deleteControl = _electronicStatesMethodFamiliesDeleteFactory.Create();
				deleteControl.ElectronicStateMethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				deleteControl.ChildControlEvent += ElectronicStatesMethodFamilies_Delete_ChildControlEvent;
				ElectronicStatesMethodFamiliesContent.Content = deleteControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesControl), nameof(ElectronicStatesMethodFamilies_Index_ChildControlEvent));
		}
	}

	private void ElectronicStatesMethodFamilies_Details_ChildControlEvent(object? sender, ChildControlEventArgs<ElectronicStatesMethodFamiliesDetailsControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesControl), nameof(ElectronicStatesMethodFamilies_Details_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "edit":
				ElectronicStatesMethodFamiliesEditControl editControl = _electronicStatesMethodFamiliesEditFactory.Create();
				editControl.ElectronicStateMethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				editControl.ChildControlEvent += ElectronicStatesMethodFamilies_Edit_ChildControlEvent;
				ElectronicStatesMethodFamiliesContent.Content = editControl;
				break;
			case "index":
				ElectronicStatesMethodFamiliesIndexControl indexControl = _electronicStatesMethodFamiliesIndexFactory.Create();
				indexControl.ChildControlEvent += ElectronicStatesMethodFamilies_Index_ChildControlEvent;
				ElectronicStatesMethodFamiliesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesControl), nameof(ElectronicStatesMethodFamilies_Details_ChildControlEvent));
		}
	}

	private void ElectronicStatesMethodFamilies_Create_ChildControlEvent(object? sender, ChildControlEventArgs<ElectronicStatesMethodFamiliesCreateControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesControl), nameof(ElectronicStatesMethodFamilies_Create_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
				ElectronicStatesMethodFamiliesDetailsControl detailsControl = _electronicStatesMethodFamiliesDetailsFactory.Create();
				detailsControl.ElectronicStateMethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += ElectronicStatesMethodFamilies_Details_ChildControlEvent;
				ElectronicStatesMethodFamiliesContent.Content = detailsControl;
				break;
			case "index":
				ElectronicStatesMethodFamiliesIndexControl indexControl = _electronicStatesMethodFamiliesIndexFactory.Create();
				indexControl.ChildControlEvent += ElectronicStatesMethodFamilies_Index_ChildControlEvent;
				ElectronicStatesMethodFamiliesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesControl), nameof(ElectronicStatesMethodFamilies_Create_ChildControlEvent));
		}
	}

	private void ElectronicStatesMethodFamilies_Edit_ChildControlEvent(object? sender, ChildControlEventArgs<ElectronicStatesMethodFamiliesEditControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesControl), nameof(ElectronicStatesMethodFamilies_Edit_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "save":
				ElectronicStatesMethodFamiliesDetailsControl detailsControl = _electronicStatesMethodFamiliesDetailsFactory.Create();
				detailsControl.ElectronicStateMethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += ElectronicStatesMethodFamilies_Details_ChildControlEvent;
				ElectronicStatesMethodFamiliesContent.Content = detailsControl;
				break;
			case "index":
				ElectronicStatesMethodFamiliesIndexControl indexControl = _electronicStatesMethodFamiliesIndexFactory.Create();
				indexControl.ChildControlEvent += ElectronicStatesMethodFamilies_Index_ChildControlEvent;
				ElectronicStatesMethodFamiliesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesControl), nameof(ElectronicStatesMethodFamilies_Edit_ChildControlEvent));
		}
	}
	private void ElectronicStatesMethodFamilies_Delete_ChildControlEvent(object? sender, ChildControlEventArgs<ElectronicStatesMethodFamiliesDeleteControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesControl), nameof(ElectronicStatesMethodFamilies_Delete_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "delete":
			case "index":
				ElectronicStatesMethodFamiliesIndexControl indexControl = _electronicStatesMethodFamiliesIndexFactory.Create();
				indexControl.ChildControlEvent += ElectronicStatesMethodFamilies_Index_ChildControlEvent;
				ElectronicStatesMethodFamiliesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesControl), nameof(ElectronicStatesMethodFamilies_Delete_ChildControlEvent));
		}
	}
}
