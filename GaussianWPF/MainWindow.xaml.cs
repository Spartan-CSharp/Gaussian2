using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

using GaussianWPF.Controls;
using GaussianWPF.FactoryHelpers;
using GaussianWPF.Properties;

using GaussianWPFLibrary.DataAccess;
using GaussianWPFLibrary.EventModels;
using GaussianWPFLibrary.Models;

using Microsoft.Extensions.Logging;

namespace GaussianWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
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
	private readonly IAbstractFactory<ElectronicStatesControl> _electronicStatesFactory;
	private readonly IAbstractFactory<AboutControl> _aboutFactory;
	private readonly IAbstractFactory<PrivacyControl> _privacyFactory;
	private readonly IAbstractFactory<ContactControl> _contactFactory;
	private readonly IAbstractFactory<ErrorControl> _errorFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="MainWindow"/> class with dependency injection.
	/// </summary>
	/// <param name="logger">The logger instance for logging window operations.</param>
	/// <param name="loggedInUser">The logged-in user model for managing authentication state.</param>
	/// <param name="apiHelper">The API helper for HTTP client operations.</param>
	/// <param name="homeControl">The home control to display on startup.</param>
	/// <param name="loginFactory">The factory for creating login controls.</param>
	/// <param name="calculationTypesFactory">The factory for creating calculation types controls.</param>
	/// <param name="methodFamiliesFactory">The factory for creating method families controls.</param>
	/// <param name="baseMethodsFactory">The factory for creating base methods controls.</param>
	/// <param name="electronicStatesFactory">The factory for creating electronic states controls.</param>
	/// <param name="aboutFactory">The factory for creating about controls.</param>
	/// <param name="privacyFactory">The factory for creating privacy controls.</param>
	/// <param name="contactFactory">The factory for creating contact controls.</param>
	/// <param name="errorFactory">The factory for creating error controls.</param>
	public MainWindow(ILogger<MainWindow> logger, ILoggedInUserModel loggedInUser, IApiHelper apiHelper, HomeControl homeControl, IAbstractFactory<LoginControl> loginFactory, IAbstractFactory<CalculationTypesControl> calculationTypesFactory, IAbstractFactory<MethodFamiliesControl> methodFamiliesFactory, IAbstractFactory<BaseMethodsControl> baseMethodsFactory, IAbstractFactory<ElectronicStatesControl> electronicStatesFactory, IAbstractFactory<AboutControl> aboutFactory, IAbstractFactory<PrivacyControl> privacyFactory, IAbstractFactory<ContactControl> contactFactory, IAbstractFactory<ErrorControl> errorFactory)
	{
		_logger = logger;
		_loggedInUser = loggedInUser;
		_apiHelper = apiHelper;
		_homeControl = homeControl;
		_loginFactory = loginFactory;
		_calculationTypesFactory = calculationTypesFactory;
		_methodFamiliesFactory = methodFamiliesFactory;
		_baseMethodsFactory = baseMethodsFactory;
		_electronicStatesFactory = electronicStatesFactory;
		_aboutFactory = aboutFactory;
		_privacyFactory = privacyFactory;
		_contactFactory = contactFactory;
		_errorFactory = errorFactory;

		InitializeComponent();
	}

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Gets or sets a value indicating whether the user is currently logged in.
	/// </summary>
	/// <remarks>
	/// Setting this property also updates the <see cref="IsLoggedOut"/> property and modifies the window title.
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
	/// This property returns the inverse of <see cref="IsLoggedIn"/>.
	/// </remarks>
	public bool IsLoggedOut
	{
		get { return !IsLoggedIn; }
	}

	/// <summary>
	/// Raises the <see cref="PropertyChanged"/> event for the specified property.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed. This parameter is optional and can be automatically populated by the caller member name.</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {Method} called with CallerMemberName = {PropertyName}.", nameof(MainWindow), nameof(OnPropertyChanged), propertyName);
		}

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {Method} returning.", nameof(MainWindow), nameof(OnPropertyChanged));
		}
	}

	/// <inheritdoc/>
	protected override void OnInitialized(EventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {EventArgs}.", nameof(MainWindow), nameof(OnInitialized), e);
		}

		base.OnInitialized(e);
		DataContext = this;
		PropertyChanged += GaussianMainWindow_PropertyChanged;
		Application.Current.DispatcherUnhandledException += App_DispatcherUnhandledException; // 1. Handles exceptions on the main UI thread (Can prevent crash)
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException; // 2. Handles exceptions on background threads (Cannot prevent crash)
		_apiHelper.AuthenticationStateChanged += MainWindow_AuthenticationStateChanged;
		Assembly? assembly = Assembly.GetEntryAssembly();
		string appName = assembly?.GetName().Name ?? "GaussianWPF";
		string appVersion = assembly?.GetName().Version?.ToString() ?? "1.0.0.0";
		_apiHelper.InitializeApiClient(Settings.Default.ApiBaseAddress, appName, appVersion);
		MainContent.Content = _homeControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(OnInitialized));
		}
	}

	/// <inheritdoc/>
	protected override void OnClosing(CancelEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {EventArgs}.", nameof(MainWindow), nameof(OnClosing), e);
		}

		Settings.Default.Save();
		base.OnClosing(e);

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(OnClosing));
		}
	}

	private void GaussianMainWindow_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MainWindow), nameof(GaussianMainWindow_PropertyChanged), sender, e);
		}

		if (e.PropertyName is nameof(IsLoggedIn) or nameof(IsLoggedOut))
		{
			Title = IsLoggedIn ? $"Gaussian Desktop App — {_loggedInUser.UserName}" : "Gaussian Desktop App";
		}

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(GaussianMainWindow_PropertyChanged));
		}
	}

	private void MainWindow_AuthenticationStateChanged(object? sender, AuthenticationEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MainWindow), nameof(MainWindow_AuthenticationStateChanged), sender, e);
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

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(MainWindow_AuthenticationStateChanged));
		}
	}

	private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MainWindow), nameof(App_DispatcherUnhandledException), sender, e);
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

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(App_DispatcherUnhandledException));
		}
	}

	private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MainWindow), nameof(CurrentDomain_UnhandledException), sender, e);
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

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(CurrentDomain_UnhandledException));
		}
	}

	private void Home_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MainWindow), nameof(Home_Click), sender, e);
		}

		MainContent.Content = _homeControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(Home_Click));
		}
	}

	private void Login_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MainWindow), nameof(Login_Click), sender, e);
		}

		LoginControl loginControl = _loginFactory.Create();
		MainContent.Content = loginControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(Login_Click));
		}
	}

	private void Logout_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MainWindow), nameof(Logout_Click), sender, e);
		}

		_ = _apiHelper.LogoutAsync();
		MainContent.Content = _homeControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(Logout_Click));
		}
	}

	private void ExitApplication_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MainWindow), nameof(ExitApplication_Click), sender, e);
		}

		Close();

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(ExitApplication_Click));
		}
	}

	private void CalculationTypes_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MainWindow), nameof(CalculationTypes_Click), sender, e);
		}

		CalculationTypesControl calculationTypesControl = _calculationTypesFactory.Create();
		MainContent.Content = calculationTypesControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(CalculationTypes_Click));
		}
	}

	private void MethodFamilies_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MainWindow), nameof(MethodFamilies_Click), sender, e);
		}

		MethodFamiliesControl methodFamiliesControl = _methodFamiliesFactory.Create();
		MainContent.Content = methodFamiliesControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(MethodFamilies_Click));
		}
	}

	private void BaseMethods_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MainWindow), nameof(BaseMethods_Click), sender, e);
		}

		BaseMethodsControl baseMethodsControl = _baseMethodsFactory.Create();
		MainContent.Content = baseMethodsControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(BaseMethods_Click));
		}
	}

	private void ElectronicStates_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MainWindow), nameof(ElectronicStates_Click), sender, e);
		}

		ElectronicStatesControl electronicStatesControl = _electronicStatesFactory.Create();
		MainContent.Content = electronicStatesControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(ElectronicStates_Click));
		}
	}

	private void About_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MainWindow), nameof(About_Click), sender, e);
		}

		AboutControl aboutControl = _aboutFactory.Create();
		MainContent.Content = aboutControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(About_Click));
		}
	}

	private void Privacy_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MainWindow), nameof(Privacy_Click), sender, e);
		}

		PrivacyControl privacyControl = _privacyFactory.Create();
		MainContent.Content = privacyControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(Privacy_Click));
		}
	}

	private void Contact_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MainWindow), nameof(Contact_Click), sender, e);
		}

		ContactControl contactControl = _contactFactory.Create();
		MainContent.Content = contactControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(Contact_Click));
		}
	}

	private void Error_Click(object sender, RoutedEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Window} {EventHandler} called with {Sender} and {EventArgs}.", nameof(MainWindow), nameof(Error_Click), sender, e);
		}

		ErrorControl errorControl = _errorFactory.Create();
		MainContent.Content = errorControl;

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("{Window} {EventHandler} returning.", nameof(MainWindow), nameof(Error_Click));
		}
	}
}
