using System.Windows.Controls;

using GaussianCommonLibrary.ErrorModels;

using GaussianWPF.Controls.FullMethods;
using GaussianWPF.FactoryHelpers;

using GaussianWPFLibrary.EventModels;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls;

/// <summary>
/// Interaction logic for FullMethodsControl.xaml
/// </summary>
public partial class FullMethodsControl : UserControl
{
	private readonly ILogger<FullMethodsControl> _logger;
	private readonly IAbstractFactory<FullMethodsIndexControl> _fullMethodsIndexFactory;
	private readonly IAbstractFactory<FullMethodsDetailsControl> _fullMethodsDetailsFactory;
	private readonly IAbstractFactory<FullMethodsCreateControl> _fullMethodsCreateFactory;
	private readonly IAbstractFactory<FullMethodsEditControl> _fullMethodsEditFactory;
	private readonly IAbstractFactory<FullMethodsDeleteControl> _fullMethodsDeleteFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodsControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="fullMethodsIndexFactory">The factory for creating Full Methods index controls.</param>
	/// <param name="fullMethodsDetailsFactory">The factory for creating Full Methods details controls.</param>
	/// <param name="fullMethodsCreateFactory">The factory for creating Full Methods create controls.</param>
	/// <param name="fullMethodsEditFactory">The factory for creating Full Methods edit controls.</param>
	/// <param name="fullMethodsDeleteFactory">The factory for creating Full Methods delete controls.</param>
	public FullMethodsControl(ILogger<FullMethodsControl> logger, IAbstractFactory<FullMethodsIndexControl> fullMethodsIndexFactory, IAbstractFactory<FullMethodsDetailsControl> fullMethodsDetailsFactory, IAbstractFactory<FullMethodsCreateControl> fullMethodsCreateFactory, IAbstractFactory<FullMethodsEditControl> fullMethodsEditFactory, IAbstractFactory<FullMethodsDeleteControl> fullMethodsDeleteFactory)
	{
		_logger = logger;
		_fullMethodsIndexFactory = fullMethodsIndexFactory;
		_fullMethodsDetailsFactory = fullMethodsDetailsFactory;
		_fullMethodsCreateFactory = fullMethodsCreateFactory;
		_fullMethodsEditFactory = fullMethodsEditFactory;
		_fullMethodsDeleteFactory = fullMethodsDeleteFactory;

		InitializeComponent();
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the OnInitialized event and displays the Full Methods index control as the initial content.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} with {EventArgs}.", nameof(FullMethodsControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		FullMethodsIndexControl indexControl = _fullMethodsIndexFactory.Create();
		indexControl.ChildControlEvent += FullMethods_Index_ChildControlEvent;
		FullMethodsContent.Content = indexControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(FullMethodsControl), nameof(OnInitialized));
		}
	}

	private void FullMethods_Index_ChildControlEvent(object? sender, ChildControlEventArgs<FullMethodsIndexControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(FullMethodsControl), nameof(FullMethods_Index_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
				FullMethodsCreateControl createControl = _fullMethodsCreateFactory.Create();
				createControl.ChildControlEvent += FullMethods_Create_ChildControlEvent;
				FullMethodsContent.Content = createControl;
				break;
			case "edit":
				FullMethodsEditControl editControl = _fullMethodsEditFactory.Create();
				editControl.FullMethodId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				editControl.ChildControlEvent += FullMethods_Edit_ChildControlEvent;
				FullMethodsContent.Content = editControl;
				break;
			case "details":
				FullMethodsDetailsControl detailsControl = _fullMethodsDetailsFactory.Create();
				detailsControl.FullMethodId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += FullMethods_Details_ChildControlEvent;
				FullMethodsContent.Content = detailsControl;
				break;
			case "delete":
				FullMethodsDeleteControl deleteControl = _fullMethodsDeleteFactory.Create();
				deleteControl.FullMethodId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				deleteControl.ChildControlEvent += FullMethods_Delete_ChildControlEvent;
				FullMethodsContent.Content = deleteControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(FullMethodsControl), nameof(FullMethods_Index_ChildControlEvent));
		}
	}

	private void FullMethods_Details_ChildControlEvent(object? sender, ChildControlEventArgs<FullMethodsDetailsControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(FullMethodsControl), nameof(FullMethods_Details_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "edit":
				FullMethodsEditControl editControl = _fullMethodsEditFactory.Create();
				editControl.FullMethodId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				editControl.ChildControlEvent += FullMethods_Edit_ChildControlEvent;
				FullMethodsContent.Content = editControl;
				break;
			case "index":
				FullMethodsIndexControl indexControl = _fullMethodsIndexFactory.Create();
				indexControl.ChildControlEvent += FullMethods_Index_ChildControlEvent;
				FullMethodsContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(FullMethodsControl), nameof(FullMethods_Details_ChildControlEvent));
		}
	}

	private void FullMethods_Create_ChildControlEvent(object? sender, ChildControlEventArgs<FullMethodsCreateControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(FullMethodsControl), nameof(FullMethods_Create_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
				FullMethodsDetailsControl detailsControl = _fullMethodsDetailsFactory.Create();
				detailsControl.FullMethodId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += FullMethods_Details_ChildControlEvent;
				FullMethodsContent.Content = detailsControl;
				break;
			case "index":
				FullMethodsIndexControl indexControl = _fullMethodsIndexFactory.Create();
				indexControl.ChildControlEvent += FullMethods_Index_ChildControlEvent;
				FullMethodsContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(FullMethodsControl), nameof(FullMethods_Create_ChildControlEvent));
		}
	}

	private void FullMethods_Edit_ChildControlEvent(object? sender, ChildControlEventArgs<FullMethodsEditControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(FullMethodsControl), nameof(FullMethods_Edit_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "save":
				FullMethodsDetailsControl detailsControl = _fullMethodsDetailsFactory.Create();
				detailsControl.FullMethodId = e.ItemId ?? throw new NullParameterException(nameof(e.ItemId), $"The {nameof(e.ItemId)} cannot be null.");
				detailsControl.ChildControlEvent += FullMethods_Details_ChildControlEvent;
				FullMethodsContent.Content = detailsControl;
				break;
			case "index":
				FullMethodsIndexControl indexControl = _fullMethodsIndexFactory.Create();
				indexControl.ChildControlEvent += FullMethods_Index_ChildControlEvent;
				FullMethodsContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(FullMethodsControl), nameof(FullMethods_Edit_ChildControlEvent));
		}
	}
	private void FullMethods_Delete_ChildControlEvent(object? sender, ChildControlEventArgs<FullMethodsDeleteControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(FullMethodsControl), nameof(FullMethods_Delete_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "delete":
			case "index":
				FullMethodsIndexControl indexControl = _fullMethodsIndexFactory.Create();
				indexControl.ChildControlEvent += FullMethods_Index_ChildControlEvent;
				FullMethodsContent.Content = indexControl;
				break;
			default:
				break;
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(FullMethodsControl), nameof(FullMethods_Delete_ChildControlEvent));
		}
	}
}
