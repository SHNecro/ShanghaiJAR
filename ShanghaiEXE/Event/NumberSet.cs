using Common;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;
using System.Threading;

namespace NSEvent
{
    internal class NumberSet : EventBase
    {
        protected bool longwaiting = false;
        protected int faseflame = 0;
        protected int fasewait = 0;
        protected byte manyopen = 0;
        protected bool closing = false;
        protected int wait = 0;
        protected bool canskip = true;
        private NumberSet.SCENE nowscene;
        protected int faseseet;
        protected byte faseNo;
        protected NumberSet.FACEPATTERN fasepattern;
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
        private readonly string[] text;
        protected const byte waitlong = 10;
        protected const byte waitshort = 4;
        private bool enginit;
        private int waittime;
        private int returnNum;

        public NumberSet(
          IAudioEngine s,
          EventManager m,
          string text1,
          int fa,
          byte faNo,
          bool mo,
          int valNumber,
          int numberDigit,
          SaveData save)
          : base(s, m, save)
        {
            this.numberDigit = numberDigit;
            this.valNumber = valNumber;
            this.numver = new int[numberDigit];
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
            this.fasepattern = NumberSet.FACEPATTERN.mono;
        }

        public NumberSet(
          IAudioEngine s,
          EventManager m,
          string text1,
          FaceId face,
          bool mo,
          int valNumber,
          int numberDigit,
          SaveData save)
          : this(s, m, text1, face.Sheet, face.Index, mo, valNumber, numberDigit, save)
        {
        }

        protected void Init()
        {
            this.numver = new int[this.numberDigit];
            this.massage = this.text;
            this.nowscene = NumberSet.SCENE.printing;
            this.fasepattern = NumberSet.FACEPATTERN.neutral;
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
                case NumberSet.SCENE.printing:
                    ++this.printfonts;
                    if (this.printfonts > strArray[this.endprint].Length)
                    {
                        this.printfonts = 0;
                        ++this.endprint;
                        if (this.endprint < this.massage.Length)
                            break;
                        this.arrowprint = true;
                        this.nowscene = NumberSet.SCENE.pushA;
                        break;
                    }
                    // ISSUE: explicit reference operation
                    this.shortmassage[this.endprint] += strArray[this.endprint][this.printfonts - 1];
                    if (strArray[this.endprint][this.printfonts - 1] == "・" && !this.mono)
                    {
                        this.sound.PlaySE(SoundEffect.message);
                        this.wait = 30;
                        this.longwaiting = true;
                    }
                    else if (strArray[this.endprint][this.printfonts - 1] == "、" && !this.mono)
                    {
                        this.sound.PlaySE(SoundEffect.message);
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
                                        var saveThread = new Thread(new ThreadStart(() => this.savedata.SaveFile(this.manager.parent.parent)));
                                        saveThread.Start();
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
                                this.sound.PlaySE((SoundEffect)Enum.Parse(typeof(SoundEffect), s));
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
                        this.sound.PlaySE(SoundEffect.message);
                        this.wait = 0;
                        this.longwaiting = false;
                    }
                    this.nowscene = NumberSet.SCENE.wait;
                    break;
                case NumberSet.SCENE.wait:
                    --this.wait;
                    if (this.wait > 0)
                        break;
                    this.wait = 0;
                    this.nowscene = NumberSet.SCENE.printing;
                    break;
                case NumberSet.SCENE.pushA:
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
            if (Input.IsPress(Button._A) && this.nowDigit == -1)
            {
                this.sound.PlaySE(SoundEffect.decide);
                this.NumChange();
                this.savedata.ValList[this.valNumber] = this.returnNum;
                this.Init();
                this.EndCommand();
            }
            if (Input.IsPress(Button._B))
            {
                this.sound.PlaySE(SoundEffect.cancel);
                this.returnNum = -1;
                this.savedata.ValList[this.valNumber] = this.returnNum;
                this.Init();
                this.EndCommand();
            }
            if (this.waittime <= 0)
            {
                if (Input.IsPush(Button.Up) && this.nowDigit != -1)
                {
                    this.sound.PlaySE(SoundEffect.movecursol);
                    ++this.numver[this.nowDigit];
                    if (this.numver[this.nowDigit] > 9)
                        this.numver[this.nowDigit] = 0;
                    this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                }
                if (Input.IsPush(Button.Down) && this.nowDigit != -1)
                {
                    this.sound.PlaySE(SoundEffect.movecursol);
                    --this.numver[this.nowDigit];
                    if (this.numver[this.nowDigit] < 0)
                        this.numver[this.nowDigit] = 9;
                    this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                }
                if (Input.IsPush(Button.Left))
                {
                    this.sound.PlaySE(SoundEffect.movecursol);
                    ++this.nowDigit;
                    if (this.nowDigit >= this.numberDigit)
                        this.nowDigit = -1;
                    this.waittime = Input.IsPress(Button.Left) ? 10 : 4;
                }
                if (Input.IsPush(Button.Right))
                {
                    this.sound.PlaySE(SoundEffect.movecursol);
                    --this.nowDigit;
                    if (this.nowDigit < -1)
                        this.nowDigit = this.numberDigit - 1;
                    this.waittime = Input.IsPress(Button.Right) ? 10 : 4;
                }
            }
            else
                --this.waittime;
            this.shortmassage[1] = "";
        }

        private void NumChange()
        {
            int num = 1;
            this.returnNum = 0;
            for (int index = 0; index < this.numver.Length; ++index)
            {
                this.returnNum += this.numver[index] * num;
                num *= 10;
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
                dg.DrawText(this.shortmassage[index], this._position, this.savedata);
            }
            for (int index1 = 0; index1 <= this.numberDigit; ++index1)
            {
                string str = "";
                int index2 = this.numberDigit - 1 - index1;
                string text = index2 <= -1 ? str + "【OK 】" : str + this.numver[index2].ToString();
                this._position = new Vector2(56 + 17 * index1, 124f);
                dg.DrawText(text, this._position, this.savedata);
            }
            if (this.printfase && this.faseseet > 0)
            {
                this._position = new Vector2(5f, 108f);
                if (!this.mono)
                    this._rect = new Rectangle((int)this.fasepattern * 40, faseNo * 48, 40, 48);
                else
                    this._rect = new Rectangle(200, faseNo * 48, 40, 48);
                string te = "Face" + this.faseseet.ToString();
                dg.DrawImage(dg, te, this._rect, true, this._position, Color.White);
            }
            if (this.savedata != null)
            {
                if (!this.arrowprint || this.saving)
                    return;
                this._position = new Vector2(24 + (this.numberDigit - this.nowDigit) * 17, 124f);
                this._rect = new Rectangle(240 + this.frame % 3 * 16, 48, 16, 16);
                dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
            }
            else if (this.arrowprint)
            {
                this._position = new Vector2(224f, 140f);
                this._rect = new Rectangle(240 + this.frame % 3 * 16, 0, 16, 16);
                dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
            }
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
                case NumberSet.FACEPATTERN.neutral:
                    if (this.arrowprint)
                    {
                        this.fasepattern = NumberSet.FACEPATTERN.harfclose;
                        this.closing = true;
                        this.fasewait = 1;
                        break;
                    }
                    if (!this.longwaiting)
                    {
                        if (this.manyopen > 2)
                        {
                            this.fasepattern = NumberSet.FACEPATTERN.mouse1;
                            this.fasewait = 2;
                            this.manyopen = 0;
                        }
                        else
                        {
                            this.fasepattern = NumberSet.FACEPATTERN.mouse2;
                            this.fasewait = 4;
                            ++this.manyopen;
                        }
                    }
                    break;
                case NumberSet.FACEPATTERN.mouse1:
                case NumberSet.FACEPATTERN.mouse2:
                    this.fasepattern = NumberSet.FACEPATTERN.neutral;
                    this.fasewait = (byte)this.Random.Next(6);
                    break;
                case NumberSet.FACEPATTERN.harfclose:
                    if (this.closing)
                    {
                        this.fasepattern = NumberSet.FACEPATTERN.close;
                        this.fasewait = 7;
                        break;
                    }
                    this.fasepattern = NumberSet.FACEPATTERN.neutral;
                    this.fasewait = this.Random.Next(60, 300);
                    break;
                case NumberSet.FACEPATTERN.close:
                    this.fasepattern = NumberSet.FACEPATTERN.harfclose;
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
