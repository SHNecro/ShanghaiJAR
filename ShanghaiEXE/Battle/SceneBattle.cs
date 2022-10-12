using NSAttack;
using NSBackground;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSEvent;
using NSGame;
using NSNet;
using NSObject;
using NSShanghaiEXE.Common;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System;

namespace NSBattle
{
    public class SceneBattle : SceneBase
    {
        private static readonly List<Type> TypeRenderOrder = new List<Type> { typeof(ObjectBase), typeof(EnemyBase), typeof(Player), typeof(AttackBase), typeof(EffectBase) };

        public int bashtime = 0;
        public byte fadealpha = byte.MaxValue;
        public Panel[,] panel = new Panel[6, 3];
        public int backscreen = 0;
        public int backscreencolor = 0;
        public bool namePrint = true;
        public int turn = 0;
        private int initcount = 0;
        private int initflame = 0;
        private readonly byte[] time = new byte[3];
        public List<EnemyBase> enemys = new List<EnemyBase>();
        public List<EnemyBase> deletedEnemies = new List<EnemyBase>();
        public List<ObjectBase> objects = new List<ObjectBase>();
        public List<AttackBase> attacks = new List<AttackBase>();
        public List<EffectBase> effects = new List<EffectBase>();
        public List<Player> players = new List<Player>();
        public byte printdelete = 0;
        public List<ChipBase> blackOutChips = new List<ChipBase>();
        private readonly List<AttackBase> attackLosted = new List<AttackBase>();
        private const byte hpwindowpositionX = 24;
        private const byte hpwindowpositionY = 8;
        public const byte enemyFadeTime = 30;
        public Player player;
        public int redRight;
        public int blueLeft;
        private Result result;
        public bool doresult;
        public bool illegal;
        public byte manyCounter;
        public Custom custom;
        public MindWindow mind;
        public SceneMain main;
        private int counterPrintTime;
        public bool canEscape;
        public bool noFanfale;
        private int startflame;
        public bool init;
        private readonly int battlecount;
        private readonly SceneMain playerstatus;
        public SceneBattle.BATTLESCENE nowscene;
        public int manyenemys;
        private BackgroundBase back;
        public Vector2 positionHPwindow;
        public bool tutorial;
        public int battlenum;
        public byte simultaneoustime;
        public byte simultaneousdel;
        public ChipFolder[] dropchip;
        public int dropzenny;
        public CustomGauge customgauge;
        public EventManager eventmanager;
        public int lastCounterBug;
        public int lastCounterFreeze;
        public int lastCounterError;
        private readonly string bgm;
        private readonly bool gameover;
        public bool blackOut;
        public Panel.COLOR blackOuter;
        public bool stopEnd;
        public bool bossFlag;
        public bool blackOutStopper;
        public bool resultError;

        public SceneBattle() : base(null, null, null) { }

        public SceneBattle(
          IAudioEngine s,
          ShanghaiEXE p,
          SceneMain main,
          EventManager e,
          bool res,
          int count,
          bool gameover,
          string bgm,
          SaveData save)
          : base(s, p, save)
        {
            p.TexClear(false);
            this.main = main;
            this.gameover = gameover;
            this.bgm = bgm;
            this.doresult = res;
            for (int x = 0; x < this.panel.GetLength(0); ++x)
            {
                for (int y = 0; y < this.panel.GetLength(1); ++y)
                {
                    this.panel[x, y] = new Panel(this.sound, this, x, y)
                    {
                        state = Panel.PANEL._nomal,
                        color = x >= 3 ? Panel.COLOR.blue : Panel.COLOR.red
                    };
                    this.panel[x, y].colordefault = this.panel[x, y].color;
                    if (x == 0 || x == 5)
                        this.panel[x, y].inviolability = true;
                }
            }
            this.battlenum = this.savedata.ValList[9];
            this.tutorial = this.parent.tutorial;
            this.eventmanager = e;
            this.playerstatus = main;
            this.positionHPwindow = new Vector2(24f, 8f);
            this.mind = new MindWindow(this.sound, this, this.savedata);
            this.player = new Player(this.sound, this, 1, 1, main, this.savedata.busterspec[0], this.savedata.busterspec[1], this.savedata.busterspec[2], this.mind, this.savedata);
            this.players.Add(this.player);
            this.parent = p;
            this.back = new BackDefault();
            this.nowscene = SceneBattle.BATTLESCENE.init;
            this.custom = new Custom(this.sound, this, main, this.player, this.savedata);
            this.customgauge = new CustomGauge(this.sound);
            this.battlecount = count;
            AttackBase.AtIDBlue = 0L;
            AttackBase.AtIDRed = 0L;
        }

        public void SetBack(int number)
        {
            this.back = BackgroundBase.BackMake(number);
        }

        public void SetBack(BackgroundBase b)
        {
            this.back = b;
        }

        public void EnemySet(
          EnemyBase enemy1,
          EnemyBase enemy2,
          EnemyBase enemy3,
          int pattern,
          Panel.PANEL panel1,
          Panel.PANEL panel2)
        {
            if (enemy1 != null)
            {
                if (enemy1.Parent == null)
                    enemy1.Parent = this;
                if (enemy1.Sound == null)
                    enemy1.Sound = this.sound;
                this.enemys.Add(enemy1);
                if (enemy1.race == EnemyBase.ENEMY.navi && !(enemy1 is NormalNavi))
                    this.bossFlag = true;
                enemy1.Init();
                enemy1.enemyCount = true;
            }
            if (enemy2 != null)
            {
                if (enemy2.Parent == null)
                    enemy2.Parent = this;
                if (enemy2.Sound == null)
                    enemy2.Sound = this.sound;
                this.enemys.Add(enemy2);
                if (enemy2.race == EnemyBase.ENEMY.navi && !(enemy2 is NormalNavi))
                    this.bossFlag = true;
                enemy2.Init();
                enemy2.enemyCount = true;
            }
            if (enemy3 != null)
            {
                if (enemy3.Parent == null)
                    enemy3.Parent = this;
                if (enemy3.Sound == null)
                    enemy3.Sound = this.sound;
                this.enemys.Add(enemy3);
                if (enemy3.race == EnemyBase.ENEMY.navi && !(enemy3 is NormalNavi))
                    this.bossFlag = true;
                enemy3.Init();
                enemy3.enemyCount = true;
            }
            this.PanelChange(pattern, panel1, panel2);
            this.manyenemys = this.enemys.Count;
            if (this.player.style == Player.STYLE.gaia)
                this.GaiaPanelChange(this.player.Element, false);
            if (this.bossFlag)
                this.mind.MindNow = MindWindow.MIND.normal;
            this.player.AddOn();
            if (this.panel[1, 1].state == Panel.PANEL._none && !this.player.addonSkill[58])
                this.player.positionX = 0;
            if (!this.player.Canmove(this.player.position) && !this.player.addonSkill[58])
            {
                this.player.position.X = 0;
                this.player.positionre.X = 0;
                this.player.positionold.X = 0;
                this.player.PositionDirectSet();
            }
            foreach (CharacterBase characterBase in this.AllChara())
                characterBase.InitAfter();
            if (this.player.style != Player.STYLE.gaia)
                return;
            this.GaiaPanelChange(this.player.Element, false);
        }

        public override void Updata()
        {
            if (this.manyenemys <= 0 && this.nowscene != SceneBattle.BATTLESCENE.end && !this.blackOut && !this.stopEnd)
            {
                this.savedata.selectQuestion = 0;
                this.BattleEnd(false);
            }
            Panel[,] panel = this.panel;
            int upperBound1 = panel.GetUpperBound(0);
            int upperBound2 = panel.GetUpperBound(1);
            for (int lowerBound1 = panel.GetLowerBound(0); lowerBound1 <= upperBound1; ++lowerBound1)
            {
                for (int lowerBound2 = panel.GetLowerBound(1); lowerBound2 <= upperBound2; ++lowerBound2)
                    panel[lowerBound1, lowerBound2].BrightReset();
            }
            this.back.Update();
            if (this.nowscene == SceneBattle.BATTLESCENE.init)
                this.EnemyInit();
            if (this.eventmanager.playevent)
            {
                this.eventmanager.UpDate();
                foreach (Player player in this.players)
                    player.mind.Update();
            }
            else
            {
                switch (this.nowscene)
                {
                    case SceneBattle.BATTLESCENE.printstart:
                        if (this.startflame > 75)
                            this.nowscene = SceneBattle.BATTLESCENE.custom;
                        ++this.startflame;
                        break;
                    case SceneBattle.BATTLESCENE.battle:
                    case SceneBattle.BATTLESCENE.end:
                    case SceneBattle.BATTLESCENE.dead:
                        this.UpdateBattle();
                        break;
                }
                if (this.nowscene == SceneBattle.BATTLESCENE.custom)
                    this.custom.Update();
                if (this.nowscene == SceneBattle.BATTLESCENE.dead)
                {
                    if (this.fadealpha < byte.MaxValue)
                    {
                        this.fadealpha += 5;
                    }
                    else
                    {
                        this.fadealpha = byte.MaxValue;
                        this.sound.StopBGM();
                        this.Death();
                    }
                }
                if (this.nowscene == SceneBattle.BATTLESCENE.battle && Input.IsPress(Button._Start))
                {
                    this.nowscene = SceneBattle.BATTLESCENE.pause;
                    this.sound.PlaySE(SoundEffect.pause);
                }
                else if (this.nowscene == SceneBattle.BATTLESCENE.pause && Input.IsPress(Button._Start))
                {
                    this.nowscene = SceneBattle.BATTLESCENE.battle;
                    this.sound.PlaySE(SoundEffect.pause);
                }
                if (!this.blackOut)
                {
                    if (this.printdelete > 0)
                        --this.printdelete;
                    if (this.simultaneoustime > 0)
                        --this.simultaneoustime;
                    if (this.counterPrintTime > 0)
                        --this.counterPrintTime;
                }
                foreach (CharacterBase characterBase in this.AllChara())
                    characterBase.HPDown();
                this.mind.Update();
            }
        }

        private void UpdateAllObject()
        {
            foreach (CharacterBase allObject in this.AllObjects())
            {
                int num = !allObject.invincibility ? 0 : (!this.blackOut ? 1 : 0);
                allObject.mastorcolor = num == 0
                    ? Color.FromArgb(byte.MaxValue, allObject.mastorcolor)
                    : Color.FromArgb(sbyte.MaxValue * Math.Abs(allObject.invincibilitytime % 3), allObject.mastorcolor);
                if (!this.blackOut)
                {
                    if (allObject.invincibilitytime == 0)
                    {
                        allObject.invincibility = false;
                    }
                    else
                    {
                        allObject.invincibility = true;
                        --allObject.invincibilitytime;
                    }
                }
            }
            this.Debug();
        }

        private void Debug()
        {
        }

        public override void Render(IRenderer dg)
        {
            this.back.Render(dg);
            if (this.player.badstatus[4])
            {
                Color color = Color.FromArgb(200, Color.Black);
                this._rect = new Rectangle(0, 0, 240, 160);
                this._position = new Vector2(0.0f, 0.0f);
                dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color);
            }
            Panel[,] panel = this.panel;
            int upperBound1 = panel.GetUpperBound(0);
            int upperBound2 = panel.GetUpperBound(1);
            for (int lowerBound1 = panel.GetLowerBound(0); lowerBound1 <= upperBound1; ++lowerBound1)
            {
                for (int lowerBound2 = panel.GetLowerBound(1); lowerBound2 <= upperBound2; ++lowerBound2)
                    panel[lowerBound1, lowerBound2].Render(dg);
            }
            if (this.backscreen > 0)
            {
                Color color = Color.FromArgb(this.backscreen, this.backscreencolor, this.backscreencolor, this.backscreencolor);
                this._rect = new Rectangle(0, 0, 240, 160);
                this._position = new Vector2(0.0f, 0.0f);
                dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color);
            }
            foreach (CharacterBase allObject in this.AllObjects())
            {
                if (allObject.downprint)
                {
                    allObject.BarrierRend(dg);
                    allObject.Render(dg);
                    allObject.BarrierPowerRend(dg);
                }
                allObject.RenderDOWN(dg);
            }

            var renderedObjects = this.AllObjects().Where(allObject =>
                (allObject.position.Y >= 0 && allObject.position.Y <= 2) 
                && (!(allObject is EnemyBase) || allObject.Hp <= 0 || !this.player.badstatus[4])
                && (!allObject.upprint && !allObject.downprint) && allObject.rend)
                .OrderBy(ao => ao.position.Y).ThenByDescending(ao => TypeRenderOrder.IndexOf(ao.GetType()));
            foreach (CharacterBase allObject in renderedObjects)
            {
                allObject.BarrierRend(dg);
                allObject.Render(dg);
                allObject.BarrierPowerRend(dg);
            }

            foreach (CharacterBase allObject in this.AllObjects())
            {
                if (allObject.upprint)
                {
                    allObject.BarrierRend(dg);
                    allObject.Render(dg);
                    allObject.BarrierPowerRend(dg);
                }
                allObject.RenderUP(dg);
            }
            if (!this.player.addonSkill[57])
            {
                foreach (CharacterBase characterBase in this.AllChara())
                    characterBase.HPRender(dg);
            }
            foreach (CharacterBase characterBase in this.AllChara())
            {
                if (characterBase is EnemyBase enemyBase)
                {
                    enemyBase.Nameprint(dg, enemyBase.printNumber);
                }
            }
            if (this.nowscene != SceneBattle.BATTLESCENE.end)
                this.mind.Render(dg, this.positionHPwindow.X);
            if (this.nowscene == SceneBattle.BATTLESCENE.custom)
            {
                this.custom.Render(dg);
                if (this.custom.scene == Custom.CUSTOMCHENE.chipMake)
                    this.customgauge.Render(dg);
            }
            else if (this.nowscene != SceneBattle.BATTLESCENE.end && this.nowscene != SceneBattle.BATTLESCENE.init && this.nowscene != SceneBattle.BATTLESCENE.printstart)
                this.customgauge.Render(dg);
            else if (this.nowscene == SceneBattle.BATTLESCENE.end)
                this.result.Render(dg);
            if (this.printdelete > 0 && this.simultaneousdel >= 2 && !this.blackOut)
            {
                this._rect = new Rectangle(456, 32 + (simultaneousdel - 2) * 16, 112, 16);
                this._position = new Vector2(120f, 24f);
                dg.DrawImage(dg, "battleobjects", this._rect, false, this._position, Color.White);
            }
            else if (this.counterPrintTime > 0)
            {
                this._rect = new Rectangle(536, 224, 96, 16);
                this._position = new Vector2(120f, 24f);
                dg.DrawImage(dg, "battleobjects", this._rect, false, this._position, Color.White);
            }
            if (this.nowscene == SceneBattle.BATTLESCENE.pause)
            {
                this._rect = new Rectangle(208, 0, 40, 16);
                this._position = new Vector2(120f, 60f);
                dg.DrawImage(dg, "battleobjects", this._rect, false, this._position, Color.White);
            }
            if (this.nowscene == SceneBattle.BATTLESCENE.printstart)
            {
                this._position = new Vector2(120f, 80f);
                this._rect = new Rectangle(456, 64, 152, 16);
                float scall = this.startflame >= 15 ? (this.startflame >= 17 ? (this.startflame >= 60 ? (float)(1.0 - (this.startflame - 60) * 0.0599999986588955) : 1f) : 1.02f) : startflame * 0.06f;
                dg.DrawImage(dg, "battleobjects", this._rect, false, this._position, scall, 0.0f, Color.White);
                int[] numArray = this.ChangeCount(this.battlecount + 1);
                foreach (var data in ((IEnumerable<int>)numArray).Select((v, i) => new
                {
                    v,
                    i
                }))
                {
                    this._rect = new Rectangle(data.v * 8, 104, 8, 16);
                    this._position = new Vector2(120 + (numArray.Length - 1) * 4 - data.i * 8, 79f);
                    dg.DrawImage(dg, "font", this._rect, false, this._position, scall, 0.0f, Color.FromArgb(byte.MaxValue, byte.MaxValue, 230, 30));
                }
            }
            Color color1 = this.nowscene != SceneBattle.BATTLESCENE.dead ? Color.FromArgb(fadealpha, byte.MaxValue, byte.MaxValue, byte.MaxValue) : Color.FromArgb(fadealpha, 0, 0, 0);
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color1);
            if (this.eventmanager.playevent)
                this.eventmanager.Render(dg);

            this.Remove();
        }

        private void UpdateBattle()
        {
            if (this.bashtime > 0)
                --this.bashtime;
            List<CharacterBase> list = new List<CharacterBase>().Concat<CharacterBase>(this.enemys.Cast<CharacterBase>()).ToList<CharacterBase>();
            list.Add(this.player);
            this.redRight = 0;
            this.blueLeft = 5;
            foreach (CharacterBase characterBase in list)
            {
                if (characterBase.union == Panel.COLOR.red)
                {
                    if (this.redRight < characterBase.position.X)
                        this.redRight = characterBase.position.X;
                }
                else if (this.blueLeft > characterBase.position.X)
                    this.blueLeft = characterBase.position.X;
            }
            foreach (CharacterBase allObject in this.AllObjects())
            {
                if (!this.blackOut || allObject.blackOutObject)
                    allObject.Updata();
                if (this.blackOut)
                {
                    if (allObject is Player player)
                    {
                        if (this.blackOutChips.Count > 0)
                        {
                            if (!this.blackOutChips[0].blackOutLend && this.blackOutChips[0].userNum != player.number && this.blackOutChips.Count < 3)
                                player.BlackOutControl();
                            if (this.blackOutChips[0].userNum == player.number && this.blackOutChips[0].BlackOut(player, this, this.blackOutChips[0].name, this.blackOutChips[0].PowerToString(player)))
                                this.blackOutChips[0].ActionBO(player, this);
                        }
                    }
                    else if (this.blackOutChips.Count > 0 && this.blackOutChips[0].userNum == allObject.number && this.blackOutChips[0].BlackOut(allObject, this, this.blackOutChips[0].name, this.blackOutChips[0].PowerToString(allObject)))
                        this.blackOutChips[0].ActionBO(allObject, this);
                }
                allObject.ShakingSingle();
            }
            foreach (EnemyBase enemy in this.enemys)
            {
                if (enemy.whitetime > 0)
                    --enemy.whitetime;
            }
            Panel[,] panel = this.panel;
            int upperBound1 = panel.GetUpperBound(0);
            int upperBound2 = panel.GetUpperBound(1);
            for (int lowerBound1 = panel.GetLowerBound(0); lowerBound1 <= upperBound1; ++lowerBound1)
            {
                for (int lowerBound2 = panel.GetLowerBound(1); lowerBound2 <= upperBound2; ++lowerBound2)
                    panel[lowerBound1, lowerBound2].Update();
            }
            this.UpdateAllObject();
            if (this.nowscene != SceneBattle.BATTLESCENE.end)
            {
                if (this.manyenemys <= 0)
                    return;
                this.HitCheck();
                if (!this.blackOut)
                {
                    this.customgauge.Update();
                    ++this.time[0];
                    if (this.time[0] >= 60)
                    {
                        this.time[0] = 0;
                        ++this.time[1];
                        if (this.time[1] >= 60)
                        {
                            this.time[1] = 0;
                            if (this.time[2] < 99)
                                ++this.time[2];
                        }
                    }
                }
            }
            else
                this.result.Update();
            this.Remove();
        }

        public void CustomReset()
        {
            this.customgauge.Reset();
        }

        public void PanelBright(int x, int y)
        {
            this.panel[x, y].Bright();
        }

        private void Remove()
        {
            foreach (AttackBase attack in this.attacks)
            {
                if (!attack.flag)
                    this.attackLosted.Add(attack);
            }
            this.deletedEnemies.AddRange(this.enemys.Where(e => !e.flag));
            this.enemys.RemoveAll(e => !e.flag);
            this.objects.RemoveAll(o => !o.flag);
            this.attacks.RemoveAll(a => !a.flag);
            this.effects.RemoveAll(e => !e.flag);

            this.manyenemys = this.enemys.Where(e => e.enemyCount).Count();
        }

        public CharacterBase[] AllChara()
        {
            return new List<CharacterBase>().Concat<CharacterBase>(this.players.Cast<CharacterBase>()).Concat<CharacterBase>(this.enemys.Cast<CharacterBase>()).ToList<CharacterBase>().ToArray();
        }

        public CharacterBase[] AllHitter()
        {
            return new List<CharacterBase>().Concat<CharacterBase>(this.players.Cast<CharacterBase>()).Concat<CharacterBase>(this.objects.Cast<CharacterBase>()).Concat<CharacterBase>(this.enemys.Cast<CharacterBase>()).ToList<CharacterBase>().ToArray();
        }

        public CharacterBase[] AllObjects()
        {
            return new List<CharacterBase>().Concat<CharacterBase>(this.players.Cast<CharacterBase>()).Concat<CharacterBase>(this.objects.Cast<CharacterBase>()).Concat<CharacterBase>(this.enemys.Cast<CharacterBase>()).Concat<CharacterBase>(this.effects.Cast<CharacterBase>()).Concat<CharacterBase>(this.attacks.Cast<CharacterBase>()).ToList<CharacterBase>().ToArray();
        }

        public bool OnPanelCheck(int x, int y, bool re)
        {
            foreach (CharacterBase characterBase in this.AllHitter())
            {
                if (characterBase.position == new Point(x, y) || re && characterBase.positionre == new Point(x, y))
                    return true;
            }
            return false;
        }

        private void EnemyInit()
        {
            if (this.fadealpha == 0)
            {
                if (this.initcount < manyenemys)
                {
                    ++this.initflame;
                    if (this.enemys[this.initcount].EnemyInitAction())
                    {
                        this.initflame = 0;
                        ++this.initcount;
                    }
                }
                else
                    this.nowscene = this.battlecount < 0 ? (this.nowscene = SceneBattle.BATTLESCENE.custom) : (this.nowscene = SceneBattle.BATTLESCENE.printstart);
            }
            else
            {
                if (this.fadealpha == byte.MaxValue)
                    this.sound.StartBGM(this.bgm);
                this.fadealpha -= 15;
            }
        }

        public void BattleEnd(bool escape)
        {
            this.main.FolderReset();
            this.nowscene = SceneBattle.BATTLESCENE.end;
            this.savedata.FlagList[6] = !escape;
            this.player.chargeTime = 0;
            this.player.chargeMax = false;
            this.player.charge = false;
            this.result = new Result(this.sound, this, this.playerstatus, this.time, this.simultaneousdel, this.player.damaged, this.player.manymove, this.player.stylepoint, this.player, this.savedata, escape);
            if (!escape)
            {
                this.SetBustedFlags();
            }
            for (int index = 0; index < this.player.haveChip.Count; ++index)
                this.player.haveChip[index] = new ChipBase(this.sound);
            this.player.numOfChips = 0;
        }

        private void SetBustedFlags()
        {
            var enemies = this.deletedEnemies;
            foreach (var enemy in enemies)
            {
                if (enemy.version >= 3)
                {
                    var v3BustFlag = default(int?);
                    switch (enemy)
                    {
                        case Cirno e: v3BustFlag = 838; break;
                        case PyroMan e: v3BustFlag = 839; break;
                        case Mrasa e: v3BustFlag = 840; break;
                        case ScissorMan e: v3BustFlag = 841; break;
                        case Chen e: v3BustFlag = 842; break;
                        case DruidMan e: v3BustFlag = 843; break;
                        case Marisa e: v3BustFlag = 844; break;
                        case Sakuya e: v3BustFlag = 845; break;
                        case TankMan e: v3BustFlag = 846; break;
                        case Iku e: v3BustFlag = 847; break;
                        case SpannerMan e: v3BustFlag = 848; break;
                        case Medicine e: v3BustFlag = 849; break;
                        case Yorihime e: v3BustFlag = 850; break;
                        case HakutakuMan e: v3BustFlag = 851; break;
                        case TortoiseMan e: v3BustFlag = 852; break;
                        case BeetleMan e: v3BustFlag = 853; break;
                        case Ran e: v3BustFlag = 854; break;
                        case Uthuho e: v3BustFlag = 855; break;
                        case Youmu e: v3BustFlag = 856; break;
                    }

                    if (v3BustFlag != null)
                    {
                        enemy.parent.parent.savedata.FlagList[v3BustFlag.Value] = true;
                    }
                }

                if (enemy.version >= 4)
                {
                    var spBustFlag = default(int?);
                    switch (enemy)
                    {
                        case Marisa e: spBustFlag = 620; break;
                        case Sakuya e: spBustFlag = 621; break;
                        case TankMan e: spBustFlag = 622; break;
                        case SpannerMan e: spBustFlag = 623; break;
                        case HakutakuMan e: spBustFlag = 625; break;
                        case TortoiseMan e: spBustFlag = 626; break;
                        case BeetleMan e: spBustFlag = 627; break;
                        case Yorihime e: spBustFlag = 628; break;
                        case Cirno e: spBustFlag = 629; break;
                        case Medicine e: spBustFlag = 630; break;
                        case Iku e: spBustFlag = 631; break;
                        case PyroMan e: spBustFlag = 632; break;
                        case Mrasa e: spBustFlag = 633; break;
                        case ScissorMan e: spBustFlag = 634; break;
                        case Chen e: spBustFlag = 635; break;
                        case Ran e: spBustFlag = 636; break;
                        case Uthuho e: spBustFlag = 640; break;
                    }

                    if (spBustFlag != null)
                    {
                        enemy.parent.parent.savedata.FlagList[spBustFlag.Value] = true;
                    }
                }

                if (enemy.version == 0)
                {
                    for (int index = 0; index < Wanted.WantedList.GetLength(0); ++index)
                    {
                        if ((EnemyBase.VIRUS)Wanted.WantedList[index, 0] == enemy.ID && enemy.parent.parent.savedata.FlagList[Wanted.WantedList[index, 2]])
                        {
                            enemy.parent.parent.savedata.FlagList[Wanted.WantedList[index, 2]] = false;
                            enemy.parent.parent.savedata.virusSPbusted[(int)enemy.ID] = true;
                            enemy.parent.parent.savedata.virusSPbustedFlug[(int)enemy.ID] = true;
                            enemy.parent.parent.savedata.ValList[19] = 0;
                            break;
                        }
                    }
                }
            }
        }

        private void HitCheck()
        {
            for (int index1 = 0; index1 < this.attacks.Count; ++index1)
            {
                AttackBase attack1 = this.attacks[index1];
                if (attack1.hitting && (!this.blackOut || attack1.blackOutObject) && !attack1.effectMode)
                {
                    foreach (Player player in this.players)
                    {
                        if (attack1.union != player.union && (!player.nohit && player.printplayer && (!player.invincibility || attack1.breakinvi) && attack1.HitCheck(player.position)))
                            attack1.HitEvent(player);
                    }
                    foreach (EnemyBase enemy in this.enemys)
                    {
                        if (!enemy.nohit && (!enemy.invincibility || attack1.breakinvi))
                        {
                            if (attack1.HitCheck(enemy.position, enemy.union))
                                attack1.HitEvent(enemy);
                            if (attack1.rehit && attack1.HitCheck(enemy.positionre, enemy.union))
                                attack1.HitEvent(enemy);
                        }
                    }
                    if (attack1.parry)
                    {
                        List<BustorShot> bustorShotList = new List<BustorShot>();
                        foreach (AttackBase attack2 in this.attacks)
                        {
                            if (attack2.hitting && (!object.Equals(attack1, attack2) && attack1.HitCheck(attack2.position, attack2.union)))
                            {
                                this.sound.PlaySE(SoundEffect.damagezero);
                                this.effects.Add(new Guard(this.sound, this, attack2.position.X, attack2.position.Y, 2));
                                BustorShot bustorShot = new BustorShot(this.sound, this, attack2.position.X, attack2.position.Y, attack1.union, attack1.power, BustorShot.SHOT.reflect, attack1.Element, true, 0)
                                {
                                    canCounter = false,
                                    breaking = false,
                                    invincibility = attack1.invincibility,
                                    invincibilitytime = attack1.invincibilitytime,
                                    invincibilitytimeA = attack1.invincibilitytimeA,
                                };
                                bustorShotList.Add(bustorShot);
                                attack2.flag = false;
                            }
                        }
                        foreach (AttackBase attackBase in bustorShotList)
                            this.attacks.Add(attackBase);
                    }
                    for (int index2 = 0; index2 < this.objects.Count; ++index2)
                    {
                        if (!this.objects[index2].nohit && (this.objects[index2].unionhit || this.objects[index2].union != attack1.union) && (attack1.HitCheck(this.objects[index2].position) && !this.objects[index2].nohit))
                            attack1.HitEvent(this.objects[index2]);
                    }
                }
                if (attack1.panelChange && !attack1.flag)
                    attack1.PanelChange();
            }
        }

        // TODO: Figure out if this can be removed safely
        public void ResetPlayerChips()
        {
            foreach (var data in this.player.haveChip.Select((v, i) => new
            {
                v,
                i
            }))
                this.player.haveChip[data.i] = new ChipBase(this.sound);
        }

        public void CounterHit()
        {
            this.sound.PlaySE(SoundEffect.counterhit);
            this.mind.MindNow = MindWindow.MIND.fullsync;
            this.counterPrintTime = 60;
        }

        public void GaiaPanelChange(ChipBase.ELEMENT element, bool half)
        {
            for (int index1 = 0; index1 < this.panel.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.panel.GetLength(1); ++index2)
                {
                    if ((!half || index1 >= 3) && ((index1 == 1 || index1 == 4) && index2 != 1 || index2 == 1) && (this.panel[index1, index2].state != Panel.PANEL._none && this.panel[index1, index2].state != Panel.PANEL._un))
                    {
                        switch (element)
                        {
                            case ChipBase.ELEMENT.heat:
                                this.panel[index1, index2].state = Panel.PANEL._burner;
                                break;
                            case ChipBase.ELEMENT.aqua:
                                this.panel[index1, index2].state = Panel.PANEL._ice;
                                break;
                            case ChipBase.ELEMENT.eleki:
                                this.panel[index1, index2].state = Panel.PANEL._thunder;
                                break;
                            case ChipBase.ELEMENT.leaf:
                                this.panel[index1, index2].state = Panel.PANEL._grass;
                                break;
                            case ChipBase.ELEMENT.poison:
                                this.panel[index1, index2].state = Panel.PANEL._poison;
                                break;
                            case ChipBase.ELEMENT.earth:
                                this.panel[index1, index2].state = Panel.PANEL._sand;
                                break;
                        }
                    }
                }
            }
        }

        public void PanelChange(int pattern, Panel.PANEL panel1, Panel.PANEL panel2)
        {
            var layout = Constants.PanelLayouts[pattern];
            for (int pX = 0; pX < this.panel.GetLength(0); ++pX)
            {
                for (int pY = 0; pY < this.panel.GetLength(1); ++pY)
                {
                    switch (layout[pY, pX])
                    {
                        case 0:
                            this.panel[pX, pY].state = panel1;
                            break;
                        case 1:
                            this.panel[pX, pY].state = panel2;
                            break;
                        case 2:
                            this.panel[pX, pY].state = Panel.PANEL._nomal;
                            break;
                        case 3:
                            this.panel[pX, pY].state = Panel.PANEL._nomal;
                            this.objects.Add(new Rock(this.sound, this, pX, pY, this.panel[pX, pY].color));
                            break;
                    }
                }
            }
        }

        public void CustomSpeedChange(int speed)
        {
            this.customgauge.Customspeed = speed;
        }

        public void CustomMax()
        {
            this.customgauge.Max();
        }

        private void Death()
        {
            if (this.gameover)
            {
                this.parent.ChangeOfSecne(Scene.GameOver);
            }
            else
            {
                this.savedata.FlagList[6] = false;
                if (this.savedata.isJackedIn)
                    this.savedata.HPnow = 1;
                else
                    this.savedata.HPnow = this.savedata.HPMax;
                this.main.mapscene.HP.hpprint = this.savedata.HPnow;
                if (this.mind.MindNow != MindWindow.MIND.pinch)
                    this.savedata.mind = 0;
                else
                    this.savedata.mind = 0;
                this.sound.StopBGM();
                this.main.NowScene = SceneMain.PLAYSCENE.map;
                this.main.mapscene.battleflag = false;
            }
        }

        public enum BATTLESCENE
        {
            init,
            custom,
            printstart,
            battle,
            pause,
            end,
            dead,
        }
    }
}
