using System;
using OpenTK.Audio.OpenAL;

namespace DumBitEngine.Core.Sound
{
    public class Source : IDisposable
    {
        public int sourceID;

        public Source()
        {
            sourceID = AL.GenSource();
            
            AL.Source(sourceID, ALSourcef.Pitch, 1f);
            AL.Source(sourceID, ALSourcef.Gain, 1f);
            AL.Source(sourceID, ALSource3f.Position, 0, 0, 2);
        }

        public void Play(int buffer)
        {
            AL.Source(sourceID, ALSourcei.Buffer, buffer);
            AL.Source(sourceID, ALSourceb.Looping, true);
            AL.SourcePlay(sourceID);
        }

        public void Dispose()
        {
            AL.DeleteSource(sourceID);
        }
    }
}