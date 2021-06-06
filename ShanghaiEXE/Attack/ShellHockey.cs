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
    internal class ShellHockey : AttackBase
    {
        private bool movestart = false;
        private int manymove = 8;
        private const int plusy = 70;
        private const int sp = 8;
        private Vector2 movespeed;
        private bool angleDOWN;
        private bool angleLEFT;
        private bool enemyaArea;
        private int refrect;

        public ShellHockey(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            this.movespeed.X = 5f;
            this.movespeed.Y = 3f;
            this.picturename = "shot";
            this.invincibility = false;
            this.breakinvi = true;
            this.breaking = true;
            this.speed = s;
            this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 74);
        }

        public override void Updata()
        {
            if (!this.enemyaArea && this.StandPanel.color == this.UnionEnemy)
                this.enemyaArea = true;
            this.PanelBright();
            if (!this.movestart)
            {
                this.movestart = true;
                this.hitting = true;
                this.sound.PlaySE(SoundEffect.canon);
                this.parent.effects.Add(new EYEBallEnd(this.sound, this.parent, this.positionDirect, this.position, this.element, this.speed, this.rebirth));
            }
            if (this.moveflame && this.frame % 2 == 0)
            {
                ++this.animationpoint.X;
                if (this.animationpoint.X >= 3)
                    this.animationpoint.X = 0;
            }
            if (this.manymove >= 8)
            {
                bool reflected = false;
                Point poji = new Point(this.position.X, this.position.Y + (this.angleDOWN ? 1 : -1));
                if (!this.InAreaCheck(poji) || this.parent.panel[poji.X, poji.Y].color == this.union && this.enemyaArea)
                {
                    this.angleDOWN = !this.angleDOWN;
                    reflected = true;
                }
                poji = new Point(this.position.X + (this.angleLEFT ? -1 : 1), this.position.Y);
                if (!this.InAreaCheck(poji))
                {
                    this.angleLEFT = !this.angleLEFT;
                    reflected = true;
                }
                poji = new Point(this.position.X + (this.angleLEFT ? -1 : 1), this.position.Y + (this.angleDOWN ? 1 : -1));
                if (this.InAreaCheck(poji) && (this.parent.panel[poji.X, poji.Y].color == this.union && this.enemyaArea))
                {
                    this.angleLEFT = !this.angleLEFT;
                    reflected = true;
                }
                poji = new Point(this.position.X + (this.angleLEFT ? -1 : 1), this.position.Y + (this.angleDOWN ? 1 : -1));
                if (this.InAreaCheck(poji) && (this.parent.panel[poji.X, poji.Y].color == this.union && this.enemyaArea))
                {
                    this.angleDOWN = !this.angleDOWN;
                    reflected = true;
                }
                poji = new Point(this.position.X + (this.angleLEFT ? -1 : 1), this.position.Y + (this.angleDOWN ? 1 : -1));
                if (this.InAreaCheck(poji) && (this.parent.panel[poji.X, poji.Y].color == this.union && this.enemyaArea))
                    this.flag = false;
                this.manymove = 0;
                if (reflected)
                {
                    this.sound.PlaySE(SoundEffect.knock);
                    ++this.refrect;
                }
                if (this.refrect >= 8)
                    this.flag = false;
            }
            this.positionDirect.X += this.angleLEFT ? -this.movespeed.X : this.movespeed.X;
            this.positionDirect.Y += this.angleDOWN ? this.movespeed.Y : -this.movespeed.Y;
            ++this.manymove;
            if (this.manymove == 4)
            {
                this.position.X += this.angleLEFT ? -1 : 1;
                this.position.Y += this.angleDOWN ? 1 : -1;
                this.HitFlagReset();
                this.parent.effects.Add(new StepShadow(this.sound, this.parent, this._rect, this.positionDirect, this.picturename, this.rebirth, this.position));
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 64, 1336, 64, 48);
            dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            return base.HitCheck(charaposition, charaunion);
        }

        public override bool HitCheck(Point charaposition)
        {
            return base.HitCheck(charaposition);
        }

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p))
                return false;
            this.ShakeStart(5, 8);
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.ShakeStart(5, 8);
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.ShakeStart(5, 8);
            return true;
        }
    }
}
