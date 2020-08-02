using MapEditor.Core;
using MapEditor.ExtensionMethods;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MapEditor.Models.Elements.Events
{
    public class BattleEvent : EventBase, ITranslatedModel
    {
        private RandomEncounter encounter;

        public RandomEncounter Encounter
        {
            get
            {
                return this.encounter;
            }

            set
            {
                if (this.Encounter != null)
                {
                    this.Encounter.PropertyChanged -= this.EncounterPropertyChanged;
                }
                this.SetValue(ref this.encounter, value);
                this.Encounter.PropertyChanged += this.EncounterPropertyChanged;
            }
        }

        public override string Info => "Begins a battle.";

        public override string Name
        {
            get
            {
                var enemiesString = this.Encounter.Enemies.Any(e => e.ID != 0) ? string.Join(" ", this.Encounter.Enemies.Select(e => e.Name)) : "N/A";
                return $"Battle: {enemiesString}";
            }
        }

        public BattleEvent()
        {
            this.Encounter = Constants.BlankEncounterCreator();
        }

        protected override string GetStringValue()
        {
            return this.Encounter.StringValue;
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed battle event \"{value}\".", e => (e.Length == 37 || e.Length == 19) && e[0] == "battle"))
            {
                return;
            }
            
            var newEncounter = new RandomEncounter { StringValue = value };

            this.Encounter = newEncounter;
        }

        protected override ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors()
        {
            return this.UpdateChildErrorStack(this.Encounter);
        }

        private void EncounterPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Encounter));
            this.OnPropertyChanged(nameof(this.Name));
        }

        public void RefreshTranslation()
        {
            this.Encounter.Enemy1.RefreshTranslation();
            this.Encounter.Enemy2.RefreshTranslation();
            this.Encounter.Enemy3.RefreshTranslation();
            this.OnPropertyChanged(nameof(this.Name));
        }
    }
}
