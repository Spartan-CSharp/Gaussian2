using System.Windows.Controls;

using GaussianWPF.Controls.MethodFamilies;
using GaussianWPF.FactoryHelpers;

using GaussianWPFLibrary.DataAccess;
using GaussianWPFLibrary.ErrorModels;
using GaussianWPFLibrary.EventModels;
using GaussianWPFLibrary.Models;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls;

/// <summary>
/// Represents a container control that manages navigation and coordination between different 
/// MethodFamily-related child controls (Index, Create, Edit, Details, Delete).
/// This control handles event-driven navigation and maintains the lifecycle of child controls
/// through dependency injection and abstract factory patterns.
/// </summary>
/// <remarks>
/// This control acts as a coordinator/container for CRUD operations on MethodFamilies.
/// It responds to events from child controls to navigate between different views while
/// maintaining clean separation of concerns.
/// </remarks>
public partial class MethodFamiliesControl : UserControl
{
	private readonly ILogger<MethodFamiliesControl> _logger;
	private readonly ILoggedInUserModel _loggedInUser;
	private readonly IApiHelper _apiHelper;
	private readonly IAbstractFactory<MethodFamiliesIndexControl> _methodFamiliesIndexFactory;
	private readonly IAbstractFactory<MethodFamiliesDetailsControl> _methodFamiliesDetailsFactory;
	private readonly IAbstractFactory<MethodFamiliesCreateControl> _methodFamiliesCreateFactory;
	private readonly IAbstractFactory<MethodFamiliesEditControl> _methodFamiliesEditFactory;
	private readonly IAbstractFactory<MethodFamiliesDeleteControl> _methodFamiliesDeleteFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="MethodFamiliesControl"/> class.
	/// </summary>
	/// <param name="logger">Logger instance for tracing control events.</param>
	/// <param name="loggedInUser">Model representing the currently logged-in user.</param>
	/// <param name="apiHelper">Helper for making API requests.</param>
	/// <param name="methodFamiliesIndexFactory">Factory for creating index controls.</param>
	/// <param name="methodFamiliesDetailsFactory">Factory for creating details controls.</param>
	/// <param name="methodFamiliesCreateFactory">Factory for creating create controls.</param>
	/// <param name="methodFamiliesEditFactory">Factory for creating edit controls.</param>
	/// <param name="methodFamiliesDeleteFactory">Factory for creating delete controls.</param>
	/// <remarks>
	/// This constructor initializes the control and immediately loads the Index view as the default view.
	/// All dependencies are injected through the constructor to support testability and loose coupling.
	/// </remarks>
	public MethodFamiliesControl(ILogger<MethodFamiliesControl> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, IAbstractFactory<MethodFamiliesIndexControl> methodFamiliesIndexFactory, IAbstractFactory<MethodFamiliesDetailsControl> methodFamiliesDetailsFactory, IAbstractFactory<MethodFamiliesCreateControl> methodFamiliesCreateFactory, IAbstractFactory<MethodFamiliesEditControl> methodFamiliesEditFactory, IAbstractFactory<MethodFamiliesDeleteControl> methodFamiliesDeleteFactory)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_methodFamiliesIndexFactory = methodFamiliesIndexFactory;
		_methodFamiliesDetailsFactory = methodFamiliesDetailsFactory;
		_methodFamiliesCreateFactory = methodFamiliesCreateFactory;
		_methodFamiliesEditFactory = methodFamiliesEditFactory;
		_methodFamiliesDeleteFactory = methodFamiliesDeleteFactory;

		InitializeComponent();

		MethodFamiliesIndexControl indexControl = _methodFamiliesIndexFactory.Create();
		indexControl.ChildControlEvent += MethodFamilies_Index_ChildControlEvent;
		MethodFamiliesContent.Content = indexControl;
	}

	private void MethodFamilies_Index_ChildControlEvent(object? sender, ChildControlEventArgs<MethodFamiliesIndexControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesControl), nameof(MethodFamilies_Index_ChildControlEvent), sender, e);
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
	}

	private void MethodFamilies_Create_ChildControlEvent(object? sender, ChildControlEventArgs<MethodFamiliesCreateControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesControl), nameof(MethodFamilies_Create_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
			case "index":
				MethodFamiliesIndexControl indexControl = _methodFamiliesIndexFactory.Create();
				indexControl.ChildControlEvent += MethodFamilies_Index_ChildControlEvent;
				MethodFamiliesContent.Content = indexControl;
				break;
			default:
				break;
		}
	}

	private void MethodFamilies_Edit_ChildControlEvent(object? sender, ChildControlEventArgs<MethodFamiliesEditControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesControl), nameof(MethodFamilies_Edit_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "save":
			case "index":
				MethodFamiliesIndexControl indexControl = _methodFamiliesIndexFactory.Create();
				indexControl.ChildControlEvent += MethodFamilies_Index_ChildControlEvent;
				MethodFamiliesContent.Content = indexControl;
				break;
			default:
				break;
		}
	}

	private void MethodFamilies_Details_ChildControlEvent(object? sender, ChildControlEventArgs<MethodFamiliesDetailsControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesControl), nameof(MethodFamilies_Details_ChildControlEvent), sender, e);
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
	}
	private void MethodFamilies_Delete_ChildControlEvent(object? sender, ChildControlEventArgs<MethodFamiliesDeleteControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesControl), nameof(MethodFamilies_Delete_ChildControlEvent), sender, e);
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
	}
}
