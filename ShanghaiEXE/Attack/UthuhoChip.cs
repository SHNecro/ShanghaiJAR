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
    internal class UthuhoChip : AttackBase
    {
        private const int hit = 8;
        private readonly int movespeed;
        private readonly bool powerUP;
        private bool anime;
        private readonly int sp;

        public UthuhoChip(IAudioEngine so, SceneBattle p, int pX, int pY, Panel.COLOR u, int po, int sp)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.heat)
        {
            if (!this.flag)
                return;
            this.sp = sp;
            this.invincibility = true;
            this.movespeed = 8;
            this.speed = 1;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union != Panel.COLOR.blue;
            this.positionre = this.position;
            this.positionDirect = new Vector2(position.X * 40f, (float)(position.Y * 24.0 + 22.0));
            this.frame = 0;
            this.OldPD = this.positionDirect;
            this.panelChange = true;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            this.PanelBright();

            this.positionDirect.X += this.movespeed * this.UnionRebirth;
            var oldX = this.position.X;
            var newX = this.Calcposition(this.positionDirect, 56, false).X;
            if (newX != oldX)
            {
                this.PanelChange();
            }
            this.position.X = newX;

            if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                this.flag = false;
            if (this.moveflame)
                this.anime = !this.anime;
            this.FlameControl(2);
        }

        public override void Render(IRenderer dg)
        {
            if (this.over || (!this.flag || !this.breaking))
                return;
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2 + 64 * this.UnionRebirth;
            double num2 = positionDirect.Y - 28.0;
            shake = this.Shake;
            double y1 = shake.Y;
            double num3 = num2 + y1 - 32.0;
            this._position = new Vector2((float)num1, (float)num3);
            int y2 = 432;
            var pictureName = "Uthuho";

			switch (this.sp)
			{
                case 1:
					y2 = 2592;
                    break;
				case 2:
					y2 = 1872;
                    break;
				case 3:
					y2 = 2592;
                    pictureName = "UthuhoAlter";
					break;
			}
            this._rect = new Rectangle(this.anime ? 360 : 480, y2, 120, 144);
			dg.DrawImage(dg, pictureName, this._rect, true, this._position, this.rebirth, Color.White);
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
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, ChipBase.ELEMENT.heat));
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, ChipBase.ELEMENT.heat));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, ChipBase.ELEMENT.heat));
            return true;
        }
    }
}
