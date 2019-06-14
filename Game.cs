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
using Vector2 = System.Numerics.Vector2;

namespace DumBitEngine
{
    public class Game : GameWindow
    {
        private Model model;
        private Cube cube;

        public static Scene scene;
        public static LightSource light;

        public static Camera mainCamera;
        public static Vector2 mousePosition;

        private Camera camera;
        private ImGuiRenderer imguiRenderer;
        private ImGuiInput imguiInput;

        public static bool isCursorVisible;

        private string testingInput;

       /* TODO add a menu of some sort
            optimize the handling of vertices (store only the vertices and different transform for each)
        */

        public Game() : base(1280, // initial width
            720, // initial height
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
            AudioMaster.Init();
            GL.Enable(EnableCap.DepthTest);

            isCursorVisible = CursorVisible = false;
            VSync = VSyncMode.On;

            camera = new Camera(Width / Height);
            mainCamera = camera;

            imguiRenderer = new ImGuiRenderer("Assets/Shaders/imgui.glsl", Width, Height);
            
            light = new LightSource();
            model = new Model("Assets/Mesh/Nanosuit/", "nanosuit.obj");
            cube = new Cube("Assets/container.jpg");

            scene = new Scene();
            scene.AddEntity(model);
            scene.AddEntity(cube);
            
            scene.AddEntity(light);

            testingInput = "";
            imguiInput = new ImGuiInput();
            
            Input.Init();
            MouseDown += (sender, args) => Input.MouseEvent(args);
            MouseUp += (sender, args) => Input.MouseEvent(args);
            MouseMove += (sender, args) => Input.MouseEvent(args);
            
            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Right)
            {
                isCursorVisible = CursorVisible = !CursorVisible;
                camera.SetMousePosition(new OpenTK.Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            }
            else if (e.Button == MouseButton.Left)
            {
                /*inputState.isClicked = true;

                if (!inputState.isBeenClicked)
                {
                    inputState.isClickedDown = true;
                    inputState.isBeenClicked = true;
                }*/
                    
            }
            
            base.OnMouseDown(e);
                
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            mousePosition.X = e.Position.X;
            mousePosition.Y = e.Position.Y;
            
            if (Focused && !CursorVisible) // check to see if the window is focused  
            {
                Mouse.SetPosition(X + Width/2f, Y + Height/2f);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            camera.UpdateFOV(e.DeltaPrecise);
            
            base.OnMouseWheel(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {  
            if (!Focused)
            {
                return;
            }

            Time.deltaTime = (float) e.Time;
            //inputState.Update(Keyboard.GetState());
            Input.Update();

            HandleInput();

            Title = "DumBit Engine: FPS " + (1f / e.Time).ToString("00");

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.Black);

            scene.Draw();

            ImGui.NewFrame();
            imguiInput.UpdateUI();


            ImGui.Begin("Yes it is");
            ImGui.Text("Testing");

            if (ImGui.Button("yess"))
            {
                Console.WriteLine("YEESS");
            }

            if (ImGui.InputText("Test", ref testingInput, 10))
            {
                Console.WriteLine("Buffer has " + testingInput);
            }
            
            ImGui.End();

            ImGui.EndFrame();
            ImGui.Render();
            imguiRenderer.DrawData();
            
            
            
            SwapBuffers();
            
            base.OnUpdateFrame(e);
        }

        private void HandleInput()
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Key.Escape))
                Exit();
            if (isCursorVisible)
            {
            }
            else
            {
                mainCamera.InputUpdate(keyboard);

                if (keyboard.IsKeyDown(Key.E))
                    scene.AddEntity(new Model("Assets/Mesh/Nanosuit/", "nanosuit.obj"));
                if (keyboard.IsKeyDown(Key.R))
                    scene.Dispose();
            }

            camera.Draw();
        }

        protected override void OnClosed(EventArgs e)
        {
            scene.Dispose();
            imguiRenderer.Dispose();
            ImGui.DestroyContext();
            base.OnClosed(e);
        }
    }
}