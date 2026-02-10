using GaussianCommonLibrary.Models;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Defines CRUD operations for managing Base Methods in the Gaussian application.
/// </summary>
public interface IBaseMethodsCrud
{
	/// <summary>
	/// Creates a new Base Method asynchronously.
	/// </summary>
	/// <param name="model">The simple model containing the Base Method data to create.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the created Base Method as a full model.</returns>
	Task<BaseMethodFullModel> CreateNewBaseMethodAsync(BaseMethodSimpleModel model);

	/// <summary>
	/// Deletes a Base Method by its identifier asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the Base Method to delete.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task DeleteBaseMethodAsync(int id);

	/// <summary>
	/// Retrieves all Base Methods with full details asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all Base Methods as full models.</returns>
	Task<List<BaseMethodFullModel>> GetAllFullBaseMethodsAsync();

	/// <summary>
	/// Retrieves all Base Methods with intermediate details asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all Base Methods as intermediate models with Method Family records.</returns>
	Task<List<BaseMethodIntermediateModel>> GetAllIntermediateBaseMethodsAsync();

	/// <summary>
	/// Retrieves all Base Methods with simplified details asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all Base Methods as simple models.</returns>
	Task<List<BaseMethodSimpleModel>> GetAllSimpleBaseMethodsAsync();

	/// <summary>
	/// Retrieves a specific Base Method by its identifier asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the Base Method to retrieve.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the Base Method as a full model, or null if not found.</returns>
	Task<BaseMethodFullModel?> GetBaseMethodByIdAsync(int id);

	/// <summary>
	/// Retrieves all Base Methods associated with a specific Method Family asynchronously.
	/// </summary>
	/// <param name="methodFamilyId">The unique identifier of the Method Family.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Base Methods as full models.</returns>
	Task<List<BaseMethodFullModel>> GetBaseMethodsByMethodFamilyIdAsync(int methodFamilyId);

	/// <summary>
	/// Updates an existing Base Method asynchronously.
	/// </summary>
	/// <param name="model">The simple model containing the updated Base Method data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the updated Base Method as a full model.</returns>
	Task<BaseMethodFullModel> UpdateBaseMethodAsync(BaseMethodSimpleModel model);
}