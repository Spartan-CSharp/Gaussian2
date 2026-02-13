namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents a Base Method with only the Method Family identifier.
/// </summary>
public class BaseMethodSimpleModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodSimpleModel"/> class.
	/// </summary>
	public BaseMethodSimpleModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodSimpleModel"/> class from an intermediate model.
	/// </summary>
	/// <param name="intermediateModel">The intermediate model to convert from.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="intermediateModel"/> is null.</exception>
	public BaseMethodSimpleModel(BaseMethodIntermediateModel intermediateModel)
	{
		ArgumentNullException.ThrowIfNull(intermediateModel, nameof(intermediateModel));
		Id = intermediateModel.Id;
		Keyword = intermediateModel.Keyword;
		MethodFamilyId = intermediateModel.MethodFamily?.Id;
		DescriptionRtf = intermediateModel.DescriptionRtf;
		DescriptionText = intermediateModel.DescriptionText;
		CreatedDate = intermediateModel.CreatedDate;
		LastUpdatedDate = intermediateModel.LastUpdatedDate;
		Archived = intermediateModel.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodSimpleModel"/> class from a full model.
	/// </summary>
	/// <param name="fullModel">The full model to convert from.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="fullModel"/> is null.</exception>
	public BaseMethodSimpleModel(BaseMethodFullModel fullModel)
	{
		ArgumentNullException.ThrowIfNull(fullModel, nameof(fullModel));
		Id = fullModel.Id;
		Keyword = fullModel.Keyword;
		MethodFamilyId = fullModel.MethodFamily?.Id;
		DescriptionRtf = fullModel.DescriptionRtf;
		DescriptionText = fullModel.DescriptionText;
		CreatedDate = fullModel.CreatedDate;
		LastUpdatedDate = fullModel.LastUpdatedDate;
		Archived = fullModel.Archived;
	}

	/// <summary>
	/// Gets or sets the unique identifier for the Base Method.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the keyword used to reference this Base Method in Gaussian input files.
	/// </summary>
	public string Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the identifier of the Method Family to which this Base Method belongs.
	/// </summary>
	public int? MethodFamilyId { get; set; }

	/// <summary>
	/// Gets or sets the description in Rich Text Format.
	/// </summary>
	public string? DescriptionRtf { get; set; }

	/// <summary>
	/// Gets or sets the plain text description.
	/// </summary>
	public string? DescriptionText { get; set; }

	/// <summary>
	/// Gets or sets the date and time when this record was created.
	/// </summary>
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when this record was last updated.
	/// </summary>
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether this Base Method is archived.
	/// </summary>
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this model to a <see cref="BaseMethodIntermediateModel"/>.
	/// </summary>
	/// <param name="methodFamily">The Method Family record to include in the intermediate model.</param>
	/// <returns>An intermediate model representation of this Base Method.</returns>
	public BaseMethodIntermediateModel ToIntermediateModel(MethodFamilyRecord methodFamily)
	{
		ArgumentNullException.ThrowIfNull(methodFamily, nameof(methodFamily));

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
	/// Converts this model to a <see cref="BaseMethodFullModel"/> using the specified Method Family.
	/// </summary>
	/// <param name="methodFamily">The Method Family full model to associate with the resulting full model.</param>
	/// <returns>A new <see cref="BaseMethodFullModel"/> instance with the properties copied from this model.</returns>
	public BaseMethodFullModel ToFullModel(MethodFamilyFullModel methodFamily)
	{
		ArgumentNullException.ThrowIfNull(methodFamily, nameof(methodFamily));

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
	/// Converts this model to a <see cref="BaseMethodRecord"/>.
	/// </summary>
	/// <returns>A record containing the identifier and name of this Base Method.</returns>
	public BaseMethodRecord ToRecord()
	{
		return new BaseMethodRecord(Id, Keyword);
	}

	/// <summary>
	/// Returns a string representation of the Base Method.
	/// </summary>
	/// <returns>A string in the format "Keyword".</returns>
	public override string? ToString()
	{
		return $"{Keyword}";
	}
}
