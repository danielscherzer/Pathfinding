using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Example
{
	/// <summary>
	/// The exercises window code derived from <seealso cref="GameWindow"/>.
	/// </summary>
	internal class MyWindow : GameWindow
	{
		private const int gridSize = 15; // we create a quadratic grid so gridSize == gridColumns == gridRows 
		private Random rand = new Random(12);
		private List<byte> tileTypes = new List<byte>(gridSize * gridSize); // list containing tile type numbers; one entry for each grid cell

		internal MyWindow()
		{
			GL.ClearColor(Color.White);
			// create a list that contains a random Truchet tile type for each grid cell
			while (tileTypes.Count < gridSize * gridSize)
			{
				tileTypes.Add(RandomTileType());
			}
		}

		/// <summary>
		/// Will be called each time the frame is rendered.
		/// </summary>
		/// <param name="arguments"></param>
		protected override void OnRenderFrame(FrameEventArgs arguments)
		{
			base.OnRenderFrame(arguments); // call the GameWindows implementation before executing the example code

			GL.Clear(ClearBufferMask.ColorBufferBit); // clear the screen
													  //TODO: Draw a grid that covers the whole window. Each row contains gridSize cells and each column contains gridSize cells.
													  //TODO: Use the function TruchetTile to draw a random Truchet tile into each grid cell 
			SwapBuffers(); // buffer swap needed for double buffering
		}

		/// <summary>
		/// Will be called for each game loop iteration, so by default exactly 60 times a second
		/// </summary>
		/// <param name="arguments"></param>
		protected override void OnUpdateFrame(FrameEventArgs arguments)
		{
			base.OnUpdateFrame(arguments);
			tileTypes[rand.Next(gridSize * gridSize)] = RandomTileType(); // each frame one random grid element gets a new random tile type
		}

		/// <summary>
		/// Draws a Truchet tile from position (x,y) to position (x + gridElementSize, y + gridElementSize).
		/// </summary>
		/// <param name="tileType">Gives the type of Truchet tile to draw.</param>
		/// <param name="x">Starting x-coordinate.</param>
		/// <param name="y">Starting y-coordinate.</param>
		/// <param name="gridElementSize">Size of the tile.</param>
		private static void TruchetTile(byte tileType, float x, float y, float gridElementSize)
		{
			var pi = (float)(Math.PI);
			//TODO: if 0 == tileType draw the first two quarter circle Truchet tile 
			//TODO: if 1 == tileType draw the 90° rotated two quarter circle Truchet tile 
		}

		/// <summary>
		/// Will be called each time the window is resized.
		/// </summary>
		/// <param name="arguments"></param>
		protected override void OnResize(EventArgs arguments)
		{
			base.OnResize(arguments); // call the GameWindows implementation before executing the example code

			GL.Viewport(0, 0, Width, Height); // tell OpenGL to use the whole window for drawing
		}

		/// <summary>
		/// Draws a polygon approximating a circle sector beginning at startAngle and ending at stopAngle.
		/// Both angles are given in radiants.
		/// For example to draw a quarter circle in the first quadrant startAngle would be 0 and stopAngle would be 0.5 * PI.
		/// For example to draw a quarter circle in the second quadrant startAngle would be 0.5 * pi and stopAngle would be PI.
		/// </summary>
		/// <param name="centerX">Circle center x-coordinate.</param>
		/// <param name="centerY">Circle center y-coordinate.</param>
		/// <param name="radius">Circle radius.</param>
		/// <param name="startAngle">Angle in radians where to start drawing the sector.</param>
		/// <param name="stopAngle">Angle in radians where to stop drawing the sector.</param>
		/// <param name="corners">How many outer corners should the polygon have.</param>
		private static void DrawSector(float centerX, float centerY, float radius, float startAngle, float stopAngle, int corners)
		{
			//TODO: Implement the draw sector function.
		}

		/// <summary>
		/// Return a random tile type number.
		/// </summary>
		/// <returns>0 for the first type or 1 for the second type</returns>
		private byte RandomTileType()
		{
			return (byte)rand.Next(2);
		}
	}
}