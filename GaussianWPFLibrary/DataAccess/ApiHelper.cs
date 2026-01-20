using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using GaussianWPFLibrary.EventModels;
using GaussianWPFLibrary.Models;

using Microsoft.Extensions.Logging;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Provides helper methods for API communication, authentication, and user session management.
/// </summary>
public class ApiHelper : IApiHelper
{
	private readonly ILogger<ApiHelper> _logger;
	private readonly MediaTypeWithQualityHeaderValue _acceptHeader = new("application/json");

	/// <summary>
	/// Initializes a new instance of the <see cref="ApiHelper"/> class.
	/// </summary>
	/// <param name="logger">The logger instance for logging API operations.</param>
	/// <param name="loggedInUser">The logged-in user model to track authentication state.</param>
	public ApiHelper(ILogger<ApiHelper> logger, ILoggedInUserModel loggedInUser)
	{
		_logger = logger;
		LoggedInUser = loggedInUser;

		string apiBaseAddress = "https://localhost:7056/"; // TODO: replace this with settings or appsettings to make user-configurable

		ApiClient = new HttpClient
		{
			BaseAddress = new Uri(apiBaseAddress)
		};

		ApiClient.DefaultRequestHeaders.Accept.Clear();
		ApiClient.DefaultRequestHeaders.Accept.Add(_acceptHeader);
	}

	/// <summary>
	/// Occurs when the authentication state changes (login or logout).
	/// </summary>
	public event EventHandler<AuthenticationEventArgs>? AuthenticationStateChanged;

	/// <summary>
	/// Gets the currently logged-in user model containing authentication information.
	/// </summary>
	public ILoggedInUserModel LoggedInUser { get; }

	/// <summary>
	/// Gets the HTTP client configured for API communication.
	/// </summary>
	public HttpClient ApiClient { get; private set; }

	/// <summary>
	/// Authenticates a user asynchronously with the provided credentials.
	/// </summary>
	/// <param name="userName">The user's email address or username.</param>
	/// <param name="password">The user's password.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the logged-in user model.</returns>
	/// <exception cref="HttpIOException">Thrown when authentication fails or the server returns a non-success status code.</exception>
	public async Task<ILoggedInUserModel> LoginAsync(string userName, string password)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {UserName} and {Password}", nameof(LoginAsync), userName, password);
		}

		Uri apiEndpoint = new("api/Authentication/token", UriKind.Relative);

		var data = new
		{
			Email = userName,
			Password = password
		};

		using HttpResponseMessage response = await ApiClient.PostAsJsonAsync(apiEndpoint, data).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} HttpResponse Had a Success Code: {ResponseCode} {ResponsePhrase}", nameof(LoginAsync), response.StatusCode, response.ReasonPhrase);
			}

			LoggedInUserModel? result = await response.Content.ReadFromJsonAsync<LoggedInUserModel>().ConfigureAwait(false);

			if (result is not null)
			{
				ApiClient.DefaultRequestHeaders.Clear();
				ApiClient.DefaultRequestHeaders.Accept.Clear();
				ApiClient.DefaultRequestHeaders.Accept.Add(_acceptHeader);
				ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {result.AccessToken}");
				LoggedInUser.AccessToken = result.AccessToken;
				LoggedInUser.UserName = result.UserName;
				AuthenticationStateChanged?.Invoke(this, new AuthenticationEventArgs(true));
			}

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} received {ResponseContent}", nameof(LoginAsync), result);
			}

			return LoggedInUser;
		}
		else
		{
			HttpIOException ex = new(HttpRequestError.UserAuthenticationError, response.ReasonPhrase);

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} HttpResponse Did Not Have a Success Code: {ResponseCode} {ResponsePhrase}", nameof(LoginAsync), response.StatusCode, response.ReasonPhrase);
			}

			throw ex;
		}
	}

	/// <summary>
	/// Logs out the current user asynchronously by clearing authentication tokens and headers.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains the updated logged-in user model with cleared credentials.</returns>
	public async Task<ILoggedInUserModel> LogoutAsync()
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called", nameof(LogoutAsync));
		}

		ApiClient.DefaultRequestHeaders.Clear();
		ApiClient.DefaultRequestHeaders.Accept.Clear();
		ApiClient.DefaultRequestHeaders.Accept.Add(_acceptHeader);
		LoggedInUser.AccessToken = string.Empty;
		LoggedInUser.UserName = string.Empty;
		AuthenticationStateChanged?.Invoke(this, new AuthenticationEventArgs(false));

		return LoggedInUser;
	}
}
