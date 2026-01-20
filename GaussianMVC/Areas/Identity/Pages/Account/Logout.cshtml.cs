// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaussianMVC.Areas.Identity.Pages.Account;

/// <summary>
/// Initializes a new instance of the <see cref="LogoutModel"/> page model for handling user logout operations.
/// </summary>
/// <param name="signInManager">The sign-in manager used to sign out the authenticated user.</param>
/// <param name="logger">The logger instance for recording logout operations and diagnostics.</param>
public class LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger) : PageModel
{
	private readonly SignInManager<IdentityUser> _signInManager = signInManager;
	private readonly ILogger<LogoutModel> _logger = logger;

	/// <summary>
	/// Handles the HTTP POST request to log out the currently authenticated user.
	/// Signs out the user, logs the logout event, and redirects to the appropriate page.
	/// </summary>
	/// <param name="returnUrl">Optional URL to redirect to after logout. If provided, the user will be redirected to this URL; otherwise, redirects to the current page.</param>
	/// <returns>
	/// A <see cref="LocalRedirectResult"/> if a return URL is specified, 
	/// or a <see cref="RedirectToPageResult"/> to refresh the current page and update the user's identity.
	/// </returns>
	public async Task<IActionResult> OnPostAsync(string returnUrl = null)
	{
		await _signInManager.SignOutAsync().ConfigureAwait(false);
		_logger.LogInformation("User logged out.");
		if (returnUrl != null)
		{
			return LocalRedirect(returnUrl);
		}
		else
		{
			// This needs to be a redirect so that the browser performs a new
			// request and the identity for the user gets updated.
			return RedirectToPage();
		}
	}
}
