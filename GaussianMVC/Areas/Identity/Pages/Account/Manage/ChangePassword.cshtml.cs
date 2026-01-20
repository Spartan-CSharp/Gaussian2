// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaussianMVC.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Represents the page model for the change password functionality in ASP.NET Core Identity's default UI. This class
/// manages user interactions for changing an existing password, including validation and status messaging.
/// </summary>
/// <remarks>This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be
/// used directly from your code. The implementation and exposed members may change or be removed in future releases.
/// For custom password change workflows, consider implementing your own page model.</remarks>
/// <param name="userManager">The user manager used to retrieve and update user information, including password changes.</param>
/// <param name="signInManager">The sign-in manager used to refresh the user's sign-in session after a successful password change.</param>
/// <param name="logger">The logger used to record informational and error messages related to password change operations.</param>
public class ChangePasswordModel(
	UserManager<IdentityUser> userManager,
	SignInManager<IdentityUser> signInManager,
	ILogger<ChangePasswordModel> logger) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly SignInManager<IdentityUser> _signInManager = signInManager;
	private readonly ILogger<ChangePasswordModel> _logger = logger;

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
		[DataType(DataType.Password)]
		[Display(Name = "Current password")]
		public string OldPassword { get; set; }

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
	/// Handles GET requests for the page, ensuring the current user is loaded and redirecting to the password setup page
	/// if the user does not have a password.
	/// </summary>
	/// <remarks>This method is typically used in Razor Pages to enforce that a user has a password before
	/// allowing access to the page. If the user is not found, a 404 Not Found response is returned. If the user does not
	/// have a password, they are redirected to a page to set one.</remarks>
	/// <returns>An <see cref="IActionResult"/> that renders the page if the user has a password; otherwise, a redirect to the
	/// password setup page. Returns <see cref="NotFoundResult"/> if the user cannot be loaded.</returns>
	public async Task<IActionResult> OnGetAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		bool hasPassword = await _userManager.HasPasswordAsync(user).ConfigureAwait(false);
		return !hasPassword ? RedirectToPage("./SetPassword") : Page();
	}

	/// <summary>
	/// Handles the HTTP POST request to change the current user's password.
	/// </summary>
	/// <remarks>This method validates the input model and attempts to change the user's password using ASP.NET
	/// Core Identity. If the password change is successful, the user's sign-in session is refreshed and a status message
	/// is set. If validation fails or the password change is unsuccessful, the page is redisplayed with error messages.
	/// The method requires the user to be authenticated.</remarks>
	/// <returns>A <see cref="IActionResult"/> that renders the page with validation errors, redirects to the page upon successful
	/// password change, or returns a NotFound result if the user cannot be loaded.</returns>
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

		IdentityResult changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword).ConfigureAwait(false);
		if (!changePasswordResult.Succeeded)
		{
			foreach (IdentityError error in changePasswordResult.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}

			return Page();
		}

		await _signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
		_logger.LogInformation("User changed their password successfully.");
		StatusMessage = "Your password has been changed.";

		return RedirectToPage();
	}
}
