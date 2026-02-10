using GaussianCommonLibrary.Models;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Defines CRUD operations for managing Electronic States.
/// </summary>
public interface IElectronicStatesCrud
{
	/// <summary>
	/// Creates a new Electronic State in the data store.
	/// </summary>
	/// <param name="model">The Electronic State model to create.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the created Electronic State with the generated ID, created date, and last updated date populated.</returns>
	Task<ElectronicStateFullModel> CreateNewElectronicStateAsync(ElectronicStateFullModel model);

	/// <summary>
	/// Deletes an Electronic State from the data store.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State to delete.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task DeleteElectronicStateAsync(int id);

	/// <summary>
	/// Retrieves all Electronic States from the data store.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all Electronic States.</returns>
	Task<List<ElectronicStateFullModel>> GetAllElectronicStatesAsync();

	/// <summary>
	/// Retrieves a specific Electronic State by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State to retrieve.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the Electronic State if found; otherwise, null.</returns>
	Task<ElectronicStateFullModel?> GetElectronicStateByIdAsync(int id);

	/// <summary>
	/// Retrieves a simplified list of Electronic States from the database.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Electronic State records with basic identifying information.</returns>
	Task<List<ElectronicStateRecord>> GetElectronicStateListAsync();

	/// <summary>
	/// Updates an existing Electronic State in the data store.
	/// </summary>
	/// <param name="model">The Electronic State model containing updated values.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the updated Electronic State with the last updated date refreshed.</returns>
	Task<ElectronicStateFullModel> UpdateElectronicStateAsync(ElectronicStateFullModel model);
}