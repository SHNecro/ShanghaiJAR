using MapEditor.Core;
using MapEditor.ExtensionMethods;
using MapEditor.Rendering;
using MapEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Panel = NSBattle.Panel;

namespace MapEditor.Models
{
    public class RandomEncounter : StringRepresentation
    {
        private int panelPatternNumber;
        private Enemy selectedEnemy;
        private Enemy enemy1;
        private Enemy enemy2;
        private Enemy enemy3;
        private Panel.PANEL primaryPanel;
        private Panel.PANEL secondaryPanel;
        private bool isTutorial;
        private bool isChipDropped;
        private bool isEscapable;
        private bool isGameEnding;
        private string backgroundMusic;
        private int backgroundNumber;

        public RandomEncounter()
        {
            // TODO: MEMORY LEAK
            // However, would require propagating Dispose() all the way down Map, which would have been nice to do at the very start
            if (BGMDataViewModel.BGMDefinitions != null)
            {
                BGMDataViewModel.BGMDefinitions.CollectionChanged += (sender, args) => { this.OnPropertyChanged(nameof(this.BackgroundMusic)); };
            }
        }

        public Enemy Enemy1
        {
            get
            {
                return this.enemy1;
            }
            set
            {
                if (this.Enemy1 != null)
                {
                    this.Enemy1.PropertyChanged -= this.Enemy1PropertyChanged;
                }
                this.SetValue(ref this.enemy1, value);
                this.Enemy1.PropertyChanged += this.Enemy1PropertyChanged;
                this.OnPropertyChanged(nameof(this.Enemies));
            }
        }

        public Enemy Enemy2
        {
            get
            {
                return this.enemy2;
            }
            set
            {
                if (this.Enemy2 != null)
                {
                    this.Enemy2.PropertyChanged -= this.Enemy2PropertyChanged;
                }
                this.SetValue(ref this.enemy2, value);
                this.Enemy2.PropertyChanged += this.Enemy2PropertyChanged;
                this.OnPropertyChanged(nameof(this.Enemies));
            }
        }
        public Enemy Enemy3
        {
            get
            {
                return this.enemy3;
            }
            set
            {
                if (this.Enemy3 != null)
                {
                    this.Enemy3.PropertyChanged -= this.Enemy3PropertyChanged;
                }
                this.SetValue(ref this.enemy3, value);
                this.Enemy3.PropertyChanged += this.Enemy3PropertyChanged;
                this.OnPropertyChanged(nameof(this.Enemies));
            }
        }
        public Panel.PANEL PrimaryPanel
        {
            get { return this.primaryPanel; }
            set { this.SetValue(ref this.primaryPanel, value); }
        }
        public Panel.PANEL SecondaryPanel
        {
            get { return this.secondaryPanel; }
            set { this.SetValue(ref this.secondaryPanel, value); }
        }
        public int PanelPatternNumber
        {
            get
            {
                return this.panelPatternNumber;
            }

            set
            {
                this.SetValue(ref this.panelPatternNumber, value);
                this.Panels = NSShanghaiEXE.Common.Constants.PanelLayouts[this.PanelPatternNumber];
            }
        }
        public bool IsTutorial
        {
            get { return this.isTutorial; }
            set { this.SetValue(ref this.isTutorial, value); }
        }
        public bool IsChipDropped
        {
            get { return this.isChipDropped; }
            set { this.SetValue(ref this.isChipDropped, value); }
        }
        public bool IsEscapable
        {
            get { return this.isEscapable; }
            set { this.SetValue(ref this.isEscapable, value); }
        }

        public bool IsGameEnding
        {
            get { return this.isGameEnding; }
            set { this.SetValue(ref this.isGameEnding, value); }
        }
        public string BackgroundMusic
        {
            get
            {
                return this.backgroundMusic;
            }
            set
            {
                if (value != null)
                {
                    this.SetValue(ref this.backgroundMusic, value);
                }
            }
        }
        public int BackgroundNumber
        {
            get { return this.backgroundNumber; }
            set { this.SetValue(ref this.backgroundNumber, value); }
        }


        public bool IsLongForm { get; set; }
        public byte[,] Panels { get; set; }

        public Enemy SelectedEnemy
        {
            get
            {
                return this.selectedEnemy ?? this.GetFirstEnemyOrDefault();
            }

            set
            {
                var setValue = value ?? this.GetFirstEnemyOrDefault();
                this.SetValue(ref this.selectedEnemy, setValue);
                EncounterRenderer.UpdateSelectedEnemy();
            }
        }

        public Enemy[] Enemies => new[] { this.Enemy1, this.Enemy2, this.Enemy3 };

        protected override string GetStringValue()
        {
            var elements = new List<object> {
                "battle",
                this.Enemy1.StringValue,
                this.Enemy2.StringValue,
                this.Enemy3.StringValue,
                (int)this.PrimaryPanel,
                (int)this.SecondaryPanel,
                this.PanelPatternNumber,
                this.IsTutorial ? "True" : "False",
                this.IsChipDropped ? "True" : "False",
                this.IsEscapable ? "True" : "False"
            };

            if (this.IsLongForm)
            {
                elements.AddRange(new[] {
                    this.IsGameEnding ? "True" : "False",
                    this.BackgroundMusic,
                    this.BackgroundNumber.ToString(),
                });
            }

            return string.Join(":", elements);
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            var isLongForm = entries.Length == 36 || entries.Length == 37;
            Enemy newEnemy1, newEnemy2, newEnemy3;
            int offset;
            if (isLongForm)
            {
                newEnemy1 = new Enemy { StringValue = string.Join(":", entries.Skip(1).Take(9)) };
                newEnemy2 = new Enemy { StringValue = string.Join(":", entries.Skip(10).Take(9)) };
                newEnemy3 = new Enemy { StringValue = string.Join(":", entries.Skip(19).Take(9)) };
                offset = 28;
            }
            else if (entries.Length == 21)
            {
                newEnemy1 = new Enemy { StringValue = string.Join(":", entries.Skip(1).Take(4)) };
                newEnemy2 = new Enemy { StringValue = string.Join(":", entries.Skip(5).Take(4)) };
                newEnemy3 = new Enemy { StringValue = string.Join(":", entries.Skip(9).Take(4)) };
                offset = 13;
            }
            else
            {
                this.Validate(entries, "Invalid number of parameters.", e => e.Length == 37 || e.Length == 19);
                return;
            }

            var newPrimaryPanelInt = this.ParseIntOrAddError(entries[offset]);
            this.Validate(newPrimaryPanelInt, () => (int)this.PrimaryPanel, i => $"Invalid primary panel type {i} (0 - 9).", ppi => ppi >= 0 && ppi <= 9);

            var newSecondaryPanelInt = this.ParseIntOrAddError(entries[offset + 1]);
            this.Validate(newSecondaryPanelInt, () => (int)this.SecondaryPanel, i => $"Invalid secondary panel type {i} (0 - 9).", spi => spi >= 0 && spi <= 9);

            var newPanelPatternNumber = this.ParseIntOrAddError(entries[offset + 2]);
            this.Validate(newPanelPatternNumber, () => this.PanelPatternNumber, i => $"Invalid panel pattern {i} (0 - 21).", ppn => ppn < NSShanghaiEXE.Common.Constants.PanelLayouts.Count);

            var newIsTutorial = this.ParseBoolOrAddError(entries[offset + 3]);
            var newIsChipDropped = this.ParseBoolOrAddError(entries[offset + 4]);
            var newIsEscapable = this.ParseBoolOrAddError(entries[offset + 5]);

            var newIsGameEnding = !isLongForm || this.ParseBoolOrAddError(entries[offset + 6]);
            var newBackgroundMusic = isLongForm ? entries[offset + 7] : "VSvirus";

            // Not used by random encounters
            var newBackgroundNumber = isLongForm ? (offset + 8 < entries.Length ? this.ParseIntOrAddError(entries[offset + 8]) : 0) : 0;

            this.Enemy1 = newEnemy1;
            this.Enemy2 = newEnemy2;
            this.Enemy3 = newEnemy3;
            this.PrimaryPanel = (Panel.PANEL)newPrimaryPanelInt;
            this.SecondaryPanel = (Panel.PANEL)newSecondaryPanelInt;
            this.PanelPatternNumber = newPanelPatternNumber;
            this.IsTutorial = newIsTutorial;
            this.IsChipDropped = newIsChipDropped;
            this.IsEscapable = newIsEscapable;

            this.IsLongForm = isLongForm;
            if (this.IsLongForm)
            {
                this.IsGameEnding = newIsGameEnding;
                this.BackgroundMusic = newBackgroundMusic;
                this.BackgroundNumber = newBackgroundNumber;
            }
        }

        protected override ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors()
        {
            if (this.Enemies == null)
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>();
            }
            else
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>(this.Enemies.SelectMany(sr => this.UpdateChildErrorStack(sr)));
            }
        }

        private Enemy GetFirstEnemyOrDefault()
        {
            return new[] { this.Enemy1, this.Enemy2, this.Enemy3 }.FirstOrDefault();
        }

        private void Enemy1PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Enemy1));
        }

        private void Enemy2PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Enemy2));
        }

        private void Enemy3PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Enemy3));
        }
    }
}
