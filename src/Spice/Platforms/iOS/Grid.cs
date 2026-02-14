using System.Collections.Specialized;

namespace Spice;

public partial class Grid
{
	/// <summary>
	/// Returns grid.NativeView
	/// </summary>
	/// <param name="grid">The Spice.Grid</param>
	public static implicit operator UIView(Grid grid) => grid.NativeView;

	/// <summary>
	/// A layout container that arranges child views in rows and columns.
	/// iOS -> Custom constraint-based layout
	/// </summary>
	public Grid() : this(v => new SpiceGridView((Grid)v) { AutoresizingMask = UIViewAutoresizing.None })
	{
		RowDefinitions.CollectionChanged += OnDefinitionsChanged;
		ColumnDefinitions.CollectionChanged += OnDefinitionsChanged;
	}

	/// <inheritdoc />
	/// <param name="frame">Pass the underlying view a frame</param>
	public Grid(CGRect frame) : this(v => new SpiceGridView((Grid)v, frame) { AutoresizingMask = UIViewAutoresizing.None })
	{
		RowDefinitions.CollectionChanged += OnDefinitionsChanged;
		ColumnDefinitions.CollectionChanged += OnDefinitionsChanged;
	}

	/// <inheritdoc />
	/// <param name="creator">Subclasses can pass in a Func to create a UIView</param>
	protected Grid(Func<View, UIView> creator) : base(creator) { }

	/// <summary>
	/// The underlying UIView
	/// </summary>
	public new SpiceGridView NativeView => (SpiceGridView)_nativeView.Value;

	partial void OnRowSpacingChanged(double value) => NativeView.InvalidateLayout();

	partial void OnColumnSpacingChanged(double value) => NativeView.InvalidateLayout();

	void OnDefinitionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		NativeView.InvalidateLayout();
	}

	/// <inheritdoc />
	protected override void AddSubview(View view)
	{
		NativeView.AddSubview(view);
		NativeView.InvalidateLayout();
	}

	/// <summary>
	/// Custom UIView for Grid layout using manual layout
	/// </summary>
	public class SpiceGridView : UIView
	{
		readonly Grid _parent;

		public SpiceGridView(Grid parent) => _parent = parent;

		public SpiceGridView(Grid parent, CGRect frame) : base(frame) => _parent = parent;

		public void InvalidateLayout()
		{
			SetNeedsLayout();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			if (Subviews.Length == 0)
				return;

			// Calculate grid dimensions
			int rowCount = Math.Max(1, _parent.RowDefinitions.Count);
			int colCount = Math.Max(1, _parent.ColumnDefinitions.Count);

			// Ensure we have enough rows/columns for all children
			foreach (UIView subview in Subviews)
			{
				var spiceView = _parent.Children.FirstOrDefault(v => ((UIView)v).Handle == subview.Handle);
				if (spiceView == null)
					continue;

				int row = GetRow(spiceView);
				int col = GetColumn(spiceView);
				int rowSpan = GetRowSpan(spiceView);
				int colSpan = GetColumnSpan(spiceView);

				rowCount = Math.Max(rowCount, row + rowSpan);
				colCount = Math.Max(colCount, col + colSpan);
			}

			// Calculate row heights and column widths
			var rowHeights = new nfloat[rowCount];
			var colWidths = new nfloat[colCount];
			var rowPositions = new nfloat[rowCount];
			var colPositions = new nfloat[colCount];

			// Calculate absolute and auto sizes first
			nfloat totalStarRowWeight = 0;
			nfloat totalStarColWeight = 0;
			nfloat usedHeight = 0;
			nfloat usedWidth = 0;

			for (int i = 0; i < rowCount; i++)
			{
				var rowDef = i < _parent.RowDefinitions.Count ? _parent.RowDefinitions[i] : new RowDefinition();
				if (rowDef.Height.IsAbsolute)
				{
					rowHeights[i] = (nfloat)rowDef.Height.Value;
					usedHeight += rowHeights[i];
				}
				else if (rowDef.Height.IsStar)
				{
					totalStarRowWeight += (nfloat)rowDef.Height.Value;
				}
				// Auto sizing would require measuring children - simplified for now
			}

			for (int i = 0; i < colCount; i++)
			{
				var colDef = i < _parent.ColumnDefinitions.Count ? _parent.ColumnDefinitions[i] : new ColumnDefinition();
				if (colDef.Width.IsAbsolute)
				{
					colWidths[i] = (nfloat)colDef.Width.Value;
					usedWidth += colWidths[i];
				}
				else if (colDef.Width.IsStar)
				{
					totalStarColWeight += (nfloat)colDef.Width.Value;
				}
			}

			// Add spacing to used dimensions
			usedHeight += (nfloat)_parent.RowSpacing * (rowCount - 1);
			usedWidth += (nfloat)_parent.ColumnSpacing * (colCount - 1);

			// Distribute remaining space to star-sized rows/columns
			nfloat remainingHeight = (nfloat)Math.Max(0, Bounds.Height - usedHeight);
			nfloat remainingWidth = (nfloat)Math.Max(0, Bounds.Width - usedWidth);

			for (int i = 0; i < rowCount; i++)
			{
				var rowDef = i < _parent.RowDefinitions.Count ? _parent.RowDefinitions[i] : new RowDefinition();
				if (rowDef.Height.IsStar && totalStarRowWeight > 0)
				{
					rowHeights[i] = remainingHeight * ((nfloat)rowDef.Height.Value / totalStarRowWeight);
				}
			}

			for (int i = 0; i < colCount; i++)
			{
				var colDef = i < _parent.ColumnDefinitions.Count ? _parent.ColumnDefinitions[i] : new ColumnDefinition();
				if (colDef.Width.IsStar && totalStarColWeight > 0)
				{
					colWidths[i] = remainingWidth * ((nfloat)colDef.Width.Value / totalStarColWeight);
				}
			}

			// Calculate positions
			nfloat currentY = 0;
			for (int i = 0; i < rowCount; i++)
			{
				rowPositions[i] = currentY;
				currentY += rowHeights[i] + (nfloat)_parent.RowSpacing;
			}

			nfloat currentX = 0;
			for (int i = 0; i < colCount; i++)
			{
				colPositions[i] = currentX;
				currentX += colWidths[i] + (nfloat)_parent.ColumnSpacing;
			}

			// Layout children
			foreach (UIView subview in Subviews)
			{
				var spiceView = _parent.Children.FirstOrDefault(v => ((UIView)v).Handle == subview.Handle);
				if (spiceView == null)
					continue;

				int row = GetRow(spiceView);
				int col = GetColumn(spiceView);
				int rowSpan = GetRowSpan(spiceView);
				int colSpan = GetColumnSpan(spiceView);

				if (row >= rowCount || col >= colCount)
					continue;

				nfloat x = colPositions[col];
				nfloat y = rowPositions[row];
				
				nfloat width = 0;
				for (int i = col; i < Math.Min(col + colSpan, colCount); i++)
				{
					width += colWidths[i];
					if (i < col + colSpan - 1)
						width += (nfloat)_parent.ColumnSpacing;
				}

				nfloat height = 0;
				for (int i = row; i < Math.Min(row + rowSpan, rowCount); i++)
				{
					height += rowHeights[i];
					if (i < row + rowSpan - 1)
						height += (nfloat)_parent.RowSpacing;
				}

				subview.Frame = new CGRect(x, y, width, height);
			}
		}
	}
}
