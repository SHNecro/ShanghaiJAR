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
    internal class EYEBall : AttackBase
    {
        private bool movestart = false;
        private const int plusy = 70;
        private readonly int movespeed;

        public EYEBall(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          Vector2 v,
          ChipBase.ELEMENT ele,
          int movespeed = 4)
          : base(so, p, pX, pY, u, po, ele)
        {
            this.movespeed = movespeed;
            this.invincibility = true;
            this.breakinvi = true;
            this.breaking = true;
            this.speed = s;
            this.positionDirect = v;
            this.OldPD = this.positionDirect;
        }

        public override void Updata()
        {
            this.PanelBright();
            if (this.frame >= 3 && !this.movestart)
            {
                this.movestart = true;
                this.hitting = true;
                this.sound.PlaySE(SoundEffect.canon);
            }
            if (this.moveflame)
            {
                if (this.frame % 3 == 0)
                {
                    this.parent.effects.Add(new EYEBallEnd(this.sound, this.parent, this.positionDirect, this.position, this.element, this.speed, this.rebirth));
                    ++this.animationpoint.X;
                    if (this.animationpoint.X > 3)
                        this.animationpoint.X = 0;
                }
                if (this.movestart)
                {
                    this.positionDirect.X += this.union == Panel.COLOR.red ? movespeed : -this.movespeed;
                    this.position.X = this.Calcposition(this.positionDirect, 36, true).X;
                    if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                        this.flag = false;
                }
            }
            if (!this.movestart) { }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 32, 888, 32, 32);
            dg.DrawImage(dg, "bomber", this._rect, false, this._position, this.rebirth, Color.White);
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

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p))
                return false;
            this.ShakeStart(5, 8);
            this.sound.PlaySE(SoundEffect.bombmiddle);
            this.parent.effects.Add(new Bomber(this.sound, this.parent, p.position.X, p.position.Y, Bomber.BOMBERTYPE.flashbomber, 2));
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.ShakeStart(5, 8);
            this.sound.PlaySE(SoundEffect.bombmiddle);
            this.parent.effects.Add(new Bomber(this.sound, this.parent, e.position.X, e.position.Y, Bomber.BOMBERTYPE.flashbomber, 2));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.sound.PlaySE(SoundEffect.bombmiddle);
            this.parent.effects.Add(new Bomber(this.sound, this.parent, o.position.X, o.position.Y, Bomber.BOMBERTYPE.flashbomber, 2));
            return true;
        }
    }
}
