using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using SlimDX;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSMap.Character.Menu
{
    internal class DataList : MenuBase
    {
        private readonly int[] comp = new int[4];
        private readonly int[] cursol = new int[4];
        private readonly int[] selectY = new int[4];
        private readonly int[] topchip = new int[4];
        private readonly int[] baseX = new int[4];
        private DataList.SCENE nowscene;
        private const int manypage = 4;
        private DataList.PAGE page;
        private bool moveLeftRight;
        private const int overtop_normal = 183;
        private const int overtop_navi = 64;
        private const int overtop_dark = 9;
        private const int overtop_PA = 25;
        private byte code;
        private const int stopLeft = 96;
        private const int stopRight = 256;
        private const int move1F = 16;
        private int cursolanime;

        private int Max
        {
            get
            {
                switch (this.page)
                {
                    case DataList.PAGE.normal:
                        return 190;
                    case DataList.PAGE.navi:
                        return 64;// 64;
                    case DataList.PAGE.dark:
                        return 16;
                    default:
                        return 32;
                }
            }
        }

        private int Topchip
        {
            get
            {
                return this.topchip[(int)this.page];
            }
            set
            {
                this.topchip[(int)this.page] = value;
                int num = 0;
                switch (this.page)
                {
                    case DataList.PAGE.normal:
                        num = 183;
                        break;
                    case DataList.PAGE.navi:
                        num = 64; //57
                        break;
                    case DataList.PAGE.dark:
                        num = 9;
                        break;
                    case DataList.PAGE.PA:
                        num = 25;
                        break;
                }
                if (this.topchip[(int)this.page] >= num)
                    this.topchip[(int)this.page] = num;
                if (this.topchip[(int)this.page] >= 0)
                    return;
                this.topchip[(int)this.page] = 0;
            }
        }

        public DataList(MyAudio s, Player p, TopMenu t, SaveData save)
          : base(s, p, t, save)
        {
            this.nowscene = DataList.SCENE.fadein;
            this.page = DataList.PAGE.normal;
            for (int index = 0; index < 4; ++index)
                this.baseX[index] = index == 0 ? 96 : 256;
            this.comp[0] = this.savedata.Comp_normal;
            this.comp[1] = this.savedata.Comp_navi;
            this.comp[2] = this.savedata.Comp_dark;
            this.comp[3] = this.savedata.Comp_PA;
        }

        public override void UpDate()
        {
            switch (this.nowscene)
            {
                case DataList.SCENE.fadein:
                    if (this.Alpha > 0)
                    {
                        this.Alpha -= 51;
                        break;
                    }
                    this.nowscene = DataList.SCENE.select;
                    break;
                case DataList.SCENE.select:
                    this.Control();
                    break;
                case DataList.SCENE.move:
                    if (this.moveLeftRight)
                    {
                        if (this.baseX[(int)this.page] < 256)
                        {
                            this.baseX[(int)this.page] += 16;
                            break;
                        }
                        this.baseX[(int)this.page] = 256;
                        --this.page;
                        this.nowscene = DataList.SCENE.select;
                        break;
                    }
                    if (this.baseX[(int)this.page] > 96)
                    {
                        this.baseX[(int)this.page] -= 16;
                    }
                    else
                    {
                        this.baseX[(int)this.page] = 96;
                        this.nowscene = DataList.SCENE.select;
                    }
                    break;
                case DataList.SCENE.fadeout:
                    if (this.Alpha < byte.MaxValue)
                    {
                        this.Alpha += 51;
                        break;
                    }
                    this.topmenu.Return();
                    break;
            }
            if (this.frame % 10 == 0)
                ++this.cursolanime;
            if (this.cursolanime >= 3)
                this.cursolanime = 0;
            this.FlamePlus();
        }

        private void Control()
        {
            if (Input.IsPress(Button._A))
            {
                int num;
                switch (this.page)
                {
                    case DataList.PAGE.normal:
                        num = this.selectY[(int)this.page] + 1;
                        break;
                    case DataList.PAGE.navi:
                        num = this.selectY[(int)this.page] + 1 + 190;
                        break;
                    case DataList.PAGE.dark:
                        num = this.selectY[(int)this.page] + 1 + 190 + 64;
                        break;
                    default:
                        num = this.selectY[(int)this.page] + 1 + 190 + 64 + 16;
                        break;
                }
                if (this.savedata.datelist[num - 1])
                {
                    ++this.code;
                    if (this.code >= 4)
                        this.code = 0;
                    this.sound.PlaySE(MyAudio.SOUNDNAMES.decide);
                }
            }
            if (Input.IsPress(Button._B))
            {
                this.sound.PlaySE(MyAudio.SOUNDNAMES.cancel);
                this.nowscene = DataList.SCENE.fadeout;
            }
            if (this.waittime <= 0)
            {
                if (Input.IsPush(Button._R))
                {
                    int num = this.Max - 7 - this.Topchip;
                    if (num > 7)
                        num = 7;
                    if (num > 0)
                    {
                        this.sound.PlaySE(MyAudio.SOUNDNAMES.movecursol);
                        this.Topchip += num;
                        this.selectY[(int)this.page] += num;
                    }
                    this.waittime = Input.IsPress(Button._R) ? 10 : 4;
                }
                if (Input.IsPush(Button._L))
                {
                    int num = this.Topchip;
                    if (num > 7)
                        num = 7;
                    if (num > 0)
                    {
                        this.sound.PlaySE(MyAudio.SOUNDNAMES.movecursol);
                        this.Topchip -= num;
                        this.selectY[(int)this.page] -= num;
                    }
                    this.waittime = Input.IsPress(Button._L) ? 10 : 4;
                }
                if (Input.IsPush(Button.Up))
                {
                    if (this.selectY[(int)this.page] > 0)
                    {
                        --this.selectY[(int)this.page];
                        if (this.cursol[(int)this.page] > 0)
                            --this.cursol[(int)this.page];
                        else
                            --this.Topchip;
                        this.sound.PlaySE(MyAudio.SOUNDNAMES.movecursol);
                    }
                    this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                }
                if (Input.IsPush(Button.Down))
                {
                    if (this.selectY[(int)this.page] < this.Max - 1)
                    {
                        ++this.selectY[(int)this.page];
                        if (this.cursol[(int)this.page] < 6)
                            ++this.cursol[(int)this.page];
                        else
                            ++this.Topchip;
                        this.sound.PlaySE(MyAudio.SOUNDNAMES.movecursol);
                    }
                    this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                }
            }
            else
                --this.waittime;
            if (Input.IsPush(Button.Left) && this.page > DataList.PAGE.normal)
            {
                this.moveLeftRight = true;
                this.nowscene = DataList.SCENE.move;
                this.sound.PlaySE(MyAudio.SOUNDNAMES.menuopen);
            }
            if (!Input.IsPush(Button.Right) || this.page >= DataList.PAGE.PA)
                return;
            ++this.page;
            this.moveLeftRight = false;
            this.nowscene = DataList.SCENE.move;
            this.sound.PlaySE(MyAudio.SOUNDNAMES.menuopen);
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(0, 624, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            Color color1 = Color.White;
            if (this.comp[(int)this.page] == this.Max)
                color1 = Color.Cyan;
            int[] numArray1 = this.ChangeCount(this.Max);
            Vector2 vector2_1 = new Vector2(208 + 8 * numArray1.Length, 0.0f);
            for (int index = 0; index < numArray1.Length; ++index)
            {
                this._rect = new Rectangle(numArray1[index] * 8, 104, 8, 16);
                this._position = new Vector2(vector2_1.X - index * 8, vector2_1.Y);
                dg.DrawImage(dg, "font", this._rect, true, this._position, color1);
            }
            int[] numArray2 = this.ChangeCount(this.comp[(int)this.page]);
            vector2_1 = new Vector2(200f, 0.0f);
            for (int index = 0; index < numArray2.Length; ++index)
            {
                this._rect = new Rectangle(numArray2[index] * 8, 104, 8, 16);
                this._position = new Vector2(vector2_1.X - index * 8, vector2_1.Y);
                dg.DrawImage(dg, "font", this._rect, true, this._position, color1);
            }
            int key;
            switch (this.page)
            {
                case DataList.PAGE.normal:
                    key = this.selectY[(int)this.page] + 1;
                    break;
                case DataList.PAGE.navi:
                    key = this.selectY[(int)this.page] + 1 + 190;
                    break;
                case DataList.PAGE.dark:
                    key = this.selectY[(int)this.page] + 1 + 190 + 64;
                    break;
                default:
                    key = this.selectY[(int)this.page] + 1 + 190 + 64 + 16;
                    break;
            }
            if (this.page == DataList.PAGE.dark)
            {
                this._rect = new Rectangle(272, 184, 88, 56);
                this._position = new Vector2(8f, 96f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            else if (this.page == DataList.PAGE.navi)
            {
                this._rect = new Rectangle(272, 128, 88, 56);
                this._position = new Vector2(8f, 96f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            else if (this.page == DataList.PAGE.PA)
            {
                this._rect = new Rectangle(760, 120, 88, 136);
                this._position = new Vector2(8f, 16f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            if (this.savedata.datelist[key - 1])
            {
                ChipFolder chipFolder = new ChipFolder(null);
                chipFolder.SettingChip(key);
                this._position = new Vector2(24f, 32f);
                chipFolder.chip.GraphicsRender(dg, this._position, code, true, true);
                foreach (var data in ((IEnumerable<string>)chipFolder.chip.information).Select((v, i) => new
                {
                    v,
                    i
                }))
                {
                    string v = data.v;
                    Vector2 point = new Vector2(10f, 102 + 16 * data.i);
                    dg.DrawMiniText(v, point, Color.Black);
                }
            }
            for (int index1 = 0; index1 < 4; ++index1)
            {
                if (this.page == (DataList.PAGE)index1 || this.nowscene == DataList.SCENE.move)
                {
                    this._rect = new Rectangle(360, 0, 136, 144);
                    this._position = new Vector2(this.baseX[index1], 8f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    Vector2 vector2_2;
                    for (int index2 = 0; index2 < 7; ++index2)
                    {
                        int num;
                        switch (index1)
                        {
                            case 0:
                                num = index2 + this.topchip[index1] + 1;
                                break;
                            case 1:
                                num = index2 + this.topchip[index1] + 1 + 190;
                                break;
                            case 2:
                                num = index2 + this.topchip[index1] + 1 + 190 + 64;
                                break;
                            default:
                                num = index2 + this.topchip[index1] + 1 + 190 + 64 + 16;
                                break;
                        }
                        if (index1 < 3)
                        {
                            int[] numArray3 = this.ChangeCount(num);
                            vector2_2 = new Vector2(this.baseX[index1] + 24, 32 + index2 * 16);
                            for (int index3 = 0; index3 < numArray3.Length; ++index3)
                            {
                                this._rect = new Rectangle(numArray3[index3] * 8, 0, 8, 16);
                                this._position = new Vector2(vector2_2.X - index3 * 8, vector2_2.Y);
                                dg.DrawImage(dg, "font", this._rect, true, this._position, Color.SkyBlue);
                            }
                        }
                        ChipS chipS = new ChipS(num, code);
                        AllBase.NAME[] nameArray = this.Nametodata(!this.savedata.datelist[num - 1] ? "　　　？？" : chipS.Name);
                        vector2_2 = new Vector2(index1 < 3 ? this.baseX[index1] + 48 : this.baseX[index1] + 16, 32 + index2 * 16);
                        foreach (var data in ((IEnumerable<AllBase.NAME>)nameArray).Select((v, j) => new
                        {
                            v,
                            j
                        }))
                        {
                            this._rect = new Rectangle((int)data.v * 8, 16, 8, 16);
                            this._position = new Vector2(vector2_2.X + 8 * data.j, vector2_2.Y);
                            dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                        }
                        if (this.savedata.datelist[num - 1] && index1 < 3)
                        {
                            ChipFolder chipFolder = new ChipFolder(null);
                            chipFolder.SettingChip(num);
                            this._position = new Vector2(this.baseX[index1] + 32, 32 + index2 * 16);
                            chipFolder.chip.IconRender(dg, this._position, false, false, code, false);
                            this._rect = new Rectangle(304, 16 * (chipS.Reality - 1), 16, 16);
                            this._position = new Vector2(this.baseX[index1] + 112, 32 + index2 * 16);
                            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        }
                    }
                    if (this.nowscene != DataList.SCENE.move)
                    {
                        this._rect = new Rectangle(112 + 16 * this.cursolanime, 160, 16, 16);
                        this._position = new Vector2(this.baseX[index1] - 8, 32 + this.cursol[index1] * 16);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    }
                    Color white = Color.White;
                    int num1;
                    string name;
                    Color color2;
                    switch (index1)
                    {
                        case 0:
                            num1 = 183;
                            name = ShanghaiEXE.Translate("DataList.Standard");
                            color2 = Color.White;
                            break;
                        case 1:
                            num1 = 57;
                            name = ShanghaiEXE.Translate("DataList.Navi");
                            color2 = Color.FromArgb(183, 231, byte.MaxValue);
                            break;
                        case 2:
                            num1 = 9;
                            name = ShanghaiEXE.Translate("DataList.Dark");
                            color2 = Color.FromArgb(206, 111, 231);
                            break;
                        default:
                            num1 = 25;
                            name = ShanghaiEXE.Translate("DataList.PAdvance");
                            color2 = Color.White;
                            break;
                    }
                    float num2 = 104f / num1 * this.topchip[index1];
                    this._rect = new Rectangle(176, 168, 8, 8);
                    this._position = new Vector2(this.baseX[index1] + 128, 32f + num2);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    AllBase.NAME[] nameArray1 = this.Nametodata(name);
                    vector2_2 = new Vector2(this.baseX[index1] + 8, 10f);
                    foreach (var data in ((IEnumerable<AllBase.NAME>)nameArray1).Select((v, j) => new
                    {
                        v,
                        j
                    }))
                    {
                        this._rect = new Rectangle((int)data.v * 8, 88, 8, 16);
                        this._position = new Vector2(vector2_2.X + 8 * data.j, vector2_2.Y);
                        dg.DrawImage(dg, "font", this._rect, true, this._position, color2);
                    }
                }
            }
            if (this.nowscene != DataList.SCENE.fadein && this.nowscene != DataList.SCENE.fadeout)
                return;
            Color color3 = Color.FromArgb(this.Alpha, 0, 0, 0);
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color3);
        }

        private enum SCENE
        {
            fadein,
            select,
            move,
            fadeout,
        }

        private enum PAGE
        {
            normal,
            navi,
            dark,
            PA,
        }
    }
}
