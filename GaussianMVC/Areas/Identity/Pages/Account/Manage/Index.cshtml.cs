// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaussianMVC.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Represents the page model for managing user profile information in the ASP.NET Core Identity default UI. This class
/// is used to display and update user account details such as the phone number.
/// </summary>
/// <remarks>This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
/// directly from your code. The API may change or be removed in future releases. The model is typically used by the
/// Identity UI to handle profile management scenarios, including displaying and updating user information.</remarks>
/// <param name="userManager">The user manager used to retrieve and update user account information.</param>
/// <param name="signInManager">The sign-in manager used to manage user authentication and refresh sign-in state.</param>
public class IndexModel(
	UserManager<IdentityUser> userManager,
	SignInManager<IdentityUser> signInManager) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly SignInManager<IdentityUser> _signInManager = signInManager;

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public string Username { get; set; }

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
		[Phone]
		[Display(Name = "Phone number")]
		public string PhoneNumber { get; set; }
	}

	/// <summary>
	/// Asynchronously loads the specified user's username and phone number into the current model.
	/// </summary>
	/// <remarks>This method updates the Username and Input properties with the user's current username and phone
	/// number. The method does not persist any changes to the user object.</remarks>
	/// <param name="user">The user whose information is to be loaded. Cannot be null.</param>
	/// <returns>A task that represents the asynchronous load operation.</returns>
	private async Task LoadAsync(IdentityUser user)
	{
		string userName = await _userManager.GetUserNameAsync(user).ConfigureAwait(false);
		string phoneNumber = await _userManager.GetPhoneNumberAsync(user).ConfigureAwait(false);

		Username = userName;

		Input = new InputModel
		{
			PhoneNumber = phoneNumber
		};
	}

	/// <summary>
	/// Handles GET requests for the page and initializes the user-specific data required for rendering.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IActionResult"/> that
	/// renders the page if the user is found; otherwise, a NotFound result.</returns>
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
	/// Handles POST requests for the page and updates the user's profile information.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IActionResult"/> that
	/// redirects to the page if the update is successful; otherwise, it re-renders the page with validation errors.</returns>
	public async Task<IActionResult> OnPostAsync()
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

		string phoneNumber = await _userManager.GetPhoneNumberAsync(user).ConfigureAwait(false);
		if (Input.PhoneNumber != phoneNumber)
		{
			IdentityResult setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber).ConfigureAwait(false);
			if (!setPhoneResult.Succeeded)
			{
				StatusMessage = "Unexpected error when trying to set phone number.";
				return RedirectToPage();
			}
		}

		await _signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
		StatusMessage = "Your profile has been updated";
		return RedirectToPage();
	}
}
