using System.ComponentModel.DataAnnotations;

using GaussianCommonLibrary.Models;

namespace GaussianMVC.Models.APIModels.V1;

/// <summary>
/// API model representing a Calculation Type for REST API operations.
/// Provides data transfer and validation for Calculation Type entities in API v1.
/// </summary>
public class CalculationTypeAPIModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CalculationTypeAPIModel"/> class.
	/// </summary>
	public CalculationTypeAPIModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CalculationTypeAPIModel"/> class
	/// from a <see cref="CalculationTypeFullModel"/>.
	/// </summary>
	/// <param name="model">The full model containing Calculation Type data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public CalculationTypeAPIModel(CalculationTypeFullModel model)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Gets or sets the unique identifier for the Calculation Type.
	/// </summary>
	[Display(Name = "Id")]
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the name of the Calculation Type.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Name")]
	[MaxLength(200)]
	[Required]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the keyword identifier for the Calculation Type.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Keyword")]
	[MaxLength(30)]
	[Required]
	public string Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the description in Rich Text Format (RTF).
	/// </summary>
	[DataType(DataType.MultilineText)]
	[Display(Name = "Description (RTF)")]
	public string? DescriptionRtf { get; set; }

	/// <summary>
	/// Gets or sets the plain text description of the Calculation Type.
	/// </summary>
	[DataType(DataType.MultilineText)]
	[Display(Name = "Description (Text)")]
	[MaxLength(2000)]
	public string? DescriptionText { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the Calculation Type was created.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Created Date")]
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when the Calculation Type was last updated.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Last Updated Date")]
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether the Calculation Type is archived.
	/// </summary>
	[Display(Name = "Archived")]
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this API model to a <see cref="CalculationTypeFullModel"/> instance.
	/// </summary>
	/// <returns>A new <see cref="CalculationTypeFullModel"/> with values copied from this API model.</returns>
	public CalculationTypeFullModel ToFullModel()
	{
		return new CalculationTypeFullModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Returns a string representation of the Calculation Type.
	/// </summary>
	/// <returns>A string in the format "Name (Keyword)".</returns>
	public override string? ToString()
	{
		return $"{Name} ({Keyword})";
	}
}
