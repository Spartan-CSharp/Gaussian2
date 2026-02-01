namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents a family of computational methods used in Gaussian calculations.
/// </summary>
public class MethodFamilyFullModel
{
	/// <summary>
	/// Gets or sets the unique identifier for the method family.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the display name of the method family.
	/// </summary>
	public string Name { get; set; } = string.Empty;

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
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when this record was last updated.
	/// </summary>
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether this method family is archived.
	/// </summary>
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this model to a <see cref="MethodFamilyRecord"/>.
	/// </summary>
	/// <returns>A record containing the identifier and name of this method family.</returns>
	public MethodFamilyRecord ToRecord()
	{
		return new MethodFamilyRecord(Id, Name);
	}

	/// <summary>
	/// Returns the name of the method family.
	/// </summary>
	/// <returns>The method family name.</returns>
	public override string? ToString()
	{
		return $"{Name}";
	}
}
