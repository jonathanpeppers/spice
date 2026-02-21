using Android.Content;
using Android.Text;

namespace Spice;

public partial class Editor
{
	/// <summary>
	/// Returns editor.NativeView
	/// </summary>
	/// <param name="editor">The Spice.Editor</param>
	public static implicit operator EditText(Editor editor) => editor.NativeView;

	static EditText Create(Context context)
	{
		var editText = new EditText(context);
		editText.SetSingleLine(false);
		editText.InputType = InputTypes.ClassText | InputTypes.TextFlagMultiLine;
		return editText;
	}

	/// <summary>
	/// Control for multi-line text input.
	/// Android -> Android.Widget.EditText (multiline)
	/// iOS -> UIKit.UITextView
	/// </summary>
	public Editor() : base(Platform.Context, Create)
	{
		EnsureTextWatcher();
	}

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public Editor(Context context) : base(context, Create)
	{
		EnsureTextWatcher();
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Editor(Func<Context, Android.Views.View> creator) : base(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Editor(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Widget.EditText
	/// </summary>
	public new EditText NativeView => (EditText)_nativeView.Value;

	partial void OnTextChanged(string value)
	{
		if (NativeView.Text != value)
			NativeView.Text = value;
	}

	partial void OnTextColorChanged(Color? value)
	{
		if (value != null)
		{
			int color = value.ToAndroidInt();
			NativeView.SetTextColor(Interop.GetEditTextColorStateList(color, color));
		}
	}

	partial void OnPlaceholderChanged(string? value) => NativeView.Hint = value;

	partial void OnPlaceholderColorChanged(Color? value)
	{
		if (value != null)
		{
			int color = value.ToAndroidInt();
			NativeView.SetHintTextColor(new Android.Content.Res.ColorStateList(
				new[] { Array.Empty<int>() },
				new[] { color }));
		}
	}

	partial void OnAutoSizeChanged(bool value)
	{
		// Auto-sizing would require observing content size changes and adjusting height
		// For minimal implementation, we'll skip this for now
	}

	EditorTextWatcher? _textWatcher;

	void EnsureTextWatcher()
	{
		if (_textWatcher == null)
		{
			_textWatcher = new EditorTextWatcher(this);
			NativeView.AddTextChangedListener(_textWatcher);
		}
	}

	class EditorTextWatcher : Java.Lang.Object, ITextWatcher
	{
		readonly WeakReference<Editor> _editor;

		public EditorTextWatcher(Editor editor)
		{
			_editor = new WeakReference<Editor>(editor);
		}

		public void AfterTextChanged(IEditable? s)
		{
			if (_editor.TryGetTarget(out var editor))
			{
				editor.Text = s?.ToString() ?? "";
			}
		}

		public void BeforeTextChanged(Java.Lang.ICharSequence? s, int start, int count, int after) { }

		public void OnTextChanged(Java.Lang.ICharSequence? s, int start, int before, int count) { }
	}
}
