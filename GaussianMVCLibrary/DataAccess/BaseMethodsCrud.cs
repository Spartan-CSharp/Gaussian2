using System.Data;

using Dapper;

using GaussianCommonLibrary.ErrorModels;
using GaussianCommonLibrary.Models;

using GaussianMVCLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Provides CRUD operations for managing base methods in the Gaussian application.
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

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	/// <exception cref="NullParameterException">Thrown when the associated method family with the specified ID does not exist.</exception>
	public async Task<BaseMethodFullModel> CreateNewBaseMethodAsync(BaseMethodSimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(BaseMethodsCrud), nameof(CreateNewBaseMethodAsync), nameof(BaseMethodSimpleModel), model);
		}

		ArgumentNullException.ThrowIfNull(model, nameof(model));
		// This is done first, to ensure the Method Family exists first, before trying to create the Base Method linked to it.
		MethodFamilyFullModel? methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync(model.MethodFamilyId).ConfigureAwait(false);

		if (methodFamily is null)
		{
			throw new NullParameterException(nameof(methodFamily), $"The {nameof(methodFamily)} with ID = {model.MethodFamilyId} is null (does not exist).");
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
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(BaseMethodsCrud), nameof(CreateNewBaseMethodAsync), nameof(BaseMethodFullModel), output);
		}

		return output;
	}

	/// <inheritdoc/>
	public async Task<List<BaseMethodSimpleModel>> GetAllSimpleBaseMethodsAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(BaseMethodsCrud), nameof(GetAllSimpleBaseMethodsAsync));
		}

		DynamicParameters p = new();
		List<BaseMethodSimpleModel> output = await _dbData.LoadDataAsync<BaseMethodSimpleModel, dynamic>(Resources.BaseMethodsGetAll, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(BaseMethodsCrud), nameof(GetAllSimpleBaseMethodsAsync), output.Count, nameof(BaseMethodSimpleModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="InvalidOperationException">Thrown when a base method references a method family that does not exist in the retrieved method family list.</exception>
	public async Task<List<BaseMethodIntermediateModel>> GetAllIntermediateBaseMethodsAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(BaseMethodsCrud), nameof(GetAllIntermediateBaseMethodsAsync));
		}

		List<BaseMethodSimpleModel> simpleModels = await GetAllSimpleBaseMethodsAsync().ConfigureAwait(false);
		List<MethodFamilyRecord> methodFamilies = await _methodFamiliesCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
		List<BaseMethodIntermediateModel> output = [];

		foreach (BaseMethodSimpleModel item in simpleModels)
		{
			MethodFamilyRecord methodFamily = methodFamilies.First(x => x.Id == item.MethodFamilyId);

			BaseMethodIntermediateModel model = new()
			{
				Id = item.Id,
				Keyword = item.Keyword,
				MethodFamily = methodFamily,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(BaseMethodsCrud), nameof(GetAllIntermediateBaseMethodsAsync), output.Count, nameof(BaseMethodIntermediateModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when a method family associated with a base method does not exist.</exception>
	public async Task<List<BaseMethodFullModel>> GetAllFullBaseMethodsAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(BaseMethodsCrud), nameof(GetAllFullBaseMethodsAsync));
		}

		List<BaseMethodSimpleModel> simpleModels = await GetAllSimpleBaseMethodsAsync().ConfigureAwait(false);
		List<BaseMethodFullModel> output = [];

		foreach (BaseMethodSimpleModel item in simpleModels)
		{
			MethodFamilyFullModel? methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync(item.MethodFamilyId).ConfigureAwait(false);

			if (methodFamily is null)
			{
				throw new NullParameterException(nameof(methodFamily), $"The {nameof(methodFamily)} with ID = {item.MethodFamilyId} is null (does not exist).");
			}

			BaseMethodFullModel model = new()
			{
				Id = item.Id,
				Keyword = item.Keyword,
				MethodFamily = methodFamily,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(BaseMethodsCrud), nameof(GetAllFullBaseMethodsAsync), output.Count, nameof(BaseMethodFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when the method family associated with the base method does not exist.</exception>
	public async Task<BaseMethodFullModel?> GetBaseMethodByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(BaseMethodsCrud), nameof(GetBaseMethodByIdAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		List<BaseMethodSimpleModel?> simpleModels = await _dbData.LoadDataAsync<BaseMethodSimpleModel?, dynamic>(Resources.BaseMethodsGetById, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		BaseMethodFullModel? output = null;

		if (simpleModels.Count > 0)
		{
			BaseMethodSimpleModel simpleModel = simpleModels.First()!;
			MethodFamilyFullModel? methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync(simpleModel.MethodFamilyId).ConfigureAwait(false);

			if (methodFamily is null)
			{
				throw new NullParameterException(nameof(methodFamily), $"The {nameof(methodFamily)} with ID = {simpleModel.MethodFamilyId} is null (does not exist).");
			}

			output = new()
			{
				Id = simpleModel.Id,
				Keyword = simpleModel.Keyword,
				MethodFamily = methodFamily,
				DescriptionRtf = simpleModel.DescriptionRtf,
				DescriptionText = simpleModel.DescriptionText,
				CreatedDate = simpleModel.CreatedDate,
				LastUpdatedDate = simpleModel.LastUpdatedDate,
				Archived = simpleModel.Archived
			};
		}

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(BaseMethodsCrud), nameof(GetBaseMethodByIdAsync), nameof(BaseMethodFullModel), output);
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when the specified method family does not exist.</exception>
	public async Task<List<BaseMethodFullModel>> GetBaseMethodsByMethodFamilyIdAsync(int methodFamilyId)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with MethodFamilyId = {MethodFamilyId}.", nameof(BaseMethodsCrud), nameof(GetBaseMethodsByMethodFamilyIdAsync), methodFamilyId);
		}

		DynamicParameters p = new();
		p.Add("@MethodFamilyId", methodFamilyId);
		List<BaseMethodSimpleModel> simpleModels = await _dbData.LoadDataAsync<BaseMethodSimpleModel, dynamic>(Resources.BaseMethodsGetByMethodFamilyId, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		MethodFamilyFullModel? methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync(methodFamilyId).ConfigureAwait(false);

		if (methodFamily is null)
		{
			throw new NullParameterException(nameof(methodFamily), $"The {nameof(methodFamily)} with ID = {methodFamilyId} is null (does not exist).");
		}

		List<BaseMethodFullModel> output = [];

		foreach (BaseMethodSimpleModel item in simpleModels)
		{
			BaseMethodFullModel model = new()
			{
				Id = item.Id,
				Keyword = item.Keyword,
				MethodFamily = methodFamily,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(BaseMethodsCrud), nameof(GetBaseMethodsByMethodFamilyIdAsync), output.Count, nameof(BaseMethodFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	/// <exception cref="NullParameterException">Thrown when the associated method family with the specified ID does not exist.</exception>
	public async Task<BaseMethodFullModel> UpdateBaseMethodAsync(BaseMethodSimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(BaseMethodsCrud), nameof(UpdateBaseMethodAsync), nameof(BaseMethodSimpleModel), model);
		}

		ArgumentNullException.ThrowIfNull(model, nameof(model));
		// This is done first, to ensure the Method Family exists first, before trying to create the Base Method linked to it.
		MethodFamilyFullModel? methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync(model.MethodFamilyId).ConfigureAwait(false);

		if (methodFamily is null)
		{
			throw new NullParameterException(nameof(methodFamily), $"The {nameof(methodFamily)} with ID = {model.MethodFamilyId} is null (does not exist).");
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
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(BaseMethodsCrud), nameof(CreateNewBaseMethodAsync), nameof(BaseMethodFullModel), output);
		}

		return output;
	}

	/// <inheritdoc/>
	public async Task DeleteBaseMethodAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(BaseMethodsCrud), nameof(DeleteBaseMethodAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		_ = await _dbData.SaveDataAsync(Resources.BaseMethodsDelete, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning.", nameof(BaseMethodsCrud), nameof(DeleteBaseMethodAsync));
		}
	}
}
