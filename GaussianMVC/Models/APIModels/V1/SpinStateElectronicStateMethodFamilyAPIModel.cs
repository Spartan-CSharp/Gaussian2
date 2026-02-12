using System.ComponentModel.DataAnnotations;

using GaussianCommonLibrary.Models;

using GaussianMVC.ValidationAttributes;

namespace GaussianMVC.Models.APIModels.V1;

/// <summary>
/// API model representing a Spin State/Electronic State/Method Family Combination for REST API operations.
/// Provides data transfer and validation for Spin State/Electronic State/Method Family Combination entities in API v1.
/// </summary>
public class SpinStateElectronicStateMethodFamilyAPIModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyAPIModel"/> class.
	/// </summary>
	public SpinStateElectronicStateMethodFamilyAPIModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyAPIModel"/> class from a full model.
	/// </summary>
	/// <param name="model">The Spin State/Electronic State/Method Family Combination full model containing the source data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilyAPIModel(SpinStateElectronicStateMethodFamilyFullModel model)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateMethodFamilyId = model.ElectronicStateMethodFamily.Id;
		SpinStateId = model.SpinState?.Id;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyAPIModel"/> class from an intermediate model.
	/// </summary>
	/// <param name="model">The Spin State/Electronic State/Method Family Combination intermediate model containing the source data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilyAPIModel(SpinStateElectronicStateMethodFamilyIntermediateModel model)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateMethodFamilyId = model.ElectronicStateMethodFamily.Id;
		SpinStateId = model.SpinState?.Id;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyAPIModel"/> class from a simple model.
	/// </summary>
	/// <param name="model">The Spin State/Electronic State/Method Family Combination simple model containing the source data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilyAPIModel(SpinStateElectronicStateMethodFamilySimpleModel model)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateMethodFamilyId = model.ElectronicStateMethodFamilyId;
		SpinStateId = model.SpinStateId;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Gets or sets the unique identifier for the Spin State/Electronic State/Method Family Combination.
	/// </summary>
	[Display(Name = "Id")]
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the name of the Spin State/Electronic State/Method Family Combination.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Name")]
	[MaxLength(200)]
	[RequireAtLeastOne(nameof(Keyword), ErrorMessage = "Either Name or Keyword must be provided.")]
	public string? Name { get; set; }

	/// <summary>
	/// Gets or sets the keyword that identifies the Spin State/Electronic State/Method Family Combination.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Keyword")]
	[MaxLength(50)]
	[RequireAtLeastOne(nameof(Name), ErrorMessage = "Either Name or Keyword must be provided.")]
	public string? Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the Electronic State/Method Family Combination identifier.
	/// </summary>
	[Display(Name = "Electronic State/Method Family Combination Id")]
	[Required]
	public int ElectronicStateMethodFamilyId { get; set; }

	/// <summary>
	/// Gets or sets the Spin State identifier.
	/// </summary>
	[Display(Name = "Spin State Id")]
	public int? SpinStateId { get; set; }

	/// <summary>
	/// Gets or sets the description in Rich Text Format (RTF).
	/// </summary>
	[DataType(DataType.MultilineText)]
	[Display(Name = "Description (RTF)")]
	public string? DescriptionRtf { get; set; }

	/// <summary>
	/// Gets or sets the description in plain text format.
	/// </summary>
	[DataType(DataType.MultilineText)]
	[Display(Name = "Description (Text)")]
	[MaxLength(4000)]
	public string? DescriptionText { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the Spin State/Electronic State/Method Family Combination was created.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Created Date")]
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when the Spin State/Electronic State/Method Family Combination was last updated.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Last Updated Date")]
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether the Spin State/Electronic State/Method Family Combination is archived.
	/// </summary>
	[Display(Name = "Archived")]
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this API model to a <see cref="SpinStateElectronicStateMethodFamilyFullModel"/> instance.
	/// </summary>
	/// <param name="electronicStateMethodFamily">The Electronic State/Method Family Combination full model to associate with the Spin State/Electronic State/Method Family Combination.</param>
	/// <param name="spinState">The Spin State full model to associate with the Spin State/Electronic State/Method Family Combination.</param>
	/// <returns>A new instance of <see cref="SpinStateElectronicStateMethodFamilyFullModel"/> populated with the API model data.</returns>
	public SpinStateElectronicStateMethodFamilyFullModel ToFullModel(ElectronicStateMethodFamilyFullModel electronicStateMethodFamily, SpinStateFullModel? spinState = null)
	{
		return new SpinStateElectronicStateMethodFamilyFullModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicStateMethodFamily = electronicStateMethodFamily,
			SpinState = spinState,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this API model to a <see cref="SpinStateElectronicStateMethodFamilyIntermediateModel"/> instance.
	/// </summary>
	/// <param name="electronicStateMethodFamily">The Electronic State/Method Family Combination record to associate with the Spin State/Electronic State/Method Family Combination.</param>
	/// <param name="spinState">The Spin State record to associate with the Spin State/Electronic State/Method Family Combination.</param>
	/// <returns>A new instance of <see cref="SpinStateElectronicStateMethodFamilyIntermediateModel"/> populated with the API model data.</returns>
	public SpinStateElectronicStateMethodFamilyIntermediateModel ToIntermediateModel(ElectronicStateMethodFamilyRecord electronicStateMethodFamily, SpinStateRecord? spinState = null)
	{
		return new SpinStateElectronicStateMethodFamilyIntermediateModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicStateMethodFamily = electronicStateMethodFamily,
			SpinState = spinState,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this API model to a <see cref="SpinStateElectronicStateMethodFamilySimpleModel"/> instance.
	/// </summary>
	/// <returns>A new instance of <see cref="SpinStateElectronicStateMethodFamilySimpleModel"/> populated with the API model data.</returns>
	public SpinStateElectronicStateMethodFamilySimpleModel ToSimpleModel()
	{
		return new SpinStateElectronicStateMethodFamilySimpleModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicStateMethodFamilyId = ElectronicStateMethodFamilyId,
			SpinStateId = SpinStateId,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this API model to a <see cref="SpinStateElectronicStateMethodFamilyRecord"/> instance.
	/// </summary>
	/// <returns>A new <see cref="SpinStateElectronicStateMethodFamilyRecord"/> containing the Id, Name, and Keyword properties.</returns>
	public SpinStateElectronicStateMethodFamilyRecord ToRecord()
	{
		return new SpinStateElectronicStateMethodFamilyRecord(Id, Name, Keyword);
	}

	/// <summary>
	/// Returns a string representation of the Spin State/Electronic State/Method Family Combination API model.
	/// </summary>
	/// <returns>The Name if available, otherwise the Keyword, or a combination of both in the format "Name/Keyword".</returns>
	public override string? ToString()
	{
		return string.IsNullOrWhiteSpace(Name) ? $"{Keyword}" : string.IsNullOrWhiteSpace(Keyword) ? $"{Name}" : $"{Name}/{Keyword}";
	}
}
