using System.Diagnostics;
using System.Globalization;
using System.Text;

using GaussianCommonLibrary.Models;

using GaussianMVC.Models;

using GaussianMVCLibrary.DataAccess;

using Microsoft.AspNetCore.Mvc;

namespace GaussianMVC.Controllers;

/// <summary>
/// MVC controller for managing Method Families.
/// Provides CRUD operations for MethodFamilyId entities through a web interface.
/// </summary>
/// <param name="logger">The logger instance for logging controller operations.</param>
/// <param name="crud">The data access service for MethodFamilyId CRUD operations.</param>
public class MethodFamiliesController(ILogger<MethodFamiliesController> logger, IMethodFamiliesCrud crud) : Controller
{
	private readonly ILogger<MethodFamiliesController> _logger = logger;
	private readonly IMethodFamiliesCrud _crud = crud;

	/// <summary>
	/// Displays a list of all Method Families.
	/// </summary>
	/// <returns>
	/// A view containing a list of <see cref="MethodFamilyViewModel"/> objects representing all Method Families.
	/// </returns>
	// GET: MethodFamilies
	[HttpGet]
	public async Task<ActionResult<List<MethodFamilyViewModel>>> IndexAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(IndexAsync));
			}

			List<MethodFamilyFullModel> methodFamilies = await _crud.GetAllMethodFamiliesAsync().ConfigureAwait(false);
			List<MethodFamilyViewModel> modelList = [];

			foreach (MethodFamilyFullModel model in methodFamilies)
			{
				MethodFamilyViewModel viewModel = new(model);
				modelList.Add(viewModel);
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(IndexAsync), modelList.Count, nameof(MethodFamilyViewModel));
			}

			return View(modelList);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(IndexAsync));
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
	/// Displays detailed information for a specific Method Family.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to display.</param>
	/// <returns>
	/// A view containing a <see cref="MethodFamilyViewModel"/> with the details of the specified Method Family.
	/// </returns>
	// GET: MethodFamilies/Details/5
	[HttpGet]
	public async Task<ActionResult<MethodFamilyViewModel?>> DetailsAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(DetailsAsync), id);
			}

			MethodFamilyFullModel? methodFamily = await _crud.GetMethodFamilyByIdAsync(id).ConfigureAwait(false);
			MethodFamilyViewModel? model = methodFamily is null ? null : new(methodFamily);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(DetailsAsync), id, nameof(MethodFamilyViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(DetailsAsync), id);
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
	/// Displays the form for creating a new Method Family.
	/// </summary>
	/// <returns>A view with an empty form for creating a new Method Family.</returns>
	// GET: MethodFamilies/Create
	[HttpGet]
	public async Task<ActionResult<MethodFamilyViewModel>> CreateAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(CreateAsync));
			}

			MethodFamilyViewModel model = new();

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(CreateAsync), nameof(MethodFamilyViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(CreateAsync));
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
	/// Processes the creation of a new Method Family.
	/// </summary>
	/// <param name="model">The view model containing the data for the new Method Family.</param>
	/// <returns>
	/// Redirects to the Index action if successful; otherwise, returns the view with validation errors.
	/// On exception, redirects to the Error page.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	// POST: MethodFamilies/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> CreateAsync(MethodFamilyViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(CreateAsync), nameof(MethodFamilyViewModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid)
			{
				MethodFamilyFullModel methodFamily = model.ToFullModel();
				methodFamily = await _crud.CreateNewMethodFamilyAsync(methodFamily).ConfigureAwait(false);
				model = new MethodFamilyViewModel(methodFamily);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(CreateAsync), nameof(MethodFamilyViewModel), model);
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
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(CreateAsync), nameof(MethodFamilyViewModel), model, sb.ToString());
				}

				Response.StatusCode = StatusCodes.Status400BadRequest;
				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(CreateAsync), nameof(MethodFamilyViewModel), model);
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
	/// Displays the form for editing an existing Method Family.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to edit.</param>
	/// <returns>
	/// A view containing a <see cref="MethodFamilyViewModel"/> populated with the existing data.
	/// </returns>
	// GET: MethodFamilies/Edit/5
	[HttpGet]
	public async Task<ActionResult<MethodFamilyViewModel?>> EditAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(EditAsync), id);
			}

			MethodFamilyFullModel? methodFamily = await _crud.GetMethodFamilyByIdAsync(id).ConfigureAwait(false);
			MethodFamilyViewModel? model = methodFamily is null ? null : new(methodFamily);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(EditAsync), id, nameof(MethodFamilyViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(EditAsync), id);
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
	/// Processes the update of an existing Method Family.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family being edited.</param>
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
	// POST: MethodFamilies/Edit/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> EditAsync(int id, MethodFamilyViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(EditAsync), id, nameof(MethodFamilyViewModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid && id == model.Id)
			{
				MethodFamilyFullModel methodFamily = model.ToFullModel();
				methodFamily = await _crud.UpdateMethodFamilyAsync(methodFamily).ConfigureAwait(false);
				model = new MethodFamilyViewModel(methodFamily);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(EditAsync), id, nameof(MethodFamilyViewModel), model);
				}

				return View("Details", model);
			}
			else
			{
				if (id != model.Id)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(EditAsync), id, nameof(MethodFamilyViewModel), model);
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
						_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(EditAsync), nameof(MethodFamilyViewModel), model, sb.ToString());
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
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(EditAsync), id, nameof(MethodFamilyViewModel), model);
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
	/// Displays the confirmation page for deleting a Method Family.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to delete.</param>
	/// <returns>
	/// A view containing a <see cref="MethodFamilyViewModel"/> with the details of the Method Family to be deleted.
	/// </returns>
	// GET: MethodFamilies/Delete/5
	[HttpGet]
	public async Task<ActionResult<MethodFamilyViewModel?>> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(DeleteAsync), id);
			}

			MethodFamilyFullModel? methodFamily = await _crud.GetMethodFamilyByIdAsync(id).ConfigureAwait(false);
			MethodFamilyViewModel? model = methodFamily is null ? null : new(methodFamily);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(DeleteAsync), id, nameof(MethodFamilyViewModel), model);
			}

			return View(model);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(DeleteAsync), id);
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
	/// Processes the deletion of a Method Family.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to delete.</param>
	/// <param name="model">The view model containing the data of the Method Family being deleted.</param>
	/// <returns>
	/// Redirects to the Index action if successful; otherwise, returns the view with an error message.
	/// On exception, redirects to the Error page.
	/// </returns>
	/// <remarks>
	/// Validates that the ID from the route matches the ID in the model before proceeding with deletion.
	/// Logs warnings for ID mismatches and errors for exceptions.
	/// </remarks>
	// POST: MethodFamilies/Delete/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> DeleteAsync(int id, MethodFamilyViewModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(DeleteAsync), id, nameof(MethodFamilyViewModel), model);
			}

			if (id == model?.Id)
			{
				await _crud.DeleteMethodFamilyAsync(id).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(DeleteAsync), id);
				}

				return RedirectToAction(nameof(Index));
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(DeleteAsync), id, nameof(MethodFamilyViewModel), model);
				}

				Response.StatusCode = StatusCodes.Status400BadRequest;
				return View(model);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(DeleteAsync), id, nameof(MethodFamilyViewModel), model);
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
