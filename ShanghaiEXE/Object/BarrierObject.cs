using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSObject
{
    internal class BarrierObject : ObjectBase
    {
        private bool breaked;
        private readonly int color;
        private bool open;

        public BarrierObject(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR union,
          int power,
          int color)
          : base(s, p, pX, pY, union)
        {
            this.color = color;
            this.height = 48;
            this.wide = 32;
            this.hp = 1;
            this.hitPower = 0;
            this.hpmax = this.hp;
            this.unionhit = false;
            this.overslip = false;
            this.shield = CharacterBase.SHIELD.ReflectP;
            this.effecting = true;
            this.nohit = true;
            this.Noslip = true;
            this.guard = CharacterBase.GUARD.guard;
            this.ReflectP = power;
            this.shieldtime = 60;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 64);
        }

        public override void Updata()
        {
            base.Updata();
            if (this.breaked)
                this.Break();
            else if (this.moveflame)
            {
                if (!this.open)
                {
                    ++this.animationpoint.X;
                    if (this.animationpoint.X == 2)
                        this.nohit = false;
                    if (this.animationpoint.X >= 9)
                    {
                        this.animationpoint.X = 3;
                        this.open = true;
                    }
                }
                if (this.shieldtime <= 0)
                    this.breaked = true;
            }
            this.FlameControl(4);
        }

        public override void Break()
        {
            if (this.animationpoint.X < 9)
            {
                this.animationpoint.X = 9;
                this.shield = CharacterBase.SHIELD.none;
                this.nohit = true;
            }
            else
            {
                if (this.moveflame)
                {
                    ++this.animationpoint.X;
                    if (this.animationpoint.X >= 13)
                        this.flag = false;
                }
                this.FlameControl(4);
            }
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(32 * this.animationpoint.X, 72 * this.color, 32, 72);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, "shield", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
