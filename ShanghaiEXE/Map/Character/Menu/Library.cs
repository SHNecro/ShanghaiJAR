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

        private int moveXOffset = 0;

        private int cursorFrame;

        public Library(MyAudio s, Player p, TopMenu t, SaveData save)
          : base(s, p, t, save)
        {
            this.UnknownChipNameText = ShanghaiEXE.Translate("DataList.UnknownChipNameText");

            this.libraryPages = new Dictionary<LibraryPageType, LibraryPage>();
            this.CreateLibraryPages();

            this.State = LibraryState.FadeIn;
        }

        public bool IsActive { get; set; }

        private LibraryState State { get; set; }

        private LibraryPageType CurrentPageType { get; set; }

        private LibraryPage CurrentPage => this.libraryPages[this.CurrentPageType];

        public override void UpDate()
        {
            switch (this.State)
            {
                case LibraryState.FadeIn:
                    if (this.Alpha > 0)
                    {
                        this.Alpha -= 51;
                    }
                    else
                    {
                        this.State = LibraryState.Active;
                    }
                    break;
                case LibraryState.FadeOut:
                    if (this.Alpha < 255)
                    {
                        this.Alpha += 51;
                    }
                    else
                    {
                        // TODO: CHANGE TO EXIT MENU
                        this.IsActive = false;
                        this.State = LibraryState.FadeIn;
                    }
                    break;
                case LibraryState.MoveLeft:
                    if (this.moveXOffset < 144)
                    {
                        this.moveXOffset += 16;
                    }
                    else
                    {
                        this.CurrentPageType = this.CurrentPage.LeftPage.Value;
                        this.State = LibraryState.Active;
                    }
                    break;
                case LibraryState.MoveRight:
                    if (this.moveXOffset > 0)
                    {
                        this.moveXOffset -= 16;
                    }
                    else
                    {
                        this.CurrentPageType = this.CurrentPage.RightPage.Value;
                        this.State = LibraryState.Active;
                    }
                    break;
                case LibraryState.Active:
                    this.Control();
                    break;
            }
            if (this.frame % 10 == 0)
            {
                this.cursorFrame = (this.cursorFrame + 1) % 3;
            }
            this.FlamePlus();
        }

        public override void Render(IRenderer dg)
        {
            // Draw background/UI
            this._rect = new Rectangle(0, 624, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);

            // Draw completion progress ex. 23/64
            var completionTextColor = this.CurrentPage.IsComplete ? Color.Cyan : Color.White;
            var maxChipsBlockText = this.ChangeCount(this.CurrentPage.Count);
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
            if (this.CurrentPage.CurrentChip.IsSeen)
            {
                this._position = new Vector2(24f, 32f);
                this.CurrentPage.CurrentChip.Chip.GraphicsRender(dg, this._position, this.CurrentPage.CurrentChip.CurrentCodeNumber, true, true);
                var chipInformation = this.CurrentPage.CurrentChip.Chip.information;
                for (var line = 0; line < chipInformation.Length; line++)
                {
                    Vector2 point = new Vector2(10f, 102 + 16 * line);
                    dg.DrawMiniText(chipInformation[line], point, Color.Black);
                }
            }

            var isMovingPage = this.State == LibraryState.MoveLeft || this.State == LibraryState.MoveRight;
            var pagesToDraw = isMovingPage ? 2 : 1;
            for (var page = 0; page < pagesToDraw; page++)
            {
                var currentMoveXOffset = page == 0 ? 0 : this.moveXOffset;
                var drawnPageType = this.CurrentPageType;
                if (this.State == LibraryState.MoveLeft)
                {
                    drawnPageType = page == 0 ? this.CurrentPageType : this.CurrentPage.LeftPage.Value;
                }
                else if (this.State == LibraryState.MoveRight)
                {
                    drawnPageType = page == 0 ? this.CurrentPageType : this.CurrentPage.RightPage.Value;
                }
                var drawnPage = this.libraryPages[drawnPageType];

                // Draw library page background
                this._rect = new Rectangle(360, 0, 136, 144);
                this._position = new Vector2(96 + currentMoveXOffset, 8f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);

                // Draw library entries
                var topChipIndex = drawnPage.CurrentTopIndex;
                var chipsOnPage = Math.Min(drawnPage.Count, 7);
                for (var visibleRowIndex = 0; visibleRowIndex < chipsOnPage; visibleRowIndex++)
                {
                    var rowChipEntry = drawnPage.Chips[topChipIndex + visibleRowIndex];

                    if (drawnPageType != LibraryPageType.PA)
                    {
                        // Draw entry chip ID (or displayed ID)
                        var chipIdBlockText = this.Nametodata(rowChipEntry.ChipDisplayNumber);
                        var chipIdLocation = new Vector2(96 + currentMoveXOffset + 24 - (chipIdBlockText.Length - 1) * 8, 32 + visibleRowIndex * 16);
                        for (int index = 0; index < chipIdBlockText.Length; ++index)
                        {
                            this._rect = new Rectangle((int)chipIdBlockText[index] * 8, 16, 8, 16);
                            this._position = new Vector2(chipIdLocation.X + index * 8, chipIdLocation.Y);
                            dg.DrawImage(dg, "font", this._rect, true, this._position, Color.SkyBlue);
                        }
                    }

                    // Draw entry chip name
                    var chipNameBlockText = this.Nametodata(rowChipEntry.IsSeen ? rowChipEntry.Chip.name : UnknownChipNameText);
                    var nameOffset = drawnPageType == LibraryPageType.PA ? 16 : 48;
                    var chipNameLocation = new Vector2(96 + currentMoveXOffset + nameOffset, 32 + visibleRowIndex * 16);
                    for (int index = 0; index < chipNameBlockText.Length; ++index)
                    {
                        this._rect = new Rectangle((int)chipNameBlockText[index] * 8, 16, 8, 16);
                        this._position = new Vector2(chipNameLocation.X + index * 8, chipNameLocation.Y);
                        dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                    }

                    if (rowChipEntry.IsSeen && drawnPageType != LibraryPageType.PA)
                    {
                        // Draw entry chip icon
                        this._position = new Vector2(96 + currentMoveXOffset + 32, 32 + visibleRowIndex * 16);
                        rowChipEntry.Chip.IconRender(dg, this._position, false, false, rowChipEntry.CurrentCodeNumber, false);

                        // Draw entry chip rarity
                        this._rect = new Rectangle(304, 16 * (rowChipEntry.Chip.reality - 1), 16, 16);
                        this._position = new Vector2(96 + currentMoveXOffset + 112, 32 + visibleRowIndex * 16);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    }
                }

                if (!isMovingPage)
                {
                    // Draw cursor
                    var cursorPosition = this.CurrentPage.CurrentIndex - this.CurrentPage.CurrentTopIndex;
                    this._rect = new Rectangle(112 + 16 * this.cursorFrame, 160, 16, 16);
                    this._position = new Vector2(96 + currentMoveXOffset - 8, 32 + cursorPosition * 16);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                }

                // Draw scrollbar
                var scrollPosition = drawnPage.Count < 7 ? 104 : 104 * ((float)drawnPage.CurrentTopIndex / (drawnPage.Count - 7));
                this._rect = new Rectangle(176, 168, 8, 8);
                this._position = new Vector2(96 + currentMoveXOffset + 128, 32f + scrollPosition);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);

                // Draw library page title
                var pageTitleBlockText = this.Nametodata(drawnPage.Title);
                var pageTitleLocation = new Vector2(96 + currentMoveXOffset + 8, 10f);
                for (int index = 0; index < pageTitleBlockText.Length; ++index)
                {
                    this._rect = new Rectangle((int)pageTitleBlockText[index] * 8, 88, 8, 16);
                    this._position = new Vector2(pageTitleLocation.X + index * 8, pageTitleLocation.Y);
                    dg.DrawImage(dg, "font", this._rect, true, this._position, drawnPage.TitleColor);
                }
            }

            // Fade in/out
            if (this.State == LibraryState.FadeIn || this.State == LibraryState.FadeOut)
            {
                var fadeColor = Color.FromArgb(this.Alpha, 0, 0, 0);
                this._rect = new Rectangle(0, 0, 240, 160);
                this._position = new Vector2(0.0f, 0.0f);
                dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, fadeColor);
            }
        }

        private void Control()
        {
            if (Input.IsPress(Button._A))
            {
                var currentCode = this.CurrentPage.CurrentChip.Chip.code[this.CurrentPage.CurrentChip.CurrentCodeNumber];
                for (var i = 0; i < 4; i++)
                {
                    this.CurrentPage.CurrentChip.CurrentCodeNumber = (this.CurrentPage.CurrentChip.CurrentCodeNumber + 1) % 4;
                    var newCode = this.CurrentPage.CurrentChip.Chip.code[this.CurrentPage.CurrentChip.CurrentCodeNumber];
                    if (newCode != currentCode)
                    {
                        break;
                    }
                }
                this.sound.PlaySE(MyAudio.SOUNDNAMES.decide);
            }
            if (Input.IsPress(Button._B))
            {
                this.sound.PlaySE(MyAudio.SOUNDNAMES.cancel);
                this.State = LibraryState.FadeOut;
            }
            if (Input.IsPress(Button._R) || (this.waittime <= 0 && Input.IsPush(Button._R)))
            {
                var proposedIndex = Math.Min(this.CurrentPage.CurrentIndex + 7, this.CurrentPage.Max);
                if (proposedIndex != this.CurrentPage.CurrentIndex)
                {
                    this.CurrentPage.CurrentIndex = proposedIndex;
                    this.CurrentPage.CurrentTopIndex = Math.Min(this.CurrentPage.CurrentTopIndex + 7, this.CurrentPage.Count - 7);
                    this.CurrentPage.CurrentTopIndex = Math.Max(this.CurrentPage.CurrentTopIndex, 0);
                    this.sound.PlaySE(MyAudio.SOUNDNAMES.movecursol);
                }

                this.waittime = Input.IsPress(Button._R) ? 10 : 4;
            }
            if (Input.IsPress(Button._L) || (this.waittime <= 0 && Input.IsPush(Button._L)))
            {
                var proposedIndex = Math.Max(this.CurrentPage.CurrentIndex - 7, 0);
                if (proposedIndex != this.CurrentPage.CurrentIndex)
                {
                    this.CurrentPage.CurrentIndex = proposedIndex;
                    this.CurrentPage.CurrentTopIndex = Math.Max(this.CurrentPage.CurrentTopIndex - 7, 0);
                    this.sound.PlaySE(MyAudio.SOUNDNAMES.movecursol);
                }

                this.waittime = Input.IsPress(Button._L) ? 10 : 4;
            }
            if (Input.IsPress(Button.Up) || (this.waittime <= 0 && Input.IsPush(Button.Up)))
            {
                if (this.CurrentPage.CurrentIndex > 0)
                {
                    this.CurrentPage.CurrentIndex--;
                    this.CurrentPage.CurrentTopIndex = Math.Min(this.CurrentPage.CurrentTopIndex, this.CurrentPage.CurrentIndex);
                    this.sound.PlaySE(MyAudio.SOUNDNAMES.movecursol);
                }

                this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
            }
            if (Input.IsPress(Button.Down) || (this.waittime <= 0 && Input.IsPush(Button.Down)))
            {
                if (this.CurrentPage.CurrentIndex < this.CurrentPage.Max)
                {
                    this.CurrentPage.CurrentIndex++;
                    this.CurrentPage.CurrentTopIndex = Math.Max(this.CurrentPage.CurrentTopIndex, this.CurrentPage.CurrentIndex - 6);
                    this.sound.PlaySE(MyAudio.SOUNDNAMES.movecursol);
                }

                this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
            }
            if (this.waittime > 0)
            {
                this.waittime--;
            }
            if (Input.IsPush(Button.Left) && this.CurrentPage.LeftPage != null)
            {
                this.moveXOffset = 0;
                this.State = LibraryState.MoveLeft;
                this.sound.PlaySE(MyAudio.SOUNDNAMES.menuopen);
            }
            if (Input.IsPush(Button.Right) && this.CurrentPage.RightPage != null)
            {
                this.moveXOffset = 144;
                this.State = LibraryState.MoveRight;
                this.sound.PlaySE(MyAudio.SOUNDNAMES.menuopen);
            }
            if (Input.IsPress(Button._Select))
            {
                // TODO: REMOVE
                this.IsActive = false;
                this.State = LibraryState.FadeIn;
            }
        }

        private void CreateLibraryPages()
        {
            var chipFolder = new ChipFolder(this.sound);
            this.libraryPages[LibraryPageType.Normal] = new LibraryPage
            {
                Chips = Enumerable.Range(1, NormalChipCount)
                    .Select(i => this.ChipEntryFromID(chipFolder, i)).ToList(),
                Title = ShanghaiEXE.Translate("DataList.Standard"),
                TitleColor = Color.White,
                LeftPage = null,
                RightPage = LibraryPageType.Navi
            };

            this.libraryPages[LibraryPageType.Navi] = new LibraryPage
            {
                Chips = Enumerable.Range(NormalChipCount + 1, NaviChipCount)
                    .Select(i => this.ChipEntryFromID(chipFolder, i)).ToList(),
                Title = ShanghaiEXE.Translate("DataList.Navi"),
                TitleColor = Color.FromArgb(183, 231, 255),
                CustomTextArea = new TextArea { Sprite = new Rectangle(272, 128, 88, 56), Position = new Vector2(8f, 96f) },
                LeftPage = LibraryPageType.Normal,
                RightPage = LibraryPageType.Dark
            };

            this.libraryPages[LibraryPageType.Dark] = new LibraryPage
            {
                Chips = Enumerable.Range(NormalChipCount + NaviChipCount + 1, DarkChipCount)
                    .Select(i => this.ChipEntryFromID(chipFolder, i)).ToList(),
                Title = ShanghaiEXE.Translate("DataList.Dark"),
                TitleColor = Color.FromArgb(206, 111, 231),
                CustomTextArea = new TextArea { Sprite = new Rectangle(272, 184, 88, 56), Position = new Vector2(8f, 96f) },
                LeftPage = LibraryPageType.Navi,
                RightPage = LibraryPageType.PA
            };

            this.libraryPages[LibraryPageType.PA] = new LibraryPage
            {
                Chips = Enumerable.Range(NormalChipCount + NaviChipCount + DarkChipCount + 1, PACount)
                    .Select(i => this.ChipEntryFromID(chipFolder, i)).ToList(),
                Title = ShanghaiEXE.Translate("DataList.PAdvance"),
                TitleColor = Color.White,
                CustomTextArea = new TextArea { Sprite = new Rectangle(760, 120, 88, 136), Position = new Vector2(8f, 16) },
                LeftPage = LibraryPageType.Dark,
                RightPage = LibraryPageType.Illegal
            };

            this.libraryPages[LibraryPageType.Illegal] = new LibraryPage
            {
                Chips = Enumerable.Range(NormalChipCount + NaviChipCount + DarkChipCount + 1 + PACount, PACount)
                    .Select(i => this.ChipEntryFromID(chipFolder, i)).ToList(),
                Title = ShanghaiEXE.Translate("DataList.Illegal"),
                TitleColor = Color.DarkRed,
                LeftPage = LibraryPageType.PA,
                RightPage = null
            };
        }

        private ChipEntry ChipEntryFromID(ChipFolder folder, int id)
        {
            var chipBase = folder.ReturnChip(id);
            var chipDisplayName = (chipBase is DammyChip) ? id : (int?)null;
            var chipIsSeen = !(chipBase is DammyChip) && this.savedata.datelist[id - 1];
            return new ChipEntry { IsSeen = chipIsSeen, Chip = chipBase, ChipDisplayNumber = $"{chipDisplayName ?? chipBase.number}" };
        }

        private enum LibraryState
        {
            FadeIn,
            FadeOut,
            MoveLeft,
            MoveRight,
            Active
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
            public TextArea CustomTextArea { get; set; }
            public string Title { get; set; }
            public Color TitleColor { get; set; }
            public LibraryPageType? LeftPage { get; set; }
            public LibraryPageType? RightPage { get; set; }

            public int CurrentIndex { get; set; }
            public int CurrentTopIndex { get; set; }

            public int Max => this.Count - 1;
            public int Count => this.Chips.Count;
            public int Seen => this.Chips.Count(c => c.IsSeen);
            public ChipEntry CurrentChip => this.Chips[this.CurrentIndex];
            public bool IsComplete => this.Seen == this.Count;
        }

        private class ChipEntry
        {
            public bool IsSeen { get; set; }
            public ChipBase Chip { get; set; }
            public string ChipDisplayNumber { get; set; }
            public int CurrentCodeNumber { get; set; }
        }

        private class TextArea
        {
            public Rectangle Sprite { get; set; }
            public Vector2 Position { get; set; }
        }
    }
}
