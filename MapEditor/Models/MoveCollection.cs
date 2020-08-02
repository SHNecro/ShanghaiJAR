using MapEditor.Core;
using MapEditor.ExtensionMethods;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MapEditor.Models
{
	public class MoveCollection : StringRepresentation
	{
		private ObservableCollection<Move> moves;
        private int lastSelectedIndex;
		private Move selectedMove;

		public MoveCollection()
		{
			this.Moves = new ObservableCollection<Move>();
		}

		public ObservableCollection<Move> Moves
		{
			get
			{
				return this.moves;
			}
			set
			{
				if (this.Moves != null)
				{
					this.Moves.CollectionChanged -= this.OnMovesCollectionChanged;
				}

				this.SetValue(ref this.moves, value);

				this.Moves.CollectionChanged += this.OnMovesCollectionChanged;

				this.SelectedMove = this.Moves.FirstOrDefault();
			}
		}

		public Move SelectedMove
		{
			get
            {
                return this.selectedMove;
			}
			set
			{
				if (value != null || this.Moves.Count == 0)
                {
                    if (this.selectedMove != null)
                    {
                        this.selectedMove.PropertyChanged -= this.OnSelectedMovePropertyChanged;
                    }
                    if (value != null)
                    {
                        value.PropertyChanged += this.OnSelectedMovePropertyChanged;
                    }
                    this.SetValue(ref this.selectedMove, value);
                    this.lastSelectedIndex = this.Moves.IndexOf(this.SelectedMove);
				}
			}
		}

        public Move this[int i] => this.Moves[i];

		protected override string GetStringValue()
		{
			return string.Join(":", this.Moves.Select(rm => rm.StringValue));
		}

		protected override void SetStringValue(string value)
		{
			var newMoves = value.Split(':').Where(ms => !string.IsNullOrEmpty(ms)).Select(ms => new Move { StringValue = ms }).ToList();

            this.Moves = new ObservableCollection<Move>(newMoves);
        }

        protected override ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors()
        {
            if (this.Moves == null)
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>();
            }
            else
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>(this.Moves.SelectMany(sr => this.UpdateChildErrorStack(sr)));
            }
        }

        private void OnMovesCollectionChanged(object sender, EventArgs args)
		{
            if (!this.Moves.Contains(this.SelectedMove))
            {
                this.SelectedMove = this.lastSelectedIndex < this.Moves.Count && this.lastSelectedIndex >= 0 ? this.Moves[this.lastSelectedIndex] : this.Moves.LastOrDefault();
            }
            this.OnPropertyChanged(nameof(this.Moves));
        }

        private void OnSelectedMovePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"SelectedMove.{e.PropertyName}");
        }
    }
}
