using System.Windows.Controls;

using GaussianCommonLibrary.ErrorModels;

using GaussianWPF.Controls.BaseMethods;
using GaussianWPF.FactoryHelpers;

using GaussianWPFLibrary.DataAccess;
using GaussianWPFLibrary.EventModels;
using GaussianWPFLibrary.Models;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls;

/// <summary>
/// Represents a container control that manages navigation and coordination between different 
/// BaseMethod-related child controls (Index, Create, Edit, Details, Delete).
/// This control handles event-driven navigation and maintains the lifecycle of child controls
/// through dependency injection and abstract factory patterns.
/// </summary>
/// <remarks>
/// This control acts as a coordinator/container for CRUD operations on BaseMethods.
/// It responds to events from child controls to navigate between different views while
/// maintaining clean separation of concerns.
/// </remarks>
public partial class BaseMethodsControl : UserControl
{
	private readonly ILogger<BaseMethodsControl> _logger;
	private readonly ILoggedInUserModel _loggedInUser;
	private readonly IApiHelper _apiHelper;
	private readonly IAbstractFactory<BaseMethodsIndexControl> _baseMethodsIndexFactory;
	private readonly IAbstractFactory<BaseMethodsDetailsControl> _baseMethodsDetailsFactory;
	private readonly IAbstractFactory<BaseMethodsCreateControl> _baseMethodsCreateFactory;
	private readonly IAbstractFactory<BaseMethodsEditControl> _baseMethodsEditFactory;
	private readonly IAbstractFactory<BaseMethodsDeleteControl> _baseMethodsDeleteFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodsControl"/> class.
	/// </summary>
	/// <param name="logger">Logger instance for tracing control events.</param>
	/// <param name="loggedInUser">Model representing the currently logged-in user.</param>
	/// <param name="apiHelper">Helper for making API requests.</param>
	/// <param name="baseMethodsIndexFactory">Factory for creating index controls.</param>
	/// <param name="baseMethodsDetailsFactory">Factory for creating details controls.</param>
	/// <param name="baseMethodsCreateFactory">Factory for creating create controls.</param>
	/// <param name="baseMethodsEditFactory">Factory for creating edit controls.</param>
	/// <param name="baseMethodsDeleteFactory">Factory for creating delete controls.</param>
	/// <remarks>
	/// This constructor initializes the control and immediately loads the Index view as the default view.
	/// All dependencies are injected through the constructor to support testability and loose coupling.
	/// </remarks>
	public BaseMethodsControl(ILogger<BaseMethodsControl> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, IAbstractFactory<BaseMethodsIndexControl> baseMethodsIndexFactory, IAbstractFactory<BaseMethodsDetailsControl> baseMethodsDetailsFactory, IAbstractFactory<BaseMethodsCreateControl> baseMethodsCreateFactory, IAbstractFactory<BaseMethodsEditControl> baseMethodsEditFactory, IAbstractFactory<BaseMethodsDeleteControl> baseMethodsDeleteFactory)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_baseMethodsIndexFactory = baseMethodsIndexFactory;
		_baseMethodsDetailsFactory = baseMethodsDetailsFactory;
		_baseMethodsCreateFactory = baseMethodsCreateFactory;
		_baseMethodsEditFactory = baseMethodsEditFactory;
		_baseMethodsDeleteFactory = baseMethodsDeleteFactory;

		InitializeComponent();

		BaseMethodsIndexControl indexControl = _baseMethodsIndexFactory.Create();
		indexControl.ChildControlEvent += BaseMethods_Index_ChildControlEvent;
		BaseMethodsContent.Content = indexControl;
	}

	private void BaseMethods_Index_ChildControlEvent(object? sender, ChildControlEventArgs<BaseMethodsIndexControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsControl), nameof(BaseMethods_Index_ChildControlEvent), sender, e);
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
	}

	private void BaseMethods_Create_ChildControlEvent(object? sender, ChildControlEventArgs<BaseMethodsCreateControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsControl), nameof(BaseMethods_Create_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
			case "index":
				BaseMethodsIndexControl indexControl = _baseMethodsIndexFactory.Create();
				indexControl.ChildControlEvent += BaseMethods_Index_ChildControlEvent;
				BaseMethodsContent.Content = indexControl;
				break;
			default:
				break;
		}
	}

	private void BaseMethods_Edit_ChildControlEvent(object? sender, ChildControlEventArgs<BaseMethodsEditControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsControl), nameof(BaseMethods_Edit_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "save":
			case "index":
				BaseMethodsIndexControl indexControl = _baseMethodsIndexFactory.Create();
				indexControl.ChildControlEvent += BaseMethods_Index_ChildControlEvent;
				BaseMethodsContent.Content = indexControl;
				break;
			default:
				break;
		}
	}

	private void BaseMethods_Details_ChildControlEvent(object? sender, ChildControlEventArgs<BaseMethodsDetailsControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsControl), nameof(BaseMethods_Details_ChildControlEvent), sender, e);
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
	}
	private void BaseMethods_Delete_ChildControlEvent(object? sender, ChildControlEventArgs<BaseMethodsDeleteControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(BaseMethodsControl), nameof(BaseMethods_Delete_ChildControlEvent), sender, e);
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
	}
}
