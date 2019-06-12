using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using Vector2 = System.Numerics.Vector2;

namespace DumBitEngine.Core.Util
{
    public class InputState
    {
        public KeyboardState keyboard;

        public bool isClicked;
        public bool isBeenClicked;
        public bool isClickedDown;

        public InputState()
        {
            isClicked = isBeenClicked = isClicked =  isClickedDown = false;
        }

        public void Update(KeyboardState updatedKeyboard)
        {
            keyboard = updatedKeyboard;
        }
    }
}