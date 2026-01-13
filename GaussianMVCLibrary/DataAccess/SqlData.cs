using System.Data;

using Dapper;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GaussianMVCLibrary.DataAccess;

public class SqlData(IConfiguration config) : IDbData
{
	private readonly IConfiguration _config = config;

	public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
	{
		string connectionString = _config.GetConnectionString(connectionStringName) ?? throw new InvalidOperationException($"Connection string {connectionStringName} not found.");
		using IDbConnection connection = new SqlConnection(connectionString);
		IEnumerable<T> rows = connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
		return [.. rows];
	}

	public async Task<List<T>> LoadDataAsync<T, U>(string storedProcedure, U parameters, string connectionStringName)
	{
		string connectionString = _config.GetConnectionString(connectionStringName) ?? throw new InvalidOperationException($"Connection string {connectionStringName} not found.");
		using IDbConnection connection = new SqlConnection(connectionString);
		IEnumerable<T> rows = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
		return [.. rows];
	}

	public int SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
	{
		string connectionString = _config.GetConnectionString(connectionStringName) ?? throw new InvalidOperationException($"Connection string {connectionStringName} not found.");
		using IDbConnection connection = new SqlConnection(connectionString);
		int rows = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
		return rows;
	}

	public async Task<int> SaveDataAsync<T>(string storedProcedure, T parameters, string connectionStringName)
	{
		string connectionString = _config.GetConnectionString(connectionStringName) ?? throw new InvalidOperationException($"Connection string {connectionStringName} not found.");
		using IDbConnection connection = new SqlConnection(connectionString);
		int rows = await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
		return rows;
	}
}
