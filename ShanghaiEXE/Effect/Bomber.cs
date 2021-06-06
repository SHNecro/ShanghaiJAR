using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class Bomber : EffectBase
    {
        private readonly Bomber.BOMBERTYPE type;

        public Bomber(IAudioEngine s, SceneBattle p, Bomber.BOMBERTYPE t, Vector2 pd, int sp, Point posi)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = sp;
            this.positionDirect = pd;
            this.type = t;
            if (this.type == Bomber.BOMBERTYPE.poison)
                this.animationpoint.Y = 7;
            else
                this.animationpoint.Y = (int)this.type;
        }

        public Bomber(IAudioEngine s, SceneBattle p, int pX, int pY, Bomber.BOMBERTYPE t, int sp)
          : base(s, p, pX, pY)
        {
            this.speed = sp;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 70);
            this.type = t;
            if (this.type == Bomber.BOMBERTYPE.poison)
                this.animationpoint.Y = 7;
            else
                this.animationpoint.Y = (int)this.type;
        }

        public override void Updata()
        {
            this.animationpoint.X = this.frame;
            if (this.frame >= 13)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.type != Bomber.BOMBERTYPE.poison)
            {
                this._rect = new Rectangle(this.animationpoint.X * 48, this.animationpoint.Y * 48, 48, 48);
                this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
                dg.DrawImage(dg, "bomber", this._rect, false, this._position, this.rebirth, Color.White);
            }
            else
            {
                this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y - 8.0));
                this._rect = new Rectangle(this.animationpoint.X * 40, 32, 40, 56);
                dg.DrawImage(dg, "mimaAttack", this._rect, false, this._position, this.rebirth, Color.White);
            }
        }

        public enum BOMBERTYPE
        {
            bomber,
            flash,
            flashbomber,
            poison,
        }
    }
}
