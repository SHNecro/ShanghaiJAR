using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSObject
{
    internal class Scissor : ObjectBase
    {
        private bool breaked;

        public Scissor(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR union,
          int HP,
          bool sp)
          : base(s, p, pX, pY, union)
        {
            this.height = 160;
            this.wide = 144;
            this.hp = HP;
            this.hitPower = 200;
            this.animationpoint.Y = sp ? 2 : 0;
            this.hpmax = this.hp;
            if (union == Panel.COLOR.red)
                this.rebirth = true;
            this.unionhit = true;
            this.overslip = true;
            this.positionre = this.position;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 64);
        }

        public override void Updata()
        {
            base.Updata();
        }

        public override void Break()
        {
            if (!this.breaked || this.StandPanel.Hole)
            {
                this.breaked = true;
                this.sound.PlaySE(SoundEffect.clincher);
                this.parent.effects.Add(new Elementhit(this.sound, this.parent, this.position.X, this.position.Y, 2, ChipBase.ELEMENT.leaf));
            }
            this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            if (this.whitetime <= 0)
                this._rect = new Rectangle(800, 432 + this.animationpoint.Y * 144, 160, 144);
            else
                this._rect = new Rectangle(800, 576, 160, 144);
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            dg.DrawImage(dg, "ScissorMan", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
