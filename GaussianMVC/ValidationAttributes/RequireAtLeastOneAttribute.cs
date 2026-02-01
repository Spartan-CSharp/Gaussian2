using System.ComponentModel.DataAnnotations;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GaussianMVC.ValidationAttributes;

/// <summary>
/// Validates that at least one of the specified properties contains a non-null, non-empty, non-whitespace value.
/// Apply this attribute to one of the properties being validated (typically the first one).
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class RequireAtLeastOneAttribute : ValidationAttribute, IClientModelValidator
{
	/// <summary>
	/// Gets the names of the other properties to validate along with the property this attribute is applied to.
	/// </summary>
	public string[] OtherPropertyNames { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="RequireAtLeastOneAttribute"/> class.
	/// </summary>
	/// <param name="otherPropertyNames">The names of the other properties to validate in addition to the property this attribute is applied to.</param>
	/// <exception cref="ArgumentException">Thrown when no other property names are provided.</exception>
	public RequireAtLeastOneAttribute(params string[] otherPropertyNames)
	{
		if (otherPropertyNames == null || otherPropertyNames.Length < 1)
		{
			throw new ArgumentException("At least one other property name must be specified.", nameof(otherPropertyNames));
		}

		OtherPropertyNames = otherPropertyNames;
	}

	/// <inheritdoc/>
	/// <summary>
	/// Validates that at least one of the specified properties has a non-null, non-empty, non-whitespace value.
	/// </summary>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="validationContext"/> is null.</exception>
	/// <exception cref="ArgumentException">Thrown when a specified property name is not found on the validated type.</exception>
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		ArgumentNullException.ThrowIfNull(validationContext);

		// Check if the current property has a value
		if (HasValue(value))
		{
			return ValidationResult.Success;
		}

		// Check other properties
		Type objectType = validationContext.ObjectType;
		object? instance = validationContext.ObjectInstance;

		foreach (string propertyName in OtherPropertyNames)
		{
			PropertyInfo? property = objectType.GetProperty(propertyName) ?? throw new ArgumentException($"Property '{propertyName}' not found on type '{objectType.Name}'.");

			object? propertyValue = property.GetValue(instance);

			if (HasValue(propertyValue))
			{
				return ValidationResult.Success;
			}
		}

		// Build the error message if not already set
		string? errorMessage = ErrorMessage;
		if (string.IsNullOrEmpty(errorMessage))
		{
			string currentPropertyName = validationContext.MemberName ?? "this field";
			string allProperties = $"{currentPropertyName}, {string.Join(", ", OtherPropertyNames)}";
			errorMessage = $"At least one of the following fields must be provided: {allProperties}.";
		}

		// Return all property names in the validation result
		List<string> memberNames = [validationContext.MemberName ?? string.Empty];
		memberNames.AddRange(OtherPropertyNames);

		return new ValidationResult(errorMessage, memberNames);
	}

	private static bool HasValue(object? value)
	{
		return value != null && (value is not string stringValue || !string.IsNullOrWhiteSpace(stringValue));
	}

	/// <inheritdoc/>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is null.</exception>
	public void AddValidation(ClientModelValidationContext context)
	{
		ArgumentNullException.ThrowIfNull(context);

		string? errorMessage = ErrorMessage;
		if (string.IsNullOrEmpty(errorMessage))
		{
			string currentPropertyName = context.ModelMetadata.PropertyName ?? "this field";
			string allProperties = $"{currentPropertyName}, {string.Join(", ", OtherPropertyNames)}";
			errorMessage = $"At least one of the following fields must be provided: {allProperties}.";
		}

		MergeAttribute(context.Attributes, "data-val", "true");
		MergeAttribute(context.Attributes, "data-val-requireatleastone", errorMessage);
		MergeAttribute(context.Attributes, "data-val-requireatleastone-otherproperties", string.Join(",", OtherPropertyNames));
	}

	private static void MergeAttribute(IDictionary<string, string> attributes, string key, string value)
	{
		if (!attributes.ContainsKey(key))
		{
			attributes.Add(key, value);
		}
	}
}