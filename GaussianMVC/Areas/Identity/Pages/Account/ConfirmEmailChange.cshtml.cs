// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace GaussianMVC.Areas.Identity.Pages.Account;

/// <summary>
/// Represents the page model for confirming an email address change in an ASP.NET Core Identity application.
/// </summary>
/// <remarks>This page model is typically used as part of the ASP.NET Core Identity default UI to handle email
/// change confirmation requests. It coordinates user lookup, email and user name updates, and sign-in refresh
/// operations after a successful confirmation.</remarks>
/// <param name="userManager">The user manager used to retrieve and update user information, including email and user name changes.</param>
/// <param name="signInManager">The sign-in manager used to refresh the user's sign-in session after the email change is confirmed.</param>
public class ConfirmEmailChangeModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly SignInManager<IdentityUser> _signInManager = signInManager;

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[TempData]
	public string StatusMessage { get; set; }

	/// <summary>
	/// Handles the confirmation of an email change for a user and updates the user's email and username if the
	/// confirmation code is valid.
	/// </summary>
	/// <remarks>In this application, the user's email and username are synchronized. When the email is changed, the
	/// username is also updated to match the new email address. The user's sign-in session is refreshed after a successful
	/// change to ensure the update is reflected immediately.</remarks>
	/// <param name="userId">The unique identifier of the user whose email is being changed. Cannot be null.</param>
	/// <param name="email">The new email address to assign to the user. Cannot be null.</param>
	/// <param name="code">The confirmation code used to validate the email change request. Must be a valid base64url-encoded string. Cannot
	/// be null.</param>
	/// <returns>An <see cref="IActionResult"/> that redirects to the index page if any parameter is null, returns a not found
	/// result if the user does not exist, or returns the page with a status message indicating the result of the email
	/// change operation.</returns>
	public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
	{
		if (userId == null || email == null || code == null)
		{
			return RedirectToPage("/Index");
		}

		IdentityUser user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{userId}'.");
		}

		code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
		IdentityResult result = await _userManager.ChangeEmailAsync(user, email, code).ConfigureAwait(false);
		if (!result.Succeeded)
		{
			StatusMessage = "Error changing email.";
			return Page();
		}

		// In our UI email and user name are one and the same, so when we update the email
		// we need to update the user name.
		IdentityResult setUserNameResult = await _userManager.SetUserNameAsync(user, email).ConfigureAwait(false);
		if (!setUserNameResult.Succeeded)
		{
			StatusMessage = "Error changing user name.";
			return Page();
		}

		await _signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
		StatusMessage = "Thank you for confirming your email change.";
		return Page();
	}
}
