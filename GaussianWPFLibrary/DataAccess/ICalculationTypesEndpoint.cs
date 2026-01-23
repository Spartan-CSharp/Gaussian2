using GaussianCommonLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for interacting with the Calculation Types API endpoint.
/// Provides methods for CRUD operations on Calculation Type resources.
/// </summary>
public interface ICalculationTypesEndpoint
{
	/// <summary>
	/// Creates a new Calculation Type via the API.
	/// </summary>
	/// <param name="model">The Calculation Type model containing the data for the new record.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains
	/// the created <see cref="CalculationTypeFullModel"/> with server-generated values, or <c>null</c>.
	/// </returns>
	Task<CalculationTypeFullModel?> CreateAsync(CalculationTypeFullModel model);

	/// <summary>
	/// Deletes a Calculation Type from the API.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to delete.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// </returns>
	Task DeleteAsync(int id);

	/// <summary>
	/// Retrieves all Calculation Types from the API.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains
	/// a list of <see cref="CalculationTypeFullModel"/> objects, or <c>null</c> if the response is empty.
	/// </returns>
	Task<List<CalculationTypeFullModel>?> GetAllAsync();

	/// <summary>
	/// Retrieves a specific Calculation Type by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to retrieve.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains
	/// the <see cref="CalculationTypeFullModel"/> if found, or <c>null</c> if not found.
	/// </returns>
	Task<CalculationTypeFullModel?> GetByIdAsync(int id);

	/// <summary>
	/// Updates an existing Calculation Type via the API.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to update.</param>
	/// <param name="model">The Calculation Type model containing the updated data.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains
	/// the updated <see cref="CalculationTypeFullModel"/>, or <c>null</c>.
	/// </returns>
	Task<CalculationTypeFullModel?> UpdateAsync(int id, CalculationTypeFullModel model);
}