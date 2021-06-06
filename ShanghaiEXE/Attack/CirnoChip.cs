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
    internal class CirnoChip : AttackBase
    {
        private const int hit = 8;
        private int movespeed;
        private bool powerUP;
        private bool hited;
        private readonly bool sp;

        public CirnoChip(IAudioEngine so, SceneBattle p, int pX, int pY, Panel.COLOR u, int po, bool sp = false)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.aqua)
        {
            if (!this.flag)
                return;
            this.sp = sp;
            this.invincibility = true;
            this.movespeed = 4;
            this.speed = 1;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union != Panel.COLOR.blue;
            this.positionre = this.position;
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0 + 24.0), (float)(position.Y * 24.0 + 58.0));
            this.frame = 0;
            this.breaking = true;
            this.OldPD = this.positionDirect;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            this.PanelBright();
            this.positionDirect.X += this.movespeed * this.UnionRebirth;
            this.position = this.Calcposition(this.positionDirect, 56, false);
            if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                this.flag = false;
            else if (!this.powerUP && (this.position.X >= 0 && this.position.X < 6 && !this.hited && this.StandPanel.state == Panel.PANEL._ice))
            {
                this.sound.PlaySE(SoundEffect.shoot);
                this.power *= 2;
                this.movespeed = 8;
                this.powerUP = true;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.over || !this.flag)
                return;
            if (this.powerUP)
            {
                this._position = new Vector2(this.positionDirect.X - 48f + Shake.X, this.positionDirect.Y - 16f + Shake.Y);
                this._rect = new Rectangle(0, 496, 64, 48);
                dg.DrawImage(dg, "bomber", this._rect, true, this._position, !this.rebirth, Color.White);
            }
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double num2 = positionDirect.Y - 28.0;
            shake = this.Shake;
            double y = shake.Y;
            double num3 = num2 + y;
            this._position = new Vector2((float)num1, (float)num3);
            this._rect = new Rectangle(this.hited ? 520 : 560, this.sp ? 112 : 0, 40, 56);
            dg.DrawImage(dg, "cirno", this._rect, true, this._position, this.rebirth, Color.White);
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
            if (!this.powerUP)
            {
                this.hited = true;
                this.hitting = false;
                this.ShakeStart(4, 8);
            }
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, ChipBase.ELEMENT.aqua));
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            if (!this.powerUP)
            {
                this.hited = true;
                this.hitting = false;
                this.ShakeStart(4, 8);
            }
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, ChipBase.ELEMENT.aqua));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            if (!this.powerUP)
            {
                this.hited = true;
                this.hitting = false;
                this.ShakeStart(4, 8);
            }
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, ChipBase.ELEMENT.aqua));
            return true;
        }
    }
}
