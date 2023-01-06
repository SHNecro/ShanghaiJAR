using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;
using NSMap.Character.Menu;
using System.Linq;

namespace NSTitle
{
    internal class SceneTitle : SceneBase
    {
        private readonly string titlemusic = "title";
        private int keywait = 0;
        private int fadealpha = byte.MaxValue;
        private byte backpx = 0;
        private Vector2 fontposition = new Vector2(120f, 128f);
        private SceneTitle.TITLEMENU menu = SceneTitle.TITLEMENU.Start;
        private bool printpush = true;
        private bool[] star = new bool[10];
        private int plus;
        private readonly bool test;
        private const byte fadespeed = 8;
        private const byte backspeed = 3;
        private SceneTitle.TITLESCENE nowscene;
        private bool printLoad;
        private int stars;
        private int starAnime;

        private SceneTitle.TITLEMENU Menu
        {
            get
            {
                return this.menu;
            }
            set
            {
                this.menu = value;
                if (this.menu < SceneTitle.TITLEMENU.Start)
                    this.menu = SceneTitle.TITLEMENU.Load;
                if (this.menu <= SceneTitle.TITLEMENU.Load)
                    return;
                this.menu = SceneTitle.TITLEMENU.Start;
            }
        }

        public SceneTitle(IAudioEngine s, ShanghaiEXE p, SaveData save)
          : base(s, p, save)
        {
            this.parent = p;
            this.nowscene = SceneTitle.TITLESCENE.init;
            this.test = false;
            this.printLoad = p.loadSUCCESS;
        }

        private void StarCheck()
        {
            this.star = new bool[10];
            this.stars = 0;
            // Game end flag
            if (this.savedata.FlagList[788])
            {
                this.star[0] = true;
                this.stars++;
            }
            if (this.savedata.FlagList[1701])
            {
                this.star[1] = true;
                this.stars++;
            }

            var completionLibrary = new Library(this.sound, null, null, this.savedata);
            if (completionLibrary.LibraryPages[Library.LibraryPageType.Normal].Chips.All(c => c.IsSeen))
            {
                this.star[2] = true;
                this.stars++;
            }
            if (completionLibrary.LibraryPages[Library.LibraryPageType.Navi].Chips.All(c => c.IsSeen))
            {
                this.star[3] = true;
                this.stars++;
            }
            if (completionLibrary.LibraryPages[Library.LibraryPageType.Dark].Chips.All(c => c.IsSeen))
            {
                this.star[4] = true;
                this.stars++;
            }
            if (completionLibrary.LibraryPages[Library.LibraryPageType.PA].Chips.All(c => c.IsSeen))
            {
                this.star[5] = true;
                this.stars++;
            }
            if (this.savedata.FlagList[1702])
            {
                this.star[6] = true;
                this.stars++;
            }

            int[] sflSP = { 620,621,622,623,625,626,
                            627,628,629,630,631,632,
                            633,634,635,636,640 };
            bool star7 = true;
            for (int flSP = 0; flSP < sflSP.Length; flSP++)
            {
                if (!this.savedata.FlagList[sflSP[flSP]])
                {
                    star7 = false;
                }
                //int a = 0;
            }

            if (star7)
            {
                this.star[7] = true;
                this.stars++;
            }

            /*if (this.savedata.FlagList[1703])
            {
                this.star[7] = true;
                this.stars++;
            }*/

            if (this.savedata.FlagList[1704])
            {
                this.star[8] = true;
                this.stars++;
            }
            if (this.savedata.FlagList[1705])
            {
                this.star[9] = true;
                this.stars++;
            }
        }

        public override void Updata()
        {
            switch (this.nowscene)
            {
                case SceneTitle.TITLESCENE.init:
                    if (this.fadealpha <= 0)
                    {
                        if (this.savedata.loadEnd)
                        {
                            this.printLoad = this.savedata.loadSucces;
                            if (this.printLoad)
                                this.menu = SceneTitle.TITLEMENU.Load;
                            this.nowscene = SceneTitle.TITLESCENE.pushbutton;
                            this.sound.StartBGM(this.titlemusic);
                            this.printpush = true;
                            break;
                        }
                        break;
                    }
                    this.fadealpha -= 8;
                    if (this.fadealpha <= 0)
                        this.fadealpha = 0;
                    break;
                case SceneTitle.TITLESCENE.pushbutton:
                    if (Input.IsPush(Button.Esc))
                        this.parent.Close();
                    if (backpx % 64 == 0)
                        this.printpush = this.savedata.loadEnd && !this.printpush;
                    if (Input.IsPress(Button._Start) && this.savedata.loadEnd)
                    {
                        this.StarCheck();
                        this.sound.PlaySE(SoundEffect.decide);
                        this.nowscene = SceneTitle.TITLESCENE.select;
                        this.frame = 0;
                        this.ShakeEnd();
                        break;
                    }
                    break;
                case SceneTitle.TITLESCENE.select:
                    if (Input.IsPush(Button.Esc))
                        this.parent.Close();
                    if (backpx % 8 == 0)
                    {
                        ++this.frame;
                        if (this.frame > 12)
                            ++this.starAnime;
                        if (this.starAnime >= 4)
                        {
                            this.starAnime = 0;
                            this.frame = 0;
                        }
                    }
                    this.KeyControl();
                    this.Command();
                    break;
                case SceneTitle.TITLESCENE.fade:
                    this.fadealpha += 8;
                    if (this.fadealpha >= byte.MaxValue)
                    {
                        this.fadealpha = byte.MaxValue;
                        this.parent.battlenum = 0;
                        switch (this.menu)
                        {
                            case SceneTitle.TITLEMENU.Start:
                                bool flag = false;
                                if (this.savedata.loadEnd)
                                    flag = this.savedata.FlagList[14];
                                this.savedata.Init();
                                if (flag)
                                    this.savedata.FlagList[99] = true;
                                this.parent.tutorial = false;
                                this.parent.ChangeOfSecne(Scene.Main);
                                this.parent.NewGame(this.plus);
                                break;
                            case SceneTitle.TITLEMENU.Load:
                                if (this.savedata.loadEnd)
                                {
                                    this.parent.ChangeOfSecne(Scene.Main);
                                    this.parent.LoadGame();
                                    break;
                                }
                                break;
                        }
                        this.sound.StopBGM();
                        break;
                    }
                    break;
            }
            this.backpx += 3;
            if (this.backpx < 240)
                return;
            this.backpx = 0;
        }

        private void KeyControl()
        {
            if (Input.IsPress(Button._A) || Input.IsPress(Button._Start))
            {
                this.nowscene = SceneTitle.TITLESCENE.fade;
                this.sound.PlaySE(SoundEffect.thiptransmission);
            }
            if (Input.IsPress(Button._B))
            {
                this.nowscene = SceneTitle.TITLESCENE.pushbutton;
                this.sound.PlaySE(SoundEffect.cancel);
            }
            if (this.keywait <= 0)
            {
                if (!this.printLoad)
                    return;
                if (Input.IsPush(Button.Up))
                {
                    --this.Menu;
                    this.keywait = Input.IsPress(Button.Up) ? 25 : 5;
                    this.sound.PlaySE(SoundEffect.movecursol);
                }
                if (Input.IsPush(Button.Down))
                {
                    ++this.Menu;
                    this.keywait = Input.IsPress(Button.Down) ? 25 : 5;
                    this.sound.PlaySE(SoundEffect.movecursol);
                }
            }
            else
                this.keywait = Input.IsUp(Button.Up) || Input.IsUp(Button.Down) ? 0 : this.keywait - 1;
        }

        private void Command()
        {
            if (!Input.IsPush(Button._Select))
                return;
            this.CommandInput("LR");
            if (this.CommandCheck("LRLLRLR"))
            {
                this.sound.PlaySE(SoundEffect.docking);
                this.plus = 1;
                this.CommandReset();
            }
            if (this.CommandCheck("RRLLRLL"))
            {
                this.sound.PlaySE(SoundEffect.docking);
                this.plus = 2;
                this.CommandReset();
            }
            if (this.CommandCheck("RRRLLLRL"))
            {
                this.sound.PlaySE(SoundEffect.docking);
                this.plus = 9;
                this.CommandReset();
            }
            if (this.CommandCheck("LLLRRRLR"))
            {
                this.sound.PlaySE(SoundEffect.docking);
                this.plus = -1;
                this.CommandReset();
            }
            if (this.CommandCheck("RLRLRRL"))
            {
                this.sound.PlaySE(SoundEffect.docking);
                this.plus = -2;
                this.CommandReset();
            }
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(0, 320, 240, 160);
            this._position = new Vector2(backpx, 0.0f);
            dg.DrawImage(dg, "title2", this._rect, true, this._position, false, Color.White);
            this._position = new Vector2(backpx - 240, 0.0f);
            dg.DrawImage(dg, "title2", this._rect, true, this._position, false, Color.White);
            this._position = Vector2.Zero;
            this._rect = new Rectangle(240, 160, 240, 160);
            dg.DrawImage(dg, "title2", this._rect, true, this._position, false, Color.White);
            var logoBorderSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("SceneTitle.LogoBorder");
            this._rect = logoBorderSprite.Item2;
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, logoBorderSprite.Item1, this._rect, true, this._position, false, Color.White);
            var logoTextSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("SceneTitle.LogoText");
            this._rect = logoTextSprite.Item2;
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, logoTextSprite.Item1, this._rect, true, this._position, false, Color.White);
            var logoCutoutSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("SceneTitle.LogoCutout");
            this._rect = logoCutoutSprite.Item2;
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, logoCutoutSprite.Item1, this._rect, true, this._position, false, Color.White);
            switch (this.nowscene)
            {
                case SceneTitle.TITLESCENE.pushbutton:
                    this.PushbuttonRender(dg);
                    break;
                case SceneTitle.TITLESCENE.select:
                case SceneTitle.TITLESCENE.fade:
                    this.FadeRender(dg);
                    break;
            }
            Color color = Color.FromArgb(this.fadealpha, 0, 0, 0);
            if (this.nowscene == SceneTitle.TITLESCENE.init)
                color = Color.FromArgb(this.fadealpha, Color.White);
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color);
        }

        private void PushbuttonRender(IRenderer dg)
        {
            if (!this.printpush)
                return;
            this._rect = new Rectangle(248, 0, 128, 16);
            this._position = new Vector2(120f, 120f);
            dg.DrawImage(dg, "title", this._rect, false, this._position, false, Color.White);
        }

        private void FadeRender(IRenderer dg)
        {
            Color color1 = Color.FromArgb(150, 0, 0, 0);
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, false, color1);
            if (this.test)
            {
                this._rect = new Rectangle(240, 208, 56, 32);
                if (this.menu == SceneTitle.TITLEMENU.Load)
                    this._rect.X += this._rect.Width;
                this._position = this.fontposition;
                dg.DrawImage(dg, "title", this._rect, false, this._position, false, Color.White);
            }
            else
            {
                Color color2;
                switch (this.plus)
                {
                    case 1:
                        color2 = Color.Yellow;
                        break;
                    case 2:
                        color2 = Color.Cyan;
                        break;
                    case 9:
                        color2 = Color.Red;
                        break;
                    default:
                        color2 = Color.White;
                        break;
                }
                var newGameSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("SceneTitle.NewGame");
                this._rect = newGameSprite.Item2;
                if (this.menu == SceneTitle.TITLEMENU.Load)
                    this._rect.X += this._rect.Width;
                this._position = new Vector2(this.fontposition.X - (newGameSprite.Item2.Width - 24), this.fontposition.Y - (newGameSprite.Item2.Height - 0));
                dg.DrawImage(dg, newGameSprite.Item1, this._rect, true, this._position, false, color2);
                if (this.printLoad)
                {
                    var continueSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("SceneTitle.Continue");
                    this._rect = continueSprite.Item2;
                    if (this.menu == SceneTitle.TITLEMENU.Load)
                        this._rect.X += this._rect.Width;
                    this._position = new Vector2(this.fontposition.X - (continueSprite.Item2.Width - 24), this.fontposition.Y);
                    dg.DrawImage(dg, "title", this._rect, true, this._position, false, Color.White);
                }
            }
            //this._position.X = (float)(fontposition.X - (double)(this._rect.Width / 2) - 16.0);
            this._position.X = this._position.X - 8;
            this._position.Y = this.menu == SceneTitle.TITLEMENU.Start ? this.fontposition.Y - 8f : this.fontposition.Y + 8f;
            this._rect = new Rectangle(240 + this.frame % 4 * 16, 192, 16, 16);
            dg.DrawImage(dg, "title", this._rect, false, this._position, false, Color.White);
            int num = 0;
            for (int index = 0; index < this.star.Length; ++index)
            {
                if (this.star[index])
                {
                    this._position.X = 144 + num;
                    this._position.Y = 128f;
                    this._rect = new Rectangle(632 + 16 * this.starAnime, index * 16, 16, 16);
                    dg.DrawImage(dg, "title2", this._rect, true, this._position, false, Color.White);
                    num += this.stars >= 5 ? 8 : 16;
                }
            }
            this._position = new Vector2(0.0f, 144f);
            this._rect = new Rectangle(440, 0, 64, 16);
            dg.DrawImage(dg, "title", this._rect, true, this._position, false, Color.White);
        }

        private enum TITLESCENE
        {
            init,
            pushbutton,
            select,
            fade,
        }

        private enum TITLEMENU
        {
            Start,
            Load,
        }

        private enum STAR
        {
            シナリオクリア,
            裏ボス撃破,
            スタンダードコンプ,
            ナビチップコンプ,
            ダークチップコンプ,
            PAメモコンプ,
            SPウィルス全撃破,
            SP5ナビ全撃破,
            グリモワスタイル発現,
            ラスボスSP撃破,
        }
    }
}
