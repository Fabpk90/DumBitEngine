using DumBitEngine.Core.Shapes;
using DumBitEngine.Core.Util;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using DumBitEngine.Core.Physics;
using DumBitEngine.Core.Sound;
using DumBitEngine.Util;
using ImGuiNET;
using Vector2 = System.Numerics.Vector2;
using Vector3 = System.Numerics.Vector3;

namespace DumBitEngine
{
    public class Game : GameWindow
    {

        public static Scene scene;
        public static LightSource light;

        private ImGuiRenderer imguiRenderer;
        private ImGuiInput imguiInput;

        public static bool isCursorVisible;

        private int testingInt = 0;

        private Cube cube;

        private Source presentationSource;
        private Cube dynCube;
        private uint frameCount;

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

        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);

            isCursorVisible = CursorVisible = false;
            VSync = VSyncMode.On;

            Camera.main = new Camera(Width / Height);

            AudioMaster.Init();

            imguiRenderer = new ImGuiRenderer("Assets/Shaders/imgui.glsl", Width, Height);
            
            GameObject lightGO = new GameObject("Light Source");
            light = new LightSource("Light", lightGO); // TODO: remove this(static) and find a solution for shader using it
            lightGO.AddComponent(light);
            
            
            scene = new Scene("Scene");
            
            GameObject cubeGO = new GameObject("Cube");
            var cube = new Cube("Assets/container.jpg", cubeGO);
            cube.Parent.getMatrix4X4() *= Matrix4x4.CreateScale(10, 0.1f, 10);
            cube.Parent.getMatrix4X4() *= Matrix4x4.CreateTranslation(0, -1f, 0);
            cubeGO.AddComponent(cube);

            var modelGO = new GameObject("Model");
            modelGO.AddComponent(new Model("Assets/Mesh/Nanosuit/", "nanosuit.obj", modelGO));
            modelGO.GetComponent<Model>().Parent.getMatrix4X4() *= Matrix4x4.CreateTranslation(0, -1.75f, 0);
            modelGO.GetComponent<Model>().Parent.getMatrix4X4() *= Matrix4x4.CreateScale(.2f, .2f, .2f);
            modelGO.GetComponent<Model>().isRotating = true;

            scene.Add(modelGO);
            scene.Add(cubeGO);
            
            scene.Add(lightGO);
            
            imguiInput = new ImGuiInput();
            
            MasterInput.Init();
            MouseDown += (sender, args) => MasterInput.MouseEvent(args);
            MouseUp += (sender, args) => MasterInput.MouseEvent(args);
            MouseMove += (sender, args) => MasterInput.MouseEvent(args);

            presentationSource = AudioMaster.LoadSourceAndSound("Assets/Sound/bounce.wav");
            presentationSource.SetPosition(Vector3.Zero);
            
            RunSimulation();
            
            base.OnLoad(e);
        }

        private void RunSimulation()
        {
            Physics.Init();
            
            GameObject g = new GameObject("Ya know it");
            dynCube = new Cube("Assets/container.jpg", g);
            
            g.AddComponent(dynCube);
            g.AddComponent(new RigidBody("RigidBody"));
            
            scene.Add(g);

            var sim = Physics.sim;
            
            for (int i = 0; i < sim.Bodies.Sets.Length; ++i)
            {
                ref var set = ref sim.Bodies.Sets[i];
                if (set.Allocated) //Islands are stored noncontiguously; skip those which have been deallocated.
                {
                    for (int bodyIndex = 0; bodyIndex < set.Count; ++bodyIndex)
                    {
                        var shape = set.Collidables[bodyIndex].Shape;
                        var p = set.Poses[bodyIndex];
                        
                            switch (shape.Type)
                            {
                                case Box.Id:
                                    Console.WriteLine("Boxxxxxxx");

                                    unsafe
                                    {
                                        //the shape data is a raw pointer to the according shape
                                        //useful for radius/length etc...
                                        Physics.sim.Shapes[shape.Type]
                                            .GetShapeData(shape.Index, out var shapeData, out var size);
                                        
                                        ref var box = ref Unsafe.AsRef<Box>(shapeData);

                                        dynCube.Parent.getMatrix4X4().Translation = p.Position;
                                        dynCube.Parent.getMatrix4X4() *= Matrix4x4.CreateScale(box.Width, box.Height, box.Length);

                                        dynCube.Parent.GetComponent<RigidBody>().info = set.Poses;
                                        dynCube.Parent.GetComponent<RigidBody>().index = bodyIndex;
                                        

                                    }
                                    

                                    break;
                            }
                    }
                }
            }
            
            
            GameObject go = new GameObject("Yeppa");
            cube = new Cube("Assets/container.jpg", go);
            go.AddComponent(cube);
            
            scene.Add(go);

            var statics = Physics.sim.Statics;
            //first draw the statics 
           // for (int i = 0; i < SimpleSelfContainedDemo.sim.Statics.Count; i++)
            //{
                PhysicsCreateShape(statics, 0);
            //}

            //SimpleSelfContainedDemo.Run();
        }

        private unsafe void PhysicsCreateShape(Statics statics, int i)
        {
            if (statics.Collidables[i].Shape.Exists)
                switch (statics.Collidables[i].Shape.Type)
                {
                    case Box.Id:
                        Console.WriteLine("Boxxxxxxx");

                        //the shape data is a raw pointer to the according shape
                        //useful for radius/length etc...
                        Physics.sim.Shapes[statics.Collidables[i].Shape.Type]
                            .GetShapeData(statics.Collidables[i].Shape.Index, out var shapeData, out var size);
                        var pose = statics.Poses[i];
                        ref var box = ref Unsafe.AsRef<Box>(shapeData);

                        cube.Parent.getMatrix4X4().Translation = pose.Position;
                        cube.Parent.getMatrix4X4() *= Matrix4x4.CreateScale(box.Width, box.Height, box.Length);


                        break;
                }
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


            Physics.Update();



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

                if (MasterInput.IsKeyDown(Key.E))
                {
                    presentationSource.Play();
                }
                else if (MasterInput.IsKeyDown(Key.P))
                {
                    dynCube.Parent.AddPosition(0, 10, 0);
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