using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSMap.Character
{
    internal class CompanionCharacter : MapEventBase
    {
        private readonly Vector3[] positionLog = new Vector3[16];
        private readonly MapCharacterBase.ANGLE[] angleLog = new MapCharacterBase.ANGLE[16];
        private readonly bool[] runLog = new bool[16];
        private readonly Player player;
        private const int manylog = 16;
        private const int newLogNum = 15;

        public CompanionCharacter(
          IAudioEngine s,
          SceneMap p,
          Point po,
          int floor,
          Player player,
          MapCharacterBase.ANGLE a,
          SaveData save)
          : base(s, p, po, floor, a, p.Field, "companion", save, "")
        {
            this.rendType = 1;
            this.player = player;
            this.position = new Vector3(player.position.X - 8f, player.position.Y - 8f, player.position.Z);
            this.angle = player.Angle;
            for (int index = 0; index < this.positionLog.Length; ++index)
            {
                this.positionLog[index] = this.position;
                this.angleLog[index] = this.angle;
                this.runLog[index] = player.run;
            }
            this.animespeed = 6;
        }

        public override void Update()
        {
            bool flag = false;
            if (player.position.X - 8.0 != positionLog[15].X || player.position.Y - 8.0 != positionLog[15].Y || player.position.Z != (double)this.positionLog[15].Z)
            {
                for (int index = 0; index < 15; ++index)
                {
                    this.positionLog[index] = this.positionLog[index + 1];
                    this.angleLog[index] = this.angleLog[index + 1];
                    this.runLog[index] = this.runLog[index + 1];
                }
                this.positionLog[15] = new Vector3(this.player.position.X - 8f, this.player.position.Y - 8f, this.player.position.Z);
                this.angleLog[15] = this.player.Angle;
                this.runLog[15] = this.player.run;
                this.position = this.positionLog[0];
                this.angle = this.angleLog[0];
                flag = true;
            }
            if (flag)
            {
                if (this.animeflame == 0)
                    this.animeflame = 1;
                else if (this.frame % this.animespeed == 0)
                {
                    ++this.animeflame;
                    if (this.runLog[0])
                    {
                        if (this.animeflame >= 9)
                            this.animeflame = 1;
                    }
                    else if (this.animeflame >= 7)
                        this.animeflame = 1;
                }
            }
            else
                this.animeflame = 0;
            this.FlameControl(0);
            this.ChangeQuarter();
        }

        public override void Render(IRenderer dg)
        {
            if (!this.player.savedata.FlagList[785])
                return;
            this._rect = new Rectangle(animeflame * 32 + (!this.runLog[0] || this.animeflame <= 0 ? 224 : 416), (int)this.angle * 48, 32, 48);
            double num1 = 120.0 - field.CameraPlus.X;
            Point shake = this.Shake;
            double x1 = shake.X;
            double num2 = num1 + x1;
            double num3 = 64.0 - field.CameraPlus.Y;
            shake = this.Shake;
            double y = shake.Y;
            double num4 = num3 + y;
            this._position = new Vector2((float)num2, (float)num4);
            this._position.Y += jumpY;
            int num5 = (int)this.positionQ.X - (int)this.field.camera.X;
            shake = this.Shake;
            int x2 = shake.X;
            this._position = new Vector2(num5 + x2, (int)this.positionQ.Y - (int)this.field.camera.Y + this.Shake.Y - 3);
            dg.DrawImage(dg, "charachip1", this._rect, false, this._position, Color.White);
        }
    }
}
