using NSAddOn;
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
    internal class AddOnManager : MenuBase
    {
        private readonly List<AddOnColor> colors = new List<AddOnColor>();
        private int useAlpha = byte.MaxValue;
        private readonly bool[] setcolor = new bool[8];
        private int moveAlpha = -51;
        private readonly int barlong = 176;
        private AddOnManager.SCENE nowscene;
        private int haveHz;
        private int haveCore;
        private const int maxHz = 40;
        private const int maxCore = 10;
        private int nowHz;
        private int nowCore;
        private int top;
        private readonly int overTop;
        private readonly bool[] setAddOn;
        private int select;
        private int cursol;
        private bool upper;
        private bool flash;
        private readonly Navi navi;
        private int cursolanime;
        private bool sortflag;
        private AddOnManager.SORT sortselect;
        private readonly EventManager eventmanager;
        private const int printSpeed = 1;
        protected int printfonts;
        private int printedItems;
        private string shortmassage;
        private string massage;
        private int barprint;
        private bool runend;
        private int barAlpha;
        private bool alphaminas;
        private bool change;

        private int UseHz
        {
            get
            {
                return this.haveHz - this.nowHz;
            }
        }

        private int UseCore
        {
            get
            {
                return this.haveCore - this.nowCore;
            }
        }

        private AddOnManager.SORT SortSelect
        {
            get
            {
                return this.sortselect;
            }
            set
            {
                this.sortselect = value;
                if (this.sortselect < AddOnManager.SORT.core)
                    this.sortselect = AddOnManager.SORT.set;
                if (this.sortselect <= AddOnManager.SORT.set)
                    return;
                this.sortselect = AddOnManager.SORT.core;
            }
        }

        private AddOnBase Selectaddon
        {
            get
            {
                return this.savedata.haveAddon[this.select];
            }
            set
            {
                this.savedata.haveAddon[this.select] = value;
            }
        }

        public AddOnManager(IAudioEngine s, Player p, TopMenu t, Navi n, SaveData save)
          : base(s, p, t, save)
        {
            this.overTop = this.savedata.haveAddon.Count - 6;
            this.navi = n;
            this.Alpha = byte.MaxValue;
            this.haveCore = this.savedata.MaxCore;
            this.haveHz = this.savedata.MaxHz;
            this.nowCore = this.haveCore;
            this.nowHz = this.haveHz;
            List<bool> boolList = new List<bool>();
            for (int index = 0; index < this.savedata.haveAddon.Count; ++index)
                boolList.Add(this.savedata.equipAddon[index]);
            this.setAddOn = boolList.ToArray();
            foreach (var data in ((IEnumerable<bool>)this.setAddOn).Select((v, j) => new
            {
                v,
                j
            }))
            {
                if (data.j < this.savedata.haveAddon.Count && data.v)
                {
                    if (this.savedata.haveAddon[data.j].Plus)
                    {
                        this.haveCore += this.savedata.haveAddon[data.j].UseCore;
                        this.haveHz += this.savedata.haveAddon[data.j].UseHz;
                        this.nowCore += this.savedata.haveAddon[data.j].UseCore;
                        this.nowHz += this.savedata.haveAddon[data.j].UseHz;
                        if ((uint)this.savedata.haveAddon[data.j].color > 0U)
                            this.setcolor[(int)this.savedata.haveAddon[data.j].color] = true;
                    }
                    else
                    {
                        this.nowCore -= this.savedata.haveAddon[data.j].UseCore;
                        this.nowHz -= this.savedata.haveAddon[data.j].UseHz;
                        this.AddonColorSet(data.j);
                        if ((uint)this.savedata.haveAddon[data.j].color > 0U)
                            this.setcolor[(int)this.savedata.haveAddon[data.j].color] = true;
                    }
                }
            }
            this.eventmanager = new EventManager(this.sound);
        }

        public override void UpDate()
        {
            switch (this.nowscene)
            {
                case AddOnManager.SCENE.fadein:
                    if (this.Alpha > 0)
                    {
                        this.Alpha -= 51;
                        break;
                    }
                    this.Tutorial();
                    this.nowscene = AddOnManager.SCENE.select;
                    break;
                case AddOnManager.SCENE.select:
                    if (this.eventmanager.playevent)
                    {
                        this.eventmanager.UpDate();
                        break;
                    }
                    if (!this.sortflag)
                        this.Control();
                    else
                        this.SortControl();
                    break;
                case AddOnManager.SCENE.runnning:
                    this.Running();
                    break;
                case AddOnManager.SCENE.eroor:
                    if (this.eventmanager.playevent)
                    {
                        this.eventmanager.UpDate();
                        break;
                    }
                    this.nowscene = this.savedata.selectQuestion != 0 ? AddOnManager.SCENE.select : AddOnManager.SCENE.fadeout;
                    break;
                case AddOnManager.SCENE.fadeout:
                    if (this.Alpha < byte.MaxValue)
                    {
                        this.Alpha += 51;
                        break;
                    }
                    this.navi.Back();
                    break;
            }
            if (this.frame % 5 == 0)
                this.flash = !this.flash;
            if (this.frame % 10 == 0)
                ++this.cursolanime;
            if (this.cursolanime >= 3)
                this.cursolanime = 0;
            this.MoveAlpha();
            this.FlamePlus();
        }

        private void Running()
        {
            bool flag1 = false;
            bool flag2 = false;
            if (!this.runend)
            {
                if (this.alphaminas)
                {
                    if (this.barAlpha < 40)
                        this.alphaminas = false;
                    else
                        --this.barAlpha;
                }
                else if (this.barAlpha > 140)
                    this.alphaminas = true;
                else
                    ++this.barAlpha;
                if (this.barprint < this.barlong)
                    ++this.barprint;
                else
                    flag1 = true;
                if (this.OneSet())
                {
                    if (this.frame % 1 == 0)
                    {
                        ++this.printfonts;
                        if (this.printfonts > this.massage.Length)
                        {
                            ++this.printedItems;
                            if (this.printedItems >= this.savedata.addonNames.Count)
                            {
                                flag2 = true;
                            }
                            else
                            {
                                this.massage = this.savedata.addonNames.Count <= 8 ? this.savedata.addonNames[this.printedItems] + "...           " : this.savedata.addonNames[this.printedItems] + "...";
                                this.shortmassage = "";
                                this.printfonts = 0;
                            }
                        }
                        else
                        {
                            this.shortmassage += this.ToDecomposition(this.massage)[this.printfonts - 1];
                            if (this.printfonts - 1 < this.savedata.addonNames[this.printedItems].Length)
                                this.sound.PlaySE(SoundEffect.openchip);
                        }
                    }
                }
                else
                    flag2 = true;
                if (!(flag1 & flag2))
                    return;
                this.sound.PlaySE(SoundEffect.bright);
                this.shortmassage = "OK!";
                this.runend = true;
            }
            else if (this.barAlpha > 0)
            {
                --this.barAlpha;
            }
            else
            {
                this.runend = false;
                this.nowscene = AddOnManager.SCENE.select;
            }
        }

        private void SortControl()
        {
            if (Input.IsPress(Button._A))
            {
                this.sound.PlaySE(SoundEffect.decide);
                this.Sort(this.SortSelect);
                this.upper = !this.upper;
            }
            if (Input.IsPress(Button._B))
            {
                this.sound.PlaySE(SoundEffect.cancel);
                this.sortflag = false;
            }
            if (this.waittime <= 0)
            {
                if (Input.IsPush(Button.Up))
                {
                    this.sound.PlaySE(SoundEffect.movecursol);
                    --this.SortSelect;
                    this.upper = false;
                    this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                }
                if (!Input.IsPush(Button.Down))
                    return;
                this.sound.PlaySE(SoundEffect.movecursol);
                ++this.SortSelect;
                this.upper = false;
                this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
            }
            else
                --this.waittime;
        }

        private void Control()
        {
            if (Input.IsPress(Button._A))
                this.AButton();
            if (Input.IsPress(Button._B))
            {
                this.sound.PlaySE(SoundEffect.cancel);
                if (this.change)
                {
                    this.nowscene = AddOnManager.SCENE.eroor;
                    this.eventmanager.events.Clear();
                    this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
                    var question = ShanghaiEXE.Translate("AddOnManager.ExitQuestion");
                    var options = ShanghaiEXE.Translate("AddOnManager.ExitOptions");
                    this.eventmanager.AddEvent(new Question(this.sound, this.eventmanager, question[0], question[1], options[0], options[1], true, true, question.Face, this.savedata, true));
                    this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
                }
                else
                    this.nowscene = AddOnManager.SCENE.fadeout;
            }
            if (this.waittime <= 0)
            {
                int num1 = this.savedata.haveAddon.Count - 5;
                if (Input.IsPush(Button.Up))
                {
                    if (this.select > 0)
                    {
                        --this.select;
                        if (this.cursol > 0)
                            --this.cursol;
                        else
                            --this.top;
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                    }
                }
                else if (Input.IsPush(Button.Down))
                {
                    if (this.select < this.savedata.haveAddon.Count - 1)
                    {
                        ++this.select;
                        if (this.cursol < 5)
                            ++this.cursol;
                        else
                            ++this.top;
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                    }
                }
                else if (Input.IsPush(Button._R))
                {
                    int num2 = this.select + 5 < this.savedata.haveAddon.Count ? 5 : this.savedata.haveAddon.Count - this.select - 1;
                    if (num2 > 5)
                        num2 = 5;
                    if (num2 > 0)
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.top += num2;
                        this.select += num2;
                    }
                    this.waittime = Input.IsPress(Button._R) ? 10 : 4;
                }
                else if (Input.IsPush(Button._L))
                {
                    int num2 = this.top;
                    if (num2 > num1)
                        num2 = num1;
                    if (num2 > 5)
                        num2 = 5;
                    if (num2 > 0)
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.top -= num2;
                        this.select -= num2;
                    }
                    this.waittime = Input.IsPress(Button._L) ? 10 : 4;
                }
            }
            else
                --this.waittime;
            if (Input.IsPush(Button._Select))
            {
                this.sound.PlaySE(SoundEffect.decide);
                this.sortflag = true;
                this.upper = false;
            }
            if (!Input.IsPush(Button._Start))
                return;
            this.sound.PlaySE(SoundEffect.decide);
            this.change = false;
            this.RunSetting();
        }

        private void AButton()
        {
            if (this.setAddOn[this.select])
            {
                if (this.Selectaddon.Plus)
                {
                    if (this.UseCore > this.haveCore - this.Selectaddon.UseCore || this.UseHz > this.haveHz - this.Selectaddon.UseHz)
                    {
                        this.sound.PlaySE(SoundEffect.error);
                    }
                    else
                    {
                        this.sound.PlaySE(SoundEffect.decide);
                        this.haveCore -= this.Selectaddon.UseCore;
                        this.haveHz -= this.Selectaddon.UseHz;
                        this.nowCore -= this.Selectaddon.UseCore;
                        this.nowHz -= this.Selectaddon.UseHz;
                        this.setAddOn[this.select] = false;
                        this.setcolor[(int)this.Selectaddon.color] = false;
                    }
                }
                else
                {
                    this.sound.PlaySE(SoundEffect.decide);
                    this.change = true;
                    this.nowCore += this.Selectaddon.UseCore;
                    this.nowHz += this.Selectaddon.UseHz;
                    this.AddonColorRemove(this.select);
                    this.setAddOn[this.select] = false;
                    this.setcolor[(int)this.Selectaddon.color] = false;
                }
            }
            else if (this.Selectaddon.Plus)
            {
                if (!this.setcolor[(int)this.Selectaddon.color])
                {
                    this.sound.PlaySE(SoundEffect.decide);
                    this.change = true;
                    this.haveCore += this.Selectaddon.UseCore;
                    this.haveHz += this.Selectaddon.UseHz;
                    this.nowCore += this.Selectaddon.UseCore;
                    this.nowHz += this.Selectaddon.UseHz;
                    this.setAddOn[this.select] = true;
                    if ((uint)this.Selectaddon.color > 0U)
                        this.setcolor[(int)this.Selectaddon.color] = true;
                }
                else
                    this.sound.PlaySE(SoundEffect.error);
            }
            else if (this.Selectaddon.UseCore > this.nowCore || this.Selectaddon.UseHz > this.nowHz || this.setcolor[(int)this.Selectaddon.color])
            {
                this.sound.PlaySE(SoundEffect.error);
            }
            else
            {
                this.sound.PlaySE(SoundEffect.decide);
                this.nowCore -= this.Selectaddon.UseCore;
                this.nowHz -= this.Selectaddon.UseHz;
                this.AddonColorSet(this.select);
                if ((uint)this.Selectaddon.color > 0U)
                    this.setcolor[(int)this.Selectaddon.color] = true;
                this.setAddOn[this.select] = true;
            }
        }

        private void AddonColorSet(int select)
        {
            this.setAddOn[select] = true;
            int useCore = this.savedata.haveAddon[select].UseCore;
            if (useCore == 0 || this.savedata.haveAddon[select].Plus)
                return;
            for (int index = 0; index < useCore; ++index)
                this.colors.Add(new AddOnColor(select, this.savedata.haveAddon[select].color));
        }

        private void AddonColorRemove(int select)
        {
            this.setAddOn[select] = true;
            if (this.savedata.haveAddon[select].UseCore == 0 || this.savedata.haveAddon[select].Plus)
                return;
            for (int index = this.colors.Count - 1; index >= 0; --index)
            {
                if (this.colors[index].number == select)
                    this.colors.RemoveAt(index);
            }
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(480, 304, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            this.AddonRender(dg);
            this._rect = new Rectangle(480, 464, 240, 16);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            this._rect = new Rectangle(360, 232, 72, 48);
            this._position = new Vector2(160f, 20f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            this._rect = new Rectangle(368, 560, 96, 56);
            this._position = new Vector2(152f, 72f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            if (this.nowscene != AddOnManager.SCENE.runnning)
            {
                foreach (var data in ((IEnumerable<string>)this.savedata.haveAddon[this.select].infomasion).Select((v, i) => new
                {
                    v,
                    i
                }))
                {
                    string v = data.v;
                    Vector2 point = new Vector2(154f, 76 + 16 * data.i);
                    dg.DrawMiniText(v, point, Color.Black);
                }
            }
            else
            {
                this._position = new Vector2(154f, 76f);
                dg.DrawMiniText("ＲＵＮ...", this._position, Color.Black);
                this._position = new Vector2(154f, 92f);
                dg.DrawMiniText(this.shortmassage, this._position, Color.Black);
            }
            this._rect = new Rectangle(112 + 16 * this.cursolanime, 160, 16, 16);
            this._position = new Vector2(0.0f, 24 + this.cursol * 16);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            this.UnderRend(dg);
            if (this.nowscene == AddOnManager.SCENE.runnning)
            {
                this._rect = new Rectangle(480, 552, this.barprint, 32);
                this._position = new Vector2(40f, 128f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.FromArgb(this.barAlpha, Color.White));
            }
            if (this.sortflag)
            {
                this._rect = new Rectangle(208, 128, 64, 88);
                this._position = new Vector2(168f, 24f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                this._rect = new Rectangle(208, 248, 64, 16);
                this._position = new Vector2(168f, 112f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                string[] strArray = new string[5]
                {
                    ShanghaiEXE.Translate("AddOnManager.Cores"),
                    ShanghaiEXE.Translate("AddOnManager.Hertz"),
                    ShanghaiEXE.Translate("AddOnManager.ABC"),
                    ShanghaiEXE.Translate("AddOnManager.Color"),
                    ShanghaiEXE.Translate("AddOnManager.Equip")
                };
                for (int index = 0; index < strArray.Length; ++index)
                {
                    this._position = new Vector2(177f, 40 + 16 * index + 1);
                    dg.DrawMiniText(strArray[index], this._position, Color.FromArgb(byte.MaxValue, 32, 32, 32));
                    this._position = new Vector2(176f, 40 + 16 * index);
                    dg.DrawMiniText(strArray[index], this._position, Color.White);
                }
                this._rect = new Rectangle(112 + 16 * this.cursolanime, 160, 16, 16);
                this._position = new Vector2(160f, 40 + (int)this.SortSelect * 16);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            if (this.nowscene != AddOnManager.SCENE.fadein && this.nowscene != AddOnManager.SCENE.fadeout)
                return;
            Color color = Color.FromArgb(this.Alpha, 0, 0, 0);
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color);
        }

        private void RunSetting()
        {
            this.savedata.AddonReset();
            this.savedata.equipAddon = ((IEnumerable<bool>)this.setAddOn).ToList<bool>();
            this.savedata.AddonSkillReset();
            foreach (var data in this.savedata.haveAddon.Select((v, i) => new
            {
                v,
                i
            }))
            {
                if (this.setAddOn[data.i])
                {
                    this.savedata.addonNames.Add(data.v.name);
                    data.v.Running(this.savedata);
                }
            }
            if (this.savedata.HPNow > this.savedata.HPMax)
            {
                this.savedata.HPNow = this.savedata.HPMax;
            }

            this.savedata.AddonSet();
            this.massage = !this.OneSet() ? "NONE...           " : this.savedata.addonNames[0] + "...           ";
            this.printedItems = 0;
            this.printfonts = 0;
            this.printedItems = 0;
            this.barprint = 0;
            this.shortmassage = "";
            this.nowscene = AddOnManager.SCENE.runnning;
            this.sound.PlaySE(SoundEffect.charge);
        }

        private void AddonRender(IRenderer dg)
        {
            for (int index = -1; index < 7 && this.top + index < this.savedata.haveAddon.Count; ++index)
            {
                if (this.top + index >= 0)
                    this.savedata.haveAddon[this.top + index].Render(dg, this.setAddOn[this.top + index], this.flash && this.top + index == this.select, new Vector2(8f, 24 + index * 16), this);
            }
        }

        private void UnderRend(IRenderer dg)
        {
            this._rect = new Rectangle(480, 480, 240, 32);
            this._position = new Vector2(0.0f, 128f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            for (int index = 0; index < 40; ++index)
            {
                if (index < this.savedata.MaxHz)
                {
                    this._rect = new Rectangle(480, 528, 8, 8);
                    this._position = new Vector2(48 + 8 * (index % 20), 128 + 8 * (index / 20));
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                }
                else if (index >= this.savedata.MaxHz && index < this.haveHz)
                {
                    bool flag = this.setAddOn[this.select] ? this.Selectaddon.Plus && index >= this.haveHz - this.Selectaddon.UseHz && index < this.haveHz && this.nowscene != AddOnManager.SCENE.runnning : this.Selectaddon.Plus && index >= this.haveHz && index < this.haveHz + this.Selectaddon.UseHz;
                    this._rect = new Rectangle(488, 528, 8, 8);
                    this._position = new Vector2(48 + 8 * (index % 20), 128 + 8 * (index / 20));
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, flag ? Color.FromArgb(this.useAlpha, Color.White) : Color.White);
                }
                if (index < this.UseHz)
                {
                    this._rect = new Rectangle(496, 528, 8, 8);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                }
                if (this.nowscene != AddOnManager.SCENE.runnning)
                {
                    if ((this.setAddOn[this.select] ? index >= this.UseHz - this.Selectaddon.UseHz && index < this.UseHz : index >= this.UseHz && index < this.UseHz + this.Selectaddon.UseHz) && !this.Selectaddon.Plus)
                    {
                        this._rect = new Rectangle(504, 528, 8, 8);
                        this._position = new Vector2(48 + 8 * (index % 20), 128 + 8 * (index / 20));
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.FromArgb(this.useAlpha, Color.White));
                    }
                    if (this.Selectaddon.Plus && !this.setAddOn[this.select] && index >= this.savedata.MaxHz && index < this.savedata.MaxHz + this.Selectaddon.UseHz)
                    {
                        this._rect = new Rectangle(488, 528, 8, 8);
                        this._position = new Vector2(48 + 8 * (index % 20), 128 + 8 * (index / 20));
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.FromArgb(this.useAlpha, Color.White));
                    }
                }
            }
            for (int index = 0; index < 10; ++index)
            {
                if (index < this.savedata.MaxCore)
                {
                    this._rect = new Rectangle(480, 512, 16, 16);
                    this._position = new Vector2(48 + 16 * index, 144f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                }
                else if (index >= this.savedata.MaxCore && index < this.haveCore)
                {
                    bool flag = this.setAddOn[this.select] ? this.Selectaddon.Plus && index >= this.haveCore - this.Selectaddon.UseCore && index < this.haveCore && this.nowscene != AddOnManager.SCENE.runnning : this.Selectaddon.Plus && index >= this.haveCore && index < this.haveCore + this.Selectaddon.UseCore;
                    this._rect = new Rectangle(496, 512, 16, 16);
                    this._position = new Vector2(48 + 16 * index, 144f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, flag ? Color.FromArgb(this.useAlpha, Color.White) : Color.White);
                }
                if (this.Selectaddon.Plus && !this.setAddOn[this.select] && (index >= this.savedata.MaxCore && index < this.savedata.MaxCore + this.Selectaddon.UseCore) && this.nowscene != AddOnManager.SCENE.runnning)
                {
                    this._rect = new Rectangle(496, 512, 16, 16);
                    this._position = new Vector2(48 + 16 * index, 144f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.FromArgb(this.useAlpha, Color.White));
                }
            }
            foreach (var data in this.colors.Select((v, i) => new
            {
                v,
                i
            }))
            {
                this._rect = new Rectangle(512 + 16 * (int)data.v.color, 512, 16, 16);
                this._position = new Vector2(48 + 16 * data.i, 144f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            if (!this.setAddOn[this.select] && this.Selectaddon.UseCore > 0 && this.nowscene != AddOnManager.SCENE.runnning)
            {
                for (int index = 0; index < this.Selectaddon.UseCore; ++index)
                {
                    this._rect = new Rectangle(512 + 16 * (int)this.Selectaddon.color, 512, 16, 16);
                    this._position = new Vector2(48 + 16 * this.colors.Count + 16 * index, 144f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.FromArgb(this.useAlpha, Color.White));
                }
            }
            int[] numArray1 = this.ChangeCount(this.nowHz);
            Color color1 = this.nowHz > 0 ? Color.White : Color.FromArgb(byte.MaxValue, byte.MaxValue, 150, 50);
            Vector2 vector2 = new Vector2(228f, 128f);
            for (int index = 0; index < numArray1.Length; ++index)
            {
                this._rect = new Rectangle(numArray1[index] * 8, 104, 8, 16);
                this._position = new Vector2(vector2.X - index * 8, vector2.Y);
                dg.DrawImage(dg, "font", this._rect, true, this._position, color1);
            }
            int[] numArray2 = this.ChangeCount(this.nowCore);
            Color color2 = this.nowCore > 0 ? Color.White : Color.FromArgb(byte.MaxValue, byte.MaxValue, 150, 50);
            vector2 = new Vector2(228f, 144f);
            for (int index = 0; index < numArray2.Length; ++index)
            {
                this._rect = new Rectangle(numArray2[index] * 8, 104, 8, 16);
                this._position = new Vector2(vector2.X - index * 8, vector2.Y);
                dg.DrawImage(dg, "font", this._rect, true, this._position, color2);
            }
            if (!this.eventmanager.playevent)
                return;
            this.eventmanager.Render(dg);
        }

        private void MoveAlpha()
        {
            this.useAlpha += this.moveAlpha;
            if (this.useAlpha < 0)
            {
                this.useAlpha = 0;
                this.moveAlpha = 51;
            }
            if (this.useAlpha <= byte.MaxValue)
                return;
            this.useAlpha = byte.MaxValue;
            this.moveAlpha = -51;
        }

        private void Sort(AddOnManager.SORT sort_)
        {
            SortAddon[] sortAddonArray = new SortAddon[this.savedata.haveAddon.Count];
            foreach (var data in this.savedata.haveAddon.Select((v, j) => new
            {
                v,
                j
            }))
                sortAddonArray[data.j] = new SortAddon(data.v, data.j, this.savedata.equipAddon[data.j], this.setAddOn[data.j]);
            switch (sort_)
            {
                case AddOnManager.SORT.core:
                    sortAddonArray = this.upper ? ((IEnumerable<SortAddon>)sortAddonArray).OrderByDescending<SortAddon, int>(n => n.addon.UseCore).ToArray<SortAddon>() : ((IEnumerable<SortAddon>)sortAddonArray).OrderBy<SortAddon, int>(n => n.addon.UseCore).ToArray<SortAddon>();
                    break;
                case AddOnManager.SORT.Hz:
                    sortAddonArray = this.upper ? ((IEnumerable<SortAddon>)sortAddonArray).OrderByDescending<SortAddon, int>(n => n.addon.UseHz).ToArray<SortAddon>() : ((IEnumerable<SortAddon>)sortAddonArray).OrderBy<SortAddon, int>(n => n.addon.UseHz).ToArray<SortAddon>();
                    break;
                case AddOnManager.SORT.aiueo:
                    sortAddonArray = this.upper ? ((IEnumerable<SortAddon>)sortAddonArray).OrderByDescending<SortAddon, string>(n => n.addon.name).ToArray<SortAddon>() : ((IEnumerable<SortAddon>)sortAddonArray).OrderBy<SortAddon, string>(n => n.addon.name).ToArray<SortAddon>();
                    break;
                case AddOnManager.SORT.color:
                    sortAddonArray = this.upper ? ((IEnumerable<SortAddon>)sortAddonArray).OrderByDescending<SortAddon, AddOnBase.ProgramColor>(n => n.addon.color).ToArray<SortAddon>() : ((IEnumerable<SortAddon>)sortAddonArray).OrderBy<SortAddon, AddOnBase.ProgramColor>(n => n.addon.color).ToArray<SortAddon>();
                    break;
                case AddOnManager.SORT.set:
                    sortAddonArray = this.upper ? ((IEnumerable<SortAddon>)sortAddonArray).OrderByDescending<SortAddon, bool>(n => n.nowset).ToArray<SortAddon>() : ((IEnumerable<SortAddon>)sortAddonArray).OrderBy<SortAddon, bool>(n => n.nowset).ToArray<SortAddon>();
                    break;
            }
            bool[] flagArray = new bool[this.colors.Count];
            foreach (var data1 in ((IEnumerable<SortAddon>)sortAddonArray).Select((v, i) => new
            {
                v,
                i
            }))
            {
                this.savedata.haveAddon[data1.i] = data1.v.addon;
                this.savedata.equipAddon[data1.i] = data1.v.set;
                this.setAddOn[data1.i] = data1.v.nowset;
                foreach (var data2 in ((IEnumerable<bool>)flagArray).Select((v, i) => new
                {
                    v,
                    i
                }))
                {
                    if (!flagArray[data2.i] && data1.v.number == this.colors[data2.i].number)
                    {
                        this.colors[data2.i].number = data1.i;
                        flagArray[data2.i] = true;
                    }
                }
            }
        }

        protected string[] ToDecomposition(string text)
        {
            char[] charArray = text.ToCharArray();
            string[] strArray = new string[charArray.Length];
            for (int index = 0; index < charArray.Length; ++index)
                strArray[index] = charArray[index].ToString();
            return strArray;
        }

        private bool OneSet()
        {
            foreach (bool flag in this.setAddOn)
            {
                if (flag)
                    return true;
            }
            return false;
        }

        private void Tutorial()
        {
            if (this.savedata.FlagList[120] || !this.savedata.FlagList[79])
                return;
            this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
            var dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage1Dialogue1");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage1Dialogue2");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage1Dialogue3");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage1Dialogue4");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage1Dialogue5");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage1Dialogue6");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
            this.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.eventmanager, 3, new Vector2(40f, 144f)));
            this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage2Dialogue1");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage2Dialogue2");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage2Dialogue3");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage2Dialogue4");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
            this.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.eventmanager, 2, new Vector2(112f, 32f)));
            this.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.eventmanager, 2, new Vector2(136f, 32f)));
            this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage3Dialogue1");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage3Dialogue2");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage3Dialogue3");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage3Dialogue4");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage3Dialogue5");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage3Dialogue6");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage3Dialogue7");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage3Dialogue8");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage3Dialogue9");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
            this.eventmanager.AddEvent(new CommandPrintArrow(this.sound, this.eventmanager, 3, new Vector2(56f, 152f)));
            this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage4Dialogue1");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage4Dialogue2");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage4Dialogue3");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage4Dialogue4");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage4Dialogue5");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage4Dialogue6");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage4Dialogue7");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage4Dialogue8");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage4Dialogue9");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage4Dialogue10");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            dialogue = ShanghaiEXE.Translate("AddOnManager.TutorialStage4Dialogue11");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
            this.eventmanager.AddEvent(new editFlag(this.sound, this.eventmanager, 120, true, this.savedata));
        }

        private enum SCENE
        {
            fadein,
            select,
            runnning,
            eroor,
            fadeout,
        }

        private enum SORT
        {
            core,
            Hz,
            aiueo,
            color,
            set,
        }
    }
}
