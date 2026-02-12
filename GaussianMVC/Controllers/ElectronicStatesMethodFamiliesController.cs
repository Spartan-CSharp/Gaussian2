using System.Diagnostics;
using System.Globalization;
using System.Text;

using GaussianCommonLibrary.Models;

using GaussianMVC.Models;

using GaussianMVCLibrary.DataAccess;

using Microsoft.AspNetCore.Mvc;

namespace GaussianMVC.Controllers;

/// <summary>
/// MVC controller for managing Electronic State/Method Family Combinations.
/// Provides CRUD operations for Electronic State/Method Family Combination entities through a web interface.
/// </summary>
/// <param name="logger">The logger instance for logging controller operations.</param>
/// <param name="esmfCrud">The data access service for Electronic State/Method Family Combinations CRUD operations.</param>
/// <param name="esCrud">The data access service for Electronic States CRUD operations.</param>
/// <param name="mfCrud">The data access service for Method Families CRUD operations.</param>
public class ElectronicStatesMethodFamiliesController(ILogger<ElectronicStatesMethodFamiliesController> logger, IElectronicStatesMethodFamiliesCrud esmfCrud, IElectronicStatesCrud esCrud, IMethodFamiliesCrud mfCrud) : Controller
{
	private readonly ILogger<ElectronicStatesMethodFamiliesController> _logger = logger;
	private readonly IElectronicStatesMethodFamiliesCrud _esmfCrud = esmfCrud;
	private readonly IElectronicStatesCrud _esCrud = esCrud;
	private readonly IMethodFamiliesCrud _mfCrud = mfCrud;

	/// <summary>
	/// Displays a list of all Electronic State/Method Family Combinations.
	/// </summary>
	/// <returns>A view containing a list of <see cref="ElectronicStateMethodFamilyViewModel"/> objects representing all Electronic State/Method Family Combinations.</returns>
	// GET: ElectronicStatesMethodFamilies
	[HttpGet]
	public async Task<ActionResult<List<ElectronicStateMethodFamilyViewModel>>> IndexAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", nameof(IndexAsync), nameof(ElectronicStatesMethodFamiliesController), nameof(IndexAsync));
			}

			List<ElectronicStateMethodFamilySimpleModel> electronicStatesMethodFamilies = await _esmfCrud.GetAllSimpleElectronicStatesMethodFamiliesAsync().ConfigureAwait(false);
			List<ElectronicStateRecord> electronicStates = await _esCrud.GetElectronicStateListAsync().ConfigureAwait(false);
			List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
			List<ElectronicStateMethodFamilyViewModel> modelList = [];

			foreach (ElectronicStateMethodFamilySimpleModel model in electronicStatesMethodFamilies)
			{
				ElectronicStateMethodFamilyViewModel viewModel = new(model, electronicStates, methodFamilies);
				modelList.Add(viewModel);
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(IndexAsync), modelList.Count, nameof(ElectronicStateMethodFamilyViewModel));
			}

			return View(modelList);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(IndexAsync));
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
	/// Displays detailed information for a specific Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State/Method Family Combination to display.</param>
	/// <returns>A view containing a <see cref="ElectronicStateMethodFamilyViewModel"/> with the details of the specified Electronic State/Method Family Combination.</returns>
	// GET: ElectronicStatesMethodFamilies/Details/5
	[HttpGet]
	public async Task<ActionResult<ElectronicStateMethodFamilyViewModel?>> DetailsAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(DetailsAsync), id);
			}

			ElectronicStateMethodFamilyFullModel? electronicStateMethodFamily = await _esmfCrud.GetElectronicStateMethodFamilyByIdAsync(id).ConfigureAwait(false);
			List<ElectronicStateRecord> electronicStates = await _esCrud.GetElectronicStateListAsync().ConfigureAwait(false);
			List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
			ElectronicStateMethodFamilyViewModel? model = electronicStateMethodFamily is null ? null : new(electronicStateMethodFamily, electronicStates, methodFamilies);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(DetailsAsync), id, nameof(ElectronicStateMethodFamilyViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(DetailsAsync), id);
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
	/// Displays the form for creating a new Electronic State/Method Family Combination.
	/// </summary>
	/// <returns>A view containing an empty <see cref="ElectronicStateMethodFamilyViewModel"/> for creating a new Electronic State/Method Family Combination.</returns>
	// GET: ElectronicStatesMethodFamilies/Create
	[HttpGet]
	public async Task<ActionResult<ElectronicStateMethodFamilyViewModel>> CreateAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(CreateAsync));
			}

			List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
			List<ElectronicStateRecord> electronicStates = await _esCrud.GetElectronicStateListAsync().ConfigureAwait(false);
			ElectronicStateMethodFamilyViewModel model = new(electronicStates, methodFamilies);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(CreateAsync), nameof(ElectronicStateMethodFamilyViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(CreateAsync));
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
	/// Processes the form submission for creating a new Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="model">The <see cref="ElectronicStateMethodFamilyViewModel"/> containing the data for the new Electronic State/Method Family Combination.</param>
	/// <returns>A redirect to the Index action if successful; otherwise, returns the Create view with validation errors.</returns>
	// POST: ElectronicStatesMethodFamilies/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> CreateAsync(ElectronicStateMethodFamilyViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(CreateAsync), nameof(ElectronicStateMethodFamilyViewModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid)
			{
				_ = await _esmfCrud.CreateNewElectronicStateMethodFamilyAsync(model.ToSimpleModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning RedirectToAction({ActionName}).", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(CreateAsync), nameof(IndexAsync));
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
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(CreateAsync), nameof(ElectronicStateMethodFamilyViewModel), model, sb.ToString());
				}

				Response.StatusCode = StatusCodes.Status400BadRequest;
				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(CreateAsync), nameof(ElectronicStateMethodFamilyViewModel), model);
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
	/// Displays the form for editing an existing Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State/Method Family Combination to edit.</param>
	/// <returns>A view containing a <see cref="ElectronicStateMethodFamilyViewModel"/> with the current data of the Electronic State/Method Family Combination.</returns>
	// GET: ElectronicStatesMethodFamilies/Edit/5
	[HttpGet]
	public async Task<ActionResult<ElectronicStateMethodFamilyViewModel?>> EditAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(EditAsync), id);
			}

			ElectronicStateMethodFamilyFullModel? electronicStateMethodFamily = await _esmfCrud.GetElectronicStateMethodFamilyByIdAsync(id).ConfigureAwait(false);
			List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
			List<ElectronicStateRecord> electronicStates = await _esCrud.GetElectronicStateListAsync().ConfigureAwait(false);
			ElectronicStateMethodFamilyViewModel? model = electronicStateMethodFamily is null ? null : new(electronicStateMethodFamily, electronicStates, methodFamilies);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(EditAsync), id, nameof(ElectronicStateMethodFamilyViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(EditAsync), id);
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
	/// Processes the form submission for editing an existing Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State/Method Family Combination to update.</param>
	/// <param name="model">The <see cref="ElectronicStateMethodFamilyViewModel"/> containing the updated data.</param>
	/// <returns>A redirect to the Index action if successful; otherwise, returns the Edit view with validation errors.</returns>
	// POST: ElectronicStatesMethodFamilies/Edit/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult<ElectronicStateMethodFamilyViewModel>> EditAsync(int id, ElectronicStateMethodFamilyViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(EditAsync), id, nameof(ElectronicStateMethodFamilyViewModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid && id == model.Id)
			{
				ElectronicStateMethodFamilySimpleModel electronicStateMethodFamily = model.ToSimpleModel();
				ElectronicStateMethodFamilyFullModel updatedElectronicStateMethodFamily = await _esmfCrud.UpdateElectronicStateMethodFamilyAsync(electronicStateMethodFamily).ConfigureAwait(false);
				List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
				List<ElectronicStateRecord> electronicStates = await _esCrud.GetElectronicStateListAsync().ConfigureAwait(false);
				model = new ElectronicStateMethodFamilyViewModel(updatedElectronicStateMethodFamily, electronicStates, methodFamilies);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(EditAsync), id, nameof(ElectronicStateMethodFamilyViewModel), model);
				}

				return View("Details", model);
			}
			else
			{
				if (id != model.Id)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(EditAsync), id, nameof(ElectronicStateMethodFamilyViewModel), model);
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
						_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(EditAsync), nameof(ElectronicStateMethodFamilyViewModel), model, sb.ToString());
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
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(EditAsync), id, nameof(ElectronicStateMethodFamilyViewModel), model);
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
	/// Displays the confirmation page for deleting a Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State/Method Family Combination to delete.</param>
	/// <returns>A view containing a <see cref="ElectronicStateMethodFamilyViewModel"/> with the details of the Electronic State/Method Family Combination to be deleted.</returns>
	// GET: ElectronicStatesMethodFamilies/Delete/5
	[HttpGet]
	public async Task<ActionResult<ElectronicStateMethodFamilyViewModel?>> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id);
			}

			ElectronicStateMethodFamilyFullModel? electronicStateMethodFamily = await _esmfCrud.GetElectronicStateMethodFamilyByIdAsync(id).ConfigureAwait(false);
			List<ElectronicStateRecord> electronicStates = await _esCrud.GetElectronicStateListAsync().ConfigureAwait(false);
			List<MethodFamilyRecord> methodFamilies = await _mfCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
			ElectronicStateMethodFamilyViewModel? model = electronicStateMethodFamily is null ? null : new(electronicStateMethodFamily, electronicStates, methodFamilies);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id, nameof(ElectronicStateMethodFamilyViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id);
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
	/// Processes the deletion confirmation for a Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State/Method Family Combination to delete.</param>
	/// <param name="model">The <see cref="ElectronicStateMethodFamilyViewModel"/> containing the Electronic State/Method Family Combination data for validation.</param>
	/// <returns>A redirect to the Index action if successful; otherwise, returns the Delete view if the ID does not match the model.</returns>
	// POST: ElectronicStatesMethodFamilies/Delete/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> DeleteAsync(int id, ElectronicStateMethodFamilyViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id);
			}

			if (id == model?.Id)
			{
				await _esmfCrud.DeleteElectronicStateMethodFamilyAsync(id).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id);
				}

				return RedirectToAction(nameof(Index));
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id, nameof(ElectronicStateMethodFamilyViewModel), model);
				}

				Response.StatusCode = StatusCodes.Status400BadRequest;
				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id);
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
