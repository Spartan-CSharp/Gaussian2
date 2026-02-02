using System.Net.Http;
using System.Net.Http.Json;

using GaussianWPFLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Provides API endpoint operations for retrieving home and informational page content.
/// </summary>
/// <param name="logger">The logger instance for logging endpoint operations.</param>
/// <param name="apiHelper">The API helper for HTTP client operations.</param>
public class HomeEndpoint(ILogger<HomeEndpoint> logger, IApiHelper apiHelper) : IHomeEndpoint
{
	private readonly ILogger<HomeEndpoint> _logger = logger;
	private readonly IApiHelper _apiHelper = apiHelper;

	/// <inheritdoc/>
	/// <exception cref="HttpIOException">Thrown when the API request fails or returns an unsuccessful status code.</exception>
	public async Task<string?> GetHomeAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(HomeEndpoint), nameof(GetHomeAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new(Resources.HomeEndpoint, UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			string? result = await response.Content.ReadFromJsonAsync<string>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {Result}.", nameof(HomeEndpoint), nameof(GetHomeAsync), result);
			}

			return result;
		}
		else
		{
			throw new HttpIOException(HttpRequestError.InvalidResponse, response.ReasonPhrase);
		}
	}

	/// <inheritdoc/>
	/// <exception cref="HttpIOException">Thrown when the API request fails or returns an unsuccessful status code.</exception>
	public async Task<string?> GetAboutAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(HomeEndpoint), nameof(GetAboutAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.HomeEndpoint}/About", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			string? result = await response.Content.ReadFromJsonAsync<string>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {Result}.", nameof(HomeEndpoint), nameof(GetAboutAsync), result);
			}

			return result;
		}
		else
		{
			throw new HttpIOException(HttpRequestError.InvalidResponse, response.ReasonPhrase);
		}
	}

	/// <inheritdoc/>
	/// <exception cref="HttpIOException">Thrown when the API request fails or returns an unsuccessful status code.</exception>
	public async Task<string?> GetContactAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(HomeEndpoint), nameof(GetContactAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.HomeEndpoint}/Contact", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			string? result = await response.Content.ReadFromJsonAsync<string>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {Result}.", nameof(HomeEndpoint), nameof(GetContactAsync), result);
			}

			return result;
		}
		else
		{
			throw new HttpIOException(HttpRequestError.InvalidResponse, response.ReasonPhrase);
		}
	}

	/// <inheritdoc/>
	/// <exception cref="HttpIOException">Thrown when the API request fails or returns an unsuccessful status code.</exception>
	public async Task<string?> GetPrivacyAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(HomeEndpoint), nameof(GetPrivacyAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.HomeEndpoint}/Privacy", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			string? result = await response.Content.ReadFromJsonAsync<string>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {Result}.", nameof(HomeEndpoint), nameof(GetPrivacyAsync), result);
			}

			return result;
		}
		else
		{
			throw new HttpIOException(HttpRequestError.InvalidResponse, response.ReasonPhrase);
		}
	}

	/// <inheritdoc/>
	/// <exception cref="HttpIOException">Thrown when the API request fails or returns an unsuccessful status code.</exception>
	public async Task<string?> GetErrorAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(HomeEndpoint), nameof(GetErrorAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.HomeEndpoint}/Error", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			string? result = await response.Content.ReadFromJsonAsync<string>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {Result}.", nameof(HomeEndpoint), nameof(GetErrorAsync), result);
			}

			return result;
		}
		else
		{
			throw new HttpIOException(HttpRequestError.InvalidResponse, response.ReasonPhrase);
		}
	}
}
