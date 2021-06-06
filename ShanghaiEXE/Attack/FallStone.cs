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
    internal class FallStone : AttackBase
    {
        public new bool bright = true;
        private int y = -144;
        private int yspeed = -60;
        private readonly int time;
        public bool flash;
        private bool end;
        private readonly int interval;
        private int intervalTime;
        private readonly Shadow shadow;

        public FallStone(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          int interval,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.interval = interval;
            this.speed = s;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = false;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 96);
            else
                this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 96);
            this.frame = 0;
            this.time = 1;
            this.shadow = new Shadow(this.sound, this.parent, this.position.X, this.position.Y, this);
            this.parent.effects.Add(shadow);
        }

        public override void Updata()
        {
            if (this.intervalTime >= this.interval)
            {
                if (this.over)
                    return;
                if (!this.end)
                {
                    if (this.yspeed < 0)
                    {
                        ++this.yspeed;
                    }
                    else
                    {
                        if (this.yspeed < 4)
                            ++this.yspeed;
                        if (this.y < 0)
                        {
                            this.y += this.yspeed;
                            if (this.y >= 0)
                            {
                                if (this.StandPanel.Hole)
                                {
                                    this.shadow.flag = false;
                                    this.flag = false;
                                }
                                else
                                {
                                    this.y = 0;
                                    this.yspeed = 0;
                                    this.hitting = true;
                                    this.end = true;
                                }
                            }
                        }
                    }
                }
                else if (this.hitting)
                {
                    ++this.yspeed;
                    if (this.yspeed > 0)
                    {
                        this.hitting = false;
                        this.shadow.flag = false;
                        this.flag = false;
                        this.sound.PlaySE(SoundEffect.breakObject);
                        this.parent.effects.Add(new BreakCube(this.sound, this.parent, this.position, this.positionDirect.X, this.positionDirect.Y - 12f, 12, this.union, 20, true, 1));
                        this.parent.effects.Add(new BreakCube(this.sound, this.parent, this.position, this.positionDirect.X, this.positionDirect.Y - 12f, 12, this.union, 20, false, 1));
                        this.yspeed = 0;
                    }
                }
                this.FlameControl(2);
            }
            else
                ++this.intervalTime;
        }

        public override void Render(IRenderer dg)
        {
            if (this.flash)
                return;
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double num2 = positionDirect.Y - 30.0 + this.y;
            shake = this.Shake;
            double y = shake.Y;
            double num3 = num2 + y;
            this._position = new Vector2((float)num1, (float)num3);
            this._rect = new Rectangle(32, 392, 40, 48);
            dg.DrawImage(dg, "towers", this._rect, false, this._position, this.rebirth, Color.White);
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
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, this.element));
            return true;
        }
    }
}
