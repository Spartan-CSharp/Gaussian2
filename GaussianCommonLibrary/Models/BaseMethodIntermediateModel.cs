namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents a base method with method family information as a record rather than a full model.
/// </summary>
public class BaseMethodIntermediateModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodIntermediateModel"/> class.
	/// </summary>
	public BaseMethodIntermediateModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodIntermediateModel"/> class from a simple model and method family record.
	/// </summary>
	/// <param name="model">The simple model containing base method properties.</param>
	/// <param name="methodFamily">The method family record to associate with this base method.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="methodFamily"/> is null.</exception>
	public BaseMethodIntermediateModel(BaseMethodSimpleModel model, MethodFamilyRecord methodFamily)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(methodFamily, nameof(methodFamily));
		Id = model.Id;
		Keyword = model.Keyword;
		MethodFamily = methodFamily;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodIntermediateModel"/> class from a full model.
	/// </summary>
	/// <param name="fullModel">The full model to convert from.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="fullModel"/> is null.</exception>
	public BaseMethodIntermediateModel(BaseMethodFullModel fullModel)
	{
		ArgumentNullException.ThrowIfNull(fullModel, nameof(fullModel));
		Id = fullModel.Id;
		Keyword = fullModel.Keyword;
		MethodFamily = fullModel.MethodFamily.ToRecord();
		DescriptionRtf = fullModel.DescriptionRtf;
		DescriptionText = fullModel.DescriptionText;
		CreatedDate = fullModel.CreatedDate;
		LastUpdatedDate = fullModel.LastUpdatedDate;
		Archived = fullModel.Archived;
	}

	/// <summary>
	/// Gets or sets the unique identifier for the base method.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the keyword used to reference this base method in Gaussian input files.
	/// </summary>
	public string Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the method family record to which this base method belongs.
	/// </summary>
	public required MethodFamilyRecord MethodFamily { get; set; }

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
	/// Gets or sets a value indicating whether this base method is archived.
	/// </summary>
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this model to a <see cref="BaseMethodSimpleModel"/>.
	/// </summary>
	/// <returns>A simple model representation of this base method.</returns>
	public BaseMethodSimpleModel ToSimpleModel()
	{
		return new BaseMethodSimpleModel
		{
			Id = Id,
			Keyword = Keyword,
			MethodFamilyId = MethodFamily.Id,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this model to a <see cref="BaseMethodFullModel"/> using the specified method family.
	/// </summary>
	/// <param name="methodFamily">The method family full model to associate with the resulting full model.</param>
	/// <returns>A full model representation of this base method.</returns>
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
	/// Returns a string representation of the base method.
	/// </summary>
	/// <returns>A string in the format "Keyword".</returns>
	public override string? ToString()
	{
		return $"{Keyword}";
	}
}
