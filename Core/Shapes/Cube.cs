using System;
using DumBitEngine.Core.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DumBitEngine.Core.Shapes
{
    public class Cube : Shape
    {
        private int vao;
        private int vbo;
        private int ebo;

        private Shader shader;

        private Matrix4 transform;

        private float[] vertices;
        
        public Cube()
        {
            transform = Matrix4.Identity;
            
            shader = new Shader("Assets/Shaders/basic.glsl");

            vao = GL.GenBuffer();
            GL.BindVertexArray(vao);

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            vertices = new float[]
            {
                0, 1, 0,
                1, 0, 0,
                -1, 0, 0
            };
            
            GL.BindVertexBuffer(0, vbo, IntPtr.Zero, 3 * sizeof(float));
        }

        public void Translate(Vector3 translation)
        {
            transform += Matrix4.CreateTranslation(translation);
        }
        
        public override void Dispose()
        {
            shader.Dispose();
            
            GL.DeleteBuffer(vao);
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
        }

        public override void Draw()
        {
            GL.BindVertexArray(vao);
            shader.Use();
            
            shader.SetMatrix4("transform", ref transform);
        }
    }
}