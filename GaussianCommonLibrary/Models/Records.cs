namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents a lightweight reference to a Base Method containing only its identifier and keyword.
/// </summary>
/// <param name="Id">The unique identifier for the Base Method.</param>
/// <param name="Keyword">The keyword of the Base Method.</param>
public record BaseMethodRecord(int Id, string Keyword);

/// <summary>
/// Represents a lightweight reference to a Electronic State/Method Family Combination containing only its identifier and name.
/// </summary>
/// <param name="Id">The unique identifier for the Electronic State/Method Family Combination.</param>
/// <param name="Name">The display name of the Electronic State/Method Family Combination/Method Family Combination.</param>
/// <param name="Keyword">The keyword associated with the Electronic State/Method Family Combination.</param>
public record ElectronicStateMethodFamilyRecord(int Id, string? Name, string? Keyword)
{
	/// <summary>
	/// Returns a string representation of the Electronic State/Method Family Combination Record.
	/// Returns the keyword if no name is specified, the name if no keyword is specified, or "Name/Keyword" if both are specified.
	/// </summary>
	/// <returns>A formatted string representing the Electronic State/Method Family Combination Record.</returns>
	public override string? ToString()
	{
		return string.IsNullOrWhiteSpace(Name) ? $"{Keyword}" : string.IsNullOrWhiteSpace(Keyword) ? $"{Name}" : $"{Name}/{Keyword}";
	}
}

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
/// Represents a lightweight reference to a Full Method containing only its identifier and keyword.
/// </summary>
/// <param name="Id">The unique identifier for the Full Method.</param>
/// <param name="Keyword">The keyword of the Full Method.</param>
public record FullMethodRecord(int Id, string Keyword);

/// <summary>
/// Represents a lightweight reference to a Method Family containing only its identifier and name.
/// </summary>
/// <param name="Id">The unique identifier for the Method Family.</param>
/// <param name="Name">The display name of the Method Family.</param>
public record MethodFamilyRecord(int Id, string Name);

/// <summary>
/// Represents a lightweight reference to a Spin State containing only its identifier and name.
/// </summary>
/// <param name="Id">The unique identifier for the Spin State.</param>
/// <param name="Name">The display name of the Spin State.</param>
/// <param name="Keyword">The keyword associated with the Spin State.</param>
public record SpinStateRecord(int Id, string? Name, string? Keyword)
{
	/// <summary>
	/// Returns a string representation of the Spin State Record.
	/// Returns the keyword if no name is specified, the name if no keyword is specified, or "Name/Keyword" if both are specified.
	/// </summary>
	/// <returns>A formatted string representing the Spin State Record.</returns>
	public override string? ToString()
	{
		return string.IsNullOrWhiteSpace(Name) ? $"{Keyword}" : string.IsNullOrWhiteSpace(Keyword) ? $"{Name}" : $"{Name}/{Keyword}";
	}
}

/// <summary>
/// Represents a lightweight reference to a Electronic State/Method Family Combination containing only its identifier and name.
/// </summary>
/// <param name="Id">The unique identifier for the Electronic State/Method Family Combination.</param>
/// <param name="Name">The display name of the Electronic State/Method Family Combination/Method Family Combination.</param>
/// <param name="Keyword">The keyword associated with the Electronic State/Method Family Combination.</param>
public record SpinStateElectronicStateMethodFamilyRecord(int Id, string? Name, string? Keyword)
{
	/// <summary>
	/// Returns a string representation of the SpinState/Electronic State/Method Family Combination Record.
	/// Returns the keyword if no name is specified, the name if no keyword is specified, or "Name/Keyword" if both are specified.
	/// </summary>
	/// <returns>A formatted string representing the SpinState/Electronic State/Method Family Combination Record.</returns>
	public override string? ToString()
	{
		return string.IsNullOrWhiteSpace(Name) ? $"{Keyword}" : string.IsNullOrWhiteSpace(Keyword) ? $"{Name}" : $"{Name}/{Keyword}";
	}
}

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
