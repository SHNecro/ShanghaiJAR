using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSGame;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSCharge
{
    internal class ChargeBase : AllBase
    {
        public int chargetime;
        public int shorttime;
        public int power;
        public Player player;
        protected ChipBase.ELEMENT element;
        public bool shadow = true;

        protected int Power
        {
            get
            {
                return !this.player.badstatus[1] ? player.busterPower * this.power : player.busterPower * this.power / 2;
            }
        }

        public ChargeBase(IAudioEngine s, Player p)
          : base(s)
        {
            this.player = p;
            this.element = p.Element;
        }

        public virtual void Action()
        {
        }

        public int UnionRebirth(Panel.COLOR union)
        {
            return union == Panel.COLOR.red ? 1 : -1;
        }

        public void End()
        {
            if ((uint)this.player.step <= 0U)
                return;
            if (this.player.step == CharacterBase.STEP.shadow)
                this.player.nohit = false;
            this.player.step = CharacterBase.STEP.none;
            this.player.flying = this.player.flyflag;
            this.player.position = this.player.stepPosition;
            this.player.PositionDirectSet();
        }

        public virtual void Render(IRenderer dg, Vector2 position, string picturename)
        {
        }

        public AttackBase CounterNone(AttackBase a)
        {
            a.canCounter = false;
            return a;
        }

        protected Point RandomTarget(Panel.COLOR union)
        {
            List<Point> pointList = new List<Point>();
            if (union != Panel.COLOR.blue)
                pointList.Add(this.player.position);
            foreach (EnemyBase enemy in this.player.parent.enemys)
            {
                if (enemy.union == union)
                    pointList.Add(enemy.position);
            }
            int index = this.Random.Next(pointList.Count);
            return pointList[index];
        }

        public Panel.COLOR UnionEnemy
        {
            get
            {
                return this.player.union == Panel.COLOR.red ? Panel.COLOR.blue : Panel.COLOR.red;
            }
        }
    }
}
