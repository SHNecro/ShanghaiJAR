using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using Common.Vectors;
using System;

namespace NSGame
{
    [Serializable]
    public class Virus
    {
        public int type;
        public int eatBug;
        public int eatFreeze;
        public int eatError;
        public int code;
        private EnemyBase enemy;

        public byte rank
        {
            get
            {
                return (byte)(this.eatBug / 20 + 1);
            }
        }

        public int eatSum
        {
            get
            {
                return this.eatBug + this.eatFreeze + this.eatError;
            }
        }

        public ChipFolder.CODE Code
        {
            get
            {
                return (ChipFolder.CODE)this.code;
            }
            set
            {
                this.code = (int)value;
            }
        }

        public int HP
        {
            get
            {
                this.NullCheck();
                return this.enemy.Hp + this.eatFreeze * 20;
            }
        }

        public int Power
        {
            get
            {
                this.NullCheck();
                return this.enemy.power + this.eatError * 5;
            }
        }

        public string Name
        {
            get
            {
                this.NullCheck();
                return this.enemy.Name + (!this.enemy.printNumber || this.rank <= 1 ? null : this.rank.ToString());
            }
        }

        public EnemyBase ReturnVirus(int key)
        {
            this.enemy = new EnemyBase(null, null, 0, 0, (byte)key, Panel.COLOR.red, this.rank);
            this.enemy = EnemyBase.EnemyMake(key, this.enemy, false);
            return this.enemy;
        }

        public void Render(IRenderer dg, Vector2 position, bool slide)
        {
            this.NullCheck();
            if (slide)
            {
                position.X += this.enemy.helpPosition.X * this.enemy.UnionRebirth;
                position.Y += enemy.helpPosition.Y;
            }
            this.enemy.printhp = false;
            this.enemy.alfha = byte.MaxValue;
            this.enemy.positionDirect = position;
            this.enemy.Render(dg);
        }

        public void NullCheck()
        {
            if (this.enemy != null)
                return;
            this.enemy = this.ReturnVirus(this.type);
        }
    }
}
