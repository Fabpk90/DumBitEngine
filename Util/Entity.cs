using System;
using System.Numerics;
using DumBitEngine.Core.Util;

namespace DumBitEngine.Util
{
    public abstract class Entity : IRenderable, IDisposable
    {
        public bool isActive;
        public string name;

        public GameObject parent;

        public Entity(string name, GameObject parent)
        {
            isActive = true;
            this.name = name;
            this.parent = parent;
        }

        public abstract void GetUiToDraw();


        public abstract void Awake();
        public abstract void Start();

        public abstract void Dispose();
        public abstract void Draw();
    }
}
