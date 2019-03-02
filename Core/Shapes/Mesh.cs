using Assimp;
using Assimp.Unmanaged;

namespace DumBitEngine.Core.Shapes
{
    public class Mesh : Shape
    {
        private Scene scene;

        public Mesh(string path)
        {
            scene = new Scene();
            Assimp.AssimpContext importer = new AssimpContext();

            scene = importer.ImportFile(path);
            
        }
        
        public override void Dispose()
        {
            scene.Clear();
        }

        public override void Draw()
        {
            throw new System.NotImplementedException();
        }
    }
}