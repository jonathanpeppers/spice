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

	[Fact]
	public void ProgressClampingBehavior()
	{
		// Native controls clamp values in their On*Changed partial methods
		// This ensures consistent behavior across platforms
		var progressBar = new ProgressBar();
		
		// Values below 0.0 should be stored as-is (clamping happens when setting native view)
		progressBar.Progress = -0.5;
		Assert.Equal(-0.5, progressBar.Progress);
		
		// Values above 1.0 should be stored as-is (clamping happens when setting native view)
		progressBar.Progress = 1.5;
		Assert.Equal(1.5, progressBar.Progress);
		
		// The native controls will display clamped values (0.0 or 1.0) even though
		// the property stores the original value
	}
}
