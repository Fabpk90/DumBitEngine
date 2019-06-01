using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace DumBitEngine.Core.Util
{
    public class LightSource : Entity
    {
        public Matrix4 transform;
        public Vector3 color;
        
        private uint[] index;
        private float[] vertex;

        private Shader shader;

        private int vao;
        private int vbo;
        private int ebo;

        public LightSource()
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
                0.5f, 0.5f, 0.5f,      // top right
                0.5f, -0.5f, 0.5f,      // bottom right
                -0.5f, -0.5f, 0.5f,     // bottom left
                -0.5f, 0.5f, 0.5f,      // top left
                0.5f, 0.5f, -0.5f,    
                0.5f, -0.5f, -0.5f,   
                -0.5f, -0.5f, -0.5f,   
                -0.5f, 0.5f, -0.5f,   
            };
            
            transform = Matrix4.Identity;
            transform *= Matrix4.CreateTranslation(1, 1, 1);
            color = new Vector3(.25f, .5f, .5f);
            
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
            shader.SetMatrix4("projection", ref Game.mainCamera.projection);
            shader.SetMatrix4("transform", ref transform);
            shader.SetVector3("color", ref color);
        }
        
        public override void Draw()
        {
            shader.Use();
            shader.SetMatrix4("view", ref Game.mainCamera.view);
            shader.SetMatrix4("projection", ref Game.mainCamera.projection);
            GL.BindVertexArray(vao);
            
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
        }
    }
}