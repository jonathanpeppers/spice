namespace Spice.Tests;

/// <summary>
/// Tests for the cross-platform WebView view behavior and defaults.
/// </summary>

public class WebViewTests
{
	[Fact]
	public void WebViewCanBeCreated()
	{
		var webView = new WebView();
		Assert.NotNull(webView);
	}

	[Fact]
	public void WebViewInheritsFromView()
	{
		var webView = new WebView();
		Assert.IsAssignableFrom<View>(webView);
	}

	[Fact]
	public void SourcePropertyDefaultsToEmptyString()
	{
		var webView = new WebView();
		Assert.Equal("", webView.Source);
	}

	[Fact]
	public void SourcePropertyCanBeSet()
	{
		var webView = new WebView { Source = "https://example.com" };
		Assert.Equal("https://example.com", webView.Source);
	}

	[Fact]
	public void IsJavaScriptEnabledDefaultsToTrue()
	{
		var webView = new WebView();
		Assert.True(webView.IsJavaScriptEnabled);
	}

	[Fact]
	public void IsJavaScriptEnabledCanBeSet()
	{
		var webView = new WebView { IsJavaScriptEnabled = false };
		Assert.False(webView.IsJavaScriptEnabled);
	}

	[Fact]
	public void PropertyChangedFiresOnSourceChange()
	{
		var webView = new WebView();
		var propertyChangedFired = false;
		webView.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(WebView.Source))
				propertyChangedFired = true;
		};

		webView.Source = "https://example.com";
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void PropertyChangedFiresOnIsJavaScriptEnabledChange()
	{
		var webView = new WebView();
		var propertyChangedFired = false;
		webView.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(WebView.IsJavaScriptEnabled))
				propertyChangedFired = true;
		};

		webView.IsJavaScriptEnabled = false;
		Assert.True(propertyChangedFired);
	}

	[Fact]
	public void CanSetAllPropertiesAtOnce()
	{
		var webView = new WebView
		{
			Source = "https://example.com",
			IsJavaScriptEnabled = false
		};

		Assert.Equal("https://example.com", webView.Source);
		Assert.False(webView.IsJavaScriptEnabled);
	}

	[Fact]
	public void SourceCanBeSetMultipleTimes()
	{
		var webView = new WebView { Source = "https://first.com" };
		Assert.Equal("https://first.com", webView.Source);
		
		webView.Source = "https://second.com";
		Assert.Equal("https://second.com", webView.Source);
	}

	[Fact]
	public void MultipleWebViewInstancesAreIndependent()
	{
		var webView1 = new WebView { Source = "https://first.com", IsJavaScriptEnabled = true };
		var webView2 = new WebView { Source = "https://second.com", IsJavaScriptEnabled = false };

		Assert.Equal("https://first.com", webView1.Source);
		Assert.Equal("https://second.com", webView2.Source);
		Assert.True(webView1.IsJavaScriptEnabled);
		Assert.False(webView2.IsJavaScriptEnabled);
	}
}
