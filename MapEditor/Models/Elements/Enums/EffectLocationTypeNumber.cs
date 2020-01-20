using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum EffectLocationTypeNumber
    {
        [Description("Position")]
        Position = 0,
        [Description("Variables")]
        Variable = 1,
        [Description("Here")]
        ParentObject = 2,
        [Description("At Object")]
        OtherObject = 3
    }
}
