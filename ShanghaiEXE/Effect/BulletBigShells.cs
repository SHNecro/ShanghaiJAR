using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class BulletBigShells : EffectBase
    {
        private const byte _speed = 1;
        private readonly int pZ;
        private readonly int time;
        private int spin;
        private readonly int count;
        private readonly Bound bound;

        private int Spin
        {
            set
            {
                this.spin = value;
                if (this.spin < 10)
                    return;
                this.spin = 0;
            }
            get
            {
                return this.spin;
            }
        }

        public BulletBigShells(
          IAudioEngine s,
          SceneBattle p,
          Point position,
          float pX,
          float pY,
          int pZ,
          Panel.COLOR union,
          int time,
          int count,
          int spin)
          : base(s, p, position.X, position.Y)
        {
            this.count = count;
            this.spin = spin;
            this.time = time;
            this.union = union;
            this.speed = 1;
            this.positionDirect = new Vector2(pX, pY);
            if (union == Panel.COLOR.red)
                this.rebirth = true;
            this.bound = new Bound(new Vector2(pX, pY), new Vector2(pX - (4 + 8 * count) * this.UnionRebirth, pY + pZ), time);
        }

        public override void Updata()
        {
            this.positionDirect = this.bound.Update(this.positionDirect);
            ++this.Spin;
            if (this.frame > this.time)
            {
                if (this.count > 0)
                    this.parent.effects.Add(new BulletBigShells(this.sound, this.parent, this.position, this.positionDirect.X, this.positionDirect.Y, 0, this.union, this.time / 2, this.count - 1, this.spin));
                this.flag = false;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.spin * 16, 784, 16, 16);
            this._position = this.positionDirect;
            this._position.Y -= this.bound.plusy / 2f;
            dg.DrawImage(dg, "shot", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
