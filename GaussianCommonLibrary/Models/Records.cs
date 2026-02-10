namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents a lightweight reference to a Electronic State containing only its identifier and name.
/// </summary>
/// <param name="Id">The unique identifier for the Electronic State.</param>
/// <param name="Name">The display name of the Electronic State.</param>
/// <param name="Keyword">The keyword associated with the Electronic State.</param>
public record ElectronicStateRecord(int Id, string? Name, string? Keyword)
{
	/// <summary>
	/// Returns a string representation of the Electronic State Record.
	/// Returns the keyword if no name is specified, the name if no keyword is specified, or "Name/Keyword" if both are specified.
	/// </summary>
	/// <returns>A formatted string representing the Electronic State Record.</returns>
	public override string? ToString()
	{
		return string.IsNullOrWhiteSpace(Name) ? $"{Keyword}" : string.IsNullOrWhiteSpace(Keyword) ? $"{Name}" : $"{Name}/{Keyword}";
	}
}

/// <summary>
/// Represents a lightweight reference to a Method Family containing only its identifier and name.
/// </summary>
/// <param name="Id">The unique identifier for the Method Family.</param>
/// <param name="Name">The display name of the Method Family.</param>
public record MethodFamilyRecord(int Id, string Name);

/// <summary>
/// Represents a request to obtain an authentication token.
/// </summary>
/// <param name="Email">The user's email address used for authentication.</param>
/// <param name="Password">The user's password for credential validation.</param>
public record TokenRequest(string Email, string Password);

/// <summary>
/// Represents the response containing the generated authentication token.
/// </summary>
/// <param name="AccessToken">The JWT access token string used for subsequent API requests.</param>
/// <param name="UserData">The authenticated user's username.</param>
public record TokenResponse(string AccessToken, Dictionary<string, object> UserData);
