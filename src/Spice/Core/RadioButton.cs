namespace Spice;

/// <summary>
/// A radio button control for single selection from a group of options.
/// Android -> Android.Widget.RadioButton
/// iOS -> UIKit.UIButton (with circle styling)
///
/// Group exclusivity is managed in shared code via <see cref="GroupName"/> rather than
/// Android's RadioGroup, because iOS has no native radio button or group concept.
/// A cross-platform dictionary keeps behavior consistent on both platforms.
/// </summary>
public partial class RadioButton : View
{
	static readonly Dictionary<string, List<WeakReference<RadioButton>>> _groups = new();

	/// <summary>
	/// Whether the radio button is checked
	/// </summary>
	[ObservableProperty]
	bool _isChecked;

	/// <summary>
	/// The group name for mutually exclusive selection
	/// </summary>
	[ObservableProperty]
	string? _groupName;

	/// <summary>
	/// The text content to display next to the radio button
	/// </summary>
	[ObservableProperty]
	string? _content;

	/// <summary>
	/// Action to run when the radio button is checked or unchecked
	/// </summary>
	[ObservableProperty]
	Action<RadioButton>? _checkedChanged;

	partial void OnIsCheckedChanged(bool value)
	{
		OnIsCheckedChangedPartial(value);

		// Handle group exclusivity
		if (value && !string.IsNullOrEmpty(GroupName))
		{
			UncheckOthersInGroup();
		}
	}

	partial void OnGroupNameChanged(string? oldValue, string? newValue)
	{
		// Remove from old group
		if (!string.IsNullOrEmpty(oldValue))
		{
			lock (_groups)
			{
				if (_groups.TryGetValue(oldValue, out var oldGroupList))
				{
					oldGroupList.RemoveAll(wr => !wr.TryGetTarget(out var rb) || rb == this);
					
					// Clean up empty groups
					if (oldGroupList.Count == 0)
					{
						_groups.Remove(oldValue);
					}
				}
			}
		}

		// Register in new group and handle exclusivity if already checked
		if (IsChecked && !string.IsNullOrEmpty(newValue))
		{
			UncheckOthersInGroup();
		}
	}

	void UncheckOthersInGroup()
	{
		if (string.IsNullOrEmpty(GroupName))
			return;

		List<RadioButton> buttonsToNotify = new();

		lock (_groups)
		{
			if (_groups.TryGetValue(GroupName, out var groupList))
			{
				// Clean up dead references and collect buttons to uncheck
				groupList.RemoveAll(wr => !wr.TryGetTarget(out _));
				
				foreach (var weakRef in groupList)
				{
					if (weakRef.TryGetTarget(out var radioButton) && radioButton != this && radioButton.IsChecked)
					{
						radioButton.IsChecked = false;
						buttonsToNotify.Add(radioButton);
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

		// Invoke callbacks outside the lock
		foreach (var button in buttonsToNotify)
		{
			button.CheckedChanged?.Invoke(button);
		}
	}

	// Platform-specific implementation
	partial void OnIsCheckedChangedPartial(bool value);
}
