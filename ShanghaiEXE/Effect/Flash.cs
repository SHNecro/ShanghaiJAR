using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class Flash : EffectBase
    {
        private const int interval = 4;
        private const int oneflash = 8;

        public Flash(IAudioEngine s, SceneBattle p, Vector2 pd, Point posi)
          : base(s, p, posi.X, posi.Y)
        {
            this.positionDirect = pd;
        }

        public Flash(IAudioEngine s, SceneBattle p, int pX, int pY)
          : base(s, p, pX, pY)
        {
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 70);
        }

        public override void Updata()
        {
            this.animationpoint.X = this.frame;
            if (this.frame >= 20)
                this.flag = false;
            this.FlameControl(2);
        }

        public override void Render(IRenderer dg)
        {
            Vector2 vector2 = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 48, 0, 48, 48);
            this._position = vector2;
            this.color = Color.White;
            dg.DrawImage(dg, "plugineffect", this._rect, false, this._position, this.rebirth, this.color);
            if (this.frame <= 8)
            {
                this._rect = new Rectangle(this.animationpoint.X * 104, 48, 104, 104);
                this._position = new Vector2(vector2.X + 52f, vector2.Y + 52f);
                this.color = Color.White;
                dg.DrawImage(dg, "plugineffect", this._rect, false, this._position, false, this.color);
            }
            if (this.frame <= 12 && this.frame > 4)
            {
                this._rect = new Rectangle((this.animationpoint.X - 4) * 104, 48, 104, 104);
                this._position = new Vector2(vector2.X - 52f, vector2.Y + 52f);
                this.color = Color.White;
                dg.DrawImage(dg, "plugineffect", this._rect, false, this._position, true, this.color);
            }
            if (this.frame <= 16 && this.frame > 8)
            {
                this._rect = new Rectangle((this.animationpoint.X - 8) * 104, 152, 104, 104);
                this._position = new Vector2(vector2.X - 52f, vector2.Y - 52f);
                this.color = Color.White;
                dg.DrawImage(dg, "plugineffect", this._rect, false, this._position, true, this.color);
            }
            if (this.frame > 20 || this.frame <= 12)
                return;
            this._rect = new Rectangle((this.animationpoint.X - 12) * 104, 152, 104, 104);
            this._position = new Vector2(vector2.X + 52f, vector2.Y - 52f);
            this.color = Color.White;
            dg.DrawImage(dg, "plugineffect", this._rect, false, this._position, false, this.color);
        }
    }
}
