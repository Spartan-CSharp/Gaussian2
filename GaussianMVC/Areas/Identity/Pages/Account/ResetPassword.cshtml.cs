// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace GaussianMVC.Areas.Identity.Pages.Account;

/// <summary>
/// Represents the page model for the password reset functionality.
/// Handles the display and processing of the password reset form where users can set a new password
/// using a reset code sent to their email address.
/// </summary>
/// <param name="userManager">The user manager service used to find users and reset their passwords.</param>
public class ResetPasswordModel(UserManager<IdentityUser> userManager) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;

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

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[Required]
		public string Code { get; set; }
	}

	/// <summary>
	/// Handles GET requests for the password reset page.
	/// Validates and decodes the password reset code from the query string,
	/// then initializes the form with the decoded code.
	/// </summary>
	/// <param name="code">The Base64Url encoded password reset code sent to the user's email.
	/// If null or not provided, the request is rejected.</param>
	/// <returns>
	/// A <see cref="BadRequestObjectResult"/> if the code is null,
	/// otherwise returns the password reset page with the decoded code populated in the form.
	/// </returns>
	public IActionResult OnGet(string code = null)
	{
		if (code == null)
		{
			return BadRequest("A code must be supplied for password reset.");
		}
		else
		{
			Input = new InputModel
			{
				Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
			};
			return Page();
		}
	}

	/// <summary>
	/// Handles POST requests to process the password reset form submission.
	/// Validates the submitted data, finds the user by email, and attempts to reset their password
	/// using the provided reset code. If the user is not found, redirects to confirmation page
	/// without revealing this information to prevent user enumeration.
	/// </summary>
	/// <returns>
	/// A <see cref="Task{IActionResult}"/> that represents the asynchronous operation.
	/// Returns a redirect to the ResetPasswordConfirmation page if the password reset succeeds
	/// or if the user is not found. Returns the current page with model errors if the model
	/// state is invalid or if the password reset fails.
	/// </returns>
	public async Task<IActionResult> OnPostAsync()
	{
		if (!ModelState.IsValid)
		{
			return Page();
		}

		IdentityUser user = await _userManager.FindByEmailAsync(Input.Email).ConfigureAwait(false);
		if (user == null)
		{
			// Don't reveal that the user does not exist
			return RedirectToPage("./ResetPasswordConfirmation");
		}

		IdentityResult result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password).ConfigureAwait(false);
		if (result.Succeeded)
		{
			return RedirectToPage("./ResetPasswordConfirmation");
		}

		foreach (IdentityError error in result.Errors)
		{
			ModelState.AddModelError(string.Empty, error.Description);
		}

		return Page();
	}
}
