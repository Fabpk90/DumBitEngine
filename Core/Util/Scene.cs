﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumBitEngine.Core.Util
{
    class Scene : Entity
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

        public override void Dispose()
        {
            foreach (var entity in sceneGraph)
            {
                entity.Dispose();
            }

            sceneGraph.Clear();
        }

        public override void Draw()
        {
            foreach (var entity in sceneGraph)
            {
                entity.Draw();
            }
        }
    }
}
