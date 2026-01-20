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

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString(Resources.IDDatabaseConnectionStringName) ?? throw new InvalidOperationException($"Connection string '{Resources.IDDatabaseConnectionStringName}' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(options =>
{
	_ = options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
	_ = options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/ExternalLogin");
	_ = options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/ForgotPassword");
	_ = options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/ForgotPasswordConfirmation");
	_ = options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Lockout");
	_ = options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Login");
	_ = options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Register");
	_ = options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/RegisterConfirmation");
	_ = options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/ResendEmailConfirmation");
	_ = options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/ResetPasswordConfirmation");
	_ = options.Conventions.AllowAnonymousToAreaPage("Identity", "/Error");
	// Add other public Identity pages as needed
});
builder.Services.Configure<IdentityOptions>(options =>
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
	options.User.AllowedUserNameCharacters =
			"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	options.User.RequireUniqueEmail = false;
});
builder.Services.ConfigureApplicationCookie(options =>
{
	options.AccessDeniedPath = "/Identity/Account/AccessDenied";
	options.Cookie.Name = "GaussianWebApplication";
	options.Cookie.HttpOnly = true;
	options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
	options.LoginPath = "/Identity/Account/Login";
	// ReturnUrlParameter requires 
	//using Microsoft.AspNetCore.Authentication.Cookies;
	options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
	options.SlidingExpiration = true;
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
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
});
builder.Services.AddApiVersioning(options =>
{
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.DefaultApiVersion = new(1, 0);
	options.ReportApiVersions = true;
}).AddMvc().AddApiExplorer(options =>
{
	options.GroupNameFormat = "'v'VVV";
	options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddAuthorizationBuilder()
	//	.AddPolicy("AdminAndUserPolicy", policy => policy.RequireRole("Admin", "User"))
	//	.AddPolicy("AdminOnlyPolicy", policy => policy.RequireRole("Admin"));
	//	.AddPolicy("UserOnlyPolicy", policy => policy.RequireRole("User"))
	.SetFallbackPolicy(new AuthorizationPolicyBuilder()
			.RequireAuthenticatedUser()
			.Build());
builder.Services.AddAuthentication(options =>
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
	{
		// Use Bearer for API requests, Cookie for everything else
		return context.Request.Path.StartsWithSegments("/api", StringComparison.InvariantCultureIgnoreCase)
			? "Bearer"
			: IdentityConstants.ApplicationScheme;
	};
});

//builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString(Resources.DataDatabaseConnectionStringName) ?? throw new InvalidOperationException($"Connection string '{Resources.DataDatabaseConnectionStringName}' not found."));

builder.Services.AddSingleton<IDbData, SqlData>();
builder.Services.AddTransient<ICalculationTypesCrud, CalculationTypesCrud>();

WebApplication app = builder.Build();

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

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets().AllowAnonymous();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}")
	.WithStaticAssets();

app.MapRazorPages()
	.WithStaticAssets();

//app.MapHealthChecks("/api/health").AllowAnonymous();

app.Run();
