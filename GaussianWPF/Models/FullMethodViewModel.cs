using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

using GaussianCommonLibrary.Models;

namespace GaussianWPF.Models;

/// <summary>
/// View model representing a Full Method for the Gaussian WPF application.
/// Provides data binding and validation for Full Method creation, editing, and display operations in the desktop client.
/// </summary>
public class FullMethodViewModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodViewModel"/> class.
	/// </summary>
	public FullMethodViewModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodViewModel"/> class from a full model.
	/// </summary>
	/// <param name="model">The Full Method full model containing the source data.</param>
	/// <param name="spinStateElectronicStateMethodFamilies">The list of available Spin State/Electronic State/Method Family Combinations for dropdown selection.</param>
	/// <param name="baseMethods">The list of available Base Methods for dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="spinStateElectronicStateMethodFamilies"/> or <paramref name="baseMethods"/> is null.</exception>
	public FullMethodViewModel(FullMethodFullModel model, IList<SpinStateElectronicStateMethodFamilyRecord> spinStateElectronicStateMethodFamilies, IList<BaseMethodRecord> baseMethods)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(spinStateElectronicStateMethodFamilies, nameof(spinStateElectronicStateMethodFamilies));
		ArgumentNullException.ThrowIfNull(baseMethods, nameof(baseMethods));
		Id = model.Id;
		Keyword = model.Keyword;
		SpinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamilies.First(x => x.Id == model.SpinStateElectronicStateMethodFamily.Id);
		BaseMethod = baseMethods.First(x => x.Id == model.BaseMethod.Id);
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (SpinStateElectronicStateMethodFamilyRecord item in spinStateElectronicStateMethodFamilies)
		{
			SpinStateElectronicStateMethodFamilyList.Add(item);
		}

		foreach (BaseMethodRecord item in baseMethods)
		{
			BaseMethodList.Add(item);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodViewModel"/> class from an intermediate model.
	/// </summary>
	/// <param name="model">The Full Method intermediate model containing the source data.</param>
	/// <param name="spinStateElectronicStateMethodFamilies">The list of available Spin State/Electronic State/Method Family Combinations for dropdown selection.</param>
	/// <param name="baseMethods">The list of available Base Methods for dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="spinStateElectronicStateMethodFamilies"/> or <paramref name="baseMethods"/> is null.</exception>
	public FullMethodViewModel(FullMethodIntermediateModel model, IList<SpinStateElectronicStateMethodFamilyRecord> spinStateElectronicStateMethodFamilies, IList<BaseMethodRecord> baseMethods)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(spinStateElectronicStateMethodFamilies, nameof(spinStateElectronicStateMethodFamilies));
		ArgumentNullException.ThrowIfNull(baseMethods, nameof(baseMethods));
		Id = model.Id;
		Keyword = model.Keyword;
		SpinStateElectronicStateMethodFamily = model.SpinStateElectronicStateMethodFamily;
		BaseMethod = model.BaseMethod;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (SpinStateElectronicStateMethodFamilyRecord item in spinStateElectronicStateMethodFamilies)
		{
			SpinStateElectronicStateMethodFamilyList.Add(item);
		}

		foreach (BaseMethodRecord item in baseMethods)
		{
			BaseMethodList.Add(item);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodViewModel"/> class from a simple model.
	/// </summary>
	/// <param name="model">The Full Method simple model containing the source data.</param>
	/// <param name="spinStateElectronicStateMethodFamilies">The list of available Spin State/Electronic State/Method Family Combinations for dropdown selection.</param>
	/// <param name="baseMethods">The list of available Base Methods for dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="spinStateElectronicStateMethodFamilies"/> or <paramref name="baseMethods"/> is null.</exception>
	public FullMethodViewModel(FullMethodSimpleModel model, IList<SpinStateElectronicStateMethodFamilyRecord> spinStateElectronicStateMethodFamilies, IList<BaseMethodRecord> baseMethods)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(spinStateElectronicStateMethodFamilies, nameof(spinStateElectronicStateMethodFamilies));
		ArgumentNullException.ThrowIfNull(baseMethods, nameof(baseMethods));
		Id = model.Id;
		Keyword = model.Keyword;
		SpinStateElectronicStateMethodFamily = spinStateElectronicStateMethodFamilies.First(x => x.Id == model.SpinStateElectronicStateMethodFamilyId);
		BaseMethod = baseMethods.First(x => x.Id == model.BaseMethodId);
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (SpinStateElectronicStateMethodFamilyRecord item in spinStateElectronicStateMethodFamilies)
		{
			SpinStateElectronicStateMethodFamilyList.Add(item);
		}

		foreach (BaseMethodRecord item in baseMethods)
		{
			BaseMethodList.Add(item);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodViewModel"/> class with Method Families for new record creation.
	/// </summary>
	/// <param name="spinStateElectronicStateMethodFamilies">The list of available Spin State/Electronic State/Method Family Combinations for dropdown selection.</param>
	/// <param name="baseMethods">The list of available Base Methods for dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="spinStateElectronicStateMethodFamilies"/> or <paramref name="baseMethods"/> is null.</exception>
	public FullMethodViewModel(IList<SpinStateElectronicStateMethodFamilyRecord> spinStateElectronicStateMethodFamilies, IList<BaseMethodRecord> baseMethods)
	{
		ArgumentNullException.ThrowIfNull(spinStateElectronicStateMethodFamilies, nameof(spinStateElectronicStateMethodFamilies));
		ArgumentNullException.ThrowIfNull(baseMethods, nameof(baseMethods));

		foreach (SpinStateElectronicStateMethodFamilyRecord item in spinStateElectronicStateMethodFamilies)
		{
			SpinStateElectronicStateMethodFamilyList.Add(item);
		}

		foreach (BaseMethodRecord item in baseMethods)
		{
			BaseMethodList.Add(item);
		}
	}

	/// <summary>
	/// Gets or sets the unique identifier for the Full Method.
	/// </summary>
	[Display(Name = "Id")]
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the keyword that identifies the Full Method.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Keyword")]
	[MaxLength(50)]
	[Required]
	public string Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the Spin State/Electronic State/Method Family Combination associated with this Full Method.
	/// </summary>
	[Display(Name = "Spin State/Electronic State/Method Family Combination")]
	[Required]
	public SpinStateElectronicStateMethodFamilyRecord? SpinStateElectronicStateMethodFamily { get; set; }

	/// <summary>
	/// Gets or sets the observable collection of Spin State/Electronic State/Method Family Combinations available for selection in the UI.
	/// </summary>
	/// <remarks>
	/// This collection supports data binding for ComboBox or similar controls in WPF.
	/// </remarks>
	public ObservableCollection<SpinStateElectronicStateMethodFamilyRecord> SpinStateElectronicStateMethodFamilyList { get; set; } = [];

	/// <summary>
	/// Gets or sets the Base Method associated with this Full Method.
	/// </summary>
	[Display(Name = "Base Method")]
	[Required]
	public BaseMethodRecord? BaseMethod { get; set; }

	/// <summary>
	/// Gets or sets the observable collection of Base Methods available for selection in the UI.
	/// </summary>
	/// <remarks>
	/// This collection supports data binding for ComboBox or similar controls in WPF.
	/// </remarks>
	public ObservableCollection<BaseMethodRecord> BaseMethodList { get; set; } = [];

	/// <summary>
	/// Gets or sets the description in RTF format.
	/// </summary>
	[DataType(DataType.MultilineText)]
	[Display(Name = "Description")]
	public string? DescriptionRtf { get; set; }

	/// <summary>
	/// Gets or sets the description in plain text format.
	/// </summary>
	[DataType(DataType.MultilineText)]
	[Display(Name = "Description")]
	[MaxLength(4000)]
	public string? DescriptionText { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the Full Method was created.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Created Date")]
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when the Full Method was last updated.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Last Updated Date")]
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether the Full Method is archived.
	/// </summary>
	[Display(Name = "Archived")]
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this view model to a <see cref="FullMethodFullModel"/> instance.
	/// </summary>
	/// <param name="spinStateElectronicStateMethodFamily">The Spin State/Electronic State/Method Family Combination full model to associate with the Full Method.</param>
	/// <param name="baseMethod">The Base Method full model to associate with the Full Method.</param>
	/// <returns>A new instance of <see cref="FullMethodFullModel"/> populated with the view model data.</returns>
	public FullMethodFullModel ToFullModel(SpinStateElectronicStateMethodFamilyFullModel spinStateElectronicStateMethodFamily, BaseMethodFullModel baseMethod)
	{
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
	/// Converts this view model to a <see cref="FullMethodIntermediateModel"/> instance.
	/// </summary>
	/// <param name="spinStateElectronicStateMethodFamily">The Spin State/Electronic State/Method Family Combination record to associate with the Full Method.</param>
	/// <param name="baseMethod">The Base Method record to associate with the Full Method.</param>
	/// <returns>A new instance of <see cref="FullMethodIntermediateModel"/> populated with the view model data.</returns>
	public FullMethodIntermediateModel ToIntermediateModel(SpinStateElectronicStateMethodFamilyRecord spinStateElectronicStateMethodFamily, BaseMethodRecord baseMethod)
	{
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
	/// Converts this view model to a <see cref="FullMethodSimpleModel"/> instance.
	/// </summary>
	/// <returns>A new instance of <see cref="FullMethodSimpleModel"/> populated with the view model data.</returns>
	public FullMethodSimpleModel ToSimpleModel()
	{
		return new FullMethodSimpleModel
		{
			Id = Id,
			Keyword = Keyword,
			SpinStateElectronicStateMethodFamilyId = SpinStateElectronicStateMethodFamily!.Id,
			BaseMethodId = BaseMethod!.Id,
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
	/// <returns>The keyword of the Full Method.</returns>
	public override string? ToString()
	{
		return $"{Keyword}";
	}
}
