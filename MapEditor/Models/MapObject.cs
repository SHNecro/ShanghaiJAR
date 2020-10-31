using MapEditor.Core;
using MapEditor.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;

namespace MapEditor.Models
{
    public abstract class MapObject : StringRepresentation
    {
        private string id;
        private int x;
        private int y;
        private int level;
        private MapEventPageCollection pages;

        public string ID
        {
            get { return this.id; }
            set { this.SetValue(ref this.id, value); }
        }

        public Point MapPosition
        {
            get
            {
                return new Point(this.X, this.Y);
            }
            set
            {
                this.X = value.X;
                this.Y = value.Y;
                this.OnPropertyChanged(nameof(this.X));
                this.OnPropertyChanged(nameof(this.Y));
            }
        }
        public int X
        {
            get { return this.x; }
            set { this.SetValue(ref this.x, value); }
        }
        public int Y
        {
            get { return this.y; }
            set { this.SetValue(ref this.y, value); }
        }
        public int Level
        {
            get { return this.level; }
            set { this.SetValue(ref this.level, value); }
        }

        public MapEventPageCollection Pages
        {
            get
            {
                return this.pages;
            }
            set
            {
                if (this.pages != null)
                {
                    this.pages.PropertyChanged -= this.OnPagesPropertyChanged;
                }
                value.PropertyChanged += this.OnPagesPropertyChanged;
                this.SetValue(ref this.pages, value);
            }
        }

        public static MapObject FromString(string value)
		{
			if (value.StartsWith("ID:", StringComparison.InvariantCulture))
			{
				return new MapEntity { StringValue = value };
			}
			else
			{
				return new MapMystery { StringValue = value };
			}
        }

        public void SetMapPosition(Point position, bool update = true)
        {
            if (update)
            {
                this.MapPosition = position;
            }
            else
            {
                this.x = position.X;
                this.y = position.Y;
            }
        }

        // GetStringValue implemented by MapEvent, MapMystery

        protected override void SetStringValue(string value)
        {
            string newId = null;
            string position = null;
			List<string> pageStrings = new List<string>();
			StringBuilder currentPage = null;
            using (var reader = new StringReader(value))
            {
                string line;
                var firstLine = true;
                while ((line = reader.ReadLine()) != null)
                {
                    if (firstLine)
                    {
                        var entries = line.Trim().Split(':');
                        if (!this.Validate(entries, "Invalid ID specification", e => e.Length == 2))
                        {
                            return;
                        }
                        newId = entries[1];
                        firstLine = false;
                    }
                    if (line.StartsWith("position:", StringComparison.InvariantCulture) && this.Validate(position, "Multiple position lines", posString => posString == null))
                    {
                        position = line.Trim();
                    }
                    else if (line.StartsWith("page:", StringComparison.InvariantCulture))
                    {
                        if (currentPage != null)
                        {
                            pageStrings.Add(currentPage.ToString());
                        }
                        currentPage = new StringBuilder();
                        currentPage.AppendLine(line);
                    }
					else if (currentPage != null)
					{
						currentPage.AppendLine(line);
					}
				}
            }
            if (this.Validate(currentPage, "Object has no pages.", p => p != null))
            {
                pageStrings.Add(currentPage.ToString());
            }

            this.Validate(position, "Missing position line.", p => p != null);

            int newX, newY, newZ;
            newX = newY = newZ = 0;
            var positionEntries = position.Split(':');
            if (this.Validate(positionEntries, "Malformed position line.", pe => pe.Length == 4))
            {
                newX = ParseIntOrAddError(positionEntries[1]);
                newY = ParseIntOrAddError(positionEntries[2]);
                newZ = ParseIntOrAddError(positionEntries[3]);
            }

            var wholePages = string.Join(Environment.NewLine + Environment.NewLine, pageStrings);
			var newPages = new MapEventPageCollection { StringValue = wholePages };
			this.Validate(newPages, "No valid event pages found", pgs => pgs.MapEventPages.Count > 0);

            this.ID = newId;
            this.X = newX;
            this.Y = newY;
            this.Level = newZ;

            this.Pages = newPages;
        }

        protected override ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors()
        {
            return this.UpdateChildErrorStack(Pages);
        }

        private void OnPagesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"Pages.{e.PropertyName}");
        }
    }
}
