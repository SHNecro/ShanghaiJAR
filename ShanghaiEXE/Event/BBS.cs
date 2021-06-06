using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap.Character;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NSEvent
{
    public class BBS : EventBase
    {
        private readonly ForumItem[][] forumlist = new ForumItem[5][]
        {
            // GenNet
            new ForumItem[41]
            {
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterBillager"), ShanghaiEXE.Translate("BBS.SubjectBoardOpen!"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMakoMako"), ShanghaiEXE.Translate("BBS.SubjectRe:BoardOpen!"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterDonkichi"), ShanghaiEXE.Translate("BBS.SubjectRe:BoardOpen!"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNetAgent"), ShanghaiEXE.Translate("BBS.SubjectAlert"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMakoMako"), ShanghaiEXE.Translate("BBS.SubjectRe:Alert"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterCorona"), ShanghaiEXE.Translate("BBS.SubjectRe:Alert"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterDonkichi"), ShanghaiEXE.Translate("BBS.SubjectRe:Alert"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterBillager"), ShanghaiEXE.Translate("BBS.SubjectRe:Alert"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterTamezou"), ShanghaiEXE.Translate("BBS.SubjectROM’sPast"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterDonkichi"), ShanghaiEXE.Translate("BBS.SubjectRe:ROM’sPast"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterCorona"), ShanghaiEXE.Translate("BBS.SubjectChipShop"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMakoMako"), ShanghaiEXE.Translate("BBS.SubjectRe:ChipShop"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterDonkichi"), ShanghaiEXE.Translate("BBS.SubjectRe:ChipShop"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterTamezou"), ShanghaiEXE.Translate("BBS.SubjectRe:ChipShop"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterCorona"), ShanghaiEXE.Translate("BBS.SubjectN1LeagueOpen!"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterDonkichi"), ShanghaiEXE.Translate("BBS.SubjectRe:N1LeagueOpen!"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMakoMako"), ShanghaiEXE.Translate("BBS.SubjectRe:N1LeagueOpen!"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterCorona"), ShanghaiEXE.Translate("BBS.SubjectN1LeagueOver!"), 260),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterBillager"), ShanghaiEXE.Translate("BBS.SubjectRe:N1LeagueOver!"), 260),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterDonkichi"), ShanghaiEXE.Translate("BBS.SubjectRe:N1LeagueOver!"), 260),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMakoMako"), ShanghaiEXE.Translate("BBS.SubjectRe:N1LeagueOver!"), 260),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterCorona"), ShanghaiEXE.Translate("BBS.SubjectSeiren"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterDonkichi"), ShanghaiEXE.Translate("BBS.SubjectRe:Seiren"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMakoMako"), ShanghaiEXE.Translate("BBS.SubjectYorikaTravel"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterDonkichi"), ShanghaiEXE.Translate("BBS.SubjectRe:YorikaTravel"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterTamezou"), ShanghaiEXE.Translate("BBS.SubjectRe:YorikaTravel"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterBillager"), ShanghaiEXE.Translate("BBS.SubjectRe:YorikaTravel"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMakoMako"), ShanghaiEXE.Translate("BBS.SubjectPeaceful,Huh?"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterDonkichi"), ShanghaiEXE.Translate("BBS.SubjectRe:Peaceful,Huh?"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterCorona"), ShanghaiEXE.Translate("BBS.SubjectRe:Peaceful,Huh?"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNetAgent"), ShanghaiEXE.Translate("BBS.SubjectAlert"), 531),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterBillager"), ShanghaiEXE.Translate("BBS.SubjectRe:HeadsUp"), 531),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMakoMako"), ShanghaiEXE.Translate("BBS.SubjectRe:HeadsUp"), 531),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterDonkichi"), ShanghaiEXE.Translate("BBS.SubjectRe:HeadsUp"), 531),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterTamezou"), ShanghaiEXE.Translate("BBS.SubjectWar!"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMakoMako"), ShanghaiEXE.Translate("BBS.SubjectRe:War!"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterDonkichi"), ShanghaiEXE.Translate("BBS.SubjectRe:War!"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterCorona"), ShanghaiEXE.Translate("BBS.SubjectRe:War!"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMakoMako"), ShanghaiEXE.Translate("BBS.SubjectRe:War!"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMakoMako"), ShanghaiEXE.Translate("BBS.SubjectPeace!"), 619),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterDonkichi"), ShanghaiEXE.Translate("BBS.SubjectRe:Peace!"), 619)
            },
            // City
            new ForumItem[42]
            {
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterYanFan"), ShanghaiEXE.Translate("BBS.SubjectRegularChip"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMars"), ShanghaiEXE.Translate("BBS.SubjectRe:RegularChip"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterRenkoSis"), ShanghaiEXE.Translate("BBS.SubjectRe:RegularChip"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMesh"), ShanghaiEXE.Translate("BBS.SubjectRe:RegularChip"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterChoChon"), ShanghaiEXE.Translate("BBS.SubjectAmazingChip"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterRenkoSis"), ShanghaiEXE.Translate("BBS.SubjectRe:AmazingChip"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterChoChon"), ShanghaiEXE.Translate("BBS.SubjectRe:AmazingChip"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterYanFan"), ShanghaiEXE.Translate("BBS.SubjectRe:AmazingChip"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterRenkoSis"), ShanghaiEXE.Translate("BBS.SubjectRe:AmazingChip"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMesh"), ShanghaiEXE.Translate("BBS.SubjectAddOnManager"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterParasol"), ShanghaiEXE.Translate("BBS.SubjectRe:AddOnManager"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMars"), ShanghaiEXE.Translate("BBS.SubjectRe:AddOnManager"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterParasol"), ShanghaiEXE.Translate("BBS.SubjectRe:AddOnManager"), 156),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterChoChon"), ShanghaiEXE.Translate("BBS.SubjectGrasshopper"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMesh"), ShanghaiEXE.Translate("BBS.SubjectRe:Grasshopper"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterRenkoSis"), ShanghaiEXE.Translate("BBS.SubjectRe:Grasshopper"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterChoChon"), ShanghaiEXE.Translate("BBS.SubjectN1League"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterRenkoSis"), ShanghaiEXE.Translate("BBS.SubjectRe:N1League"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMars"), ShanghaiEXE.Translate("BBS.SubjectRe:N1League"), 225),

                new ForumItem(ShanghaiEXE.Translate("BBS.PosterElerai"), ShanghaiEXE.Translate("BBS.SubjectSPHunting"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMars"), ShanghaiEXE.Translate("BBS.SubjectRe:SPHunting"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterElerai"), ShanghaiEXE.Translate("BBS.SubjectRe:SPHunting"), 225),

                new ForumItem(ShanghaiEXE.Translate("BBS.PosterChoChon"), ShanghaiEXE.Translate("BBS.SubjectRe:N1League"), 260),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterRenkoSis"), ShanghaiEXE.Translate("BBS.SubjectUtterDefeat!"), 260),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMars"), ShanghaiEXE.Translate("BBS.SubjectRe:UtterDefeat!"), 260),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterParasol"), ShanghaiEXE.Translate("BBS.SubjectCubeSecrets"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterRenkoSis"), ShanghaiEXE.Translate("BBS.SubjectRe:CubeSecrets"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterParasol"), ShanghaiEXE.Translate("BBS.SubjectRe:CubeSecrets"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMesh"), ShanghaiEXE.Translate("BBS.SubjectKnifeThrow?"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterRenkoSis"), ShanghaiEXE.Translate("BBS.SubjectRe:KnifeThrow?"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMesh"), ShanghaiEXE.Translate("BBS.SubjectRe:KnifeThrow?"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterChoChon"), ShanghaiEXE.Translate("BBS.SubjectChipCommands"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterChiko"), ShanghaiEXE.Translate("BBS.SubjectRe:ChipCommands"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterChoChon"), ShanghaiEXE.Translate("BBS.SubjectRe:ChipCommands"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterParasol"), ShanghaiEXE.Translate("BBS.SubjectCoreAndHertz"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMars"), ShanghaiEXE.Translate("BBS.SubjectRe:CoreAndHertz"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterParasol"), ShanghaiEXE.Translate("BBS.SubjectRequest!"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterYanFan"), ShanghaiEXE.Translate("BBS.SubjectRe:Request!"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterParasol"), ShanghaiEXE.Translate("BBS.SubjectRe:Request!"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterMesh"), ShanghaiEXE.Translate("BBS.SubjectMinusAddOn"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterRenkoSis"), ShanghaiEXE.Translate("BBS.SubjectRe:MinusAddOn"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterParasol"), ShanghaiEXE.Translate("BBS.SubjectRe:MinusAddOn"), 610)
            },
            // Eien
            new ForumItem[33]
            {
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterLordUsa"), ShanghaiEXE.Translate("BBS.SubjectBudgetChat"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterTowa"), ShanghaiEXE.Translate("BBS.SubjectSubChips"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterCrabMiso"), ShanghaiEXE.Translate("BBS.SubjectStatuses"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterTowa"), ShanghaiEXE.Translate("BBS.SubjectRe:Statuses"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterShinono"), ShanghaiEXE.Translate("BBS.SubjectHiddenPath"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterLordUsa"), ShanghaiEXE.Translate("BBS.SubjectMysteryData"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterTowa"), ShanghaiEXE.Translate("BBS.SubjectStatusImmunity"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterPeperon"), ShanghaiEXE.Translate("BBS.SubjectElements"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterLordUsa"), ShanghaiEXE.Translate("BBS.SubjectTerrain"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterPeperon"), ShanghaiEXE.Translate("BBS.SubjectRun!"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterShinono"), ShanghaiEXE.Translate("BBS.SubjectRe:Run!"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterCrabMiso"), ShanghaiEXE.Translate("BBS.SubjectBustingRank"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterTowa"), ShanghaiEXE.Translate("BBS.SubjectRe:BustingRank"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterCrabMiso"), ShanghaiEXE.Translate("BBS.SubjectHiddenEffect"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterTowa"), ShanghaiEXE.Translate("BBS.SubjectRe:HiddenEffect"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterLordUsa"), ShanghaiEXE.Translate("BBS.SubjectRe:HiddenEffect"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterShinono"), ShanghaiEXE.Translate("BBS.SubjectAmazingPoison"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterLordUsa"), ShanghaiEXE.Translate("BBS.SubjectRe:AmazingPoison"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterTowa"), ShanghaiEXE.Translate("BBS.SubjectFirePrivileges"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterShinono"), ShanghaiEXE.Translate("BBS.Subject＊Code"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterPeperon"), ShanghaiEXE.Translate("BBS.SubjectRe:＊Code"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterLordUsa"), ShanghaiEXE.Translate("BBS.SubjectRe:＊Code"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterShinono"), ShanghaiEXE.Translate("BBS.SubjectRe:＊Code"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterCrabMiso"), ShanghaiEXE.Translate("BBS.SubjectHiddenVersion"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterShinono"), ShanghaiEXE.Translate("BBS.SubjectRe:HiddenVersion"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterPeperon"), ShanghaiEXE.Translate("BBS.SubjectRe:HiddenVersion"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterCrabMiso"), ShanghaiEXE.Translate("BBS.SubjectRe:HiddenVersion"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterPeperon"), ShanghaiEXE.Translate("BBS.SubjectRe:HiddenVersion"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterTowa"), ShanghaiEXE.Translate("BBS.SubjectInterior"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterLordUsa"), ShanghaiEXE.Translate("BBS.SubjectRe:Interior"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterShinono"), ShanghaiEXE.Translate("BBS.SubjectRe:Interior"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterCrabMiso"), ShanghaiEXE.Translate("BBS.SubjectRe:Interior"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterTowa"), ShanghaiEXE.Translate("BBS.SubjectRe:Interior"), 788)
            },
            // Undernet1
            new ForumItem[60]
            {
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectUnderChat"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectDarkChips"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectLawnMowing"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectDarkChipLady"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectTypo"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectVirusBall"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:VirusBall"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectErrorFrags"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectBugFrags"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectROMNavi"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:ROMNavi"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:ROMNavi"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:ROMNavi"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectDragonVirus"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:DragonVirus"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:DragonVirus"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectCybeast"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:Cybeast"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectHunters?"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:Hunters?"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:Hunters?"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:Hunters?"), 225),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectDarkCorruption?"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:DarkCorruption?"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectLegendaryNavi"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:LegendaryNavi"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:LegendaryNavi"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:LegendaryNavi"), 291),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectDarkChipSeller"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:DarkChipSeller"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:DarkChipSeller"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectSPVirus"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:SPVirus"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:SPVirus"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:SPVirus"), 452),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectDeepUnder"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:DeepUnder"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:DeepUnder"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:DeepUnder"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:DeepUnder"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:DeepUnder"), 543),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:DeepUnder"), 543),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:DeepUnder"), 543),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectROM"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:ROM"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectSPVirusHunt"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:SPVirusHunt"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:SPVirusHunt"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:SPVirusHunt"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectGhost?"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:Ghost?"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectNetHeaven"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:NetHeaven"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:NetHeaven"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectNetHell"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:NetHell"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:NetHell"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:NetHell"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:NetHell"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterNONAME"), ShanghaiEXE.Translate("BBS.SubjectRe:NetHell"), 788)
            },
            // Undernet2
            new ForumItem[31]
            {
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectPAInfo"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:PAInfo"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:PAInfo"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:PAInfo"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:PAInfo"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:PAInfo"), -1),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectDarkPA"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:DarkPA"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:DarkPA"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectHugeDamage"), 527),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:HugeDamage"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:HugeDamage"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:HugeDamage"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:HugeDamage"), 610),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectNaviPA"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:NaviPA"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:NaviPA"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:NaviPA"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectPASpam"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:PASpam"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:PASpam"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:PASpam"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectChips"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:Chips"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectFirewall"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:Firewall"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:Firewall"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:Firewall"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:Firewall"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:Firewall"), 788),
                new ForumItem(ShanghaiEXE.Translate("BBS.PosterＮＯＮＡＭＥ"), ShanghaiEXE.Translate("BBS.SubjectRe:Firewall"), 788),
            }
        };




        private readonly List<string> questlist = new List<string>();
        private readonly List<int> forumlistNumber = new List<int>();
        private BBS.SCENE nowscene;
        private readonly EventManager eventmanager;
        private byte alpha;
        protected const string texture = "menuwindows";
        private bool shopmode;
        private int cursol;
        private int cursolanime;
        private readonly int overTop;
        private int top;
        private int waittime;
        private const byte waitlong = 10;
        private const byte waitshort = 4;
        private readonly InfoMessage info;
        private readonly int forumNo;

        private int Select
        {
            get
            {
                return this.cursol + this.top;
            }
        }

        public BBS(IAudioEngine s, EventManager m, Player player, int forumNo, SaveData save)
          : base(s, m, save)
        {
            this.forumNo = forumNo;
            this.info = player.info;
            this.eventmanager = new EventManager(this.sound);
            this.NoTimeNext = false;
            for (int index = 0; index < this.forumlist[forumNo].Length; ++index)
            {
                bool flag = true;
                if (this.forumlist[forumNo][index].flag >= 0 && !this.savedata.FlagList[this.forumlist[forumNo][index].flag])
                    flag = false;
                if (flag)
                    this.forumlistNumber.Insert(0, index);
            }
            this.overTop = this.forumlistNumber.Count - 8;
            if (this.overTop >= 0)
                return;
            this.overTop = 0;
        }

        public override void Update()
        {
            switch (this.nowscene)
            {
                case BBS.SCENE.fadein:
                    if (!this.shopmode)
                    {
                        this.alpha += 51;
                        if (this.alpha >= byte.MaxValue)
                        {
                            this.alpha = byte.MaxValue;
                            this.shopmode = true;
                            break;
                        }
                        break;
                    }
                    this.alpha -= 51;
                    if (this.alpha <= 0)
                    {
                        this.savedata.selectQuestion = 1;
                        this.nowscene = BBS.SCENE.select;
                    }
                    break;
                case BBS.SCENE.select:
                    if (this.eventmanager.playevent)
                    {
                        this.eventmanager.UpDate();
                        break;
                    }
                    this.Control();
                    break;
                case BBS.SCENE.fadeout:
                    if (this.shopmode)
                    {
                        this.alpha += 51;
                        if (this.alpha >= byte.MaxValue)
                        {
                            this.alpha = byte.MaxValue;
                            this.shopmode = false;
                            break;
                        }
                        break;
                    }
                    this.alpha -= 51;
                    if (this.alpha <= 0)
                    {
                        this.nowscene = BBS.SCENE.fadein;
                        this.cursol = 0;
                        this.top = 0;
                        this.EndCommand();
                    }
                    break;
            }
            this.FlameControl(10);
            if (!this.moveflame)
                return;
            ++this.cursolanime;
            if (this.cursolanime >= 3)
                this.cursolanime = 0;
        }

        private void Control()
        {
            if (Input.IsPress(Button._A))
                this.MessageMake();
            else if (Input.IsPress(Button._B))
            {
                this.sound.PlaySE(SoundEffect.cancel);
                this.nowscene = BBS.SCENE.fadeout;
            }
            else if (this.waittime <= 0)
            {
                if (Input.IsPush(Button.Up))
                {
                    if (this.Select <= 0)
                        return;
                    if (this.cursol > 0)
                        --this.cursol;
                    else
                        --this.top;
                    this.sound.PlaySE(SoundEffect.movecursol);
                    this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                }
                else
                {
                    if (!Input.IsPush(Button.Down) || this.Select >= this.forumlistNumber.Count - 1)
                        return;
                    if (this.cursol < 7)
                        ++this.cursol;
                    else
                        ++this.top;
                    this.sound.PlaySE(SoundEffect.movecursol);
                    this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                }
            }
            else
                --this.waittime;
        }

        private void MessageMake()
        {
            int index = this.forumlistNumber[this.Select];
            this.savedata.bbsRead[this.forumNo, index] = true;
            this.eventmanager.events.Clear();
            this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
            var forumMsgIndex = this.forumNo + 10;
            this.eventmanager.EventPass(this.info.GetMessage(forumMsgIndex, index));
            this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
        }

        public override void Render(IRenderer dg)
        {
            if (this.shopmode)
            {
                this._position = new Vector2(0.0f, 0.0f);
                this._rect = new Rectangle(240, 784, 240, 160);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                for (int index1 = 0; index1 < Math.Min(8, this.forumlistNumber.Count); ++index1)
                {
                    int index2 = this.forumlistNumber[this.top + index1];
                    if (!this.savedata.bbsRead[this.forumNo, index2])
                    {
                        this._rect = new Rectangle(528 + (this.cursolanime % 2 == 0 ? 0 : 16), 600, 16, 16);
                        this._position = new Vector2(24f, 16 + 16 * index1);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    }
                    this._position = new Vector2(48f, 17 + 16 * index1);
                    dg.DrawMiniText(this.forumlist[this.forumNo][index2].name, this._position, Color.FromArgb(32, 32, 32));
                    this._position = new Vector2(128f, 17 + 16 * index1);
                    dg.DrawMiniText(this.forumlist[this.forumNo][index2].title, this._position, Color.FromArgb(32, 32, 32));
                }
                int num1 = 0;
                foreach (int index in this.forumlistNumber)
                {
                    if (!this.savedata.bbsRead[this.forumNo, this.forumlistNumber[index]])
                        ++num1;
                }
                string txt = num1.ToString() + "/" + this.forumlistNumber.Count.ToString();
                this.TextRender(dg, txt, true, new Vector2(224f, 0.0f), false);
                this._rect = new Rectangle(528 + (this.cursolanime % 2 == 0 ? 0 : 16), 600, 16, 16);
                this._position = new Vector2(144f, 0.0f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                this._rect = new Rectangle(240 + 16 * this.cursolanime, 48, 16, 16);
                this._position = new Vector2(4f, 16 + this.cursol * 16);
                dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
                float num2 = this.overTop != 0 && this.top != 0 ? 128f / overTop * top : 0.0f;
                this._rect = new Rectangle(176, 168, 8, 8);
                this._position = new Vector2(232f, 16f + num2);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            if (this.eventmanager.playevent)
                this.eventmanager.Render(dg);
            if (this.alpha <= 0)
                return;
            Color color = Color.FromArgb(alpha, Color.Black);
            Rectangle _rect = new Rectangle(0, 0, 240, 160);
            Vector2 _point = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", _rect, true, _point, color);
        }

        private enum SCENE
        {
            fadein,
            select,
            fadeout,
        }
    }
}
