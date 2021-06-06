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
    internal class PanelShoot : AttackBase
    {
        private const int hit = 8;
        private readonly int movespeed;

        public PanelShoot(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.invincibility = false;
            this.movespeed = 8;
            this.speed = 1;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40, this.position.Y * 24 + 64);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40, this.position.Y * 24 + 64);
            this.frame = 0;
            switch (this.StandPanel.state)
            {
                case Panel.PANEL._nomal:
                case Panel.PANEL._crack:
                    this.element = ChipBase.ELEMENT.normal;
                    break;
                case Panel.PANEL._grass:
                    this.element = ChipBase.ELEMENT.leaf;
                    this.power *= 2;
                    break;
                case Panel.PANEL._ice:
                    this.element = ChipBase.ELEMENT.aqua;
                    this.power *= 2;
                    break;
                case Panel.PANEL._sand:
                    this.element = ChipBase.ELEMENT.earth;
                    this.power *= 2;
                    break;
                case Panel.PANEL._poison:
                    this.element = ChipBase.ELEMENT.poison;
                    this.power *= 2;
                    break;
                case Panel.PANEL._burner:
                    this.element = ChipBase.ELEMENT.heat;
                    this.power *= 2;
                    break;
                case Panel.PANEL._thunder:
                    this.element = ChipBase.ELEMENT.eleki;
                    this.power *= 2;
                    break;
                default:
                    this.hitting = false;
                    this.flag = false;
                    break;
            }
            if (!this.StandPanel.Hole)
            {
                this.parent.effects.Add(new Shock(this.sound, this.parent, this.position.X, this.position.Y, 2, this.union));
                this.sound.PlaySE(SoundEffect.waveshort);
                if (!this.StandPanel.OnCharaCheck())
                {
                    this.StandPanel.State = Panel.PANEL._break;
                }
                else
                {
                    this.StandPanel.State = Panel.PANEL._crack;
                    this.hitting = false;
                    this.flag = false;
                }
            }
            if (this.union == Panel.COLOR.blue)
                this.movespeed *= -1;
            this.OldPD = this.positionDirect;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame;
                if (this.frame == 5)
                    this.frame = 0;
            }
            this.positionDirect.X += movespeed;
            this.position = this.Calcposition(this.positionDirect, 48, false);
            if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.over || !this.flag)
                return;
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double num2 = positionDirect.Y - 16.0;
            shake = this.Shake;
            double y = shake.Y;
            double num3 = num2 + y;
            this._position = new Vector2((float)num1, (float)num3);
            this._rect = new Rectangle((int)this.element * 40, 1088, 40, 48);
            dg.DrawImage(dg, "shot", this._rect, true, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            this.flag = false;
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;
            this.flag = false;
            return true;
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
    }
}
