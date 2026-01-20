namespace GaussianWPFLibrary.Models;

/// <summary>
/// Represents a logged-in user with authentication credentials.
/// </summary>
public class LoggedInUserModel : ILoggedInUserModel
{
	/// <summary>
	/// Gets or sets the access token for the authenticated user.
	/// </summary>
	public required string AccessToken { get; set; }

	/// <summary>
	/// Gets or sets the username of the authenticated user.
	/// </summary>
	public required string UserName { get; set; }

	/// <summary>
	/// Resets the logged-in user by clearing the access token and username.
	/// </summary>
	public void ResetLoggedInUser()
	{
		AccessToken = string.Empty;
		UserName = string.Empty;
	}

	/// <summary>
	/// Returns a string representation of the logged-in user.
	/// </summary>
	/// <returns>A formatted string containing the username and access token.</returns>
	public override string? ToString()
	{
		return $"{UserName}: {AccessToken}";
	}
}
