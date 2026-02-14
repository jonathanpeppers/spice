namespace Spice.Tests;

/// <summary>
/// Tests for TimePicker control
/// </summary>
public class TimePickerTests
{
	[Fact]
	public void TimePickerDefaultTime()
	{
		var timePicker = new TimePicker();
		// Verify time is set to something reasonable (default is current time)
		Assert.NotEqual(default(TimeOnly), timePicker.Time);
	}

	[Fact]
	public void TimePickerSetTime()
	{
		var timePicker = new TimePicker();
		var expectedTime = new TimeOnly(14, 30);
		timePicker.Time = expectedTime;

		Assert.Equal(expectedTime, timePicker.Time);
	}

	[Fact]
	public void TimePickerPropertyChanged()
	{
		string? property = null;
		var timePicker = new TimePicker();
		timePicker.PropertyChanged += (sender, e) => property = e.PropertyName;

		var newTime = new TimeOnly(10, 15);
		timePicker.Time = newTime;

		Assert.Equal(nameof(timePicker.Time), property);
	}

	[Fact]
	public void TimePickerPropertyChanging()
	{
		string? property = null;
		var timePicker = new TimePicker();
		timePicker.PropertyChanging += (sender, e) => property = e.PropertyName;

		var newTime = new TimeOnly(16, 45);
		timePicker.Time = newTime;

		Assert.Equal(nameof(timePicker.Time), property);
	}

	[Fact]
	public void TimePickerTimeSpanConversion()
	{
		var timePicker = new TimePicker();
		var expectedTime = new TimeOnly(23, 59);
		timePicker.Time = expectedTime;

		var timeSpan = timePicker.Time.ToTimeSpan();
		Assert.Equal(23, timeSpan.Hours);
		Assert.Equal(59, timeSpan.Minutes);
	}
}
