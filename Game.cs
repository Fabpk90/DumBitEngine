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

        public static Matrix4 view;
        public static Matrix4 projection;

        private Model model;

        private Cube cube;
        
        private Vector3 cameraUp;
        private Vector3 cameraPos;
        private Vector3 cameraFront;

        public Game() : base(640, // initial width
            480, // initial height
            GraphicsMode.Default,
            "Test OpenTk", // initial title
            GameWindowFlags.Default,
            DisplayDevice.Default,
            3, // OpenGL major version
            3, // OpenGL minor version
            GraphicsContextFlags.ForwardCompatible)
        {}
        
        [STAThread]
        static void Main(string[] args)
        {
            new Game().Run(60);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CursorVisible = true;
            
            
            
            cameraPos = new Vector3(0, 0, 10f);
            cameraFront = new Vector3(0, 0, -1f);
            cameraUp = new Vector3(0, 1, 0);

            projection = Matrix4.CreatePerspectiveFieldOfView(OpenTK.MathHelper.DegreesToRadians(55.0f), Width / Height,
                0.1f, 100f);
            view = Matrix4.LookAt(cameraPos, cameraFront + cameraPos, cameraUp);
            
            model = new Model("Assets/Mesh/", "nanosuit.obj");
            cube = new Cube("Assets/container.jpg");
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
            else if (e.Key == Key.Up)
            {
                cameraPos += cameraFront * .5f;
            }
            else if (e.Key == Key.Down)
            {
                cameraPos -= cameraFront * .5f;
            }   
                
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            
            Title = "DumBit Engine: FPS " + (1f / e.Time).ToString("00");

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.Black);
            
            view = Matrix4.LookAt(cameraPos, cameraFront + cameraPos, cameraUp);
            
            model.Draw();
            cube.Draw((float)e.Time);
            
            SwapBuffers();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
           
        }
    }
}