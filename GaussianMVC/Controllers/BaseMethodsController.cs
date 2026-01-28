using GaussianCommonLibrary.Models;

using GaussianMVC.Models;

using GaussianMVCLibrary.DataAccess;

using Microsoft.AspNetCore.Mvc;

namespace GaussianMVC.Controllers;

/// <summary>
/// Controller for managing base methods in the Gaussian MVC application.
/// Provides CRUD (Create, Read, Update, Delete) operations for base methods with integrated logging and error handling.
/// </summary>
/// <param name="logger">Logger instance for tracking controller actions and errors.</param>
/// <param name="bmCrud">Data access service for base methods CRUD operations.</param>
/// <param name="mfCrud">Data access service for method families CRUD operations.</param>
public class BaseMethodsController(ILogger<BaseMethodsController> logger, IBaseMethodsCrud bmCrud, IMethodFamiliesCrud mfCrud) : Controller
{
	private readonly ILogger<BaseMethodsController> _logger = logger;
	private readonly IBaseMethodsCrud _bmCrud = bmCrud;
	private readonly IMethodFamiliesCrud _mfCrud = mfCrud;

	/// <summary>
	/// Displays a list of all base methods with their associated method families.
	/// </summary>
	/// <returns>
	/// A view containing a list of <see cref="BaseMethodViewModel"/> objects, 
	/// or redirects to the error page if an exception occurs.
	/// </returns>
	/// <remarks>
	/// This method retrieves all base methods and their corresponding method families,
	/// combines them into view models, and returns them to the Index view.
	/// Logs debug information on entry and trace information with the model count on success.
	/// </remarks>
	// GET: BaseMethods
	[HttpGet]
	public async Task<ActionResult<List<BaseMethodViewModel>>> IndexAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action}", nameof(IndexAsync), nameof(BaseMethodsController), nameof(IndexAsync));
			}

			List<BaseMethodSimpleModel> baseMethods = await _bmCrud.GetAllSimpleBaseMethodsAsync().ConfigureAwait(false);
			List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
			List<BaseMethodViewModel> modelList = [];

			foreach (BaseMethodSimpleModel model in baseMethods)
			{
				MethodFamilyRecord methodFamily = methodFamilies.Where(x => x.Id == model.MethodFamilyId).First();
				BaseMethodViewModel viewModel = new(model, methodFamily, methodFamilies);
				modelList.Add(viewModel);
			}

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} {Controller} {Action} {ModelCount}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(IndexAsync), modelList.Count);
			}

			return View(modelList);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(IndexAsync));
			}

			return RedirectToAction(nameof(HomeController.Error), "Home");
		}
	}

	/// <summary>
	/// Displays detailed information about a specific base method.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to display.</param>
	/// <returns>
	/// A view containing a <see cref="BaseMethodViewModel"/> with the base method details,
	/// or redirects to the error page if an exception occurs.
	/// </returns>
	/// <remarks>
	/// Retrieves a base method by its ID and displays it in the Details view.
	/// Returns null if the base method is not found.
	/// Logs debug information on entry and trace information with the result on success.
	/// </remarks>
	// GET: BaseMethods/Details/5
	[HttpGet]
	public async Task<ActionResult<BaseMethodViewModel?>> DetailsAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DetailsAsync), id);
			}

			BaseMethodFullModel? baseMethod = await _bmCrud.GetBaseMethodByIdAsync(id).ConfigureAwait(false);
			List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
			BaseMethodViewModel? model = baseMethod is null ? null : new(baseMethod, methodFamilies);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} {Controller} {Action} {Id} {Model}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DetailsAsync), id, model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DetailsAsync), id);
			}

			return RedirectToAction(nameof(HomeController.Error), "Home");
		}
	}

	/// <summary>
	/// Displays the form for creating a new base method.
	/// </summary>
	/// <returns>
	/// A view with an empty form for creating a new base method,
	/// or redirects to the error page if an exception occurs.
	/// </returns>
	/// <remarks>
	/// This GET action renders the creation form.
	/// Logs debug information on entry.
	/// </remarks>
	// GET: BaseMethods/Create
	[HttpGet]
	public async Task<ActionResult> CreateAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(CreateAsync));
			}

			List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
			BaseMethodViewModel model = new(methodFamilies);
			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(CreateAsync));
			}

			return RedirectToAction(nameof(HomeController.Error), "Home");
		}
	}

	/// <summary>
	/// Processes the form submission for creating a new base method.
	/// </summary>
	/// <param name="model">The view model containing the data for the new base method.</param>
	/// <returns>
	/// Redirects to the Index action on successful creation,
	/// returns the view with validation errors if the model is invalid,
	/// or redirects to the error page if an exception occurs.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	/// <remarks>
	/// Validates the model state before creating the base method.
	/// Logs debug information on entry, trace information on success, and warning information for invalid models.
	/// </remarks>
	// POST: BaseMethods/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> CreateAsync(BaseMethodViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Model}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(CreateAsync), model);
			}

			if (model is null)
			{
				throw new ArgumentNullException(nameof(model), $"The parameter {nameof(model)} cannot be null.");
			}

			if (ModelState.IsValid)
			{
				BaseMethodSimpleModel simpleBaseMethod = model.ToSimpleModel();
				BaseMethodFullModel fullBaseMethod = await _bmCrud.CreateNewBaseMethodAsync(simpleBaseMethod).ConfigureAwait(false);
				List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
				model = new BaseMethodViewModel(fullBaseMethod, methodFamilies);

				if (_logger.IsEnabled(LogLevel.Trace))
				{
					_logger.LogTrace("{Method} {Controller} {Action} {Model}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(CreateAsync), model);
				}

				return RedirectToAction(nameof(Index));
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} {Model} has an invalid model", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(CreateAsync), model);
				}

				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Model} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(CreateAsync), model);
			}

			return RedirectToAction(nameof(HomeController.Error), "Home");
		}
	}

	/// <summary>
	/// Displays the form for editing an existing base method.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to edit.</param>
	/// <returns>
	/// A view containing a <see cref="BaseMethodViewModel"/> with the base method data to edit,
	/// or redirects to the error page if an exception occurs.
	/// </returns>
	/// <remarks>
	/// Retrieves the base method by ID and displays it in the Edit view.
	/// Returns null if the base method is not found.
	/// Logs debug information on entry and trace information with the result on success.
	/// </remarks>
	// GET: BaseMethods/Edit/5
	[HttpGet]
	public async Task<ActionResult<BaseMethodViewModel>> EditAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(EditAsync), id);
			}

			BaseMethodFullModel? baseMethod = await _bmCrud.GetBaseMethodByIdAsync(id).ConfigureAwait(false);
			List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
			BaseMethodViewModel? model = baseMethod is null ? null : new(baseMethod, methodFamilies);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} {Controller} {Action} {Id} {Model}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(EditAsync), id, model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(EditAsync), id);
			}

			return RedirectToAction(nameof(HomeController.Error), "Home");
		}
	}

	/// <summary>
	/// Processes the form submission for updating an existing base method.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to update.</param>
	/// <param name="model">The view model containing the updated data for the base method.</param>
	/// <returns>
	/// Redirects to the Index action on successful update,
	/// returns the view with validation errors if the model is invalid or IDs don't match,
	/// or redirects to the error page if an exception occurs.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	/// <remarks>
	/// Validates that the model state is valid and the ID parameter matches the model's ID before updating.
	/// Logs debug information on entry, trace information on success, and warning information for invalid models.
	/// </remarks>
	// POST: BaseMethods/Edit/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> EditAsync(int id, BaseMethodViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} {Model}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(EditAsync), id, model);
			}

			if (model is null)
			{
				throw new ArgumentNullException(nameof(model), $"The parameter {nameof(model)} cannot be null.");
			}

			if (ModelState.IsValid && id == model.Id)
			{
				BaseMethodSimpleModel simpleBaseMethod = model.ToSimpleModel();
				BaseMethodFullModel fullBaseMethod = await _bmCrud.UpdateBaseMethodAsync(simpleBaseMethod).ConfigureAwait(false);
				List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
				model = new BaseMethodViewModel(fullBaseMethod, methodFamilies);

				if (_logger.IsEnabled(LogLevel.Trace))
				{
					_logger.LogTrace("{Method} {Controller} {Action} {Id} {Model}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(EditAsync), id, model);
				}

				return RedirectToAction(nameof(Index));
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} {Id} {Model} has an invalid model", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(EditAsync), id, model);
				}

				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} {Model} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(EditAsync), id, model);
			}

			return RedirectToAction(nameof(HomeController.Error), "Home");
		}
	}

	/// <summary>
	/// Displays the confirmation page for deleting a base method.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to delete.</param>
	/// <returns>
	/// A view containing a <see cref="BaseMethodViewModel"/> with the base method details for deletion confirmation,
	/// or redirects to the error page if an exception occurs.
	/// </returns>
	/// <remarks>
	/// Retrieves the base method by ID and displays it in the Delete confirmation view.
	/// Returns null if the base method is not found.
	/// Logs debug information on entry and trace information with the result on success.
	/// </remarks>
	// GET: BaseMethods/Delete/5
	[HttpGet]
	public async Task<ActionResult<BaseMethodViewModel>> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DeleteAsync), id);
			}

			BaseMethodFullModel? baseMethod = await _bmCrud.GetBaseMethodByIdAsync(id).ConfigureAwait(false);
			List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
			BaseMethodViewModel? model = baseMethod is null ? null : new(baseMethod, methodFamilies);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} {Controller} {Action} {Id} {Model}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DeleteAsync), id, model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DeleteAsync), id);
			}

			return RedirectToAction(nameof(HomeController.Error), "Home");
		}
	}

	/// <summary>
	/// Processes the deletion of a base method after user confirmation.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to delete.</param>
	/// <param name="model">The view model containing the base method data for verification.</param>
	/// <returns>
	/// Redirects to the Index action on successful deletion,
	/// returns the view if the IDs don't match for security verification,
	/// or redirects to the error page if an exception occurs.
	/// </returns>
	/// <remarks>
	/// Validates that the ID parameter matches the model's ID before deleting to prevent unauthorized deletions.
	/// Logs debug information on entry and warning information for invalid models.
	/// </remarks>
	// POST: BaseMethods/Delete/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> DeleteAsync(int id, BaseMethodViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} {Model}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DeleteAsync), id, model);
			}

			if (id == model?.Id)
			{
				await _bmCrud.DeleteBaseMethodAsync(id).ConfigureAwait(false);
				return RedirectToAction(nameof(Index));
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} {Id} {Model} has an invalid model", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DeleteAsync), id, model);
				}

				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} {Model} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DeleteAsync), id, model);
			}

			return RedirectToAction(nameof(HomeController.Error), "Home");
		}
	}
}
