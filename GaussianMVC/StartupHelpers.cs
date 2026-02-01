using System.Reflection;
using System.Text;

using GaussianMVC.Data;
using GaussianMVC.Properties;

using GaussianMVCLibrary.DataAccess;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace GaussianMVC;

internal static class StartupHelpers
{
	internal static WebApplicationBuilder ConfigureIdentity(this WebApplicationBuilder builder)
	{
		string connectionString = builder.Configuration.GetConnectionString(Resources.IDDatabaseConnectionStringName) ?? throw new InvalidOperationException($"Connection string '{Resources.IDDatabaseConnectionStringName}' not found.");
		_ = builder.Services
			.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString))
			.AddDatabaseDeveloperPageExceptionFilter();
		_ = builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddRoles<IdentityRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>();
		return builder;
	}

	internal static WebApplicationBuilder ConfigureViewsAndPages(this WebApplicationBuilder builder)
	{
		_ = builder.Services
			.AddControllersWithViews()
			// Disabling automatic 400 responses on model validation errors to allow custom handling in each controller
			.ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

		_ = builder.Services
			.AddRazorPages(options =>
				{
					_ = options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage")
						.AllowAnonymousToAreaPage("Identity", "/Account/ExternalLogin")
						.AllowAnonymousToAreaPage("Identity", "/Account/ForgotPassword")
						.AllowAnonymousToAreaPage("Identity", "/Account/ForgotPasswordConfirmation")
						.AllowAnonymousToAreaPage("Identity", "/Account/Lockout")
						.AllowAnonymousToAreaPage("Identity", "/Account/Login")
						.AllowAnonymousToAreaPage("Identity", "/Account/Register")
						.AllowAnonymousToAreaPage("Identity", "/Account/RegisterConfirmation")
						.AllowAnonymousToAreaPage("Identity", "/Account/ResendEmailConfirmation")
						.AllowAnonymousToAreaPage("Identity", "/Account/ResetPasswordConfirmation")
						.AllowAnonymousToAreaPage("Identity", "/Error");
					// Add other public Identity pages as needed
				});

		return builder;
	}

	internal static WebApplicationBuilder ConfigureIdentityOptions(this WebApplicationBuilder builder)
	{
		_ = builder.Services
			.Configure<IdentityOptions>(options =>
				{
					// Default Lockout settings.
					options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
					options.Lockout.MaxFailedAccessAttempts = 5;
					options.Lockout.AllowedForNewUsers = true;

					// Default Password settings.
					options.Password.RequireDigit = true;
					options.Password.RequireLowercase = true;
					options.Password.RequireNonAlphanumeric = true;
					options.Password.RequireUppercase = true;
					options.Password.RequiredLength = 6;
					options.Password.RequiredUniqueChars = 1;

					// Default SignIn settings.
					options.SignIn.RequireConfirmedEmail = false;
					options.SignIn.RequireConfirmedPhoneNumber = false;

					// Default User settings.
					options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
					options.User.RequireUniqueEmail = false;
				})
			.ConfigureApplicationCookie(options =>
				{
					options.AccessDeniedPath = "/Identity/Account/AccessDenied";
					options.Cookie.Name = "GaussianWebApplication";
					options.Cookie.HttpOnly = true;
					options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
					options.LoginPath = "/Identity/Account/Login";
					// ReturnUrlParameter requires using Microsoft.AspNetCore.Authentication.Cookies;
					options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
					options.SlidingExpiration = true;
				});

		return builder;
	}

	internal static WebApplicationBuilder ConfigureApiAndSwagger(this WebApplicationBuilder builder)
	{
		// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
		_ = builder.Services
			.AddOpenApi()
			.AddEndpointsApiExplorer()
			.AddSwaggerGen(options =>
				{
					options.SwaggerDoc("v1", new OpenApiInfo
					{
						Version = "v1",
						Title = "Gaussian API",
						Description = "An API to allow for storing and indexing of results of electronic structure calculations performed with the Gaussian series of programs.",
						//TermsOfService = new Uri("https://github.com/pjlplourde"),
						Contact = new OpenApiContact
						{
							Name = "Pierre J.-L. Plourde",
							Url = new Uri("https://github.com/pjlplourde")
						},
						License = new OpenApiLicense
						{
							Name = "MIT License",
							Url = new Uri("https://github.com/Spartan-CSharp/Gaussian2/blob/master/LICENSE")
						}
					});
					string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
					options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
					options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
					{
						Name = "Authorization",
						In = ParameterLocation.Header,
						Type = SecuritySchemeType.Http,
						Scheme = "Bearer",
						BearerFormat = "JWT",
						Description = "JWT Authorization header using the Bearer scheme."
					});
					options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
					{
						[new OpenApiSecuritySchemeReference("Bearer", document)] = []
					});
				})
			.AddApiVersioning(options =>
				{
					options.AssumeDefaultVersionWhenUnspecified = true;
					options.DefaultApiVersion = new(1, 0);
					options.ReportApiVersions = true;
				})
			.AddMvc()
			.AddApiExplorer(options =>
				{
					options.GroupNameFormat = "'v'VVV";
					options.SubstituteApiVersionInUrl = true;
				});

		return builder;
	}

	internal static WebApplicationBuilder ConfigureAuthorizationAndAuthentication(this WebApplicationBuilder builder)
	{
		_ = builder.Services
			.AddAuthorizationBuilder()
			.AddPolicy("AdministratorPolicy", policy => policy.RequireRole("Administrator"))
			.AddPolicy("FullUserPolicy", policy => policy.RequireRole("Administrator", "FullUser"))
			.AddPolicy("EditingUserPolicy", policy => policy.RequireRole("Administrator", "FullUser", "EditingUser"))
			.AddPolicy("CreatingUserPolicy", policy => policy.RequireRole("Administrator", "FullUser", "EditingUser", "CreatingUser"))
			.AddPolicy("ViewOnlyUserPolicy", policy => policy.RequireRole("Administrator", "FullUser", "EditingUser", "CreatingUser", "ViewOnlyUser"))
			.SetFallbackPolicy(new AuthorizationPolicyBuilder()
				.RequireAuthenticatedUser()
				.Build());

		_ = builder.Services
			.AddAuthentication(options =>
				{
					options.DefaultScheme = "MultiScheme";
					options.DefaultChallengeScheme = "MultiScheme";
				})
			.AddJwtBearer("Bearer", bearerOptions =>
				{
					bearerOptions.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
						ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
						ValidateLifetime = true,
						ClockSkew = TimeSpan.FromMinutes(5),
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Authentication:SecretKey")!))
					};
				})
			.AddPolicyScheme("MultiScheme", "Cookie or Bearer", options =>
				{
					options.ForwardDefaultSelector = context =>
						// Use Bearer for API requests, Cookie for everything else
						context.Request.Path.StartsWithSegments("/api", StringComparison.InvariantCultureIgnoreCase) ? "Bearer" : IdentityConstants.ApplicationScheme;
				});

		return builder;
	}

	internal static WebApplicationBuilder ConfigureApiHealthChecks(this WebApplicationBuilder builder)
	{
		//builder.Services
		//	.AddHealthChecks()
		//	.AddSqlServer(builder.Configuration.GetConnectionString(Resources.DataDatabaseConnectionStringName) ?? throw new InvalidOperationException($"Connection string '{Resources.DataDatabaseConnectionStringName}' not found."));

		return builder;
	}

	internal static WebApplicationBuilder ConfigureRequiredServices(this WebApplicationBuilder builder)
	{
		_ = builder.Services
			.AddSingleton<IDbData, SqlData>()
			.AddTransient<ICalculationTypesCrud, CalculationTypesCrud>()
			.AddTransient<IMethodFamiliesCrud, MethodFamiliesCrud>()
			.AddTransient<IBaseMethodsCrud, BaseMethodsCrud>()
			.AddTransient<IElectronicStatesCrud, ElectronicStatesCrud>();

		return builder;
	}

	internal static WebApplication ConfigurePipeline(this WebApplication app)
	{
		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			_ = app.UseMigrationsEndPoint();
			_ = app.MapOpenApi().AllowAnonymous();
			_ = app.UseSwagger();
			_ = app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
		}
		else
		{
			_ = app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			_ = app.UseHsts();
		}

		_ = app.UseHttpsRedirection();
		_ = app.UseRouting();

		return app;
	}

	internal static WebApplication ConfigureAuthorizationAndAuthentication(this WebApplication app)
	{
		_ = app.UseAuthentication();
		_ = app.UseAuthorization();

		return app;
	}

	internal static WebApplication ConfigureMaps(this WebApplication app)
	{
		_ = app.MapStaticAssets()
			.AllowAnonymous();

		_ = app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}")
			.WithStaticAssets();

		_ = app.MapRazorPages()
			.WithStaticAssets();

		//_ = app.MapHealthChecks("/api/health").AllowAnonymous();

		return app;
	}
}
