using System.ComponentModel.DataAnnotations;

using GaussianCommonLibrary.Models;

namespace GaussianWPF.Models;

/// <summary>
/// View model representing an Electronic State for the Gaussian WPF application.
/// Provides data binding and validation for Electronic State operations in the desktop client.
/// </summary>
public class ElectronicStateViewModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateViewModel"/> class.
	/// </summary>
	public ElectronicStateViewModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateViewModel"/> class
	/// from an existing <see cref="ElectronicStateFullModel"/>.
	/// </summary>
	/// <param name="model">The full model to convert from.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public ElectronicStateViewModel(ElectronicStateFullModel model)
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
	/// Gets or sets the unique identifier for the Electronic State.
	/// </summary>
	[Display(Name = "Id")]
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the name of the Electronic State.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Name")]
	[MaxLength(200)]
	public string? Name { get; set; }

	/// <summary>
	/// Gets or sets the keyword associated with the Electronic State.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Keyword")]
	[MaxLength(50)]
	public string? Keyword { get; set; }

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
	[MaxLength(4000)]
	public string? DescriptionText { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the Electronic State was created.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Created Date")]
	public DateTime CreatedDate { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the Electronic State was last updated.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Last Updated Date")]
	public DateTime LastUpdatedDate { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the Electronic State is archived.
	/// </summary>
	[Display(Name = "Archived")]
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this view model to an <see cref="ElectronicStateFullModel"/> instance.
	/// </summary>
	/// <returns>A new <see cref="ElectronicStateFullModel"/> populated with data from this view model.</returns>
	public ElectronicStateFullModel ToFullModel()
	{
		return new ElectronicStateFullModel
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
	/// Converts this view model to a <see cref="ElectronicStateRecord"/> instance.
	/// </summary>
	/// <returns>A new <see cref="ElectronicStateRecord"/> containing the Id, Name, and Keyword properties.</returns>
	public ElectronicStateRecord ToRecord()
	{
		return new ElectronicStateRecord(Id, Name, Keyword);
	}

	/// <summary>
	/// Returns a string representation of the Electronic State.
	/// </summary>
	/// <returns>The Name if available, otherwise the Keyword, or a combination of both in the format "Name/Keyword".</returns>
	public override string? ToString()
	{
		return string.IsNullOrWhiteSpace(Name) ? $"{Keyword}" : string.IsNullOrWhiteSpace(Keyword) ? $"{Name}" : $"{Name}/{Keyword}";
	}
}
