using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap.Character;
using Common.Vectors;
using System.Drawing;

namespace NSEvent
{
    internal class mapWarp : EventBase
    {
        private readonly string mapName;
        private Point position;
        private readonly int floor;
        private Vector2 cameraPlus;
        private readonly MapCharacterBase.ANGLE angle;
        private const int plustime = 4;
        private const int warptime = 20;
        private const int warpend = 32;

        public mapWarp(
          IAudioEngine s,
          EventManager m,
          string name,
          Point po,
          int f,
          MapCharacterBase.ANGLE a,
          SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = false;
            this.mapName = name;
            this.position = po;
            this.floor = f;
            this.angle = a;
        }

        public override void Update()
        {
            switch (this.frame)
            {
                case 0:
                    this.manager.parent.outoCamera = false;
                    Vector2 camera = this.manager.parent.Field.camera;
                    this.manager.parent.Player.position = new Vector3(position.X, position.Y, this.floor * (this.manager.parent.Field.Height / 2));
                    this.manager.parent.Player.floor = this.floor;
                    float num1 = this.manager.parent.Player.Position.X - 4f;
                    float num2 = this.manager.parent.Player.Position.Y - 4f;
                    float z = this.manager.parent.Player.Position.Z;
                    this.cameraPlus.X = (int)(this.manager.parent.Field.MapsizeX / 2 + 2.0 * num1 - num2 * 2.0 + manager.parent.Field.CameraPlus.X);
                    this.cameraPlus.Y = (int)(num1 + (double)num2 + z - 4.0 + manager.parent.Field.CameraPlus.Y);
                    this.cameraPlus.X = (float)((cameraPlus.X - (double)camera.X) / 20.0);
                    this.cameraPlus.Y = (float)((cameraPlus.Y - (double)camera.Y) / 20.0);
                    break;
                case 32:
                    this.manager.parent.outoCamera = true;
                    this.EndCommand();
                    break;
            }
            if (this.frame < 32)
            {
                if (this.frame < 4 || this.frame >= 20)
                {
                    this.manager.parent.Field.camera.X += this.cameraPlus.X / 4f;
                    this.manager.parent.Field.camera.Y += this.cameraPlus.Y / 4f;
                }
                else
                {
                    this.manager.parent.Field.camera.X += this.cameraPlus.X;
                    this.manager.parent.Field.camera.Y += this.cameraPlus.Y;
                }
            }
            ++this.frame;
        }

        public override void SkipUpdate()
        {
            this.Update();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
