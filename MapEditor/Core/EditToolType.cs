using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Core
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum EditToolType
    {
        [Description("Select Objects")]
        SelectionTool = 0,
        [Description("Draw Walkable")]
        DrawTool = 1,
    }
}
