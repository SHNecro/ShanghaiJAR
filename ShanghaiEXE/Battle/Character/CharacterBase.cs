using NSAttack;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSGame;
using NSNet;
using NSObject;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSBattle.Character
{
    public class CharacterBase : AllBase
    {
        public byte alfha = 0;
        public bool rend = true;
        public bool ghostDouble = false;
        public bool chargeBypass = false;
        public bool yuzuriai = false;
        public bool ponkothu = false;
        public bool blackMind = false;
        public bool parizeDamage = false;
        public bool nohit = false;
        public bool[,] hitflag = new bool[6, 3];
        public bool upprint = false;
        public bool downprint = false;
        public bool flag = true;
        public Color color = Color.White;
        public Color mastorcolor = Color.White;
        protected int flamesub = 0;
        public ChipBase.ELEMENT element = ChipBase.ELEMENT.normal;
        public int invincibilitytime = 0;
        public bool standbarrier = false;
        public bool printhp = true;
        public bool[] badstatus = new bool[9];
        public int[] badstatustime = new int[9];
        public bool barrierEX;
        public CharacterBase.SHIELD shield;
        public int shieldtime;
        public int ReflectP;
        public bool shieldUsed;
        public bool canOut;
        public CharacterBase.BARRIER barrierType;
        public int barierTime;
        public int barierPower;
        public bool counterTiming;
        public bool noslip;
        public bool slipping;
        private CharacterBase.DIRECTION slipdirecsion;
        protected bool slipslip;
        protected bool knockslip;
        protected int slipflame;
        public Panel.COLOR union;
        public Point stepPosition;
        public CharacterBase.GUARD guard;
        public int armarCount;
        public string picturename;
        protected int speed;
        public bool rebirth;
        protected bool neutlal;
        protected int mastorflame;
        public bool blackOutObject;
        public CharacterBase.STEP step;
        public Point animationpoint;
        public bool invincibility;
        public CharacterBase.BODY body;
        public int bodytime;
        private const byte badstatuss = 9;
        public int number;
        public Vector2 HPposition;
        protected bool badstatusresist;
        public SceneBattle parent;
        public Point position;
        public Point positionre;
        public Point positionold;
        public Point? positionReserved;
        protected Point positionnow;
        public Vector2 positionDirect;
        public bool flying;
        public int waittime;
        public bool effecting;
        protected HPGauge HP;
        protected bool weakarmor;
        protected int height;
        protected int wide;
        protected Vector2 oldPD;
        private Point positionFirst;
        private float manyslide;
        private bool moveflag;
        private float overrun;
        private int angleold;
        private bool slideBack;
        protected bool slideInit;
        private readonly bool posiChange;
        public int[] randomSeed;
        private int randomSeedUse;
        protected bool overMove;
        private bool nobreak;
        protected bool eturnalMelt;
        private int BlackOutFlame;
        private string blackOutName;
        private string blackOutPower;
        public string powertxt;
        private int nameAlpha;
        private int bariierAnime;
        public bool flyflag;
        public bool rockon;
        public bool rockonMode;
        public Point rockonPosition;
        public Point rockonChipPosition;

        public virtual bool Noslip
        {
            get
            {
                return this.noslip;
            }
            set
            {
                this.noslip = value;
            }
        }

        public ChipBase.ELEMENT Element
        {
            get
            {
                return this.element;
            }
        }

        public bool InArea
        {
            get
            {
                return this.position.X >= 0 && this.position.X < 6 && this.position.Y >= 0 && this.position.Y < 3;
            }
        }

        public bool InAreaCheck(Point poji)
        {
            return poji.X >= 0 && poji.X < 6 && poji.Y >= 0 && poji.Y < 3;
        }

        public int UnionRebirth
        {
            get
            {
                return this.union == Panel.COLOR.red ? 1 : -1;
            }
        }

        public Panel.COLOR UnionEnemy
        {
            get
            {
                return this.union == Panel.COLOR.red ? Panel.COLOR.blue : Panel.COLOR.red;
            }
        }

        public virtual bool Badstatusresist
        {
            get
            {
                return this.badstatusresist;
            }
            set
            {
                this.badstatusresist = value;
            }
        }

        public bool Flying
        {
            set
            {
                this.flying = value;
            }
            get
            {
                return (this.flying || this.barrierType == CharacterBase.BARRIER.FloteBarrier) && !this.badstatus[6];
            }
        }

        protected int hp
        {
            get
            {
                return this.HP.hp;
            }
            set
            {
                this.HP.hp = value;
            }
        }

        public virtual int Hp
        {
            get
            {
                return this.hp;
            }
            set
            {
                if (this.parent.manyenemys <= 0 && !this.parent.blackOut)
                    return;
                if ((this as Player)?.mind.MindNow == MindWindow.MIND.pinch && value > this.hp)
                    (this as Player).mind.MindNow = MindWindow.MIND.normal;

                var antiHealEffects = this.CalculatePoisonEffects();

                if (antiHealEffects.Any(b => b)
                    && value - this.hp > 0
                    && !((this as Player)?.mind.MindNow == MindWindow.MIND.smile))
                {
                    var healAmount = value - this.hp;
                    var multiplier = 1;

                    multiplier *= (int)Math.Pow(2, antiHealEffects.Count(b => b) - 1);

                    switch (this.Element)
                    {
                        case ChipBase.ELEMENT.aqua:
                        case ChipBase.ELEMENT.leaf:
                            multiplier *= 2;
                            break;
                    }
                    if (this.badstatus[(int)ChipBase.ELEMENT.aqua])
                    {
                        multiplier *= 2;
                    }
                    if (this.badstatus[(int)ChipBase.ELEMENT.leaf])
                    {
                        multiplier *= 2;
                    }

                    this.hp -= healAmount * multiplier;
                }
                else
                    this.hp = value;
                if (this.hp < 0)
                    this.hp = 0;
                if (this.hp > this.hpmax)
                    this.hp = this.hpmax;
            }
        }

        public int hpprint
        {
            get
            {
                return this.HP.hpprint;
            }
            protected set
            {
                this.HP.hpprint = value;
            }
        }

        protected int hpmax
        {
            get
            {
                return this.HP.hpmax;
            }
            set
            {
                this.HP.hpmax = value;
            }
        }

        public int HpMax
        {
            get
            {
                return this.hpmax;
            }
        }

        public void HPset(int hpset)
        {
            this.hp = hpset;
            this.hpmax = hpset;
            this.hpprint = hpset;
        }

        public bool WeakArmor
        {
            get
            {
                return this.weakarmor;
            }
        }

        public Panel StandPanel
        {
            get
            {
                if (this.InArea)
                    return this.parent.panel[this.position.X, this.position.Y];
                return new Panel(this.sound, this.parent, this.position.X, this.position.Y);
            }
            set
            {
                try
                {
                    this.parent.panel[this.position.X, this.position.Y] = value;
                }
                catch
                {
                }
            }
        }

        public void HPhalf()
        {
            if (this.hpmax <= 1)
                return;
            this.hpmax /= 2;
            if (this.hp > 1)
                this.hp /= 2;
        }

        public int Height
        {
            get
            {
                return this.height;
            }
        }

        public int Wide
        {
            get
            {
                return this.wide;
            }
        }

        public CharacterBase(IAudioEngine s, SceneBattle p)
          : base(s)
        {
            this.parent = p;
            for (int index = 0; index < this.badstatus.Length; ++index)
            {
                this.badstatus[index] = false;
                this.badstatustime[index] = 0;
            }
            this.HP = new HPGauge(this.sound, 0, 0);
            if (this.parent == null)
                return;
            this.blackOutObject = this.parent.blackOut;
        }

        public virtual void Updata()
        {
            if (this.parent.blackOut)
                return;
            this.Badstasus();
            if (this.barierTime > 0)
            {
                --this.barierTime;
                if (this.barierTime == 0)
                    this.DeleteBarier();
            }
            if (this.shieldtime > 0)
            {
                --this.shieldtime;
                if (this.shieldtime == 0)
                    this.shield = CharacterBase.SHIELD.none;
            }
            if (this.bodytime > 0)
            {
                --this.bodytime;
                if (this.bodytime <= 0)
                {
                    if (this.body == CharacterBase.BODY.Synchro && this is Player)
                    {
                        Player player = (Player)this;
                        if (!player.synchroDamage)
                            player.mind.MindNow = MindWindow.MIND.fullsync;
                    }
                    this.body = CharacterBase.BODY.none;
                }
            }
            ++this.mastorflame;
            try
            {
                var antiHealEffects = this.CalculatePoisonEffects();

                if ((antiHealEffects.Any(b => b)
                    && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                    && !(this is ObjectBase))
                {
                    if (this.mastorflame % 8 == 0)
                    {
                        var multiplier = 1;

                        multiplier *= (int)Math.Pow(2, antiHealEffects.Count(b => b) - 1);

                        switch (this.Element)
                        {
                            case ChipBase.ELEMENT.aqua:
                            case ChipBase.ELEMENT.leaf:
                                multiplier *= 2;
                                break;
                        }
                        if (this.badstatus[(int)ChipBase.ELEMENT.aqua])
                        {
                            multiplier *= 2;
                        }
                        if (this.badstatus[(int)ChipBase.ELEMENT.leaf])
                        {
                            multiplier *= 2;
                        }

                        this.Hp -= 1 * multiplier;
                        this.Dameged(new Dummy(this.sound, this.parent, this.position.X, this.position.Y, this.union, Point.Empty, 0, false));
                    }
                }
            }
            catch
            {
                if (this.position.Y < 0)
                    this.position.Y = 0;
                if (this.position.Y > 2)
                    this.position.Y = 2;
                if (this.position.X < 0)
                    this.position.X = 0;
                if (this.position.X > 5)
                    this.position.X = 5;
                this.PositionDirectSet();
            }
            if (this.InArea
                && this.parent.panel[this.position.X, this.position.Y].state == Panel.PANEL._grass && this.Element == ChipBase.ELEMENT.leaf
                && this.parent.nowscene != SceneBattle.BATTLESCENE.end
                && (this.mastorflame % 6 == 0))
                ++this.Hp;
            if (this.mastorflame % 6 == 0 && this.barrierType == CharacterBase.BARRIER.HealBarrier)
                ++this.Hp;
        }

        public virtual void DeleteBarier()
        {
            this.bariierAnime = 0;
            this.barrierType = CharacterBase.BARRIER.None;
        }

        public virtual void Render(IRenderer dg)
        {
        }

        public virtual void RenderUP(IRenderer dg)
        {
        }

        public virtual void RenderDOWN(IRenderer dg)
        {
        }

        public Point[] RandomMultiPanel(int many, Panel.COLOR union, bool underchara)
        {
            if (this.RandomSeedCanUse())
            {
                List<Point> pointList = new List<Point>();
                List<int> intList = new List<int>();
                List<Vector3> source = new List<Vector3>();
                if (union == Panel.COLOR.blue)
                {
                    for (int x = this.parent.panel.GetLength(0) - 1; x >= 0; --x)
                    {
                        for (int y = 0; y < this.parent.panel.GetLength(1); ++y)
                        {
                            if (this.parent.panel[x, y].color == union)
                            {
                                if (underchara)
                                    source.Add(new Vector3(x, y, this.RandomSeedUse() % 100));
                                else if (!this.parent.OnPanelCheck(x, y, false))
                                    source.Add(new Vector3(x, y, this.RandomSeedUse() % 100));
                            }
                        }
                    }
                }
                else
                {
                    for (int x = 0; x < this.parent.panel.GetLength(0); ++x)
                    {
                        for (int y = 0; y < this.parent.panel.GetLength(1); ++y)
                        {
                            if (this.parent.panel[x, y].color == union)
                            {
                                if (underchara)
                                    source.Add(new Vector3(x, y, this.RandomSeedUse() % 100));
                                else if (!this.parent.OnPanelCheck(x, y, false))
                                    source.Add(new Vector3(x, y, this.RandomSeedUse() % 100));
                            }
                        }
                    }
                }
                List<Vector3> list = source.OrderByDescending<Vector3, float>(n => n.Z).ToList<Vector3>();
                int index = 0;
                while (pointList.Count < many)
                {
                    pointList.Add(new Point((int)list[index].X, (int)list[index].Y));
                    ++index;
                    if (index >= list.Count)
                        index = 0;
                }
                return pointList.ToArray();
            }
            List<Point> source1 = new List<Point>();
            Eriabash.SteelX(this, this.parent);
            while (source1.Count < many && source1.Count < this.parent.panel.Length)
            {
                for (int x = 0; x < this.parent.panel.GetLength(0); ++x)
                {
                    for (int y = 0; y < this.parent.panel.GetLength(1); ++y)
                    {
                        if (this.parent.panel[x, y].color == union)
                            source1.Add(new Point(x, y));
                    }
                }
            }
            List<Point> list1 = source1.OrderBy<Point, Guid>(i => Guid.NewGuid()).ToList<Point>();
            List<Point> pointList1 = new List<Point>();
            for (int index = 0; index < many; ++index)
                pointList1.Add(list1[index]);
            return pointList1.ToArray();
        }

        protected Vector2 OldPD
        {
            get
            {
                return this.oldPD;
            }
            set
            {
                this.positionFirst = this.position;
                this.oldPD = value;
            }
        }

        protected Point Calcposition(Vector2 positionD, int y, bool oldstyle = false)
        {
            if (!oldstyle)
            {
                Point positionFirst = this.positionFirst;
                if (oldPD.X > (double)positionD.X)
                {
                    float num = this.oldPD.X - positionD.X;
                    if (num >= 20.0)
                    {
                        --positionFirst.X;
                        num -= 20f;
                    }
                    positionFirst.X -= (int)(num / 40.0);
                }
                else
                {
                    float num = positionD.X - this.oldPD.X;
                    if (num >= 20.0)
                    {
                        ++positionFirst.X;
                        num -= 20f;
                    }
                    positionFirst.X += (int)(num / 40.0);
                }
                if (oldPD.Y > (double)positionD.Y)
                {
                    float num = this.oldPD.Y - positionD.Y;
                    if (num >= 12.0)
                    {
                        --positionFirst.Y;
                        num -= 12f;
                    }
                    positionFirst.Y -= (int)(num / 24.0);
                }
                else
                {
                    float num = positionD.Y - this.oldPD.Y;
                    if (num >= 12.0)
                    {
                        ++positionFirst.Y;
                        num -= 12f;
                    }
                    positionFirst.Y += (int)(num / 24.0);
                }
                return positionFirst;
            }
            return new Point(0, 0)
            {
                X = (int)positionD.X / 40,
                Y = ((int)positionD.Y + y / 2 - 70) / 24
            };
        }

        public static int SteelX(CharacterBase character, SceneBattle battle)
        {
            int num = 99;
            if (num == 99)
            {
                if (character.union == Panel.COLOR.red)
                {
                    for (int index1 = 0; index1 < battle.panel.GetLength(0); ++index1)
                    {
                        for (int index2 = 0; index2 < battle.panel.GetLength(1); ++index2)
                        {
                            if (!battle.panel[index1, index2].inviolability)
                            {
                                num = index1;
                                break;
                            }
                        }
                        if (num != 99)
                            break;
                    }
                }
                else
                {
                    for (int index1 = battle.panel.GetLength(0) - 1; index1 >= 0; --index1)
                    {
                        for (int index2 = battle.panel.GetLength(1) - 1; index2 >= 0; --index2)
                        {
                            if (!battle.panel[index1, index2].inviolability)
                            {
                                num = index1;
                                break;
                            }
                        }
                        if (num != 99)
                            break;
                    }
                }
            }
            return num;
        }

        protected bool SlideMove(float speed, int angle)
        {
            if (!this.slideInit)
            {
                this.positionre = this.position;
                switch (angle)
                {
                    case 0:
                        this.positionre.X += this.UnionRebirth;
                        break;
                    case 1:
                        this.positionre.X -= this.UnionRebirth;
                        break;
                    case 2:
                        --this.positionre.Y;
                        break;
                    case 3:
                        ++this.positionre.Y;
                        break;
                    default:
                        return true;
                }
                this.slideInit = true;
                if (!this.InAreaCheck(this.positionre))
                {
                    this.positionre = this.position;
                    return false;
                }
            }
            if (!this.slideBack && !this.moveflag)
            {
                bool flag = false;
                if (!this.effecting)
                {
                    if (this is EnemyBase)
                    {
                        if (!this.Canmove(this.positionre, this.number))
                            flag = true;
                    }
                    else if (!this.Canmove(this.positionre))
                        flag = true;
                    if (flag)
                    {
                        this.slideBack = true;
                        this.positionre = this.position;
                        this.manyslide = angle >= 2 ? 24f - this.manyslide : 40f - this.manyslide;
                        switch (angle)
                        {
                            case 0:
                                angle = 1;
                                break;
                            case 1:
                                angle = 0;
                                break;
                            case 2:
                                angle = 3;
                                break;
                            case 3:
                                angle = 2;
                                break;
                        }
                    }
                }
            }
            switch (angle)
            {
                case 0:
                    if (!this.slideBack)
                    {
                        this.positionDirect.X += speed * UnionRebirth;
                        break;
                    }
                    this.positionDirect.X -= speed * UnionRebirth;
                    break;
                case 1:
                    if (!this.slideBack)
                    {
                        this.positionDirect.X -= speed * UnionRebirth;
                        break;
                    }
                    this.positionDirect.X += speed * UnionRebirth;
                    break;
                case 2:
                    if (!this.slideBack)
                    {
                        this.positionDirect.Y -= speed;
                        break;
                    }
                    this.positionDirect.Y += speed;
                    break;
                case 3:
                    if (!this.slideBack)
                    {
                        this.positionDirect.Y += speed;
                        break;
                    }
                    this.positionDirect.Y -= speed;
                    break;
            }
            this.manyslide += speed;
            if (angle < 2)
            {
                if (manyslide >= 20.0 && !this.moveflag)
                {
                    if (angle == 0)
                        this.position.X += this.UnionRebirth;
                    else
                        this.position.X -= this.UnionRebirth;
                    this.moveflag = true;
                }
                if (manyslide >= 40.0 && this.moveflag)
                {
                    this.moveflag = false;
                    this.angleold = angle;
                    this.overrun = this.manyslide - 40f;
                    this.manyslide = 0.0f;
                    this.slideInit = false;
                    this.slideBack = false;
                    return true;
                }
            }
            else
            {
                if (manyslide >= 12.0 && !this.moveflag)
                {
                    if (angle == 2)
                        --this.position.Y;
                    else
                        ++this.position.Y;
                    this.moveflag = true;
                }
                if (manyslide >= 24.0 && this.moveflag)
                {
                    this.overrun = this.manyslide - 24f;
                    this.SlideReset();
                    this.angleold = angle;
                    return true;
                }
            }
            return false;
        }

        protected void SlideMoveEnd()
        {
            switch (this.angleold)
            {
                case 0:
                    this.positionDirect.X -= this.overrun * UnionRebirth;
                    break;
                case 1:
                    this.positionDirect.X += this.overrun * UnionRebirth;
                    break;
                case 2:
                    this.positionDirect.Y += this.overrun;
                    break;
                case 3:
                    this.positionDirect.Y -= this.overrun;
                    break;
            }
        }

        public void SlideReset()
        {
            this.moveflag = false;
            this.manyslide = 0.0f;
            this.slideInit = false;
            this.slideBack = false;
        }

        public bool Canmove(Point gomove)
        {
            if (gomove.X < 0
                || gomove.X >= 6
                || gomove.Y < 0
                || gomove.Y >= 3
                || ((!this.Flying || this.badstatus[6] || this.parent.panel[gomove.X, gomove.Y].state == Panel.PANEL._un)
                    && (this.parent.panel[gomove.X, gomove.Y].Hole)
                    || this.parent.panel[gomove.X, gomove.Y].color != this.union))
                return false;
            bool flag = true;
            if (this.effecting)
                return flag;
            foreach (CharacterBase characterBase in this.parent.AllHitter())
            {
                if ((!this.yuzuriai || characterBase.union != this.union) && (!characterBase.effecting && characterBase != this))
                {
                    if (gomove == characterBase.position)
                        flag = false;
                    if (gomove == characterBase.positionre)
                        flag = false;
                }
            }
            return flag;
        }

        protected bool Canmove(Point gomove, int enemynumber)
        {
            if (gomove.X < 0
                || gomove.X >= 6
                || gomove.Y < 0
                || gomove.Y >= 3
                || ((!this.Flying || this.badstatus[6] || this.parent.panel[gomove.X, gomove.Y].state == Panel.PANEL._un)
                    && (this.parent.panel[gomove.X, gomove.Y].Hole)
                    || this.parent.panel[gomove.X, gomove.Y].color != this.union))
                return false;
            bool flag = true;
            if (this.effecting)
                return flag;
            foreach (CharacterBase characterBase in this.parent.AllHitter())
            {
                if (!characterBase.effecting && characterBase != this && (!this.yuzuriai || characterBase.union != this.union))
                {
                    if (gomove == characterBase.position)
                        flag = false;
                    if (gomove == characterBase.positionre)
                        flag = false;
                }
            }
            return flag;
        }

        protected bool Canmove(Point gomove, int enemynumber, Panel.COLOR uni)
        {
            if (!this.InAreaCheck(new Point(gomove.X, gomove.Y))
                || ((!this.Flying || this.badstatus[6] || this.parent.panel[gomove.X, gomove.Y].state == Panel.PANEL._un)
                    && (this.parent.panel[gomove.X, gomove.Y].Hole)
                    || this.parent.panel[gomove.X, gomove.Y].color != uni))
                return false;
            bool flag = true;
            if (this.effecting)
                return flag;
            foreach (CharacterBase characterBase in this.parent.AllHitter())
            {
                if (!characterBase.effecting && characterBase != this && (!this.yuzuriai || characterBase.union != this.union))
                {
                    if (gomove == characterBase.position)
                        flag = false;
                    if (gomove == characterBase.positionre)
                        flag = false;
                }
            }
            return flag;
        }

        public virtual void InitAfter()
        {
        }

        protected Point RandomTarget()
        {
            return this.RandomTarget(this.union);
        }

        private bool Noobject(Point gomove, int enemynumber)
        {
            bool flag = this is ObjectBase && this.effecting;
            foreach (EnemyBase enemy in this.parent.enemys)
            {
                if ((!flag || enemy.union != this.UnionEnemy) && (!enemy.effecting && enemy != this && enemy.number != enemynumber) && (gomove == enemy.position && !enemy.effecting || gomove == enemy.positionre && !enemy.effecting))
                    return false;
            }
            foreach (ObjectBase objectBase in this.parent.objects)
            {
                if (gomove == objectBase.position && !objectBase.effecting && !this.effecting)
                    return false;
            }
            return (!(gomove == this.parent.player.position) || flag && this.parent.player.union == this.UnionEnemy) && (!(gomove == this.parent.player.positionre) || flag && this.parent.player.union == this.UnionEnemy);
        }

        public int RandomSeedUse()
        {
            if (this.randomSeed == null)
                return this.Random.Next();
            int num = this.randomSeed[this.randomSeedUse];
            this.randomSeed[this.randomSeedUse] *= this.randomSeedUse + 1;
            if (this.randomSeed[this.randomSeedUse] > 100000)
                this.randomSeed[this.randomSeedUse] %= 100000;
            ++this.randomSeedUse;
            if (this.randomSeedUse >= this.randomSeed.Length)
                this.randomSeedUse = 0;
            return num;
        }

        private bool RandomSeedCanUse()
        {
            return this.randomSeed != null;
        }

        protected Point[] RandomMultiPanel(int many, Panel.COLOR union)
        {
            return this.RandomMultiPanel(many, union, true);
        }

        public bool NoObject(Point gomove, int enemynumber)
        {
            return this.Noobject(gomove, enemynumber);
        }

        public bool NoObject(Point gomove)
        {
            return this.Noobject(gomove, -1);
        }

        public virtual void PositionDirectSet()
        {
        }

        protected bool PositionOver(Point po)
        {
            bool flag = false;
            if (po.X < 0)
                flag = true;
            if (po.X > 5)
                flag = true;
            if (po.Y < 0)
                flag = true;
            if (po.Y > 2)
                flag = true;
            return flag;
        }

        protected void MoveAftar()
        {
            if (this.PositionOver(this.position) && !this.overMove)
                this.PositionDirectSet();
            if (this.position.X < 0)
                this.position.X = 0;
            if (this.position.X > 5)
                this.position.X = 5;
            if (this.position.Y < 0)
                this.position.Y = 0;
            if (this.position.Y > 2)
                this.position.Y = 2;
            if (!(this.position != this.positionnow))
                return;
            bool flag = true;
            if (this is Player)
            {
                Player player = (Player)this;
                if ((uint)this.StandPanel.state > 0U)
                    player.PluspointWing(2);
            }
            if (!this.Flying || this.badstatus[6] || this.badstatus[6])
            {
                if (this.parent.panel[this.positionnow.X, this.positionnow.Y].state == Panel.PANEL._crack)
                    this.parent.panel[this.positionnow.X, this.positionnow.Y].Crack();
                if (this.parent.panel[this.position.X, this.position.Y].state == Panel.PANEL._burner && this.Element != ChipBase.ELEMENT.heat)
                    this.parent.panel[this.position.X, this.position.Y].animationON = true;
                if (this.badstatus[6]
                    && !this.parent.panel[this.positionnow.X, this.positionnow.Y].Hole)
                    this.parent.panel[this.positionnow.X, this.positionnow.Y].state = Panel.PANEL._crack;
            }
            this.positionold = this.positionnow;
            this.positionnow = this.position;
            if (!this.Flying & flag && this.parent.panel[this.position.X, this.position.Y].state == Panel.PANEL._sand && this.element != ChipBase.ELEMENT.earth)
            {
                this.badstatus[7] = true;
                this.badstatustime[7] = 30;
                this.parent.effects.Add(new Smoke(this.sound, this.parent, this.position.X, this.position.Y, ChipBase.ELEMENT.earth));
            }
            if (this.SlipFlag())
            {
                float a1 = this.positionold.X - this.position.X;
                float a2 = this.positionold.Y - this.position.Y;
                this.slipdirecsion = MyMath.Abs(a1) <= (double)MyMath.Abs(a2) ? (a2 + (double)this.positionold.Y >= positionold.Y ? CharacterBase.DIRECTION.up : CharacterBase.DIRECTION.down) : (a1 + (double)this.positionold.X <= positionold.X ? CharacterBase.DIRECTION.right : CharacterBase.DIRECTION.left);
                if (this.CanSlip(this.slipdirecsion))
                {
                    this.slipflame = 0;
                    this.slipslip = false;
                    this.slipping = true;
                }
                else
                    this.positionre = this.position;
            }
        }

        private bool SlipFlag()
        {
            this.position = this.PosiInArea(this.position);
            return (this.parent.panel[this.position.X, this.position.Y].state == Panel.PANEL._ice || this.badstatus[2]) && (!this.Flying && this.Element != ChipBase.ELEMENT.aqua) && !this.Noslip;
        }

        protected Point PosiInArea(Point posi)
        {
            if (posi.X < 0)
                posi.X = 0;
            if (posi.X > 5)
                posi.X = 5;
            if (posi.Y < 0)
                posi.Y = 0;
            if (posi.Y > 2)
                posi.Y = 2;
            return posi;
        }
        
        protected virtual void AttackMake(int power, int slideX = 0, int slideY = 0, bool break_ = false)
        {
            if (!this.effecting)
                return;
            EnemyHit enemyHit = new EnemyHit(this.sound, this.parent, this.position.X + slideX, this.position.Y + slideY, this.union, power, this.element, this)
            {
                breaking = break_
            };
            this.parent.attacks.Add(enemyHit);
        }

        protected void Slip(int graphicHeight)
        {
            int angle = 0;
            int num = 0;
            switch (this.slipdirecsion)
            {
                case CharacterBase.DIRECTION.up:
                    angle = 2;
                    num = 6;
                    break;
                case CharacterBase.DIRECTION.down:
                    angle = 3;
                    num = 6;
                    break;
                case CharacterBase.DIRECTION.left:
                    angle = this.union == Panel.COLOR.red ? 1 : 0;
                    num = 10;
                    break;
                case CharacterBase.DIRECTION.right:
                    angle = this.union == Panel.COLOR.red ? 0 : 1;
                    num = 10;
                    break;
            }
            if (this.SlideMove(num, angle))
            {
                this.SlideMoveEnd();
                bool flag = true;
                switch (this.slipdirecsion)
                {
                    case CharacterBase.DIRECTION.up:
                        if (this.position.Y == 0)
                        {
                            flag = false;
                            break;
                        }
                        break;
                    case CharacterBase.DIRECTION.down:
                        if (this.position.Y == 2)
                        {
                            flag = false;
                            break;
                        }
                        break;
                    case CharacterBase.DIRECTION.left:
                        if (this.position.X == 0)
                        {
                            flag = false;
                            break;
                        }
                        break;
                    case CharacterBase.DIRECTION.right:
                        if (this.position.X == 5)
                        {
                            flag = false;
                            break;
                        }
                        break;
                }
                if (this.slipslip || this.SlipFlag())
                {
                    if (this.CanSlip(this.slipdirecsion) & flag)
                    {
                        this.slipflame = 0;
                        this.slipping = true;
                        this.PositionDirectSet();
                    }
                    else
                    {
                        this.slipflame = 0;
                        this.slipping = false;
                        this.slipslip = false;
                        this.knockslip = false;
                        this.positionre = this.position;
                        this.PositionDirectSet();
                    }
                }
                else
                {
                    this.slipping = false;
                    this.knockslip = false;
                    this.position = this.PosiInArea(this.position);
                    this.positionre = this.position;
                    this.PositionDirectSet();
                    this.slipflame = 0;
                }
            }
            this.MoveAftar();
        }

        protected void MoveRandom(bool near, bool target)
        {
            this.MoveRandom(near, target, this.union, false);
        }

        protected void MoveRandom(bool near, bool target, Panel.COLOR uni, bool side)
        {
            if (this.HeviSand)
            {
                this.positionre = this.position;
            }
            else
            {
                List<Point> pointList = new List<Point>();
                if (this.union == Panel.COLOR.blue)
                {
                    for (int x = this.parent.panel.GetLength(0) - 1; x >= 0; --x)
                    {
                        for (int y = 0; y < this.parent.panel.GetLength(1); ++y)
                        {
                            if (this.parent.panel[x, y].color == uni && this.Canmove(new Point(x, y), this.number, uni))
                                pointList.Add(new Point(x, y));
                        }
                    }
                }
                else
                {
                    for (int x = 0; x < this.parent.panel.GetLength(0); ++x)
                    {
                        for (int y = 0; y < this.parent.panel.GetLength(1); ++y)
                        {
                            if (this.parent.panel[x, y].color == uni && this.Canmove(new Point(x, y), this.number, uni))
                                pointList.Add(new Point(x, y));
                        }
                    }
                }
                if (near)
                {
                    int[] n = new int[3];
                    for (int j = 0; j < this.parent.panel.GetLength(1); j++)
                    {
                        n[j] = this.union == Panel.COLOR.red ? 0 : 7;
                        foreach (Point point in pointList)
                        {
                            if (point.Y == j)
                            {
                                if (this.union == Panel.COLOR.blue && n[j] > point.X)
                                    n[j] = point.X;
                                else if (this.union == Panel.COLOR.red && n[j] < point.X)
                                    n[j] = point.X;
                            }
                        }
                        if (this.union == Panel.COLOR.blue)
                            pointList.RemoveAll(c =>
                           {
                               if (c.Y == j)
                                   return c.X > n[j];
                               return false;
                           });
                        else
                            pointList.RemoveAll(c =>
                           {
                               if (c.Y == j)
                                   return c.X < n[j];
                               return false;
                           });
                    }
                }
                if (side)
                    pointList.RemoveAll(c => c.Y == 1);
                if (pointList.Count <= 0)
                    return;
                int index = !this.RandomSeedCanUse() ? this.Random.Next(pointList.Count) : this.RandomSeedUse() % pointList.Count;
                this.positionre = pointList[index];
            }
        }

        protected void MoveRandom(bool near, bool target, Panel.COLOR uni, int seed)
        {
            this.MoveRandom(near, target, uni, false);
        }

        protected bool HeviSand
        {
            get
            {
                return this.badstatus[7] && this.badstatus[6] || this.badstatus[8];
            }
        }

        protected Point RandomTarget(Panel.COLOR union)
        {
            List<Point> pointList = new List<Point>();
            foreach (Player player in this.parent.players)
            {
                if (player.union != union)
                    pointList.Add(player.position);
            }
            foreach (EnemyBase enemy in this.parent.enemys)
            {
                if (enemy.union != union)
                    pointList.Add(enemy.position);
            }
            try
            {
                int index = this.RandomSeedUse() % pointList.Count;
                return pointList[index];
            }
            catch
            {
                return this.RandomPanel(this.UnionEnemy);
            }
        }

        protected Point RandomPanel(Panel.COLOR u)
        {
            return this.RandomMultiPanel(1, u, true)[0];
        }

        private bool CanSlip(CharacterBase.DIRECTION d)
        {
            switch (d)
            {
                case CharacterBase.DIRECTION.up:
                    this.positionre = new Point(this.position.X, this.position.Y - 1);
                    break;
                case CharacterBase.DIRECTION.down:
                    this.positionre = new Point(this.position.X, this.position.Y + 1);
                    break;
                case CharacterBase.DIRECTION.left:
                    this.positionre = new Point(this.position.X - 1, this.position.Y);
                    break;
                case CharacterBase.DIRECTION.right:
                    this.positionre = new Point(this.position.X + 1, this.position.Y);
                    break;
            }
            if (!this.InAreaCheck(this.positionre))
                return false;
            if (this is ObjectBase && ((ObjectBase)this).hitPower > 0)
                this.effecting = true;
            if (this is ObjectBase)
                return this.NoObject(this.positionre);
            if (this.number < 0)
                return this.Canmove(this.positionre);
            return this.Canmove(this.positionre, this.number);
        }

        public void HitFlagReset()
        {
            this.hitflag = new bool[6, 3];
		}

		public void Knockbuck(bool push, bool slip, Panel.COLOR attackunion)
		{
			var direction = attackunion != Panel.COLOR.blue ? (!push ? CharacterBase.DIRECTION.left : CharacterBase.DIRECTION.right) : (!push ? CharacterBase.DIRECTION.right : CharacterBase.DIRECTION.left);
			this.Knockbuck(direction, slip, attackunion);
		}

		public void Knockbuck(DIRECTION direction, bool slip, Panel.COLOR attackunion)
		{
			if (this.slipping)
				return;
			if (this is ObjectBase && ((ObjectBase)this).unionhit)
				this.union = attackunion;
			if (!this.Noslip)
			{
				this.slipdirecsion = direction;
				if (this.CanSlip(this.slipdirecsion))
				{
					this.slipping = true;
					this.knockslip = true;
					this.nobreak = true;
					this.slipslip = slip;
				}
				else
				{
					this.nobreak = false;
					this.positionre = this.position;
				}
			}
		}

		public virtual void Dameged(AttackBase attack)
        {
        }

        public virtual void NoDameged(AttackBase attack)
        {
        }

        protected void Badstasus()
        {
            Color color = Color.White;
            if (this.body == CharacterBase.BODY.Shadow)
                color = Color.FromArgb(32, 32, 32);
            if (this.body == CharacterBase.BODY.Synchro)
                color = Color.FromArgb(0, byte.MaxValue, byte.MaxValue);
            if (this.mastorcolor != color)
                this.mastorcolor = color;
            if (this.badstatus[1])
            {
                if (this.badstatustime[1] == 0 && !this.eturnalMelt)
                {
                    this.badstatus[1] = false;
                    this.mastorcolor = Color.White;
                }
                else
                {
                    --this.badstatustime[1];
                    this.mastorcolor = this.badstatustime[1] % 3 != 0 ? color : Color.FromArgb(byte.MaxValue, byte.MaxValue, 55, 55);
                }
            }
            if (this.badstatus[2])
            {
                if (this.badstatustime[2] == 0)
                {
                    this.badstatus[2] = false;
                    this.mastorcolor = color;
                }
                else
                {
                    --this.badstatustime[2];
                    this.mastorcolor = this.badstatustime[2] % 3 != 0 ? color : Color.FromArgb(byte.MaxValue, 50, 155, byte.MaxValue);
                }
            }
            if (this.badstatus[3])
            {
                if (this.badstatustime[3] == 0)
                {
                    this.badstatus[3] = false;
                    this.mastorcolor = color;
                }
                else
                {
                    --this.badstatustime[3];
                    this.mastorcolor = this.badstatustime[3] % 3 != 0 ? color : Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                }
            }
            if (this.badstatus[4])
            {
                if (this.badstatustime[4] == 0)
                {
                    this.badstatus[4] = false;
                    this.mastorcolor = color;
                }
                else
                {
                    --this.badstatustime[4];
                    this.mastorcolor = this.badstatustime[4] % 3 != 0 ? color : Color.FromArgb(byte.MaxValue, 155, byte.MaxValue, 55);
                }
            }
            if (this.badstatus[5])
            {
                if (this.badstatustime[5] == 0)
                {
                    this.badstatus[5] = false;
                    this.mastorcolor = color;
                }
                else
                {
                    --this.badstatustime[5];
                    this.mastorcolor = this.badstatustime[5] % 3 != 0 ? color : Color.FromArgb(byte.MaxValue, 100, 50, byte.MaxValue);
                }
            }
            if (this.badstatus[6])
            {
                if (this.badstatustime[6] == 0)
                {
                    this.badstatus[6] = false;
                    this.mastorcolor = color;
                }
                else
                {
                    --this.badstatustime[6];
                    this.mastorcolor = this.badstatustime[6] % 3 != 0 ? color : Color.FromArgb(byte.MaxValue, 160, 100, 50);
                }
            }
            if (this.badstatus[7])
            {
                var nonSandPit = this.badstatustime[6] <= 0;
                var forceEndSandPit = this.StandPanel.State != Panel.PANEL._sand;
                if (!nonSandPit && forceEndSandPit)
                {
                    this.badstatustime[7] = 0;
                }

                if (forceEndSandPit || nonSandPit)
                {
                    if (this.badstatustime[7] == 0)
                    {
                        this.badstatus[7] = false;
                        this.mastorcolor = color;
                    }
                    else if (this.badstatustime[7] > 0)
                        --this.badstatustime[7];
                }
            }
            if (this.badstatus[8])
            {
                if (this.badstatustime[8] == 0)
                {
                    this.badstatus[8] = false;
                    this.mastorcolor = color;
                }
                else
                {
                    --this.badstatustime[8];
                    this.mastorcolor = this.badstatustime[8] % 3 != 0 ? color : Color.FromArgb(byte.MaxValue, 0, 0, 0);
                }
            }
            if (!this.counterTiming || this.parent.mind.MindNow != MindWindow.MIND.fullsync || this.frame % 2 != 0)
                return;
            this.mastorcolor = Color.FromArgb(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
        }

        protected bool OverPosition(Point p)
        {
            return p.X >= 0 && p.X < 6 && p.Y >= 0 && p.Y < 3;
        }

        protected int[] HPcount(int hp)
        {
            int length = hp.ToString().Length;
            int[] numArray = new int[length];
            for (int b = 0; b < length; ++b)
            {
                int num = (int)MyMath.Pow(10f, b);
                numArray[b] = hp / num % 10;
            }
            return numArray;
        }

        protected void HPRender(IRenderer dg, bool print)
        {
            if (!print)
                return;
            int[] numArray = this.HPcount(this.hpprint);
            Color color = this.hp != this.hpprint ? Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, 200) : Color.White;
            Vector2 vector2 = new Vector2((int)this.positionDirect.X, (int)this.positionDirect.Y + this._rect.Height / 2);
            for (int index = 0; index < numArray.Length; ++index)
            {
                this._rect = new Rectangle(numArray[index] * 8, 72, 8, 10);
                double num1 = vector2.X - (double)(index * 8) + (numArray.Length - 1) * 4;
                Point shake = this.Shake;
                double x = shake.X;
                double num2 = num1 + x;
                double num3 = vector2.Y + 2.0;
                shake = this.Shake;
                double y = shake.Y;
                double num4 = num3 + y;
                this._position = new Vector2((float)num2, (float)num4);
                dg.DrawImage(dg, "font", this._rect, false, this._position, color);
            }
        }

        protected void HPRender(IRenderer dg, Vector2 p, bool print)
        {
            if (!print || (uint)this.parent.nowscene <= 0U)
                return;
            int[] numArray = this.HPcount(this.hpprint);
            Color color = this.hp != this.hpprint ? Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, 200) : Color.White;
            for (int index = 0; index < numArray.Length; ++index)
            {
                this._rect = new Rectangle(numArray[index] * 8, 72, 8, 10);
                double num1 = p.X - (double)(index * 8) + 6.0;
                Point shake = this.Shake;
                double x = shake.X;
                double num2 = num1 + x;
                double num3 = p.Y + 2.0;
                shake = this.Shake;
                double y = shake.Y;
                double num4 = num3 + y;
                this._position = new Vector2((float)num2, (float)num4);
                dg.DrawImage(dg, "font", this._rect, false, this._position, color);
            }
        }

        public void HPRender(IRenderer dg)
        {
            if (!this.printhp || (uint)this.parent.nowscene <= 0U)
                return;
            int[] numArray = this.HPcount(this.hpprint);
            Color color = this.hp != this.hpprint ? Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, 200) : Color.White;
            for (int index = 0; index < numArray.Length; ++index)
            {
                this._rect = new Rectangle(numArray[index] * 8, 72, 8, 10);
                double num1 = HPposition.X - (double)(index * 8) + 6.0;
                Point shake = this.Shake;
                double x = shake.X;
                double num2 = num1 + x;
                double num3 = HPposition.Y + 2.0;
                shake = this.Shake;
                double y = shake.Y;
                double num4 = num3 + y;
                this._position = new Vector2((float)num2, (float)num4);
                dg.DrawImage(dg, "font", this._rect, false, this._position, color);
            }
        }

        protected void HPRender(IRenderer dg, Vector2 p)
        {
            this.HP.HPRender(dg, p);
        }

        public void HPDown()
        {
            this.HP.HPDown(this.hp, this.hpmax);
        }

        public virtual void BarrierRend(IRenderer dg)
        {
        }

        public void BarrierRender(IRenderer dg, Vector2 p)
        {
            if ((uint)this.barrierType <= 0U)
                return;
            this._rect = new Rectangle(0, (int)(this.barrierType - 1) * 56, 56, 56);
            this._position = p;
            if (this.barrierType < CharacterBase.BARRIER.PowerAura)
            {
                ++this.bariierAnime;
                if (this.bariierAnime >= 4)
                    this.bariierAnime = 0;
                if (this.bariierAnime < 2)
                    dg.DrawImage(dg, "barrier", this._rect, false, this._position, this.color);
            }
            else if (this.barierTime > 180 || this.barierTime % 4 < 2)
            {
                int num = 4;
                ++this.bariierAnime;
                if (this.bariierAnime >= num * 3)
                    this.bariierAnime = 0;
                this._rect = new Rectangle(56 * (this.bariierAnime / num), (int)(this.barrierType - 1) * 56, 56, 56);
                if (this.barierPower >= 200)
                    this._rect.Y = 336;
                dg.DrawImage(dg, "barrier", this._rect, false, this._position, this.color);
            }
        }

        public virtual void BarrierPowerRend(IRenderer dg)
        {
            if (this.barrierType < CharacterBase.BARRIER.PowerAura && this.barierPower <= 1 || this.barrierEX)
                return;
            this.BarrierPowerRender(dg, this.positionDirect);
        }

        public void BarrierPowerRender(IRenderer dg, Vector2 p)
        {
            if (this.barrierType != CharacterBase.BARRIER.PowerAura && (this.barierPower <= 1 || (this.barrierType >= CharacterBase.BARRIER.PowerAura || (uint)this.barrierType <= 0U)))
                return;
            string text = this.barierPower.ToString();
            Vector2 vector2 = new Vector2(p.X - 8f - 8 * (text.Length % 2), p.Y + 16f);
            this._position = new Vector2(vector2.X - 1f, vector2.Y - 1f);
            dg.DrawText(text, this._position, Color.FromArgb(32, 32, 32));
            this._position = new Vector2(vector2.X - 1f, vector2.Y);
            dg.DrawText(text, this._position, Color.FromArgb(32, 32, 32));
            this._position = new Vector2(vector2.X - 1f, vector2.Y + 1f);
            dg.DrawText(text, this._position, Color.FromArgb(32, 32, 32));
            this._position = new Vector2(vector2.X, vector2.Y - 1f);
            dg.DrawText(text, this._position, Color.FromArgb(32, 32, 32));
            this._position = new Vector2(vector2.X, vector2.Y + 1f);
            dg.DrawText(text, this._position, Color.FromArgb(32, 32, 32));
            this._position = new Vector2(vector2.X + 1f, vector2.Y - 1f);
            dg.DrawText(text, this._position, Color.FromArgb(32, 32, 32));
            this._position = new Vector2(vector2.X + 1f, vector2.Y);
            dg.DrawText(text, this._position, Color.FromArgb(32, 32, 32));
            this._position = new Vector2(vector2.X + 1f, vector2.Y + 1f);
            dg.DrawText(text, this._position, Color.FromArgb(32, 32, 32));
            this._position = vector2;
            dg.DrawText(text, this._position, Color.White);
        }

        public bool BlackOut(CharacterBase character, SceneBattle parent, string name, string power)
        {
            if (this.BlackOutFlame > 60)
                return true;
            if (!parent.blackOut)
            {
                this.blackOutName = name;
                this.blackOutPower = !(power != "0") ? "" : power;
                character.blackOutObject = true;
                parent.blackOut = true;
                parent.blackOuter = character.union;
                parent.backscreencolor = 0;
                parent.backscreen = 0;
                this.nameAlpha = 0;
            }
            if (parent.backscreen < 100)
                parent.backscreen += 10;
            if (this.nameAlpha < byte.MaxValue && this.BlackOutFlame >= 10 && this.BlackOutFlame < 20)
                this.nameAlpha += 51;
            if (this.BlackOutFlame == 5)
                this.sound.PlaySE(SoundEffect.barrier);
            if (this.nameAlpha > 0 && this.BlackOutFlame >= 50)
                this.nameAlpha -= 51;
            ++this.BlackOutFlame;
            if (this.BlackOutFlame > 60)
                character.waittime = -1;
            return false;
        }

        public bool BlackOutEnd(CharacterBase character, SceneBattle parent)
        {
            if (character.animationpoint.X < 0)
            {
                character.animationpoint.X = 0;
                this.nameAlpha = 0;
            }
            if (parent.backscreen > 0)
            {
                parent.backscreen -= 10;
                if (parent.backscreen < 0)
                    parent.backscreen = 0;
                return false;
            }
            this.BlackOutFlame = 0;
            character.blackOutObject = false;
            parent.blackOut = false;
            return true;
        }

        public void BlackOutRender(IRenderer dg, Panel.COLOR uni)
        {
            if (this.nameAlpha > byte.MaxValue)
                this.nameAlpha = byte.MaxValue;
            if (this.nameAlpha < 0)
                this.nameAlpha = 0;
            if (this.nameAlpha <= 0)
                return;
            int length = this.blackOutName.Length;
            this._position = new Vector2(8f, 32f);
            if (uni == Panel.COLOR.blue)
                this._position.X += 120f;
            this.TextRender(dg, this.blackOutName, false, this._position, true, Color.FromArgb(this.nameAlpha, Color.White));
            this._position = new Vector2(24 + length * 8, 32f);
            if (uni == Panel.COLOR.blue)
                this._position.X += 120f;
            this.TextRender(dg, this.blackOutPower, false, this._position, true, Color.FromArgb(this.nameAlpha, byte.MaxValue, 230, 30));
        }

        public bool Step()
        {
            Point point = new Point(this.position.X + 2 * this.UnionRebirth, this.position.Y);
            if (!this.InAreaCheck(point) || !this.NoObject(point))
                return false;
            this.flyflag = this.flying;
            this.flying = true;
            this.parent.effects.Add(new StepShadow(this.sound, this.parent, new Rectangle(this.animationpoint.X * this.Wide, this.animationpoint.Y * this.Height, this.Wide, this.Height), this.positionDirect, this.picturename, this.union == Panel.COLOR.blue, this.position));
            this.stepPosition = this.position;
            this.position = point;
            this.PositionDirectSet();
            if (this.body == CharacterBase.BODY.Shadow)
            {
                this.step = CharacterBase.STEP.shadow;
                this.nohit = true;
            }
            else
                this.step = CharacterBase.STEP.step;
            return true;
        }

        public bool RockonStep()
        {
            Point point = new Point(this.rockonPosition.X + this.rockonChipPosition.X, this.rockonPosition.Y + this.rockonChipPosition.Y);
            if (!this.InAreaCheck(point) || !this.NoObject(point))
                return false;
            this.flyflag = this.flying;
            this.flying = true;
            this.parent.effects.Add(new StepShadow(this.sound, this.parent, new Rectangle(this.animationpoint.X * this.Wide, this.animationpoint.Y * this.Height, this.Wide, this.Height), this.positionDirect, this.picturename, this.union == Panel.COLOR.blue, this.position));
            this.stepPosition = this.position;
            this.position = point;
            this.PositionDirectSet();
            if (this.body == CharacterBase.BODY.Shadow)
            {
                this.step = CharacterBase.STEP.shadow;
                this.nohit = true;
            }
            else
                this.step = CharacterBase.STEP.step;
            return true;
        }

        protected void FlameControl()
        {
            ++this.flamesub;
            if (this.flamesub >= this.speed)
            {
                ++this.frame;
                this.moveflame = true;
                this.flamesub = 0;
            }
            else
                this.moveflame = false;
        }

        private bool[] CalculatePoisonEffects()
        {
            var acidBody = this.badstatustime[5] < 0;
            var poisonImmune = this.element == ChipBase.ELEMENT.poison;
            var poisoned = this.badstatus[5] && !acidBody && !poisonImmune;
            var poisonPanelEffect = this.StandPanel.State == Panel.PANEL._poison && !this.Flying && !poisonImmune;
            var antiHealEffects = new[] { poisoned, poisonPanelEffect, acidBody };

            return antiHealEffects;
        }

        public enum BADSTATUS
        {
            none,
            melt,
            slip,
            paralyze,
            blind,
            poison,
            heavy,
            sandwait,
            stop,
        }

        public enum DIRECTION
        {
            up,
            down,
            left,
            right,
        }

        public enum BARRIER
        {
            None,
            Barrier,
            HealBarrier,
            FloteBarrier,
            PowerAura,
            ElementsAura,
            MetalAura,
        }

        public enum BODY
        {
            none,
            Shadow,
            Synchro,
        }

        public enum SHIELD
        {
            none,
            Reflect,
            ReflectP,
            Repair,
            Normal,
        }

        public enum STEP
        {
            none,
            step,
            shadow,
        }

        public enum GUARD
        {
            none,
            guard,
            armar,
            Sarmar,
            noDamage,
        }
    }
}
