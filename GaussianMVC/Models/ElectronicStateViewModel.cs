using System.ComponentModel.DataAnnotations;

using GaussianCommonLibrary.Models;

using GaussianMVC.ValidationAttributes;

using GaussianMVCLibrary.Converters;

namespace GaussianMVC.Models;

/// <summary>
/// View model representing an electronic state for the Gaussian MVC application.
/// Provides data validation and conversion capabilities for electronic state entities.
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
		DescriptionHtml = RtfConverter.RtfToHtml(model.DescriptionRtf);
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Gets or sets the unique identifier for the electronic state.
	/// </summary>
	[Display(Name = "Id")]
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the name of the electronic state.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Name")]
	[MaxLength(50)]
	[RequireAtLeastOne(nameof(Keyword), ErrorMessage = "Either Name or Keyword must be provided.")]
	public string? Name { get; set; }

	/// <summary>
	/// Gets or sets the keyword associated with the electronic state.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Keyword")]
	[MaxLength(20)]
	[RequireAtLeastOne(nameof(Name), ErrorMessage = "Either Name or Keyword must be provided.")]
	public string? Keyword { get; set; }

	/// <summary>
	/// Gets or sets the description in HTML format.
	/// </summary>
	[DataType(DataType.MultilineText)]
	[Display(Name = "Description")]
	public string? DescriptionHtml { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the electronic state was created.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Created Date")]
	public DateTime CreatedDate { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the electronic state was last updated.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Last Updated Date")]
	public DateTime LastUpdatedDate { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the electronic state is archived.
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
			DescriptionRtf = RtfConverter.HtmlToRtf(DescriptionHtml),
			DescriptionText = RtfConverter.HtmlToPlainText(DescriptionHtml),
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Returns a string representation of the electronic state.
	/// </summary>
	/// <returns>The Name if available, otherwise the Keyword, or a combination of both in the format "Name/Keyword".</returns>
	public override string? ToString()
	{
		return string.IsNullOrWhiteSpace(Name) ? $"{Keyword}" : string.IsNullOrWhiteSpace(Keyword) ? $"{Name}" : $"{Name}/{Keyword}";
	}
}
