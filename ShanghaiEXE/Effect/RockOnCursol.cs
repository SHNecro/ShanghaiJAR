using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class RockOnCursol : EffectBase
    {
        public bool on = false;
        private new readonly int speed = 4;
        private new readonly bool rebirth;
        private readonly CharacterBase player;

        public RockOnCursol(IAudioEngine s, SceneBattle p, int pX, int pY, CharacterBase player)
          : base(s, p, pX, pY)
        {
            this.player = player;
            this.positionDirect = player.positionDirect;
        }

        public override void Updata()
        {
            if (!this.on)
                return;
            this.positionDirect = this.player.positionDirect;
            if (this.moveflame)
            {
                if (this.frame >= 4)
                {
                    this.frame = 0;
                    this.sound.PlaySE(SoundEffect.search);
                }
                this.animationpoint.X = this.frame;
            }
            this.FlameControl(this.speed);
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
            this._rect = new Rectangle(this.animationpoint.X * 120, 840, 120, 120);
            dg.DrawImage(dg, "weapons", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
