using GaussianCommonLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for API operations related to calculation types.
/// </summary>
public interface ICalculationTypesEndpoint
{
	/// <summary>
	/// Creates a new calculation type.
	/// </summary>
	/// <param name="model">The calculation type data to create.</param>
	/// <returns>The created calculation type with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<CalculationTypeFullModel?> CreateAsync(CalculationTypeFullModel model);

	/// <summary>
	/// Deletes a calculation type by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the calculation type to delete.</param>
	/// <returns>A task that represents the asynchronous delete operation.</returns>
	Task DeleteAsync(int id);

	/// <summary>
	/// Retrieves all calculation types with full details.
	/// </summary>
	/// <returns>A list of all calculation types with full details, or <see langword="null"/> if none exist.</returns>
	Task<List<CalculationTypeFullModel>?> GetAllAsync();

	/// <summary>
	/// Retrieves a specific calculation type by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the calculation type.</param>
	/// <returns>The calculation type with full details, or <see langword="null"/> if not found.</returns>
	Task<CalculationTypeFullModel?> GetByIdAsync(int id);

	/// <summary>
	/// Updates an existing calculation type.
	/// </summary>
	/// <param name="id">The unique identifier of the calculation type to update.</param>
	/// <param name="model">The updated calculation type data.</param>
	/// <returns>The updated calculation type with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<CalculationTypeFullModel?> UpdateAsync(int id, CalculationTypeFullModel model);
}