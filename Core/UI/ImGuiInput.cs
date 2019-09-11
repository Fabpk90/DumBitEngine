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
        
        public void UpdateUI()
        {
            io.DeltaTime = Time.deltaTime;

            if (Game.isCursorVisible)
            {
                io.MousePos = MasterInput.MousePosition;
                io.MouseDown[0] = MasterInput.IsLeftMouseButtonDown;
                io.MouseWheel = MasterInput.MouseScroll;
                
                if (MasterInput.GetKeyPressed(out Key keyPressed))
                {
                    if (keyPressed == Key.BackSpace)
                    {
                        io.KeysDown[(int) Key.BackSpace] = true;
                    }
                    else
                    {
                        io.KeysDown[(int) Key.BackSpace] = false;
                        char c = (char) (keyPressed + 14);
                        io.AddInputCharacter(c);
                    }
                    
                }
                else
                {
                    io.KeysDown[(int) Key.BackSpace] = false;
                }
            }
            
        }
    }
}