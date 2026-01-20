using Asp.Versioning;

using GaussianCommonLibrary.Models;

using GaussianMVCLibrary.DataAccess;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GaussianMVC.Controllers.APIControllers.V1;

/// <summary>
/// API controller for managing calculation types.
/// Provides CRUD operations for calculation type resources via RESTful endpoints.
/// </summary>
/// <param name="logger">
/// Logger instance for tracking controller operations, errors, and debugging information.
/// Supports structured logging with various log levels (Debug, Trace, Error).
/// </param>
/// <param name="crud">
/// Data access layer service for performing CRUD operations on calculation type entities.
/// Handles database interactions and business logic for calculation type management.
/// </param>
/// <remarks>
/// This controller is versioned using API versioning (version 1.0) and follows RESTful conventions.
/// All endpoints are prefixed with "api/v{version}/CalculationTypes".
/// Supports the following operations:
/// - GET: Retrieve all calculation types or a specific calculation type by ID
/// - POST: Create a new calculation type
/// - PUT: Update an existing calculation type
/// - DELETE: Remove a calculation type by ID
/// </remarks>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class CalculationTypesController(ILogger<CalculationTypesController> logger, ICalculationTypesCrud crud) : ControllerBase
{
	private readonly ILogger<CalculationTypesController> _logger = logger;
	private readonly ICalculationTypesCrud _crud = crud;

	/// <summary>
	/// Retrieves all calculation types.
	/// </summary>
	/// <returns>
	/// An <see cref="ActionResult"/> containing a list of all <see cref="CalculationTypeFullModel"/> objects.
	/// Returns 200 OK with the list of calculation types on success.
	/// Returns 500 Internal Server Error if an error occurs during retrieval.
	/// </returns>
	/// <response code="200">Returns the list of calculation types.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET: api/v1/CalculationTypes
	[HttpGet(Name = "GetCalculationTypes")]
	public async Task<ActionResult<List<CalculationTypeFullModel>>> GetAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action}", nameof(CalculationTypesController), nameof(GetAsync));
			}

			List<CalculationTypeFullModel> calculationTypes = await _crud.GetAllCalculationTypesAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Controller} {Action} {Method} returned {Count}", nameof(CalculationTypesController), nameof(GetAsync), nameof(_crud.GetAllCalculationTypesAsync), calculationTypes.Count);
			}

			return Ok(calculationTypes);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} had an error", nameof(CalculationTypesController), nameof(GetAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a specific calculation type by its ID.
	/// </summary>
	/// <param name="id">The unique identifier of the calculation type.</param>
	/// <returns>
	/// An <see cref="ActionResult"/> containing the requested <see cref="CalculationTypeFullModel"/>.
	/// Returns 200 OK with the calculation type on success.
	/// Returns 400 Bad Request if the calculation type is not found.
	/// Returns 500 Internal Server Error if an error occurs.
	/// </returns>
	/// <response code="200">Returns the requested calculation type.</response>
	/// <response code="400">If no calculation type exists with the specified ID.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET api/v1/CalculationTypes/5
	[HttpGet("{id}")]
	public async Task<ActionResult<CalculationTypeFullModel>> GetAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action} {Id}", nameof(CalculationTypesController), nameof(GetAsync), id);
			}

			CalculationTypeFullModel? calculationType = await _crud.GetCalculationTypeByIdAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Controller} {Action} {Id} {Method} returned {Model}", nameof(CalculationTypesController), nameof(GetAsync), id, nameof(_crud.GetCalculationTypeByIdAsync), calculationType);
			}

			return calculationType is null ? (ActionResult<CalculationTypeFullModel>)BadRequest($"No Calculation Type exists with the supplied Id {id}.") : (ActionResult<CalculationTypeFullModel>)Ok(calculationType);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} had an error", nameof(CalculationTypesController), nameof(GetAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Creates a new calculation type.
	/// </summary>
	/// <param name="model">The calculation type model to create.</param>
	/// <returns>
	/// An <see cref="ActionResult"/> containing the newly created <see cref="CalculationTypeFullModel"/>.
	/// Returns 201 Created with the created calculation type on success.
	/// Returns 500 Internal Server Error if an error occurs during creation.
	/// </returns>
	/// <response code="201">Returns the newly created calculation type.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// POST api/v1/CalculationTypes
	[HttpPost]
	public async Task<ActionResult<CalculationTypeFullModel>> PostAsync([FromBody] CalculationTypeFullModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action} {Model}", nameof(CalculationTypesController), nameof(PostAsync), model);
			}

			CalculationTypeFullModel calculationType = await _crud.CreateNewCalculationTypeAsync(model).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Controller} {Action} {Model} {Method} returned {Model}", nameof(CalculationTypesController), nameof(PostAsync), model, nameof(_crud.CreateNewCalculationTypeAsync), calculationType);
			}

			return CreatedAtAction(nameof(PostAsync), RouteData.Values, calculationType);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} had an error", nameof(CalculationTypesController), nameof(PostAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Updates an existing calculation type.
	/// </summary>
	/// <param name="id">The unique identifier of the calculation type to update.</param>
	/// <param name="model">The updated calculation type model.</param>
	/// <returns>
	/// An <see cref="ActionResult"/> containing the updated <see cref="CalculationTypeFullModel"/>.
	/// Returns 200 OK with the updated calculation type on success.
	/// Returns 400 Bad Request if the ID doesn't match the model ID.
	/// Returns 500 Internal Server Error if an error occurs.
	/// </returns>
	/// <response code="200">Returns the updated calculation type.</response>
	/// <response code="400">If the route parameter ID does not match the model ID.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// PUT api/V1/CalculationTypes/5
	[HttpPut("{id}")]
	public async Task<ActionResult<CalculationTypeFullModel>> PutAsync(int id, [FromBody] CalculationTypeFullModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action} {Id} {Model}", nameof(CalculationTypesController), nameof(PutAsync), id, model);
			}

			if (id == model?.Id)
			{
				CalculationTypeFullModel calculationType = await _crud.UpdateCalculationTypeAsync(model).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Trace))
				{
					_logger.LogTrace("{Controller} {Action} {Id} {Model} {Method} returned {Model}", nameof(CalculationTypesController), nameof(PutAsync), id, model, nameof(_crud.UpdateCalculationTypeAsync), calculationType);
				}

				return Ok(calculationType);
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Trace))
				{
					_logger.LogTrace("{Controller} {Action} {Id} {Model} route parameter Id does not match the model ID", nameof(CalculationTypesController), nameof(PutAsync), id, model);
				}

				return BadRequest($"The route parameter ID {id} does not match the model ID {model?.Id} from the request body.");
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} had an error", nameof(CalculationTypesController), nameof(PutAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Deletes a calculation type by its ID.
	/// </summary>
	/// <param name="id">The unique identifier of the calculation type to delete.</param>
	/// <returns>
	/// An <see cref="ActionResult"/> indicating the result of the delete operation.
	/// Returns 200 OK on successful deletion.
	/// Returns 500 Internal Server Error if an error occurs during deletion.
	/// </returns>
	/// <response code="200">If the calculation type was successfully deleted.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// DELETE api/V1/CalculationTypes/5
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action} {Id}", nameof(CalculationTypesController), nameof(DeleteAsync), id);
			}

			await _crud.DeleteCalculationTypeAsync(id).ConfigureAwait(false);
			return Ok();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} had an error", nameof(CalculationTypesController), nameof(DeleteAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}
}
