namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents a complete Base Method model with full details including method family, descriptions, and metadata.
/// </summary>
public class BaseMethodFullModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodFullModel"/> class.
	/// </summary>
	public BaseMethodFullModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodFullModel"/> class using a simple model and method family.
	/// </summary>
	/// <param name="model">The simple Base Method model containing core property values.</param>
	/// <param name="methodFamily">The associated Method Family for this Base Method.</param>
	/// <exception cref="ArgumentNullException">
	/// Thrown when <paramref name="model"/> or <paramref name="methodFamily"/> is null.
	/// </exception>
	public BaseMethodFullModel(BaseMethodSimpleModel model, MethodFamilyFullModel methodFamily)
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
		MethodFamily = methodFamily;
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
	/// Gets or sets the associated Method Family for this Base Method.
	/// </summary>
	public required MethodFamilyFullModel MethodFamily { get; set; }

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
	/// Converts this full Base Method model to its simplified representation.
	/// </summary>
	/// <returns>A <see cref="BaseMethodSimpleModel"/> containing the core property values.</returns>
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
	/// Returns a string representation of the Base Method.
	/// </summary>
	/// <returns>A string in the format "Keyword".</returns>
	public override string? ToString()
	{
		return $"{Keyword}";
	}
}
