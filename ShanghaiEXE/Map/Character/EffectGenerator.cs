using NSShanghaiEXE.InputOutput.Audio;
using System.Drawing;

namespace NSMap.Character
{
    public class EffectGenerator : MapCharacterBase
    {
        public string ID;
        private readonly int effectNo;
        private readonly int rundom;
        private readonly int interval;
        private readonly SoundEffect SE;

        public EffectGenerator(
          IAudioEngine s,
          SceneMap p,
          Point po,
          int floor,
          MapField fi,
          string id,
          int effectNo,
          int interval,
          int rundom,
          int rendType,
          SoundEffect SE)
          : base(s, p, po, floor, MapCharacterBase.ANGLE.none, fi)
        {
            this.ID = id;
            this.effectNo = effectNo;
            this.rundom = rundom;
            this.rendType = rendType;
            this.interval = interval;
            this.SE = SE;
        }

        public override void Update()
        {
            if (this.interval < 0)
            {
                if (this.frame != 0)
                    return;
                this.MakeEffect();
                ++this.frame;
            }
            else
            {
                this.FlameControl(this.interval);
                if (this.moveflame)
                    this.MakeEffect();
            }
        }

        private void MakeEffect()
        {
            if (this.SE != SoundEffect.none)
                this.sound.PlaySE(this.SE);
            this.field.effect.Add(new MapEffect(this.sound, this.parent, new Point((int)this.position.X + this.Random.Next(-this.rundom, this.rundom), (int)this.position.Y + this.Random.Next(-this.rundom, this.rundom)), this.floor, this.field, this.effectNo, this.rendType));
        }

        public override float RendSetter()
        {
            return this.Position.X + this.Position.Y;
        }
    }
}
