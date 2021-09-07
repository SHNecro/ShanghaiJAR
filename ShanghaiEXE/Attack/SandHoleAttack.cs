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
    internal class SandHoleAttack : AttackBase
    {
        private SandHoleAttack.MOTION motion;
        private readonly int time;

        public SandHoleAttack(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int t,
          int c,
          SandHoleAttack.MOTION m,
          ChipBase.ELEMENT ele,
          bool warnPanel = false)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.downprint = true;
            this.time = t;
            this.speed = 5;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = false;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 82);
            this.animationpoint.Y = c;
            this.motion = m;
            switch (this.motion)
            {
                case SandHoleAttack.MOTION.init:
                    if (this.element == ChipBase.ELEMENT.leaf)
                    {
                        this.sound.PlaySE(SoundEffect.sand);
                        this.parent.panel[this.position.X, this.position.Y].state = Panel.PANEL._grass;
                        break;
                    }
                    this.parent.panel[this.position.X, this.position.Y].state = Panel.PANEL._sand;
                    this.sound.PlaySE(SoundEffect.sand);
                    this.element = ChipBase.ELEMENT.earth;
                    break;
                case SandHoleAttack.MOTION.set:
                    this.hitting = true;
                    break;
            }
            if (warnPanel)
            {
                this.parent.attacks.Add(new Dummy(so, p, pX, pY, u, Point.Empty, 4 * this.speed, true));
            }
            this.frame = 0;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.moveflame)
            {
                switch (this.motion)
                {
                    case SandHoleAttack.MOTION.init:
                        this.animationpoint.X = this.frame;
                        if (this.frame >= 4)
                        {
                            this.hitting = true;
                            this.motion = SandHoleAttack.MOTION.set;
                            break;
                        }
                        break;
                    case SandHoleAttack.MOTION.set:
                        this.animationpoint.X = this.frame % 4 + 4;
                        if (this.frame >= this.time)
                        {
                            this.hitting = false;
                            this.frame = 0;
                            this.motion = SandHoleAttack.MOTION.end;
                            break;
                        }
                        break;
                    case SandHoleAttack.MOTION.end:
                        this.animationpoint.X = 5 - this.frame;
                        if (this.frame >= 4)
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
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(192 + this.animationpoint.X * 40, this.animationpoint.Y * 24, 40, 24);
            dg.DrawImage(dg, "rieber", this._rect, false, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            this.flag = false;
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;
            this.flag = false;
            return true;
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
