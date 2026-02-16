namespace Spice;

public partial class RadioButton
{
	/// <summary>
	/// Returns radioButton.NativeView
	/// </summary>
	/// <param name="radioButton">The Spice.RadioButton</param>
	public static implicit operator UIButton(RadioButton radioButton) => radioButton.NativeView;

	/// <summary>
	/// A radio button control for single selection from a group of options.
	/// Android -> Android.Widget.RadioButton
	/// iOS -> UIKit.UIButton (with circle styling)
	/// </summary>
	public RadioButton() : base(_ => new UIButton(UIButtonType.System) { AutoresizingMask = UIViewAutoresizing.None })
	{
		InitializeRadioButton();
	}

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public RadioButton(CGRect frame) : base(_ =>
	{
		var button = new UIButton(UIButtonType.System)
		{
			AutoresizingMask = UIViewAutoresizing.None,
			Frame = frame
		};
		return button;
	})
	{
		InitializeRadioButton();
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected RadioButton(Func<View, UIView> creator) : base(creator)
	{
		InitializeRadioButton();
	}

	/// <summary>
	/// The underlying UIKit.UIButton
	/// </summary>
	public new UIButton NativeView => (UIButton)_nativeView.Value;

	void InitializeRadioButton()
	{
		NativeView.SetImage(UIImage.GetSystemImage("circle"), UIControlState.Normal);
		NativeView.SetImage(UIImage.GetSystemImage("circle.fill"), UIControlState.Selected);
		NativeView.TouchUpInside += OnTouchUpInside;
	}

	partial void OnIsCheckedChangedPartial(bool value)
	{
		NativeView.Selected = value;
	}

	partial void OnContentChanged(string? value)
	{
		NativeView.SetTitle(value, UIControlState.Normal);
	}

	void OnTouchUpInside(object? sender, EventArgs e)
	{
		// Radio buttons only get checked, not unchecked by user
		if (!IsChecked)
		{
			IsChecked = true;
			// Fire CheckedChanged only when state actually changes
			CheckedChanged?.Invoke(this);
		}
		// Tapping an already-checked radio button does nothing
	}

	partial void OnCheckedChangedChanged(Action<RadioButton>? value)
	{
		// Event subscription is handled in InitializeRadioButton
	}
}
