﻿using MapEditor.Core;
using System.Collections.Generic;
using System.Drawing;

namespace MapEditor.Models
{
    public class Header : StringRepresentation
    {
        private int walkableWidth;
        private int walkableHeight;
        private int rendX;
        private int rendY;
        private int imageWidth;
        private int imageHeight;
        private string titleKey;
        private int floorHeight;
        private int levels;
        private int backgroundNumber;
        private int specialEncounterFlag;
        private int specialEncounterCount;
        private string imagePrefix;

        public Size WalkableSize
        {
            get
            {
                return new Size(this.WalkableWidth, this.WalkableHeight);
            }
            set
            {
                this.WalkableWidth = value.Width;
                this.WalkableHeight = value.Height;
            }
        }
        public int WalkableWidth { get => this.walkableWidth; set => this.SetValue(ref this.walkableWidth, value); }
        public int WalkableHeight { get => this.walkableHeight; set => this.SetValue(ref this.walkableHeight, value); }

        public Point RendOffset
        {
            get
            {
                return new Point(this.RendX, this.RendY);
            }
            set
            {
                this.RendX = value.X;
                this.RendY = value.Y;
            }
        }
        public int RendX { get => this.rendX; set => this.SetValue(ref this.rendX, value); }
        public int RendY { get => this.rendY; set => this.SetValue(ref this.rendY, value); }

        public Size ImageSize
        {
            get
            {
                return new Size(this.ImageWidth, this.ImageHeight);
            }
            set
            {
                this.ImageWidth = value.Width;
                this.ImageHeight = value.Height;
            }
        }
        public int ImageWidth { get => this.imageWidth; set => this.SetValue(ref this.imageWidth, value); }
        public int ImageHeight { get => this.imageHeight; set => this.SetValue(ref this.imageHeight, value); }

        public string TitleKey { get => this.titleKey; set => this.SetValue(ref this.titleKey, value); }
        public int FloorHeight { get => this.floorHeight; set => this.SetValue(ref this.floorHeight, value); }
        public int Levels
        {
            get
            {
                return levels;
            }

            set
            {
                this.SetValue(ref levels, value);
                this.OnPropertyChanged(nameof(this.ImagePrefix));
            }
        }
        public int BackgroundNumber { get => this.backgroundNumber; set => this.SetValue(ref this.backgroundNumber, value); }
        public int SpecialEncounterFlag { get => this.specialEncounterFlag; set => this.SetValue(ref this.specialEncounterFlag, value); }
        public int SpecialEncounterCount { get => this.specialEncounterCount; set => this.SetValue(ref this.specialEncounterCount, value); }
        public string ImagePrefix
        {
            get
            {
                return imagePrefix;
            }

            set
            {
                this.SetValue(ref imagePrefix, value);
                this.OnPropertyChanged(nameof(this.Levels));
            }
        }

        public Point Offset => new Point(
            (int)(this.RendOffset.X + (this.ImageSize.Width - 240.0) / 2),
            (int)(this.RendOffset.Y + (this.ImageSize.Height - 160) / 2)
        );

        protected override string GetStringValue()
        {
            return string.Join(",", new List<object> {
                this.WalkableSize.Width,
                this.WalkableSize.Height,
                this.RendOffset.X,
                this.RendOffset.Y,
                this.ImageSize.Width,
                this.ImageSize.Height,
                this.TitleKey,
                this.FloorHeight,
                this.Levels,
                this.BackgroundNumber,
                this.SpecialEncounterFlag,
                this.SpecialEncounterCount,
                this.ImagePrefix,
                0
            });
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(',');
            if (!this.Validate(entries, "Invalid number of parameters.", e => e.Length == 14))
            {
                return;
            }

            var walkSize = new Size(this.ParseIntOrAddError(entries[0]), this.ParseIntOrAddError(entries[1]));
            this.Validate(walkSize, "Invalid walkable size (1 - 150).", s => s.Width > 0 && s.Height > 0 && s.Width <= 150 && s.Height <= 150);

            var rendOffset = new Point(this.ParseIntOrAddError(entries[2]), this.ParseIntOrAddError(entries[3]));
            var imageSize = new Size(this.ParseIntOrAddError(entries[4]), this.ParseIntOrAddError(entries[5]));
            this.Validate(imageSize, "Non-positive image size.", s => s.Width > 0 && s.Height > 0);

            var newTitleKey = entries[6];
            this.Validate(newTitleKey, "Title key does not exist.", k => Constants.TranslationService.CanTranslate(k));

            var newFloorHeight = this.ParseIntOrAddError(entries[7]);
            this.Validate(newFloorHeight, "Non-positive floor height.", h => h >= 0);
            this.Validate(newFloorHeight, "Floor height must be multiple of 16 (1 walkable tile distance).", h => h % 16 == 0);

            var newLevels = this.ParseIntOrAddError(entries[8]);
            this.Validate(newLevels, "Invalid number of levels (1 - 5).", l => l > 0 && l <= 5);

            var newBackgroundNumber = this.ParseIntOrAddError(entries[9]);

            var newSpecialEncounterFlag = this.ParseIntOrAddError(entries[10]);
            var newSpecialEncounterCount = this.ParseIntOrAddError(entries[11]);
            this.Validate(newSpecialEncounterCount, "Negative special encounters count.", l => l >= 0);

            var newImagePrefix = entries[12];
            //if (Constants.TextureLoadStrategy != null)
            //{
            //    this.Validate(newImagePrefix, $"Missing map images {newImagePrefix}(1 - {2 * newLevels - 1}).", s =>
            //    {
            //        var allExist = true;
            //        for (int i = 1; i < newLevels * 2; i++)
            //        {
            //            allExist &= Constants.TextureLoadStrategy.CanProvideTexture($"{newImagePrefix}{i}");
            //        }
            //        return allExist;
            //    });
            //}

            if (!this.HasErrors)
            {
                this.WalkableSize = walkSize;
                this.RendOffset = rendOffset;
                this.ImageSize = imageSize;
                this.TitleKey = newTitleKey;
                this.FloorHeight = newFloorHeight;
                this.Levels = newLevels;
                this.BackgroundNumber = newBackgroundNumber;
                this.SpecialEncounterFlag = newSpecialEncounterFlag;
                this.SpecialEncounterCount = newSpecialEncounterCount;
                this.ImagePrefix = newImagePrefix;
            }
        }
    }
}
