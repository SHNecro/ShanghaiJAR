using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class Shock : EffectBase
    {
        public Shock(IAudioEngine s, SceneBattle p, Vector2 pd, int sp, Point posi, Panel.COLOR u = Panel.COLOR.blue)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = sp;
            this.positionDirect = pd;
            if (u != Panel.COLOR.red)
                return;
            this.rebirth = true;
        }

        public Shock(IAudioEngine s, SceneBattle p, int pX, int pY, int sp, Panel.COLOR u = Panel.COLOR.blue)
          : base(s, p, pX, pY)
        {
            this.speed = sp;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 72);
            if (u != Panel.COLOR.red)
                return;
            this.rebirth = true;
        }

        public override void Updata()
        {
            this.FlameControl(this.speed);
            if (!this.moveflame)
                return;
            this.animationpoint.X = this.frame;
            if (this.animationpoint.X >= 6)
                this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(56 * this.animationpoint.X, 1040, 56, 48);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this.color = Color.White;
            dg.DrawImage(dg, "shot", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
