// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaussianMVC.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Page model for managing two-factor authentication settings.
/// Handles display of authenticator status, recovery codes, and browser remember functionality.
/// </summary>
/// <param name="userManager">The user manager for identity operations.</param>
/// <param name="signInManager">The sign-in manager for authentication operations.</param>
public class TwoFactorAuthenticationModel(
	UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly SignInManager<IdentityUser> _signInManager = signInManager;

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public bool HasAuthenticator { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public int RecoveryCodesLeft { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[BindProperty]
	public bool Is2faEnabled { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public bool IsMachineRemembered { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[TempData]
	public string StatusMessage { get; set; }

	/// <summary>
	/// Handles GET requests to display the two-factor authentication management page.
	/// Loads and populates the current user's 2FA settings including authenticator status,
	/// whether 2FA is enabled, if the current browser is remembered, and the number of remaining recovery codes.
	/// </summary>
	/// <returns>
	/// A <see cref="PageResult"/> to display the two-factor authentication page, or
	/// a <see cref="NotFoundObjectResult"/> if the current user cannot be loaded.
	/// </returns>
	public async Task<IActionResult> OnGetAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false) != null;
		Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user).ConfigureAwait(false);
		IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user).ConfigureAwait(false);
		RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user).ConfigureAwait(false);

		return Page();
	}

	/// <summary>
	/// Handles POST requests to forget the current browser for two-factor authentication.
	/// When this method is called, the current browser is removed from the list of remembered devices,
	/// requiring the user to provide a 2FA code on their next login from this browser.
	/// </summary>
	/// <returns>
	/// A <see cref="RedirectToPageResult"/> that refreshes the current page with an updated status message, or
	/// a <see cref="NotFoundObjectResult"/> if the current user cannot be loaded.
	/// </returns>
	public async Task<IActionResult> OnPostAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		await _signInManager.ForgetTwoFactorClientAsync().ConfigureAwait(false);
		StatusMessage = "The current browser has been forgotten. When you login again from this browser you will be prompted for your 2fa code.";
		return RedirectToPage();
	}
}
