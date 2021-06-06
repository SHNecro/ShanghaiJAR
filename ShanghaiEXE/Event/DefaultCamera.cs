using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using Common.Vectors;

namespace NSEvent
{
    internal class DefaultCamera : EventBase
    {
        private Vector2 target = new Vector2();
        private readonly int moveTime;
        private Vector2 flameplus;
        private bool pointset;
        private readonly SceneMap parent;

        public DefaultCamera(IAudioEngine s, EventManager m, int moveTime, SceneMap parent, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = false;
            this.parent = parent;
            this.moveTime = moveTime;
        }

        public override void Update()
        {
            if (!this.pointset)
            {
                this.flameplus = new Vector2(this.target.X / moveTime, this.target.Y / moveTime);
                this.pointset = true;
            }
            if (this.frame >= this.moveTime)
            {
                this.pointset = false;
                this.frame = 0;
                this.parent.cameraPlus = this.moveTime != 0 ? this.target : new Vector2();
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
            if (!this.pointset)
            {
                this.flameplus = new Vector2(this.target.X / moveTime, this.target.Y / moveTime);
                this.pointset = true;
            }
            this.pointset = false;
            this.frame = 0;
            this.parent.cameraPlus = this.target;
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
