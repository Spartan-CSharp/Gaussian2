using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Asp.Versioning;

using GaussianMVC.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GaussianMVC.Controllers.APIControllers;

/// <summary>
/// API controller responsible for handling authentication and JWT token generation.
/// Provides endpoints for user authentication and token-based authorization.
/// </summary>
/// <param name="context">The application database context for accessing user identity data.</param>
/// <param name="userManager">The ASP.NET Core Identity user manager for user operations.</param>
/// <param name="logger">The logger instance for recording authentication events and diagnostics.</param>
/// <param name="config">The application configuration providing authentication settings.</param>
[Route("api/[controller]")]
[ApiController]
[ApiVersionNeutral]
public class AuthenticationController(ApplicationDbContext context, UserManager<IdentityUser> userManager, ILogger<AuthenticationController> logger, IConfiguration config) : ControllerBase
{
	private readonly ApplicationDbContext _context = context;
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly ILogger<AuthenticationController> _logger = logger;
	private readonly IConfiguration _config = config;
	private DateTimeOffset _loginTime;

	/// <summary>
	/// Represents a request to obtain an authentication token.
	/// </summary>
	/// <param name="Email">The user's email address used for authentication.</param>
	/// <param name="Password">The user's password for credential validation.</param>
	public record TokenRequest(string Email, string Password);

	/// <summary>
	/// Represents the response containing the generated authentication token.
	/// </summary>
	/// <param name="AccessToken">The JWT access token string used for subsequent API requests.</param>
	/// <param name="UserName">The authenticated user's username.</param>
	public record TokenResponse(string AccessToken, string UserName);

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
	/// <remarks>
	/// This endpoint is publicly accessible (no authentication required) and follows the OAuth 2.0 token endpoint pattern.
	/// The generated JWT token includes user identity claims, role claims, and custom claims from the identity system.
	/// Token expiration is set to 24 hours from the time of generation.
	/// 
	/// Example request:
	/// POST /api/authentication/token
	/// {
	///   "Email": "user@example.com",
	///   "Password": "SecurePassword123!"
	/// }
	/// 
	/// Example response:
	/// {
	///   "AccessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
	///   "UserName": "user@example.com"
	/// }
	/// </remarks>
	// POST api/Authentication/token
	[HttpPost("token")]
	[AllowAnonymous]
	public async Task<ActionResult<TokenResponse>> PostAsync([FromBody] TokenRequest request)
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("{Controller} {Action} {RequstBody}", nameof(AuthenticationController), nameof(PostAsync), request);
			}

			if (request is null)
			{
				if (_logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning("{Controller} {Action} {RequstBody} has a null request", nameof(AuthenticationController), nameof(PostAsync), request);
				}

				return BadRequest($"The request body {nameof(request)} cannot be null.");
			}

			IdentityUser? user = ValidateCredentials(request);

			if (user is null)
			{
				if (_logger.IsEnabled(LogLevel.Information))
				{
					_logger.LogInformation("{Controller} {Action} {RequstBody} {Method} returned null", nameof(AuthenticationController), nameof(PostAsync), request, nameof(ValidateCredentials));
				}

				return Unauthorized();
			}

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Controller} {Action} {RequstBody} {Method} returned {User}", nameof(AuthenticationController), nameof(PostAsync), request, nameof(ValidateCredentials), user.UserName);
			}

			TokenResponse token = GenerateToken(user);

			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Controller} {Action} {RequstBody} {Method} returned {User}", nameof(AuthenticationController), nameof(PostAsync), request, nameof(GenerateToken), token);
			}

			return Ok(token);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{Controller} {Action} {RequstBody} had an error", nameof(AuthenticationController), nameof(PostAsync), request);
			}

			return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
		}
	}

	private TokenResponse GenerateToken(IdentityUser user)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {User}", nameof(GenerateToken), user.UserName);
		}

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

		IQueryable<IdentityRole> userRoles = from ur in _context.UserRoles
											 join r in _context.Roles on ur.RoleId equals r.Id
											 where ur.UserId == user.Id
											 select r;

		foreach (IdentityRole role in userRoles)
		{
			if (!string.IsNullOrEmpty(role.Name))
			{
				claims.Add(new Claim(ClaimTypes.Role, role.Name));
			}

			IQueryable<IdentityRoleClaim<string>> roleClaims = from rc in _context.RoleClaims
															   where rc.RoleId == role.Id
															   select rc;

			foreach (IdentityRoleClaim<string>? roleClaim in roleClaims)
			{
				if (roleClaim is not null && !string.IsNullOrEmpty(roleClaim.ClaimType) && !string.IsNullOrEmpty(roleClaim.ClaimValue))
				{
					claims.Add(new Claim(roleClaim.ClaimType, roleClaim.ClaimValue));
				}
			}
		}

		IQueryable<IdentityUserClaim<string>> userClaims = from uc in _context.UserClaims
														   where uc.UserId == user.Id
														   select uc;

		foreach (IdentityUserClaim<string>? userClaim in userClaims)
		{
			if (userClaim is not null && !string.IsNullOrEmpty(userClaim.ClaimType) && !string.IsNullOrEmpty(userClaim.ClaimValue))
			{
				claims.Add(new Claim(userClaim.ClaimType, userClaim.ClaimValue));
			}
		}

		JwtSecurityToken token = new(new JwtHeader(signingCredentials), new JwtPayload(claims));

		string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

		TokenResponse output = new(AccessToken: tokenString, UserName: user.UserName ?? string.Empty);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Returning {TokenResponse}", nameof(GenerateToken), output);
		}

		return output;
	}

	private IdentityUser? ValidateCredentials(TokenRequest request)
	{
		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Called with {TokenRequest}", nameof(ValidateCredentials), request);
		}

		IdentityUser? user = _userManager.FindByEmailAsync(request.Email).Result;

		if (user is null)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} Authentication Failed with {Email}: user is null", nameof(ValidateCredentials), request.Email);
			}

			return null;
		}

		bool isPasswordValid = _userManager.CheckPasswordAsync(user, request.Password).Result;

		if (!isPasswordValid)
		{
			if (_logger.IsEnabled(LogLevel.Trace))
			{
				_logger.LogTrace("{Method} Authentication Failed for {User}: {Password} is invalid", nameof(ValidateCredentials), user.UserName, request.Password);
			}

			return null;
		}

		_loginTime = new DateTimeOffset(DateTime.Now);

		if (_logger.IsEnabled(LogLevel.Trace))
		{
			_logger.LogTrace("{Method} Authentication Succeeded for {User}", nameof(ValidateCredentials), user.UserName);
		}

		return user;
	}
}
