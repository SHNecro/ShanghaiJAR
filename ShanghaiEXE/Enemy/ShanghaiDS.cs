using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using System;

//using NSAttack;
//using NSEnemy;
//using NSNet;

namespace NSEnemy
{
    internal class ShanghaiDS : ChipUsingNaviBase
    {
        public ShanghaiDS(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
            : base(s, p, pX, pY, n, u, v, 3000, "ShanghaiDS", "ShanghaiDS")
        {
        }

        protected override void InitializeChips()
        {
            base.InitializeChips();

            var trueSaveData = this.Parent.parent.savedata;
            var equipFolderIndex = trueSaveData.efolder;
            var playerFolder = trueSaveData.chipFolder;

            this.chips = new ChipFolder[playerFolder.GetLength(1)];

            for (int i = 0; i < playerFolder.GetLength(1); i++)
            {
                this.chips[i] = new ChipFolder(this.sound);
                this.chips[i].SettingChip(playerFolder[equipFolderIndex, i, 0]);
                this.chips[i].codeNo = playerFolder[equipFolderIndex, i, 1];
            }
        }

        protected override void SetUsedChip()
        {
            base.SetUsedChip();

            this.usechip = Random.Next(this.chips.Length);
        }
    }
}
