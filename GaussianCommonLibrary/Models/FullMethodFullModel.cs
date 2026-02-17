namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents a Full Method with full details including its associated Method Family.
/// </summary>
public class FullMethodFullModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodFullModel"/> class.
	/// </summary>
	public FullMethodFullModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodFullModel"/> class from a simple model and Method Family.
	/// </summary>
	/// <param name="model">The simple model containing Full Method properties.</param>
	/// <param name="spinStateElectronicStateMethodFamily">The Spin State/Electronic State/Method Family to associate with this Full Method.</param>
	/// <param name="baseMethod">The Base Method to associate with this Full Method.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="spinStateElectronicStateMethodFamily"/> or <paramref name="baseMethod"/> is null.</exception>
	public FullMethodFullModel(FullMethodSimpleModel model, SpinStateElectronicStateMethodFamilyFullModel spinStateElectronicStateMethodFamily, BaseMethodFullModel baseMethod)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(spinStateElectronicStateMethodFamily, nameof(spinStateElectronicStateMethodFamily));
		ArgumentNullException.ThrowIfNull(baseMethod, nameof(baseMethod));
		Id = model.Id;
		Keyword = model.Keyword;
		SpinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamily;
		BaseMethod = baseMethod;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodFullModel"/> class from an intermediate model and Method Family.
	/// </summary>
	/// <param name="model">The intermediate model containing Full Method properties.</param>
	/// <param name="spinStateElectronicStateMethodFamily">The Spin State/Electronic State/Method Family to associate with this Full Method.</param>
	/// <param name="baseMethod">The Base Method to associate with this Full Method.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="spinStateElectronicStateMethodFamily"/> or <paramref name="baseMethod"/> is null.</exception>
	public FullMethodFullModel(FullMethodIntermediateModel model, SpinStateElectronicStateMethodFamilyFullModel spinStateElectronicStateMethodFamily, BaseMethodFullModel baseMethod)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(spinStateElectronicStateMethodFamily, nameof(spinStateElectronicStateMethodFamily));
		ArgumentNullException.ThrowIfNull(baseMethod, nameof(baseMethod));
		Id = model.Id;
		Keyword = model.Keyword;
		SpinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamily;
		BaseMethod = baseMethod;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Gets or sets the unique identifier for the Full Method.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the keyword used to reference this Full Method in Gaussian input files.
	/// </summary>
	public string Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the Spin State/Electronic State/Method Family Combination to which this Full Method belongs.
	/// </summary>
	public required SpinStateElectronicStateMethodFamilyFullModel SpinStateElectronicStateMethodFamily { get; set; }

	/// <summary>
	/// Gets or sets the Base Method to which this Full Method belongs.
	/// </summary>
	public required BaseMethodFullModel BaseMethod { get; set; }

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
	/// Gets or sets a value indicating whether this Full Method is archived.
	/// </summary>
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this full Full Method model to its simplified representation.
	/// </summary>
	/// <returns>A <see cref="FullMethodSimpleModel"/> containing the core property values.</returns>
	public FullMethodSimpleModel ToSimpleModel()
	{
		return new FullMethodSimpleModel
		{
			Id = Id,
			Keyword = Keyword,
			SpinStateElectronicStateMethodFamilyId = SpinStateElectronicStateMethodFamily.Id,
			BaseMethodId = BaseMethod.Id,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this full Full Method model to its intermediate representation.
	/// </summary>
	/// <returns>A <see cref="FullMethodIntermediateModel"/> containing the property values with the Method Family as a record.</returns>
	public FullMethodIntermediateModel ToIntermediateModel()
	{
		return new FullMethodIntermediateModel
		{
			Id = Id,
			Keyword = Keyword,
			SpinStateElectronicStateMethodFamily = SpinStateElectronicStateMethodFamily.ToRecord(),
			BaseMethod = BaseMethod.ToRecord(),
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
	/// Returns a string representation of the Full Method.
	/// </summary>
	/// <returns>A string in the format "Keyword".</returns>
	public override string? ToString()
	{
		return $"{Keyword}";
	}
}
