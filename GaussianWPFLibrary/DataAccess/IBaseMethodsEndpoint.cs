using GaussianCommonLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for API operations related to Base Methods.
/// </summary>
public interface IBaseMethodsEndpoint
{
	/// <summary>
	/// Creates a new Base Method.
	/// </summary>
	/// <param name="model">The Base Method data to create.</param>
	/// <returns>The created Base Method with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<BaseMethodFullModel?> CreateAsync(BaseMethodSimpleModel model);

	/// <summary>
	/// Deletes a Base Method by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Base Method to delete.</param>
	/// <returns>A task that represents the asynchronous delete operation.</returns>
	Task DeleteAsync(int id);

	/// <summary>
	/// Retrieves all Base Methods with full details.
	/// </summary>
	/// <returns>A list of all Base Methods with full details, or <see langword="null"/> if none exist.</returns>
	Task<List<BaseMethodFullModel>?> GetAllFullAsync();

	/// <summary>
	/// Retrieves all Base Methods with intermediate details.
	/// </summary>
	/// <returns>A list of all Base Methods with intermediate details, or <see langword="null"/> if none exist.</returns>
	Task<List<BaseMethodIntermediateModel>?> GetAllIntermediateAsync();
	
	/// <summary>
	/// Retrieves all Base Methods with simple details.
	/// </summary>
	/// <returns>A list of all Base Methods with simple details, or <see langword="null"/> if none exist.</returns>
	Task<List<BaseMethodSimpleModel>?> GetAllSimpleAsync();

	/// <summary>
	/// Retrieves all Base Methods that belong to a specific Method Family.
	/// </summary>
	/// <param name="methodFamilyId">The unique identifier of the Method Family.</param>
	/// <returns>A list of Base Methods with full details for the specified family, or <see langword="null"/> if none exist.</returns>
	Task<List<BaseMethodFullModel>?> GetByMethodFamilyAsync(int methodFamilyId);

	/// <summary>
	/// Retrieves a specific Base Method by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Base Method.</param>
	/// <returns>The Base Method with full details, or <see langword="null"/> if not found.</returns>
	Task<BaseMethodFullModel?> GetByIdAsync(int id);

	/// <summary>
	/// Retrieves a simplified list of all Base Methods as records.
	/// </summary>
	/// <returns>A list of all Base Methods as records, or <see langword="null"/> if none exist.</returns>
	Task<List<BaseMethodRecord>?> GetListAsync();

	/// <summary>
	/// Updates an existing Base Method.
	/// </summary>
	/// <param name="id">The unique identifier of the Base Method to update.</param>
	/// <param name="model">The updated Base Method data.</param>
	/// <returns>The updated Base Method with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<BaseMethodFullModel?> UpdateAsync(int id, BaseMethodSimpleModel model);
}
