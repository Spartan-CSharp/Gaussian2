namespace GaussianWPFLibrary.EventModels;

/// <summary>
/// Represents event arguments for child control events, containing information about an action performed and an optional item identifier.
/// </summary>
/// <typeparam name="T">The type of data associated with the child control event.</typeparam>
/// <param name="action">The action performed by the child control (e.g., "Create", "Edit", "Delete").</param>
/// <param name="itemId">The optional identifier of the item associated with the action.</param>
public class ChildControlEventArgs<T>(string? action, int? itemId) : EventArgs()
{
	/// <summary>
	/// Gets the action performed by the child control.
	/// </summary>
	public string? Action { get; private set; } = action;

	/// <summary>
	/// Gets the optional identifier of the item associated with the action.
	/// </summary>
	public int? ItemId { get; private set; } = itemId;
}
