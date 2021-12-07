using NSAddOn;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using NSMap.Character;
using System.Collections.Generic;
using System.Linq;

namespace NSEvent
{
    internal class MysteryItem : EventBase
    {
        private RandomMystery itemData;
        private readonly MapField field;
        private readonly MysteryData mystery;
        private readonly SceneMap parent;
        private bool message;
        private bool setting;
        private EventManager em;
        private int flagNo;

        public MysteryItem(
          IAudioEngine s,
          EventManager m,
          MapField field,
          RandomMystery item,
          bool message,
          SaveData save)
          : base(s, m, save)
        {
            this.field = field;
            this.itemData = item;
            this.message = message;
            this.NoTimeNext = message;
            this.parent = m.parent;
        }

        public MysteryItem(
          IAudioEngine s,
          EventManager m,
          MapField field,
          RandomMystery item,
          bool message,
          MysteryData mystery,
          SaveData save)
          : base(s, m, save)
        {
            this.field = field;
            this.mystery = mystery;
            this.itemData = item;
            this.message = message;
            this.NoTimeNext = message;
            this.parent = m.parent;
        }

        public override void Update()
        {
            if (!this.setting)
            {
                this.ItemGet();
                this.setting = true;
                if (this.message)
                {
                    this.em = new EventManager(this.sound);
                    this.Eventmake();
                }
            }
            if (this.setting && !this.message)
            {
                this.EndCommand();
            }
            else
            {
                this.em.UpDate();
                if (!this.em.playevent)
                    this.message = false;
            }
        }

        private void ItemGet()
        {
            this.flagNo = this.itemData.flugNumber;
            if (this.itemData.type == 0)
            {
                RandomMystery[] randomMysteryArray = (RandomMystery[])this.field.randomMystery.Clone();
                // Remove all encounters if AntiVrs or no encounters
                if (this.savedata.runSubChips[2] ||
                    this.field.encounts.Count - (this.savedata.FlagList[this.field.encountCap[0]] ? 0 : this.field.encountCap[1]) <= 0)
                {
                    List<RandomMystery> list = ((IEnumerable<RandomMystery>)randomMysteryArray).ToList<RandomMystery>();
                    list.RemoveAll(r => r.itemType == 4);
                    randomMysteryArray = list.ToArray();
                }
                int index = this.Random.Next(randomMysteryArray.Length - 1);
                this.itemData = randomMysteryArray[index];
                this.mystery.getInfo = this.itemData.getInfo;
                this.mystery.EventMake();
            }
            if (this.itemData.itemType == 1)
            {
                if (this.savedata.haveSubChis[this.itemData.itemNumber] >= this.savedata.haveSubMemory)
                    this.savedata.selectQuestion = 1;
                else
                    this.savedata.selectQuestion = 0;
            }
            else
                this.savedata.selectQuestion = 0;
            if (this.savedata.selectQuestion != 0)
                return;
            this.savedata.item = this.itemData.getInfo;
            switch (this.itemData.itemType)
            {
                case 0:
                    this.savedata.category = ShanghaiEXE.Translate("MysteryItem.BattleChipText");
                    this.savedata.AddChip(this.itemData.itemNumber + 1, this.itemData.itemSub, true);
                    break;
                case 1:
                    this.savedata.category = ShanghaiEXE.Translate("MysteryItem.SubChipText");
                    ++this.savedata.haveSubChis[this.itemData.itemNumber];
                    break;
                case 2:
                    this.savedata.category = ShanghaiEXE.Translate("MysteryItem.AddText");
                    this.savedata.GetAddon(AddOnBase.AddOnSet(this.itemData.itemNumber, this.itemData.itemSub));
                    break;
                case 3:
                    this.savedata.category = "";
                    switch (this.itemData.itemNumber)
                    {
                        case 0:
                            this.savedata.HPmax += 20;
                            this.savedata.HPNow += 20;
                            break;
                        case 1:
                            this.savedata.Regularlarge += (byte)this.itemData.itemSub;
                            break;
                        case 2:
                            ++this.savedata.haveSubMemory;
                            break;
                        case 3:
                            this.savedata.MaxCore += this.itemData.itemSub;
                            break;
                        case 4:
                            this.savedata.MaxHz += this.itemData.itemSub;
                            break;
                        case 5:
                            this.savedata.havePeace[0] += this.itemData.itemSub;
                            break;
                        case 6:
                            this.savedata.havePeace[1] += this.itemData.itemSub;
                            break;
                        case 7:
                            this.savedata.havePeace[2] += this.itemData.itemSub;
                            break;
                        case 8:
                            this.savedata.Money += this.itemData.itemSub;
                            break;
                        case 9:
                            this.savedata.interiors.Add(new Interior(this.itemData.itemSub, 0, 0, false, false));
                            break;
                    }
                    break;
                default:
                    this.manager.parent.Player.EncountSet(true);
                    break;
            }
            if ((uint)this.itemData.type > 0U)
                this.savedata.GetMystery[this.flagNo] = true;
            else
                this.savedata.GetRandomMystery[this.flagNo] = true;
        }

        private void Eventmake()
        {
            string str = "";
            switch (this.itemData.itemType)
            {
                case 0:
                    str = ShanghaiEXE.Translate("MysteryItem.BattleChipPhrase");
                    break;
                case 1:
                    str = ShanghaiEXE.Translate("MysteryItem.SubChipPhrase");
                    break;
                case 2:
                    str = ShanghaiEXE.Translate("MysteryItem.AddonPhrase");
                    break;
                default:
                    break;
            }
            if (this.itemData.itemType != 4)
            {
                this.itemData.getInfo = MysteryItem.ItemNameGet(this.itemData.itemType, this.itemData.itemNumber, this.itemData.itemSub, this.itemData.getInfo);
                this.em.AddEvent(new BranchHead(this.sound, this.em, 0, this.savedata));
                this.em.AddEvent(new SEmon(this.sound, this.em, "get", 0, this.savedata));
                this.em.AddEvent(new EditValue(this.sound, this.em, 0, false, 0, 5, "0", this.parent.Player, this.savedata));
                this.em.AddEvent(new EditValue(this.sound, this.em, 1, false, 0, 5, "1", this.parent.Player, this.savedata));
                this.em.AddEvent(new EditValue(this.sound, this.em, 2, false, 0, 5, "2", this.parent.Player, this.savedata));
                this.em.AddEvent(new EffectMake(this.sound, this.em, 4, "get", 0, 1, 2, 1, -1, 0, 1, "none", this.parent, this.field, null, this.savedata));
                this.em.AddEvent(new PlayerHide(this.sound, this.em, true, this.parent.Player, this.savedata));
                var dialogue = ShanghaiEXE.Translate("MysteryData.GetItemDialogue1Format").Format(str, this.itemData.getInfo);
                this.em.AddEvent(new CommandMessage(this.sound, this.em, dialogue[0], dialogue[1], dialogue[2], false, this.savedata));
                if (this.itemData.itemType == 3 && this.itemData.itemNumber <= 4)
                {
                    switch (this.itemData.itemNumber)
                    {
                        case 0:
                            dialogue = ShanghaiEXE.Translate("MysteryData.GetHPMemoryDialogue1");
                            break;
                        case 1:
                            dialogue = ShanghaiEXE.Translate("MysteryData.GetRegUpDialogue1Format").Format(this.itemData.itemSub);
                            break;
                        case 2:
                            dialogue = ShanghaiEXE.Translate("MysteryData.GetSubMemoryDialogue1").Format(this.itemData.itemSub);
                            break;
                        case 3:
                            dialogue = ShanghaiEXE.Translate("MysteryData.GetCorePlusDialogue1").Format(this.itemData.itemSub);
                            break;
                        case 4:
                            dialogue = ShanghaiEXE.Translate("MysteryData.GetHertzUpDialogue1Format").Format(this.itemData.itemSub);
                            break;
                    }
                    this.em.AddEvent(new CommandMessage(this.sound, this.em, dialogue[0], dialogue[1], dialogue[2], false, this.savedata));
                }
                this.em.AddEvent(new PlayerHide(this.sound, this.em, false, this.parent.Player, this.savedata));
                this.em.AddEvent(new EffectEnd(this.sound, this.em, this.field, this.savedata));
                this.em.AddEvent(new BranchEnd(this.sound, this.manager, this.savedata));
                this.em.AddEvent(new BranchHead(this.sound, this.em, 1, this.savedata));
                dialogue = ShanghaiEXE.Translate("MysteryData.GetCannotCarryDialogue1Format").Format(this.itemData.getInfo);
                this.em.AddEvent(new CommandMessage(this.sound, this.em, dialogue[0], dialogue[1], dialogue[2], false, this.savedata));
                this.manager.AddEvent(new BranchEnd(this.sound, this.manager, this.savedata));
            }
            else
            {
                var dialogue = ShanghaiEXE.Translate("MysteryData.GetEncounterDialogue1").Format(this.itemData.getInfo);
                this.em.AddEvent(new CommandMessage(this.sound, this.em, dialogue[0], dialogue[1], dialogue[2], false, this.savedata));
            }
        }

        public static string ItemNameGet(int itemType, int itemNumber, int itemSub, string getInfo)
        {
            switch (itemType)
            {
                case 0:
                    ChipFolder chipFolder = new ChipFolder(null);
                    chipFolder.SettingChip(itemNumber + 1);
                    if (chipFolder.chip.code[itemSub] == ChipFolder.CODE.asterisk)
                        return chipFolder.chip.name + " ＊";
                    return chipFolder.chip.name + " " + chipFolder.chip.code[itemSub].ToString();
                case 1:
                    switch (itemNumber)
                    {
                        case 0:
                            return ShanghaiEXE.Translate("MysteryData.HalfEnrgText");
                        case 1:
                            return ShanghaiEXE.Translate("MysteryData.FullEnrgText");
                        case 2:
                            return ShanghaiEXE.Translate("MysteryData.FirewallText");
                        case 3:
                            return ShanghaiEXE.Translate("MysteryData.OpenPortText");
                        case 4:
                            return ShanghaiEXE.Translate("MysteryData.Anti-VrsText");
                        case 5:
                            return ShanghaiEXE.Translate("MysteryData.VirusScnText");
                        case 6:
                            return ShanghaiEXE.Translate("MysteryData.CrakToolText");
                    }
                    break;
                case 2:
                    return AddOnBase.AddOnSet(itemNumber, 0).name;
                case 3:
                    switch (itemNumber)
                    {
                        case 0:
                            return ShanghaiEXE.Translate("MysteryData.HPMemoryText");
                        case 1:
                            return string.Format(ShanghaiEXE.Translate("MysteryData.RegUpText"), itemSub);
                        case 2:
                            return ShanghaiEXE.Translate("MysteryData.SubMemoryText");
                        case 3:
                            return ShanghaiEXE.Translate("MysteryData.CorePlusText");
                        case 4:
                            return string.Format(ShanghaiEXE.Translate("MysteryData.HertzUpText"), itemSub);
                        case 5:
                            var bugPlural = itemSub > 1;
                            var bugGetStrFormat = bugPlural ? ShanghaiEXE.Translate("MysteryData.BugFragTextPlural") : ShanghaiEXE.Translate("MysteryData.BugFragText");
                            return string.Format(bugGetStrFormat, itemSub);
                        case 6:
                            var frzPlural = itemSub > 1;
                            var frzGetStrFormat = frzPlural ? ShanghaiEXE.Translate("MysteryData.FreezeFragTextPlural") : ShanghaiEXE.Translate("MysteryData.FreezeFragText");
                            return string.Format(frzGetStrFormat, itemSub);
                        case 7:
                            var errPlural = itemSub > 1;
                            var errGetStrFormat = errPlural ? ShanghaiEXE.Translate("MysteryData.ErrorFragTextPlural") : ShanghaiEXE.Translate("MysteryData.ErrorFragText");
                            return string.Format(errGetStrFormat, itemSub);
                        default:
                            if (itemSub > 0)
                            {
                                return string.Format(ShanghaiEXE.Translate("MysteryData.ZennyText"), itemSub);
                            }
                            return getInfo;
                        case 9:
                            var interiorNumber = itemSub;
                            var interiorName = Shop.INTERIOR.GetItem(itemSub);
                            return interiorName;
                    }
            }
            return "";
        }

        public override void SkipUpdate()
        {
            this.Update();
        }

        public override void Render(IRenderer dg)
        {
            if (!this.message)
                return;
            this.em?.Render(dg);
        }
    }
}
