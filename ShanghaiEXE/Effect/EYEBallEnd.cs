using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class EYEBallEnd : EffectBase
    {
        private new readonly int speed = 2;
        private new readonly bool rebirth;

        public EYEBallEnd(
          IAudioEngine s,
          SceneBattle p,
          Vector2 pd,
          Point posi,
          ChipBase.ELEMENT ele,
          int _speed,
          bool rebirth)
          : base(s, p, posi.X, posi.Y)
        {
            this.rebirth = rebirth;
            this.element = ele;
            this.speed = _speed;
            this.positionDirect = pd;
            this.animationpoint.X = 3;
            this.frame = 0;
        }

        public override void Updata()
        {
            this.animationpoint.X = this.frame;
            this.FlameControl(this.speed * 3);
            if (this.frame <= 5)
                return;
            this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(96 + this.animationpoint.X * 32, 888, 32, 32);
            dg.DrawImage(dg, "bomber", this._rect, false, this._position, !this.rebirth, Color.White);
        }
    }
}
