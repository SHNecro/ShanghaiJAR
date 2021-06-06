using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class Charge : EffectBase
    {
        public int chargeEffect = -1;
        private const byte _speed = 1;

        public Charge(IAudioEngine s, SceneBattle p, Vector2 pd, Point posi)
          : base(s, p, posi.X, posi.Y)
        {
            this.upprint = true;
            this.speed = 1;
            this.positionDirect = pd;
        }

        public Charge(IAudioEngine s, SceneBattle p, int pX, int pY)
          : base(s, p, pX, pY)
        {
            this.upprint = true;
            this.speed = 1;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 40);
        }

        public override void Updata()
        {
            switch (this.frame)
            {
                case 1:
                    this.sound.PlaySE(SoundEffect.charge);
                    this.chargeEffect = 1;
                    break;
                case 25:
                    this.chargeEffect = 2;
                    break;
                case 50:
                    this.flag = false;
                    break;
            }
            this.FlameControl(1);
        }

        public override void Render(IRenderer dg)
        {
            if (this.chargeEffect == 1)
            {
                this._rect = new Rectangle(this.frame % 16 / 2 * 64, 0, 64, 64);
                this._position = this.positionDirect;
                dg.DrawImage(dg, "charge", this._rect, false, this._position, Color.FromArgb(200, byte.MaxValue, byte.MaxValue, byte.MaxValue));
            }
            else
            {
                if (this.chargeEffect != 2)
                    return;
                this._rect = new Rectangle(this.frame % 16 / 2 * 64, 64, 64, 64);
                this._position = this.positionDirect;
                dg.DrawImage(dg, "charge", this._rect, false, this._position, Color.FromArgb(200, byte.MaxValue, byte.MaxValue, byte.MaxValue));
            }
        }

        private Point MoveAnimation(int waittime)
        {
            int[] numArray1 = new int[3] { 0, 1, 2 };
            int[] numArray2 = new int[3] { 8, 9, 10 };
            int y = 0;
            int index1 = 0;
            for (int index2 = 1; index2 < numArray1.Length; ++index2)
            {
                if (waittime <= numArray1[index2] && waittime > numArray1[index2 - 1])
                    index1 = index2;
            }
            if (index1 != numArray2.Length)
                return new Point(numArray2[index1], y);
            return new Point(0, 0);
        }
    }
}
