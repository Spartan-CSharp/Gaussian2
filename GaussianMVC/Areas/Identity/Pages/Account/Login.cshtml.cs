// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaussianMVC.Areas.Identity.Pages.Account;

/// <summary>
/// Initializes a new instance of the <see cref="LoginModel"/> class.
/// Handles user authentication for the login page.
/// </summary>
/// <param name="signInManager">The sign-in manager used to authenticate users and manage sign-in operations.</param>
/// <param name="logger">The logger instance for logging authentication events and errors.</param>
public class LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger) : PageModel
{
	private readonly SignInManager<IdentityUser> _signInManager = signInManager;
	private readonly ILogger<LoginModel> _logger = logger;

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
	public IList<AuthenticationScheme> ExternalLogins { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public string ReturnUrl { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[TempData]
	public string ErrorMessage { get; set; }

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
		[EmailAddress]
		public string Email { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}

	/// <summary>
	/// Handles the GET request for the login page, initializing the page state and preparing for user authentication.
	/// </summary>
	/// <remarks>
	/// This method prepares the login page by clearing any existing external authentication cookies to ensure a clean
	/// login process. It retrieves available external authentication schemes (such as Google, Microsoft, etc.) and
	/// populates the <see cref="ExternalLogins"/> property. If an error message exists from a previous request
	/// (stored in <see cref="ErrorMessage"/>), it is added to the model state for display to the user.
	/// </remarks>
	/// <param name="returnUrl">The URL to redirect to after a successful login. If null, defaults to the application's home page ("~/").</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	public async Task OnGetAsync(string returnUrl = null)
	{
		if (!string.IsNullOrEmpty(ErrorMessage))
		{
			ModelState.AddModelError(string.Empty, ErrorMessage);
		}

		returnUrl ??= Url.Content("~/");

		// Clear the existing external cookie to ensure a clean login process
		await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);

		ExternalLogins = [.. await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)];

		ReturnUrl = returnUrl;
	}

	/// <summary>
	/// Handles the POST request for user login, authenticating the user and redirecting based on the result.
	/// </summary>
	/// <remarks>If the login attempt fails, the method redisplays the login form with an error message. If
	/// two-factor authentication is required or the account is locked out, the user is redirected to the appropriate page.
	/// The method enables account lockout on repeated failed login attempts.</remarks>
	/// <param name="returnUrl">The URL to redirect to after a successful login. If null, defaults to the application's home page.</param>
	/// <returns>An <see cref="IActionResult"/> that redirects the user upon successful login, prompts for two-factor authentication
	/// if required, displays a lockout page if the account is locked, or redisplays the login form on failure.</returns>
	public async Task<IActionResult> OnPostAsync(string returnUrl = null)
	{
		returnUrl ??= Url.Content("~/");

		ExternalLogins = [.. await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)];

		if (ModelState.IsValid)
		{
			// This doesn't count login failures towards account lockout
			// To enable password failures to trigger account lockout, set lockoutOnFailure: true
			Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true).ConfigureAwait(false);
			if (result.Succeeded)
			{
				_logger.LogInformation("User logged in.");
				return LocalRedirect(returnUrl);
			}

			if (result.RequiresTwoFactor)
			{
				return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
			}

			if (result.IsLockedOut)
			{
				_logger.LogWarning("User account locked out.");
				return RedirectToPage("./Lockout");
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Invalid login attempt.");
				return Page();
			}
		}

		// If we got this far, something failed, redisplay form
		return Page();
	}
}
