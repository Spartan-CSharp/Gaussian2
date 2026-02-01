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
/// API controller for managing base methods.
/// Provides CRUD operations for base method resources with support for full and simple model representations.
/// </summary>
/// <remarks>
/// This controller implements version 1.0 of the base methods API and supports the following operations:
/// - Retrieving all base methods in full or simple format
/// - Retrieving base methods by method family
/// - Retrieving a single base method by ID
/// - Creating new base methods
/// - Updating existing base methods
/// - Deleting base methods
/// </remarks>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class BaseMethodsController(ILogger<BaseMethodsController> logger, IBaseMethodsCrud crud) : ControllerBase
{
	private readonly ILogger<BaseMethodsController> _logger = logger;
	private readonly IBaseMethodsCrud _crud = crud;

	/// <summary>
	/// Retrieves all base methods with full details.
	/// </summary>
	/// <returns>A list of all base methods with complete information.</returns>
	/// <response code="200">Returns the list of base methods successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/BaseMethods
	[HttpGet()]
	public async Task<ActionResult<List<BaseMethodFullModel>>> GetFullAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(GetFullAsync));
			}

			List<BaseMethodFullModel> baseMethods = await _crud.GetAllFullBaseMethodsAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(GetFullAsync), baseMethods.Count, nameof(BaseMethodFullModel));
			}

			return Ok(baseMethods);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(GetFullAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all base methods with intermediate details.
	/// </summary>
	/// <returns>A list of all base methods with method family records instead of full method family models.</returns>
	/// <response code="200">Returns the list of base methods successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/BaseMethods/Intermediate
	[HttpGet("Intermediate")]
	public async Task<ActionResult<List<BaseMethodIntermediateModel>>> GetIntermediateAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(GetIntermediateAsync));
			}

			List<BaseMethodIntermediateModel> baseMethods = await _crud.GetAllIntermediateBaseMethodsAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(GetIntermediateAsync), baseMethods.Count, nameof(BaseMethodIntermediateModel));
			}

			return Ok(baseMethods);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(GetIntermediateAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all base methods with simplified details.
	/// </summary>
	/// <returns>A list of all base methods with basic information.</returns>
	/// <response code="200">Returns the list of base methods successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/BaseMethods/Simple
	[HttpGet("Simple")]
	public async Task<ActionResult<List<BaseMethodSimpleModel>>> GetSimpleAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(GetSimpleAsync));
			}

			List<BaseMethodSimpleModel> baseMethods = await _crud.GetAllSimpleBaseMethodsAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(GetSimpleAsync), baseMethods.Count, nameof(BaseMethodSimpleModel));
			}

			return Ok(baseMethods);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(GetSimpleAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all base methods belonging to a specific method family.
	/// </summary>
	/// <param name="methodFamilyId">The unique identifier of the method family.</param>
	/// <returns>A list of base methods associated with the specified method family.</returns>
	/// <response code="200">Returns the list of base methods successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/BaseMethods/Family
	[HttpGet("Family")]
	public async Task<ActionResult<List<BaseMethodFullModel>>> GetByFamilyAsync([FromQuery(Name = "methodFamilyId")] int methodFamilyId)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with query parameter methodFamilyId = {MethodFamilyId}.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(GetByFamilyAsync), methodFamilyId);
			}

			List<BaseMethodFullModel> baseMethods = await _crud.GetBaseMethodsByMethodFamilyIdAsync(methodFamilyId).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model} with methodFamilyId = {MethodFamilyId}.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(GetByFamilyAsync), baseMethods.Count, nameof(BaseMethodFullModel), methodFamilyId);
			}

			return Ok(baseMethods);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with query parameter methodFamilyId = {MethodFamilyId} had an error", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(GetByFamilyAsync), methodFamilyId);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a single base method by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the base method.</param>
	/// <returns>The base method with full details if found.</returns>
	/// <response code="200">Returns the base method successfully.</response>
	/// <response code="404">No base method exists with the specified ID.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET api/v1/BaseMethods/5
	[HttpGet("{id}")]
	public async Task<ActionResult<BaseMethodFullModel>> GetAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(GetAsync), id);
			}

			BaseMethodFullModel? baseMethod = await _crud.GetBaseMethodByIdAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(GetAsync), id, nameof(BaseMethodFullModel), baseMethod);
			}

			return baseMethod is null ? (ActionResult<BaseMethodFullModel>)NotFound($"No Base Method exists with the supplied Id {id}.") : (ActionResult<BaseMethodFullModel>)Ok(baseMethod);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(GetAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Creates a new base method.
	/// </summary>
	/// <param name="model">The base method model containing the data for the new base method.</param>
	/// <returns>The newly created base method with full details.</returns>
	/// <response code="201">Returns the newly created base method.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// POST api/v1/BaseMethods
	[HttpPost]
	public async Task<ActionResult<BaseMethodFullModel>> PostAsync([FromBody] BaseMethodAPIModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(PostAsync), nameof(BaseMethodAPIModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid)
			{
				BaseMethodFullModel baseMethod = await _crud.CreateNewBaseMethodAsync(model.ToSimpleModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(PostAsync), nameof(BaseMethodFullModel), baseMethod);
				}

				return CreatedAtAction(nameof(PostAsync), RouteData.Values, baseMethod);
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
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(PostAsync), nameof(BaseMethodAPIModel), model, sb.ToString());
				}

				return BadRequest(new ValidationProblemDetails(ModelState));
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(PostAsync), nameof(BaseMethodAPIModel), model);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Updates an existing base method.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to update.</param>
	/// <param name="model">The base method model containing the updated data.</param>
	/// <returns>The updated base method with full details.</returns>
	/// <response code="200">Returns the updated base method successfully.</response>
	/// <response code="400">The route parameter ID does not match the model ID.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// PUT api/v1/BaseMethods/5
	[HttpPut("{id}")]
	public async Task<ActionResult<BaseMethodFullModel>> PutAsync(int id, [FromBody] BaseMethodAPIModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(PutAsync), id, nameof(BaseMethodAPIModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid && id == model.Id)
			{
				BaseMethodFullModel baseMethod = await _crud.UpdateBaseMethodAsync(model.ToSimpleModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(PutAsync), id, nameof(BaseMethodFullModel), baseMethod);
				}

				return Ok(baseMethod);
			}
			else
			{
				if (id != model.Id)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(PutAsync), id, nameof(BaseMethodAPIModel), model);
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
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(PutAsync), id, nameof(BaseMethodAPIModel), model, sb.ToString());
					}

					return BadRequest(new ValidationProblemDetails(ModelState));
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(PutAsync), id, nameof(BaseMethodAPIModel), model);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Deletes a base method by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to delete.</param>
	/// <returns>An action result indicating the success of the operation.</returns>
	/// <response code="200">The base method was deleted successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// DELETE api/v1/BaseMethods/5
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DeleteAsync), id);
			}

			await _crud.DeleteBaseMethodAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DeleteAsync), id);
			}

			return Ok();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(BaseMethodsController), nameof(DeleteAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}
}
