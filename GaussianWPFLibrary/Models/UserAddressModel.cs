using System.Text.Json.Serialization;

namespace GaussianWPFLibrary.Models;

/// <summary>
/// Represents a user's physical address with standard address components.
/// </summary>
public class UserAddressModel
{
	/// <summary>
	/// Gets or sets the street address, including house number and street name.
	/// </summary>
	[JsonPropertyName("street_address")]
	public string StreetAddress { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the locality (city or town).
	/// </summary>
	[JsonPropertyName("locality")]
	public string Locality { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the region (state, province, or prefecture).
	/// </summary>
	[JsonPropertyName("region")]
	public string Region { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the postal code (ZIP code).
	/// </summary>
	[JsonPropertyName("postal_code")]
	public string PostalCode { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the country name.
	/// </summary>
	[JsonPropertyName("country")]
	public string Country { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the pre-formatted complete address as a single string.
	/// </summary>
	[JsonPropertyName("formatted")]
	public string Formatted { get; set; } = string.Empty;

	/// <summary>
	/// Returns a string representation of the address.
	/// </summary>
	/// <returns>
	/// The pre-formatted address if available; otherwise, a formatted address constructed from the individual address components.
	/// </returns>
	public override string? ToString()
	{
		return string.IsNullOrWhiteSpace(Formatted) ? $"{StreetAddress}\n{Locality}, {Region} {PostalCode} {Country}" : Formatted;
	}
}
