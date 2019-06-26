using System;
using OpenTK.Graphics.OpenGL;

namespace DumBitEngine.Core.Util
{
    public class Buffer : IDisposable
    {
        public BufferTarget BufferType { get; }
        public BufferUsageHint BufferHint { get; }
        public int BufferSize { get; set; }
        public int BufferId { get; set; }
        private IntPtr data;

        public Buffer(BufferTarget type, int size, IntPtr data, BufferUsageHint usageHint)
        {
            BufferType = type;
            BufferSize = size;
            BufferHint = usageHint;
            this.data = data;

            BufferId = GL.GenBuffer();
            GL.BindBuffer(type, BufferId);
            GL.BufferData(type, (int)size, data, usageHint);
        }

        public void SetBufferSize(int size)
        {
            BufferSize = size;
            GL.DeleteBuffer(BufferId);
            
            BufferId = GL.GenBuffer();
            GL.BindBuffer(BufferType, BufferId);
            GL.BufferData(BufferType, size, data, BufferHint);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(BufferId);
        }
    }
}