using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace DumBitEngine.Core.Util
{
    public class Scene : Entity
    {
        private List<Entity> sceneGraph;

        public Scene()
        {
            sceneGraph = new List<Entity>();
        }

        public void AddEntity(Entity entity)
        {
            sceneGraph.Add(entity);
        }

        public override void Awake()
        {
            foreach (Entity entity in sceneGraph)
            {
                entity.Awake();
            }
        }

        public override void Start()
        {
            foreach (Entity entity in sceneGraph)
            {
                entity.Start();
            }
        }

        public override void Dispose()
        {
            foreach (var entity in sceneGraph)
            {
                entity.Dispose();
            }

            sceneGraph.Clear();
        }

        private void DrawTreeUI()
        {
            //ImGui.Text("Yess");
            /*if (ImGui.TreeNode("Test"))
            {
                
            }*/
        }

        public override void Draw()
        {
            foreach (var entity in sceneGraph)
            {
                entity.Draw();
            }
            
            DrawTreeUI();
        }
    }
}
