using System.Text;

using Microsoft.Extensions.Configuration;

using SautinSoft;

namespace GaussianMVCLibrary.Converters;

/// <summary>
/// Provides static methods for converting between RTF, HTML, and plain text formats using the SautinSoft library.
/// </summary>
public static class RtfConverter
{
	private static IConfiguration? _config;
	private static string? _licenseKey;

	/// <summary>
	/// Initializes the converter by setting the application configuration and retrieving the SautinSoft license key.
	/// </summary>
	/// <param name="config">The application configuration instance containing the SautinSoft license key.</param>
	/// <exception cref="InvalidOperationException">Thrown when the SautinSoft license key is not found in the configuration.</exception>
	/// <remarks>
	/// This method must be called before using any of the converter methods.
	/// The license key is expected to be stored in the configuration with the key "SautinSoftLicenseKey".
	/// </remarks>
	public static void SetConfiguration(IConfiguration config)
	{
		_config = config;
		_licenseKey = _config.GetValue<string>("SautinSoftLicenseKey") ?? throw new InvalidOperationException("SautinSoft license key not found.");
	}

	/// <summary>
	/// Converts RTF (Rich Text Format) content to HTML format.
	/// </summary>
	/// <param name="rtfString">The RTF string to convert. Can be null or whitespace.</param>
	/// <returns>
	/// An HTML5 string representation of the RTF content, containing only the body content without page borders or margins.
	/// Returns an empty string if the input is null or whitespace.
	/// </returns>
	public static string RtfToHtmlConverter(string? rtfString)
	{
		string htmlString = string.Empty;

		if (!string.IsNullOrWhiteSpace(rtfString))
		{
			RtfToHtml.SetLicense(_licenseKey);
			RtfToHtml converter = new();
			using MemoryStream inpMs = new(Encoding.UTF8.GetBytes(rtfString));
			using MemoryStream outMs = new();

			RtfToHtml.HtmlFlowingSaveOptions options = new()
			{
				BuildNavigationPage = false,
				Encoding = Encoding.UTF8,
				HeadersFootersExportMode = RtfToHtml.HtmlHeadersFootersExportMode.None,
				ProduceOnlyHtmlBody = true,
				Title = null,
				Version = RtfToHtml.HtmlVersion.Html5
			};

			converter.Convert(inpMs, outMs, options);
			htmlString = Encoding.UTF8.GetString(outMs.ToArray());
		}

		return htmlString;
	}

	/// <summary>
	/// Converts HTML content to RTF (Rich Text Format).
	/// </summary>
	/// <param name="htmlString">The HTML string to convert. Can be null or whitespace.</param>
	/// <returns>
	/// An RTF string representation of the HTML content using Segoe UI font family and 12pt font size.
	/// Returns an empty string if the input is null or whitespace.
	/// </returns>
	public static string HtmlToRtfConverter(string? htmlString)
	{
		string rtfString = string.Empty;

		if (!string.IsNullOrWhiteSpace(htmlString))
		{
			HtmlToRtf.SetLicense(_licenseKey);
			HtmlToRtf converter = new();
			using MemoryStream inpMs = new(Encoding.UTF8.GetBytes(htmlString));
			using MemoryStream outMs = new();

			HtmlToRtf.HtmlConvertOptions options = new()
			{
				Encoding = HtmlToRtf.Encoding.utf8,
				OutputFormat = HtmlToRtf.OutputFormat.Rtf,
				TextSetup = new HtmlToRtf.TextSetup()
				{
					DefaultFontFamily = "Segoe UI",
					DefaultFontSize = 12f
				}
			};

			_ = converter.Convert(inpMs, outMs, options);
			rtfString = Encoding.UTF8.GetString(outMs.ToArray());
		}

		return rtfString;
	}

	/// <summary>
	/// Converts HTML content to plain text format.
	/// </summary>
	/// <param name="htmlString">The HTML string to convert. Can be null or whitespace.</param>
	/// <returns>
	/// A UTF-8 encoded plain text representation of the HTML content with BOM (Byte Order Mark).
	/// Returns an empty string if the input is null or whitespace.
	/// </returns>
	public static string HtmlToTextConverter(string? htmlString)
	{
		string textString = string.Empty;

		if (!string.IsNullOrWhiteSpace(htmlString))
		{
			HtmlToRtf.SetLicense(_licenseKey);
			HtmlToRtf converter = new();
			using MemoryStream inpMs = new(Encoding.UTF8.GetBytes(htmlString));
			using MemoryStream outMs = new();

			HtmlToRtf.HtmlConvertOptions options = new()
			{
				Encoding = HtmlToRtf.Encoding.utf8,
				OutputFormat = HtmlToRtf.OutputFormat.TextUTF8WithBOM,
				TextSetup = new HtmlToRtf.TextSetup()
				{
					DefaultFontFamily = "Segoe UI",
					DefaultFontSize = 12f
				}
			};

			_ = converter.Convert(inpMs, outMs, options);
			textString = Encoding.UTF8.GetString(outMs.ToArray());
		}

		return textString;
	}
}
