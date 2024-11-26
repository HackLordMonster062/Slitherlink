using Slitherlink.Models;

namespace Slitherlink {
	class SquareGrid {
		const int Top = 0;
		const int Right = 1;
		const int Bottom = 2;
		const int Left = 3;

		public int Width { get; private set; }
		public int Height { get; private set; }

		Cell _gridRoot;

		public SquareGrid(int width, int height) {
			Width = width;
			Height = height;

			_gridRoot = new Cell(
				[]
			);

			Initialize();
		}

		void Initialize() {
			InitCell(_gridRoot, null, null, 0, 0);
		}

		void InitCell(Cell cell, Cell? cellAbove, Cell? cellOnLeft, int depthW, int depthH) {
			Random random = new Random();
			int value = random.Next(-5, 3);

			if (value >= 0) cell.Value = value;

			// Edge + cell above
			if (cellAbove == null) {
				cell.Edges[Top] = (new Edge(), null);
			} else {
				cell.Edges[Top] = (cellAbove.Edges[Bottom].Item1, cellAbove);
			}

			// Edge + cell on the left
			if (cellOnLeft == null) {
				cell.Edges[Left] = (new Edge(), null);
			} else {
				cell.Edges[Left] = (cellOnLeft.Edges[Right].Item1, cellOnLeft);
			}

			// Edge + cell on the right
			if (depthW < Width - 1) {
				cell.Edges[Right] = (new Edge(), new Cell([]));
				InitCell(cell.Edges[Right].Item2!, cellAbove?.Edges[Right].Item2, cell, depthW + 1, depthH);
			} else {
				cell.Edges[Right] = (new Edge(), null);
			}

			// Edge + cell below
			if (depthH < Height - 1) {
				cell.Edges[Bottom] = (new Edge(), new Cell([]));
				InitCell(cell.Edges[Bottom].Item2!, cell, null, depthW, depthH + 1);
			} else {
				cell.Edges[Bottom] = (new Edge(), null);
			}
		}

		public Cell GetCell(int x, int y) {
			Cell cell = _gridRoot;

			for (int i = 0; i < x; i++) {
				cell = cell.Edges[Right].Item2!;
			}

			for (int j = 0; j < y; j++) {
				cell = cell.Edges[Bottom].Item2!;
			}

			return cell;
		}
	}
}
