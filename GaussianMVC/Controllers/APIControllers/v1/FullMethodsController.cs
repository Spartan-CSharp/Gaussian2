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
/// API controller for managing Full Methods.
/// Provides CRUD operations for Full Method resources with support for full and simple model representations.
/// </summary>
/// <remarks>
/// This controller implements version 1.0 of the Full Methods API and supports the following operations:
/// - Retrieving all Full Methods in full or simple format
/// - Retrieving Full Methods by Spin State/Electronic State/Method Family Combination
/// - Retrieving a single Full Method by ID
/// - Creating new Full Methods
/// - Updating existing Full Methods
/// - Deleting Full Methods
/// </remarks>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class FullMethodsController(ILogger<FullMethodsController> logger, IFullMethodsCrud crud) : ControllerBase
{
	private readonly ILogger<FullMethodsController> _logger = logger;
	private readonly IFullMethodsCrud _crud = crud;

	/// <summary>
	/// Retrieves all Full Methods with full details.
	/// </summary>
	/// <returns>A list of all Full Methods with complete information.</returns>
	/// <response code="200">Returns the list of Full Methods successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/FullMethods
	[HttpGet()]
	public async Task<ActionResult<List<FullMethodFullModel>>> GetFullAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetFullAsync));
			}

			List<FullMethodFullModel> fullMethods = await _crud.GetAllFullFullMethodsAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetFullAsync), fullMethods.Count, nameof(FullMethodFullModel));
			}

			return Ok(fullMethods);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetFullAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all Full Methods with intermediate details.
	/// </summary>
	/// <returns>A list of all Full Methods with Spin State/Electronic State/Method Family Combination records instead of full Spin State/Electronic State/Method Family Combination models.</returns>
	/// <response code="200">Returns the list of Full Methods successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/FullMethods/Intermediate
	[HttpGet("Intermediate")]
	public async Task<ActionResult<List<FullMethodIntermediateModel>>> GetIntermediateAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetIntermediateAsync));
			}

			List<FullMethodIntermediateModel> fullMethods = await _crud.GetAllIntermediateFullMethodsAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetIntermediateAsync), fullMethods.Count, nameof(FullMethodIntermediateModel));
			}

			return Ok(fullMethods);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetIntermediateAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all Full Methods with simplified details.
	/// </summary>
	/// <returns>A list of all Full Methods with basic information.</returns>
	/// <response code="200">Returns the list of Full Methods successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/FullMethods/Simple
	[HttpGet("Simple")]
	public async Task<ActionResult<List<FullMethodSimpleModel>>> GetSimpleAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetSimpleAsync));
			}

			List<FullMethodSimpleModel> fullMethods = await _crud.GetAllSimpleFullMethodsAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model}", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetSimpleAsync), fullMethods.Count, nameof(FullMethodSimpleModel));
			}

			return Ok(fullMethods);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetSimpleAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a simplified list of Full Methods.
	/// </summary>
	/// <returns>
	/// An <see cref="ActionResult"/> containing a list of <see cref="FullMethodRecord"/> objects.
	/// Returns 200 OK with the list of Full Method records on success.
	/// Returns 500 Internal Server Error if an error occurs during retrieval.
	/// </returns>
	/// <response code="200">Returns the list of Full Method records.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET: api/v1/FullMethods/List
	[HttpGet("List")]
	public async Task<ActionResult<List<FullMethodRecord>>> GetListAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetListAsync));
			}

			List<FullMethodRecord> fullMethods = await _crud.GetFullMethodListAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model}", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetListAsync), fullMethods.Count, nameof(FullMethodRecord));
			}

			return Ok(fullMethods);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetListAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all Full Methods belonging to a specific Spin State/Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="spinStateElectronicStateMethodFamilyId">The unique identifier of the Spin State/Electronic State/Method Family Combination.</param>
	/// <returns>A list of Full Methods associated with the specified Spin State/Electronic State/Method Family Combination.</returns>
	/// <response code="200">Returns the list of Full Methods successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/FullMethods/SpinStateElectronicStateMethodFamily?spinStateElectronicStateMethodFamilyId=5
	[HttpGet("SpinStateElectronicStateMethodFamily")]
	public async Task<ActionResult<List<FullMethodFullModel>>> GetBySpinStateElectronicStateMethodFamilyAsync([FromQuery(Name = "spinStateElectronicStateMethodFamilyId")] int spinStateElectronicStateMethodFamilyId)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with query parameter SpinStateElectronicStateMethodFamilyId = {SpinStateElectronicStateMethodFamilyId}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetBySpinStateElectronicStateMethodFamilyAsync), spinStateElectronicStateMethodFamilyId);
			}

			List<FullMethodFullModel> fullMethods = await _crud.GetFullMethodsBySpinStateElectronicStateMethodFamilyIdAsync(spinStateElectronicStateMethodFamilyId).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model} with SpinStateElectronicStateMethodFamilyId = {SpinStateElectronicStateMethodFamilyId}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetBySpinStateElectronicStateMethodFamilyAsync), fullMethods.Count, nameof(FullMethodFullModel), spinStateElectronicStateMethodFamilyId);
			}

			return Ok(fullMethods);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with query parameter SpinStateElectronicStateMethodFamilyId = {SpinStateElectronicStateMethodFamilyId} had an error", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetBySpinStateElectronicStateMethodFamilyAsync), spinStateElectronicStateMethodFamilyId);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all Full Methods belonging to a specific Base Method.
	/// </summary>
	/// <param name="baseMethodId">The unique identifier of the Base Method.</param>
	/// <returns>A list of Full Methods associated with the specified Base Method.</returns>
	/// <response code="200">Returns the list of Full Methods successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/FullMethods/BaseMethod?baseMethodId=5
	[HttpGet("BaseMethod")]
	public async Task<ActionResult<List<FullMethodFullModel>>> GetByBaseMethodAsync([FromQuery(Name = "baseMethodId")] int baseMethodId)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with query parameter baseMethodId = {BaseMethodId}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetByBaseMethodAsync), baseMethodId);
			}

			List<FullMethodFullModel> fullMethods = await _crud.GetFullMethodsByBaseMethodIdAsync(baseMethodId).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model} with baseMethodId = {BaseMethodId}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetByBaseMethodAsync), fullMethods.Count, nameof(FullMethodFullModel), baseMethodId);
			}

			return Ok(fullMethods);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with query parameter baseMethodId = {BaseMethodId} had an error", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetByBaseMethodAsync), baseMethodId);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all Full Methods belonging to a specific Spin State/Electronic State/Method Family Combination and a specific Base Method.
	/// </summary>
	/// <param name="spinStateElectronicStateMethodFamilyId">The unique identifier of the Spin State/Electronic State/Method Family Combination.</param>
	/// <param name="baseMethodId">The unique identifier of the Base Method.</param>
	/// <returns>A list of Full Methods associated with the specified Spin State/Electronic State/Method Family Combination and Base Method.</returns>
	/// <response code="200">Returns the list of Full Methods successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/FullMethods/SpinStateElectronicStateMethodFamilyBaseMethod?spinStateElectronicStateMethodFamilyId=5&baseMethodId=5
	[HttpGet("SpinStateElectronicStateMethodFamilyBaseMethod")]
	public async Task<ActionResult<List<FullMethodFullModel>>> GetBySpinStateElectronicStateMethodFamilyAndBaseMethodAsync([FromQuery(Name = "spinStateElectronicStateMethodFamilyId")] int spinStateElectronicStateMethodFamilyId, [FromQuery(Name = "baseMethodId")] int baseMethodId)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with query parameters SpinStateElectronicStateMethodFamilyId = {SpinStateElectronicStateMethodFamilyId} and BaseMethodId = {BaseMethodId}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetBySpinStateElectronicStateMethodFamilyAndBaseMethodAsync), spinStateElectronicStateMethodFamilyId, baseMethodId);
			}

			List<FullMethodFullModel> fullMethods = await _crud.GetFullMethodsBySpinStateElectronicStateMethodFamilyIdAndBaseMethodIdAsync(spinStateElectronicStateMethodFamilyId, baseMethodId).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model} with SpinStateElectronicStateMethodFamilyId = {SpinStateElectronicStateMethodFamilyId} and BaseMethodId = {BaseMethodId}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetBySpinStateElectronicStateMethodFamilyAndBaseMethodAsync), fullMethods.Count, nameof(FullMethodFullModel), spinStateElectronicStateMethodFamilyId, baseMethodId);
			}

			return Ok(fullMethods);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with query parameters SpinStateElectronicStateMethodFamilyId = {SpinStateElectronicStateMethodFamilyId} and BaseMethodId = {BaseMethodId} had an error", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetBySpinStateElectronicStateMethodFamilyAndBaseMethodAsync), spinStateElectronicStateMethodFamilyId, baseMethodId);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a single Full Method by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Full Method.</param>
	/// <returns>The Full Method with full details if found.</returns>
	/// <response code="200">Returns the Full Method successfully.</response>
	/// <response code="404">No Full Method exists with the specified ID.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET api/v1/FullMethods/5
	[HttpGet("{id}")]
	public async Task<ActionResult<FullMethodFullModel>> GetAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetAsync), id);
			}

			FullMethodFullModel? fullMethod = await _crud.GetFullMethodByIdAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetAsync), id, nameof(FullMethodFullModel), fullMethod);
			}

			return fullMethod is null ? (ActionResult<FullMethodFullModel>)NotFound($"No Full Method exists with the supplied Id {id}.") : (ActionResult<FullMethodFullModel>)Ok(fullMethod);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(GetAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Creates a new Full Method.
	/// </summary>
	/// <param name="model">The Full Method model containing the data for the new Full Method.</param>
	/// <returns>The newly created Full Method with full details.</returns>
	/// <response code="201">Returns the newly created Full Method.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// POST api/v1/FullMethods
	[HttpPost]
	public async Task<ActionResult<FullMethodFullModel>> PostAsync([FromBody] FullMethodAPIModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(PostAsync), nameof(FullMethodAPIModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid)
			{
				FullMethodFullModel fullMethod = await _crud.CreateNewFullMethodAsync(model.ToSimpleModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(PostAsync), nameof(FullMethodFullModel), fullMethod);
				}

				return CreatedAtAction(nameof(PostAsync), RouteData.Values, fullMethod);
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
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(FullMethodsController), nameof(PostAsync), nameof(FullMethodAPIModel), model, sb.ToString());
				}

				return BadRequest(new ValidationProblemDetails(ModelState));
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(PostAsync), nameof(FullMethodAPIModel), model);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Updates an existing Full Method.
	/// </summary>
	/// <param name="id">The unique identifier of the Full Method to update.</param>
	/// <param name="model">The Full Method model containing the updated data.</param>
	/// <returns>The updated Full Method with full details.</returns>
	/// <response code="200">Returns the updated Full Method successfully.</response>
	/// <response code="400">The route parameter ID does not match the model ID.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// PUT api/v1/FullMethods/5
	[HttpPut("{id}")]
	public async Task<ActionResult<FullMethodFullModel>> PutAsync(int id, [FromBody] FullMethodAPIModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(PutAsync), id, nameof(FullMethodAPIModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid && id == model.Id)
			{
				FullMethodFullModel fullMethod = await _crud.UpdateFullMethodAsync(model.ToSimpleModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(PutAsync), id, nameof(FullMethodFullModel), fullMethod);
				}

				return Ok(fullMethod);
			}
			else
			{
				if (id != model.Id)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(PutAsync), id, nameof(FullMethodAPIModel), model);
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
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(FullMethodsController), nameof(PutAsync), id, nameof(FullMethodAPIModel), model, sb.ToString());
					}

					return BadRequest(new ValidationProblemDetails(ModelState));
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(PutAsync), id, nameof(FullMethodAPIModel), model);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Deletes a Full Method by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Full Method to delete.</param>
	/// <returns>An action result indicating the success of the operation.</returns>
	/// <response code="200">The Full Method was deleted successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// DELETE api/v1/FullMethods/5
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(DeleteAsync), id);
			}

			await _crud.DeleteFullMethodAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(DeleteAsync), id);
			}

			return Ok();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(FullMethodsController), nameof(DeleteAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}
}
