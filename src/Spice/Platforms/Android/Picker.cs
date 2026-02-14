using Android.Content;
using Android.Widget;

namespace Spice;

public partial class Picker
{
	/// <summary>
	/// Returns picker.NativeView
	/// </summary>
	/// <param name="picker">The Spice.Picker</param>
	public static implicit operator Spinner(Picker picker) => picker.NativeView;

	static Spinner Create(Context context) => new(context);

	/// <summary>
	/// Picker control for selecting an item from a list.
	/// Android -> Android.Widget.Spinner
	/// iOS -> UIKit.UIPickerView
	/// </summary>
	public Picker() : base(Platform.Context, Create)
	{
		SetupAdapter();
	}

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public Picker(Context context) : base(context, Create)
	{
		SetupAdapter();
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Picker(Func<Context, Android.Views.View> creator) : base(Platform.Context, creator)
	{
		SetupAdapter();
	}

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Picker(Context context, Func<Context, Android.Views.View> creator) : base(context, creator)
	{
		SetupAdapter();
	}

	/// <summary>
	/// The underlying Android.Widget.Spinner
	/// </summary>
	public new Spinner NativeView => (Spinner)_nativeView.Value;

	void SetupAdapter()
	{
		var adapter = new ArrayAdapter<string>(NativeView.Context!, Android.Resource.Layout.SimpleSpinnerItem);
		adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
		NativeView.Adapter = adapter;

		NativeView.ItemSelected += OnItemSelected;
	}

	void OnItemSelected(object? sender, AdapterView.ItemSelectedEventArgs e)
	{
		if (e.Position != _selectedIndex)
		{
			SelectedIndex = e.Position;
		}
	}

	partial void OnItemsChanged(ObservableCollection<string> value)
	{
		var adapter = (ArrayAdapter<string>?)NativeView.Adapter;
		if (adapter != null)
		{
			adapter.Clear();
			adapter.AddAll(value.ToArray());
			adapter.NotifyDataSetChanged();

			// If selected index is out of range, reset it
			if (_selectedIndex >= value.Count)
			{
				SelectedIndex = -1;
			}
		}
	}

	partial void OnSelectedIndexChanged(int value)
	{
		if (value >= 0 && value < Items.Count && NativeView.SelectedItemPosition != value)
		{
			NativeView.SetSelection(value);
		}
	}

	partial void OnTitleChanged(string value)
	{
		NativeView.Prompt = value;
	}

	partial void OnTextColorChanged(Color? value)
	{
		// Text color for Spinner is typically controlled via theme or adapter
		// For simplicity, we'll skip custom text color on Android Spinner
	}
}
