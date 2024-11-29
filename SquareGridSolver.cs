using Slitherlink.Models;
using static SFML.Window.Mouse;

namespace Slitherlink {
	static class SquareGridSolver {
		public static int CountSolutions(SquareGrid grid) {
			
		}

		static bool CheckPath(SquareGrid grid) {
			HashSet<Cell> traversed = [];
			Stack<Cell> stack = [];
			stack.Push(grid.GetCell(0, 0));

			while (stack.Count > 0) { 
				var cell = stack.Pop();

				if (!traversed.Contains(cell)) {
					traversed.Add(cell);

					int count = 0;

					foreach (var (edge, neighbor) in cell.Edges.Values) {
						if (edge.IsOn) count++;

						if (neighbor != null) stack.Push(neighbor);
					}

					if (count != cell.Value) return false;
				}
			}

			return true;
		}
	}
}
