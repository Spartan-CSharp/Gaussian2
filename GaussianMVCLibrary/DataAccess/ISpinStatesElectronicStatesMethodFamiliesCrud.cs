using GaussianCommonLibrary.Models;

namespace GaussianMVCLibrary.DataAccess;

/// <summary>
/// Defines CRUD operations for managing Spin State/Electronic State/Method Family Combinations in the Gaussian application.
/// </summary>
public interface ISpinStatesElectronicStatesMethodFamiliesCrud
{
	/// <summary>
	/// Creates a new Spin State/Electronic State/Method Family Combination asynchronously.
	/// </summary>
	/// <param name="model">The simple model containing the Spin State/Electronic State/Method Family Combination data to create.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the created Spin State/Electronic State/Method Family Combination as a full model.</returns>
	Task<SpinStateElectronicStateMethodFamilyFullModel> CreateNewSpinStateElectronicStateMethodFamilyAsync(SpinStateElectronicStateMethodFamilySimpleModel model);

	/// <summary>
	/// Deletes a Spin State/Electronic State/Method Family Combination by its identifier asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State/Electronic State/Method Family Combination to delete.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task DeleteSpinStateElectronicStateMethodFamilyAsync(int id);

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations with full details asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all Spin State/Electronic State/Method Family Combinations as full models.</returns>
	Task<List<SpinStateElectronicStateMethodFamilyFullModel>> GetAllFullSpinStatesElectronicStatesMethodFamiliesAsync();

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations with intermediate details asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all Spin State/Electronic State/Method Family Combinations as intermediate models with Method Family records.</returns>
	Task<List<SpinStateElectronicStateMethodFamilyIntermediateModel>> GetAllIntermediateSpinStatesElectronicStatesMethodFamiliesAsync();

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations with simplified details asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of all Spin State/Electronic State/Method Family Combinations as simple models.</returns>
	Task<List<SpinStateElectronicStateMethodFamilySimpleModel>> GetAllSimpleSpinStatesElectronicStatesMethodFamiliesAsync();

	/// <summary>
	/// Retrieves a specific Spin State/Electronic State/Method Family Combination by its identifier asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State/Electronic State/Method Family Combination to retrieve.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the Spin State/Electronic State/Method Family Combination as a full model, or null if not found.</returns>
	Task<SpinStateElectronicStateMethodFamilyFullModel?> GetSpinStateElectronicStateMethodFamilyByIdAsync(int id);

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations associated with a specific Electronic State/Method Family Combination and Spin State asynchronously.
	/// </summary>
	/// <param name="electronicStateMethodFamilyId">The unique identifier of the Electronic State/Method Family Combination.</param>
	/// <param name="spinStateId">The unique identifier of the Spin State.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Spin State/Electronic State/Method Family Combinations as full models.</returns>
	Task<List<SpinStateElectronicStateMethodFamilyFullModel>> GetSpinStatesElectronicStatesMethodFamiliesByElectronicStateMethodFamilyIdAndSpinStateIdAsync(int electronicStateMethodFamilyId, int? spinStateId = null);

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations associated with a specific Electronic State/Method Family Combination asynchronously.
	/// </summary>
	/// <param name="electronicStateMethodFamilyId">The unique identifier of the Electronic State/Method Family Combination.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Spin State/Electronic State/Method Family Combinations as full models.</returns>
	Task<List<SpinStateElectronicStateMethodFamilyFullModel>> GetSpinStatesElectronicStatesMethodFamiliesByElectronicStateMethodFamilyIdAsync(int electronicStateMethodFamilyId);

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations associated with a specific Spin State asynchronously.
	/// </summary>
	/// <param name="spinStateId">The unique identifier of the Spin State.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Spin State/Electronic State/Method Family Combinations as full models.</returns>
	Task<List<SpinStateElectronicStateMethodFamilyFullModel>> GetSpinStatesElectronicStatesMethodFamiliesBySpinStateIdAsync(int? spinStateId = null);

	/// <summary>
	/// Retrieves a simplified list of Spin State/Electronic State/Method Family Combinations from the database.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Spin State/Electronic State/Method Family Combination records with basic identifying information.</returns>
	Task<List<SpinStateElectronicStateMethodFamilyRecord>> GetSpinStateElectronicStateMethodFamilyListAsync();

	/// <summary>
	/// Updates an existing Spin State/Electronic State/Method Family Combination asynchronously.
	/// </summary>
	/// <param name="model">The simple model containing the updated Spin State/Electronic State/Method Family Combination data.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the updated Spin State/Electronic State/Method Family Combination as a full model.</returns>
	Task<SpinStateElectronicStateMethodFamilyFullModel> UpdateSpinStateElectronicStateMethodFamilyAsync(SpinStateElectronicStateMethodFamilySimpleModel model);
}