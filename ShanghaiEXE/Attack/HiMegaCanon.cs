using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Drawing;

namespace NSAttack
{
    internal class HiMegaCanon : AttackBase
    {
        private int time;

        public HiMegaCanon(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          bool pl)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.normal)
        {
            if (!this.flag)
                return;
            this.upprint = true;
            this.hitting = true;
            this.animationpoint.X = 0;
            this.hitrange.Y = 2;
            this.rebirth = this.union == Panel.COLOR.blue;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 - 5, this.position.Y * 24 + 48);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 5, this.position.Y * 24 + 48);
            --this.position.Y;
            this.speed = s;
            this.positionre = this.position;
            this.frame = 0;
        }

        public override void Updata()
        {
            this.PanelBright();
            this.FlameControl(this.speed);
            if (!this.moveflame)
                return;
            switch (this.time)
            {
                case 0:
                    this.sound.PlaySE(SoundEffect.bombmiddle);
                    break;
                case 2:
                    ++this.hitrange.X;
                    break;
                case 5:
                    this.parent.attacks.Add(new BombAttack(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth, this.position.Y + 1, this.union, this.power, this.speed, 5, this.element));
                    break;
                case 11:
                    this.flag = false;
                    break;
            }
            ++this.time;
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X + 64 * this.UnionRebirth, (float)(positionDirect.Y + (double)this.Shake.Y + 8.0));
            this._rect = new Rectangle(this.time * 96, 544, 96, 72);
            dg.DrawImage(dg, "bomber", this._rect, false, this._position, this.rebirth, Color.White);
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
