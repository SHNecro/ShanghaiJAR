using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSEvent;
using NSGame;

namespace NSNet
{
    internal class NetBattle : SceneBattle
    {
        public NetPlayer playerE;
        public bool gameEnd;
        public int result;

        public NetBattle(
          MyAudio s,
          ShanghaiEXE p,
          SceneMain main,
          EventManager e,
          bool res,
          int count,
          bool gameover,
          string bgm,
          SaveData save)
          : base(s, p, main, e, res, count, gameover, bgm, save)
        {
        }

        private void NetPanelSet()
        {
            for (int index = 0; index < 9; ++index)
            {
                Panel panel = this.panel[5 - index / 3, index % 3];
                panel.color = NetParam.ENpanelColor[index] == 0 ? Panel.COLOR.blue : Panel.COLOR.red;
                panel.state = (Panel.PANEL)NetParam.ENpanelElem[index];
            }
        }

        public override void Updata()
        {
            this.NetPanelSet();
            base.Updata();
        }
    }
}
