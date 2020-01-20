using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;

namespace NSGame
{
    public class SceneBase : AllBase
    {
        protected SaveData savedata;
        public ShanghaiEXE parent;

        protected SceneBase(MyAudio s, ShanghaiEXE p, SaveData save)
          : base(s)
        {
            this.parent = p;
            this.savedata = save;
        }

        public virtual bool Init()
        {
            return true;
        }

        public virtual void Updata()
        {
        }

        public virtual void Render(IRenderer dg)
        {
        }
    }
}
