using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSAttack
{
    internal class LeafMaker : AttackBase
    {
        private readonly List<Point> target = new List<Point>();
        private const byte _speed = 2;
        private int count;

        public LeafMaker(
          IAudioEngine s,
          SceneBattle p,
          Vector2 pd,
          Point posi,
          Panel.COLOR union,
          int power)
          : base(s, p, posi.X, posi.Y, union, power, ChipBase.ELEMENT.normal)
        {
            this.union = union;
            this.speed = 2;
            this.positionDirect = pd;
            this.power = power;
            this.hitting = false;
            int x = posi.X;
            int y = posi.Y;
            int num1 = 0;
            bool flag = false;
            int num2 = 0;
            while (this.InAreaCheck(new Point(x + num1 * this.UnionRebirth, y)))
            {
                this.target.Add(new Point(x + num1 * this.UnionRebirth, y));
                if (flag)
                {
                    if (y <= 0)
                    {
                        ++num1;
                        flag = !flag;
                    }
                    else
                        --y;
                }
                else if (y >= 2)
                {
                    ++num1;
                    flag = !flag;
                }
                else
                    ++y;
                ++num2;
            }
            this.position = new Point(8, 5);
        }

        public override void Updata()
        {
            if (this.parent.blackOut)
                return;
            if (this.count >= this.target.Count)
                this.flag = false;
            else if (this.frame % 5 == 0)
            {
                this.sound.PlaySE(SoundEffect.lance);
                List<AttackBase> attacks = this.parent.attacks;
                IAudioEngine sound = this.sound;
                SceneBattle parent = this.parent;
                Point point = this.target[this.count];
                int x = point.X;
                point = this.target[this.count];
                int y = point.Y;
                int union = (int)this.union;
                int power = this.power;
                AttackBase attackBase = this.StateCopy(new LeafWave(sound, parent, x, y, (Panel.COLOR)union, power, 1, 10));
                attacks.Add(attackBase);
                ++this.count;
                if (this.count >= this.target.Count || this.parent.panel[x, y].Hole)
                    this.flag = false;
            }
            ++this.frame;
        }

        public override void Render(IRenderer dg)
        {
        }
    }
}
