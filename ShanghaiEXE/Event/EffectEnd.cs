using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using NSMap;

namespace NSEvent
{
    internal class EffectEnd : EventBase
    {
        private MapField field;

        public EffectEnd(MyAudio s, EventManager m, MapField ma, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.field = ma;
        }

        public override void Update()
        {
            if (this.manager.parent != null)
                this.field = this.manager.parent.Field;
            for (int index = 0; index < this.field.effect.Count; ++index)
            {
                if (this.field.effect[index].effect is AliceJump || this.field.effect[index].effect is ItemGet || (this.field.effect[index].effect is ShanghaiIN || this.field.effect[index].effect is AlicePhone) || (this.field.effect[index].effect is AlicePowder || this.field.effect[index].effect is AlicePowderLeft || (this.field.effect[index].effect is FightShanhaiBack || this.field.effect[index].effect is FightMarisa)) || (this.field.effect[index].effect is SlashYorihime || this.field.effect[index].effect is AliceBed || this.field.effect[index].effect is ROMHead) || this.field.effect[index].effect is DruidAttack)
                    this.field.effect[index].Flag = false;
            }
            this.EndCommand();
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
