using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

using GaussianCommonLibrary.Models;

using GaussianWPF.Models;

using GaussianWPFLibrary.DataAccess;
using GaussianWPFLibrary.EventModels;
using GaussianWPFLibrary.Models;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls.SpinStatesElectronicStatesMethodFamilies;

/// <summary>
/// Interaction logic for SpinStatesElectronicStatesMethodFamiliesIndexControl.xaml
/// </summary>
public partial class SpinStatesElectronicStatesMethodFamiliesIndexControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<SpinStatesElectronicStatesMethodFamiliesIndexControl> _logger;
	private readonly ISpinStatesElectronicStatesMethodFamiliesEndpoint _spinStatesElectronicStatesMethodFamiliesEndpoint;
	private readonly IElectronicStatesMethodFamiliesEndpoint _electronicStatesMethodFamiliesEndpoint;
	private readonly ISpinStatesEndpoint _spinStatesEndpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStatesElectronicStatesMethodFamiliesIndexControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="spinStatesElectronicStatesMethodFamiliesEndpoint">The endpoint for Spin State/Electronic State/Method Family Combination API operations.</param>
	/// <param name="electronicStatesMethodFamiliesEndpoint">The endpoint for Electronic State/Method Family Combination API operations.</param>
	/// <param name="spinStatesEndpoint">The endpoint for Spin State API operations.</param>
	public SpinStatesElectronicStatesMethodFamiliesIndexControl(ILogger<SpinStatesElectronicStatesMethodFamiliesIndexControl> logger, ISpinStatesElectronicStatesMethodFamiliesEndpoint spinStatesElectronicStatesMethodFamiliesEndpoint, IElectronicStatesMethodFamiliesEndpoint electronicStatesMethodFamiliesEndpoint, ISpinStatesEndpoint spinStatesEndpoint)
	{
		_logger = logger;
		_spinStatesElectronicStatesMethodFamiliesEndpoint = spinStatesElectronicStatesMethodFamiliesEndpoint;
		_electronicStatesMethodFamiliesEndpoint = electronicStatesMethodFamiliesEndpoint;
		_spinStatesEndpoint = spinStatesEndpoint;

		InitializeComponent();
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Occurs when a child control event is raised to request navigation to another view.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesIndexControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the collection of Spin State/Electronic State/Method Family Combinations to display in the list.
	/// </summary>
	public Collection<SpinStateElectronicStateMethodFamilyViewModel> SpinStatesElectronicStatesMethodFamiliesList
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(SpinStatesElectronicStatesMethodFamiliesList));
			}
		}
	} = [];

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
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl), nameof(OnPropertyChanged));
		}
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the OnInitialized event, sets up data binding, and loads the list of Spin State/Electronic State/Method Family Combinations from the API.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += SpinStatesElectronicStatesMethodFamiliesIndexControl_PropertyChanged;

		try
		{
			List<SpinStateElectronicStateMethodFamilySimpleModel>? results = _spinStatesElectronicStatesMethodFamiliesEndpoint.GetAllSimpleAsync().Result;
			List<ElectronicStateMethodFamilyRecord>? electronicStatesMethodFamilies = _electronicStatesMethodFamiliesEndpoint.GetListAsync().Result;
			List<SpinStateRecord>? spinStates = _spinStatesEndpoint.GetListAsync().Result;

			if (results is not null && electronicStatesMethodFamilies is not null && electronicStatesMethodFamilies.Count > 0 && spinStates is not null && spinStates.Count > 0)
			{
				foreach (SpinStateElectronicStateMethodFamilySimpleModel item in results)
				{
					SpinStateElectronicStateMethodFamilyViewModel model = new(item, electronicStatesMethodFamilies, spinStates);
					SpinStatesElectronicStatesMethodFamiliesList.Add(model);
				}
			}
			else if (results is not null && electronicStatesMethodFamilies is not null && electronicStatesMethodFamilies.Count > 0)
			{
				foreach (SpinStateElectronicStateMethodFamilySimpleModel item in results)
				{
					SpinStateElectronicStateMethodFamilyViewModel model = new()
					{
						Id = item.Id,
						Name = item.Name,
						Keyword = item.Keyword,
						ElectronicStateMethodFamily = electronicStatesMethodFamilies.First(x => x.Id == item.ElectronicStateMethodFamilyId),
						ElectronicStateMethodFamilyList = [..electronicStatesMethodFamilies],
						SpinState = null,
						DescriptionRtf = item.DescriptionRtf,
						DescriptionText = item.DescriptionText,
						CreatedDate = item.CreatedDate,
						LastUpdatedDate = item.LastUpdatedDate,
						Archived = item.Archived
					};

					SpinStatesElectronicStatesMethodFamiliesList.Add(model);
				}
			}
			else if (results is not null && spinStates is not null && spinStates.Count > 0)
			{
				foreach (SpinStateElectronicStateMethodFamilySimpleModel item in results)
				{
					SpinStateElectronicStateMethodFamilyViewModel model = new()
					{
						Id = item.Id,
						Name = item.Name,
						Keyword = item.Keyword,
						ElectronicStateMethodFamily = null,
						SpinState = spinStates.FirstOrDefault(x => x.Id == item.SpinStateId),
						SpinStateList = [..spinStates],
						DescriptionRtf = item.DescriptionRtf,
						DescriptionText = item.DescriptionText,
						CreatedDate = item.CreatedDate,
						LastUpdatedDate = item.LastUpdatedDate,
						Archived = item.Archived
					};

					SpinStatesElectronicStatesMethodFamiliesList.Add(model);
				}
			}
			else if (electronicStatesMethodFamilies is not null && electronicStatesMethodFamilies.Count > 0 && spinStates is not null && spinStates.Count > 0)
			{
				SpinStatesElectronicStatesMethodFamiliesList.Clear();
			}
			else if (results is not null)
			{
				foreach (SpinStateElectronicStateMethodFamilySimpleModel item in results)
				{
					SpinStateElectronicStateMethodFamilyViewModel model = new()
					{
						Id = item.Id,
						Name = item.Name,
						Keyword = item.Keyword,
						ElectronicStateMethodFamily = null,
						SpinState = null,
						DescriptionRtf = item.DescriptionRtf,
						DescriptionText = item.DescriptionText,
						CreatedDate = item.CreatedDate,
						LastUpdatedDate = item.LastUpdatedDate,
						Archived = item.Archived
					};

					SpinStatesElectronicStatesMethodFamiliesList.Add(model);
				}
			}
			else if (electronicStatesMethodFamilies is not null && electronicStatesMethodFamilies.Count > 0)
			{
				SpinStatesElectronicStatesMethodFamiliesList.Clear();
			}
			else if (spinStates is not null && spinStates.Count > 0)
			{
				SpinStatesElectronicStatesMethodFamiliesList.Clear();
			}
			else
			{
				SpinStatesElectronicStatesMethodFamiliesList.Clear();
			}
		}
		catch (HttpIOException ex)
		{
			ErrorMessage = ex.Message;

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} called with {EventArgs} had an error.", nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl), nameof(OnInitialized), e);
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
						_logger.LogError(ex, "{UserControl} {EventHandler} called with {EventArgs} had an error.", nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl), nameof(OnInitialized), e);
					}

					return true;
				}

				// Return false for any other exception types to rethrow them in a new AggregateException
				return false;
			});
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl), nameof(OnInitialized));
		}
	}

	private void SpinStatesElectronicStatesMethodFamiliesIndexControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl), nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}

		// Nothing to do for change events of SpinStateElectronicStateMethodFamilyList and IsErrorVisible.

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl), nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl_PropertyChanged));
		}
	}

	private void CreateNewButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl), nameof(CreateNewButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesIndexControl>("create", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl), nameof(CreateNewButton_Click));
		}
	}

	private void EditButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl), nameof(EditButton_Click), sender, e);
		}

		Button? button = sender as Button;

		if (button is not null)
		{
			SpinStateElectronicStateMethodFamilyViewModel? itemToEdit = button.DataContext as SpinStateElectronicStateMethodFamilyViewModel;

			if (itemToEdit is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesIndexControl>("edit", itemToEdit.Id));
			}
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl), nameof(EditButton_Click));
		}
	}

	private void DetailsButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl), nameof(DetailsButton_Click), sender, e);
		}

		Button? button = sender as Button;

		if (button is not null)
		{
			SpinStateElectronicStateMethodFamilyViewModel? itemToView = button.DataContext as SpinStateElectronicStateMethodFamilyViewModel;

			if (itemToView is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesIndexControl>("details", itemToView.Id));
			}
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl), nameof(DetailsButton_Click));
		}
	}

	private void DeleteButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl), nameof(DeleteButton_Click), sender, e);
		}

		Button? button = sender as Button;

		if (button is not null)
		{
			SpinStateElectronicStateMethodFamilyViewModel? itemToDelete = button.DataContext as SpinStateElectronicStateMethodFamilyViewModel;

			if (itemToDelete is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<SpinStatesElectronicStatesMethodFamiliesIndexControl>("delete", itemToDelete.Id));
			}
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesElectronicStatesMethodFamiliesIndexControl), nameof(DeleteButton_Click));
		}
	}
}
