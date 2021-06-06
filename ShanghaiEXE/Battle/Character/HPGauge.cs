using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSBattle.Character
{
    public class HPGauge : AllBase
    {
        public int hpprint;
        public int hpmax;
        public int hp;

        public HPGauge(IAudioEngine s, int now, int max)
          : base(s)
        {
            this.hpmax = max;
            this.hp = now;
            this.hpprint = now;
        }

        public void HPRender(IRenderer dg, Vector2 p)
        {
            int[] numArray = this.HPcount(this.hpprint);
            Color color = hpmax * 0.3 <= hp && this.hp >= this.hpprint ? (this.hp <= this.hpprint ? Color.White : Color.FromArgb(byte.MaxValue, 50, byte.MaxValue, 150)) : Color.FromArgb(byte.MaxValue, byte.MaxValue, 150, 50);
            for (int index = 0; index < numArray.Length; ++index)
            {
                this._rect = new Rectangle(numArray[index] * 8, 0, 8, 16);
                this._position = new Vector2(p.X - index * 8, p.Y);
                dg.DrawImage(dg, "font", this._rect, false, this._position, color);
            }
        }

        public void HPDown(int now, int max)
        {
            this.hpmax = max;
            this.hp = now;
            if (this.hpprint < this.hp)
            {
                if (this.hp - this.hpprint < 300)
                    this.hpprint += 2;
                else
                    this.hpprint += 15;
                if (this.hpprint <= this.hp)
                    return;
                this.hpprint = this.hp;
            }
            else
            {
                if (this.hpprint <= this.hp)
                    return;
                if (this.hpprint - this.hp < 300)
                    this.hpprint -= 2;
                else
                    this.hpprint -= 15;
                if (this.hpprint < this.hp)
                    this.hpprint = this.hp;
            }
        }

        protected int[] HPcount(int hp)
        {
            int length = hp.ToString().Length;
            int[] numArray = new int[length];
            for (int b = 0; b < length; ++b)
            {
                int num = (int)MyMath.Pow(10f, b);
                numArray[b] = hp / num % 10;
            }
            return numArray;
        }
    }
}
