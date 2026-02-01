using GaussianCommonLibrary.Models;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Defines CRUD operations for managing base methods in the Gaussian application.
/// </summary>
public interface IBaseMethodsCrud
{
	/// <summary>
	/// Creates a new base method asynchronously.
	/// </summary>
	/// <param name="model">The simple model containing the base method data to create.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the created base method as a full model.</returns>
	Task<BaseMethodFullModel> CreateNewBaseMethodAsync(BaseMethodSimpleModel model);

	/// <summary>
	/// Deletes a base method by its identifier asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to delete.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task DeleteBaseMethodAsync(int id);

	/// <summary>
	/// Retrieves all base methods with full details asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all base methods as full models.</returns>
	Task<List<BaseMethodFullModel>> GetAllFullBaseMethodsAsync();

	/// <summary>
	/// Retrieves all base methods with intermediate details asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all base methods as intermediate models with method family records.</returns>
	Task<List<BaseMethodIntermediateModel>> GetAllIntermediateBaseMethodsAsync();

	/// <summary>
	/// Retrieves all base methods with simplified details asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all base methods as simple models.</returns>
	Task<List<BaseMethodSimpleModel>> GetAllSimpleBaseMethodsAsync();

	/// <summary>
	/// Retrieves a specific base method by its identifier asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to retrieve.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the base method as a full model, or null if not found.</returns>
	Task<BaseMethodFullModel?> GetBaseMethodByIdAsync(int id);

	/// <summary>
	/// Retrieves all base methods associated with a specific method family asynchronously.
	/// </summary>
	/// <param name="methodFamilyId">The unique identifier of the method family.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of base methods as full models.</returns>
	Task<List<BaseMethodFullModel>> GetBaseMethodsByMethodFamilyIdAsync(int methodFamilyId);

	/// <summary>
	/// Updates an existing base method asynchronously.
	/// </summary>
	/// <param name="model">The simple model containing the updated base method data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the updated base method as a full model.</returns>
	Task<BaseMethodFullModel> UpdateBaseMethodAsync(BaseMethodSimpleModel model);
}