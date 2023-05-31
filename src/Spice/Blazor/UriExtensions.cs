// From: https://github.com/dotnet/maui/blob/c8a6093ee805388732af8d75b437b099a301db22/src/BlazorWebView/src/Maui/Extensions/UriExtensions.cs
namespace Spice;

internal static class UriExtensions
{
	internal static bool IsBaseOfPage(this Uri baseUri, string? uriString)
	{
		if (Path.HasExtension(uriString))
		{
			// If the path ends in a file extension, it's not referring to a page.
			return false;
		}

		var uri = new Uri(uriString!);
		return baseUri.IsBaseOf(uri);
	}

	// From: https://github.com/dotnet/maui/blob/c8a6093ee805388732af8d75b437b099a301db22/src/BlazorWebView/src/SharedSource/QueryStringHelper.cs
	public static string RemovePossibleQueryString(string? url)
	{
		if (string.IsNullOrEmpty(url))
		{
			return string.Empty;
		}
		var indexOfQueryString = url.IndexOf('?', StringComparison.Ordinal);
		return (indexOfQueryString == -1)
			? url
			: url.Substring(0, indexOfQueryString);
	}
}