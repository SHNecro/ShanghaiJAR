using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
	[TypeConverter(typeof(EnumDescriptionTypeConverter))]
	public enum ProgramColorTypeNumber
	{
		[Description("Grey")]
        Grey = 0,
		[Description("Pink")]
        Pink = 1,
		[Description("Sky Blue")]
        SkyBlue = 2,
		[Description("Red")]
        Red = 3,
		[Description("Blue")]
        Blue = 4,
		[Description("Green")]
        Green = 5,
		[Description("Yellow")]
        Yellow = 6,
		[Description("Dark")]
        Dark = 7
	}
}
