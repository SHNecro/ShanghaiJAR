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
    internal class WebBomb : AttackBase
    {
        private readonly int time;
        private Vector2 endposition;
        private readonly float movex;
        private readonly float movey;
        private float plusy;
        private float speedy;
        private readonly float plusing;
        private const int startspeed = 6;
        private readonly int heattime;

        public WebBomb(
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
          int heattime,
          ChipBase.ELEMENT element)
          : base(so, p, pX, pY, u, po, element)
        {
            this.heattime = heattime;
            this.breaking = true;
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
        }

        public override void Updata()
        {
            if (this.frame % 5 == 0)
                this.bright = !this.bright;
            if (this.bright)
                this.PanelBright();
            if (this.frame == this.time)
            {
                this.flag = false;
                if (this.InArea && !this.StandPanel.Hole)
                {
                    this.sound.PlaySE(SoundEffect.lance);
                    this.parent.effects.Add(new Smoke(this.sound, this.parent, this.position.X, this.position.Y, this.element));
                    WebTrap webTrap = new WebTrap(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.heattime, this.element)
                    {
                        badstatus = this.badstatus,
                        badstatustime = this.badstatustime
                    };
                    webTrap.Init();
                    this.parent.attacks.Add(webTrap);
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
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y - this.plusy + Shake.Y);
            int num = 0;
            switch (this.element)
            {
                case ChipBase.ELEMENT.heat:
                    num = 1;
                    break;
                case ChipBase.ELEMENT.eleki:
                    num = 4;
                    break;
                case ChipBase.ELEMENT.leaf:
                    num = 0;
                    break;
                case ChipBase.ELEMENT.poison:
                    num = 2;
                    break;
                case ChipBase.ELEMENT.earth:
                    num = 3;
                    break;
            }
            this._rect = new Rectangle(48 + 16 * num, 0, 16, 16);
            dg.DrawImage(dg, "bombs", this._rect, false, this._position, this.rebirth, Color.White);
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
            return base.HitEvent(p);
        }

        public override bool HitEvent(EnemyBase e)
        {
            return base.HitEvent(e);
        }

        public override bool HitEvent(ObjectBase o)
        {
            return base.HitEvent(o);
        }
    }
}
