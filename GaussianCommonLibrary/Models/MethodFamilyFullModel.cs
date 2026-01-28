namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents a complete model for a Method Family used in Gaussian calculations.
/// Contains all properties including metadata such as creation and update timestamps.
/// </summary>
public class MethodFamilyFullModel
{
	/// <summary>
	/// Gets or sets the unique identifier for the Method Family.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the display name of the Method Family.
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the Rich Text Format (RTF) description of the Method Family.
	/// </summary>
	public string? DescriptionRtf { get; set; }

	/// <summary>
	/// Gets or sets the plain text description of the Method Family.
	/// </summary>
	public string? DescriptionText { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the Method Family was created.
	/// Defaults to the current date and time.
	/// </summary>
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when the Method Family was last updated.
	/// Defaults to the current date and time.
	/// </summary>
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether the Method Family is archived.
	/// </summary>
	public bool Archived { get; set; }

	/// <summary>
	/// Converts the current Method Family full model to a simplified Method Family record.
	/// </summary>
	/// <returns>A new <see cref="MethodFamilyRecord"/> instance containing the Id and Name properties.</returns>
	public MethodFamilyRecord ToRecord()
	{
		return new MethodFamilyRecord(Id, Name);
	}

	/// <summary>
	/// Returns a string representation of the Method Family.
	/// </summary>
	/// <returns>A string in the format "Name".</returns>
	public override string? ToString()
	{
		return $"{Name}";
	}
}
