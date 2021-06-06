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
    internal class EnemyHit : AttackBase
    {
        private readonly CharacterBase chara;

        public EnemyHit(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          ChipBase.ELEMENT ele,
          CharacterBase chara)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.breaking = true;
            this.upprint = true;
            this.speed = 1;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 - 5, this.position.Y * 24 + 48);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 5, this.position.Y * 24 + 48);
            this.frame = 0;
            this.chara = chara;
            this.hitflag = chara.hitflag;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            this.PanelBright();
            if (this.moveflame)
            {
                switch (this.frame)
                {
                    case 1:
                        this.flag = false;
                        break;
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this.ObjectBreak();
        }

        private void ObjectBreak()
        {
            if (!(this.chara is ObjectBase))
                return;
            ObjectBase chara = (ObjectBase)this.chara;
            if (chara.hitbreak)
            {
                bool[,] hitflag = this.hitflag;
                int upperBound1 = hitflag.GetUpperBound(0);
                int upperBound2 = hitflag.GetUpperBound(1);
                for (int lowerBound1 = hitflag.GetLowerBound(0); lowerBound1 <= upperBound1; ++lowerBound1)
                {
                    for (int lowerBound2 = hitflag.GetLowerBound(1); lowerBound2 <= upperBound2; ++lowerBound2)
                    {
                        if (hitflag[lowerBound1, lowerBound2])
                        {
                            chara.Break();
                            goto label_10;
                        }
                    }
                }
            label_10:;
            }
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            this.chara.hitflag = this.hitflag;
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;
            this.chara.hitflag = this.hitflag;
            return true;
        }

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p))
                return false;
            this.chara.hitflag = this.hitflag;
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.chara.hitflag = this.hitflag;
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.chara.hitflag = this.hitflag;
            return true;
        }
    }
}
