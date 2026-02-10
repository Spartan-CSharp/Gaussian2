namespace GaussianCommonLibrary.ErrorModels;

/// <summary>
/// Exception thrown when attempting to delete an object that has related records.
/// </summary>
public class ValueInUseException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ValueInUseException"/> class.
	/// </summary>
	public ValueInUseException() : base("Cannot delete this object because it has related records.")
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ValueInUseException"/> class with the specified object name.
	/// </summary>
	/// <param name="objectName">The name of the object that cannot be deleted.</param>
	public ValueInUseException(string? objectName) : base($"Cannot delete object {objectName} because it has related records.")
	{
		ObjectName = objectName;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ValueInUseException"/> class with the specified object type.
	/// </summary>
	/// <param name="objectType">The type of the object that cannot be deleted.</param>
	public ValueInUseException(Type? objectType) : base($"Cannot delete object of Type {objectType?.Name} because it has related records.")
	{
		ObjectType = objectType;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ValueInUseException"/> class with the specified object name and custom message.
	/// </summary>
	/// <param name="objectName">The name of the object that cannot be deleted.</param>
	/// <param name="message">Custom error message, or null to use the default message.</param>
	public ValueInUseException(string? objectName, string? message) : base(message ?? $"Cannot delete object {objectName} because it has related records.")
	{
		ObjectName = objectName;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ValueInUseException"/> class with the specified object type and custom message.
	/// </summary>
	/// <param name="objectType">The type of the object that cannot be deleted.</param>
	/// <param name="message">Custom error message, or null to use the default message.</param>
	public ValueInUseException(Type? objectType, string? message) : base(message ?? $"Cannot delete object of Type {objectType?.Name} because it has related records.")
	{
		ObjectType = objectType;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ValueInUseException"/> class with the specified message and inner exception.
	/// </summary>
	/// <param name="message">Custom error message, or null to use the default message.</param>
	/// <param name="innerException">The exception that caused this exception.</param>
	public ValueInUseException(string? message, Exception? innerException) : base(message ?? "Cannot delete this object because it has related records.", innerException)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ValueInUseException"/> class with the specified object name, message, and inner exception.
	/// </summary>
	/// <param name="objectName">The name of the object that cannot be deleted.</param>
	/// <param name="message">Custom error message, or null to use the default message.</param>
	/// <param name="innerException">The exception that caused this exception.</param>
	public ValueInUseException(string? objectName, string? message, Exception? innerException) : base(message ?? $"Cannot delete object {objectName} because it has related records.", innerException)
	{
		ObjectName = objectName;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ValueInUseException"/> class with the specified object type, message, and inner exception.
	/// </summary>
	/// <param name="objectType">The type of the object that cannot be deleted.</param>
	/// <param name="message">Custom error message, or null to use the default message.</param>
	/// <param name="innerException">The exception that caused this exception.</param>
	public ValueInUseException(Type? objectType, string? message, Exception? innerException) : base(message ?? $"Cannot delete object of Type {objectType?.Name} because it has related records.", innerException)
	{
		ObjectType = objectType;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ValueInUseException"/> class with the specified object type, object name, message, and inner exception.
	/// </summary>
	/// <param name="objectType">The type of the object that cannot be deleted.</param>
	/// <param name="objectName">The name of the object that cannot be deleted.</param>
	/// <param name="message">Custom error message, or null to use the default message.</param>
	/// <param name="innerException">The exception that caused this exception.</param>
	public ValueInUseException(Type? objectType, string? objectName, string? message, Exception? innerException) : base(message ?? $"Cannot delete object {objectName} of Type {objectType?.Name} because it has related records.", innerException)
	{
		ObjectType = objectType;
		ObjectName = objectName;
	}

	/// <summary>
	/// Gets the name of the object that cannot be deleted.
	/// </summary>
	public string? ObjectName { get; private set; }

	/// <summary>
	/// Gets the type of the object that cannot be deleted.
	/// </summary>
	public Type? ObjectType { get; private set; }
}
