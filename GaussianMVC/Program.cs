using System.Reflection;
using System.Text;

using GaussianMVC.Data;

using GaussianMVCLibrary.DataAccess;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("GaussianIdentity") ?? throw new InvalidOperationException("Connection string 'GaussianIdentity' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
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
	options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT",
		Description = "JWT Authorization header using the Bearer scheme."
	});
	options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
	{
		[new OpenApiSecuritySchemeReference("bearer", document)] = []
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
	.AddPolicy("AdminAndUserPolicy", policy => policy.RequireRole("Admin", "User"))
	.AddPolicy("AdminOnlyPolicy", policy => policy.RequireRole("Admin"));
	// TODO: once the main stuff is done, figure out how to not require authorization on the Home page and other pages, then put in the Fallback Policy again
	// NOTE: when I tried simply "AllowAnonymous" on the Home Controller's Index method, it allowed access to the page, but none of the view info was brought in.  Probably needs something on the View side as well.
	//.SetFallbackPolicy(new AuthorizationPolicyBuilder()
	//		.RequireAuthenticatedUser()
	//		.Build());
builder.Services.AddAuthentication("Bearer")
			.AddJwtBearer(opts =>
			{
				opts.TokenValidationParameters = new()
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
			});
builder.Services.AddHealthChecks()
			.AddSqlServer(builder.Configuration.GetConnectionString("GaussianData") ?? throw new InvalidOperationException("Connection string 'GaussianData' not found."));

builder.Services.AddSingleton<IDbData, SqlData>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	_ = app.UseMigrationsEndPoint();
	_ = app.MapOpenApi();
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

app.MapStaticAssets();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}")
	.WithStaticAssets();

app.MapRazorPages()
	.WithStaticAssets();

app.MapHealthChecks("/health").AllowAnonymous();

app.Run();
