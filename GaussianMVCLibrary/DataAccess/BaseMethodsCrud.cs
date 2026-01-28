using System.Data;

using Dapper;

using GaussianCommonLibrary.ErrorModels;
using GaussianCommonLibrary.Models;

using GaussianMVCLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Provides CRUD (Create, Read, Update, Delete) operations for Base Methods.
/// Manages database interactions for base method entities including creation, retrieval, updates, and deletion.
/// </summary>
/// <param name="dbData">The database data access service for executing queries.</param>
/// <param name="logger">The logger instance for logging operations and errors.</param>
/// <param name="methodFamiliesCrud">The CRUD service for method families used to validate and retrieve related method family data.</param>
public class BaseMethodsCrud(IDbData dbData, ILogger<BaseMethodsCrud> logger, IMethodFamiliesCrud methodFamiliesCrud) : IBaseMethodsCrud
{
	private readonly IDbData _dbData = dbData;
	private readonly ILogger<BaseMethodsCrud> _logger = logger;
	private readonly IMethodFamiliesCrud _methodFamiliesCrud = methodFamiliesCrud;

	/// <summary>
	/// Creates a new base method in the database.
	/// Validates that the associated method family exists before creating the base method.
	/// </summary>
	/// <param name="model">The simple model containing the base method data to create.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the newly created base method with full details including the method family.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	/// <exception cref="NullParameterException">Thrown when the associated method family with the specified ID does not exist.</exception>
	public async Task<BaseMethodFullModel> CreateNewBaseMethodAsync(BaseMethodSimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {BaseMethodFullModel}", nameof(CreateNewBaseMethodAsync), model);
		}

		if (model is null)
		{
			ArgumentNullException ex = new(nameof(model), $"The {nameof(model)} provided is null.");

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} Called with a null model", nameof(CreateNewBaseMethodAsync));
			}

			throw ex;
		}

		// This is done first, to ensure the Method Family exists first, before trying to create the Base Method linked to it.
		MethodFamilyFullModel? methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync(model.MethodFamilyId).ConfigureAwait(false);

		if (methodFamily is null)
		{
			NullParameterException ex = new(nameof(methodFamily), $"The {nameof(methodFamily)} with ID = {model.MethodFamilyId} is null (does not exist).");

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} Called with an invalid {MethodFamily} = {Id}", nameof(CreateNewBaseMethodAsync), nameof(model.MethodFamilyId), model.MethodFamilyId);
			}

			throw ex;
		}

		DynamicParameters p = new();
		p.Add("@Keyword", model.Keyword);
		p.Add("@MethodFamilyId", model.MethodFamilyId);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);
		p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

		_ = await _dbData.SaveDataAsync(Resources.BaseMethodsCreate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		model.Id = p.Get<int>("@Id");
		model.CreatedDate = DateTime.Now;
		model.LastUpdatedDate = DateTime.Now;

		BaseMethodFullModel output = new()
		{
			Id = model.Id,
			Keyword = model.Keyword,
			MethodFamily = methodFamily,
			DescriptionRtf = model.DescriptionRtf,
			DescriptionText = model.DescriptionText,
			CreatedDate = model.CreatedDate,
			LastUpdatedDate = model.LastUpdatedDate,
			Archived = model.Archived
		};

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Returning {BaseMethodFullModel}", nameof(CreateNewBaseMethodAsync), output);
		}

		return output;
	}

	/// <summary>
	/// Retrieves all base methods from the database as simple models.
	/// Simple models contain only basic properties without related entity details.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all base methods as simple models.</returns>
	public async Task<List<BaseMethodSimpleModel>> GetAllSimpleBaseMethodsAsync()
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called", nameof(GetAllSimpleBaseMethodsAsync));
		}

		DynamicParameters p = new();
		List<BaseMethodSimpleModel> output = await _dbData.LoadDataAsync<BaseMethodSimpleModel, dynamic>(Resources.BaseMethodsGetAll, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Returning {Count}", nameof(GetAllSimpleBaseMethodsAsync), output.Count);
		}

		return output;
	}

	/// <summary>
	/// Retrieves all base methods from the database as full models with related method family details.
	/// This method first retrieves simple models and then enriches them with full method family information.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all base methods with full details including method families.</returns>
	public async Task<List<BaseMethodFullModel>> GetAllFullBaseMethodsAsync()
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called", nameof(GetAllFullBaseMethodsAsync));
		}

		List<BaseMethodSimpleModel> simpleModels = await GetAllSimpleBaseMethodsAsync().ConfigureAwait(false);

		List<BaseMethodFullModel> output = [];

		foreach (BaseMethodSimpleModel item in simpleModels)
		{
			MethodFamilyFullModel? methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync(item.MethodFamilyId).ConfigureAwait(false);
			BaseMethodFullModel model = new()
			{
				Id = item.Id,
				Keyword = item.Keyword,
				MethodFamily = methodFamily!,
				DescriptionRtf = item.DescriptionRtf,
				DescriptionText = item.DescriptionText,
				CreatedDate = item.CreatedDate,
				LastUpdatedDate = item.LastUpdatedDate,
				Archived = item.Archived
			};
			output.Add(model);
		}

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Returning {Count}", nameof(GetAllFullBaseMethodsAsync), output.Count);
		}

		return output;
	}

	/// <summary>
	/// Retrieves a specific base method by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to retrieve.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the base method with full details if found; otherwise, null.</returns>
	public async Task<BaseMethodFullModel?> GetBaseMethodByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {Id}", nameof(GetBaseMethodByIdAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		List<BaseMethodSimpleModel?> simpleModels = await _dbData.LoadDataAsync<BaseMethodSimpleModel?, dynamic>(Resources.BaseMethodsGetById, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		BaseMethodFullModel? output = null;

		if (simpleModels.Count > 0)
		{
			BaseMethodSimpleModel simpleModel = simpleModels.First()!;
			MethodFamilyFullModel? methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync(simpleModel.MethodFamilyId).ConfigureAwait(false);

			output = new()
			{
				Id = simpleModel.Id,
				Keyword = simpleModel.Keyword,
				MethodFamily = methodFamily!,
				DescriptionRtf = simpleModel.DescriptionRtf,
				DescriptionText = simpleModel.DescriptionText,
				CreatedDate = simpleModel.CreatedDate,
				LastUpdatedDate = simpleModel.LastUpdatedDate,
				Archived = simpleModel.Archived
			};
		}

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Returning {MethodFamilyFullModel}", nameof(GetBaseMethodByIdAsync), output);
		}

		return output;
	}

	/// <summary>
	/// Retrieves all base methods that belong to a specific method family.
	/// Returns full models with complete method family details for all matching base methods.
	/// </summary>
	/// <param name="methodFamilyId">The unique identifier of the method family to filter by.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of base methods belonging to the specified method family.</returns>
	public async Task<List<BaseMethodFullModel>> GetBaseMethodsByMethodFamilyIdAsync(int methodFamilyId)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {MethodFamilyId}", nameof(GetBaseMethodsByMethodFamilyIdAsync), methodFamilyId);
		}

		DynamicParameters p = new();
		p.Add("@MethodFamilyId", methodFamilyId);

		List<BaseMethodSimpleModel> simpleModels = await _dbData.LoadDataAsync<BaseMethodSimpleModel, dynamic>(Resources.BaseMethodsGetByMethodFamilyId, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		MethodFamilyFullModel? methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync(methodFamilyId).ConfigureAwait(false);

		List<BaseMethodFullModel> output = [];

		foreach (BaseMethodSimpleModel item in simpleModels)
		{
			BaseMethodFullModel model = new()
			{
				Id = item.Id,
				Keyword = item.Keyword,
				MethodFamily = methodFamily!,
				DescriptionRtf = item.DescriptionRtf,
				DescriptionText = item.DescriptionText,
				CreatedDate = item.CreatedDate,
				LastUpdatedDate = item.LastUpdatedDate,
				Archived = item.Archived
			};
			output.Add(model);
		}

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Returning {Count}", nameof(GetBaseMethodsByMethodFamilyIdAsync), output.Count);
		}

		return output;
	}

	/// <summary>
	/// Updates an existing base method in the database.
	/// Validates that the associated method family exists before performing the update.
	/// </summary>
	/// <param name="model">The simple model containing the updated base method data. Must include a valid ID.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the updated base method with full details including the method family.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	/// <exception cref="NullParameterException">Thrown when the associated method family with the specified ID does not exist.</exception>
	public async Task<BaseMethodFullModel> UpdateBaseMethodAsync(BaseMethodSimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {BaseMethodFullModel}", nameof(UpdateBaseMethodAsync), model);
		}

		if (model is null)
		{
			ArgumentNullException ex = new(nameof(model), $"The {nameof(model)} provided is null.");

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} Called with a null model", nameof(UpdateBaseMethodAsync));
			}

			throw ex;
		}

		// This is done first, to ensure the Method Family exists first, before trying to create the Base Method linked to it.
		MethodFamilyFullModel? methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync(model.MethodFamilyId).ConfigureAwait(false);

		if (methodFamily is null)
		{
			NullParameterException ex = new(nameof(methodFamily), $"The {nameof(methodFamily)} with ID = {model.MethodFamilyId} is null (does not exist).");

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} Called with an invalid {MethodFamily} = {Id}", nameof(UpdateBaseMethodAsync), nameof(model.MethodFamilyId), model.MethodFamilyId);
			}

			throw ex;
		}

		DynamicParameters p = new();
		p.Add("@Id", model.Id);
		p.Add("@Keyword", model.Keyword);
		p.Add("@MethodFamilyId", model.MethodFamilyId);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);

		_ = await _dbData.SaveDataAsync(Resources.BaseMethodsUpdate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		model.LastUpdatedDate = DateTime.Now;

		BaseMethodFullModel output = new()
		{
			Id = model.Id,
			Keyword = model.Keyword,
			MethodFamily = methodFamily,
			DescriptionRtf = model.DescriptionRtf,
			DescriptionText = model.DescriptionText,
			CreatedDate = model.CreatedDate,
			LastUpdatedDate = model.LastUpdatedDate,
			Archived = model.Archived
		};

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Returning {BaseMethodFullModel}", nameof(CreateNewBaseMethodAsync), output);
		}

		return output;
	}

	/// <summary>
	/// Deletes a base method from the database by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to delete.</param>
	/// <returns>A task that represents the asynchronous delete operation.</returns>
	public async Task DeleteBaseMethodAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {Id}", nameof(DeleteBaseMethodAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);

		_ = await _dbData.SaveDataAsync(Resources.BaseMethodsDelete, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
	}
}
