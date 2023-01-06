using Common;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEvent;
using NSGame;
using NSNet;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSBattle
{
    internal class Result : AllBase
    {
        private byte alpha = 0;
        private Color timecolor = Color.White;
        private readonly bool[,] screen = new bool[7, 6];
        private readonly byte bustinglevel = 0;
        private byte selectedBustinglevel = 0;
        private int selectedCounterBugs;
        private int selectedCounterErrors;
        private int selectedCounterFreezes;
        private readonly Point[] openscreen = new Point[42];
        private byte screenflame = 0;
        private readonly int[] getitem = new int[3];
        public const int stylechange = 50;
        private byte[] time;
        private Result.RESULT scene;
        private readonly SceneBattle parent;
        private readonly SceneMain playerdate;
        private bool printtoggle;
        private Vector2 windowposition;
        private ChipFolder getchip;
        private readonly NSBattle.Character.Player player;
        private int dropmoney;
        private bool dropchip;
        private string stylename;
        private NSBattle.Character.Player.STYLE style;
        private ChipBase.ELEMENT element;
        private string picture;
        private bool changeevent1;
        private bool changeevent2;
        private Result.RESULT next;
        protected SaveData savedata;
        private Result.RESULT print;
        private readonly bool illegal;
        private bool chooseIllegal;
        private int illegalanime;
        private int illegalanimeSub;

        private bool resultError;

        public Result(
          IAudioEngine s,
          SceneBattle p,
          SceneMain main,
          byte[] t,
          byte simu,
          byte damage,
          byte move,
          int[] stylepoint,
          NSBattle.Character.Player pl,
          SaveData save,
          bool escape)
          : base(s)
        {
            this.savedata = save;
            this.player = pl;
            for (int index1 = 0; index1 < this.screen.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.screen.GetLength(1); ++index2)
                    this.screen[index1, index2] = true;
            }
            for (int index = 0; index < this.openscreen.Length; ++index)
            {
                this.openscreen[index] = new Point
                {
                    X = index % 7,
                    Y = index / 7
                };
            }
            this.openscreen = ((IEnumerable<Point>)this.openscreen).OrderBy<Point, Guid>(i => Guid.NewGuid()).ToArray<Point>();
            this.parent = p;
            this.playerdate = main;
            this.windowposition = new Vector2(-192f, 16f);
            if (!escape && !this.parent.noFanfale)
            {
                this.scene = Result.RESULT.finish;
                this.sound.StopBGM();
                if (this.parent.doresult)
                    this.sound.StartBGM("result");
                else
                    this.sound.StartBGM("result_short");

                this.bustinglevel = this.CalculateBustingRank(t, simu, damage, move);
                this.selectedBustinglevel = this.bustinglevel;

                this.selectedCounterBugs = this.parent.lastCounterBug;
                this.selectedCounterErrors = this.parent.lastCounterError;
                this.selectedCounterFreezes = this.parent.lastCounterFreeze;

                if (this.parent.resultError)
                {
                    this.resultError = true;
                    this.illegal = false;
                }
                else
                {
                    this.illegal = (this.parent.illegal || (this.player.addonSkill[52] && this.bustinglevel == 11));
                }
                this.chooseIllegal = this.illegal;

                this.CalculateLoot();

                int num2 = -1;
                int index1 = 0;
                for (int index2 = 0; index2 < stylepoint.Length; ++index2)
                {
                    if (stylepoint[index2] > num2)
                    {
                        index1 = index2;
                        num2 = stylepoint[index2];
                    }
                }
                ++this.savedata.stylepoint[index1];
                if (this.parent.doresult)
                    ++this.savedata.manybattle;
            }
            else
                this.scene = Result.RESULT.end;
        }

        public void Update()
        {
            ++this.frame;
            if (this.frame > 1024)
                this.frame = 0;
            if (this.frame % 5 == 0)
                this.printtoggle = !this.printtoggle;
            switch (this.scene)
            {
                case Result.RESULT.finish:
                    if (this.frame <= 90)
                        break;
                    if (this.parent.doresult)
                    {
                        this.scene = Result.RESULT.fadein;
                        if (this.savedata.FlagList[3] && this.savedata.ValList[9] == 4)
                            this.EventtutorialTutorealend();
                    }
                    else
                        this.scene = Result.RESULT.end;
                    this.frame = 0;
                    break;
                case Result.RESULT.fadein:
                    if (windowposition.X < 24.0)
                    {
                        this.windowposition.X += 8f;
                        break;
                    }
                    this.next = Result.RESULT.printchip;
                    this.scene = Result.RESULT.waitInput;
                    this.frame = 0;
                    break;
                case Result.RESULT.waitInput:
                    if (Input.IsPress(Button._A))
                    {
                        if (this.chooseIllegal)
                        {
                            this.sound.PlaySE(SoundEffect.noise);
                        }
                        this.scene = this.next;
                        this.print = this.next;
                        this.frame = 0;
                        this.AddLoot();
                    }
                    else if (this.savedata.addonSkill[72])
                    {
                        if (Input.IsPress(Button.Up))
                        {
                            if (this.selectedBustinglevel == this.bustinglevel)
                            {
                                this.chooseIllegal = this.illegal;
                            }
                            else
                            {
                                this.selectedBustinglevel = (byte)Math.Min(this.bustinglevel, this.selectedBustinglevel + 1);
                            }
                            this.CalculateLoot();
                        }
                        else if (Input.IsPress(Button.Down))
                        {
                            if (this.chooseIllegal && this.selectedBustinglevel == this.bustinglevel)
                            {
                                this.chooseIllegal = false;
                            }
                            else
                            {
                                this.selectedBustinglevel = (byte)Math.Max(1, this.selectedBustinglevel - 1);
                            }
                            this.CalculateLoot();
                        }
                        /*
                        else if (Input.IsPress(Button.Left))
                        {
                            if (this.selectedCounterBugs > 0)
                            {
                                this.selectedCounterBugs--;
                            }
                            else if (this.selectedCounterFreezes > 0)
                            {
                                this.selectedCounterFreezes--;
                            }
                            else if (this.selectedCounterErrors > 0)
                            {
                                this.selectedCounterErrors--;
                            }
                            this.CalculateLoot();
                        }
                        else if (Input.IsPress(Button.Right))
                        {
                            if (this.parent.lastCounterError > this.selectedCounterErrors)
                            {
                                this.selectedCounterErrors++;
                            }
                            else if (this.parent.lastCounterFreeze > this.selectedCounterFreezes)
                            {
                                this.selectedCounterFreezes++;
                            }
                            else if (this.parent.lastCounterBug > this.selectedCounterBugs)
                            {
                                this.selectedCounterBugs++;
                            }
                            this.CalculateLoot();
                        }
                        */
                    }
                    break;
                case Result.RESULT.printchip:
                case Result.RESULT.printBug:
                case Result.RESULT.printFreeze:
                case Result.RESULT.printError:
                    if (this.frame % 3 != 0)
                        break;
                    this.screen[this.openscreen[screenflame].X, this.openscreen[screenflame].Y] = false;
                    ++this.screenflame;
                    this.screen[this.openscreen[screenflame].X, this.openscreen[screenflame].Y] = false;
                    ++this.screenflame;

                    if (!this.chooseIllegal)
                    {
                        if (this.resultError)
                        {
                            this.sound.PlaySE(SoundEffect.error);
                        }
                        else
                        {
                            this.sound.PlaySE(SoundEffect.openchip);
                        }
                    }

                    if (this.screenflame >= 42)
                    {
                        for (int index1 = 0; index1 < this.screen.GetLength(0); ++index1)
                        {
                            for (int index2 = 0; index2 < this.screen.GetLength(1); ++index2)
                                this.screen[index1, index2] = false;
                        }
                        this.frame = 0;
                        if (this.dropchip && this.scene == Result.RESULT.printchip)
                        {
                            this.sound.PlaySE(SoundEffect.getchip);
                        }
                        else
                        {
                            this.sound.PlaySE(SoundEffect.getzenny);
                        }
                        switch (this.scene)
                        {
                            case Result.RESULT.printchip:
                                this.print = Result.RESULT.printchipend;
                                this.scene = Result.RESULT.printchipend;
                                break;
                            case Result.RESULT.printBug:
                                this.print = Result.RESULT.printBugend;
                                this.scene = Result.RESULT.printBugend;
                                break;
                            case Result.RESULT.printFreeze:
                                this.print = Result.RESULT.printFreezeend;
                                this.scene = Result.RESULT.printFreezeend;
                                break;
                            case Result.RESULT.printError:
                                this.print = Result.RESULT.printErrorend;
                                this.scene = Result.RESULT.printErrorend;
                                break;
                        }
                    }
                    break;
                case Result.RESULT.printchipend:
                case Result.RESULT.printBugend:
                case Result.RESULT.printFreezeend:
                case Result.RESULT.printErrorend:
                    if (!Input.IsPress(Button._A))
                        break;
                    bool flag = false;
                    switch (this.scene)
                    {
                        case Result.RESULT.printchipend:
                            if (this.selectedCounterBugs > 0)
                            {
                                this.next = Result.RESULT.printBug;
                                this.print = this.next;
                                break;
                            }
                            if (this.selectedCounterFreezes > 0)
                            {
                                this.next = Result.RESULT.printFreeze;
                                this.print = this.next;
                                break;
                            }
                            if (this.selectedCounterErrors > 0)
                            {
                                this.next = Result.RESULT.printError;
                                this.print = this.next;
                                break;
                            }
                            flag = true;
                            break;
                        case Result.RESULT.printBugend:
                            if (this.selectedCounterFreezes > 0)
                            {
                                this.next = Result.RESULT.printFreeze;
                                this.print = this.next;
                                break;
                            }
                            if (this.selectedCounterErrors > 0)
                            {
                                this.next = Result.RESULT.printError;
                                this.print = this.next;
                                break;
                            }
                            flag = true;
                            break;
                        case Result.RESULT.printFreezeend:
                            if (this.selectedCounterErrors > 0)
                            {
                                this.next = Result.RESULT.printError;
                                this.print = this.next;
                                break;
                            }
                            flag = true;
                            break;
                        case Result.RESULT.printErrorend:
                            flag = true;
                            break;
                    }
                    if (flag)
                    {
                        if (this.savedata.manybattle < stylechange || !this.savedata.FlagList[5] || this.savedata.FlagList[0])
                        {
                            this.scene = Result.RESULT.end;
                        }
                        else
                        {
                            this.savedata.manybattle = 0;
                            this.scene = Result.RESULT.fadeout;
                        }
                        this.frame = 0;
                    }
                    else
                    {
                        for (int index1 = 0; index1 < this.screen.GetLength(0); ++index1)
                        {
                            for (int index2 = 0; index2 < this.screen.GetLength(1); ++index2)
                                this.screen[index1, index2] = true;
                        }
                        this.screenflame = 0;
                        this.scene = this.next;
                    }
                    break;
                case Result.RESULT.fadeout:
                    if (windowposition.X < 240.0)
                    {
                        this.windowposition.X += 8f;
                        break;
                    }
                    if (this.alpha < byte.MaxValue)
                    {
                        this.alpha += 5;
                    }
                    else
                    {
                        this.alpha = byte.MaxValue;
                        this.parent.enemys.Clear();
                        this.parent.effects.Clear();
                        this.parent.attacks.Clear();
                        this.parent.objects.Clear();
                        this.player.position = new Point(1, 0);
                        this.player.PositionDirectSet();
                        this.player.barierTime = 0;
                        this.player.barrierType = CharacterBase.BARRIER.None;
                        this.parent.mind.MindNow = MindWindow.MIND.normal;
                        for (int index1 = 0; index1 < this.parent.panel.GetLength(0); ++index1)
                        {
                            for (int index2 = 0; index2 < this.parent.panel.GetLength(1); ++index2)
                            {
                                this.parent.panel[index1, index2].state = Panel.PANEL._nomal;
                                this.parent.panel[index1, index2].color = index1 >= 3 ? Panel.COLOR.blue : Panel.COLOR.red;
                            }
                        }
                        this.DecideStyle();
                        this.picture = this.StyleGraphicsName(this.style, this.element);
                        this.sound.StartBGM("stylechange");
                        this.scene = Result.RESULT.stylechange;
                    }
                    break;
                case Result.RESULT.stylechange:
                    if (this.alpha > 0)
                    {
                        this.alpha -= 5;
                        break;
                    }
                    this.parent.eventmanager.events.Clear();
                    if (!this.changeevent1)
                    {
                        if (this.savedata.firstchange)
                        {
                            this.EventStylechangeSecond();
                        }
                        else
                        {
                            this.EventStylechangeFirst();
                            this.savedata.firstchange = true;
                        }
                        this.changeevent1 = true;
                    }
                    else if (!this.changeevent2)
                    {
                        this.EventStylechangeAfter();
                        if (this.savedata.havestyles >= this.savedata.style.Length)
                            this.StyleSelect();
                        this.changeevent2 = true;
                    }
                    else
                    {
                        if (this.savedata.havestyles >= this.savedata.style.Length)
                        {
                            if (this.savedata.ValList[6] >= 0)
                            {
                                this.savedata.style[this.savedata.ValList[6] + 1].style = (int)this.style;
                                this.savedata.style[this.savedata.ValList[6] + 1].element = (int)this.element;
                                this.savedata.setstyle = this.savedata.ValList[6] + 1;
                            }
                        }
                        else
                        {
                            this.savedata.style[this.savedata.havestyles].style = (int)this.style;
                            this.savedata.style[this.savedata.havestyles].element = (int)this.element;
                            this.savedata.setstyle = this.savedata.havestyles;
                            ++this.savedata.havestyles;
                        }
                        this.scene = Result.RESULT.end;
                    }
                    break;
                case Result.RESULT.end:
                    if (this.alpha < byte.MaxValue)
                    {
                        if (this.parent.noFanfale)
                        {
                            ++this.alpha;
                            break;
                        }
                        this.alpha += 15;
                        break;
                    }
                    this.parent.parent.TexClear(false);
                    this.savedata.HPnow = !this.savedata.isJackedIn ? this.savedata.HPMax : this.parent.player.Hp;
                    this.parent.main.mapscene.HP.hpprint = this.savedata.HPnow;
                    this.savedata.mind = this.parent.mind.MindNow == MindWindow.MIND.pinch || this.parent.mind.MindNow == MindWindow.MIND.dark || this.parent.mind.MindNow == MindWindow.MIND.angry ? 0 : (int)this.parent.mind.MindNow;
                    this.sound.StopBGM();
                    this.playerdate.NowScene = SceneMain.PLAYSCENE.map;
                    this.parent.main.mapscene.battleflag = false;
                    this.parent.main.mapscene.fadeColor = Color.Black;
                    break;
            }
        }

        public void Render(IRenderer dg)
        {
            switch (this.scene)
            {
                case Result.RESULT.finish:
                    this._position = new Vector2(120f, 80f);
                    if (ShanghaiEXE.Config.FixEngrish ?? true)
                    {
                        this._rect = new Rectangle(384, 616, 128, 16);
                    }
                    else
                    {
                        this._rect = new Rectangle(384, 0, 128, 16);
                    }
                    float scall = Math.Max(0, this.frame >= 15 ? (this.frame >= 17 ? (this.frame >= 60 ? (float)(1.0 - (this.frame - 60) * 0.0599999986588955) : 1f) : 1.02f) : frame * 0.06f);
                    dg.DrawImage(dg, "battleobjects", this._rect, false, this._position, scall, 0.0f, Color.White);
                    break;
                case Result.RESULT.stylechange:
                case Result.RESULT.end:
                    if (this.frame > 300)
                    {
                        if (this.frame < 600)
                        {
                            if (this.frame % 30 == 0)
                                this.player.printplayer = !this.player.printplayer;
                            ++this.frame;
                        }
                        else if (this.frame < 800)
                        {
                            if (this.frame % 15 == 0)
                                this.player.printplayer = !this.player.printplayer;
                            ++this.frame;
                        }
                        else if (this.frame < 900)
                        {
                            if (this.frame % 7 == 0)
                                this.player.printplayer = !this.player.printplayer;
                            ++this.frame;
                        }
                        else if (this.frame >= 900)
                            this.player.printplayer = false;
                    }
                    else
                        ++this.frame;
                    if (!this.player.printplayer)
                    {
                        this._rect = new Rectangle(this.player.animationpoint.X * this.player.Wide, this.player.animationpoint.Y * this.player.Height, this.player.Wide, this.player.Height);
                        this._position = new Vector2((int)this.player.positionDirect.X, (int)this.player.positionDirect.Y);
                        dg.DrawImage(dg, this.picture, this._rect, false, this._position, false, Color.White);
                        break;
                    }
                    break;
                default:
                    var rewardPanelSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite(!this.chooseIllegal ? "Result.RewardNormal" : "Result.RewardNoise");
                    this._rect = rewardPanelSprite.Item2;
                    this._position = this.windowposition;
                    dg.DrawImage(dg, rewardPanelSprite.Item1, this._rect, true, this.windowposition, Color.White);
                    this._position = new Vector2(this.windowposition.X + 152f, this.windowposition.Y + 31f);
                    this.CountRender(dg, this.Count(this.time[0]), this._position, this.timecolor);
                    this._position = new Vector2(this.windowposition.X + 128f, this.windowposition.Y + 31f);
                    this.CountRender(dg, this.Count(this.time[1]), this._position, this.timecolor);
                    this._position = new Vector2(this.windowposition.X + 104f, this.windowposition.Y + 31f);
                    this.CountRender(dg, this.Count(this.time[2]), this._position, this.timecolor);
                    this._rect = new Rectangle(360 + selectedBustinglevel * 16, 96, 16, 16);
                    this._position = new Vector2(this.windowposition.X + 128f, this.windowposition.Y + 47f);
                    dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                    int num = 0;
                    for (int index = 0; index < this.selectedCounterBugs; ++index)
                    {
                        this._rect = new Rectangle(352, 24, 8, 8);
                        this._position = new Vector2(this.windowposition.X + 144f + num * 8, this.windowposition.Y + 55f);
                        dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                        ++num;
                    }
                    for (int index = 0; index < this.selectedCounterFreezes; ++index)
                    {
                        this._rect = new Rectangle(360, 24, 8, 8);
                        this._position = new Vector2(this.windowposition.X + 144f + num * 8, this.windowposition.Y + 55f);
                        dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                        ++num;
                    }
                    for (int index = 0; index < this.selectedCounterErrors; ++index)
                    {
                        this._rect = new Rectangle(368, 24, 8, 8);
                        this._position = new Vector2(this.windowposition.X + 144f + num * 8, this.windowposition.Y + 55f);
                        dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                        ++num;
                    }
                    if (this.savedata.addonSkill[72] && this.scene == Result.RESULT.waitInput)
                    {
                        var bobStage = (this.frame % 12) / 4;
                        if (this.selectedBustinglevel != this.bustinglevel || (this.illegal && !this.chooseIllegal))
                        {
                            this._rect = new Rectangle(160 + 8 * bobStage, 208, 8, 8);
                            this._position = new Vector2(this.windowposition.X + 122f, this.windowposition.Y + 50f);
                            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        }
                        if (this.selectedBustinglevel != 1)
                        {
                            this._rect = new Rectangle(184 + 8 * bobStage, 208, 8, 8);
                            this._position = new Vector2(this.windowposition.X + 122f, this.windowposition.Y + 56f);
                            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        }
                    }
                    if ((this.scene == Result.RESULT.waitInput || this.scene == Result.RESULT.printBugend || (this.scene == Result.RESULT.printchipend || this.scene == Result.RESULT.printErrorend) || this.scene == Result.RESULT.printFreezeend) && this.printtoggle)
                    {
                        this._rect = new Rectangle(352, 16, 80, 8);
                        this._position = new Vector2(this.windowposition.X + 16f, this.windowposition.Y + 116f);
                        dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                    }
                    this._position = new Vector2(this.windowposition.X + 112f, this.windowposition.Y + 80f);
                    var fragLabelSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("Result.FragLabel");
                    switch (this.print)
                    {
                        case Result.RESULT.printchip:
                        case Result.RESULT.printchipend:
                            if (this.dropchip)
                            {
                                this._position = new Vector2(this.windowposition.X + 112f, this.windowposition.Y + 80f);
                                this.getchip.chip.GraphicsRender(dg, this._position, this.getchip.codeNo, true, false);
                                if (this.scene == Result.RESULT.printchipend || this.scene == Result.RESULT.fadeout || this.scene == Result.RESULT.end)
                                {
                                    string name = this.getchip.chip.name;
                                    this._position = new Vector2(this.windowposition.X + 20f, this.windowposition.Y + 96f);
                                    this.TextRender(dg, name, false, this._position, true);
                                    this._rect = new Rectangle((int)this.getchip.chip.code[this.getchip.codeNo] * 8, 48, 8, 16);
                                    this._position = new Vector2(this.windowposition.X + 88f, this.windowposition.Y + 96f);
                                    dg.DrawImage(dg, "font", this._rect, true, this._position, Color.Yellow);
                                    break;
                                }
                                break;
                            }
                            this._rect = new Rectangle(0, 0, 56, 48);
                            dg.DrawImage(dg, "chipgraphic0", this._rect, true, this._position, Color.White);
                            if (this.scene == Result.RESULT.printchipend || this.scene == Result.RESULT.fadeout || this.scene == Result.RESULT.end)
                            {
                                int[] pcount = this.Count(this.dropmoney);
                                this.CountRender(dg, pcount, new Vector2(this.windowposition.X + 72f, this.windowposition.Y + 96f), Color.White);
                                this._rect = new Rectangle(200, 48, 8, 16);
                                this._position = new Vector2(this.windowposition.X + 88f, this.windowposition.Y + 96f);
                                dg.DrawImage(dg, "font", this._rect, true, this._position, Color.Yellow);
                            }
                            break;
                        case Result.RESULT.printBug:
                        case Result.RESULT.printBugend:
                            this._rect = new Rectangle(56 * this.selectedCounterBugs, 0, 56, 48);
                            dg.DrawImage(dg, "chipgraphic0", this._rect, true, this._position, Color.White);
                            if (this.scene == Result.RESULT.printBugend || this.scene == Result.RESULT.fadeout || this.scene == Result.RESULT.end)
                            {
                                string txt = ShanghaiEXE.Translate("BattleResult.EndBug");
                                this._position = new Vector2(this.windowposition.X + 20f, this.windowposition.Y + 96f);
                                this.TextRender(dg, txt, false, this._position, true);
                                this._position = new Vector2(this.windowposition.X + 20f + 8 * txt.Length, this.windowposition.Y + 96f);
                                this._rect = fragLabelSprite.Item2;
                                dg.DrawImage(dg, fragLabelSprite.Item1, this._rect, true, this._position, Color.White);
                                this._position = new Vector2(this.windowposition.X + 88f, this.windowposition.Y + 96f);
                                this.TextRender(dg, this.getitem[0].ToString(), true, this._position, true, Color.Yellow);
                                break;
                            }
                            break;
                        case Result.RESULT.printFreeze:
                        case Result.RESULT.printFreezeend:
                            this._rect = new Rectangle(168 + 56 * this.selectedCounterFreezes, 0, 56, 48);
                            dg.DrawImage(dg, "chipgraphic0", this._rect, true, this._position, Color.White);
                            if (this.scene == Result.RESULT.printFreezeend || this.scene == Result.RESULT.fadeout || this.scene == Result.RESULT.end)
                            {
                                string txt = ShanghaiEXE.Translate("BattleResult.EndFrz");
                                this._position = new Vector2(this.windowposition.X + 20f, this.windowposition.Y + 96f);
                                this.TextRender(dg, txt, false, this._position, true);
                                this._position = new Vector2(this.windowposition.X + 20f + 8 * txt.Length, this.windowposition.Y + 96f);
                                this._rect = fragLabelSprite.Item2;
                                dg.DrawImage(dg, fragLabelSprite.Item1, this._rect, true, this._position, Color.White);
                                this._position = new Vector2(this.windowposition.X + 88f, this.windowposition.Y + 96f);
                                this.TextRender(dg, this.getitem[1].ToString(), true, this._position, true, Color.Yellow);
                                break;
                            }
                            break;
                        case Result.RESULT.printError:
                        case Result.RESULT.printErrorend:
                            this._rect = new Rectangle(336 + 56 * this.selectedCounterErrors, 0, 56, 48);
                            dg.DrawImage(dg, "chipgraphic0", this._rect, true, this._position, Color.White);
                            if (this.scene == Result.RESULT.printErrorend || this.scene == Result.RESULT.fadeout || this.scene == Result.RESULT.end)
                            {
                                string txt = ShanghaiEXE.Translate("BattleResult.EndError");
                                this._position = new Vector2(this.windowposition.X + 20f, this.windowposition.Y + 96f);
                                this.TextRender(dg, txt, false, this._position, true);
                                this._position = new Vector2(this.windowposition.X + 20f + 8 * txt.Length, this.windowposition.Y + 96f);
                                this._rect = fragLabelSprite.Item2;
                                dg.DrawImage(dg, fragLabelSprite.Item1, this._rect, true, this._position, Color.White);
                                this._position = new Vector2(this.windowposition.X + 88f, this.windowposition.Y + 96f);
                                this.TextRender(dg, this.getitem[2].ToString(), true, this._position, true, Color.Yellow);
                                break;
                            }
                            break;
                    }
                    this._rect = new Rectangle(360, 272, 8, 8);
                    for (int index1 = 0; index1 < this.screen.GetLength(0); ++index1)
                    {
                        for (int index2 = 0; index2 < this.screen.GetLength(1); ++index2)
                        {
                            if (this.screen[index1, index2])
                            {
                                this._position = new Vector2(this.windowposition.X + 112f + index1 * 8, this.windowposition.Y + 80f + index2 * 8);
                                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                            }
                        }
                    }
                    if (this.chooseIllegal && this.screenflame < 42)
                    {
                        ++this.illegalanimeSub;
                        if (this.illegalanimeSub >= 3)
                        {
                            this.illegalanimeSub = 0;
                            ++this.illegalanime;
                            if (this.illegalanime >= 10)
                                this.illegalanime = 0;
                        }
                        this._position = new Vector2(this.windowposition.X + 104f, this.windowposition.Y + 72f);
                        this._rect = new Rectangle(72 * this.illegalanime, 440, 72, 64);
                        Color color = Color.FromArgb(byte.MaxValue - screenflame * 6, Color.White);
                        dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, color);
                        break;
                    }
                    break;
            }
            if (this.alpha <= 0)
                return;
            Color color1 = this.scene == Result.RESULT.end ? Color.FromArgb(alpha, 0, 0, 0) : Color.FromArgb(alpha, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color1);
        }

        private int[] Count(int m)
        {
            int[] numArray;
            if (m < 10)
            {
                numArray = new int[2] { m, 0 };
            }
            else
            {
                int length = m.ToString().Length;
                numArray = new int[length];
                for (int b = 0; b < length; ++b)
                {
                    int num = (int)MyMath.Pow(10f, b);
                    numArray[b] = m / num % 10;
                }
            }
            return numArray;
        }

        private void CountRender(IRenderer dg, int[] pcount, Vector2 p, Color c)
        {
            foreach (var data in ((IEnumerable<int>)pcount).Select((v, i) => new
            {
                v,
                i
            }))
            {
                this._rect = new Rectangle(data.v * 8, 0, 8, 16);
                this._position = new Vector2(p.X - data.i * 8, p.Y);
                dg.DrawImage(dg, "font", this._rect, true, this._position, c);
            }
        }

        private void AddLoot()
        {
            if (this.resultError)
            {
                return;
            }

            if (this.parent.doresult)
            {
                if (this.dropchip)
                {
                    this.savedata.AddChip(this.getchip.chip.number, this.getchip.codeNo, true);
                }
                else
                {
                    if (this.player.addonSkill[1])
                        this.dropmoney *= 2;
                    this.savedata.Money += this.dropmoney;
                }
                for (int index = 0; index < this.getitem.Length; ++index)
                    this.savedata.havePeace[index] += this.getitem[index];
            }
        }

        private void CalculateLoot()
        {
            if (this.resultError)
            {
                this.dropchip = true;
                var errorChipFolder = new ChipFolder(this.sound);
                errorChipFolder.chip = new Error(this.sound);
                this.getchip = errorChipFolder;
            }
            else
            {
                var chipThreshold = this.GetThresholdFromBustingLevel(this.selectedBustinglevel);

                this.dropchip = this.chooseIllegal
                    || this.parent.dropzenny <= 0
                    || this.player.addonSkill[2]
                    || (this.Random.Next(100) >= chipThreshold && !this.player.addonSkill[1]);
                if (!this.chooseIllegal)
                {
                    if (this.dropchip)
                    {
                        this.getchip = this.GetChipDrop(this.selectedBustinglevel, this.parent.dropchip);
                    }
                    else
                    {
                        this.dropmoney = this.GetZennyDrop(this.selectedBustinglevel, this.parent.dropzenny);
                    }
                }
                else
                {
                    this.getchip = this.IllegalGetMake(parent.dropchip[4].chip.reality);
                }
            }

            switch (this.selectedCounterBugs)
            {
                case 1:
                    this.getitem[0] = 1;
                    break;
                case 2:
                    this.getitem[0] = 3;
                    break;
                case 3:
                    this.getitem[0] = 10;
                    break;
            }
            switch (this.selectedCounterFreezes)
            {
                case 1:
                    this.getitem[1] = 1;
                    break;
                case 2:
                    this.getitem[1] = 3;
                    break;
                case 3:
                    this.getitem[1] = 10;
                    break;
            }
            switch (this.selectedCounterErrors)
            {
                case 1:
                    this.getitem[2] = 1;
                    break;
                case 2:
                    this.getitem[2] = 3;
                    break;
                case 3:
                    this.getitem[2] = 10;
                    break;
            }
        }

        private int GetThresholdFromBustingLevel(byte bustingLevel)
        {
            switch (bustingLevel)
            {
                default:
                case 11:
                    return 0;
                case 10:
                case 9:
                    return 10;
                case 8:
                case 7:
                    return 20;
                case 6:
                case 5:
                    return 40;
                case 4:
                case 3:
                case 2:
                    return 70;
                case 1:
                case 0:
                    return 100;
            }
        }

        private int GetZennyDrop(byte bustingLevel, int baseMoney)
        {
            switch (bustingLevel)
            {
                default:
                case 11:
                case 10:
                case 9:
                    return baseMoney;
                case 8:
                case 7:
                    return baseMoney / 2;
                case 6:
                case 5:
                    return baseMoney / 5;
                case 4:
                case 3:
                case 2:
                case 1:
                case 0:
                    return baseMoney / 10;
            }
        }

        private ChipFolder GetChipDrop(byte bustingLevel, ChipFolder[] chips)
        {
            switch (bustingLevel)
            {
                default:
                case 11:
                    return chips[4];
                case 10:
                case 9:
                    return chips[3];
                case 8:
                case 7:
                    return chips[2];
                case 6:
                case 5:
                    return chips[1];
                case 4:
                case 3:
                case 2:
                case 1:
                case 0:
                    return chips[0];
            }
        }

        private byte CalculateBustingRank(byte[] t, byte simu, byte damage, byte move)
        {
            this.time = t;
            var bustingLevel = (byte)0;
            if (this.time[2] == 0)
            {
                if (!this.parent.bossFlag)
                {
                    if (this.time[1] >= 0 && this.time[1] <= 5)
                    {
                        bustingLevel += 7;
                        this.timecolor = Color.LightGreen;
                        this.player.PluspointShinobi(4);
                    }
                    else if (this.time[1] > 5 && this.time[1] <= 12)
                        bustingLevel += 6;
                    else if (this.time[1] > 12 && this.time[1] <= 36)
                        bustingLevel += 5;
                    else
                        bustingLevel += 4;
                }
                else if (this.time[1] >= 0 && this.time[1] < 30)
                {
                    bustingLevel += 10;
                    this.timecolor = Color.LightGreen;
                    this.player.PluspointShinobi(2);
                }
                else if (this.time[1] >= 30 && this.time[1] <= 40)
                    bustingLevel += 8;
                else if (this.time[1] > 40 && this.time[1] <= 50)
                    bustingLevel += 6;
                else
                    bustingLevel += 4;
            }
            else
                bustingLevel += 4;
            switch (damage)
            {
                case 0:
                    ++bustingLevel;
                    break;
                case 1:
                    break;
                case 2:
                    --bustingLevel;
                    break;
                case 3:
                    bustingLevel -= 2;
                    break;
                default:
                    bustingLevel -= 3;
                    break;
            }
            switch (simu)
            {
                case 2:
                    bustingLevel += 2;
                    break;
                case 3:
                    bustingLevel += 4;
                    break;
            }
            bustingLevel += this.parent.manyCounter;
            if (move <= 2)
                ++bustingLevel;
            if (bustingLevel > 11)
                bustingLevel = 11;

            return bustingLevel;
        }

        private ChipFolder IllegalGetMake(int rare)
        {
            List<ChipS> chipSList = new List<ChipS>();
            for (int index = 1; index < byte.MaxValue; ++index)
            {
                ChipFolder chipFolder = new ChipFolder(this.sound);
                chipFolder.SettingChip(index);
                if (!(chipFolder.chip is DammyChip))
                    chipSList.Add(new ChipS(index, 0));
            }
            for (int index = 350; index < 429; ++index)
            {
                ChipFolder chipFolder = new ChipFolder(this.sound);
                chipFolder.SettingChip(index);
                if (!(chipFolder.chip is DammyChip))
                    chipSList.Add(new ChipS(index, 0));
            }
            int min = Math.Max(1, rare - 2);
            chipSList.RemoveAll(c =>
           {
               if (min <= c.Reality)
                   return rare < c.Reality;
               return true;
           });
            ChipS chipS = chipSList[this.Random.Next(chipSList.Count)];
            ChipFolder chipFolder1 = new ChipFolder(this.sound);
            chipFolder1.SettingChip(chipS.number);
            chipFolder1.codeNo = this.Random.Next(4);
            return chipFolder1;
        }

        private string StyleGraphicsName(NSBattle.Character.Player.STYLE st, ChipBase.ELEMENT ele)
        {
            string str = "";
            bool flag = false;
            switch (st)
            {
                case NSBattle.Character.Player.STYLE.normal:
                    this.stylename = ShanghaiEXE.Translate("BattleResult.StyleGraphicNormal");
                    str = "shanghai";
                    flag = true;
                    break;
                case NSBattle.Character.Player.STYLE.fighter:
                    this.stylename = ShanghaiEXE.Translate("BattleResult.StyleGraphicFght");
                    str = "Fighter";
                    break;
                case NSBattle.Character.Player.STYLE.shinobi:
                    this.stylename = ShanghaiEXE.Translate("BattleResult.StyleGraphicNinj");
                    str = "Shinobi";
                    break;
                case NSBattle.Character.Player.STYLE.doctor:
                    this.stylename = ShanghaiEXE.Translate("BattleResult.StyleGraphicDoc");
                    str = "Doctor";
                    break;
                case NSBattle.Character.Player.STYLE.gaia:
                    this.stylename = ShanghaiEXE.Translate("BattleResult.StyleGraphicGaia");
                    str = "Gaia";
                    break;
                case NSBattle.Character.Player.STYLE.wing:
                    this.stylename = ShanghaiEXE.Translate("BattleResult.StyleGraphicWing");
                    str = "Wing";
                    break;
                case NSBattle.Character.Player.STYLE.witch:
                    this.stylename = ShanghaiEXE.Translate("BattleResult.StyleGraphicWtch");
                    str = "Witch";
                    break;
            }
            if (flag)
                return "shanghai";
            switch (ele)
            {
                case ChipBase.ELEMENT.heat:
                    this.stylename = ShanghaiEXE.Translate("BattleResult.StyleGraphicHeat") + this.stylename;
                    str += "Heat";
                    break;
                case ChipBase.ELEMENT.aqua:
                    this.stylename = ShanghaiEXE.Translate("BattleResult.StyleGraphicAqua") + this.stylename;
                    str += "Aqua";
                    break;
                case ChipBase.ELEMENT.eleki:
                    this.stylename = ShanghaiEXE.Translate("BattleResult.StyleGraphicElec") + this.stylename;
                    str += "Eleki";
                    break;
                case ChipBase.ELEMENT.leaf:
                    this.stylename = ShanghaiEXE.Translate("BattleResult.StyleGraphicLeaf") + this.stylename;
                    str += "Leaf";
                    break;
                case ChipBase.ELEMENT.poison:
                    this.stylename = ShanghaiEXE.Translate("BattleResult.StyleGraphicPois") + this.stylename;
                    str += "Poison";
                    break;
                case ChipBase.ELEMENT.earth:
                    this.stylename = ShanghaiEXE.Translate("BattleResult.StyleGraphicErth") + this.stylename;
                    str += "Earth";
                    break;
            }
            return str;
        }

        private void DecideStyle()
        {
            int num1 = -1;
            int num2 = 0;
            bool[] flagArray = new bool[6];
            for (int index = 0; index < this.savedata.havestyles; ++index)
            {
                if ((uint)this.savedata.style[index].style > 0U)
                    flagArray[this.savedata.style[index].style - 1] = true;
            }
            foreach (var data in ((IEnumerable<int>)this.savedata.stylepoint).Select((v, i) => new
            {
                v,
                i
            }))
            {
                int v = data.v;
                if (flagArray[data.i])
                    v /= 10;
                if (v > num1)
                {
                    num2 = data.i;
                    num1 = v;
                }
            }
            this.style = (NSBattle.Character.Player.STYLE)(num2 + 1);
            this.Random.Next();
            this.Random.Next();
            this.Random.Next();
            this.element = !this.savedata.addonSkill[7] ?
                (!this.savedata.addonSkill[8] ?
                    (!this.savedata.addonSkill[9] ?
                        (!this.savedata.addonSkill[10] ?
                            (!this.savedata.addonSkill[11] ?
                                (!this.savedata.addonSkill[12] ?
                                    (ChipBase.ELEMENT)(this.Random.Next(600) / 100 + 1)
                                    : ChipBase.ELEMENT.earth)
                                : ChipBase.ELEMENT.poison)
                            : ChipBase.ELEMENT.eleki)
                        : ChipBase.ELEMENT.leaf)
                    : ChipBase.ELEMENT.aqua)
                : ChipBase.ELEMENT.heat;
            string message = "";
            switch (this.style)
            {
                case NSBattle.Character.Player.STYLE.normal:
                    message = ShanghaiEXE.Translate("BattleResult.StyleDecideNormal");
                    break;
                case NSBattle.Character.Player.STYLE.fighter:
                    message = ShanghaiEXE.Translate("BattleResult.StyleDecideFght");
                    break;
                case NSBattle.Character.Player.STYLE.shinobi:
                    message = ShanghaiEXE.Translate("BattleResult.StyleDecideNinj");
                    break;
                case NSBattle.Character.Player.STYLE.doctor:
                    message = ShanghaiEXE.Translate("BattleResult.StyleDecideDoc");
                    break;
                case NSBattle.Character.Player.STYLE.gaia:
                    message = ShanghaiEXE.Translate("BattleResult.StyleDecideGaia");
                    break;
                case NSBattle.Character.Player.STYLE.wing:
                    message = ShanghaiEXE.Translate("BattleResult.StyleDecideWing");
                    break;
                case NSBattle.Character.Player.STYLE.witch:
                    message = ShanghaiEXE.Translate("BattleResult.StyleDecideWtch");
                    break;
            }
            if (!this.savedata.FlagList[(int)(421 + this.style)])
            {
                this.parent.main.mapscene.eventmanager.AddEvent(new GetMail(this.sound, this.parent.main.mapscene.eventmanager, (int)(14 + this.style), true, this.parent.main.mapscene, this.savedata));
                this.savedata.FlagList[(int)(421 + this.style)] = true;
            }
            switch (this.element)
            {
                case ChipBase.ELEMENT.normal:
                    message = ShanghaiEXE.Translate("BattleResult.StyleDecideNormal");
                    break;
                case ChipBase.ELEMENT.heat:
                    message = ShanghaiEXE.Translate("BattleResult.StyleDecideHeat") + message;
                    break;
                case ChipBase.ELEMENT.aqua:
                    message = ShanghaiEXE.Translate("BattleResult.StyleDecideAqua") + message;
                    break;
                case ChipBase.ELEMENT.eleki:
                    message = ShanghaiEXE.Translate("BattleResult.StyleDecideElec") + message;
                    break;
                case ChipBase.ELEMENT.leaf:
                    message = ShanghaiEXE.Translate("BattleResult.StyleDecideLeaf") + message;
                    break;
                case ChipBase.ELEMENT.poison:
                    message = ShanghaiEXE.Translate("BattleResult.StyleDecidePois") + message;
                    break;
                case ChipBase.ELEMENT.earth:
                    message = ShanghaiEXE.Translate("BattleResult.StyleDecideErth") + message;
                    break;
            }
            this.savedata.stylepoint = new int[6];
        }

        private void EventtutorialTutorealend()
        {
            this.parent.eventmanager.events.Clear();
            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            var dialogue = ShanghaiEXE.Translate("BattleResult.TutorialBattleResult5Stage1Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialBattleResult5Stage1Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialBattleResult5Stage1Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialBattleResult5Stage1Dialogue4");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialBattleResult5Stage1Dialogue5");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialBattleResult5Stage1Dialogue6");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialBattleResult5Stage1Dialogue7");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialBattleResult5Stage1Dialogue8");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialBattleResult5Stage1Dialogue9");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
        }

        private void EventStylechangeFirst()
        {
            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            var dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange1Stage1Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange1Stage1Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange1Stage1Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange1Stage1Dialogue4");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange1Stage1Dialogue5");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange1Stage1Dialogue6");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange1Stage1Dialogue7");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange1Stage1Dialogue8");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
        }

        private void EventStylechangeSecond()
        {
            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            var dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange2Stage1Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange2Stage1Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange2Stage1Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
        }

        private void EventStylechangeAfter()
        {
            this.parent.eventmanager.ClearEvent();
            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            var dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange3Stage1Dialogue4Format").Format(this.stylename);
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.StyleMassage(this.style);
            if (!this.savedata.firstchange)
                this.ChangeMassage();
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange3Stage1Dialogue5Format").Format(this.stylename);
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.ElementCommentMassage(this.style);
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
        }

        private void ChangeMassage()
        {
            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            var dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange3Stage2Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange3Stage2Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange3Stage2Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange3Stage2Dialogue4");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange3Stage2Dialogue5");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.TutorialStyleChange3Stage2Dialogue6");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
        }

        private string ElementMassage(ChipBase.ELEMENT ele)
        {
            switch (ele)
            {
                case ChipBase.ELEMENT.heat:
                    return ShanghaiEXE.Translate("BattleResult.StyleInfoHeatElem");
                case ChipBase.ELEMENT.aqua:
                    return ShanghaiEXE.Translate("BattleResult.StyleInfoAquaElem");
                case ChipBase.ELEMENT.eleki:
                    return ShanghaiEXE.Translate("BattleResult.StyleInfoElecElem");
                case ChipBase.ELEMENT.leaf:
                    return ShanghaiEXE.Translate("BattleResult.StyleInfoLeafElem");
                case ChipBase.ELEMENT.poison:
                    return ShanghaiEXE.Translate("BattleResult.StyleInfoPoisElem");
                case ChipBase.ELEMENT.earth:
                    return ShanghaiEXE.Translate("BattleResult.StyleInfoErthElem");
                default:
                    return "";
            }
        }

        private void ElementWeakMassage(ChipBase.ELEMENT ele)
        {
            Dialogue dialogue;
            switch (ele)
            {
                case ChipBase.ELEMENT.heat:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoHeatWeakness");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
                case ChipBase.ELEMENT.aqua:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoAquaWeakness");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
                case ChipBase.ELEMENT.eleki:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoElecWeakness");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
                case ChipBase.ELEMENT.leaf:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoLeafWeakness");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
                case ChipBase.ELEMENT.poison:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoPoisWeakness");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
                case ChipBase.ELEMENT.earth:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoErthWeakness");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
            }
        }

        private void ElementCommentMassage(NSBattle.Character.Player.STYLE st)
        {
            Dialogue dialogue;
            switch (st)
            {
                case NSBattle.Character.Player.STYLE.fighter:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoFghtEnd");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
                case NSBattle.Character.Player.STYLE.shinobi:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoNinjEnd");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
                case NSBattle.Character.Player.STYLE.doctor:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoDocEnd");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
                case NSBattle.Character.Player.STYLE.gaia:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoGaiaEnd");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
                case NSBattle.Character.Player.STYLE.wing:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoWingEnd");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
                case NSBattle.Character.Player.STYLE.witch:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoWtchEnd");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
            }
        }

        private void StyleMassage(NSBattle.Character.Player.STYLE st)
        {
            Dialogue dialogue;
            switch (st)
            {
                case NSBattle.Character.Player.STYLE.fighter:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoFghtDialogue1");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoFghtDialogue2");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoFghtDialogue3");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoFghtDialogue4");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
                case NSBattle.Character.Player.STYLE.shinobi:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoNinjDialogue1");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoNinjDialogue2");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoNinjDialogue3");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoNinjDialogue4");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoNinjDialogue5");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoNinjDialogue6");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
                case NSBattle.Character.Player.STYLE.doctor:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoDocDialogue1");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoDocDialogue2");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoDocDialogue3");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoDocDialogue4");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoDocDialogue5");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
                case NSBattle.Character.Player.STYLE.gaia:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoGaiaDialogue1");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoGaiaDialogue2");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoGaiaDialogue3");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoGaiaDialogue4");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoGaiaDialogue5");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoGaiaDialogue6");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoGaiaDialogue7");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
                case NSBattle.Character.Player.STYLE.wing:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoWingDialogue1");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoWingDialogue2");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoWingDialogue3");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoWingDialogue4");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoWingDialogue5");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoWingDialogue6");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
                case NSBattle.Character.Player.STYLE.witch:
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoWtchDialogue1");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoWtchDialogue2");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoWtchDialogue3");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoWtchDialogue4");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoWtchDialogue5");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoWtchDialogue6");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    dialogue = ShanghaiEXE.Translate("BattleResult.StyleInfoWtchDialogue7");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    break;
            }
            this.ElementWeakMassage(this.element);
        }

        public void StyleSelect()
        {
            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            var dialogue = ShanghaiEXE.Translate("BattleResult.StyleSelectDialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new Lavel(this.sound, this.parent.eventmanager, "最初", this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.StyleSelectDialogue2Question");
            var options = ShanghaiEXE.Translate("BattleResult.StyleSelectDialogue2Options");
            this.parent.eventmanager.AddEvent(new Question(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], options[0], options[1], false, true, dialogue.Face, this.savedata, true));
            this.parent.eventmanager.AddEvent(new BranchHead(this.sound, this.parent.eventmanager, 0, this.savedata));
            this.parent.eventmanager.AddEvent(new Goto(this.sound, this.parent.eventmanager, "上書きする", this.savedata));
            this.parent.eventmanager.AddEvent(new BranchHead(this.sound, this.parent.eventmanager, 1, this.savedata));
            this.parent.eventmanager.AddEvent(new Goto(this.sound, this.parent.eventmanager, "上書きしない", this.savedata));
            this.parent.eventmanager.AddEvent(new BranchEnd(this.sound, this.parent.eventmanager, this.savedata));
            this.parent.eventmanager.AddEvent(new Lavel(this.sound, this.parent.eventmanager, "上書きする", this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.StyleSelectDialogue3Question");
            this.parent.eventmanager.AddEvent(new Question(this.sound, this.parent.eventmanager, dialogue[0], this.savedata.style[1].name, this.savedata.style[2].name, this.savedata.style[3].name, this.savedata.style[4].name, false, true, dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new EditValue(this.sound, this.parent.eventmanager, 6, false, 0, 7, "0", null, this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.StyleSelectDialogue4Question");
            options = ShanghaiEXE.Translate("BattleResult.StyleSelectDialogue4Options");
            this.parent.eventmanager.AddEvent(new Question(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], options[0], options[1], false, true, dialogue.Face, this.savedata, true));
            this.parent.eventmanager.AddEvent(new BranchHead(this.sound, this.parent.eventmanager, 0, this.savedata));
            this.parent.eventmanager.AddEvent(new Goto(this.sound, this.parent.eventmanager, "終わり", this.savedata));
            this.parent.eventmanager.AddEvent(new BranchHead(this.sound, this.parent.eventmanager, 1, this.savedata));
            this.parent.eventmanager.AddEvent(new Goto(this.sound, this.parent.eventmanager, "最初", this.savedata));
            this.parent.eventmanager.AddEvent(new BranchEnd(this.sound, this.parent.eventmanager, this.savedata));
            this.parent.eventmanager.AddEvent(new Lavel(this.sound, this.parent.eventmanager, "上書きしない", this.savedata));
            dialogue = ShanghaiEXE.Translate("BattleResult.StyleSelectDialogue5Question");
            options = ShanghaiEXE.Translate("BattleResult.StyleSelectDialogue5Options");
            this.parent.eventmanager.AddEvent(new Question(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], options[0], options[1], false, true, dialogue.Face, this.savedata, true));
            this.parent.eventmanager.AddEvent(new BranchHead(this.sound, this.parent.eventmanager, 0, this.savedata));
            this.parent.eventmanager.AddEvent(new EditValue(this.sound, this.parent.eventmanager, 6, false, 0, 0, "-1", null, this.savedata));
            this.parent.eventmanager.AddEvent(new Goto(this.sound, this.parent.eventmanager, "終わり", this.savedata));
            this.parent.eventmanager.AddEvent(new BranchHead(this.sound, this.parent.eventmanager, 1, this.savedata));
            this.parent.eventmanager.AddEvent(new Goto(this.sound, this.parent.eventmanager, "最初", this.savedata));
            this.parent.eventmanager.AddEvent(new BranchEnd(this.sound, this.parent.eventmanager, this.savedata));
            this.parent.eventmanager.AddEvent(new Lavel(this.sound, this.parent.eventmanager, "終わり", this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
        }

        private enum RESULT
        {
            finish,
            fadein,
            waitInput,
            printchip,
            printchipend,
            printBug,
            printBugend,
            printFreeze,
            printFreezeend,
            printError,
            printErrorend,
            fadeout,
            stylechange,
            end,
        }
    }
}
