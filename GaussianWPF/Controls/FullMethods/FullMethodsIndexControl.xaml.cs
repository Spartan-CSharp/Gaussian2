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

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls.FullMethods;

/// <summary>
/// Interaction logic for FullMethodsIndexControl.xaml
/// </summary>
public partial class FullMethodsIndexControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<FullMethodsIndexControl> _logger;
	private readonly IFullMethodsEndpoint _fullMethodsEndpoint;
	private readonly ISpinStatesElectronicStatesMethodFamiliesEndpoint _spinStatesElectronicStatesMethodFamiliesEndpoint;
	private readonly IBaseMethodsEndpoint _baseMethodsEndpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="FullMethodsIndexControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="fullMethodsEndpoint">The endpoint for Full Method API operations.</param>
	/// <param name="spinStatesElectronicStatesMethodFamiliesEndpoint">The endpoint for Spin State/Electronic State/Method Family Combination API operations.</param>
	/// <param name="baseMethodsEndpoint">The endpoint for Base Method API operations.</param>
	public FullMethodsIndexControl(ILogger<FullMethodsIndexControl> logger, IFullMethodsEndpoint fullMethodsEndpoint, ISpinStatesElectronicStatesMethodFamiliesEndpoint spinStatesElectronicStatesMethodFamiliesEndpoint, IBaseMethodsEndpoint baseMethodsEndpoint)
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
	public event EventHandler<ChildControlEventArgs<FullMethodsIndexControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the collection of Full Methods to display in the list.
	/// </summary>
	public Collection<FullMethodViewModel> FullMethodsList
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(FullMethodsList));
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
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(FullMethodsIndexControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(FullMethodsIndexControl), nameof(OnPropertyChanged));
		}
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the OnInitialized event, sets up data binding, and loads the list of Full Methods from the API.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(FullMethodsIndexControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += FullMethodsIndexControl_PropertyChanged;

		try
		{
			List<FullMethodSimpleModel>? results = _fullMethodsEndpoint.GetAllSimpleAsync().Result;
			List<SpinStateElectronicStateMethodFamilyRecord>? spinStatesElectronicStatesMethodFamilies = _spinStatesElectronicStatesMethodFamiliesEndpoint.GetListAsync().Result;
			List<BaseMethodRecord>? baseMethods = _baseMethodsEndpoint.GetListAsync().Result;

			if (results is not null && spinStatesElectronicStatesMethodFamilies is not null && spinStatesElectronicStatesMethodFamilies.Count > 0 && baseMethods is not null && baseMethods.Count > 0)
			{
				foreach (FullMethodSimpleModel item in results)
				{
					FullMethodViewModel model = new(item, spinStatesElectronicStatesMethodFamilies, baseMethods);
					FullMethodsList.Add(model);
				}
			}
			else if (results is not null && spinStatesElectronicStatesMethodFamilies is not null && spinStatesElectronicStatesMethodFamilies.Count > 0)
			{
				foreach (FullMethodSimpleModel item in results)
				{
					FullMethodViewModel model = new()
					{
						Id = item.Id,
						Keyword = item.Keyword,
						SpinStateElectronicStateMethodFamily = spinStatesElectronicStatesMethodFamilies.First(x => x.Id == item.SpinStateElectronicStateMethodFamilyId),
						SpinStateElectronicStateMethodFamilyList = [..spinStatesElectronicStatesMethodFamilies],
						BaseMethod = null,
						DescriptionRtf = item.DescriptionRtf,
						DescriptionText = item.DescriptionText,
						CreatedDate = item.CreatedDate,
						LastUpdatedDate = item.LastUpdatedDate,
						Archived = item.Archived
					};

					FullMethodsList.Add(model);
				}
			}
			else if (results is not null && baseMethods is not null && baseMethods.Count > 0)
			{
				foreach (FullMethodSimpleModel item in results)
				{
					FullMethodViewModel model = new()
					{
						Id = item.Id,
						Keyword = item.Keyword,
						SpinStateElectronicStateMethodFamily = null,
						BaseMethod = baseMethods.First(x => x.Id == item.BaseMethodId),
						BaseMethodList = [..baseMethods],
						DescriptionRtf = item.DescriptionRtf,
						DescriptionText = item.DescriptionText,
						CreatedDate = item.CreatedDate,
						LastUpdatedDate = item.LastUpdatedDate,
						Archived = item.Archived
					};

					FullMethodsList.Add(model);
				}
			}
			else if (spinStatesElectronicStatesMethodFamilies is not null && spinStatesElectronicStatesMethodFamilies.Count > 0 && baseMethods is not null && baseMethods.Count > 0)
			{
				FullMethodsList.Clear();
			}
			else if (results is not null)
			{
				foreach (FullMethodSimpleModel item in results)
				{
					FullMethodViewModel model = new()
					{
						Id = item.Id,
						Keyword = item.Keyword,
						SpinStateElectronicStateMethodFamily = null,
						BaseMethod = null,
						DescriptionRtf = item.DescriptionRtf,
						DescriptionText = item.DescriptionText,
						CreatedDate = item.CreatedDate,
						LastUpdatedDate = item.LastUpdatedDate,
						Archived = item.Archived
					};

					FullMethodsList.Add(model);
				}
			}
			else if (spinStatesElectronicStatesMethodFamilies is not null && spinStatesElectronicStatesMethodFamilies.Count > 0)
			{
				FullMethodsList.Clear();
			}
			else if (baseMethods is not null && baseMethods.Count > 0)
			{
				FullMethodsList.Clear();
			}
			else
			{
				FullMethodsList.Clear();
			}
		}
		catch (HttpIOException ex)
		{
			ErrorMessage = ex.Message;

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} called with {EventArgs} had an error.", nameof(FullMethodsIndexControl), nameof(OnInitialized), e);
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
						_logger.LogError(ex, "{UserControl} {EventHandler} called with {EventArgs} had an error.", nameof(FullMethodsIndexControl), nameof(OnInitialized), e);
					}

					return true;
				}

				// Return false for any other exception types to rethrow them in a new AggregateException
				return false;
			});
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(FullMethodsIndexControl), nameof(OnInitialized));
		}
	}

	private void FullMethodsIndexControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(FullMethodsIndexControl), nameof(FullMethodsIndexControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}

		// Nothing to do for change events of FullMethodList and IsErrorVisible.

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(FullMethodsIndexControl), nameof(FullMethodsIndexControl_PropertyChanged));
		}
	}

	private void CreateNewButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(FullMethodsIndexControl), nameof(CreateNewButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<FullMethodsIndexControl>("create", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(FullMethodsIndexControl), nameof(CreateNewButton_Click));
		}
	}

	private void EditButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(FullMethodsIndexControl), nameof(EditButton_Click), sender, e);
		}

		Button? button = sender as Button;

		if (button is not null)
		{
			FullMethodViewModel? itemToEdit = button.DataContext as FullMethodViewModel;

			if (itemToEdit is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<FullMethodsIndexControl>("edit", itemToEdit.Id));
			}
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(FullMethodsIndexControl), nameof(EditButton_Click));
		}
	}

	private void DetailsButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(FullMethodsIndexControl), nameof(DetailsButton_Click), sender, e);
		}

		Button? button = sender as Button;

		if (button is not null)
		{
			FullMethodViewModel? itemToView = button.DataContext as FullMethodViewModel;

			if (itemToView is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<FullMethodsIndexControl>("details", itemToView.Id));
			}
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(FullMethodsIndexControl), nameof(DetailsButton_Click));
		}
	}

	private void DeleteButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(FullMethodsIndexControl), nameof(DeleteButton_Click), sender, e);
		}

		Button? button = sender as Button;

		if (button is not null)
		{
			FullMethodViewModel? itemToDelete = button.DataContext as FullMethodViewModel;

			if (itemToDelete is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<FullMethodsIndexControl>("delete", itemToDelete.Id));
			}
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(FullMethodsIndexControl), nameof(DeleteButton_Click));
		}
	}
}
