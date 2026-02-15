namespace Spice;

public partial class Editor
{
	/// <summary>
	/// Returns editor.NativeView
	/// </summary>
	/// <param name="editor">The Spice.Editor</param>
	public static implicit operator UITextView(Editor editor) => editor.NativeView;

	/// <summary>
	/// Control for multi-line text input.
	/// Android -> Android.Widget.EditText (multiline)
	/// iOS -> UIKit.UITextView
	/// </summary>
	public Editor() : base(_ => new UITextView { AutoresizingMask = UIViewAutoresizing.None })
	{
		EnsureDelegate();
	}

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public Editor(CGRect frame) : base(_ => new UITextView(frame) { AutoresizingMask = UIViewAutoresizing.None })
	{
		EnsureDelegate();
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected Editor(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UITextView
	/// </summary>
	public new UITextView NativeView => (UITextView)_nativeView.Value;

	partial void OnTextChanged(string value)
	{
		if (NativeView.Text != value)
			NativeView.Text = value;
	}

	partial void OnTextColorChanged(Color? value) => NativeView.TextColor = value.ToUIColor();

	partial void OnPlaceholderChanged(string? value)
	{
		UpdatePlaceholder();
	}

	partial void OnPlaceholderColorChanged(Color? value)
	{
		UpdatePlaceholder();
	}

	void UpdatePlaceholder()
	{
		// UITextView doesn't have native placeholder support, so we need to handle it ourselves
		// For now, we'll skip the placeholder implementation to keep it minimal
		// A full implementation would require a delegate to show/hide placeholder text
	}

	partial void OnAutoSizeChanged(bool value)
	{
		// Auto-sizing would require observing content size changes
		// For minimal implementation, we'll skip this for now
	}

	EditorDelegate? _delegate;

	void EnsureDelegate()
	{
		if (_delegate == null)
		{
			_delegate = new EditorDelegate(this);
			NativeView.Delegate = _delegate;
		}
	}

	class EditorDelegate : UITextViewDelegate
	{
		readonly WeakReference<Editor> _editor;

		public EditorDelegate(Editor editor)
		{
			_editor = new WeakReference<Editor>(editor);
		}

		public override void Changed(UITextView textView)
		{
			if (_editor.TryGetTarget(out var editor))
			{
				editor.Text = textView.Text ?? "";
			}
		}
	}
}
