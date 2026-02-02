using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

using GaussianCommonLibrary.Models;

using GaussianWPFLibrary.EventModels;
using GaussianWPFLibrary.Models;
using GaussianWPFLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Provides HTTP client configuration, user authentication, and logged-in user state management for API communication.
/// </summary>
/// <param name="logger">The logger instance for logging operations.</param>
/// <param name="loggedInUser">The logged-in user model instance to manage user state.</param>
public class ApiHelper(ILogger<ApiHelper> logger, ILoggedInUserModel loggedInUser) : IApiHelper
{
	private readonly ILogger<ApiHelper> _logger = logger;

	/// <inheritdoc/>
	public event EventHandler<AuthenticationEventArgs>? AuthenticationStateChanged;

	/// <inheritdoc/>
	public ILoggedInUserModel LoggedInUser { get; private set; } = loggedInUser;

	/// <inheritdoc/>
	public HttpClient ApiClient { get; private set; } = new HttpClient();

	/// <inheritdoc/>
	public void InitializeApiClient(string apiBaseAddress, string productName, string productVersion)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with ApiBaseAddress = {ApiBaseAddress}, ProductName = {ProductName}, and ProductVersion = {ProductVersion}.", nameof(ApiHelper), nameof(InitializeApiClient), apiBaseAddress, productName, productVersion);
		}

		ApiClient.BaseAddress = new Uri(apiBaseAddress);
		ApiClient.DefaultRequestHeaders.Clear();
		ApiClient.DefaultRequestHeaders.Accept.Clear();
		ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		ApiClient.DefaultRequestHeaders.AcceptCharset.Clear();
		ApiClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("UTF-8"));
		ApiClient.DefaultRequestHeaders.AcceptEncoding.Clear();
		ApiClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("identity", 0.9));
		ApiClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("*", 0.5));
		ApiClient.DefaultRequestHeaders.AcceptLanguage.Clear();
		ApiClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-CA", 0.9));
		ApiClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en", 0.7));
		ApiClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("*", 0.5));
		ApiClient.DefaultRequestHeaders.Authorization = null;
		ApiClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
		{
			NoCache = true,
			NoStore = true,
			NoTransform = true
		};

		ApiClient.DefaultRequestHeaders.Connection.Clear();
		ApiClient.DefaultRequestHeaders.Connection.Add("keep-alive");
		ApiClient.DefaultRequestHeaders.ConnectionClose = false;
		ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		ApiClient.DefaultRequestHeaders.Expect.Clear();
		ApiClient.DefaultRequestHeaders.ExpectContinue = false;
		ApiClient.DefaultRequestHeaders.From = null;
		ApiClient.DefaultRequestHeaders.Host = $"{ApiClient.BaseAddress?.Host}:{ApiClient.BaseAddress?.Port}";
		ApiClient.DefaultRequestHeaders.IfMatch.Clear();
		ApiClient.DefaultRequestHeaders.IfModifiedSince = null;
		ApiClient.DefaultRequestHeaders.IfNoneMatch.Clear();
		ApiClient.DefaultRequestHeaders.IfRange = null;
		ApiClient.DefaultRequestHeaders.IfUnmodifiedSince = null;
		ApiClient.DefaultRequestHeaders.MaxForwards = null;
		ApiClient.DefaultRequestHeaders.Pragma.Clear();
		ApiClient.DefaultRequestHeaders.Protocol = string.Empty;
		ApiClient.DefaultRequestHeaders.ProxyAuthorization = null;
		ApiClient.DefaultRequestHeaders.Range = null;
		ApiClient.DefaultRequestHeaders.Referrer = null; // TODO see if there is a method we can use to get the IP and port used by the program for external communication.
		ApiClient.DefaultRequestHeaders.TE.Clear();
		ApiClient.DefaultRequestHeaders.Trailer.Clear();
		ApiClient.DefaultRequestHeaders.TransferEncoding.Clear();
		ApiClient.DefaultRequestHeaders.TransferEncodingChunked = null;
		ApiClient.DefaultRequestHeaders.Upgrade.Clear();
		ApiClient.DefaultRequestHeaders.UserAgent.Clear();
		ApiClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(productName, productVersion));
		ApiClient.DefaultRequestHeaders.Via.Clear();
		ApiClient.DefaultRequestHeaders.Warning.Clear();
		ApiClient.DefaultRequestVersion = new Version(1, 1);
		ApiClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
		ApiClient.MaxResponseContentBufferSize = 2147483647;
		ApiClient.Timeout = TimeSpan.FromSeconds(100);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning.", nameof(ApiHelper), nameof(InitializeApiClient));
		}
	}

	/// <inheritdoc/>
	/// <exception cref="HttpIOException">Thrown when authentication fails due to invalid credentials or server errors.</exception>
	public async Task<ILoggedInUserModel> LoginAsync(string userName, string password)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			// TODO add password obfuscation for logging
			_logger.LogDebug("{Class} {Method} called with UserName = {UserName} and Password = {Password}.", nameof(ApiHelper), nameof(LoginAsync), userName, password);
		}

		ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.AuthenticationEndpoint}/token", UriKind.Relative);
		TokenRequest data = new(userName, password);

		using HttpResponseMessage response = await ApiClient.PostAsJsonAsync(apiEndpoint, data).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			TokenResponse? result = await response.Content.ReadFromJsonAsync<TokenResponse>().ConfigureAwait(false);

			if (result is not null)
			{
				ApiClient.DefaultRequestHeaders.Authorization = null;
				ApiClient.DefaultRequestHeaders.Authorization = new("Bearer", result.AccessToken);
				LoggedInUser.AccessToken = result.AccessToken;
				ProcessUserData(result.UserData);
				ApiClient.DefaultRequestHeaders.From = LoggedInUser.Email;
				AuthenticationStateChanged?.Invoke(this, new AuthenticationEventArgs(true));
			}
			else
			{
				ApiClient.DefaultRequestHeaders.Authorization = null;
				ApiClient.DefaultRequestHeaders.From = null;
				LoggedInUser.ResetLoggedInUser();
				AuthenticationStateChanged?.Invoke(this, new AuthenticationEventArgs(false));
			}

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(ApiHelper), nameof(LoginAsync), nameof(ILoggedInUserModel), LoggedInUser);
			}

			return LoggedInUser;
		}
		else
		{
			throw new HttpIOException(HttpRequestError.UserAuthenticationError, response.ReasonPhrase);
		}
	}

	/// <inheritdoc/>
	public async Task<ILoggedInUserModel> LogoutAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(ApiHelper), nameof(LogoutAsync));
		}

		ApiClient.DefaultRequestHeaders.Authorization = null;
		ApiClient.DefaultRequestHeaders.From = null;
		LoggedInUser.ResetLoggedInUser();
		AuthenticationStateChanged?.Invoke(this, new AuthenticationEventArgs(false));

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(ApiHelper), nameof(LoginAsync), nameof(ILoggedInUserModel), LoggedInUser);
		}

		return LoggedInUser;
	}

	private void ProcessUserData(Dictionary<string, object> data)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} called with data = {Data}.", nameof(ApiHelper), nameof(ProcessUserData), JsonSerializer.Serialize(data));
		}

		foreach (KeyValuePair<string, object> kvp in data)
		{
			string keyName = kvp.Key;
			JsonElement value = (JsonElement)kvp.Value;

			switch (keyName)
			{
				case "Id":
					LoggedInUser.UserId = value.GetString() ?? string.Empty; // value?.ToString() ?? string.Empty;
					break;
				case "UserName":
					LoggedInUser.UserName = value.GetString() ?? string.Empty;
					break;
				case "Email":
					LoggedInUser.Email = value.GetString() ?? string.Empty;
					break;
				case "EmailConfirmed":
					LoggedInUser.EmailConfirmed = value.GetBoolean();
					break;
				case "PhoneNumber":
					LoggedInUser.PhoneNumber = value.GetString() ?? string.Empty;
					break;
				case "PhoneNumberConfirmed":
					LoggedInUser.PhoneNumberConfirmed = value.GetBoolean();
					break;
				case "TwoFactorEnabled":
					LoggedInUser.TwoFactorEnabled = value.GetBoolean();
					break;
				case "LockoutEnd":
					LoggedInUser.LockoutEnd = string.IsNullOrWhiteSpace(value.GetString()) ? null : value.GetDateTimeOffset();
					break;
				case "LockoutEnabled":
					LoggedInUser.LockoutEnabled = value.GetBoolean();
					break;
				case "AccessFailedCount":
					LoggedInUser.AccessFailedCount = value.GetInt32();
					break;
				case "UserRoles":
					ProcessUserRoleData(value.Deserialize<List<Dictionary<string, object>>>() ?? []);
					break;
				case "UserClaims":
					ProcessUserClaimData(value.Deserialize<Dictionary<string, object>>() ?? []);
					break;
				default:
					break;
			}
		}

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning.", nameof(ApiHelper), nameof(ProcessUserData));
		}
	}

	private void ProcessUserRoleData(List<Dictionary<string, object>> data)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} called with data = {Data}.", nameof(ApiHelper), nameof(ProcessUserRoleData), JsonSerializer.Serialize(data));
		}

		LoggedInUser.UserRoles.Clear();

		foreach (Dictionary<string, object> role in data)
		{
			UserRoleModel userRole = new();
			foreach (KeyValuePair<string, object> kvp in role)
			{
				string keyName = kvp.Key;
				JsonElement value = (JsonElement)kvp.Value;

				switch (keyName)
				{
					case "Id":
						userRole.Id = value.GetString() ?? string.Empty;
						break;
					case "Name":
						userRole.Name = value.GetString() ?? string.Empty;
						break;
					case "RoleClaims":
						ProcessRoleClaimData(userRole, value.Deserialize<Dictionary<string, object>>() ?? []);
						break;
					default:
						break;
				}
			}

			LoggedInUser.UserRoles.Add(userRole);
		}

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning.", nameof(ApiHelper), nameof(ProcessUserRoleData));
		}
	}

	private void ProcessUserClaimData(Dictionary<string, object> data)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} called with data = {Data}.", nameof(ApiHelper), nameof(ProcessUserClaimData), JsonSerializer.Serialize(data));
		}

		foreach (KeyValuePair<string, object> kvp in data)
		{
			string keyName = kvp.Key;
			JsonElement value = (JsonElement)kvp.Value;

			switch (keyName)
			{
				case "name":
					LoggedInUser.Name = value.GetString() ?? string.Empty;
					break;
				case "given_name":
					LoggedInUser.GivenName = value.GetString() ?? string.Empty;
					break;
				case "family_name":
					LoggedInUser.FamilyName = value.GetString() ?? string.Empty;
					break;
				case "middle_name":
					LoggedInUser.MiddleName = value.GetString() ?? string.Empty;
					break;
				case "gender":
					LoggedInUser.Gender = value.GetString() ?? string.Empty;
					break;
				case "birthdate":
					LoggedInUser.BirthDate = DateOnly.FromDateTime(value.GetDateTime());
					break;
				case "zoneinfo":
					LoggedInUser.ZoneInfo = value.GetString() ?? string.Empty;
					break;
				case "locale":
					LoggedInUser.Locale = value.GetString() ?? string.Empty;
					break;
				case "address":
					ProcessUserAddressData(LoggedInUser.Address, value.Deserialize<UserAddressModel>() ?? new UserAddressModel());
					break;
				default:
					break;
			}
		}

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning.", nameof(ApiHelper), nameof(ProcessUserClaimData));
		}
	}

	private void ProcessUserAddressData(UserAddressModel address, UserAddressModel? data)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} called with address = {Address} and data = {Data}.", nameof(ApiHelper), nameof(ProcessRoleClaimData), address, JsonSerializer.Serialize(data));
		}

		if (data is null)
		{
			address = new();
		}
		else
		{
			address.StreetAddress = !string.IsNullOrWhiteSpace(data.StreetAddress) ? data.StreetAddress : string.Empty;
			address.Locality = !string.IsNullOrWhiteSpace(data.Locality) ? data.Locality : string.Empty;
			address.Region = !string.IsNullOrWhiteSpace(data.Region) ? data.Region : string.Empty;
			address.PostalCode = !string.IsNullOrWhiteSpace(data.PostalCode) ? data.PostalCode : string.Empty;
			address.Country = !string.IsNullOrWhiteSpace(data.Country) ? data.Country : string.Empty;
			address.Formatted = !string.IsNullOrWhiteSpace(data.Formatted) ? data.Formatted : string.Empty;
		}

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning.", nameof(ApiHelper), nameof(ProcessUserAddressData));
		}
	}

	private void ProcessRoleClaimData(UserRoleModel userRole, Dictionary<string, object> data)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} called with userRole = {UserRole} and data = {Data}.", nameof(ApiHelper), nameof(ProcessRoleClaimData), userRole, JsonSerializer.Serialize(data));
		}

		foreach (KeyValuePair<string, object> kvp in data)
		{
			string keyName = kvp.Key;
			JsonElement value = (JsonElement)kvp.Value;

			switch (keyName)
			{
				default:
					break;
			}
		}

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning.", nameof(ApiHelper), nameof(ProcessRoleClaimData));
		}
	}
}
