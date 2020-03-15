using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSObject;
using System.Drawing;

namespace NSAttack
{
    internal class EndlessSahara : AttackBase
    {
        private readonly SandHoleAttack[] sands = new SandHoleAttack[4];
        private int many;
        private new readonly bool bright;

        public EndlessSahara(IAudioEngine so, SceneBattle p, int power, Panel.COLOR u)
          : base(so, p, 0, 0, u, 0, ChipBase.ELEMENT.normal)
        {
            this.power = power;
            if (this.flag)
                this.hitting = false;
            for (int index = 0; index < this.sands.Length; ++index)
            {
                Point point = this.RandomPanel(this.UnionEnemy);
                this.sands[index] = new SandHoleAttack(this.sound, this.parent, point.X, point.Y, this.union, power, 400, this.Random.Next(5), SandHoleAttack.MOTION.init, ChipBase.ELEMENT.earth);
                this.parent.attacks.Add(this.sands[index]);
                ++this.many;
            }
        }

        public override void Updata()
        {
            for (int index = 0; index < this.sands.Length; ++index)
            {
                if (!this.sands[index].flag)
                {
                    Point point = this.RandomPanel(this.UnionEnemy);
                    this.sands[index] = new SandHoleAttack(this.sound, this.parent, point.X, point.Y, this.union, this.power, 400, this.Random.Next(5), SandHoleAttack.MOTION.init, ChipBase.ELEMENT.earth);
                    this.parent.attacks.Add(this.sands[index]);
                    ++this.many;
                }
                if (this.many >= 10)
                    this.flag = false;
            }
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
