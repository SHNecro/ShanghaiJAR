using NSShanghaiEXE.InputOutput.Audio;
using NSGame;

namespace NSChip
{
    internal class VirusBall3 : VirusBall1
    {
        public VirusBall3(IAudioEngine s, bool set)
          : base(s, set)
        {
            this.number = 312;
            this.name = ShanghaiEXE.Translate("Chip.VrsBall3Name");
            this.id = 2;
            this.element = ChipBase.ELEMENT.normal;
            this.power = 10;
            this.subpower = 0;
            this.regsize = 35;
            this.reality = 5;
            this._break = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.asterisk;
            this.code[1] = ChipFolder.CODE.asterisk;
            this.code[2] = ChipFolder.CODE.asterisk;
            this.code[3] = ChipFolder.CODE.asterisk;
            var information = ShanghaiEXE.Translate("Chip.VrsBall3Desc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            if (SaveData.HAVEVirus[this.id] == null || !set)
            {
                this.Init();
            }
            else
            {
                Virus haveViru = SaveData.HAVEVirus[this.id];
                this.element = ChipBase.ELEMENT.normal;
                this.power = 0;
                this.subpower = 0;
                this.regsize = 35;
                this.reality = 5;
                this._break = false;
                this.powerprint = false;
                this.shadow = false;
                this.code[0] = haveViru.Code;
                this.code[1] = this.code[0];
                this.code[2] = this.code[0];
                this.code[3] = this.code[0];
                information = ShanghaiEXE.Translate("Chip.VrsBall3FilledDesc");
                this.information[0] = information[0];
                this.information[1] = information[1];
                this.information[2] = information[2];
                this.Init();
                this.name = haveViru.Name;
            }
        }
    }
}
