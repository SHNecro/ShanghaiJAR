using NSAttack;
using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class ElekiFangSub : EffectBase
    {
        private new readonly int speed = 2;
        private readonly ElekiFang ef;
        private new readonly bool rebirth;
        private readonly bool up;

        public ElekiFangSub(IAudioEngine s, SceneBattle p, bool up, ElekiFang ef)
          : base(s, p, ef.position.X, ef.position.Y + (up ? -1 : 1))
        {
            this.ef = ef;
            this.up = up;
            if (this.position.X < 0)
                this.position.X = 0;
            if (this.position.X > 5)
                this.position.X = 5;
            if (this.position.Y < 0)
            {
                this.position.Y = 0;
                this.downprint = true;
            }
            if (this.position.Y <= 2)
                return;
            this.position.Y = 2;
            this.upprint = true;
        }

        public override void Updata()
        {
            if (this.ef.flag)
                return;
            this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            int y1 = this.ef.animation ? 1208 : 1136;
            if (!this.up)
                y1 += 48;
            double x1 = ef.positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y2 = ef.positionDirect.Y;
            shake = this.Shake;
            double y3 = shake.Y;
            double num2 = y2 + y3 + (this.up ? -24.0 : 24.0);
            this._position = new Vector2((float)num1, (float)num2);
            this._rect = new Rectangle(0, y1, 40, 24);
            dg.DrawImage(dg, "shot", this._rect, true, this._position, this.ef.rebirth, Color.White);
            for (int index = 1; index < 6; ++index)
            {
                double x3 = ef.positionDirect.X;
                shake = this.Shake;
                double x4 = shake.X;
                double num3 = x3 + x4 + 40 * this.ef.UnionRebirth * index;
                double y4 = ef.positionDirect.Y;
                shake = this.Shake;
                double y5 = shake.Y;
                double num4 = y4 + y5 + (this.up ? -24.0 : 24.0);
                this._position = new Vector2((float)num3, (float)num4);
                this._rect = new Rectangle(40, y1, 40, 24);
                dg.DrawImage(dg, "shot", this._rect, true, this._position, this.ef.rebirth, Color.White);
            }
        }
    }
}
