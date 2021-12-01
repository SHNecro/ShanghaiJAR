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
    internal class SuzuranBlue : ObjectBase
    {
        private bool breaked;
        private int time;

        public SuzuranBlue(
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
            this.rebirth = union == Panel.COLOR.blue;
            this.positionDirect = new Vector2(pX * 40 + 16, pY * 24 + 64);
        }

        public override void Updata()
        {
            base.Updata();
            if (this.time > 0)
            {
                --this.time;
            }
            else
            {
                if (this.moveflame)
                {
                    foreach (CharacterBase characterBase in this.parent.AllChara())
                    {
                        if (characterBase.union == this.union)
                            ++characterBase.Hp;
                    }
                    if (this.frame % 3 == 0)
                        this.parent.effects.Add(new Smoke(this.sound, this.parent, this.Random.Next(6), this.Random.Next(3), ChipBase.ELEMENT.normal));
                    if (this.frame >= 2000)
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
                this._rect = new Rectangle(96, 0, 48, 48);
            else
                this._rect = new Rectangle(96, 96, 48, 48);
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
