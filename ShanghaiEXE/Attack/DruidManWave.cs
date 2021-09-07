using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;
using NSShanghaiEXE.ExtensionMethods;

namespace NSAttack
{
    internal class DruidManWave : AttackBase
    {
		private static Rectangle SpriteRect = new Rectangle(0, 792, 36, 34);

        public DruidManWave(
          IAudioEngine sound,
          SceneBattle parent,
          int pX,
          int pY,
          Panel.COLOR color,
          int power,
          int speed,
          ChipBase.ELEMENT element)
          : base(sound, parent, pX, pY, color, power, element)
        {
            if (!this.flag
				|| !this.InArea
				|| this.parent.panel[this.position.X, this.position.Y].Hole)
			{
				this.flag = false;
			}
            this.speed = speed;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.red;

            this.frame = 0;
		}

		public override void Updata()
        {
            if (this.hitting)
                this.PanelBright();
            if (this.moveflame && this.flag)
			{
				if (!this.over)
				{
					this.over = true;
					this.hitting = false;
					var xOffset = this.rebirth ? 1 : -1;
					var nextPosition = this.position.WithOffset(xOffset, 0);

					if (this.InAreaCheck(nextPosition)
						&& !this.parent.panel[nextPosition.X, nextPosition.Y].Hole)
					{
						this.parent.attacks.Add(this.StateCopy(new DruidManWave(this.sound, this.parent, nextPosition.X, nextPosition.Y, this.union, this.power, this.speed, this.element)));
					}
				}

				this.animationpoint.X++;
				if (this.animationpoint.X > 4)
				{
					this.flag = false;
				}

			}
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
			if (this.flag)
			{
				this._position = this.position.ToBattleScreenPosition(0, 56);
                if (this.rebirth)
                {
                    this._position.X += SpriteRect.Width;
                }
				this._rect = SpriteRect.WithOffset(SpriteRect.Width * this.animationpoint.X, SpriteRect.Height * ElementToYOffset(this.element));
				dg.DrawImage(dg, "druidmanSP", this._rect, true, this._position, this.rebirth, Color.White);
			}
		}

        private static int ElementToYOffset(ChipBase.ELEMENT elem)
        {
            switch (elem)
            {
                case ChipBase.ELEMENT.heat: return 0;
                case ChipBase.ELEMENT.aqua: return 1;
                case ChipBase.ELEMENT.eleki: return 2;
                case ChipBase.ELEMENT.leaf: return 3;
                case ChipBase.ELEMENT.poison: return 4;
                case ChipBase.ELEMENT.earth: return 5;
                default: return 6;
            }
        }

		private class Aftershock
		{
			public Point Position { get; set; }
			public int Frame { get; set; }
		}
	}
}
