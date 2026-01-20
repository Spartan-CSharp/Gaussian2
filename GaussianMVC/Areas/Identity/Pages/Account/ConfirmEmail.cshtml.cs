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
/// Represents the page model for handling email confirmation requests in an ASP.NET Core Identity UI workflow.
/// </summary>
/// <remarks>This class is intended for use within the ASP.NET Core Identity default UI infrastructure. It is not
/// designed for direct use in application code and may change or be removed in future releases.</remarks>
/// <param name="userManager">The user manager used to manage and confirm user identities within the email confirmation process.</param>
public class ConfirmEmailModel(UserManager<IdentityUser> userManager) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[TempData]
	public string StatusMessage { get; set; }

	/// <summary>
	/// Handles the email confirmation process for a user account.
	/// Validates the confirmation token and marks the user's email as confirmed if the token is valid.
	/// </summary>
	/// <param name="userId">The unique identifier of the user whose email is being confirmed.</param>
	/// <param name="code">The Base64Url-encoded email confirmation token that was sent to the user's email address.</param>
	/// <returns>
	/// An <see cref="IActionResult"/> representing the result of the confirmation attempt:
	/// redirects to the index page if parameters are missing, returns a 404 if the user is not found,
	/// or returns the confirmation result page with an appropriate status message.
	/// </returns>
	public async Task<IActionResult> OnGetAsync(string userId, string code)
	{
		if (userId == null || code == null)
		{
			return RedirectToPage("/Index");
		}

		IdentityUser user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{userId}'.");
		}

		code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
		IdentityResult result = await _userManager.ConfirmEmailAsync(user, code).ConfigureAwait(false);
		StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
		return Page();
	}
}
