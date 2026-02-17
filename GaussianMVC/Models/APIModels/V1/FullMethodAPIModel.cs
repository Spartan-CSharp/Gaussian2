using System.ComponentModel.DataAnnotations;

using GaussianCommonLibrary.Models;

namespace GaussianMVC.Models.APIModels.V1;

/// <summary>
/// API model representing a Full Method for REST API operations.
/// Provides data transfer and validation for Full Method entities in API v1.
/// </summary>
public class FullMethodAPIModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodAPIModel"/> class.
	/// </summary>
	public FullMethodAPIModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodAPIModel"/> class from a full model.
	/// </summary>
	/// <param name="model">The Full Method full model containing the source data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public FullMethodAPIModel(FullMethodFullModel model)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		Id = model.Id;
		Keyword = model.Keyword;
		SpinStateElectronicStateMethodFamilyId = model.SpinStateElectronicStateMethodFamily.Id;
		BaseMethodId = model.BaseMethod.Id;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodAPIModel"/> class from an intermediate model.
	/// </summary>
	/// <param name="model">The Full Method intermediate model containing the source data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public FullMethodAPIModel(FullMethodIntermediateModel model)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		Id = model.Id;
		Keyword = model.Keyword;
		SpinStateElectronicStateMethodFamilyId = model.SpinStateElectronicStateMethodFamily.Id;
		BaseMethodId = model.BaseMethod.Id;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodAPIModel"/> class from a simple model.
	/// </summary>
	/// <param name="model">The Full Method simple model containing the source data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public FullMethodAPIModel(FullMethodSimpleModel model)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		Id = model.Id;
		Keyword = model.Keyword;
		SpinStateElectronicStateMethodFamilyId = model.SpinStateElectronicStateMethodFamilyId;
		BaseMethodId = model.BaseMethodId;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Gets or sets the unique identifier for the Full Method.
	/// </summary>
	[Display(Name = "Id")]
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the keyword that identifies the Full Method.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Keyword")]
	[MaxLength(50)]
	[Required]
	public string Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the Spin State/Electronic State/Method Family Combination identifier.
	/// </summary>
	[Display(Name = "Spin State/Electronic State/Method Family Combination Id")]
	[Required]
	public int SpinStateElectronicStateMethodFamilyId { get; set; }

	/// <summary>
	/// Gets or sets the Base Method identifier.
	/// </summary>
	[Display(Name = "Base Method Id")]
	[Required]
	public int BaseMethodId { get; set; }

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
	/// Gets or sets the date and time when the Full Method was created.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Created Date")]
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when the Full Method was last updated.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Last Updated Date")]
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether the Full Method is archived.
	/// </summary>
	[Display(Name = "Archived")]
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this API model to a <see cref="FullMethodFullModel"/> instance.
	/// </summary>
	/// <param name="spinStateElectronicStateMethodFamily">The Spin State/Electronic State/Method Family Combination full model to associate with the Full Method.</param>
	/// <param name="baseMethod">The Base Method full model to associate with the Full Method.</param>
	/// <returns>A new instance of <see cref="FullMethodFullModel"/> populated with the API model data.</returns>
	public FullMethodFullModel ToFullModel(SpinStateElectronicStateMethodFamilyFullModel spinStateElectronicStateMethodFamily, BaseMethodFullModel baseMethod)
	{
		return new FullMethodFullModel
		{
			Id = Id,
			Keyword = Keyword,
			SpinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamily,
			BaseMethod = baseMethod,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this API model to a <see cref="FullMethodIntermediateModel"/> instance.
	/// </summary>
	/// <param name="spinStateElectronicStateMethodFamily">The Spin State/Electronic State/Method Family Combination record to associate with the Full Method.</param>
	/// <param name="baseMethod">The Base MEthod record to associate with the Full Method.</param>
	/// <returns>A new instance of <see cref="FullMethodIntermediateModel"/> populated with the API model data.</returns>
	public FullMethodIntermediateModel ToIntermediateModel(SpinStateElectronicStateMethodFamilyRecord spinStateElectronicStateMethodFamily, BaseMethodRecord baseMethod)
	{
		return new FullMethodIntermediateModel
		{
			Id = Id,
			Keyword = Keyword,
			SpinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamily,
			BaseMethod = baseMethod,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this API model to a <see cref="FullMethodSimpleModel"/> instance.
	/// </summary>
	/// <returns>A new instance of <see cref="FullMethodSimpleModel"/> populated with the API model data.</returns>
	public FullMethodSimpleModel ToSimpleModel()
	{
		return new FullMethodSimpleModel
		{
			Id = Id,
			Keyword = Keyword,
			SpinStateElectronicStateMethodFamilyId = SpinStateElectronicStateMethodFamilyId,
			BaseMethodId = BaseMethodId,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this model to a <see cref="FullMethodRecord"/>.
	/// </summary>
	/// <returns>A record containing the identifier and name of this Full Method.</returns>
	public FullMethodRecord ToRecord()
	{
		return new FullMethodRecord(Id, Keyword);
	}

	/// <summary>
	/// Returns a string representation of the Full Method API model.
	/// </summary>
	/// <returns>The keyword of the Full Method.</returns>
	public override string? ToString()
	{
		return $"{Keyword}";
	}
}
