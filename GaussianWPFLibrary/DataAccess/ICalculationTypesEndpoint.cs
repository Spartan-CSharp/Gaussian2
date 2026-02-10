using GaussianCommonLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for API operations related to Calculation Types.
/// </summary>
public interface ICalculationTypesEndpoint
{
	/// <summary>
	/// Creates a new Calculation Type.
	/// </summary>
	/// <param name="model">The Calculation Type data to create.</param>
	/// <returns>The created Calculation Type with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<CalculationTypeFullModel?> CreateAsync(CalculationTypeFullModel model);

	/// <summary>
	/// Deletes a Calculation Type by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to delete.</param>
	/// <returns>A task that represents the asynchronous delete operation.</returns>
	Task DeleteAsync(int id);

	/// <summary>
	/// Retrieves all Calculation Types with full details.
	/// </summary>
	/// <returns>A list of all Calculation Types with full details, or <see langword="null"/> if none exist.</returns>
	Task<List<CalculationTypeFullModel>?> GetAllAsync();

	/// <summary>
	/// Retrieves a specific Calculation Type by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type.</param>
	/// <returns>The Calculation Type with full details, or <see langword="null"/> if not found.</returns>
	Task<CalculationTypeFullModel?> GetByIdAsync(int id);

	/// <summary>
	/// Updates an existing Calculation Type.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to update.</param>
	/// <param name="model">The updated Calculation Type data.</param>
	/// <returns>The updated Calculation Type with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<CalculationTypeFullModel?> UpdateAsync(int id, CalculationTypeFullModel model);
}