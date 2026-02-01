using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

using Asp.Versioning;

using GaussianCommonLibrary.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GaussianMVC.Controllers.APIControllers;

/// <summary>
/// API controller responsible for handling authentication and JWT token generation.
/// Provides endpoints for user authentication and token-based authorization.
/// </summary>
/// <param name="userManager">The ASP.NET Core Identity user manager for user operations.</param>
/// <param name="roleManager">The ASP.NET Core Identity role manager for role operations.</param>
/// <param name="logger">The logger instance for recording authentication events and diagnostics.</param>
/// <param name="config">The application configuration providing authentication settings.</param>
[Route("api/[controller]")]
[ApiController]
[ApiVersionNeutral]
public class AuthenticationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AuthenticationController> logger, IConfiguration config) : ControllerBase
{
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly RoleManager<IdentityRole> _roleManager = roleManager;
	private readonly ILogger<AuthenticationController> _logger = logger;
	private readonly IConfiguration _config = config;
	private DateTimeOffset _loginTime;

	/// <summary>
	/// Authenticates a user and generates a JWT access token for API authorization.
	/// This endpoint validates user credentials and returns a bearer token that can be used
	/// for subsequent authenticated API requests.
	/// </summary>
	/// <param name="request">The token request containing user email and password credentials.</param>
	/// <returns>
	/// An <see cref="ActionResult{TokenResponse}"/> containing:
	/// - <see cref="StatusCodes.Status200OK"/> with a <see cref="TokenResponse"/> containing the JWT access token and username on successful authentication.
	/// - <see cref="StatusCodes.Status400BadRequest"/> if the request body is null.
	/// - <see cref="StatusCodes.Status401Unauthorized"/> if the credentials are invalid.
	/// - <see cref="StatusCodes.Status500InternalServerError"/> if an unexpected error occurs during token generation.
	/// </returns>
	/// <response code="200">OK with a response body containing the JWT access token and username on successful authentication.</response>
	/// <response code="400">Bad Request if the request body is null.</response>
	/// <response code="401">Unauthorized if the credentials are invalid.</response>
	/// <response code="500">Internal Server Error if an unexpected error occurs during token generation.</response>
	// POST api/Authentication/token
	[HttpPost("token")]
	[AllowAnonymous]
	public async Task<ActionResult<TokenResponse>> PostAsync([FromBody] TokenRequest request)
	{
		try
		{
			// TODO add password obfuscation for logging.
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action} called with {ModelName} {Model}.", nameof(AuthenticationController), nameof(PostAsync), nameof(TokenRequest), request);
			}

			ArgumentNullException.ThrowIfNull(request, nameof(request));

			if (ModelState.IsValid)
			{
				IdentityUser? user = ValidateCredentials(request);

				if (user is null)
				{
					if (_logger.IsEnabled(LogLevel.Warning))
					{
						_logger.LogWarning("{Controller} {Action} called with {ModelName} {Model} returning unauthorized.", nameof(AuthenticationController), nameof(PostAsync), nameof(TokenRequest), request);
					}

					return Unauthorized();
				}
				else
				{
					TokenResponse token = GenerateToken(user);

					if (_logger.IsEnabled(LogLevel.Debug))
					{
						_logger.LogDebug("{Controller} {Action} returning {ModelName} {Model}.", nameof(AuthenticationController), nameof(PostAsync), nameof(TokenResponse), token);
					}

					return Ok(token);
				}
			}
			else
			{
				Dictionary<string, List<string>> modelValidationErrors = ModelState.Where(kvp => kvp.Value?.Errors.Count > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList());
				StringBuilder sb = new();

				foreach (KeyValuePair<string, List<string>> validationErrors in modelValidationErrors)
				{
					_ = sb.AppendLine(validationErrors.Key);

					foreach (string validationError in validationErrors.Value)
					{
						_ = sb.AppendLine(CultureInfo.InvariantCulture, $"\t{validationError}");
					}
				}

				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Method} {Controller} {Action} called with {ModelName} {Model} had one or more validation errors occur:\n{ValidationErrors}", HttpContext.Request.Method, nameof(AuthenticationController), nameof(PostAsync), nameof(TokenRequest), request, sb.ToString());
				}

				return BadRequest(new ValidationProblemDetails(ModelState));
			}
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Method} {Controller} {Action} called with {ModelName} {Model} had an error.", HttpContext.Request.Method, nameof(AuthenticationController), nameof(PostAsync), nameof(TokenRequest), request);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	private TokenResponse GenerateToken(IdentityUser user)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} called with User = {User}.", nameof(AuthenticationController), nameof(GenerateToken), user);
		}

		Dictionary<string, object> userData = new()
		{
			{ "Id", user.Id },
			{ "UserName", user.UserName ?? string.Empty },
			{ "Email", user.Email ?? string.Empty },
			{ "EmailConfirmed", user.EmailConfirmed },
			{ "PhoneNumber", user.PhoneNumber ?? string.Empty },
			{ "PhoneNumberConfirmed", user.PhoneNumberConfirmed },
			{ "TwoFactorEnabled", user.TwoFactorEnabled },
			{ "LockoutEnd", user.LockoutEnd is null ? new { } : user.LockoutEnd },
			{ "LockoutEnabled", user.LockoutEnabled },
			{ "AccessFailedCount", user.AccessFailedCount }
		};

		string secretKey = _config.GetValue<string>("Authentication:SecretKey") ?? throw new InvalidOperationException("Authentication Secret Key Not Found!");
		string issuer = _config.GetValue<string>("Authentication:Issuer") ?? throw new InvalidOperationException("Authentication Issuer Not Found!");
		string audience = _config.GetValue<string>("Authentication:Audience") ?? throw new InvalidOperationException("Authentication Audience Not Found!");
		byte[] keyBytes = Encoding.ASCII.GetBytes(secretKey);
		SymmetricSecurityKey symmetricKey = new(keyBytes);
		SigningCredentials signingCredentials = new(symmetricKey, SecurityAlgorithms.HmacSha256);

		List<Claim> claims = [
			new(JwtRegisteredClaimNames.Iss, issuer),
			new(JwtRegisteredClaimNames.Sub, user.Id),
			new(JwtRegisteredClaimNames.Aud, audience),
			new(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture)),
			new(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture)),
			new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture)),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new(JwtRegisteredClaimNames.AuthTime, _loginTime.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture)),
			new(JwtRegisteredClaimNames.Amr, "pwd"),
			new(JwtRegisteredClaimNames.PreferredUsername, user.UserName ?? string.Empty),
			new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
			new(JwtRegisteredClaimNames.EmailVerified, user.EmailConfirmed.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)),
			new(JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber ?? string.Empty),
			new(JwtRegisteredClaimNames.PhoneNumberVerified, user.PhoneNumberConfirmed.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture))
		];

		IList<string> userRoleNames = _userManager.GetRolesAsync(user).Result;
		List<IdentityRole> userRoles = [];
		List<Dictionary<string, object>> userRoleDictionary = [];

		foreach (string item in userRoleNames)
		{
			IdentityRole role = _roleManager.Roles.First(r => r.Name == item);
			userRoles.Add(role);
		}

		foreach (IdentityRole role in userRoles)
		{
			Dictionary<string, object> roleData = new()
			{
				{ "Id", role.Id ?? string.Empty },
				{ "Name", role.Name ?? string.Empty }
			};

			if (!string.IsNullOrEmpty(role.Name))
			{
				claims.Add(new Claim(ClaimTypes.Role, role.Name));
			}

			IList<Claim> roleClaims = _roleManager.GetClaimsAsync(role).Result;
			Dictionary<string, object> roleClaimsDictionary = [];

			foreach (Claim? roleClaim in roleClaims)
			{
				if (roleClaim is not null)
				{
					claims.Add(roleClaim);

					// Try to parse the claim value as JSON
					object claimValue;

					try
					{
						using JsonDocument doc = JsonDocument.Parse(roleClaim.Value);
						// If parsing succeeds, deserialize to object
						claimValue = JsonSerializer.Deserialize<object>(roleClaim.Value) ?? roleClaim.Value;
					}
					catch (JsonException)
					{
						// If parsing fails, it's not JSON - keep as string
						claimValue = roleClaim.Value;
					}

					roleClaimsDictionary.Add(roleClaim.Type, claimValue);
				}
			}

			roleData.Add("RoleClaims", roleClaimsDictionary);
			userRoleDictionary.Add(roleData);
		}

		userData.Add("UserRoles", userRoleDictionary);
		IList<Claim> userClaims = _userManager.GetClaimsAsync(user).Result;
		Dictionary<string, object> userClaimsDictionary = [];

		foreach (Claim? userClaim in userClaims)
		{
			if (userClaim is not null)
			{
				claims.Add(userClaim);

				// Try to parse the claim value as JSON
				object claimValue;

				try
				{
					using JsonDocument doc = JsonDocument.Parse(userClaim.Value);
					// If parsing succeeds, deserialize to object
					claimValue = JsonSerializer.Deserialize<object>(userClaim.Value) ?? userClaim.Value;
				}
				catch (JsonException)
				{
					// If parsing fails, it's not JSON - keep as string
					claimValue = userClaim.Value;
				}

				userClaimsDictionary.Add(userClaim.Type, claimValue);
			}
		}

		userData.Add("UserClaims", userClaimsDictionary);
		JwtSecurityToken token = new(new JwtHeader(signingCredentials), new JwtPayload(claims));
		string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
		TokenResponse output = new(AccessToken: tokenString, UserData: userData);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning TokenResponse {TokenResponse}.", nameof(AuthenticationController), nameof(GenerateToken), output);
		}

		return output;
	}

	private IdentityUser? ValidateCredentials(TokenRequest request)
	{
		// TODO add password obfuscation for logging.
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} called with TokenRequest = {TokenRequest}", nameof(AuthenticationController), nameof(ValidateCredentials), request);
		}

		IdentityUser? user = _userManager.FindByEmailAsync(request.Email).Result;

		if (user is null)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning User = {User} as no user found with that email.", nameof(AuthenticationController), nameof(ValidateCredentials), user);
			}

			return null;
		}

		bool isPasswordValid = _userManager.CheckPasswordAsync(user, request.Password).Result;

		if (!isPasswordValid)
		{
			// Increment access failed count
			_userManager.AccessFailedAsync(user).Wait();

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Class} {Method} returning User = null instead of {User} because password was invalid.", nameof(AuthenticationController), nameof(ValidateCredentials), user);
			}

			return null;
		}

		// Reset access failed count on successful login
		_userManager.ResetAccessFailedCountAsync(user).Wait();
		_loginTime = new DateTimeOffset(DateTime.Now);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Class} {Method} returning User = {User} with valid password.", nameof(AuthenticationController), nameof(ValidateCredentials), user);
		}

		return user;
	}
}
