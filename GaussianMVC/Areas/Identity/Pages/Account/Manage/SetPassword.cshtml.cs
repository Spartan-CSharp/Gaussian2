// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaussianMVC.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Represents the page model for setting a password for a user who does not have one, as part of the ASP.NET Core
/// Identity default UI infrastructure.
/// </summary>
/// <remarks>This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
/// directly from your code. The API may change or be removed in future releases.</remarks>
/// <param name="userManager">The user manager used to manage user accounts and perform user-related operations.</param>
/// <param name="signInManager">The sign-in manager used to manage user sign-in operations.</param>
public class SetPasswordModel(
	UserManager<IdentityUser> userManager,
	SignInManager<IdentityUser> signInManager) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly SignInManager<IdentityUser> _signInManager = signInManager;

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
	[TempData]
	public string StatusMessage { get; set; }

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
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "New password")]
		public string NewPassword { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[DataType(DataType.Password)]
		[Display(Name = "Confirm new password")]
		[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	/// <summary>
	/// Handles GET requests to the Set Password page. Retrieves the current user and determines whether they should
	/// be redirected to the Change Password page (if they already have a password) or allowed to set a new password.
	/// </summary>
	/// <returns>
	/// A <see cref="NotFoundObjectResult"/> if the user cannot be loaded;
	/// a redirect to the Change Password page if the user already has a password;
	/// otherwise, the current page.
	/// </returns>
	public async Task<IActionResult> OnGetAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		bool hasPassword = await _userManager.HasPasswordAsync(user).ConfigureAwait(false);

		return hasPassword ? RedirectToPage("./ChangePassword") : Page();
	}

	/// <summary>
	/// Handles POST requests to set a new password for the current user. Validates the input model, retrieves the
	/// current user, adds the specified password to their account, and refreshes the sign-in cookie upon success.
	/// </summary>
	/// <returns>
	/// A <see cref="NotFoundObjectResult"/> if the user cannot be loaded;
	/// the current page with validation errors if the model state is invalid or the password addition fails;
	/// otherwise, a redirect to the current page with a success status message.
	/// </returns>
	public async Task<IActionResult> OnPostAsync()
	{
		if (!ModelState.IsValid)
		{
			return Page();
		}

		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		IdentityResult addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword).ConfigureAwait(false);
		if (!addPasswordResult.Succeeded)
		{
			foreach (IdentityError error in addPasswordResult.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}

			return Page();
		}

		await _signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
		StatusMessage = "Your password has been set.";

		return RedirectToPage();
	}
}
