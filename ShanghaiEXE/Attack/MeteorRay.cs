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
    internal class MeteorRay : AttackBase
    {
        public new bool bright = true;
        private readonly int time;
        public bool effect;

        public MeteorRay(
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
            this.speed = s;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = false;
            this.rebirth = this.union != Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 - 5, this.position.Y * 24 + 20);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 8, this.position.Y * 24 + 20);
            this.frame = 0;
            this.time = 9;
            this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.hitrange, this.speed * (this.time / 2), true));
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.bright)
                this.PanelBright();
            if (this.moveflame)
            {
                switch (this.frame)
                {
                    case 4:
                        this.sound.PlaySE(SoundEffect.bombmiddle);
                        this.ShakeStart(2, 2);
                        this.hitting = true;
                        break;
                    case 8:
                        this.hitting = false;
                        break;
                }
                this.animationpoint.X = this.frame;
                if (this.frame >= this.time)
                    this.flag = false;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 88, 0, 88, 152);
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
