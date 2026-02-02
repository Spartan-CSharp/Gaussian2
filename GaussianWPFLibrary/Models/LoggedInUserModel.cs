using System.Diagnostics.Contracts;

namespace GaussianWPFLibrary.Models;

/// <summary>
/// Represents a logged-in user with authentication tokens, identity information, and profile data.
/// </summary>
public class LoggedInUserModel : ILoggedInUserModel
{
	/// <inheritdoc/>
	public string AccessToken { get; set; } = string.Empty;

	/// <inheritdoc/>
	public string UserId { get; set; } = string.Empty;

	/// <inheritdoc/>
	public string UserName { get; set; } = string.Empty;

	/// <inheritdoc/>
	public string Name { get; set; } = string.Empty;

	/// <inheritdoc/>
	public string GivenName { get; set; } = string.Empty;

	/// <inheritdoc/>
	public string MiddleName { get; set; } = string.Empty;

	/// <inheritdoc/>
	public string FamilyName { get; set; } = string.Empty;

	/// <inheritdoc/>
	public string Email { get; set; } = string.Empty;

	/// <inheritdoc/>
	public bool EmailConfirmed { get; set; }

	/// <inheritdoc/>
	public string PhoneNumber { get; set; } = string.Empty;

	/// <inheritdoc/>
	public bool PhoneNumberConfirmed { get; set; }

	/// <inheritdoc/>
	public bool TwoFactorEnabled { get; set; }

	/// <inheritdoc/>
	public DateTimeOffset? LockoutEnd { get; set; }

	/// <inheritdoc/>
	public bool LockoutEnabled { get; set; } = true;

	/// <inheritdoc/>
	public int AccessFailedCount { get; set; }

	/// <inheritdoc/>
	public string Gender { get; set; } = string.Empty;

	/// <inheritdoc/>
	public DateOnly? BirthDate { get; set; }

	/// <inheritdoc/>
	public string ZoneInfo { get; set; } = string.Empty;

	/// <inheritdoc/>
	public string Locale { get; set; } = string.Empty;

	/// <inheritdoc/>
	public UserAddressModel Address { get; set; } = new();

	/// <inheritdoc/>
	public IList<UserRoleModel> UserRoles { get; set; } = [];

	/// <inheritdoc/>
	public void ResetLoggedInUser()
	{
		AccessToken = string.Empty;
		UserId = string.Empty;
		UserName = string.Empty;
		Name = string.Empty;
		GivenName = string.Empty;
		MiddleName = string.Empty;
		FamilyName = string.Empty;
		Email = string.Empty;
		EmailConfirmed = false;
		PhoneNumber = string.Empty;
		PhoneNumberConfirmed = false;
		TwoFactorEnabled = false;
		LockoutEnd = null;
		LockoutEnabled = true;
		AccessFailedCount = 0;
		Gender = string.Empty;
		BirthDate = null;
		ZoneInfo = string.Empty;
		Locale = string.Empty;
		Address = new();
		UserRoles.Clear();
	}

	/// <summary>
	/// Returns a string representation of the user.
	/// </summary>
	/// <returns>A string in the format "UserName (UserId)".</returns>
	public override string? ToString()
	{
		return $"{UserName} ({UserId})";
	}
}
