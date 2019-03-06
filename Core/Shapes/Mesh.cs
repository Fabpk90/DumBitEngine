using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Assimp;
using Assimp.Unmanaged;
using DumBitEngine.Core.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace DumBitEngine.Core.Shapes
{
    public class Mesh : Shape
    {
        private int vao;
        private int vbo;
        private int ebo;
        
        public List<Vertex> vertices;
        public List<uint> indices;
        public List<Texture> textures;

        public Mesh(List<Vertex> vertexes, List<uint> indices, List<Texture> textures)
        {
            vertices = vertexes;
            this.indices = indices;
            this.textures = textures;

            SetupBuffers();
        }

        private void SetupBuffers()
        {
            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();
            ebo = GL.GenBuffer();
            
            GL.BindVertexArray(vao);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Marshal.SizeOf<Vertex>(), vertices.ToArray(),
                BufferUsageHint.StaticDraw);
            
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(),
                BufferUsageHint.StaticDraw);
            
            GL.EnableVertexAttribArray(vao);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex>(), 0);
            
            GL.EnableVertexAttribArray(vao);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false,
                Marshal.SizeOf<Vertex>()  , Marshal.OffsetOf<Vertex>("normal"));
            
            GL.EnableVertexAttribArray(vao);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false,
                Marshal.SizeOf<Vertex>(), Marshal.OffsetOf<Vertex>("texCoord"));


        }

        public override void Dispose()
        {
            
        }

        public override void Draw()
        {
            throw new NotImplementedException();
        }
    }
}