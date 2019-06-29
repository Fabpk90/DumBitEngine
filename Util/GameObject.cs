using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using OpenTK;

namespace DumBitEngine.Core.Util
{
    public class GameObject : Entity
    {
        private List<Entity> attachedComponent;
        public Matrix4x4 transform;

        public GameObject(string name) : base(name)
        {
            attachedComponent = new List<Entity>();
            transform = Matrix4x4.Identity;
        }

        public void AddComponent(Entity ent)
        {
            //checks if there is not already an entity of the same type
            bool found = false;

            for (int i = 0; i < attachedComponent.Count && !found; i++)
            {
                if (attachedComponent[i] == ent)
                {
                    found = true;
                }
            }

            if (!found)
            {
                ent.parent = this;
                attachedComponent.Add(ent);
            }
            else
            {
                Console.WriteLine("This component " + ent.name + " is already added to " + name);
            }
            
        }

        public T GetComponent<T>() where T : class
        {
            foreach (Entity entity in attachedComponent)
            {
                if (entity is T entity1)
                {
                    return entity1;
                }
            }

            return null;
        }

        public void RemoveComponent<T>()
        {
            for (int i = 0; i < attachedComponent.Count; i++)
            {
                if (attachedComponent[i] is T)
                {
                    attachedComponent.RemoveAt(i);
                }
            }
        }

        public override void Awake()
        {
            foreach (Entity entity in attachedComponent)
            {
                entity.Awake();
            }
        }

        public override void Start()
        {
            foreach (Entity entity in attachedComponent)
            {
                entity.Start();
            }
        }

        public override void Dispose()
        {
            foreach (Entity entity in attachedComponent)
            {
                entity.Dispose();
            }
        }

        public override void Draw()
        {
            foreach (Entity entity in attachedComponent)
            {
                entity.Draw();
            }
        }
    }
}