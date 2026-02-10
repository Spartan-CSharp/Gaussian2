// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaussianMVC.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Represents the page model for resetting a user's authenticator app key in the ASP.NET Core Identity UI. This class
/// is used to handle requests for resetting two-factor authentication credentials.
/// </summary>
/// <remarks>This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
/// directly from your code. The API may change or be removed in future releases.</remarks>
/// <param name="userManager">The user manager used to manage user accounts and perform identity operations.</param>
/// <param name="signInManager">The sign-in manager used to manage user sign-in operations.</param>
/// <param name="logger">The logger used to record information and diagnostic messages for this page model.</param>
public class ResetAuthenticatorModel(
	UserManager<IdentityUser> userManager,
	SignInManager<IdentityUser> signInManager,
	ILogger<ResetAuthenticatorModel> logger) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly SignInManager<IdentityUser> _signInManager = signInManager;
	private readonly ILogger<ResetAuthenticatorModel> _logger = logger;

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[TempData]
	public string StatusMessage { get; set; }

	/// <summary>
	/// Handles GET requests to display the reset authenticator confirmation page. This method validates that the current
	/// user exists before allowing them to proceed with resetting their authenticator app key.
	/// </summary>
	/// <returns>
	/// A <see cref="NotFoundObjectResult"/> if the user cannot be found in the system; otherwise, returns the
	/// <see cref="Page"/> result to display the reset authenticator confirmation view.
	/// </returns>
	public async Task<IActionResult> OnGetAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		return user == null ? NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.") : Page();
	}

	/// <summary>
	/// Handles POST requests to reset the user's authenticator app key. This method performs the following operations:
	/// disables two-factor authentication, resets the authenticator key, logs the reset action, refreshes the user's
	/// sign-in session, and sets a status message informing the user to reconfigure their authenticator app.
	/// </summary>
	/// <returns>
	/// A <see cref="NotFoundObjectResult"/> if the user cannot be found in the system; otherwise, redirects to the
	/// <see cref="EnableAuthenticatorModel"/> page where the user can configure their authenticator app with the new key.
	/// </returns>
	public async Task<IActionResult> OnPostAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		_ = await _userManager.SetTwoFactorEnabledAsync(user, false).ConfigureAwait(false);
		_ = await _userManager.ResetAuthenticatorKeyAsync(user).ConfigureAwait(false);

		_ = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);

		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id); 
		}

		await _signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
		StatusMessage = "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.";

		return RedirectToPage("./EnableAuthenticator");
	}
}
