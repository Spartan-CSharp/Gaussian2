using GaussianCommonLibrary.Models;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Defines the contract for CRUD (Create, Read, Update, Delete) operations on calculation types.
/// </summary>
public interface ICalculationTypesCrud
{
	/// <summary>
	/// Creates a new calculation type in the database.
	/// </summary>
	/// <param name="model">The calculation type model containing the data to create.</param>
	/// <returns>The created calculation type with the generated Id, CreatedDate, and LastUpdatedDate populated.</returns>
	Task<CalculationTypeFullModel> CreateNewCalculationTypeAsync(CalculationTypeFullModel model);

	/// <summary>
	/// Deletes a calculation type from the database by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the calculation type to delete.</param>
	/// <returns>A task representing the asynchronous delete operation.</returns>
	Task DeleteCalculationTypeAsync(int id);

	/// <summary>
	/// Retrieves all calculation types from the database.
	/// </summary>
	/// <returns>A list of all calculation types.</returns>
	Task<List<CalculationTypeFullModel>> GetAllCalculationTypesAsync();

	/// <summary>
	/// Retrieves a specific calculation type by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the calculation type to retrieve.</param>
	/// <returns>The calculation type matching the specified Id.</returns>
	Task<CalculationTypeFullModel?> GetCalculationTypeByIdAsync(int id);

	/// <summary>
	/// Updates an existing calculation type in the database.
	/// </summary>
	/// <param name="model">The calculation type model containing the updated data.</param>
	/// <returns>The updated calculation type with the LastUpdatedDate refreshed.</returns>
	Task<CalculationTypeFullModel> UpdateCalculationTypeAsync(CalculationTypeFullModel model);
}