using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace DumBitEngine.Core.Util
{
    public abstract class Entity : IRenderable, IDisposable
    {
        public bool isActive;

        public Entity()
        {
            isActive = true;
        }
        
        public abstract void Awake();
        public abstract void Start();

        public abstract void Dispose();
        public abstract void Draw();
    }
}
