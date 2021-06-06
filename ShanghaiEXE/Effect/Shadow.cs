using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class Shadow : EffectBase
    {
        private const byte _speed = 2;
        private readonly CharacterBase master;
        public Point slide;

        public Shadow(IAudioEngine s, SceneBattle p, Vector2 pd, Point posi, CharacterBase master)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = 2;
            this.downprint = true;
            this.positionDirect = pd;
            this.master = master;
        }

        public Shadow(IAudioEngine s, SceneBattle p, int pX, int pY, CharacterBase master)
          : base(s, p, pX, pY)
        {
            this.speed = 2;
            this.downprint = true;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 84);
            this.master = master;
        }

        public override void Updata()
        {
            if (!this.master.flag)
                this.flag = false;
            this.FlameControl();
        }

        public void PositionDirectSet(Point position)
        {
            this.position = position;
            this.positionDirect = new Vector2(position.X * 40 + 20, position.Y * 24 + 84);
        }

        public override void Render(IRenderer dg)
        {
            if (this.StandPanel.Hole)
                return;
            this._rect = new Rectangle(0, 440, 32, 8);
            this._position = new Vector2(this.positionDirect.X + Shake.X + slide.X, this.positionDirect.Y + Shake.Y + slide.Y);
            dg.DrawImage(dg, "towers", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
