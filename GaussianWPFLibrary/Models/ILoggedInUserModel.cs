namespace GaussianWPFLibrary.Models;

/// <summary>
/// Defines the contract for managing authenticated user information in the application.
/// </summary>
/// <remarks>
/// This interface provides properties and methods for storing and managing the current
/// logged-in user's credentials and profile information throughout the application lifecycle.
/// </remarks>
public interface ILoggedInUserModel
{
	/// <summary>
	/// Gets or sets the access token for the authenticated user.
	/// </summary>
	string AccessToken { get; set; }

	/// <summary>
	/// Gets or sets the username of the authenticated user.
	/// </summary>
	string UserName { get; set; }

	/// <summary>
	/// Resets the logged-in user by clearing the access token and username.
	/// </summary>
	void ResetLoggedInUser();
}