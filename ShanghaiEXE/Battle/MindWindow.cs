using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSBattle
{
    public class MindWindow : AllBase
    {
        private const int time = 20;
        public bool perfect;
        public MindWindow.MIND mindNow;
        private MindWindow.MIND mindOld;
        private int turnTime;
        private readonly SceneBattle parent;
        private bool nowOrOld;
        private readonly Player player;
        public bool anger;
        private readonly SaveData savedata;

        public MindWindow.MIND MindNow
        {
            get
            {
                return this.mindNow;
            }
            set
            {
                if (this.mindNow == MindWindow.MIND.dark)
                    return;
                this.mindNow = value;
            }
        }

        public MindWindow(IAudioEngine s, SceneBattle p, SaveData save)
          : base(s)
        {
            this.savedata = save;
            this.MindNow = (MindWindow.MIND)save.mind;
            this.mindOld = this.MindNow;
            this.parent = p;
            this.player = this.parent.player;
        }

        public void Update()
        {
            if (this.perfect)
            {
                this.MindNow = MindWindow.MIND.fullsync;
            }
            else
            {
                if (this.savedata.FlagList[0] && this.mindNow == MindWindow.MIND.fullsync)
                    this.mindNow = MindWindow.MIND.smile;
                if (this.parent.player.blackMind)
                {
                    this.mindNow = MindWindow.MIND.dark;
                    this.mindOld = MindWindow.MIND.dark;
                }
                if (this.turnTime > 0)
                {
                    --this.turnTime;
                    if (this.turnTime % 2 == 0)
                        this.nowOrOld = !this.nowOrOld;
                    if (this.turnTime == 0)
                    {
                        this.mindOld = this.MindNow;
                        this.nowOrOld = false;
                    }
                }
                else if (this.mindOld != this.MindNow)
                    this.turnTime = 20;
            }
        }

        public void Render(IRenderer dg, float x)
        {
            if (this.parent.nowscene == SceneBattle.BATTLESCENE.dead)
                return;
            this._position = new Vector2(x, 24f);
            if (!this.perfect)
                this._rect = new Rectangle(544 + 48 * ((this.nowOrOld ? (int)this.MindNow : (int)this.mindOld) / 4), 120 + 16 * ((this.nowOrOld ? (int)this.MindNow : (int)this.mindOld) % 4), 48, 16);
            else
                this._rect = new Rectangle(592, 152, 48, 16);
            dg.DrawImage(dg, "battleobjects", this._rect, false, this._position, Color.White);
        }

        public enum MIND
        {
            normal,
            fullsync,
            pinch,
            smile,
            dark,
            angry,
        }
    }
}
