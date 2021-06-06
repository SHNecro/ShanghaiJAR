using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Drawing;

namespace NSAttack
{
    internal class BugBall : AttackBase
    {
        private bool movestart = false;
        private const int plusy = 70;
        private readonly int movespeed;

        public BugBall(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          ChipBase.ELEMENT ele,
          int movespeed = 4)
          : base(so, p, pX, pY, u, po, ele)
        {
            this.movespeed = movespeed;
            this.invincibility = true;
            this.breaking = true;
            this.speed = s;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 64);
            this.OldPD = this.positionDirect;
        }

        public override void Updata()
        {
            this.PanelBright();
            if (this.frame >= 3 && !this.movestart)
            {
                this.movestart = true;
                this.hitting = true;
            }
            if (this.moveflame)
            {
                if (this.frame % 2 == 0)
                {
                    ++this.animationpoint.X;
                    if (this.animationpoint.X > 3)
                        this.animationpoint.X = 0;
                }
                if (this.movestart)
                {
                    this.positionDirect.X += this.union == Panel.COLOR.red ? movespeed : -this.movespeed;
                    this.position = this.Calcposition(this.positionDirect, 36, false);
                    if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                        this.flag = false;
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(32 * this.animationpoint.X, 1384, 32, 40);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, "shot", this._rect, false, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            this.flag = false;
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;
            this.flag = false;
            return true;
        }

        private void Hit()
        {
            this.ShakeStart(5, 8);
            this.sound.PlaySE(SoundEffect.bombmiddle);
            this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X - 1, this.position.Y, this.union, this.power, 1, this.element)));
            this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.power, 1, this.element)));
            this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + 1, this.position.Y, this.union, this.power, 1, this.element)));
            this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.power, 1, this.element)));
            this.parent.effects.Add(new BugHoleDead(this.sound, this.parent, new Point(this.position.X - 1, this.position.Y)));
            this.parent.effects.Add(new BugHoleDead(this.sound, this.parent, new Point(this.position.X, this.position.Y - 1)));
            this.parent.effects.Add(new BugHoleDead(this.sound, this.parent, new Point(this.position.X + 1, this.position.Y)));
            this.parent.effects.Add(new BugHoleDead(this.sound, this.parent, new Point(this.position.X, this.position.Y + 1)));
        }

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p))
                return false;
            this.Hit();
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.Hit();
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.Hit();
            return true;
        }
    }
}
