using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Drawing;

namespace NSAttack
{
    internal class AnimaBall : AttackBase
    {
        private bool movestart = false;
        private const int plusy = 70;
        private readonly int movespeed;
        private bool stop;
        private int manymove;
        public bool hit;
        public bool black;
        private float upX;

        public AnimaBall(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          Vector2 v,
          ChipBase.ELEMENT ele,
          int movespeed = 8)
          : base(so, p, pX, pY, u, po, ele)
        {
            this.movespeed = movespeed;
            this.breakinvi = true;
            this.breaking = true;
            this.invincibility = false;
            this.speed = s;
            this.positionDirect = v;
            this.power /= 2;
            this.OldPD = this.positionDirect;
        }

        public override void Updata()
        {
            if (this.frame >= 3 && !this.movestart)
            {
                this.movestart = true;
                this.hitting = true;
                this.sound.PlaySE(SoundEffect.chain);
            }
            if (this.moveflame && !this.hit && this.movestart)
            {
                this.positionDirect.X += this.union == Panel.COLOR.red ? movespeed : -this.movespeed;
                this.manymove += this.movespeed;
                if (this.manymove / 40 >= 2)
                {
                    this.stop = true;
                    this.frame = 0;
                }
                this.position = this.Calcposition(this.positionDirect, 36, false);
                if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                    this.flag = false;
            }
            if (this.black)
                this.upX -= 0.5f;
            this.FlameControl();

            if (!this.parent.player.badstatus[3])
            {
                this.black = false;
                this.hit = false;
                this.parent.player.printplayer = true;
            }
        }

        public override void Render(IRenderer dg)
        {
            if (!this.hit)
            {
                double x1 = positionDirect.X;
                Point shake = this.Shake;
                double x2 = shake.X;
                double num1 = x1 + x2;
                double y1 = positionDirect.Y;
                shake = this.Shake;
                double y2 = shake.Y;
                double num2 = y1 + y2;
                this._position = new Vector2((float)num1, (float)num2);
                this._rect = new Rectangle(1024, 632, 32, 40);
                dg.DrawImage(dg, "ScissorMan", this._rect, false, this._position, this.rebirth, Color.White);
            }
            else if (!this.black)
            {
                int num1 = this.position.X * 40 + 20;
                Point shake = this.Shake;
                int x = shake.X;
                double num2 = num1 + x;
                int num3 = this.position.Y * 24 + 72;
                shake = this.Shake;
                int y = shake.Y;
                double num4 = num3 + y;
                this._position = new Vector2((float)num2, (float)num4);
                this._rect = new Rectangle(960, 432, 160, 144);
                dg.DrawImage(dg, "ScissorMan", this._rect, false, this._position, this.rebirth, Color.White);
            }
            else
            {
                int num1 = this.position.X * 40 + 20;
                Point shake = this.Shake;
                int x = shake.X;
                double num2 = num1 + x;
                int num3 = this.position.Y * 24 + 72;
                shake = this.Shake;
                int y = shake.Y;
                double num4 = num3 + y;
                this._position = new Vector2((float)num2, (float)num4);
                this._rect = new Rectangle(1120, 432, 160, 144);
                dg.DrawImage(dg, "ScissorMan", this._rect, false, this._position, this.rebirth, Color.White);
                this._position = new Vector2(this.position.X * 40 + 20 + this.upX + Shake.X, this.position.Y * 24 + 72 + this.Shake.Y);
                this._rect = new Rectangle(1280, 432, 160, 144);
                dg.DrawImage(dg, "ScissorMan", this._rect, false, this._position, this.rebirth, Color.White);
            }
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
            p.printplayer = false;
            p.badstatustime[3] = 270;
            p.badstatus[3] = true;
            this.hit = true;
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.flag = false;
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.flag = false;
            return true;
        }
    }
}
