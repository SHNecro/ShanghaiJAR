using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSEnemy
{
    internal class Marisa : NaviBase
    {
        private readonly int[] powers = new int[4] { 20, 10, 30, 0 };
        private readonly int nspeed = 2;
        private readonly int moveroop;
        private Marisa.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private int atackroop;
        private readonly bool atack;

        public Marisa(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.normal;
            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.MarisaName1");
                    this.power = 20;
                    this.hp = 300;
                    this.moveroop = 1;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.MarisaName2");
                    this.power = 40;
                    this.hp = 800;
                    this.moveroop = 1;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.MarisaName3");
                    this.power = 100;
                    this.hp = 1300;
                    this.moveroop = 2;
                    break;
                case 4:
                    this.nspeed = 1;
                    this.name = ShanghaiEXE.Translate("Enemy.MarisaName4");
                    this.power = 200;
                    this.hp = 1600;
                    this.moveroop = 2;
                    break;
                default:
                    this.nspeed = 1;
                    this.name = ShanghaiEXE.Translate("Enemy.MarisaName5") + (version - 3).ToString();
                    this.power = 200;
                    this.hp = 1800 + (version - 4) * 500;
                    this.moveroop = 3;
                    break;
            }
            this.picturename = "marisa";
            this.race = EnemyBase.ENEMY.navi;
            this.Flying = false;
            this.wide = 48;
            this.height = 64;
            this.hpmax = this.hp;
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.Flying = false;
            this.roopmove = n * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new MarisaV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MarisaV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MarisaV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new MarisaV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new MarisaV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new MarisaV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MarisaV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MarisaV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new MarisaV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new MarisaV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new MarisaV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MarisaV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MarisaV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new MarisaV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new MarisaV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new MarisaV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new MarisaV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new MarisaV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new MarisaV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new MarisaV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version < 8)
                        break;
                    this.dropchips[4].chip = new MarisaX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 18000;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 16.0), (float)(position.Y * 24.0 + 58.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeNeutral(this.waittime);
                        if (this.waittime >= 16 / version || this.atack)
                        {
                            this.waittime = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (this.roopmove > this.moveroop && !this.badstatus[4])
                                {
                                    this.roopmove = this.version > 3 ? this.Random.Next(-1, this.moveroop + 1) : 0;
                                    ++this.atackroop;
                                    this.speed = 1;
                                    if (!this.atack)
                                    {
                                        int index = this.Random.Next(this.version > 1 ? 3 : 1);
                                        this.attack = (Marisa.ATTACK)index;
                                        this.powerPlus = this.powers[index];
                                    }
                                    this.waittime = 0;
                                    this.Motion = NaviBase.MOTION.attack;
                                    this.counterTiming = true;
                                }
                                else
                                {
                                    this.waittime = 0;
                                    if (this.atack)
                                        this.roopmove = this.moveroop + 1;
                                    this.Motion = NaviBase.MOTION.move;
                                }
                            }
                        }
                        break;
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
                                case Marisa.ATTACK.narrowSpark:
                                    this.animationpoint = this.AnimeSpark(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 24:
                                            this.counterTiming = false;
                                            this.sound.PlaySE(SoundEffect.beam);
                                            this.parent.attacks.Add(new Beam(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 2, false));
                                            break;
                                        case 96:
                                            this.roopneutral = 0;
                                            this.Motion = NaviBase.MOTION.neutral;
                                            if (!this.atack)
                                            {
                                                this.speed = this.nspeed;
                                                break;
                                            }
                                            break;
                                    }
                                    if (this.waittime != 24)
                                        break;
                                    break;
                                case Marisa.ATTACK.dustBomb:
                                    this.animationpoint = this.AnimeBomb(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 32:
                                            this.counterTiming = false;
                                            this.sound.PlaySE(SoundEffect.throwbomb);
                                            var bombTargets = this.RandomMultiPanel(Math.Min(version - 1, 2), this.UnionEnemy);
                                            this.RandomTarget();
                                            Vector2 v = new Vector2(this.positionDirect.X + 8 * this.UnionRebirth(this.union), this.positionDirect.Y - 8f);
                                            foreach (var bombTarget in bombTargets)
                                            {
                                                this.parent.attacks.Add(new ClossBomb(this.sound, this.parent, bombTarget.X, bombTarget.Y, this.union, this.Power, 1, v, bombTarget, 40, ClossBomb.TYPE.closs, false, ClossBomb.TYPE.big, false, false));
                                            }
                                            break;
                                        case 44:
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
                                case Marisa.ATTACK.magicMissile:
                                    this.animationpoint = this.AnimeMissile(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 34:
                                            this.counterTiming = false;
                                            this.sound.PlaySE(SoundEffect.canon);
                                            this.parent.attacks.Add(new MagicMissile(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.Power, ChipBase.ELEMENT.normal));
                                            this.parent.attacks.Add(new MagicMissile(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.Power, ChipBase.ELEMENT.normal));
                                            break;
                                        case 42:
                                            this.sound.PlaySE(SoundEffect.canon);
                                            this.parent.attacks.Add(new MagicMissile(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.Power, ChipBase.ELEMENT.normal));
                                            this.parent.attacks.Add(new MagicMissile(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.Power, ChipBase.ELEMENT.normal));
                                            break;
                                        case 50:
                                            this.sound.PlaySE(SoundEffect.canon);
                                            this.parent.attacks.Add(new MagicMissile(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.Power, ChipBase.ELEMENT.normal));
                                            this.parent.attacks.Add(new MagicMissile(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.Power, ChipBase.ELEMENT.normal));
                                            break;
                                        case 62:
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
                    this.animationpoint = this.AnimeMove(this.waittime);
                    if (this.moveflame)
                    {
                        switch (this.waittime)
                        {
                            case 0:
                                this.MoveRandom(false, false);
                                if (this.position == this.positionre)
                                {
                                    this.Motion = NaviBase.MOTION.neutral;
                                    this.frame = 0;
                                    this.roopneutral = 0;
                                    ++this.roopmove;
                                    break;
                                }
                                break;
                            case 4:
                                this.position = this.positionre;
                                this.PositionDirectSet();
                                break;
                            case 7:
                                this.Motion = NaviBase.MOTION.neutral;
                                this.frame = 0;
                                this.roopneutral = 0;
                                ++this.roopmove;
                                break;
                        }
                        ++this.waittime;
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
                            this.PositionDirectSet();
                            break;
                        case 3:
                            this.NockMotion();
                            break;
                        case 15:
                            this.animationpoint = new Point(6, 0);
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
            this.animationpoint = new Point(14, 0);
        }

        public override void Render(IRenderer dg)
        {
            if (this.alfha < byte.MaxValue)
                this.color = Color.FromArgb(alfha, this.mastorcolor);
            else
                this.color = this.mastorcolor;
            double num1 = (int)this.positionDirect.X + this.Shake.X;
            int y1 = (int)this.positionDirect.Y;
            Point shake = this.Shake;
            int y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height);
            if (this.Hp <= 0)
            {
                this.rd = this._rect;
                this.NockMotion();
                int num3 = this.animationpoint.X * this.wide;
                shake = this.Shake;
                int x1 = shake.X;
                int x2 = num3 + x1;
                int y3 = (this.version < 4 ? 0 : 2) * this.height;
                int wide = this.wide;
                int height1 = this.height;
                shake = this.Shake;
                int y4 = shake.Y;
                int height2 = height1 + y4;
                this._rect = new Rectangle(x2, y3, wide, height2);
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, this.height, this.wide, this.height), this._position, this.picturename);
            }
            if (this.whitetime == 0)
            {
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            else
            {
                this._rect.Y = this.height;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2((int)this.positionDirect.X + 8, (int)this.positionDirect.Y - 8 - this.height / 2 + 8);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[7] { 0, 1, 2, 3, 5, 6, 7 }, new int[7]
            {
        0,
        1,
        2,
        3,
        2,
        1,
        0
            }, 1, waitflame);
        }

        private Point AnimeSpark(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[10]
            {
        4,
        4,
        4,
        4,
        4,
        4,
        60,
        4,
        4,
        4
            }, new int[11] { 0, 4, 5, 6, 7, 8, 9, 5, 6, 4, 0 }, 0, waittime);
        }

        private Point AnimeBomb(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        4,
        4,
        16,
        4,
        4,
        60
            }, new int[6] { 0, 15, 16, 17, 18, 19 }, 0, waittime);
        }

        private Point AnimeMissile(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[7]
            {
        4,
        14,
        4,
        4,
        4,
        4,
        60
            }, new int[7] { 0, 4, 5, 20, 21, 22, 23 }, 0, waittime);
        }

        private enum ATTACK
        {
            narrowSpark,
            dustBomb,
            magicMissile,
            masterSpark,
        }
    }
}

