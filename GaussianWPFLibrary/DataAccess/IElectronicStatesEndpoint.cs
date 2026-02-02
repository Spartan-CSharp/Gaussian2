using GaussianCommonLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for API operations related to electronic states.
/// </summary>
public interface IElectronicStatesEndpoint
{
	/// <summary>
	/// Creates a new electronic state.
	/// </summary>
	/// <param name="model">The electronic state data to create.</param>
	/// <returns>The created electronic state with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<ElectronicStateFullModel?> CreateAsync(ElectronicStateFullModel model);

	/// <summary>
	/// Deletes an electronic state by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the electronic state to delete.</param>
	/// <returns>A task that represents the asynchronous delete operation.</returns>
	Task DeleteAsync(int id);

	/// <summary>
	/// Retrieves all electronic states with full details.
	/// </summary>
	/// <returns>A list of all electronic states with full details, or <see langword="null"/> if none exist.</returns>
	Task<List<ElectronicStateFullModel>?> GetAllAsync();

	/// <summary>
	/// Retrieves a specific electronic state by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the electronic state.</param>
	/// <returns>The electronic state with full details, or <see langword="null"/> if not found.</returns>
	Task<ElectronicStateFullModel?> GetByIdAsync(int id);

	/// <summary>
	/// Updates an existing electronic state.
	/// </summary>
	/// <param name="id">The unique identifier of the electronic state to update.</param>
	/// <param name="model">The updated electronic state data.</param>
	/// <returns>The updated electronic state with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<ElectronicStateFullModel?> UpdateAsync(int id, ElectronicStateFullModel model);
}