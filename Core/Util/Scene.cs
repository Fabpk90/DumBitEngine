using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumBitEngine.Core.Util
{
    class Scene : Entity
    {
        public List<Entity> sceneGraph;

        public Scene()
        {
            sceneGraph = new List<Entity>();
        }

        public void AddRenderable(Entity entity)
        {
            sceneGraph.Add(entity);
        }

        public override void Dispose()
        {
            for (int i = 0; i < sceneGraph.Count; i++)
            {
                sceneGraph[i].Dispose();
            }
        }

        public override void Draw()
        {
            for (int i = 0; i < sceneGraph.Count; i++)
            {
                sceneGraph[i].Draw();
            }
        }
    }
}
