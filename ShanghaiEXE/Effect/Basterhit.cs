using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class Basterhit : EffectBase
    {
        public Basterhit(IAudioEngine s, SceneBattle p, Vector2 pd, int sp, Point posi)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = sp;
            this.positionDirect = pd;
            this.positionDirect.X += this.Random.Next(-5, 5);
            this.positionDirect.Y += this.Random.Next(-5, 5);
        }

        public Basterhit(IAudioEngine s, SceneBattle p, int pX, int pY, int sp)
          : base(s, p, pX, pY)
        {
            this.speed = sp;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 70);
            this.positionDirect.X += this.Random.Next(-5, 5);
            this.positionDirect.Y += this.Random.Next(-5, 5);
        }

        public override void Updata()
        {
            this.animationpoint.X = this.frame;
            if (this.frame >= 5)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 32, 144, 32, 24);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, "shot", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
