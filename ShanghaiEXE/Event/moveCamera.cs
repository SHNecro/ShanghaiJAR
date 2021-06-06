using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using Common.Vectors;

namespace NSEvent
{
    internal class moveCamera : EventBase
    {
        private Vector2 target;
        private readonly int moveTime;
        private Vector2 flameplus;
        private readonly SceneMap parent;
        private readonly bool relative;
        private bool pointset;
        private Vector2 goal;
        private Vector2 start;

        public moveCamera(
          IAudioEngine s,
          EventManager m,
          Vector2 target,
          int moveTime,
          bool relative,
          SceneMap parent,
          SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = false;
            this.parent = parent;
            this.relative = relative;
            this.moveTime = moveTime;
            this.target = target;
        }

        public override void Update()
        {
            if (!this.pointset)
            {
                this.start = this.parent.cameraPlus;
                if (this.relative)
                    this.target = new Vector2((float)(target.X - (double)this.parent.Field.camera.X + 16.0), (float)(target.Y - (double)this.parent.Field.camera.Y + 24.0));
                this.flameplus = new Vector2(this.target.X / moveTime, this.target.Y / moveTime);
                this.pointset = true;
                this.goal = new Vector2(this.target.X, this.target.Y);
            }
            if (this.frame >= this.moveTime)
            {
                this.pointset = false;
                this.frame = 0;
                this.parent.cameraPlus = this.relative ? this.goal : new Vector2(this.start.X + this.goal.X, this.start.Y + this.goal.Y);
                this.EndCommand();
            }
            else
            {
                this.parent.cameraPlus.X += this.flameplus.X;
                this.parent.cameraPlus.Y += this.flameplus.Y;
                ++this.frame;
            }
        }

        public override void SkipUpdate()
        {
            if (goal.X == 0.0 && goal.Y == 0.0)
            {
                this.start = this.parent.cameraPlus;
                if (this.relative)
                    this.target = new Vector2(this.start.X + this.target.X, this.start.Y + this.target.Y);
                this.flameplus = new Vector2(this.target.X / moveTime, this.target.Y / moveTime);
                this.pointset = true;
                this.goal = this.relative ? new Vector2(this.parent.cameraPlus.X + this.target.X, this.parent.cameraPlus.Y + this.target.Y) : new Vector2(this.target.X, this.target.Y);
            }
            this.pointset = false;
            this.frame = 0;
            this.parent.cameraPlus = new Vector2(this.start.X + this.goal.X, this.start.Y + this.goal.Y);
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
