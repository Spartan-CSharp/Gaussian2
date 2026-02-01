using System.Diagnostics;
using System.Globalization;
using System.Text;

using GaussianCommonLibrary.Models;

using GaussianMVC.Models;

using GaussianMVCLibrary.DataAccess;

using Microsoft.AspNetCore.Mvc;

namespace GaussianMVC.Controllers;

/// <summary>
/// Controller for managing electronic states in the Gaussian MVC application.
/// Provides CRUD operations for electronic state entities.
/// </summary>
public class ElectronicStatesController(ILogger<ElectronicStatesController> logger, IElectronicStatesCrud crud) : Controller
{
	private readonly ILogger<ElectronicStatesController> _logger = logger;
	private readonly IElectronicStatesCrud _crud = crud;

	/// <summary>
	/// Displays a list of all electronic states.
	/// </summary>
	/// <returns>A view containing a list of electronic state view models.</returns>
	// GET: ElectronicStates
	[HttpGet]
	public async Task<ActionResult<List<ElectronicStateViewModel>>> IndexAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(IndexAsync));
			}

			List<ElectronicStateFullModel> electronicStates = await _crud.GetAllElectronicStatesAsync().ConfigureAwait(false);
			List<ElectronicStateViewModel> modelList = [];

			foreach (ElectronicStateFullModel model in electronicStates)
			{
				ElectronicStateViewModel viewModel = new(model);
				modelList.Add(viewModel);
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(IndexAsync), modelList.Count, nameof(ElectronicStateViewModel));
			}

			return View(modelList);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(IndexAsync));
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
	/// Displays detailed information about a specific electronic state.
	/// </summary>
	/// <param name="id">The unique identifier of the electronic state to display.</param>
	/// <returns>A view containing the electronic state details, or null if not found.</returns>
	// GET: ElectronicStates/Details/5
	[HttpGet]
	public async Task<ActionResult<ElectronicStateViewModel?>> DetailsAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(DetailsAsync), id);
			}

			ElectronicStateFullModel? electronicState = await _crud.GetElectronicStateByIdAsync(id).ConfigureAwait(false);
			ElectronicStateViewModel? model = electronicState is null ? null : new(electronicState);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(DetailsAsync), id, nameof(ElectronicStateViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(DetailsAsync), id);
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
	/// Displays the form for creating a new electronic state.
	/// </summary>
	/// <returns>A view containing an empty electronic state view model.</returns>
	// GET: ElectronicStates/CreateAsync
	[HttpGet]
	public async Task<ActionResult<ElectronicStateViewModel>> CreateAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(CreateAsync));
			}

			ElectronicStateViewModel model = new();

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(CreateAsync), nameof(ElectronicStateViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(CreateAsync));
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
	/// Processes the creation of a new electronic state.
	/// </summary>
	/// <param name="model">The electronic state view model containing the data to create.</param>
	/// <returns>Redirects to the index page on success, or returns to the create view with validation errors.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	// POST: ElectronicStates/CreateAsync
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> CreateAsync(ElectronicStateViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(CreateAsync), nameof(ElectronicStateViewModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid)
			{
				ElectronicStateFullModel electronicState = model.ToFullModel();
				electronicState = await _crud.CreateNewElectronicStateAsync(electronicState).ConfigureAwait(false);
				model = new ElectronicStateViewModel(electronicState);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(CreateAsync), nameof(ElectronicStateViewModel), model);
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
					sb.AppendLine(validationErrors.Key);
					foreach (string validationError in validationErrors.Value)
					{
						sb.AppendLine(CultureInfo.InvariantCulture, $"\t{validationError}");
					}
				}

				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(CreateAsync), nameof(ElectronicStateViewModel), model, sb.ToString());
				}

				Response.StatusCode = StatusCodes.Status400BadRequest;
				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(CreateAsync), nameof(ElectronicStateViewModel), model);
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
	/// Displays the form for editing an existing electronic state.
	/// </summary>
	/// <param name="id">The unique identifier of the electronic state to edit.</param>
	/// <returns>A view containing the electronic state view model to edit, or null if not found.</returns>
	// GET: ElectronicStates/Edit/5
	[HttpGet]
	public async Task<ActionResult<ElectronicStateViewModel?>> EditAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(EditAsync), id);
			}

			ElectronicStateFullModel? electronicState = await _crud.GetElectronicStateByIdAsync(id).ConfigureAwait(false);
			ElectronicStateViewModel? model = electronicState is null ? null : new(electronicState);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(EditAsync), id, nameof(ElectronicStateViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(EditAsync), id);
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
	/// Processes the update of an existing electronic state.
	/// </summary>
	/// <param name="id">The unique identifier of the electronic state to update.</param>
	/// <param name="model">The electronic state view model containing the updated data.</param>
	/// <returns>Redirects to the index page on success, or returns to the edit view with validation errors.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	/// <remarks>
	/// The id parameter must match the model's Id property for the update to proceed.
	/// </remarks>
	// POST: ElectronicStates/Edit/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> EditAsync(int id, ElectronicStateViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(EditAsync), id, nameof(ElectronicStateViewModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid && id == model.Id)
			{
				ElectronicStateFullModel electronicState = model.ToFullModel();
				electronicState = await _crud.UpdateElectronicStateAsync(electronicState).ConfigureAwait(false);
				model = new ElectronicStateViewModel(electronicState);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(EditAsync), id, nameof(ElectronicStateViewModel), model);
				}

				return View("Details", model);
			}
			else
			{
				if (id != model.Id)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(EditAsync), id, nameof(ElectronicStateViewModel), model);
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
						_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(EditAsync), nameof(ElectronicStateViewModel), model, sb.ToString());
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
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(EditAsync), id, nameof(ElectronicStateViewModel), model);
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
	/// Displays the confirmation page for deleting an electronic state.
	/// </summary>
	/// <param name="id">The unique identifier of the electronic state to delete.</param>
	/// <returns>A view containing the electronic state view model to delete, or null if not found.</returns>
	/// <remarks>
	/// GET: ElectronicStates/Delete/5
	/// </remarks>
	[HttpGet]
	public async Task<ActionResult<ElectronicStateViewModel?>> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(DeleteAsync), id);
			}

			ElectronicStateFullModel? electronicState = await _crud.GetElectronicStateByIdAsync(id).ConfigureAwait(false);
			ElectronicStateViewModel? model = electronicState is null ? null : new(electronicState);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(DeleteAsync), id, nameof(ElectronicStateViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(DeleteAsync), id);
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
	/// Processes the deletion of an electronic state.
	/// </summary>
	/// <param name="id">The unique identifier of the electronic state to delete.</param>
	/// <param name="model">The electronic state view model containing the data to confirm deletion.</param>
	/// <returns>Redirects to the index page on success, or returns to the delete view if validation fails.</returns>
	/// <remarks>
	/// POST: ElectronicStates/Delete/5
	/// The id parameter must match the model's Id property for the deletion to proceed.
	/// </remarks>
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> DeleteAsync(int id, ElectronicStateViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(DeleteAsync), id, nameof(ElectronicStateViewModel), model);
			}

			if (id == model?.Id)
			{
				await _crud.DeleteElectronicStateAsync(id).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(DeleteAsync), id);
				}

				return RedirectToAction(nameof(Index));
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(DeleteAsync), id, nameof(ElectronicStateViewModel), model);
				}

				Response.StatusCode = StatusCodes.Status400BadRequest;
				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(DeleteAsync), id, nameof(ElectronicStateViewModel), model);
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
