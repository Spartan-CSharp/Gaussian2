// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaussianMVC.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Provides the page model for managing a user's external login providers within the ASP.NET Core Identity default UI.
/// This class enables viewing, adding, and removing external authentication methods linked to a user account.
/// </summary>
/// <remarks>This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
/// directly from your code. The API surface may change or be removed in future releases. The class is typically used by
/// the Identity UI to facilitate linking and unlinking external authentication providers, such as Google or Facebook,
/// to a user's account.</remarks>
/// <param name="userManager">The user manager used to retrieve and manage user information and logins.</param>
/// <param name="signInManager">The sign-in manager used to handle authentication and external login operations.</param>
/// <param name="userStore">The user store used to access and persist user data, including password information.</param>
public class ExternalLoginsModel(
	UserManager<IdentityUser> userManager,
	SignInManager<IdentityUser> signInManager,
	IUserStore<IdentityUser> userStore) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly SignInManager<IdentityUser> _signInManager = signInManager;
	private readonly IUserStore<IdentityUser> _userStore = userStore;

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public IList<UserLoginInfo> CurrentLogins { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public IList<AuthenticationScheme> OtherLogins { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public bool ShowRemoveButton { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[TempData]
	public string StatusMessage { get; set; }

	/// <summary>
	/// Handles GET requests for the page by loading the current user's external login information.
	/// </summary>
	/// <returns>A <see cref="IActionResult"/> that renders the page with the user's external logins.</returns>
	public async Task<IActionResult> OnGetAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		CurrentLogins = await _userManager.GetLoginsAsync(user).ConfigureAwait(false);
		OtherLogins = [.. (await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)).Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))];

		string passwordHash = null;
		if (_userStore is IUserPasswordStore<IdentityUser> userPasswordStore)
		{
			passwordHash = await userPasswordStore.GetPasswordHashAsync(user, HttpContext.RequestAborted).ConfigureAwait(false);
		}

		ShowRemoveButton = passwordHash != null || CurrentLogins.Count > 1;
		return Page();
	}

	/// <summary>
	/// Handles POST requests to remove an external login provider from the current user's account.
	/// After successful removal, the user's authentication session is refreshed.
	/// </summary>
	/// <param name="loginProvider">The name of the external login provider to remove (e.g., "Google", "Facebook").</param>
	/// <param name="providerKey">The unique identifier for the user in the external provider's system.</param>
	/// <returns>A <see cref="IActionResult"/> that redirects back to the page with a status message indicating success or failure.</returns>
	public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		IdentityResult result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey).ConfigureAwait(false);
		if (!result.Succeeded)
		{
			StatusMessage = "The external login was not removed.";
			return RedirectToPage();
		}

		await _signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
		StatusMessage = "The external login was removed.";
		return RedirectToPage();
	}

	/// <summary>
	/// Handles POST requests to initiate the process of linking a new external login provider to the current user's account.
	/// Clears any existing external authentication cookies and redirects to the specified provider's authentication page.
	/// </summary>
	/// <param name="provider">The name of the external login provider to link (e.g., "Google", "Facebook").</param>
	/// <returns>A <see cref="ChallengeResult"/> that redirects the user to the external provider's authentication page.</returns>
	public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
	{
		// Clear the existing external cookie to ensure a clean login process
		await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);

		// Request a redirect to the external login provider to link a login for the current user
		string redirectUrl = Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");
		AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
		return new ChallengeResult(provider, properties);
	}

	/// <summary>
	/// Handles the callback from an external login provider after the user has authenticated.
	/// Retrieves the external login information and attempts to link it to the current user's account.
	/// If successful, clears the external authentication cookie and redirects back to the page with a status message.
	/// </summary>
	/// <returns>A <see cref="IActionResult"/> that redirects back to the page with a status message indicating success or failure.</returns>
	/// <exception cref="InvalidOperationException">Thrown when external login information cannot be retrieved from the provider.</exception>
	public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		string userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
		ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync(userId).ConfigureAwait(false) ?? throw new InvalidOperationException($"Unexpected error occurred loading external login info.");

		IdentityResult result = await _userManager.AddLoginAsync(user, info).ConfigureAwait(false);
		if (!result.Succeeded)
		{
			StatusMessage = "The external login was not added. External logins can only be associated with one account.";
			return RedirectToPage();
		}

		// Clear the existing external cookie to ensure a clean login process
		await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);

		StatusMessage = "The external login was added.";
		return RedirectToPage();
	}
}
