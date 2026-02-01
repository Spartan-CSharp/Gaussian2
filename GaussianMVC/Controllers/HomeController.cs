using System.Diagnostics;

using GaussianMVC.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GaussianMVC.Controllers;

/// <summary>
/// Handles the main navigation pages for the application including the home page, privacy policy, and error handling.
/// </summary>
/// <param name="logger">The logger instance for logging controller actions and events.</param>
[AllowAnonymous]
public class HomeController(ILogger<HomeController> logger) : Controller
{
	private readonly ILogger<HomeController> _logger = logger;

	/// <summary>
	/// Displays the application's home page.
	/// </summary>
	/// <returns>The Index view.</returns>
	[HttpGet]
	public ActionResult Index()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(HomeController), nameof(Index));
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning.", HttpContext.Request.Method, nameof(HomeController), nameof(Index));
			}

			return View();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(HomeController), nameof(Index));
			}

			Response.StatusCode = StatusCodes.Status500InternalServerError;

			ErrorViewModel evm = new()
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				StatusCode = StatusCodes.Status500InternalServerError,
				StatusPhrase = "Internal Server Error",
				ExceptionType = ex.GetType().Name,
				ExceptionMessage = ex.Message
			};

			return View("Error", evm);
		}
	}

	/// <summary>
	/// Displays the privacy policy page.
	/// </summary>
	/// <returns>The Privacy view.</returns>
	[HttpGet]
	public ActionResult Privacy()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(HomeController), nameof(Privacy));
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning.", HttpContext.Request.Method, nameof(HomeController), nameof(Privacy));
			}

			return View();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(HomeController), nameof(Privacy));
			}

			Response.StatusCode = StatusCodes.Status500InternalServerError;

			ErrorViewModel evm = new()
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				StatusCode = StatusCodes.Status500InternalServerError,
				StatusPhrase = "Internal Server Error",
				ExceptionType = ex.GetType().Name,
				ExceptionMessage = ex.Message
			};

			return View("Error", evm);
		}
	}

	/// <summary>
	/// Displays the About page.
	/// </summary>
	/// <returns>The About view.</returns>
	[HttpGet]
	public ActionResult About()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(HomeController), nameof(About));
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning.", HttpContext.Request.Method, nameof(HomeController), nameof(About));
			}

			return View();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(HomeController), nameof(About));
			}

			Response.StatusCode = StatusCodes.Status500InternalServerError;

			ErrorViewModel evm = new()
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				StatusCode = StatusCodes.Status500InternalServerError,
				StatusPhrase = "Internal Server Error",
				ExceptionType = ex.GetType().Name,
				ExceptionMessage = ex.Message
			};

			return View("Error", evm);
		}
	}

	/// <summary>
	/// Displays the Contact page.
	/// </summary>
	/// <returns>The Contact view.</returns>
	[HttpGet]
	public ActionResult Contact()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(HomeController), nameof(Contact));
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning.", HttpContext.Request.Method, nameof(HomeController), nameof(Contact));
			}

			return View();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(HomeController), nameof(Contact));
			}

			Response.StatusCode = StatusCodes.Status500InternalServerError;

			ErrorViewModel evm = new()
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				StatusCode = StatusCodes.Status500InternalServerError,
				StatusPhrase = "Internal Server Error",
				ExceptionType = ex.GetType().Name,
				ExceptionMessage = ex.Message
			};

			return View("Error", evm);
		}
	}

	/// <summary>
	/// Displays the error page with diagnostic information.
	/// </summary>
	/// <returns>The Error view with an <see cref="ErrorViewModel"/> containing the request identifier.</returns>
	/// <remarks>
	/// This action is not cached to ensure fresh error information is displayed for each request.
	/// The request identifier is derived from the current activity or HTTP context trace identifier.
	/// </remarks>
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	[HttpGet]
	public ActionResult<ErrorViewModel> Error()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(HomeController), nameof(Error));
			}

			ErrorViewModel errorModel = new()
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
			};

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(HomeController), nameof(Error), nameof(ErrorViewModel), errorModel);
			}

			return View(errorModel);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(HomeController), nameof(Error));
			}

			Response.StatusCode = StatusCodes.Status500InternalServerError;

			ErrorViewModel evm = new()
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				StatusCode = StatusCodes.Status500InternalServerError,
				StatusPhrase = "Internal Server Error",
				ExceptionType = ex.GetType().Name,
				ExceptionMessage = ex.Message
			};

			return View(evm);
		}
	}
}
