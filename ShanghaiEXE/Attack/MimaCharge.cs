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
    internal class MimaCharge : AttackBase
    {
        private bool movestart = false;
        private const int plusy = 70;
        private readonly int movespeed;

        public MimaCharge(
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
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0), (float)(position.Y * 24.0 + 50.0));
            this.positionold = this.position;
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
                if (this.moveflame)
                {
                    ++this.animationpoint.X;
                    if (this.animationpoint.X > 5)
                        this.animationpoint.X = 2;
                }
                if (this.movestart)
                {
                    if (this.position.X != this.positionold.X)
                    {
                        this.positionold.X = this.position.X;
                        this.parent.attacks.Add(new MimaShock(this.sound, this.parent, this.positionold.X, this.positionold.Y - 1, this.union, this.power, MimaShock.MOTION.init));
                        this.parent.attacks.Add(new MimaShock(this.sound, this.parent, this.positionold.X, this.positionold.Y + 1, this.union, this.power, MimaShock.MOTION.init));
                    }
                    this.positionDirect.X += this.union == Panel.COLOR.red ? movespeed : -this.movespeed;
                    this.position.X = this.Calcposition(this.positionDirect, 36, false).X;
                    if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                        this.hitting = false;
                    if (positionDirect.X < -120.0 || positionDirect.X > 360.0)
                        this.flag = false;
                }
            }
            if (!this.movestart) { }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            for (int index = 0; index < 4; ++index)
            {
                double x1 = positionDirect.X;
                Point shake = this.Shake;
                double x2 = shake.X;
                double num1 = x1 + x2 + 16 * (4 - index);
                double y1 = positionDirect.Y;
                shake = this.Shake;
                double y2 = shake.Y;
                double num2 = y1 + y2;
                this._position = new Vector2((float)num1, (float)num2);
                this._rect = new Rectangle(112 * (4 - index), 344, 112, 72);
                dg.DrawImage(dg, "mimaAttack", this._rect, false, this._position, this.rebirth, Color.White);
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
    }
}
