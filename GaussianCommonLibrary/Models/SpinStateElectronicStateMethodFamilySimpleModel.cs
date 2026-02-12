namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents an Spin State/Electronic State/Method Family Combination with only the Electronic State and Method Family identifiers.
/// </summary>
public class SpinStateElectronicStateMethodFamilySimpleModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilySimpleModel"/> class.
	/// </summary>
	public SpinStateElectronicStateMethodFamilySimpleModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilySimpleModel"/> class from an intermediate model.
	/// </summary>
	/// <param name="intermediateModel">The intermediate model to convert from.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="intermediateModel"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilySimpleModel(SpinStateElectronicStateMethodFamilyIntermediateModel intermediateModel)
	{
		ArgumentNullException.ThrowIfNull(intermediateModel, nameof(intermediateModel));
		Id = intermediateModel.Id;
		Name = intermediateModel.Name;
		Keyword = intermediateModel.Keyword;
		ElectronicStateMethodFamilyId = intermediateModel.ElectronicStateMethodFamily.Id;
		SpinStateId = intermediateModel.SpinState?.Id;
		DescriptionRtf = intermediateModel.DescriptionRtf;
		DescriptionText = intermediateModel.DescriptionText;
		CreatedDate = intermediateModel.CreatedDate;
		LastUpdatedDate = intermediateModel.LastUpdatedDate;
		Archived = intermediateModel.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilySimpleModel"/> class from a full model.
	/// </summary>
	/// <param name="fullModel">The full model to convert from.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="fullModel"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilySimpleModel(SpinStateElectronicStateMethodFamilyFullModel fullModel)
	{
		ArgumentNullException.ThrowIfNull(fullModel, nameof(fullModel));
		Id = fullModel.Id;
		Name = fullModel.Name;
		Keyword = fullModel.Keyword;
		ElectronicStateMethodFamilyId = fullModel.ElectronicStateMethodFamily.Id;
		SpinStateId = fullModel.SpinState?.Id;
		DescriptionRtf = fullModel.DescriptionRtf;
		DescriptionText = fullModel.DescriptionText;
		CreatedDate = fullModel.CreatedDate;
		LastUpdatedDate = fullModel.LastUpdatedDate;
		Archived = fullModel.Archived;
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
	/// Gets or sets the identifier of the Electronic State/Method Family Combination to which this Spin State/Electronic State/Method Family Combination belongs.
	/// </summary>
	public int ElectronicStateMethodFamilyId { get; set; }

	/// <summary>
	/// Gets or sets the identifier of the Spin State to which this Spin State/Electronic State/Method Family Combination belongs.
	/// </summary>
	public int? SpinStateId { get; set; }

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
	/// Converts this model to a <see cref="SpinStateElectronicStateMethodFamilyIntermediateModel"/>.
	/// </summary>
	/// <param name="electronicStateMethodFamily">The Electronic State/Method Family Combination record to include in the intermediate model.</param>
	/// <param name="spinState">The Spin State record to include in the intermediate model.</param>
	/// <returns>An intermediate model representation of this Spin State/Electronic State/Method Family Combination.</returns>
	public SpinStateElectronicStateMethodFamilyIntermediateModel ToIntermediateModel(ElectronicStateMethodFamilyRecord electronicStateMethodFamily, SpinStateRecord? spinState = null)
	{
		ArgumentNullException.ThrowIfNull(electronicStateMethodFamily, nameof(electronicStateMethodFamily));

		return new SpinStateElectronicStateMethodFamilyIntermediateModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicStateMethodFamily = electronicStateMethodFamily,
			SpinState = spinState,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this model to a <see cref="SpinStateElectronicStateMethodFamilyFullModel"/> using the specified Electronic State/Method Family Combination and Spin State, the second of which may be null.
	/// </summary>
	/// <param name="electronicStateMethodFamily">The Electronic State/Method Family Combination full model to associate with the resulting full model.</param>
	/// <param name="spinState">The Spin State full model to associate with the resulting full model.</param>
	/// <returns>A new <see cref="SpinStateElectronicStateMethodFamilyFullModel"/> instance with the properties copied from this model.</returns>
	public SpinStateElectronicStateMethodFamilyFullModel ToFullModel(ElectronicStateMethodFamilyFullModel electronicStateMethodFamily, SpinStateFullModel? spinState = null)
	{
		ArgumentNullException.ThrowIfNull(electronicStateMethodFamily, nameof(electronicStateMethodFamily));

		return new SpinStateElectronicStateMethodFamilyFullModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicStateMethodFamily = electronicStateMethodFamily,
			SpinState = spinState,
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
