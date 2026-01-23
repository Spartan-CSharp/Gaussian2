using GaussianCommonLibrary.Models;

using GaussianMVC.Models;

using GaussianMVCLibrary.DataAccess;

using Microsoft.AspNetCore.Mvc;

namespace GaussianMVC.Controllers;

/// <summary>
/// MVC controller for managing Calculation Types.
/// Provides CRUD operations for CalculationType entities through a web interface.
/// </summary>
/// <param name="logger">The logger instance for logging controller operations.</param>
/// <param name="crud">The data access service for CalculationType CRUD operations.</param>
public class CalculationTypesController(ILogger<CalculationTypesController> logger, ICalculationTypesCrud crud) : Controller
{
	private readonly ILogger<CalculationTypesController> _logger = logger;
	private readonly ICalculationTypesCrud _crud = crud;

	/// <summary>
	/// Displays a list of all Calculation Types.
	/// </summary>
	/// <returns>
	/// A view containing a list of <see cref="CalculationTypeViewModel"/> objects representing all Calculation Types.
	/// </returns>
	// GET: CalculationTypes
	[HttpGet]
	public async Task<ActionResult<List<CalculationTypeViewModel>>> IndexAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(IndexAsync));
			}

			List<CalculationTypeFullModel> calculationTypes = await _crud.GetAllCalculationTypesAsync().ConfigureAwait(false);
			List<CalculationTypeViewModel> modelList = [];

			foreach (CalculationTypeFullModel model in calculationTypes)
			{
				CalculationTypeViewModel viewModel = new(model);
				modelList.Add(viewModel);
			}

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} {Controller} {Action} {ModelCount}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(IndexAsync), modelList.Count);
			}

			return View(modelList);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(IndexAsync));
			}

			return RedirectToAction(nameof(HomeController.Error), "Home");
		}
	}

	/// <summary>
	/// Displays detailed information for a specific Calculation Type.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to display.</param>
	/// <returns>
	/// A view containing a <see cref="CalculationTypeViewModel"/> with the details of the specified Calculation Type.
	/// </returns>
	// GET: CalculationTypes/Details/5
	[HttpGet]
	public async Task<ActionResult<CalculationTypeViewModel?>> DetailsAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DetailsAsync), id);
			}

			CalculationTypeFullModel? calculationType = await _crud.GetCalculationTypeByIdAsync(id).ConfigureAwait(false);

			CalculationTypeViewModel? model = calculationType is null ? null : new(calculationType);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} {Controller} {Action} {Id} {Model}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DetailsAsync), id, model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DetailsAsync), id);
			}

			return RedirectToAction(nameof(HomeController.Error), "Home");
		}
	}

	/// <summary>
	/// Displays the form for creating a new Calculation Type.
	/// </summary>
	/// <returns>A view with an empty form for creating a new Calculation Type.</returns>
	// GET: CalculationTypes/Create
	[HttpGet]
	public async Task<ActionResult> CreateAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(CreateAsync));
			}

			return View();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(CreateAsync));
			}

			return RedirectToAction(nameof(HomeController.Error), "Home");
		}
	}

	/// <summary>
	/// Processes the creation of a new Calculation Type.
	/// </summary>
	/// <param name="model">The view model containing the data for the new Calculation Type.</param>
	/// <returns>
	/// Redirects to the Index action if successful; otherwise, returns the view with validation errors.
	/// On exception, redirects to the Error page.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	// POST: CalculationTypes/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> CreateAsync(CalculationTypeViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Model}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(CreateAsync), model);
			}

			if (model is null)
			{
				throw new ArgumentNullException(nameof(model), $"The parameter {nameof(model)} cannot be null.");
			}

			if (ModelState.IsValid)
			{
				CalculationTypeFullModel calculationType = model.ToFullModel();
				calculationType = await _crud.CreateNewCalculationTypeAsync(calculationType).ConfigureAwait(false);
				model = new CalculationTypeViewModel(calculationType);

				if (_logger.IsEnabled(LogLevel.Trace))
				{
					_logger.LogTrace("{Method} {Controller} {Action} {Model}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(CreateAsync), model);
				}

				return RedirectToAction(nameof(Index));
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} {Model} has an invalid model", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(CreateAsync), model);
				}

				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Model} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(CreateAsync), model);
			}

			return RedirectToAction(nameof(HomeController.Error), "Home");
		}
	}

	/// <summary>
	/// Displays the form for editing an existing Calculation Type.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to edit.</param>
	/// <returns>
	/// A view containing a <see cref="CalculationTypeViewModel"/> populated with the existing data.
	/// </returns>
	// GET: CalculationTypes/Edit/5
	[HttpGet]
	public async Task<ActionResult<CalculationTypeViewModel>> EditAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(EditAsync), id);
			}

			CalculationTypeFullModel? calculationType = await _crud.GetCalculationTypeByIdAsync(id).ConfigureAwait(false);

			CalculationTypeViewModel? model = calculationType is null ? null : new(calculationType);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} {Controller} {Action} {Id} {Model}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(EditAsync), id, model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(EditAsync), id);
			}

			return RedirectToAction(nameof(HomeController.Error), "Home");
		}
	}

	/// <summary>
	/// Processes the update of an existing Calculation Type.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type being edited.</param>
	/// <param name="model">The view model containing the updated data.</param>
	/// <returns>
	/// Redirects to the Index action if successful; otherwise, returns the view with validation errors.
	/// On exception, redirects to the Error page.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	/// <remarks>
	/// Validates that the model state is valid before processing the update.
	/// Logs warnings for invalid models and errors for exceptions.
	/// </remarks>
	// POST: CalculationTypes/Edit/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> EditAsync(int id, CalculationTypeViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} {Model}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(EditAsync), id, model);
			}

			if (model is null)
			{
				throw new ArgumentNullException(nameof(model), $"The parameter {nameof(model)} cannot be null.");
			}

			if (ModelState.IsValid && id == model.Id)
			{
				CalculationTypeFullModel calculationType = model.ToFullModel();
				calculationType = await _crud.CreateNewCalculationTypeAsync(calculationType).ConfigureAwait(false);
				model = new CalculationTypeViewModel(calculationType);

				if (_logger.IsEnabled(LogLevel.Trace))
				{
					_logger.LogTrace("{Method} {Controller} {Action} {Id} {Model}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(EditAsync), id, model);
				}

				return RedirectToAction(nameof(Index));
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} {Id} {Model} has an invalid model", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(EditAsync), id, model);
				}

				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} {Model} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(EditAsync), id, model);
			}

			return RedirectToAction(nameof(HomeController.Error), "Home");
		}
	}

	/// <summary>
	/// Displays the confirmation page for deleting a Calculation Type.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to delete.</param>
	/// <returns>
	/// A view containing a <see cref="CalculationTypeViewModel"/> with the details of the Calculation Type to be deleted.
	/// </returns>
	// GET: CalculationTypes/Delete/5
	[HttpGet]
	public async Task<ActionResult<CalculationTypeViewModel>> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DeleteAsync), id);
			}

			CalculationTypeFullModel? calculationType = await _crud.GetCalculationTypeByIdAsync(id).ConfigureAwait(false);

			CalculationTypeViewModel? model = calculationType is null ? null : new(calculationType);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} {Controller} {Action} {Id} {Model}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DeleteAsync), id, model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DeleteAsync), id);
			}

			return RedirectToAction(nameof(HomeController.Error), "Home");
		}
	}

	/// <summary>
	/// Processes the deletion of a Calculation Type.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to delete.</param>
	/// <param name="model">The view model containing the data of the Calculation Type being deleted.</param>
	/// <returns>
	/// Redirects to the Index action if successful; otherwise, returns the view with an error message.
	/// On exception, redirects to the Error page.
	/// </returns>
	/// <remarks>
	/// Validates that the ID from the route matches the ID in the model before proceeding with deletion.
	/// Logs warnings for ID mismatches and errors for exceptions.
	/// </remarks>
	// POST: CalculationTypes/Delete/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> DeleteAsync(int id, CalculationTypeViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id}, {Model}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DeleteAsync), id, model);
			}

			if (id == model?.Id)
			{
				await _crud.DeleteCalculationTypeAsync(id).ConfigureAwait(false);
				return RedirectToAction(nameof(Index));
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} {Id} {Model} has an invalid model", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DeleteAsync), id, model);
				}

				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} {Model} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DeleteAsync), id, model);
			}

			return RedirectToAction(nameof(HomeController.Error), "Home");
		}
	}
}
