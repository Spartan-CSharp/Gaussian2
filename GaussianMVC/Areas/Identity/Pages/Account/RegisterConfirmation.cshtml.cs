// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Text;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace GaussianMVC.Areas.Identity.Pages.Account;

/// <summary>
/// Represents the page model for the user registration confirmation page in ASP.NET Core Identity. This model is used
/// to display confirmation information after a user registers an account.
/// </summary>
/// <remarks>This class is intended for use with the ASP.NET Core Identity default UI and is not designed to be
/// used directly in application code. The API surface and behavior may change in future releases.</remarks>
/// <param name="userManager">The user manager used to manage and query user accounts.</param>
[AllowAnonymous]
public class RegisterConfirmationModel(UserManager<IdentityUser> userManager) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public string Email { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public bool DisplayConfirmAccountLink { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public string EmailConfirmationUrl { get; set; }

	/// <summary>
	/// Handles the GET request for the registration confirmation page. This method loads user information
	/// based on the provided email address and generates an email confirmation URL for account verification.
	/// </summary>
	/// <param name="email">The email address of the user who just registered. This parameter is required.</param>
	/// <param name="returnUrl">The URL to redirect to after successful email confirmation. Defaults to the application root if not provided.</param>
	/// <returns>
	/// A <see cref="Task{IActionResult}"/> representing the asynchronous operation. Returns:
	/// <list type="bullet">
	/// <item><description>A redirect to the Index page if the email parameter is null.</description></item>
	/// <item><description>A <see cref="NotFoundObjectResult"/> if no user exists with the specified email address.</description></item>
	/// <item><description>The registration confirmation page with the email confirmation URL if the user is found.</description></item>
	/// </list>
	/// </returns>
	/// <remarks>
	/// This method sets <see cref="DisplayConfirmAccountLink"/> to true for development purposes, allowing
	/// manual email confirmation. In production environments with a real email sender configured, this
	/// behavior should be removed and the confirmation link should be sent via email instead.
	/// </remarks>
	public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
	{
		if (email == null)
		{
			return RedirectToPage("/Index");
		}

		returnUrl ??= Url.Content("~/");

		IdentityUser user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with email '{email}'.");
		}

		Email = email;
		// Once you add a real email sender, you should remove this code that lets you confirm the account
		DisplayConfirmAccountLink = true;
		if (DisplayConfirmAccountLink)
		{
			string userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
			string code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
			EmailConfirmationUrl = Url.Page(
				"/Account/ConfirmEmail",
				pageHandler: null,
				values: new { area = "Identity", userId, code, returnUrl },
				protocol: Request.Scheme);
		}

		return Page();
	}
}
