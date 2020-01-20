using Common;
using System;

namespace NSGame
{
    [Serializable]
    internal class TestItem : KeyItem
    {
        public TestItem(int number)
        {
            Dialogue dialogue;
            switch (number)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("TestItem.CompressedDataName");
                    dialogue = ShanghaiEXE.Translate("TestItem.CompressedDataDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    dialogue = ShanghaiEXE.Translate("TestItem.CompressedDataDescDialogue2");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("TestItem.HeatDataName");
                    dialogue = ShanghaiEXE.Translate("TestItem.HeatDataDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("TestItem.AntiFreezeName");
                    dialogue = ShanghaiEXE.Translate("TestItem.AntiFreezeDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("TestItem.MetroPassName");
                    dialogue = ShanghaiEXE.Translate("TestItem.MetroPassDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("TestItem.IceCrystalDataName");
                    dialogue = ShanghaiEXE.Translate("TestItem.IceCrystalDataDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 5:
                    this.name = ShanghaiEXE.Translate("TestItem.PackageDataName");
                    dialogue = ShanghaiEXE.Translate("TestItem.PackageDataDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 6:
                    this.name = ShanghaiEXE.Translate("TestItem.MariPCodeName");
                    dialogue = ShanghaiEXE.Translate("TestItem.MariPCodeDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 7:
                    this.name = ShanghaiEXE.Translate("TestItem.RemiliaPCodeName");
                    dialogue = ShanghaiEXE.Translate("TestItem.RemiliaPCodeDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 8:
                    this.name = ShanghaiEXE.Translate("TestItem.RikaPCodeName");
                    dialogue = ShanghaiEXE.Translate("TestItem.RikaPCodeDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 9:
                    this.name = ShanghaiEXE.Translate("TestItem.TsubakiPCodeName");
                    dialogue = ShanghaiEXE.Translate("TestItem.TsubakiPCodeDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 10:
                    this.name = ShanghaiEXE.Translate("TestItem.TenshiPCodeName");
                    dialogue = ShanghaiEXE.Translate("TestItem.TenshiPCodeDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 11:
                    this.name = ShanghaiEXE.Translate("TestItem.SpareCodeName");
                    dialogue = ShanghaiEXE.Translate("TestItem.SpareCodeDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 12:
                    this.name = ShanghaiEXE.Translate("TestItem.OldChipDataName");
                    dialogue = ShanghaiEXE.Translate("TestItem.OldChipDataDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 13:
                    this.name = ShanghaiEXE.Translate("TestItem.TeacherIDName");
                    dialogue = ShanghaiEXE.Translate("TestItem.TeacherIDDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 14:
                    this.name = ShanghaiEXE.Translate("TestItem.CruiseTicketName");
                    dialogue = ShanghaiEXE.Translate("TestItem.CruiseTicketDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 15:
                    this.name = ShanghaiEXE.Translate("TestItem.YinKeyName");
                    dialogue = ShanghaiEXE.Translate("TestItem.YinKeyDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 16:
                    this.name = ShanghaiEXE.Translate("TestItem.YangKeyName");
                    dialogue = ShanghaiEXE.Translate("TestItem.YangKeyDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 17:
                    this.name = ShanghaiEXE.Translate("TestItem.AdminPCodeName");
                    dialogue = ShanghaiEXE.Translate("TestItem.AdminPCodeDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 18:
                    this.name = ShanghaiEXE.Translate("TestItem.SmallParcelName");
                    dialogue = ShanghaiEXE.Translate("TestItem.SmallParcelDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 19:
                    this.name = ShanghaiEXE.Translate("TestItem.WrigglePendantName");
                    dialogue = ShanghaiEXE.Translate("TestItem.WrigglePendantDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    dialogue = ShanghaiEXE.Translate("TestItem.WrigglePendantDescDialogue2");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 20:
                    this.name = ShanghaiEXE.Translate("TestItem.ROMIDName");
                    dialogue = ShanghaiEXE.Translate("TestItem.ROMIDDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 21:
                    this.name = ShanghaiEXE.Translate("TestItem.PreservedFlowerName");
                    dialogue = ShanghaiEXE.Translate("TestItem.PreservedFlowerDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 22:
                    this.name = ShanghaiEXE.Translate("TestItem.OldKeyDataName");
                    dialogue = ShanghaiEXE.Translate("TestItem.OldKeyDataDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 23:
                    this.name = ShanghaiEXE.Translate("TestItem.OldIDDataName");
                    dialogue = ShanghaiEXE.Translate("TestItem.OldIDDataDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 24:
                    this.name = ShanghaiEXE.Translate("TestItem.OldPasscodeName");
                    dialogue = ShanghaiEXE.Translate("TestItem.OldPasscodeDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
                case 25:
                    this.name = ShanghaiEXE.Translate("TestItem.ReincarnationProgramName");
                    dialogue = ShanghaiEXE.Translate("TestItem.ReincarnationProgramDescDialogue1");
                    this.info.Add(dialogue[0]);
                    this.info.Add(dialogue[1]);
                    this.info.Add(dialogue[2]);
                    break;
            }
        }
    }
}
