namespace GaussianCommonLibrary.ErrorModels;

/// <summary>
/// Represents an exception that is thrown when a required parameter is null.
/// </summary>
public class NullParameterException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="NullParameterException"/> class.
	/// </summary>
	public NullParameterException() : base()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="NullParameterException"/> class with the specified parameter name.
	/// </summary>
	/// <param name="paramName">The name of the parameter that is null.</param>
	public NullParameterException(string? paramName) : base()
	{
		ParamName = paramName;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="NullParameterException"/> class with the specified parameter name and error message.
	/// </summary>
	/// <param name="paramName">The name of the parameter that is null.</param>
	/// <param name="message">The message that describes the error.</param>
	public NullParameterException(string? paramName, string? message) : base(message)
	{
		ParamName = paramName;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="NullParameterException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
	public NullParameterException(string? message, Exception? innerException) : base(message, innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="NullParameterException"/> class with the specified parameter name, error message, and a reference to the inner exception that is the cause of this exception.
	/// </summary>
	/// <param name="paramName">The name of the parameter that is null.</param>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
	public NullParameterException(string? paramName, string? message, Exception? innerException) : base(message, innerException)
	{
		ParamName = paramName;
	}

	/// <summary>
	/// Gets the name of the parameter that caused the exception.
	/// </summary>
	/// <value>
	/// The name of the parameter that is null, or <see langword="null"/> if not specified.
	/// </value>
	public string? ParamName { get; private set; }
}
