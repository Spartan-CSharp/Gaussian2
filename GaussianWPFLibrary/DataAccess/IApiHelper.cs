using System.Net.Http;

using GaussianWPFLibrary.EventModels;
using GaussianWPFLibrary.Models;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for API helper services that manage HTTP client configuration,
/// user authentication, and authentication state management.
/// </summary>
public interface IApiHelper
{
	/// <summary>
	/// Gets the HTTP client configured for API communication.
	/// </summary>
	HttpClient ApiClient { get; }

	/// <summary>
	/// Gets the currently logged-in user model containing authentication information.
	/// </summary>
	ILoggedInUserModel LoggedInUser { get; }

	/// <summary>
	/// Occurs when the authentication state changes (login or logout).
	/// </summary>
	event EventHandler<AuthenticationEventArgs>? AuthenticationStateChanged;

	/// <summary>
	/// Authenticates a user asynchronously with the provided credentials.
	/// </summary>
	/// <param name="userName">The user's email address or username.</param>
	/// <param name="password">The user's password.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the logged-in user model.</returns>
	Task<ILoggedInUserModel> LoginAsync(string userName, string password);

	/// <summary>
	/// Logs out the current user asynchronously by clearing authentication tokens and headers.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains the updated logged-in user model with cleared credentials.</returns>
	Task<ILoggedInUserModel> LogoutAsync();
}