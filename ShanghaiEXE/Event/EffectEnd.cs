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

        public EffectEnd(IAudioEngine s, EventManager m, MapField ma, SaveData save)
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
                var effect = this.field.effect[index].effect;
                switch (effect)
                {
                    case AliceJump _:
                    case ItemGet _:
                    case ShanghaiIN _:
                    case AlicePhone _:
                    case AlicePowder _:
                    case AlicePowderLeft _:
                    case FightShanhaiBack _:
                    case FightMarisa _:
                    case SlashYorihime _:
                    case AliceBed _:
                    case ROMHead _:
                    case DruidAttack _:
                    case HeavenTreeEvent _:
                        this.field.effect[index].Flag = false;
                        break;
                }
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
