using GaussianCommonLibrary.Models;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Defines the contract for CRUD (Create, Read, Update, Delete) operations on Calculation Types.
/// </summary>
public interface ICalculationTypesCrud
{
	/// <summary>
	/// Creates a new Calculation Type in the database.
	/// </summary>
	/// <param name="model">The Calculation Type model containing the data to create.</param>
	/// <returns>The created Calculation Type with the generated Id, CreatedDate, and LastUpdatedDate populated.</returns>
	Task<CalculationTypeFullModel> CreateNewCalculationTypeAsync(CalculationTypeFullModel model);

	/// <summary>
	/// Deletes a Calculation Type from the database by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to delete.</param>
	/// <returns>A task representing the asynchronous delete operation.</returns>
	Task DeleteCalculationTypeAsync(int id);

	/// <summary>
	/// Retrieves all Calculation Types from the database.
	/// </summary>
	/// <returns>A list of all Calculation Types.</returns>
	Task<List<CalculationTypeFullModel>> GetAllCalculationTypesAsync();

	/// <summary>
	/// Retrieves a specific Calculation Type by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to retrieve.</param>
	/// <returns>The Calculation Type matching the specified Id.</returns>
	Task<CalculationTypeFullModel?> GetCalculationTypeByIdAsync(int id);

	/// <summary>
	/// Updates an existing Calculation Type in the database.
	/// </summary>
	/// <param name="model">The Calculation Type model containing the updated data.</param>
	/// <returns>The updated Calculation Type with the LastUpdatedDate refreshed.</returns>
	Task<CalculationTypeFullModel> UpdateCalculationTypeAsync(CalculationTypeFullModel model);
}