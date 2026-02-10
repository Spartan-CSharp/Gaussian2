using System.Net.Http;
using System.Net.Http.Json;

using GaussianCommonLibrary.Models;

using GaussianWPFLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Provides API endpoint operations for managing Electronic State/Method Family Combinations.
/// </summary>
/// <param name="logger">The logger instance for logging endpoint operations.</param>
/// <param name="apiHelper">The API helper for HTTP client operations.</param>
public class ElectronicStatesMethodFamiliesEndpoint(ILogger<ElectronicStatesMethodFamiliesEndpoint> logger, IApiHelper apiHelper) : IElectronicStatesMethodFamiliesEndpoint
{
	private readonly ILogger<ElectronicStatesMethodFamiliesEndpoint> _logger = logger;
	private readonly IApiHelper _apiHelper = apiHelper;

	/// <inheritdoc/>
	/// <exception cref="HttpIOException">Thrown when the API request fails or returns an unsuccessful status code.</exception>
	public async Task<List<ElectronicStateMethodFamilyFullModel>?> GetAllFullAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(GetAllFullAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new(Resources.ElectronicStatesMethodFamiliesEndpoint, UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<ElectronicStateMethodFamilyFullModel>? result = await response.Content.ReadFromJsonAsync<List<ElectronicStateMethodFamilyFullModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(GetAllFullAsync), result?.Count, nameof(ElectronicStateMethodFamilyFullModel));
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
	public async Task<List<ElectronicStateMethodFamilyIntermediateModel>?> GetAllIntermediateAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(GetAllIntermediateAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.ElectronicStatesMethodFamiliesEndpoint}/Intermediate", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<ElectronicStateMethodFamilyIntermediateModel>? result = await response.Content.ReadFromJsonAsync<List<ElectronicStateMethodFamilyIntermediateModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(GetAllIntermediateAsync), result?.Count, nameof(ElectronicStateMethodFamilyIntermediateModel));
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
	public async Task<List<ElectronicStateMethodFamilySimpleModel>?> GetAllSimpleAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(GetAllSimpleAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.ElectronicStatesMethodFamiliesEndpoint}/Simple", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<ElectronicStateMethodFamilySimpleModel>? result = await response.Content.ReadFromJsonAsync<List<ElectronicStateMethodFamilySimpleModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(GetAllSimpleAsync), result?.Count, nameof(ElectronicStateMethodFamilySimpleModel));
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
	public async Task<List<ElectronicStateMethodFamilyFullModel>?> GetByElectronicStateAsync(int electronicStateId)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with ElectronicStateId = {ElectronicStateId}.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(GetByElectronicStateAsync), electronicStateId);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.ElectronicStatesMethodFamiliesEndpoint}/ElectronicState?electronicStateId={electronicStateId}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<ElectronicStateMethodFamilyFullModel>? result = await response.Content.ReadFromJsonAsync<List<ElectronicStateMethodFamilyFullModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(GetByElectronicStateAsync), result?.Count, nameof(ElectronicStateMethodFamilyFullModel));
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
	public async Task<List<ElectronicStateMethodFamilyFullModel>?> GetByMethodFamilyAsync(int? methodFamilyId = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with MethodFamilyId = {MethodFamilyId}.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(GetByMethodFamilyAsync), methodFamilyId);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.ElectronicStatesMethodFamiliesEndpoint}/MethodFamily?methodFamilyId={methodFamilyId}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<ElectronicStateMethodFamilyFullModel>? result = await response.Content.ReadFromJsonAsync<List<ElectronicStateMethodFamilyFullModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(GetByMethodFamilyAsync), result?.Count, nameof(ElectronicStateMethodFamilyFullModel));
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
	public async Task<List<ElectronicStateMethodFamilyFullModel>?> GetByElectronicStateAndMethodFamilyAsync(int electronicStateId, int? methodFamilyId = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with ElectronicStateId = {ElectronicStateId} and MethodFamilyId = {MethodFamilyId}.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(GetByElectronicStateAndMethodFamilyAsync), electronicStateId, methodFamilyId);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.ElectronicStatesMethodFamiliesEndpoint}/ElectronicStateMethodFamily?electronicStateId={electronicStateId}&methodFamilyId={methodFamilyId}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<ElectronicStateMethodFamilyFullModel>? result = await response.Content.ReadFromJsonAsync<List<ElectronicStateMethodFamilyFullModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(GetByElectronicStateAndMethodFamilyAsync), result?.Count, nameof(ElectronicStateMethodFamilyFullModel));
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
	public async Task<ElectronicStateMethodFamilyFullModel?> GetByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(GetByIdAsync), id);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.ElectronicStatesMethodFamiliesEndpoint}/{id}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			ElectronicStateMethodFamilyFullModel? result = await response.Content.ReadFromJsonAsync<ElectronicStateMethodFamilyFullModel>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(GetByIdAsync), nameof(ElectronicStateMethodFamilyFullModel), result);
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
	public async Task<ElectronicStateMethodFamilyFullModel?> CreateAsync(ElectronicStateMethodFamilySimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(CreateAsync), nameof(ElectronicStateMethodFamilySimpleModel), model);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new(Resources.ElectronicStatesMethodFamiliesEndpoint, UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync(apiEndpoint, model).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			ElectronicStateMethodFamilyFullModel? result = await response.Content.ReadFromJsonAsync<ElectronicStateMethodFamilyFullModel>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(CreateAsync), nameof(ElectronicStateMethodFamilyFullModel), result);
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
	public async Task<ElectronicStateMethodFamilyFullModel?> UpdateAsync(int id, ElectronicStateMethodFamilySimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id} and {ModelName} {Model}.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(UpdateAsync), id, nameof(ElectronicStateMethodFamilySimpleModel), model);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.ElectronicStatesMethodFamiliesEndpoint}/{id}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.PutAsJsonAsync(apiEndpoint, model).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			ElectronicStateMethodFamilyFullModel? result = await response.Content.ReadFromJsonAsync<ElectronicStateMethodFamilyFullModel>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(UpdateAsync), nameof(ElectronicStateMethodFamilyFullModel), result);
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
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(DeleteAsync), id);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.ElectronicStatesMethodFamiliesEndpoint}/{id}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning.", nameof(ElectronicStatesMethodFamiliesEndpoint), nameof(DeleteAsync));
			}
		}
		else
		{
			throw new HttpIOException(HttpRequestError.InvalidResponse, response.ReasonPhrase);
		}
	}
}
