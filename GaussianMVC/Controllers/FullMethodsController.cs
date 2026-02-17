using System.Diagnostics;
using System.Globalization;
using System.Text;

using GaussianCommonLibrary.Models;

using GaussianMVC.Models;

using GaussianMVCLibrary.DataAccess;

using Microsoft.AspNetCore.Mvc;

namespace GaussianMVC.Controllers;

/// <summary>
/// MVC controller for managing Full Methods.
/// Provides CRUD operations for Full Method entities through a web interface.
/// </summary>
/// <param name="logger">The logger instance for logging controller operations.</param>
/// <param name="fmCrud">The data access service for Full Methods CRUD operations.</param>
/// <param name="ssesmfCrud">The data access service for Spin State/Electronic State/Method Family Combinations CRUD operations.</param>
/// <param name="bmCrud">The data access service for Base Methods CRUD operations.</param>
public class FullMethodsController(ILogger<FullMethodsController> logger, IFullMethodsCrud fmCrud, ISpinStatesElectronicStatesMethodFamiliesCrud ssesmfCrud, IBaseMethodsCrud bmCrud) : Controller
{
	private readonly ILogger<FullMethodsController> _logger = logger;
	private readonly IFullMethodsCrud _fmCrud = fmCrud;
	private readonly ISpinStatesElectronicStatesMethodFamiliesCrud _ssesmfCrud = ssesmfCrud;
	private readonly IBaseMethodsCrud _bmCrud = bmCrud;

	/// <summary>
	/// Displays a list of all Full Methods.
	/// </summary>
	/// <returns>A view containing a list of <see cref="FullMethodViewModel"/> objects representing all Full Methods.</returns>
	// GET: FullMethods
	[HttpGet]
	public async Task<ActionResult<List<FullMethodViewModel>>> IndexAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", nameof(IndexAsync), nameof(FullMethodsController), nameof(IndexAsync));
			}

			List<FullMethodSimpleModel> fullMethods = await _fmCrud.GetAllSimpleFullMethodsAsync().ConfigureAwait(false);
			List<SpinStateElectronicStateMethodFamilyRecord> spinStatesElectronicStatesMethodFamilies = await _ssesmfCrud.GetSpinStateElectronicStateMethodFamilyListAsync().ConfigureAwait(false);
			List<BaseMethodRecord> baseMethods = await _bmCrud.GetBaseMethodListAsync().ConfigureAwait(false);
			List<FullMethodViewModel> modelList = [];

			foreach (FullMethodSimpleModel model in fullMethods)
			{
				FullMethodViewModel viewModel = new(model, spinStatesElectronicStatesMethodFamilies, baseMethods);
				modelList.Add(viewModel);
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(IndexAsync), modelList.Count, nameof(FullMethodViewModel));
			}

			return View(modelList);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(IndexAsync));
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
	/// Displays detailed information for a specific Full Method.
	/// </summary>
	/// <param name="id">The unique identifier of the Full Method to display.</param>
	/// <returns>A view containing a <see cref="FullMethodViewModel"/> with the details of the specified Full Method.</returns>
	// GET: FullMethods/Details/5
	[HttpGet]
	public async Task<ActionResult<FullMethodViewModel?>> DetailsAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(DetailsAsync), id);
			}

			FullMethodFullModel? fullMethod = await _fmCrud.GetFullMethodByIdAsync(id).ConfigureAwait(false);
			List<SpinStateElectronicStateMethodFamilyRecord> spinStatesElectronicStatesMethodFamilies = await _ssesmfCrud.GetSpinStateElectronicStateMethodFamilyListAsync().ConfigureAwait(false);
			List<BaseMethodRecord> baseMethods = await _bmCrud.GetBaseMethodListAsync().ConfigureAwait(false);
			FullMethodViewModel? model = fullMethod is null ? null : new(fullMethod, spinStatesElectronicStatesMethodFamilies, baseMethods);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(DetailsAsync), id, nameof(FullMethodViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(DetailsAsync), id);
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
	/// Displays the form for creating a new Full Method.
	/// </summary>
	/// <returns>A view containing an empty <see cref="FullMethodViewModel"/> for creating a new Full Method.</returns>
	// GET: FullMethods/Create
	[HttpGet]
	public async Task<ActionResult<FullMethodViewModel>> CreateAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(CreateAsync));
			}

			List<SpinStateElectronicStateMethodFamilyRecord> spinStatesElectronicStatesMethodFamilies = await _ssesmfCrud.GetSpinStateElectronicStateMethodFamilyListAsync().ConfigureAwait(false);
			List<BaseMethodRecord> baseMethods = await _bmCrud.GetBaseMethodListAsync().ConfigureAwait(false);
			FullMethodViewModel model = new(spinStatesElectronicStatesMethodFamilies, baseMethods);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(CreateAsync), nameof(FullMethodViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(CreateAsync));
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
	/// Processes the form submission for creating a new Full Method.
	/// </summary>
	/// <param name="model">The <see cref="FullMethodViewModel"/> containing the data for the new Full Method.</param>
	/// <returns>A redirect to the Index action if successful; otherwise, returns the Create view with validation errors.</returns>
	// POST: FullMethods/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> CreateAsync(FullMethodViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(CreateAsync), nameof(FullMethodViewModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid)
			{
				_ = await _fmCrud.CreateNewFullMethodAsync(model.ToSimpleModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning RedirectToAction({ActionName}).", HttpContext.Request.Method, nameof(FullMethodsController), nameof(CreateAsync), nameof(IndexAsync));
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
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(FullMethodsController), nameof(CreateAsync), nameof(FullMethodViewModel), model, sb.ToString());
				}

				Response.StatusCode = StatusCodes.Status400BadRequest;
				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(CreateAsync), nameof(FullMethodViewModel), model);
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
	/// Displays the form for editing an existing Full Method.
	/// </summary>
	/// <param name="id">The unique identifier of the Full Method to edit.</param>
	/// <returns>A view containing a <see cref="FullMethodViewModel"/> with the current data of the Full Method.</returns>
	// GET: FullMethods/Edit/5
	[HttpGet]
	public async Task<ActionResult<FullMethodViewModel?>> EditAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(EditAsync), id);
			}

			FullMethodFullModel? fullMethod = await _fmCrud.GetFullMethodByIdAsync(id).ConfigureAwait(false);
			List<SpinStateElectronicStateMethodFamilyRecord> spinStatesElectronicStatesMethodFamilies = await _ssesmfCrud.GetSpinStateElectronicStateMethodFamilyListAsync().ConfigureAwait(false);
			List<BaseMethodRecord> baseMethods = await _bmCrud.GetBaseMethodListAsync().ConfigureAwait(false);
			FullMethodViewModel? model = fullMethod is null ? null : new(fullMethod, spinStatesElectronicStatesMethodFamilies, baseMethods);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(EditAsync), id, nameof(FullMethodViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(EditAsync), id);
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
	/// Processes the form submission for editing an existing Full Method.
	/// </summary>
	/// <param name="id">The unique identifier of the Full Method to update.</param>
	/// <param name="model">The <see cref="FullMethodViewModel"/> containing the updated data.</param>
	/// <returns>A redirect to the Index action if successful; otherwise, returns the Edit view with validation errors.</returns>
	// POST: FullMethods/Edit/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult<FullMethodViewModel>> EditAsync(int id, FullMethodViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(EditAsync), id, nameof(FullMethodViewModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid && id == model.Id)
			{
				FullMethodSimpleModel fullMethod = model.ToSimpleModel();
				FullMethodFullModel updatedFullMethod = await _fmCrud.UpdateFullMethodAsync(fullMethod).ConfigureAwait(false);
				List<SpinStateElectronicStateMethodFamilyRecord> spinStatesElectronicStatesMethodFamilies = await _ssesmfCrud.GetSpinStateElectronicStateMethodFamilyListAsync().ConfigureAwait(false);
				List<BaseMethodRecord> baseMethods = await _bmCrud.GetBaseMethodListAsync().ConfigureAwait(false);
				model = new FullMethodViewModel(updatedFullMethod, spinStatesElectronicStatesMethodFamilies, baseMethods);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(EditAsync), id, nameof(FullMethodViewModel), model);
				}

				return View("Details", model);
			}
			else
			{
				if (id != model.Id)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(EditAsync), id, nameof(FullMethodViewModel), model);
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
						_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(FullMethodsController), nameof(EditAsync), nameof(FullMethodViewModel), model, sb.ToString());
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
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(EditAsync), id, nameof(FullMethodViewModel), model);
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
	/// Displays the confirmation page for deleting a Full Method.
	/// </summary>
	/// <param name="id">The unique identifier of the Full Method to delete.</param>
	/// <returns>A view containing a <see cref="FullMethodViewModel"/> with the details of the Full Method to be deleted.</returns>
	// GET: FullMethods/Delete/5
	[HttpGet]
	public async Task<ActionResult<FullMethodViewModel?>> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(DeleteAsync), id);
			}

			FullMethodFullModel? fullMethod = await _fmCrud.GetFullMethodByIdAsync(id).ConfigureAwait(false);
			List<SpinStateElectronicStateMethodFamilyRecord> spinStatesElectronicStatesMethodFamilies = await _ssesmfCrud.GetSpinStateElectronicStateMethodFamilyListAsync().ConfigureAwait(false);
			List<BaseMethodRecord> baseMethods = await _bmCrud.GetBaseMethodListAsync().ConfigureAwait(false);
			FullMethodViewModel? model = fullMethod is null ? null : new(fullMethod, spinStatesElectronicStatesMethodFamilies, baseMethods);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(DeleteAsync), id, nameof(FullMethodViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(DeleteAsync), id);
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
	/// Processes the deletion confirmation for a Full Method.
	/// </summary>
	/// <param name="id">The unique identifier of the Full Method to delete.</param>
	/// <param name="model">The <see cref="FullMethodViewModel"/> containing the Full Method data for validation.</param>
	/// <returns>A redirect to the Index action if successful; otherwise, returns the Delete view if the ID does not match the model.</returns>
	// POST: FullMethods/Delete/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> DeleteAsync(int id, FullMethodViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(DeleteAsync), id);
			}

			if (id == model?.Id)
			{
				await _fmCrud.DeleteFullMethodAsync(id).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(DeleteAsync), id);
				}

				return RedirectToAction(nameof(Index));
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(DeleteAsync), id, nameof(FullMethodViewModel), model);
				}

				Response.StatusCode = StatusCodes.Status400BadRequest;
				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(DeleteAsync), id);
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
