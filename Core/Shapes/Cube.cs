using System;
using System.Drawing;
using DumBitEngine.Core.Util;
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

       // private Texture texture0;

        private Shader shaderProgram;

        private Texture texture0;
        
        
        private float[] vertex;
        private uint[] index;

        private Matrix4 transform;

        public Cube(string texturePath)
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
                //pos           //color
                0.5f, 0.5f, 0.5f,     .9f, .59f, .7f, 1.0f, 1.0f, // top right
                0.5f, -0.5f, 0.5f,    .5f, .6f, .68f, 1.0f, 0.0f, // bottom right
                -0.5f, -0.5f, 0.5f,   .5f, .57f, .69f, 0.0f, 0.0f, // bottom left
                -0.5f, 0.5f, 0.5f,    .5f, .58f, .69f, 0.0f, 1.0f, // top left
                0.5f, 0.5f, -0.5f,    .5f, .6f, .7f, 1.0f, 1.0f,
                0.5f, -0.5f, -0.5f,   .5f, .58f, .67f, 1.0f, 0.0f,
                -0.5f, -0.5f, -0.5f,  .5f, .6f, .675f, 0.0f, 0.0f,
                -0.5f, 0.5f, -0.5f,   .5f, .55f, .69f, 0.0f, 1.0f,
            };
            
            transform = Matrix4.Identity;

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
            
            shaderProgram.SetMatrix4("projection", ref Game.mainCamera.projection);
            
           texture0 = new Texture(texturePath, "");
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.MirroredRepeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.MirroredRepeat);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);

            shaderProgram.SetInt("tex0", 0);
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
            transform *= Matrix4.CreateRotationY(Time.deltaTime);
            transform *= Matrix4.CreateRotationX(Time.deltaTime);

            shaderProgram.Use();
            GL.BindVertexArray(vao);

            shaderProgram.SetMatrix4("transform", ref transform);
            shaderProgram.SetMatrix4("view", ref Game.mainCamera.view);

            GL.DrawElements(PrimitiveType.Triangles, index.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}