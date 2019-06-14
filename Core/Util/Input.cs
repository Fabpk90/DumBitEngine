
using System;
using System.Data;
using OpenTK;
using OpenTK.Input;
using Vector2 = System.Numerics.Vector2;

namespace DumBitEngine.Core.Util
{
    public static class Input
    {
        public static bool[] keyDown;
        public static bool[] keyDownLastFrame;
        
        private const int MAX_KEY_SIZE = (int) Key.NonUSBackSlash;

        private static KeyboardState keyboardState;
        private static KeyboardState keyboardStateLastFrame;

        private static MouseState mouseState;
        
        public static Vector2 MousePosition => new Vector2(mouseState.X, mouseState.Y);
        public static bool IsLeftMouseButtonDown => mouseState[0];

        public static void Init()
        {
            keyDown = new bool[MAX_KEY_SIZE];
            keyDownLastFrame = keyDown;

            keyboardStateLastFrame = Keyboard.GetState();
        }

        public static void Update()
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsAnyKeyDown)
            {
                for (int i = 0; i < MAX_KEY_SIZE; i++)
                {
                    keyDown[i] = keyboardState.IsKeyDown((Key) i);
                }
            }
        }

        public static bool IsKeyPressed(Key key)
        {
            return keyDown[(int) key];
        }

        public static bool GetKeyPressed(out Key keyPressed)
        {
            if (keyboardState.IsAnyKeyDown)
            {
                for (int i = 0; i < MAX_KEY_SIZE; i++)
                {
                    if (keyDown[i])
                    {
                        keyPressed = (Key) i;
                        return true;
                    }
                }
            }
            keyPressed = Key.A;
            return false;
        }

        public static void MouseEvent(MouseEventArgs args)
        {
            mouseState = args.Mouse;
        }
    }
}