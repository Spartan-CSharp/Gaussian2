using System.Data;
using System.Security.Claims;

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
				_logger.LogDebug("{Controller} {Action}", nameof(IdentityController), nameof(GetAllUsersAsync));
			}

			List<IdentityUser> output = [.. _userManager.Users];
			return Ok(output);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} had an error", nameof(IdentityController), nameof(GetAllUsersAsync));
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
				_logger.LogDebug("{Controller} {Action} {Id}", nameof(IdentityController), nameof(GetUserAsync), id);
			}

			IdentityUser? user = _userManager.Users.Where(u => u.Id == id).FirstOrDefault();

			if (user is null)
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Controller} {Action} {Id} failed to return a valid user", nameof(IdentityController), nameof(GetUserAsync), id);
				}

				return BadRequest($"No User exists with the supplied Id {id}.");
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Trace))
				{
					_logger.LogTrace("{Controller} {Action} {Id} returned user {User}", nameof(IdentityController), nameof(GetUserAsync), id, user);
				}

				return Ok(user);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {Id} had an error", nameof(IdentityController), nameof(GetUserAsync), id);
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
				_logger.LogDebug("{Controller} {Action} {UserData}", nameof(IdentityController), nameof(CreateUserAsync), userData);
			}

			if (userData is null)
			{
				throw new ArgumentNullException(nameof(userData), $"The parameter {nameof(userData)} cannot be null.");
			}

			IdentityResult result = await _userManager.CreateAsync(new IdentityUser
			{
				UserName = userData.UserName,
				Email = userData.Email
			}, userData.Password).ConfigureAwait(false);

			if (result.Succeeded)
			{
				IdentityUser createdUser = _userManager.Users.Where(u => u.UserName == userData.UserName).First();

				if (_logger.IsEnabled(LogLevel.Trace))
				{
					_logger.LogTrace("{Controller} {Action} {UserData} returned created user {CreatedUser}", nameof(IdentityController), nameof(CreateUserAsync), userData, createdUser);
				}

				return CreatedAtAction(nameof(CreateUserAsync), RouteData.Values, createdUser);
			}
			else
			{
				foreach (IdentityError item in result.Errors)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Controller} {Action} {UserData} failed with error {Code} {Description}", nameof(IdentityController), nameof(CreateUserAsync), userData, item.Code, item.Description);
					}
				}

				return BadRequest(result.Errors);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {UserData} had an error", nameof(IdentityController), nameof(CreateUserAsync), userData);
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
				_logger.LogDebug("{Controller} {Action} {Id} {User}", nameof(IdentityController), nameof(UpdateUserAsync), id, user);
			}

			if (user?.Id == id)
			{
				IdentityResult result = await _userManager.UpdateAsync(user).ConfigureAwait(false);

				if (result.Succeeded)
				{
					IdentityUser updatedUser = _userManager.Users.Where(u => u.Id == id).First();

					if (_logger.IsEnabled(LogLevel.Trace))
					{
						_logger.LogTrace("{Controller} {Action} {Id} {User} returned updated user {UpdatedUser}", nameof(IdentityController), nameof(UpdateUserAsync), id, user, updatedUser);
					}

					return Ok(updatedUser);
				}
				else
				{
					foreach (IdentityError item in result.Errors)
					{
						if (_logger.IsEnabled(LogLevel.Warning))
						{
							_logger.LogWarning("{Controller} {Action} {User} failed with error {Code} {Description}", nameof(IdentityController), nameof(UpdateUserAsync), user, item.Code, item.Description);
						}
					}

					return BadRequest(result.Errors);
				}
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Controller} {Action} {Id} {User} has ID Mismatch", nameof(IdentityController), nameof(UpdateUserAsync), id, user);
				}

				return BadRequest($"The route parameter ID {id} does not match the user id {user?.Id} from the request body.");
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {Id} {User} had an error", nameof(IdentityController), nameof(UpdateUserAsync), id, user);
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
				_logger.LogDebug("{Controller} {Action} {Id}", nameof(IdentityController), nameof(DeleteUserAsync), id);
			}

			IdentityUser user = _userManager.Users.Where(u => u.Id == id).First();
			IdentityResult result = await _userManager.DeleteAsync(user).ConfigureAwait(false);

			if (result.Succeeded)
			{
				if (_logger.IsEnabled(LogLevel.Trace))
				{
					_logger.LogTrace("{Controller} {Action} {Id} returned success", nameof(IdentityController), nameof(DeleteUserAsync), id);
				}

				return Ok();
			}
			else
			{
				foreach (IdentityError item in result.Errors)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Controller} {Action} {Id} failed with error {Code} {Description}", nameof(IdentityController), nameof(DeleteUserAsync), id, item.Code, item.Description);
					}
				}

				return BadRequest(result.Errors);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {Id} had an error", nameof(IdentityController), nameof(DeleteUserAsync), id);
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
				_logger.LogDebug("{Controller} {Action}", nameof(IdentityController), nameof(GetAllRolesAsync));
			}

			List<IdentityRole> output = [.. _roleManager.Roles];
			return Ok(output);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} had an error", nameof(IdentityController), nameof(GetAllRolesAsync));
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
				_logger.LogDebug("{Controller} {Action} {Id}", nameof(IdentityController), nameof(GetRoleAsync), id);
			}

			IdentityRole? role = _roleManager.Roles.Where(r => r.Id == id).FirstOrDefault();

			if (role is null)
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Controller} {Action} {Id} failed to return a valid roleData", nameof(IdentityController), nameof(GetRoleAsync), id);
				}

				return BadRequest($"No Role exists with the supplied Id {id}.");
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Trace))
				{
					_logger.LogTrace("{Controller} {Action} {Id} returned roleData {Role}", nameof(IdentityController), nameof(GetRoleAsync), id, role);
				}

				return Ok(role);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {Id} had an error", nameof(IdentityController), nameof(GetRoleAsync), id);
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
				_logger.LogDebug("{Controller} {Action} {RoleData}", nameof(IdentityController), nameof(CreateRoleAsync), roleData);
			}

			if (roleData is null)
			{
				throw new ArgumentNullException(nameof(roleData), $"The parameter {nameof(roleData)} cannot be null.");
			}

			IdentityResult result = await _roleManager.CreateAsync(new IdentityRole { Name = roleData.RoleName }).ConfigureAwait(false);

			if (result.Succeeded)
			{
				IdentityRole createdRole = _roleManager.Roles.Where(r => r.Name == roleData.RoleName).First();

				if (_logger.IsEnabled(LogLevel.Trace))
				{
					_logger.LogTrace("{Controller} {Action} {RoleData} returned created roleData {CreatedRole}", nameof(IdentityController), nameof(CreateRoleAsync), roleData, createdRole);
				}

				return CreatedAtAction(nameof(CreateRoleAsync), RouteData.Values, createdRole);
			}
			else
			{
				foreach (IdentityError item in result.Errors)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Controller} {Action} {RoleData} failed with error {Code} {Description}", nameof(IdentityController), nameof(CreateRoleAsync), roleData, item.Code, item.Description);
					}
				}

				return BadRequest(result.Errors);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {RoleData} had an error", nameof(IdentityController), nameof(CreateRoleAsync), roleData);
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
				_logger.LogDebug("{Controller} {Action} {Id} {Role}", nameof(IdentityController), nameof(UpdateRoleAsync), id, role);
			}

			if (role?.Id == id)
			{
				IdentityResult result = await _roleManager.UpdateAsync(role).ConfigureAwait(false);

				if (result.Succeeded)
				{
					IdentityRole updatedRole = _roleManager.Roles.Where(r => r.Id == id).First();

					if (_logger.IsEnabled(LogLevel.Trace))
					{
						_logger.LogTrace("{Controller} {Action} {Id} {Role} returned updated roleData {UpdatedRole}", nameof(IdentityController), nameof(UpdateRoleAsync), id, role, updatedRole);
					}

					return Ok(updatedRole);
				}
				else
				{
					foreach (IdentityError item in result.Errors)
					{
						if (_logger.IsEnabled(LogLevel.Warning))
						{
							_logger.LogWarning("{Controller} {Action} {Role} failed with error {Code} {Description}", nameof(IdentityController), nameof(UpdateRoleAsync), role, item.Code, item.Description);
						}
					}

					return BadRequest(result.Errors);
				}
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Controller} {Action} {Id} {Role} has ID Mismatch", nameof(IdentityController), nameof(UpdateRoleAsync), id, role);
				}

				return BadRequest($"The route parameter ID {id} does not match the roleData Id {role?.Id} from the request body.");
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {Id} {Role} had an error", nameof(IdentityController), nameof(UpdateRoleAsync), id, role);
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
				_logger.LogDebug("{Controller} {Action} {Id}", nameof(IdentityController), nameof(DeleteRoleAsync), id);
			}

			IdentityRole role = _roleManager.Roles.Where(r => r.Id == id).First();
			IdentityResult result = await _roleManager.DeleteAsync(role).ConfigureAwait(false);
			if (result.Succeeded)
			{
				if (_logger.IsEnabled(LogLevel.Trace))
				{
					_logger.LogTrace("{Controller} {Action} {Id} returned success", nameof(IdentityController), nameof(DeleteRoleAsync), id);
				}

				return Ok();
			}
			else
			{
				foreach (IdentityError item in result.Errors)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Controller} {Action} {Id} failed with error {Code} {Description}", nameof(IdentityController), nameof(DeleteRoleAsync), id, item.Code, item.Description);
					}
				}

				return BadRequest(result.Errors);
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {Id} had an error", nameof(IdentityController), nameof(DeleteRoleAsync), id);
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
				_logger.LogDebug("{Controller} {Action} {UserId}", nameof(IdentityController), nameof(GetAllUserClaimsAsync), userId);
			}

			IdentityUser user = _userManager.Users.Where(u => u.Id == userId).First();
			IList<Claim> result = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Controller} {Action} {UserId} returned {ClaimCount},", nameof(IdentityController), nameof(GetAllUserClaimsAsync), userId, result.Count);
			}

			List<Claim> output = [.. result];
			return Ok(output);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {UserId} had an error", nameof(IdentityController), nameof(GetAllUserClaimsAsync), userId);
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
	public async Task<ActionResult<List<Claim>>> AddUserClaimAsync(string userId, [FromBody] ClaimData userClaim)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action} {UserId} {UserClaim}", nameof(IdentityController), nameof(AddUserClaimAsync), userId, userClaim);
			}

			if (userClaim is null)
			{
				throw new ArgumentNullException(nameof(userClaim), $"The parameter {nameof(userClaim)} cannot be null.");
			}

			IdentityUser user = _userManager.Users.Where(u => u.Id == userId).First();
			IList<Claim> existingClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);

			if (existingClaims.Where(c => c.Type == userClaim.ClaimType).Any())
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Controller} {Action} {UserId} {UserClaim}: Claim Already Exists", nameof(IdentityController), nameof(AddUserClaimAsync), userId, userClaim);
				}

				return BadRequest($"Claim {userClaim} for {userId} already exists");
			}
			else
			{
				Claim claim = new(userClaim.ClaimType, userClaim.ClaimValue);
				IdentityResult result = await _userManager.AddClaimAsync(user, claim).ConfigureAwait(false);

				if (result.Succeeded)
				{
					IList<Claim> newClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);

					if (_logger.IsEnabled(LogLevel.Trace))
					{
						_logger.LogTrace("{Controller} {Action} {UserId} {UserClaim} returned {ClaimCount},", nameof(IdentityController), nameof(AddUserClaimAsync), userId, userClaim, newClaims.Count);
					}

					List<Claim> output = [.. newClaims];
					return Ok(output);
				}
				else
				{
					foreach (IdentityError item in result.Errors)
					{
						if (_logger.IsEnabled(LogLevel.Warning))
						{
							_logger.LogWarning("{Controller} {Action} {UserId} {UserClaim} failed with error {Code} {Description}", nameof(IdentityController), nameof(AddUserClaimAsync), userId, userClaim, item.Code, item.Description);
						}
					}

					return BadRequest(result.Errors);
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {UserId} {UserClaim} had an error", nameof(IdentityController), nameof(AddUserClaimAsync), userId, userClaim);
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
	public async Task<ActionResult<List<Claim>>> ReplaceUserClaimAsync(string userId, [FromBody] ClaimData userClaim)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action} {UserId} {UserClaim}", nameof(IdentityController), nameof(ReplaceUserClaimAsync), userId, userClaim);
			}

			if (userClaim is null)
			{
				throw new ArgumentNullException(nameof(userClaim), $"The parameter {nameof(userClaim)} cannot be null.");
			}

			IdentityUser user = _userManager.Users.Where(u => u.Id == userId).First();
			IList<Claim> existingClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
			Claim? claimToReplace = existingClaims.Where(c => c.Type == userClaim.ClaimType).FirstOrDefault();

			if (claimToReplace is null)
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Controller} {Action} {UserId} {UserClaim}: Claim Does Not Exist", nameof(IdentityController), nameof(ReplaceUserClaimAsync), userId, userClaim);
				}

				return BadRequest($"Claim {userClaim} for {userId} does not exist");
			}
			else
			{
				Claim claim = new(userClaim.ClaimType, userClaim.ClaimValue);
				IdentityResult result = await _userManager.ReplaceClaimAsync(user, claimToReplace, claim).ConfigureAwait(false);

				if (result.Succeeded)
				{
					IList<Claim> newClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);

					if (_logger.IsEnabled(LogLevel.Trace))
					{
						_logger.LogTrace("{Controller} {Action} {UserId} {UserClaim} returned {ClaimCount},", nameof(IdentityController), nameof(ReplaceUserClaimAsync), userId, userClaim, newClaims.Count);
					}

					List<Claim> output = [.. newClaims];
					return Ok(output);
				}
				else
				{
					foreach (IdentityError item in result.Errors)
					{
						if (_logger.IsEnabled(LogLevel.Warning))
						{
							_logger.LogWarning("{Controller} {Action} {UserId} {UserClaim} failed with error {Code} {Description}", nameof(IdentityController), nameof(ReplaceUserClaimAsync), userId, userClaim, item.Code, item.Description);
						}
					}

					return BadRequest(result.Errors);
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {UserId} {UserClaim} had an error", nameof(IdentityController), nameof(ReplaceUserClaimAsync), userId, userClaim);
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
	public async Task<ActionResult<List<Claim>>> RemoveUserClaimAsync(string userId, [FromBody] ClaimData userClaim)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action} {UserId} {UserClaim}", nameof(IdentityController), nameof(RemoveUserClaimAsync), userId, userClaim);
			}

			IdentityUser user = _userManager.Users.Where(u => u.Id == userId).First();
			IList<Claim> existingClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
			Claim? claimToRemove = existingClaims.Where(c => c.Type == userClaim.ClaimType).FirstOrDefault();

			if (claimToRemove is null)
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Controller} {Action} {UserId} {UserClaim}: Claim Does Not Exist", nameof(IdentityController), nameof(RemoveUserClaimAsync), userId, userClaim);
				}

				return BadRequest($"Claim {userClaim} for {userId} does not exist");
			}
			else
			{
				IdentityResult result = await _userManager.RemoveClaimAsync(user, claimToRemove).ConfigureAwait(false);

				if (result.Succeeded)
				{
					IList<Claim> newClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);

					if (_logger.IsEnabled(LogLevel.Trace))
					{
						_logger.LogTrace("{Controller} {Action} {UserId} {UserClaim} returned {ClaimCount},", nameof(IdentityController), nameof(RemoveUserClaimAsync), userId, userClaim, newClaims.Count);
					}

					List<Claim> output = [.. newClaims];
					return Ok(output);
				}
				else
				{
					foreach (IdentityError item in result.Errors)
					{
						if (_logger.IsEnabled(LogLevel.Warning))
						{
							_logger.LogWarning("{Controller} {Action} {UserId} {UserClaim} failed with error {Code} {Description}", nameof(IdentityController), nameof(RemoveUserClaimAsync), userId, userClaim, item.Code, item.Description);
						}
					}

					return BadRequest(result.Errors);
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {UserId} {UserClaim} had an error", nameof(IdentityController), nameof(RemoveUserClaimAsync), userId, userClaim);
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
				_logger.LogDebug("{Controller} {Action} {RoleId}", nameof(IdentityController), nameof(GetAllRoleClaimsAsync), roleId);
			}

			IdentityRole role = _roleManager.Roles.Where(r => r.Id == roleId).First();
			IList<Claim> result = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Controller} {Action} {RoleId} returned {ClaimCount},", nameof(IdentityController), nameof(GetAllRoleClaimsAsync), roleId, result.Count);
			}

			List<Claim> output = [.. result];
			return Ok(output);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {RoleId} had an error", nameof(IdentityController), nameof(GetAllRoleClaimsAsync), roleId);
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
				_logger.LogDebug("{Controller} {Action} {RoleId} {RoleClaim}", nameof(IdentityController), nameof(AddRoleClaimAsync), roleId, roleClaim);
			}

			if (roleClaim is null)
			{
				throw new ArgumentNullException(nameof(roleClaim), $"The parameter {nameof(roleClaim)} cannot be null.");
			}

			IdentityRole role = _roleManager.Roles.Where(r => r.Id == roleId).First();
			IList<Claim> existingClaims = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);

			if (existingClaims.Where(c => c.Type == roleClaim.ClaimType).Any())
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Controller} {Action} {RoleId} {RoleClaim}: Claim Already Exists", nameof(IdentityController), nameof(AddRoleClaimAsync), roleId, roleClaim);
				}

				return BadRequest($"Claim {roleClaim} for {roleId} already exists");
			}
			else
			{
				Claim claim = new(roleClaim.ClaimType, roleClaim.ClaimValue);
				IdentityResult result = await _roleManager.AddClaimAsync(role, claim).ConfigureAwait(false);

				if (result.Succeeded)
				{
					IList<Claim> newClaims = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);

					if (_logger.IsEnabled(LogLevel.Trace))
					{
						_logger.LogTrace("{Controller} {Action} {RoleId} {RoleClaim} returned {ClaimCount},", nameof(IdentityController), nameof(AddRoleClaimAsync), roleId, roleClaim, newClaims.Count);
					}

					List<Claim> output = [.. newClaims];
					return Ok(output);
				}
				else
				{
					foreach (IdentityError item in result.Errors)
					{
						if (_logger.IsEnabled(LogLevel.Warning))
						{
							_logger.LogWarning("{Controller} {Action} {RoleId} {RoleClaim} failed with error {Code} {Description}", nameof(IdentityController), nameof(AddRoleClaimAsync), roleId, roleClaim, item.Code, item.Description);
						}
					}

					return BadRequest(result.Errors);
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {RoleId} {RoleClaim} had an error", nameof(IdentityController), nameof(AddRoleClaimAsync), roleId, roleClaim);
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
				_logger.LogDebug("{Controller} {Action} {RoleId} {RoleClaim}", nameof(IdentityController), nameof(RemoveRoleClaimAsync), roleId, roleClaim);
			}

			IdentityRole role = _roleManager.Roles.Where(u => u.Id == roleId).First();
			IList<Claim> existingClaims = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
			Claim? claimToRemove = existingClaims.Where(c => c.Type == roleClaim.ClaimType).FirstOrDefault();

			if (claimToRemove is null)
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Controller} {Action} {RoleId} {RoleClaim}: Claim Does Not Exist", nameof(IdentityController), nameof(RemoveRoleClaimAsync), roleId, roleClaim);
				}

				return BadRequest($"Claim {roleClaim} for {roleId} does not exist");
			}
			else
			{
				IdentityResult result = await _roleManager.RemoveClaimAsync(role, claimToRemove).ConfigureAwait(false);

				if (result.Succeeded)
				{
					IList<Claim> newClaims = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);

					if (_logger.IsEnabled(LogLevel.Trace))
					{
						_logger.LogTrace("{Controller} {Action} {RoleId} {RoleClaim} returned {ClaimCount},", nameof(IdentityController), nameof(RemoveRoleClaimAsync), roleId, roleClaim, newClaims.Count);
					}

					List<Claim> output = [.. newClaims];
					return Ok(output);
				}
				else
				{
					foreach (IdentityError item in result.Errors)
					{
						if (_logger.IsEnabled(LogLevel.Warning))
						{
							_logger.LogWarning("{Controller} {Action} {RoleId} {RoleClaim} failed with error {Code} {Description}", nameof(IdentityController), nameof(RemoveRoleClaimAsync), roleId, roleClaim, item.Code, item.Description);
						}
					}

					return BadRequest(result.Errors);
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {RoleId} {RoleClaim} had an error", nameof(IdentityController), nameof(RemoveRoleClaimAsync), roleId, roleClaim);
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
				_logger.LogDebug("{Controller} {Action} {UserId}", nameof(IdentityController), nameof(GetAllUserRolesAsync), userId);
			}

			IdentityUser user = _userManager.Users.Where(u => u.Id == userId).First();
			IList<string> result = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Controller} {Action} {UserId} returned {RoleCount},", nameof(IdentityController), nameof(GetAllUserRolesAsync), userId, result.Count);
			}

			List<IdentityRole> output = [];

			foreach (string item in result)
			{
				IdentityRole role = _roleManager.Roles.Where(r => r.Name == item).First();
				output.Add(role);
			}

			return Ok(output);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {UserId} had an error", nameof(IdentityController), nameof(GetAllUserRolesAsync), userId);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Assigns a roleData to a user.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <param name="roleName">The name of the roleData to assign.</param>
	/// <returns>The updated list of user roles.</returns>
	/// <response code="200">Returns the updated list of user roles.</response>
	/// <response code="400">If the user is already in the roleData or the operation fails.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// POST api/v1/Identity/Users/f755eafe-b9d1-4028-b060-ee12002b8c0c/Roles
	[HttpPost("Users/{userId}/Roles")]
	public async Task<ActionResult<List<IdentityRole>>> AddUserToRoleAsync(string userId, [FromBody] string roleName)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action} {UserId} {RoleName}", nameof(IdentityController), nameof(AddUserToRoleAsync), userId, roleName);
			}

			IdentityUser user = _userManager.Users.Where(u => u.Id == userId).First();
			IList<string> existingRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

			if (existingRoles.Contains(roleName))
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Controller} {Action} {UserId} {RoleName}: User is already in Role", nameof(IdentityController), nameof(AddUserToRoleAsync), userId, roleName);
				}

				return BadRequest($"User {userId} is already ir Role {roleName}");
			}
			else
			{
				IdentityResult result = await _userManager.AddToRoleAsync(user, roleName).ConfigureAwait(false);

				if (result.Succeeded)
				{
					IList<string> newRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

					if (_logger.IsEnabled(LogLevel.Trace))
					{
						_logger.LogTrace("{Controller} {Action} {UserId} {RoleName} returned {ClaimCount},", nameof(IdentityController), nameof(AddUserToRoleAsync), userId, roleName, newRoles.Count);
					}

					List<IdentityRole> output = [];

					foreach (string item in newRoles)
					{
						IdentityRole role = _roleManager.Roles.Where(r => r.Name == item).First();
						output.Add(role);
					}

					return Ok(output);
				}
				else
				{
					foreach (IdentityError item in result.Errors)
					{
						if (_logger.IsEnabled(LogLevel.Warning))
						{
							_logger.LogWarning("{Controller} {Action} {UserId} {RoleName} failed with error {Code} {Description}", nameof(IdentityController), nameof(AddUserToRoleAsync), userId, roleName, item.Code, item.Description);
						}
					}

					return BadRequest(result.Errors);
				}
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {UserId} {RoleName} had an error", nameof(IdentityController), nameof(AddUserToRoleAsync), userId, roleName);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	/// <summary>
	/// Removes a roleData from a user.
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <param name="roleName">The name of the roleData to remove.</param>
	/// <returns>The updated list of user roles.</returns>
	/// <response code="200">Returns the updated list of user roles.</response>
	/// <response code="400">If the user is not in the roleData or the operation fails.</response>
	/// <response code="500">If an internal server error occurs.</response>
	// DELETE api/v1/Identity/Users/f755eafe-b9d1-4028-b060-ee12002b8c0c/Roles
	[HttpDelete("Users/{userId}/Roles")]
	public async Task<ActionResult<List<IdentityRole>>> RemoveUserFromRoleAsync(string userId, [FromBody] string roleName)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action} {UserId} {RoleName}", nameof(IdentityController), nameof(RemoveUserFromRoleAsync), userId, roleName);
			}

			IdentityUser user = _userManager.Users.Where(u => u.Id == userId).First();
			IList<string> existingRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

			if (existingRoles.Contains(roleName))
			{
				IdentityResult result = await _userManager.RemoveFromRoleAsync(user, roleName).ConfigureAwait(false);

				if (result.Succeeded)
				{
					IList<string> newRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

					if (_logger.IsEnabled(LogLevel.Trace))
					{
						_logger.LogTrace("{Controller} {Action} {UserId} {RoleName} returned {ClaimCount},", nameof(IdentityController), nameof(RemoveUserFromRoleAsync), userId, roleName, newRoles.Count);
					}

					List<IdentityRole> output = [];

					foreach (string item in newRoles)
					{
						IdentityRole role = _roleManager.Roles.Where(r => r.Name == item).First();
						output.Add(role);
					}

					return Ok(output);
				}
				else
				{
					foreach (IdentityError item in result.Errors)
					{
						if (_logger.IsEnabled(LogLevel.Warning))
						{
							_logger.LogWarning("{Controller} {Action} {UserId} {RoleName} failed with error {Code} {Description}", nameof(IdentityController), nameof(RemoveUserFromRoleAsync), userId, roleName, item.Code, item.Description);
						}
					}

					return BadRequest(result.Errors);
				}
			}
			else
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Controller} {Action} {UserId} {RoleName}: User is already in Role", nameof(IdentityController), nameof(RemoveUserFromRoleAsync), userId, roleName);
				}

				return BadRequest($"User {userId} is already ir Role {roleName}");
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {UserId} {RoleName} had an error", nameof(IdentityController), nameof(RemoveUserFromRoleAsync), userId, roleName);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}
}
