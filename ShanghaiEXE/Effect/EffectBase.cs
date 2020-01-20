using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSEffect
{
    public class EffectBase : CharacterBase
    {
        public EffectBase(MyAudio s, SceneBattle p, int pX, int pY)
          : base(s, p)
        {
            this.position = new Point(pX, pY);
            this.number = -100;
        }
    }
}
