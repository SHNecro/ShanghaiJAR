using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;

namespace NSEnemy
{
    internal class NaviBase : EnemyBase
    {
        public NaviBase.MOTION motion = NaviBase.MOTION.neutral;
        public bool superArmor;
        protected int powerPlus;

        public virtual NaviBase.MOTION Motion
        {
            get
            {
                return this.motion;
            }
            set
            {
                this.motion = value;
                this.waittime = 0;
            }
        }

        public NaviBase(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
        }

        public virtual void NockMotion()
        {
        }

        public int UnionRebirth(Panel.COLOR union)
        {
            return union == Panel.COLOR.red ? 1 : -1;
        }

        public override int Power
        {
            get
            {
                return !this.badstatus[1] ? this.power + this.powerPlus : (this.power + this.powerPlus) / 2;
            }
        }

        public enum MOTION
        {
            neutral,
            attack,
            move,
            knockback,
        }
    }
}

