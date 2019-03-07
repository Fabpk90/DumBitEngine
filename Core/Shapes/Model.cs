using System;
using System.Collections.Generic;
using Assimp;
using Assimp.Unmanaged;
using DumBitEngine.Core.Util;
using OpenTK;

namespace DumBitEngine.Core.Shapes
{
    public class Model : Shape
    {
        private Shader shader;
        private Matrix4 modelMatrix;
        
        private Scene scene;
        public List<Mesh> meshes;

        private string meshName;
        private string path;

        public Model(string path, string meshName)
        {
            this.path = path;
            this.meshName = meshName;
            var context = new AssimpContext();
            
            shader = new Shader("Assets/Shaders/mesh.glsl");
            shader.Use();
            
            modelMatrix = Matrix4.Identity;
            modelMatrix *= Matrix4.CreateTranslation(0, -1.75f, 0);
            modelMatrix *= Matrix4.CreateScale(.2f, .2f, .2f);
            
            shader.SetMatrix4("model", ref modelMatrix);
            shader.SetMatrix4("view", ref Game.view);
            shader.SetMatrix4("projection", ref Game.projection);
            
            meshes = new List<Mesh>();
            
            scene = context.ImportFile(path+meshName,
                PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs | PostProcessSteps.GenerateNormals);

            if (scene != null)
            {
                ProcessNode(scene.RootNode, scene);
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

            for (int i = 0; i < material.GetMaterialTextureCount(type); i++)
            {
                TextureSlot texSlot;
                if (material.GetMaterialTexture(type, i, out texSlot))
                {
                    Texture texture = new Texture(path + texSlot.FilePath, typeName);
                    tex.Add(texture);
                }
                
            }

            return tex;
        }

        public override void Dispose()
        {
            scene.Clear();

            foreach (var mesh in meshes)
            {
               mesh.Dispose();
            }
            
        }

        public override void Draw()
        {
            shader.Use();
            
            shader.SetMatrix4("view", ref Game.view);
            
            foreach (var mesh in meshes)
            {
                mesh.Draw(ref shader);
            }
        }
    }
}