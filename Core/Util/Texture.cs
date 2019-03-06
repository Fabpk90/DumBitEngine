using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace DumBitEngine.Core.Util
{
    public struct Texture
    {
        public int id;
        public string type;
        public string path;

        public Texture(string path, string type)
        {
            this.path = path;
            this.type = type;
            
            GL.GenTextures(1, out id);
            GL.BindTexture(TextureTarget.Texture2D, id);
            
            var textureLoader = new Bitmap(path);
            BitmapData data = textureLoader.LockBits(new System.Drawing.Rectangle(0, 0, textureLoader.Width, textureLoader.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, textureLoader.Width, textureLoader.Height, 0,
                PixelFormat.Rgb, PixelType.UnsignedByte, data.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            
            textureLoader.UnlockBits(data);
            textureLoader.Dispose();
        }
    }
}