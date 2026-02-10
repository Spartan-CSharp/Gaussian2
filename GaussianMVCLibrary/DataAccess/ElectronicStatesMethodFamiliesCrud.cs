using System.Data;

using Dapper;

using GaussianCommonLibrary.ErrorModels;
using GaussianCommonLibrary.Models;

using GaussianMVCLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Provides CRUD operations for managing Electronic State/Method Family Combinations in the Gaussian application.
/// Manages database interactions for Electronic State/Method Family Combination entities including creation, retrieval, updates, and deletion.
/// </summary>
/// <param name="dbData">The database data access service for executing queries.</param>
/// <param name="logger">The logger instance for logging operations and errors.</param>
/// <param name="electronicStatesCrud">The CRUD service for Electronic States used to validate and retrieve related Electronic State data.</param>
/// <param name="methodFamiliesCrud">The CRUD service for Method Families used to validate and retrieve related Method Family data.</param>
public class ElectronicStatesMethodFamiliesCrud(IDbData dbData, ILogger<ElectronicStatesMethodFamiliesCrud> logger, IElectronicStatesCrud electronicStatesCrud, IMethodFamiliesCrud methodFamiliesCrud) : IElectronicStatesMethodFamiliesCrud
{
	private readonly IDbData _dbData = dbData;
	private readonly ILogger<ElectronicStatesMethodFamiliesCrud> _logger = logger;
	private readonly IElectronicStatesCrud _electronicStatesCrud = electronicStatesCrud;
	private readonly IMethodFamiliesCrud _methodFamiliesCrud = methodFamiliesCrud;

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	/// <exception cref="NullParameterException">Thrown when the associated Method Family with the specified ID does not exist.</exception>
	public async Task<ElectronicStateMethodFamilyFullModel> CreateNewElectronicStateMethodFamilyAsync(ElectronicStateMethodFamilySimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(CreateNewElectronicStateMethodFamilyAsync), nameof(ElectronicStateMethodFamilySimpleModel), model);
		}

		ArgumentNullException.ThrowIfNull(model, nameof(model));

		// This is done first, to ensure the Electronic State exists first, before trying to create the Electronic State/Method Family Combination linked to it.
		ElectronicStateFullModel? electronicState = await _electronicStatesCrud.GetElectronicStateByIdAsync(model.ElectronicStateId).ConfigureAwait(false);

		if (electronicState is null)
		{
			throw new NullParameterException(nameof(electronicState), $"The {nameof(electronicState)} with ID = {model.ElectronicStateId} is null (does not exist).");
		}

		MethodFamilyFullModel? methodFamily = null;

		if (model.MethodFamilyId is not null and > 0)
		{
			methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync((int)model.MethodFamilyId).ConfigureAwait(false);

			if (methodFamily is null)
			{
				throw new NullParameterException(nameof(methodFamily), $"The {nameof(methodFamily)} with ID = {model.MethodFamilyId} is null (does not exist).");
			}
		}

		DynamicParameters p = new();
		p.Add("@Name", model.Name);
		p.Add("@Keyword", model.Keyword);
		p.Add("@ElectronicStateId", model.ElectronicStateId);
		p.Add("@MethodFamilyId", model.MethodFamilyId);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);
		p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
		_ = await _dbData.SaveDataAsync(Resources.ElectronicStatesMethodFamiliesCreate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		model.Id = p.Get<int>("@Id");
		model.CreatedDate = DateTime.Now;
		model.LastUpdatedDate = DateTime.Now;

		ElectronicStateMethodFamilyFullModel output = new()
		{
			Id = model.Id,
			Name = model.Name,
			Keyword = model.Keyword,
			ElectronicState = electronicState,
			MethodFamily = methodFamily,
			DescriptionRtf = model.DescriptionRtf,
			DescriptionText = model.DescriptionText,
			CreatedDate = model.CreatedDate,
			LastUpdatedDate = model.LastUpdatedDate,
			Archived = model.Archived
		};

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(CreateNewElectronicStateMethodFamilyAsync), nameof(ElectronicStateMethodFamilyFullModel), output);
		}

		return output;
	}

	/// <inheritdoc/>
	public async Task<List<ElectronicStateMethodFamilySimpleModel>> GetAllSimpleElectronicStatesMethodFamiliesAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(GetAllSimpleElectronicStatesMethodFamiliesAsync));
		}

		DynamicParameters p = new();
		List<ElectronicStateMethodFamilySimpleModel> output = await _dbData.LoadDataAsync<ElectronicStateMethodFamilySimpleModel, dynamic>(Resources.ElectronicStatesMethodFamiliesGetAll, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(GetAllSimpleElectronicStatesMethodFamiliesAsync), output.Count, nameof(ElectronicStateMethodFamilySimpleModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="InvalidOperationException">Thrown when a Electronic State/Method Family Combination references an Electronic State that does not exist in the retrieved Electronic State list.</exception>
	public async Task<List<ElectronicStateMethodFamilyIntermediateModel>> GetAllIntermediateElectronicStatesMethodFamiliesAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(GetAllIntermediateElectronicStatesMethodFamiliesAsync));
		}

		List<ElectronicStateMethodFamilySimpleModel> simpleModels = await GetAllSimpleElectronicStatesMethodFamiliesAsync().ConfigureAwait(false);
		List<ElectronicStateRecord> electronicStates = await _electronicStatesCrud.GetElectronicStateListAsync().ConfigureAwait(false);
		List<MethodFamilyRecord> methodFamilies = await _methodFamiliesCrud.GetMethodFamilyListAsync().ConfigureAwait(false);
		List<ElectronicStateMethodFamilyIntermediateModel> output = [];

		foreach (ElectronicStateMethodFamilySimpleModel item in simpleModels)
		{
			ElectronicStateRecord electronicState = electronicStates.First(x => x.Id == item.ElectronicStateId);
			MethodFamilyRecord? methodFamily = null;

			if (item.MethodFamilyId is not null and > 0)
			{
				methodFamily = methodFamilies.First(x => x.Id == item.MethodFamilyId);
			}

			ElectronicStateMethodFamilyIntermediateModel model = new()
			{
				Id = item.Id,
				Name = item.Name,
				Keyword = item.Keyword,
				ElectronicState = electronicState,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(GetAllIntermediateElectronicStatesMethodFamiliesAsync), output.Count, nameof(ElectronicStateMethodFamilyIntermediateModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when an Electronic State associated with a Electronic State/Method Family Combination does not exist.</exception>
	public async Task<List<ElectronicStateMethodFamilyFullModel>> GetAllFullElectronicStatesMethodFamiliesAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(GetAllFullElectronicStatesMethodFamiliesAsync));
		}

		List<ElectronicStateMethodFamilySimpleModel> simpleModels = await GetAllSimpleElectronicStatesMethodFamiliesAsync().ConfigureAwait(false);
		List<ElectronicStateMethodFamilyFullModel> output = [];

		foreach (ElectronicStateMethodFamilySimpleModel item in simpleModels)
		{
			ElectronicStateFullModel? electronicState = await _electronicStatesCrud.GetElectronicStateByIdAsync(item.ElectronicStateId).ConfigureAwait(false);

			if (electronicState is null)
			{
				throw new NullParameterException(nameof(electronicState), $"The {nameof(electronicState)} with ID = {item.ElectronicStateId} is null (does not exist).");
			}

			MethodFamilyFullModel? methodFamily = null;

			if (item.MethodFamilyId is not null and > 0)
			{
				methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync((int)item.MethodFamilyId).ConfigureAwait(false);

				if (methodFamily is null)
				{
					throw new NullParameterException(nameof(methodFamily), $"The {nameof(methodFamily)} with ID = {item.MethodFamilyId} is null (does not exist).");
				}
			}

			ElectronicStateMethodFamilyFullModel model = new()
			{
				Id = item.Id,
				Name = item.Name,
				Keyword = item.Keyword,
				ElectronicState = electronicState,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(GetAllFullElectronicStatesMethodFamiliesAsync), output.Count, nameof(ElectronicStateMethodFamilyFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when the Method Family associated with the Electronic State/Method Family Combination does not exist.</exception>
	public async Task<ElectronicStateMethodFamilyFullModel?> GetElectronicStateMethodFamilyByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(GetElectronicStateMethodFamilyByIdAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		List<ElectronicStateMethodFamilySimpleModel?> simpleModels = await _dbData.LoadDataAsync<ElectronicStateMethodFamilySimpleModel?, dynamic>(Resources.ElectronicStatesMethodFamiliesGetById, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		ElectronicStateMethodFamilyFullModel? output = null;

		if (simpleModels.Count > 0)
		{
			ElectronicStateMethodFamilySimpleModel simpleModel = simpleModels.First()!;

			ElectronicStateFullModel? electronicState = await _electronicStatesCrud.GetElectronicStateByIdAsync(simpleModel.ElectronicStateId).ConfigureAwait(false);

			if (electronicState is null)
			{
				throw new NullParameterException(nameof(electronicState), $"The {nameof(electronicState)} with ID = {simpleModel.ElectronicStateId} is null (does not exist).");
			}

			MethodFamilyFullModel? methodFamily = null;

			if (simpleModel.MethodFamilyId is not null and > 0)
			{
				methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync((int)simpleModel.MethodFamilyId).ConfigureAwait(false);

				if (methodFamily is null)
				{
					throw new NullParameterException(nameof(methodFamily), $"The {nameof(methodFamily)} with ID = {simpleModel.MethodFamilyId} is null (does not exist).");
				}
			}

			output = new()
			{
				Id = simpleModel.Id,
				Name = simpleModel.Name,
				Keyword = simpleModel.Keyword,
				ElectronicState = electronicState,
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
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(GetElectronicStateMethodFamilyByIdAsync), nameof(ElectronicStateMethodFamilyFullModel), output);
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when the specified Electronic State does not exist.</exception>
	public async Task<List<ElectronicStateMethodFamilyFullModel>> GetElectronicStatesMethodFamiliesByElectronicStateIdAsync(int electronicStateId)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with ElectronicStateId = {ElectronicStateId}.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(GetElectronicStatesMethodFamiliesByElectronicStateIdAsync), electronicStateId);
		}

		DynamicParameters p = new();
		p.Add("@ElectronicStateId", electronicStateId);
		List<ElectronicStateMethodFamilySimpleModel> simpleModels = await _dbData.LoadDataAsync<ElectronicStateMethodFamilySimpleModel, dynamic>(Resources.ElectronicStatesMethodFamiliesGetByElectronicStateId, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		ElectronicStateFullModel? electronicState = await _electronicStatesCrud.GetElectronicStateByIdAsync(electronicStateId).ConfigureAwait(false);

		if (electronicState is null)
		{
			throw new NullParameterException(nameof(electronicState), $"The {nameof(electronicState)} with ID = {electronicStateId} is null (does not exist).");
		}

		List<ElectronicStateMethodFamilyFullModel> output = [];

		foreach (ElectronicStateMethodFamilySimpleModel item in simpleModels)
		{
			MethodFamilyFullModel? methodFamily = null;

			if (item.MethodFamilyId is not null and > 0)
			{
				methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync((int)item.MethodFamilyId).ConfigureAwait(false);

				if (methodFamily is null)
				{
					throw new NullParameterException(nameof(methodFamily), $"The {nameof(methodFamily)} with ID = {item.MethodFamilyId} is null (does not exist).");
				}
			}

			ElectronicStateMethodFamilyFullModel model = new()
			{
				Id = item.Id,
				Name = item.Name,
				Keyword = item.Keyword,
				ElectronicState = electronicState,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(GetElectronicStatesMethodFamiliesByElectronicStateIdAsync), output.Count, nameof(ElectronicStateMethodFamilyFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when the specified Method Family does not exist.</exception>
	public async Task<List<ElectronicStateMethodFamilyFullModel>> GetElectronicStatesMethodFamiliesByMethodFamilyIdAsync(int? methodFamilyId = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with MethodFamilyId = {MethodFamilyId}.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(GetElectronicStatesMethodFamiliesByMethodFamilyIdAsync), methodFamilyId);
		}

		DynamicParameters p = new();
		p.Add("@MethodFamilyId", methodFamilyId);
		List<ElectronicStateMethodFamilySimpleModel> simpleModels = await _dbData.LoadDataAsync<ElectronicStateMethodFamilySimpleModel, dynamic>(Resources.ElectronicStatesMethodFamiliesGetByMethodFamilyId, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		MethodFamilyFullModel? methodFamily = null;

		if (methodFamilyId is not null and > 0)
		{
			methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync((int)methodFamilyId).ConfigureAwait(false);

			if (methodFamily is null)
			{
				throw new NullParameterException(nameof(methodFamily), $"The {nameof(methodFamily)} with ID = {methodFamilyId} is null (does not exist).");
			}
		}

		List<ElectronicStateMethodFamilyFullModel> output = [];

		foreach (ElectronicStateMethodFamilySimpleModel item in simpleModels)
		{
			ElectronicStateFullModel? electronicState = await _electronicStatesCrud.GetElectronicStateByIdAsync(item.ElectronicStateId).ConfigureAwait(false);

			if (electronicState is null)
			{
				throw new NullParameterException(nameof(electronicState), $"The {nameof(electronicState)} with ID = {item.ElectronicStateId} is null (does not exist).");
			}

			ElectronicStateMethodFamilyFullModel model = new()
			{
				Id = item.Id,
				Name = item.Name,
				Keyword = item.Keyword,
				ElectronicState = electronicState,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(GetElectronicStatesMethodFamiliesByMethodFamilyIdAsync), output.Count, nameof(ElectronicStateMethodFamilyFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when the specified Electronic State or the specified Method Family does not exist.</exception>
	public async Task<List<ElectronicStateMethodFamilyFullModel>> GetElectronicStatesMethodFamiliesByElectronicStateIdAndMethodFamilyIdAsync(int electronicStateId, int? methodFamilyId = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with ElectronicStateId = {ElectronicStateId} and MethodFamilyId = {MethodFamilyId}.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(GetElectronicStatesMethodFamiliesByElectronicStateIdAndMethodFamilyIdAsync), electronicStateId, methodFamilyId);
		}

		DynamicParameters p = new();
		p.Add("@ElectronicStateId", electronicStateId);
		p.Add("@MethodFamilyId", methodFamilyId);
		List<ElectronicStateMethodFamilySimpleModel> simpleModels = await _dbData.LoadDataAsync<ElectronicStateMethodFamilySimpleModel, dynamic>(Resources.ElectronicStatesMethodFamiliesGetByElectronicStateIdAndMethodFamilyId, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		ElectronicStateFullModel? electronicState = await _electronicStatesCrud.GetElectronicStateByIdAsync(electronicStateId).ConfigureAwait(false);

		if (electronicState is null)
		{
			throw new NullParameterException(nameof(electronicState), $"The {nameof(electronicState)} with ID = {electronicStateId} is null (does not exist).");
		}

		MethodFamilyFullModel? methodFamily = null;

		if (methodFamilyId is not null and > 0)
		{
			methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync((int)methodFamilyId).ConfigureAwait(false);

			if (methodFamily is null)
			{
				throw new NullParameterException(nameof(methodFamily), $"The {nameof(methodFamily)} with ID = {methodFamilyId} is null (does not exist).");
			}
		}

		List<ElectronicStateMethodFamilyFullModel> output = [];

		foreach (ElectronicStateMethodFamilySimpleModel item in simpleModels)
		{
			ElectronicStateMethodFamilyFullModel model = new()
			{
				Id = item.Id,
				Name = item.Name,
				Keyword = item.Keyword,
				ElectronicState = electronicState,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(GetElectronicStatesMethodFamiliesByElectronicStateIdAndMethodFamilyIdAsync), output.Count, nameof(ElectronicStateMethodFamilyFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	/// <exception cref="NullParameterException">Thrown when the associated Method Family with the specified ID does not exist.</exception>
	public async Task<ElectronicStateMethodFamilyFullModel> UpdateElectronicStateMethodFamilyAsync(ElectronicStateMethodFamilySimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(UpdateElectronicStateMethodFamilyAsync), nameof(ElectronicStateMethodFamilySimpleModel), model);
		}

		ArgumentNullException.ThrowIfNull(model, nameof(model));
		// This is done first, to ensure the Electronic State exists first, before trying to create the Electronic State/Method Family Combination linked to it.
		ElectronicStateFullModel? electronicState = await _electronicStatesCrud.GetElectronicStateByIdAsync(model.ElectronicStateId).ConfigureAwait(false);

		if (electronicState is null)
		{
			throw new NullParameterException(nameof(electronicState), $"The {nameof(electronicState)} with ID = {model.ElectronicStateId} is null (does not exist).");
		}

		MethodFamilyFullModel? methodFamily = null;

		if (model.MethodFamilyId is not null and > 0)
		{
			methodFamily = await _methodFamiliesCrud.GetMethodFamilyByIdAsync((int)model.MethodFamilyId).ConfigureAwait(false);

			if (methodFamily is null)
			{
				throw new NullParameterException(nameof(methodFamily), $"The {nameof(methodFamily)} with ID = {model.MethodFamilyId} is null (does not exist).");
			}
		}

		DynamicParameters p = new();
		p.Add("@Id", model.Id);
		p.Add("@Name", model.Name);
		p.Add("@Keyword", model.Keyword);
		p.Add("@ElectronicStateId", model.ElectronicStateId);
		p.Add("@MethodFamilyId", model.MethodFamilyId);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);
		_ = await _dbData.SaveDataAsync(Resources.ElectronicStatesMethodFamiliesUpdate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		model.LastUpdatedDate = DateTime.Now;

		ElectronicStateMethodFamilyFullModel output = new()
		{
			Id = model.Id,
			Name = model.Name,
			Keyword = model.Keyword,
			ElectronicState = electronicState,
			MethodFamily = methodFamily,
			DescriptionRtf = model.DescriptionRtf,
			DescriptionText = model.DescriptionText,
			CreatedDate = model.CreatedDate,
			LastUpdatedDate = model.LastUpdatedDate,
			Archived = model.Archived
		};

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(CreateNewElectronicStateMethodFamilyAsync), nameof(ElectronicStateMethodFamilyFullModel), output);
		}

		return output;
	}

	/// <inheritdoc/>
	public async Task DeleteElectronicStateMethodFamilyAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(DeleteElectronicStateMethodFamilyAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		_ = await _dbData.SaveDataAsync(Resources.ElectronicStatesMethodFamiliesDelete, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning.", nameof(ElectronicStatesMethodFamiliesCrud), nameof(DeleteElectronicStateMethodFamilyAsync));
		}
	}
}
