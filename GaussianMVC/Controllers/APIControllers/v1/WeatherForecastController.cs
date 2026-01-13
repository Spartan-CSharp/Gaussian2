using Asp.Versioning;

using GaussianMVC.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable IDE0130
namespace GaussianMVC.Controllers.APIControllers;

/// <summary>
/// This is the Weather Forecast Controller, a demo API controller
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[AllowAnonymous]
public class WeatherForecastController : ControllerBase
{
	private static readonly string[] _summaries =
	[
		"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
	];

	/// <summary>
	/// Retrieves a collection of weather forecast data for the next five days.
	/// </summary>
	/// <returns>An enumerable collection of <see cref="WeatherForecast"/> objects, each representing the forecast for a single day.</returns>
	[HttpGet(Name = "GetWeatherForecast")]
	public IEnumerable<WeatherForecast> Get()
	{
		return [.. Enumerable.Range(1, 5).Select(index => new WeatherForecast
		{
			Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
			TemperatureC = Random.Shared.Next(-20, 55),
			Summary = _summaries[Random.Shared.Next(_summaries.Length)]
		})];
	}
}
