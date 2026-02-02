using GaussianCommonLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for API operations related to base methods.
/// </summary>
public interface IBaseMethodsEndpoint
{
	/// <summary>
	/// Creates a new base method.
	/// </summary>
	/// <param name="model">The base method data to create.</param>
	/// <returns>The created base method with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<BaseMethodFullModel?> CreateAsync(BaseMethodSimpleModel model);

	/// <summary>
	/// Deletes a base method by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to delete.</param>
	/// <returns>A task that represents the asynchronous delete operation.</returns>
	Task DeleteAsync(int id);

	/// <summary>
	/// Retrieves all base methods with full details.
	/// </summary>
	/// <returns>A list of all base methods with full details, or <see langword="null"/> if none exist.</returns>
	Task<List<BaseMethodFullModel>?> GetAllFullAsync();

	/// <summary>
	/// Retrieves all base methods with intermediate details.
	/// </summary>
	/// <returns>A list of all base methods with intermediate details, or <see langword="null"/> if none exist.</returns>
	Task<List<BaseMethodIntermediateModel>?> GetAllIntermediateAsync();
	
	/// <summary>
	/// Retrieves all base methods with simple details.
	/// </summary>
	/// <returns>A list of all base methods with simple details, or <see langword="null"/> if none exist.</returns>
	Task<List<BaseMethodSimpleModel>?> GetAllSimpleAsync();

	/// <summary>
	/// Retrieves all base methods that belong to a specific method family.
	/// </summary>
	/// <param name="methodFamilyId">The unique identifier of the method family.</param>
	/// <returns>A list of base methods with full details for the specified family, or <see langword="null"/> if none exist.</returns>
	Task<List<BaseMethodFullModel>?> GetByFamilyAsync(int methodFamilyId);

	/// <summary>
	/// Retrieves a specific base method by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the base method.</param>
	/// <returns>The base method with full details, or <see langword="null"/> if not found.</returns>
	Task<BaseMethodFullModel?> GetByIdAsync(int id);

	/// <summary>
	/// Updates an existing base method.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to update.</param>
	/// <param name="model">The updated base method data.</param>
	/// <returns>The updated base method with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<BaseMethodFullModel?> UpdateAsync(int id, BaseMethodSimpleModel model);
}
