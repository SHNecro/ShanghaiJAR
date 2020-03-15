using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using System.Drawing;
using SlimDX;
using System;

namespace NSEvent
{
    internal class Credit : EventBase
    {
		private string creditKey;
		private Vector2 position;
        private bool centered;
		private int fadeInTime;
		private int hangTime;
		private int fadeOutTime;

		private CreditState state;

		private int alpha;

		public Credit(
          MyAudio s,
          EventManager m,
		  string key,
          Point position,
          bool centered,
          int fadeInTime,
		  int hangTime,
          int fadeOutTime,
          SceneMap parent,
          SaveData save)
          : base(s, m, save)
        {
			this.NoTimeNext = false;

			this.creditKey = key;
			this.position = new Vector2(position.X, position.Y);
            this.centered = centered;
			this.fadeInTime = fadeInTime;
			this.hangTime = hangTime;
			this.fadeOutTime = fadeOutTime;

			this.state = CreditState.FadingIn;
			parent.fadingCredits.Add(this);

			this.alpha = 0;
        }

		public bool IsActive { get; set; }

		public override void Update()
		{
			this.IsActive = true;
			this.EndCommand();
		}

		public void MapUpdate()
		{
			this.FlameControl(1);
			switch (this.state)
			{
				case CreditState.FadingIn:
					if (this.frame >= this.fadeInTime)
					{
						this.frame = 0;
						this.alpha = 255;
						this.state = CreditState.Hanging;
					}
					else
					{
						this.alpha = (int)Math.Round(255.0 * (double)this.frame / this.fadeInTime);
					}
					break;
				case CreditState.Hanging:
					if (this.frame >= this.hangTime)
					{
						this.frame = 0;
						this.alpha = 255;
						this.state = CreditState.FadingOut;
					}
					break;
				case CreditState.FadingOut:
					if (this.frame >= this.fadeOutTime)
					{
						this.frame = 0;
						this.alpha = 0;
						this.state = CreditState.FadingIn;
						this.IsActive = false;
					}
					else
					{
						this.alpha = 255 - (int)Math.Round(255.0 * (double)this.frame / this.fadeOutTime);
					}
					break;
			}
		}

        public override void SkipUpdate()
        {
			this.IsActive = false;
        }

		public override void Render(IRenderer dg)
		{
		}

		public void MapRender(IRenderer dg)
		{
            var text = ShanghaiEXE.Translate(this.creditKey);
            var textSize = ShanghaiEXE.measurer.MeasureRegularText(text);
            var centerOffset = this.centered ? new Vector2(-textSize.Width / 2, -textSize.Height / 2) : new Vector2();
			var xOff = new[] { -1, 1, 0 };
			var yOff = new[] { -1, 1, 0 };
			foreach (var x in xOff)
			{
				foreach (var y in yOff)
				{
					dg.DrawText(
						text,
						this.position + new Vector2(x, y) + centerOffset,
						x == 0  && y == 0 ? Color.FromArgb(this.alpha, Color.White) : Color.FromArgb(this.alpha, 32, 32, 32));
				}
			}
		}

		private enum CreditState
		{
			FadingIn,
			Hanging,
			FadingOut
		}
    }
}
