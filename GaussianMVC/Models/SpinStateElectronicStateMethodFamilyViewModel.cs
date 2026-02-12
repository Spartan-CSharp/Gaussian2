using System.ComponentModel.DataAnnotations;
using System.Globalization;

using GaussianCommonLibrary.Models;

using GaussianMVC.ValidationAttributes;

using GaussianMVCLibrary.Converters;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace GaussianMVC.Models;

/// <summary>
/// View model representing a Spin State/Electronic State/Method Family Combination for the Gaussian MVC application.
/// Provides data binding and validation for Spin State/Electronic State/Method Family Combination creation, editing, and display operations.
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
	/// <param name="electronicStatesMethodFamilies">The list of available Electronic State/Method Family Combinations for the dropdown selection.</param>
	/// <param name="spinStates">The list of available Spin States for the dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicStatesMethodFamilies"/> or <paramref name="spinStates"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilyViewModel(SpinStateElectronicStateMethodFamilyFullModel model, IList<ElectronicStateMethodFamilyRecord> electronicStatesMethodFamilies, IList<SpinStateRecord> spinStates)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(electronicStatesMethodFamilies, nameof(electronicStatesMethodFamilies));
		ArgumentNullException.ThrowIfNull(spinStates, nameof(spinStates));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateMethodFamilyId = model.ElectronicStateMethodFamily.Id.ToString(CultureInfo.InvariantCulture);
		SpinStateId = model.SpinState?.Id.ToString(CultureInfo.InvariantCulture);
		DescriptionHtml = RtfConverter.RtfToHtmlConverter(model.DescriptionRtf);
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (ElectronicStateMethodFamilyRecord item in electronicStatesMethodFamilies)
		{
			SelectListItem listItem = new()
			{
				Text = string.IsNullOrWhiteSpace(item.Name) ? $"{item.Keyword}" : string.IsNullOrWhiteSpace(item.Keyword) ? $"{item.Name}" : $"{item.Name}/{item.Keyword}",
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			if (item.Id == model.ElectronicStateMethodFamily.Id)
			{
				listItem.Selected = true;
			}

			ElectronicStateMethodFamilyList.Add(listItem);
		}

		foreach (SpinStateRecord item in spinStates)
		{
			SelectListItem listItem = new()
			{
				Text = string.IsNullOrWhiteSpace(item.Name) ? $"{item.Keyword}" : string.IsNullOrWhiteSpace(item.Keyword) ? $"{item.Name}" : $"{item.Name}/{item.Keyword}",
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			if (item.Id == model.SpinState?.Id)
			{
				listItem.Selected = true;
			}

			SpinStateList.Add(listItem);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyViewModel"/> class from an intermediate model.
	/// </summary>
	/// <param name="model">The Spin State/Electronic State/Method Family Combination intermediate model containing the source data.</param>
	/// <param name="electronicStatesMethodFamilies">The list of available Electronic State/Method Family Combinations for the dropdown selection.</param>
	/// <param name="spinStates">The list of available Spin States for the dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicStatesMethodFamilies"/> or <paramref name="spinStates"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilyViewModel(SpinStateElectronicStateMethodFamilyIntermediateModel model, IList<ElectronicStateMethodFamilyRecord> electronicStatesMethodFamilies, IList<SpinStateRecord> spinStates)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(electronicStatesMethodFamilies, nameof(electronicStatesMethodFamilies));
		ArgumentNullException.ThrowIfNull(spinStates, nameof(spinStates));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateMethodFamilyId = model.ElectronicStateMethodFamily.Id.ToString(CultureInfo.InvariantCulture);
		SpinStateId = model.SpinState?.Id.ToString(CultureInfo.InvariantCulture);
		DescriptionHtml = RtfConverter.RtfToHtmlConverter(model.DescriptionRtf);
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (ElectronicStateMethodFamilyRecord item in electronicStatesMethodFamilies)
		{
			SelectListItem listItem = new()
			{
				Text = string.IsNullOrWhiteSpace(item.Name) ? $"{item.Keyword}" : string.IsNullOrWhiteSpace(item.Keyword) ? $"{item.Name}" : $"{item.Name}/{item.Keyword}",
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			if (item.Id == model.ElectronicStateMethodFamily.Id)
			{
				listItem.Selected = true;
			}

			ElectronicStateMethodFamilyList.Add(listItem);
		}

		foreach (SpinStateRecord item in spinStates)
		{
			SelectListItem listItem = new()
			{
				Text = string.IsNullOrWhiteSpace(item.Name) ? $"{item.Keyword}" : string.IsNullOrWhiteSpace(item.Keyword) ? $"{item.Name}" : $"{item.Name}/{item.Keyword}",
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			if (item.Id == model.SpinState?.Id)
			{
				listItem.Selected = true;
			}

			SpinStateList.Add(listItem);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyViewModel"/> class from a simple model.
	/// </summary>
	/// <param name="model">The Spin State/Electronic State/Method Family Combination simple model containing the source data.</param>
	/// <param name="electronicStatesMethodFamilies">The list of available Electronic State/Method Family Combinations for the dropdown selection.</param>
	/// <param name="spinStates">The list of available Spin States for the dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicStatesMethodFamilies"/> or <paramref name="spinStates"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilyViewModel(SpinStateElectronicStateMethodFamilySimpleModel model, IList<ElectronicStateMethodFamilyRecord> electronicStatesMethodFamilies, IList<SpinStateRecord> spinStates)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(electronicStatesMethodFamilies, nameof(electronicStatesMethodFamilies));
		ArgumentNullException.ThrowIfNull(spinStates, nameof(spinStates));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateMethodFamilyId = model.ElectronicStateMethodFamilyId.ToString(CultureInfo.InvariantCulture);
		SpinStateId = model.SpinStateId?.ToString(CultureInfo.InvariantCulture);
		DescriptionHtml = RtfConverter.RtfToHtmlConverter(model.DescriptionRtf);
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (ElectronicStateMethodFamilyRecord item in electronicStatesMethodFamilies)
		{
			SelectListItem listItem = new()
			{
				Text = string.IsNullOrWhiteSpace(item.Name) ? $"{item.Keyword}" : string.IsNullOrWhiteSpace(item.Keyword) ? $"{item.Name}" : $"{item.Name}/{item.Keyword}",
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			if (item.Id == model.ElectronicStateMethodFamilyId)
			{
				listItem.Selected = true;
			}

			ElectronicStateMethodFamilyList.Add(listItem);
		}

		foreach (SpinStateRecord item in spinStates)
		{
			SelectListItem listItem = new()
			{
				Text = string.IsNullOrWhiteSpace(item.Name) ? $"{item.Keyword}" : string.IsNullOrWhiteSpace(item.Keyword) ? $"{item.Name}" : $"{item.Name}/{item.Keyword}",
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			if (item.Id == model.SpinStateId)
			{
				listItem.Selected = true;
			}

			SpinStateList.Add(listItem);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStateElectronicStateMethodFamilyViewModel"/> class with Electronic States and Method Families for new record creation.
	/// </summary>
	/// <param name="electronicStatesMethodFamilies">The list of available Electronic State/Method Family Combinations for the dropdown selection.</param>
	/// <param name="spinStates">The list of available Spin States for the dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="spinStates"/> is null.</exception>
	public SpinStateElectronicStateMethodFamilyViewModel(IList<ElectronicStateMethodFamilyRecord> electronicStatesMethodFamilies, IList<SpinStateRecord> spinStates)
	{
		ArgumentNullException.ThrowIfNull(electronicStatesMethodFamilies, nameof(electronicStatesMethodFamilies));
		ArgumentNullException.ThrowIfNull(spinStates, nameof(spinStates));

		foreach (ElectronicStateMethodFamilyRecord item in electronicStatesMethodFamilies)
		{
			SelectListItem listItem = new()
			{
				Text = string.IsNullOrWhiteSpace(item.Name) ? $"{item.Keyword}" : string.IsNullOrWhiteSpace(item.Keyword) ? $"{item.Name}" : $"{item.Name}/{item.Keyword}",
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			ElectronicStateMethodFamilyList.Add(listItem);
		}

		foreach (SpinStateRecord item in spinStates)
		{
			SelectListItem listItem = new()
			{
				Text = string.IsNullOrWhiteSpace(item.Name) ? $"{item.Keyword}" : string.IsNullOrWhiteSpace(item.Keyword) ? $"{item.Name}" : $"{item.Name}/{item.Keyword}",
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			SpinStateList.Add(listItem);
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
	[RequireAtLeastOne(nameof(Keyword), ErrorMessage = "Either Name or Keyword must be provided.")]
	public string? Name { get; set; }

	/// <summary>
	/// Gets or sets the keyword that identifies the Spin State/Electronic State/Method Family Combination.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Keyword")]
	[MaxLength(50)]
	[RequireAtLeastOne(nameof(Name), ErrorMessage = "Either Name or Keyword must be provided.")]
	public string? Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the Electronic State/Method Family Combination identifier as a string representation.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Electronic State/Method Family Combination")]
	[Required]
	public string ElectronicStateMethodFamilyId { get; set; } = string.Empty;

	/// <summary>
	/// Gets the Electronic State/Method Family Combination name from the selected item in the Electronic State list.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Electronic State/Method Family Combination")]
	public string ElectronicStateMethodFamilyName
	{
		get
		{
			SelectListItem? output = ElectronicStateMethodFamilyList.FirstOrDefault(m => m.Value == ElectronicStateMethodFamilyId || m.Selected);
			return output?.Text ?? string.Empty;
		}
	}

	/// <summary>
	/// Gets or sets the list of Electronic State/Method Family Combinations available for selection in the view.
	/// </summary>
	public IList<SelectListItem> ElectronicStateMethodFamilyList { get; set; } = [];

	/// <summary>
	/// Gets or sets the Spin State identifier as a string representation.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Spin State")]
	public string? SpinStateId { get; set; } = string.Empty;

	/// <summary>
	/// Gets the Spin State name from the selected item in the Spin State list.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Spin State")]
	public string? SpinStateName
	{
		get 
		{
			SelectListItem? output = SpinStateList.FirstOrDefault(m => m.Value == SpinStateId || m.Selected);
			return output?.Text ?? string.Empty;
		}
	}

	/// <summary>
	/// Gets or sets the list of Spin States available for selection in the view.
	/// </summary>
	public IList<SelectListItem> SpinStateList { get; set; } = [];

	/// <summary>
	/// Gets or sets the description in HTML format.
	/// </summary>
	[DataType(DataType.MultilineText)]
	[Display(Name = "Description")]
	public string? DescriptionHtml { get; set; }

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
	/// <param name="electronicStateMethodFamily">The Electronic State/Method Family Combination full model to associate with the Spin State/Electronic State/Method Family Combination.</param>
	/// <param name="spinState">The Spin State full model to associate with the Spin State/Electronic State/Method Family Combination.</param>
	/// <returns>A new instance of <see cref="SpinStateElectronicStateMethodFamilyFullModel"/> populated with the view model data.</returns>
	public SpinStateElectronicStateMethodFamilyFullModel ToFullModel(ElectronicStateMethodFamilyFullModel electronicStateMethodFamily, SpinStateFullModel? spinState = null)
	{
		return new SpinStateElectronicStateMethodFamilyFullModel
		{
			Id = Id,
			Name = Name,
			Keyword = Keyword,
			ElectronicStateMethodFamily = electronicStateMethodFamily,
			SpinState = spinState,
			DescriptionRtf = RtfConverter.HtmlToRtfConverter(DescriptionHtml),
			DescriptionText = RtfConverter.HtmlToTextConverter(DescriptionHtml),
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
			DescriptionRtf = RtfConverter.HtmlToRtfConverter(DescriptionHtml),
			DescriptionText = RtfConverter.HtmlToTextConverter(DescriptionHtml),
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
			ElectronicStateMethodFamilyId = int.Parse(ElectronicStateMethodFamilyId, CultureInfo.InvariantCulture),
			SpinStateId = string.IsNullOrWhiteSpace(SpinStateId) ? null : int.Parse(SpinStateId, CultureInfo.InvariantCulture),
			DescriptionRtf = RtfConverter.HtmlToRtfConverter(DescriptionHtml),
			DescriptionText = RtfConverter.HtmlToTextConverter(DescriptionHtml),
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
