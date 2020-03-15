using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSNet;
using SlimDX;
using System;
using System.Drawing;
using System.Threading;

namespace NSEvent
{
    internal class IPset : EventBase
    {
        protected bool longwaiting = false;
        protected int faseflame = 0;
        protected int fasewait = 0;
        protected byte manyopen = 0;
        protected bool closing = false;
        protected int wait = 0;
        protected bool canskip = true;
        private IPset.SCENE nowscene;
        protected int faseseet;
        protected byte faseNo;
        protected IPset.FACEPATTERN fasepattern;
        protected bool printfase;
        protected const byte opentime = 4;
        protected const byte closetime = 7;
        protected string[] massage;
        protected int printfonts;
        protected string[] shortmassage;
        protected int endprint;
        protected const int standardwait = 0;
        protected const int longwait = 30;
        protected const int speed = 4;
        protected bool fastprint;
        protected bool arrowprint;
        protected bool endok;
        protected bool mono;
        private bool saving;
        private int[] numver;
        private readonly int valNumber;
        private readonly int numberDigit;
        private int nowDigit;
        private readonly int ID;
        private readonly string[] text;
        protected const byte waitlong = 10;
        protected const byte waitshort = 4;
        private Thread thread_1;
        private bool enginit;
        private int waittime;
        private long returnNum;

        public IPset(
          MyAudio s,
          EventManager m,
          string text1,
          byte fa,
          byte faNo,
          bool mo,
          SaveData save,
          int ID = -1)
          : base(s, m, save)
        {
            this.ID = ID;
            this.numberDigit = 12;
            this.numver = new int[this.numberDigit];
            this.faseseet = fa;
            this.faseNo = faNo;
            this.printfase = true;
            var dialogue = ShanghaiEXE.Translate("NumberSet.SetNumberFormat").Format(text1);
            this.text = new string[3]
            {
                dialogue[0],
                dialogue[1],
                dialogue[2]
            };
            this.Init();
            this.fastprint = true;
            this.mono = mo;
            if (!this.mono)
                return;
            this.fasepattern = IPset.FACEPATTERN.mono;
        }

        protected void Init()
        {
            this.numver = new int[this.numberDigit];
            this.massage = this.text;
            this.nowscene = IPset.SCENE.printing;
            this.fasepattern = IPset.FACEPATTERN.neutral;
            this.endprint = 0;
            this.printfonts = 0;
            this.arrowprint = false;
            this.faseflame = 0;
            this.frame = 0;
            this.manyopen = 0;
            this.fasewait = 0;
            this.wait = 0;
            this.shortmassage = new string[3];
            this.canskip = true;
            this.longwaiting = false;
            this.nowDigit = this.numberDigit - 1;
            this.shortmassage = new string[3] { "", "", "" };
            if (NetParam.ConnectIP < 0L)
                return;
            for (int index = 0; index < NetParam.ConnectIPAddress.Length; ++index)
                this.numver[index] = int.Parse(NetParam.ConnectIPAddress[NetParam.ConnectIPAddress.Length - 1 - index].ToString());
        }

        public override void Update()
        {
            if (!this.enginit)
            {
                this.Init();
                this.enginit = true;
            }
            if (this.printfase && !this.mono)
                this.FaseAnimation();
            string[][] strArray = new string[3][]
            {
        this.ToDecomposition(this.massage[0]),
        this.ToDecomposition(this.massage[1]),
        this.ToDecomposition(this.massage[2])
            };
            if (this.manager.alpha <= 0 && this.canskip && this.massage[0].Length > 0 && (Input.IsPress(Button._B) || Input.IsPress(Button._A) || this.fastprint))
            {
                this.endprint = strArray.Length - 1;
                this.printfonts = strArray[strArray.Length - 1].Length + 1;
                this.shortmassage = this.massage;
            }
            switch (this.nowscene)
            {
                case IPset.SCENE.printing:
                    ++this.printfonts;
                    if (this.printfonts > strArray[this.endprint].Length)
                    {
                        this.printfonts = 0;
                        ++this.endprint;
                        if (this.endprint < this.massage.Length)
                            break;
                        this.arrowprint = true;
                        this.nowscene = IPset.SCENE.pushA;
                        break;
                    }
                    // ISSUE: explicit reference operation
                    this.shortmassage[this.endprint] += strArray[this.endprint][this.printfonts - 1];
                    if (strArray[this.endprint][this.printfonts - 1] == "・" && !this.mono)
                    {
                        this.sound.PlaySE(MyAudio.SOUNDNAMES.message);
                        this.wait = 30;
                        this.longwaiting = true;
                    }
                    else if (strArray[this.endprint][this.printfonts - 1] == "、" && !this.mono)
                    {
                        this.sound.PlaySE(MyAudio.SOUNDNAMES.message);
                        this.wait = 15;
                        this.longwaiting = true;
                    }
                    else if (strArray[this.endprint][this.printfonts - 1] == "#")
                    {
                        string s = "";
                        for (int index = 2; index < 100 && (this.printfonts + index < strArray[this.endprint].Length && !(strArray[this.endprint][this.printfonts + index] == "#")); ++index)
                            s += strArray[this.endprint][this.printfonts + index];
                        string str = strArray[this.endprint][this.printfonts];
                        if (!(str == "s"))
                        {
                            if (!(str == "w"))
                            {
                                if (!(str == "b"))
                                {
                                    if (!(str == "u"))
                                    {
                                        if (str == "e")
                                        {
                                            this.Init();
                                            this.EndCommand();
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        this.manager.parent.main.FolderReset();
                                        this.thread_1 = new Thread(new ThreadStart(this.savedata.SaveFile));
                                        this.thread_1.Start();
                                        this.saving = true;
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        this.canskip = bool.Parse(s);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    this.wait = int.Parse(s);
                                    this.longwaiting = true;
                                }
                                catch
                                {
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                this.sound.PlaySE((MyAudio.SOUNDNAMES)Enum.Parse(typeof(MyAudio.SOUNDNAMES), s));
                            }
                            catch
                            {
                            }
                        }
                        do
                        {
                            ++this.printfonts;
                            // ISSUE: explicit reference operation
                            this.shortmassage[this.endprint] += strArray[this.endprint][this.printfonts - 1];
                        }
                        while (strArray[this.endprint][this.printfonts - 1] != "#");
                    }
                    else
                    {
                        this.sound.PlaySE(MyAudio.SOUNDNAMES.message);
                        this.wait = 0;
                        this.longwaiting = false;
                    }
                    this.nowscene = IPset.SCENE.wait;
                    break;
                case IPset.SCENE.wait:
                    --this.wait;
                    if (this.wait > 0)
                        break;
                    this.wait = 0;
                    this.nowscene = IPset.SCENE.printing;
                    break;
                case IPset.SCENE.pushA:
                    this.FlameControl(4);
                    if (this.savedata != null)
                    {
                        if (!this.savedata.saveEnd)
                            break;
                        if (!this.endok)
                        {
                            if (this.frame <= 1)
                                break;
                            this.frame = 0;
                            this.endok = true;
                            break;
                        }
                        if (this.frame > 2)
                            this.frame = 0;
                        if (this.manager.alpha <= 0)
                            this.Control();
                        break;
                    }
                    if (!this.endok)
                    {
                        if (this.frame > 1)
                        {
                            this.frame = 0;
                            this.endok = true;
                        }
                    }
                    else
                    {
                        if (this.frame > 2)
                            this.frame = 0;
                        if (this.manager.alpha <= 0)
                            this.Control();
                    }
                    break;
            }
        }

        private void Control()
        {
            if (Input.IsPress(Button._B))
            {
                this.sound.PlaySE(MyAudio.SOUNDNAMES.cancel);
                this.returnNum = -1L;
                NetParam.IP = this.returnNum;
                this.Init();
                this.EndCommand();
            }
            else if (this.nowDigit == -1)
            {
                if (Input.IsPress(Button._A))
                {
                    this.sound.PlaySE(MyAudio.SOUNDNAMES.decide);
                    this.NumChange();
                    NetParam.IP = this.returnNum;
                    Debug.DebugMess(NetParam.GetIPAddress(NetParam.IP));
                    this.Init();
                    this.EndCommand();
                }
            }
            else
            {
                int num1 = -1;
                for (int num2 = 0; num2 < 10; ++num2)
                {
                    if (Input.NumPress(num2))
                    {
                        num1 = num2;
                        break;
                    }
                }
                if (num1 >= 0)
                {
                    this.numver[this.nowDigit] = num1;
                    this.sound.PlaySE(MyAudio.SOUNDNAMES.decide);
                    --this.nowDigit;
                }
                else if (this.waittime <= 0)
                {
                    if (Input.IsPush(Button.Up) && this.nowDigit != -1)
                    {
                        this.sound.PlaySE(MyAudio.SOUNDNAMES.movecursol);
                        ++this.numver[this.nowDigit];
                        if (this.numver[this.nowDigit] > 9)
                            this.numver[this.nowDigit] = 0;
                        this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                    }
                    if (Input.IsPush(Button.Down) && this.nowDigit != -1)
                    {
                        this.sound.PlaySE(MyAudio.SOUNDNAMES.movecursol);
                        --this.numver[this.nowDigit];
                        if (this.numver[this.nowDigit] < 0)
                            this.numver[this.nowDigit] = 9;
                        this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                    }
                    if (Input.IsPush(Button.Left))
                    {
                        this.sound.PlaySE(MyAudio.SOUNDNAMES.movecursol);
                        ++this.nowDigit;
                        if (this.nowDigit >= this.numberDigit)
                            this.nowDigit = -1;
                        this.waittime = Input.IsPress(Button.Left) ? 10 : 4;
                    }
                    if (Input.IsPush(Button.Right))
                    {
                        this.sound.PlaySE(MyAudio.SOUNDNAMES.movecursol);
                        --this.nowDigit;
                        if (this.nowDigit < -1)
                            this.nowDigit = this.numberDigit - 1;
                        this.waittime = Input.IsPress(Button.Right) ? 10 : 4;
                    }
                }
                else
                    --this.waittime;
            }
            this.shortmassage[1] = "";
        }

        private void NumChange()
        {
            long num = 1;
            this.returnNum = 0L;
            for (int index = 0; index < this.numver.Length; ++index)
            {
                this.returnNum += this.numver[index] * num;
                num *= 10L;
            }
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(0.0f, 104f);
            this._rect = new Rectangle(0, 0, 240, 56);
            dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
            for (int index = 0; index < this.massage.Length; ++index)
            {
                this._position = new Vector2(48f, 108 + 16 * index);
                if (index < 2)
                    dg.DrawText(this.shortmassage[index], this._position, this.savedata);
                else
                    dg.DrawMiniText(this.shortmassage[index], this._position, Color.FromArgb(byte.MaxValue, 64, 56, 56), this.savedata);
            }
            int num = 0;
            for (int index1 = 0; index1 <= this.numberDigit; ++index1)
            {
                string str = "";
                int index2 = this.numberDigit - 1 - index1;
                string text = index2 <= -1 ? str + "【OK 】" : str + this.numver[index2].ToString();
                this._position = new Vector2(48 + 8 * index1 + num, 124f);
                if (index1 == this.numberDigit - this.nowDigit - 1)
                {
                    Color red = Color.Red;
                    dg.DrawText(text, this._position, red, this.savedata);
                }
                else
                    dg.DrawText(text, this._position, this.savedata);
                if (index1 % 3 == 2)
                {
                    num += 8;
                    this._position = new Vector2(48 + 8 * index1 + num, 124f);
                    dg.DrawText(".", this._position, this.savedata);
                }
            }
            if (!this.printfase || this.faseseet <= 0)
                return;
            this._position = new Vector2(5f, 108f);
            if (!this.mono)
                this._rect = new Rectangle((int)this.fasepattern * 40, faseNo * 48, 40, 48);
            else
                this._rect = new Rectangle(200, faseNo * 48, 40, 48);
            string te = "Face" + this.faseseet.ToString();
            dg.DrawImage(dg, te, this._rect, true, this._position, Color.White);
        }

        protected string[] ToDecomposition(string text)
        {
            char[] charArray = text.ToCharArray();
            string[] strArray = new string[charArray.Length];
            for (int index = 0; index < charArray.Length; ++index)
                strArray[index] = charArray[index].ToString();
            return strArray;
        }

        protected void FaseAnimation()
        {
            ++this.faseflame;
            if (this.faseflame <= this.fasewait)
                return;
            this.faseflame = 0;
            switch (this.fasepattern)
            {
                case IPset.FACEPATTERN.neutral:
                    if (this.arrowprint)
                    {
                        this.fasepattern = IPset.FACEPATTERN.harfclose;
                        this.closing = true;
                        this.fasewait = 1;
                        break;
                    }
                    if (!this.longwaiting)
                    {
                        if (this.manyopen > 2)
                        {
                            this.fasepattern = IPset.FACEPATTERN.mouse1;
                            this.fasewait = 2;
                            this.manyopen = 0;
                        }
                        else
                        {
                            this.fasepattern = IPset.FACEPATTERN.mouse2;
                            this.fasewait = 4;
                            ++this.manyopen;
                        }
                    }
                    break;
                case IPset.FACEPATTERN.mouse1:
                case IPset.FACEPATTERN.mouse2:
                    this.fasepattern = IPset.FACEPATTERN.neutral;
                    this.fasewait = (byte)this.Random.Next(6);
                    break;
                case IPset.FACEPATTERN.harfclose:
                    if (this.closing)
                    {
                        this.fasepattern = IPset.FACEPATTERN.close;
                        this.fasewait = 7;
                        break;
                    }
                    this.fasepattern = IPset.FACEPATTERN.neutral;
                    this.fasewait = this.Random.Next(60, 300);
                    break;
                case IPset.FACEPATTERN.close:
                    this.fasepattern = IPset.FACEPATTERN.harfclose;
                    this.closing = false;
                    this.fasewait = 1;
                    break;
            }
        }

        private enum SCENE
        {
            printing,
            wait,
            pushA,
        }

        public enum FACEPATTERN
        {
            neutral,
            mouse1,
            mouse2,
            harfclose,
            close,
            mono,
        }
    }
}
