
using MapEditor.Core;

namespace MapEditor.Models.Elements.Events
{
    public abstract class EventBase : StringRepresentation
    {
        public abstract string Info { get; }

        public abstract string Name { get; }
    }
}
