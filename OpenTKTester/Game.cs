using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenTKTester
{
    class Game : GameWindow
    {
        private float XCurrentPosition1 { get; set; }

        private float YCurrentPosition1 { get; set; }

        private float XCurrentPosition2 { get; set; }

        private float YCurrentPosition2 { get; set; }

        public enum Directions
        {
            None = 0,
            Left = 1,
            Right = 2,
            Up = 3,
            Down = 4
        }

        public enum Winner
        {
            None = 0,
            Player1 = 1,
            Player2 = 2
        }

        private Winner WinningPlayer { get; set; }

        private Directions Direction1 { get; set; }
        private Directions Direction2 { get; set; }
        private List<Vector3> Vectors1 { get; set; }
        private List<Vector3> Vectors2 { get; set; }

        public Game()
            : base(800, 600, GraphicsMode.Default, "OpenTK Quick Start Sample")
        {
            VSync = VSyncMode.On;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
            XCurrentPosition1 = -1.0f;
            YCurrentPosition1 = -1.0f;
            XCurrentPosition2 = 1.0f;
            YCurrentPosition2 = -1.0f;
            Vectors1 = new List<Vector3> {new Vector3(-1.0f, -1.0f, 4.0f), new Vector3(-1.0f, -1.0f, 4.0f)};
            Vectors2 = new List<Vector3> {new Vector3(1.0f, -1.0f, 4.0f), new Vector3(1.0f, -1.0f, 4.0f)};
            Direction1 = Directions.Left;
            Direction2 = Directions.Up;
            WinningPlayer = Winner.None;
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

//            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            GL.Viewport(ClientRectangle);

            Matrix4 projection =
                Matrix4.CreatePerspectiveFieldOfView((float) Math.PI / 4, Width / (float) Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        public Directions updateDirection(Directions direction, List<Key> controls)
        {
            var oldDirection = direction;
            var newDirection = Directions.None;

            if (Keyboard[controls[0]] && oldDirection != Directions.Left && oldDirection != Directions.Right)
            {
                direction = newDirection = Directions.Left;
            }

            if (Keyboard[controls[1]] && oldDirection != Directions.Left && oldDirection != Directions.Right)
            {
                direction = newDirection = Directions.Right;
            }

            if (Keyboard[controls[2]] && oldDirection != Directions.Up && oldDirection != Directions.Down)
            {
                direction = newDirection = Directions.Up;
            }

            if (Keyboard[controls[3]] && oldDirection != Directions.Up && oldDirection != Directions.Down)
            {
                direction = newDirection = Directions.Down;
            }

            return newDirection == Directions.None ? direction : newDirection;
        }

        public void Explosion()
        {
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
//            if (XCurrentPosition1 < (float) -Width / 1000 || XCurrentPosition1 > (float) Width / 1000 || YCurrentPosition1 < -Height / 1000 ||
//                YCurrentPosition1 > (float) Height / 1000)
//            {
//                WinningPlayer = Winner.Player2;
//                Console.WriteLine("game over");
//                Console.WriteLine("The Winner is ");
//                Console.Write(WinningPlayer);
//            }
//            else if (XCurrentPosition2 < -1.9f || XCurrentPosition2 > 1.9f || YCurrentPosition2 < -1.9f ||
//                     YCurrentPosition2 > 1.9f)
//            {
//                WinningPlayer = Winner.Player1;
//                Console.WriteLine("game over");
//                Console.WriteLine("The Winner is ");
//                Console.Write(WinningPlayer);
//            }

            var oldDirection1 = Direction1;
            var oldDirection2 = Direction2;

            var newDirection1 = updateDirection(Direction1, new List<Key> {Key.A, Key.D, Key.W, Key.S});
            if (newDirection1 != oldDirection1)
            {
                Direction1 = newDirection1;
            }

            var newDirection2 = updateDirection(Direction2, new List<Key> {Key.Left, Key.Right, Key.Up, Key.Down});
            if (newDirection2 != oldDirection2)
            {
                Direction2 = newDirection2;
            }

            if (WinningPlayer == Winner.None)
            {
                switch (Direction1)
                {
                    case Directions.Left:
                        XCurrentPosition1 += 0.01f;
                        break;
                    case Directions.Right:
                        XCurrentPosition1 -= 0.01f;
                        break;
                    case Directions.Up:
                        YCurrentPosition1 += 0.01f;
                        break;
                    case Directions.Down:
                        YCurrentPosition1 -= 0.01f;
                        break;
                }

                switch (Direction2)
                {
                    case Directions.Left:
                        XCurrentPosition2 += 0.01f;
                        break;
                    case Directions.Right:
                        XCurrentPosition2 -= 0.01f;
                        break;
                    case Directions.Up:
                        YCurrentPosition2 += 0.01f;
                        break;
                    case Directions.Down:
                        YCurrentPosition2 -= 0.01f;
                        break;
                }
            }

            // move this to a method
            if (newDirection1 != oldDirection1)
            {
                Vectors1.Add(new Vector3(XCurrentPosition1, YCurrentPosition1, 4.0f));
            }
            else
            {
                Vectors1[Vectors1.Count - 1] = new Vector3(XCurrentPosition1, YCurrentPosition1, 4.0f);
            }

            // move this to a method
            if (newDirection2 != oldDirection2)
            {
                Vectors2.Add(new Vector3(XCurrentPosition2, YCurrentPosition2, 4.0f));
            }
            else
            {
                Vectors2[Vectors2.Count - 1] = new Vector3(XCurrentPosition2, YCurrentPosition2, 4.0f);
            }


            if (Keyboard[Key.T])
            {
                GL.Begin(PrimitiveType.Triangles);
                GL.Color3(1.0f, 1.0f, 0.0f);
                GL.Vertex3(-1.0f, -1.0f, 4.0f);
                GL.Color3(1.0f, 0.0f, 0.0f);
                GL.Vertex3(1.0f, -1.0f, 4.0f);
                GL.Color3(0.2f, 0.9f, 1.0f);
                GL.Vertex3(0.0f, 1.0f, 4.0f);
                GL.End();
                SwapBuffers();
            }

            if (Keyboard[Key.Escape])
                Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            // draw a line that can be moved
            GL.Begin(PrimitiveType.LineStrip);
            GL.Color3(Color.Fuchsia);

            foreach (var vector in Vectors1)
            {
                GL.Vertex3(vector);
            }

            GL.End();

            // draw second line that can be moved
            GL.Begin(PrimitiveType.LineStrip);
            GL.Color3(Color.Beige);

            foreach (var vector in Vectors2)
            {
                GL.Vertex3(vector);
            }

            GL.End();


            GL.Begin(PrimitiveType.TriangleFan);

            float radius = 0.5f;
            float red, green, blue;
            red = green = blue = 0.0f;

            for (int i = 0; i < 360; i++)
            {
                red += (float) 1.0 / 360;
                double degInRad = i * 3.1416 / 180;
                GL.Color3(red, 0.0f, 0.0f);
                GL.Vertex3(Math.Cos(degInRad) * radius, Math.Sin(degInRad) * radius, 4.0f);
            }

            GL.End();

            SwapBuffers();
        }

        [STAThread]
        static void Main()
        {
            // The 'using' idiom guarantees proper resource cleanup.
            // We request 30 UpdateFrame events per second, and unlimited
            // RenderFrame events (as fast as the computer can handle).
            using (Game game = new Game())
            {
                game.Run(30.0);
            }
        }
    }
}