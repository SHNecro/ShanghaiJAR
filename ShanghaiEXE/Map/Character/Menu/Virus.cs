using Common;
using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEvent;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSMap.Character.Menu
{
    internal class Virus : MenuBase
    {
        private readonly int[] pageX = new int[2];
        private readonly int[] stylenum = new int[4];
        private readonly int flash = 0;
        private int cursol;
        private Virus.SCENE nowscene;
        private readonly EventManager eventmanager;
        private int cursolOld;
        private const int stopLeft = 112;
        private const int stopRight = 240;
        private const int move1F = 8;
        private int page;
        private readonly bool moving;
        private bool selectstyle;
        private bool bootAddon;
        private bool boot;
        private readonly AddOnManager manager;

        private int ManyPage
        {
            get
            {
                return this.savedata.haveCaptureBomb;
            }
        }

        private bool RightOK
        {
            get
            {
                return this.page < this.savedata.haveCaptureBomb - 1;
            }
        }

        private bool LeftOK
        {
            get
            {
                return this.page > 0;
            }
        }

        public Virus(IAudioEngine s, Player p, TopMenu t, SaveData save)
          : base(s, p, t, save)
        {
            this.eventmanager = new EventManager(this.sound);
            this.Alpha = byte.MaxValue;
            this.pageX[0] = 112;
            this.pageX[1] = 240;
        }

        public override void UpDate()
        {
            if (this.bootAddon)
            {
                this.manager.UpDate();
            }
            else
            {
                switch (this.nowscene)
                {
                    case Virus.SCENE.fadein:
                        if (this.Alpha > 0)
                        {
                            this.Alpha -= 51;
                            break;
                        }
                        this.nowscene = Virus.SCENE.select;
                        break;
                    case Virus.SCENE.select:
                        if (this.selectstyle && !this.eventmanager.playevent)
                        {
                            if (this.cursol == 0)
                            {
                                this.cursolOld = this.savedata.selectQuestion;
                                if (this.savedata.havePeace[this.savedata.selectQuestion] > 0)
                                {
                                    switch (this.savedata.selectQuestion)
                                    {
                                        case 0:
                                            if (this.savedata.HaveVirus[this.page].eatBug < 40)
                                            {
                                                ++this.savedata.HaveVirus[this.page].eatBug;
                                                if (this.savedata.HaveVirus[this.page].eatBug % 20 == 0)
                                                {
                                                    this.sound.PlaySE(SoundEffect.bright);
                                                    this.savedata.HaveVirus[this.page].ReturnVirus(this.savedata.HaveVirus[this.page].type);
                                                }
                                                --this.savedata.havePeace[this.savedata.selectQuestion];
                                                this.sound.PlaySE(SoundEffect.repair);
                                                break;
                                            }
                                            this.eventmanager.events.Clear();
                                            var dialogue = ShanghaiEXE.Translate("Virus.FeedFailedDialogue1");
                                            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                                            break;
                                        case 1:
                                            ++this.savedata.HaveVirus[this.page].eatFreeze;
                                            --this.savedata.havePeace[this.savedata.selectQuestion];
                                            this.sound.PlaySE(SoundEffect.repair);
                                            break;
                                        case 2:
                                            ++this.savedata.HaveVirus[this.page].eatError;
                                            --this.savedata.havePeace[this.savedata.selectQuestion];
                                            this.sound.PlaySE(SoundEffect.repair);
                                            break;
                                    }
                                }
                                else
                                {
                                    this.sound.PlaySE(SoundEffect.error);
                                    this.eventmanager.events.Clear();
                                    var dialogue = ShanghaiEXE.Translate("Virus.FeedFailedNoItemsDialogue1");
                                    this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                                }
                                this.selectstyle = false;
                            }
                            else
                            {
                                if (this.savedata.selectQuestion == 0)
                                {
                                    this.sound.PlaySE(SoundEffect.warp);
                                    this.eventmanager.events.Clear();
                                    var dialogue = ShanghaiEXE.Translate("Virus.DismissDialogue1");
                                    this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                                    this.savedata.stockVirus.Add(this.savedata.HaveVirus[this.page]);
                                    this.savedata.HaveVirus[this.page] = null;
                                }
                                this.selectstyle = false;
                            }
                        }
                        this.Control();
                        if (this.eventmanager.playevent)
                            this.eventmanager.UpDate();
                        this.FlamePlus();
                        break;
                    case Virus.SCENE.fadeout:
                        if (this.Alpha < byte.MaxValue)
                        {
                            this.Alpha += 51;
                            break;
                        }
                        if (!this.boot)
                        {
                            this.player.parent.main.FolderReset();
                            this.topmenu.Return();
                        }
                        else
                            this.bootAddon = true;
                        break;
                }
            }
        }

        public void Control()
        {
            if (!this.eventmanager.playevent && this.savedata.HaveVirus[this.page] != null)
            {
                if (Input.IsPress(Button._A))
                {
                    var question = new Dialogue();
                    var options = new Dialogue();
                    switch (this.cursol)
                    {
                        case 0:
                            if (this.savedata.HaveVirus[this.page].eatSum < 50)
                            {
                                this.sound.PlaySE(SoundEffect.decide);
                                this.selectstyle = true;
                                question = ShanghaiEXE.Translate("Virus.FeedQuestion");
                                options = ShanghaiEXE.Translate("Virus.FeedOptionsFormat").Format(this.savedata.havePeace[0], this.savedata.havePeace[1], this.savedata.havePeace[2]);
                                Question question1 = new Question(this.sound, this.eventmanager, options[0], options[1], options[2], true, true, question.Face, this.savedata);
                                question1.MoveCursol(this.cursolOld);
                                this.eventmanager.events.Clear();
                                this.eventmanager.AddEvent(question1);
                                break;
                            }
                            this.sound.PlaySE(SoundEffect.error);
                            break;
                        case 1:
                            this.sound.PlaySE(SoundEffect.decide);
                            this.selectstyle = true;
                            question = ShanghaiEXE.Translate("Virus.DismissQuestion");
                            options = ShanghaiEXE.Translate("Virus.DismissOptions");
                            Question question2 = new Question(this.sound, this.eventmanager, question[0], options[0], options[1], true, true, true, question.Face, this.savedata, true);
                            question2.MoveCursol(this.cursolOld);
                            this.eventmanager.events.Clear();
                            this.eventmanager.AddEvent(question2);
                            break;
                    }
                }
                if (Input.IsPress(Button.Up))
                {
                    this.sound.PlaySE(SoundEffect.movecursol);
                    --this.cursol;
                    if (this.cursol < 0)
                        this.cursol = 1;
                }
                if (Input.IsPress(Button.Down))
                {
                    this.sound.PlaySE(SoundEffect.movecursol);
                    ++this.cursol;
                    if (this.cursol > 1)
                        this.cursol = 0;
                }
            }
            if (Input.IsPress(Button._B))
            {
                this.sound.PlaySE(SoundEffect.cancel);
                if (this.selectstyle)
                {
                    this.selectstyle = false;
                    this.eventmanager.ClearEvent();
                }
                else if (!this.eventmanager.playevent)
                {
                    this.boot = false;
                    this.nowscene = Virus.SCENE.fadeout;
                }
            }
            if (!this.eventmanager.playevent)
            {
                if (Input.IsPress(Button._R) && this.RightOK)
                {
                    this.sound.PlaySE(SoundEffect.movecursol);
                    ++this.page;
                }
                if (!Input.IsPress(Button._L) || !this.LeftOK)
                    return;
                this.sound.PlaySE(SoundEffect.movecursol);
                --this.page;
            }
            else if (this.waittime > 0)
                --this.waittime;
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(240, 624, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            this._rect = new Rectangle(480, 584, 64, 16);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            this._rect = new Rectangle(568, 128, 112, 88);
            this._position = new Vector2(120f, 16f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            this._position = new Vector2(5f, 108f);
            this._rect = new Rectangle(0, 0, 40, 48);
            dg.DrawImage(dg, "Face1", this._rect, true, this._position, Color.White);
            if (this.eventmanager.playevent)
            {
                this.eventmanager.Render(dg);
            }
            else
            {
                this._position = new Vector2(48f, 108f);
                if (this.savedata.HaveVirus[this.page] == null)
                {
                    var dialogue = ShanghaiEXE.Translate("Virus.IntroductionNoVirusDialogue1");
                    dg.DrawText(dialogue[0], this._position);
                    this._position = new Vector2(48f, 124f);
                    dg.DrawText(dialogue[1], this._position);
                }
                else
                {
                    var dialogue = ShanghaiEXE.Translate("Virus.Introduction");
                    dg.DrawText(dialogue, this._position);
                    this._position = new Vector2(48f, 124f);
                    var options = ShanghaiEXE.Translate("Virus.OptionsFormat").Format(50 - this.savedata.HaveVirus[this.page].eatSum);
                    dg.DrawText(options[0], this._position);
                    this._position = new Vector2(48f, 140f);
                    dg.DrawText(options[1], this._position);
                    this._position = new Vector2(40f, 124 + 16 * this.cursol);
                    this._rect = new Rectangle(240 + this.frame / 4 % 3 * 16, 48, 16, 16);
                    dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
                }
            }
            if (this.savedata.HaveVirus[this.page] != null)
            {
                this._position = new Vector2(64f, 60f);
                this.savedata.HaveVirus[this.page].Render(dg, this._position, true);
                string txt1 = (this.savedata.HaveVirus[this.page].Name + "　" + ((ChipFolder.CODE)this.savedata.HaveVirus[this.page].code).ToString()).Replace('V', 'Ｖ');
                this._position = new Vector2(128f, 20f);
                this.TextRender(dg, txt1, false, this._position, false);
                this._position = new Vector2(40f, 80f);
                this.TextRender(dg, txt1, false, this._position, false);
                this._rect = new Rectangle(232, 0, 24, 16);
                this._position = new Vector2(136f, 44f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                int num = 100 + this.savedata.HaveVirus[this.page].eatBug * 5;
                string txt2 = (num / 100).ToString() + "." + (num % 100 < 10 ? "0" : "") + (num % 100).ToString();
                this._position = new Vector2(168f, 44f);
                this.TextRender(dg, txt2, false, this._position, true, Color.Yellow);
                this._position = new Vector2(136f, 60f);
                this.TextRender(dg, "HP :", false, this._position, true);
                this._position = new Vector2(192f, 60f);
                string txt3 = this.savedata.HaveVirus[this.page].HP.ToString();
                this.TextRender(dg, txt3, true, this._position, true, Color.Yellow);
                this._position = new Vector2(136f, 76f);
                this.TextRender(dg, ShanghaiEXE.Translate("Virus.Power"), false, this._position, true);
                this._position = new Vector2(192f, 76f);
                string txt4 = this.savedata.HaveVirus[this.page].Power.ToString();
                this.TextRender(dg, txt4, true, this._position, true, Color.Yellow);
                this._rect = new Rectangle(320, 0, 16, 48);
                this._position = new Vector2(208f, 44f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            else
            {
                this._position = new Vector2(44f, 52f);
                string txt = "EMPTY";
                this.TextRender(dg, txt, false, this._position, true);
            }
            if (this.LeftOK)
            {
                this._rect = new Rectangle(304, 80 + (Input.IsPush(Button._L) ? 16 : 0), 16, 16);
                this._position = new Vector2(8f, 16f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            if (this.RightOK)
            {
                this._rect = new Rectangle(320, 80 + (Input.IsPush(Button._R) ? 16 : 0), 16, 16);
                this._position = new Vector2(104f, 16f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            if (this.nowscene != Virus.SCENE.fadein && this.nowscene != Virus.SCENE.fadeout)
                return;
            Color color = Color.FromArgb(this.Alpha, 0, 0, 0);
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color);
        }

        private enum SCENE
        {
            fadein,
            select,
            fadeout,
        }
    }
}
