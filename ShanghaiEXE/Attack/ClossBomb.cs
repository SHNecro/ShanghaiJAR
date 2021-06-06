using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Drawing;

namespace NSAttack
{
    internal class ClossBomb : AttackBase
    {
        private readonly ClossBomb.TYPE type;
        private readonly ClossBomb.TYPE ctype;
        private readonly int time;
        private Vector2 endposition;
        private readonly float movex;
        private readonly float movey;
        private float plusy;
        private float speedy;
        private readonly float plusing;
        private const int startspeed = 6;
        private readonly bool cluster;
        private readonly bool poizon;
        private readonly SoundEffect soundName;

        public ClossBomb(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          Vector2 v,
          Point end,
          int t,
          ClossBomb.TYPE ty,
          bool cluster,
          ClossBomb.TYPE cty,
          bool poizon = false,
          bool short_ = false)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.normal)
        {
            this.poizon = poizon;
            this.ctype = cty;
            this.cluster = cluster;
            this.hitting = false;
            this.speed = s;
            this.positionDirect = v;
            this.time = t;
            this.position = end;
            this.invincibility = false;
            this.endposition = new Vector2(end.X * 40 + 20, end.Y * 24 + 80);
            this.movex = (v.X - this.endposition.X) / t;
            this.movey = (v.Y - this.endposition.Y) / t;
            this.plusy = 0.0f;
            this.speedy = 6f;
            this.plusing = this.speedy / (this.time / 2);
            this.rebirth = this.union == Panel.COLOR.red;
            this.type = ty;
            if (poizon)
            {
                this.invincibility = true;
                this.badstatus[5] = true;
                this.badstatustime[5] = 600;
                this.element = ChipBase.ELEMENT.aqua;
                if (short_)
                {
                    this.time /= 2;
                    this.plusy = 0.0f;
                }
                this.soundName = SoundEffect.waveshort;
            }
            else
                this.soundName = SoundEffect.bomb;
        }

        public override void Updata()
        {
            if (this.frame % 5 == 0)
                this.bright = !this.bright;
            if (this.bright)
                this.PanelBright();
            if (this.frame == this.time)
            {
                this.hitting = true;
                this.flag = false;
                if (this.InArea && !this.StandPanel.Hole)
                {
                    if (!this.poizon)
                    {
                        this.sound.PlaySE(this.soundName);
                        switch (this.type)
                        {
                            case ClossBomb.TYPE.single:
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y, Bomber.BOMBERTYPE.bomber, 2));
                                break;
                            case ClossBomb.TYPE.line:
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y, Bomber.BOMBERTYPE.bomber, 2));
                                AttackBase a1 = this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.power, 1, this.element)));
                                if (this.cluster)
                                    a1.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a1));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y - 1, Bomber.BOMBERTYPE.bomber, 2));
                                AttackBase a2 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a2.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a2));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y + 1, Bomber.BOMBERTYPE.bomber, 2));
                                break;
                            case ClossBomb.TYPE.closs:
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y, Bomber.BOMBERTYPE.bomber, 2));
                                AttackBase a3 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X - 1, this.position.Y, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a3.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a3));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X - 1, this.position.Y, Bomber.BOMBERTYPE.bomber, 2));
                                AttackBase a4 = this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.power, 1, this.element)));
                                if (this.cluster)
                                    a4.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a4));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y - 1, Bomber.BOMBERTYPE.bomber, 2));
                                AttackBase a5 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 1, this.position.Y, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a5.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a5));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X + 1, this.position.Y, Bomber.BOMBERTYPE.bomber, 2));
                                AttackBase a6 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a6.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a6));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y + 1, Bomber.BOMBERTYPE.bomber, 2));
                                break;
                            case ClossBomb.TYPE.big:
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y, Bomber.BOMBERTYPE.bomber, 2));
                                AttackBase a7 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X - 1, this.position.Y, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a7.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a7));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X - 1, this.position.Y, Bomber.BOMBERTYPE.bomber, 2));
                                AttackBase a8 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a8.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a8));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y - 1, Bomber.BOMBERTYPE.bomber, 2));
                                AttackBase a9 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 1, this.position.Y, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a9.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a9));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X + 1, this.position.Y, Bomber.BOMBERTYPE.bomber, 2));
                                AttackBase a10 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a10.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a10));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y + 1, Bomber.BOMBERTYPE.bomber, 2));
                                AttackBase a11 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X - 1, this.position.Y - 1, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a11.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a11));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X - 1, this.position.Y - 1, Bomber.BOMBERTYPE.bomber, 2));
                                AttackBase a12 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 1, this.position.Y - 1, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a12.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a12));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X + 1, this.position.Y - 1, Bomber.BOMBERTYPE.bomber, 2));
                                AttackBase a13 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 1, this.position.Y + 1, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a13.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a13));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X + 1, this.position.Y + 1, Bomber.BOMBERTYPE.bomber, 2));
                                AttackBase a14 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X - 1, this.position.Y + 1, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a14.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a14));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X - 1, this.position.Y + 1, Bomber.BOMBERTYPE.bomber, 2));
                                break;
                        }
                        if (this.cluster)
                        {
                            this.parent.attacks.Add(this.StateCopy(new ClossBomb(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.positionDirect, new Point(this.position.X - 1, this.position.Y), 20, this.ctype, false, ClossBomb.TYPE.big, false, false)));
                            this.parent.attacks.Add(this.StateCopy(new ClossBomb(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.positionDirect, new Point(this.position.X, this.position.Y - 1), 20, this.ctype, false, ClossBomb.TYPE.big, false, false)));
                            this.parent.attacks.Add(this.StateCopy(new ClossBomb(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.positionDirect, new Point(this.position.X + 1, this.position.Y), 20, this.ctype, false, ClossBomb.TYPE.big, false, false)));
                            this.parent.attacks.Add(this.StateCopy(new ClossBomb(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.positionDirect, new Point(this.position.X, this.position.Y + 1), 20, this.ctype, false, ClossBomb.TYPE.big, false, false)));
                        }
                    }
                    else
                    {
                        this.sound.PlaySE(this.soundName);
                        switch (this.type)
                        {
                            case ClossBomb.TYPE.single:
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X, this.position.Y, 2));
                                break;
                            case ClossBomb.TYPE.line:
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X, this.position.Y, 2));
                                AttackBase a1 = this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.power, 1, this.element)));
                                if (this.cluster)
                                    a1.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a1));
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X, this.position.Y - 1, 2));
                                AttackBase a2 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a2.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a2));
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X, this.position.Y + 1, 2));
                                break;
                            case ClossBomb.TYPE.closs:
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X, this.position.Y, 2));
                                AttackBase a3 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X - 1, this.position.Y, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a3.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a3));
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X - 1, this.position.Y, 2));
                                AttackBase a4 = this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.power, 1, this.element)));
                                if (this.cluster)
                                    a4.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a4));
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X, this.position.Y - 1, 2));
                                AttackBase a5 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 1, this.position.Y, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a5.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a5));
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X + 1, this.position.Y, 2));
                                AttackBase a6 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a6.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a6));
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X, this.position.Y + 1, 2));
                                break;
                            case ClossBomb.TYPE.big:
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X, this.position.Y, 2));
                                AttackBase a7 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X - 1, this.position.Y, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a7.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a7));
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X - 1, this.position.Y, 2));
                                AttackBase a8 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a8.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a8));
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X, this.position.Y - 1, 2));
                                AttackBase a9 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 1, this.position.Y, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a9.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a9));
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X + 1, this.position.Y, 2));
                                AttackBase a10 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a10.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a10));
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X, this.position.Y + 1, 2));
                                AttackBase a11 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X - 1, this.position.Y - 1, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a11.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a11));
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X - 1, this.position.Y - 1, 2));
                                AttackBase a12 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 1, this.position.Y - 1, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a12.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a12));
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X + 1, this.position.Y - 1, 2));
                                AttackBase a13 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 1, this.position.Y + 1, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a13.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a13));
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X + 1, this.position.Y + 1, 2));
                                AttackBase a14 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X - 1, this.position.Y + 1, this.union, this.power, 1, this.element));
                                if (this.cluster)
                                    a14.invincibility = false;
                                this.parent.attacks.Add(this.StateCopy(a14));
                                this.parent.effects.Add(new BadWater(this.sound, this.parent, this.position.X - 1, this.position.Y + 1, 2));
                                break;
                        }
                        if (this.cluster)
                        {
                            this.parent.attacks.Add(this.StateCopy(new ClossBomb(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.positionDirect, new Point(this.position.X - 1, this.position.Y), 20, this.ctype, false, ClossBomb.TYPE.single, this.poizon, false)));
                            this.parent.attacks.Add(this.StateCopy(new ClossBomb(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.positionDirect, new Point(this.position.X, this.position.Y - 1), 20, this.ctype, false, ClossBomb.TYPE.single, this.poizon, false)));
                            this.parent.attacks.Add(this.StateCopy(new ClossBomb(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.positionDirect, new Point(this.position.X + 1, this.position.Y), 20, this.ctype, false, ClossBomb.TYPE.single, this.poizon, false)));
                            this.parent.attacks.Add(this.StateCopy(new ClossBomb(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.positionDirect, new Point(this.position.X, this.position.Y + 1), 20, this.ctype, false, ClossBomb.TYPE.single, this.poizon, false)));
                        }
                    }
                }
            }
            else
            {
                this.positionDirect.X -= this.movex;
                this.positionDirect.Y -= this.movey;
                this.plusy += this.speedy;
                this.speedy -= this.plusing;
                if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                    this.flag = false;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (!this.poizon)
            {
                double x1 = positionDirect.X;
                Point shake = this.Shake;
                double x2 = shake.X;
                double num1 = x1 + x2;
                double num2 = positionDirect.Y - (double)this.plusy;
                shake = this.Shake;
                double y = shake.Y;
                double num3 = num2 + y;
                this._position = new Vector2((float)num1, (float)num3);
                this._rect = new Rectangle(0, 0, 16, 16);
                dg.DrawImage(dg, "bombs", this._rect, false, this._position, this.rebirth, Color.White);
            }
            else
            {
                double x1 = positionDirect.X;
                Point shake = this.Shake;
                double x2 = shake.X;
                double num1 = x1 + x2;
                double num2 = positionDirect.Y - (double)this.plusy;
                shake = this.Shake;
                double y = shake.Y;
                double num3 = num2 + y;
                this._position = new Vector2((float)num1, (float)num3);
                this._rect = new Rectangle(248, 368, 16, 16);
                dg.DrawImage(dg, "kikuriAttack", this._rect, false, this._position, this.rebirth, Color.White);
            }
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            return base.HitCheck(charaposition, charaunion);
        }

        public override bool HitCheck(Point charaposition)
        {
            return base.HitCheck(charaposition);
        }

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p))
                return false;
            this.flag = false;
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.flag = false;
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.flag = false;
            return true;
        }

        public enum TYPE
        {
            single,
            line,
            closs,
            big,
        }
    }
}
