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
    internal class Beam : AttackBase
    {
        public Beam(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          bool pl)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.eleki)
        {
            if (!this.flag)
                return;
            this.invincibility = true;
            this.upprint = false;
            this.speed = s;
            this.element = ChipBase.ELEMENT.eleki;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = false;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (pl)
            {
                if (this.union == Panel.COLOR.red)
                    this.positionDirect = new Vector2(this.position.X * 40 + 5, this.position.Y * 24 + 42);
                else
                    this.positionDirect = new Vector2((this.position.X + 1) * 40 - 5, this.position.Y * 24 + 42);
            }
            else if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 + 2, this.position.Y * 24 + 38);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40, this.position.Y * 24 + 42);
            this.frame = 0;
            this.color = Color.White;
        }

        public override void Updata()
        {
            this.PanelBright();
            if (this.moveflame)
            {
                if (this.animationpoint.X < 5)
                    ++this.animationpoint.X;
                switch (this.frame)
                {
                    case 0:
                        this.sound.PlaySE(SoundEffect.beam);
                        break;
                    case 4:
                        this.hitting = true;
                        break;
                }
                if (this.frame > 4 && this.hitrange.X < 6)
                    ++this.hitrange.X;
                if (this.frame > 20 && this.hitrange.X >= 6 && this.animationpoint.X < 10)
                    ++this.animationpoint.X;
                else if (this.animationpoint.X >= 10)
                    this.flag = false;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 48, 432, 48, 32);
            dg.DrawImage(dg, "bomber", this._rect, true, this._position, this.rebirth, this.color);
            for (int index = 1; index < this.hitrange.X; ++index)
            {
                this._position = new Vector2(this.positionDirect.X + index * 48 * this.UnionRebirth + Shake.X, this.positionDirect.Y + Shake.Y);
                if (this.animationpoint.X < 6)
                    this._rect = new Rectangle((index + 1 < this.hitrange.X ? 1 : 0) * 48, 464, 48, 32);
                else
                    this._rect = new Rectangle((this.animationpoint.X - 4) * 48, 464, 48, 32);
                dg.DrawImage(dg, "bomber", this._rect, true, this._position, this.rebirth, this.color);
            }
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
