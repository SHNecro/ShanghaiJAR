using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using NSMap.Character;
using System.Drawing;

namespace NSEvent
{
    internal class PlugOut : EventBase
    {
        private readonly SceneMap map;

        public PlugOut(IAudioEngine s, EventManager m, SceneMap map, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.map = map;
        }

        public override void Update()
        {
            this.map.FieldSet(this.savedata.pluginMap, new Point((int)this.savedata.pluginX, (int)this.savedata.pluginY), this.savedata.pluginFroor, MapCharacterBase.ANGLE.DOWN);
            this.savedata.isJackedIn = false;
            this.savedata.FlagList[2] = false;
            if (!this.savedata.FlagList[13])
            {
                this.savedata.GetRandomMystery = new bool[600];
                this.savedata.runSubChips[0] = false;
                this.savedata.runSubChips[1] = false;
                this.savedata.runSubChips[2] = false;
                this.savedata.runSubChips[3] = false;
                this.savedata.ValList[19] = 0;
                this.savedata.HPNow = this.savedata.HPMax;
                this.map.step = SceneMap.STEPS.normal;
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
