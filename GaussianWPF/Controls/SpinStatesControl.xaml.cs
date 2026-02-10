using System.Windows.Controls;

using GaussianCommonLibrary.ErrorModels;

using GaussianWPF.Controls.SpinStates;
using GaussianWPF.FactoryHelpers;

using GaussianWPFLibrary.EventModels;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls;

/// <summary>
/// Interaction logic for SpinStatesControl.xaml
/// </summary>
public partial class SpinStatesControl : UserControl
{
	private readonly ILogger<SpinStatesControl> _logger;
	private readonly IAbstractFactory<SpinStatesIndexControl> _spinStatesIndexFactory;
	private readonly IAbstractFactory<SpinStatesDetailsControl> _spinStatesDetailsFactory;
	private readonly IAbstractFactory<SpinStatesCreateControl> _spinStatesCreateFactory;
	private readonly IAbstractFactory<SpinStatesEditControl> _spinStatesEditFactory;
	private readonly IAbstractFactory<SpinStatesDeleteControl> _spinStatesDeleteFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStatesControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="spinStatesIndexFactory">The factory for creating Spin States index controls.</param>
	/// <param name="spinStatesDetailsFactory">The factory for creating Spin States details controls.</param>
	/// <param name="spinStatesCreateFactory">The factory for creating Spin States create controls.</param>
	/// <param name="spinStatesEditFactory">The factory for creating Spin States edit controls.</param>
	/// <param name="spinStatesDeleteFactory">The factory for creating Spin States delete controls.</param>
	public SpinStatesControl(ILogger<SpinStatesControl> logger, IAbstractFactory<SpinStatesIndexControl> spinStatesIndexFactory, IAbstractFactory<SpinStatesDetailsControl> spinStatesDetailsFactory, IAbstractFactory<SpinStatesCreateControl> spinStatesCreateFactory, IAbstractFactory<SpinStatesEditControl> spinStatesEditFactory, IAbstractFactory<SpinStatesDeleteControl> spinStatesDeleteFactory)
	{
		_logger = logger;
		_spinStatesIndexFactory = spinStatesIndexFactory;
		_spinStatesDetailsFactory = spinStatesDetailsFactory;
		_spinStatesCreateFactory = spinStatesCreateFactory;
		_spinStatesEditFactory = spinStatesEditFactory;
		_spinStatesDeleteFactory = spinStatesDeleteFactory;

		InitializeComponent();
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the OnInitialized event and displays the Spin States index control as the initial content.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} with {EventArgs}.", nameof(SpinStatesControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		SpinStatesIndexControl indexControl = _spinStatesIndexFactory.Create();
		indexControl.ChildControlEvent += SpinStates_Index_ChildControlEvent;
		SpinStatesContent.Content = indexControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesControl), nameof(OnInitialized));
		}
	}

	private void SpinStates_Index_ChildControlEvent(object? sender, ChildControlEventArgs<SpinStatesIndexControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesControl), nameof(SpinStates_Index_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
				SpinStatesCreateControl createControl = _spinStatesCreateFactory.Create();
				createControl.ChildControlEvent += SpinStates_Create_ChildControlEvent;
				SpinStatesContent.Content = createControl;
				break;
			case "edit":
				SpinStatesEditControl editControl = _spinStatesEditFactory.Create();
				editControl.SpinStateId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				editControl.ChildControlEvent += SpinStates_Edit_ChildControlEvent;
				SpinStatesContent.Content = editControl;
				break;
			case "details":
				SpinStatesDetailsControl detailsControl = _spinStatesDetailsFactory.Create();
				detailsControl.SpinStateId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += SpinStates_Details_ChildControlEvent;
				SpinStatesContent.Content = detailsControl;
				break;
			case "delete":
				SpinStatesDeleteControl deleteControl = _spinStatesDeleteFactory.Create();
				deleteControl.SpinStateId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				deleteControl.ChildControlEvent += SpinStates_Delete_ChildControlEvent;
				SpinStatesContent.Content = deleteControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesControl), nameof(SpinStates_Index_ChildControlEvent));
		}
	}

	private void SpinStates_Details_ChildControlEvent(object? sender, ChildControlEventArgs<SpinStatesDetailsControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesControl), nameof(SpinStates_Details_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "edit":
				SpinStatesEditControl editControl = _spinStatesEditFactory.Create();
				editControl.SpinStateId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				editControl.ChildControlEvent += SpinStates_Edit_ChildControlEvent;
				SpinStatesContent.Content = editControl;
				break;
			case "index":
				SpinStatesIndexControl indexControl = _spinStatesIndexFactory.Create();
				indexControl.ChildControlEvent += SpinStates_Index_ChildControlEvent;
				SpinStatesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesControl), nameof(SpinStates_Details_ChildControlEvent));
		}
	}

	private void SpinStates_Create_ChildControlEvent(object? sender, ChildControlEventArgs<SpinStatesCreateControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesControl), nameof(SpinStates_Create_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
				SpinStatesDetailsControl detailsControl = _spinStatesDetailsFactory.Create();
				detailsControl.SpinStateId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += SpinStates_Details_ChildControlEvent;
				SpinStatesContent.Content = detailsControl;
				break;
			case "index":
				SpinStatesIndexControl indexControl = _spinStatesIndexFactory.Create();
				indexControl.ChildControlEvent += SpinStates_Index_ChildControlEvent;
				SpinStatesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesControl), nameof(SpinStates_Create_ChildControlEvent));
		}
	}

	private void SpinStates_Edit_ChildControlEvent(object? sender, ChildControlEventArgs<SpinStatesEditControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesControl), nameof(SpinStates_Edit_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "save":
				SpinStatesDetailsControl detailsControl = _spinStatesDetailsFactory.Create();
				detailsControl.SpinStateId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += SpinStates_Details_ChildControlEvent;
				SpinStatesContent.Content = detailsControl;
				break;
			case "index":
				SpinStatesIndexControl indexControl = _spinStatesIndexFactory.Create();
				indexControl.ChildControlEvent += SpinStates_Index_ChildControlEvent;
				SpinStatesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesControl), nameof(SpinStates_Edit_ChildControlEvent));
		}
	}
	private void SpinStates_Delete_ChildControlEvent(object? sender, ChildControlEventArgs<SpinStatesDeleteControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesControl), nameof(SpinStates_Delete_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "delete":
			case "index":
				SpinStatesIndexControl indexControl = _spinStatesIndexFactory.Create();
				indexControl.ChildControlEvent += SpinStates_Index_ChildControlEvent;
				SpinStatesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesControl), nameof(SpinStates_Delete_ChildControlEvent));
		}
	}
}
