// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaussianMVC.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Razor Page model for enabling two-factor authentication using an authenticator app.
/// Handles the setup process including generating and displaying the shared key and QR code,
/// verifying the authenticator code, and generating recovery codes.
/// </summary>
/// <param name="userManager">The user manager for managing identity users and their authentication settings.</param>
/// <param name="logger">The logger for recording 2FA setup events and errors.</param>
/// <param name="urlEncoder">The URL encoder for generating the authenticator URI used in QR codes.</param>
public class EnableAuthenticatorModel(
	UserManager<IdentityUser> userManager,
	ILogger<EnableAuthenticatorModel> logger,
	UrlEncoder urlEncoder) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly ILogger<EnableAuthenticatorModel> _logger = logger;
	private readonly UrlEncoder _urlEncoder = urlEncoder;

	private const string _authenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public string SharedKey { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public string AuthenticatorUri { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[TempData]
	public string[] RecoveryCodes { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[TempData]
	public string StatusMessage { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[BindProperty]
	public InputModel Input { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public class InputModel
	{
		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[Required]
		[StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Text)]
		[Display(Name = "Verification Code")]
		public string Code { get; set; }
	}

	/// <summary>
	/// Handles GET requests for the page by loading the current user's shared key and QR code information required for
	/// two-factor authentication setup.
	/// </summary>
	/// <remarks>This method is typically called by the framework when the page is accessed via a GET request. It
	/// ensures that the necessary two-factor authentication setup data is available for the current user.</remarks>
	/// <returns>A <see cref="PageResult"/> if the user is found and the shared key and QR code are loaded successfully; otherwise,
	/// a <see cref="NotFoundResult"/> if the user cannot be loaded.</returns>
	public async Task<IActionResult> OnGetAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		await LoadSharedKeyAndQrCodeUriAsync(user).ConfigureAwait(false);

		return Page();
	}

	/// <summary>
	/// Handles the HTTP POST request to verify the authenticator app code and enable two-factor authentication (2FA) for
	/// the current user.
	/// </summary>
	/// <remarks>If the verification code is invalid or the model state is not valid, the method reloads the
	/// shared key and QR code URI and returns the page for correction. Upon successful verification, two-factor
	/// authentication is enabled for the user, and recovery codes are generated if none exist. The method requires the
	/// user to be authenticated and available in the current context.</remarks>
	/// <returns>An <see cref="IActionResult"/> that renders the page, redirects to the recovery codes page, or redirects to the
	/// two-factor authentication page, depending on the outcome of the verification process.</returns>
	public async Task<IActionResult> OnPostAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		if (!ModelState.IsValid)
		{
			await LoadSharedKeyAndQrCodeUriAsync(user).ConfigureAwait(false);
			return Page();
		}

		// Strip spaces and hyphens
		string verificationCode = Input.Code.Replace(" ", string.Empty, StringComparison.InvariantCulture).Replace("-", string.Empty, StringComparison.InvariantCulture);

		bool is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
			user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode).ConfigureAwait(false);

		if (!is2faTokenValid)
		{
			ModelState.AddModelError("Input.Code", "Verification code is invalid.");
			await LoadSharedKeyAndQrCodeUriAsync(user).ConfigureAwait(false);
			return Page();
		}

		_ = await _userManager.SetTwoFactorEnabledAsync(user, true).ConfigureAwait(false);
		string userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("User with ID '{UserId}' has enabled 2FA with an authenticator app.", userId); 
		}

		StatusMessage = "Your authenticator app has been verified.";

		if (await _userManager.CountRecoveryCodesAsync(user).ConfigureAwait(false) == 0)
		{
			IEnumerable<string> recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10).ConfigureAwait(false);
			RecoveryCodes = [.. recoveryCodes];
			return RedirectToPage("./ShowRecoveryCodes");
		}
		else
		{
			return RedirectToPage("./TwoFactorAuthentication");
		}
	}

	private async Task LoadSharedKeyAndQrCodeUriAsync(IdentityUser user)
	{
		// Load the authenticator key & QR code URI to display on the form
		string unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false);
		if (string.IsNullOrEmpty(unformattedKey))
		{
			_ = await _userManager.ResetAuthenticatorKeyAsync(user).ConfigureAwait(false);
			unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false);
		}

		SharedKey = FormatKey(unformattedKey);

		string email = await _userManager.GetEmailAsync(user).ConfigureAwait(false);
		AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey);
	}

	private static string FormatKey(string unformattedKey)
	{
		StringBuilder result = new();
		int currentPosition = 0;
		while (currentPosition + 4 < unformattedKey.Length)
		{
			_ = result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
			currentPosition += 4;
		}

		if (currentPosition < unformattedKey.Length)
		{
			_ = result.Append(unformattedKey.AsSpan(currentPosition));
		}

		return result.ToString().ToLowerInvariant();
	}

	private string GenerateQrCodeUri(string email, string unformattedKey)
	{
		return string.Format(
			CultureInfo.InvariantCulture,
			_authenticatorUriFormat,
			_urlEncoder.Encode("Microsoft.AspNetCore.Identity.UI"),
			_urlEncoder.Encode(email),
			unformattedKey);
	}
}
