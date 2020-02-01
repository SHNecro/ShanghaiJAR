using NSChip;
using NSGame;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Collections.Generic;
using System.Drawing;

namespace NSMap.Character.Menu
{
    internal class Library : MenuBase
    {
        private readonly Dictionary<LibraryPageType, LibraryPage> libraryPages;

        public Library(MyAudio s, Player p, TopMenu t, SaveData save)
          : base(s, p, t, save)
        {
        }

        public bool IsActive { get; set; }

        private LibraryPageType CurrentPageType { get; set; }

        private LibraryPage CurrentPage => this.libraryPages[this.CurrentPageType];

        public override void Render(IRenderer dg)
        {
            // Draw background/UI
            this._rect = new Rectangle(0, 624, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);

            // Draw completion progress ex. 23/64
            var completionTextColor = this.CurrentPage.IsComplete ? Color.White : Color.Cyan;
            var maxChipsBlockText = this.ChangeCount(this.CurrentPage.Max);
            var seenChipsBlockText = this.ChangeCount(this.CurrentPage.Seen);

            var maxChipLocation = new Vector2(208 + 8 * maxChipsBlockText.Length, 0.0f);
            var seenChipsLocation = new Vector2(200f, 0.0f);

            for (int index = 0; index < maxChipsBlockText.Length; ++index)
            {
                this._rect = new Rectangle(maxChipsBlockText[index] * 8, 104, 8, 16);
                this._position = new Vector2(maxChipLocation.X - index * 8, maxChipLocation.Y);
                dg.DrawImage(dg, "font", this._rect, true, this._position, completionTextColor);
            }

            for (int index = 0; index < seenChipsBlockText.Length; ++index)
            {
                this._rect = new Rectangle(seenChipsBlockText[index] * 8, 104, 8, 16);
                this._position = new Vector2(seenChipsLocation.X - index * 8, seenChipsLocation.Y);
                dg.DrawImage(dg, "font", this._rect, true, this._position, completionTextColor);
            }

            // Draw hovered chip text background
            if (this.CurrentPage.CustomTextArea != null)
            {
                this._rect = this.CurrentPage.CustomTextArea.Sprite;
                this._position = this.CurrentPage.CustomTextArea.Position;
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }

            // Draw hovered chip details
            if (this.savedata.datelist[this.CurrentPage.CurrentChip.number])
            {
                this._position = new Vector2(24f, 32f);
                this.CurrentPage.CurrentChip.GraphicsRender(dg, this._position, this.CurrentPage.CurrentCodeNumber, true, true);
                var chipInformation = this.CurrentPage.CurrentChip.information;
                for (var line = 0; line < chipInformation.Length; line++)
                {
                    Vector2 point = new Vector2(10f, 102 + 16 * line);
                    dg.DrawMiniText(chipInformation[line], point, Color.Black);
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
            {
                return;
            }
            Color color3 = Color.FromArgb(this.Alpha, 0, 0, 0);
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color3);
        }

        private enum LibraryPageType
        {
            Normal,
            Navi,
            Dark,
            PA,
            Illegal
        }

        private class LibraryPage
        {
            public int Max { get; set; }
            public int Seen { get; set; }
            public ChipBase CurrentChip { get; set; }
            public int CurrentCodeNumber { get; set; }
            public TextArea CustomTextArea { get; set; }
            public bool IsComplete => this.Seen >= this.Max;
        }

        private class TextArea
        {
            public Rectangle Sprite { get; set; }
            public Vector2 Position { get; set; }
        }
    }
}
