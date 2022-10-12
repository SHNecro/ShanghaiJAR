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
    internal class ChainAttack : AttackBase
    {
        private readonly int colol;

        private bool blocked;

        public ChainAttack(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          int c,
          bool pl,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.colol = c;
            this.upprint = false;
            this.speed = s;
            this.element = ChipBase.ELEMENT.eleki;
            this.animationpoint.X = 0;
            this.invincibility = false;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (pl)
            {
                if (this.union == Panel.COLOR.red)
                    this.positionDirect = new Vector2(this.position.X * 40 + 7, this.position.Y * 24 + 53);
                else
                    this.positionDirect = new Vector2((this.position.X + 1) * 40 - 5, this.position.Y * 24 + 53);
            }
            else if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 - 4, this.position.Y * 24 + 57);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 4, this.position.Y * 24 + 57);
            this.frame = 0;
            this.badstatus[3] = true;
            this.badstatustime[3] = 120;
            this.sound.PlaySE(SoundEffect.chain);
        }

        public override void Updata()
        {
            this.PanelBright();
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame < 3 ? this.frame : 5 - this.frame;
                switch (this.frame)
                {
                    case 4:
                        this.hitting = false;
                        break;
                    case 6:
                        this.flag = false;
                        break;
                }
                if (this.frame < 3)
                    ++this.hitrange.X;
                if (this.frame > 6)
                    this.flag = false;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(720 + this.colol * 120, this.animationpoint.X * 16, 120, 16);
            dg.DrawImage(dg, "weapons", this._rect, true, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            return base.HitCheck(charaposition, charaunion);
        }

        public override bool HitCheck(Point charaposition)
        {
            return base.HitCheck(charaposition);
        }

        public override bool BarierCheck(CharacterBase c, int damage)
        {
            var barrierCheck = base.BarierCheck(c, damage);
            this.blocked = !barrierCheck;
            return barrierCheck;
        }

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p) && !this.blocked)
                return false;
            this.frame = 6 - this.frame;
            this.hitting = false;
            p.Knockbuck(false, true, this.union);
            if (!this.blocked)
            {
                this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, this.element));
            }
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e) && !this.blocked)
                return false;
            this.frame = 6 - this.frame;
            this.hitting = false;
            e.Knockbuck(false, true, this.union);
            if (!this.blocked)
            {
                this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, this.element));
            }
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o) && !this.blocked)
                return false;
            this.frame = 6 - this.frame;
            this.hitting = false;
            o.Knockbuck(false, true, this.union);
            if (!this.blocked)
            {
                this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, this.element));
            }
            return true;
        }
    }
}
