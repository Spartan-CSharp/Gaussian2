using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

using GaussianCommonLibrary.Models;

namespace GaussianWPF.Models;

/// <summary>
/// View model representing a Spin State/Electronic State/Method Family Combination for the Gaussian WPF application.
/// Provides data binding and validation for Spin State/Electronic State/Method Family Combination creation, editing, and display operations in the desktop client.
/// </summary>
public class SpinStateElectronicStateMethodFamilyViewModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyViewModel"/> class.
	/// </summary>
	public SpinStateElectronicStateMethodFamilyViewModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyViewModel"/> class from a full model.
	/// </summary>
	/// <param name="model">The Spin State/Electronic State/Method Family Combination full model containing the source data.</param>
	/// <param name="electronicStatesMethodFamilies">The list of available Electronic State/Method Family Comibations for dropdown selection.</param>
	/// <param name="spinStates">The list of available Spin States for dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicStatesMethodFamilies"/> or <paramref name="spinStates"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilyViewModel(SpinStateElectronicStateMethodFamilyFullModel model, IList<ElectronicStateMethodFamilyRecord> electronicStatesMethodFamilies, IList<SpinStateRecord> spinStates)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(electronicStatesMethodFamilies, nameof(electronicStatesMethodFamilies));
		ArgumentNullException.ThrowIfNull(spinStates, nameof(spinStates));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateMethodFamily = electronicStatesMethodFamilies.First(x => x.Id == model.ElectronicStateMethodFamily.Id);
		SpinState = model.SpinState is null ? null : spinStates.FirstOrDefault(x => x.Id == model.SpinState.Id);
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (ElectronicStateMethodFamilyRecord item in electronicStatesMethodFamilies)
		{
			ElectronicStateMethodFamilyList.Add(item);
		}

		foreach (SpinStateRecord item in spinStates)
		{
			SpinStateList.Add(item);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyViewModel"/> class from an intermediate model.
	/// </summary>
	/// <param name="model">The Spin State/Electronic State/Method Family Combination intermediate model containing the source data.</param>
	/// <param name="electronicStatesMethodFamilies">The list of available Electronic State/Method Family Comibations for dropdown selection.</param>
	/// <param name="spinStates">The list of available Spin States for dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicStatesMethodFamilies"/> or <paramref name="spinStates"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilyViewModel(SpinStateElectronicStateMethodFamilyIntermediateModel model, IList<ElectronicStateMethodFamilyRecord> electronicStatesMethodFamilies, IList<SpinStateRecord> spinStates)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(electronicStatesMethodFamilies, nameof(electronicStatesMethodFamilies));
		ArgumentNullException.ThrowIfNull(spinStates, nameof(spinStates));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateMethodFamily = model.ElectronicStateMethodFamily;
		SpinState = model.SpinState;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (ElectronicStateMethodFamilyRecord item in electronicStatesMethodFamilies)
		{
			ElectronicStateMethodFamilyList.Add(item);
		}

		foreach (SpinStateRecord item in spinStates)
		{
			SpinStateList.Add(item);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyViewModel"/> class from a simple model.
	/// </summary>
	/// <param name="model">The Spin State/Electronic State/Method Family Combination simple model containing the source data.</param>
	/// <param name="electronicStatesMethodFamilies">The list of available Electronic State/Method Family Comibations for dropdown selection.</param>
	/// <param name="spinStates">The list of available Spin States for dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicStatesMethodFamilies"/> or <paramref name="spinStates"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilyViewModel(SpinStateElectronicStateMethodFamilySimpleModel model, IList<ElectronicStateMethodFamilyRecord> electronicStatesMethodFamilies, IList<SpinStateRecord> spinStates)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(electronicStatesMethodFamilies, nameof(electronicStatesMethodFamilies));
		ArgumentNullException.ThrowIfNull(spinStates, nameof(spinStates));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateMethodFamily = electronicStatesMethodFamilies.First(x => x.Id == model.ElectronicStateMethodFamilyId);
		SpinState = model.SpinStateId is null ? null : spinStates.FirstOrDefault(x => x.Id == model.SpinStateId);
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (ElectronicStateMethodFamilyRecord item in electronicStatesMethodFamilies)
		{
			ElectronicStateMethodFamilyList.Add(item);
		}

		foreach (SpinStateRecord item in spinStates)
		{
			SpinStateList.Add(item);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyViewModel"/> class with Method Families for new record creation.
	/// </summary>
	/// <param name="electronicStatesMethodFamilies">The list of available Electronic State/Method Family Comibations for dropdown selection.</param>
	/// <param name="spinStates">The list of available Spin States for dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="electronicStatesMethodFamilies"/> or <paramref name="spinStates"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilyViewModel(IList<ElectronicStateMethodFamilyRecord> electronicStatesMethodFamilies, IList<SpinStateRecord> spinStates)
	{
		ArgumentNullException.ThrowIfNull(electronicStatesMethodFamilies, nameof(electronicStatesMethodFamilies));
		ArgumentNullException.ThrowIfNull(spinStates, nameof(spinStates));

		foreach (ElectronicStateMethodFamilyRecord item in electronicStatesMethodFamilies)
		{
			ElectronicStateMethodFamilyList.Add(item);
		}

		foreach (SpinStateRecord item in spinStates)
		{
			SpinStateList.Add(item);
		}
	}

	/// <summary>
	/// Gets or sets the unique identifier for the Spin State/Electronic State/Method Family Combination.
	/// </summary>
	[Display(Name = "Id")]
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the name of the Spin State/Electronic State/Method Family Combination.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Name")]
	[MaxLength(200)]
	public string? Name { get; set; }

	/// <summary>
	/// Gets or sets the keyword that identifies the Spin State/Electronic State/Method Family Combination.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Keyword")]
	[MaxLength(50)]
	public string? Keyword { get; set; }

	/// <summary>
	/// Gets or sets the Electronic State/Method Family Combination associated with this Spin State/Electronic State/Method Family Combination.
	/// </summary>
	[Display(Name = "Electronic State/Method Family Combination")]
	[Required]
	public ElectronicStateMethodFamilyRecord? ElectronicStateMethodFamily { get; set; }

	/// <summary>
	/// Gets or sets the observable collection of Electronic State/Method Family Combinations available for selection in the UI.
	/// </summary>
	/// <remarks>
	/// This collection supports data binding for ComboBox or similar controls in WPF.
	/// </remarks>
	public ObservableCollection<ElectronicStateMethodFamilyRecord> ElectronicStateMethodFamilyList { get; set; } = [];

	/// <summary>
	/// Gets or sets the Spin State associated with this Spin State/Electronic State/Method Family Combination.
	/// </summary>
	[Display(Name = "Spin State")]
	public SpinStateRecord? SpinState { get; set; }

	/// <summary>
	/// Gets or sets the observable collection of Spin States available for selection in the UI.
	/// </summary>
	/// <remarks>
	/// This collection supports data binding for ComboBox or similar controls in WPF.
	/// </remarks>
	public ObservableCollection<SpinStateRecord> SpinStateList { get; set; } = [];

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
	/// Gets or sets the date and time when the Spin State/Electronic State/Method Family Combination was created.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Created Date")]
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when the Spin State/Electronic State/Method Family Combination was last updated.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Last Updated Date")]
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether the Spin State/Electronic State/Method Family Combination is archived.
	/// </summary>
	[Display(Name = "Archived")]
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this view model to a <see cref="SpinStateElectronicStateMethodFamilyFullModel"/> instance.
	/// </summary>
	/// <param name="electronicStateethodFamily">The Electronic State/Method Family Combination full model to associate with the Spin State/Electronic State/Method Family Combination.</param>
	/// <param name="spinState">The Spin State full model to associate with the Spin State/Electronic State/Method Family Combination.</param>
	/// <returns>A new instance of <see cref="SpinStateElectronicStateMethodFamilyFullModel"/> populated with the view model data.</returns>
	public SpinStateElectronicStateMethodFamilyFullModel ToFullModel(ElectronicStateMethodFamilyFullModel electronicStateethodFamily, SpinStateFullModel? spinState = null)
	{
		return new SpinStateElectronicStateMethodFamilyFullModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicStateMethodFamily = electronicStateethodFamily,
			SpinState = spinState,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this view model to a <see cref="SpinStateElectronicStateMethodFamilyIntermediateModel"/> instance.
	/// </summary>
	/// <param name="electronicStateMethodFamily">The Electronic State/Method Family Combination record to associate with the Spin State/Electronic State/Method Family Combination.</param>
	/// <param name="spinState">The Spin State record to associate with the Spin State/Electronic State/Method Family Combination.</param>
	/// <returns>A new instance of <see cref="SpinStateElectronicStateMethodFamilyIntermediateModel"/> populated with the view model data.</returns>
	public SpinStateElectronicStateMethodFamilyIntermediateModel ToIntermediateModel(ElectronicStateMethodFamilyRecord electronicStateMethodFamily, SpinStateRecord? spinState = null)
	{
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
	/// Converts this view model to a <see cref="SpinStateElectronicStateMethodFamilySimpleModel"/> instance.
	/// </summary>
	/// <returns>A new instance of <see cref="SpinStateElectronicStateMethodFamilySimpleModel"/> populated with the view model data.</returns>
	public SpinStateElectronicStateMethodFamilySimpleModel ToSimpleModel()
	{
		return new SpinStateElectronicStateMethodFamilySimpleModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicStateMethodFamilyId = ElectronicStateMethodFamily!.Id,
			SpinStateId = SpinState?.Id,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this view model to a <see cref="SpinStateElectronicStateMethodFamilyRecord"/> instance.
	/// </summary>
	/// <returns>A new <see cref="SpinStateElectronicStateMethodFamilyRecord"/> containing the Id, Name, and Keyword properties.</returns>
	public SpinStateElectronicStateMethodFamilyRecord ToRecord()
	{
		return new SpinStateElectronicStateMethodFamilyRecord(Id, Name, Keyword);
	}

	/// <summary>
	/// Returns a string representation of the Spin State/Electronic State/Method Family Combination.
	/// </summary>
	/// <returns>The Name if available, otherwise the Keyword, or a combination of both in the format "Name/Keyword".</returns>
	public override string? ToString()
	{
		return string.IsNullOrWhiteSpace(Name) ? $"{Keyword}" : string.IsNullOrWhiteSpace(Keyword) ? $"{Name}" : $"{Name}/{Keyword}";
	}
}
