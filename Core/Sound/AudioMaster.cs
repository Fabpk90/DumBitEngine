using System.Collections.Generic;
using OpenTK.Audio.OpenAL;

namespace DumBitEngine.Core.Sound
{
    public class AudioMaster
    {
        private static List<int> buffers;

        public  static void Init()
        {
            
            buffers = new List<int>();
        }

        public static void LoadSound(string path)
        {
            int buffer = AL.GenSource();
            
            
            buffers.Add(buffer);
        }
    }
}