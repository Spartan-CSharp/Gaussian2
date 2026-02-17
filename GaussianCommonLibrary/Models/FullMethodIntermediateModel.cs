namespace GaussianCommonLibrary.Models;

/// <summary>
/// Represents a Full Method with Method Family information as a record rather than a full model.
/// </summary>
public class FullMethodIntermediateModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodIntermediateModel"/> class.
	/// </summary>
	public FullMethodIntermediateModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodIntermediateModel"/> class from a simple model, a Spin State/Electronic State/Method Family record, and a Base Method record.
	/// </summary>
	/// <param name="model">The simple model containing Full Method properties.</param>
	/// <param name="spinStateElectronicStateMethodFamily">The Spin State/Electronic State/Method Family record to associate with this Full Method.</param>
	/// <param name="baseMethod">The Base Method record to associate with this Full Method.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="spinStateElectronicStateMethodFamily"/> or <paramref name="baseMethod"/> is null.</exception>
	public FullMethodIntermediateModel(FullMethodSimpleModel model, SpinStateElectronicStateMethodFamilyRecord spinStateElectronicStateMethodFamily, BaseMethodRecord baseMethod)
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
	/// Initializes a new instance of the <see cref="FullMethodIntermediateModel"/> class from a full model.
	/// </summary>
	/// <param name="fullModel">The full model to convert from.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="fullModel"/> is null.</exception>
	public FullMethodIntermediateModel(FullMethodFullModel fullModel)
	{
		ArgumentNullException.ThrowIfNull(fullModel, nameof(fullModel));
		Id = fullModel.Id;
		Keyword = fullModel.Keyword;
		SpinStateElectronicStateMethodFamily = fullModel.SpinStateElectronicStateMethodFamily.ToRecord();
		BaseMethod = fullModel.BaseMethod.ToRecord();
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
	/// Gets or sets the Spin State/Electronic State/Method Family Combination record to which this Full Method belongs.
	/// </summary>
	public required SpinStateElectronicStateMethodFamilyRecord SpinStateElectronicStateMethodFamily { get; set; }

	/// <summary>
	/// Gets or sets the Base Method record to which this Full Method belongs.
	/// </summary>
	public required BaseMethodRecord BaseMethod { get; set; }

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
	/// Converts this model to a <see cref="FullMethodSimpleModel"/>.
	/// </summary>
	/// <returns>A simple model representation of this Full Method.</returns>
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
	/// Converts this model to a <see cref="FullMethodFullModel"/> using the specified Method Family.
	/// </summary>
	/// <param name="spinStateElectronicStateMethodFamily">The Spin State/Electronic State/Method Family full model to associate with the resulting full model.</param>
	/// <param name="baseMethod">The Base Method full model to associate with the resulting full model.</param>
	/// <returns>A full model representation of this Full Method.</returns>
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
