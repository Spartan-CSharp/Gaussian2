using System.Data;

using Dapper;

using GaussianCommonLibrary.ErrorModels;
using GaussianCommonLibrary.Models;

using GaussianMVCLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Provides CRUD operations for managing Method Families.
/// </summary>
/// <param name="dbData">The database data access interface for executing database operations.</param>
/// <param name="logger">The logger instance for recording operation traces and diagnostics.</param>
public class MethodFamiliesCrud(IDbData dbData, ILogger<MethodFamiliesCrud> logger) : IMethodFamiliesCrud
{
	private readonly IDbData _dbData = dbData;
	private readonly ILogger<MethodFamiliesCrud> _logger = logger;

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when the provided model is null.</exception>
	public async Task<MethodFamilyFullModel> CreateNewMethodFamilyAsync(MethodFamilyFullModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(MethodFamiliesCrud), nameof(CreateNewMethodFamilyAsync), nameof(MethodFamilyFullModel), model);
		}

		ArgumentNullException.ThrowIfNull(model, nameof(model));
		DynamicParameters p = new();
		p.Add("@Name", model.Name);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);
		p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
		_ = await _dbData.SaveDataAsync(Resources.MethodFamiliesCreate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		model.Id = p.Get<int>("@Id");
		model.CreatedDate = DateTime.Now;
		model.LastUpdatedDate = DateTime.Now;

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(MethodFamiliesCrud), nameof(CreateNewMethodFamilyAsync), nameof(MethodFamilyFullModel), model);
		}

		return model;
	}

	/// <inheritdoc/>
	public async Task<List<MethodFamilyFullModel>> GetAllMethodFamiliesAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(MethodFamiliesCrud), nameof(GetAllMethodFamiliesAsync));
		}

		DynamicParameters p = new();
		List<MethodFamilyFullModel> output = await _dbData.LoadDataAsync<MethodFamilyFullModel, dynamic>(Resources.MethodFamiliesGetAll, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(MethodFamiliesCrud), nameof(GetAllMethodFamiliesAsync), output.Count, nameof(MethodFamilyFullModel));
		}

		return output;
	}

	/// <inheritdoc/>
	public async Task<List<MethodFamilyRecord>> GetMethodFamilyListAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(MethodFamiliesCrud), nameof(GetMethodFamilyListAsync));
		}

		DynamicParameters p = new();
		List<MethodFamilyRecord> output = await _dbData.LoadDataAsync<MethodFamilyRecord, dynamic>(Resources.MethodFamiliesGetList, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(MethodFamiliesCrud), nameof(GetMethodFamilyListAsync), output.Count, nameof(MethodFamilyRecord));
		}

		return output;
	}

	/// <inheritdoc/>
	public async Task<MethodFamilyFullModel?> GetMethodFamilyByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(MethodFamiliesCrud), nameof(GetMethodFamilyByIdAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		List<MethodFamilyFullModel?> outputList = await _dbData.LoadDataAsync<MethodFamilyFullModel?, dynamic>(Resources.MethodFamiliesGetById, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		MethodFamilyFullModel? output = outputList.FirstOrDefault();

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(MethodFamiliesCrud), nameof(GetMethodFamilyByIdAsync), nameof(MethodFamilyFullModel), output);
		}

		return output;
	}

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when the provided model is null.</exception>
	public async Task<MethodFamilyFullModel> UpdateMethodFamilyAsync(MethodFamilyFullModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(MethodFamiliesCrud), nameof(UpdateMethodFamilyAsync), nameof(MethodFamilyFullModel), model);
		}

		ArgumentNullException.ThrowIfNull(model, nameof(model));
		DynamicParameters p = new();
		p.Add("@Id", model.Id);
		p.Add("@Name", model.Name);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);
		_ = await _dbData.SaveDataAsync(Resources.MethodFamiliesUpdate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
		model.LastUpdatedDate = DateTime.Now;

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(MethodFamiliesCrud), nameof(UpdateMethodFamilyAsync), nameof(MethodFamilyFullModel), model);
		}

		return model;
	}

	/// <inheritdoc/>
	/// <exception cref="ValueInUseException">Thrown when the Method Family is in use by one or more Base Methods and cannot be deleted.</exception>
	public async Task DeleteMethodFamilyAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(MethodFamiliesCrud), nameof(DeleteMethodFamilyAsync), id);
		}

		// First check if used in any Base Methods
		DynamicParameters p = new();
		p.Add("@MethodFamilyId", id);
		List<BaseMethodSimpleModel> baseMethods = await _dbData.LoadDataAsync<BaseMethodSimpleModel, dynamic>(Resources.BaseMethodsGetByMethodFamilyId, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (baseMethods.Count > 0)
		{
			throw new ValueInUseException(nameof(MethodFamilyFullModel), $"Cannot delete Method Family with Id {id} because it is in use by one or more Base Methods.");
		}

		// Second check if used in any Electronic State/Method Family Combinations
		p = new();
		p.Add("@MethodFamilyId", id);
		List<ElectronicStateMethodFamilySimpleModel> electronicStateMethodFamilies = await _dbData.LoadDataAsync<ElectronicStateMethodFamilySimpleModel, dynamic>(Resources.ElectronicStatesMethodFamiliesGetByMethodFamilyId, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (electronicStateMethodFamilies.Count > 0)
		{
			throw new ValueInUseException(nameof(MethodFamilyFullModel), $"Cannot delete Method Family with Id {id} because it is in use by one or more Electronic State/Method Family Combinations.");
		}

		p = new();
		p.Add("@Id", id);
		_ = await _dbData.SaveDataAsync(Resources.MethodFamiliesDelete, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning.", nameof(MethodFamiliesCrud), nameof(DeleteMethodFamilyAsync));
		}
	}
}
