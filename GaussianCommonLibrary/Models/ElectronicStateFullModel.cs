namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents an Electronic State used in Gaussian calculations.
/// </summary>
public class ElectronicStateFullModel
{
	/// <summary>
	/// Gets or sets the unique identifier for the Electronic State.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the display name of the Electronic State.
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	/// Gets or sets the keyword used to reference this Electronic State in Gaussian input files.
	/// </summary>
	public string? Keyword { get; set; }

	/// <summary>
	/// Gets or sets the description in Rich Text Format.
	/// </summary>
	public string? DescriptionRtf { get; set; }

	/// <summary>
	/// Gets or sets the plain text description.
	/// </summary>
	public string? DescriptionText { get; set; }

	/// <summary>
	/// Gets or sets the date and time when this record was created.
	/// </summary>
	public DateTime CreatedDate { get; set; }

	/// <summary>
	/// Gets or sets the date and time when this record was last updated.
	/// </summary>
	public DateTime LastUpdatedDate { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether this Electronic State is archived.
	/// </summary>
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this model to a <see cref="ElectronicStateRecord"/>.
	/// </summary>
	/// <returns>A record containing the identifier and name of this Electronic State.</returns>
	public ElectronicStateRecord ToRecord()
	{
		return new ElectronicStateRecord(Id, Name, Keyword);
	}

	/// <summary>
	/// Returns a string representation of the Electronic State.
	/// Returns the keyword if no name is specified, the name if no keyword is specified, or "Name/Keyword" if both are specified.
	/// </summary>
	/// <returns>A formatted string representing the Electronic State.</returns>
	public override string? ToString()
	{
		return string.IsNullOrWhiteSpace(Name) ? $"{Keyword}" : string.IsNullOrWhiteSpace(Keyword) ? $"{Name}" : $"{Name}/{Keyword}";
	}
}
