using System.Collections.ObjectModel;

namespace Spice;

/// <summary>
/// A layout container that arranges child views in rows and columns.
/// Android -> Android.Widget.GridLayout
/// iOS -> Custom constraint-based layout
/// 
/// Note: Auto-sized rows and columns are not fully supported and will behave as star-sized with weight 1.
/// </summary>
public partial class Grid : View
{
	/// <summary>
	/// Gets or sets the row definitions collection.
	/// </summary>
	public ObservableCollection<RowDefinition> RowDefinitions { get; } = new();

	/// <summary>
	/// Gets or sets the column definitions collection.
	/// </summary>
	public ObservableCollection<ColumnDefinition> ColumnDefinitions { get; } = new();

	/// <summary>
	/// Gets or sets the spacing between rows.
	/// </summary>
	[ObservableProperty]
	double _rowSpacing = 0;

	/// <summary>
	/// Gets or sets the spacing between columns.
	/// </summary>
	[ObservableProperty]
	double _columnSpacing = 0;

	/// <summary>
	/// Gets or sets the padding inside the Grid in device-independent units (uniform padding on all sides).
	/// Platform implementations: Android.Views.ViewGroup.SetPadding / iOS manual layout adjustments
	/// </summary>
	[ObservableProperty]
	double _padding;

	// Attached properties for child views (thread-safe)
	static readonly System.Collections.Concurrent.ConcurrentDictionary<View, int> _rows = new();
	static readonly System.Collections.Concurrent.ConcurrentDictionary<View, int> _columns = new();
	static readonly System.Collections.Concurrent.ConcurrentDictionary<View, int> _rowSpans = new();
	static readonly System.Collections.Concurrent.ConcurrentDictionary<View, int> _columnSpans = new();

	/// <summary>
	/// Gets the row position of the specified view.
	/// </summary>
	public static int GetRow(View view) => _rows.TryGetValue(view, out var row) ? row : 0;

	/// <summary>
	/// Sets the row position of the specified view.
	/// </summary>
	public static void SetRow(View view, int row)
	{
		if (row < 0)
			throw new ArgumentOutOfRangeException(nameof(row), "Row must be non-negative");
		_rows[view] = row;
	}

	/// <summary>
	/// Gets the column position of the specified view.
	/// </summary>
	public static int GetColumn(View view) => _columns.TryGetValue(view, out var column) ? column : 0;

	/// <summary>
	/// Sets the column position of the specified view.
	/// </summary>
	public static void SetColumn(View view, int column)
	{
		if (column < 0)
			throw new ArgumentOutOfRangeException(nameof(column), "Column must be non-negative");
		_columns[view] = column;
	}

	/// <summary>
	/// Gets the row span of the specified view.
	/// </summary>
	public static int GetRowSpan(View view) => _rowSpans.TryGetValue(view, out var span) ? span : 1;

	/// <summary>
	/// Sets the row span of the specified view.
	/// </summary>
	public static void SetRowSpan(View view, int span)
	{
		if (span < 1)
			throw new ArgumentOutOfRangeException(nameof(span), "RowSpan must be at least 1");
		_rowSpans[view] = span;
	}

	/// <summary>
	/// Gets the column span of the specified view.
	/// </summary>
	public static int GetColumnSpan(View view) => _columnSpans.TryGetValue(view, out var span) ? span : 1;

	/// <summary>
	/// Sets the column span of the specified view.
	/// </summary>
	public static void SetColumnSpan(View view, int span)
	{
		if (span < 1)
			throw new ArgumentOutOfRangeException(nameof(span), "ColumnSpan must be at least 1");
		_columnSpans[view] = span;
	}
}
