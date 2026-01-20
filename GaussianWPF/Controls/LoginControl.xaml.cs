using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

using GaussianWPFLibrary.DataAccess;
using GaussianWPFLibrary.Models;

using Microsoft.Extensions.Logging;

namespace GaussianWPF.Controls;

/// <summary>
/// Interaction logic for LoginControl.xaml
/// </summary>
/// <remarks>
/// A WPF UserControl that provides user authentication functionality through username and password input.
/// Implements INotifyPropertyChanged to support data binding and includes error handling with visual feedback.
/// </remarks>
public partial class LoginControl : UserControl, INotifyPropertyChanged
{
	private readonly ILogger<LoginControl> _logger;
	private readonly IApiHelper _apiHelper;
	private readonly ILoggedInUserModel _loggedInUser;

	/// <summary>
	/// Initializes a new instance of the <see cref="LoginControl"/> class.
	/// </summary>
	/// <param name="logger">The logger instance for diagnostic logging.</param>
	/// <param name="apiHelper">The API helper for authentication operations.</param>
	/// <param name="loggedInUser">The model to store the logged-in user information.</param>
	public LoginControl(ILogger<LoginControl> logger, IApiHelper apiHelper, ILoggedInUserModel loggedInUser)
	{
		_logger = logger;
		_apiHelper = apiHelper;
		_loggedInUser = loggedInUser;

		InitializeComponent();
		DataContext = this;
		PropertyChanged += LoginControl_PropertyChanged;
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Gets or sets the username entered by the user.
	/// </summary>
	/// <value>The username string. Triggers property change notification when modified.</value>
	public string? UserName
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(UserName));
			}
		}
	}

	/// <summary>
	/// Gets or sets the password entered by the user.
	/// </summary>
	/// <value>The password string. Triggers property change notification when modified.</value>
	public string? Password
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(Password));
			}
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether the login button should be enabled.
	/// </summary>
	/// <value><c>true</c> if both username and password have been entered; otherwise, <c>false</c>.</value>
	public bool CanLogIn
	{
		get;

		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(CanLogIn));
			}
		}
	}

	/// <summary>
	/// Gets or sets the error message to display when login fails.
	/// </summary>
	/// <value>The error message string, or empty string if no error occurred.</value>
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
	/// <value><c>true</c> if an error message exists and should be displayed; otherwise, <c>false</c>.</value>
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
	/// <param name="propertyName">The name of the property that changed. Auto-populated by compiler when called from a property setter.</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{UserControl} {Method} with {PropertyName}", nameof(LoginControl), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {Method} with {Sender} and {EventArgs}", nameof(LoginControl), nameof(PasswordBox_PasswordChanged), sender, e);
		}

		// PasswordBox.Password cannot be bound directly for security reasons
		Password = PasswordBox.Password;
	}

	private void LoginButton_Click(object sender, RoutedEventArgs e)
	{
		// Your login logic
		try
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(LoginControl), nameof(LoginButton_Click), sender, e);
			}

			ErrorMessage = string.Empty;

			ILoggedInUserModel result = _apiHelper.LoginAsync(UserName!, Password!).Result;

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} returned {Result}", nameof(_apiHelper.LoginAsync), result);
			}

			_loggedInUser.UserName = result.UserName;
			_loggedInUser.AccessToken = result.AccessToken;
		}
		catch (HttpIOException ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(LoginControl), nameof(LoginButton_Click));
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
						_logger.LogError(ex, "{UserControl} {EventHandler} had an error", nameof(LoginControl), nameof(LoginButton_Click));
					}

					ErrorMessage = ex.Message;
					return true;
				}

				// Return false for any other exception types to rethrow them in a new AggregateException
				return false;
			});
		}
	}

	private void LoginControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{UserControl} {EventHandler} with {Sender} and {EventArgs}", nameof(LoginControl), nameof(LoginControl_PropertyChanged), sender, e);
		}

		if (e.PropertyName is (nameof(UserName)) or (nameof(Password)))
		{
			CanLogIn = UserName?.Length > 0 && Password?.Length > 0;
		}

		if (e.PropertyName == nameof(ErrorMessage))
		{
			IsErrorVisible = ErrorMessage?.Length > 0;
		}
	}
}
