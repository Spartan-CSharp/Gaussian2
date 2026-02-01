using System.Collections.ObjectModel;
using System.Data;

using Dapper;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Provides SQL Server database access functionality using Dapper for executing stored procedures.
/// </summary>
/// <param name="config">The configuration instance used to retrieve connection strings.</param>
/// <param name="logger">The logger instance for recording operation traces and diagnostics.</param>
public class SqlData(IConfiguration config, ILogger<SqlData> logger) : IDbData
{
	private readonly IConfiguration _config = config;
	private readonly ILogger<SqlData> _logger = logger;

	/// <inheritdoc/>
	/// <exception cref="InvalidOperationException">Thrown when the connection string is not found in the configuration.</exception>
	public Collection<TDataType> LoadData<TDataType, TParams>(string storedProcedure, TParams parameters, string connectionStringName)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method}<{DataType}, {ParamType}> called with {StoredProcedure}, {Parameters}, {ConnectionStringName}.", nameof(SqlData), nameof(LoadData), typeof(TDataType).Name, typeof(TParams).Name, storedProcedure, parameters, connectionStringName);
		}

		string connectionString = _config.GetConnectionString(connectionStringName) ?? throw new InvalidOperationException($"Connection string {connectionStringName} not found.");
		using IDbConnection connection = new SqlConnection(connectionString);
		IEnumerable<TDataType> rows = connection.Query<TDataType>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method}<{DataType}, {ParamType}> returning {Count} {DataType}.", nameof(SqlData), nameof(LoadData), typeof(TDataType).Name, typeof(TParams).Name, rows.Count(), typeof(TDataType).Name);
		}

		return [.. rows];
	}

	/// <inheritdoc/>
	/// <exception cref="InvalidOperationException">Thrown when the connection string is not found in the configuration.</exception>
	public async Task<List<TDataType>> LoadDataAsync<TDataType, TParams>(string storedProcedure, TParams parameters, string connectionStringName)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method}<{DataType}, {ParamType}> called with {StoredProcedure}, {Parameters}, {ConnectionStringName}.", nameof(SqlData), nameof(LoadDataAsync), typeof(TDataType).Name, typeof(TParams).Name, storedProcedure, parameters, connectionStringName);
		}

		string connectionString = _config.GetConnectionString(connectionStringName) ?? throw new InvalidOperationException($"Connection string {connectionStringName} not found.");
		using IDbConnection connection = new SqlConnection(connectionString);
		IEnumerable<TDataType> rows = await connection.QueryAsync<TDataType>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method}<{DataType}, {ParamType}> returning {Count} {DataType}.", nameof(SqlData), nameof(LoadDataAsync), typeof(TDataType).Name, typeof(TParams).Name, rows.Count(), typeof(TDataType).Name);
		}

		return [.. rows];
	}

	/// <inheritdoc/>
	/// <exception cref="InvalidOperationException">Thrown when the connection string is not found in the configuration.</exception>
	public int SaveData<TParams>(string storedProcedure, TParams parameters, string connectionStringName)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method}<{ParamType}> called with {StoredProcedure}, {Parameters}, {ConnectionStringName}.", nameof(SqlData), nameof(SaveData), typeof(TParams).Name, storedProcedure, parameters, connectionStringName);
		}

		string connectionString = _config.GetConnectionString(connectionStringName) ?? throw new InvalidOperationException($"Connection string {connectionStringName} not found.");
		using IDbConnection connection = new SqlConnection(connectionString);
		int rows = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method}<{ParamType}> Returning {Count} rows affected.", nameof(SqlData), nameof(SaveData), typeof(TParams).Name, rows);
		}

		return rows;
	}

	/// <inheritdoc/>
	public async Task<int> SaveDataAsync<TParams>(string storedProcedure, TParams parameters, string connectionStringName)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method}<{ParamType}> called with {StoredProcedure}, {Parameters}, {ConnectionStringName}.", nameof(SqlData), nameof(SaveDataAsync), typeof(TParams).Name, storedProcedure, parameters, connectionStringName);
		}

		string connectionString = _config.GetConnectionString(connectionStringName) ?? throw new InvalidOperationException($"Connection string {connectionStringName} not found.");
		using IDbConnection connection = new SqlConnection(connectionString);
		int rows = await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method}<{ParamType}> Returning {Count} rows affected.", nameof(SqlData), nameof(SaveDataAsync), typeof(TParams).Name, rows);
		}

		return rows;
	}
}
