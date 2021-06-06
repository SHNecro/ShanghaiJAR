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
    internal class MimaRockTower : AttackBase
    {
        private readonly MimaRockTower.MOTION motion;
        private readonly int time;

        public MimaRockTower(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          MimaRockTower.MOTION m)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.earth)
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
                this.breaking = true;
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
                this.animationpoint.X = this.frame;
                switch (this.frame)
                {
                    case 2:
                        this.hitting = true;
                        break;
                    case 4:
                        this.hitting = false;
                        break;
                    case 6:
                        this.flag = false;
                        break;
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y - 22f + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 48, 88, 48, 64);
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
