using GaussianCommonLibrary.Models;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Defines CRUD operations for managing Full Methods in the Gaussian application.
/// </summary>
public interface IFullMethodsCrud
{
	/// <summary>
	/// Creates a new Full Method asynchronously.
	/// </summary>
	/// <param name="model">The simple model containing the Full Method data to create.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the created Full Method as a full model.</returns>
	Task<FullMethodFullModel> CreateNewFullMethodAsync(FullMethodSimpleModel model);

	/// <summary>
	/// Deletes a Full Method by its identifier asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the Full Method to delete.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task DeleteFullMethodAsync(int id);

	/// <summary>
	/// Retrieves all Full Methods with full details asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all Full Methods as full models.</returns>
	Task<List<FullMethodFullModel>> GetAllFullFullMethodsAsync();

	/// <summary>
	/// Retrieves all Full Methods with intermediate details asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all Full Methods as intermediate models with Method Family records.</returns>
	Task<List<FullMethodIntermediateModel>> GetAllIntermediateFullMethodsAsync();

	/// <summary>
	/// Retrieves all Full Methods with simplified details asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all Full Methods as simple models.</returns>
	Task<List<FullMethodSimpleModel>> GetAllSimpleFullMethodsAsync();

	/// <summary>
	/// Retrieves a specific Full Method by its identifier asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the Full Method to retrieve.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the Full Method as a full model, or null if not found.</returns>
	Task<FullMethodFullModel?> GetFullMethodByIdAsync(int id);

	/// <summary>
	/// Retrieves all Full Methods associated with a specific Base Method asynchronously.
	/// </summary>
	/// <param name="baseMethodId">The unique identifier of the Base Method.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Full Methods as full models.</returns>
	Task<List<FullMethodFullModel>> GetFullMethodsByBaseMethodIdAsync(int baseMethodId);

	/// <summary>
	/// Retrieves all Full Methods associated with a specific Spin State/Electronic State/Method Family asynchronously.
	/// </summary>
	/// <param name="spinStateElectronicStateMethodFamilyId">The unique identifier of the Spin State/Electronic State/Method Family.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Full Methods as full models.</returns>
	Task<List<FullMethodFullModel>> GetFullMethodsBySpinStateElectronicStateMethodFamilyIdAsync(int spinStateElectronicStateMethodFamilyId);

	/// <summary>
	/// Retrieves all Full Methods associated with a specific Spin State/Electronic State/Method Family and a specific Base Method asynchronously.
	/// </summary>
	/// <param name="spinStateElectronicStateMethodFamilyId">The unique identifier of the Spin State/Electronic State/Method Family.</param>
	/// <param name="baseMethodId">The unique identifier of the Base Method.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Full Methods as full models.</returns>
	Task<List<FullMethodFullModel>> GetFullMethodsBySpinStateElectronicStateMethodFamilyIdAndBaseMethodIdAsync(int spinStateElectronicStateMethodFamilyId, int baseMethodId);

	/// <summary>
	/// Retrieves a simplified list of Full Methods from the dataFull.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Full Method records with basic identifying information.</returns>
	Task<List<FullMethodRecord>> GetFullMethodListAsync();

	/// <summary>
	/// Updates an existing Full Method asynchronously.
	/// </summary>
	/// <param name="model">The simple model containing the updated Full Method data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the updated Full Method as a full model.</returns>
	Task<FullMethodFullModel> UpdateFullMethodAsync(FullMethodSimpleModel model);
}