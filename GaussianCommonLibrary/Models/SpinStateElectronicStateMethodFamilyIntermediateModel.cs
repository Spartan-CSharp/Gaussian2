namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents an Spin State/Electronic State/Method Family Combination with Electronic State and Method Family information as records rather than full models.
/// </summary>
public class SpinStateElectronicStateMethodFamilyIntermediateModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyIntermediateModel"/> class.
	/// </summary>
	public SpinStateElectronicStateMethodFamilyIntermediateModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyIntermediateModel"/> class from a simple fullModel, an Electronic State record, and Method Family record, the last of which can be null.
	/// </summary>
	/// <param name="model">The simple fullModel containing Spin State/Electronic State/Method Family Combination properties.</param>
	/// <param name="electronicStateMethodFamily">The Electronic State/Method Family Combination record to associate with this Spin State/Electronic State/Method Family Combination.</param>
	/// <param name="spinState">The Spin State record to associate with this Spin State/Electronic State/Method Family Combination.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicStateMethodFamily"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilyIntermediateModel(SpinStateElectronicStateMethodFamilySimpleModel model, ElectronicStateMethodFamilyRecord electronicStateMethodFamily, SpinStateRecord? spinState = null)
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
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyIntermediateModel"/> class from a full fullModel.
	/// </summary>
	/// <param name="fullModel">The full model to convert from.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="fullModel"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilyIntermediateModel(SpinStateElectronicStateMethodFamilyFullModel fullModel)
	{
		ArgumentNullException.ThrowIfNull(fullModel, nameof(fullModel));
		Id = fullModel.Id;
		Name = fullModel.Name;
		Keyword = fullModel.Keyword;
		ElectronicStateMethodFamily = fullModel.ElectronicStateMethodFamily.ToRecord();
		SpinState = fullModel.SpinState?.ToRecord();
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
	/// Gets or sets the Electronic State/Method Family Combination record to which this Spin State/Electronic State/Method Family Combination belongs.
	/// </summary>
	public required ElectronicStateMethodFamilyRecord ElectronicStateMethodFamily { get; set; }

	/// <summary>
	/// Gets or sets the Spin State record to which this Spin State/Electronic State/Method Family Combination belongs.
	/// </summary>
	public SpinStateRecord? SpinState { get; set; }

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
	/// Converts this model to a <see cref="SpinStateElectronicStateMethodFamilySimpleModel"/>.
	/// </summary>
	/// <returns>A simple model representation of this Spin State/Electronic State/Method Family Combination.</returns>
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
	/// Converts this model to a <see cref="SpinStateElectronicStateMethodFamilyFullModel"/> using the specified Electronic State/Method Family Combination and Spin State, the second of which may be null.
	/// </summary>
	/// <param name="electronicStateMethodFamily">The Electronic State/Method Family Combination full model to associate with the resulting full model.</param>
	/// <param name="spinState">The Spin State full model to associate with the resulting full model.</param>
	/// <returns>A full model representation of this Base Method.</returns>
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
