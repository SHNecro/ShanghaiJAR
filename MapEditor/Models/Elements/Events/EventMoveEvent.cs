using MapEditor.Core;
using MapEditor.ExtensionMethods;
using MapEditor.Models.Elements.Enums;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MapEditor.Models.Elements.Events
{
    public class EventMoveEvent : EventBase
    {
        private bool isMapIndex;
        private int mapIndex;
        private string objectID;
        private MoveCollection moves;

        public bool IsMapIndex
        {
            get { return this.isMapIndex; }
            set { this.SetValue(ref this.isMapIndex, value); }
        }
        public EventMoveTargetOption IsMapIndexOption
        {
            get
            {
                return this.IsMapIndex ? EventMoveTargetOption.MapIndex : EventMoveTargetOption.ObjectID;
            }

            set
            {
                if (value == EventMoveTargetOption.MapIndex)
                {
                    this.IsMapIndex = true;
                }
                else if (value == EventMoveTargetOption.ObjectID)
                {
                    this.IsMapIndex = false;
                }
                this.OnPropertyChanged(nameof(this.IsMapIndexOption));
            }
        }

        public int MapIndex
        {
            get { return this.mapIndex; }
            set { this.SetValue(ref this.mapIndex, value); }
        }

        public string ObjectID
        {
            get { return this.objectID; }
            set { this.SetValue(ref this.objectID, value); }
        }

        public MoveCollection Moves
        {
            get
            {
                return this.moves;
            }
            set
            {
                if (this.moves != null)
                {
                    this.moves.PropertyChanged -= this.OnMovesPropertyChanged;
                }
                value.PropertyChanged += this.OnMovesPropertyChanged;
                this.SetValue(ref this.moves, value);
            }
        }

        public override string Info => "Adds movement to an object with an ID or the Nth object in the map.";

        public override string Name
        {
            get
            {
                var idString = this.IsMapIndex
                    ? (this.MapIndex != -1 ? $"obj[{this.MapIndex}]" : "Player")
                    : (this.ObjectID != "プレイヤー" ? $"{this.ObjectID}" : "Player");

                return $"Move {idString}";
            }
        }

        protected override string GetStringValue()
        {
            var idString = this.IsMapIndex ? $"{this.MapIndex}" : $"{this.ObjectID}";
            var moveString = this.Moves.StringValue;
            return $"emove:{idString}:{moveString}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed movement event \"{value}\".", e => e.Length >= 3 && e[0] == "emove"))
            {
                return;
            }

            var newIsMapIndex = int.TryParse(entries[1], out int newMapIndex);
            var newObjectID = entries[1];

            var newMoves = new MoveCollection { StringValue = string.Join(":", entries.Skip(2)) };

            this.IsMapIndex = newIsMapIndex;
            this.MapIndex = newMapIndex;
            this.ObjectID = newObjectID;
            this.Moves = newMoves;
        }

        protected override ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors()
        {
            return this.UpdateChildErrorStack(Moves);
        }

        private void OnMovesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"Moves.{e.PropertyName}");
        }
    }
}
