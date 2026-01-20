using System.ComponentModel.DataAnnotations;

using GaussianCommonLibrary.Models;

namespace GaussianMVC.Models;

/// <summary>
/// View model representing a calculation type for the Gaussian MVC application.
/// Provides data transfer and validation between the view layer and the business logic.
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
	/// <param name="model">The full model containing calculation type data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public CalculationTypeViewModel(CalculationTypeFullModel model)
	{
		if (model is null)
		{
			throw new ArgumentNullException(nameof(model), $"The parameter {nameof(model)} cannot be null.");
		}

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
	/// Gets or sets the unique identifier for the calculation type.
	/// </summary>
	[Display(Name = "Id")]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the name of the calculation type.
	/// </summary>
	/// <remarks>
	/// This field is required and has a maximum length of 200 characters.
	/// </remarks>
	[Display(Name = "Name")]
	[Required]
	[MaxLength(200)]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the keyword identifier for the calculation type.
	/// </summary>
	/// <remarks>
	/// This field is required and has a maximum length of 20 characters.
	/// Used as a short identifier or code for the calculation type.
	/// </remarks>
	[Display(Name = "Keyword")]
	[Required]
	[MaxLength(20)]
	public string Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the description in Rich Text Format (RTF).
	/// </summary>
	/// <remarks>
	/// This field is optional and can contain formatted text description.
	/// </remarks>
	[Display(Name = "Description (RTF)")]
	public string? DescriptionRtf { get; set; }

	/// <summary>
	/// Gets or sets the plain text description of the calculation type.
	/// </summary>
	/// <remarks>
	/// This field is optional and has a maximum length of 2000 characters.
	/// </remarks>
	[Display(Name = "Description (Text)")]
	[MaxLength(2000)]
	public string? DescriptionText { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the calculation type was created.
	/// </summary>
	/// <remarks>
	/// Defaults to the current date and time when a new instance is created.
	/// </remarks>
	[Display(Name = "Created Date")]
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when the calculation type was last updated.
	/// </summary>
	/// <remarks>
	/// Defaults to the current date and time when a new instance is created.
	/// Should be updated whenever the calculation type is modified.
	/// </remarks>
	[Display(Name = "Last Updated Date")]
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether the calculation type is archived.
	/// </summary>
	/// <remarks>
	/// Defaults to <c>false</c>. When set to <c>true</c>, the calculation type
	/// is considered archived and may be hidden from active lists.
	/// </remarks>
	[Display(Name = "Archived")]
	public bool Archived { get; set; } = false;

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
	/// Returns a string representation of the calculation type.
	/// </summary>
	/// <returns>
	/// A string in the format "Name (Keyword)", combining the calculation type's name
	/// and keyword identifier. Returns null if both <see cref="Name"/> and <see cref="Keyword"/> are null.
	/// </returns>
	public override string? ToString()
	{
		return $"{Name} ({Keyword})";
	}
}
