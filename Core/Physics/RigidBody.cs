using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using DumBitEngine.Core.Util;
using DumBitEngine.Util;
using ImGuiNET;

namespace DumBitEngine.Core.Physics
{
    public class RigidBody : Entity
    {
        public bool dynamic;
        public TypedIndex type;
        public Buffer<RigidPose> info;
        public int index;

        public RigidBody(string name, GameObject parent = null) : base(name, parent)
        {
            dynamic = false;
            type = new TypedIndex();

            if (parent != null)
            {
                parent.PositionChanged += OnParentOnPositionChanged;
            }
            
            Physics.AddBody(this);
        }

        private void OnParentOnPositionChanged(object sender, PositionChangedArgs args)
        {
            Physics.sim.Awakener.AwakenBody(0);
            info[index].Position += new Vector3(args.x, args.y, args.z);
        }

        public override void GetUiToDraw()
        {
            ImGui.Text(name);
            if (!ImGui.Checkbox("Active ?", ref isActive))
            {
                
            }
        }

        public override void Awake()
        {
            
        }

        public override void Start()
        {
            
        }

        public override void Dispose()
        {
            Parent.PositionChanged -= OnParentOnPositionChanged;
        }

        public override void Draw()
        {
            Parent.getMatrix4X4().Translation = info[index].Position;
        }

        protected override void OnAdded(GameObject go)
        {
            base.OnAdded(go);
            
            if(parent != null)
                parent.PositionChanged -= OnParentOnPositionChanged;
            
            go.PositionChanged += OnParentOnPositionChanged;
        }
    }
}