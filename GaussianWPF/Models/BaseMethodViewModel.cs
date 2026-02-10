using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

using GaussianCommonLibrary.Models;

namespace GaussianWPF.Models;

/// <summary>
/// View model representing a Base Method for the Gaussian WPF application.
/// Provides data binding and validation for Base Method creation, editing, and display operations in the desktop client.
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
	/// <param name="methodFamilies">The list of available Method Families for dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="methodFamilies"/> is null.</exception>
	public BaseMethodViewModel(BaseMethodFullModel model, IList<MethodFamilyRecord> methodFamilies)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(methodFamilies, nameof(methodFamilies));
		Id = model.Id;
		Keyword = model.Keyword;
		MethodFamily = methodFamilies.First(x => x.Id == model.MethodFamily.Id);
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (MethodFamilyRecord item in methodFamilies)
		{
			MethodFamilyList.Add(item);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodViewModel"/> class from an intermediate model.
	/// </summary>
	/// <param name="model">The Base Method intermediate model containing the source data.</param>
	/// <param name="methodFamilies">The list of available Method Families for dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="methodFamilies"/> is null.</exception>
	public BaseMethodViewModel(BaseMethodIntermediateModel model, IList<MethodFamilyRecord> methodFamilies)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(methodFamilies, nameof(methodFamilies));
		Id = model.Id;
		Keyword = model.Keyword;
		MethodFamily = model.MethodFamily;
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (MethodFamilyRecord item in methodFamilies)
		{
			MethodFamilyList.Add(item);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodViewModel"/> class from a simple model.
	/// </summary>
	/// <param name="model">The Base Method simple model containing the source data.</param>
	/// <param name="methodFamilies">The list of available Method Families for dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> or <paramref name="methodFamilies"/> is null.</exception>
	public BaseMethodViewModel(BaseMethodSimpleModel model, IList<MethodFamilyRecord> methodFamilies)
	{
		ArgumentNullException.ThrowIfNull(model, nameof(model));
		ArgumentNullException.ThrowIfNull(methodFamilies, nameof(methodFamilies));
		Id = model.Id;
		Keyword = model.Keyword;
		MethodFamily = methodFamilies.First(x => x.Id == model.MethodFamilyId);
		DescriptionRtf = model.DescriptionRtf;
		DescriptionText = model.DescriptionText;
		CreatedDate = model.CreatedDate;
		LastUpdatedDate = model.LastUpdatedDate;
		Archived = model.Archived;

		foreach (MethodFamilyRecord item in methodFamilies)
		{
			MethodFamilyList.Add(item);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodViewModel"/> class with Method Families for new record creation.
	/// </summary>
	/// <param name="methodFamilies">The list of available Method Families for dropdown selection.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="methodFamilies"/> is null.</exception>
	public BaseMethodViewModel(IList<MethodFamilyRecord> methodFamilies)
	{
		ArgumentNullException.ThrowIfNull(methodFamilies, nameof(methodFamilies));

		foreach (MethodFamilyRecord item in methodFamilies)
		{
			MethodFamilyList.Add(item);
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
	/// Gets or sets the Method Family associated with this Base Method.
	/// </summary>
	[Display(Name = "Method Family")]
	[Required]
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
	[MaxLength(4000)]
	public string? DescriptionText { get; set; }

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
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
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
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
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
			MethodFamilyId = MethodFamily!.Id,
			DescriptionRtf = DescriptionRtf,
			DescriptionText = DescriptionText,
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
