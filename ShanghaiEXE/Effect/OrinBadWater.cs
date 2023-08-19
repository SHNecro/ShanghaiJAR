using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;
using System;
using System.Collections.Generic;

namespace NSEffect
{
    internal class OrinBadWater : EffectBase
    {
        private static readonly IDictionary<TYPE, Tuple<string, Rectangle>> Types = new Dictionary<TYPE, Tuple<string, Rectangle>>
        {
            { TYPE.Poison, Tuple.Create("OrinAttack1", new Rectangle(50*2, 50*2, 50, 50)) },
            { TYPE.Blue, Tuple.Create("heavenbarrier", new Rectangle(0, 192, 40, 32)) },
            { TYPE.Pink, Tuple.Create("heavenbarrier", new Rectangle(0, 160, 40, 32)) },
        };

        private readonly TYPE type;
        private int palette;

        public OrinBadWater(IAudioEngine s, SceneBattle p, int pX, int pY, int sp, TYPE type = TYPE.Poison, int pal = 1)
          : base(s, p, pX, pY)
        {
            this.speed = sp * 2;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 74);
            this.type = type;
            this.palette = pal;
        }

        public override void Updata()
        {
            this.animationpoint.X = this.frame + 1;
            if (this.frame >= 5)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            var spritesheet = Types[this.type].Item1;
            var textureRect = Types[this.type].Item2;

            int adj = 0;
            if (this.palette == 2) { adj = 200; }
            if (this.palette == 3) { adj = 400; }
            if (this.palette == 4) { adj = 600; }

            this._rect = new Rectangle(textureRect.X + this.animationpoint.X * textureRect.Width, textureRect.Y + adj, textureRect.Width, textureRect.Height);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this.color = Color.White;
            dg.DrawImage(dg, spritesheet, this._rect, false, this._position, this.rebirth, this.color);
        }

        public enum TYPE
        {
            Poison,
            Blue,
            Pink
        }
    }
}
