using System;
using System.Collections;
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
            var asset = AssetLoader.UseElement(path + type);
            //checks if the texture has already been loaded
            if (asset != null)
            {
                Texture tex = (Texture) asset;

                id = tex.id;
                this.type = type;
                this.path = path;
                
                Console.WriteLine("Loading from hastable "+path+type);
            }
            else
            {
                this.path = path;
                this.type = type;
            
                GL.GenTextures(1, out id);
                GL.BindTexture(TextureTarget.Texture2D, id);

                ImageReader reader = new ImageReader();
                var stream = File.OpenRead(path);
            
                var img = reader.Read(stream);
                stream.Close();

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                    (int) TextureMinFilter.LinearMipmapLinear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                    (int) TextureMinFilter.Linear);

                GL.TexImage2D(TextureTarget.Texture2D, 0,
                    img.Comp == 3 ? PixelInternalFormat.Rgb : PixelInternalFormat.Rgba
                    , img.Width, img.Height, 0,
                    PixelFormat.Rgba, PixelType.UnsignedByte, img.Data);

                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

                
                GL.BindTexture(TextureTarget.Texture2D, 0);
                
                Console.WriteLine("Loading into hashable "+path+type);
                
                AssetLoader.AddElement(path+type, this);
            }
        }

        public void Dispose()
        {
            if (AssetLoader.RemoveElement(path+type))
            {
                GL.DeleteTexture(id);
                Console.WriteLine("Unloading " + path + type);
            }
        }
    }
}