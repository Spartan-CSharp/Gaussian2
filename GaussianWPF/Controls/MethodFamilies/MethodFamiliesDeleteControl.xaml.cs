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
using GaussianWPFLibrary.Models;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls.MethodFamilies;

/// <summary>
/// Interaction logic for MethodFamiliesDeleteControl.xaml
/// Provides a user interface for viewing and deleting Method Families with confirmation.
/// </summary>
public partial class MethodFamiliesDeleteControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<MethodFamiliesDeleteControl> _logger;
	private readonly ILoggedInUserModel _loggedInUser;
	private readonly IApiHelper _apiHelper;
	private readonly IMethodFamiliesEndpoint _endpoint;

	/// <summary>
	/// Initializes a new instance of the <see cref="MethodFamiliesDeleteControl"/> class.
	/// </summary>
	/// <param name="logger">Logger for diagnostic and trace information.</param>
	/// <param name="loggedInUser">The currently logged-in user model.</param>
	/// <param name="apiHelper">Helper for API interactions.</param>
	/// <param name="endpoint">Endpoint for Method Families data access operations.</param>
	public MethodFamiliesDeleteControl(ILogger<MethodFamiliesDeleteControl> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, IMethodFamiliesEndpoint endpoint)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_endpoint = endpoint;

		InitializeComponent();
		DataContext = this;
		PropertyChanged += MethodFamiliesDeleteControl_PropertyChanged;
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Occurs when a child control event is raised, such as navigation or completion events.
	/// </summary>
	public event EventHandler<ChildControlEventArgs<MethodFamiliesDeleteControl>>? ChildControlEvent;

	/// <summary>
	/// Gets or sets the ID of the Method Family to be deleted.
	/// When set, triggers loading of the Method Family details.
	/// </summary>
	public int MethodFamilyId
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(MethodFamilyId));
		}
	}

	/// <summary>
	/// Gets or sets the Method Family view model containing the data to be displayed and deleted.
	/// </summary>
	public MethodFamilyViewModel? MethodFamily
	{
		get;
		set
		{
			field = value;
			OnPropertyChanged(nameof(MethodFamily));
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether the Method Family model is not null.
	/// Used for controlling UI element visibility.
	/// </summary>
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
	/// Gets a value indicating whether the Method Family model is null.
	/// Computed property based on <see cref="ModelIsNotNull"/>.
	/// </summary>
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
	/// Automatically updated when <see cref="ErrorMessage"/> changes.
	/// </summary>
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
	/// Raises the <see cref="PropertyChanged"/> event.
	/// </summary>
	/// <param name="propertyName">Name of the property that changed. Automatically populated by the compiler.</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} with {PropertyName}", nameof(MethodFamiliesDeleteControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void MethodFamiliesDeleteControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesDeleteControl), nameof(MethodFamiliesDeleteControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(MethodFamilyId))
		{
			try
			{
				if (MethodFamilyId != 0)
				{
					MethodFamilyFullModel? results = _endpoint.GetByIdAsync(MethodFamilyId).Result;

					if (_logger.IsEnabled(LogLevel.Trace))
					{
						_logger.LogTrace("{Method} returned {Results}", nameof(_endpoint.GetByIdAsync), results);
					}

					if (results is not null)
					{
						MethodFamily = new MethodFamilyViewModel(results);
						ModelIsNotNull = true;
					}
				}
				else
				{
					MethodFamily = null;
					ModelIsNotNull = false;
				}
			}
			catch (HttpIOException ex)
			{
				if (_logger.IsEnabled(LogLevel.Error))
				{
					_logger.LogError(ex, "{UserControl} {Method} had an error", nameof(MethodFamiliesEditControl), nameof(OnInitialized));
				}

				ErrorMessage = ex.Message;
			}
			catch (AggregateException ae)
			{
				ae.Handle(ex =>
				{
					if (ex is HttpIOException)
					{
						if (_logger.IsEnabled(LogLevel.Error))
						{
							_logger.LogError(ex, "{UserControl} {Method} had an error", nameof(MethodFamiliesEditControl), nameof(OnInitialized));
						}

						ErrorMessage = ex.Message;
						return true;
					}

					// Return false for any other exception types to rethrow them in a new AggregateException
					return false;
				});
			}
		}

		if (e.PropertyName is nameof(MethodFamily))
		{
			if (MethodFamily is not null)
			{
				// Populate the RichTextBox with RTF
				DescriptionRichTextBox.SetRtfText(MethodFamily.DescriptionRtf);
			}
			else
			{
				DescriptionRichTextBox.Document.Blocks.Clear();
			}
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}
	}

	private void DeleteButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesDeleteControl), nameof(DeleteButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;

			MethodFamilyFullModel model = MethodFamily!.ToFullModel();
			_endpoint.DeleteAsync(MethodFamilyId).Wait();
			MethodFamily.LastUpdatedDate = DateTime.Now;
			MethodFamily.Archived = true;
			ChildControlEvent?.Invoke(this, new ChildControlEventArgs<MethodFamiliesDeleteControl>("delete", null));
		}
		catch (HttpIOException ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(MethodFamiliesDeleteControl), nameof(DeleteButton_Click));
			}

			ErrorMessage = ex.Message;
		}
		catch (AggregateException ae)
		{
			ae.Handle(ex =>
			{
				if (ex is HttpIOException)
				{
					if (_logger.IsEnabled(LogLevel.Error))
					{
						_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(MethodFamiliesDeleteControl), nameof(DeleteButton_Click));
					}

					ErrorMessage = ex.Message;
					return true;
				}

				// Return false for any other exception types to rethrow them in a new AggregateException
				return false;
			});
		}
	}

	private void BackToIndexButton_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(MethodFamiliesDeleteControl), nameof(BackToIndexButton_Click), sender, e);
		}

		ChildControlEvent?.Invoke(this, new ChildControlEventArgs<MethodFamiliesDeleteControl>("index", null));
	}
}
