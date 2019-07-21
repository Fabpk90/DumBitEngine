using System;
using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;

namespace DumBitEngine.Util
{
    public class GameObject : Entity
    {
        private Matrix4x4 transform;
        private List<Entity> attachedComponent;

        public event PositionChanged PositionChanged;

        public GameObject(string name, GameObject parent = null) : base(name, parent)
        {
            attachedComponent = new List<Entity>();
            transform = Matrix4x4.Identity;
            this.Parent = parent;
        }

        public ref Matrix4x4 getMatrix4X4()
        {
            return ref transform;
        }

        public void SetPosition(float x, float y, float z)
        {
            transform.Translation = new Vector3(x, y ,z);
            
            PositionChanged?.Invoke(this, new PositionChangedArgs(x, y, z));
        }
        
        public void AddPosition(float x, float y, float z)
        {
            transform.Translation += new Vector3(x, y ,z);
            
            PositionChanged?.Invoke(this, new PositionChangedArgs(x, y, z));
        }

        public override void GetUiToDraw()
        {
            Entity entityToRemove = null;
            foreach (var item in attachedComponent)
            {
                item.GetUiToDraw();
                if (ImGui.Button("Delete"))
                {
                    entityToRemove = item;
                }
            }

            if(entityToRemove != null)
            {
                attachedComponent.Remove(entityToRemove);
            }
        }

        public void AddComponent(Entity ent)
        {
            //checks if there is not already an entity of the same type
            bool found = false;

            for (int i = 0; i < attachedComponent.Count && !found; i++)
            {
                if (attachedComponent[i].GetType() == ent.GetType())
                {
                    found = true;
                }
            }

            if (!found)
            {
                ent.Parent = this;
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

    public delegate void PositionChanged(object sender, PositionChangedArgs args);

    public struct PositionChangedArgs
    {
        public float x;
        public float y;
        public float z;

        public PositionChangedArgs(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}