using System.Data;

using Dapper;

using GaussianCommonLibrary.Models;

using GaussianMVCLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Provides CRUD operations for managing Calculation Types.
/// </summary>
/// <param name="dbData">The database data access interface for executing database operations.</param>
/// <param name="logger">The logger instance for recording operation traces and diagnostics.</param>
public class CalculationTypesCrud(IDbData dbData, ILogger<CalculationTypesCrud> logger) : ICalculationTypesCrud
{
	private readonly IDbData _dbData = dbData;
	private readonly ILogger<CalculationTypesCrud> _logger = logger;

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when the provided model is null.</exception>
	public async Task<CalculationTypeFullModel> CreateNewCalculationTypeAsync(CalculationTypeFullModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(CalculationTypesCrud), nameof(CreateNewCalculationTypeAsync), nameof(CalculationTypeFullModel), model);
		}

		ArgumentNullException.ThrowIfNull(model, nameof(model));
		DynamicParameters p = new();
		p.Add("@Name", model.Name);
		p.Add("@Keyword", model.Keyword);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);
		p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
		_ = await _dbData.SaveDataAsync(Resources.CalculationTypesCreate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		model.Id = p.Get<int>("@Id");
		model.CreatedDate = DateTime.Now;
		model.LastUpdatedDate = DateTime.Now;

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(CalculationTypesCrud), nameof(CreateNewCalculationTypeAsync), nameof(CalculationTypeFullModel), model);
		}

		return model;
	}

	/// <inheritdoc/>
	public async Task<List<CalculationTypeFullModel>> GetAllCalculationTypesAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(CalculationTypesCrud), nameof(GetAllCalculationTypesAsync));
		}

		DynamicParameters p = new();
		List<CalculationTypeFullModel> output = await _dbData.LoadDataAsync<CalculationTypeFullModel, dynamic>(Resources.CalculationTypesGetAll, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(CalculationTypesCrud), nameof(GetAllCalculationTypesAsync), output.Count, nameof(CalculationTypeFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	public async Task<CalculationTypeFullModel?> GetCalculationTypeByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(CalculationTypesCrud), nameof(GetCalculationTypeByIdAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		List<CalculationTypeFullModel?> outputList = await _dbData.LoadDataAsync<CalculationTypeFullModel?, dynamic>(Resources.CalculationTypesGetById, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		CalculationTypeFullModel? output = outputList.FirstOrDefault();

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(CalculationTypesCrud), nameof(GetCalculationTypeByIdAsync), nameof(CalculationTypeFullModel), output);
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when the provided model is null.</exception>
	public async Task<CalculationTypeFullModel> UpdateCalculationTypeAsync(CalculationTypeFullModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(CalculationTypesCrud), nameof(UpdateCalculationTypeAsync), nameof(CalculationTypeFullModel), model);
		}

		ArgumentNullException.ThrowIfNull(model, nameof(model));
		DynamicParameters p = new();
		p.Add("@Id", model.Id);
		p.Add("@Name", model.Name);
		p.Add("@Keyword", model.Keyword);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);
		_ = await _dbData.SaveDataAsync(Resources.CalculationTypesUpdate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		model.LastUpdatedDate = DateTime.Now;

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(CalculationTypesCrud), nameof(UpdateCalculationTypeAsync), nameof(CalculationTypeFullModel), model);
		}

		return model;
	}

	/// <inheritdoc/>
	public async Task DeleteCalculationTypeAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(CalculationTypesCrud), nameof(DeleteCalculationTypeAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		_ = await _dbData.SaveDataAsync(Resources.CalculationTypesDelete, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning.", nameof(CalculationTypesCrud), nameof(DeleteCalculationTypeAsync));
		}
	}
}
