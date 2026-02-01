namespace GaussianMVC;

/// <summary>
/// Provides application-wide constants for the Gaussian MVC application.
/// </summary>
public static class Constants
{
	/// <summary>
	/// Gets the toolbar items configuration for Syncfusion Rich Text Editor controls.
	/// Contains the buttons and separators to be displayed in the editor's toolbar.
	/// </summary>
	public static readonly string[] SyncfusionToolbarItems =
	[
		"Bold", "Italic", "Underline", "|",
		"SuperScript", "SubScript", "|",
		"FontName", "FontSize", "FontColor", "BackgroundColor", "|",
		"Blockquote", "InsertCode", "OrderedList", "UnorderedList", "Indent", "Outdent", "|",
		"SourceCode", "Preview", "Undo", "Redo", "Maximize", "Minimize"
	];
}