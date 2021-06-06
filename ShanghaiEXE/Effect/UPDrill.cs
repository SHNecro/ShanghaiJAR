using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class UPDrill : EffectBase
    {
        private const byte _speed = 3;
        private int plusY;

        public UPDrill(IAudioEngine s, SceneBattle p, int pX, int pY)
          : base(s, p, pX, pY)
        {
            this.upprint = true;
            this.speed = 3;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 70);
        }

        public override void Updata()
        {
            this.plusY += 8;
            if (positionDirect.Y - (double)this.plusY < -56.0)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 56, 1056, 56, 56);
            this._position = new Vector2(this.positionDirect.X, this.positionDirect.Y - plusY);
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
