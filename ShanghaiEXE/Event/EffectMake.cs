using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using NSMap.Character;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSEvent
{
    internal class EffectMake : EventBase
    {
        public int effectNumber;
        private readonly int X;
        private readonly int Y;
        private readonly int Z;
        private readonly int nORv;
        private readonly string name;
        private MapField field;
        private readonly SceneMap parent;
        private readonly MapEventBase event_;
        private readonly string ID;
        private readonly int interval;
        private readonly int randomXY;
        private readonly int rendType;
        private readonly SoundEffect SE;

        public EffectMake(
          IAudioEngine s,
          EventManager m,
          int effectNumber,
          string ID,
          int X,
          int Y,
          int Z,
          int nORv,
          int interval,
          int randomXY,
          int rendType,
          string SE,
          SceneMap parent,
          MapField field,
          MapEventBase event_,
          SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.effectNumber = effectNumber;
            this.Z = Z;
            this.X = X;
            this.Y = Y;
            if (event_ != null)
                this.event_ = event_;
            this.nORv = nORv;
            this.field = field;
            this.parent = parent;
            this.ID = ID;
            this.interval = interval;
            this.randomXY = randomXY;
            this.rendType = rendType;
            try
            {
                this.SE = (SoundEffect)Enum.Parse(typeof(SoundEffect), SE);
            }
            catch
            {
                this.SE = SoundEffect.none;
            }
        }

        public EffectMake(
          IAudioEngine s,
          EventManager m,
          int effectNumber,
          string ID,
          string name,
          int nORv,
          int interval,
          int randomXY,
          int rendType,
          string SE,
          SceneMap parent,
          MapField field,
          MapEventBase event_,
          SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.effectNumber = effectNumber;
            this.name = name;
            if (event_ != null)
                this.event_ = event_;
            this.nORv = nORv;
            this.field = field;
            this.parent = parent;
            this.ID = ID;
            this.interval = interval;
            this.randomXY = randomXY;
            this.rendType = rendType;
            try
            {
                this.SE = (SoundEffect)Enum.Parse(typeof(SoundEffect), SE);
            }
            catch
            {
                this.SE = SoundEffect.none;
            }
        }

        public override void Update()
        {
            this.field = this.parent.Field;
            Point po = new Point(0, 0);
            int floor;
            if (this.nORv == 1)
            {
                po = new Point(this.savedata.ValList[this.X], this.savedata.ValList[this.Y]);
                floor = this.savedata.ValList[this.Z];
            }
            else if (this.nORv == 0)
            {
                po = new Point(this.X, this.Y);
                floor = this.Z;
            }
            else if (this.nORv == 2)
            {
                po = new Point((int)this.event_.position.X, (int)this.event_.position.Y);
                floor = (int)this.event_.position.Z;
            }
            else
            {
                Vector3 position;
                if (this.name == "プレイヤー")
                {
                    position = this.parent.Player.position;
                    floor = this.parent.Player.floor;
                }
                else
                {
                    MapEventBase mapEventBase = this.parent.Field.Events.Find(e => e.ID == this.name);
                    position = mapEventBase.position;
                    floor = mapEventBase.floor;
                }
                po = new Point((int)position.X, (int)position.Y);
            }
            this.field.effectgenerator.Add(new EffectGenerator(this.sound, this.parent, po, floor, this.field, this.ID, this.effectNumber, this.interval, this.randomXY, this.rendType, this.SE));
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
