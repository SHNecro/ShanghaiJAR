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
    internal class MimaShock : AttackBase
    {
        private MimaShock.MOTION motion;
        private readonly int time;

        public MimaShock(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          MimaShock.MOTION m)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.normal)
        {
            if (this.InArea)
            {
                if (this.parent.panel[this.position.X, this.position.Y].state == Panel.PANEL._break)
                    return;
                this.canCounter = false;
                this.time = 1;
                this.speed = 3;
                this.invincibility = false;
                this.animationpoint.X = 0;
                this.hitrange = new Point(0, 0);
                this.hitting = false;
                this.rebirth = this.union == Panel.COLOR.red;
                this.positionre = this.position;
                this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 82);
                this.sound.PlaySE(SoundEffect.waveshort);
                this.motion = m;
                this.frame = 0;
            }
            else
                this.flag = false;
        }

        public override void Updata()
        {
            if (this.hitting)
                this.PanelBright();
            if (this.over)
                return;
            if (this.moveflame)
            {
                if (this.motion == MimaShock.MOTION.init)
                {
                    if (this.frame == 4)
                    {
                        this.frame = 0;
                        this.motion = MimaShock.MOTION.set;
                    }
                }
                else
                {
                    this.animationpoint.X = this.frame;
                    switch (this.frame)
                    {
                        case 2:
                            this.hitting = true;
                            break;
                        case 4:
                            this.sound.PlaySE(SoundEffect.waveshort);
                            this.hitting = false;
                            break;
                        case 6:
                            this.flag = false;
                            break;
                    }
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.motion != MimaShock.MOTION.set)
                return;
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double num2 = positionDirect.Y - 22.0;
            shake = this.Shake;
            double y = shake.Y;
            double num3 = num2 + y;
            this._position = new Vector2((float)num1, (float)num3);
            this._rect = new Rectangle(this.animationpoint.X * 48, 288, 48, 56);
            dg.DrawImage(dg, "mimaAttack", this._rect, false, this._position, this.rebirth, Color.White);
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
