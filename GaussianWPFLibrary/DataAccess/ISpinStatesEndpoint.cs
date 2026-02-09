using GaussianCommonLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for API operations related to spin states.
/// </summary>
public interface ISpinStatesEndpoint
{
	/// <summary>
	/// Creates a new spin state.
	/// </summary>
	/// <param name="model">The spin state data to create.</param>
	/// <returns>The created spin state with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<SpinStateFullModel?> CreateAsync(SpinStateFullModel model);

	/// <summary>
	/// Deletes an spin state by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the spin state to delete.</param>
	/// <returns>A task that represents the asynchronous delete operation.</returns>
	Task DeleteAsync(int id);

	/// <summary>
	/// Retrieves all spin states with full details.
	/// </summary>
	/// <returns>A list of all spin states with full details, or <see langword="null"/> if none exist.</returns>
	Task<List<SpinStateFullModel>?> GetAllAsync();

	/// <summary>
	/// Retrieves a specific spin state by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the spin state.</param>
	/// <returns>The spin state with full details, or <see langword="null"/> if not found.</returns>
	Task<SpinStateFullModel?> GetByIdAsync(int id);

	/// <summary>
	/// Updates an existing spin state.
	/// </summary>
	/// <param name="id">The unique identifier of the spin state to update.</param>
	/// <param name="model">The updated spin state data.</param>
	/// <returns>The updated spin state with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<SpinStateFullModel?> UpdateAsync(int id, SpinStateFullModel model);
}