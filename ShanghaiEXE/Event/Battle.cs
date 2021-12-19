using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSGame;
using NSMap;
using NSNet;
using System.Drawing;
using System;

namespace NSEvent
{
    internal class Battle : EventBase
    {
        public EnemyBase.VIRUS[] enemy = new EnemyBase.VIRUS[3];
        public byte[] lank = new byte[3];
        private readonly Point[] position = new Point[3];
        private readonly int[] hp = new int[3];
        private readonly string[] name = new string[3];
        private readonly int[] chip1 = new int[3];
        private readonly int[] chip2 = new int[3];
        private readonly int[] chip3 = new int[3];
        private readonly Panel.PANEL[] panels = new Panel.PANEL[2];
        private const int manyenemy = 3;
        private bool endflag;
        private readonly int back;
        private readonly int paneltype;
        private readonly bool countRend;
        private readonly bool resultRend;
        private readonly bool canEscape;
        private readonly string bgm;
        private readonly bool gameover;

        public SceneMain MainScene
        {
            set
            {
                this.manager.parent.main = value;
            }
            get
            {
                return this.manager.parent.main;
            }
        }

        public SceneMap MapScene
        {
            set
            {
                this.manager.parent = value;
            }
            get
            {
                return this.manager.parent;
            }
        }

        public Battle(
          IAudioEngine s,
          EventManager m,
          int enemy1,
          byte lank1,
          int x1,
          int y1,
          int chip1_1,
          int chip1_2,
          int chip1_3,
          int hp1,
          string name1,
          int enemy2,
          byte lank2,
          int x2,
          int y2,
          int chip2_1,
          int chip2_2,
          int chip2_3,
          int hp2,
          string name2,
          int enemy3,
          byte lank3,
          int x3,
          int y3,
          int chip3_1,
          int chip3_2,
          int chip3_3,
          int hp3,
          string name3,
          Panel.PANEL panel1,
          Panel.PANEL panel2,
          int type,
          bool count,
          bool result,
          bool escape,
          bool gameover,
          string bgm,
          int back,
          SaveData save)
          : base(s, m, save)
        {
            this.enemy[0] = (EnemyBase.VIRUS)enemy1;
            this.enemy[1] = (EnemyBase.VIRUS)enemy2;
            this.enemy[2] = (EnemyBase.VIRUS)enemy3;
            this.lank[0] = lank1;
            this.lank[1] = lank2;
            this.lank[2] = lank3;
            this.position[0].X = x1;
            this.position[1].X = x2;
            this.position[2].X = x3;
            this.position[0].Y = y1;
            this.position[1].Y = y2;
            this.position[2].Y = y3;
            this.chip1[0] = chip1_1;
            this.chip1[1] = chip2_1;
            this.chip1[2] = chip3_1;
            this.chip2[0] = chip1_2;
            this.chip2[1] = chip2_2;
            this.chip2[2] = chip3_2;
            this.chip3[0] = chip1_3;
            this.chip3[1] = chip2_3;
            this.chip3[2] = chip3_3;
            this.hp[0] = hp1;
            this.hp[1] = hp2;
            this.hp[2] = hp3;
            this.name[0] = name1;
            this.name[1] = name2;
            this.name[2] = name3;
            this.panels[0] = panel1;
            this.panels[1] = panel2;
            this.paneltype = type;
            this.countRend = count;
            this.resultRend = result;
            this.canEscape = escape;
            this.gameover = gameover;
            this.bgm = bgm;
            this.back = back;
            this.NoTimeNext = true;
        }

        public Battle(
          IAudioEngine s,
          EventManager m,
          EnemyBase.VIRUS enemy1,
          byte lank1,
          int x1,
          int y1,
          EnemyBase.VIRUS enemy2,
          byte lank2,
          int x2,
          int y2,
          EnemyBase.VIRUS enemy3,
          byte lank3,
          int x3,
          int y3,
          Panel.PANEL panel1,
          Panel.PANEL panel2,
          int type,
          bool count,
          bool result,
          bool escape,
          SaveData save)
          : base(s, m, save)
        {
            this.enemy[0] = enemy1;
            this.enemy[1] = enemy2;
            this.enemy[2] = enemy3;
            this.lank[0] = lank1;
            this.lank[1] = lank2;
            this.lank[2] = lank3;
            this.position[0].X = x1;
            this.position[1].X = x2;
            this.position[2].X = x3;
            this.position[0].Y = y1;
            this.position[1].Y = y2;
            this.position[2].Y = y3;
            this.panels[0] = panel1;
            this.panels[1] = panel2;
            this.paneltype = type;
            this.countRend = count;
            this.resultRend = result;
            this.canEscape = escape;
            this.NoTimeNext = true;
        }

        public override void Update()
        {
            if (this.manager.alpha >= byte.MaxValue)
                this.frame = 60;
            if (this.endflag)
            {
                this.endflag = false;
                this.frame = 0;
                this.EndCommand();
            }
            else
            {
                switch (this.frame)
                {
                    case 0:
                        this.MapScene.fadeColor = Color.White;
                        this.sound.StopBGM();
                        if (this.manager.alpha <= 0)
                            this.sound.PlaySE(SoundEffect.encount);
                        this.manager.canSkip = false;
                        break;
                    case 60:
                        if (this.savedata.FlagList[3])
                        {
                            this.savedata.HPNow = this.savedata.HPMax;
                            this.savedata.mind = 0;
                        }
                        int count = -1;
                        if (this.countRend)
                        {
                            ++this.savedata.ValList[9];
                            count = this.savedata.ValList[9];
                        }
                        this.MainScene.battlescene = new SceneBattle(this.sound, this.MainScene.parent, this.MainScene, new EventManager(this.sound), this.resultRend, count, this.gameover, this.bgm, this.savedata);
                        this.MainScene.battlescene.EnemySet(this.EnemyMake(0), this.EnemyMake(1), this.EnemyMake(2), this.paneltype, this.panels[0], this.panels[1]);
                        this.MainScene.battlescene.SetBack(this.back);
                        this.MapScene.alpha = byte.MaxValue;
                        this.MainScene.parent.battlenum = MapScene.battlecouunt;
                        this.MapScene.main.NowScene = SceneMain.PLAYSCENE.battle;
                        this.MainScene.battlescene.canEscape = this.canEscape;
                        this.MainScene.battlescene.doresult = this.resultRend;
                        this.manager.canSkip = false;
                        this.endflag = true;
                        break;
                }
                if (this.frame < 60)
                {
                    if (MapScene.alpha < (double)byte.MaxValue)
                        this.MapScene.alpha += 15f;
                    ++this.frame;
                }
            }
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }

        public override void SkipUpdate()
        {
            this.Update();
        }

        private EnemyBase EnemyMake(int number)
        {
            EnemyBase e = EnemyBase.EnemyMake((int)this.enemy[number], null, true);
            if (e != null)
            {
                e.Sound = this.sound;
                e.position = this.position[number];
                e.version = this.lank[number];
                int val = this.savedata.ValList[28];
                if (e.version != 0)
                {
                    if (!(e is NormalNavi))
                    {
                        e.version += (byte)val;
                        if (e.version > 8)
                            e.version = 8;
                        if (e.version < 1)
                            e.version = 1;
                    }
                }

                e.number = number;
                e.union = Panel.COLOR.blue;
                if (e is NormalNavi normalNavi)
                {
                    normalNavi.InitialHP = this.hp[number];
                    normalNavi.InitialChips[0] = this.chip1[number];
                    normalNavi.InitialChips[1] = this.chip2[number];
                    normalNavi.InitialChips[2] = this.chip3[number];
                    normalNavi.InitialName = this.name[number];
                    e = normalNavi;
                }

                var battleVersion = e.version;
                // HP-only scaling for SP and normalnavi
                e = EnemyBase.EnemyMake((int)this.enemy[number], e, true);
                if (val != 0 && (battleVersion == 0 || e is NormalNavi))
                {
                    var initialHp = e.HpMax;
                    var rawIncrease = initialHp / 5;
                    var roundingValue = rawIncrease > 100 ? 100.0 : 20.0;
                    var roundedIncrease = (int)(Math.Floor(rawIncrease / roundingValue) * roundingValue);
                    var cappedTotalIncrease = (int)Math.Min(roundedIncrease * val, initialHp * 2);
                    e.HPplus(cappedTotalIncrease);
                }
            }
            return e;
        }
    }
}
