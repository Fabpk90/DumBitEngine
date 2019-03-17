using System;
using System.Collections;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DumBitEngine.Core.Util
{
    public class Shader : IDisposable
    {
        public static Hashtable table = new Hashtable();
        public int ProgramId { get; }

        private string path;

        public Shader(string path)
        {
            if (table.ContainsKey(path))
            {
                Shader shader = (Shader)table[path];
                ProgramId = shader.ProgramId;
            }
            else
            {
                this.path = path;
                
                string fragmentShader, vertexShader;

                ProgramId = GL.CreateProgram();
            
                ParseShader(path, out vertexShader, out fragmentShader);

                int fs = CompileShader(fragmentShader, ShaderType.FragmentShader);
                int vs = CompileShader(vertexShader, ShaderType.VertexShader);
            
                GL.AttachShader(ProgramId, fs);
                GL.AttachShader(ProgramId, vs);

            
                GL.LinkProgram(ProgramId);
                GL.ValidateProgram(ProgramId);

                //clean intermediates
                GL.DeleteShader(vs);
                GL.DeleteShader(fs);
                
                table.Add(path, this);
            }
            
           
        }
        private void ParseShader(string path, out string shaderVertex, out string shaderFragment)
        {
            StreamReader r = new StreamReader(path);

            string[] buffers = new string[2];
            int typeParsing = -1; //0 == vertex 1 == fragment

            while (!r.EndOfStream)
            {
                var line = r.ReadLine();
                if (line.Contains("#shader"))
                {
                    if (line.Contains("vertex"))
                    {
                        typeParsing = 0;
                    }
                    else if (line.Contains("fragment"))
                    {
                        typeParsing = 1;
                    }
                }
                else
                {
                    buffers[typeParsing] += line + "\n";
                }
            }

            shaderVertex = buffers[0];
            shaderFragment = buffers[1];
        }
        
        private int CompileShader(string source, ShaderType type)
        {
            int id = GL.CreateShader(type);

            GL.ShaderSource(id, source);
            GL.CompileShader(id);

            string log = "";
            GL.GetShaderInfoLog(id, out log);

            if (log != "")
            {
                Console.WriteLine(log);
                GL.DeleteShader(id);
            }

            return id;
        }

        public void Use()
        {
            GL.UseProgram(ProgramId);
        }

        public void SetInt(string name, int value)
        {   
            GL.Uniform1(GL.GetUniformLocation(ProgramId, name), value);
        }

        public void SetMatrix4(string name, ref Matrix4 matrix)
        {
            GL.UniformMatrix4(GL.GetUniformLocation(ProgramId, name),false, ref matrix);
        }

        public void Dispose()
        {  
            if (table.ContainsKey(path))
            {
                GL.DeleteProgram(ProgramId);
                table.Remove(path);

                Console.WriteLine("Unloading shader at: "+path+" remaining "+table.Count);
            }         
        }
    }
}