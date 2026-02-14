namespace Spice.Tests;

/// <summary>
/// Tests for ProgressBar control
/// </summary>
public class ProgressBarTests
{
	[Fact]
	public void Constructor()
	{
		var progressBar = new ProgressBar();
		Assert.NotNull(progressBar);
	}

	[Fact]
	public void DefaultProgress()
	{
		var progressBar = new ProgressBar();
		Assert.Equal(0.0, progressBar.Progress);
	}

	[Fact]
	public void SetProgress()
	{
		var progressBar = new ProgressBar
		{
			Progress = 0.5
		};
		Assert.Equal(0.5, progressBar.Progress);
	}

	[Fact]
	public void ProgressRange()
	{
		var progressBar = new ProgressBar();
		
		// Test minimum
		progressBar.Progress = 0.0;
		Assert.Equal(0.0, progressBar.Progress);
		
		// Test maximum
		progressBar.Progress = 1.0;
		Assert.Equal(1.0, progressBar.Progress);
		
		// Test middle value
		progressBar.Progress = 0.75;
		Assert.Equal(0.75, progressBar.Progress);
	}

	[Fact]
	public void PropertyChangedFires()
	{
		string? property = null;
		var progressBar = new ProgressBar();
		progressBar.PropertyChanged += (sender, e) => property = e.PropertyName;
		progressBar.Progress = 0.3;

		Assert.Equal(nameof(progressBar.Progress), property);
	}
}
