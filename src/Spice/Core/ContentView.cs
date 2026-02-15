namespace Spice;

/// <summary>
/// A container view that holds a single child view via the Content property.
/// Used for creating reusable custom controls and encapsulating UI components.
/// Android -> Android.Views.ViewGroup (FrameLayout)
/// iOS -> UIKit.UIView
/// </summary>
public partial class ContentView : View
{
	View? _content;

	/// <summary>
	/// Gets or sets the content (child view) of this ContentView.
	/// </summary>
	public View? Content
	{
		get => _content;
		set
		{
			var oldContent = _content;

			if (SetProperty(ref _content, value))
			{
				OnContentChanged(oldContent, value);
			}
		}
	}

	/// <summary>
	/// Gets or sets the padding inside the ContentView in device-independent units (uniform padding on all sides).
	/// Platform implementations: Android.Views.ViewGroup.SetPadding / UIKit.UIView frame adjustments
	/// </summary>
	[ObservableProperty]
	double _padding;

	partial void OnContentChanged(View? oldContent, View? newContent);
}
