using GaussianCommonLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for API operations related to Spin States.
/// </summary>
public interface ISpinStatesEndpoint
{
	/// <summary>
	/// Creates a new Spin State.
	/// </summary>
	/// <param name="model">The Spin State data to create.</param>
	/// <returns>The created Spin State with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<SpinStateFullModel?> CreateAsync(SpinStateFullModel model);

	/// <summary>
	/// Deletes an Spin State by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State to delete.</param>
	/// <returns>A task that represents the asynchronous delete operation.</returns>
	Task DeleteAsync(int id);

	/// <summary>
	/// Retrieves all Spin States with full details.
	/// </summary>
	/// <returns>A list of all Spin States with full details, or <see langword="null"/> if none exist.</returns>
	Task<List<SpinStateFullModel>?> GetAllAsync();

	/// <summary>
	/// Retrieves a specific Spin State by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State.</param>
	/// <returns>The Spin State with full details, or <see langword="null"/> if not found.</returns>
	Task<SpinStateFullModel?> GetByIdAsync(int id);

	/// <summary>
	/// Updates an existing Spin State.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State to update.</param>
	/// <param name="model">The updated Spin State data.</param>
	/// <returns>The updated Spin State with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<SpinStateFullModel?> UpdateAsync(int id, SpinStateFullModel model);
}