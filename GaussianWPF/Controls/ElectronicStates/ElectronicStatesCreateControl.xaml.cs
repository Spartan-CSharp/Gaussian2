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

namespace GaussianWPF.Controls.ElectronicStates;

/// <summary>
/// Interaction logic for ElectronicStatesCreateControl.xaml
/// </summary>
public partial class ElectronicStatesCreateControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<ElectronicStatesCreateControl> _logger;
	private readonly IElectronicStatesEndpoint _endpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStatesCreateControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="endpoint">The endpoint for electronic state API operations.</param>
	public ElectronicStatesCreateControl(ILogger<ElectronicStatesCreateControl> logger, IElectronicStatesEndpoint endpoint)
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
	public event EventHandler<ChildControlEventArgs<ElectronicStatesCreateControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the electronic state view model being created.
	/// </summary>
	public ElectronicStateViewModel ElectronicState
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(ElectronicState));
		}
	} = new();

	/// <summary>
	/// Gets or sets the name of the electronic state.
	/// </summary>
	public string ElectronicStateName
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(ElectronicStateName));
		}
	} = string.Empty;

	/// <summary>
	/// Gets or sets the keyword that identifies the electronic state.
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
	/// Gets or sets a value indicating whether the create button should be enabled.
	/// </summary>
	/// <remarks>
	/// This property is automatically set to <see langword="true"/> when the name and keyword are valid.
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
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(ElectronicStatesCreateControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesCreateControl), nameof(OnPropertyChanged));
		}
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the On Initialized event and sets up data binding and property change notifications.
	/// </summary>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(ElectronicStatesCreateControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += ElectronicStatesCreateControl_PropertyChanged;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesCreateControl), nameof(OnInitialized));
		}
	}

	private void ElectronicStatesCreateControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesCreateControl), nameof(ElectronicStatesCreateControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(ElectronicState))
		{
			if (ElectronicState is not null)
			{
				ElectronicStateName = ElectronicState.Name ?? string.Empty;
				Keyword = ElectronicState.Keyword ?? string.Empty;
				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(ElectronicState.DescriptionRtf);
				CanSave = ElectronicState.Name?.Length is > 0 and <= 50 && ElectronicState.Keyword?.Length is > 0 and <= 20 && (ElectronicState.DescriptionText?.Length is <= 2000 || string.IsNullOrEmpty(ElectronicState.DescriptionText));
			}
			else
			{
				ElectronicStateName = string.Empty;
				Keyword = string.Empty;
				DescriptionRichTextBox.Document.Blocks.Clear();
				CanSave = false;
			}
		}

		if (e.PropertyName is (nameof(ElectronicStateName)) or (nameof(Keyword)))
		{
			CanSave = ElectronicStateName?.Length is > 0 and <= 50 || Keyword?.Length is > 0 and <= 20;
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}

		// Nothing to do for change events of IsErrorVisible and CanSave.

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesCreateControl), nameof(ElectronicStatesCreateControl_PropertyChanged));
		}
	}

	private void CreateButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesCreateControl), nameof(CreateButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;
			ElectronicState.Name = ElectronicStateName;
			ElectronicState.Keyword = Keyword;
			ElectronicState.DescriptionRtf = DescriptionRichTextBox.GetRtfText();
			ElectronicState.DescriptionText = DescriptionRichTextBox.GetPlainText();
			ElectronicStateFullModel model = ElectronicState.ToFullModel();
			ElectronicStateFullModel? result = _endpoint.CreateAsync(model).Result;

			if (result is not null)
			{
				ElectronicState.Id = result.Id;
				ElectronicState.CreatedDate = result.CreatedDate;
				ElectronicState.LastUpdatedDate = result.LastUpdatedDate;
				ElectronicState.Archived = result.Archived;
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesCreateControl>("create", ElectronicState.Id));
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesCreateControl), nameof(CreateButton_Click));
			}
		}
		catch (HttpIOException ex)
		{
			ErrorMessage = ex.Message;

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(ElectronicStatesCreateControl), nameof(CreateButton_Click), sender, e);
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
						_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(ElectronicStatesCreateControl), nameof(CreateButton_Click), sender, e);
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesCreateControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesCreateControl>("index", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesCreateControl), nameof(BackToIndexButton_Click));
		}
	}

	private void BoldButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(ElectronicStatesCreateControl), nameof(BoldButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleFontWeight();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(ElectronicStatesCreateControl), nameof(BoldButton_Click));
		}
	}

	private void ItalicButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(ElectronicStatesCreateControl), nameof(ItalicButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleFontStyle();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(ElectronicStatesCreateControl), nameof(ItalicButton_Click));
		}
	}

	private void UnderlineButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(ElectronicStatesCreateControl), nameof(UnderlineButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleUnderline();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(ElectronicStatesCreateControl), nameof(UnderlineButton_Click));
		}
	}

	private void SubscriptButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(ElectronicStatesCreateControl), nameof(SubscriptButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleBaselineAlignment(BaselineAlignment.Subscript);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(ElectronicStatesCreateControl), nameof(SubscriptButton_Click));
		}
	}

	private void SuperscriptButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(ElectronicStatesCreateControl), nameof(SuperscriptButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleBaselineAlignment(BaselineAlignment.Superscript);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(ElectronicStatesCreateControl), nameof(SuperscriptButton_Click));
		}
	}

	private void BulletsButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(ElectronicStatesCreateControl), nameof(BulletsButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleList(TextMarkerStyle.Disc);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(ElectronicStatesCreateControl), nameof(BulletsButton_Click));
		}
	}

	private void NumberingButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(ElectronicStatesCreateControl), nameof(NumberingButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleList(TextMarkerStyle.Decimal);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(ElectronicStatesCreateControl), nameof(NumberingButton_Click));
		}
	}
}
