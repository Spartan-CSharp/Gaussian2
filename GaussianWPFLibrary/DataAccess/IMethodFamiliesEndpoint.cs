using GaussianCommonLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for API operations related to Method Families.
/// </summary>
public interface IMethodFamiliesEndpoint
{
	/// <summary>
	/// Creates a new Method Family.
	/// </summary>
	/// <param name="model">The Method Family data to create.</param>
	/// <returns>The created Method Family with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<MethodFamilyFullModel?> CreateAsync(MethodFamilyFullModel model);

	/// <summary>
	/// Deletes a Method Family by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to delete.</param>
	/// <returns>A task that represents the asynchronous delete operation.</returns>
	Task DeleteAsync(int id);

	/// <summary>
	/// Retrieves all Method Families with full details.
	/// </summary>
	/// <returns>A list of all Method Families with full details, or <see langword="null"/> if none exist.</returns>
	Task<List<MethodFamilyFullModel>?> GetAllAsync();

	/// <summary>
	/// Retrieves a specific Method Family by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family.</param>
	/// <returns>The Method Family with full details, or <see langword="null"/> if not found.</returns>
	Task<MethodFamilyFullModel?> GetByIdAsync(int id);

	/// <summary>
	/// Retrieves a simplified list of all Method Families as records.
	/// </summary>
	/// <returns>A list of all Method Families as records, or <see langword="null"/> if none exist.</returns>
	Task<List<MethodFamilyRecord>?> GetListAsync();

	/// <summary>
	/// Updates an existing Method Family.
	/// </summary>
	/// <param name="id">The unique identifier of the Method Family to update.</param>
	/// <param name="model">The updated Method Family data.</param>
	/// <returns>The updated Method Family with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<MethodFamilyFullModel?> UpdateAsync(int id, MethodFamilyFullModel model);
}