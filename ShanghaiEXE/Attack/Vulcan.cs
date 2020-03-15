using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSObject;
using System.Drawing;

namespace NSAttack
{
    internal class Vulcan : AttackBase
    {
        private readonly Vulcan.SHOT shot;
        private bool hit;

        public Vulcan(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          Vulcan.SHOT s,
          ChipBase.ELEMENT ele,
          bool bre)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.knock = true;
            this.invincibility = false;
            this.breaking = bre;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            this.shot = s;
            this.frame = 0;
            this.positionDirect.Y = 82 + 24 * this.position.Y;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.hit)
            {
                this.flag = false;
                switch (this.shot)
                {
                    case Vulcan.SHOT.Bubble:
                        this.sound.PlaySE(SoundEffect.bubble);
                        this.parent.effects.Add(new Bubblehit(this.sound, this.parent, this.position.X, this.position.Y, 2));
                        this.parent.effects.Add(new Bubblehit(this.sound, this.parent, this.position.X + (this.union == Panel.COLOR.red ? 1 : -1), this.position.Y, 2));
                        AttackBase attackBase1 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + (this.union == Panel.COLOR.red ? 1 : -1), this.position.Y, this.union, this.power, 1, this.element));
                        attackBase1.invincibility = false;
                        this.parent.attacks.Add(attackBase1);
                        break;
                    case Vulcan.SHOT.Vulcan:
                        this.parent.effects.Add(new GunHit(this.sound, this.parent, this.position.X, this.position.Y, this.union));
                        this.parent.effects.Add(new GunHit(this.sound, this.parent, this.position.X + (this.union == Panel.COLOR.red ? 1 : -1), this.position.Y, this.union));
                        AttackBase attackBase2 = this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + (this.union == Panel.COLOR.red ? 1 : -1), this.position.Y, this.union, this.power, 1, this.element));
                        attackBase2.invincibility = false;
                        this.parent.attacks.Add(attackBase2);
                        break;
                }
            }
            this.positionDirect.X = 40 * this.position.X + 20;
            if (this.frame <= 5 && this.frame > 0)
                this.position.X = this.union == Panel.COLOR.red ? this.position.X + 1 : this.position.X - 1;
            else if (this.frame > 6)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!this.flag || !base.HitCheck(charaposition, charaunion))
                return false;
            this.hit = true;
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!this.flag || !base.HitCheck(charaposition))
                return false;
            this.hit = true;
            return true;
        }

        public override bool HitEvent(Player p)
        {
            return base.HitEvent(p);
        }

        public override bool HitEvent(EnemyBase e)
        {
            return base.HitEvent(e);
        }

        public override bool HitEvent(ObjectBase o)
        {
            return base.HitEvent(o);
        }

        public enum SHOT
        {
            Bubble,
            Vulcan,
        }
    }
}
