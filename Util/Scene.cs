using System.Collections.Generic;
using System.IO;
using System.Numerics;
using DumBitEngine.Core.Util;
using ImGuiNET;
using Newtonsoft.Json;

namespace DumBitEngine.Util
{
    public class Scene : Entity
    {
        private List<GameObject> sceneGraph;
        private GameObject selectedObject;

        public List<LightSource> lightSources;

        private string sceneName;

        public Scene(string name) : base(name, null)
        {
            sceneGraph = new List<GameObject>();
            sceneName = name;
            lightSources = new List<LightSource>();
        }

        public Vector3 GetSceneColor()
        {
            if(lightSources.Count == 0)
                return Vector3.Zero;;

            //TODO: compute for each light maybe ?
            return lightSources[0].color;
        }

        public void Add(GameObject entity)
        {
            sceneGraph.Add(entity);
        }

        public GameObject Get(int index)
        {
            return sceneGraph[index];
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


            if (ImGui.Button("Add GO"))
            {
                GameObject go = new GameObject("GameObject");
                sceneGraph.Add(go);
            }

            if (selectedObject != null)
            {
                selectedObject.GetUiToDraw();

                if (ImGui.Button("Add Component"))
                {
                    
                }
            }

           
            ImGui.InputText("Scene name: ", ref sceneName, 250);
            if (ImGui.Button("Save Scene"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Include;
                serializer.Formatting = Formatting.Indented;
                
                using (StreamWriter sw = new StreamWriter(sceneName + ".level"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, sceneGraph);
                    // {"ExpiryDate":new Date(1230375600000),"Price":0}
                }
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

        public override void GetUiToDraw()
        {
            
        }
    }
}
