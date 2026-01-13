namespace GaussianMVC.Models;

/// <summary>
/// This is the demo WeatherForecast model.
/// </summary>
public class WeatherForecast
{
	/// <summary>
	/// The Date of the Weather Forecast
	/// </summary>
	public DateOnly Date { get; set; }

	/// <summary>
	/// The temperature in degrees Celsius
	/// </summary>
	public int TemperatureC { get; set; }

	/// <summary>
	/// The temperature in degrees Fahrenheit
	/// </summary>
	public int TemperatureF
	{
		get
		{
			return 32 + (int)(TemperatureC / 0.5556);
		}
	}

	/// <summary>
	/// The weather forecast summary
	/// </summary>
	public string? Summary { get; set; }
}
