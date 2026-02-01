using System.ComponentModel.DataAnnotations;

namespace GaussianMVC.Models;

/// <summary>
/// View model representing error information for the Gaussian MVC application.
/// Provides diagnostic information for error tracking and display purposes.
/// </summary>
public class ErrorViewModel
{
	/// <summary>
	/// Gets or sets the request identifier that generated the error.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Request Id")]
	public string? RequestId { get; set; }

	/// <summary>
	/// Gets a value indicating whether the request identifier should be displayed.
	/// </summary>
	[Display(Name = "Show Request Id")]
	public bool ShowRequestId
	{
		get
		{
			return !string.IsNullOrEmpty(RequestId);
		}
	}

	/// <summary>
	/// Gets or sets the HTTP status code associated with the error.
	/// </summary>
	[Display(Name = "Status Code")]
	public int? StatusCode { get; set; }

	/// <summary>
	/// Gets or sets the HTTP status phrase describing the error.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Status")]
	public string? StatusPhrase { get; set; }

	/// <summary>
	/// Gets or sets the type of the exception that occurred.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Exception Type")]
	public string? ExceptionType { get; set; }

	/// <summary>
	/// Gets or sets the message from the exception that occurred.
	/// </summary>
	[DataType(DataType.Text)]
	[Display(Name = "Exception Message")]
	public string? ExceptionMessage { get; set; }

	/// <summary>
	/// Returns a string representation of the error view model containing all non-empty error details.
	/// </summary>
	/// <returns>A formatted string containing the error details, or an empty string if all properties are null or empty.</returns>
	public override string? ToString()
	{
		string? output = null;
		string? evmString = string.Empty;

		if (!string.IsNullOrWhiteSpace(RequestId))
		{
			evmString += $"RequestId: {RequestId}; ";
		}

		if (StatusCode is not null)
		{
			evmString += $"StatusCode: {StatusCode}; ";
		}

		if (!string.IsNullOrWhiteSpace(StatusPhrase))
		{
			evmString += $"StatusPhrase: {StatusPhrase}; ";
		}

		if (!string.IsNullOrWhiteSpace(ExceptionType))
		{
			evmString += $"ExceptionType: {ExceptionType}; ";
		}

		if (!string.IsNullOrWhiteSpace(ExceptionMessage))
		{
			evmString += $"ExceptionMessage: {ExceptionMessage}; ";
		}

		if (!string.IsNullOrWhiteSpace(evmString) && evmString.Length > 2)
		{
			output = evmString[..^2];
		}

		return output;
	}
}
