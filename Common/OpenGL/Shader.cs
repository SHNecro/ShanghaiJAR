using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Common.OpenGL
{
    public class Shader : IDisposable
    {
        private readonly int Handle;

        private readonly Dictionary<string, int> cachedUniformLocations = new Dictionary<string, int>();

        public Shader(string vertPath, string fragPath)
        {
            int VertexShader;
            int FragmentShader;

            string VertexShaderSource = LoadSource(vertPath);

            VertexShader = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(VertexShader, VertexShaderSource);

            GL.CompileShader(VertexShader);

            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != System.String.Empty)
                System.Console.WriteLine(infoLogVert);

            string FragmentShaderSource = LoadSource(fragPath);
            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);
            GL.CompileShader(FragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(VertexShader);
            if (infoLogFrag != System.String.Empty)
                System.Console.WriteLine(infoLogFrag);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);

            string infoLogLink = GL.GetProgramInfoLog(Handle);
            if (infoLogLink != System.String.Empty)
                System.Console.WriteLine(infoLogLink);

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        private string LoadSource(string path)
        {
            string readContents;

            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(path, StringComparison.InvariantCultureIgnoreCase));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                readContents = new StreamReader(stream).ReadToEnd();
            }

            return readContents;
        }

        public void SetInt(string name, int data)
        {
            var location = this.GetOrCacheUniformLocation(name);
            GL.Uniform1(location, data);
        }

        public void SetFloat(string name, float data)
        {
            var location = this.GetOrCacheUniformLocation(name);
            GL.Uniform1(location, data);
        }

        public void SetVector2(string name, Vector2 data)
        {
            var location = this.GetOrCacheUniformLocation(name);
            GL.Uniform2(location, data);
        }

        public void SetVector4(string name, Vector4 data)
        {
            var location = this.GetOrCacheUniformLocation(name);
            GL.Uniform4(location, data);
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            var location = this.GetOrCacheUniformLocation(name);
            GL.UniformMatrix4(location, true, ref data);
        }

        private int GetOrCacheUniformLocation(string name)
        {
            if (this.cachedUniformLocations.TryGetValue(name, out var location))
            {
                return location;
            }
            else
            {
                GL.UseProgram(Handle);
                var newLocation = GL.GetUniformLocation(Handle, name);
                this.cachedUniformLocations[name] = newLocation;
                return newLocation;
            }
        }

        //This section is dedicated to cleaning up the shader after it's finished.
        //Doing this solely in a finalizer results in a crash because of the Object-Oriented Language Problem
        //( https://www.khronos.org/opengl/wiki/Common_Mistakes#The_Object_Oriented_Language_Problem )
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
