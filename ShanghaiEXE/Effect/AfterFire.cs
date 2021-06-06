using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class AfterFire : EffectBase
    {
        private new readonly int speed = 2;
        private new readonly bool rebirth;

        public AfterFire(
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
            this.frame = 3;
            this.color = Color.White;
        }

        public override void Updata()
        {
            this.animationpoint.X = this.frame;
            this.FlameControl(this.speed);
            if (this.frame <= 8)
                return;
            this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 64, 664 + 48 * (int)this.element, 64, 48);
            dg.DrawImage(dg, "towers", this._rect, true, this._position, !this.rebirth, this.color);
        }
    }
}
