using GaussianCommonLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for interacting with the Method Families API endpoint.
/// Provides methods for CRUD operations on Method Family resources.
/// </summary>
public interface IMethodFamiliesEndpoint
{
	/// <summary>
	/// Creates a new Method Family via the API.
	/// </summary>
	/// <param name="model">The Method Family model containing the data for the new record.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains
	/// the created <see cref="MethodFamilyFullModel"/> with server-generated values, or <c>null</c>.
	/// </returns>
	Task<MethodFamilyFullModel?> CreateAsync(MethodFamilyFullModel model);

	/// <summary>
	/// Deletes a Method Family from the API.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to delete.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// </returns>
	Task DeleteAsync(int id);

	/// <summary>
	/// Retrieves all Method Families from the API.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains
	/// a list of <see cref="MethodFamilyFullModel"/> objects, or <c>null</c> if the response is empty.
	/// </returns>
	Task<List<MethodFamilyFullModel>?> GetAllAsync();

	/// <summary>
	/// Retrieves a specific Method Family by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to retrieve.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains
	/// the <see cref="MethodFamilyFullModel"/> if found, or <c>null</c> if not found.
	/// </returns>
	Task<MethodFamilyFullModel?> GetByIdAsync(int id);

	/// <summary>
	/// Retrieves a list of Method Family records from the API.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains
	/// a list of <see cref="MethodFamilyRecord"/> objects, or <c>null</c> if the response is empty.
	/// </returns>
	Task<List<MethodFamilyRecord>?> GetListAsync();

	/// <summary>
	/// Updates an existing Method Family via the API.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to update.</param>
	/// <param name="model">The Method Family model containing the updated data.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains
	/// the updated <see cref="MethodFamilyFullModel"/>, or <c>null</c>.
	/// </returns>
	Task<MethodFamilyFullModel?> UpdateAsync(int id, MethodFamilyFullModel model);
}