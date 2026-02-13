using System.Xml.Linq;

namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents a Base Method with full details including its associated Method Family.
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
	/// Initializes a new instance of the <see cref="BaseMethodFullModel"/> class from a simple model and Method Family.
	/// </summary>
	/// <param name="model">The simple model containing Base Method properties.</param>
	/// <param name="methodFamily">The Method Family to associate with this Base Method.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="methodFamily"/> is null.</exception>
	public BaseMethodFullModel(BaseMethodSimpleModel model, MethodFamilyFullModel methodFamily)
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
	/// Initializes a new instance of the <see cref="BaseMethodFullModel"/> class from an intermediate model and Method Family.
	/// </summary>
	/// <param name="model">The intermediate model containing Base Method properties.</param>
	/// <param name="methodFamily">The Method Family to associate with this Base Method.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="methodFamily"/> is null.</exception>
	public BaseMethodFullModel(BaseMethodIntermediateModel model, MethodFamilyFullModel methodFamily)
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
	/// Gets or sets the unique identifier for the Base Method.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the keyword used to reference this Base Method in Gaussian input files.
	/// </summary>
	public string Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the Method Family to which this Base Method belongs.
	/// </summary>
	public MethodFamilyFullModel? MethodFamily { get; set; }

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
	/// Converts this full Base Method model to its simplified representation.
	/// </summary>
	/// <returns>A <see cref="BaseMethodSimpleModel"/> containing the core property values.</returns>
	public BaseMethodSimpleModel ToSimpleModel()
	{
		return new BaseMethodSimpleModel
		{
			Id = Id,
			Keyword = Keyword,
			MethodFamilyId = MethodFamily?.Id,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this full Base Method model to its intermediate representation.
	/// </summary>
	/// <returns>A <see cref="BaseMethodIntermediateModel"/> containing the property values with the Method Family as a record.</returns>
	public BaseMethodIntermediateModel ToIntermediateModel()
	{
		return new BaseMethodIntermediateModel
		{
			Id = Id,
			Keyword = Keyword,
			MethodFamily = MethodFamily?.ToRecord(),
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
