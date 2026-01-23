using System.Data;

using Dapper;

using GaussianCommonLibrary.Models;

using GaussianMVCLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Provides CRUD (Create, Read, Update, Delete) operations for Calculation Types.
/// </summary>
/// <param name="dbData">The database data access interface for executing database operations.</param>
/// <param name="logger">The logger instance for recording operation traces and diagnostics.</param>
public class CalculationTypesCrud(IDbData dbData, ILogger<CalculationTypesCrud> logger) : ICalculationTypesCrud
{
	private readonly IDbData _dbData = dbData;
	private readonly ILogger<CalculationTypesCrud> _logger = logger;

	/// <summary>
	/// Creates a new Calculation Type in the database.
	/// </summary>
	/// <param name="model">The Calculation Type model containing the data to create.</param>
	/// <returns>The created Calculation Type with the generated Id, CreatedDate, and LastUpdatedDate populated.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the provided model is null.</exception>
	public async Task<CalculationTypeFullModel> CreateNewCalculationTypeAsync(CalculationTypeFullModel model)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {CalculationTypeFullModel}", nameof(CreateNewCalculationTypeAsync), model);
		}

		if (model is null)
		{
			ArgumentNullException ex = new(nameof(model), $"The {nameof(model)} provided is null.");

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} Called with a null model", nameof(CreateNewCalculationTypeAsync));
			}

			throw ex;
		}

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
			_logger.LogTrace("{Method} Returning {CalculationTypeFullModel}", nameof(CreateNewCalculationTypeAsync), model);
		}

		return model;
	}

	/// <summary>
	/// Retrieves all Calculation Types from the database.
	/// </summary>
	/// <returns>A list of all Calculation Types.</returns>
	public async Task<List<CalculationTypeFullModel>> GetAllCalculationTypesAsync()
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called", nameof(GetAllCalculationTypesAsync));
		}

		DynamicParameters p = new();
		List<CalculationTypeFullModel> output = await _dbData.LoadDataAsync<CalculationTypeFullModel, dynamic>(Resources.CalculationTypesGetAll, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Returning {Count}", nameof(GetAllCalculationTypesAsync), output.Count);
		}

		return output;
	}

	/// <summary>
	/// Retrieves a specific Calculation Type by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to retrieve.</param>
	/// <returns>The Calculation Type matching the specified Id.</returns>
	/// <exception cref="InvalidOperationException">Thrown when no Calculation Type is found with the specified Id.</exception>
	public async Task<CalculationTypeFullModel?> GetCalculationTypeByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {Id}", nameof(GetCalculationTypeByIdAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		List<CalculationTypeFullModel?> output = await _dbData.LoadDataAsync<CalculationTypeFullModel?, dynamic>(Resources.CalculationTypesGetById, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Returning {CalculationTypeFullModel}", nameof(GetCalculationTypeByIdAsync), output.First());
		}

		return output.FirstOrDefault();
	}

	/// <summary>
	/// Updates an existing Calculation Type in the database.
	/// </summary>
	/// <param name="model">The Calculation Type model containing the updated data.</param>
	/// <returns>The updated Calculation Type with the LastUpdatedDate refreshed.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the provided model is null.</exception>
	public async Task<CalculationTypeFullModel> UpdateCalculationTypeAsync(CalculationTypeFullModel model)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {CalculationTypeFullModel}", nameof(UpdateCalculationTypeAsync), model);
		}

		if (model is null)
		{
			ArgumentNullException ex = new(nameof(model), $"The {nameof(model)} provided is null.");

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} Called with a null model", nameof(UpdateCalculationTypeAsync));
			}

			throw ex;
		}

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
			_logger.LogTrace("{Method} Returning {CalculationTypeFullModel}", nameof(UpdateCalculationTypeAsync), model);
		}

		return model;
	}

	/// <summary>
	/// Deletes a Calculation Type from the database by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to delete.</param>
	/// <returns>A task representing the asynchronous delete operation.</returns>
	public async Task DeleteCalculationTypeAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {Id}", nameof(DeleteCalculationTypeAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);

		_ = await _dbData.SaveDataAsync(Resources.CalculationTypesDelete, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
	}
}
