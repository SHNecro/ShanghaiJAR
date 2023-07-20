using MapEditor.Core;
using MapEditor.Models.Elements;
using System.Collections.Generic;
using System.Drawing;

namespace MapEditor.Models
{
    public class Enemy : StringRepresentation, ITranslatedModel
    {
        private int id;
        private int rank;
        private int x;
        private int y;
        private Chip[] chips;
        private int hp;
        private string name;
        private string nameKey;

        public int ID
        {
            get
            {
                return id;
            }

            set
            {
                this.SetValue(ref this.id, value);

                this.OnPropertyChanged(nameof(this.IsNormalNavi));

                if (value == 0)
                {
                    this.HP = 1;
                    this.Chip1 = null;
                    this.Chip2 = null;
                    this.Chip3 = null;
                    this.Chip4 = null;
                    this.Chip5 = null;
                    this.Name = null;
                    this.NameKey = string.Empty;
                }
                else
                {
                    this.RefreshEnemyDefinition();
                }
            }
        }

        public int Rank
        {
            get
            {
                return rank;
            }

            set
            {
                this.SetValue(ref this.rank, value);
                this.RefreshEnemyDefinition();
            }
        }

        public Point Position
        {
            get
            {
                return new Point(this.X, this.Y);
            }

            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        public int X
        {
            get
            {
                return x;
            }

            set
            {
                this.SetValue(ref this.x, value);
                this.OnPropertyChanged(nameof(this.Position));
                this.RefreshEnemyDefinition();
            }
        }

        public int Y
        {
            get
            {
                return y;
            }

            set
            {
                this.SetValue(ref this.y, value);
                this.OnPropertyChanged(nameof(this.Position));
                this.RefreshEnemyDefinition();
            }
        }

        public bool IsNormalNavi => this.ID == Constants.NormalNaviID;

        public Chip Chip5
        {
            get
            {
                return chips[4];
            }

            set
            {
                this.SetValue(ref this.chips[4], value);
                this.RefreshEnemyDefinition();
            }
        }
        public Chip Chip4
        {
            get
            {
                return chips[3];
            }

            set
            {
                this.SetValue(ref this.chips[3], value);
                this.RefreshEnemyDefinition();
            }
        }
        public Chip Chip3
        {
            get
            {
                return chips[2];
            }

            set
            {
                this.SetValue(ref this.chips[2], value);
                this.RefreshEnemyDefinition();
            }
        }
        public Chip Chip2
        {
            get
            {
                return chips[1];
            }

            set
            {
                this.SetValue(ref this.chips[1], value);
                this.RefreshEnemyDefinition();
            }
        }
        public Chip Chip1
        {
            get
            {
                return chips[0];
            }

            set
            {
                this.SetValue(ref this.chips[0], value);
                this.RefreshEnemyDefinition();
            }
        }
        public int HP
        {
            get
            {
                return hp;
            }

            set
            {
                this.SetValue(ref this.hp, value);
                this.RefreshEnemyDefinition();
            }
        }
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                this.SetValue(ref this.name, value);
                this.RefreshEnemyDefinition();
            }
        }
        public string NameKey
        {
            get
            {
                return this.nameKey;
            }

            set
            {
                this.SetValue(ref this.nameKey, value);
                this.RefreshEnemyDefinition();
            }
        }

        public EnemyDefinition EnemyDefinition { get; set; }

        public void RefreshEnemyDefinition()
        {
            try
			{
				if (this.IsNormalNavi)
				{
					this.EnemyDefinition = EnemyDefinition.GetEnemyDefinition(this.ID, this.X, this.Y, this.Rank, this.HP, this.Chip5.ID, this.Chip4.ID, this.Chip3.ID, this.NameKey);
				}
				else
				{
					this.EnemyDefinition = EnemyDefinition.GetEnemyDefinition(this.ID, this.X, this.Y, this.Rank);
				}
			}
            catch
            {

            }

            if (this.EnemyDefinition != null)
            {
                this.chips = this.EnemyDefinition.Chips;

                if (this.IsNormalNavi)
                {
                    foreach (var c in this.chips)
                    {
                        c.CodeNumber = null;
                    }
                }

                this.hp = this.EnemyDefinition.HP;
                this.name = this.EnemyDefinition.Name;
                this.nameKey = this.EnemyDefinition.NameKey;

                this.OnPropertyChanged(nameof(this.Chip1));
                this.OnPropertyChanged(nameof(this.Chip2));
                this.OnPropertyChanged(nameof(this.Chip3));
                this.OnPropertyChanged(nameof(this.Chip4));
                this.OnPropertyChanged(nameof(this.Chip5));
                this.OnPropertyChanged(nameof(this.HP));
                this.OnPropertyChanged(nameof(this.Name));
                this.OnPropertyChanged(nameof(this.NameKey));
            }
        }

        public void RefreshTranslation()
        {
            this.RefreshEnemyDefinition();
        }

        protected override string GetStringValue()
        {
            var elements = new List<object>
            {
                this.ID,
                this.Rank,
                this.X,
                this.Y
            };
            if (this.IsNormalNavi)
            {
                elements.AddRange(new object[]
                {
                    this.Chip1.ID,
                    this.Chip2.ID,
                    this.Chip3.ID,
                    this.HP,
                    this.NameKey
                });
            }
            else
            {
                elements.AddRange(new object[]
                {
                    1,
                    1,
                    1,
                    0,
                    string.Empty
                });
            }

            return string.Join(":", elements);
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            this.Validate(entries, "Malformed enemy specification.", e => e.Length == 4 || e.Length == 9);

            var newId = this.ParseIntOrAddError(entries[0]);
            var newRank = this.ParseIntOrAddError(entries[1]);
			this.Validate(newRank, () => this.Rank, "Negative rank.", rankVal => rankVal >= 0);
			var newX = this.ParseIntOrAddError(entries[2]);
			this.Validate(newX, () => this.X, "X position out of enemy area.", xPos => xPos >= 3 && xPos <= 5);
			var newY = this.ParseIntOrAddError(entries[3]);
			this.Validate(newY, () => this.Y, "Y position out of enemy area.", yPos => yPos >= 0 && yPos <= 2);

			Chip chip1, chip2, chip3;
            int newHP = 1;
            string newNameKey = string.Empty;
            chip1 = chip2 = chip3 = null;

            if (newId == Constants.NormalNaviID)
            {
                chip1 = new Chip { ID = this.ParseIntOrAddError(entries[4]), CodeNumber = null };
                chip2 = new Chip { ID = this.ParseIntOrAddError(entries[5]), CodeNumber = null };
                chip3 = new Chip { ID = this.ParseIntOrAddError(entries[6]), CodeNumber = null };
                newHP = this.ParseIntOrAddError(entries[7]);
                newNameKey = entries[8];
                this.Validate(newNameKey, () => this.NameKey, s => $"Name key \"{s}\" does not exist.", Constants.TranslationService.CanTranslate);
            }

            this.id = newId;
            this.x = newX;
            this.y = newY;
            this.rank = newRank;

            this.chips = new[] { chip1, chip2, chip3, chip2, chip1 };
            this.hp = newHP;
            this.nameKey = newNameKey;

            this.RefreshEnemyDefinition();
        }
    }
}
