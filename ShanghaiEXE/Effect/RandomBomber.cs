using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSEffect
{
    internal class RandomBomber : EffectBase
    {
        private readonly List<Point> positions = new List<Point>();
        private readonly Bomber.BOMBERTYPE type;
        private readonly int interval;
        private Point posi;
        private Point range;
        private readonly int many;
        private int manymake;

        public RandomBomber(
          IAudioEngine s,
          SceneBattle p,
          Bomber.BOMBERTYPE t,
          int interval,
          Point posi,
          Point range,
          Panel.COLOR union,
          int many)
          : base(s, p, posi.X, posi.Y)
        {
            this.union = union;
            this.interval = interval;
            this.posi = posi;
            this.range = range;
            this.many = many;
            while (this.positions.Count < many)
            {
                for (int index1 = 0; index1 <= range.X; ++index1)
                {
                    for (int index2 = 0; index2 <= range.Y; ++index2)
                        this.positions.Add(new Point(posi.X + index1 * this.UnionRebirth, posi.Y + index2));
                }
            }
            this.positions = this.positions.OrderBy<Point, Guid>(i => Guid.NewGuid()).ToList<Point>();
            this.type = t;
            this.animationpoint.Y = (int)this.type;
        }

        public RandomBomber(
          IAudioEngine s,
          SceneBattle p,
          Bomber.BOMBERTYPE t,
          int interval,
          Panel.COLOR targetunion,
          Panel.COLOR union,
          int many)
          : base(s, p, 0, 0)
        {
            this.union = union;
            this.interval = interval;
            this.many = many;
            List<Point> pointList = new List<Point>();
            for (int x = 0; x < this.parent.panel.GetLength(0); ++x)
            {
                for (int y = 0; y < this.parent.panel.GetLength(1); ++y)
                {
                    if (this.parent.panel[x, y].color == targetunion && this.InAreaCheck(new Point(x, y)))
                        pointList.Add(new Point(x, y));
                }
            }
            while (this.positions.Count < many)
            {
                for (int index = 0; index < pointList.Count; ++index)
                    this.positions.Add(pointList[index]);
            }
            this.positions = this.positions.OrderBy<Point, Guid>(i => Guid.NewGuid()).ToList<Point>();
            this.type = t;
            this.animationpoint.Y = (int)this.type;
        }

        public override void Updata()
        {
            this.FlameControl(this.interval);
            if (!this.moveflame)
                return;
            List<EffectBase> effects = this.parent.effects;

			if (this.manymake >= this.positions.Count)
			{
				return;
			}

            IAudioEngine sound = this.sound;
            SceneBattle parent = this.parent;
            Point position = this.positions[this.manymake];
            int x = position.X;
            position = this.positions[this.manymake];
            int y = position.Y;
            int type = (int)this.type;
            Bomber bomber = new Bomber(sound, parent, x, y, (Bomber.BOMBERTYPE)type, 2);
            effects.Add(bomber);
            ++this.manymake;
            if (this.manymake >= this.many)
                this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
        }
    }
}
