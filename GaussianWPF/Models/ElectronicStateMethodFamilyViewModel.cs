using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

using GaussianCommonLibrary.Models;

namespace GaussianWPF.Models;

/// <summary>
/// View model representing a Electronic State/Method Family Combination for the Gaussian WPF application.
/// Provides data binding and validation for Electronic State/Method Family Combination creation, editing, and display operations in the desktop client.
/// </summary>
public class ElectronicStateMethodFamilyViewModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyViewModel"/> class.
	/// </summary>
	public ElectronicStateMethodFamilyViewModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyViewModel"/> class from a full model.
	/// </summary>
	/// <param name="model">The Electronic State/Method Family Combination full model containing the source data.</param>
	/// <param name="electronicStates">The list of available Electronic States for dropdown selection.</param>
	/// <param name="methodFamilies">The list of available Method Families for dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicState"/> or <paramref name="methodFamilies"/> is null.</exception>
	public ElectronicStateMethodFamilyViewModel(ElectronicStateMethodFamilyFullModel model, IList<ElectronicStateRecord> electronicStates, IList<MethodFamilyRecord> methodFamilies)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(electronicStates, nameof(electronicStates));
		ArgumentNullException.ThrowIfNull(methodFamilies, nameof(methodFamilies));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicState = electronicStates.First(x => x.Id == model.ElectronicState.Id);
		MethodFamily = model.MethodFamily is null ? null : methodFamilies.FirstOrDefault(x => x.Id == model.MethodFamily.Id);
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (ElectronicStateRecord item in electronicStates)
		{
			ElectronicStateList.Add(item);
		}

		foreach (MethodFamilyRecord item in methodFamilies)
		{
			MethodFamilyList.Add(item);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyViewModel"/> class from an intermediate model.
	/// </summary>
	/// <param name="model">The Electronic State/Method Family Combination intermediate model containing the source data.</param>
	/// <param name="electronicStates">The list of available Electronic States for dropdown selection.</param>
	/// <param name="methodFamilies">The list of available Method Families for dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicState"/> or <paramref name="methodFamilies"/> is null.</exception>
	public ElectronicStateMethodFamilyViewModel(ElectronicStateMethodFamilyIntermediateModel model, IList<ElectronicStateRecord> electronicStates, IList<MethodFamilyRecord> methodFamilies)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(electronicStates, nameof(electronicStates));
		ArgumentNullException.ThrowIfNull(methodFamilies, nameof(methodFamilies));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicState = model.ElectronicState;
		MethodFamily = model.MethodFamily;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (ElectronicStateRecord item in electronicStates)
		{
			ElectronicStateList.Add(item);
		}

		foreach (MethodFamilyRecord item in methodFamilies)
		{
			MethodFamilyList.Add(item);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyViewModel"/> class from a simple model.
	/// </summary>
	/// <param name="model">The Electronic State/Method Family Combination simple model containing the source data.</param>
	/// <param name="electronicStates">The list of available Electronic States for dropdown selection.</param>
	/// <param name="methodFamilies">The list of available Method Families for dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicState"/> or <paramref name="methodFamilies"/> is null.</exception>
	public ElectronicStateMethodFamilyViewModel(ElectronicStateMethodFamilySimpleModel model, IList<ElectronicStateRecord> electronicStates, IList<MethodFamilyRecord> methodFamilies)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(electronicStates, nameof(electronicStates));
		ArgumentNullException.ThrowIfNull(methodFamilies, nameof(methodFamilies));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicState = electronicStates.First(x => x.Id == model.ElectronicStateId);
		MethodFamily = model.MethodFamilyId is null ? null : methodFamilies.FirstOrDefault(x => x.Id == model.MethodFamilyId);
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (ElectronicStateRecord item in electronicStates)
		{
			ElectronicStateList.Add(item);
		}

		foreach (MethodFamilyRecord item in methodFamilies)
		{
			MethodFamilyList.Add(item);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyViewModel"/> class with Method Families for new record creation.
	/// </summary>
	/// <param name="electronicStates">The list of available Electronic States for dropdown selection.</param>
	/// <param name="methodFamilies">The list of available Method Families for dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="electronicState"/> or <paramref name="methodFamilies"/> is null.</exception>
	public ElectronicStateMethodFamilyViewModel(IList<ElectronicStateRecord> electronicStates, IList<MethodFamilyRecord> methodFamilies)
	{
		ArgumentNullException.ThrowIfNull(electronicStates, nameof(electronicStates));
		ArgumentNullException.ThrowIfNull(methodFamilies, nameof(methodFamilies));

		foreach (ElectronicStateRecord item in electronicStates)
		{
			ElectronicStateList.Add(item);
		}

		foreach (MethodFamilyRecord item in methodFamilies)
		{
			MethodFamilyList.Add(item);
		}
	}

	/// <summary>
	/// Gets or sets the unique identifier for the Electronic State/Method Family Combination.
	/// </summary>
	[Display(Name = "Id")]
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the name of the Electronic State/Method Family Combination.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Name")]
	[MaxLength(100)]
	public string? Name { get; set; }

	/// <summary>
	/// Gets or sets the keyword that identifies the Electronic State/Method Family Combination.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Keyword")]
	[MaxLength(20)]
	public string? Keyword { get; set; }

	/// <summary>
	/// Gets or sets the Electronic State associated with this Electronic State/Method Family Combination.
	/// </summary>
	[Display(Name = "Electronic State")]
	[Required]
	public ElectronicStateRecord? ElectronicState { get; set; }

	/// <summary>
	/// Gets or sets the observable collection of Electronic States available for selection in the UI.
	/// </summary>
	/// <remarks>
	/// This collection supports data binding for ComboBox or similar controls in WPF.
	/// </remarks>
	public ObservableCollection<ElectronicStateRecord> ElectronicStateList { get; set; } = [];

	/// <summary>
	/// Gets or sets the Method Family associated with this Electronic State/Method Family Combination.
	/// </summary>
	[Display(Name = "Method Family")]
	public MethodFamilyRecord? MethodFamily { get; set; }

	/// <summary>
	/// Gets or sets the observable collection of Method Families available for selection in the UI.
	/// </summary>
	/// <remarks>
	/// This collection supports data binding for ComboBox or similar controls in WPF.
	/// </remarks>
	public ObservableCollection<MethodFamilyRecord> MethodFamilyList { get; set; } = [];

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
	[MaxLength(2000)]
	public string? DescriptionText { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the Electronic State/Method Family Combination was created.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Created Date")]
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when the Electronic State/Method Family Combination was last updated.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Last Updated Date")]
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether the Electronic State/Method Family Combination is archived.
	/// </summary>
	[Display(Name = "Archived")]
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this view model to a <see cref="ElectronicStateMethodFamilyFullModel"/> instance.
	/// </summary>
	/// <param name="electronicState">The Electronic State full model to associate with the Electronic State/Method Family Combination.</param>
	/// <param name="methodFamily">The Method Family full model to associate with the Electronic State/Method Family Combination.</param>
	/// <returns>A new instance of <see cref="ElectronicStateMethodFamilyFullModel"/> populated with the view model data.</returns>
	public ElectronicStateMethodFamilyFullModel ToFullModel(ElectronicStateFullModel electronicState, MethodFamilyFullModel? methodFamily = null)
	{
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
	/// Converts this view model to a <see cref="ElectronicStateMethodFamilyIntermediateModel"/> instance.
	/// </summary>
	/// <param name="electronicState">The Electronic State record to associate with the Electronic State/Method Family Combination.</param>
	/// <param name="methodFamily">The Method Family record to associate with the Electronic State/Method Family Combination.</param>
	/// <returns>A new instance of <see cref="ElectronicStateMethodFamilyIntermediateModel"/> populated with the view model data.</returns>
	public ElectronicStateMethodFamilyIntermediateModel ToIntermediateModel(ElectronicStateRecord electronicState, MethodFamilyRecord? methodFamily = null)
	{
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
	/// Converts this view model to a <see cref="ElectronicStateMethodFamilySimpleModel"/> instance.
	/// </summary>
	/// <returns>A new instance of <see cref="ElectronicStateMethodFamilySimpleModel"/> populated with the view model data.</returns>
	public ElectronicStateMethodFamilySimpleModel ToSimpleModel()
	{
		return new ElectronicStateMethodFamilySimpleModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicStateId = ElectronicState!.Id,
			MethodFamilyId = MethodFamily?.Id,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Returns a string representation of the Electronic State/Method Family Combination.
	/// </summary>
	/// <returns>The Name if available, otherwise the Keyword, or a combination of both in the format "Name/Keyword".</returns>
	public override string? ToString()
	{
		return string.IsNullOrWhiteSpace(Name) ? $"{Keyword}" : string.IsNullOrWhiteSpace(Keyword) ? $"{Name}" : $"{Name}/{Keyword}";
	}
}
