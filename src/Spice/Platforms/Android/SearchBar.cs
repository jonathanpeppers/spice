using Android.Content;

namespace Spice;

public partial class SearchBar
{
	/// <summary>
	/// Returns searchBar.NativeView
	/// </summary>
	/// <param name="searchBar">The Spice.SearchBar</param>
	public static implicit operator SearchView(SearchBar searchBar) => searchBar.NativeView;

	static SearchView Create(Context context) => new(context) { Iconified = false };

	/// <summary>
	/// Control for search input with a search button.
	/// Android -> Android.Widget.SearchView
	/// iOS -> UIKit.UISearchBar
	/// </summary>
	public SearchBar() : base(Platform.Context, Create) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public SearchBar(Context context) : base(context, Create) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected SearchBar(Func<Context, Android.Views.View> creator) : base(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected SearchBar(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Widget.SearchView
	/// </summary>
	public new SearchView NativeView => (SearchView)_nativeView.Value;

	partial void OnTextChanged(string value)
	{
		if (NativeView.Query != value)
			NativeView.SetQuery(value, false);
	}

	partial void OnPlaceholderChanged(string? value) => NativeView.SetQueryHint(value);

	partial void OnTextColorChanged(Color? value)
	{
		// SearchView text color is set via the internal EditText
		var editTextId = NativeView.Context?.Resources?.GetIdentifier("android:id/search_src_text", null, null) ?? 0;
		if (editTextId != 0)
		{
			var editText = NativeView.FindViewById<Android.Widget.EditText>(editTextId);
			if (editText != null)
			{
				if (value != null)
				{
					int color = value.ToAndroidInt();
					editText.SetTextColor(Interop.GetEditTextColorStateList(color, color));
				}
				else
				{
					editText.SetTextColor(null);
				}
			}
		}
	}

	partial void OnPlaceholderColorChanged(Color? value)
	{
		// SearchView hint color is set via the internal EditText
		var editTextId = NativeView.Context?.Resources?.GetIdentifier("android:id/search_src_text", null, null) ?? 0;
		if (editTextId != 0)
		{
			var editText = NativeView.FindViewById<Android.Widget.EditText>(editTextId);
			if (editText != null && value != null)
			{
				editText.SetHintTextColor(value.ToAndroidInt());
			}
		}
	}

	SearchView.IOnQueryTextListener? _queryTextListener;

	void UpdateQueryTextListener()
	{
		if (SearchButtonPressed == null && TextChanged == null)
		{
			NativeView.SetOnQueryTextListener(null);
			_queryTextListener = null;
		}
		else
		{
			if (_queryTextListener == null)
			{
				_queryTextListener = new QueryTextListener(this);
				NativeView.SetOnQueryTextListener(_queryTextListener);
			}
		}
	}

	partial void OnSearchButtonPressedChanged(Action<SearchBar>? value) => UpdateQueryTextListener();

	partial void OnTextChangedChanged(Action<SearchBar>? value) => UpdateQueryTextListener();

	class QueryTextListener : Java.Lang.Object, SearchView.IOnQueryTextListener
	{
		readonly SearchBar _searchBar;

		public QueryTextListener(SearchBar searchBar)
		{
			_searchBar = searchBar;
		}

		public bool OnQueryTextSubmit(string? query)
		{
			_searchBar.Text = query ?? "";
			_searchBar.SearchButtonPressed?.Invoke(_searchBar);
			return true;
		}

		public bool OnQueryTextChange(string? newText)
		{
			_searchBar.Text = newText ?? "";
			_searchBar.TextChanged?.Invoke(_searchBar);
			return true;
		}
	}
}
