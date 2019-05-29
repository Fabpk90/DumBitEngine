using DumBitEngine.Core.Shapes;
using DumBitEngine.Core.Util;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;
using DumBitEngine.Core.Sound;
using ImGuiNET;

namespace DumBitEngine
{
    public class Game : GameWindow
    {
        private Model model;
        private Cube cube;

        private Scene scene;

        public static Camera mainCamera;

        private Camera camera;
        private ImGuiIOPtr _io;

       /* TODO add a real camera class - add a menu of some sort
            optimize the handling of vertices (store only the vertices and different transform for each)
        */

        public Game() : base(640, // initial width
            480, // initial height
            GraphicsMode.Default,
            "DumBit Engine", 
            GameWindowFlags.Default,
            DisplayDevice.Default,
            3, // OpenGL major version
            3, // OpenGL minor version
            GraphicsContextFlags.ForwardCompatible)
        { }

        [STAThread]
        static void Main(string[] args)
        {
            new Game().Run();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            AudioMaster.Init();

            CursorVisible = false;
            VSync = VSyncMode.On;

            camera = new Camera(Width, Height);
            mainCamera = camera;

            model = new Model("Assets/Mesh/Nanosuit/", "nanosuit.obj");
            cube = new Cube("Assets/container.jpg");

            scene = new Scene();
            scene.AddEntity(model);
            scene.AddEntity(cube);
            
            scene.AddEntity(new LightSource());

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (Focused) // check to see if the window is focused  
            {
                Mouse.SetPosition(X + Width/2f, Y + Height/2f);
            }
            base.OnMouseMove(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            
            if (!Focused)
            {
                return;
            }

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

            mainCamera.InputUpdate(keyboard);

            if (keyboard.IsKeyDown(Key.E))
                scene.AddEntity(new Model("Assets/Mesh/Nanosuit/", "nanosuit.obj"));
            if (keyboard.IsKeyDown(Key.R))
                scene.Dispose();


            camera.Draw();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            scene.Dispose();
        }
    }
}