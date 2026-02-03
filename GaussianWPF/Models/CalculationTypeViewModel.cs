using System.ComponentModel.DataAnnotations;

using GaussianCommonLibrary.Models;

namespace GaussianWPF.Models;

/// <summary>
/// View model representing a Calculation Type for the Gaussian WPF application.
/// Provides data binding and validation for calculation type operations in the desktop client.
/// </summary>
public class CalculationTypeViewModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CalculationTypeViewModel"/> class.
	/// </summary>
	public CalculationTypeViewModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CalculationTypeViewModel"/> class
	/// from a <see cref="CalculationTypeFullModel"/>.
	/// </summary>
	/// <param name="model">The full model containing Calculation Type data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public CalculationTypeViewModel(CalculationTypeFullModel model)
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
	[Required]
	[MaxLength(200)]
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
	/// Gets or sets the description in RTF format.
	/// </summary>
	[DataType(DataType.MultilineText)]
	[Display(Name = "Description")]
	public string? DescriptionRtf { get; set; }

	/// <summary>
	/// Gets or sets the description in plain text format.
	/// </summary>
	[DataType(DataType.MultilineText)]
	[Display(Name = "Description")]
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
	/// Converts this view model to a <see cref="CalculationTypeFullModel"/> instance.
	/// </summary>
	/// <returns>A new <see cref="CalculationTypeFullModel"/> with values copied from this view model.</returns>
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
