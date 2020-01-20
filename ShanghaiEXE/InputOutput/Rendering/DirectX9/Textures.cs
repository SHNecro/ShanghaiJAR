using NSGame;
using SlimDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NSShanghaiEXE.InputOutput.Rendering.DirectX9
{
    public class Textures
    {
        public event EventHandler<TextureLoadProgressUpdatedEventArgs> ProgressUpdated;

        public static Dictionary<string, int[]> texSizeList = new Dictionary<string, int[]>();
        public List<string> texturenames = new List<string>();
        private readonly ShanghaiEXE parent;

        public Textures(ShanghaiEXE p)
        {
            this.parent = p;
        }

        public void Tex()
        {
            string path = "ShaGPattern.tcd";
            if (!File.Exists(path))
                return;
            StreamReader streamReader = new StreamReader(path, Encoding.GetEncoding("Shift_JIS"));
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            Masking.GenerateKeyFromPassword("sasanasi", rijndaelManaged.KeySize, out byte[] key, rijndaelManaged.BlockSize, out byte[] iv);
            FileStream fileStream = new FileStream("ShaGResource.tcd", FileMode.Open, FileAccess.Read);
            int num1 = 0;
            string sourceString;
            while ((sourceString = streamReader.ReadLine()) != null)
            {
                List<byte> byteList = new List<byte>();
                string message = Masking.DecryptString(sourceString, "sasanasi");
                string[] strArray = message.Split('_');
                int num2 = int.Parse(strArray[1]);
                if (!File.Exists(path))
                    return;
                int num3 = num2;
                Textures.texSizeList.Add(strArray[0], new int[2]
                {
                    num1,
                    int.Parse(strArray[1])
                });
                num1 += num3;
                this.ProgressUpdated?.Invoke(this, new TextureLoadProgressUpdatedEventArgs(strArray[0], (double)num1 / fileStream.Length));
            }
            fileStream.Close();
            streamReader.Close();
            for (int index = 0; index < this.parent.KeepTexList.Length; ++index)
            {
                this.ReadTex(this.parent.KeepTexList[index]);
            }
            this.ProgressUpdated?.Invoke(this, null);
        }

        public void ReadTex(string graphicsName)
        {
            List<byte> byteList = new List<byte>();
            FileStream fileStream = new FileStream("ShaGResource.tcd", FileMode.Open, FileAccess.Read);
            if (!this.parent.Tex.ContainsKey(graphicsName))
            {
                int num1 = 0;
                fileStream.Position = Textures.texSizeList[graphicsName][0];
                int num2;
                for (int index = Textures.texSizeList[graphicsName][1]; (num2 = fileStream.ReadByte()) != -1 && num1 < index - 1; ++num1)
                {
                    byte code = (byte)num2;
                    if (num1 > 1024)
                        byteList.Add(code);
                    else
                        byteList.Add(Masking.DecryptByte(code));
                }
                Stream Memory = new MemoryStream(byteList.ToArray());
                try
                {
                    this.parent.Tex.Add(graphicsName, new SlimTex(Memory, Usage.None));
                }
                catch
                {
                }
            }
            fileStream.Close();
            fileStream.Dispose();
        }
    }
}
