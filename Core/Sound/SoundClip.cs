
namespace DumBitEngine.Core.Sound
{
    public class SoundClip
    {
        public string path;
        public int buffer;
        public float volume;

        //TODO: make this an interface of some sort
        public AudioMaster.WaveFormat format;
        
        public byte[] data;
        
        public SoundClip(string path, AudioMaster.WaveFormat format, byte[] data)
        {
            this.data = data;
            this.path = path;
            this.format = format;
        }
    }
}