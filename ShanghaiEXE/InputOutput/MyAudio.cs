﻿using NSGame;
using SlimDX.Multimedia;
using SlimDX.XAudio2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Tsukikage.Audio;
using NSTsukikage.WinMM.WaveIO;

namespace NSShanghaiEXE.InputOutput
{
    public class MyAudio : IDisposable
    {
        public static bool BGM = true;
        public static float volumeSE = 1f;
        public static float volumeBGM = 100f;
        public static float volumeParsent = 100f;
        private OggDecodeStream waveStream = null;
        private readonly MasteringVoice xaMaster = null;
        private readonly WaveStream xaStreamBGM = null;
        private readonly SourceVoice xaSourceBGM = null;
        private readonly AudioBuffer xaBufferBGM = null;
        private readonly Dictionary<string, WaveStream> xaStreamSE = new Dictionary<string, WaveStream>();
        private readonly Dictionary<string, SourceVoice> xaSourceSE = new Dictionary<string, SourceVoice>();
        private readonly Dictionary<string, AudioBuffer> xaBufferSE = new Dictionary<string, AudioBuffer>();
        private readonly List<string> soundNames = new List<string>();
        private readonly List<WaveStream> musicstream = new List<WaveStream>();
        private readonly List<string> musicNames = new List<string>();
        public ShanghaiEXE parent;
        public WaveOut waveOut;
        private byte music_timer;
        private bool musicPlay;
        public string playmusic;
        private readonly SlimDX.XAudio2.XAudio2 xaDevice;
        public int[,] musiclength;
        public SaveData savedata;
        private readonly Thread thread_1;
        private float plusParsent;
        private float endParsent;
        private bool fadeBGM;
        private bool disabled;

        public bool MusicPlay
        {
            get
            {
                return this.musicPlay;
            }
        }

        public MyAudio(ShanghaiEXE parent)
        {
            this.parent = parent;
            DirectoryInfo directoryInfo = new DirectoryInfo("music");
            int length = 0;
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (file.Name.Split('.')[1] == "ogg")
                {
                    ++length;
                    this.musicNames.Add(file.Name);
                }
            }
            this.musiclength = new int[2, length];
            try
            {
                this.waveOut = new WaveOut(-1, 44100, 16, 2);
                this.xaDevice = new SlimDX.XAudio2.XAudio2();
                this.xaMaster = new MasteringVoice(this.xaDevice);
                MyAudio.volumeSE = parent.volSE;
            }
            catch
            {
                this.disabled = true;
            }
            this.thread_1 = new Thread(new ThreadStart(this.Init));
            this.thread_1.Start();
        }

        public void Init()
        {
            string path1 = "music/looppoint.txt";
            if (!File.Exists(path1))
                return;
            StreamReader streamReader1 = new StreamReader(path1, Encoding.GetEncoding("Shift_JIS"));
            int index1 = 0;
            this.musicNames.Clear();
            string str;
            while ((str = streamReader1.ReadLine()) != null)
            {
                string[] strArray = str.Split(',');
                this.musiclength[0, index1] = int.Parse(strArray[0]);
                this.musiclength[1, index1] = int.Parse(strArray[1]);
                this.musicNames.Add(strArray[3] + ".ogg");
                ++index1;
            }
            streamReader1.Close();
            string path2 = "ShaSPattern.tcd";
            if (!File.Exists(path2))
                return;
            StreamReader streamReader2 = new StreamReader(path2, Encoding.GetEncoding("Shift_JIS"));
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            Masking.GenerateKeyFromPassword("sasanasi", rijndaelManaged.KeySize, out byte[] key, rijndaelManaged.BlockSize, out byte[] iv);
            FileStream fileStream = new FileStream("ShaSResource.tcd", FileMode.Open, FileAccess.Read);
            string sourceString;
            while ((sourceString = streamReader2.ReadLine()) != null)
            {
                List<byte> byteList = new List<byte>();
                string[] strArray = Masking.DecryptString(sourceString, "sasanasi").Split('_');
                int num1 = int.Parse(strArray[1]);
                if (!File.Exists(path2))
                    return;
                int num2;
                for (int index2 = 0; (num2 = fileStream.ReadByte()) != -1 && index2 < num1 - 1; ++index2)
                {
                    byte code = (byte)num2;
                    if (index2 > 1024)
                        byteList.Add(code);
                    else
                        byteList.Add(Masking.DecryptByte(code));
                }
                Stream stream = new MemoryStream(byteList.ToArray());
                if (strArray[0].Split('.')[1] == "wav")
                {
                    this.SoundMake(strArray[0], stream);
                    this.soundNames.Add(strArray[0]);
                }
            }
            fileStream.Close();
            streamReader2.Close();
            this.parent.soundLoad = true;
        }

        private void SoundMake(string soundname)
        {
            this.xaStreamSE.Add(soundname, new WaveStream(new FileStream("sound/" + soundname + ".wav", FileMode.Open)));
            this.xaSourceSE.Add(soundname, null);
            this.xaBufferSE.Add(soundname, null);
        }

        private void SoundMake(string soundname, Stream stream)
        {
            this.xaStreamSE.Add(soundname, new WaveStream(stream));
            this.xaSourceSE.Add(soundname, null);
            this.xaBufferSE.Add(soundname, null);
        }

        public void SetBGM(string name)
        {
            try
            {
                bool flag = false;
                if (File.Exists("mod/music/" + name.ToString() + ".txt"))
                {
                    StreamReader streamReader = new StreamReader("mod/music/" + name.ToString() + ".txt", Encoding.GetEncoding("Shift_JIS"));
                    List<byte> byteList = new List<byte>();
                    string[] strArray = new string[0];
                    string str;
                    while ((str = streamReader.ReadLine()) != null)
                        strArray = str.Split(',');
                    try
                    {
                        this.waveStream = new OggDecodeStream(File.OpenRead("mod/music/" + strArray[2]), int.Parse(strArray[0]), int.Parse(strArray[1]));
                        flag = true;
                    }
                    catch
                    {
                        flag = false;
                    }
                }
                if (!flag)
                {
                    string path = "music/" + name.ToString() + ".ogg";
                    string str = name.ToString() + ".ogg";
                    this.waveStream = new OggDecodeStream(File.OpenRead(path), this.musiclength[0, this.musicNames.IndexOf(str)], this.musiclength[1, this.musicNames.IndexOf(str)]);
                }
            }
            catch
            {
            }
            GC.Collect();
        }

        public void StartBGM(string name)
        {
            if (!MyAudio.BGM || !(name != this.playmusic))
                return;
            this.playmusic = name;
            this.SetBGM(name);
            this.musicPlay = true;
        }

        public void ReStartBGM()
        {
            this.musicPlay = true;
        }

        public void StopBGM()
        {
            this.playmusic = "none_";
            this.BGMVolumeSet(100);
            this.musicPlay = false;
        }

        public void StopSE(MyAudio.SOUNDNAMES sname)
        {
            string soundName = this.soundNames[(int)sname];
            if (this.xaSourceSE[soundName] != null)
                this.xaSourceSE[soundName].Dispose();
            if (this.xaBufferSE[soundName] == null)
                return;
            this.xaBufferSE[soundName].Dispose();
        }

        public void PlaySE(MyAudio.SOUNDNAMES sname)
        {
            if (this.disabled)
            {
                return;
            }
            if (sname == MyAudio.SOUNDNAMES.none)
                return;
            string soundName = this.soundNames[(int)sname];
            if (this.xaSourceSE[soundName] != null)
                this.xaSourceSE[soundName].Dispose();
            if (this.xaBufferSE[soundName] != null)
                this.xaBufferSE[soundName].Dispose();
            Dictionary<string, SourceVoice> xaSourceSe = this.xaSourceSE;
            string index = soundName;
            SourceVoice sourceVoice = new SourceVoice(this.xaDevice, this.xaStreamSE[soundName].Format)
            {
                Volume = MyAudio.volumeSE
            };
            xaSourceSe[index] = sourceVoice;
            this.xaBufferSE[soundName] = new AudioBuffer
            {
                AudioBytes = (int)this.xaStreamSE[soundName].Length,
                AudioData = this.xaStreamSE[soundName],
                LoopCount = 0,
                LoopBegin = 0,
                LoopLength = 0,
                Flags = BufferFlags.EndOfStream
            };
            this.xaSourceSE[soundName].SubmitSourceBuffer(this.xaBufferSE[soundName]);
            this.xaSourceSE[soundName].Start();
        }

        public void PlayingMusic()
        {
            ++this.music_timer;
            if (this.music_timer < 6)
                return;
            this.music_timer = 0;
            this.Fade();
        }

        public void Dispose()
        {
            this.xaMaster.Dispose();
            this.xaDevice.Dispose();
            if (this.xaBufferBGM != null)
                this.xaBufferBGM.Dispose();
            if (this.xaSourceBGM != null)
                this.xaSourceBGM.Dispose();
            if (this.xaStreamBGM != null)
                this.xaStreamBGM.Dispose();
            this.xaSourceSE.Clear();
            this.xaStreamSE.Clear();
            this.xaBufferSE.Clear();
        }

        private void Fade()
        {
            if (this.disabled)
            {
                return;
            }
            while (this.waveOut.EnqueuedBufferSize < 65536)
            {
                byte[] numArray = new byte[16384];
                int offset = 0;
                int count = 16384;
                int num1 = 100;
                if (float.IsNaN(MyAudio.volumeParsent))
                    MyAudio.volumeParsent = this.endParsent;
                float num2 = volumeParsent != 0.0 ? MyAudio.volumeBGM * (MyAudio.volumeParsent / 100f) : 0.0f;
                this.waveStream.Read(numArray, offset, count);
                int num3 = count / 2;
                int num4 = num3 * 2;
                for (int index = 0; index < num3; ++index)
                {
                    short num5 = (short)((short)(numArray[offset] | numArray[offset + 1] << 8) * (double)num2 / num1);
                    numArray[offset] = (byte)((uint)num5 & byte.MaxValue);
                    numArray[offset + 1] = (byte)(num5 >> 8 & byte.MaxValue);
                    offset += 2;
                }
                this.waveOut.Write(numArray);
            }
        }

        public void BGMFade()
        {
            if (!this.fadeBGM)
                return;
            MyAudio.volumeParsent += this.plusParsent;
            if (plusParsent < 0.0)
            {
                if (volumeParsent < (double)this.endParsent)
                {
                    MyAudio.volumeParsent = this.endParsent;
                    this.fadeBGM = false;
                }
            }
            else if (plusParsent > 0.0)
            {
                if (volumeParsent > (double)this.endParsent)
                {
                    MyAudio.volumeParsent = this.endParsent;
                    this.fadeBGM = false;
                }
            }
            else
                this.fadeBGM = false;
        }

        public void BGMFadeStart(int flame, int endparsent)
        {
            this.plusParsent = (endparsent - MyAudio.volumeParsent) / flame;
            this.endParsent = endparsent;
            this.fadeBGM = true;
        }

        public void BGMVolumeSet(int volume)
        {
            MyAudio.volumeParsent = volume;
            this.endParsent = volume;
            this.fadeBGM = false;
        }

        public enum SOUNDNAMES
        {
            alert,
            barrier,
            battleend,
            beam,
            beamlong,
            bird,
            bomb,
            bombbig,
            bombmiddle,
            bound,
            breakObject,
            bright,
            bubble,
            buster,
            cancel,
            canon,
            chain,
            charge,
            chargemax,
            Cheer,
            chime,
            chime2,
            clincher,
            CommandSuccess,
            conveyor,
            counterhit,
            damageenemy,
            damageplayer,
            damagezero,
            dark,
            death,
            decide,
            docking,
            dragonVoice,
            drill1,
            drill2,
            encount,
            enemydeath,
            enterenemy,
            eriasteal1,
            eriasteal2,
            error,
            fire,
            flash,
            fullcustom,
            futon,
            get,
            getchip,
            getzenny,
            gun,
            heat,
            knife,
            knock,
            lance,
            machineRunning,
            mail,
            menuclose,
            menuopen,
            message,
            movecursol,
            noise,
            openchip,
            pause,
            phone,
            pi,
            pikin,
            pinpon,
            quake,
            repair,
            rockon,
            rockopen,
            sand,
            search,
            shoot,
            shotwave,
            switchon,
            sword,
            teacharrow,
            thiptransmission,
            throw_,
            thunder,
            treader,
            vulcan,
            vulcan2,
            warp,
            water,
            wave,
            waveshort,
            Zblade,
            none,
        }
    }
}
