using System.Windows;

using GaussianWPF.Controls;
using GaussianWPF.Controls.CalculationTypes;
using GaussianWPF.FactoryHelpers;

using GaussianWPFLibrary.DataAccess;
using GaussianWPFLibrary.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GaussianWPF;

/// <summary>
/// Interaction logic for App.xaml
/// Represents the main application class that configures dependency injection and manages the application lifecycle.
/// </summary>
public partial class App : Application
{
	/// <summary>
	/// Gets the application's dependency injection host that manages service lifetimes and resolution.
	/// This property provides access to the configured services throughout the application.
	/// </summary>
	public static IHost? AppHost { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="App"/> class.
	/// Creates and configures the dependency injection host with all required services.
	/// </summary>
	public App()
	{
		AppHost = Host.CreateDefaultBuilder()
			.ConfigureServices(ConfigureServices)
			.Build();
	}

	/// <summary>
	/// Configures the dependency injection container with application services, views, and controls.
	/// </summary>
	/// <param name="context">The host builder context containing configuration and environment information.</param>
	/// <param name="services">The service collection to register dependencies into.</param>
	/// <remarks>
	/// Registers the following services:
	/// - Singleton instances of ILoggedInUserModel and IApiHelper for data access
	/// - Singleton instances of MainWindow and HomeControl
	/// - Form factories for LoginControl, AboutControl, and PrivacyControl to enable dynamic creation
	/// </remarks>
	private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
	{
		// Register your services
		_ = services.AddSingleton<IApiHelper, ApiHelper>();
		_ = services.AddSingleton<ICalculationTypesEndpoint, CalculationTypesEndpoint>();
		_ = services.AddSingleton<ILoggedInUserModel, LoggedInUserModel>();

		// Register your Views and ViewModels
		_ = services.AddSingleton<MainWindow>();
		_ = services.AddSingleton<HomeControl>();
		_ = services.AddFormFactory<LoginControl>();
		_ = services.AddFormFactory<CalculationTypesControl>();
		_ = services.AddFormFactory<AboutControl>();
		_ = services.AddFormFactory<PrivacyControl>();
		_ = services.AddFormFactory<ErrorControl>();

		// You can register other views/viewmodels as needed, e.g.
		_ = services.AddFormFactory<CalculationTypesIndexControl>();
		_ = services.AddFormFactory<CalculationTypesDetailsControl>();
		_ = services.AddFormFactory<CalculationTypesCreateControl>();
		_ = services.AddFormFactory<CalculationTypesEditControl>();
		_ = services.AddFormFactory<CalculationTypesDeleteControl>();
	}

	/// <summary>
	/// Called when the application is shutting down.
	/// Ensures proper cleanup by stopping and disposing of the dependency injection host.
	/// </summary>
	/// <param name="e">The event arguments containing information about the application exit.</param>
	protected override void OnExit(ExitEventArgs e)
	{
		using (AppHost)
		{
			AppHost!.StopAsync().Wait();
		}

		base.OnExit(e);
	}

	/// <summary>
	/// Called when the application starts.
	/// Initializes the dependency injection host and displays the main window.
	/// </summary>
	/// <param name="e">The event arguments containing startup information and command-line arguments.</param>
	protected override void OnStartup(StartupEventArgs e)
	{
		AppHost!.StartAsync().Wait();

		MainWindow? mainWindow = AppHost.Services.GetService<MainWindow>();
		mainWindow?.Show();

		base.OnStartup(e);
	}
}
