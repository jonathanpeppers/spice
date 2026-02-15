namespace Spice;

public partial class SearchBar
{
	/// <summary>
	/// Returns searchBar.NativeView
	/// </summary>
	/// <param name="searchBar">The Spice.SearchBar</param>
	public static implicit operator UISearchBar(SearchBar searchBar) => searchBar.NativeView;

	/// <summary>
	/// Control for search input with a search button.
	/// Android -> Android.Widget.SearchView
	/// iOS -> UIKit.UISearchBar
	/// </summary>
	public SearchBar() : base(_ => new UISearchBar { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public SearchBar(CGRect frame) : base(_ => new UISearchBar(frame) { AutoresizingMask = UIViewAutoresizing.None }) { }

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected SearchBar(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UISearchBar
	/// </summary>
	public new UISearchBar NativeView => (UISearchBar)_nativeView.Value;

	partial void OnTextChanged(string value) => NativeView.Text = value;

	partial void OnPlaceholderChanged(string? value) => NativeView.Placeholder = value;

	partial void OnTextColorChanged(Color? value)
	{
		var textField = NativeView.SearchTextField;
		if (textField != null)
			textField.TextColor = value.ToUIColor();
	}

	partial void OnPlaceholderColorChanged(Color? value)
	{
		var textField = NativeView.SearchTextField;
		if (textField != null)
		{
			var placeholderText = textField.Placeholder ?? "";
			if (value != null)
			{
				var attributes = new UIStringAttributes { ForegroundColor = value.ToUIColor() };
				textField.AttributedPlaceholder = new NSAttributedString(placeholderText, attributes);
			}
			else
			{
				// Reset to default
				textField.AttributedPlaceholder = new NSAttributedString(placeholderText);
			}
		}
	}

	SearchBarDelegate? _delegate;

	void UpdateDelegate()
	{
		if (SearchButtonPressed == null && TextChanged == null)
		{
			NativeView.Delegate = null;
			_delegate = null;
		}
		else
		{
			if (_delegate == null)
			{
				_delegate = new SearchBarDelegate(this);
				NativeView.Delegate = _delegate;
			}
		}
	}

	partial void OnSearchButtonPressedChanged(Action<SearchBar>? value) => UpdateDelegate();

	partial void OnTextChangedChanged(Action<SearchBar>? value) => UpdateDelegate();

	class SearchBarDelegate : UISearchBarDelegate
	{
		readonly WeakReference<SearchBar> _searchBar;

		public SearchBarDelegate(SearchBar searchBar)
		{
			_searchBar = new WeakReference<SearchBar>(searchBar);
		}

		public override void SearchButtonClicked(UISearchBar uiSearchBar)
		{
			if (_searchBar.TryGetTarget(out var searchBar))
			{
				searchBar.Text = uiSearchBar.Text ?? "";
				searchBar.SearchButtonPressed?.Invoke(searchBar);
			}
		}

		public override void TextChanged(UISearchBar uiSearchBar, string searchText)
		{
			if (_searchBar.TryGetTarget(out var searchBar))
			{
				searchBar.Text = searchText;
				searchBar.TextChanged?.Invoke(searchBar);
			}
		}
	}
}
