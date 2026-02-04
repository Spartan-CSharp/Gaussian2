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

}
