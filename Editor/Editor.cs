using System;
using DumBitEngine.Core.Sound;
using DumBitEngine.Core.Util;
using DumBitEngine.Util;
using ImGuiNET;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace DumBitEngine.Editor
{
    public class Editor : GameWindow
    {
        private ImGuiRenderer imguiRenderer;
        private Scene scene;
        private ImGuiInput imguiInput;

        public Editor() : base(1024, 720, GraphicsMode.Default, "Editor", GameWindowFlags.Default,
            DisplayDevice.Default, 3, 3, GraphicsContextFlags.ForwardCompatible)
        {}
        
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            GL.Enable(EnableCap.DepthTest);

            CursorVisible = false;
            VSync = VSyncMode.On;

            Camera.main = new Camera(Width / Height);

            AudioMaster.Init();

            imguiRenderer = new ImGuiRenderer("Assets/Shaders/imgui.glsl", Width, Height);
            scene = new Scene("Scene");
            
            imguiInput = new ImGuiInput();
            
            MasterInput.Init();
            MouseDown += (sender, args) => MasterInput.MouseEvent(args);
            MouseUp += (sender, args) => MasterInput.MouseEvent(args);
            MouseMove += (sender, args) => MasterInput.MouseEvent(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!Focused)
            {
                return;
            }

            Time.deltaTime = (float) e.Time;
            //inputState.Update(Keyboard.GetState());
            MasterInput.Update();
            AudioMaster.Update();

            HandleInput();
        }

        private void HandleInput()
        {
            if(MasterInput.IsKeyPressed(Key.Escape))
                Exit();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            
            GL.Viewport(0, 0, Width, Height);
            imguiRenderer.ResizeScreen(Width, Height);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            
            if (e.Button == MouseButton.Right)
            {
                CursorVisible = false;
                Camera.main.SetMousePosition(new OpenTK.Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            
            if (e.Button == MouseButton.Right)
            {
                CursorVisible = true;
                Camera.main.SetMousePosition(new OpenTK.Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (Focused && !CursorVisible) // check to see if the window is focused  
            {
                Mouse.SetPosition(X + Width/2f, Y + Height/2f);
            }
            base.OnMouseMove(e);
        }

        protected override void OnFileDrop(FileDropEventArgs e)
        {
            base.OnFileDrop(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            
            scene.Dispose();
            imguiRenderer.Dispose();
            ImGui.DestroyContext();
        }
    }
}