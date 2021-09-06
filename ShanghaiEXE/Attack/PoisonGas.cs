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
    internal class PoisonGas : AttackBase
    {
        private readonly bool gas;

        public PoisonGas(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          bool gas,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.gas = gas;
            this.upprint = true;
            this.speed = 0;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = false;
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
            foreach (CharacterBase characterBase in this.parent.AllHitter())
            {
                if (characterBase.position == this.position && characterBase.union == this.UnionEnemy && !characterBase.nohit)
                {
                    var multiplier = 1;
                    switch (characterBase.Element)
                    {
                        case ChipBase.ELEMENT.aqua:
                        case ChipBase.ELEMENT.leaf:
                            multiplier *= 2;
                            break;
                    }
                    if (characterBase.badstatus[(int)ChipBase.ELEMENT.aqua])
                    {
                        multiplier *= 2;
                    }
                    if (characterBase.badstatus[(int)ChipBase.ELEMENT.leaf])
                    {
                        multiplier *= 2;
                    }

                    characterBase.Hp -= this.power * multiplier;
                    characterBase.Dameged(this);
                }
            }
            if (this.gas)
            {
                EffectBase effectBase = new Smoke(this.sound, this.parent, this.position.X, this.position.Y, ChipBase.ELEMENT.poison);
                effectBase.positionDirect.X += this.Random.Next(16);
                effectBase.positionDirect.Y += this.Random.Next(16);
                effectBase.positionDirect.Y -= 8f;
                this.parent.effects.Add(effectBase);
            }
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
