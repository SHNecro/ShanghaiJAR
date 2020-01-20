using MapEditor.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MapEditor.Models
{
	public class WalkableMap : StringRepresentation
	{
        public WalkableMap()
        {
            this.WalkableTiles = new Dictionary<Tuple<int, int, int>, int>();
        }

		private Dictionary<Tuple<int, int, int>, int> WalkableTiles { get; set; }
        private int Depth { get; set; }
        private int Height { get; set; }
        private int Width { get; set; }

        public int this[int z, int y, int x]
        {
            get
            {
                return this.WalkableTiles.TryGetValue(new Tuple<int, int, int>(z, y, x), out int tile) ? tile : 0;
            }

            set
            {
                this.WalkableTiles[new Tuple<int, int, int>(z, y, x)] = value;
                this.OnPropertyChanged($"{x},{y},{z}");
            }
        }

        protected override string GetStringValue()
        {
            string createRowFunc(IEnumerable<int> tileList) => string.Join(",", tileList);
            string createLevelFunc(IEnumerable<string> rowList) => string.Join("\r\n", rowList);
            string createMapFunc(IEnumerable<string> levelList) => string.Join("\r\n\r\n", levelList);
            var walkableMap = new int[this.Depth][][];
            for (int z = 0; z < this.Depth; z++)
            {
                walkableMap[z] = new int[this.Height][];
                for (int y = 0; y < this.Height; y++)
                {
                    walkableMap[z][y] = new int[this.Width];
                    for (int x = 0; x < this.Width; x++)
                    {
                        walkableMap[z][y][x] = this.WalkableTiles[new Tuple<int, int, int>(z, y, x)];
                    }
                }
            }
            return createMapFunc(walkableMap.Select(levels => createLevelFunc(levels.Select(createRowFunc))));
        }

        protected override void SetStringValue(string value)
        {
            var levelStrings = value.Split(new[] { "\r\n\r\n", "\r\r", "\n\n" }, StringSplitOptions.None);
            var levelRowStrings = levelStrings.Select((level) => level.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)).ToArray();
            this.Depth = levelRowStrings.Length;
            for (int z = 0; z < this.Depth; z++)
            {
                var rowStrings = levelRowStrings[z];
                this.Height = this.SetOrThrowIfAlreadySet(z == 0, this.Height, rowStrings.Length);
                for (int y = 0; y < this.Height; y++)
                {
                    var tiles = rowStrings[y].Split(',').Select(s => this.ParseIntOrAddError(s)).ToArray();
                    this.Width = this.SetOrThrowIfAlreadySet(y == 0, this.Width, tiles.Length);
                    for (int x = 0; x < this.Width; x++)
                    {
                        this.WalkableTiles[new Tuple<int, int, int>(z, y, x)] = tiles[x];
                    }
                }
            }
        }

        private TSet SetOrThrowIfAlreadySet<TSet>(bool initialSet, TSet originalValue, TSet newValue)
            where TSet : IComparable
        {
            if (initialSet || originalValue.Equals(newValue))
            {
                return newValue;
            }
            else
            {
                throw new InvalidOperationException("Non-square walkable area.");
            }
        }
	}
}
