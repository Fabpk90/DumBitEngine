using DumBitEngine.Core.Util;
using OpenTK;

namespace DumBitEngine.Core.Shapes
{
    public abstract class Shape : Entity
    {
        public Matrix4 transform;
        //TODO: add new things here, like a transform maybe
        //insert a shader in there, with a corresponding dispose
    }
}