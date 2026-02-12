using System.ComponentModel.DataAnnotations;

using GaussianCommonLibrary.Models;

using GaussianMVC.ValidationAttributes;

namespace GaussianMVC.Models.APIModels.V1;

/// <summary>
/// API model representing an Spin State for REST API operations.
/// Provides data validation and conversion for Spin State entities in API v1.
/// </summary>
public class SpinStateAPIModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateAPIModel"/> class.
	/// </summary>
	public SpinStateAPIModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateAPIModel"/> class
	/// from an existing <see cref="SpinStateFullModel"/>.
	/// </summary>
	/// <param name="model">The full model to convert from.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public SpinStateAPIModel(SpinStateFullModel model)
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
	/// Gets or sets the unique identifier for the Spin State.
	/// </summary>
	[Display(Name = "Id")]
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the name of the Spin State.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Name")]
	[MaxLength(200)]
	[RequireAtLeastOne(nameof(Keyword), ErrorMessage = "Either Name or Keyword must be provided.")]
	public string? Name { get; set; }

	/// <summary>
	/// Gets or sets the keyword associated with the Spin State.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Keyword")]
	[MaxLength(50)]
	[RequireAtLeastOne(nameof(Name), ErrorMessage = "Either Name or Keyword must be provided.")]
	public string? Keyword { get; set; }

	/// <summary>
	/// Gets or sets the description in Rich Text Format (RTF).
	/// </summary>
	[DataType(DataType.MultilineText)]
	[Display(Name = "Description (RTF)")]
	public string? DescriptionRtf { get; set; }

	/// <summary>
	/// Gets or sets the plain text description of the Spin State.
	/// </summary>
	[DataType(DataType.MultilineText)]
	[Display(Name = "Description (Text)")]
	[MaxLength(4000)]
	public string? DescriptionText { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the Spin State was created.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Created Date")]
	public DateTime CreatedDate { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the Spin State was last updated.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Last Updated Date")]
	public DateTime LastUpdatedDate { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the Spin State is archived.
	/// </summary>
	[Display(Name = "Archived")]
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this API model to an <see cref="SpinStateFullModel"/> instance.
	/// </summary>
	/// <returns>A new <see cref="SpinStateFullModel"/> populated with data from this API model.</returns>
	public SpinStateFullModel ToFullModel()
	{
		return new SpinStateFullModel
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
	/// Converts this API model to a <see cref="SpinStateRecord"/> instance.
	/// </summary>
	/// <returns>A new <see cref="SpinStateRecord"/> containing the Id, Name, and Keyword properties.</returns>
	public SpinStateRecord ToRecord()
	{
		return new SpinStateRecord(Id, Name, Keyword);
	}

	/// <summary>
	/// Returns a string representation of the Spin State.
	/// </summary>
	/// <returns>The Name if available, otherwise the Keyword, or a combination of both in the format "Name/Keyword".</returns>
	public override string? ToString()
	{
		return string.IsNullOrWhiteSpace(Name) ? $"{Keyword}" : string.IsNullOrWhiteSpace(Keyword) ? $"{Name}" : $"{Name}/{Keyword}";
	}
}
