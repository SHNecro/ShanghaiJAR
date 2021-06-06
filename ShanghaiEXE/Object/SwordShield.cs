using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSObject
{
    internal class SwordShield : ObjectBase
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
        private readonly bool breaked;
        private Point target;
        private Tower tower;

        public SwordShield(IAudioEngine s, SceneBattle p, int pX, int pY, int wait, Panel.COLOR union)
          : base(s, p, pX, pY, union)
        {
            this.height = 56;
            this.wide = 16;
            this.hp = 20000;
            this.hitPower = 200;
            this.wait = wait;
            this.speed = 3;
            this.hpmax = this.hp;
            this.unionhit = false;
            this.overslip = false;
            this.effecting = true;
            this.noslip = true;
            this.guard = CharacterBase.GUARD.guard;
            this.positionre = this.position;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 64);
        }

        public override void Updata()
        {
            if (this.nohit && !this.counter)
            {
                ++this.frame;
                if (!this.tower.flag && this.frame >= this.resettime)
                {
                    this.frame = 0;
                    this.nohit = false;
                }
            }
            else if (this.counter)
            {
                this.FlameControl(this.speed);
                ++this.frame;
                if (this.frame < this.anime.Length)
                    this.animationpoint.X = this.anime[this.frame];
                else
                    this.nohit = true;
                if (this.frame >= this.wait && this.animationpoint.X == 0)
                {
                    this.frame = 0;
                    this.tower = new Tower(this.sound, this.parent, this.target.X, this.target.Y, this.union, this.hitPower, this.wave ? 4 : 9999, ChipBase.ELEMENT.normal);
                    this.parent.attacks.Add(tower);
                    this.counter = false;
                }
            }
            else if (this.hit)
            {
                this.frame = 0;
                this.counter = true;
                this.target = this.RandomTarget();
                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target.X, this.target.Y, this.union, new Point(), this.wait, true));
            }
            this.hp = 20000;
            base.Updata();
        }

        public override void Break()
        {
            this.flag = false;
        }

        public void PotisionSet()
        {
            this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 56);
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * this.Wide, 448, this.Wide, this.Height);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, "towers", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
