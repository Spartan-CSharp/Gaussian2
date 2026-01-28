using GaussianCommonLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for base methods data access operations.
/// Provides CRUD operations for managing base method entities.
/// </summary>
public interface IBaseMethodsEndpoint
{
	/// <summary>
	/// Creates a new base method asynchronously.
	/// </summary>
	/// <param name="model">The simple model containing the base method data to create.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. 
	/// The task result contains the created base method as a full model, or null if creation failed.
	/// </returns>
	Task<BaseMethodFullModel?> CreateAsync(BaseMethodSimpleModel model);

	/// <summary>
	/// Deletes a base method by its identifier asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to delete.</param>
	/// <returns>A task that represents the asynchronous delete operation.</returns>
	Task DeleteAsync(int id);

	/// <summary>
	/// Retrieves all base methods as full models asynchronously.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation. 
	/// The task result contains a list of all base methods as full models, or null if retrieval failed.
	/// </returns>
	Task<List<BaseMethodFullModel>?> GetAllFullAsync();

	/// <summary>
	/// Retrieves all base methods as simple models asynchronously.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation. 
	/// The task result contains a list of all base methods as simple models, or null if retrieval failed.
	/// </returns>
	Task<List<BaseMethodSimpleModel>?> GetAllSimpleAsync();

	/// <summary>
	/// Retrieves all base methods that belong to a specific method family asynchronously.
	/// </summary>
	/// <param name="methodFamilyId">The unique identifier of the method family to filter by.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. 
	/// The task result contains a list of base methods belonging to the specified family, or null if retrieval failed.
	/// </returns>
	Task<List<BaseMethodFullModel>?> GetByFamilyAsync(int methodFamilyId);

	/// <summary>
	/// Retrieves a single base method by its identifier asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to retrieve.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. 
	/// The task result contains the base method as a full model, or null if not found.
	/// </returns>
	Task<BaseMethodFullModel?> GetByIdAsync(int id);

	/// <summary>
	/// Updates an existing base method asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to update.</param>
	/// <param name="model">The simple model containing the updated base method data.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. 
	/// The task result contains the updated base method as a full model, or null if update failed.
	/// </returns>
	Task<BaseMethodFullModel?> UpdateAsync(int id, BaseMethodSimpleModel model);
}
