using NSBattle;
using NSEffect;
using NSGame;
using NSShanghaiEXE.InputOutput.Audio;
using Common.Vectors;
using System;

namespace NSAddOn
{
    [Serializable]
    internal class Mammon : AddOnBase
    {
        public Mammon(AddOnBase.ProgramColor color) : base(color)
        {
            this.Init();
        }

        private void Init()
        {
            this.ID = 97;
            this.name = ShanghaiEXE.Translate("AddOn.MammonName");
            this.Plus = true;
            this.UseHz = 6;
            this.UseCore = 4;
            var information = ShanghaiEXE.Translate("AddOn.MammonDesc");
            this.infomasion[0] = information[0];
            this.infomasion[1] = information[1];
            this.infomasion[2] = information[2];
        }

        public override void Running(SaveData save)
        {
            save.addonSkill[74] = true;
        }
        
        public static Action ApplyMammonPunishments(IAudioEngine sound, SceneBattle battle, ref int canopenchips, int chips)
        {
            if (chips >= 2)
            {
                sound.PlaySE(SoundEffect.damageplayer);

                var playerChar = battle.player;
                battle.effects.Add(new Debuff(
                    sound,
                    battle,
                    new Vector2(
                        (int)playerChar.positionDirect.X * (playerChar.union == Panel.COLOR.red ? 1 : -1),
                        (int)playerChar.positionDirect.Y + 16),
                    2,
                    playerChar.position));
            }

            var chipPain = chips * 10;
            if (chips >= 2)
            {
                battle.player.chipPain += chipPain;
            }

            var isSlipImmune = battle.player.Element == NSChip.ChipBase.ELEMENT.aqua || battle.player.style == NSBattle.Character.Player.STYLE.wing;
            var replacedStatuses = default(Tuple<bool[], int[]>);
            if (chips >= 3)
            {
                if (!isSlipImmune)
                {
                    replacedStatuses = Tuple.Create(new bool[9], new int[9]);
                    for (var i = 1; i <= 6; i++)
                    {
                        replacedStatuses.Item1[i] = battle.player.badstatus[i];
                        replacedStatuses.Item2[i] = battle.player.badstatustime[i];
                    }
                    battle.player.badstatus[2] = true;
                    battle.player.badstatustime[2] = -1;
                }
                else
                {
                    for (var i = 0; i < 3; i++)
                    {
                        battle.panel[battle.player.position.X, i].State = Panel.PANEL._thunder;
                    }
                }
            }

            var busterDowngrade = new int[] { 0, 0, 0 };
            if (chips >= 4)
            {
                var downgradeAmount = chips - 2;
                var newBusterStats = new int[3];
                newBusterStats[0] = Math.Max(1, battle.player.busterPower - downgradeAmount);
                newBusterStats[1] = Math.Max(1, battle.player.busterRapid - downgradeAmount);
                newBusterStats[2] = Math.Max(1, battle.player.busterCharge - downgradeAmount);

                busterDowngrade[0] = battle.player.busterPower - newBusterStats[0];
                busterDowngrade[1] = battle.player.busterRapid - newBusterStats[1];
                busterDowngrade[2] = battle.player.busterCharge - newBusterStats[2];

                battle.player.busterPower = (byte)newBusterStats[0];
                battle.player.busterRapid = (byte)newBusterStats[1];
                battle.player.busterCharge = (byte)newBusterStats[2];
            }

            var custSpeedDowngrade = 0;
            if (chips >= 5)
            {
                var newCustSpeed = 1;
                custSpeedDowngrade = battle.customgauge.Customspeed - newCustSpeed;
                battle.CustomSpeedChange(newCustSpeed);
            }

            // Not undone at end of round, using a full, doc-boosted hand is asking for it.
            if (chips >= 6)
            {
                canopenchips = Math.Max(1, canopenchips - 2);
            }

            return () =>
            {
                battle.player.chipPain -= chipPain;

                battle.player.busterPower += (byte)busterDowngrade[0];
                battle.player.busterRapid += (byte)busterDowngrade[1];
                battle.player.busterCharge += (byte)busterDowngrade[2];

                if (replacedStatuses != null)
                {
                    for (var i = 1; i <= 6; i++)
                    {
                        battle.player.badstatus[i] = replacedStatuses.Item1[i];
                        battle.player.badstatustime[i] = replacedStatuses.Item2[i];
                    }
                }

                battle.CustomSpeedChange(battle.customgauge.Customspeed + custSpeedDowngrade);
            };
        }
    }
}
