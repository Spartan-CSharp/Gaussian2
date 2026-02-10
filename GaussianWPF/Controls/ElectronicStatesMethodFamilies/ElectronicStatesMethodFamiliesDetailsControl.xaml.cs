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

namespace GaussianWPF.Controls.ElectronicStatesMethodFamilies;

/// <summary>
/// Interaction logic for ElectronicStatesMethodFamiliesDetailsControl.xaml
/// </summary>
public partial class ElectronicStatesMethodFamiliesDetailsControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<ElectronicStatesMethodFamiliesIndexControl> _logger;
	private readonly IElectronicStatesMethodFamiliesEndpoint _electronicStatesMethodFamiliesEndpoint;
	private readonly IElectronicStatesEndpoint _electronicStatesEndpoint;
	private readonly IMethodFamiliesEndpoint _methodFamiliesEndpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStatesMethodFamiliesDetailsControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="electronicStatesMethodFamiliesEndpoint">The endpoint for Electronic State/Method Family Combination API operations.</param>
	/// <param name="electronicStatesEndpoint">The endpoint for Electronic State API operations.</param>
	/// <param name="methodFamiliesEndpoint">The endpoint for Method Family API operations.</param>
	public ElectronicStatesMethodFamiliesDetailsControl(ILogger<ElectronicStatesMethodFamiliesIndexControl> logger, IElectronicStatesMethodFamiliesEndpoint electronicStatesMethodFamiliesEndpoint, IElectronicStatesEndpoint electronicStatesEndpoint, IMethodFamiliesEndpoint methodFamiliesEndpoint)
	{
		_logger = logger;
		_electronicStatesMethodFamiliesEndpoint = electronicStatesMethodFamiliesEndpoint;
		_electronicStatesEndpoint = electronicStatesEndpoint;
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
	public event EventHandler<ChildControlEventArgs<ElectronicStatesMethodFamiliesDetailsControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the Electronic State/Method Family Combination view model being displayed.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically populates the RTF content and updates the <see cref="ModelIsNotNull"/> property.
	/// </remarks>
	public ElectronicStateMethodFamilyViewModel? ElectronicStateMethodFamily
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(ElectronicStateMethodFamily));
		}
	}

	/// <summary>
	/// Gets or sets the identifier of the Electronic State/Method Family Combination to display.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically loads the Electronic State/Method Family Combination data from the API.
	/// </remarks>
	public int ElectronicStateMethodFamilyId
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(ElectronicStateMethodFamilyId));
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether a valid Electronic State/Method Family Combination model is loaded.
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
	/// Gets a value indicating whether no Electronic State/Method Family Combination model is loaded.
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
	/// Raises the <see cref="PropertyChanged"/> event for the specified property.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed. This parameter is optional and can be automatically populated by the caller member name.</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(ElectronicStatesMethodFamiliesDetailsControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesMethodFamiliesDetailsControl), nameof(OnPropertyChanged));
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(ElectronicStatesMethodFamiliesDetailsControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += ElectronicStatesMethodFamiliesDetailsControl_PropertyChanged;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesDetailsControl), nameof(OnInitialized));
		}
	}

	private void ElectronicStatesMethodFamiliesDetailsControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesDetailsControl), nameof(ElectronicStatesMethodFamiliesDetailsControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(ElectronicStateMethodFamily))
		{
			if (ElectronicStateMethodFamily is not null)
			{
				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(ElectronicStateMethodFamily.DescriptionRtf);
				ModelIsNotNull = true;
			}
			else
			{
				DescriptionRichTextBox.Document.Blocks.Clear();
				ModelIsNotNull = false;
			}
		}

		if (e.PropertyName is nameof(ElectronicStateMethodFamilyId))
		{
			try
			{
				if (ElectronicStateMethodFamilyId != 0 && (ElectronicStateMethodFamily is null || ElectronicStateMethodFamily.Id != ElectronicStateMethodFamilyId))
				{
					ElectronicStateMethodFamilyFullModel? results = _electronicStatesMethodFamiliesEndpoint.GetByIdAsync(ElectronicStateMethodFamilyId).Result;
					List<ElectronicStateRecord>? electronicStates = _electronicStatesEndpoint.GetListAsync().Result;
					List<MethodFamilyRecord>? methodFamilies = _methodFamiliesEndpoint.GetListAsync().Result;

					if (results is not null && electronicStates is not null && electronicStates.Count > 0 && methodFamilies is not null && methodFamilies.Count > 0)
					{
						ElectronicStateMethodFamily = new ElectronicStateMethodFamilyViewModel(results, electronicStates, methodFamilies);
						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(ElectronicStateMethodFamily.DescriptionRtf);
						ModelIsNotNull = true;
					}
					else if (results is not null && electronicStates is not null && electronicStates.Count > 0)
					{
						ElectronicStateMethodFamily = new ElectronicStateMethodFamilyViewModel()
						{
							Id = results.Id,
							Name = results.Name,
							Keyword = results.Keyword,
							ElectronicState = electronicStates.First(x => x.Id == results.ElectronicState.Id),
							ElectronicStateList = [.. electronicStates],
							MethodFamily = results.MethodFamily?.ToRecord(),
							DescriptionRtf = results.DescriptionRtf,
							DescriptionText = results.DescriptionText,
							CreatedDate = results.CreatedDate,
							LastUpdatedDate = results.LastUpdatedDate,
							Archived = results.Archived
						};

						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(ElectronicStateMethodFamily.DescriptionRtf);
						ModelIsNotNull = true;
					}
					else if (results is not null && methodFamilies is not null && methodFamilies.Count > 0)
					{
						ElectronicStateMethodFamily = new ElectronicStateMethodFamilyViewModel()
						{
							Id = results.Id,
							Name = results.Name,
							Keyword = results.Keyword,
							ElectronicState = results.ElectronicState.ToRecord(),
							MethodFamily = methodFamilies.FirstOrDefault(x => x.Id == results.MethodFamily?.Id),
							MethodFamilyList = [.. methodFamilies],
							DescriptionRtf = results.DescriptionRtf,
							DescriptionText = results.DescriptionText,
							CreatedDate = results.CreatedDate,
							LastUpdatedDate = results.LastUpdatedDate,
							Archived = results.Archived
						};

						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(ElectronicStateMethodFamily.DescriptionRtf);
						ModelIsNotNull = true;
					}
					else if (electronicStates is not null && electronicStates.Count > 0 && methodFamilies is not null && methodFamilies.Count > 0)
					{
						ElectronicStateMethodFamily = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
					}
					else if (results is not null)
					{
						ElectronicStateMethodFamily = new ElectronicStateMethodFamilyViewModel()
						{
							Id = results.Id,
							Name = results.Name,
							Keyword = results.Keyword,
							ElectronicState = results.ElectronicState.ToRecord(),
							MethodFamily = results.MethodFamily?.ToRecord(),
							DescriptionRtf = results.DescriptionRtf,
							DescriptionText = results.DescriptionText,
							CreatedDate = results.CreatedDate,
							LastUpdatedDate = results.LastUpdatedDate,
							Archived = results.Archived
						};

						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(ElectronicStateMethodFamily.DescriptionRtf);
						ModelIsNotNull = true;
					}
					else if (electronicStates is not null && electronicStates.Count > 0)
					{
						ElectronicStateMethodFamily = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
					}
					else if (methodFamilies is not null && methodFamilies.Count > 0)
					{
						ElectronicStateMethodFamily = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
					}
					else
					{
						ElectronicStateMethodFamily = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
					}
				}
				else if (ElectronicStateMethodFamilyId == 0)
				{
					ElectronicStateMethodFamily = null;
					DescriptionRichTextBox.Document.Blocks.Clear();
					ModelIsNotNull = false;
				}
			}
			catch (HttpIOException ex)
			{
				ErrorMessage = ex.Message;

				if (_logger.IsEnabled(LogLevel.Error))
				{
					_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(ElectronicStatesMethodFamiliesDeleteControl), nameof(ElectronicStatesMethodFamiliesDetailsControl_PropertyChanged), sender, e);
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
							_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(ElectronicStatesMethodFamiliesDeleteControl), nameof(ElectronicStatesMethodFamiliesDetailsControl_PropertyChanged), sender, e);
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
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesDetailsControl), nameof(ElectronicStatesMethodFamiliesDetailsControl_PropertyChanged));
		}
	}

	private void EditButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesDetailsControl), nameof(EditButton_Click), sender, e);
		}

		if (ModelIsNotNull)
		{
			ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesMethodFamiliesDetailsControl>("edit", ElectronicStateMethodFamily!.Id));
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesDetailsControl), nameof(EditButton_Click));
		}
	}

	private void BackToIndexButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesDetailsControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesMethodFamiliesDetailsControl>("index", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesDetailsControl), nameof(BackToIndexButton_Click));
		}
	}
}
