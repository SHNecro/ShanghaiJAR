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
    internal class SatelliteBarn : AttackBase
    {
        public new bool bright = true;
        private readonly int time;
        public bool effect;
        private readonly int roop;
        private int roopNow;

        public SatelliteBarn(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.roop = 2;
            this.speed = s;
            this.animationpoint.X = 0;
            this.hitrange = new Point(2, 2);
            --this.position.Y;
            this.hitting = false;
            this.breaking = true;
            this.rebirth = this.union != Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 + 60, this.position.Y * 24 + 68);
            else
                this.positionDirect = new Vector2(this.position.X * 40 - 20, this.position.Y * 24 + 68);
            if (this.position.Y < 0)
            {
                ++this.position.Y;
                --this.hitrange.Y;
            }
            this.frame = 0;
            this.time = 9;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.moveflame)
            {
                switch (this.frame)
                {
                    case 5:
                        this.sound.PlaySE(SoundEffect.bombmiddle);
                        this.ShakeStart(2, 2);
                        this.hitting = true;
                        break;
                    case 11:
                        ++this.roopNow;
                        if (this.roopNow >= this.roop)
                        {
                            this.hitting = false;
                            this.animationpoint.X = 3;
                            break;
                        }
                        this.frame = 9;
                        break;
                    case 12:
                        this.animationpoint.X = 2;
                        break;
                    case 13:
                        this.animationpoint.X = 1;
                        break;
                    case 14:
                        this.animationpoint.X = 0;
                        break;
                    case 15:
                        this.flag = false;
                        break;
                }
                if (this.frame < 11)
                    this.animationpoint.X = this.frame;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 120, 160, 120, 144);
            this._position = new Vector2(this.positionDirect.X, this.positionDirect.Y);
            dg.DrawImage(dg, "kikuriAttack", this._rect, false, this._position, this.rebirth, Color.White);
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
            if (this.effect)
                this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            if (this.effect)
                this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            if (this.effect)
                this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, this.element));
            return true;
        }
    }
}
