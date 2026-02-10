namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents an Electronic State/Method Family Combination with Electronic State and Method Family information as records rather than full models.
/// </summary>
public class ElectronicStateMethodFamilyIntermediateModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyIntermediateModel"/> class.
	/// </summary>
	public ElectronicStateMethodFamilyIntermediateModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyIntermediateModel"/> class from a simple fullModel, an Electronic State record, and Method Family record, the last of which can be null.
	/// </summary>
	/// <param name="model">The simple fullModel containing Electronic State/Method Family Combination properties.</param>
	/// <param name="electronicState">The Electronic State record to associate with this Electronic State/Method Family Combination.</param>
	/// <param name="methodFamily">The Method Family record to associate with this Electronic State/Method Family Combination.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicState"/> is null.</exception>
	public ElectronicStateMethodFamilyIntermediateModel(ElectronicStateMethodFamilySimpleModel model, ElectronicStateRecord electronicState, MethodFamilyRecord? methodFamily = null)
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
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyIntermediateModel"/> class from a full fullModel.
	/// </summary>
	/// <param name="fullModel">The full model to convert from.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="fullModel"/> is null.</exception>
	public ElectronicStateMethodFamilyIntermediateModel(ElectronicStateMethodFamilyFullModel fullModel)
	{
		ArgumentNullException.ThrowIfNull(fullModel, nameof(fullModel));
		Id = fullModel.Id;
		Name = fullModel.Name;
		Keyword = fullModel.Keyword;
		ElectronicState = fullModel.ElectronicState.ToRecord();
		MethodFamily = fullModel.MethodFamily?.ToRecord();
		DescriptionRtf = fullModel.DescriptionRtf;
		DescriptionText = fullModel.DescriptionText;
		CreatedDate = fullModel.CreatedDate;
		LastUpdatedDate = fullModel.LastUpdatedDate;
		Archived = fullModel.Archived;
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
	/// Gets or sets the Electronic State record to which this Electronic State/Method Family Combination belongs.
	/// </summary>
	public required ElectronicStateRecord ElectronicState { get; set; }

	/// <summary>
	/// Gets or sets the Method Family record to which this Electronic State/Method Family Combination belongs.
	/// </summary>
	public MethodFamilyRecord? MethodFamily { get; set; }

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
	/// Converts this model to a <see cref="ElectronicStateMethodFamilySimpleModel"/>.
	/// </summary>
	/// <returns>A simple model representation of this Base Method.</returns>
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
	/// Converts this model to a <see cref="ElectronicStateMethodFamilyFullModel"/> using the specified Electronic State and Method Family, the second of which may be null.
	/// </summary>
	/// <param name="electronicState">The Electronic State full model to associate with the resulting full model.</param>
	/// <param name="methodFamily">The Method Family full model to associate with the resulting full model.</param>
	/// <returns>A full model representation of this Base Method.</returns>
	public ElectronicStateMethodFamilyFullModel ToFullModel(ElectronicStateFullModel electronicState, MethodFamilyFullModel? methodFamily = null)
	{
		ArgumentNullException.ThrowIfNull(electronicState, nameof(electronicState));

		return new ElectronicStateMethodFamilyFullModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicState = electronicState,
			MethodFamily = methodFamily,
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
