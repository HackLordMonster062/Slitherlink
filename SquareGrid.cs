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

			_gridRoot = new Cell([]);

			Initialize();

			GenerateLevel(.8f);
		}

		public SquareGrid(int width, int height, Cell gridRoot) {
			Width = width;
			Height = height;

			_gridRoot = gridRoot;
		}

		public SquareGrid CopyWithEdges() {
			Cell? currCell = _gridRoot;
			Cell? currNewCell = null;

			Cell? rowOrigin = currCell;
			Cell? newRowOrigin = null;

			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					int count = 0;

					foreach ((Edge edge, Cell? neighbor) in currCell!.Edges.Values) {
						if (edge.IsOn) count++;
					}

					currCell.Value = count;

					currCell = currCell.Edges[Right].Item2;
				}

				rowOrigin = rowOrigin!.Edges[Bottom].Item2;
				currCell = rowOrigin;
			}
		}

		void Initialize() {
			Cell[,] grid = new Cell[Width, Height];

			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					Cell newCell = new([]);

					grid[x, y] = newCell;

					if (x > 0) {
						Cell cellAbove = grid[x - 1, y];
						cellAbove.Edges[Right] = (new Edge(), newCell);
						newCell.Edges[Left] = (cellAbove.Edges[Right].Item1, cellAbove);
					} else {
						newCell.Edges[Left] = (new Edge(), null);
					}

					if (y > 0) {
						Cell cellOnLeft = grid[x, y - 1];
						cellOnLeft.Edges[Bottom] = (new Edge(), newCell);
						newCell.Edges[Top] = (cellOnLeft.Edges[Bottom].Item1, cellOnLeft);
					} else {
						newCell.Edges[Top] = (new Edge(), null);
					}

					if (x == Width - 1) {
						newCell.Edges[Right] = (new Edge(), null);
					}

					if (y == Height - 1) {
						newCell.Edges[Bottom] = (new Edge(), null);
					}
				}
			}

			_gridRoot = grid[0, 0];
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

		public void GenerateLevel(float branchingRate) {
			Random rand = new Random();
			Cell currCell = GetCell(rand.Next(Width), rand.Next(Height));

			HashSet<Cell> path = [currCell];

			LevelBranch(path, currCell, branchingRate, rand);

			foreach (Cell cell in path) {
				foreach ((Edge edge, Cell? neighbor) in cell.Edges.Values) {
					if (neighbor == null || !path.Contains(neighbor)) {
						edge.IsOn = true;
					}
				}
			}

			UpdateNumbers();
		}

		void LevelBranch(HashSet<Cell> path, Cell origin, float branchingRate, Random rand) {
			for (int i = 0; i < 50; i++) {
				int dir = rand.Next(4);

				(Edge _, Cell? newCell) = origin.Edges[dir];

				if (newCell == null || path.Contains(newCell) || IsNearPath(newCell, path, origin, dir))
					continue;

				path.Add(newCell);

				origin = newCell;

				if (rand.NextSingle() > branchingRate)
					LevelBranch(path, newCell, branchingRate, rand);
			}
		}

		bool IsNearPath(Cell cell, HashSet<Cell> path, Cell exclude, int direction) {
			foreach ((Edge _, Cell? neighbor) in cell.Edges.Values) {
				if (neighbor != null && neighbor != exclude && path.Contains(neighbor))
					return true;
			}

			Cell? next = cell.Edges[direction].Item2;
			Cell? nextLeft = next?.Edges[(direction + 1) % 4].Item2;
			Cell? nextRight = next?.Edges[(direction + 3) % 4].Item2;

			if (next != null) {
				if (nextLeft != null && path.Contains(nextLeft) || nextRight != null && path.Contains(nextRight))
					return true;
			}

			return false;
		}

		void UpdateNumbers() {
			Cell? currCell = _gridRoot;
			Cell? rowOrigin = currCell;

			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					int count = 0;

					foreach ((Edge edge, Cell? neighbor) in currCell!.Edges.Values) {
						if (edge.IsOn) count++;
					}

					currCell.Value = count;

					currCell = currCell.Edges[Right].Item2;
				}

				rowOrigin = rowOrigin!.Edges[Bottom].Item2;
				currCell = rowOrigin;
			}
		}
	}
}
