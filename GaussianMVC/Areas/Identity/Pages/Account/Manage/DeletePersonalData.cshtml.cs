// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaussianMVC.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Razor page model for handling user account deletion and personal data removal.
/// Provides functionality to permanently delete a user's account and all associated personal data.
/// </summary>
/// <param name="userManager">The user manager for managing user accounts and identity operations.</param>
/// <param name="signInManager">The sign-in manager for handling user authentication and sign-out operations.</param>
/// <param name="logger">The logger instance for recording account deletion events and errors.</param>
public class DeletePersonalDataModel(
	UserManager<IdentityUser> userManager,
	SignInManager<IdentityUser> signInManager,
	ILogger<DeletePersonalDataModel> logger) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly SignInManager<IdentityUser> _signInManager = signInManager;
	private readonly ILogger<DeletePersonalDataModel> _logger = logger;

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
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public bool RequirePassword { get; set; }

	/// <summary>
	/// Handles HTTP GET requests to display the delete personal data confirmation page.
	/// Retrieves the current user and determines whether password confirmation is required for account deletion.
	/// </summary>
	/// <returns>
	/// A <see cref="Task{IActionResult}"/> representing the asynchronous operation.
	/// Returns <see cref="NotFoundObjectResult"/> if the user cannot be found.
	/// Returns <see cref="PageResult"/> to display the delete personal data page.
	/// </returns>
	public async Task<IActionResult> OnGetAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		RequirePassword = await _userManager.HasPasswordAsync(user).ConfigureAwait(false);
		return Page();
	}

	/// <summary>
	/// Handles HTTP POST requests to permanently delete the user's account and all associated personal data.
	/// Validates the user's password (if required), performs the deletion, signs out the user, and redirects to the home page.
	/// </summary>
	/// <returns>
	/// A <see cref="Task{IActionResult}"/> representing the asynchronous operation.
	/// Returns <see cref="NotFoundObjectResult"/> if the user cannot be found.
	/// Returns <see cref="PageResult"/> if password validation fails (when password is required).
	/// Returns <see cref="RedirectResult"/> to the home page after successful deletion.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when an unexpected error occurs during the user deletion process.
	/// </exception>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	public async Task<IActionResult> OnPostAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		RequirePassword = await _userManager.HasPasswordAsync(user).ConfigureAwait(false);
		if (RequirePassword)
		{
			if (!await _userManager.CheckPasswordAsync(user, Input.Password).ConfigureAwait(false))
			{
				ModelState.AddModelError(string.Empty, "Incorrect password.");
				return Page();
			}
		}

		IdentityResult result = await _userManager.DeleteAsync(user).ConfigureAwait(false);
		string userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
		if (!result.Succeeded)
		{
			throw new InvalidOperationException($"Unexpected error occurred deleting user.");
		}

		await _signInManager.SignOutAsync().ConfigureAwait(false);

		_logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

		return Redirect("~/");
	}
}
