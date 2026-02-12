using System.ComponentModel.DataAnnotations;

using GaussianCommonLibrary.Models;

using GaussianMVC.ValidationAttributes;

namespace GaussianMVC.Models.APIModels.V1;

/// <summary>
/// API model representing a Electronic State/Method Family Combination for REST API operations.
/// Provides data transfer and validation for Electronic State/Method Family Combination entities in API v1.
/// </summary>
public class ElectronicStateMethodFamilyAPIModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyAPIModel"/> class.
	/// </summary>
	public ElectronicStateMethodFamilyAPIModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyAPIModel"/> class from a full model.
	/// </summary>
	/// <param name="model">The Electronic State/Method Family Combination full model containing the source data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public ElectronicStateMethodFamilyAPIModel(ElectronicStateMethodFamilyFullModel model)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateId = model.ElectronicState.Id;
		MethodFamilyId = model.MethodFamily?.Id;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyAPIModel"/> class from an intermediate model.
	/// </summary>
	/// <param name="model">The Electronic State/Method Family Combination intermediate model containing the source data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public ElectronicStateMethodFamilyAPIModel(ElectronicStateMethodFamilyIntermediateModel model)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateId = model.ElectronicState.Id;
		MethodFamilyId = model.MethodFamily?.Id;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyAPIModel"/> class from a simple model.
	/// </summary>
	/// <param name="model">The Electronic State/Method Family Combination simple model containing the source data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public ElectronicStateMethodFamilyAPIModel(ElectronicStateMethodFamilySimpleModel model)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateId = model.ElectronicStateId;
		MethodFamilyId = model.MethodFamilyId;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Gets or sets the unique identifier for the Electronic State/Method Family Combination.
	/// </summary>
	[Display(Name = "Id")]
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the name of the Electronic State/Method Family Combination.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Name")]
	[MaxLength(200)]
	[RequireAtLeastOne(nameof(Keyword), ErrorMessage = "Either Name or Keyword must be provided.")]
	public string? Name { get; set; }

	/// <summary>
	/// Gets or sets the keyword that identifies the Electronic State/Method Family Combination.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Keyword")]
	[MaxLength(50)]
	[RequireAtLeastOne(nameof(Name), ErrorMessage = "Either Name or Keyword must be provided.")]
	public string? Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the Electronic State identifier.
	/// </summary>
	[Display(Name = "Electronic State Id")]
	[Required]
	public int ElectronicStateId { get; set; }

	/// <summary>
	/// Gets or sets the Method Family identifier.
	/// </summary>
	[Display(Name = "Method Family Id")]
	public int? MethodFamilyId { get; set; }

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
	/// Gets or sets the date and time when the Electronic State/Method Family Combination was created.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Created Date")]
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when the Electronic State/Method Family Combination was last updated.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Last Updated Date")]
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether the Electronic State/Method Family Combination is archived.
	/// </summary>
	[Display(Name = "Archived")]
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this API model to a <see cref="ElectronicStateMethodFamilyFullModel"/> instance.
	/// </summary>
	/// <param name="electronicState">The Electronic State full model to associate with the Electronic State/Method Family Combination.</param>
	/// <param name="methodFamily">The Method Family full model to associate with the Electronic State/Method Family Combination.</param>
	/// <returns>A new instance of <see cref="ElectronicStateMethodFamilyFullModel"/> populated with the API model data.</returns>
	public ElectronicStateMethodFamilyFullModel ToFullModel(ElectronicStateFullModel electronicState, MethodFamilyFullModel? methodFamily = null)
	{
		return new ElectronicStateMethodFamilyFullModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicState = electronicState,
			MethodFamily = methodFamily,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this API model to a <see cref="ElectronicStateMethodFamilyIntermediateModel"/> instance.
	/// </summary>
	/// <param name="electronicState">The Electronic State record to associate with the Electronic State/Method Family Combination.</param>
	/// <param name="methodFamily">The Method Family record to associate with the Electronic State/Method Family Combination.</param>
	/// <returns>A new instance of <see cref="ElectronicStateMethodFamilyIntermediateModel"/> populated with the API model data.</returns>
	public ElectronicStateMethodFamilyIntermediateModel ToIntermediateModel(ElectronicStateRecord electronicState, MethodFamilyRecord? methodFamily = null)
	{
		return new ElectronicStateMethodFamilyIntermediateModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicState = electronicState,
			MethodFamily = methodFamily,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this API model to a <see cref="ElectronicStateMethodFamilySimpleModel"/> instance.
	/// </summary>
	/// <returns>A new instance of <see cref="ElectronicStateMethodFamilySimpleModel"/> populated with the API model data.</returns>
	public ElectronicStateMethodFamilySimpleModel ToSimpleModel()
	{
		return new ElectronicStateMethodFamilySimpleModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicStateId = ElectronicStateId,
			MethodFamilyId = MethodFamilyId,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this API model to a <see cref="ElectronicStateMethodFamilyRecord"/> instance.
	/// </summary>
	/// <returns>A new <see cref="ElectronicStateMethodFamilyRecord"/> containing the Id, Name, and Keyword properties.</returns>
	public ElectronicStateMethodFamilyRecord ToRecord()
	{
		return new ElectronicStateMethodFamilyRecord(Id, Name, Keyword);
	}

	/// <summary>
	/// Returns a string representation of the Electronic State/Method Family Combination API model.
	/// </summary>
	/// <returns>The Name if available, otherwise the Keyword, or a combination of both in the format "Name/Keyword".</returns>
	public override string? ToString()
	{
		return string.IsNullOrWhiteSpace(Name) ? $"{Keyword}" : string.IsNullOrWhiteSpace(Keyword) ? $"{Name}" : $"{Name}/{Keyword}";
	}
}
