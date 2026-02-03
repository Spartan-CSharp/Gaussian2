using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

using GaussianCommonLibrary.Models;

using GaussianWPF.Models;
using GaussianWPF.WPFHelpers;

using GaussianWPFLibrary.DataAccess;
using GaussianWPFLibrary.EventModels;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls.BaseMethods;

/// <summary>
/// Interaction logic for BaseMethodsEditControl.xaml
/// </summary>
public partial class BaseMethodsEditControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<BaseMethodsEditControl> _logger;
	private readonly IBaseMethodsEndpoint _baseMethodsEndpoint;
	private readonly IMethodFamiliesEndpoint _methodFamiliesEndpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodsEditControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="baseMethodsEndpoint">The endpoint for base method API operations.</param>
	/// <param name="methodFamiliesEndpoint">The endpoint for method family API operations.</param>
	public BaseMethodsEditControl(ILogger<BaseMethodsEditControl> logger, IBaseMethodsEndpoint baseMethodsEndpoint, IMethodFamiliesEndpoint methodFamiliesEndpoint)
	{
		_logger = logger;
		_baseMethodsEndpoint = baseMethodsEndpoint;
		_methodFamiliesEndpoint = methodFamiliesEndpoint;

		InitializeComponent();
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Occurs when a child control event is raised to request navigation to another view.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<BaseMethodsEditControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the base method view model being edited.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically populates the keyword, RTF content, and updates validation state.
	/// </remarks>
	public BaseMethodViewModel? BaseMethod
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(BaseMethod));
		}
	}

	/// <summary>
	/// Gets or sets the identifier of the base method to edit.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically loads the base method data from the API.
	/// </remarks>
	public int BaseMethodId
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(BaseMethodId));
		}
	}

	/// <summary>
	/// Gets or sets the keyword that identifies the base method.
	/// </summary>
	public string Keyword
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(Keyword));
		}
	} = string.Empty;

	/// <summary>
	/// Gets or sets the selected method family for the base method.
	/// </summary>
	public MethodFamilyRecord? SelectedMethodFamily
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(SelectedMethodFamily));
		}
	}

	/// <summary>
	/// Gets or sets the observable collection of method families available for selection.
	/// </summary>
	/// <remarks>
	/// This collection supports data binding for ComboBox controls in WPF.
	/// </remarks>
	public ObservableCollection<MethodFamilyRecord> MethodFamilyList
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(MethodFamilyList));
		}
	} = [];

	/// <summary>
	/// Gets or sets a value indicating whether a valid base method model is loaded.
	/// </summary>
	/// <remarks>
	/// Setting this property also updates the <see cref="ModelIsNull"/> property.
	/// </remarks>
	public bool ModelIsNotNull
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(ModelIsNotNull));
			OnPropertyChanged(nameof(ModelIsNull));
		}
	}

	/// <summary>
	/// Gets a value indicating whether no base method model is loaded.
	/// </summary>
	/// <remarks>
	/// This property returns the inverse of <see cref="ModelIsNotNull"/>.
	/// </remarks>
	public bool ModelIsNull
	{
		get { return !ModelIsNotNull; }
	}

	/// <summary>
	/// Gets or sets the error message to display when an operation fails.
	/// </summary>
	public string? ErrorMessage
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(ErrorMessage));
			}
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether the error message should be visible.
	/// </summary>
	/// <remarks>
	/// This property is automatically set based on whether <see cref="ErrorMessage"/> has a value.
	/// </remarks>
	public bool IsErrorVisible
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(IsErrorVisible));
			}
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether the save button should be enabled.
	/// </summary>
	/// <remarks>
	/// This property is automatically set to <see langword="true"/> when the keyword, selected method family, and description are valid.
	/// </remarks>
	public bool CanSave
	{
		get;

		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(CanSave));
			}
		}
	}

	/// <summary>
	/// Raises the <see cref="PropertyChanged"/> event for the specified property.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed. This parameter is optional and can be automatically populated by the caller member name.</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(BaseMethodsEditControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(BaseMethodsEditControl), nameof(OnPropertyChanged));
		}
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the OnInitialized event and sets up data binding and property change notifications.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(BaseMethodsEditControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += BaseMethodsEditControl_PropertyChanged;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(BaseMethodsEditControl), nameof(OnInitialized));
		}
	}

	private void BaseMethodsEditControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(BaseMethodsEditControl), nameof(BaseMethodsEditControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(BaseMethod))
		{
			if (BaseMethod is not null)
			{
				Keyword = BaseMethod.Keyword;
				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(BaseMethod.DescriptionRtf);
				ModelIsNotNull = true;
				CanSave = BaseMethod.MethodFamily is not null && BaseMethod.Keyword?.Length is > 0 and <= 20 && (BaseMethod.DescriptionText?.Length is <= 2000 || string.IsNullOrEmpty(BaseMethod.DescriptionText));
			}
			else
			{
				Keyword = string.Empty;
				DescriptionRichTextBox.Document.Blocks.Clear();
				ModelIsNotNull = false;
				CanSave = false;
			}
		}

		if (e.PropertyName is nameof(BaseMethodId))
		{
			try
			{
				if (BaseMethodId != 0 && (BaseMethod is null || BaseMethod.Id != BaseMethodId))
				{
					BaseMethodFullModel? results = _baseMethodsEndpoint.GetByIdAsync(BaseMethodId).Result;
					List<MethodFamilyRecord>? methodFamilies = _methodFamiliesEndpoint.GetListAsync().Result;

					if (results is not null && methodFamilies is not null && methodFamilies.Count > 0)
					{
						MethodFamilyList.Clear();

						foreach (MethodFamilyRecord item in methodFamilies)
						{
							MethodFamilyList.Add(item);
						}

						SelectedMethodFamily = MethodFamilyList.FirstOrDefault(mf => mf.Id == results.MethodFamily.Id);
						BaseMethod = new BaseMethodViewModel(results, methodFamilies);
						Keyword = BaseMethod.Keyword;
						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(BaseMethod.DescriptionRtf);
						ModelIsNotNull = true;
						CanSave = BaseMethod.MethodFamily is not null && BaseMethod.Keyword?.Length is > 0 and <= 20 && (BaseMethod.DescriptionText?.Length is <= 2000 || string.IsNullOrEmpty(BaseMethod.DescriptionText));
					}
					else if (results is not null)
					{
						SelectedMethodFamily = MethodFamilyList.FirstOrDefault(mf => mf.Id == results.MethodFamily.Id);
						List<MethodFamilyRecord> methodFamilyList = [.. MethodFamilyList];
						BaseMethod = new BaseMethodViewModel(results, methodFamilyList);
						Keyword = BaseMethod.Keyword ?? string.Empty;
						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(BaseMethod.DescriptionRtf);
						ModelIsNotNull = true;
						CanSave = BaseMethod.MethodFamily is not null && BaseMethod.Keyword?.Length is > 0 and <= 20 && (BaseMethod.DescriptionText?.Length is <= 2000 || string.IsNullOrEmpty(BaseMethod.DescriptionText));
					}
					else if (methodFamilies is not null && methodFamilies.Count > 0)
					{
						MethodFamilyList.Clear();

						foreach (MethodFamilyRecord item in methodFamilies)
						{
							MethodFamilyList.Add(item);
						}

						BaseMethod = null;
						Keyword = string.Empty;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
						CanSave = false;
					}
					else
					{
						MethodFamilyList.Clear();
						BaseMethod = null;
						Keyword = string.Empty;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
						CanSave = false;
					}
				}
				else if (BaseMethodId == 0)
				{
					BaseMethod = null;
					Keyword = string.Empty;
					DescriptionRichTextBox.Document.Blocks.Clear();
					ModelIsNotNull = false;
					CanSave = false;
				}
			}
			catch (HttpIOException ex)
			{
				ErrorMessage = ex.Message;

				if (_logger.IsEnabled(LogLevel.Error))
				{
					_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(BaseMethodsEditControl), nameof(BaseMethodsEditControl_PropertyChanged), sender, e);
				}
			}
			catch (AggregateException ae)
			{
				ae.Handle(ex =>
				{
					if (ex is HttpIOException)
					{
						ErrorMessage = ex.Message;

						if (_logger.IsEnabled(LogLevel.Error))
						{
							_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(BaseMethodsEditControl), nameof(BaseMethodsEditControl_PropertyChanged), sender, e);
						}

						return true;
					}

					// Return false for any other exception types to rethrow them in a new AggregateException
					return false;
				});
			}
		}

		if (e.PropertyName is (nameof(Keyword)) or (nameof(SelectedMethodFamily)))
		{
			CanSave = SelectedMethodFamily is not null && Keyword?.Length is > 0 and <= 20;
		}

		if (e.PropertyName is nameof(MethodFamilyList))
		{
			BaseMethod?.MethodFamily = null;
			BaseMethod?.MethodFamilyList = MethodFamilyList;
			SelectedMethodFamily = null;
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}

		// Nothing to do for change events of ModelIsNull, ModelIsNotNull, IsErrorVisible and CanSave.

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(BaseMethodsEditControl), nameof(BaseMethodsEditControl_PropertyChanged));
		}
	}

	private void SaveButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(BaseMethodsEditControl), nameof(SaveButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;
			BaseMethod!.Keyword = Keyword;
			BaseMethod!.DescriptionRtf = DescriptionRichTextBox.GetRtfText();
			BaseMethod!.DescriptionText = DescriptionRichTextBox.GetPlainText();
			BaseMethodSimpleModel model = BaseMethod!.ToSimpleModel();
			BaseMethodFullModel? result = _baseMethodsEndpoint.UpdateAsync(BaseMethodId, model).Result;

			if (result is not null)
			{
				BaseMethod.LastUpdatedDate = result.LastUpdatedDate;
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<BaseMethodsEditControl>("save", null));
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(BaseMethodsEditControl), nameof(SaveButton_Click));
			}
		}
		catch (HttpIOException ex)
		{
			ErrorMessage = ex.Message;

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(BaseMethodsEditControl), nameof(SaveButton_Click), sender, e);
			}
		}
		catch (AggregateException ae)
		{
			ae.Handle(ex =>
			{
				if (ex is HttpIOException)
				{
					ErrorMessage = ex.Message;

					if (_logger.IsEnabled(LogLevel.Error))
					{
						_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(BaseMethodsEditControl), nameof(SaveButton_Click), sender, e);
					}

					return true;
				}

				// Return false for any other exception types to rethrow them in a new AggregateException
				return false;
			});
		}
	}

	private void BackToIndexButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(BaseMethodsEditControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<BaseMethodsEditControl>("index", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(BaseMethodsEditControl), nameof(BackToIndexButton_Click));
		}
	}

	private void BoldButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(BaseMethodsEditControl), nameof(BoldButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleFontWeight();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(BaseMethodsEditControl), nameof(BoldButton_Click));
		}
	}

	private void ItalicButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(BaseMethodsEditControl), nameof(ItalicButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleFontStyle();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(BaseMethodsEditControl), nameof(ItalicButton_Click));
		}
	}

	private void UnderlineButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(BaseMethodsEditControl), nameof(UnderlineButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleUnderline();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(BaseMethodsEditControl), nameof(UnderlineButton_Click));
		}
	}

	private void SubscriptButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(BaseMethodsEditControl), nameof(SubscriptButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleBaselineAlignment(BaselineAlignment.Subscript);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(BaseMethodsEditControl), nameof(SubscriptButton_Click));
		}
	}

	private void SuperscriptButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(BaseMethodsEditControl), nameof(SuperscriptButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleBaselineAlignment(BaselineAlignment.Superscript);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(BaseMethodsEditControl), nameof(SuperscriptButton_Click));
		}
	}

	private void BulletsButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(BaseMethodsEditControl), nameof(BulletsButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleList(TextMarkerStyle.Disc);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(BaseMethodsEditControl), nameof(BulletsButton_Click));
		}
	}

	private void NumberingButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(BaseMethodsEditControl), nameof(NumberingButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleList(TextMarkerStyle.Decimal);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(BaseMethodsEditControl), nameof(NumberingButton_Click));
		}
	}
}
