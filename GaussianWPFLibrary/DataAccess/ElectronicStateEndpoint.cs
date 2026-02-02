using System.Net.Http;
using System.Net.Http.Json;

using GaussianCommonLibrary.Models;

using GaussianWPFLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Provides API endpoint operations for managing Electronic States.
/// </summary>
/// <param name="logger">The logger instance for logging endpoint operations.</param>
/// <param name="apiHelper">The API helper for HTTP client operations.</param>
public class ElectronicStatesEndpoint(ILogger<ElectronicStatesEndpoint> logger, IApiHelper apiHelper) : IElectronicStatesEndpoint
{
	private readonly ILogger<ElectronicStatesEndpoint> _logger = logger;
	private readonly IApiHelper _apiHelper = apiHelper;

	/// <inheritdoc/>
	/// <exception cref="HttpIOException">Thrown when the API request fails or returns an unsuccessful status code.</exception>
	public async Task<List<ElectronicStateFullModel>?> GetAllAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(ElectronicStatesEndpoint), nameof(GetAllAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new(Resources.ElectronicStatesEndpoint, UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<ElectronicStateFullModel>? result = await response.Content.ReadFromJsonAsync<List<ElectronicStateFullModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(ElectronicStatesEndpoint), nameof(GetAllAsync), result?.Count, nameof(ElectronicStateFullModel));
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
	public async Task<ElectronicStateFullModel?> GetByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(ElectronicStatesEndpoint), nameof(GetByIdAsync), id);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.ElectronicStatesEndpoint}/{id}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			ElectronicStateFullModel? result = await response.Content.ReadFromJsonAsync<ElectronicStateFullModel>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(ElectronicStatesEndpoint), nameof(GetByIdAsync), nameof(ElectronicStateFullModel), result);
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
	public async Task<ElectronicStateFullModel?> CreateAsync(ElectronicStateFullModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(ElectronicStatesEndpoint), nameof(CreateAsync), nameof(ElectronicStateFullModel), model);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new(Resources.ElectronicStatesEndpoint, UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync(apiEndpoint, model).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			ElectronicStateFullModel? result = await response.Content.ReadFromJsonAsync<ElectronicStateFullModel>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(ElectronicStatesEndpoint), nameof(CreateAsync), nameof(ElectronicStateFullModel), result);
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
	public async Task<ElectronicStateFullModel?> UpdateAsync(int id, ElectronicStateFullModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id} and {ModelName} {Model}.", nameof(ElectronicStatesEndpoint), nameof(UpdateAsync), id, nameof(ElectronicStateFullModel), model);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.ElectronicStatesEndpoint}/{id}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.PutAsJsonAsync(apiEndpoint, model).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			ElectronicStateFullModel? result = await response.Content.ReadFromJsonAsync<ElectronicStateFullModel>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(ElectronicStatesEndpoint), nameof(UpdateAsync), nameof(ElectronicStateFullModel), result);
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
	public async Task DeleteAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(ElectronicStatesEndpoint), nameof(DeleteAsync), id);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.ElectronicStatesEndpoint}/{id}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning.", nameof(ElectronicStatesEndpoint), nameof(DeleteAsync));
			}
		}
		else
		{
			throw new HttpIOException(HttpRequestError.InvalidResponse, response.ReasonPhrase);
		}
	}
}
