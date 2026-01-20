// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaussianMVC.Areas.Identity.Pages.Account;

/// <summary>
/// Initializes a new instance of the <see cref="LoginWith2faModel"/> class.
/// Handles two-factor authentication (2FA) login process using authenticator codes.
/// </summary>
/// <param name="signInManager">The sign-in manager for handling user authentication operations.</param>
/// <param name="userManager">The user manager for retrieving and managing user information.</param>
/// <param name="logger">The logger for recording authentication events and errors.</param>
public class LoginWith2faModel(
	SignInManager<IdentityUser> signInManager,
	UserManager<IdentityUser> userManager,
	ILogger<LoginWith2faModel> logger) : PageModel
{
	private readonly SignInManager<IdentityUser> _signInManager = signInManager;
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly ILogger<LoginWith2faModel> _logger = logger;

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
	public bool RememberMe { get; set; }

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
		[Required]
		[StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Text)]
		[Display(Name = "Authenticator code")]
		public string TwoFactorCode { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[Display(Name = "Remember this machine")]
		public bool RememberMachine { get; set; }
	}

	/// <summary>
	/// Handles the GET request for the two-factor authentication login page.
	/// Validates that the user has successfully completed the initial username and password authentication
	/// before being allowed to proceed to the 2FA step.
	/// </summary>
	/// <param name="rememberMe">
	/// Indicates whether the user chose to be remembered on this device from the initial login attempt.
	/// This value will be passed through to the final sign-in operation if 2FA succeeds.
	/// </param>
	/// <param name="returnUrl">
	/// The URL to redirect to after successful two-factor authentication.
	/// If null, the user will be redirected to the application's home page.
	/// </param>
	/// <returns>
	/// A <see cref="PageResult"/> that displays the two-factor authentication input form,
	/// allowing the user to enter their authenticator code.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when no two-factor authentication user is found in the current sign-in context,
	/// indicating the user has not completed the initial username/password authentication step.
	/// </exception>
	public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
	{
		// Ensure the user has gone through the username & password screen first
		_ = await _signInManager.GetTwoFactorAuthenticationUserAsync().ConfigureAwait(false) ?? throw new InvalidOperationException($"Unable to load two-factor authentication user.");

		ReturnUrl = returnUrl;
		RememberMe = rememberMe;

		return Page();
	}

	/// <summary>
	/// Handles the POST request for two-factor authentication login verification.
	/// Validates the authenticator code entered by the user and completes the sign-in process
	/// if the code is valid. Handles various authentication outcomes including success,
	/// account lockout, and invalid codes.
	/// </summary>
	/// <param name="rememberMe">
	/// Indicates whether the user should remain signed in on this device after closing the browser.
	/// This value is passed to the sign-in manager for cookie persistence configuration.
	/// </param>
	/// <param name="returnUrl">
	/// The URL to redirect to after successful authentication.
	/// Defaults to the application's home page ("~/") if null or not provided.
	/// </param>
	/// <returns>
	/// A <see cref="LocalRedirectResult"/> to the return URL if authentication succeeds;
	/// a <see cref="RedirectToPageResult"/> to the Lockout page if the account is locked out;
	/// or a <see cref="PageResult"/> with a model error if the authenticator code is invalid
	/// or the model state is invalid.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when no two-factor authentication user is found in the current sign-in context,
	/// indicating the user session may have expired or the initial authentication was not completed.
	/// </exception>
	public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
	{
		if (!ModelState.IsValid)
		{
			return Page();
		}

		returnUrl ??= Url.Content("~/");

		IdentityUser user = await _signInManager.GetTwoFactorAuthenticationUserAsync().ConfigureAwait(false) ?? throw new InvalidOperationException($"Unable to load two-factor authentication user.");

		string authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty, StringComparison.InvariantCulture).Replace("-", string.Empty, StringComparison.InvariantCulture);

		Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, Input.RememberMachine).ConfigureAwait(false);

		_ = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);

		if (result.Succeeded)
		{
			_logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
			return LocalRedirect(returnUrl);
		}
		else if (result.IsLockedOut)
		{
			_logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
			return RedirectToPage("./Lockout");
		}
		else
		{
			_logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
			ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
			return Page();
		}
	}
}
