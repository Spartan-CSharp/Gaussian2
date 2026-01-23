using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace GaussianWPF.WPFHelpers;

internal static class RTBHelpers
{
	internal static string GetPlainText(this RichTextBox rtb)
	{
		TextRange range = new(rtb.Document.ContentStart, rtb.Document.ContentEnd);
		string output = range.Text;
		return output;
	}

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
