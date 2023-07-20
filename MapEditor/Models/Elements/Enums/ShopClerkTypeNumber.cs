using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ShopClerkTypeNumber
    {
        [Description("Professional")]
        Professional = 0,
        [Description("Casual")]
        Casual = 1,
        [Description("Annoyed")]
        Annoyed = 2,
		[Description("Shady")]
		Shady = 3,
		[Description("Glitchy")]
		Glitchy = 4,
	}
}
