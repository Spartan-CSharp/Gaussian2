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
/// API controller for managing Electronic States with CRUD operations.
/// Provides RESTful endpoints for retrieving, creating, updating, and deleting Electronic State records.
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class ElectronicStatesController(ILogger<ElectronicStatesController> logger, IElectronicStatesCrud crud) : ControllerBase
{
	private readonly ILogger<ElectronicStatesController> _logger = logger;
	private readonly IElectronicStatesCrud _crud = crud;

	/// <summary>
	/// Retrieves all Electronic States from the database.
	/// </summary>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing a list of <see cref="ElectronicStateFullModel"/> objects.
	/// Returns 200 OK with the list of Electronic States, or 500 Internal Server Error if an exception occurs.
	/// </returns>
	/// <response code="200">Returns the list of all Electronic States.</response>
	/// <response code="500">An validationError occurred while retrieving the Electronic States.</response>
	// GET: api/v1/ElectronicStates
	[HttpGet()]
	public async Task<ActionResult<List<ElectronicStateFullModel>>> GetAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(GetAsync));
			}

			List<ElectronicStateFullModel> electronicStates = await _crud.GetAllElectronicStatesAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(GetAsync), electronicStates.Count, nameof(ElectronicStateFullModel));
			}

			return Ok(electronicStates);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(GetAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a simplified list of Electronic States.
	/// </summary>
	/// <returns>
	/// An <see cref="ActionResult"/> containing a list of <see cref="ElectronicStateRecord"/> objects.
	/// Returns 200 OK with the list of Electronic State records on success.
	/// Returns 500 Internal Server Error if an error occurs during retrieval.
	/// </returns>
	/// <response code="200">Returns the list of Electronic State records.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET: api/v1/ElectronicStates/List
	[HttpGet("List")]
	public async Task<ActionResult<List<ElectronicStateRecord>>> GetListAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(GetListAsync));
			}

			List<ElectronicStateRecord> electronicStates = await _crud.GetElectronicStateListAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model}", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(GetListAsync), electronicStates.Count, nameof(ElectronicStateRecord));
			}

			return Ok(electronicStates);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(GetListAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a specific Electronic State by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State to retrieve.</param>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing the <see cref="ElectronicStateFullModel"/> if found.
	/// Returns 200 OK with the Electronic State, 404 Not Found if the Electronic State doesn't exist, 
	/// or 500 Internal Server Error if an exception occurs.
	/// </returns>
	/// <response code="200">Returns the requested Electronic State.</response>
	/// <response code="404">No Electronic State was found with the specified ID.</response>
	/// <response code="500">An validationError occurred while retrieving the Electronic State.</response>
	// GET api/v1/ElectronicStates/5
	[HttpGet("{id}")]
	public async Task<ActionResult<ElectronicStateFullModel>> GetAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(GetAsync), id);
			}

			ElectronicStateFullModel? electronicState = await _crud.GetElectronicStateByIdAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(GetAsync), id, nameof(ElectronicStateFullModel), electronicState);
			}

			return electronicState is null ? (ActionResult<ElectronicStateFullModel>)NotFound($"No Electronic State exists with the supplied Id {id}.") : (ActionResult<ElectronicStateFullModel>)Ok(electronicState);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(GetAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Creates a new Electronic State in the database.
	/// </summary>
	/// <param name="model">The <see cref="ElectronicStateFullModel"/> containing the data for the new Electronic State.</param>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing the created <see cref="ElectronicStateFullModel"/>.
	/// Returns 201 Created with the new Electronic State and location header, 
	/// or 500 Internal Server Error if an exception occurs.
	/// </returns>
	/// <response code="201">Returns the newly created Electronic State.</response>
	/// <response code="500">An validationError occurred while creating the Electronic State.</response>
	// POST api/v1/ElectronicStates
	[HttpPost]
	public async Task<ActionResult<ElectronicStateFullModel>> PostAsync([FromBody] ElectronicStateAPIModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(PostAsync), nameof(ElectronicStateAPIModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid)
			{
				ElectronicStateFullModel electronicState = await _crud.CreateNewElectronicStateAsync(model.ToFullModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(PostAsync), nameof(ElectronicStateFullModel), electronicState);
				}

				return CreatedAtAction(nameof(PostAsync), RouteData.Values, electronicState);
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
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(PostAsync), nameof(ElectronicStateAPIModel), model, sb.ToString());
				}

				return BadRequest(new ValidationProblemDetails(ModelState));
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(PostAsync), nameof(ElectronicStateAPIModel), model);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Updates an existing Electronic State in the database.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State to update. Must match the ID in the model.</param>
	/// <param name="model">The <see cref="ElectronicStateAPIModel"/> containing the updated data.</param>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing the updated <see cref="ElectronicStateFullModel"/>.
	/// Returns 200 OK with the updated Electronic State, 400 Bad Request if the route ID doesn't match the model ID,
	/// or 500 Internal Server Error if an exception occurs.
	/// </returns>
	/// <response code="200">Returns the updated Electronic State.</response>
	/// <response code="400">The route parameter ID does not match the model ID.</response>
	/// <response code="500">An validationError occurred while updating the Electronic State.</response>
	// PUT api/V1/ElectronicStates/5
	[HttpPut("{id}")]
	public async Task<ActionResult<ElectronicStateFullModel>> PutAsync(int id, [FromBody] ElectronicStateAPIModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(PutAsync), id, nameof(ElectronicStateAPIModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid && id == model.Id)
			{
				ElectronicStateFullModel electronicState = await _crud.UpdateElectronicStateAsync(model.ToFullModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(PutAsync), id, nameof(ElectronicStateFullModel), electronicState);
				}

				return Ok(electronicState);
			}
			else
			{
				if (id != model.Id)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(PutAsync), id, nameof(ElectronicStateAPIModel), model);
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
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(PutAsync), id, nameof(ElectronicStateAPIModel), model, sb.ToString());
					}

					return BadRequest(new ValidationProblemDetails(ModelState));
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(PutAsync), id, nameof(ElectronicStateAPIModel), model);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Deletes an Electronic State from the database.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State to delete.</param>
	/// <returns>
	/// An <see cref="ActionResult"/> indicating the result of the delete operation.
	/// Returns 200 OK if the deletion was successful, 
	/// or 500 Internal Server Error if an exception occurs.
	/// </returns>
	/// <response code="200">The Electronic State was successfully deleted.</response>
	/// <response code="500">An validationError occurred while deleting the Electronic State.</response>
	// DELETE api/V1/ElectronicStates/5
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(DeleteAsync), id);
			}

			await _crud.DeleteElectronicStateAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(DeleteAsync), id);
			}

			return Ok();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesController), nameof(DeleteAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}
}
