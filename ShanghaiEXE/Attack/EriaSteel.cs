using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSObject;
using SlimDX;
using System.Drawing;

namespace NSAttack
{
    internal class EriaSteel : AttackBase
    {
        private bool hitedflag = false;
        private const int settime = 1800;
        private const int steeltime = 20;

        public EriaSteel(
          MyAudio so,
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
            this.upprint = false;
            this.animationpoint.X = 0;
            this.speed = 2;
            this.hitrange = new Point(0, 0);
            this.hitting = false;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 - 110);
            this.rehit = true;
            this.frame = 0;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            this.PanelBright(true);
            switch (this.frame)
            {
                case 0:
                    this.sound.PlaySE(MyAudio.SOUNDNAMES.eriasteal1);
                    break;
                case 20:
                    this.hitting = true;
                    this.sound.PlaySE(MyAudio.SOUNDNAMES.eriasteal2);
                    this.parent.effects.Add(new AfterSteal(this.sound, this.parent, this.position.X, this.position.Y));
                    break;
            }
            if (this.frame < 20)
            {
                this.positionDirect.Y += 9f;
                ++this.frame;
            }
            else
                this.FlameControl();
            if (this.frame == 21)
            {
                if (!this.hitedflag && !this.parent.panel[this.position.X, this.position.Y].inviolability && this.NoObject(this.position))
                {
                    this.parent.panel[this.position.X, this.position.Y].bashed = true;
                    this.parent.panel[this.position.X, this.position.Y].color = this.union;
                    this.parent.bashtime = 1800;
                }
                this.flag = false;
            }
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.frame % 4 * 32, 0, 32, 32);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, "steal", this._rect, false, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (this.hitedflag || !base.HitCheck(charaposition, charaunion))
                return false;
            this.hitedflag = true;
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (this.hitedflag || !base.HitCheck(charaposition))
                return false;
            this.hitedflag = true;
            return true;
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
