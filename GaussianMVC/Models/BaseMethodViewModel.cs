using System.ComponentModel.DataAnnotations;
using System.Globalization;

using GaussianCommonLibrary.Models;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace GaussianMVC.Models;

/// <summary>
/// Represents a view model for base method data used in Razor Pages views.
/// Provides data binding and validation for base method creation, editing, and display operations.
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
	/// <param name="model">The base method full model containing the source data.</param>
	/// <param name="methodFamilies">The list of available method families for the dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is null or when <paramref name="methodFamilies"/> is null or empty.</exception>
	public BaseMethodViewModel(BaseMethodFullModel model, IList<MethodFamilyRecord> methodFamilies)
	{
		if (model is null)
		{
			throw new ArgumentNullException(nameof(model), $"The parameter {nameof(model)} cannot be null.");
		}

		if (methodFamilies is null || !methodFamilies.Any())
		{
			throw new ArgumentNullException(nameof(methodFamilies), $"The parameter {nameof(methodFamilies)} cannot be null or empty.");
		}

		Id = model.Id;
		Keyword = model.Keyword;
		MethodFamily = model.MethodFamily.Id.ToString(CultureInfo.InvariantCulture);
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
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
	/// Initializes a new instance of the <see cref="BaseMethodViewModel"/> class from a simple model and method family record.
	/// </summary>
	/// <param name="model">The base method simple model containing the source data.</param>
	/// <param name="methodFamily">The method family record associated with the base method.</param>
	/// <param name="methodFamilies">The list of available method families for the dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/>, <paramref name="methodFamily"/>, or <paramref name="methodFamilies"/> is null, or when <paramref name="methodFamilies"/> is empty.</exception>
	public BaseMethodViewModel(BaseMethodSimpleModel model, MethodFamilyRecord methodFamily, IList<MethodFamilyRecord> methodFamilies)
	{
		if (model is null)
		{
			throw new ArgumentNullException(nameof(model), $"The parameter {nameof(model)} cannot be null.");
		}

		if (methodFamily is null)
		{
			throw new ArgumentNullException(nameof(methodFamily), $"The parameter {nameof(methodFamily)} cannot be null.");
		}

		if (methodFamilies is null || !methodFamilies.Any())
		{
			throw new ArgumentNullException(nameof(methodFamilies), $"The parameter {nameof(methodFamilies)} cannot be null or empty.");
		}

		Id = model.Id;
		Keyword = model.Keyword;
		MethodFamily = methodFamily.Id.ToString(CultureInfo.InvariantCulture);
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
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

			if (item.Id == methodFamily.Id)
			{
				listItem.Selected = true;
			}

			MethodFamilyList.Add(listItem);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodViewModel"/> class from a simple model and method family full model.
	/// </summary>
	/// <param name="model">The base method simple model containing the source data.</param>
	/// <param name="methodFamily">The method family full model associated with the base method.</param>
	/// <param name="methodFamilies">The list of available method families for the dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/>, <paramref name="methodFamily"/>, or <paramref name="methodFamilies"/> is null, or when <paramref name="methodFamilies"/> is empty.</exception>
	public BaseMethodViewModel(BaseMethodSimpleModel model, MethodFamilyFullModel methodFamily, IList<MethodFamilyRecord> methodFamilies)
	{
		if (model is null)
		{
			throw new ArgumentNullException(nameof(model), $"The parameter {nameof(model)} cannot be null.");
		}

		if (methodFamily is null)
		{
			throw new ArgumentNullException(nameof(methodFamily), $"The parameter {nameof(methodFamily)} cannot be null.");
		}

		if (methodFamilies is null || !methodFamilies.Any())
		{
			throw new ArgumentNullException(nameof(methodFamilies), $"The parameter {nameof(methodFamilies)} cannot be null or empty.");
		}

		Id = model.Id;
		Keyword = model.Keyword;
		MethodFamily = methodFamily.Id.ToString(CultureInfo.InvariantCulture);
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
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

			if (item.Id == methodFamily.Id)
			{
				listItem.Selected = true;
			}

			MethodFamilyList.Add(listItem);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodViewModel"/> class with method families for new record creation.
	/// </summary>
	/// <param name="methodFamilies">The list of available method families for the dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="methodFamilies"/> is null.</exception>
	public BaseMethodViewModel(IList<MethodFamilyRecord> methodFamilies)
	{
		if (methodFamilies is null)
		{
			throw new ArgumentNullException(nameof(methodFamilies), $"The parameter {nameof(methodFamilies)} cannot be null.");
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
	/// Gets or sets the unique identifier for the base method.
	/// </summary>
	[Display(Name = "Id")]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the keyword that identifies the base method.
	/// Maximum length is 20 characters.
	/// </summary>
	[Display(Name = "Keyword")]
	[Required]
	[MaxLength(20)]
	public string Keyword { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the method family identifier as a string representation.
	/// </summary>
	[Display(Name = "Method Family")]
	[Required]
	public string MethodFamily { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the list of method families available for selection in the view.
	/// </summary>
	public IList<SelectListItem> MethodFamilyList { get; set; } = [];

	/// <summary>
	/// Gets or sets the description in Rich Text Format (RTF).
	/// </summary>
	[Display(Name = "Description (RTF)")]
	public string? DescriptionRtf { get; set; }

	/// <summary>
	/// Gets or sets the description in plain text format.
	/// </summary>
	[Display(Name = "Description (Text)")]
	public string? DescriptionText { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the base method was created.
	/// Defaults to the current date and time.
	/// </summary>
	[Display(Name = "Created Date")]
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets the date and time when the base method was last updated.
	/// Defaults to the current date and time.
	/// </summary>
	[Display(Name = "Last Updated Date")]
	public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

	/// <summary>
	/// Gets or sets a value indicating whether the base method is archived.
	/// </summary>
	[Display(Name = "Archived")]
	public bool Archived { get; set; }

	/// <summary>
	/// Converts the view model to a <see cref="BaseMethodFullModel"/>.
	/// </summary>
	/// <param name="methodFamily">The method family full model to associate with the base method.</param>
	/// <returns>A new instance of <see cref="BaseMethodFullModel"/> populated with the view model data.</returns>
	public BaseMethodFullModel ToFullModel(MethodFamilyFullModel methodFamily)
	{
		return new BaseMethodFullModel
		{
			Id = Id,
			Keyword = Keyword,
			MethodFamily = methodFamily,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Converts the view model to a <see cref="BaseMethodSimpleModel"/>.
	/// </summary>
	/// <returns>A new instance of <see cref="BaseMethodSimpleModel"/> populated with the view model data.</returns>
	public BaseMethodSimpleModel ToSimpleModel()
	{
		return new BaseMethodSimpleModel
		{
			Id = Id,
			Keyword = Keyword,
			MethodFamilyId = int.Parse(MethodFamily, CultureInfo.InvariantCulture),
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
			CreatedDate = CreatedDate,
			LastUpdatedDate = LastUpdatedDate,
			Archived = Archived
		};
	}

	/// <summary>
	/// Returns a string representation of the base method view model.
	/// </summary>
	/// <returns>The keyword of the base method.</returns>
	public override string? ToString()
	{
		return $"{Keyword}";
	}
}
