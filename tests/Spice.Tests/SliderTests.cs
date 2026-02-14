namespace Spice.Tests;

/// <summary>
/// Tests for the Slider control
/// </summary>
public class SliderTests
{
	[Fact]
	public void DefaultValues()
	{
		var slider = new Slider();
		Assert.Equal(0, slider.Minimum);
		Assert.Equal(1, slider.Maximum);
		Assert.Equal(0, slider.Value);
		Assert.Null(slider.ValueChanged);
	}

	[Fact]
	public void SetMinimum()
	{
		var slider = new Slider();
		slider.Minimum = 10;
		Assert.Equal(10, slider.Minimum);
	}

	[Fact]
	public void SetMaximum()
	{
		var slider = new Slider();
		slider.Maximum = 100;
		Assert.Equal(100, slider.Maximum);
	}

	[Fact]
	public void SetValue()
	{
		var slider = new Slider();
		slider.Value = 0.5;
		Assert.Equal(0.5, slider.Value);
	}

	[Fact]
	public void SetValueWithRange()
	{
		var slider = new Slider
		{
			Minimum = 0,
			Maximum = 100,
			Value = 50
		};
		Assert.Equal(0, slider.Minimum);
		Assert.Equal(100, slider.Maximum);
		Assert.Equal(50, slider.Value);
	}

	[Fact]
	public void ValueChangedEvent()
	{
		var slider = new Slider();
		Slider? changedSlider = null;
		slider.ValueChanged = s => changedSlider = s;
		Assert.NotNull(slider.ValueChanged);
		slider.ValueChanged.Invoke(slider);
		Assert.Same(slider, changedSlider);
	}

	[Fact]
	public void PropertyChangedFires()
	{
		var slider = new Slider();
		string? propertyName = null;
		slider.PropertyChanged += (sender, e) => propertyName = e.PropertyName;

		slider.Value = 0.7;
		Assert.Equal(nameof(slider.Value), propertyName);

		propertyName = null;
		slider.Minimum = 5;
		Assert.Equal(nameof(slider.Minimum), propertyName);

		propertyName = null;
		slider.Maximum = 95;
		Assert.Equal(nameof(slider.Maximum), propertyName);
	}
}
