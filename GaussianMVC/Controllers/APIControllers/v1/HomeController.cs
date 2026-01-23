using System.Diagnostics;

using Asp.Versioning;

using GaussianMVC.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GaussianMVC.Controllers.APIControllers.V1;

/// <summary>
/// API controller for home-related endpoints providing general application information.
/// This is version 1.0 of the Home API controller.
/// </summary>
/// <remarks>
/// This controller provides RESTful endpoints for accessing general application information
/// including welcome messages, privacy information, about details, contact information,
/// and error handling. All endpoints are accessible without authentication.
/// </remarks>
/// <param name="logger">The logger instance for logging application events and errors.</param>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[AllowAnonymous]
public class HomeController(ILogger<HomeController> logger) : ControllerBase
{
	private readonly ILogger<HomeController> _logger = logger;

	/// <summary>
	/// Gets the welcome message for the GaussianMVC API.
	/// </summary>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing a welcome message string.
	/// Returns HTTP 200 (OK) with the welcome message on success,
	/// or HTTP 500 (Internal Server Error) if an exception occurs.
	/// </returns>
	/// <remarks>
	/// GET: api/v1/Home
	/// 
	/// This endpoint returns a simple welcome message identifying the API version.
	/// Debug logging is performed if debug level logging is enabled.
	/// </remarks>
	/// <response code="200">Returns the welcome message successfully.</response>
	/// <response code="500">An internal server error occurred.</response>
	// GET: api/v1/Home
	[HttpGet]
	public async Task<ActionResult<string>> GetIndexAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action}", nameof(HomeController), nameof(GetIndexAsync));
			}

			string response = "Welcome to GaussianMVC API v1.0";
			return Ok(response);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} had an error", nameof(HomeController), nameof(GetIndexAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Gets the privacy policy information.
	/// </summary>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing privacy policy information as a string.
	/// Returns HTTP 200 (OK) with the privacy message on success,
	/// or HTTP 500 (Internal Server Error) if an exception occurs.
	/// </returns>
	/// <remarks>
	/// GET: api/v1/Home/Privacy
	/// 
	/// This endpoint returns information about the site's privacy policy.
	/// Debug logging is performed if debug level logging is enabled.
	/// </remarks>
	/// <response code="200">Returns the privacy policy information successfully.</response>
	/// <response code="500">An internal server error occurred.</response>
	// GET: api/v1/Home/Privacy
	[HttpGet("Privacy")]
	public async Task<ActionResult<string>> GetPrivacyAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action}", nameof(HomeController), nameof(GetPrivacyAsync));
			}

			string response = "Use this page to detail your site's privacy policy.";
			return Ok(response);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} had an error", nameof(HomeController), nameof(GetPrivacyAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Gets information about the Gaussian Web Application.
	/// </summary>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing detailed information about the application.
	/// Returns HTTP 200 (OK) with the about information on success,
	/// or HTTP 500 (Internal Server Error) if an exception occurs.
	/// </returns>
	/// <remarks>
	/// GET: api/v1/Home/About
	/// 
	/// This endpoint returns comprehensive information about the Gaussian Web Application,
	/// including its purpose, creators, and functionality. The application is designed to
	/// store and index results of electronic structure calculations performed with the
	/// Gaussian series of programs.
	/// 
	/// More information is available at: https://github.com/Spartan-CSharp/Gaussian2
	/// </remarks>
	/// <response code="200">Returns the application information successfully.</response>
	/// <response code="500">An internal server error occurred.</response>
	// GET: api/v1/Home/About
	[HttpGet("About")]
	public async Task<ActionResult<string>> GetAboutAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action}", nameof(HomeController), nameof(GetAboutAsync));
			}

			string response = "Gaussian Web Application by Pierre J.-L. Plourde & Spartan C#. This C# application & SQL database allow storing and indexing of results of electronic structure calculations performed with the Gaussian series of programs. For more information: https://github.com/Spartan-CSharp/Gaussian2";
			return Ok(response);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} had an error", nameof(HomeController), nameof(GetAboutAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Gets contact information for the Gaussian Web Application support and marketing.
	/// </summary>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing contact details including address, phone, and email.
	/// Returns HTTP 200 (OK) with the contact information on success,
	/// or HTTP 500 (Internal Server Error) if an exception occurs.
	/// </returns>
	/// <remarks>
	/// GET: api/v1/Home/Contact
	/// 
	/// This endpoint returns complete contact information for Spartan C#,
	/// including physical address, phone number, support email, and marketing email.
	/// </remarks>
	/// <response code="200">Returns the contact information successfully.</response>
	/// <response code="500">An internal server error occurred.</response>
	// GET: api/v1/Home/Contact
	[HttpGet("Contact")]
	public async Task<ActionResult<string>> GetContactAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action}", nameof(HomeController), nameof(GetContactAsync));
			}

			string response = "Pierre J.-L. Plourde, Spartan C#, 76 Delwood Drive, Upper Unit, Scarborough, Ontario, Canada M1L 2S7. P: 905.439.7645.  Support: pierre@spartancsharp.net. Marketing: info@spartancsharp.net";
			return Ok(response);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} had an error", nameof(HomeController), nameof(GetContactAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Gets error information for diagnostic purposes.
	/// </summary>
	/// <returns>
	/// An <see cref="ActionResult{T}"/> containing an <see cref="ErrorViewModel"/> with request tracking information.
	/// Returns HTTP 200 (OK) with the error model on success,
	/// or HTTP 500 (Internal Server Error) if an exception occurs.
	/// </returns>
	/// <remarks>
	/// GET: api/v1/Home/Error
	/// 
	/// This endpoint returns an error view model containing the current request identifier
	/// for tracking and diagnostic purposes. The request ID is obtained from either the
	/// current Activity or the HttpContext TraceIdentifier.
	/// </remarks>
	/// <response code="200">Returns the error view model successfully.</response>
	/// <response code="500">An internal server error occurred.</response>
	// GET: api/v1/Home/Error
	[HttpGet("Error")]
	public async Task<ActionResult<ErrorViewModel>> GetErrorAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action}", nameof(HomeController), nameof(GetErrorAsync));
			}

			string requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

			ErrorViewModel errorModel = new()
			{
				RequestId = requestId
			};

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError("{Controller} {Action} {ErrorViewModel}", nameof(HomeController), nameof(GetErrorAsync), errorModel);
			}

			return Ok(errorModel);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} had an error", nameof(HomeController), nameof(GetErrorAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}
}
