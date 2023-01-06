using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSTitle
{
    internal class FirstTitle : SceneBase
    {
        private int fadealpha = byte.MaxValue;
        private byte backpx = 0;
        private int rogopx = 240;
        private float rogorota = 210f;
        private float scall = 8f;
        private const byte fadespeed = 8;
        private const byte backspeed = 6;
        private FirstTitle.TITLESCENE nowscene;

        private float Rogorota
        {
            get
            {
                return this.rogorota;
            }
            set
            {
                this.rogorota = value;
                if (rogorota < 0.0)
                    this.rogorota += 360f;
                if (rogorota <= 360.0)
                    return;
                this.rogorota -= 360f;
            }
        }

        public FirstTitle(IAudioEngine s, ShanghaiEXE p, SaveData save)
          : base(s, p, save)
        {
            this.parent = p;
            this.nowscene = FirstTitle.TITLESCENE.rogoinit;
        }

        public override void Updata()
        {
            switch (this.nowscene)
            {
                case FirstTitle.TITLESCENE.rogoinit:
                    if (this.fadealpha <= 0)
                    {
                        this.nowscene = FirstTitle.TITLESCENE.rogopushbutton;
                        break;
                    }
                    ++this.frame;
                    if (this.frame > 30)
                        this.fadealpha -= 8;
                    if (this.fadealpha <= 0)
                        this.fadealpha = 0;
                    break;
                case FirstTitle.TITLESCENE.rogopushbutton:
                    ++this.frame;
                    if (this.frame >= 120 && this.savedata.loadEnd || this.Push())
                    {
                        this.nowscene = FirstTitle.TITLESCENE.rogofade;
                        this.frame = 0;
                        break;
                    }
                    break;
                case FirstTitle.TITLESCENE.rogofade:
                    this.fadealpha += 8;
                    if (this.fadealpha >= byte.MaxValue)
                    {
                        ++this.frame;
                        this.fadealpha = byte.MaxValue;
                        if (this.frame > 30)
                            this.nowscene = FirstTitle.TITLESCENE.titleinit;
                        break;
                    }
                    break;
                case FirstTitle.TITLESCENE.titleinit:
                    if (this.fadealpha <= 0)
                    {
                        this.nowscene = FirstTitle.TITLESCENE.titlepush;
                        break;
                    }
                    this.fadealpha -= 8;
                    if (this.fadealpha <= 0)
                        this.fadealpha = 0;
                    break;
                case FirstTitle.TITLESCENE.titlepush:
                    this.rogopx -= 6;
                    if (this.Push())
                    {
                        this.fadealpha = byte.MaxValue;
                        this.parent.battlenum = 0;
                        this.parent.ChangeOfSecne(Scene.Title);
                    }
                    if (this.rogopx <= -512)
                    {
                        this.nowscene = FirstTitle.TITLESCENE.titlespin;
                        this.frame = 0;
                        break;
                    }
                    break;
                case FirstTitle.TITLESCENE.titlespin:
                    if (scall > 1.0)
                        this.scall -= 0.2f;
                    this.Rogorota -= 30f;
                    if (this.frame >= 40 && this.savedata.loadEnd)
                    {
                        this.scall = 1f;
                        this.rogorota = 0.0f;
                        this.parent.ChangeOfSecne(Scene.Title);
                        this.nowscene = FirstTitle.TITLESCENE.titlefade;
                        break;
                    }
                    ++this.frame;
                    break;
                case FirstTitle.TITLESCENE.titlefade:
                    this.fadealpha += 50;
                    if (this.fadealpha >= byte.MaxValue)
                    {
                        this.fadealpha = byte.MaxValue;
                        this.parent.battlenum = 0;
                        this.parent.ChangeOfSecne(Scene.Title);
                        break;
                    }
                    break;
            }
            if ((uint)(this.nowscene - 3) > 3U)
                return;
            this.backpx += 6;
            if (this.backpx < 240)
                return;
            this.backpx = 0;
        }

        private bool Push()
        {
            return Input.IsPress(Button._A) || Input.IsPress(Button._Start);
        }

        public override void Render(IRenderer dg)
        {
            switch (this.nowscene)
            {
                case FirstTitle.TITLESCENE.rogoinit:
                case FirstTitle.TITLESCENE.rogopushbutton:
                case FirstTitle.TITLESCENE.rogofade:
                    this._rect = new Rectangle(0, 0, 240, 160);
                    this._position = new Vector2(0.0f, 0.0f);
                    dg.DrawImage(dg, "title2", this._rect, true, this._position, false, Color.White);
                    break;
                case FirstTitle.TITLESCENE.titleinit:
                case FirstTitle.TITLESCENE.titlepush:
                case FirstTitle.TITLESCENE.titlespin:
                case FirstTitle.TITLESCENE.titlefade:
                    this._rect = new Rectangle(0, 320, 240, 160);
                    this._position = new Vector2(backpx, 0.0f);
                    dg.DrawImage(dg, "title2", this._rect, true, this._position, false, Color.White);
                    this._position = new Vector2(backpx - 240, 0.0f);
                    dg.DrawImage(dg, "title2", this._rect, true, this._position, false, Color.White);
                    switch (this.nowscene)
                    {
                        case FirstTitle.TITLESCENE.titlepush:
                            var scrollingTitleSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("FirstTitle.ScrollingTitle");
                            this._rect = scrollingTitleSprite.Item2;
                            this._position = new Vector2(rogopx, 0.0f);
                            dg.DrawImage(dg, scrollingTitleSprite.Item1, this._rect, true, this._position, false, Color.White);
                            break;
                        case FirstTitle.TITLESCENE.titlespin:
                        case FirstTitle.TITLESCENE.titlefade:
                            var spinningTitleSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("FirstTitle.SpinningTitle");
                            this._rect = spinningTitleSprite.Item2;
                            this._position = new Vector2(120f, 80f);
                            dg.DrawImage(dg, spinningTitleSprite.Item1, this._rect, false, this._position, this.scall, this.rogorota, Color.White);
                            break;
                    }
                    break;
            }
            if (this.nowscene == FirstTitle.TITLESCENE.titlefade)
            {
                Color white = Color.White;
            }
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
            rogoinit,
            rogopushbutton,
            rogofade,
            titleinit,
            titlepush,
            titlespin,
            titlefade,
        }
    }
}
