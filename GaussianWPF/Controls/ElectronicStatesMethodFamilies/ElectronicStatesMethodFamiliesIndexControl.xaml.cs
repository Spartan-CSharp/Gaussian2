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

namespace GaussianWPF.Controls.ElectronicStatesMethodFamilies;

/// <summary>
/// Interaction logic for ElectronicStatesMethodFamiliesIndexControl.xaml
/// </summary>
public partial class ElectronicStatesMethodFamiliesIndexControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<ElectronicStatesMethodFamiliesIndexControl> _logger;
	private readonly IElectronicStatesMethodFamiliesEndpoint _electronicStatesMethodFamiliesEndpoint;
	private readonly IElectronicStatesEndpoint _electronicStatesEndpoint;
	private readonly IMethodFamiliesEndpoint _methodFamiliesEndpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStatesMethodFamiliesIndexControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="electronicStatesMethodFamiliesEndpoint">The endpoint for Electronic State/Method Family Combination API operations.</param>
	/// <param name="electronicStatesEndpoint">The endpoint for Electronic State API operations.</param>
	/// <param name="methodFamiliesEndpoint">The endpoint for Method Family API operations.</param>
	public ElectronicStatesMethodFamiliesIndexControl(ILogger<ElectronicStatesMethodFamiliesIndexControl> logger, IElectronicStatesMethodFamiliesEndpoint electronicStatesMethodFamiliesEndpoint, IElectronicStatesEndpoint electronicStatesEndpoint, IMethodFamiliesEndpoint methodFamiliesEndpoint)
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
	public event EventHandler<ChildControlEventArgs<ElectronicStatesMethodFamiliesIndexControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the collection of Electronic State/Method Family Combinations to display in the list.
	/// </summary>
	public Collection<ElectronicStateMethodFamilyViewModel> ElectronicStatesMethodFamiliesList
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(ElectronicStatesMethodFamiliesList));
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
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(ElectronicStatesMethodFamiliesIndexControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesMethodFamiliesIndexControl), nameof(OnPropertyChanged));
		}
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the OnInitialized event, sets up data binding, and loads the list of Electronic State/Method Family Combinations from the API.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(ElectronicStatesMethodFamiliesIndexControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += ElectronicStatesMethodFamiliesIndexControl_PropertyChanged;

		try
		{
			List<ElectronicStateMethodFamilySimpleModel>? results = _electronicStatesMethodFamiliesEndpoint.GetAllSimpleAsync().Result;
			List<ElectronicStateRecord>? electronicStates = _electronicStatesEndpoint.GetListAsync().Result;
			List<MethodFamilyRecord>? methodFamilies = _methodFamiliesEndpoint.GetListAsync().Result;

			if (results is not null && electronicStates is not null && electronicStates.Count > 0 && methodFamilies is not null && methodFamilies.Count > 0)
			{
				foreach (ElectronicStateMethodFamilySimpleModel item in results)
				{
					ElectronicStateMethodFamilyViewModel model = new(item, electronicStates, methodFamilies);
					ElectronicStatesMethodFamiliesList.Add(model);
				}
			}
			else if (results is not null && electronicStates is not null && electronicStates.Count > 0)
			{
				foreach (ElectronicStateMethodFamilySimpleModel item in results)
				{
					ElectronicStateMethodFamilyViewModel model = new()
					{
						Id = item.Id,
						Name = item.Name,
						Keyword = item.Keyword,
						ElectronicState = electronicStates.First(x => x.Id == item.ElectronicStateId),
						ElectronicStateList = [..electronicStates],
						MethodFamily = null,
						DescriptionRtf = item.DescriptionRtf,
						DescriptionText = item.DescriptionText,
						CreatedDate = item.CreatedDate,
						LastUpdatedDate = item.LastUpdatedDate,
						Archived = item.Archived
					};

					ElectronicStatesMethodFamiliesList.Add(model);
				}
			}
			else if (results is not null && methodFamilies is not null && methodFamilies.Count > 0)
			{
				foreach (ElectronicStateMethodFamilySimpleModel item in results)
				{
					ElectronicStateMethodFamilyViewModel model = new()
					{
						Id = item.Id,
						Name = item.Name,
						Keyword = item.Keyword,
						ElectronicState = null,
						MethodFamily = methodFamilies.FirstOrDefault(x => x.Id == item.MethodFamilyId),
						MethodFamilyList = [..methodFamilies],
						DescriptionRtf = item.DescriptionRtf,
						DescriptionText = item.DescriptionText,
						CreatedDate = item.CreatedDate,
						LastUpdatedDate = item.LastUpdatedDate,
						Archived = item.Archived
					};

					ElectronicStatesMethodFamiliesList.Add(model);
				}
			}
			else if (electronicStates is not null && electronicStates.Count > 0 && methodFamilies is not null && methodFamilies.Count > 0)
			{
				ElectronicStatesMethodFamiliesList.Clear();
			}
			else if (results is not null)
			{
				foreach (ElectronicStateMethodFamilySimpleModel item in results)
				{
					ElectronicStateMethodFamilyViewModel model = new()
					{
						Id = item.Id,
						Name = item.Name,
						Keyword = item.Keyword,
						ElectronicState = null,
						MethodFamily = null,
						DescriptionRtf = item.DescriptionRtf,
						DescriptionText = item.DescriptionText,
						CreatedDate = item.CreatedDate,
						LastUpdatedDate = item.LastUpdatedDate,
						Archived = item.Archived
					};

					ElectronicStatesMethodFamiliesList.Add(model);
				}
			}
			else if (electronicStates is not null && electronicStates.Count > 0)
			{
				ElectronicStatesMethodFamiliesList.Clear();
			}
			else if (methodFamilies is not null && methodFamilies.Count > 0)
			{
				ElectronicStatesMethodFamiliesList.Clear();
			}
			else
			{
				ElectronicStatesMethodFamiliesList.Clear();
			}
		}
		catch (HttpIOException ex)
		{
			ErrorMessage = ex.Message;

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} called with {EventArgs} had an error.", nameof(ElectronicStatesMethodFamiliesIndexControl), nameof(OnInitialized), e);
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
						_logger.LogError(ex, "{UserControl} {EventHandler} called with {EventArgs} had an error.", nameof(ElectronicStatesMethodFamiliesIndexControl), nameof(OnInitialized), e);
					}

					return true;
				}

				// Return false for any other exception types to rethrow them in a new AggregateException
				return false;
			});
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesIndexControl), nameof(OnInitialized));
		}
	}

	private void ElectronicStatesMethodFamiliesIndexControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesIndexControl), nameof(ElectronicStatesMethodFamiliesIndexControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}

		// Nothing to do for change events of ElectronicStateMethodFamilyList and IsErrorVisible.

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesIndexControl), nameof(ElectronicStatesMethodFamiliesIndexControl_PropertyChanged));
		}
	}

	private void CreateNewButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesIndexControl), nameof(CreateNewButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesMethodFamiliesIndexControl>("create", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesIndexControl), nameof(CreateNewButton_Click));
		}
	}

	private void EditButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesIndexControl), nameof(EditButton_Click), sender, e);
		}

		Button? button = sender as Button;

		if (button is not null)
		{
			ElectronicStateMethodFamilyViewModel? itemToEdit = button.DataContext as ElectronicStateMethodFamilyViewModel;

			if (itemToEdit is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesMethodFamiliesIndexControl>("edit", itemToEdit.Id));
			}
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesIndexControl), nameof(EditButton_Click));
		}
	}

	private void DetailsButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesIndexControl), nameof(DetailsButton_Click), sender, e);
		}

		Button? button = sender as Button;

		if (button is not null)
		{
			ElectronicStateMethodFamilyViewModel? itemToView = button.DataContext as ElectronicStateMethodFamilyViewModel;

			if (itemToView is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesMethodFamiliesIndexControl>("details", itemToView.Id));
			}
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesIndexControl), nameof(DetailsButton_Click));
		}
	}

	private void DeleteButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesMethodFamiliesIndexControl), nameof(DeleteButton_Click), sender, e);
		}

		Button? button = sender as Button;

		if (button is not null)
		{
			ElectronicStateMethodFamilyViewModel? itemToDelete = button.DataContext as ElectronicStateMethodFamilyViewModel;

			if (itemToDelete is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesMethodFamiliesIndexControl>("delete", itemToDelete.Id));
			}
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesMethodFamiliesIndexControl), nameof(DeleteButton_Click));
		}
	}
}
