using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class DreamMeteo : EffectBase
    {
        private const byte _speed = 3;
        private int hittime;

        public DreamMeteo(
          IAudioEngine s,
          SceneBattle p,
          Vector2 pd,
          Point posi,
          int hittime,
          bool rebirth)
          : base(s, p, posi.X, posi.Y)
        {
            this.hittime = hittime;
            this.upprint = true;
            this.speed = 3;
            this.positionDirect = pd;
            this.rebirth = rebirth;
        }

        public DreamMeteo(IAudioEngine s, SceneBattle p, int pX, int pY, int hittime, bool rebirth)
          : base(s, p, pX, pY)
        {
            this.hittime = hittime;
            this.upprint = true;
            this.speed = 3;
            this.positionDirect = new Vector2(pX * 40, pY * 24 + 58);
            this.rebirth = rebirth;
        }

        public override void Updata()
        {
            if (this.moveflame)
            {
                --this.hittime;
                ++this.frame;
            }
            if (this.hittime <= 0)
                this.flag = false;
            this.FlameControl(1);
        }

        public override void Render(IRenderer dg)
        {
            if (positionDirect.Y - (double)(this.hittime * 8) < -40.0)
                return;
            double num1 = positionDirect.X - (double)(this.hittime * 8 * this.UnionRebirth);
            Point shake = this.Shake;
            double x = shake.X;
            double num2 = num1 + x;
            double num3 = positionDirect.Y - (double)(this.hittime * 8);
            shake = this.Shake;
            double y = shake.Y;
            double num4 = num3 + y;
            this._position = new Vector2((float)num2, (float)num4);
            this._rect = new Rectangle(0, 872, 48, 48);
            dg.DrawImage(dg, "darkPA", this._rect, true, this._position, this.rebirth, Color.White);
        }
    }
}
