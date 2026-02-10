using GaussianCommonLibrary.Models;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Defines CRUD operations for managing Electronic State/Method Family Combinations in the Gaussian application.
/// </summary>
public interface IElectronicStatesMethodFamiliesCrud
{
	/// <summary>
	/// Creates a new Electronic State/Method Family Combination asynchronously.
	/// </summary>
	/// <param name="model">The simple model containing the Electronic State/Method Family Combination data to create.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the created Electronic State/Method Family Combination as a full model.</returns>
	Task<ElectronicStateMethodFamilyFullModel> CreateNewElectronicStateMethodFamilyAsync(ElectronicStateMethodFamilySimpleModel model);

	/// <summary>
	/// Deletes a Electronic State/Method Family Combination by its identifier asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State/Method Family Combination to delete.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task DeleteElectronicStateMethodFamilyAsync(int id);

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations with full details asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all Electronic State/Method Family Combinations as full models.</returns>
	Task<List<ElectronicStateMethodFamilyFullModel>> GetAllFullElectronicStatesMethodFamiliesAsync();

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations with intermediate details asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all Electronic State/Method Family Combinations as intermediate models with Method Family records.</returns>
	Task<List<ElectronicStateMethodFamilyIntermediateModel>> GetAllIntermediateElectronicStatesMethodFamiliesAsync();

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations with simplified details asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all Electronic State/Method Family Combinations as simple models.</returns>
	Task<List<ElectronicStateMethodFamilySimpleModel>> GetAllSimpleElectronicStatesMethodFamiliesAsync();

	/// <summary>
	/// Retrieves a specific Electronic State/Method Family Combination by its identifier asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State/Method Family Combination to retrieve.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the Electronic State/Method Family Combination as a full model, or null if not found.</returns>
	Task<ElectronicStateMethodFamilyFullModel?> GetElectronicStateMethodFamilyByIdAsync(int id);

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations associated with a specific Electronic State and Method Family asynchronously.
	/// </summary>
	/// <param name="electronicStateId">The unique identifier of the Electronic State.</param>
	/// <param name="methodFamilyId">The unique identifier of the Method Family.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Electronic State/Method Family Combinations as full models.</returns>
	Task<List<ElectronicStateMethodFamilyFullModel>> GetElectronicStatesMethodFamiliesByElectronicStateIdAndMethodFamilyIdAsync(int electronicStateId, int? methodFamilyId = null);

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations associated with a specific Electronic State asynchronously.
	/// </summary>
	/// <param name="electronicStateId">The unique identifier of the Electronic State.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Electronic State/Method Family Combinations as full models.</returns>
	Task<List<ElectronicStateMethodFamilyFullModel>> GetElectronicStatesMethodFamiliesByElectronicStateIdAsync(int electronicStateId);

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations associated with a specific Method Family asynchronously.
	/// </summary>
	/// <param name="methodFamilyId">The unique identifier of the Method Family.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Electronic State/Method Family Combinations as full models.</returns>
	Task<List<ElectronicStateMethodFamilyFullModel>> GetElectronicStatesMethodFamiliesByMethodFamilyIdAsync(int? methodFamilyId = null);

	/// <summary>
	/// Updates an existing Electronic State/Method Family Combination asynchronously.
	/// </summary>
	/// <param name="model">The simple model containing the updated Electronic State/Method Family Combination data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the updated Electronic State/Method Family Combination as a full model.</returns>
	Task<ElectronicStateMethodFamilyFullModel> UpdateElectronicStateMethodFamilyAsync(ElectronicStateMethodFamilySimpleModel model);
}