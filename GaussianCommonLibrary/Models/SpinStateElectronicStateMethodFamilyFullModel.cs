namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents an Spin State/Electronic State/Method Family Combination with full details including its associated Electronic State and Method Family.
/// </summary>
public class SpinStateElectronicStateMethodFamilyFullModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyFullModel"/> class.
	/// </summary>
	public SpinStateElectronicStateMethodFamilyFullModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyFullModel"/> class from a simple model, an Electronic State, and Method Family, the last of which can be null.
	/// </summary>
	/// <param name="model">The simple model containing Spin State/Electronic State/Method Family Combination properties.</param>
	/// <param name="electronicStateMethodFamily">The Electronic State/Method Family Combination to associate with this Spin State/Electronic State/Method Family Combination.</param>
	/// <param name="spinState">The Method Family to associate with this Spin State/Electronic State/Method Family Combination.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicStateMethodFamily"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilyFullModel(SpinStateElectronicStateMethodFamilySimpleModel model, ElectronicStateMethodFamilyFullModel electronicStateMethodFamily, SpinStateFullModel? spinState = null)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(electronicStateMethodFamily, nameof(electronicStateMethodFamily));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateMethodFamily = electronicStateMethodFamily;
		SpinState = spinState;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyFullModel"/> class from an intermediate model, an Electronic State, and Method Family, the last of which can be null.
	/// </summary>
	/// <param name="model">The intermediate model containing Spin State/Electronic State/Method Family Combination properties.</param>
	/// <param name="electronicStateMethodFamily">The Electronic State/Method Family Combination to associate with this Spin State/Electronic State/Method Family Combination.</param>
	/// <param name="spinState">The Spin State to associate with this Spin State/Electronic State/Method Family Combination.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicStateMethodFamily"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilyFullModel(SpinStateElectronicStateMethodFamilyIntermediateModel model, ElectronicStateMethodFamilyFullModel electronicStateMethodFamily, SpinStateFullModel? spinState = null)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(electronicStateMethodFamily, nameof(electronicStateMethodFamily));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateMethodFamily = electronicStateMethodFamily;
		SpinState = spinState;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;
	}

	/// <summary>
	/// Gets or sets the unique identifier for the Spin State/Electronic State/Method Family Combination.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the display name of the Spin State/Electronic State/Method Family Combination.
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	/// Gets or sets the keyword used to reference this Spin State/Electronic State/Method Family Combination in Gaussian input files.
	/// </summary>
	public string? Keyword { get; set; }

	/// <summary>
	/// Gets or sets the Electronic State/Method Family Combination to which this Spin State/Electronic State/Method Family Combination belongs.
	/// </summary>
	public required ElectronicStateMethodFamilyFullModel ElectronicStateMethodFamily { get; set; }

	/// <summary>
	/// Gets or sets the Spin State to which this Spin State/Electronic State/Method Family Combination belongs.
	/// </summary>
	public SpinStateFullModel? SpinState { get; set; }

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
	/// Converts this full Spin State/Electronic State/Method Family Combination model to its simplified representation.
	/// </summary>
	/// <returns>A <see cref="SpinStateElectronicStateMethodFamilySimpleModel"/> containing the core property values.</returns>
	public SpinStateElectronicStateMethodFamilySimpleModel ToSimpleModel()
	{
		return new SpinStateElectronicStateMethodFamilySimpleModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicStateMethodFamilyId = ElectronicStateMethodFamily.Id,
			SpinStateId = SpinState?.Id,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this full Spin State/Electronic State/Method Family Combination model to its intermediate representation.
	/// </summary>
	/// <returns>A <see cref="SpinStateElectronicStateMethodFamilyIntermediateModel"/> containing the property values with the Method Family as a record.</returns>
	public SpinStateElectronicStateMethodFamilyIntermediateModel ToIntermediateModel()
	{
		return new SpinStateElectronicStateMethodFamilyIntermediateModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicStateMethodFamily = ElectronicStateMethodFamily.ToRecord(),
			SpinState = SpinState?.ToRecord(),
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this model to a <see cref="SpinStateElectronicStateMethodFamilyRecord"/>.
	/// </summary>
	/// <returns>A record containing the identifier and name of this Spin State/Electronic State/Method Family Combination.</returns>
	public SpinStateElectronicStateMethodFamilyRecord ToRecord()
	{
		return new SpinStateElectronicStateMethodFamilyRecord(Id, Name, Keyword);
	}

	/// <summary>
	/// Returns a string representation of the Spin State/Electronic State/Method Family Combination.
	/// Returns the keyword if no name is specified, the name if no keyword is specified, or "Name/Keyword" if both are specified.
	/// </summary>
	/// <returns>A formatted string representing the Spin State/Electronic State/Method Family Combination.</returns>
	public override string? ToString()
	{
		return string.IsNullOrWhiteSpace(Name) ? $"{Keyword}" : string.IsNullOrWhiteSpace(Keyword) ? $"{Name}" : $"{Name}/{Keyword}";
	}
}
