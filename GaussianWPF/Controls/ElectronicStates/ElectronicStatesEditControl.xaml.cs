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
/// Interaction logic for ElectronicStatesEditControl.xaml
/// </summary>
public partial class ElectronicStatesEditControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<ElectronicStatesEditControl> _logger;
	private readonly IElectronicStatesEndpoint _endpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="ElectronicStatesEditControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="endpoint">The endpoint for electronic state API operations.</param>
	public ElectronicStatesEditControl(ILogger<ElectronicStatesEditControl> logger, IElectronicStatesEndpoint endpoint)
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
	public event EventHandler<ChildControlEventArgs<ElectronicStatesEditControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the electronic state view model being edited.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically populates the name, keyword, RTF content, and updates validation state.
	/// </remarks>
	public ElectronicStateViewModel? ElectronicState
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(ElectronicState));
		}
	}

	/// <summary>
	/// Gets or sets the identifier of the electronic state to edit.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically loads the electronic state data from the API.
	/// </remarks>
	public int ElectronicStateId
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(ElectronicStateId));
		}
	}

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
	/// Gets or sets a value indicating whether a valid electronic state model is loaded.
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
	/// Gets a value indicating whether no electronic state model is loaded.
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
	/// This property is automatically set to <see langword="true"/> when the name, keyword, and description are valid.
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
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(ElectronicStatesEditControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(ElectronicStatesEditControl), nameof(OnPropertyChanged));
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += ElectronicStatesEditControl_PropertyChanged;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(OnInitialized));
		}
	}

	private void ElectronicStatesEditControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(ElectronicStatesEditControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(ElectronicState))
		{
			if (ElectronicState is not null)
			{
				ElectronicStateName = ElectronicState.Name ?? string.Empty;
				Keyword = ElectronicState.Keyword ?? string.Empty;
				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(ElectronicState.DescriptionRtf);
				ModelIsNotNull = true;
				CanSave = ElectronicState.Name?.Length is > 0 and <= 50 && ElectronicState.Keyword?.Length is > 0 and <= 20 && (ElectronicState.DescriptionText?.Length is <= 2000 || string.IsNullOrEmpty(ElectronicState.DescriptionText));
			}
			else
			{
				ElectronicStateName = string.Empty;
				Keyword = string.Empty;
				DescriptionRichTextBox.Document.Blocks.Clear();
				ModelIsNotNull = false;
				CanSave = false;
			}
		}

		if (e.PropertyName is nameof(ElectronicStateId))
		{
			try
			{
				if (ElectronicStateId != 0 && (ElectronicState is null || ElectronicState.Id != ElectronicStateId))
				{
					ElectronicStateFullModel? results = _endpoint.GetByIdAsync(ElectronicStateId).Result;

					if (results is not null)
					{
						ElectronicState = new ElectronicStateViewModel(results);
						ElectronicStateName = ElectronicState.Name ?? string.Empty;
						Keyword = ElectronicState.Keyword ?? string.Empty;
						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(ElectronicState.DescriptionRtf);
						ModelIsNotNull = true;
						CanSave = ElectronicState.Name?.Length is > 0 and <= 50 && ElectronicState.Keyword?.Length is > 0 and <= 20 && (ElectronicState.DescriptionText?.Length is <= 2000 || string.IsNullOrEmpty(ElectronicState.DescriptionText));
					}
					else
					{
						ElectronicState = null;
						ElectronicStateName = string.Empty;
						Keyword = string.Empty;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
						CanSave = false;
					}
				}
				else if (ElectronicStateId == 0)
				{
					ElectronicState = null;
					ElectronicStateName = string.Empty;
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
					_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(ElectronicStatesEditControl), nameof(ElectronicStatesEditControl_PropertyChanged), sender, e);
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
							_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(ElectronicStatesEditControl), nameof(ElectronicStatesEditControl_PropertyChanged), sender, e);
						}

						return true;
					}

					// Return false for any other exception types to rethrow them in a new AggregateException
					return false;
				});
			}
		}

		if (e.PropertyName is (nameof(ElectronicStateName)) or (nameof(Keyword)))
		{
			CanSave = ElectronicStateName?.Length is > 0 and <= 50 && Keyword?.Length is > 0 and <= 20;
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}

		// Nothing to do for change events of ModelIsNull, ModelIsNotNull, IsErrorVisible and CanSave.

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(ElectronicStatesEditControl_PropertyChanged));
		}
	}

	private void SaveButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(SaveButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;
			ElectronicState!.Name = ElectronicStateName;
			ElectronicState!.Keyword = Keyword;
			ElectronicState!.DescriptionRtf = DescriptionRichTextBox.GetRtfText();
			ElectronicState!.DescriptionText = DescriptionRichTextBox.GetPlainText();
			ElectronicStateFullModel model = ElectronicState!.ToFullModel();
			ElectronicStateFullModel? result = _endpoint.UpdateAsync(ElectronicStateId, model).Result;

			if (result is not null)
			{
				ElectronicState.LastUpdatedDate = result.LastUpdatedDate;
				ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesEditControl>("save", null));
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(SaveButton_Click));
			}
		}
		catch (HttpIOException ex)
		{
			ErrorMessage = ex.Message;

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(ElectronicStatesEditControl), nameof(SaveButton_Click), sender, e);
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
						_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(ElectronicStatesEditControl), nameof(SaveButton_Click), sender, e);
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(ElectronicStatesEditControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<ElectronicStatesEditControl>("index", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(ElectronicStatesEditControl), nameof(BackToIndexButton_Click));
		}
	}

	private void BoldButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(ElectronicStatesEditControl), nameof(BoldButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleFontWeight();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(ElectronicStatesEditControl), nameof(BoldButton_Click));
		}
	}

	private void ItalicButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(ElectronicStatesEditControl), nameof(ItalicButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleFontStyle();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(ElectronicStatesEditControl), nameof(ItalicButton_Click));
		}
	}

	private void UnderlineButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(ElectronicStatesEditControl), nameof(UnderlineButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleUnderline();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(ElectronicStatesEditControl), nameof(UnderlineButton_Click));
		}
	}

	private void SubscriptButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(ElectronicStatesEditControl), nameof(SubscriptButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleBaselineAlignment(BaselineAlignment.Subscript);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(ElectronicStatesEditControl), nameof(SubscriptButton_Click));
		}
	}

	private void SuperscriptButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(ElectronicStatesEditControl), nameof(SuperscriptButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleBaselineAlignment(BaselineAlignment.Superscript);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(ElectronicStatesEditControl), nameof(SuperscriptButton_Click));
		}
	}

	private void BulletsButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(ElectronicStatesEditControl), nameof(BulletsButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleList(TextMarkerStyle.Disc);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(ElectronicStatesEditControl), nameof(BulletsButton_Click));
		}
	}

	private void NumberingButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.",
				nameof(ElectronicStatesEditControl), nameof(NumberingButton_Click), sender, e);
		}

		DescriptionRichTextBox.ToggleList(TextMarkerStyle.Decimal);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.",
				nameof(ElectronicStatesEditControl), nameof(NumberingButton_Click));
		}
	}
}
