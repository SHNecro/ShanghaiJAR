using MapEditor.Models.Elements.Enums;
using System;
using System.IO;

namespace MapEditor.Models
{
	public class MapMystery : MapObject
	{
        private int type;
        private RandomMystery baseMystery;
        private int flag;

        public int Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.SetValue(ref this.type, value);
                var newMystery = new MapMystery { StringValue = this.GetStringValue() };
                this.Pages = newMystery.Pages;
            }
        }
        public RandomMystery BaseMystery
        {
            get { return this.baseMystery; }
            set { this.SetValue(ref this.baseMystery, value); }
        }
        public int Flag
        {
            get { return this.flag; }
            set { this.SetValue(ref this.flag, value); }
        }

        protected override string GetStringValue()
        {
            var mystery = this.Type == 0 ? new RandomMystery { Category = 0, ID = 0, Data = 0, ItemKey = string.Empty } : this.BaseMystery;
            return string.Join("\r\n", new[]
            {
                $"MysteryID:{this.ID}",
                $"position:{this.X}:{this.Y}:{this.Level}",
                $"Mystery:{this.Type},{mystery.StringValue},{this.Flag}",
                "page:1",
                "startterms:Abutton",
                "type:1",
                "terms:",
                "move:",
                "speed:3",
                $"graphic:-2:0,{40 * this.Type},16,40,16",
                "hitrange:2:0:8:8",
                "hitform:circle",
                "event:",
                "end",
                "page:2",
                "startterms:Abutton",
                "type:0",
                "terms:",
                "move:",
                "speed:0",
                "graphic:-1:0,0,0,0,0",
                "hitrange:0:0:0:0",
                "hitform:square",
                "event:",
                "end"
            });
        }

        protected override void SetStringValue(string value)
        {
            base.SetStringValue(value);

            var newType = default(int);
            var mystery = default(RandomMystery);
            var newFlag = default(int);

            using (var reader = new StringReader(value))
            {
                string line;
                var lineNo = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    lineNo++;
                    if (lineNo == 3)
                    {
                        if (!this.Validate(line, "Missing mystery definition line 3.", l => l.StartsWith("Mystery:", StringComparison.InvariantCulture)))
                        {
                            break;
                        }

                        var mysterySections = line.Split(':');
                        if (!this.Validate(mysterySections, "Malformed mystery definition.", s => s.Length == 2))
                        {
                            break;
                        }

                        var mysteryEntries = mysterySections[1].Split(',');
                        if (!this.Validate(mysteryEntries, "Invalid number of parameters.", e => e.Length == 6))
                        {
                            break;
                        }

                        newType = (int)this.ParseEnumOrAddError<MysteryTypeNumber>(mysteryEntries[0]);
                        mystery = new RandomMystery
                        {
                            Category = this.ParseIntOrAddError(mysteryEntries[1]),
                            ID = this.ParseIntOrAddError(mysteryEntries[2]),
                            Data = this.ParseIntOrAddError(mysteryEntries[3]),
                            ItemKey = mysteryEntries[4]
                        };
                        newFlag = this.ParseIntOrAddError(mysteryEntries[5]);
                    }
                }
            }

            this.type = newType;
            this.OnPropertyChanged(nameof(this.Type));
            this.BaseMystery = mystery;
            this.Flag = newFlag;
        }
    }
}
