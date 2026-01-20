using System.Collections.ObjectModel;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Defines a contract for database operations that execute stored procedures.
/// </summary>
/// <remarks>
/// This interface provides an abstraction layer for data access operations,
/// supporting both synchronous and asynchronous methods for loading and saving data
/// through stored procedures. Implementations should handle database connections,
/// command execution, and object mapping.
/// </remarks>
public interface IDbData
{
	/// <summary>
	/// Synchronously loads data from the database by executing a stored procedure.
	/// </summary>
	/// <typeparam name="TDataType">The type of data to return.</typeparam>
	/// <typeparam name="TParams">The type of the parameters object.</typeparam>
	/// <param name="storedProcedure">The name of the stored procedure to execute.</param>
	/// <param name="parameters">The parameters to pass to the stored procedure.</param>
	/// <param name="connectionStringName">The name of the connection string in the configuration.</param>
	/// <returns>A collection of <typeparamref name="TDataType"/> objects returned from the stored procedure.</returns>
	Collection<TDataType> LoadData<TDataType, TParams>(string storedProcedure, TParams parameters, string connectionStringName);

	/// <summary>
	/// Asynchronously loads data from the database by executing a stored procedure.
	/// </summary>
	/// <typeparam name="TDataType">The type of data to return.</typeparam>
	/// <typeparam name="TParams">The type of the parameters object.</typeparam>
	/// <param name="storedProcedure">The name of the stored procedure to execute.</param>
	/// <param name="parameters">The parameters to pass to the stored procedure.</param>
	/// <param name="connectionStringName">The name of the connection string in the configuration.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of <typeparamref name="TDataType"/> objects returned from the stored procedure.</returns>
	Task<List<TDataType>> LoadDataAsync<TDataType, TParams>(string storedProcedure, TParams parameters, string connectionStringName);

	/// <summary>
	/// Synchronously saves data to the database by executing a stored procedure.
	/// </summary>
	/// <typeparam name="TParams">The type of the parameters object.</typeparam>
	/// <param name="storedProcedure">The name of the stored procedure to execute.</param>
	/// <param name="parameters">The parameters to pass to the stored procedure.</param>
	/// <param name="connectionStringName">The name of the connection string in the configuration.</param>
	/// <returns>The number of rows affected by the stored procedure execution.</returns>
	int SaveData<TParams>(string storedProcedure, TParams parameters, string connectionStringName);

	/// <summary>
	/// Asynchronously saves data to the database by executing a stored procedure.
	/// </summary>
	/// <typeparam name="TParams">The type of the parameters object.</typeparam>
	/// <param name="storedProcedure">The name of the stored procedure to execute.</param>
	/// <param name="parameters">The parameters to pass to the stored procedure.</param>
	/// <param name="connectionStringName">The name of the connection string in the configuration.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected by the stored procedure execution.</returns>
	/// <exception cref="InvalidOperationException">Thrown when the connection string is not found in the configuration.</exception>
	Task<int> SaveDataAsync<TParams>(string storedProcedure, TParams parameters, string connectionStringName);
}
