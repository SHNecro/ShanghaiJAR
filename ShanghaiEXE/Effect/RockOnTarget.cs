using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class RockOnTarget : EffectBase
    {
        public bool on = false;
        private new readonly int speed = 4;
        private new readonly bool rebirth;
        private readonly CharacterBase player;

        public RockOnTarget(IAudioEngine s, SceneBattle p, int pX, int pY, CharacterBase player)
          : base(s, p, pX, pY)
        {
            this.upprint = true;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 78);
            this.animationpoint.X = 3;
            this.frame = 3;
        }

        public override void Updata()
        {
            this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 78);
            if (!this.on)
                return;
            this.FlameControl(this.speed);
            if (this.frame >= 4)
                this.frame = 0;
            this.animationpoint.X = this.frame;
        }

        public override void Render(IRenderer dg)
        {
            if (!this.on)
                return;
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            this._rect = new Rectangle(this.animationpoint.X * 40, 1016, 40, 40);
            dg.DrawImage(dg, "bomber", this._rect, false, this._position, !this.rebirth, Color.White);
        }
    }
}
