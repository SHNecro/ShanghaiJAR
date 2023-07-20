using Common;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace NSEvent
{
    internal class CommandMessage : EventBase
    {
        protected bool longwaiting = false;
        protected int faseflame = 0;
        protected int fasewait = 0;
        protected byte manyopen = 0;
        protected bool closing = false;
        protected int wait = 0;
        protected bool canskip = true;
        private CommandMessage.SCENE nowscene;
        protected int faseseet;
        protected byte faseNo;
        protected CommandMessage.FACEPATTERN fasepattern;
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
		protected int autoFrame;
		protected int autoFrameRaw;
		private bool saving;
        private bool noTalk;
        protected string[] text;
        private List<EventManager> parallelEventManagers;

		public CommandMessage(
          IAudioEngine s,
          EventManager m,
          string text1,
          string text2,
          string text3,
          bool fast,
          SaveData save)
          : base(s, m, save)
        {
            this.fastprint = fast;
            this.text = new string[3] { text1, text2, text3 };
            this.Init();
        }

        public CommandMessage(
          IAudioEngine s,
          EventManager m,
          string text1,
          string text2,
          string text3,
          int fa,
          byte faNo,
          SaveData save)
          : base(s, m, save)
        {
            this.faseseet = fa;
            this.faseNo = faNo;
            this.printfase = true;
            this.text = new string[3] { text1, text2, text3 };
            this.Init();
        }

        public CommandMessage(
          IAudioEngine s,
          EventManager m,
          string text1,
          string text2,
          string text3,
          FaceId face,
          SaveData save)
          : this(s, m, text1, text2, text3, face.Sheet, face.Index, face.Mono, face.Auto, save)
        {
        }

        public CommandMessage(
          IAudioEngine s,
          EventManager m,
          string text1,
          string text2,
          string text3,
          int fa,
          byte faNo,
          bool mo,
          bool auto,
          SaveData save)
          : base(s, m, save)
        {
            this.faseseet = fa;
            this.faseNo = faNo;
            this.printfase = true;
            this.text = new string[3] { text1, text2, text3 };
            this.Init();
            if (auto)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.auto;
			}
            else if (mo)
            {
                this.FacePattern = CommandMessage.FACEPATTERN.mono;
            }
        }

        public CommandMessage(
          IAudioEngine s,
          EventManager m,
          string text1,
          string text2,
          string text3,
          FaceId face,
          bool mo,
          bool auto,
          SaveData save)
          : this(s, m, text1, text2, text3, face.Sheet, face.Index, mo, auto, save)
        {
        }

        public CommandMessage(
          IAudioEngine s,
          EventManager m,
          string text1,
          string text2,
          string text3,
          bool fast,
          int fa,
          byte faNo,
          bool mo,
          bool auto,
          SaveData save)
          : base(s, m, save)
        {
            this.faseseet = fa;
            this.faseNo = faNo;
            this.printfase = true;
            this.text = new string[3] { text1, text2, text3 };
            this.Init();
            this.fastprint = fast;
			if (auto)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.auto;
			}
			else if (mo)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.mono;
			}
		}

        public CommandMessage(
          IAudioEngine s,
          EventManager m,
          string text1,
          string text2,
          string text3,
          bool fast,
          FaceId face,
          bool mo,
          SaveData save)
          : this(s, m, text1, text2, text3, fast, face.Sheet, face.Index, mo, face.Auto, save)
        {
		}

		protected bool EmoteDisabled => this.FacePattern == FACEPATTERN.mono || this.FacePattern == FACEPATTERN.auto;
        protected FACEPATTERN FacePattern
        {
            get => this.fasepattern;
            set
            {
                if (!this.EmoteDisabled)
                {
                    this.fasepattern = value;
                }
            }
        }

		protected void Init()
        {
            this.massage = this.text;
            this.nowscene = CommandMessage.SCENE.printing;
            this.FacePattern = CommandMessage.FACEPATTERN.neutral;
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
            this.shortmassage = new string[3] { "", "", "" };
            this.parallelEventManagers = new List<EventManager>();
        }

        public override void Update()
        {
            this.autoFrameRaw++;
            if (this.autoFrameRaw++ >= 5)
            {
                this.autoFrameRaw = 0;

                this.autoFrame = (this.autoFrame + 1) % 6;
            }

            string[][] strArray = new string[3][]
            {
                this.ToDecomposition(this.massage[0]),
                this.ToDecomposition(this.massage[1]),
                this.ToDecomposition(this.massage[2])
            };
            foreach (var parallelEvents in this.parallelEventManagers.Where(em => em.playevent))
            {
                parallelEvents.UpDate();
            }
            if (this.manager.alpha <= 0 && this.canskip && this.massage[0].Length > 0 && (Input.IsPress(Button._B) || Input.IsPress(Button._A) || this.fastprint))
            {
                this.endprint = strArray.Length - 1;
                this.printfonts = strArray[strArray.Length - 1].Length + 1;
                this.shortmassage = this.massage;
            }
            switch (this.nowscene)
            {
                case CommandMessage.SCENE.printing:
                    ++this.printfonts;
                    if (this.endprint >= strArray.Length || this.printfonts > strArray[this.endprint].Length)
                    {
                        this.printfonts = 0;
                        ++this.endprint;
                        var parallelComplete = this.parallelEventManagers.All(em => !em.playevent);
                        var endOfMessage = this.endprint >= this.massage.Length;
                        this.noTalk = endOfMessage;
                        if (parallelComplete && endOfMessage)
                        {
                            this.arrowprint = true;
                            this.nowscene = CommandMessage.SCENE.pushA;
                            break;
                        }
                        break;
                    }
                    // ISSUE: explicit reference operation
                    this.shortmassage[this.endprint] += strArray[this.endprint][this.printfonts - 1];
                    var inEllipsesFunc = new Func<string[], Func<string, bool>, int, int>(
                        (str, ellipseCheck, index) => {
                            if (index < 0 || index >= str.Length || !ellipseCheck(str[index])) return 0;
                            var seq = 1;
                            for (int d = 1; index + d < str.Length && ellipseCheck(str[index + d]); d += 1)
                            {
                                seq += 1;
                            }

                            for (int d = 1; index - d >= 0 && ellipseCheck(str[index - d]); d += 1)
                            {
                                seq += 1;
                            }

                            return seq;
                        });
                    var interpunctEllipseLength = inEllipsesFunc(strArray[this.endprint], s => s == "・", this.printfonts - 1);
                    var periodEllipseLength = inEllipsesFunc(strArray[this.endprint], s => s == ".", this.printfonts - 1);
                    var shortpause = strArray[this.endprint][this.printfonts - 1] == "、" || strArray[this.endprint][this.printfonts - 1] == "，";
                    var thinkStart = strArray[this.endprint][this.printfonts - 1] == "（" || strArray[this.endprint][this.printfonts - 1] == "(";
                    if ((interpunctEllipseLength > 0 || periodEllipseLength > 1) && !this.EmoteDisabled)
                    {
                        this.sound.PlaySE(SoundEffect.message);
                        this.wait = 30 / Math.Max(interpunctEllipseLength, periodEllipseLength);
                        this.longwaiting = true;
                    }
                    else if (shortpause && !this.EmoteDisabled)
                    {
                        this.sound.PlaySE(SoundEffect.message);
                        this.wait = 15;
                        this.longwaiting = true;
                    }
                    else if (thinkStart && !this.EmoteDisabled)
                    {
                        this.noTalk = true;
                    }
                    else
                    {
                        try
                        {
                            var thinkStop = strArray[this.endprint][this.printfonts - 2] == "）" || strArray[this.endprint][this.printfonts - 2] == ")";
                            if (thinkStop && !this.EmoteDisabled)
                                this.noTalk = false;
                        }
                        catch
                        {
                        }
                        if (strArray[this.endprint][this.printfonts - 1] == "#")
                        {
                            string s = "";
                            for (int index = 2; index < 100 && (this.printfonts + index < strArray[this.endprint].Length && !(strArray[this.endprint][this.printfonts + index] == "#")); ++index)
                                s += strArray[this.endprint][this.printfonts + index];
                            string str = strArray[this.endprint][this.printfonts];
                            switch (str)
                            {
                                case "s":
                                    try
                                    {
                                        this.sound.PlaySE((SoundEffect)Enum.Parse(typeof(SoundEffect), s));
                                    }
                                    catch { }
                                    break;
                                case "w":
                                    try
                                    {
                                        this.wait = int.Parse(s);
                                        this.longwaiting = true;
                                    }
                                    catch { }
                                    break;
                                case "b":
                                    try
                                    {
                                        this.canskip = bool.Parse(s);
                                    }
                                    catch { }
                                    break;
                                case "u":
                                    var saveThread = new Thread(new ThreadStart(() => this.savedata.SaveFile(this.manager.parent.parent)));
                                    this.manager.parent.main.FolderSave();
                                    this.savedata.saveEnd = false;
                                    saveThread.Start();
                                    this.saving = true;
                                    break;
                                case "e":
                                    this.Init();
                                    this.EndCommand();
                                    return;
                                case "p":
                                    var newEventManager = new EventManager(this.manager.parent, this.sound);
                                    newEventManager.AddEvent(new RunEvent(this.sound, newEventManager, s, -1, newEventManager.parent, newEventManager.parent.Field, this.savedata));
                                    this.parallelEventManagers.Add(newEventManager);
                                    break;
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
                    }
                    this.nowscene = CommandMessage.SCENE.wait;
                    break;
                case CommandMessage.SCENE.wait:
                    --this.wait;
                    if (this.wait <= 0)
                    {
                        this.wait = 0;
                        this.nowscene = CommandMessage.SCENE.printing;
                        break;
                    }
                    break;
                case CommandMessage.SCENE.pushA:
                    this.FlameControl(4);
                    if (this.savedata != null)
                    {
                        if (this.savedata.saveEnd)
                        {
                            if (!this.endok)
                            {
                                if (this.frame > 1)
                                {
                                    this.frame = 0;
                                    this.endok = true;
                                    break;
                                }
                                break;
                            }
                            if (this.frame > 2)
                                this.frame = 0;
                            if (this.manager.alpha <= 0 && (Input.IsPress(Button._A) || Input.IsPress(Button._B) || this.saving))
                            {
                                this.Init();
                                this.EndCommand();
                                return;
                            }
                            break;
                        }
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
                        if (this.manager.alpha <= 0 && (Input.IsPress(Button._A) || Input.IsPress(Button._B)))
                        {
                            this.Init();
                            this.EndCommand();
                            return;
                        }
                    }
                    break;
            }
            if (!this.printfase || this.EmoteDisabled)
                return;
            this.FaseAnimation();
        }

        public override void Render(IRenderer dg)
        {
            foreach (var parallelEvents in this.parallelEventManagers.Where(em => em.playevent))
            {
                parallelEvents.Render(dg);
            }
            this._position = new Vector2(0.0f, 104f);
            this._rect = new Rectangle(0, 0, 240, 56);
            dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
            for (int index = 0; index < this.massage.Length; ++index)
            {
                this._position = new Vector2(48f, 108 + 16 * index);
                dg.DrawText(this.shortmassage[index], this._position, this.savedata);
            }
            if (this.printfase && this.faseseet > 0)
            {
                this._position = new Vector2(5f, 108f);
                if (!this.EmoteDisabled)
				{
					this._rect = new Rectangle((int)this.FacePattern * 40, faseNo * 48, 40, 48);
				}
                else if (this.FacePattern == FACEPATTERN.auto)
				{
					this._rect = new Rectangle(40 * this.autoFrame, faseNo * 48, 40, 48);
				}
				else if (this.FacePattern == FACEPATTERN.mono)
				{
					this._rect = new Rectangle(200, faseNo * 48, 40, 48);
				}
				string te = "Face" + this.faseseet.ToString();
                dg.DrawImage(dg, te, this._rect, true, this._position, Color.White);
            }
            if (this.savedata != null)
            {
                if (!this.arrowprint || this.saving)
                    return;
                this._position = new Vector2(224f, 140f);
                this._rect = new Rectangle(240 + this.frame % 3 * 16, 0, 16, 16);
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
			this.autoFrameRaw++;
			if (this.autoFrameRaw++ >= 5)
			{
				this.autoFrameRaw = 0;

				this.autoFrame = (this.autoFrame + 1) % 6;
			}

			++this.faseflame;
            if (this.faseflame <= this.fasewait)
                return;
            this.faseflame = 0;
            switch (this.FacePattern)
            {
                case CommandMessage.FACEPATTERN.neutral:
                    if (this.arrowprint)
                    {
                        this.FacePattern = CommandMessage.FACEPATTERN.harfclose;
                        this.closing = true;
                        this.fasewait = 1;
                        break;
                    }
                    if (!this.longwaiting && !this.noTalk)
                    {
                        if (this.manyopen > 2)
                        {
                            this.FacePattern = CommandMessage.FACEPATTERN.mouse1;
                            this.fasewait = 2;
                            this.manyopen = 0;
                        }
                        else
                        {
                            this.FacePattern = CommandMessage.FACEPATTERN.mouse2;
                            this.fasewait = 4;
                            ++this.manyopen;
                        }
                    }
                    break;
                case CommandMessage.FACEPATTERN.mouse1:
                case CommandMessage.FACEPATTERN.mouse2:
                    this.FacePattern = CommandMessage.FACEPATTERN.neutral;
                    this.fasewait = (byte)this.Random.Next(6);
                    break;
                case CommandMessage.FACEPATTERN.harfclose:
                    if (this.closing)
                    {
                        this.FacePattern = CommandMessage.FACEPATTERN.close;
                        this.fasewait = 7;
                        break;
                    }
                    this.FacePattern = CommandMessage.FACEPATTERN.neutral;
                    this.fasewait = this.Random.Next(60, 300);
                    break;
                case CommandMessage.FACEPATTERN.close:
                    this.FacePattern = CommandMessage.FACEPATTERN.harfclose;
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
            auto,
        }
    }
}
