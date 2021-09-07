using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSObject
{
    internal class Jizou : ObjectBase
    {
        private bool breaked;
        private readonly int power;
        private bool attacked;
        private readonly int colorNo;
        private bool attack;
        private Panel.COLOR attakedunion;

        public Jizou(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR union,
          int colorNo,
          int power)
          : base(s, p, pX, pY, union)
        {
            this.power = power;
            this.colorNo = colorNo;
            this.height = 40;
            this.wide = 40;
            this.hp = 10;
            this.hitPower = 200;
            this.hpmax = this.hp;
            this.unionhit = true;
            this.overslip = true;
            this.rebirth = (uint)union > 0U;
            this.positionre = this.position;
            this.positionDirect = new Vector2(pX * 40 + 24, pY * 24 + 72);
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2(this.position.X * 40 + 24, this.position.Y * 24 + 72);
        }

        public override void Updata()
        {
            if (this.attack && !this.attacked)
            {
                foreach (CharacterBase characterBase in this.parent.AllChara())
                {
                    if (characterBase.union == this.attakedunion)
                    {
                        Point position = characterBase.position;
                        CrackThunder crackThunder = new CrackThunder(this.sound, this.parent, position.X, position.Y, this.union, this.power, false)
                        {
                            effectMode = true
                        };
                        this.parent.attacks.Add(crackThunder);
                        this.attacked = true;
                        this.nohit = true;
                        this.effecting = true;
                    }
                }
                BombAttack bombAttack = new BombAttack(this.sound, this.parent, this.attakedunion == Panel.COLOR.blue ? 0 : 5, 0, this.attakedunion == Panel.COLOR.blue ? Panel.COLOR.red : Panel.COLOR.blue, this.power, 1, ChipBase.ELEMENT.eleki)
                {
                    hitrange = new Point(6, 3),
                    breaking = true,
                    breakinvi = true,
                    throughObject = true
                };
                this.parent.attacks.Add(bombAttack);
                this.noslip = true;
            }
            if (this.attacked)
            {
                this.FlameControl(2);
                if (this.frame >= 15)
                    this.flag = false;
            }
            base.Updata();
        }

        public override void Break()
        {
            if (this.attacked || this.attack)
                return;
            if (!this.breaked || this.StandPanel.Hole)
            {
                this.breaked = true;
                this.sound.PlaySE(SoundEffect.breakObject);
                this.parent.effects.Add(new BreakCube(this.sound, this.parent, this.position, this.positionDirect.X, this.positionDirect.Y - 12f, 12, this.union, 20, true, 0));
                this.parent.effects.Add(new BreakCube(this.sound, this.parent, this.position, this.positionDirect.X, this.positionDirect.Y - 12f, 12, this.union, 20, false, 0));
            }
            this.flag = false;
        }

        public override void Dameged(AttackBase attack)
        {
            if (attack is Dummy)
            {
                return;
            }

            if (attack.breaking || this.attacked)
                return;
            this.attakedunion = attack.union;
            this.attack = true;
        }

        public override void Render(IRenderer dg)
        {
            if (!this.attacked)
                this._rect = new Rectangle(280, 40 * this.colorNo, 40, 40);
            else
                this._rect = new Rectangle(320 + 40 * (this.frame % 3), 40 * this.colorNo, 40, 40);
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            dg.DrawImage(dg, "ponpoko", this._rect, false, this._position, !this.rebirth, Color.White);
        }
    }
}
