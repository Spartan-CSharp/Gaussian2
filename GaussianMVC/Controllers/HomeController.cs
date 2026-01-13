using System.Diagnostics;

using GaussianMVC.Models;

using Microsoft.AspNetCore.Mvc;

namespace GaussianMVC.Controllers;

/// <summary>
/// The Home Controller
/// </summary>
public class HomeController : Controller
{
	/// <summary>
	/// Returns the Index View
	/// </summary>
	/// <returns></returns>
	public IActionResult Index()
	{
		return View();
	}

	/// <summary>
	/// Returns the Privacy View
	/// </summary>
	/// <returns></returns>
	public IActionResult Privacy()
	{
		return View();
	}

	/// <summary>
	/// Returns the Error View
	/// </summary>
	/// <returns></returns>
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
