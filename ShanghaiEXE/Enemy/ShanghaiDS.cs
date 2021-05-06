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

        protected ChipFolder[] hand; // currently unused
        protected int deckLoc;
        protected int deckSize;

        public ShanghaiDS(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
            : base(s, p, pX, pY, n, u, v, 3000, "ShanghaiDS", "ShanghaiDS")
        {


            /* todo:
             *      have shanghaiDS assemble 'hands' by taking 5-6 chips from the deck
             *          check hands for potential PAs
             *          remove applicable chips from hand, replace with PA
             *          make it so shanghaiDS assembles new hands when the player opens the custom screen
             *      have shanghaiDS use buster attack when out of chips for that turn?
             *      
             *      have shanghaiDS mirror style changes (sprites at least?)
             *      
             *      chip 'location memory'?
             *          this would require a pretty massive retcon in game code and adding stuff to the save
             *          tl;dr MMBN4/5 recorded where on the screen you like to use chips, to use as a reference for what location MMDS would be when using it himself
             *      
             *      chip 'combo system'?
             *          like location memory, but records which chips you like to use before and after each other, plus locations
             *          
             *      //
             *      
             */



        }

        // Knuth Shuffle
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

            // randomize deck order
            Shuffle(this.chips);
            Shuffle(this.chips);

            // set deckSize/deckLoc
            this.deckSize = this.chips.Length;
            this.deckLoc = 0;

            for (var i = 0; i < this.dropchips.Length; i++)
            {
                this.dropchips[i] = this.chips[i];
            }
        }

        protected override void SetUsedChip()
        {
            base.SetUsedChip();

            //this.usechip = Random.Next(this.chips.Length);

            this.usechip = this.deckLoc;
            this.deckLoc++;

            // ensure deckLoc does not go out of bounds, reshuffle deck
            if (this.deckLoc >= this.deckSize)
            {
                //
                this.deckLoc = 0;
                Shuffle(this.chips);
            }



        }
    }
}
