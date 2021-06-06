using Common;
using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSMap.Character.Menu
{
    internal class FolderEdit : MenuBase
    {
        private readonly int[] selectY = new int[2];
        private readonly int[] cursol = new int[2];
        private readonly Color illegalColor = Color.SkyBlue;
        private bool error;
        private FolderEdit.ERROR errorMassage;
        private bool selected;
        private FolderEdit.SORT sortselect;
        private bool sortmode;
        private bool sortflag;
        private bool cursolanime;
        private byte cursolanime2;
        private const byte overtop = 23;
        private int topchip;
        private int overtop2;
        private int topchip2;
        private bool leftRight;
        private bool selectedForB;
        private int selectedY;
        private int seetX;
        private FolderEdit.SCENE nowscene;
        private readonly SceneMain main;
        private readonly FolderSelect folder;
        private int manychips;

        private int SelectY_Folder
        {
            get
            {
                return this.Topchip + this.cursol[0];
            }
        }

        private int SelectY_Bag
        {
            get
            {
                return this.Topchip2 + this.cursol[1];
            }
        }

        private FolderEdit.SORT SortSelect
        {
            get
            {
                return this.sortselect;
            }
            set
            {
                this.sortselect = value;
                if (this.sortselect < FolderEdit.SORT.id)
                    this.sortselect = FolderEdit.SORT.size;
                if (this.sortselect <= FolderEdit.SORT.size)
                    return;
                this.sortselect = FolderEdit.SORT.id;
            }
        }

        private int Topchip
        {
            get
            {
                return this.topchip;
            }
            set
            {
                this.topchip = value;
                if (this.topchip >= 23)
                    this.topchip = 23;
                if (this.topchip >= 0)
                    return;
                this.topchip = 0;
            }
        }

        private int Topchip2
        {
            get
            {
                return this.topchip2;
            }
            set
            {
                this.topchip2 = value;
                this.overtop2 = this.savedata.havechips.Count < 7 ? 0 : this.savedata.havechips.Count - 7;
                if (this.topchip2 >= this.overtop2)
                {
                    this.topchip2 = this.overtop2;
                    this.cursol[1] = 6;
                }
                if (this.topchip2 >= 0)
                    return;
                this.topchip2 = 0;
            }
        }

        private int Selectfolder
        {
            get
            {
                return this.folder.Select;
            }
        }

        public FolderEdit(
          IAudioEngine s,
          Player p,
          SceneMain m,
          TopMenu t,
          FolderSelect f,
          SaveData save)
          : base(s, p, t, save)
        {
            this.overtop2 = this.savedata.havechips.Count < 7 ? 0 : this.savedata.havechips.Count - 7;
            this.main = m;
            this.folder = f;
            this.nowscene = FolderEdit.SCENE.fadein;
            this.Alpha = byte.MaxValue;
            this.ManyChip();
            this.Init();
        }

        public void ManyChip()
        {
            this.manychips = 0;
            for (int index = 0; index < 30; ++index)
            {
                if (this.main.chipfolder[this.Selectfolder, index].inchip && !(this.main.chipfolder[this.Selectfolder, index].chip is DammyChip))
                    ++this.manychips;
            }
        }

        public void Init()
        {
            this.nowscene = FolderEdit.SCENE.fadein;
            this.Alpha = byte.MaxValue;
            this.ManyChip();
            this.seetX = 0;
            this.Topchip = 0;
            this.Topchip2 = 0;
            this.cursol[0] = 0;
            this.cursol[1] = 0;
            this.ManyChip();
        }

        public override void UpDate()
        {
            if (this.error)
            {
                if (this.errorMassage != FolderEdit.ERROR.allOut)
                {
                    if (Input.IsPress(Button._A) || Input.IsPress(Button._B))
                        this.error = false;
                }
                else if (Input.IsPress(Button._A))
                {
                    this.sound.PlaySE(SoundEffect.decide);
                    for (int index = 0; index < 30; ++index)
                    {
                        if (!(this.main.chipfolder[this.Selectfolder, index].chip is DammyChip))
                        {
                            this.savedata.AddChip(this.main.chipfolder[this.Selectfolder, index].chip.number, this.main.chipfolder[this.Selectfolder, index].codeNo, false);
                            if (Debug.RegularMove && (this.savedata.regularflag[this.Selectfolder] && this.savedata.regularchip[this.Selectfolder] == index))
                                this.savedata.regularflag[this.Selectfolder] = false;
                            this.main.chipfolder[this.Selectfolder, index] = new ChipFolder(this.sound);
                            --this.manychips;
                        }
                    }
                    this.error = false;
                }
                else if (Input.IsPress(Button._B))
                {
                    this.sound.PlaySE(SoundEffect.cancel);
                    this.error = false;
                }
            }
            else if (this.sortflag)
            {
                if (Input.IsPress(Button._A))
                {
                    this.sound.PlaySE(SoundEffect.decide);
                    this.Sort();
                    this.sortmode = !this.sortmode;
                }
                else if (Input.IsPress(Button._B))
                {
                    this.sound.PlaySE(SoundEffect.cancel);
                    this.sortflag = false;
                }
                else if (Input.IsPush(Button._Start) && Input.IsPush(Button._Select))
                {
                    this.error = true;
                    this.errorMassage = FolderEdit.ERROR.allOut;
                    this.sound.PlaySE(SoundEffect.decide);
                }
                else if (this.waittime <= 0)
                {
                    if (Input.IsPush(Button.Up))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        --this.SortSelect;
                        this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                        this.sortmode = false;
                    }
                    else if (Input.IsPush(Button.Down))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        ++this.SortSelect;
                        this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                        this.sortmode = false;
                    }
                }
                else
                    --this.waittime;
            }
            else
            {
                switch (this.nowscene)
                {
                    case FolderEdit.SCENE.fadein:
                        if (this.Alpha > 0)
                        {
                            this.Alpha -= 51;
                            break;
                        }
                        this.nowscene = FolderEdit.SCENE.select;
                        break;
                    case FolderEdit.SCENE.select:
                        this.Control();
                        break;
                    case FolderEdit.SCENE.goside:
                        if (this.leftRight)
                        {
                            if (this.seetX > -240)
                            {
                                this.seetX -= 20;
                                break;
                            }
                            this.nowscene = FolderEdit.SCENE.bag;
                            break;
                        }
                        if (this.seetX < 0)
                            this.seetX += 20;
                        else
                            this.nowscene = FolderEdit.SCENE.select;
                        break;
                    case FolderEdit.SCENE.bag:
                        this.Control();
                        break;
                    case FolderEdit.SCENE.fadeout:
                        if (this.Alpha < byte.MaxValue)
                        {
                            this.Alpha += 51;
                            break;
                        }
                        this.folder.Back();
                        break;
                }
            }
            if (this.frame % 3 == 0)
                this.cursolanime = !this.cursolanime;
            if (this.frame % 10 == 0)
                ++this.cursolanime2;
            if (this.cursolanime2 >= 3)
                this.cursolanime2 = 0;
            if (this.savedata.havechips.Count > 7 && this.savedata.havechips.Count < this.Topchip2 + 7)
                --this.topchip2;
            this.FlamePlus();
        }

        private void Control()
        {
            if (Input.IsPress(Button._A))
                this.Decide();
            else if (Input.IsPress(Button._B))
            {
                if (!this.selected)
                {
                    if (this.manychips == 30 || this.savedata.havefolder[2])
                    {
                        this.sound.PlaySE(SoundEffect.cancel);
                        this.nowscene = FolderEdit.SCENE.fadeout;
                        this.folder.FolderCheckAll();
                    }
                    else
                    {
                        this.error = true;
                        this.errorMassage = FolderEdit.ERROR.nochip;
                        this.sound.PlaySE(SoundEffect.error);
                    }
                }
                else
                {
                    this.sound.PlaySE(SoundEffect.cancel);
                    this.selected = false;
                }
            }
            else if (Input.IsPush(Button._Start) && Input.IsPush(Button._Select))
            {
                this.error = true;
                this.errorMassage = FolderEdit.ERROR.allOut;
                this.sound.PlaySE(SoundEffect.decide);
            }
            else if (Input.IsPress(Button._Start))
            {
                this.sound.PlaySE(SoundEffect.decide);
                this.sortflag = true;
                this.sortmode = false;
                this.sortselect = FolderEdit.SORT.id;
            }
            else if (Input.IsPress(Button._Select) && this.nowscene == FolderEdit.SCENE.select)
            {
                ChipBase chip = this.main.chipfolder[this.Selectfolder, this.SelectY_Folder].chip;
                if (chip is DammyChip)
                {
                }
                else if (this.savedata.regularflag[this.Selectfolder] && this.savedata.regularchip[this.Selectfolder] == this.SelectY_Folder)
                {
                    this.sound.PlaySE(SoundEffect.decide);
                    this.savedata.regularflag[this.Selectfolder] = false;
                }
                else if (chip.regsize <= savedata.Regularlarge)
                {
                    this.sound.PlaySE(SoundEffect.decide);
                    this.savedata.regularflag[this.Selectfolder] = true;
                    this.savedata.regularchip[this.Selectfolder] = (byte)this.SelectY_Folder;
                }
                else
                {
                    this.error = true;
                    this.errorMassage = FolderEdit.ERROR.overbyte;
                    this.sound.PlaySE(SoundEffect.error);
                }
            }
            else if (this.waittime <= 0)
            {
                if (Input.IsPush(Button.Up))
                {
                    if (this.nowscene == FolderEdit.SCENE.bag)
                    {
                        if (this.SelectY_Bag > 0)
                        {
                            if (this.cursol[1] > 0)
                                --this.cursol[1];
                            else
                                --this.Topchip2;
                            this.sound.PlaySE(SoundEffect.movecursol);
                        }
                    }
                    else if (this.SelectY_Folder > 0)
                    {
                        if (this.cursol[0] > 0)
                            --this.cursol[0];
                        else
                            --this.Topchip;
                        this.sound.PlaySE(SoundEffect.movecursol);
                    }
                    this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                }
                else if (Input.IsPush(Button.Down))
                {
                    if (this.nowscene == FolderEdit.SCENE.bag)
                    {
                        if (this.SelectY_Bag < this.savedata.havechips.Count - 1)
                        {
                            if (this.cursol[1] < 6)
                                ++this.cursol[1];
                            else
                                ++this.Topchip2;
                            this.sound.PlaySE(SoundEffect.movecursol);
                        }
                    }
                    else if (this.SelectY_Folder < 29)
                    {
                        if (this.cursol[0] < 6)
                            ++this.cursol[0];
                        else
                            ++this.Topchip;
                        this.sound.PlaySE(SoundEffect.movecursol);
                    }
                    this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                }
                else if (Input.IsPush(Button._R))
                {
                    if (this.nowscene == FolderEdit.SCENE.bag)
                    {
                        this.overtop2 = this.savedata.havechips.Count < 7 ? 0 : this.savedata.havechips.Count - 7;
                        int num = this.overtop2 - this.Topchip2;
                        if (num > 7)
                            num = 7;
                        if (num > 0)
                        {
                            this.sound.PlaySE(SoundEffect.movecursol);
                            this.Topchip2 += num;
                        }
                    }
                    else
                    {
                        int num = 23 - this.Topchip;
                        if (num > 7)
                            num = 7;
                        if (num > 0)
                        {
                            this.sound.PlaySE(SoundEffect.movecursol);
                            this.Topchip += num;
                        }
                    }
                    this.waittime = Input.IsPress(Button._R) ? 10 : 4;
                }
                else if (Input.IsPush(Button._L))
                {
                    if (this.nowscene == FolderEdit.SCENE.bag)
                    {
                        this.overtop2 = this.savedata.havechips.Count < 7 ? 0 : this.savedata.havechips.Count - 7;
                        int num = this.Topchip2;
                        if (num > 7)
                            num = 7;
                        if (num > 0)
                        {
                            this.sound.PlaySE(SoundEffect.movecursol);
                            this.Topchip2 -= num;
                        }
                    }
                    else
                    {
                        int num = this.Topchip;
                        if (num > 7)
                            num = 7;
                        if (num > 0)
                        {
                            this.sound.PlaySE(SoundEffect.movecursol);
                            this.Topchip -= num;
                        }
                    }
                    this.waittime = Input.IsPress(Button._L) ? 10 : 4;
                }
            }
            else
                --this.waittime;
            bool flag = false;
            for (int index = 0; index < 10; ++index)
            {
                if (Input.IsPush((Button)index))
                    flag = true;
            }
            if (!flag)
                this.waittime = 0;
            if (Input.IsPress(Button.Left))
            {
                if (this.nowscene != FolderEdit.SCENE.bag)
                    return;
                this.leftRight = false;
                this.sound.PlaySE(SoundEffect.menuopen);
                this.nowscene = FolderEdit.SCENE.goside;
            }
            else
            {
                if (!Input.IsPress(Button.Right) || this.nowscene != FolderEdit.SCENE.select)
                    return;
                this.leftRight = true;
                this.sound.PlaySE(SoundEffect.menuopen);
                this.nowscene = FolderEdit.SCENE.goside;
            }
        }

        private int Manychip(ChipFolder chipFolder)
        {
            throw new NotImplementedException();
        }

        public override void Render(IRenderer dg)
        {
            int num1 = 0;
            if (this.nowscene == FolderEdit.SCENE.goside)
                num1 = 240 + this.seetX;
            if (this.nowscene != FolderEdit.SCENE.select)
            {
                this._rect = new Rectangle(240, 304, 240, 160);
                this._position = new Vector2(num1, 0.0f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                if (this.savedata.havechips.Count > 0)
                {
                    for (int index1 = 0; index1 < 7 && this.Topchip2 + index1 < this.savedata.havechips.Count; ++index1)
                    {
                        ChipFolder chipFolder = new ChipFolder(this.sound);
                        chipFolder.SettingChip(this.savedata.havechips[this.Topchip2 + index1].number);
                        chipFolder.codeNo = this.savedata.havechips[this.Topchip2 + index1].code;
                        if (!(chipFolder.chip is DammyChip))
                        {
                            Color color = Color.White;
                            if (chipFolder.chip.number >= 350)
                                color = this.illegalColor;
                            this._position = new Vector2(num1 + 8, 32 + 16 * index1);
                            chipFolder.chip.IconRender(dg, this._position, false, false, chipFolder.codeNo, false);
                            var blockCharacters = this.Nametodata(chipFolder.chip.name);
                            this._position = new Vector2(num1 + 24, 32 + 16 * index1);
                            DrawBlockCharacters(dg, blockCharacters, 16, this._position, color, out this._rect, out this._position);
                            this._rect = new Rectangle(216 + (int)chipFolder.chip.element * 16, 88, 16, 16);
                            this._position = new Vector2(num1 + 88, 32 + index1 * 16);
                            dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                            this._rect = new Rectangle((int)chipFolder.chip.code[chipFolder.codeNo] * 8, 32, 8, 16);
                            this._position = new Vector2(num1 + 104, 32 + index1 * 16);
                            dg.DrawImage(dg, "font", this._rect, true, this._position, color);
                            this._rect = new Rectangle(160, 160, 16, 16);
                            this._position = new Vector2(num1 + 112, 32 + index1 * 16);
                            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                            int[] numArray1 = this.ChangeCount(chipFolder.chip.regsize);
                            PointF pointF = new PointF(num1 + 120, 32 + index1 * 16);
                            for (int index2 = 0; index2 < numArray1.Length; ++index2)
                            {
                                this._rect = new Rectangle(numArray1[index2] * 8, 72, 8, 16);
                                this._position = new Vector2(pointF.X - index2 * 8, pointF.Y);
                                dg.DrawImage(dg, "font", this._rect, true, this._position, Color.Yellow);
                            }
                            int[] numArray2 = this.ChangeCount(this.savedata.Havechip[chipFolder.chip.number, chipFolder.codeNo]);
                            pointF = new PointF(num1 + 136, 32 + index1 * 16);
                            for (int index2 = 0; index2 < numArray2.Length; ++index2)
                            {
                                this._rect = new Rectangle(numArray2[index2] * 8, 0, 8, 16);
                                this._position = new Vector2(pointF.X - index2 * 8, pointF.Y);
                                dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                            }
                        }
                    }
                    if (this.savedata.havechips.Count > this.SelectY_Bag)
                    {
                        ChipS havechip = this.savedata.havechips[this.SelectY_Bag];
                        if (true)
                        {
                            ChipFolder chipFolder = new ChipFolder(this.sound);
                            chipFolder.SettingChip(this.savedata.havechips[this.SelectY_Bag].number);
                            chipFolder.codeNo = this.savedata.havechips[this.SelectY_Bag].code;
                            Vector2 vector2 = new Vector2(num1 + 168, 32f);
                            chipFolder.chip.GraphicsRender(dg, vector2, chipFolder.codeNo, true, true);
                            if (chipFolder.chip.dark)
                            {
                                this._rect = new Rectangle(272, 184, 88, 56);
                                this._position = new Vector2(num1 + 152, 96f);
                                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                            }
                            else if (chipFolder.chip.navi)
                            {
                                this._rect = new Rectangle(272, 128, 88, 56);
                                this._position = new Vector2(num1 + 152, 96f);
                                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                            }
                            foreach (var data in ((IEnumerable<string>)chipFolder.chip.information).Select((v, i) => new
                            {
                                v,
                                i
                            }))
                            {
                                string v = data.v;
                                vector2 = new Vector2(num1 + 154, 102 + 16 * data.i);
                                dg.DrawMiniText(v, vector2, Color.Black);
                            }
                        }
                    }
                }
                if (this.selectedForB && this.cursolanime && (this.selected && this.selectedY >= this.Topchip2) && this.selectedY < this.Topchip2 + 7)
                {
                    this._rect = new Rectangle(152, 144, 16, 16);
                    this._position = new Vector2(num1, 32 + (this.selectedY - this.Topchip2) * 16);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                }
                this._rect = new Rectangle(112 + 16 * cursolanime2, 160, 16, 16);
                this._position = new Vector2(num1, 32 + this.cursol[1] * 16);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                this._rect = new Rectangle(168 + 8 * cursolanime2, 152, 8, 8);
                this._position = new Vector2(num1, 24f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                float num2 = this.overtop2 != 0 && this.Topchip2 != 0 ? 104f / overtop2 * Topchip2 : 0.0f;
                this._rect = new Rectangle(176, 168, 8, 8);
                this._position = new Vector2(num1 + 144, 32f + num2);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                var bagNames = this.Nametodata(ShanghaiEXE.Translate("FolderEdit.BagName"));
                this._position = new Vector2(num1 + 24, 8f);
                DrawBlockCharacters(dg, bagNames, 88, this._position, Color.White, out this._rect, out this._position);
            }
            if (this.nowscene != FolderEdit.SCENE.bag)
            {
                this._rect = new Rectangle(0, 304, 240, 160);
                this._position = new Vector2(seetX, 0.0f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                for (int index1 = 0; index1 < 7; ++index1)
                {
                    if (!(this.main.chipfolder[this.Selectfolder, this.Topchip + index1].chip is DammyChip))
                    {
                        Color color = Color.White;
                        if (this.main.chipfolder[this.Selectfolder, this.Topchip + index1].chip.number >= 350)
                            color = this.illegalColor;
                        this._position = new Vector2(this.seetX + 104, 32 + 16 * index1);
                        this.main.chipfolder[this.Selectfolder, this.Topchip + index1].chip.IconRender(dg, this._position, false, false, this.main.chipfolder[this.Selectfolder, this.Topchip + index1].codeNo, false);
                        var blockCharacters = this.Nametodata(this.main.chipfolder[this.Selectfolder, this.Topchip + index1].chip.name);
                        this._position = new Vector2(this.seetX + 120, 32 + 16 * index1);
                        DrawBlockCharacters(dg, blockCharacters, 16, this._position, color, out this._rect, out this._position);
                        this._rect = new Rectangle(216 + (int)this.main.chipfolder[this.Selectfolder, this.Topchip + index1].chip.element * 16, 88, 16, 16);
                        this._position = new Vector2(this.seetX + 184, 32 + index1 * 16);
                        dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                        this._rect = new Rectangle((int)this.main.chipfolder[this.Selectfolder, this.Topchip + index1].chip.code[this.main.chipfolder[this.Selectfolder, this.Topchip + index1].codeNo] * 8, 32, 8, 16);
                        this._position = new Vector2(this.seetX + 200, 32 + index1 * 16);
                        dg.DrawImage(dg, "font", this._rect, true, this._position, color);
                        this._rect = new Rectangle(160, 160, 16, 16);
                        this._position = new Vector2(this.seetX + 208, 32 + index1 * 16);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        int[] numArray = this.ChangeCount(this.main.chipfolder[this.Selectfolder, this.Topchip + index1].chip.regsize);
                        PointF pointF = new PointF(this.seetX + 216, 32 + index1 * 16);
                        for (int index2 = 0; index2 < numArray.Length; ++index2)
                        {
                            this._rect = new Rectangle(numArray[index2] * 8, 72, 8, 16);
                            this._position = new Vector2(pointF.X - index2 * 8, pointF.Y);
                            dg.DrawImage(dg, "font", this._rect, true, this._position, Color.Yellow);
                        }
                        if (this.savedata.regularflag[this.Selectfolder] && this.savedata.regularchip[this.Selectfolder] == this.Topchip + index1)
                        {
                            this._rect = new Rectangle(112, 288, 128, 16);
                            this._position = new Vector2(this.seetX + 96, 32 + index1 * 16);
                            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        }
                    }
                }
                if (!(this.main.chipfolder[this.Selectfolder, this.SelectY_Folder].chip is DammyChip))
                {
                    Vector2 p = new Vector2(this.seetX + 24, 32f);
                    this.main.chipfolder[this.Selectfolder, this.SelectY_Folder].chip.GraphicsRender(dg, p, this.main.chipfolder[this.Selectfolder, this.SelectY_Folder].codeNo, true, true);
                    if (this.main.chipfolder[this.Selectfolder, this.SelectY_Folder].chip.dark)
                    {
                        this._rect = new Rectangle(272, 184, 88, 56);
                        this._position = new Vector2(this.seetX + 8, 96f);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    }
                    else if (this.main.chipfolder[this.Selectfolder, this.SelectY_Folder].chip.navi)
                    {
                        this._rect = new Rectangle(272, 128, 88, 56);
                        this._position = new Vector2(this.seetX + 8, 96f);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    }
                    foreach (var data in ((IEnumerable<string>)this.main.chipfolder[this.Selectfolder, this.SelectY_Folder].chip.information).Select((v, i) => new
                    {
                        v,
                        i
                    }))
                    {
                        string v = data.v;
                        Vector2 point = new Vector2(this.seetX + 10, 102 + 16 * data.i);
                        dg.DrawMiniText(v, point, Color.Black);
                    }
                }
                if (!this.selectedForB && this.cursolanime && (this.selected && this.selectedY >= this.Topchip) && this.selectedY < this.Topchip + 7)
                {
                    this._rect = new Rectangle(152, 144, 16, 16);
                    this._position = new Vector2(this.seetX + 88, 32 + (this.selectedY - this.Topchip) * 16);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                }
                this._rect = new Rectangle(112 + 16 * cursolanime2, 160, 16, 16);
                this._position = new Vector2(this.seetX + 88, 32 + this.cursol[0] * 16);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                this._rect = new Rectangle(168 + 8 * cursolanime2, 144, 8, 8);
                this._position = new Vector2(this.seetX + 232, 24f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                float num2 = this.Topchip != 0 ? 4.521739f * Topchip : 0.0f;
                this._rect = new Rectangle(176, 168, 8, 8);
                this._position = new Vector2(this.seetX + 224, 32f + num2);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                string name = "";
                switch (this.Selectfolder)
                {
                    case 0:
                        name = ShanghaiEXE.Translate("FolderEdit.Folder1");
                        break;
                    case 1:
                        name = ShanghaiEXE.Translate("FolderEdit.Folder2");
                        break;
                    case 2:
                        name = this.savedata.foldername;
                        break;
                }
                var nameBlockCharacters = this.Nametodata(name);
                this._position = new Vector2(this.seetX + 104, 10f);
                DrawBlockCharacters(dg, nameBlockCharacters, 88, this._position, Color.White, out this._rect, out this._position);
                int[] numArray1 = this.ChangeCount(savedata.Regularlarge);
                PointF pointF1 = new PointF(this.seetX + 192, 0.0f);
                for (int index = 0; index < numArray1.Length; ++index)
                {
                    this._rect = new Rectangle(numArray1[index] * 8, 72, 8, 16);
                    this._position = new Vector2(pointF1.X - index * 8, pointF1.Y);
                    dg.DrawImage(dg, "font", this._rect, true, this._position, Color.Yellow);
                }
                int[] numArray2 = this.ChangeCount(this.manychips);
                PointF pointF2 = new PointF(this.seetX + 209, 2f);
                for (int index = 0; index < numArray2.Length; ++index)
                {
                    this._rect = new Rectangle(numArray2[index] * 8, 104, 8, 16);
                    this._position = new Vector2(pointF2.X - index * 8, pointF2.Y);
                    dg.DrawImage(dg, "font", this._rect, true, this._position, this.manychips == 30 ? Color.White : Color.Pink);
                }
                int[] numArray3 = this.ChangeCount(30);
                pointF2 = new PointF(this.seetX + 231, 2f);
                for (int index = 0; index < numArray3.Length; ++index)
                {
                    this._rect = new Rectangle(numArray3[index] * 8, 104, 8, 16);
                    this._position = new Vector2(pointF2.X - index * 8, pointF2.Y);
                    dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                }
            }
            if (this.sortflag)
            {
                int num2 = this.nowscene == FolderEdit.SCENE.select ? 24 : 168;
                this._rect = new Rectangle(208, 128, 64, 136);
                this._position = new Vector2(num2, 16f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                string[] strArray = new string[7]
                {
                    ShanghaiEXE.Translate("FolderEdit.ID"),
                    ShanghaiEXE.Translate("FolderEdit.ABC"),
                    ShanghaiEXE.Translate("FolderEdit.Code"),
                    ShanghaiEXE.Translate("FolderEdit.Power"),
                    ShanghaiEXE.Translate("FolderEdit.Element"),
                    ShanghaiEXE.Translate("FolderEdit.Quantity"),
                    ShanghaiEXE.Translate("FolderEdit.MB")
                };
                for (int index = 0; index < 7; ++index)
                {
                    this._position = new Vector2(num2 + 8 + 1, 32 + 16 * index + 1);
                    dg.DrawMiniText(strArray[index], this._position, Color.FromArgb(byte.MaxValue, 32, 32, 32));
                    this._position = new Vector2(num2 + 8, 32 + 16 * index);
                    dg.DrawMiniText(strArray[index], this._position, Color.White);
                }
                this._rect = new Rectangle(112 + 16 * cursolanime2, 160, 16, 16);
                this._position = new Vector2(num2 - 8, 32 + (int)this.SortSelect * 16);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            if (this.error)
            {
                this._rect = new Rectangle(176, 24, 128, 64);
                this._position = new Vector2(56f, 56f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                var dialogue = new Dialogue();
                switch (this.errorMassage)
                {
                    case FolderEdit.ERROR.nochip:
                        if (!this.savedata.FlagList[0])
                        {
                            dialogue = ShanghaiEXE.Translate("FolderEdit.FolderSizeDialogue1");
                            break;
                        }
                        dialogue = ShanghaiEXE.Translate("FolderEdit.FolderSizeSpecialDialogue1");
                        break;
                    case FolderEdit.ERROR.overchip19:
                        if (!this.savedata.FlagList[0])
                        {
                            dialogue = ShanghaiEXE.Translate("FolderEdit.Memory0-19LimitDialogue1Format").Format(5 + this.savedata.plusFolder);
                            break;
                        }
                        dialogue = ShanghaiEXE.Translate("FolderEdit.Memory0-19LimitSpecialDialogue1Format").Format(5 + this.savedata.plusFolder);
                        break;
                    case FolderEdit.ERROR.overchip29:
                        if (!this.savedata.FlagList[0])
                        {
                            dialogue = ShanghaiEXE.Translate("FolderEdit.Memory20-29LimitDialogue1Format").Format(4 + this.savedata.plusFolder);
                            break;
                        }
                        dialogue = ShanghaiEXE.Translate("FolderEdit.Memory20-29LimitSpecialDialogue1Format").Format(4 + this.savedata.plusFolder);
                        break;
                    case FolderEdit.ERROR.overchip39:
                        if (!this.savedata.FlagList[0])
                        {
                            dialogue = ShanghaiEXE.Translate("FolderEdit.Memory30-39LimitDialogue1Format").Format(3 + this.savedata.plusFolder);
                            break;
                        }
                        dialogue = ShanghaiEXE.Translate("FolderEdit.Memory30-39LimitSpecialDialogue1Format").Format(3 + this.savedata.plusFolder);
                        break;
                    case FolderEdit.ERROR.overchip49:
                        if (!this.savedata.FlagList[0])
                        {
                            dialogue = ShanghaiEXE.Translate("FolderEdit.Memory40-49LimitDialogue1Format").Format(2 + this.savedata.plusFolder);
                            break;
                        }
                        dialogue = ShanghaiEXE.Translate("FolderEdit.Memory40-49LimitSpecialDialogue1Format").Format(2 + this.savedata.plusFolder);
                        break;
                    case FolderEdit.ERROR.overchip99:
                        if (!this.savedata.FlagList[0])
                        {
                            dialogue = ShanghaiEXE.Translate("FolderEdit.Memory50+LimitDialogue1Format").Format(1 + this.savedata.plusFolder);
                            break;
                        }
                        dialogue = ShanghaiEXE.Translate("FolderEdit.Memory50+LimitSpecialDialogue1Format").Format(1 + this.savedata.plusFolder);
                        break;
                    case FolderEdit.ERROR.overnavi:
                        if (!this.savedata.FlagList[0])
                        {
                            dialogue = ShanghaiEXE.Translate("FolderEdit.NaviLimitDialogue1Format").Format(this.savedata.NaviFolder);
                            break;
                        }
                        dialogue = ShanghaiEXE.Translate("FolderEdit.NaviLimitSpecialDialogue1Format").Format(this.savedata.NaviFolder); ;
                        break;
                    case FolderEdit.ERROR.overdark:
                        if (!this.savedata.FlagList[0])
                        {
                            dialogue = ShanghaiEXE.Translate("FolderEdit.DarkChipLimitDialogue1Format").Format(this.savedata.darkFolder);
                            break;
                        }
                        dialogue = ShanghaiEXE.Translate("FolderEdit.DarkChipLimitSpecialDialogue1Format").Format(this.savedata.darkFolder);
                        break;
                    case FolderEdit.ERROR.overbyte:
                        if (!this.savedata.FlagList[0])
                        {
                            dialogue = ShanghaiEXE.Translate("FolderEdit.RegChipMemoryLimitDialogue1");
                            break;
                        }
                        dialogue = ShanghaiEXE.Translate("FolderEdit.RegChipMemoryLimitSpecialDialogue1");
                        break;
                    case FolderEdit.ERROR.allOut:
                        if (!this.savedata.FlagList[0])
                        {
                            dialogue = ShanghaiEXE.Translate("FolderEdit.ClearFolderDialogue1");
                            break;
                        }
                        dialogue = ShanghaiEXE.Translate("FolderEdit.ClearFolderSpecialDialogue1");
                        break;
                }
                Vector2 point = new Vector2(62f, 62f);
                dg.DrawMiniText(dialogue[0], point, Color.Black);
                point = new Vector2(62f, 78f);
                dg.DrawMiniText(dialogue[1], point, Color.Black);
                point = new Vector2(62f, 94f);
                dg.DrawMiniText(dialogue[2], point, Color.Black);
                this._rect = new Rectangle(240 + 16 * cursolanime2, 0, 16, 16);
                this._position = new Vector2(152f, 112f);
                dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
            }
            if (this.nowscene != FolderEdit.SCENE.fadein && this.nowscene != FolderEdit.SCENE.fadeout)
                return;
            Color color1 = Color.FromArgb(this.Alpha, 0, 0, 0);
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color1);
        }

        private void Decide()
        {
            if (!this.selected)
            {
                this.sound.PlaySE(SoundEffect.decide);
                this.selectedForB = this.nowscene == FolderEdit.SCENE.bag;
                this.selectedY = this.nowscene == FolderEdit.SCENE.bag ? this.SelectY_Bag : this.SelectY_Folder;
                this.selected = true;
            }
            else
            {
                if (this.selectedForB == (this.nowscene == FolderEdit.SCENE.bag) && this.selectedY == (this.nowscene == FolderEdit.SCENE.bag ? this.SelectY_Bag : this.SelectY_Folder))
                {
                    if (this.nowscene == FolderEdit.SCENE.bag)
                    {
                        if (this.savedata.havechips.Count > 0 && this.manychips < 30)
                        {
                            int num1 = 0;
                            int index1;
                            if (this.selectedForB)
                            {
                                index1 = this.selectedY;
                                num1 = this.SelectY_Folder;
                            }
                            else
                            {
                                index1 = this.SelectY_Bag;
                                num1 = this.selectedY;
                            }
                            bool flag = true;
                            int num2 = this.Manychip(this.savedata.havechips[index1].number);
                            ChipFolder chipFolder = new ChipFolder(this.sound);
                            chipFolder.SettingChip(this.savedata.havechips[index1].number);
                            if (chipFolder.chip.regsize <= 19 && num2 >= 5 + this.savedata.plusFolder)
                            {
                                this.error = true;
                                this.errorMassage = FolderEdit.ERROR.overchip19;
                                this.sound.PlaySE(SoundEffect.error);
                                flag = false;
                            }
                            else if (chipFolder.chip.regsize > 19 && chipFolder.chip.regsize <= 29 && num2 >= 4 + this.savedata.plusFolder)
                            {
                                this.error = true;
                                this.errorMassage = FolderEdit.ERROR.overchip29;
                                this.sound.PlaySE(SoundEffect.error);
                                flag = false;
                            }
                            else if (chipFolder.chip.regsize > 29 && chipFolder.chip.regsize <= 39 && num2 >= 3 + this.savedata.plusFolder)
                            {
                                this.error = true;
                                this.errorMassage = FolderEdit.ERROR.overchip39;
                                this.sound.PlaySE(SoundEffect.error);
                                flag = false;
                            }
                            else if (chipFolder.chip.regsize > 39 && chipFolder.chip.regsize <= 49 && num2 >= 2 + this.savedata.plusFolder)
                            {
                                this.error = true;
                                this.errorMassage = FolderEdit.ERROR.overchip49;
                                this.sound.PlaySE(SoundEffect.error);
                                flag = false;
                            }
                            else if (chipFolder.chip.regsize > 49 && num2 >= 1 + this.savedata.plusFolder)
                            {
                                this.error = true;
                                this.errorMassage = FolderEdit.ERROR.overchip99;
                                this.sound.PlaySE(SoundEffect.error);
                                flag = false;
                            }
                            int num3 = this.Manynavi();
                            if (chipFolder.chip.navi)
                                ++num3;
                            if (num3 > this.savedata.NaviFolder)
                            {
                                this.error = true;
                                this.errorMassage = FolderEdit.ERROR.overnavi;
                                this.sound.PlaySE(SoundEffect.error);
                                flag = false;
                            }
                            int num4 = this.Manydark();
                            if (chipFolder.chip.dark)
                                ++num4;
                            if (num4 > this.savedata.darkFolder)
                            {
                                this.error = true;
                                this.errorMassage = FolderEdit.ERROR.overdark;
                                this.sound.PlaySE(SoundEffect.error);
                                flag = false;
                            }
                            if (flag)
                            {
                                this.sound.PlaySE(SoundEffect.decide);
                                int index2 = 0;
                                for (int index3 = 0; index3 < 30; ++index3)
                                {
                                    if (!this.main.chipfolder[this.Selectfolder, index3].inchip || this.main.chipfolder[this.Selectfolder, index3].chip is DammyChip)
                                    {
                                        index2 = index3;
                                        break;
                                    }
                                }
                                this.main.chipfolder[this.Selectfolder, index2].SettingChip(this.savedata.havechips[this.selectedY].number);
                                this.main.chipfolder[this.Selectfolder, index2].codeNo = this.savedata.havechips[this.selectedY].code;
                                this.savedata.LosChip(this.savedata.havechips[this.selectedY].number, this.savedata.havechips[this.selectedY].code);
                                if (this.SelectY_Bag >= this.savedata.havechips.Count && this.SelectY_Bag > 0)
                                {
                                    if (this.cursol[1] > 0 && this.savedata.havechips.Count >= this.cursol[1] && this.Topchip2 == 0)
                                        --this.cursol[1];
                                    --this.Topchip2;
                                }
                                ++this.manychips;
                            }
                        }
                    }
                    else if (!(this.main.chipfolder[this.Selectfolder, this.selectedY].chip is DammyChip))
                    {
                        this.sound.PlaySE(SoundEffect.decide);
                        this.savedata.AddChip(this.main.chipfolder[this.Selectfolder, this.selectedY].chip.number, this.main.chipfolder[this.Selectfolder, this.selectedY].codeNo, false);
                        if (Debug.RegularMove && (this.savedata.regularflag[this.Selectfolder] && this.savedata.regularchip[this.Selectfolder] == this.selectedY))
                            this.savedata.regularflag[this.Selectfolder] = false;
                        this.main.chipfolder[this.Selectfolder, this.selectedY] = new ChipFolder(this.sound);
                        --this.manychips;
                    }
                }
                else if (this.selectedForB == (this.nowscene == FolderEdit.SCENE.bag) && this.selectedY != (this.nowscene == FolderEdit.SCENE.bag ? this.SelectY_Bag : this.SelectY_Folder))
                {
                    this.sound.PlaySE(SoundEffect.decide);
                    if (this.selectedForB)
                    {
                        ChipS havechip = this.savedata.havechips[this.selectedY];
                        this.savedata.havechips[this.selectedY] = this.savedata.havechips[this.SelectY_Bag];
                        this.savedata.havechips[this.SelectY_Bag] = havechip;
                    }
                    else
                    {
                        ChipFolder chipFolder = this.main.chipfolder[this.Selectfolder, this.selectedY];
                        this.main.chipfolder[this.Selectfolder, this.selectedY] = this.main.chipfolder[this.Selectfolder, this.SelectY_Folder];
                        this.main.chipfolder[this.Selectfolder, this.SelectY_Folder] = chipFolder;
                        if (Debug.RegularMove)
                        {
                            if (this.savedata.regularflag[this.Selectfolder] && this.savedata.regularchip[this.Selectfolder] == this.selectedY)
                                this.savedata.regularchip[this.Selectfolder] = (byte)this.SelectY_Folder;
                            else if (this.savedata.regularflag[this.Selectfolder] && this.savedata.regularchip[this.Selectfolder] == this.SelectY_Folder)
                                this.savedata.regularchip[this.Selectfolder] = (byte)this.selectedY;
                        }
                    }
                }
                else if (this.selectedForB != (this.nowscene == FolderEdit.SCENE.bag))
                {
                    int index1;
                    int index2;
                    if (this.selectedForB)
                    {
                        index1 = this.selectedY;
                        index2 = this.SelectY_Folder;
                    }
                    else
                    {
                        index1 = this.SelectY_Bag;
                        index2 = this.selectedY;
                    }
                    if (this.savedata.havechips.Count == 0)
                    {
                        this.sound.PlaySE(SoundEffect.decide);
                        if (!(this.main.chipfolder[this.Selectfolder, index2].chip is DammyChip))
                        {
                            this.savedata.AddChip(this.main.chipfolder[this.Selectfolder, index2].chip.number, this.main.chipfolder[this.Selectfolder, index2].codeNo, false);
                            this.main.chipfolder[this.Selectfolder, index2] = new ChipFolder(this.sound);
                            if (Debug.RegularMove && (this.savedata.regularflag[this.Selectfolder] && this.savedata.regularchip[this.Selectfolder] == index2))
                                this.savedata.regularflag[this.Selectfolder] = false;
                            --this.manychips;
                        }
                    }
                    else if (this.main.chipfolder[this.Selectfolder, index2].chip is DammyChip)
                    {
                        bool flag = true;
                        int num = this.Manychip(this.savedata.havechips[index1].number);
                        ChipFolder chipFolder = new ChipFolder(this.sound);
                        chipFolder.SettingChip(this.savedata.havechips[index1].number);
                        if (chipFolder.chip.regsize <= 19 && num >= 5 + this.savedata.plusFolder)
                        {
                            this.error = true;
                            this.errorMassage = FolderEdit.ERROR.overchip19;
                            this.sound.PlaySE(SoundEffect.error);
                            flag = false;
                        }
                        else if (chipFolder.chip.regsize > 19 && chipFolder.chip.regsize <= 29 && num >= 4 + this.savedata.plusFolder)
                        {
                            this.error = true;
                            this.errorMassage = FolderEdit.ERROR.overchip29;
                            this.sound.PlaySE(SoundEffect.error);
                            flag = false;
                        }
                        else if (chipFolder.chip.regsize > 29 && chipFolder.chip.regsize <= 39 && num >= 3 + this.savedata.plusFolder)
                        {
                            this.error = true;
                            this.errorMassage = FolderEdit.ERROR.overchip39;
                            this.sound.PlaySE(SoundEffect.error);
                            flag = false;
                        }
                        else if (chipFolder.chip.regsize > 39 && chipFolder.chip.regsize <= 49 && num >= 2 + this.savedata.plusFolder)
                        {
                            this.error = true;
                            this.errorMassage = FolderEdit.ERROR.overchip49;
                            this.sound.PlaySE(SoundEffect.error);
                            flag = false;
                        }
                        else if (chipFolder.chip.regsize > 49 && num >= 1 + this.savedata.plusFolder)
                        {
                            this.error = true;
                            this.errorMassage = FolderEdit.ERROR.overchip99;
                            this.sound.PlaySE(SoundEffect.error);
                            flag = false;
                        }
                        if (this.Manynavi() >= this.savedata.NaviFolder && chipFolder.chip.navi)
                        {
                            this.error = true;
                            this.errorMassage = FolderEdit.ERROR.overnavi;
                            this.sound.PlaySE(SoundEffect.error);
                            flag = false;
                        }
                        if (this.Manydark() >= this.savedata.darkFolder && chipFolder.chip.dark)
                        {
                            this.error = true;
                            this.errorMassage = FolderEdit.ERROR.overdark;
                            this.sound.PlaySE(SoundEffect.error);
                            flag = false;
                        }
                        if (flag)
                        {
                            this.sound.PlaySE(SoundEffect.decide);
                            this.main.chipfolder[this.Selectfolder, index2].SettingChip(this.savedata.havechips[index1].number);
                            this.main.chipfolder[this.Selectfolder, index2].codeNo = this.savedata.havechips[index1].code;
                            if (Debug.RegularMove && (this.savedata.regularflag[this.Selectfolder] && this.savedata.regularchip[this.Selectfolder] == index2))
                                this.savedata.regularflag[this.Selectfolder] = false;
                            this.savedata.LosChip(this.savedata.havechips[index1].number, this.savedata.havechips[index1].code);
                            if (this.SelectY_Bag >= this.savedata.havechips.Count && this.SelectY_Bag > 0)
                            {
                                if (this.cursol[1] > 0)
                                    --this.cursol[1];
                                else
                                    --this.Topchip2;
                            }
                            ++this.manychips;
                        }
                        else
                            this.sound.PlaySE(SoundEffect.error);
                    }
                    else
                    {
                        bool flag = true;
                        int num1 = this.Manychip(this.savedata.havechips[index1].number);
                        if (this.savedata.havechips[index1].number == this.main.chipfolder[this.Selectfolder, index2].chip.number)
                            --num1;
                        ChipFolder chipFolder = new ChipFolder(this.sound);
                        chipFolder.SettingChip(this.savedata.havechips[index1].number);
                        if (chipFolder.chip.regsize <= 19 && num1 >= 5 + this.savedata.plusFolder)
                        {
                            this.error = true;
                            this.errorMassage = FolderEdit.ERROR.overchip19;
                            this.sound.PlaySE(SoundEffect.error);
                            flag = false;
                        }
                        else if (chipFolder.chip.regsize > 19 && chipFolder.chip.regsize <= 29 && num1 >= 4 + this.savedata.plusFolder)
                        {
                            this.error = true;
                            this.errorMassage = FolderEdit.ERROR.overchip29;
                            this.sound.PlaySE(SoundEffect.error);
                            flag = false;
                        }
                        else if (chipFolder.chip.regsize > 29 && chipFolder.chip.regsize <= 39 && num1 >= 3 + this.savedata.plusFolder)
                        {
                            this.error = true;
                            this.errorMassage = FolderEdit.ERROR.overchip39;
                            this.sound.PlaySE(SoundEffect.error);
                            flag = false;
                        }
                        else if (chipFolder.chip.regsize > 39 && chipFolder.chip.regsize <= 49 && num1 >= 2 + this.savedata.plusFolder)
                        {
                            this.error = true;
                            this.errorMassage = FolderEdit.ERROR.overchip49;
                            this.sound.PlaySE(SoundEffect.error);
                            flag = false;
                        }
                        else if (chipFolder.chip.regsize > 49 && num1 >= 1 + this.savedata.plusFolder)
                        {
                            this.error = true;
                            this.errorMassage = FolderEdit.ERROR.overchip99;
                            this.sound.PlaySE(SoundEffect.error);
                            flag = false;
                        }
                        int num2 = this.Manynavi();
                        if (chipFolder.chip.navi)
                            ++num2;
                        if (this.main.chipfolder[this.Selectfolder, index2].chip.navi)
                            --num2;
                        if (num2 > this.savedata.NaviFolder)
                        {
                            this.error = true;
                            this.errorMassage = FolderEdit.ERROR.overnavi;
                            this.sound.PlaySE(SoundEffect.error);
                            flag = false;
                        }
                        int num3 = this.Manydark();
                        if (chipFolder.chip.dark)
                            ++num3;
                        if (this.main.chipfolder[this.Selectfolder, index2].chip.dark)
                            --num3;
                        if (num3 > this.savedata.darkFolder)
                        {
                            this.error = true;
                            this.errorMassage = FolderEdit.ERROR.overdark;
                            this.sound.PlaySE(SoundEffect.error);
                            flag = false;
                        }
                        if (flag)
                        {
                            this.sound.PlaySE(SoundEffect.decide);
                            int number = this.main.chipfolder[this.Selectfolder, index2].chip.number;
                            int codeNo = this.main.chipfolder[this.Selectfolder, index2].codeNo;
                            this.main.chipfolder[this.Selectfolder, index2].SettingChip(this.savedata.havechips[index1].number);
                            this.main.chipfolder[this.Selectfolder, index2].codeNo = this.savedata.havechips[index1].code;
                            this.savedata.LosChip(this.savedata.havechips[index1].number, this.savedata.havechips[index1].code);
                            this.savedata.AddChip(number, codeNo, false);
                            if (Debug.RegularMove && (this.savedata.regularflag[this.Selectfolder] && this.savedata.regularchip[this.Selectfolder] == index2))
                                this.savedata.regularflag[this.Selectfolder] = false;
                        }
                        else
                            this.sound.PlaySE(SoundEffect.error);
                    }
                }
                this.selected = false;
            }
        }

        public int Manychip(int no)
        {
            int num = 0;
            for (int index = 0; index < 30; ++index)
            {
                if (!(this.main.chipfolder[this.Selectfolder, index].chip is DammyChip) && this.main.chipfolder[this.Selectfolder, index].chip.number == no)
                    ++num;
            }
            return num;
        }

        private int Manynavi()
        {
            int num = 0;
            for (int index = 0; index < 30; ++index)
            {
                if (!(this.main.chipfolder[this.Selectfolder, index].chip is DammyChip) && this.main.chipfolder[this.Selectfolder, index].chip.navi)
                    ++num;
            }
            return num;
        }

        private int Manydark()
        {
            int num = 0;
            for (int index = 0; index < 30; ++index)
            {
                if (!(this.main.chipfolder[this.Selectfolder, index].chip is DammyChip) && this.main.chipfolder[this.Selectfolder, index].chip.dark)
                    ++num;
            }
            return num;
        }

        private void Sort()
        {
            int number = this.main.chipfolder[this.Selectfolder, this.savedata.regularchip[this.Selectfolder]].chip.number;
            int codeNo = this.main.chipfolder[this.Selectfolder, this.savedata.regularchip[this.Selectfolder]].codeNo;
            if (this.nowscene == FolderEdit.SCENE.select)
            {
                ChipFolder[] array = ((IEnumerable<ChipFolder>)((IEnumerable<ChipFolder>)this.ChangeArray()).OrderBy<ChipFolder, ChipFolder.CODE>(n => n.chip.code[n.codeNo]).ToArray<ChipFolder>()).OrderBy<ChipFolder, float>(n => n.chip.sortNumber).ToArray<ChipFolder>();
                switch (this.SortSelect)
                {
                    case FolderEdit.SORT.aiueo:
                        array = ((IEnumerable<ChipFolder>)array).OrderBy<ChipFolder, string>(n => n.chip.name).ToArray<ChipFolder>();
                        break;
                    case FolderEdit.SORT.cord:
                        array = ((IEnumerable<ChipFolder>)array).OrderBy<ChipFolder, ChipFolder.CODE>(n => n.chip.code[n.codeNo]).ToArray<ChipFolder>();
                        break;
                    case FolderEdit.SORT.power:
                        array = ((IEnumerable<ChipFolder>)array).OrderByDescending<ChipFolder, int>(n => n.chip.power).ToArray<ChipFolder>();
                        break;
                    case FolderEdit.SORT.element:
                        array = ((IEnumerable<ChipFolder>)array).OrderBy<ChipFolder, ChipBase.ELEMENT>(n => n.chip.element).ToArray<ChipFolder>();
                        break;
                    case FolderEdit.SORT.many:
                        array = ((IEnumerable<ChipFolder>)array).OrderBy<ChipFolder, int>(n => this.Manychip(n.chip.number)).ToArray<ChipFolder>();
                        break;
                    case FolderEdit.SORT.size:
                        array = ((IEnumerable<ChipFolder>)array).OrderByDescending<ChipFolder, int>(n => n.chip.regsize).ToArray<ChipFolder>();
                        break;
                }
                if (this.sortmode)
                    array = ((IEnumerable<ChipFolder>)array).Reverse<ChipFolder>().ToArray<ChipFolder>();
                this.SortEnd(array);
            }
            else
            {
                if (this.sortmode)
                    this.savedata.havechips = this.savedata.havechips.OrderByDescending<ChipS, int>(n => n.Code).ToList<ChipS>();
                else
                    this.savedata.havechips = this.savedata.havechips.OrderBy<ChipS, int>(n => n.Code).ToList<ChipS>();
                if (this.sortmode)
                    this.savedata.havechips = this.savedata.havechips.OrderByDescending<ChipS, float>(n => n.sortNumber).ToList<ChipS>();
                else
                    this.savedata.havechips = this.savedata.havechips.OrderBy<ChipS, float>(n => n.sortNumber).ToList<ChipS>();
                switch (this.SortSelect)
                {
                    case FolderEdit.SORT.aiueo:
                        if (this.sortmode)
                        {
                            this.savedata.havechips = this.savedata.havechips.OrderByDescending<ChipS, string>(n => n.Name).ToList<ChipS>();
                            break;
                        }
                        this.savedata.havechips = this.savedata.havechips.OrderBy<ChipS, string>(n => n.Name).ToList<ChipS>();
                        break;
                    case FolderEdit.SORT.cord:
                        if (this.sortmode)
                        {
                            this.savedata.havechips = this.savedata.havechips.OrderByDescending<ChipS, int>(n => n.Code).ToList<ChipS>();
                            break;
                        }
                        this.savedata.havechips = this.savedata.havechips.OrderBy<ChipS, int>(n => n.Code).ToList<ChipS>();
                        break;
                    case FolderEdit.SORT.power:
                        if (!this.sortmode)
                        {
                            this.savedata.havechips = this.savedata.havechips.OrderByDescending<ChipS, int>(n => n.Power).ToList<ChipS>();
                            break;
                        }
                        this.savedata.havechips = this.savedata.havechips.OrderBy<ChipS, int>(n => n.Power).ToList<ChipS>();
                        break;
                    case FolderEdit.SORT.element:
                        if (this.sortmode)
                        {
                            this.savedata.havechips = this.savedata.havechips.OrderByDescending<ChipS, int>(n => n.Element).ToList<ChipS>();
                            break;
                        }
                        this.savedata.havechips = this.savedata.havechips.OrderBy<ChipS, int>(n => n.Element).ToList<ChipS>();
                        break;
                    case FolderEdit.SORT.many:
                        if (this.sortmode)
                        {
                            this.savedata.havechips = this.savedata.havechips.OrderBy<ChipS, int>(n => n.Many(this.savedata)).ToList<ChipS>();
                            break;
                        }
                        this.savedata.havechips = this.savedata.havechips.OrderByDescending<ChipS, int>(n => n.Many(this.savedata)).ToList<ChipS>();
                        break;
                    case FolderEdit.SORT.size:
                        if (!this.sortmode)
                        {
                            this.savedata.havechips = this.savedata.havechips.OrderByDescending<ChipS, int>(n => n.Regsize).ToList<ChipS>();
                            break;
                        }
                        this.savedata.havechips = this.savedata.havechips.OrderBy<ChipS, int>(n => n.Regsize).ToList<ChipS>();
                        break;
                }
            }
            if (!Debug.RegularMove || !this.savedata.regularflag[this.Selectfolder])
                return;
            for (byte index = 0; index < 30; ++index)
            {
                if (this.main.chipfolder[this.Selectfolder, index].chip.number == number && this.main.chipfolder[this.Selectfolder, index].codeNo == codeNo)
                {
                    this.savedata.regularchip[this.Selectfolder] = index;
                    break;
                }
            }
        }

        private ChipFolder[] ChangeArray()
        {
            ChipFolder[] chipFolderArray = new ChipFolder[30];
            foreach (var data in ((IEnumerable<ChipFolder>)chipFolderArray).Select((v, i) => new
            {
                v,
                i
            }))
                chipFolderArray[data.i] = this.main.chipfolder[this.Selectfolder, data.i];
            return chipFolderArray;
        }

        private void SortEnd(ChipFolder[] chip)
        {
            foreach (var data in ((IEnumerable<ChipFolder>)chip).Select((v, i) => new
            {
                v,
                i
            }))
                this.main.chipfolder[this.Selectfolder, data.i] = chip[data.i];
        }

        private enum SCENE
        {
            fadein,
            select,
            goside,
            bag,
            fadeout,
        }

        private enum ERROR
        {
            nochip,
            overchip19,
            overchip29,
            overchip39,
            overchip49,
            overchip99,
            overnavi,
            overdark,
            overbyte,
            allOut,
        }

        private enum SORT
        {
            id,
            aiueo,
            cord,
            power,
            element,
            many,
            size,
        }
    }
}
