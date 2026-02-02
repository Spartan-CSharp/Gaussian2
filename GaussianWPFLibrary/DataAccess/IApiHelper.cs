using System.Net.Http;

using GaussianWPFLibrary.EventModels;
using GaussianWPFLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for an API helper that manages HTTP client configuration, user authentication, and logged-in user state.
/// </summary>
public interface IApiHelper
{
	/// <summary>
	/// Gets the configured HTTP client for making API requests.
	/// </summary>
	HttpClient ApiClient { get; }

	/// <summary>
	/// Gets the currently logged-in user model containing authentication tokens and user profile data.
	/// </summary>
	ILoggedInUserModel LoggedInUser { get; }

	/// <summary>
	/// Occurs when the user's authentication state changes (login or logout).
	/// </summary>
	event EventHandler<AuthenticationEventArgs>? AuthenticationStateChanged;

	/// <summary>
	/// Initializes the API client with the specified base address and configures default HTTP headers.
	/// </summary>
	/// <param name="apiBaseAddress">The base URL address of the API.</param>
	/// <param name="productName">The product name to include in the User-Agent header.</param>
	/// <param name="productVersion">The product version to include in the User-Agent header.</param>
	void InitializeApiClient(string apiBaseAddress, string productName, string productVersion);

	/// <summary>
	/// Authenticates a user with the specified credentials and retrieves user data.
	/// </summary>
	/// <param name="userName">The username for authentication.</param>
	/// <param name="password">The password for authentication.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the logged-in user model with populated authentication data.</returns>
	Task<ILoggedInUserModel> LoginAsync(string userName, string password);

	/// <summary>
	/// Logs out the current user and clears all authentication data.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains the reset user model.</returns>
	Task<ILoggedInUserModel> LogoutAsync();
}