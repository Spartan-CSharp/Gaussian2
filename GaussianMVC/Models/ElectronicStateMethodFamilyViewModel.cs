using System.ComponentModel.DataAnnotations;
using System.Globalization;

using GaussianCommonLibrary.Models;

using GaussianMVC.ValidationAttributes;

using GaussianMVCLibrary.Converters;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace GaussianMVC.Models;

/// <summary>
/// View model representing a Electronic State/Method Family Combination for the Gaussian MVC application.
/// Provides data binding and validation for Electronic State/Method Family Combination creation, editing, and display operations.
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
	/// <param name="electronicStates">The list of available Electronic States for the dropdown selection.</param>
	/// <param name="methodFamilies">The list of available Method Families for the dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicStates"/> or <paramref name="methodFamilies"/> is null.</exception>
	public ElectronicStateMethodFamilyViewModel(ElectronicStateMethodFamilyFullModel model, IList<ElectronicStateRecord> electronicStates, IList<MethodFamilyRecord> methodFamilies)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(electronicStates, nameof(electronicStates));
		ArgumentNullException.ThrowIfNull(methodFamilies, nameof(methodFamilies));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateId = model.ElectronicState.Id.ToString(CultureInfo.InvariantCulture);
		MethodFamilyId = model.MethodFamily?.Id.ToString(CultureInfo.InvariantCulture);
		DescriptionHtml = RtfConverter.RtfToHtmlConverter(model.DescriptionRtf);
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (ElectronicStateRecord item in electronicStates)
		{
			SelectListItem listItem = new()
			{
				Text = string.IsNullOrWhiteSpace(item.Name) ? $"{item.Keyword}" : string.IsNullOrWhiteSpace(item.Keyword) ? $"{item.Name}" : $"{item.Name}/{item.Keyword}",
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			if (item.Id == model.ElectronicState.Id)
			{
				listItem.Selected = true;
			}

			ElectronicStateList.Add(listItem);
		}

		foreach (MethodFamilyRecord item in methodFamilies)
		{
			SelectListItem listItem = new()
			{
				Text = item.Name,
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			if (item.Id == model.MethodFamily?.Id)
			{
				listItem.Selected = true;
			}

			MethodFamilyList.Add(listItem);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyViewModel"/> class from an intermediate model.
	/// </summary>
	/// <param name="model">The Electronic State/Method Family Combination intermediate model containing the source data.</param>
	/// <param name="electronicStates">The list of available Electronic States for the dropdown selection.</param>
	/// <param name="methodFamilies">The list of available Method Families for the dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicStates"/> or <paramref name="methodFamilies"/> is null.</exception>
	public ElectronicStateMethodFamilyViewModel(ElectronicStateMethodFamilyIntermediateModel model, IList<ElectronicStateRecord> electronicStates, IList<MethodFamilyRecord> methodFamilies)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(electronicStates, nameof(electronicStates));
		ArgumentNullException.ThrowIfNull(methodFamilies, nameof(methodFamilies));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateId = model.ElectronicState.Id.ToString(CultureInfo.InvariantCulture);
		MethodFamilyId = model.MethodFamily?.Id.ToString(CultureInfo.InvariantCulture);
		DescriptionHtml = RtfConverter.RtfToHtmlConverter(model.DescriptionRtf);
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (ElectronicStateRecord item in electronicStates)
		{
			SelectListItem listItem = new()
			{
				Text = string.IsNullOrWhiteSpace(item.Name) ? $"{item.Keyword}" : string.IsNullOrWhiteSpace(item.Keyword) ? $"{item.Name}" : $"{item.Name}/{item.Keyword}",
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			if (item.Id == model.ElectronicState.Id)
			{
				listItem.Selected = true;
			}

			ElectronicStateList.Add(listItem);
		}

		foreach (MethodFamilyRecord item in methodFamilies)
		{
			SelectListItem listItem = new()
			{
				Text = item.Name,
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			if (item.Id == model.MethodFamily?.Id)
			{
				listItem.Selected = true;
			}

			MethodFamilyList.Add(listItem);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyViewModel"/> class from a simple model.
	/// </summary>
	/// <param name="model">The Electronic State/Method Family Combination simple model containing the source data.</param>
	/// <param name="electronicStates">The list of available Electronic States for the dropdown selection.</param>
	/// <param name="methodFamilies">The list of available Method Families for the dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="electronicStates"/> or <paramref name="methodFamilies"/> is null.</exception>
	public ElectronicStateMethodFamilyViewModel(ElectronicStateMethodFamilySimpleModel model, IList<ElectronicStateRecord> electronicStates, IList<MethodFamilyRecord> methodFamilies)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(electronicStates, nameof(electronicStates));
		ArgumentNullException.ThrowIfNull(methodFamilies, nameof(methodFamilies));
		Id = model.Id;
		Name = model.Name;
		Keyword = model.Keyword;
		ElectronicStateId = model.ElectronicStateId.ToString(CultureInfo.InvariantCulture);
		MethodFamilyId = model.MethodFamilyId?.ToString(CultureInfo.InvariantCulture);
		DescriptionHtml = RtfConverter.RtfToHtmlConverter(model.DescriptionRtf);
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (ElectronicStateRecord item in electronicStates)
		{
			SelectListItem listItem = new()
			{
				Text = string.IsNullOrWhiteSpace(item.Name) ? $"{item.Keyword}" : string.IsNullOrWhiteSpace(item.Keyword) ? $"{item.Name}" : $"{item.Name}/{item.Keyword}",
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			if (item.Id == model.ElectronicStateId)
			{
				listItem.Selected = true;
			}

			ElectronicStateList.Add(listItem);
		}

		foreach (MethodFamilyRecord item in methodFamilies)
		{
			SelectListItem listItem = new()
			{
				Text = item.Name,
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			if (item.Id == model.MethodFamilyId)
			{
				listItem.Selected = true;
			}

			MethodFamilyList.Add(listItem);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStateMethodFamilyViewModel"/> class with Electronic States and Method Families for new record creation.
	/// </summary>
	/// <param name="electronicStates">The list of available Electronic States for the dropdown selection.</param>
	/// <param name="methodFamilies">The list of available Method Families for the dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="methodFamilies"/> is null.</exception>
	public ElectronicStateMethodFamilyViewModel(IList<ElectronicStateRecord> electronicStates, IList<MethodFamilyRecord> methodFamilies)
	{
		ArgumentNullException.ThrowIfNull(electronicStates, nameof(electronicStates));
		ArgumentNullException.ThrowIfNull(methodFamilies, nameof(methodFamilies));

		foreach (ElectronicStateRecord item in electronicStates)
		{
			SelectListItem listItem = new()
			{
				Text = string.IsNullOrWhiteSpace(item.Name) ? $"{item.Keyword}" : string.IsNullOrWhiteSpace(item.Keyword) ? $"{item.Name}" : $"{item.Name}/{item.Keyword}",
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			ElectronicStateList.Add(listItem);
		}

		foreach (MethodFamilyRecord item in methodFamilies)
		{
			SelectListItem listItem = new()
			{
				Text = item.Name,
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			MethodFamilyList.Add(listItem);
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
	[MaxLength(200)]
	[RequireAtLeastOne(nameof(Keyword), ErrorMessage = "Either Name or Keyword must be provided.")]
	public string? Name { get; set; }

	/// <summary>
	/// Gets or sets the keyword that identifies the Electronic State/Method Family Combination.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Keyword")]
	[MaxLength(50)]
	[RequireAtLeastOne(nameof(Name), ErrorMessage = "Either Name or Keyword must be provided.")]
	public string? Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the Electronic State identifier as a string representation.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Electronic State")]
	[Required]
	public string ElectronicStateId { get; set; } = string.Empty;

	/// <summary>
	/// Gets the Electronic State name from the selected item in the Electronic State list.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Electronic State")]
	public string ElectronicStateName
	{
		get
		{
			SelectListItem? output = ElectronicStateList.FirstOrDefault(m => m.Value == ElectronicStateId || m.Selected);
			return output?.Text ?? string.Empty;
		}
	}

	/// <summary>
	/// Gets or sets the list of Electronic States available for selection in the view.
	/// </summary>
	public IList<SelectListItem> ElectronicStateList { get; set; } = [];

	/// <summary>
	/// Gets or sets the Method Family identifier as a string representation.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Method Family")]
	public string? MethodFamilyId { get; set; } = string.Empty;

	/// <summary>
	/// Gets the Method Family name from the selected item in the Method Family list.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Method Family")]
	public string? MethodFamilyName
	{
		get 
		{
			SelectListItem? output = MethodFamilyList.FirstOrDefault(m => m.Value == MethodFamilyId || m.Selected);
			return output?.Text ?? string.Empty;
		}
	}

	/// <summary>
	/// Gets or sets the list of Method Families available for selection in the view.
	/// </summary>
	public IList<SelectListItem> MethodFamilyList { get; set; } = [];

	/// <summary>
	/// Gets or sets the description in HTML format.
	/// </summary>
	[DataType(DataType.MultilineText)]
	[Display(Name = "Description")]
	public string? DescriptionHtml { get; set; }

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
			DescriptionRtf = RtfConverter.HtmlToRtfConverter(DescriptionHtml),
			DescriptionText = RtfConverter.HtmlToTextConverter(DescriptionHtml),
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
			DescriptionRtf = RtfConverter.HtmlToRtfConverter(DescriptionHtml),
			DescriptionText = RtfConverter.HtmlToTextConverter(DescriptionHtml),
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
			ElectronicStateId = int.Parse(ElectronicStateId, CultureInfo.InvariantCulture),
			MethodFamilyId = string.IsNullOrWhiteSpace(MethodFamilyId) ? null : int.Parse(MethodFamilyId, CultureInfo.InvariantCulture),
			DescriptionRtf = RtfConverter.HtmlToRtfConverter(DescriptionHtml),
			DescriptionText = RtfConverter.HtmlToTextConverter(DescriptionHtml),
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this view model to a <see cref="ElectronicStateMethodFamilyRecord"/> instance.
	/// </summary>
	/// <returns>A new <see cref="ElectronicStateMethodFamilyRecord"/> containing the Id, Name, and Keyword properties.</returns>
	public ElectronicStateMethodFamilyRecord ToRecord()
	{
		return new ElectronicStateMethodFamilyRecord(Id, Name, Keyword);
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
