using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls;

/// <summary>
/// Interaction logic for ErrorControl.xaml
/// </summary>
/// <remarks>
/// A WPF UserControl that displays error messages with automatic visibility management.
/// Implements <see cref="INotifyPropertyChanged"/> to support data binding and automatically
/// shows or hides itself based on whether error information is present.
/// </remarks>
public partial class ErrorControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<ErrorControl> _logger;

	/// <summary>
	/// Initializes a new instance of the <see cref="ErrorControl"/> class.
	/// </summary>
	/// <param name="logger">The logger instance used for diagnostic logging.</param>
	public ErrorControl(ILogger<ErrorControl> logger)
	{
		InitializeComponent();
		DataContext = this;
		PropertyChanged += ErrorControl_PropertyChanged;
		_logger = logger;
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Gets or sets the type or category of the error being displayed.
	/// </summary>
	/// <value>
	/// A string representing the error type, or <c>null</c> if no error type is set.
	/// </value>
	/// <remarks>
	/// Setting this property triggers property change notification and may affect
	/// the <see cref="IsErrorVisible"/> property.
	/// </remarks>
	public string? ErrorType
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(ErrorType));
			}
		}
	}

	/// <summary>
	/// Gets or sets the detailed error message to be displayed.
	/// </summary>
	/// <value>
	/// A string containing the error message, or <c>null</c> if no message is set.
	/// </value>
	/// <remarks>
	/// Setting this property triggers property change notification and may affect
	/// the <see cref="IsErrorVisible"/> property.
	/// </remarks>
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
	/// Gets or sets a value indicating whether the error control should be visible.
	/// </summary>
	/// <value>
	/// <c>true</c> if the error control is visible; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	/// This property is automatically updated when <see cref="ErrorType"/> or 
	/// <see cref="ErrorMessage"/> changes. The control is visible if either property
	/// contains a non-empty value.
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
	/// <param name="propertyName">
	/// The name of the property that changed. This parameter is automatically populated
	/// by the compiler when called from a property setter.
	/// </param>
	/// <remarks>
	/// This method includes debug-level logging to track property changes for diagnostic purposes.
	/// </remarks>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} with {PropertyName}", nameof(ErrorControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void ErrorControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(ErrorControl), nameof(ErrorControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is (nameof(ErrorType)) or (nameof(ErrorMessage)))
		{
			IsErrorVisible = ErrorType?.Length > 0 || ErrorMessage?.Length > 0;

			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError("{UserControl} {EventHandler} received {ErrorType}: {ErrorMessage}", nameof(ErrorControl), nameof(ErrorControl_PropertyChanged), ErrorType, ErrorMessage);
			}
		}
	}
}
