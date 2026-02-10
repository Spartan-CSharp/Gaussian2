using System.Diagnostics;
using System.Globalization;
using System.Text;

using GaussianCommonLibrary.Models;

using GaussianMVC.Models;

using GaussianMVCLibrary.DataAccess;

using Microsoft.AspNetCore.Mvc;

namespace GaussianMVC.Controllers;

/// <summary>
/// MVC controller for managing Base Methods.
/// Provides CRUD operations for Base Method entities through a web interface.
/// </summary>
/// <param name="logger">The logger instance for logging controller operations.</param>
/// <param name="bmCrud">The data access service for Base Methods CRUD operations.</param>
/// <param name="mfCrud">The data access service for Method Families CRUD operations.</param>
public class BaseMethodsController(ILogger<BaseMethodsController> logger, IBaseMethodsCrud bmCrud, IMethodFamiliesCrud mfCrud) : Controller
{
	private readonly ILogger<BaseMethodsController> _logger = logger;
	private readonly IBaseMethodsCrud _bmCrud = bmCrud;
	private readonly IMethodFamiliesCrud _mfCrud = mfCrud;

	/// <summary>
	/// Displays a list of all Base Methods.
	/// </summary>
	/// <returns>A view containing a list of <see cref="BaseMethodViewModel"/> objects representing all Base Methods.</returns>
	// GET: BaseMethods
	[HttpGet]
	public async Task<ActionResult<List<BaseMethodViewModel>>> IndexAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", nameof(IndexAsync), nameof(BaseMethodsController), nameof(IndexAsync));
			}

			List<BaseMethodSimpleModel> baseMethods = await _bmCrud.GetAllSimpleBaseMethodsAsync().ConfigureAwait(false);
			List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
			List<BaseMethodViewModel> modelList = [];

			foreach (BaseMethodSimpleModel model in baseMethods)
			{
				BaseMethodViewModel viewModel = new(model, methodFamilies);
				modelList.Add(viewModel);
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(IndexAsync), modelList.Count, nameof(BaseMethodViewModel));
			}

			return View(modelList);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(IndexAsync));
			}

			Response.StatusCode = StatusCodes.Status500InternalServerError;

			ErrorViewModel evm = new()
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				StatusCode = StatusCodes.Status500InternalServerError,
				StatusPhrase = "Internal Server Error",
				ExceptionType = ex.GetType().Name,
				ExceptionMessage = ex.Message
			};

			return View("Error", evm);
		}
	}

	/// <summary>
	/// Displays detailed information for a specific Base Method.
	/// </summary>
	/// <param name="id">The unique identifier of the Base Method to display.</param>
	/// <returns>A view containing a <see cref="BaseMethodViewModel"/> with the details of the specified Base Method.</returns>
	// GET: BaseMethods/Details/5
	[HttpGet]
	public async Task<ActionResult<BaseMethodViewModel?>> DetailsAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DetailsAsync), id);
			}

			BaseMethodFullModel? baseMethod = await _bmCrud.GetBaseMethodByIdAsync(id).ConfigureAwait(false);
			List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
			BaseMethodViewModel? model = baseMethod is null ? null : new(baseMethod, methodFamilies);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DetailsAsync), id, nameof(BaseMethodViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DetailsAsync), id);
			}

			Response.StatusCode = StatusCodes.Status500InternalServerError;

			ErrorViewModel evm = new()
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				StatusCode = StatusCodes.Status500InternalServerError,
				StatusPhrase = "Internal Server Error",
				ExceptionType = ex.GetType().Name,
				ExceptionMessage = ex.Message
			};

			return View("Error", evm);
		}
	}

	/// <summary>
	/// Displays the form for creating a new Base Method.
	/// </summary>
	/// <returns>A view containing an empty <see cref="BaseMethodViewModel"/> for creating a new Base Method.</returns>
	// GET: BaseMethods/Create
	[HttpGet]
	public async Task<ActionResult<BaseMethodViewModel>> CreateAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(CreateAsync));
			}

			List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
			BaseMethodViewModel model = new(methodFamilies);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(CreateAsync), nameof(BaseMethodViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(CreateAsync));
			}

			Response.StatusCode = StatusCodes.Status500InternalServerError;

			ErrorViewModel evm = new()
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				StatusCode = StatusCodes.Status500InternalServerError,
				StatusPhrase = "Internal Server Error",
				ExceptionType = ex.GetType().Name,
				ExceptionMessage = ex.Message
			};

			return View("Error", evm);
		}
	}

	/// <summary>
	/// Processes the form submission for creating a new Base Method.
	/// </summary>
	/// <param name="model">The <see cref="BaseMethodViewModel"/> containing the data for the new Base Method.</param>
	/// <returns>A redirect to the Index action if successful; otherwise, returns the Create view with validation errors.</returns>
	// POST: BaseMethods/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> CreateAsync(BaseMethodViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(CreateAsync), nameof(BaseMethodViewModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid)
			{
				_ = await _bmCrud.CreateNewBaseMethodAsync(model.ToSimpleModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning RedirectToAction({ActionName}).", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(CreateAsync), nameof(IndexAsync));
				}

				Response.StatusCode = StatusCodes.Status201Created;
				return View("Details", model);
			}
			else
			{
				Dictionary<string, List<string>> modelValidationErrors = ModelState.Where(kvp => kvp.Value?.Errors.Count > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList());
				StringBuilder sb = new();

				foreach (KeyValuePair<string, List<string>> validationErrors in modelValidationErrors)
				{
					_ = sb.AppendLine(validationErrors.Key);

					foreach (string validationError in validationErrors.Value)
					{
						_ = sb.AppendLine(CultureInfo.InvariantCulture, $"\t{validationError}");
					}
				}

				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(CreateAsync), nameof(BaseMethodViewModel), model, sb.ToString());
				}

				Response.StatusCode = StatusCodes.Status400BadRequest;
				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(CreateAsync), nameof(BaseMethodViewModel), model);
			}

			Response.StatusCode = StatusCodes.Status500InternalServerError;

			ErrorViewModel evm = new()
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				StatusCode = StatusCodes.Status500InternalServerError,
				StatusPhrase = "Internal Server Error",
				ExceptionType = ex.GetType().Name,
				ExceptionMessage = ex.Message
			};

			return View("Error", evm);
		}
	}

	/// <summary>
	/// Displays the form for editing an existing Base Method.
	/// </summary>
	/// <param name="id">The unique identifier of the Base Method to edit.</param>
	/// <returns>A view containing a <see cref="BaseMethodViewModel"/> with the current data of the Base Method.</returns>
	// GET: BaseMethods/Edit/5
	[HttpGet]
	public async Task<ActionResult<BaseMethodViewModel?>> EditAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(EditAsync), id);
			}

			BaseMethodFullModel? baseMethod = await _bmCrud.GetBaseMethodByIdAsync(id).ConfigureAwait(false);
			List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
			BaseMethodViewModel? model = baseMethod is null ? null : new(baseMethod, methodFamilies);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(EditAsync), id, nameof(BaseMethodViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(EditAsync), id);
			}

			Response.StatusCode = StatusCodes.Status500InternalServerError;

			ErrorViewModel evm = new()
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				StatusCode = StatusCodes.Status500InternalServerError,
				StatusPhrase = "Internal Server Error",
				ExceptionType = ex.GetType().Name,
				ExceptionMessage = ex.Message
			};

			return View("Error", evm);
		}
	}

	/// <summary>
	/// Processes the form submission for editing an existing Base Method.
	/// </summary>
	/// <param name="id">The unique identifier of the Base Method to update.</param>
	/// <param name="model">The <see cref="BaseMethodViewModel"/> containing the updated data.</param>
	/// <returns>A redirect to the Index action if successful; otherwise, returns the Edit view with validation errors.</returns>
	// POST: BaseMethods/Edit/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult<BaseMethodViewModel>> EditAsync(int id, BaseMethodViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(EditAsync), id, nameof(BaseMethodViewModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid && id == model.Id)
			{
				BaseMethodSimpleModel baseMethod = model.ToSimpleModel();
				BaseMethodFullModel updatedBaseMethod = await _bmCrud.UpdateBaseMethodAsync(baseMethod).ConfigureAwait(false);
				List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
				model = new BaseMethodViewModel(updatedBaseMethod, methodFamilies);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(EditAsync), id, nameof(BaseMethodViewModel), model);
				}

				return View("Details", model);
			}
			else
			{
				if (id != model.Id)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(EditAsync), id, nameof(BaseMethodViewModel), model);
					}
				}
				else
				{
					Dictionary<string, List<string>> modelValidationErrors = ModelState.Where(kvp => kvp.Value?.Errors.Count > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList());
					StringBuilder sb = new();

					foreach (KeyValuePair<string, List<string>> validationErrors in modelValidationErrors)
					{
						sb.AppendLine(validationErrors.Key);
						foreach (string validationError in validationErrors.Value)
						{
							sb.AppendLine(CultureInfo.InvariantCulture, $"\t{validationError}");
						}
					}

					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(EditAsync), nameof(BaseMethodViewModel), model, sb.ToString());
					}
				}

				Response.StatusCode = StatusCodes.Status400BadRequest;
				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(EditAsync), id, nameof(BaseMethodViewModel), model);
			}

			Response.StatusCode = StatusCodes.Status500InternalServerError;

			ErrorViewModel evm = new()
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				StatusCode = StatusCodes.Status500InternalServerError,
				StatusPhrase = "Internal Server Error",
				ExceptionType = ex.GetType().Name,
				ExceptionMessage = ex.Message
			};

			return View("Error", evm);
		}
	}

	/// <summary>
	/// Displays the confirmation page for deleting a Base Method.
	/// </summary>
	/// <param name="id">The unique identifier of the Base Method to delete.</param>
	/// <returns>A view containing a <see cref="BaseMethodViewModel"/> with the details of the Base Method to be deleted.</returns>
	// GET: BaseMethods/Delete/5
	[HttpGet]
	public async Task<ActionResult<BaseMethodViewModel?>> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DeleteAsync), id);
			}

			BaseMethodFullModel? baseMethod = await _bmCrud.GetBaseMethodByIdAsync(id).ConfigureAwait(false);
			List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
			BaseMethodViewModel? model = baseMethod is null ? null : new(baseMethod, methodFamilies);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DeleteAsync), id, nameof(BaseMethodViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DeleteAsync), id);
			}

			Response.StatusCode = StatusCodes.Status500InternalServerError;

			ErrorViewModel evm = new()
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				StatusCode = StatusCodes.Status500InternalServerError,
				StatusPhrase = "Internal Server Error",
				ExceptionType = ex.GetType().Name,
				ExceptionMessage = ex.Message
			};

			return View("Error", evm);
		}
	}

	/// <summary>
	/// Processes the deletion confirmation for a Base Method.
	/// </summary>
	/// <param name="id">The unique identifier of the Base Method to delete.</param>
	/// <param name="model">The <see cref="BaseMethodViewModel"/> containing the Base Method data for validation.</param>
	/// <returns>A redirect to the Index action if successful; otherwise, returns the Delete view if the ID does not match the model.</returns>
	// POST: BaseMethods/Delete/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> DeleteAsync(int id, BaseMethodViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DeleteAsync), id);
			}

			if (id == model?.Id)
			{
				await _bmCrud.DeleteBaseMethodAsync(id).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DeleteAsync), id);
				}

				return RedirectToAction(nameof(Index));
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DeleteAsync), id, nameof(BaseMethodViewModel), model);
				}

				Response.StatusCode = StatusCodes.Status400BadRequest;
				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DeleteAsync), id);
			}

			Response.StatusCode = StatusCodes.Status500InternalServerError;

			ErrorViewModel evm = new()
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				StatusCode = StatusCodes.Status500InternalServerError,
				StatusPhrase = "Internal Server Error",
				ExceptionType = ex.GetType().Name,
				ExceptionMessage = ex.Message
			};

			return View("Error", evm);
		}
	}
}
