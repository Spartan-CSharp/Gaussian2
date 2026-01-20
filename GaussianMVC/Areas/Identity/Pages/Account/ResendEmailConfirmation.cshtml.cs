// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace GaussianMVC.Areas.Identity.Pages.Account;

/// <summary>
/// Page model for handling the resending of email confirmation links to users.
/// Allows users to request a new confirmation email if they did not receive the original one.
/// This page is accessible to anonymous users.
/// </summary>
/// <param name="userManager">The UserManager instance used to manage user operations, including finding users and generating email confirmation tokens.</param>
/// <param name="emailSender">The email sender service used to send the confirmation email to the user.</param>
[AllowAnonymous]
public class ResendEmailConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender emailSender) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly IEmailSender _emailSender = emailSender;

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
	public class InputModel
	{
		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}

	/// <summary>
	/// Handles GET requests to display the resend email confirmation page.
	/// This method is called when the user navigates to the page to request a new confirmation email.
	/// </summary>
	public void OnGet()
	{
	}

	/// <summary>
	/// Handles POST requests to resend the email confirmation link to the user.
	/// Validates the provided email address, generates a new confirmation token, and sends it to the user.
	/// For security purposes, displays a generic success message regardless of whether the email exists in the system.
	/// </summary>
	/// <returns>
	/// A <see cref="Task{IActionResult}"/> representing the asynchronous operation.
	/// Returns the current page with a status message indicating that the verification email has been sent.
	/// </returns>
	/// <remarks>
	/// This method implements the following workflow:
	/// <list type="number">
	/// <item><description>Validates the model state to ensure the email format is correct.</description></item>
	/// <item><description>Searches for a user with the provided email address.</description></item>
	/// <item><description>If found, generates a new email confirmation token using Base64 URL encoding.</description></item>
	/// <item><description>Constructs a callback URL pointing to the ConfirmEmail page with the user ID and token.</description></item>
	/// <item><description>Sends the confirmation email with an HTML-encoded link.</description></item>
	/// <item><description>Displays a generic message to prevent email enumeration attacks.</description></item>
	/// </list>
	/// Note: The same message is shown whether or not the email exists to prevent attackers from determining valid email addresses.
	/// </remarks>
	public async Task<IActionResult> OnPostAsync()
	{
		if (!ModelState.IsValid)
		{
			return Page();
		}

		IdentityUser user = await _userManager.FindByEmailAsync(Input.Email).ConfigureAwait(false);
		if (user == null)
		{
			ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
			return Page();
		}

		string userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
		string code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
		code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
		string callbackUrl = Url.Page(
			"/Account/ConfirmEmail",
			pageHandler: null,
			values: new { userId, code },
			protocol: Request.Scheme);
		await _emailSender.SendEmailAsync(
			Input.Email,
			"Confirm your email",
			$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.").ConfigureAwait(false);

		ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
		return Page();
	}
}
