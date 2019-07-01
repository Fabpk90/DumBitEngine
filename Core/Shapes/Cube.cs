using System;
using System.Drawing;
using System.Numerics;
using DumBitEngine.Core.Sound;
using DumBitEngine.Core.Util;
using ImGuiNET;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace DumBitEngine.Core.Shapes
{
    public class Cube : Shape
    {
        private int vao;
        private int vbo;
        private int ebo;

        private Shader shaderProgram;

        private Texture texture0;
        
        
        private float[] vertex;
        private uint[] index;
        public bool isRotating;

        public Cube(string texturePath) : base("Cube")
        {
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
                //pos                   //color        //texcoord
                0.5f, 0.5f, 0.5f,     .9f, .59f, .7f, 1.0f, 1.0f, // top right
                0.5f, -0.5f, 0.5f,    .5f, .6f, .68f, 1.0f, 0.0f, // bottom right
                -0.5f, -0.5f, 0.5f,   .5f, .57f, .69f, 0.0f, 0.0f, // bottom left
                -0.5f, 0.5f, 0.5f,    .5f, .58f, .69f, 0.0f, 1.0f, // top left
                0.5f, 0.5f, -0.5f,    .5f, .6f, .7f, 1.0f, 1.0f,
                0.5f, -0.5f, -0.5f,   .5f, .58f, .67f, 1.0f, 0.0f,
                -0.5f, -0.5f, -0.5f,  .5f, .6f, .675f, 0.0f, 0.0f,
                -0.5f, 0.5f, -0.5f,   .5f, .55f, .69f, 0.0f, 1.0f
            };
            
            transform = Matrix4x4.Identity;

            name = "Cube";

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertex.Length * sizeof(float), vertex,
                BufferUsageHint.StaticDraw);

            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * index.Length, index, BufferUsageHint.StaticDraw);
            
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, true, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            shaderProgram = new Shader("Assets/Shaders/cube.glsl");
            shaderProgram.Use();
            
            shaderProgram.SetMatrix4("projection", ref Camera.main.projection);
            
           texture0 = new Texture(texturePath, "");

           shaderProgram.SetInt("tex0", 0);
        }

        public override void Awake()
        {
           
        }

        public override void Start()
        {
           
        }

        public override void Dispose()
        {
            shaderProgram.Dispose();

            GL.DeleteVertexArray(vao);

            GL.DeleteBuffer(ebo);
            GL.DeleteBuffer(vbo);

            texture0.Dispose();
        }

        public override void Draw()
        {
            if (isRotating)
            {
                transform *= Matrix4x4.CreateRotationY(Time.deltaTime);
                transform *= Matrix4x4.CreateRotationX(Time.deltaTime);
            }

            GL.BindVertexArray(vao);

            shaderProgram.Use();
            shaderProgram.SetMatrix4("transform", ref transform);
            shaderProgram.SetMatrix4("view", ref Camera.main.view);
            shaderProgram.SetMatrix4("projection", ref Camera.main.projection);
            
            shaderProgram.SetVector3("lightColor", ref Game.light.color);
            
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D ,texture0.id);
            GL.DrawElements(PrimitiveType.Triangles, index.Length, DrawElementsType.UnsignedInt, 0);
        }

        public override void GetUiToDraw()
        {
            ImGui.Text(name);

            var position = transform.Translation;
            ImGui.DragFloat3("Position", ref position);
            transform.Translation = position;
        }
    }
}