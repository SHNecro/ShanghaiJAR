using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class YoumuShadow : EffectBase
    {
        private readonly int angle;
        public bool jittaika;
        private const byte _speed = 4;
        private bool reset;

        public YoumuShadow(IAudioEngine s, SceneBattle p, int pX, int pY, int angle)
          : base(s, p, pX, pY)
        {
            this.angle = angle;
            this.speed = 4;
            this.positionDirect = new Vector2(pX * 40 + 4, pY * 24 + 48);
            switch (angle)
            {
                case 0:
                    this.positionDirect.X += 32f;
                    break;
                case 2:
                case 3:
                    this.positionDirect.X += 8f;
                    break;
            }
            this.waittime = 3;
            this.sound.PlaySE(SoundEffect.lance);
        }

        public override void Updata()
        {
            switch (this.angle)
            {
                case 0:
                    this.animationpoint = this.MoveAnimation0(this.waittime);
                    this.rebirth = true;
                    break;
                case 1:
                    this.animationpoint = this.MoveAnimation0(this.waittime);
                    break;
                case 2:
                    this.animationpoint = this.MoveAnimation2(this.waittime);
                    break;
                case 3:
                    this.animationpoint = this.MoveAnimation3(this.waittime);
                    break;
            }
            if (this.moveflame)
            {
                if (!this.reset && this.jittaika)
                {
                    this.waittime = -1;
                    this.speed = 4;
                    this.reset = true;
                }
                if (this.jittaika)
                {
                    if (this.waittime == 1)
                        this.speed = 20;
                    else if (this.waittime == 2)
                        this.speed = 4;
                    if (this.waittime > 5)
                        this.flag = false;
                }
                ++this.waittime;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 104, this.animationpoint.Y * 96, 104, 96);
            this._position = this.positionDirect;
            dg.DrawImage(dg, "youmu", this._rect, false, this._position, this.rebirth, Color.White);
        }

        private Point MoveAnimation0(int waittime)
        {
            int[] numArray1 = new int[5] { 0, 1, 2, 3, 100 };
            int[] numArray2 = new int[5] { 2, 3, 3, 2, -1 };
            int y = 4;
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

        private Point MoveAnimation2(int waittime)
        {
            int[] numArray1 = new int[5] { 0, 1, 2, 3, 100 };
            int[] numArray2 = new int[5] { 4, 5, 6, 4, -1 };
            int y = 4;
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

        private Point MoveAnimation3(int waittime)
        {
            int[] numArray1 = new int[5] { 0, 1, 2, 3, 100 };
            int[] numArray2 = new int[5] { 7, 8, 9, 7, -1 };
            int y = 4;
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
