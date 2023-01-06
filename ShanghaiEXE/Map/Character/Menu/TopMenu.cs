using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System;

namespace NSMap.Character.Menu
{
    public class TopMenu : AllBase
    {
        private readonly byte waitlong = 10;
        private readonly byte waitshort = 4;
        private TopMenu.TOPMENU selectmenu;
        private TopMenu.TOPMENUSCENE nowscene;
        private readonly Player player;
        private MenuBase menu;
        private bool noTop;
        public const byte TopmenuSelect = 9;
        protected SaveData savedata;
        private readonly SceneMain main;
        private const int berStart = 240;
        private const int berDifference = 48;
        private const int phoneStart = -112;
        private const int speed = 16;
        private int bertopX;
        private int berunderX;
        private byte printmenu;
        private int phoneX;
        private const string texture = "menuwindows";
        private int alphaUnderFade;
        private int alphaTopFade;
        private int iconX;
        private bool iconBright;
        private int waittime;
        private bool[] canselectmenu;

        public TopMenu(IAudioEngine s, Player p, SceneMain m, SaveData save)
          : base(s)
        {
            this.savedata = save;
            this.main = m;
            this.player = p;
            this.Init();
        }

        public void Init()
        {
            this.nowscene = TopMenu.TOPMENUSCENE.init;
            this.selectmenu = TopMenu.TOPMENU.chipfolder;
            this.canselectmenu = new bool[this.savedata.canselectmenu.Length];
            for (int index = 0; index < this.savedata.canselectmenu.Length; ++index)
                this.canselectmenu[index] = this.savedata.canselectmenu[index];
            if (this.savedata.FlagList[0])
            {
                if (!this.savedata.isJackedIn)
                {
                    this.canselectmenu[1] = false;
                    this.canselectmenu[3] = false;
                    this.canselectmenu[4] = false;
                    this.canselectmenu[6] = false;
                }
                else
                {
                    this.canselectmenu[0] = false;
                    this.canselectmenu[1] = false;
                    this.canselectmenu[3] = false;
                    this.canselectmenu[4] = false;
                    this.canselectmenu[5] = false;
                    this.canselectmenu[6] = false;
                }
            }
            while (!this.canselectmenu[(int)this.selectmenu])
            {
                ++this.selectmenu;
                if (this.selectmenu > TopMenu.TOPMENU.save)
                    this.selectmenu = TopMenu.TOPMENU.chipfolder;
            }
            this.bertopX = 240;
            this.berunderX = 288;
            this.printmenu = 0;
            this.phoneX = -112;
            this.alphaUnderFade = 0;
            this.iconBright = false;
        }

        public void Return()
        {
            this.noTop = false;
            this.nowscene = TopMenu.TOPMENUSCENE.backmenu;
        }

        public void UpDate()
        {
            if (!this.noTop)
                this.Topmenu();
            else
                this.menu.UpDate();
            this.FlamePlus();
        }

        private void Control()
        {
            if (Input.IsPress(Button._A))
            {
                this.sound.PlaySE(SoundEffect.decide);
                this.nowscene = TopMenu.TOPMENUSCENE.gomenu;
                switch (this.selectmenu)
                {
                    case TopMenu.TOPMENU.chipfolder:
                        this.menu = new FolderSelect(this.sound, this.player, this.main, this, this.savedata);
                        break;
                    case TopMenu.TOPMENU.subchip:
                        this.menu = new SubChip(this.sound, this.player, this, this.main.eventmanager, this.savedata);
                        break;
                    case TopMenu.TOPMENU.datelist:
                        this.menu = new Library(this.sound, this.player, this, this.savedata);
                        break;
                    case TopMenu.TOPMENU.navi:
                        this.menu = new Navi(this.sound, this.player, this.main, this, this.savedata);
                        break;
                    case TopMenu.TOPMENU.virus:
                        this.menu = new Virus(this.sound, this.player, this, this.savedata);
                        break;
                    case TopMenu.TOPMENU.mail:
                        this.menu = new MailMenu(this.sound, this.player, this, this.savedata);
                        break;
                    case TopMenu.TOPMENU.keyitem:
                        this.menu = new KeyItemMenu(this.sound, this.player, this, this.savedata);
                        break;
                    case TopMenu.TOPMENU.network:
                        // this.menu = new NetWork(this.sound, this.player, this, this.savedata);
                        break;
                    case TopMenu.TOPMENU.save:
                        this.menu = new Save(this.sound, this.player, this, this.savedata, this.main);
                        break;
                }
            }
            else
            {
                if (Input.IsPress(Button._B) || Input.IsPress(Button._Start))
                {
                    this.sound.PlaySE(SoundEffect.menuclose);
                    this.nowscene = TopMenu.TOPMENUSCENE.end;
                }
                bool flag1 = false;
                if (this.waittime <= 0)
                {
                    if (Input.IsPush(Button.Up))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        --this.selectmenu;
                        if (this.selectmenu < TopMenu.TOPMENU.chipfolder)
                            this.selectmenu = TopMenu.TOPMENU.save;
                        while (!this.canselectmenu[(int)this.selectmenu])
                        {
                            --this.selectmenu;
                            if (this.selectmenu < TopMenu.TOPMENU.chipfolder)
                                this.selectmenu = TopMenu.TOPMENU.save;
                        }
                        flag1 = true;
                        this.waittime = Input.IsPress(Button.Up) ? waitlong : waitshort;
                    }
                    if (Input.IsPush(Button.Down))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        ++this.selectmenu;
                        if (this.selectmenu > TopMenu.TOPMENU.save)
                            this.selectmenu = TopMenu.TOPMENU.chipfolder;
                        while (!this.canselectmenu[(int)this.selectmenu])
                        {
                            ++this.selectmenu;
                            if (this.selectmenu > TopMenu.TOPMENU.save)
                                this.selectmenu = TopMenu.TOPMENU.chipfolder;
                        }
                        flag1 = true;
                        this.waittime = Input.IsPress(Button.Down) ? waitlong : waitshort;
                    }
                    if (Input.IsPress(Button.Left))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.selectmenu = TopMenu.TOPMENU.chipfolder;
                        while (!this.canselectmenu[(int)this.selectmenu])
                        {
                            ++this.selectmenu;
                            if (this.selectmenu < TopMenu.TOPMENU.chipfolder)
                                this.selectmenu = TopMenu.TOPMENU.save;
                        }
                        flag1 = true;
                        this.waittime = Input.IsPress(Button.Left) ? waitlong : waitshort;
                    }
                    if (Input.IsPress(Button.Right))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.selectmenu = TopMenu.TOPMENU.save;
                        while (!this.canselectmenu[(int)this.selectmenu])
                        {
                            --this.selectmenu;
                            if (this.selectmenu > TopMenu.TOPMENU.save)
                                this.selectmenu = TopMenu.TOPMENU.chipfolder;
                        }
                        flag1 = true;
                        this.waittime = Input.IsPress(Button.Right) ? waitlong : waitshort;
                    }
                }
                else
                    --this.waittime;
                if (flag1)
                    this.iconX = 0;
                bool flag2 = false;
                for (int index = 0; index < 10; ++index)
                {
                    if (Input.IsPush((Button)index))
                        flag2 = true;
                }
                if (!flag2)
                    this.waittime = 0;
            }
        }

        public void Render(IRenderer dg)
        {
            if (!this.noTop)
                this.TopmenuRender(dg);
            else
                this.menu.Render(dg);
        }

        private void Topmenu()
        {
            switch (this.nowscene)
            {
                case TopMenu.TOPMENUSCENE.init:
                    bool flag1 = true;
                    if (this.bertopX > 0)
                    {
                        this.bertopX -= 16;
                        flag1 = false;
                    }
                    if (this.berunderX > 0)
                    {
                        this.berunderX -= 16;
                        flag1 = false;
                    }
                    if (this.printmenu < 9)
                    {
                        ++this.printmenu;
                        flag1 = false;
                    }
                    if (this.phoneX < 112)
                    {
                        this.phoneX += 16;
                        flag1 = false;
                    }
                    this.alphaUnderFade += 8;
                    if (!flag1)
                        break;
                    ++this.nowscene;
                    break;
                case TopMenu.TOPMENUSCENE.select:
                    if (this.frame % 16 == 0)
                        this.iconBright = !this.iconBright;
                    if (this.iconX < 8)
                        this.iconX += 2;
                    this.Control();
                    break;
                case TopMenu.TOPMENUSCENE.gomenu:
                    if (this.alphaTopFade < byte.MaxValue)
                    {
                        this.alphaTopFade += 51;
                        if (this.alphaTopFade <= byte.MaxValue)
                            break;
                        this.alphaTopFade = byte.MaxValue;
                        break;
                    }
                    this.noTop = true;
                    break;
                case TopMenu.TOPMENUSCENE.backmenu:
                    if (this.alphaTopFade > 0)
                    {
                        this.alphaTopFade -= 51;
                        if (this.alphaTopFade >= 0)
                            break;
                        this.alphaTopFade = 0;
                        break;
                    }
                    if (this.player.openmystery)
                        this.player.CloseMenu();
                    else
                        this.nowscene = TopMenu.TOPMENUSCENE.select;
                    break;
                case TopMenu.TOPMENUSCENE.end:
                    bool flag2 = true;
                    if (this.bertopX < 240)
                    {
                        this.bertopX += 16;
                        flag2 = false;
                    }
                    if (this.berunderX < 288)
                    {
                        this.berunderX += 16;
                        flag2 = false;
                    }
                    if (this.printmenu > 0)
                    {
                        --this.printmenu;
                        flag2 = false;
                    }
                    if (this.phoneX > -112)
                    {
                        this.phoneX -= 16;
                        flag2 = false;
                    }
                    if (this.alphaUnderFade > 0)
                        this.alphaUnderFade -= 8;
                    if (!flag2)
                        break;
                    this.player.CloseMenu();
                    break;
            }
        }

        private static string[,] MenuSprites = {
            { "MainMenu.FolderUnselected", "MainMenu.FolderSelected" },
            { "MainMenu.SubChipUnselected", "MainMenu.SubChipSelected" },
            { "MainMenu.LibraryUnselected", "MainMenu.LibrarySelected" },
            { "MainMenu.NaviUnselected", "MainMenu.NaviSelected" },
            { "MainMenu.VirusUnselected", "MainMenu.VirusSelected" },
            { "MainMenu.MailUnselected", "MainMenu.MailSelected" },
            { "MainMenu.KeyItemUnselected", "MainMenu.KeyItemSelected" },
            { "MainMenu.NetworkUnselected", "MainMenu.NetworkSelected" },
            { "MainMenu.SaveUnselected", "MainMenu.SaveSelected" }
        };
        private Tuple<string, Rectangle> GetMenuSprite(bool unSelected, int index)
        {
            return ShanghaiEXE.languageTranslationService.GetLocalizedSprite(MenuSprites[index, unSelected ? 0 : 1]);
        }

        private void TopmenuRender(IRenderer dg)
        {
            Color color1 = Color.FromArgb(this.alphaUnderFade, 0, 0, 0);
            if (!this.player.openmystery)
            {
                this._rect = new Rectangle(0, 0, 240, 160);
                this._position = new Vector2(0.0f, 0.0f);
                dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color1);
                this._rect = new Rectangle(0, 144, 152, 8);
                this._position = new Vector2(bertopX, 0.0f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                this._rect = new Rectangle(0, 152, 152, 8);
                this._position = new Vector2(berunderX, 152f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                for (int i = 0; i < printmenu; ++i)
                {
                    if (this.canselectmenu[i])
                    {
                        var menuSprite = this.GetMenuSprite(this.SelectNumber(i), i);
                        this._rect = menuSprite.Item2;
                        this._position = new Vector2(24f, 8 + 16 * i);
                        dg.DrawImage(dg, menuSprite.Item1, this._rect, true, this._position, Color.White);
                        this._rect = new Rectangle(this.iconBright || this.SelectNumber(i) ? 0 : 16, 16 * i, 16, 16);
                        this._position = new Vector2(this.selectmenu != (TopMenu.TOPMENU)i ? 8f : 8 + this.iconX, 8 + 16 * i);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    }
                }
                this._rect = new Rectangle(0, 160, 112, 144);
                this._position = new Vector2(phoneX, 8f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                if (this.nowscene == TopMenu.TOPMENUSCENE.select || this.nowscene == TopMenu.TOPMENUSCENE.gomenu || this.nowscene == TopMenu.TOPMENUSCENE.backmenu)
                {
                    this._rect = new Rectangle(176, 0, 16, 8);
                    this._position = new Vector2(128f, 32f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    this._rect = new Rectangle(216, 0, 16, 16);
                    this._position = new Vector2(160f, 40f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    int[] numArray1 = this.ChangeCount(this.savedata.HPnow);
                    Color color2 = savedata.HPMax * 0.3 <= savedata.HPnow ? Color.White : Color.FromArgb(byte.MaxValue, byte.MaxValue, 150, 50);
                    PointF pointF1 = new PointF(154f, 40f);
                    for (int index = 0; index < numArray1.Length; ++index)
                    {
                        this._rect = new Rectangle(numArray1[index] * 8, 104, 8, 16);
                        this._position = new Vector2(pointF1.X - index * 8, pointF1.Y);
                        dg.DrawImage(dg, "font", this._rect, true, this._position, color2);
                    }
                    int[] numArray2 = this.ChangeCount(this.savedata.HPMax);
                    Color white1 = Color.White;
                    PointF pointF2 = new PointF(164 + 8 * numArray2.Length, 40f);
                    for (int index = 0; index < numArray2.Length; ++index)
                    {
                        this._rect = new Rectangle(numArray2[index] * 8, 104, 8, 16);
                        this._position = new Vector2(pointF2.X - index * 8, pointF2.Y);
                        dg.DrawImage(dg, "font", this._rect, true, this._position, white1);
                    }
                    this._rect = new Rectangle(176, 8, 40, 8);
                    this._position = new Vector2(128f, 64f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    int[] numArray3 = this.ChangeCount(this.savedata.Money);
                    Color white2 = Color.White;
                    if (this.savedata.Money < this.savedata.moneyover)
                    {
                        PointF pointF3 = new PointF(192f, 72f);
                        for (int index = 0; index < numArray3.Length; ++index)
                        {
                            this._rect = new Rectangle(numArray3[index] * 8, 104, 8, 16);
                            this._position = new Vector2(pointF3.X - index * 8, pointF3.Y);
                            dg.DrawImage(dg, "font", this._rect, true, this._position, white2);
                        }
                    }
                    else
                    {
                        this._position = new Vector2(128f, 72f);
                        this.TextRender(dg, ShanghaiEXE.Translate("TopMenu.Billionaire"), false, this._position, false);
                    }
                    this._rect = new Rectangle(320, 0, 16, 48);
                    this._position = new Vector2(128f, 88f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    for (int index = 0; index < 3; ++index)
                    {
                        this.ChangeCount(this.savedata.havePeace[index]);
                        Color white3 = Color.White;
                        PointF pointF3 = new PointF(144f, 86 + index * 16);
                        var names = this.Nametodata(string.Format(ShanghaiEXE.Translate("TopMenu.FragCountFormat"), this.savedata.havePeace[index]));
                        this._position = new Vector2(pointF3.X, pointF3.Y);
                        DrawBlockCharacters(dg, names, 88, this._position, white3, out this._rect, out this._position);
                    }
                }
            }
            if (this.nowscene != TopMenu.TOPMENUSCENE.gomenu && this.nowscene != TopMenu.TOPMENUSCENE.backmenu)
                return;
            Color color3 = Color.FromArgb(this.alphaTopFade, 0, 0, 0);
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color3);
        }

        private bool SelectNumber(int i)
        {
            return this.selectmenu != (TopMenu.TOPMENU)i;
        }

        public enum TOPMENU
        {
            chipfolder,
            subchip,
            datelist,
            navi,
            virus,
            mail,
            keyitem,
            network,
            save,
        }

        private enum TOPMENUSCENE
        {
            init,
            select,
            gomenu,
            backmenu,
            end,
        }
    }
}
