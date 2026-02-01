using GaussianMVC;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// Register code page encodings required by RtfPipe (must be before any RtfPipe usage)
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JGaF5cXGpCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdlWX5dcXVVRWhYUEdzV0BWYEs=");

// Add services to the container.
builder.ConfigureIdentity()
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

app.ConfigurePipeline()
	.ConfigureAuthorizationAndAuthentication()
	.ConfigureMaps()
	.Run();

if (logger.IsEnabled(LogLevel.Information))
{
	logger.LogInformation("{Class} Top Level Statements Entry Point returning.", nameof(Program));
}
