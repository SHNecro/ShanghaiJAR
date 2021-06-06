using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSMap.Character.Menu
{
    internal class FolderSelect : MenuBase
    {
        private readonly Color illegalColor = Color.SkyBlue;
        private readonly bool[] canEq = new bool[3];
        private bool nextmenu;
        private bool gonext;
        private bool goOrEq;
        private bool cursolanime;
        private const byte overtop = 22;
        private int topchip;
        private byte cursolanime2;
        private FolderSelect.SCENE nowscene;
        private readonly FolderEdit subMenu;
        private int select;
        private readonly SceneMain main;

        private int Topchip
        {
            get
            {
                return this.topchip;
            }
            set
            {
                this.topchip = value;
                if (this.topchip >= 22)
                    this.topchip = 22;
                if (this.topchip >= 0)
                    return;
                this.topchip = 0;
            }
        }

        public int Select
        {
            get
            {
                return this.select;
            }
        }

        public FolderSelect(IAudioEngine s, Player p, SceneMain m, TopMenu t, SaveData save)
          : base(s, p, t, save)
        {
            this.main = m;
            this.main.FolderReset();
            this.nowscene = FolderSelect.SCENE.fadein;
            this.subMenu = new FolderEdit(this.sound, p, m, t, this, save);
            this.Alpha = byte.MaxValue;
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
        }

        public override void UpDate()
        {
            if (!this.savedata.havefolder[1] && !this.savedata.havefolder[2] || this.nextmenu)
            {
                this.subMenu.UpDate();
            }
            else
            {
                this.Control();
                switch (this.nowscene)
                {
                    case FolderSelect.SCENE.fadein:
                        if (this.Alpha > 0)
                        {
                            this.Alpha -= 51;
                            break;
                        }
                        this.nowscene = FolderSelect.SCENE.select;
                        break;
                    case FolderSelect.SCENE.select:
                        if (this.frame % 10 == 0)
                        {
                            this.cursolanime = !this.cursolanime;
                            break;
                        }
                        break;
                    case FolderSelect.SCENE.goOrEq:
                        if (this.frame % 10 == 0)
                            ++this.cursolanime2;
                        if (this.cursolanime2 >= 3)
                        {
                            this.cursolanime2 = 0;
                            break;
                        }
                        break;
                    case FolderSelect.SCENE.fadeout:
                        if (this.Alpha < byte.MaxValue)
                        {
                            this.Alpha += 51;
                            break;
                        }
                        if (this.gonext)
                        {
                            this.subMenu.Init();
                            this.nextmenu = true;
                        }
                        else
                            this.topmenu.Return();
                        break;
                }
            }
            this.FlamePlus();
        }

        private void Control()
        {
            switch (this.nowscene)
            {
                case FolderSelect.SCENE.select:
                    if (Input.IsPress(Button.Up))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        --this.select;
                        if (!this.savedata.havefolder[1] && this.select == 1)
                            --this.select;
                        if (this.select < 0)
                        {
                            if (this.savedata.havefolder[2])
                                this.select = 2;
                            else if (this.savedata.havefolder[1])
                                this.select = 1;
                        }
                        this.Topchip = 0;
                    }
                    if (Input.IsPress(Button.Down))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        ++this.select;
                        if (!this.savedata.havefolder[1] && this.select == 1)
                            ++this.select;
                        if (this.select > 2)
                            this.select = 0;
                        this.Topchip = 0;
                    }
                    if (Input.IsPress(Button._A))
                    {
                        this.sound.PlaySE(SoundEffect.decide);
                        this.goOrEq = this.select == 2;
                        this.nowscene = FolderSelect.SCENE.goOrEq;
                    }
                    if (Input.IsPress(Button._B))
                    {
                        this.sound.PlaySE(SoundEffect.cancel);
                        this.gonext = false;
                        this.nowscene = FolderSelect.SCENE.fadeout;
                    }
                    if (Input.IsPress(Button._Select) && this.savedata.efolder != this.select)
                    {
                        if (!this.savedata.FlagList[8] && this.canEq[this.select])
                        {
                            this.sound.PlaySE(SoundEffect.bright);
                            this.savedata.efolder = this.select;
                        }
                        else
                            this.sound.PlaySE(SoundEffect.error);
                    }
                    if (Input.IsPress(Button._R))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.Topchip += 8;
                    }
                    if (!Input.IsPress(Button._L))
                        break;
                    this.sound.PlaySE(SoundEffect.movecursol);
                    this.Topchip -= 8;
                    break;
                case FolderSelect.SCENE.goOrEq:
                    if (Input.IsPress(Button.Down) || Input.IsPress(Button.Up))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        if (this.select != 2)
                            this.goOrEq = !this.goOrEq;
                    }
                    if (Input.IsPress(Button._A))
                    {
                        if (!this.goOrEq)
                        {
                            this.sound.PlaySE(SoundEffect.decide);
                            this.gonext = true;
                            this.nowscene = FolderSelect.SCENE.fadeout;
                        }
                        else if (!this.savedata.FlagList[8] && this.canEq[this.select])
                        {
                            if (this.savedata.efolder != this.select)
                            {
                                this.sound.PlaySE(SoundEffect.bright);
                                this.savedata.efolder = this.select;
                            }
                            else
                                this.sound.PlaySE(SoundEffect.cancel);
                            this.nowscene = FolderSelect.SCENE.select;
                            this.goOrEq = false;
                        }
                        else
                            this.sound.PlaySE(SoundEffect.error);
                    }
                    if (!Input.IsPress(Button._B))
                        break;
                    this.sound.PlaySE(SoundEffect.cancel);
                    this.nowscene = FolderSelect.SCENE.select;
                    break;
            }
        }

        public override void Render(IRenderer dg)
        {
            if (!this.savedata.havefolder[1] && !this.savedata.havefolder[2] || this.nextmenu)
            {
                this.subMenu.Render(dg);
            }
            else
            {
                this._rect = new Rectangle(0, 464, 240, 160);
                this._position = new Vector2(0.0f, 0.0f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                for (int index = 0; index < 3; ++index)
                {
                    if (this.savedata.havefolder[index])
                    {
                        if (this.canEq[index])
                            this._rect = new Rectangle(760, 392, 80, 32);
                        else
                            this._rect = new Rectangle(760, 424, 80, 32);
                        this._position = new Vector2(8f, 16 + 40 * index);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        AllBase.NAME[] nameArray;
                        switch (index)
                        {
                            case 0:
                                nameArray = this.Nametodata(ShanghaiEXE.Translate("FolderSelect.Folder1"));
                                break;
                            case 1:
                                nameArray = this.Nametodata(ShanghaiEXE.Translate("FolderSelect.Folder2"));
                                break;
                            default:
                                nameArray = this.Nametodata(this.savedata.foldername);
                                break;
                        }
                        this._position = new Vector2(10, 24 + 40 * index);
                        DrawBlockCharacters(dg, nameArray, 88, this._position, Color.White, out this._rect, out this._position);
                    }
                }
                this._rect = new Rectangle(112, this.cursolanime ? 216 : 248, 96, 32);
                this._position = new Vector2(0.0f, 16 + 40 * this.select);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                this._rect = new Rectangle(112, 208, 40, 8);
                this._position = new Vector2(40f, 16 + 40 * this.savedata.efolder);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                int[] numArray1 = this.ChangeCount(savedata.Regularlarge);
                PointF pointF1 = new PointF(76f, 129f);
                for (int index = 0; index < numArray1.Length; ++index)
                {
                    this._rect = new Rectangle(numArray1[index] * 8, 72, 8, 16);
                    this._position = new Vector2(pointF1.X - index * 8, pointF1.Y);
                    dg.DrawImage(dg, "font", this._rect, true, this._position, Color.Yellow);
                }
                float num = 5.454545f * Topchip;
                this._rect = new Rectangle(176, 168, 8, 8);
                this._position = new Vector2(224f, 16f + num);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                for (int index1 = 0; index1 < 8; ++index1)
                {
                    if (!(this.main.chipfolder[this.select, this.Topchip + index1].chip is DammyChip))
                    {
                        this._position = new Vector2(104f, 16 + 16 * index1);
                        this.main.chipfolder[this.select, this.Topchip + index1].chip.IconRender(dg, this._position, false, false, this.main.chipfolder[this.select, this.Topchip + index1].codeNo, false);
                        AllBase.NAME[] nameArray = this.Nametodata(this.main.chipfolder[this.select, this.Topchip + index1].chip.name);
                        Color color = Color.White;
                        if (this.main.chipfolder[this.select, this.Topchip + index1].chip.number >= 350)
                            color = this.illegalColor;
                        this._position = new Vector2(120, 16 + 16 * index1);
                        DrawBlockCharacters(dg, nameArray, 16, this._position, color, out this._rect, out this._position);
                        this._rect = new Rectangle(216 + (int)this.main.chipfolder[this.select, this.Topchip + index1].chip.element * 16, 88, 16, 16);
                        this._position = new Vector2(184f, 16 + index1 * 16);
                        dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                        this._rect = new Rectangle((int)this.main.chipfolder[this.select, this.Topchip + index1].chip.code[this.main.chipfolder[this.select, this.Topchip + index1].codeNo] * 8, 32, 8, 16);
                        this._position = new Vector2(200f, 16 + index1 * 16);
                        dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                        this._rect = new Rectangle(160, 160, 16, 16);
                        this._position = new Vector2(208f, 16 + index1 * 16);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        int[] numArray2 = this.ChangeCount(this.main.chipfolder[this.select, this.Topchip + index1].chip.regsize);
                        PointF pointF2 = new PointF(216f, 16 + index1 * 16);
                        for (int index2 = 0; index2 < numArray2.Length; ++index2)
                        {
                            this._rect = new Rectangle(numArray2[index2] * 8, 72, 8, 16);
                            this._position = new Vector2(pointF2.X - index2 * 8, pointF2.Y);
                            dg.DrawImage(dg, "font", this._rect, true, this._position, Color.Yellow);
                        }
                    }
                    if (this.savedata.regularflag[this.select] && this.savedata.regularchip[this.select] == this.Topchip + index1)
                    {
                        this._rect = new Rectangle(112, 288, 128, 16);
                        this._position = new Vector2(96f, 16 + index1 * 16);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    }
                }
                if (this.nowscene == FolderSelect.SCENE.goOrEq)
                {
                    this._rect = new Rectangle(192, 88, 104, 40);
                    this._position = new Vector2(0.0f, 120f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    if (this.select != 2)
                    {
                        this._position = new Vector2(16f, 128f);
                        dg.DrawMiniText(ShanghaiEXE.Translate("FolderSelect.EditFolder"), this._position, Color.Black);
                    }
                    this._position = new Vector2(16f, 144f);
                    dg.DrawMiniText(ShanghaiEXE.Translate("FolderSelect.EquipFolder"), this._position, Color.Black);
                    this._rect = new Rectangle(112 + 16 * cursolanime2, 160, 16, 16);
                    this._position = new Vector2(0.0f, !this.goOrEq ? 128f : 144f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                }
                if (this.nowscene == FolderSelect.SCENE.fadein || this.nowscene == FolderSelect.SCENE.fadeout)
                {
                    Color color = Color.FromArgb(this.Alpha, 0, 0, 0);
                    this._rect = new Rectangle(0, 0, 240, 160);
                    this._position = new Vector2(0.0f, 0.0f);
                    dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color);
                }
            }
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

        public void Back()
        {
            this.nextmenu = false;
            this.nowscene = FolderSelect.SCENE.fadein;
            if (this.savedata.havefolder[1] || this.savedata.havefolder[2])
                return;
            this.gonext = false;
            this.nowscene = FolderSelect.SCENE.fadeout;
            this.topmenu.Return();
        }

        private enum SCENE
        {
            fadein,
            select,
            goOrEq,
            fadeout,
        }
    }
}
