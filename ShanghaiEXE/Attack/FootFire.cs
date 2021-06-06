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
    internal class FootFire : AttackBase
    {
        private FootFire.MOTION motion;
        private readonly int time;

        public FootFire(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          FootFire.MOTION m)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.heat)
        {
            if (this.parent.panel[this.position.X, this.position.Y].state == Panel.PANEL._break)
                return;
            this.canCounter = false;
            this.time = 1;
            this.speed = 2;
            this.invincibility = false;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = false;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 82);
            this.sound.PlaySE(SoundEffect.heat);
            this.motion = m;
            this.frame = 0;
            this.badstatus[1] = true;
            this.badstatustime[1] = 180;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.moveflame)
            {
                switch (this.motion)
                {
                    case FootFire.MOTION.init:
                        this.animationpoint.X = this.frame;
                        if (this.frame >= 3)
                        {
                            this.hitting = true;
                            this.motion = FootFire.MOTION.set;
                            break;
                        }
                        break;
                    case FootFire.MOTION.set:
                        this.animationpoint.X = 3;
                        if (this.frame >= this.time)
                        {
                            this.hitting = false;
                            this.frame = 0;
                            this.motion = FootFire.MOTION.end;
                            break;
                        }
                        break;
                    case FootFire.MOTION.end:
                        this.animationpoint.X = 4 - this.frame;
                        if (this.frame >= 3)
                        {
                            this.flag = false;
                            break;
                        }
                        break;
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y - 30f + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 32, 0, 32, 80);
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

        public enum MOTION
        {
            init,
            set,
            end,
        }
    }
}
