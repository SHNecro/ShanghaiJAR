using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using System.Drawing;

namespace NSObject
{
    public class ObjectBase : CharacterBase
    {
        public int whitetime = 0;
        public bool hitbreak = true;
        public bool hit;
        public int hitPower;
        public bool overslip;
        public bool unionhit;

        public ObjectBase(IAudioEngine s, SceneBattle p, int pX, int pY, Panel.COLOR union)
          : base(s, p)
        {
            this.position = new Point(pX, pY);
            this.positionre = this.position;
            this.union = union;
            this.number = 100;
        }

        public override void Updata()
        {
            if (this.whitetime > 0)
                --this.whitetime;
            if (this.hp <= 0)
                this.Break();
            if (!this.Noslip)
            {
                if (!this.effecting && this.nohit)
                {
                    this.nohit = false;
                    this.invincibilitytime = 2;
                }
                if (this.slipping && (this.neutlal || this.knockslip))
                {
                    if (this.hitPower > 0)
                    {
                        this.effecting = true;
                        this.nohit = true;
                    }
                    this.Slip(this.height);
                }
                else if (!this.Noslip)
                    this.effecting = false;
            }
            if (this.effecting && this.nohit && this.slipping)
                this.AttackMake(this.hitPower, 0, 0, true);
            this.hit = false;
            base.Updata();
        }

        public virtual void Break()
        {
        }
    }
}
