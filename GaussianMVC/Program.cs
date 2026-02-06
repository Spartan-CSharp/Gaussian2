using GaussianMVC;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
	.LicenseSyncfusion()
	.ConfigureIdentity()
	.ConfigureViewsAndPages()
	.ConfigureIdentityOptions()
	.ConfigureApiAndSwagger()
	.ConfigureAuthorizationAndAuthentication()
	.ConfigureApiHealthChecks()
	.ConfigureRequiredServices();

// Build the application
WebApplication app = builder.Build();

// Starting to log here as this is the earliest point at which an ILogger<Program> can be resolved from the service provider
ILogger<Program> logger = app.Services.GetRequiredService<ILogger<Program>>();

if (logger.IsEnabled(LogLevel.Information))
{
	logger.LogInformation("{Class} Top Level Statements Entry Point called with {ArgumentCount} command-line arguments: {Arguments}.", nameof(Program), args.Length, string.Join(", ", args));
}

app.SetStaticConfigurations()
	.ConfigurePipeline()
	.ConfigureAuthorizationAndAuthentication()
	.ConfigureMaps()
	.Run();

if (logger.IsEnabled(LogLevel.Information))
{
	logger.LogInformation("{Class} Top Level Statements Entry Point returning.", nameof(Program));
}
