using Common;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;
using Button = NSShanghaiEXE.InputOutput.Button;

namespace NSEvent
{
    internal class Question : CommandMessage
    {
        private readonly Vector2[] cursolposi = new Vector2[4];
        private bool cansel = false;
        private readonly bool height = false;
        private Question.SCENE nowscene;
        private readonly int manyQuestion;
        private int cursor;
        private readonly bool cancel;

        public Question(
          IAudioEngine s,
          EventManager m,
          string text1,
          string text2,
          string q1,
          string q2,
          bool fast,
          bool pfase,
          int fa,
          byte faNo,
		  // TODO:
		  bool mono,
          bool auto,
          SaveData save)
          : base(s, m, text1, text2, "　" + q1 + "　　" + q2, false, save)
        {
            this.faseseet = fa;
            this.faseNo = faNo;
            this.printfase = pfase;
            this.manyQuestion = 2;
            this.fastprint = fast;

            this.cursolposi[0] = new Vector2(40f, 140f);

            var optionSize = ShanghaiEXE.measurer.MeasureRegularText("　" + q1 + "　");
            this.cursolposi[1] = new Vector2(40 + optionSize.Width, 140f);
			if (auto)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.auto;
			}
			else if (mono)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.mono;
			}
		}

        public Question(
          IAudioEngine s,
          EventManager m,
          string text1,
          string text2,
          string q1,
          string q2,
          bool fast,
          bool pfase,
          FaceId face,
          SaveData save)
          : this(s, m, text1, text2, q1, q2, fast, pfase, face.Sheet, face.Index, face.Mono, face.Auto, save)
        {
        }

        public Question(
          IAudioEngine s,
          EventManager m,
          string text1,
          string q1,
          string q2,
          bool fast,
          bool h,
          bool pfase,
          int fa,
          byte faNo,
		  bool mono,
		  bool auto,
		  SaveData save)
          : base(s, m, text1, "　" + q1, "　" + q2, false, save)
        {
            this.faseseet = fa;
            this.faseNo = faNo;
            this.printfase = pfase;
            this.manyQuestion = 2;
            this.fastprint = fast;
            this.height = true;
            Graphics.FromImage(new Bitmap(100, 100));

            this.cursolposi[0] = new Vector2(40f, 124f);
            this.cursolposi[1] = new Vector2(40f, 140f);
			if (auto)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.auto;
			}
			else if (mono)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.mono;
			}
		}

        public Question(
          IAudioEngine s,
          EventManager m,
          string text1,
          string q1,
          string q2,
          bool fast,
          bool h,
          bool pfase,
          FaceId face,
          SaveData save)
          : this(s, m, text1, q1, q2, fast, h, pfase, face.Sheet, face.Index, face.Mono, face.Auto, save)
        {
        }

        public Question(
          IAudioEngine s,
          EventManager m,
          string q1,
          string q2,
          string q3,
          bool fast,
          bool pfase,
          int fa,
          byte faNo,
		  bool mono,
		  bool auto,
		  SaveData save)
          : base(s, m, "　" + q1, "　" + q2, "　" + q3, false, save)
        {
            this.faseseet = fa;
            this.faseNo = faNo;
            this.printfase = pfase;
            this.manyQuestion = 3;
            this.fastprint = fast;
            this.cursolposi[0] = new Vector2(40f, 108f);
            this.cursolposi[1] = new Vector2(40f, 124f);
            this.cursolposi[2] = new Vector2(40f, 140f);
			if (auto)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.auto;
			}
			else if (mono)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.mono;
			}
		}

        public Question(
          IAudioEngine s,
          EventManager m,
          string q1,
          string q2,
          string q3,
          bool fast,
          bool pfase,
          FaceId face,
          SaveData save)
          : this(s, m, q1, q2, q3, fast, pfase, face.Sheet, face.Index, face.Mono, face.Auto, save)
        {
        }

        public Question(
          IAudioEngine s,
          EventManager m,
          string text1,
          string q1,
          string q2,
          string q3,
          string q4,
          bool fast,
          bool pfase,
          int fa,
          byte faNo,
		  bool mono,
		  bool auto,
		  SaveData save)
          : base(s, m, text1, "　" + q1 + "　　" + q2, "　" + q3 + "　　" + q4, false, save)
        {
            this.faseseet = fa;
            this.faseNo = faNo;
            this.printfase = pfase;
            this.manyQuestion = 4;
            this.fastprint = fast;
            Graphics graphics = Graphics.FromImage(new Bitmap(100, 100));

            this.cursolposi[0] = new Vector2(40f, 124f);
            var option1Size = ShanghaiEXE.measurer.MeasureMiniText("　" + q1 + "　");
            var option3Size = ShanghaiEXE.measurer.MeasureMiniText("　" + q3 + "　");
            this.cursolposi[1] = new Vector2(40 + option1Size.Width, 124);
            this.cursolposi[2] = new Vector2(40, 140f);
            this.cursolposi[3] = new Vector2(40 + option3Size.Width, 140f);
			if (auto)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.auto;
			}
			else if (mono)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.mono;
			}
		}

        public Question(
          IAudioEngine s,
          EventManager m,
          string text1,
          string q1,
          string q2,
          string q3,
          string q4,
          bool fast,
          bool pfase,
          FaceId face,
          SaveData save)
          : this(s, m, text1, q1, q2, q3, q4, fast, pfase, face.Sheet, face.Index, face.Mono, face.Auto, save)
        {
        }

        public Question(
          IAudioEngine s,
          EventManager m,
          string text1,
          string text2,
          string q1,
          string q2,
          bool fast,
          bool pfase,
          int fa,
          byte faNo,
		  bool mono,
		  bool auto,
		  SaveData save,
          bool cancel)
          : base(s, m, text1, text2, "　" + q1 + "　　" + q2, false, save)
        {
            this.cancel = cancel;
            this.faseseet = fa;
            this.faseNo = faNo;
            this.printfase = pfase;
            this.manyQuestion = 2;
            this.fastprint = fast;
            Graphics graphics = Graphics.FromImage(new Bitmap(100, 100));
            
            this.cursolposi[0] = new Vector2(40f, 140f);
            var optionSize = ShanghaiEXE.measurer.MeasureRegularText("　" + q1 + "　");
            this.cursolposi[1] = new Vector2(40 + optionSize.Width, 140f);
			if (auto)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.auto;
			}
			else if (mono)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.mono;
			}
		}

        public Question(
          IAudioEngine s,
          EventManager m,
          string text1,
          string text2,
          string q1,
          string q2,
          bool fast,
          bool pfase,
          FaceId face,
          SaveData save,
          bool cancel)
          : this(s, m, text1, text2, q1, q2, fast, pfase, face.Sheet, face.Index, face.Mono, face.Auto, save, cancel)
        {
        }

        public Question(
          IAudioEngine s,
          EventManager m,
          string text1,
          string q1,
          string q2,
          bool fast,
          bool h,
          bool pfase,
          int fa,
          byte faNo,
		  bool mono,
		  bool auto,
		  SaveData save,
          bool cancel)
          : base(s, m, text1, "　" + q1, "　" + q2, false, save)
        {
            this.cancel = cancel;
            this.faseseet = fa;
            this.faseNo = faNo;
            this.printfase = pfase;
            this.manyQuestion = 2;
            this.fastprint = fast;
            this.height = true;
            Graphics.FromImage(new Bitmap(100, 100));

            this.cursolposi[0] = new Vector2(40f, 124f);
            this.cursolposi[1] = new Vector2(40f, 140f);
			if (auto)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.auto;
			}
			else if (mono)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.mono;
			}
		}

        public Question(
          IAudioEngine s,
          EventManager m,
          string text1,
          string q1,
          string q2,
          bool fast,
          bool h,
          bool pfase,
          FaceId face,
          SaveData save,
          bool cancel)
          : this(s, m, text1, q1, q2, fast, h, pfase, face.Sheet, face.Index, face.Mono, face.Auto, save, cancel)
        {
        }

        public Question(
          IAudioEngine s,
          EventManager m,
          string q1,
          string q2,
          string q3,
          bool fast,
          bool pfase,
          int fa,
          byte faNo,
		  bool mono,
		  bool auto,
		  SaveData save,
          bool cancel)
          : base(s, m, "　" + q1, "　" + q2, "　" + q3, false, save)
        {
            this.cancel = cancel;
            this.faseseet = fa;
            this.faseNo = faNo;
            this.printfase = pfase;
            this.manyQuestion = 3;
            this.fastprint = fast;
            this.cursolposi[0] = new Vector2(40f, 108f);
            this.cursolposi[1] = new Vector2(40f, 124f);
            this.cursolposi[2] = new Vector2(40f, 140f);
			if (auto)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.auto;
			}
			else if (mono)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.mono;
			}
		}

        public Question(
          IAudioEngine s,
          EventManager m,
          string q1,
          string q2,
          string q3,
          bool fast,
          bool pfase,
          FaceId face,
          SaveData save,
          bool cancel)
          : this(s, m, q1, q2, q3, fast, pfase, face.Sheet, face.Index, face.Mono, face.Auto, save, cancel)
        {
        }

        public Question(
          IAudioEngine s,
          EventManager m,
          string text1,
          string q1,
          string q2,
          string q3,
          string q4,
          bool fast,
          bool pfase,
          int fa,
          byte faNo,
		  bool mono,
		  bool auto,
		  SaveData save,
          bool cancel)
          : base(s, m, text1, "　" + q1 + "　　" + q2, "　" + q3 + "　　" + q4, false, save)
        {
            this.cancel = cancel;
            this.faseseet = fa;
            this.faseNo = faNo;
            this.printfase = pfase;
            this.manyQuestion = 4;
            this.fastprint = fast;
            Graphics graphics = Graphics.FromImage(new Bitmap(100, 100));

            this.cursolposi[0] = new Vector2(40f, 124f);
            var option1Size = ShanghaiEXE.measurer.MeasureMiniText("　" + q1 + "　");
            var option3Size = ShanghaiEXE.measurer.MeasureMiniText("　" + q3 + "　");

            this.cursolposi[1] = new Vector2(40 + option1Size.Width, 124);
            this.cursolposi[2] = new Vector2(40, 140f);
            this.cursolposi[3] = new Vector2(40 + option3Size.Width, 140f);
			if (auto)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.auto;
			}
			else if (mono)
			{
				this.FacePattern = CommandMessage.FACEPATTERN.mono;
			}
		}

        public Question(
          IAudioEngine s,
          EventManager m,
          string text1,
          string q1,
          string q2,
          string q3,
          string q4,
          bool fast,
          bool pfase,
          FaceId face,
          SaveData save,
          bool cancel)
          : this(s, m, text1, q1, q2, q3, q4, fast, pfase, face.Sheet, face.Index, face.Mono, face.Auto, save, cancel)
        {
        }

        private new void Init()
        {
            this.nowscene = Question.SCENE.printing;
            this.FacePattern = CommandMessage.FACEPATTERN.neutral;
            this.endprint = 0;
            this.printfonts = 0;
            this.arrowprint = false;
            this.faseflame = 0;
            this.frame = 0;
            this.manyopen = 0;
            this.fasewait = 0;
            this.wait = 0;
            this.cursor = 0;
            this.cansel = false;
            this.shortmassage = new string[3] { "", "", "" };
        }

        public override void Update()
		{
			if (this.printfase)
                this.FaseAnimation();
            string[][] strArray = new string[3][]
            {
                this.ToDecomposition(this.massage[0]),
                this.ToDecomposition(this.massage[1]),
                this.ToDecomposition(this.massage[2])
            };
            if (Input.IsPress(Button._B) || Input.IsPress(Button._A) || this.fastprint)
            {
                this.endprint = strArray.Length - 1;
                this.printfonts = strArray[strArray.Length - 1].Length + 1;
                this.shortmassage = this.massage;
            }
            switch (this.nowscene)
            {
                case Question.SCENE.printing:
                    ++this.printfonts;
                    if (this.printfonts > strArray[this.endprint].Length)
                    {
                        this.printfonts = 0;
                        ++this.endprint;
                        if (this.endprint < this.massage.Length)
                            break;
                        this.arrowprint = true;
                        this.nowscene = Question.SCENE.pushA;
                        break;
                    }
                    // ISSUE: explicit reference operation
                    this.shortmassage[this.endprint] += strArray[this.endprint][this.printfonts - 1];
                    this.sound.PlaySE(SoundEffect.message);
                    if (strArray[this.endprint][this.printfonts - 1] == "・")
                    {
                        this.wait = 30;
                        this.longwaiting = true;
                    }
                    else if (strArray[this.endprint][this.printfonts - 1] == "、")
                    {
                        this.wait = 15;
                        this.longwaiting = true;
                    }
                    else if (strArray[this.endprint][this.printfonts - 1] == "#")
                    {
                        string s = "";
                        for (int index = 2; index < 100 && (this.printfonts + index < strArray[this.endprint].Length && !(strArray[this.endprint][this.printfonts + index] == "#")); ++index)
                            s += strArray[this.endprint][this.printfonts + index];
                        string str = strArray[this.endprint][this.printfonts];
                        if (str == "s")
                        {
                            try
                            {
                                this.sound.PlaySE((SoundEffect)Enum.Parse(typeof(SoundEffect), s));
                            }
                            catch { }
                        }
                        else if (str == "w")
                        {
                            try
                            {
                                this.wait = int.Parse(s);
                                this.longwaiting = true;
                            }
                            catch { }
                        }
                        else if (str == "b")
                        {
                            try
                            {
                                this.canskip = bool.Parse(s);
                            }
                            catch { }
                        }
                        else if (str == "e")
                        {
                            this.Init();
                            this.EndCommand();
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
                        this.wait = 0;
                        this.longwaiting = false;
                    }
                    this.nowscene = Question.SCENE.wait;
                    break;
                case Question.SCENE.wait:
                    --this.wait;
                    if (this.wait > 0)
                        break;
                    this.wait = 0;
                    this.nowscene = Question.SCENE.printing;
                    break;
                case Question.SCENE.pushA:
                    this.FlameControl(4);
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
                    this.Control();
                    break;
            }
        }

        public void MoveCursol(int c)
        {
            this.cursor = c;
        }

        private void Control()
        {
            switch (this.manyQuestion)
            {
                case 2:
                    if (!this.height)
                    {
                        if (Input.IsPress(Button.Left))
                        {
                            this.sound.PlaySE(SoundEffect.movecursol);
                            --this.cursor;
                            if (this.cursor < 0)
                                this.cursor = this.manyQuestion - 1;
                        }
                        if (Input.IsPress(Button.Right))
                        {
                            this.sound.PlaySE(SoundEffect.movecursol);
                            ++this.cursor;
                            if (this.cursor > this.manyQuestion - 1)
                                this.cursor = 0;
                            break;
                        }
                        break;
                    }
                    if (Input.IsPress(Button.Up))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        --this.cursor;
                        if (this.cursor < 0)
                            this.cursor = this.manyQuestion - 1;
                    }
                    if (Input.IsPress(Button.Down))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        ++this.cursor;
                        if (this.cursor > this.manyQuestion - 1)
                            this.cursor = 0;
                    }
                    break;
                case 3:
                    if (Input.IsPress(Button.Up))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        --this.cursor;
                        if (this.cursor < 0)
                            this.cursor = this.manyQuestion - 1;
                    }
                    if (Input.IsPress(Button.Down))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        ++this.cursor;
                        if (this.cursor > this.manyQuestion - 1)
                            this.cursor = 0;
                        break;
                    }
                    break;
                case 4:
                    if (Input.IsPress(Button.Left))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        --this.cursor;
                        if (this.cursor < 0)
                            this.cursor = this.manyQuestion - 1;
                    }
                    if (Input.IsPress(Button.Right))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        ++this.cursor;
                        if (this.cursor > this.manyQuestion - 1)
                            this.cursor = 0;
                    }
                    if (Input.IsPress(Button.Up))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        if (this.cursor >= 2)
                            this.cursor -= 2;
                    }
                    if (Input.IsPress(Button.Down))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        if (this.cursor < 2)
                            this.cursor += 2;
                        break;
                    }
                    break;
            }
            if (Input.IsPress(Button._A) || this.cansel)
            {
                if (!this.cansel)
                    this.sound.PlaySE(SoundEffect.decide);
                this.savedata.selectQuestion = this.cursor;
                this.Init();
                this.EndCommand();
            }
            if (!Input.IsPress(Button._B) || !this.cancel)
                return;
            this.sound.PlaySE(SoundEffect.cancel);
            this.cursor = this.manyQuestion != 2 ? this.manyQuestion : this.manyQuestion - 1;
            this.savedata.selectQuestion = this.cursor;
            this.cansel = true;
            this.endok = false;
            this.Init();
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(0.0f, 104f);
            this._rect = new Rectangle(0, 0, 240, 56);
            dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
            for (int index = 0; index < this.massage.Length; ++index)
            {
                this._position = new Vector2(48f, 108 + 16 * index);
                if (this.manyQuestion != 4 || index == 0)
                    dg.DrawText(this.shortmassage[index], this._position, this.savedata);
                else
                    dg.DrawMiniText(this.shortmassage[index], this._position, Color.FromArgb(byte.MaxValue, 64, 56, 56));
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
				dg.DrawImage(dg, "Face" + faseseet, this._rect, true, this._position, Color.White);
            }
            if (!this.arrowprint)
                return;
            this._position = this.cursolposi[this.cursor];
            this._rect = new Rectangle(240 + this.frame % 3 * 16, 48, 16, 16);
            dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
        }

        private enum SCENE
        {
            printing,
            wait,
            pushA,
        }
    }
}
