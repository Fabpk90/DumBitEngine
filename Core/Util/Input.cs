
using System;
using System.Data;
using OpenTK;
using OpenTK.Input;
using Vector2 = System.Numerics.Vector2;

namespace DumBitEngine.Core.Util
{
    public static class Input
    {
        private static bool[] keyPressed;
        private static bool[] keyPressedLastFrame;

        private static bool[] keyDown;
        private static bool[] keyDownCanBeActivated;
        
        private const int MAX_KEY_SIZE = (int) Key.NonUSBackSlash;

        private static KeyboardState keyboardState;
        private static KeyboardState keyboardStateLastFrame;

        private static MouseState mouseState;
        
        public static Vector2 MousePosition => new Vector2(mouseState.X, mouseState.Y);
        public static bool IsLeftMouseButtonDown => mouseState[0];

        public static void Init()
        {
            keyPressed = new bool[MAX_KEY_SIZE];
            keyDown = new bool[MAX_KEY_SIZE];
            keyPressedLastFrame = new bool[MAX_KEY_SIZE];
            keyDownCanBeActivated = new bool[MAX_KEY_SIZE];

            //by default, all keys can activated for one time press
            for (int i = 0; i < MAX_KEY_SIZE; i++)
            {
                keyDownCanBeActivated[i] = true;
            }

            keyboardStateLastFrame = Keyboard.GetState();
        }

        /// <summary>
        /// Update the state of the keyDown and keyPressed buffer
        /// </summary>
        public static void Update()
        {
            keyboardState = Keyboard.GetState();

            for (int i = 0; i < MAX_KEY_SIZE; i++)
            {
                keyPressed[i] = keyboardState.IsKeyDown((Key) i);

                if (keyPressed[i] == false)
                {
                    keyDown[i] = false;
                    keyDownCanBeActivated[i] = true;
                }
                else if (keyPressed[i] == true)
                {
                    if(keyDown[i] == false && keyDownCanBeActivated[i])
                    {
                        keyDown[i] = true;
                        keyDownCanBeActivated[i] = false;
                    }
                    else if (keyDown[i])
                    {
                        keyDown[i] = false;
                    }
                }
            }
        }

        public static bool IsKeyPressed(Key key)
        {
            return keyPressed[(int) key];
        }

        public static bool IsKeyDown(Key key)
        {
            return keyDown[(int) key];
        }

        /// <summary>
        /// Retrieves the first key pressed during this frame
        /// </summary>
        /// <param name="keyPressed"></param>
        /// <returns>The key pressed during the frame</returns>
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