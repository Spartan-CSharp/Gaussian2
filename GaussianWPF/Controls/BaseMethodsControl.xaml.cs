using System.Windows.Controls;

using GaussianCommonLibrary.ErrorModels;

using GaussianWPF.Controls.BaseMethods;
using GaussianWPF.FactoryHelpers;

using GaussianWPFLibrary.EventModels;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls;

/// <summary>
/// Interaction logic for BaseMethodsControl.xaml
/// </summary>
public partial class BaseMethodsControl : UserControl
{
	private readonly ILogger<BaseMethodsControl> _logger;
	private readonly IAbstractFactory<BaseMethodsIndexControl> _baseMethodsIndexFactory;
	private readonly IAbstractFactory<BaseMethodsDetailsControl> _baseMethodsDetailsFactory;
	private readonly IAbstractFactory<BaseMethodsCreateControl> _baseMethodsCreateFactory;
	private readonly IAbstractFactory<BaseMethodsEditControl> _baseMethodsEditFactory;
	private readonly IAbstractFactory<BaseMethodsDeleteControl> _baseMethodsDeleteFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodsControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="baseMethodsIndexFactory">The factory for creating Base Methods index controls.</param>
	/// <param name="baseMethodsDetailsFactory">The factory for creating Base Methods details controls.</param>
	/// <param name="baseMethodsCreateFactory">The factory for creating Base Methods create controls.</param>
	/// <param name="baseMethodsEditFactory">The factory for creating Base Methods edit controls.</param>
	/// <param name="baseMethodsDeleteFactory">The factory for creating Base Methods delete controls.</param>
	public BaseMethodsControl(ILogger<BaseMethodsControl> logger, IAbstractFactory<BaseMethodsIndexControl> baseMethodsIndexFactory, IAbstractFactory<BaseMethodsDetailsControl> baseMethodsDetailsFactory, IAbstractFactory<BaseMethodsCreateControl> baseMethodsCreateFactory, IAbstractFactory<BaseMethodsEditControl> baseMethodsEditFactory, IAbstractFactory<BaseMethodsDeleteControl> baseMethodsDeleteFactory)
	{
		_logger = logger;
		_baseMethodsIndexFactory = baseMethodsIndexFactory;
		_baseMethodsDetailsFactory = baseMethodsDetailsFactory;
		_baseMethodsCreateFactory = baseMethodsCreateFactory;
		_baseMethodsEditFactory = baseMethodsEditFactory;
		_baseMethodsDeleteFactory = baseMethodsDeleteFactory;

		InitializeComponent();
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the OnInitialized event and displays the Base Methods index control as the initial content.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} with {EventArgs}.", nameof(BaseMethodsControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		BaseMethodsIndexControl indexControl = _baseMethodsIndexFactory.Create();
		indexControl.ChildControlEvent += BaseMethods_Index_ChildControlEvent;
		BaseMethodsContent.Content = indexControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(BaseMethodsControl), nameof(OnInitialized));
		}
	}

	private void BaseMethods_Index_ChildControlEvent(object? sender, ChildControlEventArgs<BaseMethodsIndexControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(BaseMethodsControl), nameof(BaseMethods_Index_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
				BaseMethodsCreateControl createControl = _baseMethodsCreateFactory.Create();
				createControl.ChildControlEvent += BaseMethods_Create_ChildControlEvent;
				BaseMethodsContent.Content = createControl;
				break;
			case "edit":
				BaseMethodsEditControl editControl = _baseMethodsEditFactory.Create();
				editControl.BaseMethodId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				editControl.ChildControlEvent += BaseMethods_Edit_ChildControlEvent;
				BaseMethodsContent.Content = editControl;
				break;
			case "details":
				BaseMethodsDetailsControl detailsControl = _baseMethodsDetailsFactory.Create();
				detailsControl.BaseMethodId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += BaseMethods_Details_ChildControlEvent;
				BaseMethodsContent.Content = detailsControl;
				break;
			case "delete":
				BaseMethodsDeleteControl deleteControl = _baseMethodsDeleteFactory.Create();
				deleteControl.BaseMethodId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				deleteControl.ChildControlEvent += BaseMethods_Delete_ChildControlEvent;
				BaseMethodsContent.Content = deleteControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(BaseMethodsControl), nameof(BaseMethods_Index_ChildControlEvent));
		}
	}

	private void BaseMethods_Details_ChildControlEvent(object? sender, ChildControlEventArgs<BaseMethodsDetailsControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(BaseMethodsControl), nameof(BaseMethods_Details_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "edit":
				BaseMethodsEditControl editControl = _baseMethodsEditFactory.Create();
				editControl.BaseMethodId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				editControl.ChildControlEvent += BaseMethods_Edit_ChildControlEvent;
				BaseMethodsContent.Content = editControl;
				break;
			case "index":
				BaseMethodsIndexControl indexControl = _baseMethodsIndexFactory.Create();
				indexControl.ChildControlEvent += BaseMethods_Index_ChildControlEvent;
				BaseMethodsContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(BaseMethodsControl), nameof(BaseMethods_Details_ChildControlEvent));
		}
	}

	private void BaseMethods_Create_ChildControlEvent(object? sender, ChildControlEventArgs<BaseMethodsCreateControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(BaseMethodsControl), nameof(BaseMethods_Create_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
				BaseMethodsDetailsControl detailsControl = _baseMethodsDetailsFactory.Create();
				detailsControl.BaseMethodId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += BaseMethods_Details_ChildControlEvent;
				BaseMethodsContent.Content = detailsControl;
				break;
			case "index":
				BaseMethodsIndexControl indexControl = _baseMethodsIndexFactory.Create();
				indexControl.ChildControlEvent += BaseMethods_Index_ChildControlEvent;
				BaseMethodsContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(BaseMethodsControl), nameof(BaseMethods_Create_ChildControlEvent));
		}
	}

	private void BaseMethods_Edit_ChildControlEvent(object? sender, ChildControlEventArgs<BaseMethodsEditControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(BaseMethodsControl), nameof(BaseMethods_Edit_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "save":
				BaseMethodsDetailsControl detailsControl = _baseMethodsDetailsFactory.Create();
				detailsControl.BaseMethodId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += BaseMethods_Details_ChildControlEvent;
				BaseMethodsContent.Content = detailsControl;
				break;
			case "index":
				BaseMethodsIndexControl indexControl = _baseMethodsIndexFactory.Create();
				indexControl.ChildControlEvent += BaseMethods_Index_ChildControlEvent;
				BaseMethodsContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(BaseMethodsControl), nameof(BaseMethods_Edit_ChildControlEvent));
		}
	}
	private void BaseMethods_Delete_ChildControlEvent(object? sender, ChildControlEventArgs<BaseMethodsDeleteControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(BaseMethodsControl), nameof(BaseMethods_Delete_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "delete":
			case "index":
				BaseMethodsIndexControl indexControl = _baseMethodsIndexFactory.Create();
				indexControl.ChildControlEvent += BaseMethods_Index_ChildControlEvent;
				BaseMethodsContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(BaseMethodsControl), nameof(BaseMethods_Delete_ChildControlEvent));
		}
	}
}
