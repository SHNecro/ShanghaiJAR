using NSAttack;
using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class PushWind : EffectBase
    {
        private const byte _speed = 2;
        private int count;

        public PushWind(IAudioEngine s, SceneBattle p, Vector2 pd, Point posi, Panel.COLOR union)
          : base(s, p, posi.X, posi.Y)
        {
            this.union = union;
            this.speed = 2;
            this.positionDirect = pd;
        }

        public override void Updata()
        {
            if (this.parent.blackOut)
                return;
            if (this.frame >= 1200)
                this.flag = false;
            if (this.frame % 15 == 0)
            {
                int pX = this.union == Panel.COLOR.red ? 0 : 5;
                int pY = 2 - this.count;
                Wind wind = new Wind(this.sound, this.parent, pX, pY, this.union, true);
                if (pX == 0)
                    wind.positionDirect.X = 0.0f;
                if (pX == 5)
                    wind.positionDirect.X = 240f;
                this.parent.attacks.Add(wind);
                ++this.count;
                if (this.count >= 3)
                    this.count = 0;
            }
            ++this.frame;
        }

        public override void Render(IRenderer dg)
        {
        }
    }
}
