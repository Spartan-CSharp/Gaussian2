using System.Net.Http;
using System.Net.Http.Json;

using GaussianCommonLibrary.Models;

using GaussianWPFLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Provides API endpoint operations for managing Spin State/Electronic State/Method Family Combinations.
/// </summary>
/// <param name="logger">The logger instance for logging endpoint operations.</param>
/// <param name="apiHelper">The API helper for HTTP client operations.</param>
public class SpinStatesElectronicStatesMethodFamiliesEndpoint(ILogger<SpinStatesElectronicStatesMethodFamiliesEndpoint> logger, IApiHelper apiHelper) : ISpinStatesElectronicStatesMethodFamiliesEndpoint
{
	private readonly ILogger<SpinStatesElectronicStatesMethodFamiliesEndpoint> _logger = logger;
	private readonly IApiHelper _apiHelper = apiHelper;

	/// <inheritdoc/>
	/// <exception cref="HttpIOException">Thrown when the API request fails or returns an unsuccessful status code.</exception>
	public async Task<List<SpinStateElectronicStateMethodFamilyFullModel>?> GetAllFullAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(GetAllFullAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new(Resources.SpinStatesElectronicStatesMethodFamiliesEndpoint, UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<SpinStateElectronicStateMethodFamilyFullModel>? result = await response.Content.ReadFromJsonAsync<List<SpinStateElectronicStateMethodFamilyFullModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(GetAllFullAsync), result?.Count, nameof(SpinStateElectronicStateMethodFamilyFullModel));
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
	public async Task<List<SpinStateElectronicStateMethodFamilyIntermediateModel>?> GetAllIntermediateAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(GetAllIntermediateAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.SpinStatesElectronicStatesMethodFamiliesEndpoint}/Intermediate", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<SpinStateElectronicStateMethodFamilyIntermediateModel>? result = await response.Content.ReadFromJsonAsync<List<SpinStateElectronicStateMethodFamilyIntermediateModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(GetAllIntermediateAsync), result?.Count, nameof(SpinStateElectronicStateMethodFamilyIntermediateModel));
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
	public async Task<List<SpinStateElectronicStateMethodFamilySimpleModel>?> GetAllSimpleAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(GetAllSimpleAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.SpinStatesElectronicStatesMethodFamiliesEndpoint}/Simple", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<SpinStateElectronicStateMethodFamilySimpleModel>? result = await response.Content.ReadFromJsonAsync<List<SpinStateElectronicStateMethodFamilySimpleModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(GetAllSimpleAsync), result?.Count, nameof(SpinStateElectronicStateMethodFamilySimpleModel));
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
	public async Task<List<SpinStateElectronicStateMethodFamilyRecord>?> GetListAsync()
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(GetListAsync));
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.SpinStatesElectronicStatesMethodFamiliesEndpoint}/List", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<SpinStateElectronicStateMethodFamilyRecord>? result = await response.Content.ReadFromJsonAsync<List<SpinStateElectronicStateMethodFamilyRecord>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(GetListAsync), result?.Count, nameof(SpinStateElectronicStateMethodFamilyRecord));
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
	public async Task<List<SpinStateElectronicStateMethodFamilyFullModel>?> GetByElectronicStateMethodFamilyAsync(int electronicStateMethodFamilyId)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with ElectronicStateMethodFamilyId = {ElectronicStateMethodFamilyId}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(GetByElectronicStateMethodFamilyAsync), electronicStateMethodFamilyId);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.SpinStatesElectronicStatesMethodFamiliesEndpoint}/ElectronicStateMethodFamily?electronicStateMethodFamilyId={electronicStateMethodFamilyId}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<SpinStateElectronicStateMethodFamilyFullModel>? result = await response.Content.ReadFromJsonAsync<List<SpinStateElectronicStateMethodFamilyFullModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(GetByElectronicStateMethodFamilyAsync), result?.Count, nameof(SpinStateElectronicStateMethodFamilyFullModel));
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
	public async Task<List<SpinStateElectronicStateMethodFamilyFullModel>?> GetBySpinStateAsync(int? spinStateId = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with SpinStateId = {SpinStateId}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(GetBySpinStateAsync), spinStateId);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.SpinStatesElectronicStatesMethodFamiliesEndpoint}/SpinState?spinStateId={spinStateId}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<SpinStateElectronicStateMethodFamilyFullModel>? result = await response.Content.ReadFromJsonAsync<List<SpinStateElectronicStateMethodFamilyFullModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(GetBySpinStateAsync), result?.Count, nameof(SpinStateElectronicStateMethodFamilyFullModel));
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
	public async Task<List<SpinStateElectronicStateMethodFamilyFullModel>?> GetByElectronicStateMethodFamilyAndSpinStateAsync(int electronicStateMethodFamilyId, int? spinStateId = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with ElectronicStateMethodFamilyId = {ElectronicStateMethodFamilyId} and SpinStateId = {SpinStateId}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(GetByElectronicStateMethodFamilyAndSpinStateAsync), electronicStateMethodFamilyId, spinStateId);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.SpinStatesElectronicStatesMethodFamiliesEndpoint}/SpinStateElectronicStateMethodFamily?electronicStateMethodFamilyId={electronicStateMethodFamilyId}&spinStateId={spinStateId}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			List<SpinStateElectronicStateMethodFamilyFullModel>? result = await response.Content.ReadFromJsonAsync<List<SpinStateElectronicStateMethodFamilyFullModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelCount} {ModelName}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(GetByElectronicStateMethodFamilyAndSpinStateAsync), result?.Count, nameof(SpinStateElectronicStateMethodFamilyFullModel));
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
	public async Task<SpinStateElectronicStateMethodFamilyFullModel?> GetByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(GetByIdAsync), id);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.SpinStatesElectronicStatesMethodFamiliesEndpoint}/{id}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			SpinStateElectronicStateMethodFamilyFullModel? result = await response.Content.ReadFromJsonAsync<SpinStateElectronicStateMethodFamilyFullModel>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(GetByIdAsync), nameof(SpinStateElectronicStateMethodFamilyFullModel), result);
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
	public async Task<SpinStateElectronicStateMethodFamilyFullModel?> CreateAsync(SpinStateElectronicStateMethodFamilySimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with {ModelName} {Model}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(CreateAsync), nameof(SpinStateElectronicStateMethodFamilySimpleModel), model);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new(Resources.SpinStatesElectronicStatesMethodFamiliesEndpoint, UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync(apiEndpoint, model).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			SpinStateElectronicStateMethodFamilyFullModel? result = await response.Content.ReadFromJsonAsync<SpinStateElectronicStateMethodFamilyFullModel>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(CreateAsync), nameof(SpinStateElectronicStateMethodFamilyFullModel), result);
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
	public async Task<SpinStateElectronicStateMethodFamilyFullModel?> UpdateAsync(int id, SpinStateElectronicStateMethodFamilySimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Class} {Method} called with Id = {Id} and {ModelName} {Model}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(UpdateAsync), id, nameof(SpinStateElectronicStateMethodFamilySimpleModel), model);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.SpinStatesElectronicStatesMethodFamiliesEndpoint}/{id}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.PutAsJsonAsync(apiEndpoint, model).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			SpinStateElectronicStateMethodFamilyFullModel? result = await response.Content.ReadFromJsonAsync<SpinStateElectronicStateMethodFamilyFullModel>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning {ModelName} {Model}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(UpdateAsync), nameof(SpinStateElectronicStateMethodFamilyFullModel), result);
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
			_logger.LogDebug("{Class} {Method} called with Id = {Id}.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(DeleteAsync), id);
		}

		_apiHelper.ApiClient.DefaultRequestHeaders.Date = DateTimeOffset.UtcNow;
		Uri apiEndpoint = new($"{Resources.SpinStatesElectronicStatesMethodFamiliesEndpoint}/{id}", UriKind.Relative);
		using HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesEndpoint), nameof(DeleteAsync));
			}
		}
		else
		{
			throw new HttpIOException(HttpRequestError.InvalidResponse, response.ReasonPhrase);
		}
	}
}
