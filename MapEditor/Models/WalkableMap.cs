using MapEditor.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MapEditor.Models
{
	public class WalkableMap : StringRepresentation
    {
        private readonly Dictionary<Tuple<int, int, int>, int> walkableTiles;

        private readonly Header header;
        
        private int depth;
        private int height;
        private int width;

        public WalkableMap(Header header)
        {
            this.walkableTiles = new Dictionary<Tuple<int, int, int>, int>();

            this.header = header;
            this.header.PropertyChanged += HeaderPropertyChanged;

            this.width = this.header.WalkableWidth;
            this.height = this.header.WalkableHeight;
            this.depth = this.header.Levels;
        }

        public int this[int z, int y, int x]
        {
            get
            {
                return this.walkableTiles.TryGetValue(new Tuple<int, int, int>(z, y, x), out int tile) ? tile : 0;
            }

            set
            {
                this.walkableTiles[new Tuple<int, int, int>(z, y, x)] = value;
                this.OnPropertyChanged($"{x},{y},{z}");
            }
        }

        protected override string GetStringValue()
        {
            string createRowFunc(IEnumerable<int> tileList) => string.Join(",", tileList);
            string createLevelFunc(IEnumerable<string> rowList) => string.Join("\r\n", rowList);
            string createMapFunc(IEnumerable<string> levelList) => string.Join("\r\n\r\n", levelList);
            var walkableMap = new int[this.depth][][];
            for (int z = 0; z < this.depth; z++)
            {
                walkableMap[z] = new int[this.height][];
                for (int y = 0; y < this.height; y++)
                {
                    walkableMap[z][y] = new int[this.width];
                    for (int x = 0; x < this.width; x++)
                    {
                        int existingCell;
                        if (!this.walkableTiles.TryGetValue(new Tuple<int, int, int>(z, y, x), out existingCell))
                        {
                            existingCell = 0;
                        }

                        walkableMap[z][y][x] = existingCell;
                    }
                }
            }

            return createMapFunc(walkableMap.Select(levels => createLevelFunc(levels.Select(createRowFunc))));
        }

        protected override void SetStringValue(string value)
        {
            var levelStrings = value.Split(new[] { "\r\n\r\n", "\r\r", "\n\n" }, StringSplitOptions.None);
            var levelRowStrings = levelStrings.Select((level) => level.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)).ToArray();
            var foundDepth = levelRowStrings.Length;
            if (!this.Validate(foundDepth, "Depth mismatch, adjust header", d => d == this.depth))
            {
                return;
            }

            for (int z = 0; z < this.depth; z++)
            {
                var rowStrings = levelRowStrings[z];
                var foundHeight = rowStrings.Length;
                if (!this.Validate(foundHeight, "Height mismatch, adjust header", d => d == this.height))
                {
                    return;
                }

                for (int y = 0; y < this.height; y++)
                {
                    var tiles = rowStrings[y].Split(',').Select(s => this.ParseIntOrAddError(s)).ToArray();
                    var foundWidth = tiles.Length;
                    if (!this.Validate(foundWidth, "Width mismatch, adjust header", d => d == this.width))
                    {
                        return;
                    }

                    for (int x = 0; x < this.width; x++)
                    {
                        this.walkableTiles[new Tuple<int, int, int>(z, y, x)] = tiles[x];
                    }
                }
            }
        }

        private void HeaderPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Header.WalkableWidth):
                case nameof(Header.WalkableHeight):
                case nameof(Header.Levels):
                    this.width = this.header.WalkableWidth;
                    this.height = this.header.WalkableHeight;
                    this.depth = this.header.Levels;
                    this.SetStringValue(this.GetStringValue());
                    break;
            }
        }
    }
}
