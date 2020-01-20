﻿using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSChip
{
    internal class VirusBall2 : VirusBall1
    {
        public VirusBall2(MyAudio s, bool set)
          : base(s, set)
        {
            this.number = 311;
            this.name = ShanghaiEXE.Translate("Chip.VrsBall2Name");
            this.element = ChipBase.ELEMENT.normal;
            this.id = 1;
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
            var information = ShanghaiEXE.Translate("Chip.VrsBall2Desc");
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
                information = ShanghaiEXE.Translate("Chip.VrsBall2FilledDesc");
                this.information[0] = information[0];
                this.information[1] = information[1];
                this.information[2] = information[2];
                this.Init();
                this.name = haveViru.Name;
            }
        }
    }
}
