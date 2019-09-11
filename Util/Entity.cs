using System;
using System.IO;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using DumBitEngine.Core.Util;
using JsonSubTypes;
using Newtonsoft.Json;

namespace DumBitEngine.Util
{
    [JsonConverter(typeof(JsonSubtypes), "EntityType")]
    public abstract class Entity : IRenderable, IDisposable
    {
        public bool isActive;
        public string name;

        protected string EntityType = "Test";

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
        public virtual void Serialize(StreamWriter sw, JsonSerializer serializer)
        {
            serializer.Serialize(sw, isActive);
            serializer.Serialize(sw, name);
        }
    }
}
