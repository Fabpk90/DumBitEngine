using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumBitEngine.Core.Util
{
    class Scene : IRenderable
    {
        public List<IRenderable> sceneGraph;

        public Scene()
        {
            sceneGraph = new List<IRenderable>();
        }

        public void AddRenderable(IRenderable renderable)
        {
            sceneGraph.Add(renderable);
        }

        public void Draw()
        {
            for (int i = 0; i < sceneGraph.Count; i++)
            {
                sceneGraph[i].Draw();
            }
        }
    }
}
