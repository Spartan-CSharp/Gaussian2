
namespace GaussianMVCLibrary.DataAccess;

public interface IDbData
{
	List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);
	Task<List<T>> LoadDataAsync<T, U>(string storedProcedure, U parameters, string connectionStringName);
	int SaveData<T>(string storedProcedure, T parameters, string connectionStringName);
	Task<int> SaveDataAsync<T>(string storedProcedure, T parameters, string connectionStringName);
}