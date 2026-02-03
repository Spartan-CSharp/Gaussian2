using System.Windows.Controls;

using GaussianCommonLibrary.ErrorModels;

using GaussianWPF.Controls.ElectronicStates;
using GaussianWPF.FactoryHelpers;

using GaussianWPFLibrary.EventModels;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls;

/// <summary>
/// Interaction logic for ElectronicStatesControl.xaml
/// </summary>
public partial class ElectronicStatesControl : UserControl
{
	private readonly ILogger<ElectronicStatesControl> _logger;
	private readonly IAbstractFactory<ElectronicStatesIndexControl> _electronicStatesIndexFactory;
	private readonly IAbstractFactory<ElectronicStatesDetailsControl> _electronicStatesDetailsFactory;
	private readonly IAbstractFactory<ElectronicStatesCreateControl> _electronicStatesCreateFactory;
	private readonly IAbstractFactory<ElectronicStatesEditControl> _electronicStatesEditFactory;
	private readonly IAbstractFactory<ElectronicStatesDeleteControl> _electronicStatesDeleteFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStatesControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="electronicStatesIndexFactory">The factory for creating electronic states index controls.</param>
	/// <param name="electronicStatesDetailsFactory">The factory for creating electronic states details controls.</param>
	/// <param name="electronicStatesCreateFactory">The factory for creating electronic states create controls.</param>
	/// <param name="electronicStatesEditFactory">The factory for creating electronic states edit controls.</param>
	/// <param name="electronicStatesDeleteFactory">The factory for creating electronic states delete controls.</param>
	public ElectronicStatesControl(ILogger<ElectronicStatesControl> logger, IAbstractFactory<ElectronicStatesIndexControl> electronicStatesIndexFactory, IAbstractFactory<ElectronicStatesDetailsControl> electronicStatesDetailsFactory, IAbstractFactory<ElectronicStatesCreateControl> electronicStatesCreateFactory, IAbstractFactory<ElectronicStatesEditControl> electronicStatesEditFactory, IAbstractFactory<ElectronicStatesDeleteControl> electronicStatesDeleteFactory)
	{
		_logger = logger;
		_electronicStatesIndexFactory = electronicStatesIndexFactory;
		_electronicStatesDetailsFactory = electronicStatesDetailsFactory;
		_electronicStatesCreateFactory = electronicStatesCreateFactory;
		_electronicStatesEditFactory = electronicStatesEditFactory;
		_electronicStatesDeleteFactory = electronicStatesDeleteFactory;

		InitializeComponent();
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the OnInitialized event and displays the electronic states index control as the initial content.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} with {EventArgs}.", nameof(ElectronicStatesControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		ElectronicStatesIndexControl indexControl = _electronicStatesIndexFactory.Create();
		indexControl.ChildControlEvent += ElectronicStates_Index_ChildControlEvent;
		ElectronicStatesContent.Content = indexControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesControl), nameof(OnInitialized));
		}
	}

	private void ElectronicStates_Index_ChildControlEvent(object? sender, ChildControlEventArgs<ElectronicStatesIndexControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesControl), nameof(ElectronicStates_Index_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
				ElectronicStatesCreateControl createControl = _electronicStatesCreateFactory.Create();
				createControl.ChildControlEvent += ElectronicStates_Create_ChildControlEvent;
				ElectronicStatesContent.Content = createControl;
				break;
			case "edit":
				ElectronicStatesEditControl editControl = _electronicStatesEditFactory.Create();
				editControl.ElectronicStateId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				editControl.ChildControlEvent += ElectronicStates_Edit_ChildControlEvent;
				ElectronicStatesContent.Content = editControl;
				break;
			case "details":
				ElectronicStatesDetailsControl detailsControl = _electronicStatesDetailsFactory.Create();
				detailsControl.ElectronicStateId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += ElectronicStates_Details_ChildControlEvent;
				ElectronicStatesContent.Content = detailsControl;
				break;
			case "delete":
				ElectronicStatesDeleteControl deleteControl = _electronicStatesDeleteFactory.Create();
				deleteControl.ElectronicStateId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				deleteControl.ChildControlEvent += ElectronicStates_Delete_ChildControlEvent;
				ElectronicStatesContent.Content = deleteControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesControl), nameof(ElectronicStates_Index_ChildControlEvent));
		}
	}

	private void ElectronicStates_Details_ChildControlEvent(object? sender, ChildControlEventArgs<ElectronicStatesDetailsControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesControl), nameof(ElectronicStates_Details_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "edit":
				ElectronicStatesEditControl editControl = _electronicStatesEditFactory.Create();
				editControl.ElectronicStateId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				editControl.ChildControlEvent += ElectronicStates_Edit_ChildControlEvent;
				ElectronicStatesContent.Content = editControl;
				break;
			case "index":
				ElectronicStatesIndexControl indexControl = _electronicStatesIndexFactory.Create();
				indexControl.ChildControlEvent += ElectronicStates_Index_ChildControlEvent;
				ElectronicStatesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesControl), nameof(ElectronicStates_Details_ChildControlEvent));
		}
	}

	private void ElectronicStates_Create_ChildControlEvent(object? sender, ChildControlEventArgs<ElectronicStatesCreateControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesControl), nameof(ElectronicStates_Create_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
				ElectronicStatesDetailsControl detailsControl = _electronicStatesDetailsFactory.Create();
				detailsControl.ElectronicStateId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += ElectronicStates_Details_ChildControlEvent;
				ElectronicStatesContent.Content = detailsControl;
				break;
			case "index":
				ElectronicStatesIndexControl indexControl = _electronicStatesIndexFactory.Create();
				indexControl.ChildControlEvent += ElectronicStates_Index_ChildControlEvent;
				ElectronicStatesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesControl), nameof(ElectronicStates_Create_ChildControlEvent));
		}
	}

	private void ElectronicStates_Edit_ChildControlEvent(object? sender, ChildControlEventArgs<ElectronicStatesEditControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesControl), nameof(ElectronicStates_Edit_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "save":
				ElectronicStatesDetailsControl detailsControl = _electronicStatesDetailsFactory.Create();
				detailsControl.ElectronicStateId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += ElectronicStates_Details_ChildControlEvent;
				ElectronicStatesContent.Content = detailsControl;
				break;
			case "index":
				ElectronicStatesIndexControl indexControl = _electronicStatesIndexFactory.Create();
				indexControl.ChildControlEvent += ElectronicStates_Index_ChildControlEvent;
				ElectronicStatesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesControl), nameof(ElectronicStates_Edit_ChildControlEvent));
		}
	}
	private void ElectronicStates_Delete_ChildControlEvent(object? sender, ChildControlEventArgs<ElectronicStatesDeleteControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesControl), nameof(ElectronicStates_Delete_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "delete":
			case "index":
				ElectronicStatesIndexControl indexControl = _electronicStatesIndexFactory.Create();
				indexControl.ChildControlEvent += ElectronicStates_Index_ChildControlEvent;
				ElectronicStatesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesControl), nameof(ElectronicStates_Delete_ChildControlEvent));
		}
	}
}
