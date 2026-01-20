// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Text.Json;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaussianMVC.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Represents the page model for downloading a user's personal data in JSON format as part of an ASP.NET Core Identity
/// area.
/// </summary>
/// <remarks>This page model is typically used in account management scenarios to allow authenticated users to
/// download a copy of their personal data, as required by privacy regulations such as GDPR. The model retrieves only
/// properties marked with the PersonalDataAttribute and includes external login provider keys and the authenticator
/// key, if available. The resulting data is returned as a downloadable JSON file.</remarks>
/// <param name="userManager">The user manager used to retrieve user information and personal data.</param>
/// <param name="logger">The logger used to record information about personal data download requests.</param>
public class DownloadPersonalDataModel(
	UserManager<IdentityUser> userManager,
	ILogger<DownloadPersonalDataModel> logger) : PageModel
{
	private readonly UserManager<IdentityUser> _userManager = userManager;
	private readonly ILogger<DownloadPersonalDataModel> _logger = logger;

	/// <summary>
	/// Handles HTTP GET requests for the page and returns a 404 Not Found result.
	/// </summary>
	/// <returns>A <see cref="NotFoundResult"/> indicating that the requested resource was not found.</returns>
	public IActionResult OnGet()
	{
		return NotFound();
	}

	/// <summary>
	/// Handles POST requests to generate and return the current user's personal data as a downloadable JSON file.
	/// </summary>
	/// <remarks>The returned JSON file includes all properties of the user marked with <see
	/// cref="PersonalDataAttribute"/>, as well as external login provider keys and the authenticator key if available. The
	/// response is sent with a 'Content-Disposition' header to prompt file download. This method is intended to help users
	/// export their personal data in compliance with data privacy requirements.</remarks>
	/// <returns>A <see cref="FileContentResult"/> containing the user's personal data in JSON format, or a <see
	/// cref="NotFoundResult"/> if the user cannot be found.</returns>
	public async Task<IActionResult> OnPostAsync()
	{
		IdentityUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
		}

		_logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

		// Only include personal data for download
		Dictionary<string, string> personalData = [];
		IEnumerable<System.Reflection.PropertyInfo> personalDataProps = typeof(IdentityUser).GetProperties().Where(
						prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
		foreach (System.Reflection.PropertyInfo p in personalDataProps)
		{
			personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
		}

		IList<UserLoginInfo> logins = await _userManager.GetLoginsAsync(user).ConfigureAwait(false);
		foreach (UserLoginInfo l in logins)
		{
			personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
		}

		personalData.Add($"Authenticator Key", await _userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false));

		_ = Response.Headers.TryAdd("Content-Disposition", "attachment; filename=PersonalData.json");
		return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
	}
}
