using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DumBitEngine.Core.Util;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace DumBitEngine.Core.Sound
{
    public static class AudioMaster
    {
        private static List<int> buffers;
        private static AudioContext context;

        private static Source source;

        public  static void Init()
        {   
            context = new AudioContext();
            context.MakeCurrent();
            buffers = new List<int> {LoadSound("Assets/Sound/bounce.wav")};

            AL.Listener(ALListener3f.Position, ref Camera.main.cameraPos);
            AL.Listener(ALListener3f.Velocity, 0, 0, 0);
            
            source = new Source();
            source.Play(buffers[0]);
            
            Console.WriteLine(AL.GetError());
        }

        public static int LoadSound(string path)
        {
            int buffer = AL.GenBuffer();

            SoundClip clip = LoadWav(path);

            Console.WriteLine(clip.format.channels);
            Console.WriteLine(clip.format.bitDepth);
            
            AL.BufferData(buffer,GetSoundFormat(clip.format), clip.data, clip.data.Length, clip.format.sampleRate);
            
            Console.WriteLine(AL.GetError());
           

            return buffer;
        }

        private static ALFormat GetSoundFormat(WaveFormat clipFormat)
        {
            switch (clipFormat.channels)
            {
                case 1:
                    switch (clipFormat.bitDepth)
                    {
                        case 8:
                            return ALFormat.Mono8;

                        case 16:
                            return ALFormat.Mono16;
                    }
                    break;
                
                case 2:
                    switch (clipFormat.sampleRate)
                    {
                        case 8:
                            return ALFormat.Stereo8;

                        case 16:
                            return ALFormat.Stereo16;
                    }
                    break;
                default:
                    throw new NotSupportedException("The specified sound format is not supported.");
            }

            return ALFormat.Mono8;
        }

        public static SoundClip LoadWav(string path)
        {
            BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open));
            
            string fileFormat = new string(br.ReadChars(4));

            if (fileFormat != "RIFF")
            {
                Console.WriteLine("Hold on, this is not a wav");
            }
            
            //size of the entire file
            var sizeOfFile = br.ReadInt32();
            
            string signature = new string(br.ReadChars(4));

            if (signature != "WAVE")
            {
                Console.WriteLine("This thing is insane");
            }
            
            string subChunk = new string(br.ReadChars(4));

            if (subChunk != "fmt ")
            {
                Console.WriteLine("Hello mista offica");
            }

            var subChunkSize = br.ReadInt32();

            //this is the pcm, should be equals to 1, otherwise some compression is present
            var audioFormat = br.ReadInt16();

            //this is needed by openal
            var numChannel = br.ReadInt16();
            //this too
            var sampleRate = br.ReadInt32();

            var byteRate = br.ReadInt32();
            var blockAlign = br.ReadInt16();
            var bitSample = br.ReadInt16();
            
            string subChunk2 = new string(br.ReadChars(4));

            if (subChunk2 != "data")
            {
                Console.WriteLine("Wow this is really not working out huh ?");
            }

            var subChunk2Size = br.ReadInt32();

            byte[] data = br.ReadBytes((int)br.BaseStream.Length);

            WaveFormat format;
            format.channels = numChannel;
            format.sampleRate = sampleRate;
            format.bitDepth = bitSample;
            
            return new SoundClip(path, format, data);
        }
        
        public struct WaveFormat
        {
            // public string ChunkID;
            // public int FileSize;
            // public int RiffType;
            // public int FormatID;
            // public int FormatSize;
            // public int FormatExtraSize;
            // public int FormatCode;
            public int channels;
            public int sampleRate;
            // public int FormatAverageBps;
            // public int FormatBlockAlign;
            public int bitDepth;
            // public int DataID;
            // public int DataSize;
        }

        public static void Update()
        {
            var rotation = Camera.main.projection.ExtractRotation().Xyz;
           AL.Source(source.sourceID, ALSource3f.Position, ref Camera.main.cameraPos);
           AL.Source(source.sourceID, ALSource3f.Direction, ref rotation);
        }
    }
}