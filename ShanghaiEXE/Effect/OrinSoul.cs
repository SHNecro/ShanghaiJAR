using NSAttack;
using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class OrinSoul : EffectBase
    {
        private const byte _speed = 4;
        public bool Bomb;
        public int z;
        public AttackBase attack;
        private readonly int bombtime;
        private int palette;

        public OrinSoul(
          IAudioEngine s,
          SceneBattle p,
          Vector2 pd,
          Point posi,
          int z,
          AttackBase attack,
          int bombtime)
          : base(s, p, posi.X, posi.Y)
        {
            this.z = z;
            this.attack = attack;
            this.bombtime = bombtime;
            this.upprint = true;
            this.speed = 4;
            this.positionDirect = pd;
        }

        public OrinSoul(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          int z,
          AttackBase attack,
          int bombtime, int pal)
          : base(s, p, pX, pY)
        {
            this.z = z;
            this.attack = attack;
            this.bombtime = bombtime;
            this.speed = 4;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 68);
            this.palette = pal;
        }

        public override void Updata()
        {
            this.animationpoint.X = this.frame;
            if (!this.Bomb)
            {
                if (this.frame > 4)
                    this.frame = 2;
            }
            else
            {
                if (this.frame < 4)
                    this.frame = 4;
                if (this.frame == 7)
                    this.parent.attacks.Add(this.attack);
                if (this.frame > 10)
                    this.flag = false;
            }
            this.FlameControl();
            if (this.waittime >= this.bombtime && !this.Bomb)
                this.Bomb = true;
            ++this.waittime;
        }

        public override void Render(IRenderer dg)
        {
            int adj = 0;
            if (this.palette == 2) { adj = 200; }
            if (this.palette == 3) { adj = 400; }
            if (this.palette == 4) { adj = 600; }

            this._rect = new Rectangle(this.animationpoint.X * 50+(50*8), 50*2+adj, 50, 50);
            //this._rect = new Rectangle(0, 0, 50, 50);
            this._position = new Vector2(this.positionDirect.X, this.positionDirect.Y - z);
            dg.DrawImage(dg, "OrinAttack1", this._rect, false, this._position, this.rebirth, Color.White);
        }

        public override void RenderDOWN(IRenderer dg)
        {
            this._rect = new Rectangle(384, 312, 32, 40);
            this._position = new Vector2(this.positionDirect.X, this.positionDirect.Y);
            dg.DrawImage(dg, "kikuriAttack", this._rect, false, this._position, this.rebirth, Color.White);
        }

        private Point MoveAnimation(int waittime)
        {
            int[] numArray1 = new int[9]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8
            };
            int[] numArray2 = new int[9]
            {
        0,
        1,
        2,
        1,
        2,
        1,
        2,
        1,
        0
            };
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
