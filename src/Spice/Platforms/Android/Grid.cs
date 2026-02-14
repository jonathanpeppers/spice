using System.Collections.Specialized;
using Android.Content;
using Android.Widget;

namespace Spice;

public partial class Grid
{
	/// <summary>
	/// Returns grid.NativeView
	/// </summary>
	/// <param name="grid">The Spice.Grid</param>
	public static implicit operator GridLayout(Grid grid) => grid.NativeView;

	static GridLayout Create(Context context, Grid grid)
	{
		var layout = new GridLayout(context);
		layout.RowCount = Math.Max(1, grid.RowDefinitions.Count);
		layout.ColumnCount = Math.Max(1, grid.ColumnDefinitions.Count);
		return layout;
	}

	/// <summary>
	/// A layout container that arranges child views in rows and columns.
	/// Android -> Android.Widget.GridLayout
	/// </summary>
	public Grid() : this(Platform.Context, (context) => Create(context, null!))
	{
		// Update the layout after construction
		NativeView.RowCount = Math.Max(1, RowDefinitions.Count);
		NativeView.ColumnCount = Math.Max(1, ColumnDefinitions.Count);
		RowDefinitions.CollectionChanged += OnDefinitionsChanged;
		ColumnDefinitions.CollectionChanged += OnDefinitionsChanged;
	}

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	public Grid(Context context) : this(context, (ctx) => Create(ctx, null!))
	{
		NativeView.RowCount = Math.Max(1, RowDefinitions.Count);
		NativeView.ColumnCount = Math.Max(1, ColumnDefinitions.Count);
		RowDefinitions.CollectionChanged += OnDefinitionsChanged;
		ColumnDefinitions.CollectionChanged += OnDefinitionsChanged;
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Grid(Func<Context, Android.Views.View> creator) : this(Platform.Context, creator) { }

	/// <inheritdoc />
	/// <param name="context">Option to pass the desired Context, otherwise Platform.Context is used</param>
	/// <param name="creator">Subclasses can pass in a Func to create a Android.Views.View</param>
	protected Grid(Context context, Func<Context, Android.Views.View> creator) : base(context, creator) { }

	/// <summary>
	/// The underlying Android.Widget.GridLayout
	/// </summary>
	public new GridLayout NativeView => (GridLayout)_nativeView.Value;

	partial void OnRowSpacingChanged(double value)
	{
		// Android GridLayout doesn't have direct spacing properties
		// We need to update the margins on all children
		UpdateChildMargins();
	}

	partial void OnColumnSpacingChanged(double value)
	{
		// Android GridLayout doesn't have direct spacing properties
		// We need to update the margins on all children
		UpdateChildMargins();
	}

	void OnDefinitionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		NativeView.RowCount = Math.Max(1, RowDefinitions.Count);
		NativeView.ColumnCount = Math.Max(1, ColumnDefinitions.Count);
		UpdateLayout();
	}

	/// <inheritdoc />
	protected override void AddSubview(View view)
	{
		var row = GetRow(view);
		var column = GetColumn(view);
		var rowSpan = GetRowSpan(view);
		var columnSpan = GetColumnSpan(view);

		var layoutParams = CreateGridLayoutParams(row, column, rowSpan, columnSpan);
		view.NativeView.LayoutParameters = layoutParams;
		NativeView.AddView(view.NativeView);
	}

	GridLayout.LayoutParams CreateGridLayoutParams(int row, int column, int rowSpan, int columnSpan)
	{
		var rowSpec = GridLayout.InvokeSpec(row, rowSpan, GridLayout.Fill);
		var columnSpec = GridLayout.InvokeSpec(column, columnSpan, GridLayout.Fill);
		var layoutParams = new GridLayout.LayoutParams(rowSpec, columnSpec);

		// Apply row/column definitions sizing
		if (row < RowDefinitions.Count)
		{
			var rowDef = RowDefinitions[row];
			if (rowDef.Height.IsAbsolute)
			{
				layoutParams.Height = (int)rowDef.Height.Value;
			}
			else if (rowDef.Height.IsAuto)
			{
				layoutParams.Height = Android.Views.ViewGroup.LayoutParams.WrapContent;
			}
			else // Star
			{
				layoutParams.Height = 0;
				layoutParams.RowSpec = GridLayout.InvokeSpec(row, rowSpan, GridLayout.Fill, (float)rowDef.Height.Value);
			}
		}
		else
		{
			layoutParams.Height = 0;
			layoutParams.RowSpec = GridLayout.InvokeSpec(row, rowSpan, GridLayout.Fill, 1f);
		}

		if (column < ColumnDefinitions.Count)
		{
			var colDef = ColumnDefinitions[column];
			if (colDef.Width.IsAbsolute)
			{
				layoutParams.Width = (int)colDef.Width.Value;
			}
			else if (colDef.Width.IsAuto)
			{
				layoutParams.Width = Android.Views.ViewGroup.LayoutParams.WrapContent;
			}
			else // Star
			{
				layoutParams.Width = 0;
				layoutParams.ColumnSpec = GridLayout.InvokeSpec(column, columnSpan, GridLayout.Fill, (float)colDef.Width.Value);
			}
		}
		else
		{
			layoutParams.Width = 0;
			layoutParams.ColumnSpec = GridLayout.InvokeSpec(column, columnSpan, GridLayout.Fill, 1f);
		}

		// Apply spacing as margins
		if (RowSpacing > 0 || ColumnSpacing > 0)
		{
			var left = column > 0 ? (int)(ColumnSpacing / 2) : 0;
			var right = column < ColumnDefinitions.Count - 1 ? (int)(ColumnSpacing / 2) : 0;
			var top = row > 0 ? (int)(RowSpacing / 2) : 0;
			var bottom = row < RowDefinitions.Count - 1 ? (int)(RowSpacing / 2) : 0;
			layoutParams.SetMargins(left, top, right, bottom);
		}

		return layoutParams;
	}

	void UpdateChildMargins()
	{
		// Update margins on all existing children
		for (int i = 0; i < NativeView.ChildCount; i++)
		{
			var child = NativeView.GetChildAt(i);
			if (child?.LayoutParameters is GridLayout.LayoutParams layoutParams)
			{
				var spiceView = Children.FirstOrDefault(v => ReferenceEquals(v.NativeView, child));
				if (spiceView != null)
				{
					var row = GetRow(spiceView);
					var column = GetColumn(spiceView);

					var left = column > 0 ? (int)(ColumnSpacing / 2) : 0;
					var right = column < ColumnDefinitions.Count - 1 ? (int)(ColumnSpacing / 2) : 0;
					var top = row > 0 ? (int)(RowSpacing / 2) : 0;
					var bottom = row < RowDefinitions.Count - 1 ? (int)(RowSpacing / 2) : 0;
					layoutParams.SetMargins(left, top, right, bottom);
				}
			}
		}
		NativeView.RequestLayout();
	}

	void UpdateLayout()
	{
		// Remove all views and re-add with updated parameters
		var children = Children.ToList();
		NativeView.RemoveAllViews();
		foreach (var child in children)
		{
			AddSubview(child);
		}
	}
}
