using GaussianCommonLibrary.Models;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Defines CRUD operations for managing Calculation Types.
/// </summary>
public interface ICalculationTypesCrud
{
	/// <summary>
	/// Creates a new Calculation Type in the database.
	/// </summary>
	/// <param name="model">The Calculation Type model containing the data to create.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the created Calculation Type with the generated ID, created date, and last updated date populated.</returns>
	Task<CalculationTypeFullModel> CreateNewCalculationTypeAsync(CalculationTypeFullModel model);

	/// <summary>
	/// Deletes a Calculation Type from the database by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to delete.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task DeleteCalculationTypeAsync(int id);

	/// <summary>
	/// Retrieves all Calculation Types from the database.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all Calculation Types.</returns>
	Task<List<CalculationTypeFullModel>> GetAllCalculationTypesAsync();

	/// <summary>
	/// Retrieves a specific Calculation Type by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to retrieve.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the Calculation Type if found; otherwise, null.</returns>
	Task<CalculationTypeFullModel?> GetCalculationTypeByIdAsync(int id);

	/// <summary>
	/// Updates an existing Calculation Type in the database.
	/// </summary>
	/// <param name="model">The Calculation Type model containing the updated data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the updated Calculation Type with the last updated date refreshed.</returns>
	Task<CalculationTypeFullModel> UpdateCalculationTypeAsync(CalculationTypeFullModel model);
}