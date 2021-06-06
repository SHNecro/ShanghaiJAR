using Common;
using ExtensionMethods;
using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEvent;
using NSGame;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSMap.Character.Menu
{
    internal class Navi : MenuBase
    {
        private readonly int[] pageX = new int[2];
        private readonly int[] stylenum = new int[4];
        private int flash = 0;
        private readonly bool[] canEq = new bool[3];
        private string playBGM;
        private Navi.SCENE nowscene;
        private readonly EventManager eventmanager;
        private int top;
        private const int manypage = 2;
        private const int stopLeft = 112;
        private const int stopRight = 240;
        private const int move1F = 8;
        private int page;
        private bool moving;
        private bool moveToAddBuster;
        private bool cursol;
        private bool selectstyle;
        private bool bootAddon;
        private bool boot;
        private AddOnManager manager;
        private readonly SceneMain main;
        private bool error;

        private int OverTop
        {
            get
            {
                return this.savedata.addonNames.Count - 3;
            }
        }

        private int Top
        {
            get
            {
                return this.top;
            }
            set
            {
                this.top = value;
            }
        }

        public Navi(IAudioEngine s, Player p, SceneMain m, TopMenu t, SaveData save)
          : base(s, p, t, save)
        {
            this.eventmanager = new EventManager(this.sound);
            this.Alpha = byte.MaxValue;
            this.pageX[0] = 112;
            this.pageX[1] = 240;
            this.main = m;
            this.manager = new AddOnManager(s, p, t, this, this.savedata);
        }

        public override void UpDate()
        {
            if (this.bootAddon)
            {
                this.manager.UpDate();
            }
            else
            {
                switch (this.nowscene)
                {
                    case Navi.SCENE.fadein:
                        if (this.Alpha > 0)
                        {
                            this.Alpha -= 51;
                            break;
                        }
                        this.nowscene = Navi.SCENE.select;
                        if (this.error)
                        {
                            this.error = false;
                            this.eventmanager.events.Clear();
                            var dialogue = ShanghaiEXE.Translate("Navi.InvalidFolderAddonChangeDialogue1");
                            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
                        }
                        break;
                    case Navi.SCENE.select:
                        if (this.selectstyle && !this.eventmanager.playevent)
                        {
                            this.sound.PlaySE(SoundEffect.bright);
                            this.savedata.setstyle = this.stylenum[this.savedata.selectQuestion];
                            this.flash = byte.MaxValue;
                            this.selectstyle = false;
                            this.eventmanager.ClearEvent();
                        }
                        this.Control();
                        if (this.eventmanager.playevent)
                            this.eventmanager.UpDate();
                        if (this.moving)
                        {
                            if (!this.moveToAddBuster)
                            {
                                this.pageX[0] += 8;
                                this.pageX[1] -= 8;
                                if (this.pageX[1] <= 112)
                                {
                                    this.pageX[0] = 240;
                                    this.pageX[1] = 112;
                                    ++this.page;
                                    this.moving = false;
                                }
                            }
                            else
                            {
                                this.pageX[0] -= 8;
                                this.pageX[1] += 8;
                                if (this.pageX[0] <= 112)
                                {
                                    this.pageX[0] = 112;
                                    this.pageX[1] = 240;
                                    --this.page;
                                    this.moving = false;
                                }
                            }
                        }
                        if (this.flash > 0)
                            this.flash -= 5;
                        this.FlamePlus();
                        break;
                    case Navi.SCENE.fadeout:
                        if (this.Alpha < byte.MaxValue)
                        {
                            this.Alpha += 51;
                            break;
                        }
                        if (!this.boot)
                            this.topmenu.Return();
                        else
                            this.bootAddon = true;
                        break;
                }
            }
        }

        public void Control()
        {
            if (!this.eventmanager.playevent)
            {
                bool flag1 = this.savedata.havestyles > 1;
                bool flag2 = this.savedata.haveAddon.Count > 0;
                if (Input.IsPress(Button._A))
                {
                    this.sound.PlaySE(SoundEffect.decide);
                    if (!flag1 & flag2)
                        this.BootAddonManager();
                    if (flag1 && !flag2)
                        this.StyleChange();
                    if (flag1 & flag2)
                    {
                        if (this.cursol)
                            this.StyleChange();
                        else
                            this.BootAddonManager();
                    }
                }
                if (flag1 & flag2 && this.waittime <= 0)
                {
                    if (Input.IsPush(Button.Up))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.cursol = !this.cursol;
                        this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                    }
                    if (Input.IsPush(Button.Down))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.cursol = !this.cursol;
                        this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                    }
                }
            }
            if (Input.IsPress(Button._B))
            {
                this.sound.PlaySE(SoundEffect.cancel);
                if (this.selectstyle)
                {
                    this.selectstyle = false;
                    this.eventmanager.ClearEvent();
                }
                else
                {
                    this.boot = false;
                    this.nowscene = Navi.SCENE.fadeout;
                }
            }
            if (this.savedata.haveAddon.Count > 0 && !this.moving && Input.IsPress(Button._Select))
            {
                this.sound.PlaySE(SoundEffect.menuopen);
                this.moveToAddBuster = this.page != 0;
                this.moving = true;
            }
            if (this.page == 1 && this.waittime <= 0)
            {
                if (Input.IsPush(Button._R) && this.Top < this.OverTop)
                {
                    ++this.Top;
                    this.sound.PlaySE(SoundEffect.movecursol);
                    this.waittime = Input.IsPress(Button._R) ? 10 : 4;
                }
                if (!Input.IsPush(Button._L) || this.Top <= 0)
                    return;
                --this.Top;
                this.sound.PlaySE(SoundEffect.movecursol);
                this.waittime = Input.IsPress(Button._L) ? 10 : 4;
            }
            else if (this.waittime > 0)
                --this.waittime;
        }

        private string StyleGlaphics()
        {
            int style = this.savedata.style[this.savedata.setstyle].style;
            string str = "";
            switch (style)
            {
                case 0:
                    return "shanghai";
                case 1:
                    str = "Fighter";
                    break;
                case 2:
                    str = "Shinobi";
                    break;
                case 3:
                    str = "Doctor";
                    break;
                case 4:
                    str = "Gaia";
                    break;
                case 5:
                    str = "Wing";
                    break;
                case 6:
                    str = "Witch";
                    break;
            }
            switch (this.savedata.style[this.savedata.setstyle].element)
            {
                case 1:
                    str += "Heat";
                    break;
                case 2:
                    str += "Aqua";
                    break;
                case 3:
                    str += "Eleki";
                    break;
                case 4:
                    str += "Leaf";
                    break;
                case 5:
                    str += "Poison";
                    break;
                case 6:
                    str += "Earth";
                    break;
            }
            return str;
        }

        public override void Render(IRenderer dg)
        {
            if (this.bootAddon)
            {
                this.manager.Render(dg);
            }
            else
            {
                this._rect = new Rectangle(240, 624, 240, 160);
                this._position = new Vector2(0.0f, 0.0f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                this._rect = new Rectangle(0, 0, 120, 120);
                this._position = new Vector2(40f, 48f);
                dg.DrawImage(dg, this.StyleGlaphics(), this._rect, false, this._position, Color.White);
                if (this.flash > 0)
                {
                    this._rect = new Rectangle(120 * this.savedata.style[this.savedata.setstyle].style, 0, 120, 120);
                    this._position = new Vector2(40f, 48f);
                    dg.DrawImage(dg, "Silhouette", this._rect, false, this._position, Color.FromArgb(this.flash, Color.White));
                }
                this.TextRender(dg, this.savedata.style[this.savedata.setstyle].name, false, new Vector2(32f, 80f), false);
                this._rect = new Rectangle(272, 240, 88, 24);
                this._position = new Vector2(8f, 16f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                int[] numArray1 = this.ChangeCount(this.savedata.HPnow);
                Color color1 = savedata.HPMax * 0.3 <= savedata.HPnow ? Color.White : Color.FromArgb(byte.MaxValue, byte.MaxValue, 150, 50);
                Vector2 position = new Vector2(40f, 16f);
                for (int index = 0; index < numArray1.Length; ++index)
                {
                    this._rect = new Rectangle(numArray1[index] * 8, 104, 8, 16);
                    this._position = new Vector2(position.X - index * 8, position.Y);
                    dg.DrawImage(dg, "font", this._rect, true, this._position, color1);
                }
                int[] numArray2 = this.ChangeCount(this.savedata.HPMax);
                Color white = Color.White;
                position = new Vector2(48 + 8 * numArray2.Length, 16f);
                for (int index = 0; index < numArray2.Length; ++index)
                {
                    this._rect = new Rectangle(numArray2[index] * 8, 104, 8, 16);
                    this._position = new Vector2(position.X - index * 8, position.Y);
                    dg.DrawImage(dg, "font", this._rect, true, this._position, white);
                }
                this._rect = new Rectangle(360, 144, 112, 88);
                this._position = new Vector2(this.pageX[0], 16f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                AllBase.NAME[] nameArray1 = this.Nametodata(ShanghaiEXE.Translate("Navi.Buster"));
                position = new Vector2(this.pageX[0] + 8, 18f);
                this._position = new Vector2(position.X, position.Y);
                DrawBlockCharacters(dg, nameArray1, 88, this._position, Color.White, out this._rect, out this._position);
                for (int index = 0; index < this.savedata.busterspec.Length; ++index)
                {
                    string str = "";
                    switch (index)
                    {
                        case 0:
                            str = ShanghaiEXE.Translate("Navi.Power");
                            break;
                        case 1:
                            str = ShanghaiEXE.Translate("Navi.Rapid");
                            break;
                        case 2:
                            str = ShanghaiEXE.Translate("Navi.Charge");
                            break;
                    }
                    position = new Vector2(this.pageX[0] + 16, 44 + 16 * index);
                    this.TextRender(dg, str, false, position, true);
                    this._rect = new Rectangle(this.savedata.busterspec[index] * 8, 0, 8, 16);
                    this._position = new Vector2(this._position.X + 16f, this._position.Y);
                    dg.DrawImage(dg, "font", this._rect, true, this._position, Color.Yellow);
                }
                if (this.savedata.haveAddon.Count > 0)
                {
                    this._rect = new Rectangle(320, 48 + (Input.IsPush(Button._Select) ? 16 : 0), 32, 16);
                    this._position = new Vector2(184f, 16f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    this._rect = new Rectangle(360, 144, 112, 88);
                    this._position = new Vector2(this.pageX[1], 16f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    position = new Vector2(this.pageX[1] + 8, 18f);
                    this.TextRender(dg, ShanghaiEXE.Translate("Navi.AddOn"), false, position, false);
                    for (int index = 0; index < 3; ++index)
                    {
                        string txt = this.savedata.addonNames.Count - 1 >= this.Top + index
                            ? this.savedata.addonNames[this.Top + index]
                            : ShanghaiEXE.Translate("Navi.NoAddOns");
                    position = new Vector2(this.pageX[1] + 8, 44 + 16 * index);
                        this.TextRender(dg, txt, false, position, true);
                    }
                    float num = this.OverTop != 0 && this.Top != 0 ? 48f / OverTop * Top : 0.0f;
                    this._rect = new Rectangle(176, 168, 8, 8);
                    this._position = new Vector2(this.pageX[1] + 104, 40f + num);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    if (this.page == 1 && !this.moving)
                    {
                        this._rect = new Rectangle(304, 80 + (Input.IsPush(Button._L) ? 16 : 0), 16, 16);
                        this._position = new Vector2(168f, 16f);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        this._rect = new Rectangle(320, 80 + (Input.IsPush(Button._R) ? 16 : 0), 16, 16);
                        this._position = new Vector2(216f, 16f);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    }
                }
                this._position = new Vector2(5f, 108f);
                this._rect = new Rectangle(0, 0, 40, 48);
                dg.DrawImage(dg, "Face1", this._rect, true, this._position, Color.White);
                if (this.eventmanager.playevent)
                {
                    this.eventmanager.Render(dg);
                }
                else
                {
                    this._position = new Vector2(48f, 108f);
                    dg.DrawText(ShanghaiEXE.Translate("Navi.StatusMessage"), this._position);
                    bool flag1 = this.savedata.havestyles > 1;
                    bool flag2 = this.savedata.haveAddon.Count > 0;
                    if (!flag1 & flag2)
                    {
                        this._position = new Vector2(48f, 124f);
                        dg.DrawText(ShanghaiEXE.Translate("Navi.AddOnManager"), this._position);
                    }
                    else if (flag1 && !flag2)
                    {
                        this._position = new Vector2(48f, 124f);
                        dg.DrawText(ShanghaiEXE.Translate("Navi.ChangeStyle"), this._position);
                    }
                    else if (flag1 & flag2)
                    {
                        this._position = new Vector2(48f, 124f);
                        dg.DrawText(ShanghaiEXE.Translate("Navi.AddOnManager"), this._position);
                        this._position = new Vector2(48f, 140f);
                        dg.DrawText(ShanghaiEXE.Translate("Navi.ChangeStyle"), this._position);
                    }
                    if (flag1 | flag2)
                    {
                        this._position = new Vector2(40f, 124 + (this.cursol ? 16 : 0));
                        this._rect = new Rectangle(240 + this.frame / 4 % 3 * 16, 48, 16, 16);
                        dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
                    }
                }
                if (this.nowscene == Navi.SCENE.fadein || this.nowscene == Navi.SCENE.fadeout)
                {
                    Color color2 = Color.FromArgb(this.Alpha, 0, 0, 0);
                    this._rect = new Rectangle(0, 0, 240, 160);
                    this._position = new Vector2(0.0f, 0.0f);
                    dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color2);
                }
            }
        }

        private void StyleChange()
        {
            if (this.savedata.havestyles > 2)
            {
                string[] strArray = new string[4];
                int index1 = 0;
                for (int index2 = 0; index2 < this.savedata.havestyles; ++index2)
                {
                    if (index2 != this.savedata.setstyle)
                    {
                        strArray[index1] = this.savedata.style[index2].name;
                        this.stylenum[index1] = index2;
                        ++index1;
                    }
                }
                this.selectstyle = true;
                this.eventmanager.events.Clear();
                switch (this.savedata.havestyles)
                {
                    case 3:
                        this.eventmanager.AddEvent(new Question(this.sound, this.eventmanager, "", strArray[0], strArray[1], true, true, true, FACE.Shanghai.ToFaceId(), this.savedata));
                        break;
                    case 4:
                        this.eventmanager.AddEvent(new Question(this.sound, this.eventmanager, strArray[0], strArray[1], strArray[2], true, true, FACE.Shanghai.ToFaceId(), this.savedata));
                        break;
                    case 5:
                        this.eventmanager.AddEvent(new Question(this.sound, this.eventmanager, "", strArray[0], strArray[1], strArray[2], strArray[3], true, true, FACE.Shanghai.ToFaceId(), this.savedata));
                        break;
                }
            }
            else
            {
                this.sound.PlaySE(SoundEffect.bright);
                if (this.savedata.setstyle == 0)
                    ++this.savedata.setstyle;
                else
                    --this.savedata.setstyle;
                this.flash = byte.MaxValue;
            }
        }

        private void BootAddonManager()
        {
            this.boot = true;
            this.nowscene = Navi.SCENE.fadeout;
            this.playBGM = this.sound.CurrentBGM;
            this.sound.StartBGM("add_on");
            this.manager = new AddOnManager(this.sound, this.player, this.topmenu, this, this.savedata);
        }

        public void Back()
        {
            this.sound.StartBGM(this.playBGM);
            this.boot = false;
            this.bootAddon = false;
            this.Top = 0;
            this.nowscene = Navi.SCENE.fadein;
            this.FolderCheckAll();
        }

        public void FolderCheckAll()
        {
            this.canEq[0] = this.FolderCheck(0);
            this.canEq[1] = this.FolderCheck(1);
            this.canEq[2] = true;
            if (this.canEq[this.savedata.efolder])
                return;
            this.savedata.efolder = 2;
            this.error = true;
        }

        public bool FolderCheck(int checkFolder)
        {
            int num1 = 0;
            int num2 = 0;
            ChipFolder chipFolder = new ChipFolder(this.sound);
            for (int index = 0; index < 30; ++index)
            {
                if (this.main.chipfolder[checkFolder, index].chip.navi)
                    ++num1;
                if (this.main.chipfolder[checkFolder, index].chip.dark)
                    ++num2;
                chipFolder.SettingChip(this.main.chipfolder[checkFolder, index].chip.number);
                if (chipFolder.chip is DammyChip)
                    return false;
                int num3 = this.Manychip(chipFolder.chip.number, checkFolder);
                if (chipFolder.chip.regsize <= 19 && num3 > 5 + this.savedata.plusFolder || chipFolder.chip.regsize > 19 && chipFolder.chip.regsize <= 29 && num3 > 4 + this.savedata.plusFolder || (chipFolder.chip.regsize > 29 && chipFolder.chip.regsize <= 39 && num3 > 3 + this.savedata.plusFolder || chipFolder.chip.regsize > 39 && chipFolder.chip.regsize <= 49 && num3 > 2 + this.savedata.plusFolder) || (chipFolder.chip.regsize > 49 && num3 > 1 + this.savedata.plusFolder || num1 > this.savedata.NaviFolder || num2 > this.savedata.darkFolder))
                    return false;
            }
            return true;
        }

        public int Manychip(int no, int select)
        {
            int num = 0;
            for (int index = 0; index < 30; ++index)
            {
                if (!(this.main.chipfolder[select, index].chip is DammyChip) && this.main.chipfolder[select, index].chip.number == no)
                    ++num;
            }
            return num;
        }

        private enum SCENE
        {
            fadein,
            select,
            fadeout,
        }
    }
}
