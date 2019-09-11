using System;
using DumBitEngine.Core.Physics;
using DumBitEngine.Core.Sound;
using DumBitEngine.Core.Util;

namespace DumBitEngine
{
    public class EntryPoint
    {
        private const bool isGame = true;
        
        static void Main(string[] args)
        {
            Engine.StartSystems();
            
            if (isGame)
            {
                new Game().Run();
            }
            else
            {
                new Editor.Editor().Run();
            }
        }
    }

    public static class Engine
    {
        public static void StartSystems()
        {
            MasterInput.Init();
            Physics.Init();
            AudioMaster.Init();
        }
    }
}