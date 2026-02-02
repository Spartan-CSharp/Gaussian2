using GaussianCommonLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for API operations related to method families.
/// </summary>
public interface IMethodFamiliesEndpoint
{
	/// <summary>
	/// Creates a new method family.
	/// </summary>
	/// <param name="model">The method family data to create.</param>
	/// <returns>The created method family with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<MethodFamilyFullModel?> CreateAsync(MethodFamilyFullModel model);

	/// <summary>
	/// Deletes a method family by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the method family to delete.</param>
	/// <returns>A task that represents the asynchronous delete operation.</returns>
	Task DeleteAsync(int id);

	/// <summary>
	/// Retrieves all method families with full details.
	/// </summary>
	/// <returns>A list of all method families with full details, or <see langword="null"/> if none exist.</returns>
	Task<List<MethodFamilyFullModel>?> GetAllAsync();

	/// <summary>
	/// Retrieves a specific method family by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the method family.</param>
	/// <returns>The method family with full details, or <see langword="null"/> if not found.</returns>
	Task<MethodFamilyFullModel?> GetByIdAsync(int id);

	/// <summary>
	/// Retrieves a simplified list of all method families as records.
	/// </summary>
	/// <returns>A list of all method families as records, or <see langword="null"/> if none exist.</returns>
	Task<List<MethodFamilyRecord>?> GetListAsync();

	/// <summary>
	/// Updates an existing method family.
	/// </summary>
	/// <param name="id">The unique identifier of the method family to update.</param>
	/// <param name="model">The updated method family data.</param>
	/// <returns>The updated method family with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<MethodFamilyFullModel?> UpdateAsync(int id, MethodFamilyFullModel model);
}