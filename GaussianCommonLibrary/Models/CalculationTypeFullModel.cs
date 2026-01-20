namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents a complete model for a calculation type used in Gaussian calculations.
/// Contains all properties including metadata such as creation and update timestamps.
/// </summary>
public class CalculationTypeFullModel
{
	/// <summary>
	/// Gets or sets the unique identifier for the calculation type.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the display name of the calculation type.
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the keyword identifier used for the calculation type.
	/// </summary>
	public string Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the Rich Text Format (RTF) description of the calculation type.
	/// </summary>
	public string? DescriptionRtf { get; set; }

	/// <summary>
	/// Gets or sets the plain text description of the calculation type.
	/// </summary>
	public string? DescriptionText { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the calculation type was created.
	/// Defaults to the current date and time.
	/// </summary>
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when the calculation type was last updated.
	/// Defaults to the current date and time.
	/// </summary>
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether the calculation type is archived.
	/// </summary>
	public bool Archived { get; set; }

	/// <summary>
	/// Returns a string representation of the calculation type.
	/// </summary>
	/// <returns>A string in the format "Name (Keyword)".</returns>
	public override string? ToString()
	{
		return $"{Name} ({Keyword})";
	}
}
