using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

		// Subscribe to initial Items collection
		Items.CollectionChanged += OnItemsCollectionChanged;
	}

	void OnItemSelected(object? sender, AdapterView.ItemSelectedEventArgs e)
	{
		if (e.Position != _selectedIndex)
		{
			SelectedIndex = e.Position;
		}
	}

	void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		RefreshAdapter();
	}

	partial void OnItemsChanged(ObservableCollection<string>? oldValue, ObservableCollection<string> newValue)
	{
		if (oldValue != null)
			oldValue.CollectionChanged -= OnItemsCollectionChanged;
		newValue.CollectionChanged += OnItemsCollectionChanged;

		RefreshAdapter();
	}

	void RefreshAdapter()
	{
		var adapter = (ArrayAdapter<string>?)NativeView.Adapter;
		if (adapter != null)
		{
			adapter.Clear();
			foreach (var item in Items)
				adapter.Add(item);
			adapter.NotifyDataSetChanged();

			// Clamp selected index: Spinner always has a selection when items exist
			if (Items.Count > 0 && (_selectedIndex < 0 || _selectedIndex >= Items.Count))
			{
				SelectedIndex = 0;
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
