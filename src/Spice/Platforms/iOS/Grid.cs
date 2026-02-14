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
		view.NativeView.TranslatesAutoresizingMaskIntoConstraints = false;
		NativeView.InvalidateLayout();
	}

	/// <summary>
	/// Custom UIView for Grid layout using constraints
	/// </summary>
	public class SpiceGridView : UIView
	{
		readonly Grid _parent;
		readonly List<NSLayoutConstraint> _constraints = new();

		public SpiceGridView(Grid parent) => _parent = parent;

		public SpiceGridView(Grid parent, CGRect frame) : base(frame) => _parent = parent;

		public void InvalidateLayout()
		{
			// Remove old constraints
			if (_constraints.Count > 0)
			{
				NSLayoutConstraint.DeactivateConstraints(_constraints.ToArray());
				_constraints.Clear();
			}

			if (Subviews.Length == 0)
				return;

			// Calculate grid dimensions
			int rowCount = Math.Max(1, _parent.RowDefinitions.Count);
			int colCount = Math.Max(1, _parent.ColumnDefinitions.Count);

			// Ensure we have enough rows/columns for all children
			foreach (var subview in Subviews)
			{
				if (subview is not UIView)
					continue;

				var spiceView = _parent.Children.FirstOrDefault(v => ReferenceEquals(((UIView)v).Handle, subview.Handle));
				if (spiceView == null)
					continue;

				int row = GetRow(spiceView);
				int col = GetColumn(spiceView);
				int rowSpan = GetRowSpan(spiceView);
				int colSpan = GetColumnSpan(spiceView);

				rowCount = Math.Max(rowCount, row + rowSpan);
				colCount = Math.Max(colCount, col + colSpan);
			}

			// Create guide views for each cell boundary
			var rowGuides = new UILayoutGuide[rowCount + 1];
			var colGuides = new UILayoutGuide[colCount + 1];

			for (int i = 0; i <= rowCount; i++)
			{
				rowGuides[i] = new UILayoutGuide();
				AddLayoutGuide(rowGuides[i]);
			}

			for (int i = 0; i <= colCount; i++)
			{
				colGuides[i] = new UILayoutGuide();
				AddLayoutGuide(colGuides[i]);
			}

			// Position row guides
			_constraints.Add(rowGuides[0].TopAnchor.ConstraintEqualTo(TopAnchor));
			for (int i = 0; i < rowCount; i++)
			{
				var rowDef = i < _parent.RowDefinitions.Count ? _parent.RowDefinitions[i] : new RowDefinition();
				var topGuide = rowGuides[i];
				var bottomGuide = rowGuides[i + 1];

				// Add spacing
				if (i > 0)
				{
					_constraints.Add(topGuide.TopAnchor.ConstraintEqualTo(rowGuides[i].TopAnchor, (nfloat)_parent.RowSpacing));
				}

				// Height constraint based on GridLength
				if (rowDef.Height.IsAbsolute)
				{
					_constraints.Add(bottomGuide.TopAnchor.ConstraintEqualTo(topGuide.TopAnchor, (nfloat)rowDef.Height.Value));
				}
				else if (rowDef.Height.IsAuto)
				{
					// Auto sizing - will be determined by content
					_constraints.Add(bottomGuide.TopAnchor.ConstraintGreaterThanOrEqualTo(topGuide.TopAnchor));
				}
				else // Star
				{
					// Star sizing - proportional
					// We'll handle this by making all star rows equal and then multiplying
					_constraints.Add(bottomGuide.TopAnchor.ConstraintGreaterThanOrEqualTo(topGuide.TopAnchor));
				}

				// Zero height for guides
				_constraints.Add(topGuide.HeightAnchor.ConstraintEqualTo(0));
				_constraints.Add(bottomGuide.HeightAnchor.ConstraintEqualTo(0));
			}
			_constraints.Add(rowGuides[rowCount].TopAnchor.ConstraintEqualTo(BottomAnchor));

			// Position column guides
			_constraints.Add(colGuides[0].LeadingAnchor.ConstraintEqualTo(LeadingAnchor));
			for (int i = 0; i < colCount; i++)
			{
				var colDef = i < _parent.ColumnDefinitions.Count ? _parent.ColumnDefinitions[i] : new ColumnDefinition();
				var leftGuide = colGuides[i];
				var rightGuide = colGuides[i + 1];

				// Add spacing
				if (i > 0)
				{
					_constraints.Add(leftGuide.LeadingAnchor.ConstraintEqualTo(colGuides[i].LeadingAnchor, (nfloat)_parent.ColumnSpacing));
				}

				// Width constraint based on GridLength
				if (colDef.Width.IsAbsolute)
				{
					_constraints.Add(rightGuide.LeadingAnchor.ConstraintEqualTo(leftGuide.LeadingAnchor, (nfloat)colDef.Width.Value));
				}
				else if (colDef.Width.IsAuto)
				{
					// Auto sizing - will be determined by content
					_constraints.Add(rightGuide.LeadingAnchor.ConstraintGreaterThanOrEqualTo(leftGuide.LeadingAnchor));
				}
				else // Star
				{
					// Star sizing - proportional
					_constraints.Add(rightGuide.LeadingAnchor.ConstraintGreaterThanOrEqualTo(leftGuide.LeadingAnchor));
				}

				// Zero width for guides
				_constraints.Add(leftGuide.WidthAnchor.ConstraintEqualTo(0));
				_constraints.Add(rightGuide.WidthAnchor.ConstraintEqualTo(0));
			}
			_constraints.Add(colGuides[colCount].LeadingAnchor.ConstraintEqualTo(TrailingAnchor));

			// Handle star sizing by making all star rows/columns proportional
			var starRows = new List<int>();
			var starCols = new List<int>();

			for (int i = 0; i < rowCount; i++)
			{
				var rowDef = i < _parent.RowDefinitions.Count ? _parent.RowDefinitions[i] : new RowDefinition();
				if (rowDef.Height.IsStar)
					starRows.Add(i);
			}

			for (int i = 0; i < colCount; i++)
			{
				var colDef = i < _parent.ColumnDefinitions.Count ? _parent.ColumnDefinitions[i] : new ColumnDefinition();
				if (colDef.Width.IsStar)
					starCols.Add(i);
			}

			// Make star rows proportional
			if (starRows.Count > 0)
			{
				var firstStarRow = starRows[0];
				var firstRowDef = firstStarRow < _parent.RowDefinitions.Count ? _parent.RowDefinitions[firstStarRow] : new RowDefinition();
				var firstRowHeight = rowGuides[firstStarRow + 1].TopAnchor.ConstraintEqualTo(rowGuides[firstStarRow].TopAnchor);

				foreach (var starRow in starRows.Skip(1))
				{
					var rowDef = starRow < _parent.RowDefinitions.Count ? _parent.RowDefinitions[starRow] : new RowDefinition();
					var multiplier = (nfloat)(rowDef.Height.Value / firstRowDef.Height.Value);
					var constraint = NSLayoutConstraint.Create(
						rowGuides[starRow + 1].TopAnchor,
						NSLayoutRelation.Equal,
						rowGuides[starRow].TopAnchor,
						1,
						0);
					_constraints.Add(constraint);
				}
			}

			// Make star columns proportional
			if (starCols.Count > 0)
			{
				var firstStarCol = starCols[0];
				var firstColDef = firstStarCol < _parent.ColumnDefinitions.Count ? _parent.ColumnDefinitions[firstStarCol] : new ColumnDefinition();

				foreach (var starCol in starCols.Skip(1))
				{
					var colDef = starCol < _parent.ColumnDefinitions.Count ? _parent.ColumnDefinitions[starCol] : new ColumnDefinition();
					var multiplier = (nfloat)(colDef.Width.Value / firstColDef.Width.Value);
					var constraint = NSLayoutConstraint.Create(
						colGuides[starCol + 1].LeadingAnchor,
						NSLayoutRelation.Equal,
						colGuides[starCol].LeadingAnchor,
						1,
						0);
					_constraints.Add(constraint);
				}
			}

			// Position child views within cells
			foreach (var subview in Subviews)
			{
				if (subview is not UIView uiView)
					continue;

				var spiceView = _parent.Children.FirstOrDefault(v => ReferenceEquals(((UIView)v).Handle, subview.Handle));
				if (spiceView == null)
					continue;

				int row = GetRow(spiceView);
				int col = GetColumn(spiceView);
				int rowSpan = GetRowSpan(spiceView);
				int colSpan = GetColumnSpan(spiceView);

				// Ensure indices are within bounds
				if (row >= rowCount || col >= colCount)
					continue;

				// Constrain to cell boundaries
				_constraints.Add(uiView.TopAnchor.ConstraintEqualTo(rowGuides[row].TopAnchor));
				_constraints.Add(uiView.BottomAnchor.ConstraintEqualTo(rowGuides[Math.Min(row + rowSpan, rowCount)].TopAnchor));
				_constraints.Add(uiView.LeadingAnchor.ConstraintEqualTo(colGuides[col].LeadingAnchor));
				_constraints.Add(uiView.TrailingAnchor.ConstraintEqualTo(colGuides[Math.Min(col + colSpan, colCount)].LeadingAnchor));
			}

			// Activate all constraints
			NSLayoutConstraint.ActivateConstraints(_constraints.ToArray());
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			
			// Ensure layout is set up
			if (_constraints.Count == 0)
			{
				InvalidateLayout();
			}
		}
	}
}
