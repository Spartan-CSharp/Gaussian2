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
	public IActionResult Index()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action}", HttpContext.Request.Method, nameof(HomeController), nameof(Index));
			}

			return View();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(HomeController), nameof(Index));
			}

			return RedirectToAction(nameof(Error));
		}
	}

	/// <summary>
	/// Displays the privacy policy page.
	/// </summary>
	/// <returns>The Privacy view.</returns>
	[HttpGet]
	public IActionResult Privacy()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action}", HttpContext.Request.Method, nameof(HomeController), nameof(Privacy));
			}

			return View();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(HomeController), nameof(Privacy));
			}

			return RedirectToAction(nameof(Error));
		}
	}

	/// <summary>
	/// Displays the About page.
	/// </summary>
	/// <returns>The About view.</returns>
	[HttpGet]
	public IActionResult About()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action}", HttpContext.Request.Method, nameof(HomeController), nameof(About));
			}

			return View();
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(HomeController), nameof(About));
			}

			return RedirectToAction(nameof(Error));
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
	public IActionResult Error()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action}", HttpContext.Request.Method, nameof(HomeController), nameof(Error));
			}

			string requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
			ErrorViewModel errorModel = new()
			{
				RequestId = requestId
			};

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError("{Method} {Controller} {Action} {ErrorViewModel}", HttpContext.Request.Method, nameof(HomeController), nameof(Error), errorModel.RequestId);
			}

			return View(errorModel);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(HomeController), nameof(Error));
			}

			return View();
		}
	}
}
