namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents an Electronic State/Method Family Combination with full details including its associated Electronic State and Method Family.
/// </summary>
public class ElectronicStateMethodFamilyFullModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyFullModel"/> class.
	/// </summary>
	public ElectronicStateMethodFamilyFullModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyFullModel"/> class from a simple model, an Electronic State, and Method Family, the last of which can be null.
	/// </summary>
	/// <param name="model">The simple model containing Electronic State/Method Family Combination properties.</param>
	/// <param name="electronicState">The Electronic State to associate with this Electronic State/Method Family Combination.</param>
	/// <param name="methodFamily">The Method Family to associate with this Electronic State/Method Family Combination.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicState"/> is null.</exception>
	public ElectronicStateMethodFamilyFullModel(ElectronicStateMethodFamilySimpleModel model, ElectronicStateFullModel electronicState, MethodFamilyFullModel? methodFamily = null)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(electronicState, nameof(electronicState));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicState = electronicState;
		MethodFamily = methodFamily;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyFullModel"/> class from an intermediate model, an Electronic State, and Method Family, the last of which can be null.
	/// </summary>
	/// <param name="model">The intermediate model containing Electronic State/Method Family Combination properties.</param>
	/// <param name="electronicState">The Electronic State to associate with this Electronic State/Method Family Combination.</param>
	/// <param name="methodFamily">The Method Family to associate with this Electronic State/Method Family Combination.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicState"/> is null.</exception>
	public ElectronicStateMethodFamilyFullModel(ElectronicStateMethodFamilyIntermediateModel model, ElectronicStateFullModel electronicState, MethodFamilyFullModel? methodFamily = null)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(electronicState, nameof(electronicState));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicState = electronicState;
		MethodFamily = methodFamily;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Gets or sets the unique identifier for the Electronic State/Method Family Combination.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the display name of the Electronic State/Method Family Combination.
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	/// Gets or sets the keyword used to reference this Electronic State/Method Family Combination in Gaussian input files.
	/// </summary>
	public string? Keyword { get; set; }

	/// <summary>
	/// Gets or sets the Electronic State to which this Electronic State/Method Family Combination belongs.
	/// </summary>
	public required ElectronicStateFullModel ElectronicState { get; set; }

	/// <summary>
	/// Gets or sets the Method Family to which this Electronic State/Method Family Combination belongs.
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
	public DateTime CreatedDate { get; set; }

	/// <summary>
	/// Gets or sets the date and time when this record was last updated.
	/// </summary>
	public DateTime LastUpdatedDate { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether this Electronic State is archived.
	/// </summary>
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this full Electronic State/Method Family Combination model to its simplified representation.
	/// </summary>
	/// <returns>A <see cref="ElectronicStateMethodFamilySimpleModel"/> containing the core property values.</returns>
	public ElectronicStateMethodFamilySimpleModel ToSimpleModel()
	{
		return new ElectronicStateMethodFamilySimpleModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicStateId = ElectronicState.Id,
			MethodFamilyId = MethodFamily?.Id,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this full Electronic State/Method Family Combination model to its intermediate representation.
	/// </summary>
	/// <returns>A <see cref="ElectronicStateMethodFamilyIntermediateModel"/> containing the property values with the Method Family as a record.</returns>
	public ElectronicStateMethodFamilyIntermediateModel ToIntermediateModel()
	{
		return new ElectronicStateMethodFamilyIntermediateModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicState = ElectronicState.ToRecord(),
			MethodFamily = MethodFamily?.ToRecord(),
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Returns a string representation of the Electronic State/Method Family Combination.
	/// Returns the keyword if no name is specified, the name if no keyword is specified, or "Name/Keyword" if both are specified.
	/// </summary>
	/// <returns>A formatted string representing the Electronic State/Method Family Combination.</returns>
	public override string? ToString()
	{
		return string.IsNullOrWhiteSpace(Name) ? $"{Keyword}" : string.IsNullOrWhiteSpace(Keyword) ? $"{Name}" : $"{Name}/{Keyword}";
	}
}
