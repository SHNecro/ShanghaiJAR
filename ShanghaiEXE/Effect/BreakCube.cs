using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class BreakCube : EffectBase
    {
        private const byte _speed = 1;
        private readonly int pZ;
        private readonly int time;
        private readonly Bound bound;
        private readonly bool rightleft;
        private readonly int color;

        public BreakCube(
          IAudioEngine s,
          SceneBattle p,
          Point position,
          float pX,
          float pY,
          int pZ,
          Panel.COLOR union,
          int time,
          bool rightleft,
          int color)
          : base(s, p, position.X, position.Y)
        {
            this.rightleft = rightleft;
            this.color = color;
            this.time = time;
            this.union = union;
            this.speed = 1;
            this.positionDirect = new Vector2(pX, pY);
            if (union == Panel.COLOR.red)
                this.rebirth = true;
            this.bound = new Bound(new Vector2(pX, pY), new Vector2(pX - 32 * (!rightleft ? -1 : 1), pY + pZ), time);
        }

        public override void Updata()
        {
            this.positionDirect = this.bound.Update(this.positionDirect);
            if (this.frame > this.time)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(!this.rightleft ? 40 : 72, this.color * 48, !this.rightleft ? 32 : 24, 32);
            this._position = this.positionDirect;
            this._position.Y -= this.bound.plusy;
            dg.DrawImage(dg, "objects1", this._rect, false, this._position, !this.rebirth, Color.White);
        }
    }
}
