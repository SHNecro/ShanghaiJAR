using MapEditor.Core;

namespace MapEditor.Models.Elements.Terms
{
    public abstract class TermBase : StringRepresentation
    {
        public abstract string Name { get; }
    }
}
