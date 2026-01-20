// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaussianMVC.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Initializes a new instance of the <see cref="GenerateRecoveryCodesModel"/> class.
/// This page model handles the generation of new two-factor authentication recovery codes for users.
/// </summary>
/// <param name="userManager">The user manager for managing Identity users.</param>
/// <param name="logger">The logger for recording recovery code generation events.</param>
public class GenerateRecoveryCodesModel(
	UserManager<IdentityUser> userManager,
	ILogger<GenerateRecoveryCodesModel> logger) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly ILogger<GenerateRecoveryCodesModel> _logger = logger;

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
	/// Handles the GET request for the recovery code generation page.
	/// Verifies that the current user exists and has two-factor authentication enabled before displaying the page.
	/// </summary>
	/// <returns>
	/// A <see cref="NotFoundObjectResult"/> if the user cannot be found;
	/// otherwise, a <see cref="PageResult"/> to display the recovery code generation page.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when the user does not have two-factor authentication enabled.
	/// Recovery codes can only be generated for users with 2FA enabled.
	/// </exception>
	public async Task<IActionResult> OnGetAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		bool isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user).ConfigureAwait(false);
		return !isTwoFactorEnabled
			? throw new InvalidOperationException($"Cannot generate recovery codes for user because they do not have 2FA enabled.")
			: (IActionResult)Page();
	}

	/// <summary>
	/// Handles the POST request to generate new two-factor authentication recovery codes.
	/// Generates 10 new recovery codes for the current user, stores them in TempData,
	/// logs the generation event, and redirects to the page that displays the codes.
	/// </summary>
	/// <returns>
	/// A <see cref="NotFoundObjectResult"/> if the user cannot be found;
	/// otherwise, a <see cref="RedirectToPageResult"/> to the ShowRecoveryCodes page
	/// where the newly generated codes are displayed.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when the user does not have two-factor authentication enabled.
	/// Recovery codes can only be generated for users with 2FA enabled.
	/// </exception>
	public async Task<IActionResult> OnPostAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		bool isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user).ConfigureAwait(false);
		string userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
		if (!isTwoFactorEnabled)
		{
			throw new InvalidOperationException($"Cannot generate recovery codes for user as they do not have 2FA enabled.");
		}

		IEnumerable<string> recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10).ConfigureAwait(false);
		RecoveryCodes = [.. recoveryCodes];

		_logger.LogInformation("User with ID '{UserId}' has generated new 2FA recovery codes.", userId);
		StatusMessage = "You have generated new recovery codes.";
		return RedirectToPage("./ShowRecoveryCodes");
	}
}
