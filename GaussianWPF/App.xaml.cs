using System.Windows;

using GaussianWPF.Controls;
using GaussianWPF.Controls.BaseMethods;
using GaussianWPF.Controls.CalculationTypes;
using GaussianWPF.Controls.ElectronicStates;
using GaussianWPF.Controls.ElectronicStatesMethodFamilies;
using GaussianWPF.Controls.FullMethods;
using GaussianWPF.Controls.MethodFamilies;
using GaussianWPF.Controls.SpinStates;
using GaussianWPF.Controls.SpinStatesElectronicStatesMethodFamilies;
using GaussianWPF.FactoryHelpers;

using GaussianWPFLibrary.DataAccess;
using GaussianWPFLibrary.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GaussianWPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	private readonly ILogger<App> _logger;

	/// <summary>
	/// Initializes a new instance of the <see cref="App"/> class and configures the application host with dependency injection.
	/// </summary>
	public App()
	{
		AppHost = Host.CreateDefaultBuilder()
			.ConfigureServices(ConfigureServices)
			.Build();
		_logger = AppHost.Services.GetRequiredService<ILogger<App>>();
	}

	/// <summary>
	/// Gets the application's host instance used for dependency injection and service management.
	/// </summary>
	public static IHost? AppHost { get; private set; }

	/// <inheritdoc/>
	/// <summary>
	/// Raises the <see cref="Application.Startup"/> event and initializes the application host and main window.
	/// </summary>
	protected override void OnStartup(StartupEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Class} {EventHandler} with {EventArgs} called.", nameof(App), nameof(OnStartup), e);
		}

		AppHost!.StartAsync().Wait();
		base.OnStartup(e);
		MainWindow? mainWindow = AppHost.Services.GetService<MainWindow>();
		mainWindow?.Show();

		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Class} {EventHandler} with {EventArgs} returning.", nameof(App), nameof(OnStartup), e);
		}
	}

	/// <inheritdoc/>
	/// <summary>
	/// Raises the <see cref="Application.Exit"/> event and performs application cleanup, including stopping the host.
	/// </summary>
	protected override void OnExit(ExitEventArgs e)
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Class} {EventHandler} with {EventArgs} called.", nameof(App), nameof(OnExit), e);
		}

		using (AppHost)
		{
			AppHost!.StopAsync().Wait();
		}

		base.OnExit(e);

		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("{Class} {EventHandler} with {EventArgs} returning.", nameof(App), nameof(OnExit), e);
		}
	}

	private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
	{
		// Register your services
		_ = services.AddSingleton<IApiHelper, ApiHelper>();
		_ = services.AddSingleton<IHomeEndpoint, HomeEndpoint>();
		_ = services.AddSingleton<ICalculationTypesEndpoint, CalculationTypesEndpoint>();
		_ = services.AddSingleton<IMethodFamiliesEndpoint, MethodFamiliesEndpoint>();
		_ = services.AddSingleton<IBaseMethodsEndpoint, BaseMethodsEndpoint>();
		_ = services.AddSingleton<IElectronicStatesEndpoint, ElectronicStatesEndpoint>();
		_ = services.AddSingleton<IElectronicStatesMethodFamiliesEndpoint, ElectronicStatesMethodFamiliesEndpoint>();
		_ = services.AddSingleton<ISpinStatesEndpoint, SpinStatesEndpoint>();
		_ = services.AddSingleton<ILoggedInUserModel, LoggedInUserModel>();
		_ = services.AddSingleton<ISpinStatesElectronicStatesMethodFamiliesEndpoint, SpinStatesElectronicStatesMethodFamiliesEndpoint>();
		_ = services.AddSingleton<IFullMethodsEndpoint, FullMethodsEndpoint>();

		// Register your Views and ViewModels
		_ = services.AddSingleton<MainWindow>();
		_ = services.AddSingleton<HomeControl>();
		_ = services.AddFormFactory<LoginControl>();
		_ = services.AddFormFactory<CalculationTypesControl>();
		_ = services.AddFormFactory<MethodFamiliesControl>();
		_ = services.AddFormFactory<BaseMethodsControl>();
		_ = services.AddFormFactory<ElectronicStatesControl>();
		_ = services.AddFormFactory<ElectronicStatesMethodFamiliesControl>();
		_ = services.AddFormFactory<SpinStatesControl>();
		_ = services.AddFormFactory<SpinStatesElectronicStatesMethodFamiliesControl>();
		_ = services.AddFormFactory<FullMethodsControl>();
		_ = services.AddFormFactory<AboutControl>();
		_ = services.AddFormFactory<PrivacyControl>();
		_ = services.AddFormFactory<ContactControl>();
		_ = services.AddFormFactory<ErrorControl>();

		// You can register other views/viewmodels as needed, e.g.
		_ = services.AddFormFactory<CalculationTypesIndexControl>();
		_ = services.AddFormFactory<CalculationTypesDetailsControl>();
		_ = services.AddFormFactory<CalculationTypesCreateControl>();
		_ = services.AddFormFactory<CalculationTypesEditControl>();
		_ = services.AddFormFactory<CalculationTypesDeleteControl>();
		_ = services.AddFormFactory<MethodFamiliesIndexControl>();
		_ = services.AddFormFactory<MethodFamiliesDetailsControl>();
		_ = services.AddFormFactory<MethodFamiliesCreateControl>();
		_ = services.AddFormFactory<MethodFamiliesEditControl>();
		_ = services.AddFormFactory<MethodFamiliesDeleteControl>();
		_ = services.AddFormFactory<BaseMethodsIndexControl>();
		_ = services.AddFormFactory<BaseMethodsDetailsControl>();
		_ = services.AddFormFactory<BaseMethodsCreateControl>();
		_ = services.AddFormFactory<BaseMethodsEditControl>();
		_ = services.AddFormFactory<BaseMethodsDeleteControl>();
		_ = services.AddFormFactory<ElectronicStatesIndexControl>();
		_ = services.AddFormFactory<ElectronicStatesDetailsControl>();
		_ = services.AddFormFactory<ElectronicStatesCreateControl>();
		_ = services.AddFormFactory<ElectronicStatesEditControl>();
		_ = services.AddFormFactory<ElectronicStatesDeleteControl>();
		_ = services.AddFormFactory<ElectronicStatesMethodFamiliesIndexControl>();
		_ = services.AddFormFactory<ElectronicStatesMethodFamiliesDetailsControl>();
		_ = services.AddFormFactory<ElectronicStatesMethodFamiliesCreateControl>();
		_ = services.AddFormFactory<ElectronicStatesMethodFamiliesEditControl>();
		_ = services.AddFormFactory<ElectronicStatesMethodFamiliesDeleteControl>();
		_ = services.AddFormFactory<SpinStatesIndexControl>();
		_ = services.AddFormFactory<SpinStatesDetailsControl>();
		_ = services.AddFormFactory<SpinStatesCreateControl>();
		_ = services.AddFormFactory<SpinStatesEditControl>();
		_ = services.AddFormFactory<SpinStatesDeleteControl>();
		_ = services.AddFormFactory<SpinStatesElectronicStatesMethodFamiliesIndexControl>();
		_ = services.AddFormFactory<SpinStatesElectronicStatesMethodFamiliesDetailsControl>();
		_ = services.AddFormFactory<SpinStatesElectronicStatesMethodFamiliesCreateControl>();
		_ = services.AddFormFactory<SpinStatesElectronicStatesMethodFamiliesEditControl>();
		_ = services.AddFormFactory<SpinStatesElectronicStatesMethodFamiliesDeleteControl>();
		_ = services.AddFormFactory<FullMethodsIndexControl>();
		_ = services.AddFormFactory<FullMethodsDetailsControl>();
		_ = services.AddFormFactory<FullMethodsCreateControl>();
		_ = services.AddFormFactory<FullMethodsEditControl>();
		_ = services.AddFormFactory<FullMethodsDeleteControl>();
	}
}
