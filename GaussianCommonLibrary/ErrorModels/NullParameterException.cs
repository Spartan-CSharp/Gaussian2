namespace GaussianCommonLibrary.ErrorModels;

/// <summary>
/// Exception thrown when a required parameter is null.
/// </summary>
public class NullParameterException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="NullParameterException"/> class.
	/// </summary>
	public NullParameterException() : base("A required parameter is null.")
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="NullParameterException"/> class with the specified parameter name.
	/// </summary>
	/// <param name="paramName">The name of the null parameter.</param>
	public NullParameterException(string? paramName) : base("A required parameter is null.")
	{
		ParamName = paramName;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="NullParameterException"/> class with the specified parameter name and error message.
	/// </summary>
	/// <param name="paramName">The name of the null parameter.</param>
	/// <param name="message">Custom error message, or null to use the default message.</param>
	public NullParameterException(string? paramName, string? message) : base(message ?? "A required parameter is null.")
	{
		ParamName = paramName;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="NullParameterException"/> class with the specified message and inner exception.
	/// </summary>
	/// <param name="message">Custom error message, or null to use the default message.</param>
	/// <param name="innerException">The exception that caused this exception.</param>
	public NullParameterException(string? message, Exception? innerException) : base(message ?? "A required parameter is null.", innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="NullParameterException"/> class with the specified parameter name, message, and inner exception.
	/// </summary>
	/// <param name="paramName">The name of the null parameter.</param>
	/// <param name="message">Custom error message, or null to use the default message.</param>
	/// <param name="innerException">The exception that caused this exception.</param>
	public NullParameterException(string? paramName, string? message, Exception? innerException) : base(message ?? "A required parameter is null.", innerException)
	{
		ParamName = paramName;
	}

	/// <summary>
	/// Gets the name of the parameter that was null.
	/// </summary>
	public string? ParamName { get; private set; }
}
