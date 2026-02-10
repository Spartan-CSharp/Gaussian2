using System.Data;

using Dapper;

using GaussianCommonLibrary.Models;

using GaussianMVCLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Provides CRUD operations for managing Electronic States.
/// </summary>
/// <param name="dbData">The database data access interface.</param>
/// <param name="logger">The logger instance for this class.</param>
public class ElectronicStatesCrud(IDbData dbData, ILogger<ElectronicStatesCrud> logger) : IElectronicStatesCrud
{
	private readonly IDbData _dbData = dbData;
	private readonly ILogger<ElectronicStatesCrud> _logger = logger;

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when the provided model is null.</exception>
	public async Task<ElectronicStateFullModel> CreateNewElectronicStateAsync(ElectronicStateFullModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(ElectronicStatesCrud), nameof(CreateNewElectronicStateAsync), nameof(ElectronicStateFullModel), model);
		}

		ArgumentNullException.ThrowIfNull(model, nameof(model));
		DynamicParameters p = new();
		p.Add("@Name", model.Name);
		p.Add("@Keyword", model.Keyword);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);
		p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
		_ = await _dbData.SaveDataAsync(Resources.ElectronicStatesCreate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		model.Id = p.Get<int>("@Id");
		model.CreatedDate = DateTime.Now;
		model.LastUpdatedDate = DateTime.Now;

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(ElectronicStatesCrud), nameof(CreateNewElectronicStateAsync), nameof(ElectronicStateFullModel), model);
		}

		return model;
	}

	/// <inheritdoc/>
	public async Task<List<ElectronicStateFullModel>> GetAllElectronicStatesAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(ElectronicStatesCrud), nameof(GetAllElectronicStatesAsync));
		}

		DynamicParameters p = new();
		List<ElectronicStateFullModel> output = await _dbData.LoadDataAsync<ElectronicStateFullModel, dynamic>(Resources.ElectronicStatesGetAll, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(ElectronicStatesCrud), nameof(GetAllElectronicStatesAsync), output.Count, nameof(ElectronicStateFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	public async Task<List<ElectronicStateRecord>> GetElectronicStateListAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(ElectronicStatesCrud), nameof(GetElectronicStateListAsync));
		}

		DynamicParameters p = new();
		List<ElectronicStateRecord> output = await _dbData.LoadDataAsync<ElectronicStateRecord, dynamic>(Resources.ElectronicStatesGetList, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(ElectronicStatesCrud), nameof(GetElectronicStateListAsync), output.Count, nameof(ElectronicStateRecord));
		}

		return output;
	}

	/// <inheritdoc/>
	public async Task<ElectronicStateFullModel?> GetElectronicStateByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(ElectronicStatesCrud), nameof(GetElectronicStateByIdAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		List<ElectronicStateFullModel?> outputList = await _dbData.LoadDataAsync<ElectronicStateFullModel?, dynamic>(Resources.ElectronicStatesGetById, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		ElectronicStateFullModel? output = outputList.FirstOrDefault();

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(ElectronicStatesCrud), nameof(GetElectronicStateByIdAsync), nameof(ElectronicStateFullModel), output);
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when the provided model is null.</exception>
	public async Task<ElectronicStateFullModel> UpdateElectronicStateAsync(ElectronicStateFullModel model)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} called with {ModelName} {Model}.", nameof(ElectronicStatesCrud), nameof(UpdateElectronicStateAsync), nameof(ElectronicStateFullModel), model);
		}

		ArgumentNullException.ThrowIfNull(model, nameof(model));
		DynamicParameters p = new();
		p.Add("@Id", model.Id);
		p.Add("@Name", model.Name);
		p.Add("@Keyword", model.Keyword);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);
		_ = await _dbData.SaveDataAsync(Resources.ElectronicStatesUpdate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		model.LastUpdatedDate = DateTime.Now;

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(ElectronicStatesCrud), nameof(UpdateElectronicStateAsync), nameof(ElectronicStateFullModel), model);
		}

		return model;
	}

	/// <inheritdoc/>
	public async Task DeleteElectronicStateAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(ElectronicStatesCrud), nameof(DeleteElectronicStateAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		_ = await _dbData.SaveDataAsync(Resources.ElectronicStatesDelete, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning.", nameof(ElectronicStatesCrud), nameof(DeleteElectronicStateAsync));
		}
	}
}
