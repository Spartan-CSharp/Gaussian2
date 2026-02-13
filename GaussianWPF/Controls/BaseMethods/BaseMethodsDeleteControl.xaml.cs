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
/// Interaction logic for BaseMethodsDeleteControl.xaml
/// </summary>
public partial class BaseMethodsDeleteControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<BaseMethodsDeleteControl> _logger;
	private readonly IBaseMethodsEndpoint _baseMethodsEndpoint;
	private readonly IMethodFamiliesEndpoint _methodFamiliesEndpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseMethodsDeleteControl"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging control operations.</param>
	/// <param name="baseMethodsEndpoint">The endpoint for Base Method API operations.</param>
	/// <param name="methodFamiliesEndpoint">The endpoint for Method Family API operations.</param>
	public BaseMethodsDeleteControl(ILogger<BaseMethodsDeleteControl> logger, IBaseMethodsEndpoint baseMethodsEndpoint, IMethodFamiliesEndpoint methodFamiliesEndpoint)
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
	public event EventHandler<ChildControlEventArgs<BaseMethodsDeleteControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the Base Method view model being deleted.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically populates the RTF content and updates the <see cref="ModelIsNotNull"/> property.
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
	/// Gets or sets the identifier of the Base Method to delete.
	/// </summary>
	/// <remarks>
	/// When this property is set, the control automatically loads the Base Method data from the API.
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
	/// Gets or sets a value indicating whether a valid Base Method model is loaded.
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
	/// Gets a value indicating whether no Base Method model is loaded.
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
			_logger.LogDebug("{UserControl} {Method} called with CallerMemberName = {PropertyName}.", nameof(BaseMethodsDeleteControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} returning.", nameof(BaseMethodsDeleteControl), nameof(OnPropertyChanged));
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {EventArgs}.", nameof(BaseMethodsDeleteControl), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += BaseMethodsDeleteControl_PropertyChanged;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(BaseMethodsDeleteControl), nameof(OnInitialized));
		}
	}

	private void BaseMethodsDeleteControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(BaseMethodsDeleteControl), nameof(BaseMethodsDeleteControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(BaseMethod))
		{
			if (BaseMethod is not null)
			{
				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(BaseMethod.DescriptionRtf);
				ModelIsNotNull = true;
			}
			else
			{
				DescriptionRichTextBox.Document.Blocks.Clear();
				ModelIsNotNull = false;
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
						BaseMethod = new BaseMethodViewModel(results, methodFamilies);
						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(BaseMethod.DescriptionRtf);
						ModelIsNotNull = true;
					}
					else if (results is not null)
					{
						BaseMethod = new BaseMethodViewModel()
						{
							Id = results.Id,
							Keyword = results.Keyword,
							MethodFamily = results.MethodFamily?.ToRecord(),
							DescriptionRtf = results.DescriptionRtf,
							DescriptionText = results.DescriptionText,
							CreatedDate = results.CreatedDate,
							LastUpdatedDate = results.LastUpdatedDate,
							Archived = results.Archived
						};

						// Populate the RichTextBox with RTF
						DescriptionRichTextBox.SetRtfText(BaseMethod.DescriptionRtf);
						ModelIsNotNull = true;
					}
					else if (methodFamilies is not null && methodFamilies.Count > 0)
					{
						BaseMethod = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
					}
					else
					{
						BaseMethod = null;
						DescriptionRichTextBox.Document.Blocks.Clear();
						ModelIsNotNull = false;
					}
				}
				else if (BaseMethodId == 0)
				{
					BaseMethod = null;
					DescriptionRichTextBox.Document.Blocks.Clear();
					ModelIsNotNull = false;
				}
			}
			catch (HttpIOException ex)
			{
				ErrorMessage = ex.Message;

				if (_logger.IsEnabled(LogLevel.Error))
				{
					_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(BaseMethodsDeleteControl), nameof(BaseMethodsDeleteControl_PropertyChanged), sender, e);
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
							_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(BaseMethodsDeleteControl), nameof(BaseMethodsDeleteControl_PropertyChanged), sender, e);
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
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(BaseMethodsDeleteControl), nameof(BaseMethodsDeleteControl_PropertyChanged));
		}
	}

	private void DeleteButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(BaseMethodsDeleteControl), nameof(DeleteButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;
			_baseMethodsEndpoint.DeleteAsync(BaseMethodId).Wait();
			BaseMethod!.LastUpdatedDate = DateTime.Now;
			BaseMethod!.Archived = true;
			ChildControlEvent?.Invoke(this, new ChildControlEventArgs<BaseMethodsDeleteControl>("delete", null));

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(BaseMethodsDeleteControl), nameof(DeleteButton_Click));
			}
		}
		catch (HttpIOException ex)
		{
			ErrorMessage = ex.Message;

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(BaseMethodsDeleteControl), nameof(DeleteButton_Click), sender, e);
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
						_logger.LogError(ex, "{UserControl} {EventHandler} called with {Sender} and {EventArgs} had an error.", nameof(BaseMethodsDeleteControl), nameof(DeleteButton_Click), sender, e);
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
			_logger.LogDebug("{UserControl} {EventHandler} called with {Sender} and {EventArgs}.", nameof(BaseMethodsDeleteControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<BaseMethodsDeleteControl>("index", null));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {EventHandler} returning.", nameof(BaseMethodsDeleteControl), nameof(BackToIndexButton_Click));
		}
	}
}
