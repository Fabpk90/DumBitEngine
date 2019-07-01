using System;
using System.Numerics;


namespace DumBitEngine.Core.Util
{
    public abstract class Entity : IRenderable, IDisposable
    {
        public bool isActive;
        public string name;
        
        public Matrix4x4 transform;
        public GameObject parent;

        public Entity(string name)
        {
            isActive = true;
            this.name = name;

            transform = Matrix4x4.Identity;
        }

        public abstract void GetUiToDraw();


        public abstract void Awake();
        public abstract void Start();

        public abstract void Dispose();
        public abstract void Draw();
    }
}
