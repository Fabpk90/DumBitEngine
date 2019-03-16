using DumBitEngine.Core.Shapes;
using DumBitEngine.Core.Util;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace DumBitEngine
{
    public class Game : GameWindow
    {
        private Model model;
        private Cube cube;

        private Scene scene;

        public static Camera mainCamera;

        private Camera camera;

        public Game() : base(640, // initial width
            480, // initial height
            GraphicsMode.Default,
            "Test OpenTk", // initial title
            GameWindowFlags.Default,
            DisplayDevice.Default,
            3, // OpenGL major version
            3, // OpenGL minor version
            GraphicsContextFlags.ForwardCompatible)
        { }

        [STAThread]
        static void Main(string[] args)
        {
            new Game().Run(60);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CursorVisible = true;

            camera = new Camera(Width, Height);
            mainCamera = camera;

            model = new Model("Assets/Mesh/Nanosuit/", "nanosuit.obj");
            cube = new Cube("Assets/container.jpg");

            scene = new Scene();
            scene.AddRenderable(model);
            scene.AddRenderable(cube);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Time.deltaTime = (float) e.Time;
            base.OnUpdateFrame(e);
            HandleInput();

            Title = "DumBit Engine: FPS " + (1f / e.Time).ToString("00");

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.Black);

            scene.Draw();

            SwapBuffers();
        }

        private void HandleInput()
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Key.Escape))
                Exit();

            if (keyboard.IsKeyDown(Key.Up))
            {
                mainCamera.cameraPos += mainCamera.CameraFront * .5f;
            }
            else if (keyboard.IsKeyDown(Key.Down))
            {
                mainCamera.cameraPos -= mainCamera.CameraFront * .5f;
            }

            if (keyboard.IsKeyDown(Key.Left))
            {
                mainCamera.cameraPos.X -= .5f;
            }
            else if (keyboard.IsKeyDown(Key.Right))
            {
                mainCamera.cameraPos.X += .5f;
            }

            camera.Draw();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

        }
    }
}