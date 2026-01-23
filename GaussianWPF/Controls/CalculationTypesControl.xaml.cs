using System.Windows.Controls;

using GaussianWPF.Controls.CalculationTypes;
using GaussianWPF.FactoryHelpers;

using GaussianWPFLibrary.DataAccess;
using GaussianWPFLibrary.ErrorModels;
using GaussianWPFLibrary.EventModels;
using GaussianWPFLibrary.Models;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls;

/// <summary>
/// Represents a container control that manages navigation and coordination between different 
/// CalculationType-related child controls (Index, Create, Edit, Details, Delete).
/// This control handles event-driven navigation and maintains the lifecycle of child controls
/// through dependency injection and abstract factory patterns.
/// </summary>
/// <remarks>
/// This control acts as a coordinator/container for CRUD operations on CalculationTypes.
/// It responds to events from child controls to navigate between different views while
/// maintaining clean separation of concerns.
/// </remarks>
public partial class CalculationTypesControl : UserControl
{
	private readonly ILogger<CalculationTypesControl> _logger;
	private readonly ILoggedInUserModel _loggedInUser;
	private readonly IApiHelper _apiHelper;
	private readonly IAbstractFactory<CalculationTypesIndexControl> _calculationTypesIndexFactory;
	private readonly IAbstractFactory<CalculationTypesDetailsControl> _calculationTypesDetailsFactory;
	private readonly IAbstractFactory<CalculationTypesCreateControl> _calculationTypesCreateFactory;
	private readonly IAbstractFactory<CalculationTypesEditControl> _calculationTypesEditFactory;
	private readonly IAbstractFactory<CalculationTypesDeleteControl> _calculationTypesDeleteFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="CalculationTypesControl"/> class.
	/// </summary>
	/// <param name="logger">Logger instance for tracing control events.</param>
	/// <param name="loggedInUser">Model representing the currently logged-in user.</param>
	/// <param name="apiHelper">Helper for making API requests.</param>
	/// <param name="calculationTypesIndexFactory">Factory for creating index controls.</param>
	/// <param name="calculationTypesDetailsFactory">Factory for creating details controls.</param>
	/// <param name="calculationTypesCreateFactory">Factory for creating create controls.</param>
	/// <param name="calculationTypesEditFactory">Factory for creating edit controls.</param>
	/// <param name="calculationTypesDeleteFactory">Factory for creating delete controls.</param>
	/// <remarks>
	/// This constructor initializes the control and immediately loads the Index view as the default view.
	/// All dependencies are injected through the constructor to support testability and loose coupling.
	/// </remarks>
	public CalculationTypesControl(ILogger<CalculationTypesControl> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, IAbstractFactory<CalculationTypesIndexControl> calculationTypesIndexFactory, IAbstractFactory<CalculationTypesDetailsControl> calculationTypesDetailsFactory, IAbstractFactory<CalculationTypesCreateControl> calculationTypesCreateFactory, IAbstractFactory<CalculationTypesEditControl> calculationTypesEditFactory, IAbstractFactory<CalculationTypesDeleteControl> calculationTypesDeleteFactory)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_calculationTypesIndexFactory = calculationTypesIndexFactory;
		_calculationTypesDetailsFactory = calculationTypesDetailsFactory;
		_calculationTypesCreateFactory = calculationTypesCreateFactory;
		_calculationTypesEditFactory = calculationTypesEditFactory;
		_calculationTypesDeleteFactory = calculationTypesDeleteFactory;

		InitializeComponent();

		CalculationTypesIndexControl indexControl = _calculationTypesIndexFactory.Create();
		indexControl.ChildControlEvent += CalculationTypes_Index_ChildControlEvent;
		CalculationTypesContent.Content = indexControl;
	}

	private void CalculationTypes_Index_ChildControlEvent(object? sender, ChildControlEventArgs<CalculationTypesIndexControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesControl), nameof(CalculationTypes_Index_ChildControlEvent), sender, e);
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
	}

	private void CalculationTypes_Create_ChildControlEvent(object? sender, ChildControlEventArgs<CalculationTypesCreateControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesControl), nameof(CalculationTypes_Create_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "create":
			case "index":
				CalculationTypesIndexControl indexControl = _calculationTypesIndexFactory.Create();
				indexControl.ChildControlEvent += CalculationTypes_Index_ChildControlEvent;
				CalculationTypesContent.Content = indexControl;
				break;
			default:
				break;
		}
	}

	private void CalculationTypes_Edit_ChildControlEvent(object? sender, ChildControlEventArgs<CalculationTypesEditControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesControl), nameof(CalculationTypes_Edit_ChildControlEvent), sender, e);
		}

		switch (e.Action)
		{
			case "save":
			case "index":
				CalculationTypesIndexControl indexControl = _calculationTypesIndexFactory.Create();
				indexControl.ChildControlEvent += CalculationTypes_Index_ChildControlEvent;
				CalculationTypesContent.Content = indexControl;
				break;
			default:
				break;
		}
	}

	private void CalculationTypes_Details_ChildControlEvent(object? sender, ChildControlEventArgs<CalculationTypesDetailsControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesControl), nameof(CalculationTypes_Details_ChildControlEvent), sender, e);
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
	}
	private void CalculationTypes_Delete_ChildControlEvent(object? sender, ChildControlEventArgs<CalculationTypesDeleteControl> e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(CalculationTypesControl), nameof(CalculationTypes_Delete_ChildControlEvent), sender, e);
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
	}
}
