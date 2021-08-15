using NSAttack;
using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSEnemy
{
    internal class DammyEnemy : EnemyBase
    {
        public bool nomove = false;
        public EnemyBase MainEnemy;
        private const int HP = 100000;
        public Point slidePosition;

        public DammyEnemy(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          EnemyBase MainEnemy,
          bool effect)
          : base(s, p, MainEnemy.position.X, MainEnemy.position.Y, (byte)MainEnemy.number, MainEnemy.union, MainEnemy.version)
        {
            this.slidePosition = new Point(pX, pY);
            this.position = new Point(MainEnemy.position.X + this.slidePosition.X, MainEnemy.position.Y + this.slidePosition.Y);
            this.MainEnemy = MainEnemy;
            this.effecting = effect;
            this.hpmax = 100000;
            this.hp = 100000;
            this.HPposition.X = -100f;
            this.positionDirect = MainEnemy.positionDirect;
            this.noslip = true;
            this.dropchips = MainEnemy.dropchips;
            this.race = MainEnemy.race;
            this.ID = MainEnemy.ID;
        }

        public override void Dameged(AttackBase attack)
        {
            if (attack is Dummy)
            {
                return;
            }

            this.MainEnemy.Hp -= 100000 - this.hp;
            this.hp = 100000;
            this.MainEnemy.whitetime = this.whitetime;
            base.Dameged(attack);
        }

        public override void Updata()
        {
            if (this.hp < 100000)
            {
                this.MainEnemy.Hp -= 100000 - this.hp;
                this.hp = 100000;
            }
            if (!this.nomove)
            {
                this.position = new Point(this.MainEnemy.position.X + this.slidePosition.X, this.MainEnemy.position.Y + this.slidePosition.Y);
                this.positionre = this.position;
                this.positionDirect = this.MainEnemy.positionDirect;
            }
            this.counterTiming = this.MainEnemy.counterTiming;
            this.element = this.MainEnemy.Element;
            this.guard = this.MainEnemy.guard;
            this.badstatus = this.MainEnemy.badstatus;
            this.badstatustime = this.MainEnemy.badstatustime;
            this.flag = this.MainEnemy.flag;
        }

        public override void PositionDirectSet()
        {
        }

        protected override void Moving()
        {
        }

        public override void Render(IRenderer dg)
        {
        }
    }
}

