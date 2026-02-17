using GaussianCommonLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for API operations related to Full Methods.
/// </summary>
public interface IFullMethodsEndpoint
{
	/// <summary>
	/// Creates a new Full Method.
	/// </summary>
	/// <param name="model">The Full Method data to create.</param>
	/// <returns>The created Full Method with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<FullMethodFullModel?> CreateAsync(FullMethodSimpleModel model);

	/// <summary>
	/// Deletes a Full Method by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Full Method to delete.</param>
	/// <returns>A task that represents the asynchronous delete operation.</returns>
	Task DeleteAsync(int id);

	/// <summary>
	/// Retrieves all Full Methods with full details.
	/// </summary>
	/// <returns>A list of all Full Methods with full details, or <see langword="null"/> if none exist.</returns>
	Task<List<FullMethodFullModel>?> GetAllFullAsync();

	/// <summary>
	/// Retrieves all Full Methods with intermediate details.
	/// </summary>
	/// <returns>A list of all Full Methods with intermediate details, or <see langword="null"/> if none exist.</returns>
	Task<List<FullMethodIntermediateModel>?> GetAllIntermediateAsync();
	
	/// <summary>
	/// Retrieves all Full Methods with simple details.
	/// </summary>
	/// <returns>A list of all Full Methods with simple details, or <see langword="null"/> if none exist.</returns>
	Task<List<FullMethodSimpleModel>?> GetAllSimpleAsync();

	/// <summary>
	/// Retrieves all Full Methods that belong to a specific Base Method.
	/// </summary>
	/// <param name="baseMethodId">The unique identifier of the Base Method.</param>
	/// <returns>A list of Full Methods with full details for the specified method, or <see langword="null"/> if none exist.</returns>
	Task<List<FullMethodFullModel>?> GetByBaseMethodAsync(int baseMethodId);

	/// <summary>
	/// Retrieves all Full Methods that belong to a specific Spin State/Electronic State/Method Family Combination and a specific Base Method.
	/// </summary>
	/// <param name="spinStateElectronicStateMethodFamilyId">The unique identifier of the Spin State/Electronic State/Method Family Combination.</param>
	/// <param name="baseMethodId">The unique identifier of the Base Method.</param>
	/// <returns>A list of Full Methods with full details for the specified combination and method, or <see langword="null"/> if none exist.</returns>
	Task<List<FullMethodFullModel>?> GetBySpinStateElectronicStateMethodFamilyAndBaseMethodAsync(int spinStateElectronicStateMethodFamilyId, int baseMethodId);

	/// <summary>
	/// Retrieves all Full Methods that belong to a specific Spin State/Electronic State/Method Family Combination.
	/// </summary>
	/// <param name="spinStateElectronicStateMethodFamilyId">The unique identifier of the Spin State/Electronic State/Method Family Combination.</param>
	/// <returns>A list of Full Methods with full details for the specified combination, or <see langword="null"/> if none exist.</returns>
	Task<List<FullMethodFullModel>?> GetBySpinStateElectronicStateMethodFamilyAsync(int spinStateElectronicStateMethodFamilyId);

	/// <summary>
	/// Retrieves a specific Full Method by its identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Full Method.</param>
	/// <returns>The Full Method with full details, or <see langword="null"/> if not found.</returns>
	Task<FullMethodFullModel?> GetByIdAsync(int id);

	/// <summary>
	/// Retrieves a simplified list of all Full Methods as records.
	/// </summary>
	/// <returns>A list of all Full Methods as records, or <see langword="null"/> if none exist.</returns>
	Task<List<FullMethodRecord>?> GetListAsync();

	/// <summary>
	/// Updates an existing Full Method.
	/// </summary>
	/// <param name="id">The unique identifier of the Full Method to update.</param>
	/// <param name="model">The updated Full Method data.</param>
	/// <returns>The updated Full Method with full details, or <see langword="null"/> if the operation failed.</returns>
	Task<FullMethodFullModel?> UpdateAsync(int id, FullMethodSimpleModel model);
}
