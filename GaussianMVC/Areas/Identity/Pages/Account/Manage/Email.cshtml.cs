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

namespace GaussianMVC.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Razor Page model for managing user email addresses in ASP.NET Core Identity.
/// Handles email display, email change requests with confirmation, and verification email resending.
/// </summary>
/// <param name="userManager">The UserManager instance for managing user account operations.</param>
/// <param name="emailSender">The email sender service for sending confirmation and verification emails.</param>
public class EmailModel(
	UserManager<IdentityUser> userManager,
	IEmailSender emailSender) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly IEmailSender _emailSender = emailSender;

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public string Email { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public bool IsEmailConfirmed { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[TempData]
	public string StatusMessage { get; set; }

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
		[Display(Name = "New email")]
		public string NewEmail { get; set; }
	}

	/// <summary>
	/// Asynchronously loads the user's email information and confirmation status into the current model.
	/// </summary>
	/// <param name="user">The user whose email information is to be loaded. Cannot be null.</param>
	/// <returns>A task that represents the asynchronous load operation.</returns>
	private async Task LoadAsync(IdentityUser user)
	{
		string email = await _userManager.GetEmailAsync(user).ConfigureAwait(false);
		Email = email;

		Input = new InputModel
		{
			NewEmail = email,
		};

		IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false);
	}

	/// <summary>
	/// Handles HTTP GET requests to display the email management page.
	/// Loads the current user's email information and confirmation status.
	/// </summary>
	/// <returns>
	/// A <see cref="Task{IActionResult}"/> representing the asynchronous operation.
	/// Returns <see cref="NotFoundObjectResult"/> if the user cannot be found;
	/// otherwise, returns <see cref="PageResult"/> displaying the email management page.
	/// </returns>
	public async Task<IActionResult> OnGetAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		await LoadAsync(user).ConfigureAwait(false);
		return Page();
	}

	/// <summary>
	/// Handles HTTP POST requests to change the user's email address.
	/// Generates a confirmation token and sends a confirmation email to the new address.
	/// </summary>
	/// <returns>
	/// A <see cref="Task{IActionResult}"/> representing the asynchronous operation.
	/// Returns <see cref="NotFoundObjectResult"/> if the user cannot be found;
	/// returns <see cref="PageResult"/> if model validation fails;
	/// otherwise, returns <see cref="RedirectToPageResult"/> with a status message indicating
	/// whether a confirmation email was sent or if the email was unchanged.
	/// </returns>
	public async Task<IActionResult> OnPostChangeEmailAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		if (!ModelState.IsValid)
		{
			await LoadAsync(user).ConfigureAwait(false);
			return Page();
		}

		string email = await _userManager.GetEmailAsync(user).ConfigureAwait(false);
		if (Input.NewEmail != email)
		{
			string userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
			string code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail).ConfigureAwait(false);
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
			string callbackUrl = Url.Page(
				"/Account/ConfirmEmailChange",
				pageHandler: null,
				values: new { area = "Identity", userId, email = Input.NewEmail, code },
				protocol: Request.Scheme);
			await _emailSender.SendEmailAsync(
				Input.NewEmail,
				"Confirm your email",
				$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.").ConfigureAwait(false);

			StatusMessage = "Confirmation link to change email sent. Please check your email.";
			return RedirectToPage();
		}

		StatusMessage = "Your email is unchanged.";
		return RedirectToPage();
	}

	/// <summary>
	/// Handles HTTP POST requests to resend the email verification to the user's current email address.
	/// Generates a new email confirmation token and sends a verification email.
	/// </summary>
	/// <returns>
	/// A <see cref="Task{IActionResult}"/> representing the asynchronous operation.
	/// Returns <see cref="NotFoundObjectResult"/> if the user cannot be found;
	/// returns <see cref="PageResult"/> if model validation fails;
	/// otherwise, returns <see cref="RedirectToPageResult"/> with a status message indicating
	/// that the verification email was sent.
	/// </returns>
	public async Task<IActionResult> OnPostSendVerificationEmailAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		if (!ModelState.IsValid)
		{
			await LoadAsync(user).ConfigureAwait(false);
			return Page();
		}

		string userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
		string email = await _userManager.GetEmailAsync(user).ConfigureAwait(false);
		string code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
		code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
		string callbackUrl = Url.Page(
			"/Account/ConfirmEmail",
			pageHandler: null,
			values: new { area = "Identity", userId, code },
			protocol: Request.Scheme);
		await _emailSender.SendEmailAsync(
			email,
			"Confirm your email",
			$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.").ConfigureAwait(false);

		StatusMessage = "Verification email sent. Please check your email.";
		return RedirectToPage();
	}
}
