using NSAttack;
using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSObject
{
    internal class FireNaparm : ObjectBase
    {
        private bool bomb;
        private bool breaked;

        public FireNaparm(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR union,
          int power,
          int hp)
          : base(s, p, pX, pY, union)
        {
            this.height = 16;
            this.wide = 24;
            this.hp = hp;
            this.hitPower = power;
            this.hpmax = hp;
            this.unionhit = true;
            this.overslip = true;
            this.positionre = this.position;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 80);
        }

        public override void Updata()
        {
            if (this.bomb)
            {
                int x = Eriabash.SteelX(this, this.parent);
                this.sound.PlaySE(SoundEffect.bombbig);
                this.ShakeStart(4, 90);
                this.parent.effects.Add(new RandomBomber(this.sound, this.parent, Bomber.BOMBERTYPE.flashbomber, 2, new Point(x, 0), new Point(6, 2), this.union, 36));
                for (int pX = 0; pX < this.parent.panel.GetLength(0); ++pX)
                {
                    for (int pY = 0; pY < this.parent.panel.GetLength(1); ++pY)
                    {
                        if (this.parent.panel[pX, pY].color == this.UnionEnemy)
                            this.parent.attacks.Add(new BombAttack(this.sound, this.parent, pX, pY, this.union, this.hitPower, 1, ChipBase.ELEMENT.normal));
                    }
                }
                this.flag = false;
            }
            base.Updata();
        }

        public override void Dameged(AttackBase attack)
        {
            if (attack is Dummy)
            {
                return;
            }

            if (attack.Element != ChipBase.ELEMENT.heat)
                return;
            this.bomb = true;
        }

        public override void Break()
        {
            if (this.bomb)
                return;
            if (!this.breaked || this.StandPanel.Hole)
            {
                this.breaked = true;
                this.sound.PlaySE(SoundEffect.clincher);
                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y, Bomber.BOMBERTYPE.bomber, 2));
            }
            this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            if (this.whitetime <= 0)
                this._rect = new Rectangle(0, 304, 16, 24);
            else
                this._rect = new Rectangle(0, 328, 16, 24);
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            dg.DrawImage(dg, "objects1", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
