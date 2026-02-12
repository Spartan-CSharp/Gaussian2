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

namespace GaussianWPF.Controls.ElectronicStates;

/// <summary>
/// Interaction logic for ElectronicStatesIndexControl.xaml
/// </summary>
public partial class ElectronicStatesIndexControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<ElectronicStatesIndexControl> _logger;
	private readonly IElectronicStatesEndpoint _endpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStatesIndexControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="endpoint">The endpoint for Electronic State API operations.</param>
	public ElectronicStatesIndexControl(ILogger<ElectronicStatesIndexControl> logger, IElectronicStatesEndpoint endpoint)
	{
		_logger = logger;
		_endpoint = endpoint;

		InitializeComponent();
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Occurs when a child control event is raised to request navigation to another view.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<ElectronicStatesIndexControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the collection of Electronic States to display in the list.
	/// </summary>
	public Collection<ElectronicStateViewModel> ElectronicStatesList
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(ElectronicStatesList));
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
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(ElectronicStatesIndexControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesIndexControl), nameof(OnPropertyChanged));
		}
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the OnInitialized event, sets up data binding, and loads the list of Electronic States from the API.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(ElectronicStatesIndexControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += ElectronicStatesIndexControl_PropertyChanged;

		try
		{
			List<ElectronicStateFullModel>? results = _endpoint.GetAllAsync().Result;

			if (results is not null)
			{
				foreach (ElectronicStateFullModel item in results)
				{
					ElectronicStateViewModel model = new(item);
					ElectronicStatesList.Add(model);
				}
			}
		}
		catch (HttpIOException ex)
		{
			ErrorMessage = ex.Message;

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} called with {EventArgs} had an error.", nameof(ElectronicStatesIndexControl), nameof(OnInitialized), e);
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
						_logger.LogError(ex, "{UserControl} {EventHandler} called with {EventArgs} had an error.", nameof(ElectronicStatesIndexControl), nameof(OnInitialized), e);
					}

					return true;
				}

				// Return false for any other exception types to rethrow them in a new AggregateException
				return false;
			});
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesIndexControl), nameof(OnInitialized));
		}
	}

	private void ElectronicStatesIndexControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesIndexControl), nameof(ElectronicStatesIndexControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}

		// Nothing to do for change events of ElectronicStateMethodFamilyList and IsErrorVisible.

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesIndexControl), nameof(ElectronicStatesIndexControl_PropertyChanged));
		}
	}

	private void CreateNewButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesIndexControl), nameof(CreateNewButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesIndexControl>("create", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesIndexControl), nameof(CreateNewButton_Click));
		}
	}

	private void EditButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesIndexControl), nameof(EditButton_Click), sender, e);
		}

		Button? button = sender as Button;

		if (button is not null)
		{
			ElectronicStateViewModel? itemToEdit = button.DataContext as ElectronicStateViewModel;

			if (itemToEdit is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesIndexControl>("edit", itemToEdit.Id));
			}
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesIndexControl), nameof(EditButton_Click));
		}
	}

	private void DetailsButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesIndexControl), nameof(DetailsButton_Click), sender, e);
		}

		Button? button = sender as Button;

		if (button is not null)
		{
			ElectronicStateViewModel? itemToView = button.DataContext as ElectronicStateViewModel;

			if (itemToView is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesIndexControl>("details", itemToView.Id));
			}
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesIndexControl), nameof(DetailsButton_Click));
		}
	}

	private void DeleteButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesIndexControl), nameof(DeleteButton_Click), sender, e);
		}

		Button? button = sender as Button;

		if (button is not null)
		{
			ElectronicStateViewModel? itemToDelete = button.DataContext as ElectronicStateViewModel;

			if (itemToDelete is not null)
			{
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesIndexControl>("delete", itemToDelete.Id));
			}
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesIndexControl), nameof(DeleteButton_Click));
		}
	}
}
