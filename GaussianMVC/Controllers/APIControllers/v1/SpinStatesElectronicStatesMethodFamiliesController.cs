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
/// API controller for managing Spin State/Electronic State/Method Family Combinations.
/// Provides CRUD operations for Spin State/Electronic State/Method Family Combination resources with support for full and simple model representations.
/// </summary>
/// <remarks>
/// This controller implements version 1.0 of the Spin State/Electronic State/Method Family Combinations API and supports the following operations:
/// - Retrieving all Spin State/Electronic State/Method Family Combinations in full or simple format
/// - Retrieving Spin State/Electronic State/Method Family Combinations by Method Family
/// - Retrieving a single Spin State/Electronic State/Method Family Combination by ID
/// - Creating new Spin State/Electronic State/Method Family Combinations
/// - Updating existing Spin State/Electronic State/Method Family Combinations
/// - Deleting Spin State/Electronic State/Method Family Combinations
/// </remarks>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class SpinStatesElectronicStatesMethodFamiliesController(ILogger<SpinStatesElectronicStatesMethodFamiliesController> logger, ISpinStatesElectronicStatesMethodFamiliesCrud crud) : ControllerBase
{
	private readonly ILogger<SpinStatesElectronicStatesMethodFamiliesController> _logger = logger;
	private readonly ISpinStatesElectronicStatesMethodFamiliesCrud _crud = crud;

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations with full details.
	/// </summary>
	/// <returns>A list of all Spin State/Electronic State/Method Family Combinations with complete information.</returns>
	/// <response code="200">Returns the list of Spin State/Electronic State/Method Family Combinations successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/SpinStatesElectronicStatesMethodFamilies
	[HttpGet()]
	public async Task<ActionResult<List<SpinStateElectronicStateMethodFamilyFullModel>>> GetFullAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetFullAsync));
			}

			List<SpinStateElectronicStateMethodFamilyFullModel> spinStatesElectronicStatesMethodFamilies = await _crud.GetAllFullSpinStatesElectronicStatesMethodFamiliesAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetFullAsync), spinStatesElectronicStatesMethodFamilies.Count, nameof(SpinStateElectronicStateMethodFamilyFullModel));
			}

			return Ok(spinStatesElectronicStatesMethodFamilies);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetFullAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations with intermediate details.
	/// </summary>
	/// <returns>A list of all Spin State/Electronic State/Method Family Combinations with Method Family records instead of full Method Family models.</returns>
	/// <response code="200">Returns the list of Spin State/Electronic State/Method Family Combinations successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/SpinStatesElectronicStatesMethodFamilies/Intermediate
	[HttpGet("Intermediate")]
	public async Task<ActionResult<List<SpinStateElectronicStateMethodFamilyIntermediateModel>>> GetIntermediateAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetIntermediateAsync));
			}

			List<SpinStateElectronicStateMethodFamilyIntermediateModel> spinStatesElectronicStatesMethodFamilies = await _crud.GetAllIntermediateSpinStatesElectronicStatesMethodFamiliesAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetIntermediateAsync), spinStatesElectronicStatesMethodFamilies.Count, nameof(SpinStateElectronicStateMethodFamilyIntermediateModel));
			}

			return Ok(spinStatesElectronicStatesMethodFamilies);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetIntermediateAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations with simplified details.
	/// </summary>
	/// <returns>A list of all Spin State/Electronic State/Method Family Combinations with basic information.</returns>
	/// <response code="200">Returns the list of Spin State/Electronic State/Method Family Combinations successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/SpinStatesElectronicStatesMethodFamilies/Simple
	[HttpGet("Simple")]
	public async Task<ActionResult<List<SpinStateElectronicStateMethodFamilySimpleModel>>> GetSimpleAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetSimpleAsync));
			}

			List<SpinStateElectronicStateMethodFamilySimpleModel> spinStatesElectronicStatesMethodFamilies = await _crud.GetAllSimpleSpinStatesElectronicStatesMethodFamiliesAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model}", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetSimpleAsync), spinStatesElectronicStatesMethodFamilies.Count, nameof(SpinStateElectronicStateMethodFamilySimpleModel));
			}

			return Ok(spinStatesElectronicStatesMethodFamilies);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetSimpleAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a simplified list of Spin State/Electronic State/Method Family Combinations.
	/// </summary>
	/// <returns>
	/// An <see cref="ActionResult"/> containing a list of <see cref="SpinStateElectronicStateMethodFamilyRecord"/> objects.
	/// Returns 200 OK with the list of Spin State/Electronic State/Method Family Combination records on success.
	/// Returns 500 Internal Server Error if an error occurs during retrieval.
	/// </returns>
	/// <response code="200">Returns the list of Spin State/Electronic State/Method Family Combination records.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET: api/v1/ElectronicStatesMethodFamilies/List
	[HttpGet("List")]
	public async Task<ActionResult<List<SpinStateElectronicStateMethodFamilyRecord>>> GetListAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetListAsync));
			}

			List<SpinStateElectronicStateMethodFamilyRecord> spinStatesElectronicStatesMethodFamilies = await _crud.GetSpinStateElectronicStateMethodFamilyListAsync().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model}", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetListAsync), spinStatesElectronicStatesMethodFamilies.Count, nameof(SpinStateElectronicStateMethodFamilyRecord));
			}

			return Ok(spinStatesElectronicStatesMethodFamilies);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetListAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations belonging to a specific Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="electronicStateMethodFamilyId">The unique identifier of the Electronic State/Method Family Combination.</param>
	/// <returns>A list of Spin State/Electronic State/Method Family Combinations associated with the specified Electronic State/Method Family Combination.</returns>
	/// <response code="200">Returns the list of Spin State/Electronic State/Method Family Combinations successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/SpinStatesElectronicStatesMethodFamilies/ElectronicStateMethodFamily?electronicStateMethodFamilyId=5
	[HttpGet("ElectronicStateMethodFamily")]
	public async Task<ActionResult<List<SpinStateElectronicStateMethodFamilyFullModel>>> GetByElectronicStateMethodFamilyAsync([FromQuery(Name = "electronicStateMethodFamilyId")] int electronicStateMethodFamilyId)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with query parameter electronicStateMethodFamilyId = {ElectronicStateMethodFamilyId}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetByElectronicStateMethodFamilyAsync), electronicStateMethodFamilyId);
			}

			List<SpinStateElectronicStateMethodFamilyFullModel> spinStatesElectronicStatesMethodFamilies = await _crud.GetSpinStatesElectronicStatesMethodFamiliesByElectronicStateMethodFamilyIdAsync(electronicStateMethodFamilyId).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model} with electronicStateMethodFamilyId = {ElectronicStateMethodFamilyId}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetByElectronicStateMethodFamilyAsync), spinStatesElectronicStatesMethodFamilies.Count, nameof(SpinStateElectronicStateMethodFamilyFullModel), electronicStateMethodFamilyId);
			}

			return Ok(spinStatesElectronicStatesMethodFamilies);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with query parameter electronicStateMethodFamilyId = {ElectronicStateMethodFamilyId} had an error", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetByElectronicStateMethodFamilyAsync), electronicStateMethodFamilyId);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations belonging to a specific Spin State.
	/// </summary>
	/// <param name="spinStateId">The unique identifier of the Spin State.</param>
	/// <returns>A list of Spin State/Electronic State/Method Family Combinations associated with the specified Spin State.</returns>
	/// <response code="200">Returns the list of Spin State/Electronic State/Method Family Combinations successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/SpinStatesElectronicStatesMethodFamilies/SpinState?spinStateId=5
	[HttpGet("SpinState")]
	public async Task<ActionResult<List<SpinStateElectronicStateMethodFamilyFullModel>>> GetBySpinStateAsync([FromQuery(Name = "spinStateId")] int? spinStateId)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with query parameter spinStateId = {SpinStateId}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetBySpinStateAsync), spinStateId);
			}

			List<SpinStateElectronicStateMethodFamilyFullModel> spinStatesElectronicStatesMethodFamilies = await _crud.GetSpinStatesElectronicStatesMethodFamiliesBySpinStateIdAsync(spinStateId).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model} with spinStateId = {MethodFamilyId}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetBySpinStateAsync), spinStatesElectronicStatesMethodFamilies.Count, nameof(SpinStateElectronicStateMethodFamilyFullModel), spinStateId);
			}

			return Ok(spinStatesElectronicStatesMethodFamilies);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with query parameter spinStateId = {SpinStateId} had an error", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetBySpinStateAsync), spinStateId);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations belonging to a specific Electronic State/Method Family Combination and a specific Spin State.
	/// </summary>
	/// <param name="electronicStateMethodFamilyId">The unique identifier of the Electronic State/Method Family Combination.</param>
	/// <param name="spinStateId">The unique identifier of the Spin State.</param>
	/// <returns>A list of Spin State/Electronic State/Method Family Combinations associated with the specified Electronic State/Method Family Combination and Spin State.</returns>
	/// <response code="200">Returns the list of Spin State/Electronic State/Method Family Combinations successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET: api/v1/SpinStatesElectronicStatesMethodFamilies/SpinStateElectronicStateMethodFamily?electronicStateMethodFamilyId=5&spinStateId=5
	[HttpGet("SpinStateElectronicStateMethodFamily")]
	public async Task<ActionResult<List<SpinStateElectronicStateMethodFamilyFullModel>>> GetByElectronicStateMethodFamilyAndSpinStateAsync([FromQuery(Name = "electronicStateMethodFamilyId")] int electronicStateMethodFamilyId, [FromQuery(Name = "spinStateId")] int? spinStateId)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with query parameters electronicStateMethodFamilyId = {ElectronicStateMethodFamilyId} and spinStateId = {SpinStateId}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetByElectronicStateMethodFamilyAndSpinStateAsync), electronicStateMethodFamilyId, spinStateId);
			}

			List<SpinStateElectronicStateMethodFamilyFullModel> spinStatesElectronicStatesMethodFamilies = await _crud.GetSpinStatesElectronicStatesMethodFamiliesByElectronicStateMethodFamilyIdAndSpinStateIdAsync(electronicStateMethodFamilyId, spinStateId).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {Model} with electronicStateMethodFamilyId = {ElectronicStateMethodFamilyId} and spinStateId = {SpinStateId}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetByElectronicStateMethodFamilyAndSpinStateAsync), spinStatesElectronicStatesMethodFamilies.Count, nameof(SpinStateElectronicStateMethodFamilyFullModel), electronicStateMethodFamilyId, spinStateId);
			}

			return Ok(spinStatesElectronicStatesMethodFamilies);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with query parameters electronicStateMethodFamilyId = {ElectronicStateMethodFamilyId} and spinStateId = {SpinStateId} had an error", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetByElectronicStateMethodFamilyAndSpinStateAsync), electronicStateMethodFamilyId, spinStateId);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a single Spin State/Electronic State/Method Family Combination by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State/Electronic State/Method Family Combination.</param>
	/// <returns>The Spin State/Electronic State/Method Family Combination with full details if found.</returns>
	/// <response code="200">Returns the Spin State/Electronic State/Method Family Combination successfully.</response>
	/// <response code="404">No Spin State/Electronic State/Method Family Combination exists with the specified ID.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// GET api/v1/SpinStatesElectronicStatesMethodFamilies/5
	[HttpGet("{id}")]
	public async Task<ActionResult<SpinStateElectronicStateMethodFamilyFullModel>> GetAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetAsync), id);
			}

			SpinStateElectronicStateMethodFamilyFullModel? spinStateElectronicStateMethodFamily = await _crud.GetSpinStateElectronicStateMethodFamilyByIdAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetAsync), id, nameof(SpinStateElectronicStateMethodFamilyFullModel), spinStateElectronicStateMethodFamily);
			}

			return spinStateElectronicStateMethodFamily is null ? (ActionResult<SpinStateElectronicStateMethodFamilyFullModel>)NotFound($"No Spin State/Electronic State/Method Family Combination exists with the supplied Id {id}.") : (ActionResult<SpinStateElectronicStateMethodFamilyFullModel>)Ok(spinStateElectronicStateMethodFamily);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(GetAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Creates a new Spin State/Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="model">The Spin State/Electronic State/Method Family Combination model containing the data for the new Spin State/Electronic State/Method Family Combination.</param>
	/// <returns>The newly created Spin State/Electronic State/Method Family Combination with full details.</returns>
	/// <response code="201">Returns the newly created Spin State/Electronic State/Method Family Combination.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// POST api/v1/SpinStatesElectronicStatesMethodFamilies
	[HttpPost]
	public async Task<ActionResult<SpinStateElectronicStateMethodFamilyFullModel>> PostAsync([FromBody] SpinStateElectronicStateMethodFamilyAPIModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(PostAsync), nameof(SpinStateElectronicStateMethodFamilyAPIModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid)
			{
				SpinStateElectronicStateMethodFamilyFullModel spinStateElectronicStateMethodFamily = await _crud.CreateNewSpinStateElectronicStateMethodFamilyAsync(model.ToSimpleModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(PostAsync), nameof(SpinStateElectronicStateMethodFamilyFullModel), spinStateElectronicStateMethodFamily);
				}

				return CreatedAtAction(nameof(PostAsync), RouteData.Values, spinStateElectronicStateMethodFamily);
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
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(PostAsync), nameof(SpinStateElectronicStateMethodFamilyAPIModel), model, sb.ToString());
				}

				return BadRequest(new ValidationProblemDetails(ModelState));
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(PostAsync), nameof(SpinStateElectronicStateMethodFamilyAPIModel), model);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Updates an existing Spin State/Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State/Electronic State/Method Family Combination to update.</param>
	/// <param name="model">The Spin State/Electronic State/Method Family Combination model containing the updated data.</param>
	/// <returns>The updated Spin State/Electronic State/Method Family Combination with full details.</returns>
	/// <response code="200">Returns the updated Spin State/Electronic State/Method Family Combination successfully.</response>
	/// <response code="400">The route parameter ID does not match the model ID.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// PUT api/v1/SpinStatesElectronicStatesMethodFamilies/5
	[HttpPut("{id}")]
	public async Task<ActionResult<SpinStateElectronicStateMethodFamilyFullModel>> PutAsync(int id, [FromBody] SpinStateElectronicStateMethodFamilyAPIModel model)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(PutAsync), id, nameof(SpinStateElectronicStateMethodFamilyAPIModel), model);
			}

			ArgumentNullException.ThrowIfNull(model, nameof(model));

			if (ModelState.IsValid && id == model.Id)
			{
				SpinStateElectronicStateMethodFamilyFullModel spinStateElectronicStateMethodFamily = await _crud.UpdateSpinStateElectronicStateMethodFamilyAsync(model.ToSimpleModel()).ConfigureAwait(false);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(PutAsync), id, nameof(SpinStateElectronicStateMethodFamilyFullModel), spinStateElectronicStateMethodFamily);
				}

				return Ok(spinStateElectronicStateMethodFamily);
			}
			else
			{
				if (id != model.Id)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(PutAsync), id, nameof(SpinStateElectronicStateMethodFamilyAPIModel), model);
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
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(PutAsync), id, nameof(SpinStateElectronicStateMethodFamilyAPIModel), model, sb.ToString());
					}

					return BadRequest(new ValidationProblemDetails(ModelState));
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(PutAsync), id, nameof(SpinStateElectronicStateMethodFamilyAPIModel), model);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Deletes a Spin State/Electronic State/Method Family Combination by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State/Electronic State/Method Family Combination to delete.</param>
	/// <returns>An action result indicating the success of the operation.</returns>
	/// <response code="200">The Spin State/Electronic State/Method Family Combination was deleted successfully.</response>
	/// <response code="500">An internal server error occurred while processing the request.</response>
	// DELETE api/v1/SpinStatesElectronicStatesMethodFamilies/5
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteAsync(int id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id);
			}

			await _crud.DeleteSpinStateElectronicStateMethodFamilyAsync(id).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id);
			}

			return Ok();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(SpinStatesElectronicStatesMethodFamiliesController), nameof(DeleteAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}
}
