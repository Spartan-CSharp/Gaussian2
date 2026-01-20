// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace GaussianMVC.Areas.Identity.Pages.Account;

/// <summary>
/// Represents the page model for handling the 'Forgot Password' workflow in ASP.NET Core Identity. This model manages
/// user input and initiates the password reset process by sending a reset link to the user's email address.
/// </summary>
/// <remarks>This class is intended for use with the ASP.NET Core Identity default UI and is not designed to be
/// used directly in application code. The implementation ensures that user existence and email confirmation status are
/// not disclosed to prevent information disclosure. For more information on enabling account confirmation and password
/// reset, see the official ASP.NET Core Identity documentation.</remarks>
/// <param name="userManager">The user manager used to retrieve and manage user accounts.</param>
/// <param name="emailSender">The email sender service used to send password reset emails to users.</param>
public class ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender) : PageModel
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
	/// Handles the POST request for the forgot password workflow. Validates the user's email input, generates a password
	/// reset token, and sends a password reset email with a callback URL if the user exists and their email is confirmed.
	/// </summary>
	/// <returns>
	/// A <see cref="Task{IActionResult}"/> representing the asynchronous operation. Returns a redirect to the
	/// <c>ForgotPasswordConfirmation</c> page if the model state is valid (regardless of whether the user exists or their
	/// email is confirmed, to prevent information disclosure), or returns the current page if the model state is invalid.
	/// </returns>
	/// <remarks>
	/// This method implements security best practices by not revealing whether a user exists or if their email is confirmed.
	/// Even when a user is not found or their email is not confirmed, the method redirects to the confirmation page to
	/// prevent enumeration attacks. The password reset token is Base64 URL-encoded before being included in the callback URL.
	/// </remarks>
	public async Task<IActionResult> OnPostAsync()
	{
		if (ModelState.IsValid)
		{
			IdentityUser user = await _userManager.FindByEmailAsync(Input.Email).ConfigureAwait(false);
			if (user == null || !await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false))
			{
				// Don't reveal that the user does not exist or is not confirmed
				return RedirectToPage("./ForgotPasswordConfirmation");
			}

			// For more information on how to enable account confirmation and password reset please
			// visit https://go.microsoft.com/fwlink/?LinkID=532713
			string code = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
			string callbackUrl = Url.Page(
				"/Account/ResetPassword",
				pageHandler: null,
				values: new { area = "Identity", code },
				protocol: Request.Scheme);

			await _emailSender.SendEmailAsync(
				Input.Email,
				"Reset Password",
				$"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.").ConfigureAwait(false);

			return RedirectToPage("./ForgotPasswordConfirmation");
		}

		return Page();
	}
}
