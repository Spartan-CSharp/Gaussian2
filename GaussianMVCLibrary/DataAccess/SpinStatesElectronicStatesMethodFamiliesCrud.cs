using System.Data;

using Dapper;

using GaussianCommonLibrary.ErrorModels;
using GaussianCommonLibrary.Models;

using GaussianMVCLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Provides CRUD operations for managing Spin State/Electronic State/Method Family Combinations in the Gaussian application.
/// Manages database interactions for Spin State/Electronic State/Method Family Combination entities including creation, retrieval, updates, and deletion.
/// </summary>
/// <param name="dbData">The database data access service for executing queries.</param>
/// <param name="logger">The logger instance for logging operations and errors.</param>
/// <param name="electronicStatesMethodFamiliesCrud">The CRUD service for Electronic State/Method Family Combinations used to validate and retrieve related Electronic State data.</param>
/// <param name="spinStatesCrud">The CRUD service for Spin States used to validate and retrieve related Spin State data.</param>
public class SpinStatesElectronicStatesMethodFamiliesCrud(IDbData dbData, ILogger<SpinStatesElectronicStatesMethodFamiliesCrud> logger, IElectronicStatesMethodFamiliesCrud electronicStatesMethodFamiliesCrud, ISpinStatesCrud spinStatesCrud) : ISpinStatesElectronicStatesMethodFamiliesCrud
{
	private readonly IDbData _dbData = dbData;
	private readonly ILogger<SpinStatesElectronicStatesMethodFamiliesCrud> _logger = logger;
	private readonly IElectronicStatesMethodFamiliesCrud _electronicStatesMethodFamiliesCrud = electronicStatesMethodFamiliesCrud;
	private readonly ISpinStatesCrud _spinStatesCrud = spinStatesCrud;

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	/// <exception cref="NullParameterException">Thrown when the associated Electronic State/Method Family Combination with the specified ID does not exist or when the associated Spin State with the specified ID does not exist.</exception>
	public async Task<SpinStateElectronicStateMethodFamilyFullModel> CreateNewSpinStateElectronicStateMethodFamilyAsync(SpinStateElectronicStateMethodFamilySimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(CreateNewSpinStateElectronicStateMethodFamilyAsync), nameof(SpinStateElectronicStateMethodFamilySimpleModel), model);
		}

		ArgumentNullException.ThrowIfNull(model, nameof(model));

		// This is done first, to ensure the Electronic State/Method Family Combination exists first, before trying to create the Spin State/Electronic State/Method Family Combination linked to it.
		ElectronicStateMethodFamilyFullModel? electronicStateMethodFamily = await _electronicStatesMethodFamiliesCrud.GetElectronicStateMethodFamilyByIdAsync(model.ElectronicStateMethodFamilyId).ConfigureAwait(false);

		if (electronicStateMethodFamily is null)
		{
			throw new NullParameterException(nameof(electronicStateMethodFamily), $"The {nameof(electronicStateMethodFamily)} with ID = {model.ElectronicStateMethodFamilyId} is null (does not exist).");
		}

		SpinStateFullModel? spinState = null;

		if (model.SpinStateId is not null and > 0)
		{
			spinState = await _spinStatesCrud.GetSpinStateByIdAsync((int)model.SpinStateId).ConfigureAwait(false) ?? throw new NullParameterException(nameof(spinState), $"The {nameof(spinState)} with ID = {model.SpinStateId} is null (does not exist).");
		}

		DynamicParameters p = new();
		p.Add("@Name", model.Name);
		p.Add("@Keyword", model.Keyword);
		p.Add("@ElectronicStateMethodFamilyId", model.ElectronicStateMethodFamilyId);
		p.Add("@SpinStateId", model.SpinStateId);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);
		p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
		_ = await _dbData.SaveDataAsync(Resources.SpinStatesElectronicStatesMethodFamiliesCreate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		model.Id = p.Get<int>("@Id");
		model.CreatedDate = DateTime.Now;
		model.LastUpdatedDate = DateTime.Now;

		SpinStateElectronicStateMethodFamilyFullModel output = new()
		{
			Id = model.Id,
			Name = model.Name,
			Keyword = model.Keyword,
			ElectronicStateMethodFamily = electronicStateMethodFamily,
			SpinState = spinState,
			DescriptionRtf = model.DescriptionRtf,
			DescriptionText = model.DescriptionText,
			CreatedDate = model.CreatedDate,
			LastUpdatedDate = model.LastUpdatedDate,
			Archived = model.Archived
		};

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(CreateNewSpinStateElectronicStateMethodFamilyAsync), nameof(SpinStateElectronicStateMethodFamilyFullModel), output);
		}

		return output;
	}

	/// <inheritdoc/>
	public async Task<List<SpinStateElectronicStateMethodFamilySimpleModel>> GetAllSimpleSpinStatesElectronicStatesMethodFamiliesAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(GetAllSimpleSpinStatesElectronicStatesMethodFamiliesAsync));
		}

		DynamicParameters p = new();
		List<SpinStateElectronicStateMethodFamilySimpleModel> output = await _dbData.LoadDataAsync<SpinStateElectronicStateMethodFamilySimpleModel, dynamic>(Resources.SpinStatesElectronicStatesMethodFamiliesGetAll, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(GetAllSimpleSpinStatesElectronicStatesMethodFamiliesAsync), output.Count, nameof(SpinStateElectronicStateMethodFamilySimpleModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="InvalidOperationException">Thrown when a Spin State/Electronic State/Method Family Combination references an Electronic State/Method Family Combination that does not exist in the retrieved Electronic State/Method Family Combination list.</exception>
	public async Task<List<SpinStateElectronicStateMethodFamilyIntermediateModel>> GetAllIntermediateSpinStatesElectronicStatesMethodFamiliesAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(GetAllIntermediateSpinStatesElectronicStatesMethodFamiliesAsync));
		}

		List<SpinStateElectronicStateMethodFamilySimpleModel> simpleModels = await GetAllSimpleSpinStatesElectronicStatesMethodFamiliesAsync().ConfigureAwait(false);
		List<ElectronicStateMethodFamilyRecord> electronicStatesMethodFamilies = await _electronicStatesMethodFamiliesCrud.GetElectronicStateMethodFamilyListAsync().ConfigureAwait(false);
		List<SpinStateRecord> spinStates = await _spinStatesCrud.GetSpinStateListAsync().ConfigureAwait(false);
		List<SpinStateElectronicStateMethodFamilyIntermediateModel> output = [];

		foreach (SpinStateElectronicStateMethodFamilySimpleModel item in simpleModels)
		{
			ElectronicStateMethodFamilyRecord electronicStateMethodFamily = electronicStatesMethodFamilies.First(x => x.Id == item.ElectronicStateMethodFamilyId);
			SpinStateRecord? spinState = null;

			if (item.SpinStateId is not null and > 0)
			{
				spinState = spinStates.First(x => x.Id == item.SpinStateId);
			}

			SpinStateElectronicStateMethodFamilyIntermediateModel model = new()
			{
				Id = item.Id,
				Name = item.Name,
				Keyword = item.Keyword,
				ElectronicStateMethodFamily = electronicStateMethodFamily,
				SpinState = spinState,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(GetAllIntermediateSpinStatesElectronicStatesMethodFamiliesAsync), output.Count, nameof(SpinStateElectronicStateMethodFamilyIntermediateModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when an Electronic State/Method Family Combination associated with a Spin State/Electronic State/Method Family Combination does not exist.</exception>
	public async Task<List<SpinStateElectronicStateMethodFamilyFullModel>> GetAllFullSpinStatesElectronicStatesMethodFamiliesAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(GetAllFullSpinStatesElectronicStatesMethodFamiliesAsync));
		}

		List<SpinStateElectronicStateMethodFamilySimpleModel> simpleModels = await GetAllSimpleSpinStatesElectronicStatesMethodFamiliesAsync().ConfigureAwait(false);
		List<SpinStateElectronicStateMethodFamilyFullModel> output = [];

		foreach (SpinStateElectronicStateMethodFamilySimpleModel item in simpleModels)
		{
			ElectronicStateMethodFamilyFullModel? electronicStateMethodFamily = await _electronicStatesMethodFamiliesCrud.GetElectronicStateMethodFamilyByIdAsync(item.ElectronicStateMethodFamilyId).ConfigureAwait(false);

			if (electronicStateMethodFamily is null)
			{
				throw new NullParameterException(nameof(electronicStateMethodFamily), $"The {nameof(electronicStateMethodFamily)} with ID = {item.ElectronicStateMethodFamilyId} is null (does not exist).");
			}

			SpinStateFullModel? spinState = null;

			if (item.SpinStateId is not null and > 0)
			{
				spinState = await _spinStatesCrud.GetSpinStateByIdAsync((int)item.SpinStateId).ConfigureAwait(false) ?? throw new NullParameterException(nameof(spinState), $"The {nameof(spinState)} with ID = {item.SpinStateId} is null (does not exist).");
			}

			SpinStateElectronicStateMethodFamilyFullModel model = new()
			{
				Id = item.Id,
				Name = item.Name,
				Keyword = item.Keyword,
				ElectronicStateMethodFamily = electronicStateMethodFamily,
				SpinState = spinState,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(GetAllFullSpinStatesElectronicStatesMethodFamiliesAsync), output.Count, nameof(SpinStateElectronicStateMethodFamilyFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	public async Task<List<SpinStateElectronicStateMethodFamilyRecord>> GetSpinStateElectronicStateMethodFamilyListAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(GetSpinStateElectronicStateMethodFamilyListAsync));
		}

		DynamicParameters p = new();
		List<SpinStateElectronicStateMethodFamilyRecord> output = await _dbData.LoadDataAsync<SpinStateElectronicStateMethodFamilyRecord, dynamic>(Resources.SpinStatesElectronicStatesMethodFamiliesGetList, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(GetSpinStateElectronicStateMethodFamilyListAsync), output.Count, nameof(SpinStateElectronicStateMethodFamilyRecord));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when the Spin State associated with the Spin State/Electronic State/Method Family Combination does not exist.</exception>
	public async Task<SpinStateElectronicStateMethodFamilyFullModel?> GetSpinStateElectronicStateMethodFamilyByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(GetSpinStateElectronicStateMethodFamilyByIdAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		List<SpinStateElectronicStateMethodFamilySimpleModel?> simpleModels = await _dbData.LoadDataAsync<SpinStateElectronicStateMethodFamilySimpleModel?, dynamic>(Resources.SpinStatesElectronicStatesMethodFamiliesGetById, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		SpinStateElectronicStateMethodFamilyFullModel? output = null;

		if (simpleModels.Count > 0)
		{
			SpinStateElectronicStateMethodFamilySimpleModel simpleModel = simpleModels.First()!;

			ElectronicStateMethodFamilyFullModel? electronicStateMethodFamily = await _electronicStatesMethodFamiliesCrud.GetElectronicStateMethodFamilyByIdAsync(simpleModel.ElectronicStateMethodFamilyId).ConfigureAwait(false);

			if (electronicStateMethodFamily is null)
			{
				throw new NullParameterException(nameof(electronicStateMethodFamily), $"The {nameof(electronicStateMethodFamily)} with ID = {simpleModel.ElectronicStateMethodFamilyId} is null (does not exist).");
			}

			SpinStateFullModel? spinState = null;

			if (simpleModel.SpinStateId is not null and > 0)
			{
				spinState = await _spinStatesCrud.GetSpinStateByIdAsync((int)simpleModel.SpinStateId).ConfigureAwait(false) ?? throw new NullParameterException(nameof(spinState), $"The {nameof(spinState)} with ID = {simpleModel.SpinStateId} is null (does not exist).");
			}

			output = new()
			{
				Id = simpleModel.Id,
				Name = simpleModel.Name,
				Keyword = simpleModel.Keyword,
				ElectronicStateMethodFamily = electronicStateMethodFamily,
				SpinState = spinState,
				DescriptionRtf = simpleModel.DescriptionRtf,
				DescriptionText = simpleModel.DescriptionText,
				CreatedDate = simpleModel.CreatedDate,
				LastUpdatedDate = simpleModel.LastUpdatedDate,
				Archived = simpleModel.Archived
			};
		}

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(GetSpinStateElectronicStateMethodFamilyByIdAsync), nameof(SpinStateElectronicStateMethodFamilyFullModel), output);
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when the specified Electronic State/Method Family Combination does not exist.</exception>
	public async Task<List<SpinStateElectronicStateMethodFamilyFullModel>> GetSpinStatesElectronicStatesMethodFamiliesByElectronicStateMethodFamilyIdAsync(int electronicStateMethodFamilyId)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with ElectronicStateMethodFamilyId = {ElectronicStateMethodFamilyId}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(GetSpinStatesElectronicStatesMethodFamiliesByElectronicStateMethodFamilyIdAsync), electronicStateMethodFamilyId);
		}

		DynamicParameters p = new();
		p.Add("@ElectronicStateMethodFamilyId", electronicStateMethodFamilyId);
		List<SpinStateElectronicStateMethodFamilySimpleModel> simpleModels = await _dbData.LoadDataAsync<SpinStateElectronicStateMethodFamilySimpleModel, dynamic>(Resources.SpinStatesElectronicStatesMethodFamiliesGetByElectronicStateMethodFamilyId, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		ElectronicStateMethodFamilyFullModel? electronicStateMethodFamily = await _electronicStatesMethodFamiliesCrud.GetElectronicStateMethodFamilyByIdAsync(electronicStateMethodFamilyId).ConfigureAwait(false);

		if (electronicStateMethodFamily is null)
		{
			throw new NullParameterException(nameof(electronicStateMethodFamily), $"The {nameof(electronicStateMethodFamily)} with ID = {electronicStateMethodFamilyId} is null (does not exist).");
		}

		List<SpinStateElectronicStateMethodFamilyFullModel> output = [];

		foreach (SpinStateElectronicStateMethodFamilySimpleModel item in simpleModels)
		{
			SpinStateFullModel? spinState = null;

			if (item.SpinStateId is not null and > 0)
			{
				spinState = await _spinStatesCrud.GetSpinStateByIdAsync((int)item.SpinStateId).ConfigureAwait(false) ?? throw new NullParameterException(nameof(spinState), $"The {nameof(spinState)} with ID = {item.SpinStateId} is null (does not exist).");
			}

			SpinStateElectronicStateMethodFamilyFullModel model = new()
			{
				Id = item.Id,
				Name = item.Name,
				Keyword = item.Keyword,
				ElectronicStateMethodFamily = electronicStateMethodFamily,
				SpinState = spinState,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(GetSpinStatesElectronicStatesMethodFamiliesByElectronicStateMethodFamilyIdAsync), output.Count, nameof(SpinStateElectronicStateMethodFamilyFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when the specified Spin State does not exist.</exception>
	public async Task<List<SpinStateElectronicStateMethodFamilyFullModel>> GetSpinStatesElectronicStatesMethodFamiliesBySpinStateIdAsync(int? spinStateId = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with SpinStateId = {SpinStateId}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(GetSpinStatesElectronicStatesMethodFamiliesBySpinStateIdAsync), spinStateId);
		}

		DynamicParameters p = new();
		p.Add("@SpinStateId", spinStateId);
		List<SpinStateElectronicStateMethodFamilySimpleModel> simpleModels = await _dbData.LoadDataAsync<SpinStateElectronicStateMethodFamilySimpleModel, dynamic>(Resources.SpinStatesElectronicStatesMethodFamiliesGetBySpinStateId, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		SpinStateFullModel? spinState = null;

		if (spinStateId is not null and > 0)
		{
			spinState = await _spinStatesCrud.GetSpinStateByIdAsync((int)spinStateId).ConfigureAwait(false) ?? throw new NullParameterException(nameof(spinState), $"The {nameof(spinState)} with ID = {spinStateId} is null (does not exist).");
		}

		List<SpinStateElectronicStateMethodFamilyFullModel> output = [];

		foreach (SpinStateElectronicStateMethodFamilySimpleModel item in simpleModels)
		{
			ElectronicStateMethodFamilyFullModel? electronicStateMethodFamily = await _electronicStatesMethodFamiliesCrud.GetElectronicStateMethodFamilyByIdAsync(item.ElectronicStateMethodFamilyId).ConfigureAwait(false);

			if (electronicStateMethodFamily is null)
			{
				throw new NullParameterException(nameof(electronicStateMethodFamily), $"The {nameof(electronicStateMethodFamily)} with ID = {item.ElectronicStateMethodFamilyId} is null (does not exist).");
			}

			SpinStateElectronicStateMethodFamilyFullModel model = new()
			{
				Id = item.Id,
				Name = item.Name,
				Keyword = item.Keyword,
				ElectronicStateMethodFamily = electronicStateMethodFamily,
				SpinState = spinState,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(GetSpinStatesElectronicStatesMethodFamiliesBySpinStateIdAsync), output.Count, nameof(SpinStateElectronicStateMethodFamilyFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="NullParameterException">Thrown when the specified Electronic State/Method Family Combination or the specified Spin State does not exist.</exception>
	public async Task<List<SpinStateElectronicStateMethodFamilyFullModel>> GetSpinStatesElectronicStatesMethodFamiliesByElectronicStateMethodFamilyIdAndSpinStateIdAsync(int electronicStateMethodFamilyId, int? spinStateId = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with ElectronicStateMethodFamilyId = {ElectronicStateMethodFamilyId} and SpinStateId = {SpinStateId}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(GetSpinStatesElectronicStatesMethodFamiliesByElectronicStateMethodFamilyIdAndSpinStateIdAsync), electronicStateMethodFamilyId, spinStateId);
		}

		DynamicParameters p = new();
		p.Add("@ElectronicStateMethodFamilyId", electronicStateMethodFamilyId);
		p.Add("@SpinStateId", spinStateId);
		List<SpinStateElectronicStateMethodFamilySimpleModel> simpleModels = await _dbData.LoadDataAsync<SpinStateElectronicStateMethodFamilySimpleModel, dynamic>(Resources.SpinStatesElectronicStatesMethodFamiliesGetByElectronicStateMethodFamilyIdAndSpinStateId, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		ElectronicStateMethodFamilyFullModel? electronicStateMethodFamily = await _electronicStatesMethodFamiliesCrud.GetElectronicStateMethodFamilyByIdAsync(electronicStateMethodFamilyId).ConfigureAwait(false);

		if (electronicStateMethodFamily is null)
		{
			throw new NullParameterException(nameof(electronicStateMethodFamily), $"The {nameof(electronicStateMethodFamily)} with ID = {electronicStateMethodFamilyId} is null (does not exist).");
		}

		SpinStateFullModel? spinState = null;

		if (spinStateId is not null and > 0)
		{
			spinState = await _spinStatesCrud.GetSpinStateByIdAsync((int)spinStateId).ConfigureAwait(false) ?? throw new NullParameterException(nameof(spinState), $"The {nameof(spinState)} with ID = {spinStateId} is null (does not exist).");
		}

		List<SpinStateElectronicStateMethodFamilyFullModel> output = [];

		foreach (SpinStateElectronicStateMethodFamilySimpleModel item in simpleModels)
		{
			SpinStateElectronicStateMethodFamilyFullModel model = new()
			{
				Id = item.Id,
				Name = item.Name,
				Keyword = item.Keyword,
				ElectronicStateMethodFamily = electronicStateMethodFamily,
				SpinState = spinState,
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
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(GetSpinStatesElectronicStatesMethodFamiliesByElectronicStateMethodFamilyIdAndSpinStateIdAsync), output.Count, nameof(SpinStateElectronicStateMethodFamilyFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when the model parameter is null.</exception>
	/// <exception cref="NullParameterException">Thrown when the associated Method Family with the specified ID does not exist.</exception>
	public async Task<SpinStateElectronicStateMethodFamilyFullModel> UpdateSpinStateElectronicStateMethodFamilyAsync(SpinStateElectronicStateMethodFamilySimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(UpdateSpinStateElectronicStateMethodFamilyAsync), nameof(SpinStateElectronicStateMethodFamilySimpleModel), model);
		}

		ArgumentNullException.ThrowIfNull(model, nameof(model));
		// This is done first, to ensure the Electronic State/Method Family Combination exists first, before trying to create the Spin State/Electronic State/Method Family Combination linked to it.
		ElectronicStateMethodFamilyFullModel? electronicStateMethodFamily = await _electronicStatesMethodFamiliesCrud.GetElectronicStateMethodFamilyByIdAsync(model.ElectronicStateMethodFamilyId).ConfigureAwait(false);

		if (electronicStateMethodFamily is null)
		{
			throw new NullParameterException(nameof(electronicStateMethodFamily), $"The {nameof(electronicStateMethodFamily)} with ID = {model.ElectronicStateMethodFamilyId} is null (does not exist).");
		}

		SpinStateFullModel? spinState = null;

		if (model.SpinStateId is not null and > 0)
		{
			spinState = await _spinStatesCrud.GetSpinStateByIdAsync((int)model.SpinStateId).ConfigureAwait(false) ?? throw new NullParameterException(nameof(spinState), $"The {nameof(spinState)} with ID = {model.SpinStateId} is null (does not exist).");
		}

		DynamicParameters p = new();
		p.Add("@Id", model.Id);
		p.Add("@Name", model.Name);
		p.Add("@Keyword", model.Keyword);
		p.Add("@ElectronicStateMethodFamilyId", model.ElectronicStateMethodFamilyId);
		p.Add("@SpinStateId", model.SpinStateId);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);
		_ = await _dbData.SaveDataAsync(Resources.SpinStatesElectronicStatesMethodFamiliesUpdate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		model.LastUpdatedDate = DateTime.Now;

		SpinStateElectronicStateMethodFamilyFullModel output = new()
		{
			Id = model.Id,
			Name = model.Name,
			Keyword = model.Keyword,
			ElectronicStateMethodFamily = electronicStateMethodFamily,
			SpinState = spinState,
			DescriptionRtf = model.DescriptionRtf,
			DescriptionText = model.DescriptionText,
			CreatedDate = model.CreatedDate,
			LastUpdatedDate = model.LastUpdatedDate,
			Archived = model.Archived
		};

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(CreateNewSpinStateElectronicStateMethodFamilyAsync), nameof(SpinStateElectronicStateMethodFamilyFullModel), output);
		}

		return output;
	}

	/// <inheritdoc/>
	public async Task DeleteSpinStateElectronicStateMethodFamilyAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(DeleteSpinStateElectronicStateMethodFamilyAsync), id);
		}

		// First check if used in any Full Methods
		DynamicParameters p = new();
		p.Add("@SpinStateElectronicStateMethodFamilyId", id);
		List<FullMethodSimpleModel> fullMethods = await _dbData.LoadDataAsync<FullMethodSimpleModel, dynamic>(Resources.FullMethodsGetBySpinStateElectronicStateMethodFamilyId, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (fullMethods.Count > 0)
		{
			throw new ValueInUseException(nameof(SpinStateElectronicStateMethodFamilyFullModel), $"Cannot delete Spin State/Electronic State/Method Family Combination with Id {id} because it is in use by one or more Full Methods.");
		}

		p = new();
		p.Add("@Id", id);
		_ = await _dbData.SaveDataAsync(Resources.SpinStatesElectronicStatesMethodFamiliesDelete, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesCrud), nameof(DeleteSpinStateElectronicStateMethodFamilyAsync));
		}
	}
}
