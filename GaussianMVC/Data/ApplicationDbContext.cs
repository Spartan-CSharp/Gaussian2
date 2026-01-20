using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GaussianMVC.Data;

/// <summary>
/// Represents the Entity Framework Core database context for the GaussianMVC application.
/// Extends <see cref="IdentityDbContext"/> to provide ASP.NET Core Identity support
/// including user authentication, roles, and related identity tables.
/// </summary>
/// <param name="options">
/// The options to be used by the <see cref="DbContext"/>.
/// Configures the database provider, connection string, and other context behaviors.
/// </param>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
}
