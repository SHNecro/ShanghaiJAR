using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap.Character;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NSEvent
{
    public class QuestSelect : EventBase
    {
        private readonly List<string> questlist = new List<string>();
        private readonly List<int> questlistNumber = new List<int>();
        private readonly int[][] questFlug = new int[17][]
        {
            new int[1]{ -1 },
            new int[1]{ -1 },
            new int[1]{ -1 },
            new int[1]{ 156 },
            new int[1]{ 156 },
            new int[1]{ 156 },
            new int[1]{ 156 },
            new int[1]{ 225 },
            new int[1]{ 225 },
            new int[1]{ 225 },
            new int[1]{ 225 },
            new int[1]{ 528 },
            new int[1]{ 528 },
            new int[1]{ 528 },
            new int[1]{ 528 },
            new int[1]{ 528 },
            new int[1]{ 609 }
        };
        private readonly string[] questName = new string[]
        {
            ShanghaiEXE.Translate("Request.DataDeliveryName"),
            ShanghaiEXE.Translate("Request.CreepyNaviName"),
            ShanghaiEXE.Translate("Request.IceCrystalsName"),
            ShanghaiEXE.Translate("Request.SparringPartnerName"),
            ShanghaiEXE.Translate("Request.NeedBugFragsName"),
            ShanghaiEXE.Translate("Request.Stalker!Help!Name"),
            ShanghaiEXE.Translate("Request.PleaseHelp!Name"),
            ShanghaiEXE.Translate("Request.PunishAdminName"),
            ShanghaiEXE.Translate("Request.CheapChips!Name"),
            ShanghaiEXE.Translate("Request.MotionActing!Name"),
            ShanghaiEXE.Translate("Request.UrgentGift!Name"),
            ShanghaiEXE.Translate("Request.BirthdayGiftName"),
            ShanghaiEXE.Translate("Request.RehabName"),
            ShanghaiEXE.Translate("Request.BecomingANaviName"),
            ShanghaiEXE.Translate("Request.SisterBouquetName"),
            ShanghaiEXE.Translate("Request.TalkPhilosophyName"),
            ShanghaiEXE.Translate("Request.ToBePresidentName"),
        };
        private readonly int[] questHint = new int[17]
        {
            1,
            3,
            6,
            9,
            10,
            11,
            12,
            15,
            18,
            19,
            24,
            25,
            27,
            28,
            33,
            39,
            40
        };
        private readonly int[] endFlag = new int[17]
        {
            81,
            84,
            87,
            89,
            91,
            92,
            95,
            383,
            384,
            385,
            390,
            395,
            396,
            710,
            716,
            717,
            746
        };
        private QuestSelect.SCENE nowscene;
        private readonly EventManager eventmanager;
        private byte alpha;
        protected const string texture = "menuwindows";
        private bool shopmode;
        private int cursol;
        private int cursolanime;
        private readonly int overTop;
        private int top;
        private int waittime;
        private const byte waitlong = 10;
        private const byte waitshort = 4;
        private readonly InfoMessage info;

        private int Select
        {
            get
            {
                return this.cursol + this.top;
            }
        }

        public QuestSelect(IAudioEngine s, EventManager m, Player player, SaveData save)
          : base(s, m, save)
        {
            this.info = player.info;
            this.eventmanager = new EventManager(this.sound);
            this.NoTimeNext = false;
            for (int index1 = 0; index1 < this.questFlug.Length; ++index1)
            {
                bool flag = true;
                for (int index2 = 0; index2 < this.questFlug[index1].Length; ++index2)
                {
                    if (this.questFlug[index1][index2] >= 0 && !this.savedata.FlagList[this.questFlug[index1][index2]])
                        flag = false;
                }
                if (flag)
                {
                    this.questlist.Insert(0, this.questName[index1]);
                    this.questlistNumber.Insert(0, index1);
                }
            }
            this.overTop = this.questlistNumber.Count - 8;
            if (this.overTop < 0)
                this.overTop = 0;
            for (int index = 0; index < this.endFlag.Length; ++index)
            {
                if (this.savedata.FlagList[this.endFlag[index]])
                {
                    this.savedata.questEnd[index] = true;
                    if (this.savedata.ValList[11] == index)
                        this.savedata.ValList[11] = -1;
                }
            }
        }

        public override void Update()
        {
            switch (this.nowscene)
            {
                case QuestSelect.SCENE.fadein:
                    if (!this.shopmode)
                    {
                        this.alpha += 51;
                        if (this.alpha >= byte.MaxValue)
                        {
                            this.alpha = byte.MaxValue;
                            this.shopmode = true;
                            break;
                        }
                        break;
                    }
                    this.alpha -= 51;
                    if (this.alpha <= 0)
                    {
                        this.savedata.selectQuestion = 1;
                        this.nowscene = QuestSelect.SCENE.select;
                    }
                    break;
                case QuestSelect.SCENE.select:
                    if (this.eventmanager.playevent)
                    {
                        this.eventmanager.UpDate();
                        break;
                    }
                    if (this.savedata.selectQuestion == 0)
                    {
                        this.savedata.ValList[11] = this.questlistNumber[this.Select];
                        this.savedata.ValList[4] = this.questHint[this.questlistNumber[this.Select]];
                        this.savedata.selectQuestion = 1;
                    }
                    else
                        this.Control();
                    break;
                case QuestSelect.SCENE.fadeout:
                    if (this.shopmode)
                    {
                        this.alpha += 51;
                        if (this.alpha >= byte.MaxValue)
                        {
                            this.alpha = byte.MaxValue;
                            this.shopmode = false;
                            break;
                        }
                        break;
                    }
                    this.alpha -= 51;
                    if (this.alpha <= 0)
                    {
                        this.nowscene = QuestSelect.SCENE.fadein;
                        this.cursol = 0;
                        this.top = 0;
                        this.EndCommand();
                    }
                    break;
            }
            this.FlameControl(10);
            if (!this.moveflame)
                return;
            ++this.cursolanime;
            if (this.cursolanime >= 3)
                this.cursolanime = 0;
        }

        private void Control()
        {
            if (Input.IsPress(Button._A))
                this.MessageMake();
            else if (Input.IsPress(Button._B))
            {
                this.sound.PlaySE(SoundEffect.cancel);
                this.nowscene = QuestSelect.SCENE.fadeout;
            }
            else if (this.waittime <= 0)
            {
                if (Input.IsPush(Button.Up))
                {
                    if (this.Select <= 0)
                        return;
                    if (this.cursol > 0)
                        --this.cursol;
                    else
                        --this.top;
                    this.sound.PlaySE(SoundEffect.movecursol);
                    this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                }
                else
                {
                    if (!Input.IsPush(Button.Down) || this.Select >= this.questlist.Count - 1)
                        return;
                    if (this.cursol < 7)
                        ++this.cursol;
                    else
                        ++this.top;
                    this.sound.PlaySE(SoundEffect.movecursol);
                    this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                }
            }
            else
                --this.waittime;
        }

        private void MessageMake()
        {
            this.savedata.selectQuestion = 1;
            bool flag = this.savedata.questEnd[this.questlistNumber[this.Select]];
            int index = this.questlistNumber[this.Select];
            this.eventmanager.events.Clear();
            this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
            if (this.savedata.questEnd[index])
            {
                var dialogue = ShanghaiEXE.Translate("Request.RequestComplete");
                this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face.Mono, dialogue.Face, dialogue.Face.Mono, this.savedata));
            }
            this.eventmanager.EventPass(this.info.GetMessage(flag ? Data.MessageType.RequestBoardComplete : Data.MessageType.RequestBoard, index));
            if (!this.savedata.questEnd[index])
            {
                if (this.savedata.ValList[11] < 0)
                {
                    var question = ShanghaiEXE.Translate("Request.AcceptRequestQuestion");
                    var options = ShanghaiEXE.Translate("Request.AcceptRequestOptions");
                    this.eventmanager.AddEvent(new Question(this.sound, this.eventmanager, question[0], options[0], options[1], false, true, false, question.Face, this.savedata, true));
                }
                else if (index == this.savedata.ValList[11])
                {
                    var dialogue = ShanghaiEXE.Translate("Request.RequestInProgressDialogue1");
                    this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
                }
                else
                {
                    var dialogue = ShanghaiEXE.Translate("Request.RequestAlreadyTakenDialogue1");
                    this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
                    dialogue = ShanghaiEXE.Translate("Request.RequestAlreadyTakenDialogue2");
                    this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
                }
            }
            this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
        }

        public override void Render(IRenderer dg)
        {
            if (this.shopmode)
            {
                this._position = new Vector2(0.0f, 0.0f);
                this._rect = new Rectangle(0, 784, 240, 160);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                for (int index1 = 0; index1 < Math.Min(8, this.questlist.Count); ++index1)
                {
                    int index2 = this.questlistNumber[this.top + index1];
                    if (this.savedata.ValList[11] != index2)
                    {
                        var checkmarkSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("RequestBoard.CompleteMark");
                        if (this.savedata.questEnd[index2])
                        {
                            this._rect = checkmarkSprite.Item2;
                        }
                        else
                            this._rect = new Rectangle(528 + (this.cursolanime % 2 == 0 ? 0 : 16), 600, 16, 16);
                        this._position = new Vector2(24f, 16 + 16 * index1);
                        dg.DrawImage(dg, checkmarkSprite.Item1, this._rect, true, this._position, Color.White);
                    }
                    this._position = new Vector2(48f, 17 + 16 * index1);
                    dg.DrawMiniText(this.questlist[this.top + index1], this._position, Color.FromArgb(32, 32, 32));
                }
                int num1 = 0;
                foreach (int index in this.questlistNumber)
                {
                    if (this.savedata.ValList[11] != index && !this.savedata.questEnd[index])
                        ++num1;
                }
                string txt = num1.ToString() + "/" + this.questlist.Count.ToString();
                this.TextRender(dg, txt, false, new Vector2(176f, 0.0f), false);
                this._rect = new Rectangle(528 + (this.cursolanime % 2 == 0 ? 0 : 16), 600, 16, 16);
                this._position = new Vector2(160f, 0.0f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                this._rect = new Rectangle(240 + 16 * this.cursolanime, 48, 16, 16);
                this._position = new Vector2(4f, 16 + this.cursol * 16);
                dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
                float num2 = this.overTop != 0 && this.top != 0 ? 128f / overTop * top : 0.0f;
                this._rect = new Rectangle(176, 168, 8, 8);
                this._position = new Vector2(232f, 16f + num2);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            if (this.eventmanager.playevent)
                this.eventmanager.Render(dg);
            if (this.alpha <= 0)
                return;
            Color color = Color.FromArgb(alpha, Color.Black);
            Rectangle _rect = new Rectangle(0, 0, 240, 160);
            Vector2 _point = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", _rect, true, _point, color);
        }

        private enum SCENE
        {
            fadein,
            select,
            fadeout,
        }
    }
}
