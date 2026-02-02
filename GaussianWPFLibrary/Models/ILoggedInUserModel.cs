namespace GaussianWPFLibrary.Models;

/// <summary>
/// Defines the contract for a logged-in user model containing authentication tokens, identity information, and user profile data.
/// </summary>
public interface ILoggedInUserModel
{
	/// <summary>
	/// Gets or sets the number of failed access attempts for the user account.
	/// </summary>
	int AccessFailedCount { get; set; }

	/// <summary>
	/// Gets or sets the authentication access token for API requests.
	/// </summary>
	string AccessToken { get; set; }

	/// <summary>
	/// Gets or sets the user's physical address information.
	/// </summary>
	UserAddressModel Address { get; set; }

	/// <summary>
	/// Gets or sets the user's date of birth.
	/// </summary>
	DateOnly? BirthDate { get; set; }

	/// <summary>
	/// Gets or sets the user's email address.
	/// </summary>
	string Email { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the user's email address has been confirmed.
	/// </summary>
	bool EmailConfirmed { get; set; }

	/// <summary>
	/// Gets or sets the user's family name (surname/last name).
	/// </summary>
	string FamilyName { get; set; }

	/// <summary>
	/// Gets or sets the user's gender.
	/// </summary>
	string Gender { get; set; }

	/// <summary>
	/// Gets or sets the user's given name (first name).
	/// </summary>
	string GivenName { get; set; }

	/// <summary>
	/// Gets or sets the user's locale preference for language and regional settings.
	/// </summary>
	string Locale { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether lockout is enabled for the user account.
	/// </summary>
	bool LockoutEnabled { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the user account lockout ends.
	/// </summary>
	DateTimeOffset? LockoutEnd { get; set; }

	/// <summary>
	/// Gets or sets the user's middle name.
	/// </summary>
	string MiddleName { get; set; }

	/// <summary>
	/// Gets or sets the user's full display name.
	/// </summary>
	string Name { get; set; }

	/// <summary>
	/// Gets or sets the user's phone number.
	/// </summary>
	string PhoneNumber { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the user's phone number has been confirmed.
	/// </summary>
	bool PhoneNumberConfirmed { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether two-factor authentication is enabled for the user account.
	/// </summary>
	bool TwoFactorEnabled { get; set; }

	/// <summary>
	/// Gets or sets the unique identifier for the user.
	/// </summary>
	string UserId { get; set; }

	/// <summary>
	/// Gets or sets the user's login username.
	/// </summary>
	string UserName { get; set; }

	/// <summary>
	/// Gets or sets the list of roles assigned to the user.
	/// </summary>
	IList<UserRoleModel> UserRoles { get; set; }

	/// <summary>
	/// Gets or sets the user's time zone information.
	/// </summary>
	string ZoneInfo { get; set; }

	/// <summary>
	/// Resets all user data to default values, effectively logging out the user.
	/// </summary>
	void ResetLoggedInUser();
}