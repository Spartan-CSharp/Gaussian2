using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

using GaussianWPF.Controls;
using GaussianWPF.FactoryHelpers;

using GaussianWPFLibrary.DataAccess;
using GaussianWPFLibrary.EventModels;
using GaussianWPFLibrary.Models;

using Microsoft.Extensions.Logging;

namespace GaussianWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
/// <remarks>
/// The main application window that manages navigation between different controls
/// and handles user authentication state. Implements INotifyPropertyChanged to support
/// data binding for authentication-related UI updates.
/// </remarks>
public partial class MainWindow : Window, INotifyPropertyChanged
{
	private readonly ILogger<MainWindow> _logger;
	private readonly ILoggedInUserModel _loggedInUser;
	private readonly IApiHelper _apiHelper;
	private readonly HomeControl _homeControl;
	private readonly IAbstractFactory<LoginControl> _loginFactory;
	private readonly IAbstractFactory<CalculationTypesControl> _calculationTypesFactory;
	private readonly IAbstractFactory<MethodFamiliesControl> _methodFamiliesFactory;
	private readonly IAbstractFactory<BaseMethodsControl> _baseMethodsFactory;
	private readonly IAbstractFactory<AboutControl> _aboutFactory;
	private readonly IAbstractFactory<PrivacyControl> _privacyFactory;
	private readonly IAbstractFactory<ContactControl> _contactFactory;
	private readonly IAbstractFactory<ErrorControl> _errorFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="MainWindow"/> class.
	/// </summary>
	/// <param name="logger">The logger instance for logging window events.</param>
	/// <param name="loggedInUser">The logged-in user model.</param>
	/// <param name="apiHelper">The API helper for authentication operations.</param>
	/// <param name="homeControl">The home control instance to display by default.</param>
	/// <param name="loginFactory">The factory for creating login control instances.</param>
	/// <param name="calculationTypesFactory">The factory for creating Calculation Types control instances.</param>
	/// <param name="methodFamiliesFactory">The factory for creating Method Families control instances.</param>
	/// <param name="baseMethodsFactory">The factory for creating Base Methods control instances.</param>
	/// <param name="aboutFactory">The factory for creating about control instances.</param>
	/// <param name="contactFactory">The factory for creating contact control instances.</param>
	/// <param name="privacyFactory">The factory for creating privacy control instances.</param>
	/// <param name="errorFactory">The factory for creating error control instances.</param>
	public MainWindow(ILogger<MainWindow> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, HomeControl homeControl, IAbstractFactory<LoginControl> loginFactory, IAbstractFactory<CalculationTypesControl> calculationTypesFactory, IAbstractFactory<MethodFamiliesControl> methodFamiliesFactory, IAbstractFactory<BaseMethodsControl> baseMethodsFactory, IAbstractFactory<AboutControl> aboutFactory, IAbstractFactory<PrivacyControl> privacyFactory, IAbstractFactory<ContactControl> contactFactory, IAbstractFactory<ErrorControl> errorFactory)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_homeControl = homeControl;
		_loginFactory = loginFactory;
		_calculationTypesFactory = calculationTypesFactory;
		_methodFamiliesFactory = methodFamiliesFactory;
		_baseMethodsFactory = baseMethodsFactory;
		_aboutFactory = aboutFactory;
		_privacyFactory = privacyFactory;
		_contactFactory = contactFactory;
		_errorFactory = errorFactory;

		InitializeComponent();
		DataContext = this;
		PropertyChanged += MainWindow_PropertyChanged;

		// 1. Handles exceptions on the main UI thread (Can prevent crash)
		Application.Current.DispatcherUnhandledException += App_DispatcherUnhandledException;

		// 2. Handles exceptions on background threads (Cannot prevent crash)
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

		_apiHelper.AuthenticationStateChanged += MainWindow_AuthenticationStateChanged;
		MainContent.Content = _homeControl;
	}

	/// <summary>
	/// Event that is raised when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Gets or sets a value indicating whether the user is currently logged in.
	/// </summary>
	/// <remarks>
	/// When this property changes, it triggers notifications for both <see cref="IsLoggedIn"/>
	/// and <see cref="IsLoggedOut"/> properties to update the UI bindings.
	/// </remarks>
	public bool IsLoggedIn
	{
		get;

		set
		{
			if (field != value)
			{
				field = value;
				OnPropertyChanged(nameof(IsLoggedIn));
				OnPropertyChanged(nameof(IsLoggedOut));
			}
		}
	}

	/// <summary>
	/// Gets a value indicating whether the user is currently logged out.
	/// </summary>
	/// <remarks>
	/// This property is the inverse of <see cref="IsLoggedIn"/> and is used for UI binding purposes.
	/// </remarks>
	public bool IsLoggedOut
	{
		get { return !IsLoggedIn; }
	}

	/// <summary>
	/// Raises the <see cref="PropertyChanged"/> event for the specified property.
	/// </summary>
	/// <param name="propertyName">
	/// The name of the property that changed. This parameter is automatically populated
	/// by the compiler when called from a property setter.
	/// </param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {Method} with {PropertyName}", nameof(MainWindow), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void MainWindow_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Window} {EventHandler} with {Sender} and {EventArgs}", nameof(MainWindow), nameof(MainWindow_PropertyChanged), sender, e);
		}
	}

	private void MainWindow_AuthenticationStateChanged(object? sender, AuthenticationEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Window} {EventHandler} with {Sender} and {EventArgs}", nameof(MainWindow), nameof(MainWindow_AuthenticationStateChanged), sender, e);
		}

		_ = Dispatcher.BeginInvoke(() =>
		{
			if (e.IsAuthenticated)
			{
				IsLoggedIn = true;
				MainContent.Content = _homeControl;
			}
			else
			{
				IsLoggedIn = false;
				MainContent.Content = _homeControl;
			}
		});
	}

	private void Home_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Window} {EventHandler} with {Sender} and {EventArgs}", nameof(MainWindow), nameof(Home_Click), sender, e);
		}

		MainContent.Content = _homeControl;
	}

	private void Login_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Window} {EventHandler} with {Sender} and {EventArgs}", nameof(MainWindow), nameof(Login_Click), sender, e);
		}

		LoginControl loginControl = _loginFactory.Create();
		MainContent.Content = loginControl;
	}

	private void Logout_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Window} {EventHandler} with {Sender} and {EventArgs}", nameof(MainWindow), nameof(Logout_Click), sender, e);
		}

		_ = _apiHelper.LogoutAsync();
	}

	private void ExitApplication_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Window} {EventHandler} with {Sender} and {EventArgs}", nameof(MainWindow), nameof(ExitApplication_Click), sender, e);
		}

		Close();
	}

	private void CalculationTypes_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Window} {EventHandler} with {Sender} and {EventArgs}", nameof(MainWindow), nameof(CalculationTypes_Click), sender, e);
		}

		CalculationTypesControl calculationTypesControl = _calculationTypesFactory.Create();
		MainContent.Content = calculationTypesControl;
	}

	private void MethodFamilies_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Window} {EventHandler} with {Sender} and {EventArgs}", nameof(MainWindow), nameof(MethodFamilies_Click), sender, e);
		}

		MethodFamiliesControl methodFamiliesControl = _methodFamiliesFactory.Create();
		MainContent.Content = methodFamiliesControl;
	}

	private void BaseMethods_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Window} {EventHandler} with {Sender} and {EventArgs}", nameof(MainWindow), nameof(BaseMethods_Click), sender, e);
		}

		BaseMethodsControl baseMethodsControl = _baseMethodsFactory.Create();
		MainContent.Content = baseMethodsControl;
	}

	private void About_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Window} {EventHandler} with {Sender} and {EventArgs}", nameof(MainWindow), nameof(About_Click), sender, e);
		}

		AboutControl aboutControl = _aboutFactory.Create();
		MainContent.Content = aboutControl;
	}

	private void Privacy_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Window} {EventHandler} with {Sender} and {EventArgs}", nameof(MainWindow), nameof(Privacy_Click), sender, e);
		}

		PrivacyControl privacyControl = _privacyFactory.Create();
		MainContent.Content = privacyControl;
	}

	private void Contact_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Window} {EventHandler} with {Sender} and {EventArgs}", nameof(MainWindow), nameof(Contact_Click), sender, e);
		}

		ContactControl contactControl = _contactFactory.Create();
		MainContent.Content = contactControl;
	}

	private void Error_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Window} {EventHandler} with {Sender} and {EventArgs}", nameof(MainWindow), nameof(Error_Click), sender, e);
		}

		ErrorControl errorControl = _errorFactory.Create();
		MainContent.Content = errorControl;
	}

	private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Window} {EventHandler} with {Sender} and {EventArgs}", nameof(MainWindow), nameof(App_DispatcherUnhandledException), sender, e);
		}

		if (_logger.IsEnabled(LogLevel.Error))
		{
			_logger.LogError("Dispatcher Unhandled Exception Event in UI Thread with {ExceptionType} and {ExceptionMessage}", e.Exception.GetType().Name, e.Exception.Message);
		}

		// Set Handled to true to stop the exception from crashing the app
		e.Handled = true;

		// Show your custom error screen/message
		ErrorControl errorControl = _errorFactory.Create();
		errorControl.ErrorType = e.Exception.GetType().Name;
		errorControl.ErrorMessage = e.Exception.Message;
		MainContent.Content = errorControl;
	}

	private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Window} {EventHandler} with {Sender} and {EventArgs}", nameof(MainWindow), nameof(CurrentDomain_UnhandledException), sender, e);
		}

		// AppDomain exceptions are usually fatal. We log and show the error before the process terminates.
		Exception ex = (Exception)e.ExceptionObject;

		// Optionally: Log the exception to a file here
		if (_logger.IsEnabled(LogLevel.Critical))
		{
			_logger.LogCritical("Fatal Unhandled Exception Event in Non-UI Thread with {ExceptionType} and {ExceptionMessage}", ex?.GetType().Name, ex?.Message);
		}

		string message = $"An unexpected fatal error occurred and the application needs to close.\n{ex?.GetType().Name}\n{ex?.Message}";

		// For a simple error screen, use a MessageBox or open a custom Window
		_ = MessageBox.Show(this, message, "Application Error (Fatal)", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None);
	}
}
