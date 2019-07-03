using System;
using System.Collections;
using System.Collections.Generic;
using Assimp;
using Assimp.Unmanaged;
using DumBitEngine.Core.Util;
using Matrix4x4 = System.Numerics.Matrix4x4;
using OpenTK;
using Camera = DumBitEngine.Core.Util.Camera;
using Scene = Assimp.Scene;
using ImGuiNET;
using System.Windows.Forms;

namespace DumBitEngine.Core.Shapes
{
    public class Model : Shape
    {
        private Shader shader;

        public List<Mesh> meshes;

        private string meshName;
        private string path;

        public bool isRotating;

        public Model(string path, string meshName) : base("mesh")
        {
            var asset = AssetLoader.UseElement(path + meshName);
            
            if (asset != null)
            {
                Model model = asset as Model;

                Console.WriteLine("Loading model from table");

                if (model != null)
                {
                    meshes = model.meshes;
                    shader = model.shader;

                    this.meshName = meshName;
                    this.path = path;
                    
                    transform = Matrix4x4.Identity;
                }
            }
            else
            {
                this.path = path;
                this.meshName = meshName;
                var context = new AssimpContext();

                shader = new Shader("Assets/Shaders/mesh.glsl");
                shader.Use();

                transform = Matrix4x4.Identity;

                shader.SetMatrix4("model", ref transform);
                shader.SetMatrix4("view", ref Camera.main.view);
                shader.SetMatrix4("projection", ref Camera.main.projection);
                
                shader.SetVector3("lightColor", ref Game.light.color);
                shader.SetVector3("light.lightPos",  Game.light.transform.Translation);

                meshes = new List<Mesh>();

                var scene = context.ImportFile(path + meshName,
                    PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs | PostProcessSteps.GenerateNormals);

                if (scene != null)
                {
                    ProcessNode(scene.RootNode, scene);
                }
                
                scene.Clear();
                
                AssetLoader.AddElement(path+meshName, this);
            }           
        }

        private void ProcessNode(Node node, Scene scene)
        {
            for (int i = 0; i < node.MeshCount; i++)
            {
                meshes.Add(ProcessMesh(scene.Meshes[node.MeshIndices[i]], scene));
            }

            for (int i = 0; i < node.ChildCount; i++)
            {
                ProcessNode(node.Children[i], scene);
            }
        }

        private Mesh ProcessMesh(Assimp.Mesh mesh, Scene scene)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<uint> indices = new List<uint>();
            List<Texture> textures = new List<Texture>();

            //loading vertex
            for (int i = 0; i < mesh.VertexCount; i++)
            {
                Vertex v;
                
                v.position.X = mesh.Vertices[i].X;
                v.position.Y = mesh.Vertices[i].Y;
                v.position.Z = mesh.Vertices[i].Z;
                
                v.normal.X = mesh.Normals[i].X;
                v.normal.Y = mesh.Normals[i].Y;
                v.normal.Z = mesh.Normals[i].Z;

                v.texCoord.X = mesh.TextureCoordinateChannels[0][i].X;
                v.texCoord.Y = mesh.TextureCoordinateChannels[0][i].Y;

                vertices.Add(v);
            }

            //loading indices
            for (int i = 0; i < mesh.FaceCount; i++)
            {
                var face = mesh.Faces[i];
                for (int j = 0; j < face.IndexCount; j++)
                {
                    indices.Add((uint)face.Indices[j]);
                }
            }
            
            //loading material
            if (mesh.MaterialIndex >= 0)
            {
                var mat = scene.Materials[mesh.MaterialIndex];

                List<Texture> texDiffuse = LoadMaterialTextures(mat, TextureType.Diffuse, "texture_diffuse");
                textures.AddRange(texDiffuse);

                List<Texture> texSpecular = LoadMaterialTextures(mat, TextureType.Specular, "texture_specular");
                textures.AddRange(texSpecular);
            }
            
            return new Mesh(vertices, indices, textures);
        }

        private List<Texture> LoadMaterialTextures(Material material, TextureType type, string typeName)
        {
            List<Texture> tex = new List<Texture>();

            var time = System.DateTime.Now;

            for (int i = 0; i < material.GetMaterialTextureCount(type); i++)
            {
                if (material.GetMaterialTexture(type, i, out var texSlot))
                {
                    //texSlot.FilePath. fix that, relative path
                    var texFullPath = texSlot.FilePath.Split('\\');
                    string texPath = texFullPath[texFullPath.Length - 1]; 
                    
                    Texture texture = new Texture(path + texPath, typeName);
                    tex.Add(texture);
                }
                
            }
            
           // var diff = DateTime.Now - time;
           // Console.WriteLine("Loading "+material.Name+" took : "+diff.Milliseconds);

            return tex;
        }

        public override void Awake()
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            if(AssetLoader.RemoveElement(path+meshName))
            {
                foreach (var mesh in meshes)
                {
                    mesh.Dispose();
                }

                shader.Dispose();
            }  
        }

        public override void Draw()
        {
            shader.Use();
            
            //Console.WriteLine(transform);
            if(isRotating)
                transform *= parent.transform * Matrix4x4.CreateRotationY(Time.deltaTime);
            
            shader.SetMatrix4("model", ref transform);
            shader.SetMatrix4("view", ref Camera.main.view);
            shader.SetVector3("viewPos", Camera.main.view.ExtractTranslation());
            shader.SetVector3("light.lightColor", ref Game.light.color);
            shader.SetMatrix4("projection", ref Camera.main.projection);
            shader.SetVector3("light.lightPos",  Game.light.transform.Translation);

            //Console.WriteLine(Game.light.transform.ExtractTranslation());
            
            shader.SetFloat("material.shininess", 32);
            
            foreach (var mesh in meshes)
            {
                mesh.Draw(ref shader);
            }
        }

        public override void GetUiToDraw()
        {
            ImGui.Text(name);

            var position = transform.Translation;
            ImGui.DragFloat3("Position", ref position);
            transform.Translation = position;

            if(ImGui.Button("Load Another Model"))
            {
                //TODO: make this crossplatform
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.ShowDialog();

                Console.WriteLine(fileDialog.FileName);
                Console.WriteLine(fileDialog.SafeFileName);

                string newPath = fileDialog.FileName.Replace('\\', '/');
                newPath = newPath.Remove(newPath.Length - fileDialog.SafeFileName.Length);
                Console.WriteLine(newPath);

                Model m = new Model(newPath, fileDialog.SafeFileName);

                AssetLoader.RemoveElement(path + meshName);

                meshes.Clear();
                meshes = m.meshes;
                meshName = fileDialog.SafeFileName;
                path = newPath;
            }
        }
    }
}