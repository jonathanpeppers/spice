namespace Spice;

public partial class CheckBox
{
	/// <summary>
	/// Returns checkBox.NativeView
	/// </summary>
	/// <param name="checkBox">The Spice.CheckBox</param>
	public static implicit operator UIButton(CheckBox checkBox) => checkBox.NativeView;

	/// <summary>
	/// A checkbox control for boolean selection.
	/// Android -> Android.Widget.CheckBox
	/// iOS -> UIKit.UIButton (with checkmark styling)
	/// </summary>
	public CheckBox() : base(_ => new UIButton(UIButtonType.System) { AutoresizingMask = UIViewAutoresizing.None })
	{
		InitializeCheckBox();
	}

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public CheckBox(CGRect frame) : base(_ =>
	{
		var button = new UIButton(UIButtonType.System)
		{
			AutoresizingMask = UIViewAutoresizing.None,
			Frame = frame
		};
		return button;
	})
	{
		InitializeCheckBox();
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected CheckBox(Func<View, UIView> creator) : base(creator)
	{
		InitializeCheckBox();
	}

	/// <summary>
	/// The underlying UIKit.UIButton
	/// </summary>
	public new UIButton NativeView => (UIButton)_nativeView.Value;

	void InitializeCheckBox()
	{
		NativeView.SetImage(UIImage.GetSystemImage("square"), UIControlState.Normal);
		NativeView.SetImage(UIImage.GetSystemImage("checkmark.square.fill"), UIControlState.Selected);
		NativeView.TouchUpInside += OnTouchUpInside;
	}

	partial void OnIsCheckedChanged(bool value)
	{
		NativeView.Selected = value;
	}

	void OnTouchUpInside(object? sender, EventArgs e)
	{
		IsChecked = !IsChecked;
		CheckedChanged?.Invoke(this);
	}

	partial void OnCheckedChangedChanged(Action<CheckBox>? value)
	{
		// Event subscription is handled in InitializeCheckBox
	}
}
