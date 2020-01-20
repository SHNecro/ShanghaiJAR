using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
	[TypeConverter(typeof(EnumDescriptionTypeConverter))]
	public enum StartTermType
	{
		[Description("A Button")]
        AButton = 0,
        [Description("R Button")]
        RButton = 1,
        [Description("Touch")]
        Touch = 2,
        [Description("Auto")]
        Auto = 3,
        [Description("None")]
        None = 4,
        [Description("Parallel")]
        Parallel = 5
    }
}
