namespace Spice;

public partial class RadioButton
{
	/// <summary>
	/// Returns radioButton.NativeView
	/// </summary>
	/// <param name="radioButton">The Spice.RadioButton</param>
	public static implicit operator UIButton(RadioButton radioButton) => radioButton.NativeView;

	static Dictionary<string, List<WeakReference<RadioButton>>> _groups = new();

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

	partial void OnIsCheckedChanged(bool value)
	{
		NativeView.Selected = value;
		
		// Handle group exclusivity
		if (value && !string.IsNullOrEmpty(GroupName))
		{
			UncheckOthersInGroup();
		}
	}

	partial void OnGroupNameChanged(string? value)
	{
		// Group management happens when IsChecked changes
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
			CheckedChanged?.Invoke(this);
		}
	}

	partial void OnCheckedChangedChanged(Action<RadioButton>? value)
	{
		// Event subscription is handled in InitializeRadioButton
	}

	void UncheckOthersInGroup()
	{
		if (string.IsNullOrEmpty(GroupName))
			return;

		lock (_groups)
		{
			if (_groups.TryGetValue(GroupName, out var groupList))
			{
				// Clean up dead references and uncheck others
				groupList.RemoveAll(wr => !wr.TryGetTarget(out _));
				
				foreach (var weakRef in groupList)
				{
					if (weakRef.TryGetTarget(out var radioButton) && radioButton != this && radioButton.IsChecked)
					{
						radioButton.IsChecked = false;
						radioButton.CheckedChanged?.Invoke(radioButton);
					}
				}
			}
			else
			{
				// Create the group list
				groupList = new List<WeakReference<RadioButton>>();
				_groups[GroupName] = groupList;
			}

			// Add this radio button to the group if not already present
			bool found = false;
			foreach (var weakRef in groupList)
			{
				if (weakRef.TryGetTarget(out var rb) && rb == this)
				{
					found = true;
					break;
				}
			}
			
			if (!found)
			{
				groupList.Add(new WeakReference<RadioButton>(this));
			}
		}
	}
}
