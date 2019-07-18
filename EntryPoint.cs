using System;

namespace DumBitEngine
{
    public class EntryPoint
    {

        private const bool isGame = true;
        
        [STAThread]
        static void Main(string[] args)
        {
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
}