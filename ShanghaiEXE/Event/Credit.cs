using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using System.Drawing;
using Common.Vectors;
using System;
using System.Linq;

namespace NSEvent
{
    internal class Credit : EventBase, IPersistentEvent
    {
        private const int Timed = 0;
        private const int FadeIn = -1;
        private const int FadeOut = -2;

        private SceneMap parent;

		private string creditKey;
		private Vector2 position;
        private bool centered;
        private bool movesWithCamera;
		private int fadeInTime;
		private int hangTime;
		private int fadeOutTime;

		private CreditState state;

        private Vector2 initialCamera;

		private int alpha;

		public Credit(
          IAudioEngine s,
          EventManager m,
		  string key,
          Point position,
          bool centered,
          bool movesWithCamera,
          int fadeInTime,
		  int hangTime,
          int fadeOutTime,
          SceneMap parent,
          SaveData save)
          : base(s, m, save)
        {
			this.NoTimeNext = false;

            this.parent = parent;

			this.creditKey = key;
			this.position = new Vector2(position.X, position.Y);
            this.centered = centered;
            this.movesWithCamera = movesWithCamera;
			this.fadeInTime = fadeInTime;
			this.hangTime = hangTime;
			this.fadeOutTime = fadeOutTime;

            if (this.hangTime < 0)
            {
                this.PersistentId = this.hangTime == -1 ? this.fadeOutTime : this.fadeInTime;
            }

			this.state = CreditState.FadingIn;

			this.alpha = 0;
        }

		public bool IsActive { get; set; }

        public int? PersistentId { get; set; }

		public override void Update()
        {
            this.initialCamera = parent.Field.camera;

            if (this.hangTime != FadeOut)
            {
                parent.persistentEvents.Add(this);
            }
            else
            {
                var matchingCredits = parent.persistentEvents.OfType<Credit>().Where(c => c.PersistentId == this.PersistentId);
                foreach (var c in matchingCredits)
                {
                    c.ForceFadeOut(this.fadeOutTime);
                }
            }
            this.IsActive = true;
			this.EndCommand();
		}

		public void PersistentUpdate()
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
					if (this.hangTime != FadeIn && this.frame >= this.hangTime)
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

        public void PersistentRender(IRenderer dg)
        {
            var text = ShanghaiEXE.Translate(this.creditKey);
            var textSize = ShanghaiEXE.measurer.MeasureRegularText(text);
            var centerOffset = this.centered ? new Vector2(-textSize.Width / 2, -textSize.Height / 2) : new Vector2();
            var cameraOffset = this.movesWithCamera ? this.initialCamera - this.parent.Field.camera : Vector2.Zero;
            var xOff = new[] { -1, 1, 0 };
            var yOff = new[] { -1, 1, 0 };
            foreach (var x in xOff)
            {
                foreach (var y in yOff)
                {
                    dg.DrawText(
                        text,
                        this.position + new Vector2(x, y) + centerOffset + cameraOffset,
                        x == 0 && y == 0 ? Color.FromArgb(this.alpha, Color.White) : Color.FromArgb(this.alpha, 32, 32, 32));
                }
            }
        }

        public override void SkipUpdate()
        {
			this.IsActive = false;
        }

		public override void Render(IRenderer dg)
		{
		}

        private void ForceFadeOut(int fadeTime)
        {
            this.frame = 0;
            this.alpha = 255;
            this.fadeOutTime = fadeTime;
            this.state = CreditState.FadingOut;
        }

		private enum CreditState
		{
			FadingIn,
			Hanging,
			FadingOut
		}
    }
}
