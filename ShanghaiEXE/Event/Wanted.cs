using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSGame;
using NSMap.Character;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NSEvent
{
    public class Wanted : EventBase
    {
        private readonly string[] encountArea = new string[41]
        {
            ShanghaiEXE.Translate("Bounty.Area1"),
            ShanghaiEXE.Translate("Bounty.Area2"),
            ShanghaiEXE.Translate("Bounty.Area3"),
            ShanghaiEXE.Translate("Bounty.Area4"),
            ShanghaiEXE.Translate("Bounty.Area5"),
            ShanghaiEXE.Translate("Bounty.Area6"),
            ShanghaiEXE.Translate("Bounty.Area7"),
            ShanghaiEXE.Translate("Bounty.Area8"),
            ShanghaiEXE.Translate("Bounty.Area9"),
            ShanghaiEXE.Translate("Bounty.Area10"),
            ShanghaiEXE.Translate("Bounty.Area11"),
            ShanghaiEXE.Translate("Bounty.Area12"),
            ShanghaiEXE.Translate("Bounty.Area13"),
            ShanghaiEXE.Translate("Bounty.Area14"),
            ShanghaiEXE.Translate("Bounty.Area15"),
            ShanghaiEXE.Translate("Bounty.Area16"),
            ShanghaiEXE.Translate("Bounty.Area17"),
            ShanghaiEXE.Translate("Bounty.Area18"),
            ShanghaiEXE.Translate("Bounty.Area19"),
            ShanghaiEXE.Translate("Bounty.Area20"),
            ShanghaiEXE.Translate("Bounty.Area21"),
            ShanghaiEXE.Translate("Bounty.Area22"),
            ShanghaiEXE.Translate("Bounty.Area23"),
            ShanghaiEXE.Translate("Bounty.Area24"),
            ShanghaiEXE.Translate("Bounty.Area25"),
            ShanghaiEXE.Translate("Bounty.Area26"),
            ShanghaiEXE.Translate("Bounty.Area27"),
            ShanghaiEXE.Translate("Bounty.Area28"),
            ShanghaiEXE.Translate("Bounty.Area29"),
            ShanghaiEXE.Translate("Bounty.Area30"),
            ShanghaiEXE.Translate("Bounty.Area31"),
            ShanghaiEXE.Translate("Bounty.Area32"),
            ShanghaiEXE.Translate("Bounty.Area33"),
            ShanghaiEXE.Translate("Bounty.Area34"),
            ShanghaiEXE.Translate("Bounty.Area35"),
            ShanghaiEXE.Translate("Bounty.Area36"),
            ShanghaiEXE.Translate("Bounty.Area37"),
            ShanghaiEXE.Translate("Bounty.Area38"),
            ShanghaiEXE.Translate("Bounty.Area39"),
            ShanghaiEXE.Translate("Bounty.Area40"),
            ShanghaiEXE.Translate("Bounty.Area41"),
        };
        private readonly List<int> list = new List<int>();
        private Wanted.SCENE nowscene;
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

        // Virus #, enable flag, on-task flag, reward
        public static int[,] WantedList { get; } = new int[41, 4]
        {
            {6, -1, 100, 10000},
            {1, -1, 101, 12000},
            {11, -1, 102, 15000},
            {17, -1, 113, 18000},
            {14, -1, 114, 20000},
            {24, 156, 103, 21000},
            {7, 156, 104, 23000},
            {36, 156, 105, 25000},
            {27, 156, 106, 27000},
            {18, 156, 115, 29000},
            {21, 156, 107, 35000},
            {8, 225, 115, 36000},
            {39, 225, 108, 38000},
            {20, 225, 109, 40000},
            {33, 225, 110, 42000},
            {41, 225, 116, 42000},
            {19, 225, 111, 47000},
            {37, 225, 112, 50000},
            {29, 283, 116, 51000},
            {3, 283, 117, 53000},
            {13, 283, 118, 56000},
            {2, 283, 119, 58000},
            {10, 283, 661, 60000},
            {32, 283, 662, 65000},
            {25, 452, 663, 67000},
            {34, 452, 664, 69000},
            {38, 452, 665, 72000},
            {40, 452, 666, 74000},
            {26, 452, 667, 77000},
            {35, 452, 668, 80000},
            {5, 527, 669, 82000},
            {30, 527, 670, 85000},
            {22, 527, 671, 89000},
            {15, 527, 672, 92000},
            {16, 527, 673, 100000},
            {31, 610, 674, 120000},
            {23, 744, 675, 140000},
            {28, 610, 676, 200000},
            {9, 788, 677, 100000},
            {12, 788, 678, 150000},
            {4, 788, 679, 190000}
        };

        private int Select
        {
            get
            {
                return this.cursol + this.top;
            }
        }

        public Wanted(IAudioEngine s, EventManager m, Player player, SaveData save)
          : base(s, m, save)
        {
            this.info = player.info;
            this.eventmanager = new EventManager(this.sound);
            this.NoTimeNext = false;
            for (int index = 0; index < Wanted.WantedList.GetLength(0); ++index)
            {
                bool flag = true;
                if (Wanted.WantedList[index, 1] >= 0 && !this.savedata.FlagList[Wanted.WantedList[index, 1]])
                    flag = false;
                if (flag)
                    this.list.Add(index);
            }
            this.overTop = this.list.Count - 8;
            if (this.overTop >= 0)
                return;
            this.overTop = 0;
        }

        public override void Update()
        {
            switch (this.nowscene)
            {
                case Wanted.SCENE.fadein:
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
                        this.nowscene = Wanted.SCENE.select;
                        bool flag = false;
                        EventManager eventManager = new EventManager(this.sound);
                        eventManager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
                        for (int index1 = 0; index1 < this.savedata.virusSPbustedFlug.Length; ++index1)
                        {
                            if (this.savedata.virusSPbustedFlug[index1])
                            {
                                this.savedata.ValList[12] = -1;
                                this.savedata.virusSPbustedFlug[index1] = false;
                                int money = 0;
                                EnemyBase enemyBase = EnemyBase.EnemyMake(0, null, false);
                                for (int index2 = 0; index2 < Wanted.WantedList.GetLength(0); ++index2)
                                {
                                    if (Wanted.WantedList[index2, 0] == index1)
                                    {
                                        enemyBase = EnemyBase.EnemyMake(Wanted.WantedList[index2, 0], null, false);
                                        money = Wanted.WantedList[index2, 3];
                                        break;
                                    }
                                }
                                flag = true;
                                var dialogue = ShanghaiEXE.Translate("Bounty.ClaimDialogue1Format").Format(enemyBase.Name);
                                eventManager.AddEvent(new CommandMessage(this.sound, eventManager, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
                                eventManager.AddEvent(new SEmon(this.sound, eventManager, "get", 0, this.savedata));
                                dialogue = ShanghaiEXE.Translate("Bounty.ClaimDialogue2Format").Format(money);
                                eventManager.AddEvent(new CommandMessage(this.sound, eventManager, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
                                eventManager.AddEvent(new MoneyPlus(this.sound, eventManager, money, true, this.savedata));
                                dialogue = ShanghaiEXE.Translate("Bounty.ClaimDialogue3");
                                eventManager.AddEvent(new CommandMessage(this.sound, eventManager, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
                            }
                        }
                        eventManager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
                        if (flag)
                        {
                            this.eventmanager.EventClone(eventManager);
                            this.eventmanager.playevent = true;
                        }
                    }
                    break;
                case Wanted.SCENE.select:
                    if (this.eventmanager.playevent)
                    {
                        this.eventmanager.UpDate();
                        break;
                    }
                    if (this.savedata.selectQuestion == 0)
                    {
                        if (this.savedata.ValList[12] >= 0)
                            this.savedata.FlagList[Wanted.WantedList[this.savedata.ValList[12], 2]] = false;
                        this.savedata.ValList[12] = this.list[this.Select];
                        this.savedata.FlagList[Wanted.WantedList[this.list[this.Select], 2]] = true;
                        this.savedata.selectQuestion = 1;
                    }
                    else
                        this.Control();
                    break;
                case Wanted.SCENE.fadeout:
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
                        this.nowscene = Wanted.SCENE.fadein;
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
                this.nowscene = Wanted.SCENE.fadeout;
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
                    if (!Input.IsPush(Button.Down) || this.Select >= this.list.Count - 1)
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
            bool flag = this.savedata.virusSPbusted[Wanted.WantedList[this.list[this.Select], 0]];
            int wanted = Wanted.WantedList[this.list[this.Select], 0];
            this.eventmanager.events.Clear();
            this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
            var dialogue = ShanghaiEXE.Translate("Bounty.AcceptDialogue1Format").Format(this.encountArea[this.list[this.Select]]);
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
            if (this.savedata.ValList[12] == this.list[this.Select])
            {
                dialogue = ShanghaiEXE.Translate("Bounty.AcceptDialogue2AlreadyTaken");
                this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
            }
            else
            {
                var question = ShanghaiEXE.Translate("Bounty.AcceptDialogue2Question");
                var options = ShanghaiEXE.Translate("Bounty.AcceptDialogue2Options");
                this.eventmanager.AddEvent(new Question(this.sound, this.eventmanager, question[0], options[0], options[1], false, true, false, question.Face, this.savedata, true));
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
                this._position = new Vector2(8f, 0.0f);
                this._rect = new Rectangle(648, 584, 48, 16);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                for (int index1 = 0; index1 < Math.Min(8, this.list.Count); ++index1)
                {
                    EnemyBase enemyBase = EnemyBase.EnemyMake(Wanted.WantedList[this.list[this.top + index1], 0], null, false);
                    enemyBase.version = 0;
                    var enemyVersion = EnemyBase.EnemyMake(Wanted.WantedList[this.list[this.top + index1], 0], enemyBase, false);
                    int index2 = this.list[this.top + index1];
                    if (this.savedata.ValList[12] != index2)
                    {
                        var checkmarkSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("BountyBoard.CompleteMark");
                        if (this.savedata.virusSPbusted[Wanted.WantedList[index2, 0]])
                        {
                            this._rect = checkmarkSprite.Item2;
                        }
                        else
                            this._rect = new Rectangle(528 + (this.cursolanime % 2 == 0 ? 0 : 16), 600, 16, 16);
                        this._position = new Vector2(24f, 16 + 16 * index1);
                        dg.DrawImage(dg, checkmarkSprite.Item1, this._rect, true, this._position, Color.White);
                    }
                    this._position = new Vector2(48f, 17 + 16 * index1);
                    dg.DrawMiniText(enemyVersion.Name, this._position, Color.FromArgb(32, 32, 32));
                    this._position = new Vector2(128f, 17 + 16 * index1);
                    dg.DrawMiniText(Wanted.WantedList[index2, 3].ToString() + " Z", this._position, Color.FromArgb(32, 32, 32));
                    if (this.Select == this.top + index1)
                    {
                        if (this.savedata.virusSPbusted[Wanted.WantedList[index2, 0]])
                        {
                            enemyVersion.positionDirect = new Vector2(208 + enemyVersion.wantedPosition.X - enemyVersion.Wide / 4, 80 + enemyVersion.wantedPosition.Y);
                            enemyVersion.rebirth = false;
                            enemyVersion.alfha = byte.MaxValue;
                            enemyVersion.Render(dg);
                        }
                        else
                        {
                            this._rect = new Rectangle(0, 0, enemyVersion.Wide, enemyVersion.Height);
                            this._position = new Vector2(208 + enemyVersion.wantedPosition.X, 80 + enemyVersion.wantedPosition.Y);
                            dg.DrawImage(dg, enemyVersion.picturename, this._rect, false, this._position, Color.Black);
                        }
                    }
                }
                int num1 = 0;
                foreach (int index in this.list)
                {
                    if (this.savedata.ValList[12] != index && !this.savedata.virusSPbusted[Wanted.WantedList[index, 0]])
                        ++num1;
                }
                string txt = num1.ToString() + "/" + this.list.Count.ToString();
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
