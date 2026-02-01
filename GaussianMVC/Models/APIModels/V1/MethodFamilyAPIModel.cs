using System.ComponentModel.DataAnnotations;

using GaussianCommonLibrary.Models;

namespace GaussianMVC.Models.APIModels.V1;

/// <summary>
/// API model representing a Method Family for REST API operations.
/// Provides data transfer and validation for Method Family entities in API v1.
/// </summary>
public class MethodFamilyAPIModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MethodFamilyAPIModel"/> class.
	/// </summary>
	public MethodFamilyAPIModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="MethodFamilyAPIModel"/> class
	/// from a <see cref="MethodFamilyFullModel"/>.
	/// </summary>
	/// <param name="model">The full model containing Method Family data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public MethodFamilyAPIModel(MethodFamilyFullModel model)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		Id = model.Id;
		Name = model.Name;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Gets or sets the unique identifier for the Method Family.
	/// </summary>
	[Display(Name = "Id")]
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the name of the Method Family.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Name")]
	[MaxLength(200)]
	[Required]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the description in Rich Text Format (RTF).
	/// </summary>
	[DataType(DataType.MultilineText)]
	[Display(Name = "Description (RTF)")]
	public string? DescriptionRtf { get; set; }

	/// <summary>
	/// Gets or sets the plain text description of the Method Family.
	/// </summary>
	[DataType(DataType.MultilineText)]
	[Display(Name = "Description (Text)")]
	[MaxLength(2000)]
	public string? DescriptionText { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the Method Family was created.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Created Date")]
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when the Method Family was last updated.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Last Updated Date")]
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether the Method Family is archived.
	/// </summary>
	[Display(Name = "Archived")]
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this API model to a <see cref="MethodFamilyFullModel"/> instance.
	/// </summary>
	/// <returns>A new <see cref="MethodFamilyFullModel"/> with values copied from this API model.</returns>
	public MethodFamilyFullModel ToFullModel()
	{
		return new MethodFamilyFullModel
		{
			Id = Id,
			Name = Name,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this API model to a <see cref="MethodFamilyRecord"/> instance.
	/// </summary>
	/// <returns>A new <see cref="MethodFamilyRecord"/> containing the Id and Name properties.</returns>
	public MethodFamilyRecord ToRecord()
	{
		return new MethodFamilyRecord(Id, Name);
	}

	/// <summary>
	/// Returns a string representation of the Method Family.
	/// </summary>
	/// <returns>The name of the Method Family.</returns>
	public override string? ToString()
	{
		return $"{Name}";
	}
}
