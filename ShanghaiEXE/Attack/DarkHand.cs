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
    internal class DarkHand : AttackBase
    {
        private readonly int time;
        private Vector2 endposition;
        private readonly float movex;
        private readonly float movey;
        private float plusy;
        private float speedy;
        private readonly float plusing;
        private const int startspeed = 6;
        private new bool bright;

        public DarkHand(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          Vector2 v,
          Point end,
          int t)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.normal)
        {
            this.hitting = false;
            this.speed = s;
            this.positionDirect = v;
            this.time = t;
            this.position = end;
            this.endposition = new Vector2(end.X * 40 + 20, end.Y * 24 + 80);
            this.movex = (v.X - this.endposition.X) / t;
            this.movey = (v.Y - this.endposition.Y) / t;
            this.plusy = 0.0f;
            this.speedy = 6f;
            this.plusing = this.speedy / (this.time / 2);
            this.rebirth = (uint)this.union > 0U;
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
                this.sound.PlaySE(SoundEffect.bombmiddle);
                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y, Bomber.BOMBERTYPE.poison, 2));
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
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y - this.plusy + Shake.Y);
            this._rect = new Rectangle(0, 960, 112, 40);
            dg.DrawImage(dg, "darkPA", this._rect, false, this._position, this.rebirth, Color.White);
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
    }
}
