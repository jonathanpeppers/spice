using Android.Content;
using Android.Widget;
using AndroidRadioButton = Android.Widget.RadioButton;

namespace Spice;

public partial class RadioButton
{
	/// <summary>
	/// Returns radioButton.NativeView
	/// </summary>
	/// <param name="radioButton">The Spice.RadioButton</param>
	public static implicit operator AndroidRadioButton(RadioButton radioButton) => radioButton.NativeView;

	static Dictionary<string, List<WeakReference<RadioButton>>> _groups = new();

	static AndroidRadioButton Create(Context context) => new(context);

	/// <summary>
	/// A radio button control for single selection from a group of options.
	/// Android -> Android.Widget.RadioButton
	/// iOS -> UIKit.UIButton (with circle styling)
	/// </summary>
	public RadioButton() : this(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public RadioButton(Context context) : this(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected RadioButton(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected RadioButton(Context context, Func<Context, Android.Views.View> creator) : base(context, creator)
	{
		// Always subscribe to keep IsChecked in sync
		NativeView.CheckedChange += OnCheckedChange;
	}

	/// <summary>
	/// The underlying Android.Widget.RadioButton
	/// </summary>
	public new AndroidRadioButton NativeView => (AndroidRadioButton)_nativeView.Value;

	bool _updatingChecked;

	partial void OnIsCheckedChanged(bool value)
	{
		if (_updatingChecked)
			return;

		_updatingChecked = true;
		NativeView.Checked = value;
		_updatingChecked = false;

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
		NativeView.Text = value;
	}

	void OnCheckedChange(object? sender, CompoundButton.CheckedChangeEventArgs e)
	{
		if (_updatingChecked)
			return;

		_updatingChecked = true;
		IsChecked = e.IsChecked;
		_updatingChecked = false;

		// Handle group exclusivity for user interaction
		if (e.IsChecked && !string.IsNullOrEmpty(GroupName))
		{
			UncheckOthersInGroup();
		}

		// Fire CheckedChanged for user interaction
		CheckedChanged?.Invoke(this);
	}

	partial void OnCheckedChangedChanged(Action<RadioButton>? value)
	{
		// Event subscription is handled in constructor
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
