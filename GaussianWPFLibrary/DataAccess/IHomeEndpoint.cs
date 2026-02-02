namespace GaussianWPFLibrary.DataAccess;

/// <summary>
/// Defines the contract for API operations related to home and informational pages.
/// </summary>
public interface IHomeEndpoint
{
	/// <summary>
	/// Retrieves the content of the home page.
	/// </summary>
	/// <returns>The home page content, or <see langword="null"/> if not available.</returns>
	Task<string?> GetHomeAsync();
	
	/// <summary>
	/// Retrieves the content of the privacy page.
	/// </summary>
	/// <returns>The privacy page content, or <see langword="null"/> if not available.</returns>
	Task<string?> GetPrivacyAsync();

	/// <summary>
	/// Retrieves the content of the about page.
	/// </summary>
	/// <returns>The about page content, or <see langword="null"/> if not available.</returns>
	Task<string?> GetAboutAsync();
	
	/// <summary>
	/// Retrieves the content of the contact page.
	/// </summary>
	/// <returns>The contact page content, or <see langword="null"/> if not available.</returns>
	Task<string?> GetContactAsync();
	
	/// <summary>
	/// Retrieves the content of the error page.
	/// </summary>
	/// <returns>The error page content, or <see langword="null"/> if not available.</returns>
	Task<string?> GetErrorAsync();
}
