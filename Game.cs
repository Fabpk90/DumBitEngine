using DumBitEngine.Core.Shapes;
using DumBitEngine.Core.Util;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;
using System.Numerics;
using DumBitEngine.Core.Sound;
using ImGuiNET;
using Vector2 = System.Numerics.Vector2;

namespace DumBitEngine
{
    public class Game : GameWindow
    {

        public static Scene scene;
        public static LightSource light;

        public static Vector2 mousePosition;
        
        private ImGuiRenderer imguiRenderer;
        private ImGuiInput imguiInput;

        public static bool isCursorVisible;

        private int testingInt = 0;

        private Source presentationSource;
        
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
            GL.Enable(EnableCap.DepthTest);

            isCursorVisible = CursorVisible = false;
            VSync = VSyncMode.On;

            Camera.main = new Camera(Width / Height);

            AudioMaster.Init();

            imguiRenderer = new ImGuiRenderer("Assets/Shaders/imgui.glsl", Width, Height);
            
            light = new LightSource("Light"); // TODO: remove this(static) and find a solution for shader using it

            scene = new Scene("Scene");
            
            GameObject cubeGO = new GameObject("Cube");
            var cube = new Cube("Assets/container.jpg");
            cube.transform = Matrix4x4.CreateScale(10, 0.1f, 10);
            cube.transform *= Matrix4x4.CreateTranslation(0, -1f, 0);
            cubeGO.AddComponent(cube);
            
            var cube0 = new Cube("Assets/container.jpg");
            cube0.transform *= Matrix4x4.CreateTranslation(-1, 0, 0);
            cube0.isRotating = true;
            cubeGO.AddComponent(cube0);

            
            var modelGO = new GameObject("Model");
            modelGO.AddComponent(new Model("Assets/Mesh/Nanosuit/", "nanosuit.obj"));
            modelGO.GetComponent<Model>().transform *= Matrix4x4.CreateTranslation(0, -1.75f, 0);
            modelGO.GetComponent<Model>().transform *= Matrix4x4.CreateScale(.2f, .2f, .2f);
            modelGO.GetComponent<Model>().isRotating = true;


            var sofa = new Model("Assets/Mesh/Sofa/", "Sofa.obj");
            sofa.transform *= Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(45f));
            sofa.transform *= Matrix4x4.CreateScale(.1f, .1f, .1f);
            sofa.transform *= Matrix4x4.CreateTranslation(0, 0, -9f);

            modelGO.AddComponent(sofa);
            
            var lightGO = new GameObject("Light");
            lightGO.AddComponent(light);

            scene.Add(modelGO);
            scene.Add(cubeGO);
            
            scene.Add(lightGO);
            
            imguiInput = new ImGuiInput();
            
            MasterInput.Init();
            MouseDown += (sender, args) => MasterInput.MouseEvent(args);
            MouseUp += (sender, args) => MasterInput.MouseEvent(args);
            MouseMove += (sender, args) => MasterInput.MouseEvent(args);

            presentationSource = AudioMaster.LoadSourceAndSound("Assets/Sound/bounce.wav");
            
            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            imguiRenderer.ResizeScreen(Width, Height);
            base.OnResize(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Right)
            {
                isCursorVisible = CursorVisible = !CursorVisible;
                Camera.main.SetMousePosition(new OpenTK.Vector2(Mouse.GetState().X, Mouse.GetState().Y));
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
            Camera.main.UpdateFOV(e.DeltaPrecise);
            
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
            MasterInput.Update();
            AudioMaster.Update();

            HandleInput();

            Title = "DumBit Engine: FPS " + (1f / e.Time).ToString("00");

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.Black);

            ImGui.NewFrame();
            scene.Draw();

            
            imguiInput.UpdateUI();

            ImGui.Text("sa");
            ImGui.Begin("Yes it is");
            ImGui.Text("Testing");

            if (ImGui.Button("yess"))
            {
                Console.WriteLine("YEESS");
            }


            //ImGui.DragInt4()
            
            ImGui.End();

            ImGui.EndFrame();
            ImGui.Render();
            imguiRenderer.DrawData();
            
            
            
            SwapBuffers();
            
            base.OnUpdateFrame(e);
        }

        private void HandleInput()
        {

            if (MasterInput.IsKeyPressed(Key.Escape))
                Exit();
            if (!isCursorVisible)
            {
                Camera.main.InputUpdate();

                if (MasterInput.IsKeyDown(Key.Space))
                {
                    presentationSource.Play();
                }
            }

            Camera.main.Draw();
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