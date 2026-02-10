// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaussianMVC.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Model for disabling two-factor authentication (2FA) for a user.
/// </summary>
/// <param name="userManager">The user manager for managing user accounts.</param>
/// <param name="logger">The logger for logging events and errors.</param>
public class Disable2faModel(UserManager<IdentityUser> userManager, ILogger<Disable2faModel> logger) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly ILogger<Disable2faModel> _logger = logger;

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[TempData]
	public string StatusMessage { get; set; }

	/// <summary>
	/// Handles GET requests to display the disable 2FA page.
	/// Verifies that the current user exists and has two-factor authentication currently enabled.
	/// </summary>
	/// <returns>
	/// A <see cref="PageResult"/> if 2FA is enabled and can be disabled;
	/// otherwise, a <see cref="NotFoundObjectResult"/> if the user cannot be found.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when attempting to disable 2FA for a user that does not have 2FA currently enabled.
	/// </exception>
	public async Task<IActionResult> OnGetAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		return user == null
			? NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.")
			: !await _userManager.GetTwoFactorEnabledAsync(user).ConfigureAwait(false)
			? throw new InvalidOperationException($"Cannot disable 2FA for user as it's not currently enabled.")
			: (IActionResult)Page();
	}

	/// <summary>
	/// Handles POST requests to disable two-factor authentication for the current user.
	/// Disables 2FA, logs the action, sets a status message, and redirects to the two-factor authentication management page.
	/// </summary>
	/// <returns>
	/// A <see cref="RedirectToPageResult"/> to the TwoFactorAuthentication page upon successful disabling of 2FA;
	/// otherwise, a <see cref="NotFoundObjectResult"/> if the user cannot be found.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when an unexpected error occurs while attempting to disable two-factor authentication.
	/// </exception>
	public async Task<IActionResult> OnPostAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		IdentityResult disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false).ConfigureAwait(false);
		if (!disable2faResult.Succeeded)
		{
			throw new InvalidOperationException($"Unexpected error occurred disabling 2FA.");
		}

		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("User with ID '{UserId}' has disabled 2fa.", _userManager.GetUserId(User)); 
		}

		StatusMessage = "2fa has been disabled. You can reenable 2fa when you setup an authenticator app";
		return RedirectToPage("./TwoFactorAuthentication");
	}
}
