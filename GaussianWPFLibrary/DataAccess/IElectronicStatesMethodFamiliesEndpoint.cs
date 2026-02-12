using GaussianCommonLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for API operations related to Electronic State/Method Family Combinations.
/// </summary>
public interface IElectronicStatesMethodFamiliesEndpoint
{
	/// <summary>
	/// Creates a new Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="model">The Electronic State/Method Family Combination data to create.</param>
	/// <returns>The created Electronic State/Method Family Combination with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<ElectronicStateMethodFamilyFullModel?> CreateAsync(ElectronicStateMethodFamilySimpleModel model);

	/// <summary>
	/// Deletes a Electronic State/Method Family Combination by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State/Method Family Combination to delete.</param>
	/// <returns>A task that represents the asynchronous delete operation.</returns>
	Task DeleteAsync(int id);

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations with full details.
	/// </summary>
	/// <returns>A list of all Electronic State/Method Family Combinations with full details, or <see langword="null"/> if none exist.</returns>
	Task<List<ElectronicStateMethodFamilyFullModel>?> GetAllFullAsync();

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations with intermediate details.
	/// </summary>
	/// <returns>A list of all Electronic State/Method Family Combinations with intermediate details, or <see langword="null"/> if none exist.</returns>
	Task<List<ElectronicStateMethodFamilyIntermediateModel>?> GetAllIntermediateAsync();
	
	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations with simple details.
	/// </summary>
	/// <returns>A list of all Electronic State/Method Family Combinations with simple details, or <see langword="null"/> if none exist.</returns>
	Task<List<ElectronicStateMethodFamilySimpleModel>?> GetAllSimpleAsync();

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations that belong to a specific Electronic State.
	/// </summary>
	/// <param name="electronicStateId">The unique identifier of the Electronic State.</param>
	/// <returns>A list of Electronic State/Method Family Combinations with full details for the specified state, or <see langword="null"/> if none exist.</returns>
	Task<List<ElectronicStateMethodFamilyFullModel>?> GetByElectronicStateAsync(int electronicStateId);

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations that belong to a specific Method Family.
	/// </summary>
	/// <param name="methodFamilyId">The unique identifier of the Method Family.</param>
	/// <returns>A list of Electronic State/Method Family Combinations with full details for the specified family, or <see langword="null"/> if none exist.</returns>
	Task<List<ElectronicStateMethodFamilyFullModel>?> GetByMethodFamilyAsync(int? methodFamilyId = null);

	/// <summary>
	/// Retrieves all Electronic State/Method Family Combinations that belong to a specific Electronic State and Method Family.
	/// </summary>
	/// <param name="electronicStateId">The unique identifier of the Electronic State.</param>
	/// <param name="methodFamilyId">The unique identifier of the Method Family.</param>
	/// <returns>A list of Electronic State/Method Family Combinations with full details for the specified state and family, or <see langword="null"/> if none exist.</returns>
	Task<List<ElectronicStateMethodFamilyFullModel>?> GetByElectronicStateAndMethodFamilyAsync(int electronicStateId, int? methodFamilyId = null);

	/// <summary>
	/// Retrieves a specific Electronic State/Method Family Combination by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State/Method Family Combination.</param>
	/// <returns>The Electronic State/Method Family Combination with full details, or <see langword="null"/> if not found.</returns>
	Task<ElectronicStateMethodFamilyFullModel?> GetByIdAsync(int id);

	/// <summary>
	/// Retrieves a simplified list of all Electronic State/Method Family Combinations as records.
	/// </summary>
	/// <returns>A list of all Electronic State/MethodFamily Combinations as records, or <see langword="null"/> if none exist.</returns>
	Task<List<ElectronicStateMethodFamilyRecord>?> GetListAsync();

	/// <summary>
	/// Updates an existing Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="id">The unique identifier of the Electronic State/Method Family Combination to update.</param>
	/// <param name="model">The updated Electronic State/Method Family Combination data.</param>
	/// <returns>The updated Electronic State/Method Family Combination with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<ElectronicStateMethodFamilyFullModel?> UpdateAsync(int id, ElectronicStateMethodFamilySimpleModel model);
}
