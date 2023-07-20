using Common;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSEvent;
using NSGame;
using NSNet;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NSEffect;
using NSAddOn;

namespace NSBattle
{
    public class Custom : AllBase
    {
        public int[] chipsort = new int[30];
        private readonly bool[] chipUseFlag = new bool[30];
        private int chipopen = 6;
        private int canopenchips = 6;
        public int[] sortnumber = new int[12];
        private byte canselects = 5;
        private readonly ChipFolder[] canchips = new ChipFolder[12];
        private readonly bool[] chipSelecFlag = new bool[12];
        private readonly bool[] paflag = new bool[2];
        private bool noChipADD = false;
        private bool chipadd = false;
        private bool closein = false;
        private readonly int[] selectedchips = new int[6];
        private int[,] panum = new int[2, 3];
        private int stylemenu = 8;
        private readonly bool[] styleused = new bool[5];
        private bool[] stylechange = new bool[2];
        private readonly Vector2[] styleposition = new Vector2[2];
        private const int stylemenudefault = 8;
        private readonly SceneBattle parent;
        private readonly SceneMain playerdate;
        private Point cursor;
        private const byte folderchips = 30;
        private const byte canopen = 12;
        private bool close;
        private bool transmission;
        private byte selectchips;
        private bool panamebright;
        private bool paprintname;
        private readonly Player player;
        private bool styleselecting;
        private byte paalpha;
        private bool init;
        private bool styleset;
        private int styleCursol;
        private int style;
        protected string stylepicture;
        public Custom.CUSTOMCHENE scene;
        private int darksound;
        private int darkA;
        protected SaveData savedata;
        public int escapeV;
        private const int escapeplus = 50;
        private bool gaiaChange;
        private bool canescape;
        private bool getData;
        private bool sendData;
        private bool escape;
        private Action revertMammonAction;

        public Custom(IAudioEngine s, SceneBattle p, SceneMain main, Player pl, SaveData save)
          : base(s)
        {
            this.savedata = save;
            this.player = pl;
            for (int index = 0; index < this.styleposition.Length; ++index)
                this.styleposition[index] = new Vector2();
            for (int index = 0; index < this.chipUseFlag.Length; ++index)
                this.chipUseFlag[index] = false;
            for (int index = 0; index < this.styleused.Length; ++index)
                this.styleused[index] = false;
            for (int index = 0; index < this.selectedchips.Length; ++index)
                this.selectedchips[index] = 0;
            for (int index1 = 0; index1 < this.panum.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.panum.GetLength(1); ++index2)
                    this.panum[index1, index2] = 0;
            }
            this.parent = p;
            this.playerdate = main;
            for (int index = 0; index < this.chipsort.Length; ++index)
                this.chipsort[index] = index;
            int[] chipsort = this.chipsort;
            this.chipsort = Debug.ChipStock || this.player.addonSkill[71] ? ((IEnumerable<int>)chipsort).ToArray<int>() : ((IEnumerable<int>)chipsort).OrderBy<int, Guid>(i => Guid.NewGuid()).ToArray<int>();
            if (this.savedata.regularflag[this.savedata.efolder])
                this.SortThips(0, this.savedata.regularchip[this.savedata.efolder]);
            this.scene = Custom.CUSTOMCHENE.fadeIn;
            if (this.parent.player.style == Player.STYLE.doctor)
                this.canselects = 6;
            this.chipopen += this.savedata.custom;
            this.canopenchips += this.savedata.custom;
            if (this.canopenchips <= 5 && this.parent.player.style != Player.STYLE.doctor)
                --this.canopenchips;
            this.paprintname = false;
            this.panamebright = false;
            this.TutorialSort();
            this.escapeV = this.savedata.addonSkill[39] ? 100 : 0;
        }

        public void Init()
        {
            this.stylechange = new bool[2];
            this.stylemenu = 8;
            this.styleCursol = 0;
            this.style = -1;
            this.styleselecting = false;
            this.styleset = false;
            this.escape = false;
            this.canescape = false;
            this.panum = new int[2, 3];
            foreach (var data in ((IEnumerable<bool>)this.paflag).Select((v, i) => new
            {
                v,
                i
            }))
                this.paflag[data.i] = false;
            this.sound.PlaySE(SoundEffect.menuopen);
            this.scene = Custom.CUSTOMCHENE.fadeIn;
            this.frame = 0;
            this.selectchips = 0;
            foreach (var data in ((IEnumerable<int>)this.selectedchips).Select((v, i) => new
            {
                v,
                i
            }))
                this.selectedchips[data.i] = -1;
            foreach (var data in ((IEnumerable<bool>)this.chipSelecFlag).Select((v, i) => new
            {
                v,
                i
            }))
            {
                this.chipSelecFlag[data.i] = false;
                this.canchips[data.i] = new ChipFolder(this.sound);
            }
            if (this.noChipADD)
            {
                this.chipopen = 12;
                this.noChipADD = false;
            }
            else
                this.chipopen = this.canopenchips;
            if (this.savedata.addonSkill[6] && this.parent.turn == 0)
                this.chipopen = 12;
            int index = 0;
            foreach (var data in ((IEnumerable<int>)this.chipsort).Select((v, i) => new
            {
                v,
                i
            }))
            {
                if (!this.chipUseFlag[data.i] && index < this.chipopen)
                {
                    this.canchips[index].chip = this.playerdate.chipfolder[this.savedata.FlagList[3] ? 2 : this.savedata.efolder, this.chipsort[data.i]].chip;
                    this.canchips[index].codeNo = this.playerdate.chipfolder[this.savedata.FlagList[3] ? 2 : this.savedata.efolder, this.chipsort[data.i]].codeNo;
                    this.sortnumber[index] = data.i;
                    ++index;
                    if (index % 6 == 5 && this.parent.player.style != Player.STYLE.doctor)
                        ++index;
                }
            }
            this.cursor = this.canchips[0].chip is DammyChip ? new Point(6, 0) : new Point(0, 0);
            this.init = true;
        }

        private void FadeIn()
        {
            ++this.frame;
            this.parent.positionHPwindow.X = 24 + this.frame * 9;
            if (this.frame < 15)
                return;
            this.scene = Custom.CUSTOMCHENE.custom;
            this.parent.positionHPwindow.X = 152f;
            this.frame = 0;
            if (!this.closein)
                this.EventStartcustom();
            else
                this.closein = false;
        }

        private void FadeOut()
        {
            ++this.frame;
            this.parent.positionHPwindow.X = 152 - this.frame * 9;
            if (this.frame < 15)
                return;
            if (this.close)
            {
                this.scene = Custom.CUSTOMCHENE.close;
            }
            else
            {
                this.parent.CustomReset();
                bool paFormed = false;
                if (this.transmission)
                {
                    var chips = this.selectedchips.Take(this.selectchips).Select(i => new ChipS(this.canchips[i].chip.number, this.canchips[i].codeNo)).ToList();

                    var paCount = 0;
                    for (var startIndex = 0; startIndex < this.selectchips; startIndex++)
                    {
                        var chipSubset = chips.Skip(startIndex).ToList();
                        var pa = ChipFolder.ProgramAdvanceCheck(chipSubset);

                        if (pa != null)
                        {
                            paFormed = true;
                            this.panum[paCount, 0] = startIndex;
                            this.panum[paCount, 1] = pa.ProgramAdvance;
                            this.panum[paCount, 2] = pa.Chips.Count;
                            this.paflag[paCount] = true;
                            paCount++;
                            startIndex += pa.Chips.Count - 1;
                        }
                    }
                    for (; paCount < this.panum.GetLength(0); paCount++)
                    {
                        this.panum[paCount, 0] = -1;
                        this.panum[paCount, 1] = -1;
                        this.panum[paCount, 2] = -1;
                    }
                }
                if (paFormed)
                {
                    this.scene = Custom.CUSTOMCHENE.paprint;
                }
                else
                {
                    if (this.escape)
                    {
                        if (this.parent.namePrint)
                            this.parent.namePrint = false;
                        if (this.canescape)
                        {
                            this.parent.eventmanager.events.Clear();
                            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
                            var dialogue = ShanghaiEXE.Translate("Custom.EscapeSuccess");
                            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, dialogue.Face.Mono, dialogue.Face.Auto, this.savedata));
                            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
                            this.parent.BattleEnd(true);
                            this.escape = false;
                        }
                        else
                        {
                            this.parent.eventmanager.events.Clear();
                            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
                            var dialogue = ShanghaiEXE.Translate("Custom.EscapeFailure");
                            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, dialogue.Face.Mono, dialogue.Face.Auto, this.savedata));
                            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
                            this.escapeV += 50;

                            this.selectchips = 0;
                        }
                        this.escape = false;
                    }
                    this.player.needAbuutonPush = true;
                    this.scene = Custom.CUSTOMCHENE.chipMake;
                }
            }
            this.parent.positionHPwindow.X = 24f;
            this.frame = 0;
        }

        private void PAprint()
        {
            ++this.frame;
            if (this.frame < 20)
                this.paalpha += 6;
            if (this.frame > 180)
                this.paalpha -= 6;
            switch (this.frame)
            {
                case 20:
                    this.player.PluspointDoctor(40);
                    this.sound.PlaySE(SoundEffect.barrier);
                    this.paprintname = true;
                    break;
                case 80:
                    this.paprintname = false;
                    break;
                case 100:
                    this.sound.PlaySE(SoundEffect.docking);
                    for (int index = 0; index < this.panum.GetLength(0); ++index)
                    {
                        if (this.panum[index, 1] != -1)
                        {
                            this.savedata.datelist[this.panum[index, 1] - 1] = true;
                            this.canchips[this.selectedchips[this.panum[index, 0]]].SettingChip(this.panum[index, 1]);
                            for (var i = 1; i < this.panum[index, 2]; i++)
                            {
                                this.canchips[this.selectedchips[this.panum[index, 0] + i]].SettingChip(0);
                            }
                        }
                    }
                    this.paprintname = true;
                    break;
                case 200:
                    this.frame = 0;
                    this.paalpha = 0;
                    this.scene = Custom.CUSTOMCHENE.chipMake;
                    break;
            }
            if (this.frame % 10 != 0)
                return;
            this.panamebright = !this.panamebright;
        }

        public void Stylechange()
        {
            ++this.frame;
            if (this.frame < 30)
                this.parent.backscreen += 8;
            else if (this.frame == 30)
            {
                if (this.parent.backscreen != byte.MaxValue)
                    this.parent.backscreen = byte.MaxValue;
                this.sound.PlaySE(SoundEffect.eriasteal1);
            }
            else if (this.frame > 30 && this.frame < 75)
            {
                for (int index = 0; index < this.styleposition.Length; ++index)
                    this.styleposition[index] = new Vector2();
                int num1 = 135 - (this.frame - 30) * 3;
                int num2 = (this.frame - 30) * 8;
                if (num2 >= 360)
                    num2 -= 360;
                if (num2 < 0)
                    num2 += 360;
                this.styleposition[0].X += num1 * (float)Math.Cos(num2);
                this.styleposition[0].Y += num1 * (float)Math.Sin(num2);
                int num3 = num2 + 180;
                if (num3 >= 360)
                    num3 -= 360;
                if (num3 < 0)
                    num3 += 360;
                this.styleposition[1].X += num1 * (float)Math.Cos(num3);
                this.styleposition[1].Y += num1 * (float)Math.Sin(num3);
            }
            else if (this.frame == 75)
            {
                if (this.parent.backscreencolor != byte.MaxValue)
                    this.parent.backscreencolor = byte.MaxValue;
                this.sound.PlaySE(SoundEffect.fullcustom);
                this.gaiaChange = false;
                if (this.stylechange[0])
                    this.StyleSet(this.player);
            }
            else if (this.frame >= 75 && this.frame < 105)
            {
                if (this.frame == 80)
                    this.parent.backscreencolor = 0;
                this.parent.backscreen -= 8;
            }
            else if (this.frame >= 105)
            {
                if (this.stylechange[0])
                {
                    if (this.style != this.savedata.setstyle && !this.player.addonSkill[29])
                        this.styleused[this.style] = true;
                    this.player.setstyle = this.style;
                }
                this.parent.backscreen = 0;
                this.stylechange = new bool[2];
                this.frame = 0;
                this.scene = Custom.CUSTOMCHENE.start;
            }
        }

        private void StyleSet(Player player)
        {
            player.printplayer = true;
            player.Setstyle(this.style);
            player.moveWaitTime = player.style != Player.STYLE.shinobi ? 10 : 6;
            if (player.style == Player.STYLE.gaia)
            {
                this.parent.GaiaPanelChange(player.Element, this.gaiaChange);
                this.ShakeStart(2, 30);
                this.gaiaChange = true;
            }
            this.canselects = player.style != Player.STYLE.doctor ? (byte)5 : (byte)6;
            if ((uint)player.step > 0U)
            {
                player.position = player.stepPosition;
                player.PositionDirectSet();
                if (player.step == CharacterBase.STEP.shadow)
                    player.nohit = false;
                player.flying = player.flyflag;
                player.step = CharacterBase.STEP.none;
            }
            player.ChargeTimeSet();
        }

        public void Update()
        {
            if (!this.init)
                this.Init();
            switch (this.scene)
            {
                case Custom.CUSTOMCHENE.fadeIn:
                    this.FadeIn();
                    break;
                case Custom.CUSTOMCHENE.custom:
                case Custom.CUSTOMCHENE.selectstyle:
                    this.Control();
                    if (this.cursor.Y * 6 + this.cursor.X < 12)
                    {
                        if (this.scene == Custom.CUSTOMCHENE.custom && this.canchips[this.cursor.Y * 6 + this.cursor.X].chip.dark && this.cursor.X != 6)
                        {
                            if (this.darksound <= 0)
                            {
                                this.sound.PlaySE(SoundEffect.dark);
                                this.darksound = 60;
                            }
                            if (this.darkA < 80)
                                this.darkA += 10;
                        }
                        else if (this.darkA > 0)
                            this.darkA -= 10;
                    }
                    if (this.darksound > 0)
                        --this.darksound;
                    ++this.frame;
                    if (this.frame < 29)
                        break;
                    this.frame = 0;
                    break;
                case Custom.CUSTOMCHENE.openstyle:
                    this.stylemenu += 8;
                    if (this.stylemenu < 96)
                        break;
                    this.scene = Custom.CUSTOMCHENE.selectstyle;
                    this.styleselecting = true;
                    break;
                case Custom.CUSTOMCHENE.close:
                    for (int index = 0; index < 10; ++index)
                    {
                        if (Input.IsPress((Button)index))
                        {
                            this.close = false;
                            this.scene = Custom.CUSTOMCHENE.fadeIn;
                            this.sound.PlaySE(SoundEffect.menuopen);
                            this.closein = true;
                            this.frame = 0;
                        }
                    }
                    break;
                case Custom.CUSTOMCHENE.fadeOut:
                    this.FadeOut();
                    break;
                case Custom.CUSTOMCHENE.paprint:
                    this.PAprint();
                    break;
                case Custom.CUSTOMCHENE.stylechange:
                    this.Stylechange();
                    break;
                case Custom.CUSTOMCHENE.chipMake:
                    ++this.frame;
                    if (this.frame == 1)
                    {
                        if (this.selectchips > 0)
                        {
                            this.parent.ResetPlayerChips();
                            if (this.transmission)
                            {
                                int usableChips = 0;
                                bool darkChip = false;
                                for (int index = 0; index < this.selectedchips.Length; ++index)
                                {
                                    int selectedchip = this.selectedchips[index];
                                    if (selectedchip >= 0)
                                    {
                                        if (!(this.canchips[selectedchip].chip is DammyChip))
                                        {
                                            if (this.canchips[selectedchip].chip.dark)
                                                darkChip = true;
                                            bool modifierChip = false;
                                            bool stungame = false;
                                            bool timestop = false;
                                            int num2 = 0;
                                            switch (this.canchips[selectedchip].chip.number)
                                            {
                                                case 157:
                                                    if (index > 0)
                                                    {
                                                        this.player.PluspointDoctor(20);
                                                        timestop = true;
                                                        modifierChip = true;
                                                        break;
                                                    }
                                                    break;
                                                case 188:
                                                    if (index > 0 && this.player.haveChip[usableChips - 1].powerprint)
                                                    {
                                                        this.player.PluspointDoctor(20);
                                                        num2 = 10;
                                                        modifierChip = true;
                                                        break;
                                                    }
                                                    break;
                                                case 189:
                                                    if (index > 0 && this.player.haveChip[usableChips - 1].powerprint)
                                                    {
                                                        this.player.PluspointDoctor(20);
                                                        num2 = 30;
                                                        modifierChip = true;
                                                        break;
                                                    }
                                                    break;
                                                case 190:
                                                    if (index > 0 && this.player.haveChip[usableChips - 1].powerprint)
                                                    {
                                                        this.player.PluspointDoctor(20);
                                                        stungame = true;
                                                        modifierChip = true;
                                                        break;
                                                    }
                                                    break;
                                            }
                                            if (!modifierChip)
                                            {
                                                this.player.haveChip.Add(this.canchips[selectedchip].ReturnChip(this.canchips[selectedchip].chip.number));
                                                ++usableChips;
                                            }
                                            else
                                            {
                                                this.player.haveChip[usableChips - 1].pluspower += num2;
                                                if (stungame)
                                                    this.player.haveChip[usableChips - 1].paralyze = true;
                                                if (timestop)
                                                    this.player.haveChip[usableChips - 1].timeStopper = true;
                                            }
                                        }
                                        this.chipUseFlag[this.sortnumber[selectedchip]] = true;
                                    }
                                }
                                if (darkChip)
                                    this.player.mind.mindNow = MindWindow.MIND.dark;
                                else if (this.player.mind.mindNow == MindWindow.MIND.dark)
                                    this.player.mind.mindNow = MindWindow.MIND.normal;
                                this.parent.player.numOfChips = usableChips;
                            }
                        }

                        this.EventStartbattle();
                    }
                    if (this.frame <= 2)
                        break;
                    if (this.chipadd)
                    {
                        if (this.player.style != Player.STYLE.doctor && this.canopenchips == 5)
                            this.canopenchips = 6;
                        if (this.canopenchips <= 4 && this.canopenchips + this.selectchips >= 5 && this.player.style != Player.STYLE.doctor)
                            ++this.canopenchips;
                        this.canopenchips += this.selectchips;
                        if (this.canopenchips > 12)
                            this.canopenchips = 12;
                        this.chipadd = false;
                    }
                    if (this.styleset)
                        this.stylechange[0] = true;
                    if (this.stylechange[0] || this.stylechange[1])
                    {
                        this.parent.backscreencolor = 0;
                        this.frame = 0;
                        this.scene = Custom.CUSTOMCHENE.stylechange;
                    }
                    else
                    {
                        this.scene = Custom.CUSTOMCHENE.start;
                        this.frame = 0;
                    }
                    break;
                case Custom.CUSTOMCHENE.start:
                    ++this.frame;
                    if (this.frame <= 75)
                        break;

                    Player.STYLE newTurnStyle;
                    ChipBase.ELEMENT element;
                    if (this.styleset)
                    {
                        newTurnStyle = (Player.STYLE)this.savedata.style[this.style].style;
                        element = (ChipBase.ELEMENT)this.savedata.style[this.style].element;
                    }
                    else
                    {
                        newTurnStyle = this.player.style;
                        element = this.player.Element;
                    }

                    var evade = this.parent.player.haveChip.Where(c => c is Flyng).ToList();
                    this.parent.player.haveChip.RemoveAll(c => evade.Contains(c));
                    this.parent.player.numOfChips -= evade.Count;

                    if (newTurnStyle == Player.STYLE.wing)
                    {
                        this.parent.player.haveChip.Add(new Flyng(this.sound));
                        ++this.parent.player.numOfChips;
                        player.haveChip.RemoveAll(a => a == null);
                    }

                    ++this.parent.turn;
                    if (this.player.addonSkill[68])
                        ++this.canopenchips;

                    if (this.savedata.addonSkill[74])
                    {
                        this.revertMammonAction?.Invoke();
                        this.revertMammonAction = this.transmission
                            ? Mammon.ApplyMammonPunishments(this.sound, this.parent, ref this.canopenchips, this.selectchips)
                            : null;
                    }

                    if (this.canopenchips > 12)
                        this.canopenchips = 12;
                    this.parent.nowscene = SceneBattle.BATTLESCENE.battle;

                    break;
                case Custom.CUSTOMCHENE.escape:
                    if (this.savedata.selectQuestion == 0 && this.parent.canEscape)
                    {
                        this.canescape = true;
                        if (!this.parent.canEscape)
                            this.canescape = false;
                        else
                        {
                            int sumEnemyHp = 0;
                            foreach (EnemyBase enemy in this.parent.enemys)
                            {
                                if (enemy.union == Panel.COLOR.blue && !(enemy is DammyEnemy))
                                    sumEnemyHp += enemy.Hp;
                            }
                            int hpAdvantageFactor = this.player.HpMax - sumEnemyHp + 100;
                            if (hpAdvantageFactor < 0)
                                hpAdvantageFactor = 0;
                            if (this.Random.Next(100) > hpAdvantageFactor + this.escapeV)
                                this.canescape = false;
                        }
                        this.frame = 0;
                        if (!this.parent.init)
                            this.parent.init = true;
                        this.scene = Custom.CUSTOMCHENE.fadeOut;
                        break;
                    }
                    this.escape = false;
                    this.scene = Custom.CUSTOMCHENE.custom;
                    break;
            }
        }

        public void Control()
        {
            if (Input.IsPress(Button._L) && !this.styleselecting)
            {
                this.escape = true;
                this.scene = Custom.CUSTOMCHENE.escape;
                this.parent.eventmanager.events.Clear();
                this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
                if (this.parent.canEscape)
                {
                    Dialogue dialogue;
                    Dialogue options;
                    if (!this.savedata.FlagList[0])
                    {
                        dialogue = ShanghaiEXE.Translate("Custom.EscapeQuestion");
                        options = ShanghaiEXE.Translate("Custom.EscapeOptions");
                    }
                    else
                    {
                        dialogue = ShanghaiEXE.Translate("Custom.EscapeSpecialQuestion");
                        options = ShanghaiEXE.Translate("Custom.EscapeSpecialOptions");
                    }

                    this.parent.eventmanager.AddEvent(new Question(
                        this.sound,
                        this.parent.eventmanager,
                        dialogue[0],
                        dialogue[1],
                        options[0],
                        options[1],
                        false, true, dialogue.Face, this.savedata, true));
                }
                else
                {
                    Dialogue dialogue;
                    if (!this.savedata.FlagList[0])
                    {
                        dialogue = ShanghaiEXE.Translate("Custom.EscapeForbidden");
                    }
                    else
                    {
                        dialogue = ShanghaiEXE.Translate("Custom.EscapeSpecialForbidden");
                    }
                    this.parent.eventmanager.AddEvent(new CommandMessage(
                        this.sound,
                        this.parent.eventmanager,
                        dialogue[0],
                        dialogue[1],
                        dialogue[2],
                        dialogue.Face, this.savedata));
                }
                this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
            }
            else if (Input.IsPress(Button.Up))
            {
                if (this.scene == Custom.CUSTOMCHENE.custom)
                {
                    if (this.cursor.X == 6 && this.cursor.Y == 1)
                    {
                        --this.cursor.Y;
                        this.sound.PlaySE(SoundEffect.movecursol);
                    }
                    else if (this.cursor.Y == 1 && !(this.canchips[this.cursor.X].chip is DammyChip))
                    {
                        --this.cursor.Y;
                        this.sound.PlaySE(SoundEffect.movecursol);
                    }
                    else
                    {
                        if (this.savedata.havestyles <= 1 || this.player.mind.MindNow == MindWindow.MIND.pinch)
                            return;
                        if (this.cursor.X > 6)
                        {
                            this.sound.PlaySE(SoundEffect.error);
                        }
                        else
                        {
                            this.scene = Custom.CUSTOMCHENE.openstyle;
                            this.styleCursol = 0;
                            this.sound.PlaySE(SoundEffect.menuopen);
                        }
                    }
                }
                else if (this.styleCursol > 0)
                {
                    --this.styleCursol;
                    this.sound.PlaySE(SoundEffect.movecursol);
                }
            }
            else if (Input.IsPress(Button.Down))
            {
                if (this.scene == Custom.CUSTOMCHENE.custom)
                {
                    if (this.cursor.X == 6 && this.cursor.Y == 0)
                        ++this.cursor.Y;
                    else if (this.cursor.Y == 0 && !(this.canchips[this.cursor.X + 6].chip is DammyChip))
                        ++this.cursor.Y;
                    this.sound.PlaySE(SoundEffect.movecursol);
                }
                else if (this.styleCursol + 1 < this.savedata.havestyles)
                {
                    ++this.styleCursol;
                    this.sound.PlaySE(SoundEffect.movecursol);
                }
                else
                    this.CloseStyleMenu();
            }
            else if (Input.IsPress(Button.Right))
            {
                if (this.scene != Custom.CUSTOMCHENE.custom)
                    return;
                if (!(this.canchips[0].chip is DammyChip))
                {
                    ++this.cursor.X;
                    if (this.cursor.X <= 6)
                    {
                        if (this.canchips[11].chip is DammyChip && (this.canchips[this.cursor.X + this.cursor.Y * 6].chip is DammyChip || this.canchips[0].chip is DammyChip))
                        {
                            this.cursor.X = 6;
                            this.cursor.Y = 0;
                        }
                    }
                    else if (this.canchips[6].chip is DammyChip)
                    {
                        this.cursor.Y = 0;
                        this.cursor.X = 0;
                    }
                    else
                        this.cursor.X = 0;
                    if (this.cursor.X == 5 && this.parent.player.style != Player.STYLE.doctor)
                    {
                        this.cursor.X = 6;
                        this.cursor.Y = 0;
                    }
                    else if (this.cursor.X == 5 && this.parent.player.style != Player.STYLE.doctor)
                        this.cursor.Y = 0;
                }
                this.sound.PlaySE(SoundEffect.movecursol);
            }
            else if (Input.IsPress(Button.Left))
            {
                if (this.scene != Custom.CUSTOMCHENE.custom)
                    return;
                if (!(this.canchips[0].chip is DammyChip))
                {
                    if (this.cursor.Y == 1 && this.cursor.X == 6 && this.canchips[6].chip is DammyChip)
                    {
                        this.close = true;
                        this.scene = Custom.CUSTOMCHENE.fadeOut;
                        this.frame = 0;
                    }
                    else
                    {
                        if (this.cursor.X > 0)
                        {
                            if (this.canchips[this.cursor.Y * 6 + this.cursor.X - 1].chip is DammyChip)
                            {
                                for (int index = 0; index < 6; ++index)
                                {
                                    if (!(this.canchips[this.cursor.Y * 6 + index].chip is DammyChip))
                                        this.cursor.X = index;
                                }
                            }
                            else
                                --this.cursor.X;
                        }
                        else
                        {
                            this.cursor.X = 6;
                            this.cursor.Y = 0;
                        }
                        if (this.cursor.X == 5 && this.parent.player.style != Player.STYLE.doctor)
                            --this.cursor.X;
                    }
                }
                this.sound.PlaySE(SoundEffect.movecursol);
            }
            else if (Input.IsPress(Button._A))
            {
                if (this.scene != Custom.CUSTOMCHENE.selectstyle)
                {
                    if (this.cursor.X == 6)
                    {
                        if (!this.parent.init)
                            this.parent.init = true;
                        // Send chips
                        // New chips added at Custom.CUSTOMCHENE.chipMake
                        if (this.cursor.Y == 0)
                        {
                            if (this.parent.namePrint)
                                this.parent.namePrint = false;
                            if (this.savedata.FlagList[3] && this.parent.battlenum == 2 && this.parent.turn == 0)
                            {
                                this.EventCustomend();
                            }
                            else
                            {
                                // Clear existing chips
                                if (this.selectchips > 0)
                                {
                                    for (int index = 0; index < 6; ++index)
                                        this.parent.player.haveChip.Clear();
                                }
                                this.transmission = true;
                                this.frame = 0;
                                this.EventCustomend();
                                this.scene = Custom.CUSTOMCHENE.fadeOut;
                            }
                        }
                        // ADD
                        else
                        {
                            if (this.parent.namePrint)
                                this.parent.namePrint = false;
                            this.player.PluspointDoctor(30);
                            if (this.savedata.FlagList[3] && (this.parent.battlenum < 2 || this.parent.battlenum == 3) || this.savedata.FlagList[0])
                            {
                                this.EventAdd(false);
                            }
                            else
                            {
                                this.EventAdd(true);
                                if (this.selectchips > 0)
                                {
                                    for (int index = 0; index < selectchips; ++index)
                                        this.chipUseFlag[this.sortnumber[this.selectedchips[index]]] = true;
                                    this.chipadd = true;
                                }
                                else
                                    this.noChipADD = true;
                                this.parent.player.haveChip.Clear();
                                this.parent.player.numOfChips = 0;
                                this.transmission = false;
                                this.frame = 0;
                                this.scene = Custom.CUSTOMCHENE.fadeOut;
                            }
                        }
                        if (this.styleset)
                        {
                            this.stylechange[0] = true;
                            this.stylepicture = this.player.StyleGraphicsName(this.style);
                        }
                        this.sound.PlaySE(SoundEffect.thiptransmission);
                    }
                    else if (selectchips < canselects)
                    {
                        if (!this.chipSelecFlag[this.cursor.Y * 6 + this.cursor.X])
                        {
                            if (this.selectchips == 0 || this.CanChipSelect(this.cursor.Y * 6 + this.cursor.X))
                            {
                                this.chipSelecFlag[this.cursor.Y * 6 + this.cursor.X] = true;
                                this.selectedchips[selectchips] = (byte)(this.cursor.Y * 6 + this.cursor.X);
                                ++this.selectchips;
                                this.sound.PlaySE(SoundEffect.decide);
                            }
                            else
                                this.sound.PlaySE(SoundEffect.error);
                        }
                        else
                            this.sound.PlaySE(SoundEffect.error);
                    }
                    else
                        this.sound.PlaySE(SoundEffect.error);
                }
                else if (!this.styleset)
                {
                    if (this.styleused[this.styleCursol] || this.styleCursol == this.player.setstyle)
                    {
                        this.sound.PlaySE(SoundEffect.error);
                    }
                    else
                    {
                        this.sound.PlaySE(SoundEffect.bright);
                        this.styleset = true;
                        this.scene = Custom.CUSTOMCHENE.custom;
                        this.style = this.styleCursol;
                        this.styleselecting = false;
                        this.stylemenu = 0;
                    }
                }
            }
            else if (Input.IsPress(Button._B))
            {
                if (this.scene == Custom.CUSTOMCHENE.custom)
                {
                    if (this.selectchips > 0)
                    {
                        this.chipSelecFlag[this.selectedchips[this.selectchips - 1]] = false;
                        this.selectedchips[selectchips - 1] = -1;
                        --this.selectchips;
                        this.sound.PlaySE(SoundEffect.cancel);
                    }
                    else if (this.styleset)
                    {
                        this.styleset = false;
                        this.style = -1;
                        this.CloseStyleMenu();
                    }
                    else
                        this.sound.PlaySE(SoundEffect.error);
                }
                else if (this.styleset)
                {
                    this.styleset = false;
                    this.style = -1;
                    this.sound.PlaySE(SoundEffect.cancel);
                }
                else
                    this.CloseStyleMenu();
            }
            else if (Input.IsPress(Button._Start) && !this.styleselecting)
            {
                if (this.cursor.X != 6 || (uint)this.cursor.Y > 0U)
                    this.cursor = new Point(6, 0);
                this.sound.PlaySE(SoundEffect.movecursol);
            }
            else if (Input.IsPress(Button._Select) && !this.styleselecting)
            {
                if (this.parent.namePrint)
                    this.parent.namePrint = false;
                this.close = true;
                this.scene = Custom.CUSTOMCHENE.fadeOut;
                this.frame = 0;
                this.sound.PlaySE(SoundEffect.menuclose);
            }
            else
            {
                if (!Input.IsPress(Button._R))
                    return;
                this.sound.PlaySE(SoundEffect.movecursol);
                if (!this.styleselecting)
                {
                    if (this.cursor.X < 6)
                    {
                        this.parent.eventmanager.events.Clear();
                        this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, this.canchips[this.cursor.Y * 6 + this.cursor.X].chip.information[0], this.canchips[this.cursor.Y * 6 + this.cursor.X].chip.information[1], this.canchips[this.cursor.Y * 6 + this.cursor.X].chip.information[2], true, this.savedata));
                    }
                    else
                    {
                        this.parent.eventmanager.events.Clear();
                        Dialogue dialogue = new Dialogue();
                        switch (this.cursor.Y)
                        {
                            case 0:
                                dialogue = this.selectchips > 0 ? ShanghaiEXE.Translate("Custom.SendChips") : ShanghaiEXE.Translate("Custom.SendNoChips");
                                break;
                            case 1:
                                dialogue = this.selectchips > 0 ? ShanghaiEXE.Translate("Custom.ADDChips") : ShanghaiEXE.Translate("Custom.ADDTempChips");
                                break;
                        }
                        this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], true, this.savedata));
                    }
                }
                else
                {
                    this.parent.eventmanager.events.Clear();
                    Dialogue dialogue = new Dialogue();
                    switch (this.savedata.style[this.styleCursol].style)
                    {
                        case 0:
                            dialogue = ShanghaiEXE.Translate("Custom.NullElemDesc");
                            break;
                        case 1:
                            dialogue = ShanghaiEXE.Translate("Custom.FghtStyleDesc");
                            break;
                        case 2:
                            dialogue = ShanghaiEXE.Translate("Custom.NinjStyleDesc");
                            break;
                        case 3:
                            dialogue = ShanghaiEXE.Translate("Custom.DocStyleDesc");
                            break;
                        case 4:
                            dialogue = ShanghaiEXE.Translate("Custom.GaiaStyleDesc");
                            break;
                        case 5:
                            dialogue = ShanghaiEXE.Translate("Custom.WingStyleDesc");
                            break;
                        case 6:
                            dialogue = ShanghaiEXE.Translate("Custom.WtchStyleDesc");
                            break;
                    }
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], true, this.savedata));
                }
            }
        }

        private bool CanChipSelect(int chip)
        {
            if (chip >= 12)
                return false;
            int index1 = chip;
            int num = 9999;
            ChipFolder.CODE code = ChipFolder.CODE.asterisk;
            bool flag;
            if (this.selectchips == 0)
            {
                flag = true;
            }
            else
            {
                for (int index2 = 0; index2 < selectchips; ++index2)
                {
                    if (!(this.canchips[this.selectedchips[index2]].chip is DammyChip))
                    {
                        if (code == ChipFolder.CODE.asterisk)
                            code = this.canchips[this.selectedchips[index2]].chip.code[this.canchips[this.selectedchips[index2]].codeNo];
                        else if (code != this.canchips[this.selectedchips[index2]].chip.code[this.canchips[this.selectedchips[index2]].codeNo] && this.canchips[this.selectedchips[index2]].chip.code[this.canchips[this.selectedchips[index2]].codeNo] != ChipFolder.CODE.asterisk)
                            code = ChipFolder.CODE.none;
                        if (num == 9999)
                            num = this.canchips[this.selectedchips[index2]].chip.number;
                        else if (num != this.canchips[this.selectedchips[index2]].chip.number)
                            num = 0;
                    }
                }
                flag = this.canchips[index1].chip.number == num || code != ChipFolder.CODE.none && (this.canchips[index1].chip.code[this.canchips[index1].codeNo] == code || code == ChipFolder.CODE.asterisk || this.canchips[index1].chip.code[this.canchips[index1].codeNo] == ChipFolder.CODE.asterisk);
                for (int index2 = 0; index2 < selectchips; ++index2)
                {
                    int selectedchip = this.selectedchips[index2];
                }
            }
            return flag;
        }

        private static string[,] StyleSprites = {
            { "Custom.StyleNormUnselected", "Custom.StyleNormHovered", "Custom.StyleNormDisabled", "Custom.StyleNormSelected" },
            { "Custom.StyleFghtUnselected", "Custom.StyleFghtHovered", "Custom.StyleFghtDisabled", "Custom.StyleFghtSelected" },
            { "Custom.StyleNinjUnselected", "Custom.StyleNinjHovered", "Custom.StyleNinjDisabled", "Custom.StyleNinjSelected" },
            { "Custom.StyleDocUnselected", "Custom.StyleDocHovered", "Custom.StyleDocDisabled", "Custom.StyleDocSelected" },
            { "Custom.StyleGaiaUnselected", "Custom.StyleGaiaHovered", "Custom.StyleGaiaDisabled", "Custom.StyleGaiaSelected" },
            { "Custom.StyleWingUnselected", "Custom.StyleWingHovered", "Custom.StyleWingDisabled", "Custom.StyleWingSelected" },
            { "Custom.StyleWtchUnselected", "Custom.StyleWtchHovered", "Custom.StyleWtchDisabled", "Custom.StyleWtchSelected" }
        };
        private Tuple<string, Rectangle> GetStyleSprite(int status, int styleIndex)
        {
            return ShanghaiEXE.languageTranslationService.GetLocalizedSprite(StyleSprites[styleIndex, status]);
        }

        public void Render(IRenderer dg)
        {
            Vector2 _point = new Vector2(0.0f, 0.0f);
            switch (this.scene)
            {
                case Custom.CUSTOMCHENE.paprint:
                    Color color1 = Color.FromArgb(paalpha, 0, 0, 0);
                    this._rect = new Rectangle(0, 0, 240, 160);
                    this._position = new Vector2(0.0f, 0.0f);
                    dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color1);
                    if (this.frame <= 20 || this.frame >= 180)
                        break;
                    int num1 = this.player.style == Player.STYLE.doctor ? 2 : 16;
                    this._position = new Vector2(16f, num1);
                    var programAdvanceSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("Custom.ProgramAdvance");
                    this._rect = programAdvanceSprite.Item2;
                    dg.DrawImage(dg, programAdvanceSprite.Item1, this._rect, true, this._position, Color.White);
                    foreach (var data in ((IEnumerable<int>)this.selectedchips).Select((v, i) => new
                    {
                        v,
                        i
                    }))
                    {
                        if (data.v >= 0 && !(this.canchips[data.v].chip is DammyChip))
                        {
                            AllBase.NAME[] nameArray = this.canchips[data.v].chip.Nametodata(this.canchips[data.v].chip.name);
                            Color color2 = Color.White;
                            bool flag = false;
                            if (this.paflag[0] && data.i >= this.panum[0, 0] && data.i <= this.panum[0, 0] + 2)
                            {
                                color2 = Color.GreenYellow;
                                flag = true;
                            }
                            else if (this.paflag[1] && data.i >= this.panum[1, 0] && data.i <= this.panum[1, 0] + 2)
                            {
                                color2 = Color.SkyBlue;
                                flag = true;
                            }
                            if (!this.panamebright)
                                color2 = Color.White;
                            if (!flag || this.paprintname)
                            {
                                this._position = new Vector2(16, num1 + 16 + 24 * data.i);
                                DrawBlockCharacters(dg, nameArray, 16, this._position, color2, out this._rect, out this._position);
                                if (this.canchips[data.v].chip.code[this.canchips[data.v].codeNo] != ChipFolder.CODE.none)
                                {
                                    this._rect = new Rectangle((int)this.canchips[data.v].chip.code[this.canchips[data.v].codeNo] * 8, 32, 8, 16);
                                    this._position = new Vector2(88f, num1 + 16 + 24 * data.i);
                                    dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                                }
                            }
                        }
                    }
                    break;
                case Custom.CUSTOMCHENE.stylechange:
                    if (this.frame > 30 && this.frame <= 75)
                    {
                        if (this.stylechange[0])
                        {
                            this._rect = new Rectangle(0, 0, this.player.Wide, this.player.Height);
                            Color color2 = Color.FromArgb(128, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                            this._position = this.player.positionDirect;
                            this._position.X += this.styleposition[0].X;
                            this._position.Y += this.styleposition[0].Y;
                            this._position.X += Shake.X;
                            this._position.Y += Shake.Y;
                            dg.DrawImage(dg, this.stylepicture, this._rect, false, this._position, color2);
                            Color color3 = Color.FromArgb(128, 128, 128, 128);
                            this._position = this.player.positionDirect;
                            this._position.X += this.styleposition[1].X;
                            this._position.Y += this.styleposition[1].Y;
                            this._position.X += Shake.X;
                            this._position.Y += Shake.Y;
                            dg.DrawImage(dg, this.stylepicture, this._rect, false, this._position, color3);
                        }
                        break;
                    }
                    if (this.frame <= 75)
                        break;
                    Point shake;
                    if (this.stylechange[0])
                    {
                        Color color2 = Color.FromArgb(this.parent.backscreen, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                        this._position = this.player.positionDirect;
                        ref float local1 = ref this._position.X;
                        double num2 = local1;
                        shake = this.Shake;
                        double x = shake.X;
                        local1 = (float)(num2 + x);
                        ref float local2 = ref this._position.Y;
                        double num3 = local2;
                        shake = this.Shake;
                        double y = shake.Y;
                        local2 = (float)(num3 + y);
                        this._rect = new Rectangle(this.savedata.style[this.style].style * 120, 0, this.player.Wide, this.player.Height);
                        dg.DrawImage(dg, "Silhouette", this._rect, false, this._position, color2);
                    }
                    break;
                case Custom.CUSTOMCHENE.start:
                    if (this.scene != Custom.CUSTOMCHENE.start)
                        break;
                    this._position = new Vector2(120f, 80f);
                    this._rect = new Rectangle(248, 0, 136, 16);
                    float scall = this.frame >= 15 ? (this.frame >= 17 ? (this.frame >= 60 ? (float)(1.0 - (this.frame - 60) * 0.0599999986588955) : 1f) : 1.02f) : frame * 0.06f;
                    dg.DrawImage(dg, "battleobjects", this._rect, false, this._position, scall, 0.0f, Color.White);
                    break;
                default:
                    this._rect = new Rectangle(80, 24, 128, 160);
                    switch (this.scene)
                    {
                        case Custom.CUSTOMCHENE.fadeIn:
                            _point = new Vector2(this.frame * 9 - 128, 0.0f);
                            break;
                        case Custom.CUSTOMCHENE.close:
                        case Custom.CUSTOMCHENE.paprint:
                        case Custom.CUSTOMCHENE.chipMake:
                        case Custom.CUSTOMCHENE.start:
                            _point = new Vector2(sbyte.MinValue, 0.0f);
                            break;
                        case Custom.CUSTOMCHENE.fadeOut:
                            _point = new Vector2(-(this.frame * 9), 0.0f);
                            break;
                        default:
                            _point = new Vector2(0.0f, 0.0f);
                            break;
                    }
                    dg.DrawImage(dg, "battleobjects", this._rect, true, _point, Color.White);
                    if (this.parent.player.style == Player.STYLE.doctor)
                    {
                        this._rect = new Rectangle(351, 39, 98, 50);
                        this._position = new Vector2(_point.X + 7f, 103f);
                        dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                        this._rect = new Rectangle(351, 96, 18, 17);
                        this._position = new Vector2(_point.X + 103f, 88f);
                        dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                    }
                    for (int chip = 0; chip < this.chipopen; ++chip)
                    {
                        if (this.parent.player.style == Player.STYLE.doctor)
                            this._position = new Vector2(_point.X + 8f + 16 * (chip % 6), 104 + 24 * (chip / 6));
                        else
                            this._position = new Vector2(_point.X + 16f + 16 * (chip % 6), 104 + 24 * (chip / 6));
                        if ((chip % 6 != 5 || this.parent.player.style == Player.STYLE.doctor) && !(this.canchips[chip].chip is DammyChip))
                            this.canchips[chip].chip.IconRender(dg, this._position, !this.CanChipSelect(chip), true, this.canchips[chip].codeNo, this.chipSelecFlag[chip]);
                        if (this.savedata.regularflag[this.savedata.efolder] && !this.chipUseFlag[0] && chip == 0 && !this.savedata.FlagList[3])
                        {
                            this._rect = new Rectangle(112, 288, 24, 16);
                            this._position.X -= 8f;
                            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        }
                    }
                    for (int index = 0; index < selectchips; ++index)
                    {
                        this._position = new Vector2(_point.X + 104f, 8 + 16 * index);
                        this.canchips[this.selectedchips[index]].chip.IconRender(dg, this._position, false, false, this.canchips[this.selectedchips[index]].codeNo, false);
                    }
                    if (this.scene != Custom.CUSTOMCHENE.openstyle)
                    {
                        this._position = new Vector2(_point.X + 28f, 24f);
                        if (this.cursor.X != 6)
                        {
                            int index = this.cursor.X + this.cursor.Y * 6;
                            if (this.canchips[index].chip.dark)
                            {
                                this._rect = new Rectangle(536, 344, 80, 104);
                                this._position = new Vector2(_point.X + 16f, 0.0f);
                                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                            }
                            else if (this.canchips[index].chip.navi)
                            {
                                this._rect = new Rectangle(536, 240, 80, 104);
                                this._position = new Vector2(_point.X + 16f, 0.0f);
                                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                            }
                            this._position = new Vector2(_point.X + 28f, 24f);
                            this.canchips[index].chip.GraphicsRender(dg, this._position, this.canchips[index].codeNo, true, true);
                            var blockCharacters = this.Nametodata(this.canchips[index].chip.name);
                            this._position = new Vector2(_point.X + 24f, 7f);
                            DrawBlockCharacters(dg, blockCharacters, 88, this._position, Color.White, out this._rect, out this._position);
                        }
                        else
                        {
                            var infoCardSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("Custom.NoChipInfo");
                            if (this.cursor.Y == 0)
                            {
                                if (this.selectchips == 0)
                                {
                                    infoCardSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("Custom.NoChipInfo");
                                }
                                else
                                {
                                    infoCardSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("Custom.SendChipInfo");
                                }
                            }
                            else if (this.selectchips == 0)
                            {
                                infoCardSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("Custom.AddChipInfo1");
                            }
                            else
                            {
                                infoCardSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("Custom.AddChipInfo2");
                            }
                            this._rect = infoCardSprite.Item2;
                            dg.DrawImage(dg, infoCardSprite.Item1, this._rect, true, this._position, Color.White);
                        }
                    }
                    if (this.savedata.havestyles > 1 && this.player.mind.MindNow != MindWindow.MIND.pinch)
                    {
                        this._rect = new Rectangle(447, 192, 82, this.stylemenu);
                        this._position = new Vector2(_point.X + 15f, 103 - this.stylemenu);
                        dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                    }
                    if (this.styleselecting)
                    {
                        for (int index = 0; index < this.savedata.havestyles; ++index)
                        {
                            var styleStatus = this.styleused[index] || index == this.player.setstyle
                                    ? 2
                                    : (!this.styleset || this.style != index
                                        ? (index == this.savedata.setstyle
                                            ? 1
                                            : 0)
                                        : 3);
                            var styleSprite = this.GetStyleSprite(styleStatus, this.savedata.style[index].style);
                            this._rect = styleSprite.Item2;
                            this._position = new Vector2(_point.X + 16f, 19 + index * 16);
                            dg.DrawImage(dg, styleSprite.Item1, this._rect, true, this._position, Color.White);
                            this._rect = new Rectangle(216 + this.savedata.style[index].element * 16, 88, 16, 16);
                            this._position = new Vector2(_point.X + 80f, 19 + index * 16);
                            dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                        }
                    }
                    if (this.scene != Custom.CUSTOMCHENE.selectstyle)
                    {
                        if (this.cursor.X != 6)
                        {
                            this._rect = new Rectangle(128 + this.frame / 15 * 16, 0, 16, 16);
                            int num2 = this.parent.player.style == Player.STYLE.doctor ? 0 : 8;
                            this._position = new Vector2(_point.X + 8f + num2 + this.cursor.X * 16, 104 + this.cursor.Y * 24);
                        }
                        else
                        {
                            this._rect = new Rectangle(160 + this.frame / 15 * 24, 0, 24, 24);
                            if (this.cursor.Y == 0)
                                this._position = new Vector2(_point.X + 104f, 112f);
                            else
                                this._position = new Vector2(_point.X + 104f, 132f);
                        }
                        dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                    }
                    else
                    {
                        if (this.styleset) { }
                        this._rect = new Rectangle(536, 192 + this.frame / 15 * 16, 80, 16);
                        this._position = new Vector2(_point.X + 16f, 19 + this.styleCursol * 16);
                        dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                    }
                    Color.FromArgb(paalpha, 0, 0, 0);
                    this._rect = new Rectangle(0, 0, 240, 160);
                    this._position = new Vector2(0.0f, 0.0f);
                    dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, Color.FromArgb(this.darkA, Color.Black));
                    break;
            }
        }

        private void CloseStyleMenu()
        {
            this.scene = Custom.CUSTOMCHENE.custom;
            this.styleselecting = false;
            this.stylemenu = 8;
            this.sound.PlaySE(SoundEffect.menuclose);
        }

        private void SortThips(int no, int thip)
        {
            int num = this.chipsort[no];
            for (int index = 0; index < this.chipsort.Length; ++index)
            {
                if (this.chipsort[index] == thip)
                {
                    this.chipsort[no] = this.chipsort[index];
                    this.chipsort[index] = num;
                    break;
                }
            }
        }

        private void TutorialSort()
        {
            if (this.WhatTutorial())
            {
                switch (this.parent.battlenum)
                {
                    case 0:
                        this.SortThips(0, 1);
                        this.SortThips(1, 5);
                        this.SortThips(2, 3);
                        this.SortThips(3, 10);
                        this.SortThips(4, 13);
                        break;
                    case 1:
                        this.SortThips(0, 22);
                        this.SortThips(1, 10);
                        this.SortThips(2, 1);
                        this.SortThips(3, 15);
                        this.SortThips(4, 20);
                        this.SortThips(5, 9);
                        this.SortThips(6, 29);
                        break;
                    case 2:
                        this.SortThips(0, 26);
                        this.SortThips(1, 24);
                        this.SortThips(2, 25);
                        this.SortThips(3, 4);
                        this.SortThips(4, 22);
                        this.SortThips(5, 16);
                        this.SortThips(6, 8);
                        break;
                    case 3:
                        this.SortThips(0, 5);
                        this.SortThips(1, 4);
                        this.SortThips(2, 7);
                        this.SortThips(3, 6);
                        this.SortThips(4, 29);
                        break;
                }
            }
            else if (!Debug.ChipStock) { }
        }

        private bool WhatTutorial()
        {
            return this.savedata.FlagList[3];
        }

        private void EventStartcustom()
        {
            if (!this.WhatTutorial())
                return;
            switch (this.parent.battlenum)
            {
                case 0:
                    if (this.parent.turn == 0)
                    {
                        this.EventtutorialIncustom1_1();
                        break;
                    }
                    break;
                case 1:
                    if (this.parent.turn == 0)
                    {
                        this.EventtutorialIncustom2_1();
                        break;
                    }
                    break;
                case 2:
                    if (this.parent.turn == 0)
                    {
                        this.EventtutorialIncustom3_1();
                        break;
                    }
                    break;
                case 3:
                    if (this.parent.turn == 0)
                    {
                        this.EventtutorialIncustom4_1();
                        break;
                    }
                    break;
                case 4:
                    if (this.parent.turn == 0)
                    {
                        this.EventtutorialIncustom5_1();
                        break;
                    }
                    break;
            }
        }

        private void EventtutorialIncustom1_1()
        {
            this.parent.eventmanager.events.Clear();
            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            var dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage1Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage1Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage1Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage2Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage2Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 3, new Vector2(128f, 72f)));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage3Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 1, new Vector2(27f, 116f)));
            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 1, new Vector2(43f, 116f)));
            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 1, new Vector2(59f, 116f)));
            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 1, new Vector2(75f, 116f)));
            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 1, new Vector2(91f, 116f)));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage4Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage4Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage4Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 3, new Vector2(80f, 84f)));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage5Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage5Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage5Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage5Dialogue4");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage5Dialogue5");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 2, new Vector2(27f, 116f)));
            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 2, new Vector2(59f, 116f)));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage6Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage6Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage6Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage6Dialogue4");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage6Dialogue5");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1Stage6Dialogue6");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
        }

        private void EventtutorialIncustom2_1()
        {
            this.parent.eventmanager.events.Clear();
            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            var dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage1Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage1Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage1Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 3, new Vector2(27f, 116f)));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage2Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 3, new Vector2(43f, 116f)));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage3Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage3Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage3Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 2, new Vector2(27f, 124f)));
            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 2, new Vector2(43f, 124f)));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage4Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage4Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage4Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage4Dialogue4");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage5Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage5Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage5Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage5Dialogue4");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage5Dialogue5");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle2Stage5Dialogue6");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
        }

        private void EventtutorialIncustom3_1()
        {
            this.parent.eventmanager.events.Clear();
            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            var dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage1Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 3, new Vector2(116f, 144f)));

            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage2Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage2Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage2Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage2Dialogue4");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage2Dialogue5");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage2Dialogue6");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage2Dialogue7");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage3Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage3Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage3Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage3Dialogue4");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage3Dialogue5");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage4Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage4Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage4Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage4Dialogue4");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage4Dialogue5");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage4Dialogue6");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage4Dialogue7");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage4Dialogue8");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage4Dialogue9");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3Stage5Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
        }

        private void EventtutorialIncustom4_1()
        {
            int num = 32;
            this.parent.eventmanager.events.Clear();
            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            var dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage1Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 3, new Vector2(160f, num)));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage2Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage2Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage2Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage2Dialogue4");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new MindChange(this.sound, this.parent.eventmanager, this.player.mind, MindWindow.MIND.pinch, this.savedata));
            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 3, new Vector2(160f, num)));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage3Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new MindChange(this.sound, this.parent.eventmanager, this.player.mind, MindWindow.MIND.smile, this.savedata));
            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 3, new Vector2(160f, num)));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage4Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage4Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage4Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new MindChange(this.sound, this.parent.eventmanager, this.player.mind, MindWindow.MIND.fullsync, this.savedata));
            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 3, new Vector2(160f, num)));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage5Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage5Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage5Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage5Dialogue4");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage5Dialogue5");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage5Dialogue6");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage6Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage6Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage6Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage6Dialogue4");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage6Dialogue5");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle4Stage6Dialogue6");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
        }
        private void EventtutorialIncustom5_1()
        {
            this.parent.eventmanager.events.Clear();
            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            var dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle5Stage1Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
        }

        private void EventCustomend()
        {
            if (!this.WhatTutorial())
                return;
            switch (this.parent.battlenum)
            {
                case 0:
                    if (this.parent.turn == 0)
                    {
                        this.EventtutorialCustomend1_1();
                        break;
                    }
                    break;
                case 2:
                    if (this.parent.turn == 0)
                    {
                        this.EventtutorialCustomend3_1();
                        break;
                    }
                    break;
            }
        }

        private void EventtutorialCustomend1_1()
        {
            this.parent.eventmanager.events.Clear();
            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            var dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1ActionOKDialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1ActionOKDialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
        }

        private void EventtutorialCustomend3_1()
        {
            this.parent.eventmanager.events.Clear();
            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            var dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle3ActionOKDialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
        }

        private void EventAdd(bool addok)
        {
            if (!addok)
                this.EventAddForbidden();
            else
                this.EventAddNormal();
        }

        private void EventAddForbidden()
        {
            if (!this.savedata.FlagList[0])
            {
                this.parent.eventmanager.events.Clear();
                this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
                var dialogue = ShanghaiEXE.Translate("Custom.TutorialBattle1ActionADDDialogue1");
                this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
            }
            else
            {
                this.parent.eventmanager.events.Clear();
                this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
                var dialogue = ShanghaiEXE.Translate("Custom.ActionSpecialADD");
                this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
            }
        }

        private void EventAddNormal()
        {
            this.parent.eventmanager.events.Clear();
            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            var dialogue = ShanghaiEXE.Translate("Custom.ActionADD");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
        }

        private void EventStartbattle()
        {
            if (!this.WhatTutorial() || this.parent.battlenum != 0 || this.parent.turn != 0)
                return;
            this.EventtutorialStartbattle1_1();
        }

        private void EventtutorialStartbattle1_1()
        {
            this.parent.eventmanager.events.Clear();
            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            var dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage1Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage1Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage1Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage1Dialogue4");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage1Dialogue5");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage1Dialogue6");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage1Dialogue7");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage1Dialogue8");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage1Dialogue9");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage2Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage2Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 3, new Vector2(32f, 16f)));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage3Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage3Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 3, new Vector2(140f, 92f)));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage4Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage5Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage5Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage5Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));

            this.parent.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.parent.eventmanager, 3, new Vector2(120f, 16f)));

            this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage6Dialogue1");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage6Dialogue2");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage6Dialogue3");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("Custom.TutorialBattleStart1Stage6Dialogue4");
            this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
        }

        public enum CUSTOMCHENE
        {
            fadeIn,
            custom,
            openstyle,
            selectstyle,
            close,
            fadeOut,
            paprint,
            stylechange,
            chipMake,
            start,
            escape,
        }
    }
}
