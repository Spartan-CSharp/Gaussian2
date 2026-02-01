using System.Diagnostics;
using System.Globalization;
using System.Text;

using GaussianCommonLibrary.Models;

using GaussianMVC.Models;

using GaussianMVCLibrary.DataAccess;

using Microsoft.AspNetCore.Mvc;

namespace GaussianMVC.Controllers;

/// <summary>
/// MVC controller for managing Calculation Types.
/// Provides CRUD operations for Calculation Type entities through a web interface.
/// </summary>
/// <param name="logger">The logger instance for logging controller operations.</param>
/// <param name="crud">The data access service for Calculation Type CRUD operations.</param>
public class CalculationTypesController(ILogger<CalculationTypesController> logger, ICalculationTypesCrud crud) : Controller
{
	private readonly ILogger<CalculationTypesController> _logger = logger;
	private readonly ICalculationTypesCrud _crud = crud;

	/// <summary>
	/// Displays a list of all Calculation Types.
	/// </summary>
	/// <returns>A view containing a list of <see cref="CalculationTypeViewModel"/> objects representing all Calculation Types.</returns>
	// GET: CalculationTypes
	[HttpGet]
	public async Task<ActionResult<List<CalculationTypeViewModel>>> IndexAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(IndexAsync));
			}

			List<CalculationTypeFullModel> calculationTypes = await _crud.GetAllCalculationTypesAsync().ConfigureAwait(false);
			List<CalculationTypeViewModel> modelList = [];

			foreach (CalculationTypeFullModel model in calculationTypes)
			{
				CalculationTypeViewModel viewModel = new(model);
				modelList.Add(viewModel);
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(IndexAsync), modelList.Count, nameof(CalculationTypeViewModel));
			}

			return View(modelList);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(IndexAsync));
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
	/// Displays detailed information for a specific Calculation Type.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to display.</param>
	/// <returns>A view containing a <see cref="CalculationTypeViewModel"/> with the details of the specified Calculation Type.</returns>
	// GET: CalculationTypes/Details/5
	[HttpGet]
	public async Task<ActionResult<CalculationTypeViewModel?>> DetailsAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DetailsAsync), id);
			}

			CalculationTypeFullModel? calculationType = await _crud.GetCalculationTypeByIdAsync(id).ConfigureAwait(false);
			CalculationTypeViewModel? model = calculationType is null ? null : new(calculationType);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DetailsAsync), id, nameof(CalculationTypeViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DetailsAsync), id);
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
	/// Displays the form for creating a new Calculation Type.
	/// </summary>
	/// <returns>A view with an empty form for creating a new Calculation Type.</returns>
	// GET: CalculationTypes/Create
	[HttpGet]
	public async Task <ActionResult<CalculationTypeViewModel>> CreateAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(CreateAsync));
			}

			CalculationTypeViewModel model = new();

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(CreateAsync), nameof(CalculationTypeViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(CreateAsync));
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
	/// Processes the form submission for creating a new Calculation Type.
	/// </summary>
	/// <param name="model">The <see cref="CalculationTypeViewModel"/> containing the data for the new Calculation Type.</param>
	/// <returns>A redirect to the Index action if successful; otherwise, returns the Create view with validation errors.</returns>
	// POST: CalculationTypes/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult<CalculationTypeViewModel>> CreateAsync(CalculationTypeViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(CreateAsync), nameof(CalculationTypeViewModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid)
			{
				_ = await _crud.CreateNewCalculationTypeAsync(model.ToFullModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning RedirectToAction({ActionName}).", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(CreateAsync), nameof(IndexAsync));
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
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(CreateAsync), nameof(CalculationTypeViewModel), model, sb.ToString());
				}

				Response.StatusCode = StatusCodes.Status400BadRequest;
				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(CreateAsync), nameof(CalculationTypeViewModel), model);
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
	/// Displays the form for editing an existing Calculation Type.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to edit.</param>
	/// <returns>A view containing a <see cref="CalculationTypeViewModel"/> with the current data of the Calculation Type.</returns>
	// GET: CalculationTypes/Edit/5
	[HttpGet]
	public async Task<ActionResult<CalculationTypeViewModel?>> EditAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(EditAsync), id);
			}

			CalculationTypeFullModel? calculationType = await _crud.GetCalculationTypeByIdAsync(id).ConfigureAwait(false);
			CalculationTypeViewModel? model = calculationType is null ? null : new(calculationType);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(EditAsync), id, nameof(CalculationTypeViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(EditAsync), id);
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
	/// Processes the form submission for editing an existing Calculation Type.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to update.</param>
	/// <param name="model">The <see cref="CalculationTypeViewModel"/> containing the updated data.</param>
	/// <returns>A redirect to the Index action if successful; otherwise, returns the Edit view with validation errors.</returns>
	// POST: CalculationTypes/Edit/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult<CalculationTypeViewModel>> EditAsync(int id, CalculationTypeViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(EditAsync), id, nameof(CalculationTypeViewModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid && id == model.Id)
			{
				CalculationTypeFullModel calculationType = model.ToFullModel();
				calculationType = await _crud.UpdateCalculationTypeAsync(calculationType).ConfigureAwait(false);
				model = new CalculationTypeViewModel(calculationType);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(EditAsync), id, nameof(CalculationTypeViewModel), model);
				}

				return View("Details", model);
			}
			else
			{
				if (id != model.Id)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(EditAsync), id, nameof(CalculationTypeViewModel), model);
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
						_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(EditAsync), nameof(CalculationTypeViewModel), model, sb.ToString());
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
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(EditAsync), id, nameof(CalculationTypeViewModel), model);
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
	/// Displays the confirmation page for deleting a Calculation Type.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to delete.</param>
	/// <returns>A view containing a <see cref="CalculationTypeViewModel"/> with the details of the Calculation Type to be deleted.</returns>
	// GET: CalculationTypes/Delete/5
	[HttpGet]
	public async Task<ActionResult<CalculationTypeViewModel?>> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DeleteAsync), id);
			}

			CalculationTypeFullModel? calculationType = await _crud.GetCalculationTypeByIdAsync(id).ConfigureAwait(false);
			CalculationTypeViewModel? model = calculationType is null ? null : new(calculationType);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DeleteAsync), id, nameof(CalculationTypeViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DeleteAsync), id);
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
	/// Processes the deletion confirmation for a Calculation Type.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to delete.</param>
	/// <param name="model">The <see cref="CalculationTypeViewModel"/> containing the Calculation Type data for validation.</param>
	/// <returns>A redirect to the Index action if successful; otherwise, returns the Delete view if the ID does not match the model.</returns>
	// POST: CalculationTypes/Delete/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> DeleteAsync(int id, CalculationTypeViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DeleteAsync), id);
			}

			if (id == model?.Id)
			{
				await _crud.DeleteCalculationTypeAsync(id).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DeleteAsync), id);
				}

				return RedirectToAction(nameof(Index));
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DeleteAsync), id, nameof(CalculationTypeViewModel), model);
				}

				Response.StatusCode = StatusCodes.Status400BadRequest;
				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DeleteAsync), id);
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
