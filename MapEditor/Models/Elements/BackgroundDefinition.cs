using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using NSBackground;

namespace MapEditor.Models.Elements
{
	public class BackgroundDefinition
    {
        public BackgroundDefinition(BackgroundBase bgBase)
        {
            var bindFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            this.BackColor = (Color)typeof(BackgroundBase).GetField("backcolor", bindFlags).GetValue(bgBase);
            this.PictureName = (string)typeof(BackgroundBase).GetField("picturename", bindFlags).GetValue(bgBase);
            this.Size = new Size((Point)typeof(BackgroundBase).GetField("size", bindFlags).GetValue(bgBase));
            this.Speed = (int)typeof(BackgroundBase).GetField("speed", bindFlags).GetValue(bgBase);
			this.AnimationSpeed = (int)typeof(BackgroundBase).GetField("animespeed", bindFlags).GetValue(bgBase);
			this.Frames = (int)typeof(BackgroundBase).GetField("flames", bindFlags).GetValue(bgBase);
            this.Scroll = (Point)typeof(BackgroundBase).GetField("scroll", bindFlags).GetValue(bgBase);
            this.ScrollSpeed = (Point)typeof(BackgroundBase).GetField("scrollspeed", bindFlags).GetValue(bgBase);
			if (this.Frames == 0)
			{
				this.GetBGFrame = (checkedFrame) => 0;
			}
			else
			{
				var animation = (int[,])typeof(BackgroundBase).GetField("animasion", bindFlags).GetValue(bgBase);
				var frameList = new List<int>();
				var frame = 0;
				for (int i = 0; i < this.Frames; i++)
				{
					var nextFrame = i;
					var framesBeforeI = animation[1, nextFrame] == 1 ? this.Speed : animation[1, nextFrame];
					frameList.AddRange(Enumerable.Repeat(animation[0, frame], framesBeforeI));

					frame = (frame + 1) % this.Frames;
				}
				this.GetBGFrame = (checkedFrame) => frameList[checkedFrame % frameList.Count];
			}
		}

        public Color BackColor { get; }
        public string PictureName { get; }
        public Size Size { get; }
        public int Speed { get; }
		public int AnimationSpeed { get; }

		public Func<int, int> GetBGFrame { get; }
		public int Frames { get; }
        public Point Scroll { get; }
        public Point ScrollSpeed { get; }
    }
}
