using System.Net.Http;
using System.Net.Http.Json;

using GaussianCommonLibrary.Models;

using GaussianWPFLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Provides endpoint access for base method operations via HTTP API calls.
/// </summary>
/// <param name="logger">The logger instance for tracking operation execution and errors.</param>
/// <param name="apiHelper">The API helper instance for making HTTP requests.</param>
public class BaseMethodsEndpoint(ILogger<BaseMethodsEndpoint> logger, IApiHelper apiHelper) : IBaseMethodsEndpoint
{
	private readonly ILogger<BaseMethodsEndpoint> _logger = logger;
	private readonly IApiHelper _apiHelper = apiHelper;

	/// <summary>
	/// Retrieves all base methods with full details asynchronously.
	/// </summary>
	/// <returns>A list of <see cref="BaseMethodFullModel"/> containing all base methods with complete information, or null if no data is available.</returns>
	/// <exception cref="HttpIOException">Thrown when the HTTP response indicates a failure.</exception>
	public async Task<List<BaseMethodFullModel>?> GetAllFullAsync()
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called", nameof(GetAllFullAsync));
		}

		Uri apiEndpoint = new($"{Resources.BaseMethodsEndpoint}/Full", UriKind.Relative);

		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} HttpResponse Had a Success Code: {ResponseCode} {ResponsePhrase}", nameof(GetAllFullAsync), response.StatusCode, response.ReasonPhrase);
			}

			List<BaseMethodFullModel>? result = await response.Content.ReadFromJsonAsync<List<BaseMethodFullModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} received {ResponseContent}", nameof(GetAllFullAsync), result);
			}

			return result;
		}
		else
		{
			HttpIOException ex = new(HttpRequestError.InvalidResponse, response.ReasonPhrase);

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} HttpResponse Did Not Have a Success Code: {ResponseCode} {ResponsePhrase}", nameof(GetAllFullAsync), response.StatusCode, response.ReasonPhrase);
			}

			throw ex;
		}
	}

	/// <summary>
	/// Retrieves all base methods with simplified information asynchronously.
	/// </summary>
	/// <returns>A list of <see cref="BaseMethodSimpleModel"/> containing all base methods with basic information, or null if no data is available.</returns>
	/// <exception cref="HttpIOException">Thrown when the HTTP response indicates a failure.</exception>
	public async Task<List<BaseMethodSimpleModel>?> GetAllSimpleAsync()
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called", nameof(GetAllSimpleAsync));
		}

		Uri apiEndpoint = new($"{Resources.BaseMethodsEndpoint}/Simple", UriKind.Relative);

		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} HttpResponse Had a Success Code: {ResponseCode} {ResponsePhrase}", nameof(GetAllSimpleAsync), response.StatusCode, response.ReasonPhrase);
			}

			List<BaseMethodSimpleModel>? result = await response.Content.ReadFromJsonAsync<List<BaseMethodSimpleModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} received {ResponseContent}", nameof(GetAllSimpleAsync), result);
			}

			return result;
		}
		else
		{
			HttpIOException ex = new(HttpRequestError.InvalidResponse, response.ReasonPhrase);

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} HttpResponse Did Not Have a Success Code: {ResponseCode} {ResponsePhrase}", nameof(GetAllSimpleAsync), response.StatusCode, response.ReasonPhrase);
			}

			throw ex;
		}
	}

	/// <summary>
	/// Retrieves all base methods belonging to a specific method family asynchronously.
	/// </summary>
	/// <param name="methodFamilyId">The unique identifier of the method family to filter by.</param>
	/// <returns>A list of <see cref="BaseMethodFullModel"/> containing all base methods in the specified family, or null if no data is available.</returns>
	/// <exception cref="HttpIOException">Thrown when the HTTP response indicates a failure.</exception>
	public async Task<List<BaseMethodFullModel>?> GetByFamilyAsync(int methodFamilyId)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {MethodFamilyId}", nameof(GetByFamilyAsync), methodFamilyId);
		}

		Uri apiEndpoint = new($"{Resources.BaseMethodsEndpoint}/Family?methodFamilyId={methodFamilyId}", UriKind.Relative);

		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} HttpResponse Had a Success Code: {ResponseCode} {ResponsePhrase}", nameof(GetByFamilyAsync), response.StatusCode, response.ReasonPhrase);
			}

			List<BaseMethodFullModel>? result = await response.Content.ReadFromJsonAsync<List<BaseMethodFullModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} received {ResponseContent}", nameof(GetByFamilyAsync), result);
			}

			return result;
		}
		else
		{
			HttpIOException ex = new(HttpRequestError.InvalidResponse, response.ReasonPhrase);

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} HttpResponse Did Not Have a Success Code: {ResponseCode} {ResponsePhrase}", nameof(GetByFamilyAsync), response.StatusCode, response.ReasonPhrase);
			}

			throw ex;
		}
	}

	/// <summary>
	/// Retrieves a specific base method by its unique identifier asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to retrieve.</param>
	/// <returns>A <see cref="BaseMethodFullModel"/> containing the requested base method details, or null if not found.</returns>
	/// <exception cref="HttpIOException">Thrown when the HTTP response indicates a failure.</exception>
	public async Task<BaseMethodFullModel?> GetByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {Id}", nameof(GetByIdAsync), id);
		}

		Uri apiEndpoint = new($"{Resources.BaseMethodsEndpoint}/{id}", UriKind.Relative);

		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} HttpResponse Had a Success Code: {ResponseCode} {ResponsePhrase}", nameof(GetByIdAsync), response.StatusCode, response.ReasonPhrase);
			}

			BaseMethodFullModel? result = await response.Content.ReadFromJsonAsync<BaseMethodFullModel>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} received {ResponseContent}", nameof(GetByIdAsync), result);
			}

			return result;
		}
		else
		{
			HttpIOException ex = new(HttpRequestError.InvalidResponse, response.ReasonPhrase);

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} HttpResponse Did Not Have a Success Code: {ResponseCode} {ResponsePhrase}", nameof(GetByIdAsync), response.StatusCode, response.ReasonPhrase);
			}

			throw ex;
		}
	}

	/// <summary>
	/// Creates a new base method asynchronously.
	/// </summary>
	/// <param name="model">The <see cref="BaseMethodSimpleModel"/> containing the data for the new base method.</param>
	/// <returns>A <see cref="BaseMethodFullModel"/> representing the newly created base method with complete information, or null if creation failed.</returns>
	/// <exception cref="HttpIOException">Thrown when the HTTP response indicates a failure.</exception>
	public async Task<BaseMethodFullModel?> CreateAsync(BaseMethodSimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {Model}", nameof(CreateAsync), model);
		}

		Uri apiEndpoint = new(Resources.BaseMethodsEndpoint, UriKind.Relative);

		using HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync(apiEndpoint, model).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} HttpResponse Had a Success Code: {ResponseCode} {ResponsePhrase}", nameof(CreateAsync), response.StatusCode, response.ReasonPhrase);
			}

			BaseMethodFullModel? result = await response.Content.ReadFromJsonAsync<BaseMethodFullModel>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} received {ResponseContent}", nameof(CreateAsync), result);
			}

			return result;
		}
		else
		{
			HttpIOException ex = new(HttpRequestError.InvalidResponse, response.ReasonPhrase);

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} HttpResponse Did Not Have a Success Code: {ResponseCode} {ResponsePhrase}", nameof(CreateAsync), response.StatusCode, response.ReasonPhrase);
			}

			throw ex;
		}
	}

	/// <summary>
	/// Updates an existing base method asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to update.</param>
	/// <param name="model">The <see cref="BaseMethodSimpleModel"/> containing the updated data.</param>
	/// <returns>A <see cref="BaseMethodFullModel"/> representing the updated base method with complete information, or null if update failed.</returns>
	/// <exception cref="HttpIOException">Thrown when the HTTP response indicates a failure.</exception>
	public async Task<BaseMethodFullModel?> UpdateAsync(int id, BaseMethodSimpleModel model)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {Id} and {Model}", nameof(UpdateAsync), id, model);
		}

		Uri apiEndpoint = new($"{Resources.BaseMethodsEndpoint}/{id}", UriKind.Relative);

		using HttpResponseMessage response = await _apiHelper.ApiClient.PutAsJsonAsync(apiEndpoint, model).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} HttpResponse Had a Success Code: {ResponseCode} {ResponsePhrase}", nameof(UpdateAsync), response.StatusCode, response.ReasonPhrase);
			}

			BaseMethodFullModel? result = await response.Content.ReadFromJsonAsync<BaseMethodFullModel>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} received {ResponseContent}", nameof(UpdateAsync), result);
			}

			return result;
		}
		else
		{
			HttpIOException ex = new(HttpRequestError.InvalidResponse, response.ReasonPhrase);

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} HttpResponse Did Not Have a Success Code: {ResponseCode} {ResponsePhrase}", nameof(UpdateAsync), response.StatusCode, response.ReasonPhrase);
			}

			throw ex;
		}
	}

	/// <summary>
	/// Deletes a base method asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the base method to delete.</param>
	/// <returns>A task representing the asynchronous delete operation.</returns>
	/// <exception cref="HttpIOException">Thrown when the HTTP response indicates a failure.</exception>
	public async Task DeleteAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {Id}", nameof(DeleteAsync), id);
		}

		Uri apiEndpoint = new($"{Resources.BaseMethodsEndpoint}/{id}", UriKind.Relative);

		using HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} HttpResponse Had a Success Code: {ResponseCode} {ResponsePhrase}", nameof(DeleteAsync), response.StatusCode, response.ReasonPhrase);
			}
		}
		else
		{
			HttpIOException ex = new(HttpRequestError.InvalidResponse, response.ReasonPhrase);

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} HttpResponse Did Not Have a Success Code: {ResponseCode} {ResponsePhrase}", nameof(DeleteAsync), response.StatusCode, response.ReasonPhrase);
			}

			throw ex;
		}
	}
}
