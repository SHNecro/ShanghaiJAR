using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;
using System.Threading;

namespace NSTitle
{
    internal class GameOver : SceneBase
    {
        private readonly string titlemusic = nameof(GameOver);
        private int fadealpha = byte.MaxValue;
        private byte backpx = 240;
        private Vector2 fontposition = new Vector2(120f, 128f);
        private const byte fadespeed = 8;
        private const byte backspeed = 8;
        private GameOver.TITLESCENE nowscene;

        public GameOver(IAudioEngine s, ShanghaiEXE p, SaveData save)
          : base(s, p, save)
        {
            this.parent = p;
            this.nowscene = GameOver.TITLESCENE.init;
            this.savedata.Init();
            var loadThread = new Thread(new ThreadStart(() => this.savedata.Load(this.parent)));
            loadThread.Start();
            this.sound.StartBGM(this.titlemusic);
        }

        public override void Updata()
        {
            switch (this.nowscene)
            {
                case GameOver.TITLESCENE.init:
                    if (this.fadealpha <= 0)
                    {
                        this.nowscene = GameOver.TITLESCENE.pushbutton;
                        break;
                    }
                    this.fadealpha -= 8;
                    if (this.fadealpha <= 0)
                        this.fadealpha = 0;
                    break;
                case GameOver.TITLESCENE.pushbutton:
                    ++this.frame;
                    if (this.frame >= 240 && this.savedata.loadEnd)
                    {
                        this.nowscene = GameOver.TITLESCENE.fade;
                        break;
                    }
                    break;
                case GameOver.TITLESCENE.fade:
                    this.fadealpha += 8;
                    if (this.fadealpha >= byte.MaxValue)
                    {
                        this.fadealpha = byte.MaxValue;
                        this.parent.battlenum = 0;
                        this.parent.ChangeOfSecne(Scene.Title);
                        break;
                    }
                    break;
            }
            this.backpx -= 8;
            if (this.backpx <= 0)
                this.backpx = 240;
            if (!Input.IsPush(Button._L) || !Input.IsPush(Button._R) || !Input.IsPush(Button._Select) || !Input.IsPush(Button._Start))
                return;
            this.sound.StopBGM();
            this.parent.ChangeOfSecne(Scene.Title);
            this.savedata = new SaveData();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(240, 320, 240, 160);
            this._position = new Vector2(backpx, 0.0f);
            dg.DrawImage(dg, "title", this._rect, true, this._position, false, Color.White);
            this._position = new Vector2(backpx - 240, 0.0f);
            dg.DrawImage(dg, "title", this._rect, true, this._position, false, Color.White);
            this._rect = new Rectangle(0, 320, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "title", this._rect, true, this._position, false, Color.White);
            Color color = Color.FromArgb(this.fadealpha, 0, 0, 0);
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color);
        }

        private void FadeRender(IRenderer dg)
        {
            Color color = Color.FromArgb(150, 0, 0, 0);
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, false, color);
        }

        private enum TITLESCENE
        {
            init,
            pushbutton,
            fade,
        }
    }
}
