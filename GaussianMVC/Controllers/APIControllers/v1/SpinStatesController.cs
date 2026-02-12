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
/// API controller for managing Spin States with CRUD operations.
/// Provides RESTful endpoints for retrieving, creating, updating, and deleting Spin State records.
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class SpinStatesController(ILogger<SpinStatesController> logger, ISpinStatesCrud crud) : ControllerBase
{
	private readonly ILogger<SpinStatesController> _logger = logger;
	private readonly ISpinStatesCrud _crud = crud;

	/// <summary>
	/// Retrieves all Spin States from the database.
	/// </summary>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing a list of <see cref="SpinStateFullModel"/> objects.
	/// Returns 200 OK with the list of Spin States, or 500 Internal Server Error if an exception occurs.
	/// </returns>
	/// <response code="200">Returns the list of all Spin States.</response>
	/// <response code="500">An validationError occurred while retrieving the Spin States.</response>
	// GET: api/v1/SpinStates
	[HttpGet()]
	public async Task<ActionResult<List<SpinStateFullModel>>> GetAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(GetAsync));
			}

			List<SpinStateFullModel> spinStates = await _crud.GetAllSpinStatesAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(GetAsync), spinStates.Count, nameof(SpinStateFullModel));
			}

			return Ok(spinStates);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(GetAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a simplified list of Spin States.
	/// </summary>
	/// <returns>
	/// An <see cref="ActionResult"/> containing a list of <see cref="ElectronicStateRecord"/> objects.
	/// Returns 200 OK with the list of Spin State records on success.
	/// Returns 500 Internal Server Error if an error occurs during retrieval.
	/// </returns>
	/// <response code="200">Returns the list of Spin State records.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET: api/v1/SpinStates/List
	[HttpGet("List")]
	public async Task<ActionResult<List<SpinStateRecord>>> GetListAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(GetListAsync));
			}

			List<SpinStateRecord> spinStates = await _crud.GetSpinStateListAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model}", HttpContext.Request.Method, nameof(SpinStatesController), nameof(GetListAsync), spinStates.Count, nameof(SpinStateRecord));
			}

			return Ok(spinStates);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(GetListAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a specific Spin State by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State to retrieve.</param>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing the <see cref="SpinStateFullModel"/> if found.
	/// Returns 200 OK with the Spin State, 404 Not Found if the Spin State doesn't exist, 
	/// or 500 Internal Server Error if an exception occurs.
	/// </returns>
	/// <response code="200">Returns the requested Spin State.</response>
	/// <response code="404">No Spin State was found with the specified ID.</response>
	/// <response code="500">An validationError occurred while retrieving the Spin State.</response>
	// GET api/v1/SpinStates/5
	[HttpGet("{id}")]
	public async Task<ActionResult<SpinStateFullModel>> GetAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(GetAsync), id);
			}

			SpinStateFullModel? spinState = await _crud.GetSpinStateByIdAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(GetAsync), id, nameof(SpinStateFullModel), spinState);
			}

			return spinState is null ? (ActionResult<SpinStateFullModel>)NotFound($"No Spin State exists with the supplied Id {id}.") : (ActionResult<SpinStateFullModel>)Ok(spinState);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(GetAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Creates a new Spin State in the database.
	/// </summary>
	/// <param name="model">The <see cref="SpinStateFullModel"/> containing the data for the new Spin State.</param>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing the created <see cref="SpinStateFullModel"/>.
	/// Returns 201 Created with the new Spin State and location header, 
	/// or 500 Internal Server Error if an exception occurs.
	/// </returns>
	/// <response code="201">Returns the newly created Spin State.</response>
	/// <response code="500">An validationError occurred while creating the Spin State.</response>
	// POST api/v1/SpinStates
	[HttpPost]
	public async Task<ActionResult<SpinStateFullModel>> PostAsync([FromBody] SpinStateAPIModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(PostAsync), nameof(SpinStateAPIModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid)
			{
				SpinStateFullModel spinState = await _crud.CreateNewSpinStateAsync(model.ToFullModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(PostAsync), nameof(SpinStateFullModel), spinState);
				}

				return CreatedAtAction(nameof(PostAsync), RouteData.Values, spinState);
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
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(SpinStatesController), nameof(PostAsync), nameof(SpinStateAPIModel), model, sb.ToString());
				}

				return BadRequest(new ValidationProblemDetails(ModelState));
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(PostAsync), nameof(SpinStateAPIModel), model);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Updates an existing Spin State in the database.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State to update. Must match the ID in the model.</param>
	/// <param name="model">The <see cref="SpinStateAPIModel"/> containing the updated data.</param>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing the updated <see cref="SpinStateFullModel"/>.
	/// Returns 200 OK with the updated Spin State, 400 Bad Request if the route ID doesn't match the model ID,
	/// or 500 Internal Server Error if an exception occurs.
	/// </returns>
	/// <response code="200">Returns the updated Spin State.</response>
	/// <response code="400">The route parameter ID does not match the model ID.</response>
	/// <response code="500">An validationError occurred while updating the Spin State.</response>
	// PUT api/V1/SpinStates/5
	[HttpPut("{id}")]
	public async Task<ActionResult<SpinStateFullModel>> PutAsync(int id, [FromBody] SpinStateAPIModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(PutAsync), id, nameof(SpinStateAPIModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid && id == model.Id)
			{
				SpinStateFullModel spinState = await _crud.UpdateSpinStateAsync(model.ToFullModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(PutAsync), id, nameof(SpinStateFullModel), spinState);
				}

				return Ok(spinState);
			}
			else
			{
				if (id != model.Id)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(PutAsync), id, nameof(SpinStateAPIModel), model);
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
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(SpinStatesController), nameof(PutAsync), id, nameof(SpinStateAPIModel), model, sb.ToString());
					}

					return BadRequest(new ValidationProblemDetails(ModelState));
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(PutAsync), id, nameof(SpinStateAPIModel), model);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Deletes an Spin State from the database.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State to delete.</param>
	/// <returns>
	/// An <see cref="ActionResult"/> indicating the result of the delete operation.
	/// Returns 200 OK if the deletion was successful, 
	/// or 500 Internal Server Error if an exception occurs.
	/// </returns>
	/// <response code="200">The Spin State was successfully deleted.</response>
	/// <response code="500">An validationError occurred while deleting the Spin State.</response>
	// DELETE api/V1/SpinStates/5
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(DeleteAsync), id);
			}

			await _crud.DeleteSpinStateAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(DeleteAsync), id);
			}

			return Ok();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(SpinStatesController), nameof(DeleteAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}
}
