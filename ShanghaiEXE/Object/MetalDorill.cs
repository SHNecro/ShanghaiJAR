using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSObject
{
    internal class MetalDorill : ObjectBase
    {
        private bool breaked;
        private readonly bool color;
        public int speednow;
        private new readonly int speed;

        public MetalDorill(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          int power,
          bool color,
          int speed,
          Panel.COLOR union)
          : base(s, p, pX, pY, union)
        {
            this.height = 80;
            this.wide = 72;
            this.hp = 10;
            this.hitPower = power;
            this.hpmax = this.hp;
            this.hitbreak = false;
            this.unionhit = false;
            this.overslip = false;
            this.Noslip = true;
            this.effecting = true;
            this.color = color;
            this.speed = speed;
            this.rebirth = this.union == Panel.COLOR.red;
            this.guard = CharacterBase.GUARD.guard;
            this.positionre = this.position;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 64);
        }

        public override void Updata()
        {
            this.positionDirect.X += this.speednow * this.UnionRebirth;
            if (positionDirect.X < -40.0 || positionDirect.X > 280.0)
                this.flag = false;
            this.position.X = this.Calcposition(this.positionDirect, 72, false).X;
            this.effecting = true;
            this.AttackMake(this.hitPower, 0, 0);
            this.FlameControl(3);
            if (this.moveflame)
            {
                if (this.speednow < this.speed)
                    ++this.speednow;
                ++this.animationpoint.X;
                if (this.animationpoint.X >= 3)
                    this.animationpoint.X = 0;
            }
            base.Updata();
        }

        public override void Break()
        {
            if (!this.breaked)
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
            {
                if (this.color)
                    this._rect = new Rectangle(1360 + this.animationpoint.X * 80, 0, 80, 72);
                else
                    this._rect = new Rectangle(1360 + this.animationpoint.X * 80, 144, 80, 72);
            }
            else
                this._rect = new Rectangle(1360 + this.animationpoint.X * 80, 72, 80, 72);
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            dg.DrawImage(dg, "iku", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
