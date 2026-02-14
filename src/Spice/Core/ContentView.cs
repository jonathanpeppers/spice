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
			if (_content == value)
				return;

			var oldContent = _content;
			_content = value;

			OnPropertyChanged(nameof(Content));
			OnContentChanged(oldContent, value);
		}
	}

	partial void OnContentChanged(View? oldContent, View? newContent);
}
