using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class KnifeAfter : EffectBase
    {
        private const byte _speed = 2;
        private bool flash;

        public KnifeAfter(IAudioEngine s, SceneBattle p, Vector2 pd, Point posi, Panel.COLOR union)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = 2;
            this.downprint = true;
            this.positionDirect = pd;
            this.rebirth = union != Panel.COLOR.blue;
        }

        public KnifeAfter(IAudioEngine s, SceneBattle p, int pX, int pY, Panel.COLOR union)
          : base(s, p, pX, pY)
        {
            this.speed = 2;
            this.downprint = true;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 80);
            this.rebirth = union != Panel.COLOR.blue;
        }

        public override void Updata()
        {
            if (this.StandPanel.Hole)
                this.flag = false;
            if (this.moveflame)
            {
                if (this.animationpoint.X < 3)
                    ++this.animationpoint.X;
                if (this.frame >= 4)
                    this.flash = !this.flash;
                if (this.frame > 10)
                    this.flag = false;
            }
            this.FlameControl(2);
        }

        public override void Render(IRenderer dg)
        {
            if (this.flash)
                return;
            this._rect = new Rectangle(256 + 32 * this.animationpoint.X, 176, 32, 32);
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            dg.DrawImage(dg, "sword", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
