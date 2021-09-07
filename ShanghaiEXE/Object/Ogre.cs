using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSObject
{
    internal class Ogre : ObjectBase
    {
        private readonly int wait = 30;
        private readonly int resettime = 60;
        private readonly int[] anime = new int[15]
        {
      0,
      1,
      2,
      3,
      4,
      5,
      5,
      5,
      5,
      5,
      4,
      3,
      2,
      1,
      0
        };
        private bool counter;
        public bool wave;
        private bool UpDown;
        private readonly int movetime;
        private readonly int movespeed;
        private bool stop;
        private readonly bool breaked;
        private Point target;
        private Tower tower;
        private readonly bool red;
        private readonly bool sp;
        private bool flash;

        public Ogre(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          int wait,
          Panel.COLOR union,
          int power,
          bool red,
          bool sp)
          : base(s, p, pX, pY, union)
        {
            this.red = red;
            this.sp = sp;
            this.wide = 128;
            this.height = 96;
            this.hp = 20000;
            this.hitPower = power;
            this.wait = wait;
            this.speed = 2;
            this.movespeed = 1;
            this.hpmax = this.hp;
            this.unionhit = false;
            this.overslip = false;
            this.effecting = true;
            this.noslip = true;
            if (this.position.Y == 2)
                this.UpDown = true;
            this.guard = CharacterBase.GUARD.guard;
            this.positionre = this.position;
            this.animationpoint.X = red ? 12 : 13;
            this.positionDirect = new Vector2(pX * 40, pY * 24 + 44);
        }

        public override void Updata()
        {
            if (this.counter && this.stop)
            {
                switch (this.frame)
                {
                    case 30:
                        this.flash = false;
                        this.tower = new Tower(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.hitPower, 16, this.red ? ChipBase.ELEMENT.heat : ChipBase.ELEMENT.aqua);
                        this.parent.attacks.Add(tower);
                        break;
                    case 90:
                        this.counter = false;
                        this.hit = false;
                        this.stop = false;
                        this.frame = 0;
                        break;
                }
            }
            else if (this.SlideMove(1f / movespeed, this.UpDown ? 2 : 3))
            {
                this.PotisionSet();
                int num = this.UpDown ? -1 : 1;
                Point position = this.position;
                position.Y += num;
                if (!this.Canmove(position, this.number))
                    this.UpDown = !this.UpDown;
                if (this.counter || this.flash)
                {
                    this.counter = true;
                    this.flash = true;
                    this.frame = 0;
                    this.stop = true;
                }
            }
            if (this.hit)
            {
                this.counter = true;
                this.flash = true;
            }
            this.FlameControl(2);
            this.hp = 20000;
            base.Updata();
        }

        public override void Break()
        {
            this.flag = false;
            this.sound.PlaySE(SoundEffect.clincher);
            this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y, Bomber.BOMBERTYPE.bomber, 2));
        }

        public void PotisionSet()
        {
            this.positionDirect = new Vector2(this.position.X * 40, this.position.Y * 24 + 44);
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * this.Wide, this.sp ? this.Height * 2 : 0, this.Wide, this.Height);
            double num1 = positionDirect.X + (double)this.Shake.X;
            double y1 = positionDirect.Y;
            Point shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            dg.DrawImage(dg, "chen", this._rect, false, this._position, this.rebirth, Color.White);
            if (!this.flash || this.frame % 2 != 0)
                return;
            this._rect = new Rectangle(this.animationpoint.X * this.Wide, this.Height, this.Wide, this.Height);
            double x1 = positionDirect.X;
            shake = this.Shake;
            double x2 = shake.X;
            double num3 = x1 + x2;
            double y3 = positionDirect.Y;
            shake = this.Shake;
            double y4 = shake.Y;
            double num4 = y3 + y4;
            this._position = new Vector2((float)num3, (float)num4);
            dg.DrawImage(dg, "chen", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
