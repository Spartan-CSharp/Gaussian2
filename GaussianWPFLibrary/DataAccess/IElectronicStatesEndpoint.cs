using GaussianCommonLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for API operations related to Electronic States.
/// </summary>
public interface IElectronicStatesEndpoint
{
	/// <summary>
	/// Creates a new Electronic State.
	/// </summary>
	/// <param name="model">The Electronic State data to create.</param>
	/// <returns>The created Electronic State with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<ElectronicStateFullModel?> CreateAsync(ElectronicStateFullModel model);

	/// <summary>
	/// Deletes an Electronic State by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State to delete.</param>
	/// <returns>A task that represents the asynchronous delete operation.</returns>
	Task DeleteAsync(int id);

	/// <summary>
	/// Retrieves all Electronic States with full details.
	/// </summary>
	/// <returns>A list of all Electronic States with full details, or <see langword="null"/> if none exist.</returns>
	Task<List<ElectronicStateFullModel>?> GetAllAsync();

	/// <summary>
	/// Retrieves a specific Electronic State by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State.</param>
	/// <returns>The Electronic State with full details, or <see langword="null"/> if not found.</returns>
	Task<ElectronicStateFullModel?> GetByIdAsync(int id);

	/// <summary>
	/// Retrieves a simplified list of all Electronic States as records.
	/// </summary>
	/// <returns>A list of all Electronic States as records, or <see langword="null"/> if none exist.</returns>
	Task<List<ElectronicStateRecord>?> GetListAsync();

	/// <summary>
	/// Updates an existing Electronic State.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State to update.</param>
	/// <param name="model">The updated Electronic State data.</param>
	/// <returns>The updated Electronic State with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<ElectronicStateFullModel?> UpdateAsync(int id, ElectronicStateFullModel model);
}