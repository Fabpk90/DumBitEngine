using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using DumBitEngine.Core.Shapes;
using OpenTK.Input;

namespace DumBitEngine
{
    public class Game : GameWindow
    {

        private Cube cube;
        private Mesh mesh;
        
        public Game() : base(1280, 720, GraphicsMode.Default
            , "DumBit Engine", GameWindowFlags.Default, DisplayDevice.Default
            , 3, 3, GraphicsContextFlags.ForwardCompatible)
        {}
        
        [STAThread]
        static void Main(string[] args)
        {
            new Game().Run( 60);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CursorVisible = true;
            
            cube = new Cube();
            mesh = new Mesh("Seat.obj");
        }
        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Escape)
                Exit();
                
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            
            Title = "DumBit Engine: FPS " + (1f / e.Time).ToString("00");

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.Black);
            
            cube.Draw();
            
            SwapBuffers();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            
            cube.Dispose();
            mesh.Dispose();
        }
    }
}