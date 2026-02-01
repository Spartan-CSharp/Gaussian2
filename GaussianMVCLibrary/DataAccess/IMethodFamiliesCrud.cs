using GaussianCommonLibrary.Models;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Defines CRUD operations for managing Method Families.
/// </summary>
public interface IMethodFamiliesCrud
{
	/// <summary>
	/// Creates a new Method Family in the database.
	/// </summary>
	/// <param name="model">The Method Family model containing the data to create.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the created Method Family with the generated ID, created date, and last updated date populated.</returns>
	Task<MethodFamilyFullModel> CreateNewMethodFamilyAsync(MethodFamilyFullModel model);

	/// <summary>
	/// Deletes a Method Family from the database by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to delete.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task DeleteMethodFamilyAsync(int id);

	/// <summary>
	/// Retrieves all Method Families from the database.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all Method Families.</returns>
	Task<List<MethodFamilyFullModel>> GetAllMethodFamiliesAsync();

	/// <summary>
	/// Retrieves a specific Method Family by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to retrieve.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the Method Family if found; otherwise, null.</returns>
	Task<MethodFamilyFullModel?> GetMethodFamilyByIdAsync(int id);

	/// <summary>
	/// Retrieves a simplified list of Method Families from the database.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Method Family records with basic identifying information.</returns>
	Task<List<MethodFamilyRecord>> GetMethodFamilyListAsync();

	/// <summary>
	/// Updates an existing Method Family in the database.
	/// </summary>
	/// <param name="model">The Method Family model containing the updated data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the updated Method Family with the last updated date refreshed.</returns>
	Task<MethodFamilyFullModel> UpdateMethodFamilyAsync(MethodFamilyFullModel model);
}