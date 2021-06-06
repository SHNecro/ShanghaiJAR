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
    internal class DrillAttack : AttackBase
    {
        public DrillAttack(
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
            if (!this.flag)
                return;
            this.breaking = true;
            this.invincibility = false;
            this.upprint = true;
            this.speed = s;
            this.animationpoint.X = 0;
            this.hitrange = new Point(1, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 - 5, this.position.Y * 24 + 48);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 5, this.position.Y * 24 + 48);
            this.frame = 0;
        }

        public override void Updata()
        {
            this.PanelBright();
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame;
                switch (this.frame)
                {
                    case 2:
                        this.hitting = false;
                        break;
                    case 5:
                        this.flag = false;
                        break;
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
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
            p.Knockbuck(true, false, this.union);
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            e.Knockbuck(true, false, this.union);
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            return base.HitEvent(o);
        }
    }
}
