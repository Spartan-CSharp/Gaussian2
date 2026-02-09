using GaussianCommonLibrary.Models;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Defines CRUD operations for managing spin states.
/// </summary>
public interface ISpinStatesCrud
{
	/// <summary>
	/// Creates a new spin state in the data store.
	/// </summary>
	/// <param name="model">The spin state model to create.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the created spin state with the generated ID, created date, and last updated date populated.</returns>
	Task<SpinStateFullModel> CreateNewSpinStateAsync(SpinStateFullModel model);

	/// <summary>
	/// Deletes an spin state from the data store.
	/// </summary>
	/// <param name="id">The unique identifier of the spin state to delete.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task DeleteSpinStateAsync(int id);

	/// <summary>
	/// Retrieves all spin states from the data store.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all spin states.</returns>
	Task<List<SpinStateFullModel>> GetAllSpinStatesAsync();

	/// <summary>
	/// Retrieves a specific spin state by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the spin state to retrieve.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the spin state if found; otherwise, null.</returns>
	Task<SpinStateFullModel?> GetSpinStateByIdAsync(int id);

	/// <summary>
	/// Updates an existing spin state in the data store.
	/// </summary>
	/// <param name="model">The spin state model containing updated values.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the updated spin state with the last updated date refreshed.</returns>
	Task<SpinStateFullModel> UpdateSpinStateAsync(SpinStateFullModel model);
}