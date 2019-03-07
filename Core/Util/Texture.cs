using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL;
using StbSharp;
using Image = StbSharp.Image;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace DumBitEngine.Core.Util
{
    public class Texture : IDisposable
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

            ImageReader reader = new ImageReader();
            var stream = File.OpenRead(path);
            
            var img = reader.Read(stream);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Linear);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba
                , img.Width, img.Height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte, img.Data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            
            stream.Close();
        }

        public void Dispose()
        {
            
        }
    }
}