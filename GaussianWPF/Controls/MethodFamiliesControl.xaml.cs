using System.Windows.Controls;

using GaussianCommonLibrary.ErrorModels;

using GaussianWPF.Controls.MethodFamilies;
using GaussianWPF.FactoryHelpers;

using GaussianWPFLibrary.EventModels;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls;

/// <summary>
/// Interaction logic for MethodFamiliesControl.xaml
/// </summary>
public partial class MethodFamiliesControl : UserControl
{
	private readonly ILogger<MethodFamiliesControl> _logger;
	private readonly IAbstractFactory<MethodFamiliesIndexControl> _methodFamiliesIndexFactory;
	private readonly IAbstractFactory<MethodFamiliesDetailsControl> _methodFamiliesDetailsFactory;
	private readonly IAbstractFactory<MethodFamiliesCreateControl> _methodFamiliesCreateFactory;
	private readonly IAbstractFactory<MethodFamiliesEditControl> _methodFamiliesEditFactory;
	private readonly IAbstractFactory<MethodFamiliesDeleteControl> _methodFamiliesDeleteFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="MethodFamiliesControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="methodFamiliesIndexFactory">The factory for creating method families index controls.</param>
	/// <param name="methodFamiliesDetailsFactory">The factory for creating method families details controls.</param>
	/// <param name="methodFamiliesCreateFactory">The factory for creating method families create controls.</param>
	/// <param name="methodFamiliesEditFactory">The factory for creating method families edit controls.</param>
	/// <param name="methodFamiliesDeleteFactory">The factory for creating method families delete controls.</param>
	public MethodFamiliesControl(ILogger<MethodFamiliesControl> logger, IAbstractFactory<MethodFamiliesIndexControl> methodFamiliesIndexFactory, IAbstractFactory<MethodFamiliesDetailsControl> methodFamiliesDetailsFactory, IAbstractFactory<MethodFamiliesCreateControl> methodFamiliesCreateFactory, IAbstractFactory<MethodFamiliesEditControl> methodFamiliesEditFactory, IAbstractFactory<MethodFamiliesDeleteControl> methodFamiliesDeleteFactory)
	{
		_logger = logger;
		_methodFamiliesIndexFactory = methodFamiliesIndexFactory;
		_methodFamiliesDetailsFactory = methodFamiliesDetailsFactory;
		_methodFamiliesCreateFactory = methodFamiliesCreateFactory;
		_methodFamiliesEditFactory = methodFamiliesEditFactory;
		_methodFamiliesDeleteFactory = methodFamiliesDeleteFactory;

		InitializeComponent();
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the OnInitialized event and displays the method families index control as the initial content.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} with {EventArgs}.", nameof(MethodFamiliesControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		MethodFamiliesIndexControl indexControl = _methodFamiliesIndexFactory.Create();
		indexControl.ChildControlEvent += MethodFamilies_Index_ChildControlEvent;
		MethodFamiliesContent.Content = indexControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(MethodFamiliesControl), nameof(OnInitialized));
		}
	}

	private void MethodFamilies_Index_ChildControlEvent(object? sender, ChildControlEventArgs<MethodFamiliesIndexControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MethodFamiliesControl), nameof(MethodFamilies_Index_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
				MethodFamiliesCreateControl createControl = _methodFamiliesCreateFactory.Create();
				createControl.ChildControlEvent += MethodFamilies_Create_ChildControlEvent;
				MethodFamiliesContent.Content = createControl;
				break;
			case "edit":
				MethodFamiliesEditControl editControl = _methodFamiliesEditFactory.Create();
				editControl.MethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				editControl.ChildControlEvent += MethodFamilies_Edit_ChildControlEvent;
				MethodFamiliesContent.Content = editControl;
				break;
			case "details":
				MethodFamiliesDetailsControl detailsControl = _methodFamiliesDetailsFactory.Create();
				detailsControl.MethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += MethodFamilies_Details_ChildControlEvent;
				MethodFamiliesContent.Content = detailsControl;
				break;
			case "delete":
				MethodFamiliesDeleteControl deleteControl = _methodFamiliesDeleteFactory.Create();
				deleteControl.MethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				deleteControl.ChildControlEvent += MethodFamilies_Delete_ChildControlEvent;
				MethodFamiliesContent.Content = deleteControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(MethodFamiliesControl), nameof(MethodFamilies_Index_ChildControlEvent));
		}
	}

	private void MethodFamilies_Details_ChildControlEvent(object? sender, ChildControlEventArgs<MethodFamiliesDetailsControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MethodFamiliesControl), nameof(MethodFamilies_Details_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "edit":
				MethodFamiliesEditControl editControl = _methodFamiliesEditFactory.Create();
				editControl.MethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				editControl.ChildControlEvent += MethodFamilies_Edit_ChildControlEvent;
				MethodFamiliesContent.Content = editControl;
				break;
			case "index":
				MethodFamiliesIndexControl indexControl = _methodFamiliesIndexFactory.Create();
				indexControl.ChildControlEvent += MethodFamilies_Index_ChildControlEvent;
				MethodFamiliesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(MethodFamiliesControl), nameof(MethodFamilies_Details_ChildControlEvent));
		}
	}

	private void MethodFamilies_Create_ChildControlEvent(object? sender, ChildControlEventArgs<MethodFamiliesCreateControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MethodFamiliesControl), nameof(MethodFamilies_Create_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
				MethodFamiliesDetailsControl detailsControl = _methodFamiliesDetailsFactory.Create();
				detailsControl.MethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += MethodFamilies_Details_ChildControlEvent;
				MethodFamiliesContent.Content = detailsControl;
				break;
			case "index":
				MethodFamiliesIndexControl indexControl = _methodFamiliesIndexFactory.Create();
				indexControl.ChildControlEvent += MethodFamilies_Index_ChildControlEvent;
				MethodFamiliesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(MethodFamiliesControl), nameof(MethodFamilies_Create_ChildControlEvent));
		}
	}

	private void MethodFamilies_Edit_ChildControlEvent(object? sender, ChildControlEventArgs<MethodFamiliesEditControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MethodFamiliesControl), nameof(MethodFamilies_Edit_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "save":
				MethodFamiliesDetailsControl detailsControl = _methodFamiliesDetailsFactory.Create();
				detailsControl.MethodFamilyId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += MethodFamilies_Details_ChildControlEvent;
				MethodFamiliesContent.Content = detailsControl;
				break;
			case "index":
				MethodFamiliesIndexControl indexControl = _methodFamiliesIndexFactory.Create();
				indexControl.ChildControlEvent += MethodFamilies_Index_ChildControlEvent;
				MethodFamiliesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(MethodFamiliesControl), nameof(MethodFamilies_Edit_ChildControlEvent));
		}
	}
	private void MethodFamilies_Delete_ChildControlEvent(object? sender, ChildControlEventArgs<MethodFamiliesDeleteControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MethodFamiliesControl), nameof(MethodFamilies_Delete_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "delete":
			case "index":
				MethodFamiliesIndexControl indexControl = _methodFamiliesIndexFactory.Create();
				indexControl.ChildControlEvent += MethodFamilies_Index_ChildControlEvent;
				MethodFamiliesContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(MethodFamiliesControl), nameof(MethodFamilies_Delete_ChildControlEvent));
		}
	}
}
