using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DumBitEngine.Core.Shapes;
using ImGuiNET;

namespace DumBitEngine.Core.Util
{
    public class Scene : Entity
    {
        private List<GameObject> sceneGraph;
        private GameObject selectedObject;

        public Scene(string name) : base(name)
        {
            sceneGraph = new List<GameObject>();
        }

        public void Add(GameObject entity)
        {
            sceneGraph.Add(entity);
        }

        public override void Awake()
        {
            foreach (GameObject gameObject in sceneGraph)
            {
                gameObject.Awake();
            }
        }

        public override void Start()
        {
            foreach (GameObject gameObject in sceneGraph)
            {
                gameObject.Start();
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
            ImGui.Text("Yess");
            if (ImGui.TreeNode("Objects of the scene"))
            {
                foreach (GameObject entity in sceneGraph)
                {
                    if (ImGui.Button(entity.name))
                    {
                        selectedObject = entity;
                    }
                        
                }
            }

            if (selectedObject != null)
            {
                var vec = selectedObject.GetComponent<Model>().transform.Translation;
                ImGui.DragFloat3("Position", ref vec);
                selectedObject.GetComponent<Model>().transform.Translation = vec;
            }
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
