using System.Windows.Media;

namespace GaussianWPF;

/// <summary>
/// Provides application-wide constants for the Gaussian WPF application.
/// </summary>
public static class Constants
{
	/// <summary>
	/// Gets a standard font color palette containing 60 predefined colors.
	/// The colors are organized in a 6-row by 10-column grid layout,
	/// ranging from transparent/black tones to various shades of gray, blue, orange, yellow, green, and red.
	/// </summary>
	/// <remarks>
	/// The palette includes:
	/// <list type="bullet">
	/// <item><description>Row 1: Transparent, black, light gray, and primary accent colors</description></item>
	/// <item><description>Rows 2-3: Light to medium tones of various hues</description></item>
	/// <item><description>Rows 4-5: Medium to dark tones</description></item>
	/// <item><description>Row 6: Darkest shades</description></item>
	/// </list>
	/// All colors except the first (transparent) have full alpha opacity (255).
	/// </remarks>
	public static readonly Color[] FontColors =
	[
		// Row 1
		Color.FromArgb(0, 0, 0, 0), // Transparent
		Color.FromArgb(255, 0, 0, 0), // Black
		Color.FromArgb(255, 231, 230, 230),
		Color.FromArgb(255, 68, 84, 106),
		Color.FromArgb(255, 68, 114, 196),
		Color.FromArgb(255, 237, 125, 49),
		Color.FromArgb(255, 165, 165, 165),
		Color.FromArgb(255, 255, 192, 0),
		Color.FromArgb(255, 112, 173, 71),
		Color.FromArgb(255, 255, 0, 0), // Red
		// Row 2
		Color.FromArgb(255, 242, 242, 242),
		Color.FromArgb(255, 128, 128, 128), // Gray
		Color.FromArgb(255, 207, 205, 205),
		Color.FromArgb(255, 213, 220, 228),
		Color.FromArgb(255, 217, 226, 243),
		Color.FromArgb(255, 251, 228, 213),
		Color.FromArgb(255, 237, 237, 237),
		Color.FromArgb(255, 255, 242, 204),
		Color.FromArgb(255, 226, 239, 217),
		Color.FromArgb(255, 255, 204, 204),
		// Row 3
		Color.FromArgb(255, 217, 217, 217),
		Color.FromArgb(255, 89, 89, 89),
		Color.FromArgb(255, 174, 170, 170),
		Color.FromArgb(255, 172, 185, 202),
		Color.FromArgb(255, 180, 198, 231),
		Color.FromArgb(255, 247, 202, 172),
		Color.FromArgb(255, 219, 219, 219),
		Color.FromArgb(255, 255, 229, 153),
		Color.FromArgb(255, 197, 224, 179),
		Color.FromArgb(255, 255, 128, 128),
		// Row 4
		Color.FromArgb(255, 191, 191, 191), // Silver
		Color.FromArgb(255, 64, 64, 64),
		Color.FromArgb(255, 116, 112, 112),
		Color.FromArgb(255, 132, 150, 176),
		Color.FromArgb(255, 142, 170, 219),
		Color.FromArgb(255, 244, 176, 131),
		Color.FromArgb(255, 201, 201, 201),
		Color.FromArgb(255, 255, 217, 102),
		Color.FromArgb(255, 168, 208, 141),
		Color.FromArgb(255, 255, 51, 51),
		// Row 5
		Color.FromArgb(255, 166, 166, 166),
		Color.FromArgb(255, 38, 38, 38),
		Color.FromArgb(255, 59, 56, 56),
		Color.FromArgb(255, 50, 62, 79),
		Color.FromArgb(255, 47, 84, 150),
		Color.FromArgb(255, 196, 89, 17),
		Color.FromArgb(255, 123, 123, 123),
		Color.FromArgb(255, 191, 143, 0),
		Color.FromArgb(255, 83, 129, 53),
		Color.FromArgb(255, 179, 0, 0),
		// Row 6
		Color.FromArgb(255, 127, 127, 127),
		Color.FromArgb(255, 13, 13, 13),
		Color.FromArgb(255, 22, 22, 22),
		Color.FromArgb(255, 33, 41, 52),
		Color.FromArgb(255, 31, 55, 99),
		Color.FromArgb(255, 130, 59, 11),
		Color.FromArgb(255, 82, 82, 82),
		Color.FromArgb(255, 127, 95, 0),
		Color.FromArgb(255, 55, 86, 35),
		Color.FromArgb(255, 102, 0, 0) // Maroon
	];

	/// <summary>
	/// Gets a standard background color palette containing 60 predefined colors.
	/// The colors are organized in a 6-row by 10-column grid layout,
	/// featuring bright primary colors, pastels, and progressively darker shades.
	/// </summary>
	/// <remarks>
	/// The palette includes:
	/// <list type="bullet">
	/// <item><description>Row 1: Transparent white, black, and bright primary colors (yellow, green, cyan, blue, red, purple, brown)</description></item>
	/// <item><description>Rows 2-3: Light to medium pastel tones</description></item>
	/// <item><description>Rows 4-5: Medium to saturated tones</description></item>
	/// <item><description>Row 6: Darkest shades</description></item>
	/// </list>
	/// All colors except the first (transparent) have full alpha opacity (255).
	/// This palette is designed for background highlighting and cell coloring.
	/// </remarks>
	public static readonly Color[] BackgroundColors =
	[
		// Row 1
		Color.FromArgb(0, 255, 255, 255), // Transparent
		Color.FromArgb(255, 0, 0, 0), // Black
		Color.FromArgb(255, 255, 255, 0), // Yellow
		Color.FromArgb(255, 0, 255, 0), // Lime
		Color.FromArgb(255, 0, 255, 255), // Cyan
		Color.FromArgb(255, 0, 0, 255), // Blue
		Color.FromArgb(255, 255, 0, 0), // Red
		Color.FromArgb(255, 0, 0, 128), // Navy
		Color.FromArgb(255, 128, 0, 128), // Purple
		Color.FromArgb(255, 153, 102, 51),
		// Row 2
		Color.FromArgb(255, 242, 242, 242),
		Color.FromArgb(255, 128, 128, 128), // Gray
		Color.FromArgb(255, 255, 255, 204),
		Color.FromArgb(255, 179, 255, 179),
		Color.FromArgb(255, 204, 255, 255),
		Color.FromArgb(255, 204, 204, 254),
		Color.FromArgb(255, 255, 204, 204),
		Color.FromArgb(255, 204, 204, 255),
		Color.FromArgb(255, 255, 128, 255),
		Color.FromArgb(255, 242, 230, 217),
		// Row 3
		Color.FromArgb(255, 217, 217, 217),
		Color.FromArgb(255, 89, 89, 89),
		Color.FromArgb(255, 255, 255, 128),
		Color.FromArgb(255, 128, 255, 128),
		Color.FromArgb(255, 179, 255, 255),
		Color.FromArgb(255, 128, 128, 254),
		Color.FromArgb(255, 255, 128, 128),
		Color.FromArgb(255, 128, 128, 255),
		Color.FromArgb(255, 255, 0, 255), // Magenta
		Color.FromArgb(255, 223, 191, 159),
		// Row 4
		Color.FromArgb(255, 191, 191, 191), // Silver
		Color.FromArgb(255, 64, 64, 64),
		Color.FromArgb(255, 255, 255, 51),
		Color.FromArgb(255, 51, 255, 51),
		Color.FromArgb(255, 51, 255, 255),
		Color.FromArgb(255, 51, 51, 255),
		Color.FromArgb(255, 255, 51, 51),
		Color.FromArgb(255, 0, 0, 179),
		Color.FromArgb(255, 179, 0, 179),
		Color.FromArgb(255, 198, 140, 83),
		// Row 5
		Color.FromArgb(255, 166, 166, 166),
		Color.FromArgb(255, 38, 38, 38),
		Color.FromArgb(255, 230, 230, 0),
		Color.FromArgb(255, 0, 179, 0),
		Color.FromArgb(255, 0, 153, 153),
		Color.FromArgb(255, 0, 0, 153),
		Color.FromArgb(255, 179, 0, 0),
		Color.FromArgb(255, 0, 0, 101),
		Color.FromArgb(255, 102, 0, 102),
		Color.FromArgb(255, 134, 89, 45),
		// Row 6
		Color.FromArgb(255, 127, 127, 127),
		Color.FromArgb(255, 13, 13, 13),
		Color.FromArgb(255, 153, 153, 0),
		Color.FromArgb(255, 0, 102, 0), // DarkGreen
		Color.FromArgb(255, 0, 102, 102),
		Color.FromArgb(255, 0, 0, 102),
		Color.FromArgb(255, 102, 0, 0), // Maroon
		Color.FromArgb(255, 0, 0, 77),
		Color.FromArgb(255, 77, 0, 77),
		Color.FromArgb(255, 115, 77, 38)
	];
}