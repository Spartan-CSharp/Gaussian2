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
/// API controller for managing Electronic State/Method Family Combinations.
/// Provides CRUD operations for Electronic State/Method Family Combination resources with support for full and simple model representations.
/// </summary>
/// <remarks>
/// This controller implements version 1.0 of the Electronic State/Method Family Combinations API and supports the following operations:
/// - Retrieving all Electronic State/Method Family Combinations in full or simple format
/// - Retrieving Electronic State/Method Family Combinations by Method Family
/// - Retrieving a single Electronic State/Method Family Combination by ID
/// - Creating new Electronic State/Method Family Combinations
/// - Updating existing Electronic State/Method Family Combinations
/// - Deleting Electronic State/Method Family Combinations
/// </remarks>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class ElectronicStatesMethodFamiliesController(ILogger<ElectronicStatesMethodFamiliesController> logger, IElectronicStatesMethodFamiliesCrud crud) : ControllerBase
{
	private readonly ILogger<ElectronicStatesMethodFamiliesController> _logger = logger;
	private readonly IElectronicStatesMethodFamiliesCrud _crud = crud;

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations with full details.
	/// </summary>
	/// <returns>A list of all Electronic State/Method Family Combinations with complete information.</returns>
	/// <response code="200">Returns the list of Electronic State/Method Family Combinations successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/ElectronicStatesMethodFamilies
	[HttpGet()]
	public async Task<ActionResult<List<ElectronicStateMethodFamilyFullModel>>> GetFullAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetFullAsync));
			}

			List<ElectronicStateMethodFamilyFullModel> electronicStatesMethodFamilies = await _crud.GetAllFullElectronicStatesMethodFamiliesAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetFullAsync), electronicStatesMethodFamilies.Count, nameof(ElectronicStateMethodFamilyFullModel));
			}

			return Ok(electronicStatesMethodFamilies);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetFullAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations with intermediate details.
	/// </summary>
	/// <returns>A list of all Electronic State/Method Family Combinations with Method Family records instead of full Method Family models.</returns>
	/// <response code="200">Returns the list of Electronic State/Method Family Combinations successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/ElectronicStatesMethodFamilies/Intermediate
	[HttpGet("Intermediate")]
	public async Task<ActionResult<List<ElectronicStateMethodFamilyIntermediateModel>>> GetIntermediateAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetIntermediateAsync));
			}

			List<ElectronicStateMethodFamilyIntermediateModel> electronicStatesMethodFamilies = await _crud.GetAllIntermediateElectronicStatesMethodFamiliesAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetIntermediateAsync), electronicStatesMethodFamilies.Count, nameof(ElectronicStateMethodFamilyIntermediateModel));
			}

			return Ok(electronicStatesMethodFamilies);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetIntermediateAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations with simplified details.
	/// </summary>
	/// <returns>A list of all Electronic State/Method Family Combinations with basic information.</returns>
	/// <response code="200">Returns the list of Electronic State/Method Family Combinations successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/ElectronicStatesMethodFamilies/Simple
	[HttpGet("Simple")]
	public async Task<ActionResult<List<ElectronicStateMethodFamilySimpleModel>>> GetSimpleAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetSimpleAsync));
			}

			List<ElectronicStateMethodFamilySimpleModel> electronicStatesMethodFamilies = await _crud.GetAllSimpleElectronicStatesMethodFamiliesAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model}", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetSimpleAsync), electronicStatesMethodFamilies.Count, nameof(ElectronicStateMethodFamilySimpleModel));
			}

			return Ok(electronicStatesMethodFamilies);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetSimpleAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a simplified list of Electronic State/Method Family Combinations.
	/// </summary>
	/// <returns>
	/// An <see cref="ActionResult"/> containing a list of <see cref="ElectronicStateMethodFamilyRecord"/> objects.
	/// Returns 200 OK with the list of Electronic State/Method Family Combination records on success.
	/// Returns 500 Internal Server Error if an error occurs during retrieval.
	/// </returns>
	/// <response code="200">Returns the list of Electronic State/Method Family Combination records.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET: api/v1/ElectronicStatesMethodFamilies/List
	[HttpGet("List")]
	public async Task<ActionResult<List<ElectronicStateMethodFamilyRecord>>> GetListAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetListAsync));
			}

			List<ElectronicStateMethodFamilyRecord> electronicStatesMethodFamilies = await _crud.GetElectronicStateMethodFamilyListAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model}", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetListAsync), electronicStatesMethodFamilies.Count, nameof(ElectronicStateMethodFamilyRecord));
			}

			return Ok(electronicStatesMethodFamilies);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetListAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations belonging to a specific Electronic State.
	/// </summary>
	/// <param name="electronicStateId">The unique identifier of the Electronic State.</param>
	/// <returns>A list of Electronic State/Method Family Combinations associated with the specified Electronic State.</returns>
	/// <response code="200">Returns the list of Electronic State/Method Family Combinations successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/ElectronicStatesMethodFamilies/ElectronicState?electronicStateId=5
	[HttpGet("ElectronicState")]
	public async Task<ActionResult<List<ElectronicStateMethodFamilyFullModel>>> GetByElectronicStateAsync([FromQuery(Name = "electronicStateId")] int electronicStateId)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with query parameter electronicStateId = {ElectronicStateId}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetByElectronicStateAsync), electronicStateId);
			}

			List<ElectronicStateMethodFamilyFullModel> electronicStatesMethodFamilies = await _crud.GetElectronicStatesMethodFamiliesByElectronicStateIdAsync(electronicStateId).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model} with electronicStateId = {ElectronicStateId}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetByElectronicStateAsync), electronicStatesMethodFamilies.Count, nameof(ElectronicStateMethodFamilyFullModel), electronicStateId);
			}

			return Ok(electronicStatesMethodFamilies);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with query parameter electronicStateId = {ElectronicStateId} had an error", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetByElectronicStateAsync), electronicStateId);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations belonging to a specific Method Family.
	/// </summary>
	/// <param name="methodFamilyId">The unique identifier of the Method Family.</param>
	/// <returns>A list of Electronic State/Method Family Combinations associated with the specified Method Family.</returns>
	/// <response code="200">Returns the list of Electronic State/Method Family Combinations successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/ElectronicStatesMethodFamilies/MethodFamily?methodFamilyId=5
	[HttpGet("MethodFamily")]
	public async Task<ActionResult<List<ElectronicStateMethodFamilyFullModel>>> GetByMethodFamilyAsync([FromQuery(Name = "methodFamilyId")] int? methodFamilyId)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with query parameter methodFamilyId = {MethodFamilyId}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetByMethodFamilyAsync), methodFamilyId);
			}

			List<ElectronicStateMethodFamilyFullModel> electronicStatesMethodFamilies = await _crud.GetElectronicStatesMethodFamiliesByMethodFamilyIdAsync(methodFamilyId).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model} with methodFamilyId = {MethodFamilyId}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetByMethodFamilyAsync), electronicStatesMethodFamilies.Count, nameof(ElectronicStateMethodFamilyFullModel), methodFamilyId);
			}

			return Ok(electronicStatesMethodFamilies);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with query parameter methodFamilyId = {MethodFamilyId} had an error", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetByMethodFamilyAsync), methodFamilyId);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations belonging to a specific Method Family and a specific Electronic State.
	/// </summary>
	/// <param name="electronicStateId">The unique identifier of the Electronic State.</param>
	/// <param name="methodFamilyId">The unique identifier of the Method Family.</param>
	/// <returns>A list of Electronic State/Method Family Combinations associated with the specified Method Family and Electronic State.</returns>
	/// <response code="200">Returns the list of Electronic State/Method Family Combinations successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/ElectronicStatesMethodFamilies/ElectronicStateMethodFamily?electronicStateId=5&methodFamilyId=5
	[HttpGet("ElectronicStateMethodFamily")]
	public async Task<ActionResult<List<ElectronicStateMethodFamilyFullModel>>> GetByElectronicStateAndMethodFamilyAsync([FromQuery(Name = "electronicStateId")] int electronicStateId, [FromQuery(Name = "methodFamilyId")] int? methodFamilyId)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with query parameters electronicStateId = {ElectronicStateId} and methodFamilyId = {MethodFamilyId}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetByElectronicStateAndMethodFamilyAsync), electronicStateId, methodFamilyId);
			}

			List<ElectronicStateMethodFamilyFullModel> electronicStatesMethodFamilies = await _crud.GetElectronicStatesMethodFamiliesByElectronicStateIdAndMethodFamilyIdAsync(electronicStateId, methodFamilyId).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model} with electronicStateId = {ElectronicStateId} and methodFamilyId = {MethodFamilyId}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetByElectronicStateAndMethodFamilyAsync), electronicStatesMethodFamilies.Count, nameof(ElectronicStateMethodFamilyFullModel), electronicStateId, methodFamilyId);
			}

			return Ok(electronicStatesMethodFamilies);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with query parameters electronicStateId = {ElectronicStateId} and methodFamilyId = {MethodFamilyId} had an error", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetByElectronicStateAndMethodFamilyAsync), electronicStateId, methodFamilyId);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a single Electronic State/Method Family Combination by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State/Method Family Combination.</param>
	/// <returns>The Electronic State/Method Family Combination with full details if found.</returns>
	/// <response code="200">Returns the Electronic State/Method Family Combination successfully.</response>
	/// <response code="404">No Electronic State/Method Family Combination exists with the specified ID.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET api/v1/ElectronicStatesMethodFamilies/5
	[HttpGet("{id}")]
	public async Task<ActionResult<ElectronicStateMethodFamilyFullModel>> GetAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetAsync), id);
			}

			ElectronicStateMethodFamilyFullModel? electronicStateMethodFamily = await _crud.GetElectronicStateMethodFamilyByIdAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetAsync), id, nameof(ElectronicStateMethodFamilyFullModel), electronicStateMethodFamily);
			}

			return electronicStateMethodFamily is null ? (ActionResult<ElectronicStateMethodFamilyFullModel>)NotFound($"No Electronic State/Method Family Combination exists with the supplied Id {id}.") : (ActionResult<ElectronicStateMethodFamilyFullModel>)Ok(electronicStateMethodFamily);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(GetAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Creates a new Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="model">The Electronic State/Method Family Combination model containing the data for the new Electronic State/Method Family Combination.</param>
	/// <returns>The newly created Electronic State/Method Family Combination with full details.</returns>
	/// <response code="201">Returns the newly created Electronic State/Method Family Combination.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// POST api/v1/ElectronicStatesMethodFamilies
	[HttpPost]
	public async Task<ActionResult<ElectronicStateMethodFamilyFullModel>> PostAsync([FromBody] ElectronicStateMethodFamilyAPIModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(PostAsync), nameof(ElectronicStateMethodFamilyAPIModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid)
			{
				ElectronicStateMethodFamilyFullModel electronicStateMethodFamily = await _crud.CreateNewElectronicStateMethodFamilyAsync(model.ToSimpleModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(PostAsync), nameof(ElectronicStateMethodFamilyFullModel), electronicStateMethodFamily);
				}

				return CreatedAtAction(nameof(PostAsync), RouteData.Values, electronicStateMethodFamily);
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
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(PostAsync), nameof(ElectronicStateMethodFamilyAPIModel), model, sb.ToString());
				}

				return BadRequest(new ValidationProblemDetails(ModelState));
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(PostAsync), nameof(ElectronicStateMethodFamilyAPIModel), model);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Updates an existing Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State/Method Family Combination to update.</param>
	/// <param name="model">The Electronic State/Method Family Combination model containing the updated data.</param>
	/// <returns>The updated Electronic State/Method Family Combination with full details.</returns>
	/// <response code="200">Returns the updated Electronic State/Method Family Combination successfully.</response>
	/// <response code="400">The route parameter ID does not match the model ID.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// PUT api/v1/ElectronicStatesMethodFamilies/5
	[HttpPut("{id}")]
	public async Task<ActionResult<ElectronicStateMethodFamilyFullModel>> PutAsync(int id, [FromBody] ElectronicStateMethodFamilyAPIModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(PutAsync), id, nameof(ElectronicStateMethodFamilyAPIModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid && id == model.Id)
			{
				ElectronicStateMethodFamilyFullModel electronicStateMethodFamily = await _crud.UpdateElectronicStateMethodFamilyAsync(model.ToSimpleModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(PutAsync), id, nameof(ElectronicStateMethodFamilyFullModel), electronicStateMethodFamily);
				}

				return Ok(electronicStateMethodFamily);
			}
			else
			{
				if (id != model.Id)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(PutAsync), id, nameof(ElectronicStateMethodFamilyAPIModel), model);
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
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(PutAsync), id, nameof(ElectronicStateMethodFamilyAPIModel), model, sb.ToString());
					}

					return BadRequest(new ValidationProblemDetails(ModelState));
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(PutAsync), id, nameof(ElectronicStateMethodFamilyAPIModel), model);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Deletes a Electronic State/Method Family Combination by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State/Method Family Combination to delete.</param>
	/// <returns>An action result indicating the success of the operation.</returns>
	/// <response code="200">The Electronic State/Method Family Combination was deleted successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// DELETE api/v1/ElectronicStatesMethodFamilies/5
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id);
			}

			await _crud.DeleteElectronicStateMethodFamilyAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id);
			}

			return Ok();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(ElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}
}
