using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK.Audio.OpenAL;

namespace DumBitEngine.Core.Sound
{
    public class AudioMaster
    {
        private static List<int> buffers;

        public  static void Init()
        {
            buffers = new List<int>();
            buffers.Add(LoadSound("Assets/Sound/bounce.wav"));
        }

        public static int LoadSound(string path)
        {
            int buffer = AL.GenSource();
           // AL.BufferData(buffer, ALFormat.Stereo8, );
           
            LoadWav(path);

            return buffer;
        }

        public static char[] LoadWav(string path)
        {
            List<char> data = new List<char>();
            BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open));
            
            
            
            return data.ToArray();
        }
    }
}