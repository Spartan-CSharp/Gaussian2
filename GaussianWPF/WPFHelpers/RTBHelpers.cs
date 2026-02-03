using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace GaussianWPF.WPFHelpers;

/// <summary>
/// Provides extension methods for working with <see cref="RichTextBox"/> controls, including text extraction, formatting, and styling operations.
/// </summary>
internal static class RTBHelpers
{
	/// <summary>
	/// Extracts the plain text content from a <see cref="RichTextBox"/> control, removing all formatting.
	/// </summary>
	/// <param name="rtb">The <see cref="RichTextBox"/> to extract text from.</param>
	/// <returns>The plain text content of the rich text box.</returns>
	internal static string GetPlainText(this RichTextBox rtb)
	{
		TextRange range = new(rtb.Document.ContentStart, rtb.Document.ContentEnd);
		string output = range.Text;
		return output;
	}

	/// <summary>
	/// Extracts the RTF-formatted content from a <see cref="RichTextBox"/> control as a string.
	/// </summary>
	/// <param name="rtb">The <see cref="RichTextBox"/> to extract RTF content from.</param>
	/// <returns>The RTF-formatted string representing the content.</returns>
	internal static string GetRtfText(this RichTextBox rtb)
	{
		TextRange range = new(rtb.Document.ContentStart, rtb.Document.ContentEnd);
		using MemoryStream stream = new();
		range.Save(stream, DataFormats.Rtf);
		stream.Position = 0;
		using StreamReader reader = new(stream);
		string output = reader.ReadToEnd();
		return output;
	}

	/// <summary>
	/// Sets the content of a <see cref="RichTextBox"/> control from an RTF-formatted string.
	/// </summary>
	/// <param name="rtb">The <see cref="RichTextBox"/> to set content for.</param>
	/// <param name="rtfText">The RTF-formatted string to load. If <see langword="null"/> or empty, the document is cleared.</param>
	internal static void SetRtfText(this RichTextBox rtb, string? rtfText)
	{
		if (string.IsNullOrEmpty(rtfText))
		{
			rtb.Document.Blocks.Clear();
		}
		else
		{
			TextRange range = new(rtb.Document.ContentStart, rtb.Document.ContentEnd);
			using MemoryStream stream = new(Encoding.UTF8.GetBytes(rtfText));
			range.Load(stream, DataFormats.Rtf);
		}
	}

	/// <summary>
	/// Toggles the font weight of the current selection between bold and normal.
	/// </summary>
	/// <param name="rtb">The <see cref="RichTextBox"/> containing the selection.</param>
	/// <remarks>
	/// If the selection is currently bold, it becomes normal. If normal or mixed, it becomes bold.
	/// No action is taken if the selection is empty.
	/// </remarks>
	internal static void ToggleFontWeight(this RichTextBox rtb)
	{
		TextSelection selection = rtb.Selection;
		if (!selection.IsEmpty)
		{
			object currentValue = selection.GetPropertyValue(TextElement.FontWeightProperty);
			FontWeight newWeight = (currentValue != DependencyProperty.UnsetValue &&
				(FontWeight)currentValue == FontWeights.Bold) ? FontWeights.Normal : FontWeights.Bold;

			selection.ApplyPropertyValue(TextElement.FontWeightProperty, newWeight);
		}
	}

	/// <summary>
	/// Toggles the font style of the current selection between italic and normal.
	/// </summary>
	/// <param name="rtb">The <see cref="RichTextBox"/> containing the selection.</param>
	/// <remarks>
	/// If the selection is currently italic, it becomes normal. If normal or mixed, it becomes italic.
	/// No action is taken if the selection is empty.
	/// </remarks>
	internal static void ToggleFontStyle(this RichTextBox rtb)
	{
		TextSelection selection = rtb.Selection;
		if (!selection.IsEmpty)
		{
			object currentValue = selection.GetPropertyValue(TextElement.FontStyleProperty);
			FontStyle newStyle = (currentValue != DependencyProperty.UnsetValue &&
				(FontStyle)currentValue == FontStyles.Italic) ? FontStyles.Normal : FontStyles.Italic;

			selection.ApplyPropertyValue(TextElement.FontStyleProperty, newStyle);
		}
	}

	/// <summary>
	/// Toggles the underline decoration of the current selection.
	/// </summary>
	/// <param name="rtb">The <see cref="RichTextBox"/> containing the selection.</param>
	/// <remarks>
	/// If the selection is currently underlined, the underline is removed. If not underlined or mixed, underline is applied.
	/// No action is taken if the selection is empty.
	/// </remarks>
	internal static void ToggleUnderline(this RichTextBox rtb)
	{
		TextSelection selection = rtb.Selection;
		if (!selection.IsEmpty)
		{
			object currentValue = selection.GetPropertyValue(Inline.TextDecorationsProperty);
			TextDecorationCollection? newDecoration = (currentValue != DependencyProperty.UnsetValue &&
				currentValue is TextDecorationCollection tdc && tdc == TextDecorations.Underline)
				? null
				: TextDecorations.Underline;

			selection.ApplyPropertyValue(Inline.TextDecorationsProperty, newDecoration);
		}
	}

	/// <summary>
	/// Toggles the baseline alignment of the current selection for creating superscript or subscript text.
	/// </summary>
	/// <param name="rtb">The <see cref="RichTextBox"/> containing the selection.</param>
	/// <param name="alignment">The baseline alignment to toggle (typically <see cref="BaselineAlignment.Superscript"/> or <see cref="BaselineAlignment.Subscript"/>).</param>
	/// <remarks>
	/// <para>If the selection already has the specified alignment, it is reset to <see cref="BaselineAlignment.Baseline"/>.</para>
	/// <para>When toggling on, the font size is reduced by 36%. When toggling off, the font size is increased by 56.25% to restore the original size.</para>
	/// <para>No action is taken if the selection is empty.</para>
	/// </remarks>
	internal static void ToggleBaselineAlignment(this RichTextBox rtb, BaselineAlignment alignment)
	{
		TextSelection selection = rtb.Selection;
		if (!selection.IsEmpty)
		{
			object currentValue = selection.GetPropertyValue(Inline.BaselineAlignmentProperty);
			BaselineAlignment newAlignment = (currentValue != DependencyProperty.UnsetValue &&
				(BaselineAlignment)currentValue == alignment)
				? BaselineAlignment.Baseline
				: alignment;

			selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, newAlignment);

			// Adjust font size based on baseline alignment
			object currentFontSizeValue = selection.GetPropertyValue(TextElement.FontSizeProperty);

			if (currentFontSizeValue != DependencyProperty.UnsetValue && currentFontSizeValue is double currentFontSize)
			{
				double newFontSize;

				if (newAlignment == BaselineAlignment.Baseline)
				{
					// Toggling off: restore font size (increase by 56.25% to undo the 36% reduction)
					newFontSize = currentFontSize * 1.5625;
				}
				else
				{
					// Toggling on: reduce font size by 36%
					newFontSize = currentFontSize * 0.64;
				}

				selection.ApplyPropertyValue(TextElement.FontSizeProperty, newFontSize);
			}
		}
	}

	/// <summary>
	/// Toggles list formatting for the paragraphs in the current selection.
	/// </summary>
	/// <param name="rtb">The <see cref="RichTextBox"/> containing the selection.</param>
	/// <param name="markerStyle">The marker style to use for the list (e.g., bullets, numbers).</param>
	/// <remarks>
	/// <para>If the paragraphs are already in a list, they are removed from the list and converted back to regular paragraphs.</para>
	/// <para>If the paragraphs are not in a list, a new list is created with the specified marker style.</para>
	/// <para>All paragraphs between the start and end of the selection are included in the operation.</para>
	/// <para>No action is taken if the selection does not contain valid paragraphs.</para>
	/// </remarks>
	internal static void ToggleList(this RichTextBox rtb, TextMarkerStyle markerStyle)
	{
		TextSelection selection = rtb.Selection;

		Paragraph? startParagraph = selection.Start.Paragraph;
		Paragraph? endParagraph = selection.End.Paragraph;

		if (startParagraph == null || endParagraph == null)
		{
			return;
		}

		// Check if the paragraph is already in a list
		if (startParagraph.Parent is ListItem listItem && listItem.Parent is List list)
		{
			// Remove from list
			FlowDocument? doc = rtb.Document;
			List<Paragraph> paragraphs = [];

			foreach (ListItem item in list.ListItems)
			{
				foreach (Block block in item.Blocks.ToList())
				{
					if (block is Paragraph para)
					{
						paragraphs.Add(para);
					}
				}
			}

			int listIndex = doc.Blocks.ToList().IndexOf(list);
			_ = doc.Blocks.Remove(list);

			foreach (Paragraph para in paragraphs)
			{
				doc.Blocks.InsertBefore(doc.Blocks.ElementAt(Math.Min(listIndex, doc.Blocks.Count - 1)), para);
			}
		}
		else
		{
			// Add to list
			List newList = new() { MarkerStyle = markerStyle };

			FlowDocument? doc = rtb.Document;
			List<Paragraph> paragraphsToAdd = [];

			// Collect all paragraphs in selection
			Block? currentBlock = startParagraph;
			while (currentBlock != null)
			{
				if (currentBlock is Paragraph para)
				{
					paragraphsToAdd.Add(para);
				}

				if (currentBlock == endParagraph)
				{
					break;
				}

				currentBlock = currentBlock.NextBlock;
			}

			// Get insertion index
			int insertIndex = doc.Blocks.ToList().IndexOf(startParagraph);

			// Remove paragraphs from document
			foreach (Paragraph para in paragraphsToAdd)
			{
				_ = doc.Blocks.Remove(para);
			}

			// Add paragraphs to list
			foreach (Paragraph para in paragraphsToAdd)
			{
				ListItem item = new();
				item.Blocks.Add(para);
				newList.ListItems.Add(item);
			}

			// Insert list at original position
			if (doc.Blocks.Count == 0)
			{
				doc.Blocks.Add(newList);
			}
			else
			{
				doc.Blocks.InsertBefore(doc.Blocks.ElementAt(Math.Min(insertIndex, doc.Blocks.Count - 1)), newList);
			}
		}
	}
}
