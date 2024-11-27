using SFML.Graphics;
using SFML.System;
using Slitherlink.Models;

namespace Slitherlink.View {
	class Board {
		const int margin = 10;

		RenderWindow _window;
		SquareGrid _grid;

		int _edgeWidth;
		Font _font;

		public Board(RenderWindow window, Font font, int width, int height, int edgeWidth) {
			_window = window;

			_grid = new SquareGrid(width, height);

			_edgeWidth = edgeWidth;
			_font = font;
		}

		public void Draw() {
			int windowWidth = (int)_window.Size.X - _edgeWidth - margin;
			int windowHeight = (int)_window.Size.Y - _edgeWidth - margin;

			int cellSize;
			Vector2f topLeft;

			cellSize = Math.Min(windowWidth / _grid.Width, windowHeight / _grid.Height);

			if (_window.Size.X / _grid.Width < _window.Size.Y / _grid.Height) {
				topLeft = new Vector2f(margin + _edgeWidth, (float)windowHeight - _grid.Height * cellSize) / 2;
			} else {
				topLeft = new Vector2f((float)windowWidth - _grid.Width * cellSize, margin + _edgeWidth) / 2;
			}

			for (int x = 0; x < _grid.Width; x++) {
				for (int y = 0; y < _grid.Height; y++) {
					DrawCell(_grid.GetCell(x, y), topLeft + new Vector2f(x, y) * cellSize, cellSize);
				}
			}
		}

		void DrawCell(Cell cell, Vector2f position, int cellSize) {
			RectangleShape rect = new(
				new Vector2f(cellSize, cellSize)
			) {
				FillColor = new Color(200, 200, 200),
				Position = position
			};

			_window.Draw(rect);

			if (cell.Value != null) {
				Text text = new Text(cell.Value.ToString(), _font, (uint)cellSize / 2) {
					FillColor = Color.Black
				};

				Vector2f center = position + new Vector2f(cellSize / 2, cellSize / 2);

				FloatRect textBounds = text.GetLocalBounds();

				Vector2f centeredPosition = new(
					center.X - textBounds.Width / 2f - textBounds.Left,
					center.Y - textBounds.Height / 2f - textBounds.Top
				);

				text.Position = centeredPosition;

				_window.Draw(text);
			}

			for (int i = 0; i < 2; i++) {
				int width = cell.Edges[i * 2].Item1.IsOn ? _edgeWidth : _edgeWidth / 2;

				RectangleShape edge = new(new Vector2f(cellSize + _edgeWidth, width)) {
					FillColor = new Color(Color.Black),
					Position = position + new Vector2f(-_edgeWidth / 2, cellSize * i - width / 2)
				};

				_window.Draw(edge);
			}

			for (int i = 0; i < 2; i++) {
				int width = cell.Edges[i * 2].Item1.IsOn ? _edgeWidth : _edgeWidth / 2;

				RectangleShape edge = new(new Vector2f(width, cellSize + _edgeWidth)) {
					FillColor = new Color(Color.Black),
					Position = position + new Vector2f(cellSize * i - width / 2, -_edgeWidth / 2)
				};

				_window.Draw(edge);
			}
		}
	}
}
