using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;

namespace NSEvent
{
    internal class GetMail : EventBase
    {
        public int number;
        private readonly SceneMap map;
        private readonly bool effect;
        private bool alreadyAdded;

        public GetMail(IAudioEngine s, EventManager m, int ID, bool effect, SceneMap map, SaveData save)
          : base(s, m, save)
        {
            this.effect = effect;
            this.map = map;
            this.NoTimeNext = true;
            this.number = ID;
        }

        public override void Update()
        {
            if (this.effect)
                this.map.MailOn(true);
            if (this.savedata.mail.Count < this.savedata.mailread.Count)
            {
                for (int index = this.savedata.mail.Count - 1; index < this.savedata.mailread.Count; ++index)
                    this.savedata.mailread.RemoveAt(index);
            }
            this.savedata.mail.Add(this.number);
            this.savedata.mailread.Add(false);
            this.alreadyAdded = true;
            this.EndCommand();
        }

        public override void SkipUpdate()
        {
            if (!alreadyAdded)
            {
                this.savedata.mail.Add(this.number);
                this.savedata.mailread.Add(false);
            }
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
