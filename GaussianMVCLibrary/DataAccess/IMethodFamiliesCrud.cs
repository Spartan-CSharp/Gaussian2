using GaussianCommonLibrary.Models;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Defines the contract for CRUD (Create, Read, Update, Delete) operations on Method Families.
/// </summary>
public interface IMethodFamiliesCrud
{
	/// <summary>
	/// Creates a new Method Family in the database.
	/// </summary>
	/// <param name="model">The Method Family model containing the data to create.</param>
	/// <returns>The created Method Family with the generated Id, CreatedDate, and LastUpdatedDate populated.</returns>
	Task<MethodFamilyFullModel> CreateNewMethodFamilyAsync(MethodFamilyFullModel model);

	/// <summary>
	/// Deletes a Method Family from the database by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to delete.</param>
	/// <returns>A task representing the asynchronous delete operation.</returns>
	Task DeleteMethodFamilyAsync(int id);

	/// <summary>
	/// Retrieves all Method Families from the database.
	/// </summary>
	/// <returns>A list of all Method Families.</returns>
	Task<List<MethodFamilyFullModel>> GetAllMethodFamiliesAsync();

	/// <summary>
	/// Retrieves a specific Method Family by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to retrieve.</param>
	/// <returns>The Method Family matching the specified Id.</returns>
	Task<MethodFamilyFullModel?> GetMethodFamilyByIdAsync(int id);

	/// <summary>
	/// Updates an existing Method Family in the database.
	/// </summary>
	/// <param name="model">The Method Family model containing the updated data.</param>
	/// <returns>The updated Method Family with the LastUpdatedDate refreshed.</returns>
	Task<MethodFamilyFullModel> UpdateMethodFamilyAsync(MethodFamilyFullModel model);
}