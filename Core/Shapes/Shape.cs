using System.Numerics;
using DumBitEngine.Core.Util;
using OpenTK;

namespace DumBitEngine.Core.Shapes
{
    public abstract class Shape : Entity
    {
        
        //TODO: add new things here, like a transform maybe
        //insert a shader in there, with a corresponding dispose
        public Shape(string name) : base(name)
        {
           transform = Matrix4x4.Identity;
        }
    }
}