using MapEditor.Core;
using MapEditor.Models.Elements.Enums;
using NSMap.Character;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MapEditor.Models
{
    public class Move : StringRepresentation
	{
		private static readonly List<Tuple<MoveType, MoveType, MoveType>> Angles = new List<Tuple<MoveType, MoveType, MoveType>>
		{
			Tuple.Create(MoveType.WalkLeft, MoveType.WarpLeft, MoveType.AngleLeft),
			Tuple.Create(MoveType.WalkUp, MoveType.WarpUp, MoveType.AngleUp),
			Tuple.Create(MoveType.WalkDown, MoveType.WarpDown, MoveType.AngleDown),
			Tuple.Create(MoveType.WalkRight, MoveType.WarpRight, MoveType.AngleRight)
		};

		private MoveType lastAngledType = MoveType.WalkLeft;

		private MoveType type;
		private int distance;

        public MoveType Type
		{
			get
			{
				return this.type;
			}

			set
			{
				this.SetValue(ref this.type, value);
				this.OnPropertyChanged(nameof(this.StringValue));
				switch (this.Type)
				{
					case MoveType.WalkUp:
					case MoveType.WalkDown:
					case MoveType.WalkLeft:
					case MoveType.WalkRight:
					case MoveType.WarpUp:
					case MoveType.WarpDown:
					case MoveType.WarpLeft:
					case MoveType.WarpRight:
					case MoveType.AngleUp:
					case MoveType.AngleDown:
					case MoveType.AngleLeft:
					case MoveType.AngleRight:
						this.lastAngledType = value;
						break;
                }
                switch (this.Type)
                {
                    case MoveType.Jump:
                    case MoveType.AngleUp:
                    case MoveType.AngleDown:
                    case MoveType.AngleLeft:
                    case MoveType.AngleRight:
                    case MoveType.AngleLock:
                    case MoveType.AngleUnlock:
                        this.Distance = 0;
                        break;
                }
            }
		}
        public int Distance
		{
			get
			{
				return this.distance;
			}

			set
			{
				this.SetValue(ref this.distance, value);
				this.OnPropertyChanged(nameof(this.StringValue));
			}
		}

		public MoveCategoryOption Category
		{
			get
			{
				switch (this.Type)
				{
					case MoveType.WalkUp:
					case MoveType.WalkDown:
					case MoveType.WalkLeft:
					case MoveType.WalkRight:
						return MoveCategoryOption.Walk;
					case MoveType.WarpUp:
					case MoveType.WarpDown:
					case MoveType.WarpLeft:
					case MoveType.WarpRight:
						return MoveCategoryOption.Warp;
					case MoveType.AngleUp:
					case MoveType.AngleDown:
					case MoveType.AngleLeft:
					case MoveType.AngleRight:
						return MoveCategoryOption.Angle;
					case MoveType.Jump:
						return MoveCategoryOption.Jump;
					case MoveType.AngleLock:
						return MoveCategoryOption.Lock;
					case MoveType.AngleUnlock:
						return MoveCategoryOption.Unlock;
					case MoveType.Wait:
					default:
						return MoveCategoryOption.Wait;
				}
			}

			set
			{
				switch (value)
				{
					case MoveCategoryOption.Walk:
						this.Type = this.GetAngleOfNewType(MoveCategoryOption.Walk, this.Type);
						break;
					case MoveCategoryOption.Warp:
						this.Type = this.GetAngleOfNewType(MoveCategoryOption.Warp, this.Type);
						break;
					case MoveCategoryOption.Angle:
						this.Type = this.GetAngleOfNewType(MoveCategoryOption.Angle, this.Type);
						break;
					case MoveCategoryOption.Jump:
						this.Type = MoveType.Jump;
						break;
					case MoveCategoryOption.Lock:
						this.Type = MoveType.AngleLock;
						break;
					case MoveCategoryOption.Unlock:
						this.Type = MoveType.AngleUnlock;
						break;
					case MoveCategoryOption.Wait:
						this.Type = MoveType.Wait;
						break;
				}

				this.OnPropertyChanged(nameof(this.Category));
			}
		}

        protected override string GetStringValue()
        {
            var moveType = (EventMove.MOVE)(int)this.Type;
            return $"{moveType},{this.Distance}";
        }

        protected override void SetStringValue(string value)
        {
            var newType = default(MoveType);
            var newDistance = default(int);
            var moveParams = value.Split(',');
            if (this.Validate(moveParams, $"Malformed move entry \"{value}\".", mp => mp.Length == 2))
            {
                var moveType = this.ParseEnumOrAddError<EventMove.MOVE>(moveParams[0]);
                newType = (MoveType)(int)moveType;
                newDistance = this.ParseIntOrAddError(moveParams[1]);
            }

            this.Type = newType;
            this.Distance = newDistance;
        }

		private MoveType GetAngleOfNewType(MoveCategoryOption category, MoveType originalMove)
		{
			var angle = Move.Angles.FirstOrDefault(ttt => ttt.Item1 == originalMove || ttt.Item2 == originalMove || ttt.Item3 == originalMove)
				?? Move.Angles.FirstOrDefault(ttt => ttt.Item1 == this.lastAngledType || ttt.Item2 == this.lastAngledType || ttt.Item3 == this.lastAngledType)
				?? Tuple.Create(MoveType.WalkLeft, MoveType.WarpLeft, MoveType.AngleLeft);

			switch (category)
			{
				case MoveCategoryOption.Walk:
					return angle.Item1;
				case MoveCategoryOption.Warp:
					return angle.Item2;
				case MoveCategoryOption.Angle:
					return angle.Item3;
				default:
					return MoveType.Wait;
			}
		}
	}
}
