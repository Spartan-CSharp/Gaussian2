using System.Data;

using Dapper;

using GaussianCommonLibrary.Models;

using GaussianMVCLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Provides CRUD operations for managing Spin States.
/// </summary>
/// <param name="dbData">The database data access interface.</param>
/// <param name="logger">The logger instance for this class.</param>
public class SpinStatesCrud(IDbData dbData, ILogger<SpinStatesCrud> logger) : ISpinStatesCrud
{
	private readonly IDbData _dbData = dbData;
	private readonly ILogger<SpinStatesCrud> _logger = logger;

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when the provided model is null.</exception>
	public async Task<SpinStateFullModel> CreateNewSpinStateAsync(SpinStateFullModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(SpinStatesCrud), nameof(CreateNewSpinStateAsync), nameof(SpinStateFullModel), model);
		}

		ArgumentNullException.ThrowIfNull(model, nameof(model));
		DynamicParameters p = new();
		p.Add("@Name", model.Name);
		p.Add("@Keyword", model.Keyword);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);
		p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
		_ = await _dbData.SaveDataAsync(Resources.SpinStatesCreate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		model.Id = p.Get<int>("@Id");
		model.CreatedDate = DateTime.Now;
		model.LastUpdatedDate = DateTime.Now;

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(SpinStatesCrud), nameof(CreateNewSpinStateAsync), nameof(SpinStateFullModel), model);
		}

		return model;
	}

	/// <inheritdoc/>
	public async Task<List<SpinStateFullModel>> GetAllSpinStatesAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(SpinStatesCrud), nameof(GetAllSpinStatesAsync));
		}

		DynamicParameters p = new();
		List<SpinStateFullModel> output = await _dbData.LoadDataAsync<SpinStateFullModel, dynamic>(Resources.SpinStatesGetAll, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(SpinStatesCrud), nameof(GetAllSpinStatesAsync), output.Count, nameof(SpinStateFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	public async Task<SpinStateFullModel?> GetSpinStateByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(SpinStatesCrud), nameof(GetSpinStateByIdAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		List<SpinStateFullModel?> outputList = await _dbData.LoadDataAsync<SpinStateFullModel?, dynamic>(Resources.SpinStatesGetById, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		SpinStateFullModel? output = outputList.FirstOrDefault();

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(SpinStatesCrud), nameof(GetSpinStateByIdAsync), nameof(SpinStateFullModel), output);
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when the provided model is null.</exception>
	public async Task<SpinStateFullModel> UpdateSpinStateAsync(SpinStateFullModel model)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} called with {ModelName} {Model}.", nameof(SpinStatesCrud), nameof(UpdateSpinStateAsync), nameof(SpinStateFullModel), model);
		}

		ArgumentNullException.ThrowIfNull(model, nameof(model));
		DynamicParameters p = new();
		p.Add("@Id", model.Id);
		p.Add("@Name", model.Name);
		p.Add("@Keyword", model.Keyword);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);
		_ = await _dbData.SaveDataAsync(Resources.SpinStatesUpdate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		model.LastUpdatedDate = DateTime.Now;

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(SpinStatesCrud), nameof(UpdateSpinStateAsync), nameof(SpinStateFullModel), model);
		}

		return model;
	}

	/// <inheritdoc/>
	public async Task DeleteSpinStateAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(SpinStatesCrud), nameof(DeleteSpinStateAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		_ = await _dbData.SaveDataAsync(Resources.SpinStatesDelete, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning.", nameof(SpinStatesCrud), nameof(DeleteSpinStateAsync));
		}
	}
}
