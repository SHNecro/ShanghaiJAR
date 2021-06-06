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
    internal class Dummy : AttackBase
    {
        private readonly int time;
        private readonly bool pop;
        private new bool bright;

        public Dummy(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          Point hitrange,
          int time,
          bool pop)
          : base(so, p, pX, pY, u, 0, ChipBase.ELEMENT.normal)
        {
            this.upprint = true;
            this.animationpoint.X = 0;
            this.hitrange = hitrange;
            this.hitting = false;
            this.pop = pop;
            this.time = time;
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
            if (this.over)
                return;
            if (this.pop)
            {
                if (this.frame % 5 == 0)
                    this.bright = !this.bright;
                if (this.bright)
                    this.PanelBright();
            }
            else
                this.PanelBright();
            if (this.frame >= this.time)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
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
