using NSChip;
using NSGame;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSMap.Character.Menu
{
    internal class Library : MenuBase
    {
        private const int NormalChipCount = 190;
        private const int NaviChipCount = 64;
        private const int DarkChipCount = 16;
        private const int PACount = 32;

        private readonly string UnknownChipNameText;

        private readonly Dictionary<LibraryPageType, LibraryPage> libraryPages;

        private int moveXShift = 96;

        public Library(MyAudio s, Player p, TopMenu t, SaveData save)
          : base(s, p, t, save)
        {
            this.UnknownChipNameText = ShanghaiEXE.Translate("DataList.UnknownChipNameText");

            this.libraryPages = new Dictionary<LibraryPageType, LibraryPage>();
            this.CreateLibraryPages();
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
            var completionTextColor = this.CurrentPage.IsComplete ? Color.Cyan : Color.White;
            var maxChipsBlockText = this.ChangeCount(this.CurrentPage.Max);
            var seenChipsBlockText = this.ChangeCount(this.CurrentPage.Seen);

            var maxChipLocation = new Vector2(208 + 8 * maxChipsBlockText.Length, 0.0f);
            var seenChipsLocation = new Vector2(200f, 0.0f);

            for (int index = 0; index < maxChipsBlockText.Length; ++index)
            {
                this._rect = new Rectangle(maxChipsBlockText[index] * 8, 104, 8, DarkChipCount);
                this._position = new Vector2(maxChipLocation.X - index * 8, maxChipLocation.Y);
                dg.DrawImage(dg, "font", this._rect, true, this._position, completionTextColor);
            }

            for (int index = 0; index < seenChipsBlockText.Length; ++index)
            {
                this._rect = new Rectangle(seenChipsBlockText[index] * 8, 104, 8, DarkChipCount);
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
            if (this.CurrentPage.CurrentChip.IsSeen)
            {
                this._position = new Vector2(24f, 32f);
                this.CurrentPage.CurrentChip.Chip.GraphicsRender(dg, this._position, this.CurrentPage.CurrentCodeNumber, true, true);
                var chipInformation = this.CurrentPage.CurrentChip.Chip.information;
                for (var line = 0; line < chipInformation.Length; line++)
                {
                    Vector2 point = new Vector2(10f, 102 + DarkChipCount * line);
                    dg.DrawMiniText(chipInformation[line], point, Color.Black);
                }
            }

            // Draw library page background
            this._rect = new Rectangle(360, 0, 136, 144);
            this._position = new Vector2(this.moveXShift, 8f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);

            // Draw library entries
            var topChipIndex = Math.Min(this.CurrentPage.CurrentIndex, this.CurrentPage.Max - 7);
            for (var visibleRowIndex = 0; visibleRowIndex < 7; visibleRowIndex++)
            {
                var rowChipEntry = this.CurrentPage.Chips[topChipIndex + visibleRowIndex];

                // Draw entry chip ID (or displayed ID)
                var chipIdBlockText = this.Nametodata(rowChipEntry.ChipDisplayNumber);
                var chipIdLocation = new Vector2(this.moveXShift + 24, 32 + visibleRowIndex * 16);
                for (int index = 0; index < chipIdBlockText.Length; ++index)
                {
                    this._rect = new Rectangle((int)chipIdBlockText[index] * 8, 16, 8, 16);
                    this._position = new Vector2(chipIdLocation.X - index * 8, chipIdLocation.Y);
                    dg.DrawImage(dg, "font", this._rect, true, this._position, Color.SkyBlue);
                }

                // Draw entry chip name
                var chipNameBlockText = this.Nametodata(rowChipEntry.IsSeen ? rowChipEntry.Chip.name : UnknownChipNameText );
                var nameOffset = this.CurrentPageType == LibraryPageType.PA ? 16 : 48;
                var chipNameLocation = new Vector2(this.moveXShift + nameOffset, 32 + visibleRowIndex * 16);
                for (int index = 0; index < chipNameBlockText.Length; ++index)
                {
                    this._rect = new Rectangle((int)chipNameBlockText[index] * 8, 16, 8, 16);
                    this._position = new Vector2(chipNameLocation.X + index * 8, chipNameLocation.Y);
                    dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                }

                // Draw entry chip rarity
                if (this.CurrentPageType != LibraryPageType.PA)
                {
                    this._position = new Vector2(this.moveXShift + 32, 32 + visibleRowIndex * 16);
                    rowChipEntry.Chip.IconRender(dg, this._position, false, false, 0, false);
                    this._rect = new Rectangle(304, 16 * (rowChipEntry.Chip.reality - 1), 16, 16);
                    this._position = new Vector2(this.moveXShift + 112, 32 + visibleRowIndex * 16);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                }
            }

            /*
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
            */
        }

        private void CreateLibraryPages()
        {
            var chipFolder = new ChipFolder(this.sound);
            this.libraryPages[LibraryPageType.Normal] = new LibraryPage
            {
                Chips = Enumerable.Range(1, NormalChipCount)
                .Select(i => this.ChipEntryFromID(chipFolder, i)).ToList()
            };

            this.libraryPages[LibraryPageType.Navi] = new LibraryPage
            {
                Chips = Enumerable.Range(NormalChipCount + 1, NaviChipCount)
                .Select(i => this.ChipEntryFromID(chipFolder, i)).ToList()
            };

            this.libraryPages[LibraryPageType.Dark] = new LibraryPage
            {
                Chips = Enumerable.Range(NormalChipCount + NaviChipCount + 1, DarkChipCount)
                .Select(i => this.ChipEntryFromID(chipFolder, i)).ToList()
            };

            this.libraryPages[LibraryPageType.PA] = new LibraryPage
            {
                Chips = Enumerable.Range(NormalChipCount + NaviChipCount + DarkChipCount + 1, PACount)
                .Select(i => this.ChipEntryFromID(chipFolder, i)).ToList()
            };

            this.libraryPages[LibraryPageType.Illegal] = new LibraryPage
            {
                Chips = Enumerable.Range(NormalChipCount + NaviChipCount + DarkChipCount + 1 + PACount, PACount)
                .Select(i => this.ChipEntryFromID(chipFolder, i)).ToList()
            };
        }

        private ChipEntry ChipEntryFromID(ChipFolder folder, int id)
        {
            var chipBase = folder.ReturnChip(id);
            var chipDisplayName = (int?)null;
            return new ChipEntry { IsSeen = this.savedata.datelist[id], Chip = chipBase, ChipDisplayNumber = $"{chipDisplayName ?? chipBase.number}" };
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
            public List<ChipEntry> Chips { get; set; }
            public int CurrentIndex { get; set; }
            public int CurrentCodeNumber { get; set; }
            public TextArea CustomTextArea { get; set; }

            public int Max => this.Chips.Count;
            public int Seen => this.Chips.Count(c => c.IsSeen);
            public ChipEntry CurrentChip => this.Chips[this.CurrentIndex];
            public bool IsComplete => this.Seen >= this.Max;
        }

        private class ChipEntry
        {
            public bool IsSeen { get; set; }
            public ChipBase Chip { get; set; }
            public string ChipDisplayNumber { get; set; }
        }

        private class TextArea
        {
            public Rectangle Sprite { get; set; }
            public Vector2 Position { get; set; }
        }
    }
}
