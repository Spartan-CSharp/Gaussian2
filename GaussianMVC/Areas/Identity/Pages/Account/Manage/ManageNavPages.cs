// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Mvc.Rendering;

namespace GaussianMVC.Areas.Identity.Pages.Account.Manage;

/// <summary>
///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
///     directly from your code. This API may change or be removed in future releases.
/// </summary>
public static class ManageNavPages
{
	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string Index
	{
		get
		{
			return "Index";
		}
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string Email
	{
		get
		{
			return "Email";
		}
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string ChangePassword
	{
		get
		{
			return "ChangePassword";
		}
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string DownloadPersonalData
	{
		get
		{
			return "DownloadPersonalData";
		}
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string DeletePersonalData
	{
		get
		{
			return "DeletePersonalData";
		}
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string ExternalLogins
	{
		get
		{
			return "ExternalLogins";
		}
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string PersonalData
	{
		get
		{
			return "PersonalData";
		}
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string TwoFactorAuthentication
	{
		get
		{
			return "TwoFactorAuthentication";
		}
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string IndexNavClass(ViewContext viewContext)
	{
		return PageNavClass(viewContext, Index);
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string EmailNavClass(ViewContext viewContext)
	{
		return PageNavClass(viewContext, Email);
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string ChangePasswordNavClass(ViewContext viewContext)
	{
		return PageNavClass(viewContext, ChangePassword);
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string DownloadPersonalDataNavClass(ViewContext viewContext)
	{
		return PageNavClass(viewContext, DownloadPersonalData);
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string DeletePersonalDataNavClass(ViewContext viewContext)
	{
		return PageNavClass(viewContext, DeletePersonalData);
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string ExternalLoginsNavClass(ViewContext viewContext)
	{
		return PageNavClass(viewContext, ExternalLogins);
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string PersonalDataNavClass(ViewContext viewContext)
	{
		return PageNavClass(viewContext, PersonalData);
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string TwoFactorAuthenticationNavClass(ViewContext viewContext)
	{
		return PageNavClass(viewContext, TwoFactorAuthentication);
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public static string PageNavClass(ViewContext viewContext, string page)
	{
		if (viewContext is null)
		{
			throw new ArgumentNullException(nameof(viewContext), $"{nameof(viewContext)} cannot be null.");
		}

		string activePage = viewContext.ViewData["ActivePage"] as string
			?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
		return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
	}
}
