using GaussianCommonLibrary.Models;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Defines CRUD operations for managing electronic states.
/// </summary>
public interface IElectronicStatesCrud
{
	/// <summary>
	/// Creates a new electronic state in the data store.
	/// </summary>
	/// <param name="model">The electronic state model to create.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the created electronic state with the generated ID, created date, and last updated date populated.</returns>
	Task<ElectronicStateFullModel> CreateNewElectronicStateAsync(ElectronicStateFullModel model);

	/// <summary>
	/// Deletes an electronic state from the data store.
	/// </summary>
	/// <param name="id">The unique identifier of the electronic state to delete.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task DeleteElectronicStateAsync(int id);

	/// <summary>
	/// Retrieves all electronic states from the data store.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all electronic states.</returns>
	Task<List<ElectronicStateFullModel>> GetAllElectronicStatesAsync();

	/// <summary>
	/// Retrieves a specific electronic state by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the electronic state to retrieve.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the electronic state if found; otherwise, null.</returns>
	Task<ElectronicStateFullModel?> GetElectronicStateByIdAsync(int id);

	/// <summary>
	/// Updates an existing electronic state in the data store.
	/// </summary>
	/// <param name="model">The electronic state model containing updated values.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the updated electronic state with the last updated date refreshed.</returns>
	Task<ElectronicStateFullModel> UpdateElectronicStateAsync(ElectronicStateFullModel model);
}