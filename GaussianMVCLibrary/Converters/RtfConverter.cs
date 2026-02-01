using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using RtfPipe;

namespace GaussianMVCLibrary.Converters;

/// <summary>
/// Provides static methods for converting text between RTF (Rich Text Format), HTML, and plain text formats.
/// </summary>
public static class RtfConverter
{
	private static readonly string[] _separator = ["\r\n", "\r", "\n"];

	/// <summary>
	/// Converts RTF (Rich Text Format) text to HTML format.
	/// </summary>
	/// <param name="rtfText">The RTF text to convert. Can be null or whitespace.</param>
	/// <returns>
	/// The HTML representation of the RTF text, or an empty string if the input is null or whitespace.
	/// Returns an error message string if the conversion fails.
	/// </returns>
	/// <remarks>
	/// This method uses the RtfPipe library to perform the conversion and extracts only the body content
	/// from the generated HTML document, removing any wrapper elements.
	/// </remarks>
	public static string RtfToHtml(string? rtfText)
	{
		if (string.IsNullOrWhiteSpace(rtfText))
		{
			return string.Empty;
		}

		try
		{
			// Use RtfPipe to convert RTF to HTML
			string html = Rtf.ToHtml(rtfText);

			// RtfPipe returns a full HTML document, extract just the body content
			Match bodyMatch = Regex.Match(html, @"<body[^>]*>(.*?)</body>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
			if (bodyMatch.Success)
			{
				html = bodyMatch.Groups[1].Value.Trim();
			}

			// Clean up any extra div wrappers that RtfPipe might add
			html = html.Trim();

			return html;
		}
		catch (Exception ex)
		{
			// Log the exception if needed
			Debug.WriteLine($"RTF to HTML conversion error: {ex.Message}");
			return $"{ex.GetType().Name}: {ex.Message}";
		}
	}

	/// <summary>
	/// Converts HTML text to RTF (Rich Text Format) format.
	/// </summary>
	/// <param name="htmlText">The HTML text to convert. Can be null or whitespace.</param>
	/// <returns>
	/// The RTF representation of the HTML text, or an empty string if the input is null or whitespace.
	/// Returns an error message string if the conversion fails.
	/// </returns>
	/// <remarks>
	/// This method supports a wide range of HTML elements including formatting (bold, italic, underline),
	/// lists (ordered and unordered), code blocks, inline styles (colors, fonts, sizes), superscript,
	/// subscript, and common HTML entities. The conversion includes proper RTF font and color table generation.
	/// </remarks>
	public static string HtmlToRtf(string? htmlText)
	{
		if (string.IsNullOrWhiteSpace(htmlText))
		{
			return string.Empty;
		}

		try
		{
			string text = htmlText;

			// STEP 1: Extract font settings from outer div if present
			string defaultFontFamily = "Segoe UI";
			int defaultFontSize = 12; // Default 12pt

			Match divMatch = Regex.Match(text, @"<div[^>]*style=[""']([^""']*)[""'][^>]*>", RegexOptions.IgnoreCase);
			if (divMatch.Success)
			{
				string style = divMatch.Groups[1].Value;

				// Extract font-family
				Match fontFamilyMatch = Regex.Match(style, @"font-family:\s*([^;]+)", RegexOptions.IgnoreCase);
				if (fontFamilyMatch.Success)
				{
					defaultFontFamily = fontFamilyMatch.Groups[1].Value.Trim();
				}

				// Extract font-size
				Match fontSizeMatch = Regex.Match(style, @"font-size:\s*(\d+)pt", RegexOptions.IgnoreCase);
				if (fontSizeMatch.Success)
				{
					defaultFontSize = int.Parse(fontSizeMatch.Groups[1].Value, CultureInfo.InvariantCulture);
				}
			}

			// STEP 2: Build font table by finding all unique fonts
			List<string> fonts = [defaultFontFamily];
			MatchCollection fontMatches = Regex.Matches(text, @"font-family:\s*([^;""']+)", RegexOptions.IgnoreCase);
			foreach (Match match in fontMatches)
			{
				string font = match.Groups[1].Value.Trim().Trim('\'', '"');
				if (!fonts.Contains(font, StringComparer.OrdinalIgnoreCase))
				{
					fonts.Add(font);
				}
			}
			// Add Courier New for code blocks if not already present
			if (!fonts.Contains("Courier New", StringComparer.OrdinalIgnoreCase))
			{
				fonts.Add("Courier New");
			}

			// STEP 3: Build color table by finding all unique colors
			List<(int R, int G, int B)> colors = [(0, 0, 0)]; // Default black

			// Find RGB colors
			MatchCollection rgbMatches = Regex.Matches(text, @"(?:color|background-color):\s*rgb\((\d+),\s*(\d+),\s*(\d+)\)", RegexOptions.IgnoreCase);
			foreach (Match match in rgbMatches)
			{
				int r = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
				int g = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
				int b = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
				if (!colors.Contains((r, g, b)))
				{
					colors.Add((r, g, b));
				}
			}

			// Find hex colors
			MatchCollection hexMatches = Regex.Matches(text, @"(?:color|background-color):\s*#([0-9A-Fa-f]{6})", RegexOptions.IgnoreCase);
			foreach (Match match in hexMatches)
			{
				string hex = match.Groups[1].Value;
				int r = Convert.ToInt32(hex[..2], 16);
				int g = Convert.ToInt32(hex.Substring(2, 2), 16);
				int b = Convert.ToInt32(hex.Substring(4, 2), 16);
				if (!colors.Contains((r, g, b)))
				{
					colors.Add((r, g, b));
				}
			}

			// STEP 4: Build RTF header
			StringBuilder rtf = new();
			int defaultFontSizeHalfPoints = defaultFontSize * 2;

			_ = rtf.Append(@"{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang1033");

			// Font table
			_ = rtf.Append(@"{\fonttbl");
			for (int i = 0; i < fonts.Count; i++)
			{
				_ = rtf.Append(CultureInfo.InvariantCulture, $@"{{\f{i}\fnil\fcharset0 {fonts[i]};}}");
			}

			_ = rtf.Append('}');

			// Color table
			_ = rtf.Append(@"{\colortbl;");
			foreach ((int R, int G, int B) color in colors)
			{
				_ = rtf.Append(CultureInfo.InvariantCulture, $@"\red{color.R}\green{color.G}\blue{color.B};");
			}

			_ = rtf.Append('}');

			// List tables
			_ = rtf.Append(@"{\*\listtable");
			_ = rtf.Append(@"{\list\listtemplateid1\listhybrid");
			_ = rtf.Append(@"{\listlevel\levelnfc23\levelnfcn23\leveljc0\leveljcn0\levelfollow0\levelstartat1\levelspace0\levelindent0{\leveltext\leveltemplateid1\'01\'b7;}{\levelnumbers;}\fi-360\li720\jclisttab\tx720}");
			_ = rtf.Append(@"{\listname ;}\listid1}");
			_ = rtf.Append(@"{\list\listtemplateid2\listhybrid");
			_ = rtf.Append(@"{\listlevel\levelnfc0\levelnfcn0\leveljc0\leveljcn0\levelfollow0\levelstartat1\levelspace0\levelindent0{\leveltext\leveltemplateid2\'02\'00.;}{\levelnumbers\'01;}\fi-360\li720\jclisttab\tx720}");
			_ = rtf.Append(@"{\listname ;}\listid2}");
			_ = rtf.Append('}');
			_ = rtf.Append(@"{\*\listoverridetable{\listoverride\listid1\listoverridecount0\ls1}{\listoverride\listid2\listoverridecount0\ls2}}");

			_ = rtf.Append(@"{\*\generator RtfConverter;}");
			_ = rtf.Append(CultureInfo.InvariantCulture, $@"\viewkind4\uc1\pard\sa200\sl276\slmult1\f0\fs{defaultFontSizeHalfPoints} ");

			// STEP 5: Decode HTML entities FIRST (before escaping RTF characters)
			text = text.Replace("&nbsp;", " ", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#160;", " ", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&lt;", "<", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#60;", "<", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&gt;", ">", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#62;", ">", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&amp;", "&", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#38;", "&", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&quot;", "\"", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#160;", "\"", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&apos;", "'", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#39;", "'", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&cent;", "¢", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#162;", "¢", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&pound;", "£", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#163;", "£", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&yen;", "¥", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#165;", "¥", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&euro;", "€", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#8364;", "€", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&copy;", "©", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#169;", "©", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&reg;", "®", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#174;", "®", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&trade;", "™", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#8482;", "™", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&macr;", "¯", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#0175;", "¯", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&mdash;", "—", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#0151;", "—", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&ndash;", "–", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#0150;", "–", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&micro;", "µ", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#0181;", "µ", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&times;", "×", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#0215;", "×", StringComparison.InvariantCultureIgnoreCase);

			// STEP 6: Escape RTF special characters SECOND (before converting tags)
			text = text.Replace(@"\", @"\\", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("{", @"\{", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("}", @"\}", StringComparison.InvariantCultureIgnoreCase);

			// STEP 7: Convert HTML tags to RTF codes

			// Handle code blocks first (pre tag)
			text = Regex.Replace(text, @"<pre[^>]*>(.*?)</pre>", m =>
			{
				string content = m.Groups[1].Value;
				int courierIndex = fonts.FindIndex(f => f.Equals("Courier New", StringComparison.OrdinalIgnoreCase));
				return $@"{{\pard\f{courierIndex}\fs{defaultFontSizeHalfPoints} {content}\par\f0}}";
			}, RegexOptions.IgnoreCase | RegexOptions.Singleline);

			// Handle inline code
			text = Regex.Replace(text, @"<code>(.*?)</code>", m =>
			{
				string content = m.Groups[1].Value;
				int courierIndex = fonts.FindIndex(f => f.Equals("Courier New", StringComparison.OrdinalIgnoreCase));
				return $@"{{\f{courierIndex} {content}}}";
			}, RegexOptions.IgnoreCase | RegexOptions.Singleline);

			// Handle blockquote
			text = Regex.Replace(text, @"<blockquote[^>]*>(.*?)</blockquote>", @"\li720\ri720 $1\li0\ri0", RegexOptions.IgnoreCase | RegexOptions.Singleline);

			// Handle spans with multiple styles (font-family, color, background-color, font-size)
			text = Regex.Replace(text, @"<span[^>]*style=[""']([^""']*)[""'][^>]*>(.*?)</span>", m =>
			{
				string style = m.Groups[1].Value;
				string content = m.Groups[2].Value;
				StringBuilder spanRtf = new();

				_ = spanRtf.Append('{');

				// Font family
				Match fontMatch = Regex.Match(style, @"font-family:\s*([^;]+)", RegexOptions.IgnoreCase);
				if (fontMatch.Success)
				{
					string fontName = fontMatch.Groups[1].Value.Trim().Trim('\'', '"');
					int fontIndex = fonts.FindIndex(f => f.Equals(fontName, StringComparison.OrdinalIgnoreCase));
					if (fontIndex >= 0)
					{
						_ = spanRtf.Append(CultureInfo.InvariantCulture, $@"\f{fontIndex} ");
					}
				}

				// Font size
				Match sizeMatch = Regex.Match(style, @"font-size:\s*(\d+)pt", RegexOptions.IgnoreCase);
				if (sizeMatch.Success)
				{
					int points = int.Parse(sizeMatch.Groups[1].Value, CultureInfo.InvariantCulture);
					_ = spanRtf.Append(CultureInfo.InvariantCulture, $@"\fs{points * 2} ");
				}

				// Text color
				Match colorMatch = Regex.Match(style, @"(?:^|;)\s*color:\s*(?:rgb\((\d+),\s*(\d+),\s*(\d+)\)|#([0-9A-Fa-f]{6}))", RegexOptions.IgnoreCase);
				if (colorMatch.Success)
				{
					int r, g, b;
					if (colorMatch.Groups[4].Success)
					{
						string hex = colorMatch.Groups[4].Value;
						r = Convert.ToInt32(hex[..2], 16);
						g = Convert.ToInt32(hex.Substring(2, 2), 16);
						b = Convert.ToInt32(hex.Substring(4, 2), 16);
					}
					else
					{
						r = int.Parse(colorMatch.Groups[1].Value, CultureInfo.InvariantCulture);
						g = int.Parse(colorMatch.Groups[2].Value, CultureInfo.InvariantCulture);
						b = int.Parse(colorMatch.Groups[3].Value, CultureInfo.InvariantCulture);
					}

					int colorIndex = colors.IndexOf((r, g, b));
					if (colorIndex >= 0)
					{
						_ = spanRtf.Append(CultureInfo.InvariantCulture, $@"\cf{colorIndex} ");
					}
				}

				// Background color
				Match bgColorMatch = Regex.Match(style, @"background-color:\s*(?:rgb\((\d+),\s*(\d+),\s*(\d+)\)|#([0-9A-Fa-f]{6}))", RegexOptions.IgnoreCase);
				if (bgColorMatch.Success)
				{
					int r, g, b;
					if (bgColorMatch.Groups[4].Success)
					{
						string hex = bgColorMatch.Groups[4].Value;
						r = Convert.ToInt32(hex[..2], 16);
						g = Convert.ToInt32(hex.Substring(2, 2), 16);
						b = Convert.ToInt32(hex.Substring(4, 2), 16);
					}
					else
					{
						r = int.Parse(bgColorMatch.Groups[1].Value, CultureInfo.InvariantCulture);
						g = int.Parse(bgColorMatch.Groups[2].Value, CultureInfo.InvariantCulture);
						b = int.Parse(bgColorMatch.Groups[3].Value, CultureInfo.InvariantCulture);
					}

					int colorIndex = colors.IndexOf((r, g, b));
					if (colorIndex >= 0)
					{
						_ = spanRtf.Append(CultureInfo.InvariantCulture, $@"\highlight{colorIndex} ");
					}
				}

				// Text decoration underline
				if (Regex.IsMatch(style, @"text-decoration:\s*underline", RegexOptions.IgnoreCase))
				{
					_ = spanRtf.Append(@"\ul ");
				}

				// Indentation (margin-left or padding-left)
				Match indentMatch = Regex.Match(style, @"(?:margin-left|padding-left):\s*(\d+)px", RegexOptions.IgnoreCase);
				if (indentMatch.Success)
				{
					int pixels = int.Parse(indentMatch.Groups[1].Value, CultureInfo.InvariantCulture);
					int twips = pixels * 15; // Approximate conversion
					_ = spanRtf.Append(CultureInfo.InvariantCulture, $@"\li{twips} ");
				}

				_ = spanRtf.Append(content);
				_ = spanRtf.Append('}');

				return spanRtf.ToString();
			}, RegexOptions.IgnoreCase | RegexOptions.Singleline);

			// Handle superscript
			text = Regex.Replace(text, @"<sup>(.*?)</sup>", @"{\super $1}", RegexOptions.IgnoreCase | RegexOptions.Singleline);

			// Handle subscript
			text = Regex.Replace(text, @"<sub>(.*?)</sub>", @"{\sub $1}", RegexOptions.IgnoreCase | RegexOptions.Singleline);

			// Handle bold
			text = Regex.Replace(text, @"<strong>(.*?)</strong>", @"{\b $1}", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			text = Regex.Replace(text, @"<b>(.*?)</b>", @"{\b $1}", RegexOptions.IgnoreCase | RegexOptions.Singleline);

			// Handle italic
			text = Regex.Replace(text, @"<em>(.*?)</em>", @"{\i $1}", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			text = Regex.Replace(text, @"<i>(.*?)</i>", @"{\i $1}", RegexOptions.IgnoreCase | RegexOptions.Singleline);

			// Handle underline (plain u tag)
			text = Regex.Replace(text, @"<u>(.*?)</u>", @"{\ul $1}", RegexOptions.IgnoreCase | RegexOptions.Singleline);

			// Handle ordered (numbered) lists
			int listCounter = 1;
			text = Regex.Replace(text, @"<ol[^>]*>(.*?)</ol>", m =>
			{
				string items = m.Groups[1].Value;
				listCounter = 1;
				StringBuilder listRtf = new();

				MatchCollection matches = Regex.Matches(items, @"<li[^>]*>(.*?)</li>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
				foreach (Match match in matches)
				{
					string content = match.Groups[1].Value;
					_ = listRtf.Append(CultureInfo.InvariantCulture, $@"{{\pntext {listCounter}.\tab}}{{\*\pn\pnlvl1\pndec\pnstart1\pnindent720\pnhang{{\pntxta .}}}}\ls2\li720\fi-360\jclisttab\tx720 {content}\par ");
					listCounter++;
				}

				return listRtf.ToString();
			}, RegexOptions.IgnoreCase | RegexOptions.Singleline);

			// Handle unordered (bulleted) lists
			text = Regex.Replace(text, @"<ul[^>]*>(.*?)</ul>", m =>
			{
				string items = m.Groups[1].Value;
				StringBuilder listRtf = new();

				MatchCollection matches = Regex.Matches(items, @"<li[^>]*>(.*?)</li>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
				foreach (Match match in matches)
				{
					string content = match.Groups[1].Value;
					_ = listRtf.Append(CultureInfo.InvariantCulture, $@"{{\pntext \'b7\tab}}{{\*\pn\pnlvlblt\pnstart1{{\pntxtb \'b7}}}}\ls1\li720\fi-360\jclisttab\tx720 {content}\par ");
				}

				return listRtf.ToString();
			}, RegexOptions.IgnoreCase | RegexOptions.Singleline);

			// Handle paragraphs
			text = Regex.Replace(text, @"<p[^>]*>(.*?)</p>", @"\pard $1\par ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			text = Regex.Replace(text, @"<div[^>]*>(.*?)</div>", @"$1", RegexOptions.IgnoreCase | RegexOptions.Singleline);

			// Convert line breaks
			text = Regex.Replace(text, @"<br\s*/?>", @"\line ", RegexOptions.IgnoreCase);

			// Remove remaining HTML tags
			text = Regex.Replace(text, @"<[^>]+>", "");

			_ = rtf.Append(text);
			_ = rtf.Append('}');

			return rtf.ToString();
		}
		catch (Exception ex)
		{
			// Log the exception if needed
			Debug.WriteLine($"HTML to RTF conversion error: {ex.Message}");
			return $"{ex.GetType().Name}: {ex.Message}";
		}
	}

	/// <summary>
	/// Converts HTML text to plain text format, removing all HTML tags and preserving basic formatting structure.
	/// </summary>
	/// <param name="htmlText">The HTML text to convert. Can be null or whitespace.</param>
	/// <returns>
	/// The plain text representation of the HTML with basic formatting preserved (indentation for code blocks,
	/// quote markers for blockquotes, bullets/numbers for lists), or an empty string if the input is null or whitespace.
	/// Returns an error message string if the conversion fails.
	/// </returns>
	/// <remarks>
	/// This method preserves structural elements like code block indentation, blockquote markers (>),
	/// list bullets (•), and numbered list items. HTML entities are decoded to their character equivalents.
	/// </remarks>
	public static string HtmlToPlainText(string? htmlText)
	{
		if (string.IsNullOrWhiteSpace(htmlText))
		{
			return string.Empty;
		}

		try
		{
			string text = htmlText;

			// Handle code blocks - preserve formatting with indentation
			text = Regex.Replace(text, @"<pre[^>]*>(.*?)</pre>", m =>
			{
				string code = m.Groups[1].Value;
				// Indent each line
				string[] lines = code.Split(_separator, StringSplitOptions.None);
				return "\n" + string.Join("\n", lines.Select(line => "    " + line)) + "\n";
			}, RegexOptions.IgnoreCase | RegexOptions.Singleline);

			// Handle blockquotes - add quote marker
			text = Regex.Replace(text, @"<blockquote[^>]*>(.*?)</blockquote>", m =>
			{
				string quote = m.Groups[1].Value;
				string[] lines = quote.Split(_separator, StringSplitOptions.None);
				return "\n" + string.Join("\n", lines.Select(line => "> " + line.Trim())) + "\n";
			}, RegexOptions.IgnoreCase | RegexOptions.Singleline);

			// Convert block-level elements to line breaks
			text = Regex.Replace(text, @"<br\s*/?>", "\n", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"</p>", "\n\n", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"</div>", "\n", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"</h[1-6]>", "\n\n", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"</li>", "\n", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"</tr>", "\n", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"</td>", "\t", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"</th>", "\t", RegexOptions.IgnoreCase);

			// Add line breaks before/after lists
			text = Regex.Replace(text, @"<ul[^>]*>", "\n", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"</ul>", "\n", RegexOptions.IgnoreCase);

			// Handle ordered lists - number the items properly
			text = Regex.Replace(text, @"<ol[^>]*>(.*?)</ol>", m =>
			{
				string listContent = m.Value;
				int itemNumber = 1;

				// Replace each <li> with numbered prefix
				listContent = Regex.Replace(listContent, @"<li[^>]*>", match => $"\n{itemNumber++}. ", RegexOptions.IgnoreCase);

				// Remove the <ol> tags
				listContent = Regex.Replace(listContent, @"</?ol[^>]*>", "\n", RegexOptions.IgnoreCase);

				return listContent;
			}, RegexOptions.IgnoreCase | RegexOptions.Singleline);

			// Handle unordered lists - add bullet prefix
			text = Regex.Replace(text, @"<li[^>]*>", "• ", RegexOptions.IgnoreCase);

			// Remove all remaining HTML tags
			text = Regex.Replace(text, @"<[^>]+>", "");

			// Decode HTML entities
			text = text.Replace("&nbsp;", " ", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#160;", " ", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&lt;", "<", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#60;", "<", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&gt;", ">", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#62;", ">", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&amp;", "&", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#38;", "&", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&quot;", "\"", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#160;", "\"", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&apos;", "'", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#39;", "'", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&cent;", "¢", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#162;", "¢", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&pound;", "£", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#163;", "£", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&yen;", "¥", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#165;", "¥", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&euro;", "€", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#8364;", "€", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&copy;", "©", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#169;", "©", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&reg;", "®", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#174;", "®", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&trade;", "™", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#8482;", "™", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&macr;", "¯", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#0175;", "¯", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&mdash;", "—", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#0151;", "—", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&ndash;", "–", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#0150;", "–", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&micro;", "µ", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#0181;", "µ", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&times;", "×", StringComparison.InvariantCultureIgnoreCase);
			text = text.Replace("&#0215;", "×", StringComparison.InvariantCultureIgnoreCase);

			// Clean up extra whitespace on each line
			text = Regex.Replace(text, @"[ \t]+", " ");

			// Clean up multiple consecutive newlines (max 2)
			text = Regex.Replace(text, @"\n{3,}", "\n\n");

			// Remove leading/trailing whitespace but preserve newlines
			string[] lines = text.Split('\n');
			text = string.Join("\n", lines.Select(line => line.Trim()));

			return text.Trim();
		}
		catch (Exception ex)
		{
			// Log the exception if needed
			Debug.WriteLine($"HTML to plain text conversion error: {ex.Message}");
			return $"{ex.GetType().Name}: {ex.Message}";
		}
	}
}
