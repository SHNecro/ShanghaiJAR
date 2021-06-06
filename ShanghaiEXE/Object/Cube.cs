using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSObject
{
    internal class Cube : ObjectBase
    {
        private bool breaked;

        public Cube(IAudioEngine s, SceneBattle p, int pX, int pY, Panel.COLOR union)
          : base(s, p, pX, pY, union)
        {
            this.height = 48;
            this.wide = 32;
            this.hp = 200;
            this.hitPower = 200;
            this.hpmax = this.hp;
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
                this.sound.PlaySE(SoundEffect.breakObject);
                this.parent.effects.Add(new BreakCube(this.sound, this.parent, this.position, this.positionDirect.X, this.positionDirect.Y - 12f, 12, this.union, 20, true, 0));
                this.parent.effects.Add(new BreakCube(this.sound, this.parent, this.position, this.positionDirect.X, this.positionDirect.Y - 12f, 12, this.union, 20, false, 0));
            }
            this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            if (this.whitetime <= 0)
                this._rect = new Rectangle(0, 0, 32, 48);
            else
                this._rect = new Rectangle(0, 96, 32, 48);
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            dg.DrawImage(dg, "objects1", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
