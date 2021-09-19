using NSShanghaiEXE.InputOutput.Audio;

namespace NSChip
{
    internal class DammyChip : ChipBase
    {
        public DammyChip(IAudioEngine s)
          : base(s)
        {
            this.number = 999;
            this.name = "ン";
            this.element = ChipBase.ELEMENT.poison;
            this.power = 9999;
            this.subpower = 9999;
            this.regsize = 999;
            this.reality = byte.MaxValue;
            this._break = false;
            this.powerprint = false;
            this.code[0] = ChipFolder.CODE.asterisk;
            this.code[1] = ChipFolder.CODE.asterisk;
            this.code[2] = ChipFolder.CODE.asterisk;
            this.code[3] = ChipFolder.CODE.asterisk;
        }
    }
}
