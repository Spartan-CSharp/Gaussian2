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
/// API controller for managing Method Families.
/// Provides CRUD operations for Method Family resources via RESTful endpoints.
/// </summary>
/// <param name="logger">
/// Logger instance for tracking controller operations, errors, and debugging information.
/// Supports structured logging with various log levels (Debug, Debug, Error).
/// </param>
/// <param name="crud">
/// Data access layer service for performing CRUD operations on Method Family entities.
/// Handles database interactions and business logic for Method Family management.
/// </param>
/// <remarks>
/// This controller is versioned using API versioning (version 1.0) and follows RESTful conventions.
/// All endpoints are prefixed with "api/v{version}/MethodFamilies".
/// Supports the following operations:
/// - GET: Retrieve all Method Families or a specific Method Family by ID
/// - POST: Create a new Method Family
/// - PUT: Update an existing Method Family
/// - DELETE: Remove a Method Family by ID
/// </remarks>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class MethodFamiliesController(ILogger<MethodFamiliesController> logger, IMethodFamiliesCrud crud) : ControllerBase
{
	private readonly ILogger<MethodFamiliesController> _logger = logger;
	private readonly IMethodFamiliesCrud _crud = crud;

	/// <summary>
	/// Retrieves all Method Families.
	/// </summary>
	/// <returns>
	/// An <see cref="ActionResult"/> containing a list of all <see cref="MethodFamilyFullModel"/> objects.
	/// Returns 200 OK with the list of Method Families on success.
	/// Returns 500 Internal Server Error if an error occurs during retrieval.
	/// </returns>
	/// <response code="200">Returns the list of Method Families.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET: api/v1/MethodFamilies
	[HttpGet()]
	public async Task<ActionResult<List<MethodFamilyFullModel>>> GetAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(GetAsync));
			}

			List<MethodFamilyFullModel> methodFamilies = await _crud.GetAllMethodFamiliesAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(GetAsync), methodFamilies.Count, nameof(MethodFamilyFullModel));
			}

			return Ok(methodFamilies);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(GetAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a simplified list of Method Families.
	/// </summary>
	/// <returns>
	/// An <see cref="ActionResult"/> containing a list of <see cref="MethodFamilyRecord"/> objects.
	/// Returns 200 OK with the list of Method Family records on success.
	/// Returns 500 Internal Server Error if an error occurs during retrieval.
	/// </returns>
	/// <response code="200">Returns the list of Method Family records.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET: api/v1/MethodFamilies/List
	[HttpGet("List")]
	public async Task<ActionResult<List<MethodFamilyRecord>>> GetListAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(GetListAsync));
			}

			List<MethodFamilyRecord> methodFamilies = await _crud.GetMethodFamilyListAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model}", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(GetListAsync), methodFamilies.Count, nameof(MethodFamilyRecord));
			}

			return Ok(methodFamilies);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(GetListAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a specific Method Family by its ID.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family.</param>
	/// <returns>
	/// An <see cref="ActionResult"/> containing the requested <see cref="MethodFamilyFullModel"/>.
	/// Returns 200 OK with the Method Family on success.
	/// Returns 404 Not Found if the Method Family is not found.
	/// Returns 500 Internal Server Error if an error occurs.
	/// </returns>
	/// <response code="200">Returns the requested Method Family.</response>
	/// <response code="404">If no Method Family exists with the specified ID.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET api/v1/MethodFamilies/5
	[HttpGet("{id}")]
	public async Task<ActionResult<MethodFamilyFullModel>> GetAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(GetAsync), id);
			}

			MethodFamilyFullModel? methodFamily = await _crud.GetMethodFamilyByIdAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(GetAsync), id, nameof(MethodFamilyFullModel), methodFamily);
			}

			return methodFamily is null ? (ActionResult<MethodFamilyFullModel>)NotFound($"No Method Family exists with the supplied Id {id}.") : (ActionResult<MethodFamilyFullModel>)Ok(methodFamily);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(GetAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Creates a new Method Family.
	/// </summary>
	/// <param name="model">The Method Family model to create.</param>
	/// <returns>
	/// An <see cref="ActionResult"/> containing the newly created <see cref="MethodFamilyFullModel"/>.
	/// Returns 201 Created with the created Method Family on success.
	/// Returns 500 Internal Server Error if an error occurs during creation.
	/// </returns>
	/// <response code="201">Returns the newly created Method Family.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// POST api/v1/MethodFamilies
	[HttpPost]
	public async Task<ActionResult<MethodFamilyFullModel>> PostAsync([FromBody] MethodFamilyAPIModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(PostAsync), nameof(MethodFamilyAPIModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid)
			{
				MethodFamilyFullModel methodFamily = await _crud.CreateNewMethodFamilyAsync(model.ToFullModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(PostAsync), nameof(MethodFamilyFullModel), methodFamily);
				}

				return CreatedAtAction(nameof(PostAsync), RouteData.Values, methodFamily);
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
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(PostAsync), nameof(MethodFamilyAPIModel), model, sb.ToString());
				}

				return BadRequest(new ValidationProblemDetails(ModelState));
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(PostAsync), nameof(MethodFamilyAPIModel), model);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Updates an existing Method Family.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to update.</param>
	/// <param name="model">The updated Method Family model.</param>
	/// <returns>
	/// An <see cref="ActionResult"/> containing the updated <see cref="MethodFamilyFullModel"/>.
	/// Returns 200 OK with the updated Method Family on success.
	/// Returns 400 Bad Request if the ID doesn't match the model ID.
	/// Returns 500 Internal Server Error if an error occurs.
	/// </returns>
	/// <response code="200">Returns the updated Method Family.</response>
	/// <response code="400">If the route parameter ID does not match the model ID.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// PUT api/V1/MethodFamilies/5
	[HttpPut("{id}")]
	public async Task<ActionResult<MethodFamilyFullModel>> PutAsync(int id, [FromBody] MethodFamilyAPIModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(PutAsync), id, nameof(MethodFamilyAPIModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid && id == model.Id)
			{
				MethodFamilyFullModel methodFamily = await _crud.UpdateMethodFamilyAsync(model.ToFullModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(PutAsync), id, nameof(MethodFamilyFullModel), methodFamily);
				}

				return Ok(methodFamily);
			}
			else
			{
				if (id != model.Id)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(PutAsync), id, nameof(MethodFamilyAPIModel), model);
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
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(PutAsync), id, nameof(MethodFamilyAPIModel), model, sb.ToString());
					}

					return BadRequest(new ValidationProblemDetails(ModelState));
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(PutAsync), id, nameof(MethodFamilyAPIModel), model);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Deletes a Method Family by its ID.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to delete.</param>
	/// <returns>
	/// An <see cref="ActionResult"/> indicating the result of the delete operation.
	/// Returns 200 OK on successful deletion.
	/// Returns 500 Internal Server Error if an error occurs during deletion.
	/// </returns>
	/// <response code="200">If the Method Family was successfully deleted.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// DELETE api/V1/MethodFamilies/5
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(DeleteAsync), id);
			}

			await _crud.DeleteMethodFamilyAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(DeleteAsync), id);
			}

			return Ok();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(MethodFamiliesController), nameof(DeleteAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}
}
