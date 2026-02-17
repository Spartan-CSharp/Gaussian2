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

namespace GaussianWPF.Controls.FullMethods;

/// <summary>
/// Interaction logic for FullMethodsDetailsControl.xaml
/// </summary>
public partial class FullMethodsDetailsControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<FullMethodsIndexControl> _logger;
	private readonly IFullMethodsEndpoint _fullMethodsEndpoint;
	private readonly ISpinStatesElectronicStatesMethodFamiliesEndpoint _spinStatesElectronicStatesMethodFamiliesEndpoint;
	private readonly IBaseMethodsEndpoint _baseMethodsEndpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodsDetailsControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="fullMethodsEndpoint">The endpoint for Full Method API operations.</param>
	/// <param name="spinStatesElectronicStatesMethodFamiliesEndpoint">The endpoint for Spin State/Electronic State/Method Family Combination API operations.</param>
	/// <param name="baseMethodsEndpoint">The endpoint for Base Method API operations.</param>
	public FullMethodsDetailsControl(ILogger<FullMethodsIndexControl> logger, IFullMethodsEndpoint fullMethodsEndpoint, ISpinStatesElectronicStatesMethodFamiliesEndpoint spinStatesElectronicStatesMethodFamiliesEndpoint, IBaseMethodsEndpoint baseMethodsEndpoint)
	{
		_logger = logger;
		_fullMethodsEndpoint = fullMethodsEndpoint;
		_spinStatesElectronicStatesMethodFamiliesEndpoint = spinStatesElectronicStatesMethodFamiliesEndpoint;
		_baseMethodsEndpoint = baseMethodsEndpoint;

		InitializeComponent();
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Occurs when a child control event is raised to request navigation to another view.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<FullMethodsDetailsControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the Full Method view model being displayed.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically populates the RTF content and updates the <see cref="ModelIsNotNull"/> property.
	/// </remarks>
	public FullMethodViewModel? FullMethod
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(FullMethod));
		}
	}

	/// <summary>
	/// Gets or sets the identifier of the Full Method to display.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically loads the Full Method data from the API.
	/// </remarks>
	public int FullMethodId
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(FullMethodId));
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether a valid Full Method model is loaded.
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
	/// Gets a value indicating whether no Full Method model is loaded.
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
	/// This property is automatically set fulld on whether <see cref="ErrorMessage"/> has a value.
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
	/// Raises the <see cref="PropertyChanged"/> event for the specified property.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed. This parameter is optional and can be automatically populated by the caller member name.</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(FullMethodsDetailsControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(FullMethodsDetailsControl), nameof(OnPropertyChanged));
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(FullMethodsDetailsControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += FullMethodsDetailsControl_PropertyChanged;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(FullMethodsDetailsControl), nameof(OnInitialized));
		}
	}

	private void FullMethodsDetailsControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(FullMethodsDetailsControl), nameof(FullMethodsDetailsControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(FullMethod))
		{
			if (FullMethod is not null)
			{
				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(FullMethod.DescriptionRtf);
				ModelIsNotNull = true;
			}
			else
			{
				DescriptionRichTextBox.Document.Blocks.Clear();
				ModelIsNotNull = false;
			}
		}

		if (e.PropertyName is nameof(FullMethodId))
		{
			try
			{
				if (FullMethodId != 0 && (FullMethod is null || FullMethod.Id != FullMethodId))
				{
					FullMethodFullModel? results = _fullMethodsEndpoint.GetByIdAsync(FullMethodId).Result;
					List<SpinStateElectronicStateMethodFamilyRecord>? spinStatesElectronicStatesMethodFamilies = _spinStatesElectronicStatesMethodFamiliesEndpoint.GetListAsync().Result;
					List<BaseMethodRecord>? baseMethods = _baseMethodsEndpoint.GetListAsync().Result;

					if (results is not null && spinStatesElectronicStatesMethodFamilies is not null && spinStatesElectronicStatesMethodFamilies.Count > 0 && baseMethods is not null && baseMethods.Count > 0)
					{
						FullMethod = new FullMethodViewModel(results, spinStatesElectronicStatesMethodFamilies, baseMethods);
						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(FullMethod.DescriptionRtf);
						ModelIsNotNull = true;
					}
					else if (results is not null && spinStatesElectronicStatesMethodFamilies is not null && spinStatesElectronicStatesMethodFamilies.Count > 0)
					{
						FullMethod = new FullMethodViewModel()
						{
							Id = results.Id,
							Keyword = results.Keyword,
							SpinStateElectronicStateMethodFamily = results.SpinStateElectronicStateMethodFamily?.ToRecord(),
							SpinStateElectronicStateMethodFamilyList = [..spinStatesElectronicStatesMethodFamilies],
							BaseMethod = results.BaseMethod?.ToRecord(),
							DescriptionRtf = results.DescriptionRtf,
							DescriptionText = results.DescriptionText,
							CreatedDate = results.CreatedDate,
							LastUpdatedDate = results.LastUpdatedDate,
							Archived = results.Archived
						};

						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(FullMethod.DescriptionRtf);
						ModelIsNotNull = true;
					}
					else if (results is not null && baseMethods is not null && baseMethods.Count > 0)
					{
						FullMethod = new FullMethodViewModel()
						{
							Id = results.Id,
							Keyword = results.Keyword,
							SpinStateElectronicStateMethodFamily = results.SpinStateElectronicStateMethodFamily?.ToRecord(),
							BaseMethod = results.BaseMethod?.ToRecord(),
							BaseMethodList = [..baseMethods],
							DescriptionRtf = results.DescriptionRtf,
							DescriptionText = results.DescriptionText,
							CreatedDate = results.CreatedDate,
							LastUpdatedDate = results.LastUpdatedDate,
							Archived = results.Archived
						};

						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(FullMethod.DescriptionRtf);
						ModelIsNotNull = true;
					}
					else if (spinStatesElectronicStatesMethodFamilies is not null && spinStatesElectronicStatesMethodFamilies.Count > 0 && baseMethods is not null && baseMethods.Count > 0)
					{
						FullMethod = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
					}
					else if (results is not null)
					{
						FullMethod = new FullMethodViewModel()
						{
							Id = results.Id,
							Keyword = results.Keyword,
							SpinStateElectronicStateMethodFamily = results.SpinStateElectronicStateMethodFamily?.ToRecord(),
							BaseMethod = results.BaseMethod?.ToRecord(),
							DescriptionRtf = results.DescriptionRtf,
							DescriptionText = results.DescriptionText,
							CreatedDate = results.CreatedDate,
							LastUpdatedDate = results.LastUpdatedDate,
							Archived = results.Archived
						};

						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(FullMethod.DescriptionRtf);
						ModelIsNotNull = true;
					}
					else if (spinStatesElectronicStatesMethodFamilies is not null && spinStatesElectronicStatesMethodFamilies.Count > 0)
					{
						FullMethod = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
					}
					else if (baseMethods is not null && baseMethods.Count > 0)
					{
						FullMethod = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
					}
					else
					{
						FullMethod = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
					}
				}
				else if (FullMethodId == 0)
				{
					FullMethod = null;
					DescriptionRichTextBox.Document.Blocks.Clear();
					ModelIsNotNull = false;
				}
			}
			catch (HttpIOException ex)
			{
				ErrorMessage = ex.Message;

				if (_logger.IsEnabled(LogLevel.Error))
				{
					_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(FullMethodsDeleteControl), nameof(FullMethodsDetailsControl_PropertyChanged), sender, e);
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
							_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(FullMethodsDeleteControl), nameof(FullMethodsDetailsControl_PropertyChanged), sender, e);
						}

						return true;
					}

					// Return false for any other exception types to rethrow them in a new AggregateException
					return false;
				});
			}
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}

		// Nothing to do for change events of ModelIsNull, ModelIsNotNull, IsErrorVisible and CanSave.

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(FullMethodsDetailsControl), nameof(FullMethodsDetailsControl_PropertyChanged));
		}
	}

	private void EditButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(FullMethodsDetailsControl), nameof(EditButton_Click), sender, e);
		}

		if (ModelIsNotNull)
		{
			ChildControlEvent?.Invoke(this, new ChildControlEventArgs<FullMethodsDetailsControl>("edit", FullMethod!.Id));
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(FullMethodsDetailsControl), nameof(EditButton_Click));
		}
	}

	private void BackToIndexButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(FullMethodsDetailsControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<FullMethodsDetailsControl>("index", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(FullMethodsDetailsControl), nameof(BackToIndexButton_Click));
		}
	}
}
