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

namespace GaussianWPF.Controls.SpinStates;

/// <summary>
/// Interaction logic for SpinStatesDetailsControl.xaml
/// </summary>
public partial class SpinStatesDetailsControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<SpinStatesDetailsControl> _logger;
	private readonly ISpinStatesEndpoint _endpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="SpinStatesDetailsControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="endpoint">The endpoint for spin state API operations.</param>
	public SpinStatesDetailsControl(ILogger<SpinStatesDetailsControl> logger, ISpinStatesEndpoint endpoint)
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
	public event EventHandler<ChildControlEventArgs<SpinStatesDetailsControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the spin state view model being displayed.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically populates the RTF content and updates the <see cref="ModelIsNotNull"/> property.
	/// </remarks>
	public SpinStateViewModel? SpinState
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(SpinState));
		}
	}

	/// <summary>
	/// Gets or sets the identifier of the spin state to display.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically loads the spin state data from the API.
	/// </remarks>
	public int SpinStateId
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(SpinStateId));
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether a valid spin state model is loaded.
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
	/// Gets a value indicating whether no spin state model is loaded.
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
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(SpinStatesDetailsControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(SpinStatesDetailsControl), nameof(OnPropertyChanged));
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(SpinStatesDetailsControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += SpinStatesDetailsControl_PropertyChanged;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesDetailsControl), nameof(OnInitialized));
		}
	}

	private void SpinStatesDetailsControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesDetailsControl), nameof(SpinStatesDetailsControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(SpinState))
		{
			if (SpinState is not null)
			{
				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(SpinState.DescriptionRtf);
				ModelIsNotNull = true;
			}
			else
			{
				DescriptionRichTextBox.Document.Blocks.Clear();
				ModelIsNotNull = false;
			}
		}

		if (e.PropertyName is nameof(SpinStateId))
		{
			try
			{
				if (SpinStateId != 0 && (SpinState is null || SpinState.Id != SpinStateId))
				{
					SpinStateFullModel? results = _endpoint.GetByIdAsync(SpinStateId).Result;

					if (results is not null)
					{
						SpinState = new SpinStateViewModel(results);
						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(SpinState.DescriptionRtf);
						ModelIsNotNull = true;
					}
					else
					{
						SpinState = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
					}
				}
				else if (SpinStateId == 0)
				{
					SpinState = null;
					DescriptionRichTextBox.Document.Blocks.Clear();
					ModelIsNotNull = false;
				}
			}
			catch (HttpIOException ex)
			{
				ErrorMessage = ex.Message;

				if (_logger.IsEnabled(LogLevel.Error))
				{
					_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(SpinStatesDetailsControl), nameof(SpinStatesDetailsControl_PropertyChanged), sender, e);
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
							_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(SpinStatesDetailsControl), nameof(SpinStatesDetailsControl_PropertyChanged), sender, e);
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
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesDetailsControl), nameof(SpinStatesDetailsControl_PropertyChanged));
		}
	}

	private void EditButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesDetailsControl), nameof(EditButton_Click), sender, e);
		}

		if (ModelIsNotNull)
		{
			ChildControlEvent?.Invoke(this, new ChildControlEventArgs<SpinStatesDetailsControl>("edit", SpinState!.Id));
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesDetailsControl), nameof(EditButton_Click));
		}
	}

	private void BackToIndexButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(SpinStatesDetailsControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<SpinStatesDetailsControl>("index", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(SpinStatesDetailsControl), nameof(BackToIndexButton_Click));
		}
	}
}
