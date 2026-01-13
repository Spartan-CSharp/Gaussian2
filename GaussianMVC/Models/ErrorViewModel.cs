namespace GaussianMVC.Models;

/// <summary>
/// The ErrorViewModel
/// </summary>
public class ErrorViewModel
{
	/// <summary>
	/// The RequestId that generated the error
	/// </summary>
	public string? RequestId { get; set; }

	/// <summary>
	/// Whether to show the request id
	/// </summary>
	public bool ShowRequestId
	{
		get
		{
			return !string.IsNullOrEmpty(RequestId);
		}
	}
}
