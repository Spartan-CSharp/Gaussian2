namespace GaussianWPFLibrary.Models;

/// <summary>
/// Represents a user role in the system with its unique identifier and display name.
/// </summary>
public class UserRoleModel
{
	/// <summary>
	/// Gets or sets the unique identifier for the role.
	/// </summary>
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the display name of the role.
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Returns a string representation of the role.
	/// </summary>
	/// <returns>A string in the format "Name (Id)".</returns>
	public override string? ToString()
	{
		return $"{Name} ({Id})";
	}
}
