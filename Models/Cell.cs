namespace Slitherlink.Models {
	public class Cell(
		Dictionary<int, (Edge, Cell?)> edges,
		int? value = null
	) {
		public Dictionary<int, (Edge, Cell?)> Edges { get; private set; } = edges;
		public int? Value { get; set; } = value;
	}
}
