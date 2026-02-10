namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents an Electronic State/Method Family Combination with only the Electronic State and Method Family identifiers.
/// </summary>
public class ElectronicStateMethodFamilySimpleModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilySimpleModel"/> class.
	/// </summary>
	public ElectronicStateMethodFamilySimpleModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilySimpleModel"/> class from an intermediate model.
	/// </summary>
	/// <param name="intermediateModel">The intermediate model to convert from.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="intermediateModel"/> is null.</exception>
	public ElectronicStateMethodFamilySimpleModel(ElectronicStateMethodFamilyIntermediateModel intermediateModel)
	{
		ArgumentNullException.ThrowIfNull(intermediateModel, nameof(intermediateModel));
		Id = intermediateModel.Id;
		Name = intermediateModel.Name;
		Keyword = intermediateModel.Keyword;
		ElectronicStateId = intermediateModel.ElectronicState.Id;
		MethodFamilyId = intermediateModel.MethodFamily?.Id;
		DescriptionRtf = intermediateModel.DescriptionRtf;
		DescriptionText = intermediateModel.DescriptionText;
		CreatedDate = intermediateModel.CreatedDate;
		LastUpdatedDate = intermediateModel.LastUpdatedDate;
		Archived = intermediateModel.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilySimpleModel"/> class from a full model.
	/// </summary>
	/// <param name="fullModel">The full model to convert from.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="fullModel"/> is null.</exception>
	public ElectronicStateMethodFamilySimpleModel(ElectronicStateMethodFamilyFullModel fullModel)
	{
		ArgumentNullException.ThrowIfNull(fullModel, nameof(fullModel));
		Id = fullModel.Id;
		Name = fullModel.Name;
		Keyword = fullModel.Keyword;
		ElectronicStateId = fullModel.ElectronicState.Id;
		MethodFamilyId = fullModel.MethodFamily?.Id;
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
	/// Gets or sets the identifier of the Electronic State to which this Electronic State/Method Family Combination belongs.
	/// </summary>
	public int ElectronicStateId { get; set; }

	/// <summary>
	/// Gets or sets the identifier of the Method Family to which this Electronic State/Method Family Combination belongs.
	/// </summary>
	public int? MethodFamilyId { get; set; }

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
	/// Converts this model to a <see cref="ElectronicStateMethodFamilyIntermediateModel"/>.
	/// </summary>
	/// <param name="electronicState">The Electronic State record to include in the intermediate model.</param>
	/// <param name="methodFamily">The Method Family record to include in the intermediate model.</param>
	/// <returns>An intermediate model representation of this Base Method.</returns>
	public ElectronicStateMethodFamilyIntermediateModel ToIntermediateModel(ElectronicStateRecord electronicState, MethodFamilyRecord? methodFamily = null)
	{
		ArgumentNullException.ThrowIfNull(electronicState, nameof(electronicState));

		return new ElectronicStateMethodFamilyIntermediateModel
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
	/// Converts this model to a <see cref="ElectronicStateMethodFamilyFullModel"/> using the specified Electronic State and Method Family.
	/// </summary>
	/// <param name="electronicState">The Electronic State full model to associate with the resulting full model.</param>
	/// <param name="methodFamily">The Method Family full model to associate with the resulting full model.</param>
	/// <returns>A new <see cref="ElectronicStateMethodFamilyFullModel"/> instance with the properties copied from this model.</returns>
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
