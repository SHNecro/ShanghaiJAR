using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using System.Drawing;

namespace NSEffect
{
    public class EffectBase : CharacterBase
    {
        public EffectBase(IAudioEngine s, SceneBattle p, int pX, int pY)
          : base(s, p)
        {
            this.position = new Point(pX, pY);
            this.number = -100;
            this.blackOutObject = true;
        }

        public virtual void SkipUpdate()
        {
        }
    }
}
