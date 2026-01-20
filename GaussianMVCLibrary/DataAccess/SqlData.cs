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

	/// <summary>
	/// Synchronously loads data from the database by executing a stored procedure.
	/// </summary>
	/// <typeparam name="TDataType">The type of data to return.</typeparam>
	/// <typeparam name="TParams">The type of the parameters object.</typeparam>
	/// <param name="storedProcedure">The name of the stored procedure to execute.</param>
	/// <param name="parameters">The parameters to pass to the stored procedure.</param>
	/// <param name="connectionStringName">The name of the connection string in the configuration.</param>
	/// <returns>A collection of <typeparamref name="TDataType"/> objects returned from the stored procedure.</returns>
	/// <exception cref="InvalidOperationException">Thrown when the connection string is not found in the configuration.</exception>
	public Collection<TDataType> LoadData<TDataType, TParams>(string storedProcedure, TParams parameters, string connectionStringName)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method}<{DataType}, {ParamType}> Called with {StoredProcedure}, {Parameters}, {ConnectionStringName}", nameof(LoadData), typeof(TDataType).Name, typeof(TParams).Name, storedProcedure, parameters, connectionStringName);
		}

		string connectionString = _config.GetConnectionString(connectionStringName) ?? throw new InvalidOperationException($"Connection string {connectionStringName} not found.");
		using IDbConnection connection = new SqlConnection(connectionString);
		IEnumerable<TDataType> rows = connection.Query<TDataType>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method}<{DataType}, {ParamType}> Returning {Count}", nameof(LoadData), typeof(TDataType).Name, typeof(TParams).Name, rows.Count());
		}

		return [.. rows];
	}

	/// <summary>
	/// Asynchronously loads data from the database by executing a stored procedure.
	/// </summary>
	/// <typeparam name="TDataType">The type of data to return.</typeparam>
	/// <typeparam name="TParams">The type of the parameters object.</typeparam>
	/// <param name="storedProcedure">The name of the stored procedure to execute.</param>
	/// <param name="parameters">The parameters to pass to the stored procedure.</param>
	/// <param name="connectionStringName">The name of the connection string in the configuration.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of <typeparamref name="TDataType"/> objects returned from the stored procedure.</returns>
	/// <exception cref="InvalidOperationException">Thrown when the connection string is not found in the configuration.</exception>
	public async Task<List<TDataType>> LoadDataAsync<TDataType, TParams>(string storedProcedure, TParams parameters, string connectionStringName)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method}<{DataType}, {ParamType}> Called with {StoredProcedure}, {Parameters}, {ConnectionStringName}", nameof(LoadDataAsync), typeof(TDataType).Name, typeof(TParams).Name, storedProcedure, parameters, connectionStringName);
		}

		string connectionString = _config.GetConnectionString(connectionStringName) ?? throw new InvalidOperationException($"Connection string {connectionStringName} not found.");
		using IDbConnection connection = new SqlConnection(connectionString);
		IEnumerable<TDataType> rows = await connection.QueryAsync<TDataType>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method}<{DataType}, {ParamType}> Returning {Count}", nameof(LoadDataAsync), typeof(TDataType).Name, typeof(TParams).Name, rows.Count());
		}

		return [.. rows];
	}

	/// <summary>
	/// Synchronously saves data to the database by executing a stored procedure.
	/// </summary>
	/// <typeparam name="TParams">The type of the parameters object.</typeparam>
	/// <param name="storedProcedure">The name of the stored procedure to execute.</param>
	/// <param name="parameters">The parameters to pass to the stored procedure.</param>
	/// <param name="connectionStringName">The name of the connection string in the configuration.</param>
	/// <returns>The number of rows affected by the stored procedure execution.</returns>
	/// <exception cref="InvalidOperationException">Thrown when the connection string is not found in the configuration.</exception>
	public int SaveData<TParams>(string storedProcedure, TParams parameters, string connectionStringName)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method}<{ParamType}> Called with {StoredProcedure}, {Parameters}, {ConnectionStringName}", nameof(SaveData), typeof(TParams).Name, storedProcedure, parameters, connectionStringName);
		}

		string connectionString = _config.GetConnectionString(connectionStringName) ?? throw new InvalidOperationException($"Connection string {connectionStringName} not found.");
		using IDbConnection connection = new SqlConnection(connectionString);
		int rows = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method}<{ParamType}> Returning {Count}", nameof(SaveData), typeof(TParams).Name, rows);
		}

		return rows;
	}

	/// <summary>
	/// Asynchronously saves data to the database by executing a stored procedure.
	/// </summary>
	/// <typeparam name="TParams">The type of the parameters object.</typeparam>
	/// <param name="storedProcedure">The name of the stored procedure to execute.</param>
	/// <param name="parameters">The parameters to pass to the stored procedure.</param>
	/// <param name="connectionStringName">The name of the connection string in the configuration.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected by the stored procedure execution.</returns>
	/// <exception cref="InvalidOperationException">Thrown when the connection string is not found in the configuration.</exception>
	public async Task<int> SaveDataAsync<TParams>(string storedProcedure, TParams parameters, string connectionStringName)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method}<{ParamType}> Called with {StoredProcedure}, {Parameters}, {ConnectionStringName}", nameof(SaveDataAsync), typeof(TParams).Name, storedProcedure, parameters, connectionStringName);
		}

		string connectionString = _config.GetConnectionString(connectionStringName) ?? throw new InvalidOperationException($"Connection string {connectionStringName} not found.");
		using IDbConnection connection = new SqlConnection(connectionString);
		int rows = await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method}<{ParamType}> Returning {Count}", nameof(SaveDataAsync), typeof(TParams).Name, rows);
		}

		return rows;
	}
}
