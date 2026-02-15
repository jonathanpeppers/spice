using System;

namespace Spice;

/// <summary>
/// Values that represent layout alignment.
/// Maps to UIKit/Android alignment options.
/// </summary>
[Flags]
public enum LayoutAlignment
{
	/// <summary>
	/// The start of an alignment. Usually the Top or Left.
	/// Maps to: <c>Android.Widget.RelativeLayout.AlignParentLeft</c> / <c>Android.Widget.RelativeLayout.AlignParentTop</c> / UIKit frame positioning
	/// </summary>
	Start = 0,
	/// <summary>
	/// The center of an alignment.
	/// Maps to: <c>Android.Widget.RelativeLayout.CenterHorizontal</c> / <c>Android.Widget.RelativeLayout.CenterVertical</c> / UIKit frame positioning
	/// </summary>
	Center = 1,
	/// <summary>
	/// The end of an alignment. Usually the Bottom or Right.
	/// Maps to: <c>Android.Widget.RelativeLayout.AlignParentRight</c> / <c>Android.Widget.RelativeLayout.AlignParentBottom</c> / UIKit frame positioning
	/// </summary>
	End = 2,
	/// <summary>
	/// Fill the entire area if possible.
	/// Maps to: <c>Android.Views.ViewGroup.LayoutParams.MatchParent</c> / UIKit full frame size
	/// </summary>
	Fill = 3
}
