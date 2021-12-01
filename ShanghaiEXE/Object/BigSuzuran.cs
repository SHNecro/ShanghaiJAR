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
    internal class BigSuzuran : ObjectBase
    {
        private bool breaked;
        private int time;

        public BigSuzuran(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR union,
          int HP,
          int time)
          : base(s, p, pX, pY, union)
        {
            this.height = 48;
            this.wide = 32;
            this.hp = HP;
            this.hitPower = 0;
            this.hpmax = this.hp;
            this.unionhit = true;
            this.noslip = true;
            this.overslip = true;
            this.time = time;
            this.rebirth = union == Panel.COLOR.red;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 60);
        }

        public override void Updata()
        {
            base.Updata();
            if (this.time > 0)
                --this.time;
            else if (!this.parent.blackOut)
            {
                if (this.moveflame)
                {
                    foreach (CharacterBase characterBase in this.parent.AllChara())
                    {
                        if (characterBase.union == this.UnionEnemy && characterBase.Element != ChipBase.ELEMENT.poison)
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

                            characterBase.Hp -= 1 * multiplier;
                        }
                    }
                    if (this.frame % 3 == 0)
                        this.parent.effects.Add(new Smoke(this.sound, this.parent, this.Random.Next(6), this.Random.Next(3), ChipBase.ELEMENT.poison));
                    if (this.frame >= 1000)
                        this.Break();
                }
                this.FlameControl(1);
            }
            if (this.StandPanel.state == Panel.PANEL._grass)
                return;
            this.StandPanel.state = Panel.PANEL._grass;
        }

        public override void Break()
        {
            if (!this.breaked)
            {
                this.breaked = true;
                this.sound.PlaySE(SoundEffect.clincher);
                this.parent.effects.Add(new Elementhit(this.sound, this.parent, this.position.X, this.position.Y, 2, ChipBase.ELEMENT.leaf));
            }
            this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            if (this.whitetime <= 0)
                this._rect = new Rectangle(192, 0, 48, 64);
            else
                this._rect = new Rectangle(192, 64, 48, 64);
            if (this.time > 0)
                this._rect.X += 48;
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
