using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class Get : EffectBase
    {
        private const byte _speed = 3;

        public Get(IAudioEngine s, SceneBattle p, Vector2 pd, Point posi)
          : base(s, p, posi.X, posi.Y)
        {
            this.upprint = true;
            this.speed = 3;
            this.positionDirect = pd;
        }

        public Get(IAudioEngine s, SceneBattle p, int pX, int pY)
          : base(s, p, pX, pY)
        {
            this.upprint = true;
            this.speed = 3;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 70);
        }

        public override void Updata()
        {
            this.animationpoint = this.MoveAnimation(this.frame);
            if (this.frame > 4)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(144 + this.animationpoint.X * 48, 288, 48, 48);
            this._position = this.positionDirect;
            dg.DrawImage(dg, "bomber", this._rect, false, this._position, this.rebirth, Color.White);
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
