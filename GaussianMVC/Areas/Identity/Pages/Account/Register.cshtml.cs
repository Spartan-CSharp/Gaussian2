// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace GaussianMVC.Areas.Identity.Pages.Account;

/// <summary>
/// Handles user registration for the application, including account creation, email confirmation, and sign-in.
/// This page model manages the registration process using ASP.NET Core Identity.
/// </summary>
public class RegisterModel : PageModel
{
	private readonly SignInManager<IdentityUser> _signInManager;
	private readonly UserManager<IdentityUser> _userManager;
	private readonly IUserStore<IdentityUser> _userStore;
	private readonly IUserEmailStore<IdentityUser> _emailStore;
	private readonly ILogger<RegisterModel> _logger;
	private readonly IEmailSender _emailSender;

	/// <summary>
	/// Initializes a new instance of the <see cref="RegisterModel"/> class with required dependencies
	/// for user registration, authentication, and email services.
	/// </summary>
	/// <param name="userManager">The user manager service for creating and managing user accounts.</param>
	/// <param name="userStore">The user store that provides persistence for user data.</param>
	/// <param name="signInManager">The sign-in manager service for handling user authentication and sign-in operations.</param>
	/// <param name="logger">The logger instance for recording registration events and errors.</param>
	/// <param name="emailSender">The email sender service for sending confirmation and notification emails.</param>
	public RegisterModel(
		UserManager<IdentityUser> userManager,
		IUserStore<IdentityUser> userStore,
		SignInManager<IdentityUser> signInManager,
		ILogger<RegisterModel> logger,
		IEmailSender emailSender)
	{
		_userManager = userManager;
		_userStore = userStore;
		_emailStore = GetEmailStore();
		_signInManager = signInManager;
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
	public string ReturnUrl { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public IList<AuthenticationScheme> ExternalLogins { get; set; }

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
		[Display(Name = "Email")]
		public string Email { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	/// <summary>
	/// Handles GET requests to the registration page. Initializes the page by setting the return URL
	/// and loading available external authentication providers (e.g., Google, Facebook, Microsoft).
	/// </summary>
	/// <param name="returnUrl">The URL to redirect to after successful registration. Defaults to null, which redirects to the home page.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public async Task OnGetAsync(string returnUrl = null)
	{
		ReturnUrl = returnUrl;
		ExternalLogins = [.. await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)];
	}

	/// <summary>
	/// Handles POST requests when the registration form is submitted. Validates user input, creates a new user account,
	/// generates an email confirmation token, and sends a confirmation email. If email confirmation is not required,
	/// the user is automatically signed in. Otherwise, redirects to the registration confirmation page.
	/// </summary>
	/// <param name="returnUrl">The URL to redirect to after successful registration. Defaults to the home page ("~/") if null.</param>
	/// <returns>
	/// A task that represents the asynchronous operation, containing an <see cref="IActionResult"/>:
	/// - <see cref="RedirectToPageResult"/> to "RegisterConfirmation" if email confirmation is required
	/// - <see cref="LocalRedirectResult"/> to the return URL if the user is automatically signed in
	/// - <see cref="PageResult"/> if model validation fails or user creation fails, redisplaying the form with errors
	/// </returns>
	public async Task<IActionResult> OnPostAsync(string returnUrl = null)
	{
		returnUrl ??= Url.Content("~/");
		ExternalLogins = [.. await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)];
		if (ModelState.IsValid)
		{
			IdentityUser user = CreateUser();

			await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None).ConfigureAwait(false);
			await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None).ConfigureAwait(false);
			IdentityResult result = await _userManager.CreateAsync(user, Input.Password).ConfigureAwait(false);

			if (result.Succeeded)
			{
				_logger.LogInformation("User created a new account with password.");

				string userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
				string code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
				string callbackUrl = Url.Page(
					"/Account/ConfirmEmail",
					pageHandler: null,
					values: new { area = "Identity", userId, code, returnUrl },
					protocol: Request.Scheme);

				await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
					$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.").ConfigureAwait(false);

				if (_userManager.Options.SignIn.RequireConfirmedAccount)
				{
					return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
				}
				else
				{
					await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);
					return LocalRedirect(returnUrl);
				}
			}

			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}

		// If we got this far, something failed, redisplay form
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
				$"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
		}
	}

	private IUserEmailStore<IdentityUser> GetEmailStore()
	{
		return !_userManager.SupportsUserEmail
			? throw new NotSupportedException("The default UI requires a user store with email support.")
			: (IUserEmailStore<IdentityUser>)_userStore;
	}
}
