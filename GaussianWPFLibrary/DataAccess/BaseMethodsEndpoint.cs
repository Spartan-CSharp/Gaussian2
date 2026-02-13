using System.Net.Http;
using System.Net.Http.Json;

using GaussianCommonLibrary.Models;

using GaussianWPFLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Provides API endpoint operations for managing Base Methods.
/// </summary>
/// <param name="logger">The logger instance for logging endpoint operations.</param>
/// <param name="apiHelper">The API helper for HTTP client operations.</param>
public class BaseMethodsEndpoint(ILogger<BaseMethodsEndpoint> logger, IApiHelper apiHelper) : IBaseMethodsEndpoint
{
	private readonly ILogger<BaseMethodsEndpoint> _logger = logger;
	private readonly IApiHelper _apiHelper = apiHelper;

	/// <inheritdoc/>
	/// <exception cref="HttpIOException">Thrown when the API request fails or returns an unsuccessful status code.</exception>
	public async Task<List<BaseMethodFullModel>?> GetAllFullAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(BaseMethodsEndpoint), nameof(GetAllFullAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new(Resources.BaseMethodsEndpoint, UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<BaseMethodFullModel>? result = await response.Content.ReadFromJsonAsync<List<BaseMethodFullModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(BaseMethodsEndpoint), nameof(GetAllFullAsync), result?.Count, nameof(BaseMethodFullModel));
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
	public async Task<List<BaseMethodIntermediateModel>?> GetAllIntermediateAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(BaseMethodsEndpoint), nameof(GetAllIntermediateAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.BaseMethodsEndpoint}/Intermediate", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<BaseMethodIntermediateModel>? result = await response.Content.ReadFromJsonAsync<List<BaseMethodIntermediateModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(BaseMethodsEndpoint), nameof(GetAllIntermediateAsync), result?.Count, nameof(BaseMethodIntermediateModel));
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
	public async Task<List<BaseMethodSimpleModel>?> GetAllSimpleAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(BaseMethodsEndpoint), nameof(GetAllSimpleAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.BaseMethodsEndpoint}/Simple", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<BaseMethodSimpleModel>? result = await response.Content.ReadFromJsonAsync<List<BaseMethodSimpleModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(BaseMethodsEndpoint), nameof(GetAllSimpleAsync), result?.Count, nameof(BaseMethodSimpleModel));
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
	public async Task<List<BaseMethodRecord>?> GetListAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(BaseMethodsEndpoint), nameof(GetListAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.BaseMethodsEndpoint}/List", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<BaseMethodRecord>? result = await response.Content.ReadFromJsonAsync<List<BaseMethodRecord>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(BaseMethodsEndpoint), nameof(GetListAsync), result?.Count, nameof(BaseMethodRecord));
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
	public async Task<List<BaseMethodFullModel>?> GetByMethodFamilyAsync(int methodFamilyId)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with MethodFamilyId = {MethodFamilyId}.", nameof(BaseMethodsEndpoint), nameof(GetByMethodFamilyAsync), methodFamilyId);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.BaseMethodsEndpoint}/MethodFamily?methodFamilyId={methodFamilyId}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<BaseMethodFullModel>? result = await response.Content.ReadFromJsonAsync<List<BaseMethodFullModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(BaseMethodsEndpoint), nameof(GetByMethodFamilyAsync), result?.Count, nameof(BaseMethodFullModel));
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
	public async Task<BaseMethodFullModel?> GetByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(BaseMethodsEndpoint), nameof(GetByIdAsync), id);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.BaseMethodsEndpoint}/{id}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			BaseMethodFullModel? result = await response.Content.ReadFromJsonAsync<BaseMethodFullModel>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(BaseMethodsEndpoint), nameof(GetByIdAsync), nameof(BaseMethodFullModel), result);
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
	public async Task<BaseMethodFullModel?> CreateAsync(BaseMethodSimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(BaseMethodsEndpoint), nameof(CreateAsync), nameof(BaseMethodSimpleModel), model);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new(Resources.BaseMethodsEndpoint, UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync(apiEndpoint, model).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			BaseMethodFullModel? result = await response.Content.ReadFromJsonAsync<BaseMethodFullModel>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(BaseMethodsEndpoint), nameof(CreateAsync), nameof(BaseMethodFullModel), result);
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
	public async Task<BaseMethodFullModel?> UpdateAsync(int id, BaseMethodSimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id} and {ModelName} {Model}.", nameof(BaseMethodsEndpoint), nameof(UpdateAsync), id, nameof(BaseMethodSimpleModel), model);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.BaseMethodsEndpoint}/{id}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.PutAsJsonAsync(apiEndpoint, model).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			BaseMethodFullModel? result = await response.Content.ReadFromJsonAsync<BaseMethodFullModel>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(BaseMethodsEndpoint), nameof(UpdateAsync), nameof(BaseMethodFullModel), result);
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
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(BaseMethodsEndpoint), nameof(DeleteAsync), id);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.BaseMethodsEndpoint}/{id}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning.", nameof(BaseMethodsEndpoint), nameof(DeleteAsync));
			}
		}
		else
		{
			throw new HttpIOException(HttpRequestError.InvalidResponse, response.ReasonPhrase);
		}
	}
}
