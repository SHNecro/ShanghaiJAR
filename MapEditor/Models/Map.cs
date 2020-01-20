using MapEditor.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace MapEditor.Models
{
    public class Map : StringRepresentation
    {
        private Header header;
        private WalkableMap walkableMap;
        private RandomEncounterCollection randomEncounters;
        private RandomMysteryCollection randomMysteryData;
        private MapObjectCollection mapObjects;

        public Map()
        {
            this.Header = new Header();
            this.WalkableMap = new WalkableMap();
            this.RandomEncounters = new RandomEncounterCollection();
            this.RandomMysteryData = new RandomMysteryCollection();
            this.MapObjects = new MapObjectCollection();
        }

        public string Name { get; set; }

        public Header Header
        {
            get
            {
                return this.header;
            }

            set
            {
                if (this.header != null)
                {
                    this.header.PropertyChanged -= this.OnHeaderPropertyChanged;
                }
                value.PropertyChanged += this.OnHeaderPropertyChanged;
                this.SetValue(ref this.header, value);
            }
        }

        public WalkableMap WalkableMap
        {
            get
            {
                return this.walkableMap;
            }

            set
            {
                if (this.walkableMap != null)
                {
                    this.walkableMap.PropertyChanged -= this.OnWalkableMapPropertyChanged;
                }
                value.PropertyChanged += this.OnWalkableMapPropertyChanged;
                this.SetValue(ref this.walkableMap, value);
            }
        }

        public RandomEncounterCollection RandomEncounters
        {
            get
            {
                return this.randomEncounters;
            }

            set
            {
                if (this.randomEncounters != null)
                {
                    this.randomEncounters.PropertyChanged -= this.OnRandomEncountersPropertyChanged;
                }
                value.PropertyChanged += this.OnRandomEncountersPropertyChanged;
                this.SetValue(ref this.randomEncounters, value);
            }
        }

        public RandomMysteryCollection RandomMysteryData
        {
            get
            {
                return this.randomMysteryData;
            }

            set
            {
                if (this.randomMysteryData != null)
                {
                    this.randomMysteryData.PropertyChanged -= this.OnRandomMysteryDataPropertyChanged;
                }
                value.PropertyChanged += this.OnRandomMysteryDataPropertyChanged;
                this.SetValue(ref this.randomMysteryData, value);
            }
        }

        public MapObjectCollection MapObjects
        {
            get
            {
                return this.mapObjects;
            }

            set
            {
                if (this.mapObjects != null)
                {
                    this.mapObjects.PropertyChanged -= this.OnMapObjectsPropertyChanged;
                }
                value.PropertyChanged += this.OnMapObjectsPropertyChanged;
                this.SetValue(ref this.mapObjects, value);
            }
        }

        protected override string GetStringValue()
        {
            var encountersWithNewlineOrBlank = this.RandomEncounters.StringValue;
            if (!string.IsNullOrEmpty(encountersWithNewlineOrBlank))
            {
                encountersWithNewlineOrBlank += "\r\n";
            }

            return string.Join("\r\n", new[]
            {
                this.Header.StringValue,
                this.WalkableMap.StringValue,
                string.Empty,
                encountersWithNewlineOrBlank,
                this.RandomMysteryData.StringValue,
                string.Empty,
                this.MapObjects.StringValue
            }).TrimEnd() + "\r\n\r\n\r\n";
        }

        protected override void SetStringValue(string value)
        {
            // Read header
            var reader = new StringReader(value);
            var newHeader = new Header { StringValue = reader.ReadLine() };

            // Read walkable area
            var walkableMapLines = new List<string>();
            for (int lineNo = 0; lineNo < (newHeader.Levels * (newHeader.WalkableSize.Height + 1)) - 1; lineNo++)
            {
                walkableMapLines.Add(reader.ReadLine());
            }

            var newWalkableMap = new WalkableMap { StringValue = string.Join("\r\n", walkableMapLines) };

            reader.ReadLine();

            // Read encounters
            string battleLine;
            var battleLines = new List<string>();
            while (!string.IsNullOrWhiteSpace(battleLine = reader.ReadLine()))
            {
                battleLines.Add(battleLine);
            }
            var newRandomEncounters = new RandomEncounterCollection { StringValue = string.Join("\r\n", battleLines) };

            // Read GMD
            var newRandomMysteryData = new RandomMysteryCollection { StringValue = reader.ReadLine() };

            reader.ReadLine();

            // Read map objects
            var newMapObjects = new MapObjectCollection { StringValue = reader.ReadToEnd().TrimEnd() };
			
			this.AddChildErrors(null, new[] { newHeader });
			this.AddChildErrors(null, new[] { newWalkableMap });
			this.AddChildErrors(null, new[] { newRandomEncounters });
			this.AddChildErrors(null, new[] { newRandomMysteryData });
			this.AddChildErrors(null, new[] { newMapObjects });

			if (!this.HasErrors)
            {
                this.Header = newHeader;
                this.WalkableMap = newWalkableMap;
                this.RandomEncounters = newRandomEncounters;
                this.RandomMysteryData = newRandomMysteryData;
                this.MapObjects = newMapObjects;
            }
        }

        private void OnHeaderPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"Header.{e.PropertyName}");
        }

        private void OnWalkableMapPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"WalkableMap.{e.PropertyName}");
        }

        private void OnRandomEncountersPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"RandomEncounters.{e.PropertyName}");
        }

        private void OnRandomMysteryDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"RandomMysteryData.{e.PropertyName}");
        }

        private void OnMapObjectsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"MapObjects.{e.PropertyName}");
        }
    }
}
