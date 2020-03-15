using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;

namespace NSGame
{
    public class SceneBase : AllBase
    {
        protected SaveData savedata;
        public ShanghaiEXE parent;

        protected SceneBase(IAudioEngine s, ShanghaiEXE p, SaveData save)
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
