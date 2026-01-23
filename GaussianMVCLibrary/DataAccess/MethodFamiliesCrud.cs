using System.Data;

using Dapper;

using GaussianCommonLibrary.Models;

using GaussianMVCLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Provides CRUD (Create, Read, Update, Delete) operations for Method Families.
/// </summary>
/// <param name="dbData">The database data access interface for executing database operations.</param>
/// <param name="logger">The logger instance for recording operation traces and diagnostics.</param>
public class MethodFamiliesCrud(IDbData dbData, ILogger<MethodFamiliesCrud> logger) : IMethodFamiliesCrud
{
	private readonly IDbData _dbData = dbData;
	private readonly ILogger<MethodFamiliesCrud> _logger = logger;

	/// <summary>
	/// Creates a new Method Family in the database.
	/// </summary>
	/// <param name="model">The Method Family model containing the data to create.</param>
	/// <returns>The created Method Family with the generated Id, CreatedDate, and LastUpdatedDate populated.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the provided model is null.</exception>
	public async Task<MethodFamilyFullModel> CreateNewMethodFamilyAsync(MethodFamilyFullModel model)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {MethodFamilyFullModel}", nameof(CreateNewMethodFamilyAsync), model);
		}

		if (model is null)
		{
			ArgumentNullException ex = new(nameof(model), $"The {nameof(model)} provided is null.");

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} Called with a null model", nameof(CreateNewMethodFamilyAsync));
			}

			throw ex;
		}

		DynamicParameters p = new();
		p.Add("@Keyword", model.Keyword);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);
		p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

		_ = await _dbData.SaveDataAsync(Resources.MethodFamiliesCreate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		model.Id = p.Get<int>("@Id");
		model.CreatedDate = DateTime.Now;
		model.LastUpdatedDate = DateTime.Now;

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Returning {MethodFamilyFullModel}", nameof(CreateNewMethodFamilyAsync), model);
		}

		return model;
	}

	/// <summary>
	/// Retrieves all Method Families from the database.
	/// </summary>
	/// <returns>A list of all Method Families.</returns>
	public async Task<List<MethodFamilyFullModel>> GetAllMethodFamiliesAsync()
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called", nameof(GetAllMethodFamiliesAsync));
		}

		DynamicParameters p = new();
		List<MethodFamilyFullModel> output = await _dbData.LoadDataAsync<MethodFamilyFullModel, dynamic>(Resources.MethodFamiliesGetAll, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Returning {Count}", nameof(GetAllMethodFamiliesAsync), output.Count);
		}

		return output;
	}

	/// <summary>
	/// Retrieves a specific Method Family by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to retrieve.</param>
	/// <returns>The Method Family matching the specified Id.</returns>
	/// <exception cref="InvalidOperationException">Thrown when no Method Family is found with the specified Id.</exception>
	public async Task<MethodFamilyFullModel?> GetMethodFamilyByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {Id}", nameof(GetMethodFamilyByIdAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);
		List<MethodFamilyFullModel?> output = await _dbData.LoadDataAsync<MethodFamilyFullModel?, dynamic>(Resources.MethodFamiliesGetById, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Returning {MethodFamilyFullModel}", nameof(GetMethodFamilyByIdAsync), output.First());
		}

		return output.FirstOrDefault();
	}

	/// <summary>
	/// Updates an existing Method Family in the database.
	/// </summary>
	/// <param name="model">The Method Family model containing the updated data.</param>
	/// <returns>The updated Method Family with the LastUpdatedDate refreshed.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the provided model is null.</exception>
	public async Task<MethodFamilyFullModel> UpdateMethodFamilyAsync(MethodFamilyFullModel model)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {MethodFamilyFullModel}", nameof(UpdateMethodFamilyAsync), model);
		}

		if (model is null)
		{
			ArgumentNullException ex = new(nameof(model), $"The {nameof(model)} provided is null.");

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} Called with a null model", nameof(UpdateMethodFamilyAsync));
			}

			throw ex;
		}

		DynamicParameters p = new();
		p.Add("@Id", model.Id);
		p.Add("@Keyword", model.Keyword);
		p.Add("@DescriptionRtf", model.DescriptionRtf);
		p.Add("@DescriptionText", model.DescriptionText);

		_ = await _dbData.SaveDataAsync(Resources.MethodFamiliesUpdate, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);

		model.LastUpdatedDate = DateTime.Now;

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Returning {MethodFamilyFullModel}", nameof(UpdateMethodFamilyAsync), model);
		}

		return model;
	}

	/// <summary>
	/// Deletes a Method Family from the database by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to delete.</param>
	/// <returns>A task representing the asynchronous delete operation.</returns>
	public async Task DeleteMethodFamilyAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {Id}", nameof(DeleteMethodFamilyAsync), id);
		}

		DynamicParameters p = new();
		p.Add("@Id", id);

		_ = await _dbData.SaveDataAsync(Resources.MethodFamiliesDelete, p, Resources.DataDatabaseConnectionString).ConfigureAwait(false);
	}
}
