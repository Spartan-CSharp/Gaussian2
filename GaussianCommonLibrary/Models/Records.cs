namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents a family of computational methods used in Gaussian calculations.
/// </summary>
/// <param name="Id">The unique identifier for the method family.</param>
/// <param name="Name">The display name of the method family.</param>
public record MethodFamilyRecord(int Id, string Name);
