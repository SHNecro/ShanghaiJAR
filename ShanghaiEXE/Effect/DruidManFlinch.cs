using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class DruidManFlinch : EffectBase
	{
		private static readonly Rectangle FullFrameRect = new Rectangle(0, 0, 104, 99);

		private Vector2 posi;

		private FlinchState flinchState;

        public DruidManFlinch(
          IAudioEngine audio,
          SceneBattle parent,
          Vector2 positionDirect,
          bool reverse,
          Point battlePosition)
          : base(audio, parent, battlePosition.X, battlePosition.Y)
        {
            this.picturename = "druidman";
            this.posi = positionDirect;
            this.animationpoint.Y = 2;
            this.rebirth = reverse;

			this.flinchState = FlinchState.PurpleScale;
        }

        public override void Updata()
        {
            if (this.parent.blackOut && this.frame < 2)
			{
				return;
			}
            this.animationpoint.X = this.frame;
            if (this.frame % 4 == 0)
			{
				switch (this.flinchState)
				{
					case FlinchState.PurpleScale:
						this.flinchState = FlinchState.Outline;
						break;
					case FlinchState.Outline:
						this.flinchState = FlinchState.DoubleImage;
						break;
					case FlinchState.DoubleImage:
						this.flinchState = FlinchState.PurpleScale;
						break;
				}
			}
            if (this.frame >= 40)
            {
                this.flag = false;
            }
            this.FlameControl();
            ++this.frame;
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.posi.X + this.Shake.X, this.posi.Y + this.Shake.Y);
			switch (this.flinchState)
			{
				case FlinchState.PurpleScale:
					this.animationpoint = new Point(3, 4);
					this._rect = new Rectangle(FrameCoordX(this.animationpoint.X), FrameCoordY(this.animationpoint.Y), FullFrameRect.Width, FullFrameRect.Height);
					dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, Color.White);
					break;
				case FlinchState.Outline:
					this.animationpoint = new Point(6, 4);
					this._rect = new Rectangle(FrameCoordX(this.animationpoint.X), FrameCoordY(this.animationpoint.Y), FullFrameRect.Width, FullFrameRect.Height);
					dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, Color.White);
					break;
				case FlinchState.DoubleImage:
					var leftFrame = 4;
					var rightFrame = 5;

					if (this.frame % 2 == 0)
					{
						leftFrame = 5;
						rightFrame = 4;
					}

					this.animationpoint = new Point(leftFrame, 4);
					this._rect = new Rectangle(FrameCoordX(this.animationpoint.X), FrameCoordY(this.animationpoint.Y), FullFrameRect.Width, FullFrameRect.Height);
					dg.DrawImage(dg, this.picturename, this._rect, false, this._position + new Vector2(this.frame / 3, 0), this.rebirth, Color.White);
					
					this.animationpoint = new Point(rightFrame, 4);
					this._rect = new Rectangle(FrameCoordX(this.animationpoint.X), FrameCoordY(this.animationpoint.Y), FullFrameRect.Width, FullFrameRect.Height);
					dg.DrawImage(dg, this.picturename, this._rect, false, this._position + new Vector2(-this.frame / 3, 0), this.rebirth, Color.White);
					break;
			}
		}

		private static int FrameCoordX(int frameNumber)
		{
			return FullFrameRect.Width * frameNumber;
		}
		private static int FrameCoordY(int frameNumber)
		{
			return FullFrameRect.Height * frameNumber;
		}

		private enum FlinchState
		{
			PurpleScale,
			Outline,
			DoubleImage
		}
    }
}
