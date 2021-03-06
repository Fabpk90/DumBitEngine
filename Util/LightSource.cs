using System;
using System.Numerics;
using DumBitEngine.Util;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;

namespace DumBitEngine.Core.Util
{
    public class LightSource : Entity
    {
        public Vector3 color;
        
        private uint[] index;
        private float[] vertex;

        private Shader shader;

        private int vao;
        private int vbo;
        private int ebo;

        public LightSource(string name, GameObject parent) : base(name, parent)
        {
            
            LevelManager.activeScene.lightSources.Add(this);
            
            index = new uint[]
            {
                // note that we start from 0!
                0, 1, 2,
                2, 3, 0,
                4, 5, 6,
                6, 7, 4,
                0, 4, 1,
                1, 5, 4,
                2, 6, 3,
                3, 7, 6,
                0, 4, 3,
                3, 7, 4,
                2, 6, 5,
                2, 1, 5
            };

            vertex = new float[]
            {
                //pos           //color
                0.5f, 0.5f, 0.5f,      // top right
                0.5f, -0.5f, 0.5f,      // bottom right
                -0.5f, -0.5f, 0.5f,     // bottom left
                -0.5f, 0.5f, 0.5f,      // top left
                0.5f, 0.5f, -0.5f,    
                0.5f, -0.5f, -0.5f,   
                -0.5f, -0.5f, -0.5f,   
                -0.5f, 0.5f, -0.5f,   
            };
            
            color = new Vector3(.5f, .5f, .5f);
            
            shader = new Shader("Assets/Shaders/lightSource.glsl");

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            
            GL.BufferData(BufferTarget.ArrayBuffer, vertex.Length * sizeof(float), vertex, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, index.Length * sizeof(uint), index, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            shader.Use();
            shader.SetMatrix4("projection", ref Camera.main.projection);
            shader.SetMatrix4("transform", ref parent.getMatrix4X4());
            shader.SetVector3("color", ref color);
        }
        
        public override void Draw()
        {
            GL.BindVertexArray(vao);

            shader.Use();
            shader.SetMatrix4("view", ref Camera.main.view);
            shader.SetMatrix4("transform", ref Parent.getMatrix4X4());
            shader.SetMatrix4("projection", ref Camera.main.projection);
            
            
            GL.DrawElements(PrimitiveType.Triangles, index.Length, DrawElementsType.UnsignedInt, 0);
        }

        public override void Awake()
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            shader.Dispose();
            
            GL.DeleteBuffer(vao);
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);

            LevelManager.activeScene.lightSources.Remove(this);
        }

        public override void GetUiToDraw()
        {
            ImGui.Text(name);

            var position = Parent.getMatrix4X4().Translation;
            ImGui.DragFloat3("Position", ref position);
            Parent.getMatrix4X4().Translation = position;

            ImGui.ColorPicker3("Color", ref color);
        }
    }
}