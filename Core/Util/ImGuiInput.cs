using System;
using ImGuiNET;
using OpenTK.Input;

namespace DumBitEngine.Core.Util
{
    public class ImGuiInput
    {

        private ImGuiIOPtr io;
        
        public ImGuiInput()
        {
            io = ImGui.GetIO();
        }
        
        public void UpdateUI(ref InputState inputState)
        {
            io.DeltaTime = Time.deltaTime;

            if (Game.isCursorVisible)
            {
                io.MousePos = Game.mousePosition;
                io.MouseDown[0] = inputState.isClicked;

                if (inputState.keyboard.IsAnyKeyDown)
                {
                    var keyboard = inputState.keyboard;
                    //goes from A to Z (enum of opentk)
                    for (int i = 83; i < 109; i++)
                    {
                        if (keyboard.IsKeyDown((Key) i))
                        {
                            char c = (char)(i + 14);
                            io.AddInputCharacter(c);

                            Console.WriteLine(c);
                        }
                    }
                }
                
            }
            
        }
    }
}