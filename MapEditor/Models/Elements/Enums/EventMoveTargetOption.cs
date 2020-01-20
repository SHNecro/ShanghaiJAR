using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum EventMoveTargetOption
    {
        [Description("Map Index")]
        MapIndex = 0,
        [Description("Object ID")]
        ObjectID = 1,
    }
}
