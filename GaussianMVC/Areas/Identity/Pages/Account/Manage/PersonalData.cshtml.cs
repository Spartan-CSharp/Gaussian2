// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaussianMVC.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Represents the page model for displaying and managing a user's personal data in an ASP.NET Core Identity
/// application.
/// </summary>
/// <remarks>This page model is typically used in account management scenarios to allow users to view or update
/// their personal data. It relies on ASP.NET Core Identity for user management operations.</remarks>
/// <param name="userManager">The user manager used to retrieve and manage user information.</param>
public class PersonalDataModel(
	UserManager<IdentityUser> userManager) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;

	/// <summary>
	/// Handles HTTP GET requests to the Personal Data page. Retrieves the current user's identity and 
	/// verifies that the user exists before displaying their personal data management options.
	/// </summary>
	/// <returns>
	/// A <see cref="Task{IActionResult}"/> representing the asynchronous operation. Returns:
	/// <list type="bullet">
	/// <item><description>A <see cref="NotFoundObjectResult"/> if the user cannot be loaded from the database.</description></item>
	/// <item><description>A <see cref="PageResult"/> if the user is successfully loaded, allowing the page to render.</description></item>
	/// </list>
	/// </returns>
	public async Task<IActionResult> OnGetAsync()
	{
		IdentityUser? user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		return user == null ? NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.") : Page();
	}
}
