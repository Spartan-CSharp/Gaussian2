using GaussianCommonLibrary.Models;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Defines CRUD operations for managing Spin States.
/// </summary>
public interface ISpinStatesCrud
{
	/// <summary>
	/// Creates a new Spin State in the data store.
	/// </summary>
	/// <param name="model">The Spin State model to create.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the created Spin State with the generated ID, created date, and last updated date populated.</returns>
	Task<SpinStateFullModel> CreateNewSpinStateAsync(SpinStateFullModel model);

	/// <summary>
	/// Deletes an Spin State from the data store.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State to delete.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task DeleteSpinStateAsync(int id);

	/// <summary>
	/// Retrieves all Spin States from the data store.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all Spin States.</returns>
	Task<List<SpinStateFullModel>> GetAllSpinStatesAsync();

	/// <summary>
	/// Retrieves a specific Spin State by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State to retrieve.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the Spin State if found; otherwise, null.</returns>
	Task<SpinStateFullModel?> GetSpinStateByIdAsync(int id);

	/// <summary>
	/// Updates an existing Spin State in the data store.
	/// </summary>
	/// <param name="model">The Spin State model containing updated values.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the updated Spin State with the last updated date refreshed.</returns>
	Task<SpinStateFullModel> UpdateSpinStateAsync(SpinStateFullModel model);
}