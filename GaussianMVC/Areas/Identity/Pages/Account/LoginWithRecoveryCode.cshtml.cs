// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaussianMVC.Areas.Identity.Pages.Account;

/// <summary>
/// Razor Pages model for handling user login with a recovery code.
/// This page is used when a user needs to sign in using a recovery code during two-factor authentication.
/// </summary>
/// <param name="signInManager">The sign-in manager for handling user authentication operations.</param>
/// <param name="userManager">The user manager for managing Identity user operations.</param>
/// <param name="logger">The logger for recording login and error events.</param>
public class LoginWithRecoveryCodeModel(
	SignInManager<IdentityUser> signInManager,
	UserManager<IdentityUser> userManager,
	ILogger<LoginWithRecoveryCodeModel> logger) : PageModel
{
	private readonly SignInManager<IdentityUser> _signInManager = signInManager;
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly ILogger<LoginWithRecoveryCodeModel> _logger = logger;

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
	public string ReturnUrl { get; set; }

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
		[BindProperty]
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Recovery Code")]
		public string RecoveryCode { get; set; }
	}

	/// <summary>
	/// Handles GET requests for the recovery code login page.
	/// Validates that the user has completed the initial two-factor authentication challenge
	/// and displays the recovery code input form.
	/// </summary>
	/// <param name="returnUrl">The URL to redirect to after successful login. If null, redirects to the home page.</param>
	/// <returns>The recovery code login page.</returns>
	/// <exception cref="InvalidOperationException">Thrown when the two-factor authentication user cannot be loaded, 
	/// indicating the user has not completed the username and password authentication step.</exception>
	public async Task<IActionResult> OnGetAsync(string returnUrl = null)
	{
		// Ensure the user has gone through the username & password screen first
		_ = await _signInManager.GetTwoFactorAuthenticationUserAsync().ConfigureAwait(false) ?? throw new InvalidOperationException($"Unable to load two-factor authentication user.");

		ReturnUrl = returnUrl;

		return Page();
	}

	/// <summary>
	/// Handles POST requests for recovery code authentication.
	/// Validates the submitted recovery code and signs in the user if the code is valid.
	/// Handles various authentication outcomes including successful login, account lockout, and invalid codes.
	/// </summary>
	/// <param name="returnUrl">The URL to redirect to after successful login. If null, redirects to the home page.</param>
	/// <returns>
	/// - On success: Redirects to the returnUrl or home page.
	/// - On lockout: Redirects to the Lockout page.
	/// - On invalid code or model state: Returns the page with validation errors.
	/// </returns>
	/// <exception cref="InvalidOperationException">Thrown when the two-factor authentication user cannot be loaded,
	/// indicating the user has not completed the username and password authentication step.</exception>
	public async Task<IActionResult> OnPostAsync(string returnUrl = null)
	{
		if (!ModelState.IsValid)
		{
			return Page();
		}

		IdentityUser user = await _signInManager.GetTwoFactorAuthenticationUserAsync().ConfigureAwait(false) ?? throw new InvalidOperationException($"Unable to load two-factor authentication user.");

		string recoveryCode = Input.RecoveryCode.Replace(" ", string.Empty, StringComparison.InvariantCulture);

		Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode).ConfigureAwait(false);

		_ = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);

		if (result.Succeeded)
		{
			_logger.LogInformation("User with ID '{UserId}' logged in with a recovery code.", user.Id);
			return LocalRedirect(returnUrl ?? Url.Content("~/"));
		}

		if (result.IsLockedOut)
		{
			_logger.LogWarning("User account locked out.");
			return RedirectToPage("./Lockout");
		}
		else
		{
			_logger.LogWarning("Invalid recovery code entered for user with ID '{UserId}' ", user.Id);
			ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
			return Page();
		}
	}
}
