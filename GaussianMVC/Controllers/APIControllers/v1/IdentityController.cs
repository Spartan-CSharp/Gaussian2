using System.Data;
using System.Globalization;
using System.Security.Claims;
using System.Text;

using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GaussianMVC.Controllers.APIControllers.V1;

/// <summary>
/// API controller for managing ASP.NET Core Identity users, roles, and claims.
/// </summary>
/// <remarks>
/// Provides RESTful endpoints for CRUD operations on users, roles, claims, and their relationships.
/// All endpoints are versioned and accessible under the api/v1/Identity route.
/// </remarks>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize(Policy = "AdministratorPolicy")]
public class IdentityController(ILogger<IdentityController> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : ControllerBase
{
	private readonly ILogger<IdentityController> _logger = logger;
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly RoleManager<IdentityRole> _roleManager = roleManager;

	/// <summary>
	/// Represents user data for creating or registering new identity users.
	/// </summary>
	/// <param name="UserName">The username for the new user account.</param>
	/// <param name="Email">The email address for the new user account.</param>
	/// <param name="Password">The password for the new user account.</param>
	public record UserData(string UserName, string Email, string Password);

	/// <summary>
	/// Represents roleData data for creating or managing identity roles.
	/// </summary>
	/// <param name="RoleName">The name of the roleData.</param>
	public record RoleData(string RoleName);

	/// <summary>
	/// Represents claim data for creating or modifying claims.
	/// </summary>
	/// <param name="ClaimType">The type of the claim.</param>
	/// <param name="ClaimValue">The value of the claim.</param>
	public record ClaimData(string ClaimType, string ClaimValue);

	/// <summary>
	/// Retrieves all users in the system.
	/// </summary>
	/// <returns>A list of all identity users.</returns>
	/// <response code="200">Returns the list of all users.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET api/v1/Identity/Users
	[HttpGet("Users")]
	public async Task<ActionResult<List<IdentityUser>>> GetAllUsersAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(IdentityController), nameof(GetAllUsersAsync));
			}

			List<IdentityUser> output = [.. _userManager.Users];

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(IdentityController), nameof(GetAllUsersAsync), output.Count, nameof(IdentityUser));
			}

			return Ok(output);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(IdentityController), nameof(GetAllUsersAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a specific user by their ID.
	/// </summary>
	/// <param name="id">The unique identifier of the user.</param>
	/// <returns>The identity user with the specified ID.</returns>
	/// <response code="200">Returns the requested user.</response>
	/// <response code="400">If no user exists with the specified ID.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET api/v1/Identity/Users/f755eafe-b9d1-4028-b060-ee12002b8c0c
	[HttpGet("Users/{id}")]
	public async Task<ActionResult<IdentityUser>> GetUserAsync(string id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(IdentityController), nameof(GetUserAsync), id);
			}

			IdentityUser? user = _userManager.Users.FirstOrDefault(u => u.Id == id);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), nameof(GetUserAsync), id, nameof(IdentityUser), user);
			}

			return user is null ? (ActionResult<IdentityUser>)NotFound($"No User exists with the supplied Id {id}.") : (ActionResult<IdentityUser>)Ok(user);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(IdentityController), nameof(GetUserAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Creates a new user.
	/// </summary>
	/// <param name="userData">The identity user object to create.</param>
	/// <returns>The created identity user.</returns>
	/// <response code="201">Returns the newly created user.</response>
	/// <response code="400">If the user creation fails.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// POST api/v1/Identity/Users
	[HttpPost("Users")]
	public async Task<ActionResult<IdentityUser>> CreateUserAsync([FromBody] UserData userData)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), nameof(CreateUserAsync), nameof(UserData), userData);
			}

			ArgumentNullException.ThrowIfNull(userData, nameof(userData));

			IdentityResult result = await _userManager.CreateAsync(new IdentityUser
			{
				UserName = userData.UserName,
				Email = userData.Email
			}, userData.Password).ConfigureAwait(false);

			if (result.Succeeded)
			{
				IdentityUser createdUser = _userManager.Users.First(u => u.UserName == userData.UserName);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), nameof(CreateUserAsync), nameof(IdentityUser), createdUser);
				}

				return CreatedAtAction(nameof(CreateUserAsync), RouteData.Values, createdUser);
			}
			else
			{
				StringBuilder sb = new();

				foreach (IdentityError item in result.Errors)
				{
					_ = sb.AppendLine(CultureInfo.InvariantCulture, $"\t{item.Code}: {item.Description}");
				}

				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more identity errors occur:\n{IdentityErrors}", HttpContext.Request.Method, nameof(IdentityController), nameof(CreateUserAsync), nameof(UserData), userData, sb.ToString());
				}

				return BadRequest(result.Errors);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(IdentityController), nameof(CreateUserAsync), nameof(UserData), userData);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Updates an existing user.
	/// </summary>
	/// <param name="id">The unique identifier of the user to update.</param>
	/// <param name="user">The updated identity user object.</param>
	/// <returns>The updated identity user.</returns>
	/// <response code="200">Returns the updated user.</response>
	/// <response code="400">If the ID mismatch occurs or update fails.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// PUT api/v1/Identity/Users/f755eafe-b9d1-4028-b060-ee12002b8c0c
	[HttpPut("Users/{id}")]
	public async Task<ActionResult<IdentityUser>> UpdateUserAsync(string id, [FromBody] IdentityUser user)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), nameof(UpdateUserAsync), id, nameof(IdentityUser), user);
			}

			if (user?.Id == id)
			{
				IdentityResult result = await _userManager.UpdateAsync(user).ConfigureAwait(false);

				if (result.Succeeded)
				{
					IdentityUser updatedUser = _userManager.Users.First(u => u.Id == id);

					if (_logger.IsEnabled(LogLevel.Debug))
					{
						_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), nameof(UpdateUserAsync), id, nameof(IdentityUser), updatedUser);
					}

					return Ok(updatedUser);
				}
				else
				{
					StringBuilder sb = new();

					foreach (IdentityError item in result.Errors)
					{
						_ = sb.AppendLine(CultureInfo.InvariantCulture, $"\t{item.Code}: {item.Description}");
					}

					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more identity errors occur:\n{IdentityErrors}", HttpContext.Request.Method, nameof(IdentityController), nameof(UpdateUserAsync), nameof(IdentityUser), user, sb.ToString());
					}

					return BadRequest(result.Errors);
				}
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(IdentityController), nameof(UpdateUserAsync), id, nameof(IdentityUser), user);
				}

				return BadRequest($"The route parameter Id {id} does not match the User Id {user?.Id} from the request body.");
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(IdentityController), nameof(UpdateUserAsync), id, nameof(IdentityUser), user);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Deletes a user by their ID.
	/// </summary>
	/// <param name="id">The unique identifier of the user to delete.</param>
	/// <returns>Success status if the user was deleted.</returns>
	/// <response code="200">If the user was successfully deleted.</response>
	/// <response code="400">If the deletion fails.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// DELETE api/v1/Identity/Users/f755eafe-b9d1-4028-b060-ee12002b8c0c
	[HttpDelete("Users/{id}")]
	public async Task<ActionResult> DeleteUserAsync(string id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(IdentityController), nameof(DeleteUserAsync), id);
			}

			IdentityUser user = _userManager.Users.First(u => u.Id == id);
			IdentityResult result = await _userManager.DeleteAsync(user).ConfigureAwait(false);

			if (result.Succeeded)
			{
				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(IdentityController), nameof(DeleteUserAsync), id);
				}

				return Ok();
			}
			else
			{
				StringBuilder sb = new();

				foreach (IdentityError item in result.Errors)
				{
					_ = sb.AppendLine(CultureInfo.InvariantCulture, $"\t{item.Code}: {item.Description}");
				}

				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} had one or more identity errors occur:\n{IdentityErrors}", HttpContext.Request.Method, nameof(IdentityController), nameof(DeleteUserAsync), sb.ToString());
				}

				return BadRequest(result.Errors);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(IdentityController), nameof(DeleteUserAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all roles in the system.
	/// </summary>
	/// <returns>A list of all identity roles.</returns>
	/// <response code="200">Returns the list of all roles.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET api/v1/Identity/Roles
	[HttpGet("Roles")]
	public async Task<ActionResult<List<IdentityRole>>> GetAllRolesAsync()
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called.", HttpContext.Request.Method, nameof(IdentityController), nameof(GetAllRolesAsync));
			}

			List<IdentityRole> output = [.. _roleManager.Roles];

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} returning {ModelCount} {ModelName}.", HttpContext.Request.Method, nameof(IdentityController), nameof(GetAllRolesAsync), output.Count, nameof(IdentityRole));
			}

			return Ok(output);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} had an error.", HttpContext.Request.Method, nameof(IdentityController), nameof(GetAllRolesAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves a specific roleData by its ID.
	/// </summary>
	/// <param name="id">The unique identifier of the roleData.</param>
	/// <returns>The identity roleData with the specified ID.</returns>
	/// <response code="200">Returns the requested roleData.</response>
	/// <response code="400">If no roleData exists with the specified ID.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET api/v1/Identity/Roles/f755eafe-b9d1-4028-b060-ee12002b8c0c
	[HttpGet("Roles/{id}")]
	public async Task<ActionResult<IdentityRole>> GetRoleAsync(string id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(IdentityController), nameof(GetRoleAsync), id);
			}

			IdentityRole? role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), nameof(GetRoleAsync), id, nameof(IdentityRole), role);
			}

			return role is null ? (ActionResult<IdentityRole>)NotFound($"No Role exists with the supplied Id {id}.") : (ActionResult<IdentityRole>)Ok(role);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(IdentityController), nameof(GetRoleAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Creates a new role.
	/// </summary>
	/// <param name="roleData">The identity role object to create.</param>
	/// <returns>The created identity role.</returns>
	/// <response code="201">Returns the newly created role.</response>
	/// <response code="400">If the role creation fails.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// POST api/v1/Identity/Roles
	[HttpPost("Roles")]
	public async Task<ActionResult<IdentityRole>> CreateRoleAsync([FromBody] RoleData roleData)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), nameof(CreateRoleAsync), nameof(RoleData), roleData);
			}

			ArgumentNullException.ThrowIfNull(roleData, nameof(roleData));

			IdentityResult result = await _roleManager.CreateAsync(new IdentityRole
			{
				Name = roleData.RoleName
			}).ConfigureAwait(false);

			if (result.Succeeded)
			{
				IdentityRole createdRole = _roleManager.Roles.First(r => r.Name == roleData.RoleName);

				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), nameof(CreateRoleAsync), nameof(IdentityRole), createdRole);
				}

				return CreatedAtAction(nameof(CreateRoleAsync), RouteData.Values, createdRole);
			}
			else
			{
				StringBuilder sb = new();

				foreach (IdentityError item in result.Errors)
				{
					_ = sb.AppendLine(CultureInfo.InvariantCulture, $"\t{item.Code}: {item.Description}");
				}

				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more identity errors occur:\n{IdentityErrors}", HttpContext.Request.Method, nameof(IdentityController), nameof(CreateUserAsync), nameof(RoleData), roleData, sb.ToString());
				}

				return BadRequest(result.Errors);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(IdentityController), nameof(CreateRoleAsync), nameof(RoleData), roleData);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Updates an existing roleData.
	/// </summary>
	/// <param name="id">The unique identifier of the roleData to update.</param>
	/// <param name="role">The updated identity roleData object.</param>
	/// <returns>The updated identity roleData.</returns>
	/// <response code="200">Returns the updated roleData.</response>
	/// <response code="400">If the ID mismatch occurs or update fails.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// PUT api/v1/Identity/Roles/f755eafe-b9d1-4028-b060-ee12002b8c0c
	[HttpPut("Roles/{id}")]
	public async Task<ActionResult<IdentityRole>> UpdateRoleAsync(string id, [FromBody] IdentityRole role)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), nameof(UpdateRoleAsync), id, nameof(IdentityRole), role);
			}

			if (role?.Id == id)
			{
				IdentityResult result = await _roleManager.UpdateAsync(role).ConfigureAwait(false);

				if (result.Succeeded)
				{
					IdentityRole updatedRole = _roleManager.Roles.First(r => r.Id == id);

					if (_logger.IsEnabled(LogLevel.Debug))
					{
						_logger.LogDebug("{Method} {Controller} {Action} {Id} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), nameof(UpdateRoleAsync), id, nameof(IdentityRole), updatedRole);
					}

					return Ok(updatedRole);
				}
				else
				{
					StringBuilder sb = new();

					foreach (IdentityError item in result.Errors)
					{
						_ = sb.AppendLine(CultureInfo.InvariantCulture, $"\t{item.Code}: {item.Description}");
					}

					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had one or more identity errors occur:\n{IdentityErrors}", HttpContext.Request.Method, nameof(IdentityController), nameof(UpdateRoleAsync), id, nameof(IdentityRole), role, sb.ToString());
					}

					return BadRequest(result.Errors);
				}
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} {Id} called with {ModelName} {Model} has a mismatching Id.", HttpContext.Request.Method, nameof(IdentityController), nameof(UpdateRoleAsync), id, nameof(IdentityRole), role);
				}

				return BadRequest($"The route parameter Id {id} does not match the Role Id {role?.Id} from the request body.");
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(IdentityController), nameof(UpdateRoleAsync), id, nameof(IdentityRole), role);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Deletes a roleData by its ID.
	/// </summary>
	/// <param name="id">The unique identifier of the roleData to delete.</param>
	/// <returns>Success status if the roleData was deleted.</returns>
	/// <response code="200">If the roleData was successfully deleted.</response>
	/// <response code="400">If the deletion fails.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// DELETE api/v1/Identity/Roles/f755eafe-b9d1-4028-b060-ee12002b8c0c
	[HttpDelete("Roles/{id}")]
	public async Task<ActionResult> DeleteRoleAsync(string id)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {Action} {Id} called.", HttpContext.Request.Method, nameof(IdentityController), nameof(DeleteRoleAsync), id);
			}

			IdentityRole role = _roleManager.Roles.First(r => r.Id == id);
			IdentityResult result = await _roleManager.DeleteAsync(role).ConfigureAwait(false);

			if (result.Succeeded)
			{
				if (_logger.IsEnabled(LogLevel.Debug))
				{
					_logger.LogDebug("{Method} {Controller} {Action} {Id} returning.", HttpContext.Request.Method, nameof(IdentityController), nameof(DeleteRoleAsync), id);
				}

				return Ok();
			}
			else
			{
				StringBuilder sb = new();

				foreach (IdentityError item in result.Errors)
				{
					_ = sb.AppendLine(CultureInfo.InvariantCulture, $"\t{item.Code}: {item.Description}");
				}

				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} had one or more identity errors occur:\n{IdentityErrors}", HttpContext.Request.Method, nameof(IdentityController), nameof(DeleteRoleAsync), sb.ToString());
				}

				return BadRequest(result.Errors);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} {Id} had an error.", HttpContext.Request.Method, nameof(IdentityController), nameof(DeleteRoleAsync), id);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all claims for a specific user.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <returns>A list of claims associated with the user.</returns>
	/// <response code="200">Returns the list of user claims.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET api/v1/Identity/Users/f755eafe-b9d1-4028-b060-ee12002b8c0c/Claims
	[HttpGet("Users/{userId}/Claims")]
	public async Task<ActionResult<List<Claim>>> GetAllUserClaimsAsync(string userId)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {UserId} {Action} called.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(GetAllUserClaimsAsync));
			}

			IdentityUser user = _userManager.Users.First(u => u.Id == userId);
			IList<Claim> result = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
			List<Claim> output = [.. result];

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {UserId} {Action} returning {ModelCount} {Model}.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(GetAllUserClaimsAsync), output.Count, nameof(Claim));
			}

			return Ok(output);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {UserId} {Action} had an error.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(GetAllUserClaimsAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Adds a new claim to a user.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <param name="userClaim">The claim data to add.</param>
	/// <returns>The updated list of user claims.</returns>
	/// <response code="200">Returns the updated list of user claims.</response>
	/// <response code="400">If the claim already exists or the operation fails.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// POST api/v1/Identity/Users/f755eafe-b9d1-4028-b060-ee12002b8c0c/Claims
	[HttpPost("Users/{userId}/Claims")]
	public async Task<ActionResult<Claim>> AddUserClaimAsync(string userId, [FromBody] ClaimData userClaim)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {UserId} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(AddUserClaimAsync), nameof(ClaimData), userClaim);
			}

			ArgumentNullException.ThrowIfNull(userClaim, nameof(userClaim));
			IdentityUser user = _userManager.Users.First(u => u.Id == userId);
			IList<Claim> existingClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);

			if (existingClaims.Any(c => c.Type == userClaim.ClaimType))
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {UserId} {Action} called with {ModelName} {Model} tried to add claim that already exists.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(AddUserClaimAsync), nameof(ClaimData), userClaim);
				}

				return BadRequest($"Claim {userClaim} for {userId} already exists.");
			}
			else
			{
				Claim claim = new(userClaim.ClaimType, userClaim.ClaimValue);
				IdentityResult result = await _userManager.AddClaimAsync(user, claim).ConfigureAwait(false);

				if (result.Succeeded)
				{
					IList<Claim> newClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
					Claim output = newClaims.First(c => c.Type == userClaim.ClaimType);

					if (_logger.IsEnabled(LogLevel.Debug))
					{
						_logger.LogDebug("{Method} {Controller} {UserId} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(AddUserClaimAsync), nameof(Claim), output);
					}

					return Ok(output);
				}
				else
				{
					StringBuilder sb = new();

					foreach (IdentityError item in result.Errors)
					{
						_ = sb.AppendLine(CultureInfo.InvariantCulture, $"\t{item.Code}: {item.Description}");
					}

					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {UserId} {Action} called with {ModelName} {Model} had one or more identity errors occur:\n{IdentityErrors}", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(AddUserClaimAsync), nameof(ClaimData), userClaim, sb.ToString());
					}

					return BadRequest(result.Errors);
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {UserId} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(AddUserClaimAsync), nameof(ClaimData), userClaim);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Replaces an existing user claim with a new value.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <param name="userClaim">The claim data with updated value.</param>
	/// <returns>The updated list of user claims.</returns>
	/// <response code="200">Returns the updated list of user claims.</response>
	/// <response code="400">If the claim does not exist or the operation fails.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// PUT api/v1/Identity/Users/f755eafe-b9d1-4028-b060-ee12002b8c0c/Claims
	[HttpPut("Users/{userId}/Claims")]
	public async Task<ActionResult<Claim>> ReplaceUserClaimAsync(string userId, [FromBody] ClaimData userClaim)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {UserId} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(ReplaceUserClaimAsync), nameof(ClaimData), userClaim);
			}

			ArgumentNullException.ThrowIfNull(userClaim, nameof(userClaim));
			IdentityUser user = _userManager.Users.First(u => u.Id == userId);
			IList<Claim> existingClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
			Claim? claimToReplace = existingClaims.FirstOrDefault(c => c.Type == userClaim.ClaimType);

			if (claimToReplace is null)
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {UserId} {Action} called with {ModelName} {Model} tried to replace claim that does not exist.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(ReplaceUserClaimAsync), nameof(ClaimData), userClaim);
				}

				return BadRequest($"Claim {userClaim} for {userId} does not exist.");
			}
			else
			{
				Claim claim = new(userClaim.ClaimType, userClaim.ClaimValue);
				IdentityResult result = await _userManager.ReplaceClaimAsync(user, claimToReplace, claim).ConfigureAwait(false);

				if (result.Succeeded)
				{
					IList<Claim> newClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
					Claim output = newClaims.Where(c => c.Type == userClaim.ClaimType).First();

					if (_logger.IsEnabled(LogLevel.Debug))
					{
						_logger.LogDebug("{Method} {Controller} {UserId} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(ReplaceUserClaimAsync), nameof(Claim), output);
					}

					return Ok(output);
				}
				else
				{
					StringBuilder sb = new();

					foreach (IdentityError item in result.Errors)
					{
						_ = sb.AppendLine(CultureInfo.InvariantCulture, $"\t{item.Code}: {item.Description}");
					}

					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {UserId} {Action} called with {ModelName} {Model} had one or more identity errors occur:\n{IdentityErrors}", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(ReplaceUserClaimAsync), nameof(ClaimData), userClaim, sb.ToString());
					}

					return BadRequest(result.Errors);
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {UserId} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(ReplaceUserClaimAsync), nameof(ClaimData), userClaim);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Removes a claim from a user.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <param name="userClaim">The claim data to remove.</param>
	/// <returns>The updated list of user claims.</returns>
	/// <response code="200">Returns the updated list of user claims.</response>
	/// <response code="400">If the claim does not exist or the operation fails.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// DELETE api/v1/Identity/Users/f755eafe-b9d1-4028-b060-ee12002b8c0c/Claims
	[HttpDelete("Users/{userId}/Claims")]
	public async Task<ActionResult> RemoveUserClaimAsync(string userId, [FromBody] ClaimData userClaim)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {UserId} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(RemoveUserClaimAsync), nameof(ClaimData), userClaim);
			}

			ArgumentNullException.ThrowIfNull(userClaim, nameof(userClaim));
			IdentityUser user = _userManager.Users.Where(u => u.Id == userId).First();
			IList<Claim> existingClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
			Claim? claimToRemove = existingClaims.Where(c => c.Type == userClaim.ClaimType).FirstOrDefault();

			if (claimToRemove is null)
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {UserId} {Action} called with {ModelName} {Model} tried to remove claim that does not exist.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(RemoveUserClaimAsync), nameof(ClaimData), userClaim);
				}

				return BadRequest($"Claim {userClaim} for {userId} does not exist.");
			}
			else
			{
				IdentityResult result = await _userManager.RemoveClaimAsync(user, claimToRemove).ConfigureAwait(false);

				if (result.Succeeded)
				{
					if (_logger.IsEnabled(LogLevel.Debug))
					{
						_logger.LogDebug("{Method} {Controller} {UserId} {Action} returning.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(RemoveUserClaimAsync));
					}

					return Ok();
				}
				else
				{
					StringBuilder sb = new();

					foreach (IdentityError item in result.Errors)
					{
						_ = sb.AppendLine(CultureInfo.InvariantCulture, $"\t{item.Code}: {item.Description}");
					}

					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {UserId} {Action} called with {ModelName} {Model} had one or more identity errors occur:\n{IdentityErrors}", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(RemoveUserClaimAsync), nameof(ClaimData), userClaim, sb.ToString());
					}

					return BadRequest(result.Errors);
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {UserId} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(RemoveUserClaimAsync), nameof(ClaimData), userClaim);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all claims for a specific roleData.
	/// </summary>
	/// <param name="roleId">The unique identifier of the roleData.</param>
	/// <returns>A list of claims associated with the roleData.</returns>
	/// <response code="200">Returns the list of roleData claims.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET api/v1/Identity/Roles/f755eafe-b9d1-4028-b060-ee12002b8c0c/Claims
	[HttpGet("Roles/{roleId}/Claims")]
	public async Task<ActionResult<List<Claim>>> GetAllRoleClaimsAsync(string roleId)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {RoleId} {Action} called.", HttpContext.Request.Method, nameof(IdentityController), roleId, nameof(GetAllRoleClaimsAsync));
			}

			IdentityRole role = _roleManager.Roles.Where(u => u.Id == roleId).First();
			IList<Claim> result = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
			List<Claim> output = [.. result];

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {RoleId} {Action} returning {ModelCount} {Model}.", HttpContext.Request.Method, nameof(IdentityController), roleId, nameof(GetAllRoleClaimsAsync), output.Count, nameof(Claim));
			}

			return Ok(output);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {RoleId} {Action} had an error.", HttpContext.Request.Method, nameof(IdentityController), roleId, nameof(GetAllRoleClaimsAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Adds a new claim to a roleData.
	/// </summary>
	/// <param name="roleId">The unique identifier of the roleData.</param>
	/// <param name="roleClaim">The claim data to add.</param>
	/// <returns>The updated list of roleData claims.</returns>
	/// <response code="200">Returns the updated list of roleData claims.</response>
	/// <response code="400">If the claim already exists or the operation fails.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// POST api/v1/Identity/Roles/f755eafe-b9d1-4028-b060-ee12002b8c0c/Claims
	[HttpPost("Roles/{roleId}/Claims")]
	public async Task<ActionResult<List<Claim>>> AddRoleClaimAsync(string roleId, [FromBody] ClaimData roleClaim)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {RoleId} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), roleId, nameof(AddRoleClaimAsync), nameof(ClaimData), roleClaim);
			}

			ArgumentNullException.ThrowIfNull(roleClaim, nameof(roleClaim));
			IdentityRole role = _roleManager.Roles.Where(u => u.Id == roleId).First();
			IList<Claim> existingClaims = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);

			if (existingClaims.Where(c => c.Type == roleClaim.ClaimType).Any())
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {RoleId} {Action} called with {ModelName} {Model} tried to add claim that already exists.", HttpContext.Request.Method, nameof(IdentityController), roleId, nameof(AddRoleClaimAsync), nameof(ClaimData), roleClaim);
				}

				return BadRequest($"Claim {roleClaim} for {roleId} already exists.");
			}
			else
			{
				Claim claim = new(roleClaim.ClaimType, roleClaim.ClaimValue);
				IdentityResult result = await _roleManager.AddClaimAsync(role, claim).ConfigureAwait(false);

				if (result.Succeeded)
				{
					IList<Claim> newClaims = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
					Claim output = newClaims.Where(c => c.Type == roleClaim.ClaimType).First();

					if (_logger.IsEnabled(LogLevel.Debug))
					{
						_logger.LogDebug("{Method} {Controller} {RoleId} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), roleId, nameof(AddRoleClaimAsync), nameof(Claim), output);
					}

					return Ok(output);
				}
				else
				{
					StringBuilder sb = new();

					foreach (IdentityError item in result.Errors)
					{
						_ = sb.AppendLine(CultureInfo.InvariantCulture, $"\t{item.Code}: {item.Description}");
					}

					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {RoleId} {Action} called with {ModelName} {Model} had one or more identity errors occur:\n{IdentityErrors}", HttpContext.Request.Method, nameof(IdentityController), roleId, nameof(AddRoleClaimAsync), nameof(ClaimData), roleClaim, sb.ToString());
					}

					return BadRequest(result.Errors);
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {RoleId} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(IdentityController), roleId, nameof(AddRoleClaimAsync), nameof(ClaimData), roleClaim);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Removes a claim from a roleData.
	/// </summary>
	/// <param name="roleId">The unique identifier of the roleData.</param>
	/// <param name="roleClaim">The claim data to remove.</param>
	/// <returns>The updated list of roleData claims.</returns>
	/// <response code="200">Returns the updated list of roleData claims.</response>
	/// <response code="400">If the claim does not exist or the operation fails.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// DELETE api/v1/Identity/Roles/f755eafe-b9d1-4028-b060-ee12002b8c0c/Claims
	[HttpDelete("Roles/{roleId}/Claims")]
	public async Task<ActionResult<List<Claim>>> RemoveRoleClaimAsync(string roleId, [FromBody] ClaimData roleClaim)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {RoleId} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), roleId, nameof(RemoveRoleClaimAsync), nameof(ClaimData), roleClaim);
			}

			ArgumentNullException.ThrowIfNull(roleClaim, nameof(roleClaim));
			IdentityRole role = _roleManager.Roles.Where(u => u.Id == roleId).First();
			IList<Claim> existingClaims = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
			Claim? claimToRemove = existingClaims.Where(c => c.Type == roleClaim.ClaimType).FirstOrDefault();

			if (claimToRemove is null)
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {RoleId} {Action} called with {ModelName} {Model} tried to remove claim that does not exist.", HttpContext.Request.Method, nameof(IdentityController), roleId, nameof(RemoveRoleClaimAsync), nameof(ClaimData), roleClaim);
				}

				return BadRequest($"Claim {roleClaim} for {roleId} does not exist.");
			}
			else
			{
				IdentityResult result = await _roleManager.RemoveClaimAsync(role, claimToRemove).ConfigureAwait(false);

				if (result.Succeeded)
				{
					if (_logger.IsEnabled(LogLevel.Debug))
					{
						_logger.LogDebug("{Method} {Controller} {RoleId} {Action} returning.", HttpContext.Request.Method, nameof(IdentityController), roleId, nameof(RemoveRoleClaimAsync));
					}

					return Ok();
				}
				else
				{
					StringBuilder sb = new();

					foreach (IdentityError item in result.Errors)
					{
						_ = sb.AppendLine(CultureInfo.InvariantCulture, $"\t{item.Code}: {item.Description}");
					}

					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {RoleId} {Action} called with {ModelName} {Model} had one or more identity errors occur:\n{IdentityErrors}", HttpContext.Request.Method, nameof(IdentityController), roleId, nameof(RemoveRoleClaimAsync), nameof(ClaimData), roleClaim, sb.ToString());
					}

					return BadRequest(result.Errors);
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {RoleId} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(IdentityController), roleId, nameof(RemoveRoleClaimAsync), nameof(ClaimData), roleClaim);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Retrieves all roles assigned to a specific user.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <returns>A list of roles assigned to the user.</returns>
	/// <response code="200">Returns the list of user roles.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// GET api/v1/Identity/Users/f755eafe-b9d1-4028-b060-ee12002b8c0c/Roles
	[HttpGet("Users/{userId}/Roles")]
	public async Task<ActionResult<List<IdentityRole>>> GetAllUserRolesAsync(string userId)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {UserId} {Action} called.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(GetAllUserRolesAsync));
			}

			IdentityUser user = _userManager.Users.Where(u => u.Id == userId).First();
			IList<string> result = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
			List<IdentityRole> output = [];

			foreach (string item in result)
			{
				IdentityRole role = _roleManager.Roles.Where(r => r.Name == item).First();
				output.Add(role);
			}

			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {UserId} {Action} returning {ModelCount} {Model}.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(GetAllUserRolesAsync), output.Count, nameof(IdentityRole));
			}

			return Ok(output);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {UserId} {Action} had an error.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(GetAllUserRolesAsync));
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Assigns a roleData to a user.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <param name="roleData">The name of the roleData to assign.</param>
	/// <returns>The updated list of user roles.</returns>
	/// <response code="200">Returns the updated list of user roles.</response>
	/// <response code="400">If the user is already in the roleData or the operation fails.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// POST api/v1/Identity/Users/f755eafe-b9d1-4028-b060-ee12002b8c0c/Roles
	[HttpPost("Users/{userId}/Roles")]
	public async Task<ActionResult<IdentityRole>> AddUserToRoleAsync(string userId, [FromBody] RoleData roleData)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {UserId} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(AddUserToRoleAsync), nameof(RoleData), roleData);
			}

			ArgumentNullException.ThrowIfNull(roleData, nameof(roleData));
			IdentityUser user = _userManager.Users.Where(u => u.Id == userId).First();
			IList<string> existingRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

			if (existingRoles.Contains(roleData.RoleName))
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {UserId} {Action} called with {ModelName} {Model} tried to add User to a Role in which the User already is.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(AddUserToRoleAsync), nameof(RoleData), roleData);
				}

				return BadRequest($"User {userId} is already in Role {roleData.RoleName}");
			}
			else
			{
				IdentityResult result = await _userManager.AddToRoleAsync(user, roleData.RoleName).ConfigureAwait(false);

				if (result.Succeeded)
				{
					IList<string> newRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
					IdentityRole output = _roleManager.Roles.Where(r => r.Name == roleData.RoleName).First();

					if (_logger.IsEnabled(LogLevel.Debug))
					{
						_logger.LogDebug("{Method} {Controller} {UserId} {Action} returning {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(AddUserToRoleAsync), nameof(IdentityRole), output);
					}

					return Ok(output);
				}
				else
				{
					StringBuilder sb = new();

					foreach (IdentityError item in result.Errors)
					{
						_ = sb.AppendLine(CultureInfo.InvariantCulture, $"\t{item.Code}: {item.Description}");
					}

					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {UserId} {Action} called with {ModelName} {Model} had one or more identity errors occur:\n{IdentityErrors}", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(AddUserToRoleAsync), nameof(RoleData), roleData, sb.ToString());
					}

					return BadRequest(result.Errors);
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {UserId} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(AddUserToRoleAsync), nameof(RoleData), roleData);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Removes a roleData from a user.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <param name="roleData"></param>
	/// <returns>The updated list of user roles.</returns>
	/// <response code="200">Returns the updated list of user roles.</response>
	/// <response code="400">If the user is not in the roleData or the operation fails.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// DELETE api/v1/Identity/Users/f755eafe-b9d1-4028-b060-ee12002b8c0c/Roles
	[HttpDelete("Users/{userId}/Roles")]
	public async Task<ActionResult> RemoveUserFromRoleAsync(string userId, [FromBody] RoleData roleData)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Method} {Controller} {UserId} {Action} called with {ModelName} {Model}.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(RemoveUserFromRoleAsync), nameof(RoleData), roleData);
			}

			ArgumentNullException.ThrowIfNull(roleData, nameof(roleData));
			IdentityUser user = _userManager.Users.Where(u => u.Id == userId).First();
			IList<string> existingRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

			if (existingRoles.Contains(roleData.RoleName))
			{
				IdentityResult result = await _userManager.RemoveFromRoleAsync(user, roleData.RoleName).ConfigureAwait(false);

				if (result.Succeeded)
				{
					if (_logger.IsEnabled(LogLevel.Debug))
					{
						_logger.LogDebug("{Method} {Controller} {UserId} {Action} returning.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(RemoveUserFromRoleAsync));
					}

					return Ok();
				}
				else
				{
					StringBuilder sb = new();

					foreach (IdentityError item in result.Errors)
					{
						_ = sb.AppendLine(CultureInfo.InvariantCulture, $"\t{item.Code}: {item.Description}");
					}

					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Method} {Controller} {UserId} {Action} called with {ModelName} {Model} had one or more identity errors occur:\n{IdentityErrors}", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(RemoveUserFromRoleAsync), nameof(RoleData), roleData, sb.ToString());
					}

					return BadRequest(result.Errors);
				}
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {UserId} {Action} called with {ModelName} {Model} tried to remove User from a Role in which the User is not.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(RemoveUserFromRoleAsync), nameof(RoleData), roleData);
				}

				return BadRequest($"User {userId} is not in Role {roleData.RoleName}");
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {UserId} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(IdentityController), userId, nameof(RemoveUserFromRoleAsync), nameof(RoleData), roleData);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}
}
