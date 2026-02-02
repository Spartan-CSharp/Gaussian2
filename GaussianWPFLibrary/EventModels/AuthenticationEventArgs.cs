namespace GaussianWPFLibrary.EventModels;

/// <summary>
/// Provides data for authentication-related events.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AuthenticationEventArgs"/> class.
/// </remarks>
/// <param name="isAuthenticated">A value indicating whether the authentication was successful.</param>
public class AuthenticationEventArgs(bool isAuthenticated) : EventArgs()
{
	/// <summary>
	/// Gets a value indicating whether the authentication was successful.
	/// </summary>
	public bool IsAuthenticated { get; private set; } = isAuthenticated;
}
