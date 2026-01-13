using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GaussianMVC.Data;

/// <summary>
/// The ApplicationDbContext part of Entity Framework for the Individual Accounts authentication and authorization
/// </summary>
/// <param name="options">The options for the ApplicationDbContext</param>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
}
