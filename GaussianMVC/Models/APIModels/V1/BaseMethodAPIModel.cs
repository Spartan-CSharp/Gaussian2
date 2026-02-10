using System.ComponentModel.DataAnnotations;

using GaussianCommonLibrary.Models;

namespace GaussianMVC.Models.APIModels.V1;

/// <summary>
/// API model representing a Base Method for REST API operations.
/// Provides data transfer and validation for Base Method entities in API v1.
/// </summary>
public class BaseMethodAPIModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodAPIModel"/> class.
	/// </summary>
	public BaseMethodAPIModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodAPIModel"/> class from a full model.
	/// </summary>
	/// <param name="model">The Base Method full model containing the source data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public BaseMethodAPIModel(BaseMethodFullModel model)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		Id = model.Id;
		Keyword = model.Keyword;
		MethodFamilyId = model.MethodFamily.Id;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodAPIModel"/> class from an intermediate model.
	/// </summary>
	/// <param name="model">The Base Method intermediate model containing the source data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public BaseMethodAPIModel(BaseMethodIntermediateModel model)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		Id = model.Id;
		Keyword = model.Keyword;
		MethodFamilyId = model.MethodFamily.Id;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodAPIModel"/> class from a simple model.
	/// </summary>
	/// <param name="model">The Base Method simple model containing the source data.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	public BaseMethodAPIModel(BaseMethodSimpleModel model)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		Id = model.Id;
		Keyword = model.Keyword;
		MethodFamilyId = model.MethodFamilyId;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Gets or sets the unique identifier for the Base Method.
	/// </summary>
	[Display(Name = "Id")]
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the keyword that identifies the Base Method.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Keyword")]
	[MaxLength(50)]
	[Required]
	public string Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the Method Family identifier.
	/// </summary>
	[Display(Name = "Method Family Id")]
	[Required]
	public int MethodFamilyId { get; set; }

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
	/// Gets or sets the date and time when the Base Method was created.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Created Date")]
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when the Base Method was last updated.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Last Updated Date")]
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether the Base Method is archived.
	/// </summary>
	[Display(Name = "Archived")]
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this API model to a <see cref="BaseMethodFullModel"/> instance.
	/// </summary>
	/// <param name="methodFamily">The Method Family full model to associate with the Base Method.</param>
	/// <returns>A new instance of <see cref="BaseMethodFullModel"/> populated with the API model data.</returns>
	public BaseMethodFullModel ToFullModel(MethodFamilyFullModel methodFamily)
	{
		return new BaseMethodFullModel
		{
			Id = Id,
			Keyword = Keyword,
			MethodFamily = methodFamily,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this API model to a <see cref="BaseMethodIntermediateModel"/> instance.
	/// </summary>
	/// <param name="methodFamily">The Method Family record to associate with the Base Method.</param>
	/// <returns>A new instance of <see cref="BaseMethodIntermediateModel"/> populated with the API model data.</returns>
	public BaseMethodIntermediateModel ToIntermediateModel(MethodFamilyRecord methodFamily)
	{
		return new BaseMethodIntermediateModel
		{
			Id = Id,
			Keyword = Keyword,
			MethodFamily = methodFamily,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this API model to a <see cref="BaseMethodSimpleModel"/> instance.
	/// </summary>
	/// <returns>A new instance of <see cref="BaseMethodSimpleModel"/> populated with the API model data.</returns>
	public BaseMethodSimpleModel ToSimpleModel()
	{
		return new BaseMethodSimpleModel
		{
			Id = Id,
			Keyword = Keyword,
			MethodFamilyId = MethodFamilyId,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Returns a string representation of the Base Method API model.
	/// </summary>
	/// <returns>The keyword of the Base Method.</returns>
	public override string? ToString()
	{
		return $"{Keyword}";
	}
}
