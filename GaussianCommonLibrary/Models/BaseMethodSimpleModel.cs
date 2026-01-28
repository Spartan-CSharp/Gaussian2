namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents a simplified model of a Base Method containing essential properties.
/// This model is typically used for data transfer and display purposes where the full model is not required.
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
	/// Initializes a new instance of the <see cref="BaseMethodSimpleModel"/> class from a <see cref="BaseMethodFullModel"/>.
	/// </summary>
	/// <param name="fullModel">The full model to convert from.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="fullModel"/> is null.</exception>
	public BaseMethodSimpleModel(BaseMethodFullModel fullModel)
	{
		if (fullModel is null)
		{
			throw new ArgumentNullException(nameof(fullModel), $"The {nameof(fullModel)} parameter cannot be null.");
		}

		Id = fullModel.Id;
		Keyword = fullModel.Keyword;
		MethodFamilyId = fullModel.MethodFamily?.Id ?? 0;
		DescriptionRtf = fullModel.DescriptionRtf;
		DescriptionText = fullModel.DescriptionText;
		CreatedDate = fullModel.CreatedDate;
		LastUpdatedDate = fullModel.LastUpdatedDate;
		Archived = fullModel.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodSimpleModel"/> class from an existing simple model and method family record.
	/// </summary>
	/// <param name="model">The existing simple model to copy from.</param>
	/// <param name="methodFamily">The method family record to associate with this model.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="methodFamily"/> is null.</exception>
	public BaseMethodSimpleModel(BaseMethodSimpleModel model, MethodFamilyRecord methodFamily)
	{
		if (model is null)
		{
			throw new ArgumentNullException(nameof(model), $"The parameter {nameof(model)} cannot be null.");
		}

		if (methodFamily is null)
		{
			throw new ArgumentNullException(nameof(methodFamily), $"The parameter {nameof(methodFamily)} cannot be null.");
		}

		Id = model.Id;
		Keyword = model.Keyword;
		MethodFamilyId = methodFamily.Id;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodSimpleModel"/> class from a full model and method family full model.
	/// </summary>
	/// <param name="model">The full model to convert from.</param>
	/// <param name="methodFamily">The method family full model to associate with this model.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null.</exception>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="methodFamily"/> is null.</exception>
	public BaseMethodSimpleModel(BaseMethodFullModel model, MethodFamilyFullModel methodFamily)
	{
		if (model is null)
		{
			throw new ArgumentNullException(nameof(model), $"The parameter {nameof(model)} cannot be null.");
		}

		if (methodFamily is null)
		{
			throw new ArgumentNullException(nameof(methodFamily), $"The parameter {nameof(methodFamily)} cannot be null.");
		}

		Id = model.Id;
		Keyword = model.Keyword;
		MethodFamilyId = methodFamily.Id;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Gets or sets the unique identifier for the Base Method.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the keyword identifier used for the Base Method.
	/// </summary>
	public string Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the identifier of the Method Family to which this Base Method belongs.
	/// </summary>
	public int MethodFamilyId { get; set; }

	/// <summary>
	/// Gets or sets the Rich Text Format (RTF) description of the Base Method.
	/// </summary>
	public string? DescriptionRtf { get; set; }

	/// <summary>
	/// Gets or sets the plain text description of the Base Method.
	/// </summary>
	public string? DescriptionText { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the Base Method was created.
	/// Defaults to the current date and time.
	/// </summary>
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when the Base Method was last updated.
	/// Defaults to the current date and time.
	/// </summary>
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether the Base Method is archived.
	/// </summary>
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this simple model to a <see cref="BaseMethodFullModel"/> using the specified method family.
	/// </summary>
	/// <param name="methodFamily">The method family full model to associate with the resulting full model.</param>
	/// <returns>A new <see cref="BaseMethodFullModel"/> instance with the properties copied from this model.</returns>
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
	/// Returns a string representation of the Base Method.
	/// </summary>
	/// <returns>A string in the format "Keyword".</returns>
	public override string? ToString()
	{
		return $"{Keyword}";
	}
}
