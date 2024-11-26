using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Slitherlink.View;

class Program {
	static void Main() {
		RenderWindow window = new RenderWindow(new VideoMode(932, 600), "Slitherlink");
		window.SetFramerateLimit(60);

		window.Closed += (sender, e) => ((RenderWindow)sender!).Close();
		window.Resized += (sender, e) => {
			window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
		};

		Font font = new Font("Teko-Bold.ttf");

		Board board = new(
			window,
			font,
			10,
			10,
			4
		);

		while (window.IsOpen) {
			window.Clear(Color.White);

			board.Draw();

			window.DispatchEvents();

			window.Display();
		}
	}
}