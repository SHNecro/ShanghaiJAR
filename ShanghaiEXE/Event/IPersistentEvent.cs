using NSShanghaiEXE.InputOutput.Rendering;

namespace NSEvent
{
    public interface IPersistentEvent
    {
        bool IsActive { get; set; }

        void PersistentUpdate();

        void PersistentRender(IRenderer dg);
    }
}
