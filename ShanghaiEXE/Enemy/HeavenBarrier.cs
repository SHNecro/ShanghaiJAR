using NSAttack;
using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSEnemy
{
    internal class HeavenBarrier : EnemyBase
    {
        private bool isBroken;

        public HeavenBarrier(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.name = ShanghaiEXE.Translate("Enemy.HeavenBarrierName1");
            this.picturename = "heavenbarrier";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.normal;
            this.Flying = false;
            this.power = 0;
            this.wide = 32;
            this.height = 80;
            this.version = v;
            this.frame = 0;
            this.speed = 7 - version;
            this.printhp = true;
            this.printNumber = true;
            this.effecting = false;
            if (this.parent != null)
                this.roop = (byte)(parent.manyenemys - (uint)this.number);
            this.PositionDirectSet();

            if (this.version <= 0 || this.version > 3)
            {
                this.version = (byte)((this.version % 3) + 1);
            }

            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.HeavenBarrierName1");
                    this.hp = 600;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.HeavenBarrierName2");
                    this.hp = 800;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.HeavenBarrierName3");
                    this.hp = 1000;
                    break;
            }
            this.animationpoint = new Point(this.version, 0);
            this.printNumber = false;

            // No chip or zenny reward
            this.dropchips[0].chip = new Reygun(this.sound);
            this.dropchips[0].codeNo = 0;
            this.dropchips[1].chip = new Reygun(this.sound);
            this.dropchips[1].codeNo = 1;
            this.dropchips[2].chip = new Reygun(this.sound);
            this.dropchips[2].codeNo = 2;
            this.dropchips[3].chip = new Reygun(this.sound);
            this.dropchips[3].codeNo = 2;
            this.dropchips[4].chip = new Reygun(this.sound);
            this.dropchips[4].codeNo = 3;

            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.neutlal = true;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0) + 4 * this.UnionRebirth + 2, (float)(position.Y * 24.0 + 54.0) - 4);
        }

        protected override void Moving()
        {
            this.neutlal = true;
            this.animationpoint = new Point(this.version, 0);
            this.MoveAftar();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.isBroken ? 1 : 0) * this.height, this.wide, this.height);
            if (this.Hp <= 0)
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, 0, this.wide, this.height), this._position, this.picturename);
            if (this.alfha < byte.MaxValue)
                this.color = Color.FromArgb(alfha, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            else
                this.color = this.mastorcolor;
            if (this.whitetime == 0)
            {
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            else
            {
                this._rect.X = 0;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2(this.positionDirect.X + 4f, this.positionDirect.Y - 32f);
            this.Nameprint(dg, this.printNumber);
        }

        private enum MOTION
        {
            neutral,
            boost,
            attack,
        }
    }
}

