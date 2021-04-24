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

        protected ChipFolder[] hand;
        protected int deckLoc;
        protected int deckSize;

        public ShanghaiDS(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
            : base(s, p, pX, pY, n, u, v, 3000, "ShanghaiDS", "ShanghaiDS")
        {

            /*tst*/




        }

        /*
        private void shuffleDeck(this Random rng, T[] array)
        {
            //
            var rng = new Random();

            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }

        }
        */

        private void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = Random.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        protected override void InitializeChips()
        {
            base.InitializeChips();

            var trueSaveData = this.Parent.parent.savedata;
            var equipFolderIndex = trueSaveData.efolder;
            var playerFolder = this.Parent.main.chipfolder;

            this.chips = new ChipFolder[playerFolder.GetLength(1)];

            for (int i = 0; i < playerFolder.GetLength(1); i++)
            {
                this.chips[i] = new ChipFolder(this.sound);
                this.chips[i].SettingChip(playerFolder[equipFolderIndex, i].chip.number);
                this.chips[i].codeNo = playerFolder[equipFolderIndex, i].codeNo;
            }


            var rng = new Random();
            Shuffle(this.chips);
            Shuffle(this.chips);

            this.deckSize = this.chips.Length;
            this.deckLoc = 0;

        }

        protected override void SetUsedChip()
        {
            base.SetUsedChip();

            //this.usechip = Random.Next(this.chips.Length);

            this.usechip = this.deckLoc;
            this.deckLoc++;
            if (this.deckLoc >= this.deckSize)
            {
                //
                this.deckLoc = 0;

            }



        }
    }
}
