using GaussianCommonLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for API operations related to Spin State/Electronic State/Method Family Combinations.
/// </summary>
public interface ISpinStatesElectronicStatesMethodFamiliesEndpoint
{
	/// <summary>
	/// Creates a new Spin State/Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="model">The Spin State/Electronic State/Method Family Combination data to create.</param>
	/// <returns>The created Spin State/Electronic State/Method Family Combination with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<SpinStateElectronicStateMethodFamilyFullModel?> CreateAsync(SpinStateElectronicStateMethodFamilySimpleModel model);

	/// <summary>
	/// Deletes a Spin State/Electronic State/Method Family Combination by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State/Electronic State/Method Family Combination to delete.</param>
	/// <returns>A task that represents the asynchronous delete operation.</returns>
	Task DeleteAsync(int id);

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations with full details.
	/// </summary>
	/// <returns>A list of all Spin State/Electronic State/Method Family Combinations with full details, or <see langword="null"/> if none exist.</returns>
	Task<List<SpinStateElectronicStateMethodFamilyFullModel>?> GetAllFullAsync();

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations with intermediate details.
	/// </summary>
	/// <returns>A list of all Spin State/Electronic State/Method Family Combinations with intermediate details, or <see langword="null"/> if none exist.</returns>
	Task<List<SpinStateElectronicStateMethodFamilyIntermediateModel>?> GetAllIntermediateAsync();
	
	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations with simple details.
	/// </summary>
	/// <returns>A list of all Spin State/Electronic State/Method Family Combinations with simple details, or <see langword="null"/> if none exist.</returns>
	Task<List<SpinStateElectronicStateMethodFamilySimpleModel>?> GetAllSimpleAsync();

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations that belong to a specific Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="electronicStateMethodFamilyId">The unique identifier of the Electronic State/Method Family Combination.</param>
	/// <returns>A list of Spin State/Electronic State/Method Family Combinations with full details for the specified state, or <see langword="null"/> if none exist.</returns>
	Task<List<SpinStateElectronicStateMethodFamilyFullModel>?> GetByElectronicStateMethodFamilyAsync(int electronicStateMethodFamilyId);

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations that belong to a specific Spin State.
	/// </summary>
	/// <param name="spinStateId">The unique identifier of the Spin State.</param>
	/// <returns>A list of Spin State/Electronic State/Method Family Combinations with full details for the specified state, or <see langword="null"/> if none exist.</returns>
	Task<List<SpinStateElectronicStateMethodFamilyFullModel>?> GetBySpinStateAsync(int? spinStateId = null);

	/// <summary>
	/// Retrieves all Spin State/Electronic State/Method Family Combinations that belong to a specific Electronic State/Method Family Combination and Spin State.
	/// </summary>
	/// <param name="electronicStateMethodFamilyId">The unique identifier of the Electronic State/Method Family Combination.</param>
	/// <param name="spinStateId">The unique identifier of the Spin State.</param>
	/// <returns>A list of Spin State/Electronic State/Method Family Combinations with full details for the specified combination and state, or <see langword="null"/> if none exist.</returns>
	Task<List<SpinStateElectronicStateMethodFamilyFullModel>?> GetByElectronicStateMethodFamilyAndSpinStateAsync(int electronicStateMethodFamilyId, int? spinStateId = null);

	/// <summary>
	/// Retrieves a specific Spin State/Electronic State/Method Family Combination by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State/Electronic State/Method Family Combination.</param>
	/// <returns>The Spin State/Electronic State/Method Family Combination with full details, or <see langword="null"/> if not found.</returns>
	Task<SpinStateElectronicStateMethodFamilyFullModel?> GetByIdAsync(int id);

	/// <summary>
	/// Retrieves a simplified list of all Spin State/Electronic State/Method Family Combinations as records.
	/// </summary>
	/// <returns>A list of all Electronic State/MethodFamily Combinations as records, or <see langword="null"/> if none exist.</returns>
	Task<List<SpinStateElectronicStateMethodFamilyRecord>?> GetListAsync();

	/// <summary>
	/// Updates an existing Spin State/Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="id">The unique identifier of the Spin State/Electronic State/Method Family Combination to update.</param>
	/// <param name="model">The updated Spin State/Electronic State/Method Family Combination data.</param>
	/// <returns>The updated Spin State/Electronic State/Method Family Combination with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<SpinStateElectronicStateMethodFamilyFullModel?> UpdateAsync(int id, SpinStateElectronicStateMethodFamilySimpleModel model);
}
