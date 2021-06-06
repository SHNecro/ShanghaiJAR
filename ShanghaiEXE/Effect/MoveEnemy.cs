using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class MoveEnemy : EffectBase
    {
        private const byte _speed = 2;

        public MoveEnemy(IAudioEngine s, SceneBattle p, Vector2 pd, Point posi)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = 2;
            this.positionDirect = pd;
        }

        public MoveEnemy(IAudioEngine s, SceneBattle p, int pX, int pY)
          : base(s, p, pX, pY)
        {
            this.speed = 2;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 72);
        }

        public override void Updata()
        {
            this.animationpoint.X = this.frame;
            if (this.frame >= 4)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 24, 216, 24, 40);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, "bomber", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
