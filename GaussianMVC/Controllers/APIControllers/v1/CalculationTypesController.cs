using System.Globalization;
using System.Text;

using Asp.Versioning;

using GaussianCommonLibrary.Models;

using GaussianMVC.Models.APIModels.V1;

using GaussianMVCLibrary.DataAccess;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GaussianMVC.Controllers.APIControllers.V1;

/// <summary>
/// API controller for managing Calculation Types.
/// Provides CRUD operations for Calculation Type resources via RESTful endpoints.
/// </summary>
/// <param name="logger">
/// Logger instance for tracking controller operations, errors, and debugging information.
/// Supports structured logging with various log levels (Debug, Debug, Error).
/// </param>
/// <param name="crud">
/// Data access layer service for performing CRUD operations on Calculation Type entities.
/// Handles database interactions and business logic for Calculation Type management.
/// </param>
/// <remarks>
/// This controller is versioned using API versioning (version 1.0) and follows RESTful conventions.
/// All endpoints are prefixed with "api/v{version}/CalculationTypes".
/// Supports the following operations:
/// - GET: Retrieve all Calculation Types or a specific Calculation Type by ID
/// - POST: Create a new Calculation Type
/// - PUT: Update an existing Calculation Type
/// - DELETE: Remove a Calculation Type by ID
/// </remarks>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class CalculationTypesController(ILogger<CalculationTypesController> logger, ICalculationTypesCrud crud) : ControllerBase
{
	private readonly ILogger<CalculationTypesController> _logger = logger;
	private readonly ICalculationTypesCrud _crud = crud;

	/// <summary>
	/// Retrieves all Calculation Types.
	/// </summary>
	/// <returns>
	/// An <see cref="ActionResult"/> containing a list of all <see cref="CalculationTypeFullModel"/> objects.
	/// Returns 200 OK with the list of Calculation Types on success.
	/// Returns 500 Internal Server Error if an error occurs during retrieval.
	/// </returns>
	/// <response code="200">Returns the list of Calculation Types.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET: api/v1/CalculationTypes
	[HttpGet()]
	public async Task<ActionResult<List<CalculationTypeFullModel>>> GetAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(GetAsync));
			}

			List<CalculationTypeFullModel> calculationTypes = await _crud.GetAllCalculationTypesAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(GetAsync), calculationTypes.Count, nameof(CalculationTypeFullModel));
			}

			return Ok(calculationTypes);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(GetAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a specific Calculation Type by its ID.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type.</param>
	/// <returns>
	/// An <see cref="ActionResult"/> containing the requested <see cref="CalculationTypeFullModel"/>.
	/// Returns 200 OK with the Calculation Type on success.
	/// Returns 404 Not Found if the Calculation Type is not found.
	/// Returns 500 Internal Server Error if an error occurs.
	/// </returns>
	/// <response code="200">Returns the requested Calculation Type.</response>
	/// <response code="404">If no Calculation Type exists with the specified ID.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET api/v1/CalculationTypes/5
	[HttpGet("{id}")]
	public async Task<ActionResult<CalculationTypeFullModel>> GetAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(GetAsync), id);
			}

			CalculationTypeFullModel? calculationType = await _crud.GetCalculationTypeByIdAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(GetAsync), id, nameof(CalculationTypeFullModel), calculationType);
			}

			return calculationType is null ? (ActionResult<CalculationTypeFullModel>)NotFound($"No Calculation Type exists with the supplied Id {id}.") : (ActionResult<CalculationTypeFullModel>)Ok(calculationType);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(GetAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Creates a new Calculation Type.
	/// </summary>
	/// <param name="model">The Calculation Type model to create.</param>
	/// <returns>
	/// An <see cref="ActionResult"/> containing the newly created <see cref="CalculationTypeFullModel"/>.
	/// Returns 201 Created with the created Calculation Type on success.
	/// Returns 500 Internal Server Error if an error occurs during creation.
	/// </returns>
	/// <response code="201">Returns the newly created Calculation Type.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// POST api/v1/CalculationTypes
	[HttpPost]
	public async Task<ActionResult<CalculationTypeFullModel>> PostAsync([FromBody] CalculationTypeAPIModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(PostAsync), nameof(CalculationTypeAPIModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid)
			{
				CalculationTypeFullModel calculationType = await _crud.CreateNewCalculationTypeAsync(model.ToFullModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(PostAsync), nameof(CalculationTypeFullModel), calculationType);
				}

				return CreatedAtAction(nameof(PostAsync), RouteData.Values, calculationType);
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
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(PostAsync), nameof(CalculationTypeAPIModel), model, sb.ToString());
				}

				return BadRequest(new ValidationProblemDetails(ModelState));
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(PostAsync), nameof(CalculationTypeAPIModel), model);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Updates an existing Calculation Type.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to update.</param>
	/// <param name="model">The updated Calculation Type model.</param>
	/// <returns>
	/// An <see cref="ActionResult"/> containing the updated <see cref="CalculationTypeFullModel"/>.
	/// Returns 200 OK with the updated Calculation Type on success.
	/// Returns 400 Bad Request if the ID doesn't match the model ID.
	/// Returns 500 Internal Server Error if an error occurs.
	/// </returns>
	/// <response code="200">Returns the updated Calculation Type.</response>
	/// <response code="400">If the route parameter ID does not match the model ID.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// PUT api/V1/CalculationTypes/5
	[HttpPut("{id}")]
	public async Task<ActionResult<CalculationTypeFullModel>> PutAsync(int id, [FromBody] CalculationTypeAPIModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(PutAsync), id, nameof(CalculationTypeAPIModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid && id == model.Id)
			{
				CalculationTypeFullModel calculationType = await _crud.UpdateCalculationTypeAsync(model.ToFullModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(PutAsync), id, nameof(CalculationTypeFullModel), calculationType);
				}

				return Ok(calculationType);
			}
			else
			{
				if (id != model.Id)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(PutAsync), id, nameof(CalculationTypeAPIModel), model);
					}

					return BadRequest($"The route parameter Id {id} does not match the Model Id {model?.Id} from the request body.");
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
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(PutAsync), id, nameof(CalculationTypeAPIModel), model, sb.ToString());
					}

					return BadRequest(new ValidationProblemDetails(ModelState));
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(PutAsync), id, nameof(CalculationTypeAPIModel), model);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Deletes a Calculation Type by its ID.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to delete.</param>
	/// <returns>
	/// An <see cref="ActionResult"/> indicating the result of the delete operation.
	/// Returns 200 OK on successful deletion.
	/// Returns 500 Internal Server Error if an error occurs during deletion.
	/// </returns>
	/// <response code="200">If the Calculation Type was successfully deleted.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// DELETE api/V1/CalculationTypes/5
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DeleteAsync), id);
			}

			await _crud.DeleteCalculationTypeAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DeleteAsync), id);
			}

			return Ok();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(CalculationTypesController), nameof(DeleteAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}
}
