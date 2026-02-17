namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents a Full Method with only the Method Family identifier.
/// </summary>
public class FullMethodSimpleModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodSimpleModel"/> class.
	/// </summary>
	public FullMethodSimpleModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodSimpleModel"/> class from an intermediate model.
	/// </summary>
	/// <param name="intermediateModel">The intermediate model to convert from.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="intermediateModel"/> is null.</exception>
	public FullMethodSimpleModel(FullMethodIntermediateModel intermediateModel)
	{
		ArgumentNullException.ThrowIfNull(intermediateModel, nameof(intermediateModel));
		Id = intermediateModel.Id;
		Keyword = intermediateModel.Keyword;
		SpinStateElectronicStateMethodFamilyId = intermediateModel.SpinStateElectronicStateMethodFamily.Id;
		BaseMethodId = intermediateModel.BaseMethod.Id;
		DescriptionRtf = intermediateModel.DescriptionRtf;
		DescriptionText = intermediateModel.DescriptionText;
		CreatedDate = intermediateModel.CreatedDate;
		LastUpdatedDate = intermediateModel.LastUpdatedDate;
		Archived = intermediateModel.Archived;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodSimpleModel"/> class from a full model.
	/// </summary>
	/// <param name="fullModel">The full model to convert from.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="fullModel"/> is null.</exception>
	public FullMethodSimpleModel(FullMethodFullModel fullModel)
	{
		ArgumentNullException.ThrowIfNull(fullModel, nameof(fullModel));
		Id = fullModel.Id;
		Keyword = fullModel.Keyword;
		SpinStateElectronicStateMethodFamilyId = fullModel.SpinStateElectronicStateMethodFamily.Id;
		BaseMethodId = fullModel.BaseMethod.Id;
		DescriptionRtf = fullModel.DescriptionRtf;
		DescriptionText = fullModel.DescriptionText;
		CreatedDate = fullModel.CreatedDate;
		LastUpdatedDate = fullModel.LastUpdatedDate;
		Archived = fullModel.Archived;
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
	/// Gets or sets the identifier of the Spin State/Electronic State/Method Family Combination to which this Full Method belongs.
	/// </summary>
	public int SpinStateElectronicStateMethodFamilyId { get; set; }

	/// <summary>
	/// Gets or sets the identifier of the Base Method to which this Full Method belongs.
	/// </summary>
	public int BaseMethodId { get; set; }

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
	/// Converts this model to a <see cref="FullMethodIntermediateModel"/>.
	/// </summary>
	/// <param name="spinStateElectronicStateMethodFamily">The Spin State/Electronic State/Method Family record to include in the intermediate model.</param>
	/// <param name="baseMethod">The Base Method record to include in the intermediate model.</param>
	/// <returns>An intermediate model representation of this Full Method.</returns>
	public FullMethodIntermediateModel ToIntermediateModel(SpinStateElectronicStateMethodFamilyRecord spinStateElectronicStateMethodFamily, BaseMethodRecord baseMethod)
	{
		ArgumentNullException.ThrowIfNull(spinStateElectronicStateMethodFamily, nameof(spinStateElectronicStateMethodFamily));
		ArgumentNullException.ThrowIfNull(baseMethod, nameof(baseMethod));
		return new FullMethodIntermediateModel
		{
			Id = Id,
			Keyword = Keyword,
			SpinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamily,
			BaseMethod = baseMethod,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this model to a <see cref="FullMethodFullModel"/> using the specified Method Family.
	/// </summary>
	/// <param name="spinStateElectronicStateMethodFamily">The Spin State/Electronic State/Method Family full model to associate with the resulting full model.</param>
	/// <param name="baseMethod">The Base Method full model to associate with the resulting full model.</param>
	/// <returns>A new <see cref="FullMethodFullModel"/> instance with the properties copied from this model.</returns>
	public FullMethodFullModel ToFullModel(SpinStateElectronicStateMethodFamilyFullModel spinStateElectronicStateMethodFamily, BaseMethodFullModel baseMethod)
	{
		ArgumentNullException.ThrowIfNull(spinStateElectronicStateMethodFamily, nameof(spinStateElectronicStateMethodFamily));
		ArgumentNullException.ThrowIfNull(baseMethod, nameof(baseMethod));

		return new FullMethodFullModel
		{
			Id = Id,
			Keyword = Keyword,
			SpinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamily,
			BaseMethod = baseMethod,
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
