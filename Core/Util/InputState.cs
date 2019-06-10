using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using Vector2 = System.Numerics.Vector2;

namespace DumBitEngine.Core.Util
{
    public struct InputState
    {
        public List<Key> keysPressed;

        public bool isClicked;
    }
}