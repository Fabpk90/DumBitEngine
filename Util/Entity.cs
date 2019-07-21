using System;
using System.Numerics;
using DumBitEngine.Core.Util;

namespace DumBitEngine.Util
{
    public abstract class Entity : IRenderable, IDisposable
    {
        public bool isActive;
        public string name;

        protected GameObject parent;
        public GameObject Parent
        {
            get { return parent; }
            set
            {
                if(value != null)
                    OnAdded(value);
                
                parent = value;
            }
        }

        public Entity(string name, GameObject parent)
        {
            isActive = true;
            this.name = name;
            this.Parent = parent;
        }

        public abstract void GetUiToDraw();

        public T GetComponent<T>() where T : class
        {
            return Parent.GetComponent<T>();
        }

        /// <summary>
        /// Called when added to a GO
        /// </summary>
        protected virtual void OnAdded(GameObject go)
        {
            //TODO: find a better solution to notify physics objects
        }
        
        
        public abstract void Awake();
        public abstract void Start();

        public abstract void Dispose();
        public abstract void Draw();
    }
}
