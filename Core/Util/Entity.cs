using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumBitEngine.Core.Util
{
    public abstract class Entity : IRenderable, IDisposable
    {
        public abstract void Dispose();
        public abstract void Draw();
    }
}
