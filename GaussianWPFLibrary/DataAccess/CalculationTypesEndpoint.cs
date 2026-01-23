using System.Net.Http;
using System.Net.Http.Json;

using GaussianCommonLibrary.Models;

using GaussianWPFLibrary.Properties;

using Microsoft.Extensions.Logging;

namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Provides HTTP endpoint access for managing Calculation Types through the API.
/// </summary>
/// <remarks>
/// This class handles all CRUD operations for Calculation Types, including retrieving,
/// creating, updating, and deleting Calculation Type records via HTTP requests.
/// All methods include comprehensive logging and error handling.
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="CalculationTypesEndpoint"/> class.
/// </remarks>
/// <param name="logger">The logger instance for diagnostic logging.</param>
/// <param name="apiHelper">The API helper providing configured HTTP client access.</param>
public class CalculationTypesEndpoint(ILogger<CalculationTypesEndpoint> logger, IApiHelper apiHelper) : ICalculationTypesEndpoint
{
	private readonly ILogger<CalculationTypesEndpoint> _logger = logger;
	private readonly IApiHelper _apiHelper = apiHelper;

	/// <summary>
	/// Retrieves all Calculation Types from the API.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains
	/// a list of <see cref="CalculationTypeFullModel"/> objects, or <c>null</c> if the response is empty.
	/// </returns>
	/// <exception cref="HttpIOException">
	/// Thrown when the HTTP response does not indicate success.
	/// </exception>
	public async Task<List<CalculationTypeFullModel>?> GetAllAsync()
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called", nameof(GetAllAsync));
		}

		Uri apiEndpoint = new(Resources.CalculationTypesEndpoint, UriKind.Relative);

		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} HttpResponse Had a Success Code: {ResponseCode} {ResponsePhrase}", nameof(GetAllAsync), response.StatusCode, response.ReasonPhrase);
			}

			List<CalculationTypeFullModel>? result = await response.Content.ReadFromJsonAsync<List<CalculationTypeFullModel>>().ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} received {ResponseContent}", nameof(GetAllAsync), result);
			}

			return result;
		}
		else
		{
			HttpIOException ex = new(HttpRequestError.InvalidResponse, response.ReasonPhrase);

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} HttpResponse Did Not Have a Success Code: {ResponseCode} {ResponsePhrase}", nameof(GetAllAsync), response.StatusCode, response.ReasonPhrase);
			}

			throw ex;
		}
	}

	/// <summary>
	/// Retrieves a specific Calculation Type by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to retrieve.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains
	/// the <see cref="CalculationTypeFullModel"/> if found, or <c>null</c> if not found.
	/// </returns>
	/// <exception cref="HttpIOException">
	/// Thrown when the HTTP response does not indicate success.
	/// </exception>
	public async Task<CalculationTypeFullModel?> GetByIdAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {Id}", nameof(GetByIdAsync), id);
		}

		Uri apiEndpoint = new($"{Resources.CalculationTypesEndpoint}/{id}", UriKind.Relative);

		using HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(apiEndpoint).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} HttpResponse Had a Success Code: {ResponseCode} {ResponsePhrase}", nameof(GetByIdAsync), response.StatusCode, response.ReasonPhrase);
			}

			CalculationTypeFullModel? result = await response.Content.ReadFromJsonAsync<CalculationTypeFullModel>().ConfigureAwait(false);

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
				_logger.LogError(ex, "{Method} HttpResponse Did Not Have a Success Code: {ResponseCode} {ResponsePhrase}", nameof(GetAllAsync), response.StatusCode, response.ReasonPhrase);
			}

			throw ex;
		}
	}

	/// <summary>
	/// Creates a new Calculation Type via the API.
	/// </summary>
	/// <param name="model">The Calculation Type model containing the data for the new record.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains
	/// the created <see cref="CalculationTypeFullModel"/> with server-generated values, or <c>null</c>.
	/// </returns>
	/// <exception cref="HttpIOException">
	/// Thrown when the HTTP response does not indicate success.
	/// </exception>
	public async Task<CalculationTypeFullModel?> CreateAsync(CalculationTypeFullModel model)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {Model}", nameof(CreateAsync), model);
		}

		Uri apiEndpoint = new(Resources.CalculationTypesEndpoint, UriKind.Relative);

		using HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync(apiEndpoint, model).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} HttpResponse Had a Success Code: {ResponseCode} {ResponsePhrase}", nameof(CreateAsync), response.StatusCode, response.ReasonPhrase);
			}

			CalculationTypeFullModel? result = await response.Content.ReadFromJsonAsync<CalculationTypeFullModel>().ConfigureAwait(false);

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
	/// Updates an existing Calculation Type via the API.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to update.</param>
	/// <param name="model">The Calculation Type model containing the updated data.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains
	/// the updated <see cref="CalculationTypeFullModel"/>, or <c>null</c>.
	/// </returns>
	/// <exception cref="HttpIOException">
	/// Thrown when the HTTP response does not indicate success.
	/// </exception>
	public async Task<CalculationTypeFullModel?> UpdateAsync(int id, CalculationTypeFullModel model)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {Id} and {Model}", nameof(UpdateAsync), id, model);
		}

		Uri apiEndpoint = new($"{Resources.CalculationTypesEndpoint}/{id}", UriKind.Relative);

		using HttpResponseMessage response = await _apiHelper.ApiClient.PutAsJsonAsync(apiEndpoint, model).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} HttpResponse Had a Success Code: {ResponseCode} {ResponsePhrase}", nameof(UpdateAsync), response.StatusCode, response.ReasonPhrase);
			}

			CalculationTypeFullModel? result = await response.Content.ReadFromJsonAsync<CalculationTypeFullModel>().ConfigureAwait(false);

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
				_logger.LogError(ex, "{Method} HttpResponse Did Not Have a Success Code: {ResponseCode} {ResponsePhrase}", nameof(CreateAsync), response.StatusCode, response.ReasonPhrase);
			}

			throw ex;
		}
	}

	/// <summary>
	/// Deletes a Calculation Type from the API.
	/// </summary>
	/// <param name="id">The unique identifier of the Calculation Type to delete.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// </returns>
	/// <exception cref="HttpIOException">
	/// Thrown when the HTTP response does not indicate success.
	/// </exception>
	public async Task DeleteAsync(int id)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {Id}", nameof(DeleteAsync), id);
		}

		Uri apiEndpoint = new($"{Resources.CalculationTypesEndpoint}/{id}", UriKind.Relative);

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
