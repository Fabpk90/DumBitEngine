using DumBitEngine.Core.Util;

namespace DumBitEngine.Core.Shapes
{
    public abstract class Shape : IDisposable, IRenderable
    {
        public abstract void Dispose();
        public abstract void Draw();
    }
}