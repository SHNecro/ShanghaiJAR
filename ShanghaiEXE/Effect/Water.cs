using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class Water : EffectBase
    {
        public Water(IAudioEngine s, SceneBattle p, Vector2 pd, int sp, Point posi)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = sp;
            this.positionDirect = pd;
        }

        public Water(IAudioEngine s, SceneBattle p, int pX, int pY, int sp)
          : base(s, p, pX, pY)
        {
            this.speed = sp;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 64);
        }

        public override void Updata()
        {
            this.animationpoint.X = this.MoveAnimation(this.frame).X;
            if (this.frame >= 5)
                this.flag = false;
            this.FlameControl(this.speed);
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(56 * this.animationpoint.X, 1000, 56, 64);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this.color = Color.White;
            dg.DrawImage(dg, "towers", this._rect, false, this._position, this.rebirth, this.color);
        }

        private Point MoveAnimation(int waittime)
        {
            int[] numArray1 = new int[5] { 0, 1, 2, 3, 4 };
            int[] numArray2 = new int[5] { 0, 1, 2, 3, 1 };
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
