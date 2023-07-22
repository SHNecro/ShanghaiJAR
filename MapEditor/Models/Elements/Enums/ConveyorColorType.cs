using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ConveyorColorType
    {
        [Description("Red")]
        Red = 0,
		[Description("Blue")]
		Blue = 1,
		[Description("Green Tile")]
		GreenTile = 2,
		[Description("Blue Tile")]
		BlueTile = 3,
	}
}
