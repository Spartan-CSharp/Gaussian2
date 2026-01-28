using System.ComponentModel.DataAnnotations;

using GaussianCommonLibrary.Models;

namespace GaussianMVC.Models;

/// <summary>
/// View model representing a Method Family for the Gaussian MVC application.
/// Provides data transfer and validation between the view layer and the business logic.
/// </summary>
public class MethodFamilyViewModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MethodFamilyViewModel"/> class.
	/// </summary>
	public MethodFamilyViewModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="MethodFamilyViewModel"/> class
	/// from a <see cref="MethodFamilyFullModel"/>.
	/// </summary>
	/// <param name="model">The full model containing Method Family data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public MethodFamilyViewModel(MethodFamilyFullModel model)
	{
		if (model is null)
		{
			throw new ArgumentNullException(nameof(model), $"The parameter {nameof(model)} cannot be null.");
		}

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
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the keyword identifier for the Method Family.
	/// </summary>
	/// <remarks>
	/// This field is required and has a maximum length of 200 characters.
	/// Used as a short identifier or code for the Method Family.
	/// </remarks>
	[Display(Name = "Name")]
	[Required]
	[MaxLength(200)]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the description in Rich Text Format (RTF).
	/// </summary>
	/// <remarks>
	/// This field is optional and can contain formatted text description.
	/// </remarks>
	[Display(Name = "Description (RTF)")]
	public string? DescriptionRtf { get; set; }

	/// <summary>
	/// Gets or sets the plain text description of the Method Family.
	/// </summary>
	/// <remarks>
	/// This field is optional and has a maximum length of 2000 characters.
	/// </remarks>
	[Display(Name = "Description (Text)")]
	[MaxLength(2000)]
	public string? DescriptionText { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the Method Family was created.
	/// </summary>
	/// <remarks>
	/// Defaults to the current date and time when a new instance is created.
	/// </remarks>
	[Display(Name = "Created Date")]
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when the Method Family was last updated.
	/// </summary>
	/// <remarks>
	/// Defaults to the current date and time when a new instance is created.
	/// Should be updated whenever the Method Family is modified.
	/// </remarks>
	[Display(Name = "Last Updated Date")]
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether the Method Family is archived.
	/// </summary>
	/// <remarks>
	/// Defaults to <c>false</c>. When set to <c>true</c>, the Method Family
	/// is considered archived and may be hidden from active lists.
	/// </remarks>
	[Display(Name = "Archived")]
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this view model to a <see cref="MethodFamilyFullModel"/> instance.
	/// </summary>
	/// <returns>A new <see cref="MethodFamilyFullModel"/> with values copied from this view model.</returns>
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
	/// <returns>
	/// A string in the format "Name (Name)", combining the Method Family's name
	/// and keyword identifier. Returns null if <see cref="Name"/> is null.
	/// </returns>
	public override string? ToString()
	{
		return $"{Name}";
	}
}
