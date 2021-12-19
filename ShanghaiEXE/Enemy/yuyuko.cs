using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

using NSEffect;


namespace NSEnemy
{
    internal class yuyuko : NaviBase
    {
        private readonly int[] powers = new int[4] { 20, 10, 30, 0 };
        private readonly int nspeed = 2;
        private readonly int moveroop;
        private yuyuko.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private int atackroop;
        private int aspeed;
        private int attackspeed;

        private int preX;
        private readonly bool atack;

        private int count;
        private int attackCount;
        private int flyflame;
        private readonly int time;
        private Vector2 endposition;
        private float movex;
        private float movey;
        private float plusy;
        private float speedy;
        private float plusing;
        private const int startspeed = 6;
        private DammyEnemy dammy;
        private yuyuMine[] mineList;
        private bool ready;
        private Charge chargeEffect;
        private int attackProcess;
        private int attackroop;

        private int attackMode;
        protected int[] targetY;
        protected int hit = 5;//3;
        private int atacks;
        private bool fan = false;

        public yuyuko(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.normal;
            this.nspeed = 3;
            attackspeed = 3;
            //this.time = 20;
            this.time = 10;
            this.targetY = new int[21];

            this.aspeed = 4;

            if (this.version != 1)
            {
                this.version = 4;
            }

            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.YuyukoName1");
                    this.power = 200;
                    this.hp = 3000;
                    this.moveroop = 1;
                    break;
                case 0:
                default:
                    this.nspeed = 2;
                    this.name = ShanghaiEXE.Translate("Enemy.YuyukoName2");
                    this.power = 250;
                    this.hp = 4000;
                    this.moveroop = 2;
                    this.aspeed = 3;
                    attackspeed = 2;
                    break;
            }
            this.picturename = "yuyuko";
            this.race = EnemyBase.ENEMY.navi;
            this.Flying = true;
            this.wide = 80;
            this.height = 80;
            this.hpmax = this.hp;
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.roopmove = n * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                default:
                    this.dropchips[0].chip = new YoumuV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new YoumuV1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new YoumuV1(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new YoumuV1(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new YoumuV1(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 100000;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 16.0), (float)(position.Y * 24.0 + 58.0));
        }

        public override void InitAfter()
        {
            base.InitAfter();

            this.parent.resultError = !this.parent.parent.savedata.FlagList[894];
        }

        protected override void Moving()
        {
            // shadow
            //this.ShadowYuyu();


            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
                    if (this.moveflame)
                        ++this.waittime;

                    this.animationpoint = this.AnimeNeutral(this.waittime);

                    if (this.moveflame)
                    {

                        this.animationpoint = this.AnimeNeutral(this.waittime);

                        if (this.waittime >= 8 / version || this.atack)
                        {
                            this.waittime = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (this.roopmove > this.moveroop && !this.badstatus[4])
                                {
                                    this.roopmove = this.version > 0 ? this.Random.Next(-1, this.moveroop + 1) : 0;
                                    ++this.atackroop;
                                    this.speed = 1;
                                    if (!this.atack)
                                    {
                                        //int index = this.Random.Next(this.version > 0 ? 3 : 1);
                                        int randMax = 4;
                                        int index = this.Random.Next(randMax);

                                        this.attack = (yuyuko.ATTACK)index;
                                        //this.attack = yuyuko.ATTACK.butterflyAirhoc;
                                        //this.attack = yuyuko.ATTACK.yuyuMine;
                                        this.powerPlus = this.powers[index];

                                        int mineLim = 2;
                                        if (this.version == 1)
                                        {
                                            mineLim--;
                                        }
                                        mineLim = 1;
                                        if (this.attack == yuyuko.ATTACK.yuyuMine)
                                        {
                                            int mineC = 0;
                                            for (int indec = 0; indec < this.parent.attacks.Count; ++indec)
                                            {
                                                if (this.parent.attacks[indec] is yuyuMine)
                                                {
                                                    if (this.version == 1)
                                                    {
                                                        this.attack = yuyuko.ATTACK.butterflyStraight;
                                                        index = this.parent.attacks.Count;
                                                    }

                                                    mineC++;
                                                    if (mineC >= mineLim)
                                                    {
                                                        this.attack = yuyuko.ATTACK.butterflyStraight;
                                                        index = this.parent.attacks.Count;

                                                    }
                                                    
                                                }
                                            }

                                        }

                                        //this.attack = yuyuko.ATTACK.yuyuFanBarrage;

                                    }
                                    this.waittime = 0;
                                    this.Motion = NaviBase.MOTION.attack;
                                    this.ready = false;
                                    this.attackProcess = 0;
                                    this.attackMode = 0;
                                    this.atacks = 0;

                                    this.counterTiming = true;
                                }
                                else
                                {
                                    this.waittime = 0;
                                    if (this.atack)
                                        this.roopmove = this.moveroop + 1;

                                    this.fan = false;
                                    this.Motion = NaviBase.MOTION.move;
                                    // implement not quite serenade/bass movement?
                                    // twitter mp4s have her move only on one axis at a time

                                    /*this.MoveRandom(false, false, this.union, 0);
                                    if (this.positionre == this.position)
                                    {
                                        this.MoveRandom(false, false, this.union, 0);
                                    }*/

                                    if (!this.HeviSand)
                                    {
                                        var remainingMovementTries = 18;
                                        while (remainingMovementTries > 0 && this.positionre == this.position)
                                        {
                                            this.MoveRandom(false, false);
                                            remainingMovementTries--;
                                        }
                                    }
                                    //this.MoveRandom(false, false, this.union, 0);
                                    //this.MoveRandom(false, false);
                                    //this.slideX = this.positionre.X;
                                    //this.slideY = this.positionre.Y;

                                    //this.positionre = this.position;
                                    this.Throw();
                                    //this.Shadow();

                                    /*
                                    this.attack = yuyuko.ATTACK.butterflyStraight;
                                    this.waittime = 0;
                                    this.Motion = NaviBase.MOTION.attack;
                                    */

                                }
                            }
                        }

                    }

                        
                    break;

                case NaviBase.MOTION.attack:
                    if (this.moveflame)
                    {
                        ++this.waittime;
                        if (this.moveflame)
                        {
                            switch (this.attack)
                            {
                                case yuyuko.ATTACK.butterflyStraight:

                                    this.animationpoint = this.AnimeButterfly(this.waittime);
                                    //int aTime = this.aspeed * 2;
                                    switch (this.waittime)
                                    {
                                        case 8:
                                            this.counterTiming = false;
                                            int butMov = 8;
                                            //butMov = 1;
                                            butMov = 4;
                                            if (this.version == 1)
                                            {
                                                butMov = 3;
                                            }

                                            this.sound.PlaySE(SoundEffect.rockopen);
                                            this.parent.attacks.Add(new Butterfly(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 2, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, butMov));

                                            break;

                                        case 48:

                                            butMov = 7;
                                            this.sound.PlaySE(SoundEffect.rockopen);
                                            this.parent.attacks.Add(new Butterfly(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 2, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, butMov));

                                            break;
                                        case 20:

                                            this.roopneutral = 0;
                                            this.Motion = NaviBase.MOTION.neutral;
                                            if (!this.atack)
                                            {
                                                this.speed = this.nspeed;
                                                break;
                                            }
                                            break;
                                    }
                                    /*
                                    this.animationpoint = this.AnimeButterfly(this.waittime);
                                    //int aTime = this.aspeed * 2;
                                    switch (this.waittime)
                                    {
                                        case 8:
                                            this.counterTiming = false;
                                            int butMov = 8;
                                            butMov = 1;
                                            this.sound.PlaySE(SoundEffect.rockopen);
                                            this.parent.attacks.Add(new Butterfly(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 2, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, butMov));

                                            break;

                                        case 48:
                                            
                                            butMov = 7;
                                            this.sound.PlaySE(SoundEffect.rockopen);
                                            this.parent.attacks.Add(new Butterfly(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 2, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, butMov));

                                            break;
                                        case 60:

                                            this.roopneutral = 0;
                                            this.Motion = NaviBase.MOTION.neutral;
                                            if (!this.atack)
                                            {
                                                this.speed = this.nspeed;
                                                break;
                                            }
                                            break;
                                    }*/




                                    break;
                                case yuyuko.ATTACK.butterflyAirhoc:

                                    this.animationpoint = this.AnimeButterfly2(this.waittime);
                                    //int aTime = this.aspeed * 2;
                                    switch (this.waittime)
                                    {
                                        case 8:
                                            this.counterTiming = false;
                                            int butMov = 8;

                                            break;

                                        //case 16:
                                        case 12:
                                            butMov = 7;
                                            this.sound.PlaySE(SoundEffect.rockopen);
                                            int s = 2;
                                            int aS = 3;
                                            int neg = 1;
                                            if (this.position.X == 5) { neg = -1; }
                                            if (this.version == 1) { aS = 4; }

                                            if (this.position.Y == 1)
                                            {
                                                this.parent.attacks.Add(new ButterflyHoc(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, s, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, false, aS));
                                                this.parent.attacks.Add(new ButterflyHoc(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, s, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, true, aS));


                                            }

                                            if (this.position.Y == 0)
                                            {
                                                this.parent.attacks.Add(new ButterflyHoc(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, s, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, false, aS));
                                                this.parent.attacks.Add(new ButterflyHoc(this.sound, this.parent, this.position.X + 1 * neg, this.position.Y, this.union, this.Power, s, new Vector2(this.positionDirect.X + 40 * neg, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, false, aS));


                                            }

                                            if (this.position.Y == 2)
                                            {
                                                this.parent.attacks.Add(new ButterflyHoc(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, s, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, true, aS));
                                                this.parent.attacks.Add(new ButterflyHoc(this.sound, this.parent, this.position.X + 1 * neg, this.position.Y, this.union, this.Power, s, new Vector2(this.positionDirect.X + 40 * neg, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, true, aS));


                                            }

                                            //this.parent.attacks.Add(new Butterfly(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 2, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, butMov));

                                            break;

                                        /*
                                    case 16:
                                        butMov = 7;

                                        s = 2;
                                        aS = 3;
                                        if (this.version == 1) { aS = 4; }


                                        if (this.position.Y == 0)
                                        {
                                            this.parent.attacks.Add(new ButterflyHoc(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, s, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, false, aS));
                                            this.sound.PlaySE(SoundEffect.rockopen);

                                        }

                                        if (this.position.Y == 2)
                                        {
                                            this.parent.attacks.Add(new ButterflyHoc(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, s, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, true, aS));
                                            this.sound.PlaySE(SoundEffect.rockopen);

                                        }

                                        //this.parent.attacks.Add(new Butterfly(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 2, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, butMov));

                                        break;

                                        */
                                        case 17:

                                            this.roopneutral = 0;
                                            this.Motion = NaviBase.MOTION.neutral;
                                            if (!this.atack)
                                            {
                                                this.speed = this.nspeed;
                                                break;
                                            }
                                            break;
                                    }

                                    break;

                                case yuyuko.ATTACK.yuyuMine:

                                    // literally the mine enemy/chip but with different graphics
                                    /*
									this.sound.PlaySE(SoundEffect.enterenemy);
									battle.attacks.Add(this.Paralyze(new Brocla(this.sound, battle, point.X, point.Y, character.union, this.Power(character), Panel.PANEL._crack, true, this.element)));
									*/

                                    this.animationpoint = this.AnimeMine(this.waittime);
                                    //int aTime = this.aspeed * 2;
                                    switch (this.waittime)
                                    {
                                        case 8:
                                            this.counterTiming = false;
                                            int butMov = 8;

                                            break;

                                        case 20:

                                            butMov = 7;

                                            this.MoveRandom(false, false, this.union == Panel.COLOR.red ? Panel.COLOR.blue : Panel.COLOR.red, 0);
                                            Point point = this.positionre;
                                            this.positionre = this.position;

                                            this.sound.PlaySE(SoundEffect.rockopen);
                                            yuyuMine mine = new yuyuMine(this.sound, this.parent, point.X, point.Y, this.union, this.Power, Panel.PANEL._crack, false, this.element);
                                            this.parent.attacks.Add(mine);
                                            //this.mineList[this.mineList.Length+1] = mine;
                                            break;
                                        case 21:

                                            this.roopneutral = 0;
                                            this.Motion = NaviBase.MOTION.neutral;
                                            if (!this.atack)
                                            {
                                                this.speed = this.nspeed;
                                                break;
                                            }
                                            break;
                                    }

                                    break;

                                case yuyuko.ATTACK.yuyuFanBarrage:

                                    // code from Ran's barrage? or her chip
                                    // needs significant improvement
                                    int lim = 12;
                                    if (this.fan)
                                    {
                                        this.attackMode = 1;
                                        this.fan = false;
                                    }
                                    switch (this.attackMode)
                                    {
                                        case 0:
                                            
                                            
                                            switch (this.waittime)
                                            {
                                                default:

                                                    this.positionre = new Point(this.union == Panel.COLOR.blue ? 5 : 0, 1);
                                                    this.counterTiming = false;
                                                    

                                                    this.waittime = 0;
                                                    

                                                    this.fan = true;
                                                    this.Motion = NaviBase.MOTION.move;
                                                    this.Throw();
                                                    break;
                                                
                                            }
                                            

                                            break;

                                        case 1:
                                            this.animationpoint = this.AnimeFanBarrageStart(this.waittime);
                                            switch (this.waittime)
                                            {
                                                case 0:

                                                    break;

                                                case 12:
                                                    this.waittime = 0;
                                                    ++this.attackMode;
                                                    break;
                                            }

                                            break;
                                        case 2:
                                            this.animationpoint = this.AnimeFanBarrage(this.waittime);
                                            switch (this.waittime)
                                            {
                                                case 0:

                                                    break;

                                                case 9:

                                                    //this.sound.PlaySE(SoundEffect.gun);
                                                    int num = this.Random.Next(2);
                                                    if (num == 1)
                                                        num = 2;
                                                    /*
                                                    BustorShot bustorShot2 = new BustorShot(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.Random.Next(100) < 60 ? this.Random.Next(3) : num, this.union, this.Power, BustorShot.SHOT.ranShot, this.element, false, 6)
                                                    {
                                                        blackOutObject = false
                                                    };
                                                    this.parent.attacks.Add(bustorShot2);
                                                    */
                                                    int butMov = 10;
                                                    if (this.version == 1) { butMov = 8; }
                                                    this.sound.PlaySE(SoundEffect.rockopen);
                                                    yuyuFireball fireball1 = new yuyuFireball(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.Random.Next(100) < 60 ? this.Random.Next(3) : num, this.union, this.Power, 2, new Vector2(this.positionDirect.X, this.positionDirect.Y + 0f), ChipBase.ELEMENT.normal, butMov)
                                                    {
                                                        blackOutObject = false
                                                    };

                                                    this.parent.attacks.Add(fireball1);


                                                    break;

                                                case 18:
                                                    ++this.atacks;
                                                    this.waittime = 0;
                                                    if (this.atacks > lim)
                                                    {
                                                        ++this.attackMode;
                                                    }

                                                    break;
                                            }
                                            break;
                                        case 3:
                                            this.roopneutral = 0;
                                            this.Motion = NaviBase.MOTION.neutral;
                                            if (!this.atack)
                                            {
                                                this.speed = this.nspeed;
                                                break;
                                            }
                                            break;
                                    }

                                    
                            
                            
                                    break;

									
									
                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.move:

                    //if (this.moveflame)
                        //this.animationpoint = this.AnimeMove(this.frame);

                    //this.animationpoint = this.AnimeNeutral(this.waittime);
                    this.animationpoint = this.AnimeNeutral(this.frame);
                    

                    if (this.flyflame % 3 == 0)
                    {
                        this.ShadowYuyu();
                    }

                    if (this.frame > 2)
                    {
                        if (this.flyflame == this.time)
                        {
                            this.motion = yuyuko.MOTION.neutral;
                            if (this.fan)
                            {
                                this.motion = yuyuko.MOTION.attack;
                            }
                            this.counterTiming = false;
                            this.plusy = 0.0f;
                            this.frame = 0;
                            //this.dammy.flag = false;
                            //this.Motion = NaviBase.MOTION.neutral;
                            //this.frame = 0;
                            this.roopneutral = 0;
                            ++this.roopmove;

                        }
                        else
                        {

                            //this.ShadowYuyu();

                            this.positionDirect.X -= this.movex;
                            this.positionDirect.Y -= this.movey;
                            this.plusy -= this.speedy;
                            this.speedy -= this.plusing;
                            //this.nohit = speedy * (double)this.speedy < 25.0;
                            if (speedy < 0.0)
                                this.position = this.positionre;
                        }
                        ++this.flyflame;
                        break;
                    }


                    break;


                case NaviBase.MOTION.knockback:
                    switch (this.waittime)
                    {
                        case 2:
                            this.NockMotion();
                            this.counterTiming = false;
                            this.effecting = false;

                            if (this.chargeEffect != null)
                                this.chargeEffect.flag = false;

                            this.PositionDirectSet();
                            break;
                        case 3:
                            this.NockMotion();
                            break;
                        case 15:
                            this.animationpoint = new Point(3, 0);
                            this.PositionDirectSet();
                            break;
                        case 21:
                            this.animationpoint = new Point(0, 0);
                            this.waittime = 0;
                            this.Motion = NaviBase.MOTION.neutral;
                            break;
                    }
                    if (this.waittime >= 2 && this.waittime <= 6)
                        this.positionDirect.X -= this.UnionRebirth(this.union);
                    ++this.waittime;
                    break;
            }
            this.FlameControl();
            this.MoveAftar();
        }

        public override void NockMotion()
        {
            this.animationpoint = new Point(2, 0);
        }

        public override void Render(IRenderer dg)
        {
                if (this.alfha < byte.MaxValue)
                    this.color = Color.FromArgb(alfha, this.mastorcolor);
                else
                    this.color = this.mastorcolor;

                int xOff = 4;
                int yOff = -12;

                this._position = new Vector2((int)this.positionDirect.X + xOff + this.Shake.X, (int)this.positionDirect.Y + yOff + this.Shake.Y);
                this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height);

                if (this.Hp <= 0)
                {
                    this.rd = this._rect;
                    this.NockMotion();
                    this._rect = new Rectangle(this.animationpoint.X * this.wide + this.Shake.X, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height + this.Shake.Y);
                    this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, this.height, this.wide, this.height), this._position, this.picturename);
                }

                if (this.whitetime == 0)
                {
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);

                    /*
                    if (this.motion == NaviBase.MOTION.attack && this.attack == Iku.ATTACK.drillArm && this.waittime >= 7 && this.waittime < 20)
                    {
                        int num = this.waittime % 3;
                        this._position = new Vector2((int)this.positionDirect.X + 44 * this.UnionRebirth(this.union) + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
                        this._rect = new Rectangle(1600 + num * this.Wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height);
                        dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                    }*/

                }
                else
                {
                    this._rect.Y = this.height;
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                /*
                    if (this.motion == NaviBase.MOTION.attack && this.attack == Iku.ATTACK.drillArm && this.waittime >= 7 && this.waittime < 20)
                    {
                        int num = this.waittime % 3;
                        this._position = new Vector2((int)this.positionDirect.X + 44 * this.UnionRebirth(this.union) + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
                        this._rect = new Rectangle(1600 + num * this.Wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height)
                        {
                            Y = this.height
                        };
                        dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                    }
                    */
                }
                this.HPposition = new Vector2((int)this.positionDirect.X + 12, (int)this.positionDirect.Y - this.height / 2 - 3);
            
                //this.HPposition.X = 400f;
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            //return this.Return(new int[1], new int[1], 1, waitflame);
            return this.Return(new int[5] { 0, 1, 2, 4, 5 }, new int[5]
            {
        8, 9, 10 , 11, 12
            }, 1, waitflame);

        }

        private Point AnimeButterfly(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5] {
                4,
                4,
                4,
                4,
                100
            }, new int[5]
            {
                27, 28, 29, 30, 27
            }, 0, waittime);
        }

        private Point AnimeButterfly1(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5] {
                4,
                4,
                4,
                4,
                100
            }, new int[5]
            {
                27, 28, 29, 30, 27
            }, 0, waittime);
        }

        private Point AnimeButterfly2(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5] {
                4,
                4,
                4,
                4,
                100
            }, new int[5]
            {
                36, 32, 33, 34, 35
            }, 0, waittime);
        }

        private Point AnimeMine(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6] {
                4,
                4,
                4,
                4,
                4,
                100
            }, new int[6]
            {
                36, 37, 38, 39, 40, 41
            }, 0, waittime);
        }

        private Point AnimeMachinegunRay(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        this.aspeed * 3,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        100
            }, new int[4] { 5, 6, 7, 5 }, 0, waittime);
        }

        private Point AnimeFanBarrageStart(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        4,
        4,
        4,
        100
            }, new int[4] { 3, 24, 25, 26 }, 0, waittime);
        }

        private Point AnimeFanBarrage(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        4,
        4,
        4,
        4,
        4,
        4
            }, new int[6] { 23, 22, 23, 22, 23, 22 }, 0, waittime);
        }

        private enum ATTACK
        {
            butterflyStraight,
            butterflyAirhoc,
            yuyuMine,
            yuyuFanBarrage,
        }

        private void DammySet()
        {
            this.dammy.position = this.position;
            this.dammy.flag = true;
            this.dammy.nomove = true;
            this.dammy.effecting = true;
            this.parent.enemys.Add(dammy);
        }

        private void Throw()
        {
            //int xOff = 22;
            //int yOff = 72;
            int xOff = 21+2;
            int yOff = 64-7;

            this.endposition = new Vector2(this.positionre.X * 40 + xOff + 6 * this.UnionRebirth(this.union), this.positionre.Y * 24 + yOff);
            this.movex = (this.positionDirect.X - this.endposition.X) / time;
            this.movey = (this.positionDirect.Y - this.endposition.Y) / time;
            this.plusy = 0.0f;
            this.speedy = 6f;
            this.plusing = this.speedy / (this.time / 2);
            this.flyflame = 0;
        }

        /*
        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 22.0) + 6 * this.UnionRebirth, (float)(position.Y * 24.0 + 72.0));
        }*/

        public void ShadowYuyu()
        {
            int xOff = 21 + 2;
            int yOff = 64 - 7;

            //Vector2 position = new Vector2(this.positionDirect.X + xOff, this.positionDirect.Y + yOff);
            Vector2 position = this.positionDirect;
            position.X += 4;
            position.Y -= 12;
            int a, b, c;
            a = 0;
            b = 0;
            c = 100;

            this.parent.effects.Add(new StepShadowYuyu(this.sound, this.parent, new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height), position, this.picturename, this.rebirth, this.position, a, b, c));

        }

    }
}

