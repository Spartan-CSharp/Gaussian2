// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace GaussianMVC.Areas.Identity.Pages.Account;

/// <summary>
/// Represents the Razor Page Model for handling external login workflows in ASP.NET Core Identity, including
/// authentication via third-party providers and account confirmation.
/// </summary>
/// <remarks>This class is part of the ASP.NET Core Identity default UI infrastructure and is not intended to be
/// used directly in application code. It manages the process of signing in users with external providers, handling
/// callbacks, and confirming new accounts. The API surface and behavior may change in future releases. For
/// customization, override or replace the corresponding Razor Page in your application.</remarks>
[AllowAnonymous]
public class ExternalLoginModel : PageModel
{
	private readonly SignInManager<IdentityUser> _signInManager;
	private readonly UserManager<IdentityUser> _userManager;
	private readonly IUserStore<IdentityUser> _userStore;
	private readonly IUserEmailStore<IdentityUser> _emailStore;
	private readonly IEmailSender _emailSender;
	private readonly ILogger<ExternalLoginModel> _logger;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExternalLoginModel"/> class with the required services
	/// for managing external authentication workflows.
	/// </summary>
	/// <param name="signInManager">The sign-in manager used to handle external login sign-in operations.</param>
	/// <param name="userManager">The user manager for managing user accounts and identity operations.</param>
	/// <param name="userStore">The user store abstraction for persisting user data.</param>
	/// <param name="logger">The logger for recording external login events and diagnostics.</param>
	/// <param name="emailSender">The email sender service for sending account confirmation and notification emails.</param>
	public ExternalLoginModel(
		SignInManager<IdentityUser> signInManager,
		UserManager<IdentityUser> userManager,
		IUserStore<IdentityUser> userStore,
		ILogger<ExternalLoginModel> logger,
		IEmailSender emailSender)
	{
		_signInManager = signInManager;
		_userManager = userManager;
		_userStore = userStore;
		_emailStore = GetEmailStore();
		_logger = logger;
		_emailSender = emailSender;
	}

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
	public string ProviderDisplayName { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public string ReturnUrl { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[TempData]
	public string ErrorMessage { get; set; }

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
	/// Handles GET requests by redirecting the user to the login page.
	/// </summary>
	/// <returns>A <see cref="IActionResult"/> that redirects the user to the login page.</returns>
	public IActionResult OnGet()
	{
		return RedirectToPage("./Login");
	}

	/// <summary>
	/// Handles POST requests to initiate an external authentication workflow by redirecting the user to the specified
	/// external login provider (e.g., Google, Facebook, Microsoft).
	/// </summary>
	/// <param name="provider">The name of the external authentication provider to use for login.</param>
	/// <param name="returnUrl">The optional URL to redirect the user to after successful authentication. 
	/// Defaults to the application root if not specified.</param>
	/// <returns>A <see cref="ChallengeResult"/> that redirects the user to the external provider's authentication page.</returns>
	/// <remarks>
	/// This method configures the authentication properties including the callback URL, then issues a challenge
	/// to the specified external provider. The callback URL points to the <see cref="OnGetCallbackAsync"/> method
	/// which handles the response from the external provider.
	/// </remarks>
	public IActionResult OnPost(string provider, string returnUrl = null)
	{
		// Request a redirect to the external login provider.
		string redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
		AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
		return new ChallengeResult(provider, properties);
	}

	/// <summary>
	/// Handles the callback from an external authentication provider after the user has attempted to authenticate.
	/// This method processes the authentication result and either signs in an existing user or prompts for account creation.
	/// </summary>
	/// <param name="returnUrl">The optional URL to redirect the user to after successful sign-in. 
	/// Defaults to the application root if not specified.</param>
	/// <param name="remoteError">An optional error message from the external provider if authentication failed.</param>
	/// <returns>
	/// A <see cref="Task{IActionResult}"/> representing the asynchronous operation, containing one of the following results:
	/// <list type="bullet">
	/// <item><description>A redirect to the <paramref name="returnUrl"/> if the user successfully authenticated with an existing account.</description></item>
	/// <item><description>A redirect to the Lockout page if the user account is locked.</description></item>
	/// <item><description>The current page with pre-filled email if the user needs to create a new account.</description></item>
	/// <item><description>A redirect to the Login page if an error occurred or external login information could not be retrieved.</description></item>
	/// </list>
	/// </returns>
	/// <remarks>
	/// This method retrieves the external login information from the authentication context, then attempts to sign in
	/// the user if they have previously associated the external provider with their account. If no association exists,
	/// the user is prompted to confirm their email and create a new account.
	/// </remarks>
	public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
	{
		returnUrl ??= Url.Content("~/");
		if (remoteError != null)
		{
			ErrorMessage = $"Error from external provider: {remoteError}";
			return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
		}

		ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync().ConfigureAwait(false);
		if (info == null)
		{
			ErrorMessage = "Error loading external login information.";
			return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
		}

		// Sign in the user with this external login provider if the user already has a login.
		Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true).ConfigureAwait(false);
		if (result.Succeeded)
		{
			if (_logger.IsEnabled(LogLevel.Information))
			{
				_logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
			}

			return LocalRedirect(returnUrl);
		}

		if (result.IsLockedOut)
		{
			return RedirectToPage("./Lockout");
		}
		else
		{
			// If the user does not have an account, then ask the user to create an account.
			ReturnUrl = returnUrl;
			ProviderDisplayName = info.ProviderDisplayName;
			if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
			{
				Input = new InputModel
				{
					Email = info.Principal.FindFirstValue(ClaimTypes.Email)
				};
			}

			return Page();
		}
	}

	/// <summary>
	/// Handles POST requests to confirm and create a new user account associated with an external login provider.
	/// This method creates the user account, associates it with the external provider, sends a confirmation email,
	/// and optionally signs in the user.
	/// </summary>
	/// <param name="returnUrl">The optional URL to redirect the user to after successful account creation and sign-in.
	/// Defaults to the application root if not specified.</param>
	/// <returns>
	/// A <see cref="Task{IActionResult}"/> representing the asynchronous operation, containing one of the following results:
	/// <list type="bullet">
	/// <item><description>A redirect to the <paramref name="returnUrl"/> if the account was created and the user was signed in successfully.</description></item>
	/// <item><description>A redirect to the RegisterConfirmation page if email confirmation is required before sign-in.</description></item>
	/// <item><description>A redirect to the Login page if external login information could not be retrieved.</description></item>
	/// <item><description>The current page with validation errors if account creation failed.</description></item>
	/// </list>
	/// </returns>
	/// <remarks>
	/// This method performs the following operations:
	/// <list type="number">
	/// <item><description>Retrieves external login information from the authentication context.</description></item>
	/// <item><description>Creates a new <see cref="IdentityUser"/> with the provided email address.</description></item>
	/// <item><description>Associates the new user account with the external login provider.</description></item>
	/// <item><description>Sends an email confirmation link to the user's email address.</description></item>
	/// <item><description>Either redirects to the confirmation page or signs in the user based on configuration.</description></item>
	/// </list>
	/// If any operation fails, appropriate error messages are added to the model state.
	/// </remarks>
	public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
	{
		returnUrl ??= Url.Content("~/");
		// Get the information about the user from the external login provider
		ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync().ConfigureAwait(false);
		if (info == null)
		{
			ErrorMessage = "Error loading external login information during confirmation.";
			return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
		}

		if (ModelState.IsValid)
		{
			IdentityUser user = CreateUser();

			await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None).ConfigureAwait(false);
			await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None).ConfigureAwait(false);

			IdentityResult result = await _userManager.CreateAsync(user).ConfigureAwait(false);
			if (result.Succeeded)
			{
				result = await _userManager.AddLoginAsync(user, info).ConfigureAwait(false);
				if (result.Succeeded)
				{
					if (_logger.IsEnabled(LogLevel.Information))
					{
						_logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider); 
					}

					string userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
					string code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
					code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
					string callbackUrl = Url.Page(
						"/Account/ConfirmEmail",
						pageHandler: null,
						values: new { area = "Identity", userId, code },
						protocol: Request.Scheme);

					await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
						$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.").ConfigureAwait(false);

					// If account confirmation is required, we need to show the link if we don't have a real email sender
					if (_userManager.Options.SignIn.RequireConfirmedAccount)
					{
						return RedirectToPage("./RegisterConfirmation", new { Input.Email });
					}

					await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider).ConfigureAwait(false);
					return LocalRedirect(returnUrl);
				}
			}

			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}

		ProviderDisplayName = info.ProviderDisplayName;
		ReturnUrl = returnUrl;
		return Page();
	}

	private IdentityUser CreateUser()
	{
		try
		{
			return Activator.CreateInstance<IdentityUser>();
		}
		catch
		{
			throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
				$"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
				$"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
		}
	}

	private IUserEmailStore<IdentityUser> GetEmailStore()
	{
		return !_userManager.SupportsUserEmail
			? throw new NotSupportedException("The default UI requires a user store with email support.")
			: (IUserEmailStore<IdentityUser>)_userStore;
	}

	/// <summary>
	/// Handles POST requests to initiate authentication with the specified external provider.
	/// </summary>
	/// <param name="provider">The name of the external authentication provider to use. This value determines which provider's authentication flow
	/// will be initiated.</param>
	/// <param name="returnUrl">The URI to redirect to after authentication is complete. If null, a default location may be used.</param>
	/// <returns>An <see cref="IActionResult"/> that initiates the authentication challenge or redirects the user based on the
	/// authentication outcome.</returns>
	/// <exception cref="NotImplementedException">The method is not implemented.</exception>
	public IActionResult OnPost(string provider, Uri returnUrl = null)
	{
		throw new NotImplementedException();
	}
}
