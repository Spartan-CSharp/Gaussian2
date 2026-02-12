using System.Diagnostics;
using System.Globalization;
using System.Text;

using GaussianCommonLibrary.Models;

using GaussianMVC.Models;

using GaussianMVCLibrary.DataAccess;

using Microsoft.AspNetCore.Mvc;

namespace GaussianMVC.Controllers;

/// <summary>
/// MVC controller for managing Spin State/Electronic State/Method Family Combinations.
/// Provides CRUD operations for Spin State/Electronic State/Method Family Combination entities through a web interface.
/// </summary>
/// <param name="logger">The logger instance for logging controller operations.</param>
/// <param name="ssesmfCrud">The data access service for Spin State/Electronic State/Method Family Combinations CRUD operations.</param>
/// <param name="esmfCrud">The data access service for Electronic State/Method Family Combinations CRUD operations.</param>
/// <param name="ssCrud">The data access service for Spin States CRUD operations.</param>
public class SpinStatesElectronicStatesMethodFamiliesController(ILogger<SpinStatesElectronicStatesMethodFamiliesController> logger, ISpinStatesElectronicStatesMethodFamiliesCrud ssesmfCrud, IElectronicStatesMethodFamiliesCrud esmfCrud, ISpinStatesCrud ssCrud) : Controller
{
	private readonly ILogger<SpinStatesElectronicStatesMethodFamiliesController> _logger = logger;
	private readonly ISpinStatesElectronicStatesMethodFamiliesCrud _ssesmfCrud = ssesmfCrud;
	private readonly IElectronicStatesMethodFamiliesCrud _esmfCrud = esmfCrud;
	private readonly ISpinStatesCrud _ssCrud = ssCrud;

	/// <summary>
	/// Displays a list of all Spin State/Electronic State/Method Family Combinations.
	/// </summary>
	/// <returns>A view containing a list of <see cref="SpinStateElectronicStateMethodFamilyViewModel"/> objects representing all Spin State/Electronic State/Method Family Combinations.</returns>
	// GET: SpinStatesElectronicStatesMethodFamilies
	[HttpGet]
	public async Task<ActionResult<List<SpinStateElectronicStateMethodFamilyViewModel>>> IndexAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", nameof(IndexAsync), nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(IndexAsync));
			}

			List<SpinStateElectronicStateMethodFamilySimpleModel> spinStatesElectronicStatesMethodFamilies = await _ssesmfCrud.GetAllSimpleSpinStatesElectronicStatesMethodFamiliesAsync().ConfigureAwait(false);
			List<ElectronicStateMethodFamilyRecord> electronicStatesMethodFamilies = await _esmfCrud.GetElectronicStateMethodFamilyListAsync().ConfigureAwait(false);
			List<SpinStateRecord> spinStates = await _ssCrud.GetSpinStateListAsync().ConfigureAwait(false);
			List<SpinStateElectronicStateMethodFamilyViewModel> modelList = [];

			foreach (SpinStateElectronicStateMethodFamilySimpleModel model in spinStatesElectronicStatesMethodFamilies)
			{
				SpinStateElectronicStateMethodFamilyViewModel viewModel = new(model, electronicStatesMethodFamilies, spinStates);
				modelList.Add(viewModel);
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(IndexAsync), modelList.Count, nameof(SpinStateElectronicStateMethodFamilyViewModel));
			}

			return View(modelList);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(IndexAsync));
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
	/// Displays detailed information for a specific Spin State/Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State/Electronic State/Method Family Combination to display.</param>
	/// <returns>A view containing a <see cref="SpinStateElectronicStateMethodFamilyViewModel"/> with the details of the specified Spin State/Electronic State/Method Family Combination.</returns>
	// GET: SpinStatesElectronicStatesMethodFamilies/Details/5
	[HttpGet]
	public async Task<ActionResult<SpinStateElectronicStateMethodFamilyViewModel?>> DetailsAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(DetailsAsync), id);
			}

			SpinStateElectronicStateMethodFamilyFullModel? spinStateElectronicStateMethodFamily = await _ssesmfCrud.GetSpinStateElectronicStateMethodFamilyByIdAsync(id).ConfigureAwait(false);
			List<ElectronicStateMethodFamilyRecord> electronicStatesMethodFamilies = await _esmfCrud.GetElectronicStateMethodFamilyListAsync().ConfigureAwait(false);
			List<SpinStateRecord> spinStates = await _ssCrud.GetSpinStateListAsync().ConfigureAwait(false);
			SpinStateElectronicStateMethodFamilyViewModel? model = spinStateElectronicStateMethodFamily is null ? null : new(spinStateElectronicStateMethodFamily, electronicStatesMethodFamilies, spinStates);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(DetailsAsync), id, nameof(SpinStateElectronicStateMethodFamilyViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(DetailsAsync), id);
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
	/// Displays the form for creating a new Spin State/Electronic State/Method Family Combination.
	/// </summary>
	/// <returns>A view containing an empty <see cref="SpinStateElectronicStateMethodFamilyViewModel"/> for creating a new Spin State/Electronic State/Method Family Combination.</returns>
	// GET: SpinStatesElectronicStatesMethodFamilies/Create
	[HttpGet]
	public async Task<ActionResult<SpinStateElectronicStateMethodFamilyViewModel>> CreateAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(CreateAsync));
			}

			List<SpinStateRecord> spinStates = await _ssCrud.GetSpinStateListAsync().ConfigureAwait(false);
			List<ElectronicStateMethodFamilyRecord> electronicStatesMethodFamilies = await _esmfCrud.GetElectronicStateMethodFamilyListAsync().ConfigureAwait(false);
			SpinStateElectronicStateMethodFamilyViewModel model = new(electronicStatesMethodFamilies, spinStates);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(CreateAsync), nameof(SpinStateElectronicStateMethodFamilyViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(CreateAsync));
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
	/// Processes the form submission for creating a new Spin State/Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="model">The <see cref="SpinStateElectronicStateMethodFamilyViewModel"/> containing the data for the new Spin State/Electronic State/Method Family Combination.</param>
	/// <returns>A redirect to the Index action if successful; otherwise, returns the Create view with validation errors.</returns>
	// POST: SpinStatesElectronicStatesMethodFamilies/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> CreateAsync(SpinStateElectronicStateMethodFamilyViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(CreateAsync), nameof(SpinStateElectronicStateMethodFamilyViewModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid)
			{
				_ = await _ssesmfCrud.CreateNewSpinStateElectronicStateMethodFamilyAsync(model.ToSimpleModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning RedirectToAction({ActionName}).", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(CreateAsync), nameof(IndexAsync));
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
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(CreateAsync), nameof(SpinStateElectronicStateMethodFamilyViewModel), model, sb.ToString());
				}

				Response.StatusCode = StatusCodes.Status400BadRequest;
				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(CreateAsync), nameof(SpinStateElectronicStateMethodFamilyViewModel), model);
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
	/// Displays the form for editing an existing Spin State/Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State/Electronic State/Method Family Combination to edit.</param>
	/// <returns>A view containing a <see cref="SpinStateElectronicStateMethodFamilyViewModel"/> with the current data of the Spin State/Electronic State/Method Family Combination.</returns>
	// GET: SpinStatesElectronicStatesMethodFamilies/Edit/5
	[HttpGet]
	public async Task<ActionResult<SpinStateElectronicStateMethodFamilyViewModel?>> EditAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(EditAsync), id);
			}

			SpinStateElectronicStateMethodFamilyFullModel? spinStateElectronicStateMethodFamily = await _ssesmfCrud.GetSpinStateElectronicStateMethodFamilyByIdAsync(id).ConfigureAwait(false);
			List<SpinStateRecord> spinStates = await _ssCrud.GetSpinStateListAsync().ConfigureAwait(false);
			List<ElectronicStateMethodFamilyRecord> electronicStatesMethodFamilies = await _esmfCrud.GetElectronicStateMethodFamilyListAsync().ConfigureAwait(false);
			SpinStateElectronicStateMethodFamilyViewModel? model = spinStateElectronicStateMethodFamily is null ? null : new(spinStateElectronicStateMethodFamily, electronicStatesMethodFamilies, spinStates);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(EditAsync), id, nameof(SpinStateElectronicStateMethodFamilyViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(EditAsync), id);
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
	/// Processes the form submission for editing an existing Spin State/Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State/Electronic State/Method Family Combination to update.</param>
	/// <param name="model">The <see cref="SpinStateElectronicStateMethodFamilyViewModel"/> containing the updated data.</param>
	/// <returns>A redirect to the Index action if successful; otherwise, returns the Edit view with validation errors.</returns>
	// POST: SpinStatesElectronicStatesMethodFamilies/Edit/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult<SpinStateElectronicStateMethodFamilyViewModel>> EditAsync(int id, SpinStateElectronicStateMethodFamilyViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(EditAsync), id, nameof(SpinStateElectronicStateMethodFamilyViewModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid && id == model.Id)
			{
				SpinStateElectronicStateMethodFamilySimpleModel spinStateElectronicStateMethodFamily = model.ToSimpleModel();
				SpinStateElectronicStateMethodFamilyFullModel updatedSpinStateElectronicStateMethodFamily = await _ssesmfCrud.UpdateSpinStateElectronicStateMethodFamilyAsync(spinStateElectronicStateMethodFamily).ConfigureAwait(false);
				List<SpinStateRecord> spinStates = await _ssCrud.GetSpinStateListAsync().ConfigureAwait(false);
				List<ElectronicStateMethodFamilyRecord> electronicStatesMethodFamilies = await _esmfCrud.GetElectronicStateMethodFamilyListAsync().ConfigureAwait(false);
				model = new SpinStateElectronicStateMethodFamilyViewModel(updatedSpinStateElectronicStateMethodFamily, electronicStatesMethodFamilies, spinStates);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(EditAsync), id, nameof(SpinStateElectronicStateMethodFamilyViewModel), model);
				}

				return View("Details", model);
			}
			else
			{
				if (id != model.Id)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(EditAsync), id, nameof(SpinStateElectronicStateMethodFamilyViewModel), model);
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
						_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(EditAsync), nameof(SpinStateElectronicStateMethodFamilyViewModel), model, sb.ToString());
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
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(EditAsync), id, nameof(SpinStateElectronicStateMethodFamilyViewModel), model);
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
	/// Displays the confirmation page for deleting a Spin State/Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State/Electronic State/Method Family Combination to delete.</param>
	/// <returns>A view containing a <see cref="SpinStateElectronicStateMethodFamilyViewModel"/> with the details of the Spin State/Electronic State/Method Family Combination to be deleted.</returns>
	// GET: SpinStatesElectronicStatesMethodFamilies/Delete/5
	[HttpGet]
	public async Task<ActionResult<SpinStateElectronicStateMethodFamilyViewModel?>> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id);
			}

			SpinStateElectronicStateMethodFamilyFullModel? spinStateElectronicStateMethodFamily = await _ssesmfCrud.GetSpinStateElectronicStateMethodFamilyByIdAsync(id).ConfigureAwait(false);
			List<ElectronicStateMethodFamilyRecord> electronicStatesMethodFamilies = await _esmfCrud.GetElectronicStateMethodFamilyListAsync().ConfigureAwait(false);
			List<SpinStateRecord> spinState = await _ssCrud.GetSpinStateListAsync().ConfigureAwait(false);
			SpinStateElectronicStateMethodFamilyViewModel? model = spinStateElectronicStateMethodFamily is null ? null : new(spinStateElectronicStateMethodFamily, electronicStatesMethodFamilies, spinState);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id, nameof(SpinStateElectronicStateMethodFamilyViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id);
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
	/// Processes the deletion confirmation for a Spin State/Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State/Electronic State/Method Family Combination to delete.</param>
	/// <param name="model">The <see cref="SpinStateElectronicStateMethodFamilyViewModel"/> containing the Spin State/Electronic State/Method Family Combination data for validation.</param>
	/// <returns>A redirect to the Index action if successful; otherwise, returns the Delete view if the ID does not match the model.</returns>
	// POST: SpinStatesElectronicStatesMethodFamilies/Delete/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> DeleteAsync(int id, SpinStateElectronicStateMethodFamilyViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id);
			}

			if (id == model?.Id)
			{
				await _ssesmfCrud.DeleteSpinStateElectronicStateMethodFamilyAsync(id).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id);
				}

				return RedirectToAction(nameof(Index));
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id, nameof(SpinStateElectronicStateMethodFamilyViewModel), model);
				}

				Response.StatusCode = StatusCodes.Status400BadRequest;
				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id);
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
