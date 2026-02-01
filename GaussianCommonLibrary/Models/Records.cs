namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents a lightweight reference to a method family containing only its identifier and name.
/// </summary>
/// <param name="Id">The unique identifier for the method family.</param>
/// <param name="Name">The display name of the method family.</param>
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
