using System.ComponentModel.DataAnnotations;
using System.Globalization;

using GaussianCommonLibrary.Models;

using GaussianMVCLibrary.Converters;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace GaussianMVC.Models;

/// <summary>
/// View model representing a Base Method for the Gaussian MVC application.
/// Provides data binding and validation for Base Method creation, editing, and display operations.
/// </summary>
public class BaseMethodViewModel
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodViewModel"/> class.
	/// </summary>
	public BaseMethodViewModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodViewModel"/> class from a full model.
	/// </summary>
	/// <param name="model">The Base Method full model containing the source data.</param>
	/// <param name="methodFamilies">The list of available Method Families for the dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="methodFamilies"/> is null.</exception>
	public BaseMethodViewModel(BaseMethodFullModel model, IList<MethodFamilyRecord> methodFamilies)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(methodFamilies, nameof(methodFamilies));
		Id = model.Id;
		Keyword = model.Keyword;
		MethodFamilyId = model.MethodFamily.Id.ToString(CultureInfo.InvariantCulture);
		DescriptionHtml = RtfConverter.RtfToHtmlConverter(model.DescriptionRtf);
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (MethodFamilyRecord item in methodFamilies)
		{
			SelectListItem listItem = new()
			{
				Text = item.Name,
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			if (item.Id == model.MethodFamily.Id)
			{
				listItem.Selected = true;
			}

			MethodFamilyList.Add(listItem);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodViewModel"/> class from an intermediate model.
	/// </summary>
	/// <param name="model">The Base Method intermediate model containing the source data.</param>
	/// <param name="methodFamilies">The list of available Method Families for the dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="methodFamilies"/> is null.</exception>
	public BaseMethodViewModel(BaseMethodIntermediateModel model, IList<MethodFamilyRecord> methodFamilies)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(methodFamilies, nameof(methodFamilies));
		Id = model.Id;
		Keyword = model.Keyword;
		MethodFamilyId = model.MethodFamily.Id.ToString(CultureInfo.InvariantCulture);
		DescriptionHtml = RtfConverter.RtfToHtmlConverter(model.DescriptionRtf);
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (MethodFamilyRecord item in methodFamilies)
		{
			SelectListItem listItem = new()
			{
				Text = item.Name,
				Value = item.Id.ToString(CultureInfo.InvariantCulture),
				Disabled = false
			};

			if (item.Id == model.MethodFamily.Id)
			{
				listItem.Selected = true;
			}

			MethodFamilyList.Add(listItem);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodViewModel"/> class from a simple model.
	/// </summary>
	/// <param name="model">The Base Method simple model containing the source data.</param>
	/// <param name="methodFamilies">The list of available Method Families for the dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="methodFamilies"/> is null.</exception>
	public BaseMethodViewModel(BaseMethodSimpleModel model, IList<MethodFamilyRecord> methodFamilies)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(methodFamilies, nameof(methodFamilies));
		Id = model.Id;
		Keyword = model.Keyword;
		MethodFamilyId = model.MethodFamilyId.ToString(CultureInfo.InvariantCulture);
		DescriptionHtml = RtfConverter.RtfToHtmlConverter(model.DescriptionRtf);
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

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
	/// Initializes a new instance of the <see cref="BaseMethodViewModel"/> class with Method Families for new record creation.
	/// </summary>
	/// <param name="methodFamilies">The list of available Method Families for the dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="methodFamilies"/> is null.</exception>
	public BaseMethodViewModel(IList<MethodFamilyRecord> methodFamilies)
	{
		ArgumentNullException.ThrowIfNull(methodFamilies, nameof(methodFamilies));

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
	/// Gets or sets the unique identifier for the Base Method.
	/// </summary>
	[Display(Name = "Id")]
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the keyword that identifies the Base Method.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Keyword")]
	[MaxLength(50)]
	[Required]
	public string Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the Method Family identifier as a string representation.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Method Family")]
	[Required]
	public string MethodFamilyId { get; set; } = string.Empty;

	/// <summary>
	/// Gets the Method Family name from the selected item in the Method Family list.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Method Family")]
	public string MethodFamilyName
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
	/// Gets or sets the date and time when the Base Method was created.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Created Date")]
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when the Base Method was last updated.
	/// </summary>
	[DataType(DataType.DateTime)]
	[Display(Name = "Last Updated Date")]
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether the Base Method is archived.
	/// </summary>
	[Display(Name = "Archived")]
	public bool Archived { get; set; }

	/// <summary>
	/// Converts this view model to a <see cref="BaseMethodFullModel"/> instance.
	/// </summary>
	/// <param name="methodFamily">The Method Family full model to associate with the Base Method.</param>
	/// <returns>A new instance of <see cref="BaseMethodFullModel"/> populated with the view model data.</returns>
	public BaseMethodFullModel ToFullModel(MethodFamilyFullModel methodFamily)
	{
		return new BaseMethodFullModel
		{
			Id = Id,
			Keyword = Keyword,
			MethodFamily = methodFamily,
			DescriptionRtf = RtfConverter.HtmlToRtfConverter(DescriptionHtml),
			DescriptionText = RtfConverter.HtmlToTextConverter(DescriptionHtml),
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this view model to a <see cref="BaseMethodIntermediateModel"/> instance.
	/// </summary>
	/// <param name="methodFamily">The Method Family record to associate with the Base Method.</param>
	/// <returns>A new instance of <see cref="BaseMethodIntermediateModel"/> populated with the view model data.</returns>
	public BaseMethodIntermediateModel ToIntermediateModel(MethodFamilyRecord methodFamily)
	{
		return new BaseMethodIntermediateModel
		{
			Id = Id,
			Keyword = Keyword,
			MethodFamily = methodFamily,
			DescriptionRtf = RtfConverter.HtmlToRtfConverter(DescriptionHtml),
			DescriptionText = RtfConverter.HtmlToTextConverter(DescriptionHtml),
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts this view model to a <see cref="BaseMethodSimpleModel"/> instance.
	/// </summary>
	/// <returns>A new instance of <see cref="BaseMethodSimpleModel"/> populated with the view model data.</returns>
	public BaseMethodSimpleModel ToSimpleModel()
	{
		return new BaseMethodSimpleModel
		{
			Id = Id,
			Keyword = Keyword,
			MethodFamilyId = int.Parse(MethodFamilyId, CultureInfo.InvariantCulture),
			DescriptionRtf = RtfConverter.HtmlToRtfConverter(DescriptionHtml),
			DescriptionText = RtfConverter.HtmlToTextConverter(DescriptionHtml),
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Returns a string representation of the Base Method.
	/// </summary>
	/// <returns>The keyword of the Base Method.</returns>
	public override string? ToString()
	{
		return $"{Keyword}";
	}
}
