using System;
using System.Collections.Generic;
using OpenTK;

namespace DumBitEngine.Core.Util
{
    public class GameObject : Entity
    {
        public bool isActive;
        public Matrix4 transform;
        public List<Entity> attachedComponent;

        public GameObject()
        {
            attachedComponent = new List<Entity>();
            transform = Matrix4.Identity;
            isActive = true;
        }

        public void AddComponent(Entity ent)
        {
            attachedComponent.Add(ent);
        }

        public void RemoveComponent(Entity ent)
        {
            //TODO
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