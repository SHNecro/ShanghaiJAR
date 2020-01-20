using SlimDX.Direct3D9;
using System;
using System.Collections.Generic;

namespace NSShanghaiEXE.InputOutput.Rendering.DirectX9
{
    public class GraphicsClass : IDisposable
    {
        protected Dictionary<string, Effect> Sharder = new Dictionary<string, Effect>();
        protected static Device device;
        public static Sprite sprite;

        public static Device Direct3DDevice
        {
            set
            {
                GraphicsClass.device = value;
            }
        }

        public static Sprite Direct3DSprite
        {
            set
            {
                GraphicsClass.sprite = value;
            }
        }

        public virtual void Dispose()
        {
        }
    }
}
