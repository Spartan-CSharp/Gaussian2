using System.Data;

using Dapper;

using GaussianCommonLibrary.ErrorModels;
using GaussianCommonLibrary.Models;

using GaussianMVCLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Provides CRUD operations for managing Full Methods in the Gaussian application.
/// Manages dataFull interactions for Full Method entities including creation, retrieval, updates, and deletion.
/// </summary>
/// <param name="dbData">The dataFull data access service for executing queries.</param>
/// <param name="logger">The logger instance for logging operations and errors.</param>
/// <param name="spinStatesElectronicStatesMethodFamiliesCrud">The CRUD service for Spin State/Electronic State/Method Family Combinations used to validate and retrieve related Spin State/Electronic State/Method Family data.</param>
/// <param name="baseMethodsCrud">The CRUD service for Base Methods used to validate and retrieve related Base Method data.</param>
public class FullMethodsCrud(IDbData dbData, ILogger<FullMethodsCrud> logger, ISpinStatesElectronicStatesMethodFamiliesCrud spinStatesElectronicStatesMethodFamiliesCrud, IBaseMethodsCrud baseMethodsCrud) : IFullMethodsCrud
{
	private readonly IDbData _dbData = dbData;
	private readonly ILogger<FullMethodsCrud> _logger = logger;
	private readonly ISpinStatesElectronicStatesMethodFamiliesCrud _spinStatesElectronicStatesMethodFamiliesCrud = spinStatesElectronicStatesMethodFamiliesCrud;
	private readonly IBaseMethodsCrud _baseMethodsCrud = baseMethodsCrud;

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	/// <exception cref="NullParameterException">Thrown when the associated Spin State/Electronic State/Method Family Combination or the Base Method with the specified ID does not exist.</exception>
	public async Task<FullMethodFullModel> CreateNewFullMethodAsync(FullMethodSimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(FullMethodsCrud), nameof(CreateNewFullMethodAsync), nameof(FullMethodSimpleModel), model);
		}

		ArgumentNullException.ThrowIfNull(model, nameof(model));
		// This is done first, to ensure the Spin State/Electronic State/Method Family Combination exists first, before trying to create the Full Method linked to it.
		SpinStateElectronicStateMethodFamilyFullModel? spinStateElectronicStateMethodFamily = await _spinStatesElectronicStatesMethodFamiliesCrud.GetSpinStateElectronicStateMethodFamilyByIdAsync(model.SpinStateElectronicStateMethodFamilyId).ConfigureAwait(false);

		if (spinStateElectronicStateMethodFamily is null)
		{
			throw new NullParameterException(nameof(spinStateElectronicStateMethodFamily), $"The {nameof(spinStateElectronicStateMethodFamily)} with ID = {model.SpinStateElectronicStateMethodFamilyId} is null (does not exist).");
		}

		// This is done first, to ensure the Base Method exists first, before trying to create the Full Method linked to it.
		BaseMethodFullModel? baseMethod = await _baseMethodsCrud.GetBaseMethodByIdAsync(model.BaseMethodId).ConfigureAwait(false);
		
		if (baseMethod is null)
		{
			throw new NullParameterException(nameof(baseMethod), $"The {nameof(baseMethod)} with ID = {model.BaseMethodId} is null (does not exist).");
		}

		DynamicParameters p = new();
		p.Add("@Keyword", model.Keyword);
		p.Add("@SpinStateElectronicStateMethodFamilyId", model.SpinStateElectronicStateMethodFamilyId);
		p.Add("@BaseMethodId", model.BaseMethodId);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);
		p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
		_ = await _dbData.SaveDataAsync(Resources.FullMethodsCreate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		model.Id = p.Get<int>("@Id");
		model.CreatedDate = DateTime.Now;
		model.LastUpdatedDate = DateTime.Now;

		FullMethodFullModel output = new()
		{
			Id = model.Id,
			Keyword = model.Keyword,
			SpinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamily,
			BaseMethod = baseMethod,
			DescriptionRtf = model.DescriptionRtf,
			DescriptionText = model.DescriptionText,
			CreatedDate = model.CreatedDate,
			LastUpdatedDate = model.LastUpdatedDate,
			Archived = model.Archived
		};

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(FullMethodsCrud), nameof(CreateNewFullMethodAsync), nameof(FullMethodFullModel), output);
		}

		return output;
	}

	/// <inheritdoc/>
	public async Task<List<FullMethodSimpleModel>> GetAllSimpleFullMethodsAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(FullMethodsCrud), nameof(GetAllSimpleFullMethodsAsync));
		}

		DynamicParameters p = new();
		List<FullMethodSimpleModel> output = await _dbData.LoadDataAsync<FullMethodSimpleModel, dynamic>(Resources.FullMethodsGetAll, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(FullMethodsCrud), nameof(GetAllSimpleFullMethodsAsync), output.Count, nameof(FullMethodSimpleModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="InvalidOperationException">Thrown when a Full Method references a Spin State/Electronic State/Method Family Combination that does not exist in the retrieved Spin State/Electronic State/Method Family list or a Base Method that does not exist in the retrieved Base Method list.</exception>
	public async Task<List<FullMethodIntermediateModel>> GetAllIntermediateFullMethodsAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(FullMethodsCrud), nameof(GetAllIntermediateFullMethodsAsync));
		}

		List<FullMethodSimpleModel> simpleModels = await GetAllSimpleFullMethodsAsync().ConfigureAwait(false);
		List<SpinStateElectronicStateMethodFamilyRecord> spinStateElectronicStateMethodFamilies = await _spinStatesElectronicStatesMethodFamiliesCrud.GetSpinStateElectronicStateMethodFamilyListAsync().ConfigureAwait(false);
		List<BaseMethodRecord> baseMethods = await _baseMethodsCrud.GetBaseMethodListAsync().ConfigureAwait(false);
		List<FullMethodIntermediateModel> output = [];

		foreach (FullMethodSimpleModel item in simpleModels)
		{
			SpinStateElectronicStateMethodFamilyRecord spinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamilies.First(x => x.Id == item.SpinStateElectronicStateMethodFamilyId);
			BaseMethodRecord baseMethod = baseMethods.First(x => x.Id == item.BaseMethodId);

			FullMethodIntermediateModel model = new()
			{
				Id = item.Id,
				Keyword = item.Keyword,
				SpinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamily,
				BaseMethod = baseMethod,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(FullMethodsCrud), nameof(GetAllIntermediateFullMethodsAsync), output.Count, nameof(FullMethodIntermediateModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when a Spin State/Electronic State/Method Family Combination or the Base Method associated with a Full Method does not exist.</exception>
	public async Task<List<FullMethodFullModel>> GetAllFullFullMethodsAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(FullMethodsCrud), nameof(GetAllFullFullMethodsAsync));
		}

		List<FullMethodSimpleModel> simpleModels = await GetAllSimpleFullMethodsAsync().ConfigureAwait(false);
		List<FullMethodFullModel> output = [];

		foreach (FullMethodSimpleModel item in simpleModels)
		{
			SpinStateElectronicStateMethodFamilyFullModel? spinStateElectronicStateMethodFamily = await _spinStatesElectronicStatesMethodFamiliesCrud.GetSpinStateElectronicStateMethodFamilyByIdAsync(item.SpinStateElectronicStateMethodFamilyId).ConfigureAwait(false);

			if (spinStateElectronicStateMethodFamily is null)
			{
				throw new NullParameterException(nameof(spinStateElectronicStateMethodFamily), $"The {nameof(spinStateElectronicStateMethodFamily)} with ID = {item.SpinStateElectronicStateMethodFamilyId} is null (does not exist).");
			}

			BaseMethodFullModel? baseMethod = await _baseMethodsCrud.GetBaseMethodByIdAsync(item.BaseMethodId).ConfigureAwait(false);

			if (baseMethod is null)
			{
				throw new NullParameterException(nameof(baseMethod), $"The {nameof(baseMethod)} with ID = {item.BaseMethodId} is null (does not exist).");
			}

			FullMethodFullModel model = new()
			{
				Id = item.Id,
				Keyword = item.Keyword,
				SpinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamily,
				BaseMethod = baseMethod,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(FullMethodsCrud), nameof(GetAllFullFullMethodsAsync), output.Count, nameof(FullMethodFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	public async Task<List<FullMethodRecord>> GetFullMethodListAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(FullMethodsCrud), nameof(GetFullMethodListAsync));
		}

		DynamicParameters p = new();
		List<FullMethodRecord> output = await _dbData.LoadDataAsync<FullMethodRecord, dynamic>(Resources.FullMethodsGetList, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(FullMethodsCrud), nameof(GetFullMethodListAsync), output.Count, nameof(FullMethodRecord));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when the Spin State/Electronic State/Method Family Combination or the Base Method associated with the Full Method does not exist.</exception>
	public async Task<FullMethodFullModel?> GetFullMethodByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(FullMethodsCrud), nameof(GetFullMethodByIdAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		List<FullMethodSimpleModel?> simpleModels = await _dbData.LoadDataAsync<FullMethodSimpleModel?, dynamic>(Resources.FullMethodsGetById, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		FullMethodFullModel? output = null;

		if (simpleModels.Count > 0)
		{
			FullMethodSimpleModel simpleModel = simpleModels.First()!;
			SpinStateElectronicStateMethodFamilyFullModel? spinStateElectronicStateMethodFamily = await _spinStatesElectronicStatesMethodFamiliesCrud.GetSpinStateElectronicStateMethodFamilyByIdAsync(simpleModel.SpinStateElectronicStateMethodFamilyId).ConfigureAwait(false);

			if (spinStateElectronicStateMethodFamily is null)
			{
				throw new NullParameterException(nameof(spinStateElectronicStateMethodFamily), $"The {nameof(spinStateElectronicStateMethodFamily)} with ID = {simpleModel.SpinStateElectronicStateMethodFamilyId} is null (does not exist).");
			}

			BaseMethodFullModel? baseMethod = await _baseMethodsCrud.GetBaseMethodByIdAsync(simpleModel.BaseMethodId).ConfigureAwait(false);

			if (baseMethod is null)
			{
				throw new NullParameterException(nameof(baseMethod), $"The {nameof(baseMethod)} with ID = {simpleModel.BaseMethodId} is null (does not exist).");
			}

			output = new()
			{
				Id = simpleModel.Id,
				Keyword = simpleModel.Keyword,
				SpinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamily,
				BaseMethod = baseMethod,
				DescriptionRtf = simpleModel.DescriptionRtf,
				DescriptionText = simpleModel.DescriptionText,
				CreatedDate = simpleModel.CreatedDate,
				LastUpdatedDate = simpleModel.LastUpdatedDate,
				Archived = simpleModel.Archived
			};
		}

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(FullMethodsCrud), nameof(GetFullMethodByIdAsync), nameof(FullMethodFullModel), output);
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when the specified Spin State/Electronic State/Method Family does not exist.</exception>
	public async Task<List<FullMethodFullModel>> GetFullMethodsBySpinStateElectronicStateMethodFamilyIdAsync(int spinStateElectronicStateMethodFamilyId)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with SpinStateElectronicStateMethodFamilyId = {SpinStateElectronicStateMethodFamilyId}.", nameof(FullMethodsCrud), nameof(GetFullMethodsBySpinStateElectronicStateMethodFamilyIdAsync), spinStateElectronicStateMethodFamilyId);
		}

		DynamicParameters p = new();
		p.Add("@SpinStateElectronicStateMethodFamilyId", spinStateElectronicStateMethodFamilyId);
		List<FullMethodSimpleModel> simpleModels = await _dbData.LoadDataAsync<FullMethodSimpleModel, dynamic>(Resources.FullMethodsGetBySpinStateElectronicStateMethodFamilyId, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		SpinStateElectronicStateMethodFamilyFullModel? spinStateElectronicStateMethodFamily = await _spinStatesElectronicStatesMethodFamiliesCrud.GetSpinStateElectronicStateMethodFamilyByIdAsync(spinStateElectronicStateMethodFamilyId).ConfigureAwait(false);

		if (spinStateElectronicStateMethodFamily is null)
		{
			throw new NullParameterException(nameof(spinStateElectronicStateMethodFamily), $"The {nameof(spinStateElectronicStateMethodFamily)} with ID = {spinStateElectronicStateMethodFamilyId} is null (does not exist).");
		}

		List<FullMethodFullModel> output = [];

		foreach (FullMethodSimpleModel item in simpleModels)
		{
			BaseMethodFullModel? baseMethod = await _baseMethodsCrud.GetBaseMethodByIdAsync(item.BaseMethodId).ConfigureAwait(false);

			if (baseMethod is null)
			{
				throw new NullParameterException(nameof(baseMethod), $"The {nameof(baseMethod)} with ID = {item.BaseMethodId} is null (does not exist).");
			}

			FullMethodFullModel model = new()
			{
				Id = item.Id,
				Keyword = item.Keyword,
				SpinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamily,
				BaseMethod = baseMethod,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(FullMethodsCrud), nameof(GetFullMethodsBySpinStateElectronicStateMethodFamilyIdAsync), output.Count, nameof(FullMethodFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when the specified Base Method does not exist.</exception>
	public async Task<List<FullMethodFullModel>> GetFullMethodsByBaseMethodIdAsync(int baseMethodId)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with BaseMethodId = {BaseMethodId}.", nameof(FullMethodsCrud), nameof(GetFullMethodsByBaseMethodIdAsync), baseMethodId);
		}

		DynamicParameters p = new();
		p.Add("@BaseMethodId", baseMethodId);
		List<FullMethodSimpleModel> simpleModels = await _dbData.LoadDataAsync<FullMethodSimpleModel, dynamic>(Resources.FullMethodsGetByBaseMethodId, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		BaseMethodFullModel? baseMethod = await _baseMethodsCrud.GetBaseMethodByIdAsync(baseMethodId).ConfigureAwait(false);

		if (baseMethod is null)
		{
			throw new NullParameterException(nameof(baseMethod), $"The {nameof(baseMethod)} with ID = {baseMethodId} is null (does not exist).");
		}

		List<FullMethodFullModel> output = [];

		foreach (FullMethodSimpleModel item in simpleModels)
		{
			SpinStateElectronicStateMethodFamilyFullModel? spinStateElectronicStateMethodFamily = await _spinStatesElectronicStatesMethodFamiliesCrud.GetSpinStateElectronicStateMethodFamilyByIdAsync(item.SpinStateElectronicStateMethodFamilyId).ConfigureAwait(false);

			if (spinStateElectronicStateMethodFamily is null)
			{
				throw new NullParameterException(nameof(spinStateElectronicStateMethodFamily), $"The {nameof(spinStateElectronicStateMethodFamily)} with ID = {item.SpinStateElectronicStateMethodFamilyId} is null (does not exist).");
			}

			FullMethodFullModel model = new()
			{
				Id = item.Id,
				Keyword = item.Keyword,
				SpinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamily,
				BaseMethod = baseMethod,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(FullMethodsCrud), nameof(GetFullMethodsByBaseMethodIdAsync), output.Count, nameof(FullMethodFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when the specified Spin State/Electronic State/Method Family or the specified Base Method does not exist.</exception>
	public async Task<List<FullMethodFullModel>> GetFullMethodsBySpinStateElectronicStateMethodFamilyIdAndBaseMethodIdAsync(int spinStateElectronicStateMethodFamilyId, int baseMethodId)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with SpinStateElectronicStateMethodFamilyId = {SpinStateElectronicStateMethodFamilyId} and BaseMethodId = {BaseMethodId}.", nameof(FullMethodsCrud), nameof(GetFullMethodsBySpinStateElectronicStateMethodFamilyIdAsync), spinStateElectronicStateMethodFamilyId, baseMethodId);
		}

		DynamicParameters p = new();
		p.Add("@SpinStateElectronicStateMethodFamilyId", spinStateElectronicStateMethodFamilyId);
		p.Add("@BaseMethodId", baseMethodId);
		List<FullMethodSimpleModel> simpleModels = await _dbData.LoadDataAsync<FullMethodSimpleModel, dynamic>(Resources.FullMethodsGetBySpinStateElectronicStateMethodFamilyIdAndBaseMethodId, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		SpinStateElectronicStateMethodFamilyFullModel? spinStateElectronicStateMethodFamily = await _spinStatesElectronicStatesMethodFamiliesCrud.GetSpinStateElectronicStateMethodFamilyByIdAsync(spinStateElectronicStateMethodFamilyId).ConfigureAwait(false);

		if (spinStateElectronicStateMethodFamily is null)
		{
			throw new NullParameterException(nameof(spinStateElectronicStateMethodFamily), $"The {nameof(spinStateElectronicStateMethodFamily)} with ID = {spinStateElectronicStateMethodFamilyId} is null (does not exist).");
		}

		BaseMethodFullModel? baseMethod = await _baseMethodsCrud.GetBaseMethodByIdAsync(baseMethodId).ConfigureAwait(false);

		if (baseMethod is null)
		{
			throw new NullParameterException(nameof(baseMethod), $"The {nameof(baseMethod)} with ID = {baseMethodId} is null (does not exist).");
		}

		List<FullMethodFullModel> output = [];

		foreach (FullMethodSimpleModel item in simpleModels)
		{
			FullMethodFullModel model = new()
			{
				Id = item.Id,
				Keyword = item.Keyword,
				SpinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamily,
				BaseMethod = baseMethod,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(FullMethodsCrud), nameof(GetFullMethodsBySpinStateElectronicStateMethodFamilyIdAsync), output.Count, nameof(FullMethodFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	/// <exception cref="NullParameterException">Thrown when the associated Spin State/Electronic State/Method Family or Base Method with the specified ID does not exist.</exception>
	public async Task<FullMethodFullModel> UpdateFullMethodAsync(FullMethodSimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(FullMethodsCrud), nameof(UpdateFullMethodAsync), nameof(FullMethodSimpleModel), model);
		}

		ArgumentNullException.ThrowIfNull(model, nameof(model));
		// This is done first, to ensure the Spin State/Electronic State/Method Family Combination exists first, before trying to create the Full Method linked to it.
		SpinStateElectronicStateMethodFamilyFullModel? spinStateElectronicStateMethodFamily = await _spinStatesElectronicStatesMethodFamiliesCrud.GetSpinStateElectronicStateMethodFamilyByIdAsync(model.SpinStateElectronicStateMethodFamilyId).ConfigureAwait(false);

		if (spinStateElectronicStateMethodFamily is null)
		{
			throw new NullParameterException(nameof(spinStateElectronicStateMethodFamily), $"The {nameof(spinStateElectronicStateMethodFamily)} with ID = {model.SpinStateElectronicStateMethodFamilyId} is null (does not exist).");
		}

		// This is done first, to ensure the Base Method exists first, before trying to create the Full Method linked to it.
		BaseMethodFullModel? baseMethod = await _baseMethodsCrud.GetBaseMethodByIdAsync(model.BaseMethodId).ConfigureAwait(false);

		if (baseMethod is null)
		{
			throw new NullParameterException(nameof(baseMethod), $"The {nameof(baseMethod)} with ID = {model.BaseMethodId} is null (does not exist).");
		}

		DynamicParameters p = new();
		p.Add("@Id", model.Id);
		p.Add("@Keyword", model.Keyword);
		p.Add("@SpinStateElectronicStateMethodFamilyId", model.SpinStateElectronicStateMethodFamilyId);
		p.Add("@BaseMethodId", model.BaseMethodId);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);
		_ = await _dbData.SaveDataAsync(Resources.FullMethodsUpdate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		model.LastUpdatedDate = DateTime.Now;

		FullMethodFullModel output = new()
		{
			Id = model.Id,
			Keyword = model.Keyword,
			SpinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamily,
			BaseMethod = baseMethod,
			DescriptionRtf = model.DescriptionRtf,
			DescriptionText = model.DescriptionText,
			CreatedDate = model.CreatedDate,
			LastUpdatedDate = model.LastUpdatedDate,
			Archived = model.Archived
		};

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(FullMethodsCrud), nameof(CreateNewFullMethodAsync), nameof(FullMethodFullModel), output);
		}

		return output;
	}

	/// <inheritdoc/>
	public async Task DeleteFullMethodAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(FullMethodsCrud), nameof(DeleteFullMethodAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		_ = await _dbData.SaveDataAsync(Resources.FullMethodsDelete, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning.", nameof(FullMethodsCrud), nameof(DeleteFullMethodAsync));
		}
	}
}
