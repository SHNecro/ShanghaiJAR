using NSShanghaiEXE.InputOutput.Audio;
using NSEvent;
using NSGame;
using System.Drawing;
using System.IO;
using Common;
using ExtensionMethods;

namespace NSMap.Character
{
    public class MysteryData : MapEventBase
    {
        public string getInfo = ShanghaiEXE.Translate("MysteryData.DefaultContents");
        public string category = "";
        public RandomMystery itemData;

        private EventManager Manager
        {
            get
            {
                return this.eventPages[0].eventmanager;
            }
            set
            {
                this.eventPages[0].eventmanager = value;
            }
        }

        public override EventPage LunPage
        {
            get
            {
                return this.eventPages[this.lunPage];
            }
        }

        public override byte CharaAnimeFlame
        {
            set
            {
                this.animeflame = value;
                if (animeflame <= this.LunPage.graphicNo[4])
                    return;
                this.animeflame = 0;
            }
            get
            {
                return this.animeflame;
            }
        }

        public void EventMake()
        {
            this.Manager.events.Clear();
            this.Manager.AddEvent(new OpenMassageWindow(this.sound, this.Manager));
            if (this.itemData.type < 2)
            {
                var dialogue = ShanghaiEXE.Translate("MysteryItem.OpenDialogue1");
                this.Manager.AddEvent(new CommandMessage(this.sound, this.Manager, dialogue[0], dialogue[1], dialogue[2], false, this.savedate));
                this.Manager.AddEvent(new MysteryItem(this.sound, this.Manager, this.field, this.itemData, true, this, this.savedate));
            }
            else
            {
                this.Manager.AddEvent(new BranchHead(this.sound, this.Manager, 0, this.savedate));
                var dialogue = ShanghaiEXE.Translate("MysteryItem.LockedDialogue1");
                this.Manager.AddEvent(new CommandMessage(this.sound, this.Manager, dialogue[0], dialogue[1], dialogue[2], false, this.savedate));
                dialogue = ShanghaiEXE.Translate("MysteryItem.LockedDialogue2");
                this.Manager.AddEvent(new CommandMessage(this.sound, this.Manager, dialogue[0], dialogue[1], dialogue[2], false, this.savedate));
                this.Manager.AddEvent(new Special(this.sound, this.Manager, 16, this.savedate));
                this.Manager.AddEvent(new BranchHead(this.sound, this.Manager, 1, this.savedate));
                dialogue = new Dialogue { Text = string.Format(ShanghaiEXE.Translate("MysteryItem.LockUseCrakToolQuestionFormat").Text, this.savedate.haveSubChis[6]) };
                var questionOptions = ShanghaiEXE.Translate("MysteryItem.LockUseCrakToolQuestionOptions");
                this.Manager.AddEvent(new Question(
                    this.sound,
                    this.Manager,
                    dialogue[0],
                    dialogue[1],
                    questionOptions[0],
                    questionOptions[1],
                    false,
                    true,
                    FACE.None.ToFaceId(),
                    this.savedate,
                    true));
                this.Manager.AddEvent(new Special(this.sound, this.Manager, 17, this.savedate));
                this.Manager.AddEvent(new BranchEnd(this.sound, this.Manager, this.savedate));
                this.Manager.AddEvent(new BranchHead(this.sound, this.Manager, 1, this.savedate));
                dialogue = ShanghaiEXE.Translate("MysteryItem.LockOpenedDialogue1");
                this.Manager.AddEvent(new CommandMessage(this.sound, this.Manager, dialogue[0], dialogue[1], dialogue[2], false, this.savedate));
                this.Manager.AddEvent(new MysteryItem(this.sound, this.Manager, this.field, this.itemData, true, this, this.savedate));
                this.Manager.AddEvent(new BranchEnd(this.sound, this.Manager, this.savedate));
            }
            this.Manager.AddEvent(new CloseMassageWindow(this.sound, this.Manager));
        }

        public MysteryData(
          IAudioEngine s,
          SceneMap p,
          Point po,
          int floor,
          MapCharacterBase.ANGLE a,
          MapField fi,
          string id,
          SaveData save,
          StreamReader reader,
          RandomMystery random)
          : base(s, p, po, floor, a, fi, id, save, reader, "")
        {
            this.position.Z = this.field.Height / 2 * floor;
            this.itemData = random;
            this.getInfo = this.itemData.getInfo;
            this.Update();
        }

        public override float RendSetter()
        {
            return (float)(position.X + (double)this.LunPage.hitShift.X + (position.Y + (double)this.LunPage.hitShift.Y) - 16.0);
        }

        public override void Update()
        {
            this.lunPage = -1;
            if (this.itemData.type == 0)
                this.lunPage = this.savedate.GetRandomMystery[this.itemData.flugNumber] ? 1 : 0;
            else
                this.lunPage = this.savedate.GetMystery[this.itemData.flugNumber] ? 1 : 0;
            if (this.lunPage == 0 && !this.parent.eventmanager.playevent)
                this.EventMake();
            this.eventPages[this.lunPage].Update();
            this.rendType = this.LunPage.rendType;
        }
    }
}
