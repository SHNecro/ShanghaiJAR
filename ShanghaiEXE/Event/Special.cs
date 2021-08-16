using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using System;
using NSMap.Character.Menu;
using System.Linq;
using NSChip;
using System.Collections.Generic;

namespace NSEvent
{
    internal class Special : EventBase
    {
        private readonly int ID;

        public Special(IAudioEngine s, EventManager m, int id, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.ID = id;
        }

        public override void Update()
        {
            switch (this.ID)
            {
                case 0:
                    this.manager.parent.main.FolderSave();
                    int index1 = 1;
                    for (int index2 = 0; index2 < this.savedata.chipFolder.GetLength(1); ++index2)
                    {
                        for (int index3 = 0; index3 < this.savedata.chipFolder.GetLength(2); ++index3)
                        {
                            if (index2 < 3)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index1, index2, index3] = 2;
                                else
                                    this.savedata.chipFolder[index1, index2, index3] = 0;
                            }
                            else if (index2 < 6)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index1, index2, index3] = 14;
                                else
                                    this.savedata.chipFolder[index1, index2, index3] = 1;
                            }
                            else if (index2 < 8)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index1, index2, index3] = 36;
                                else
                                    this.savedata.chipFolder[index1, index2, index3] = 0;
                            }
                            else if (index2 < 10)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index1, index2, index3] = 67;
                                else
                                    this.savedata.chipFolder[index1, index2, index3] = 1;
                            }
                            else if (index2 < 12)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index1, index2, index3] = 72;
                                else
                                    this.savedata.chipFolder[index1, index2, index3] = 1;
                            }
                            else if (index2 < 14)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index1, index2, index3] = 74;
                                else
                                    this.savedata.chipFolder[index1, index2, index3] = 2;
                            }
                            else if (index2 < 16)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index1, index2, index3] = 103;
                                else
                                    this.savedata.chipFolder[index1, index2, index3] = 1;
                            }
                            else if (index2 < 17)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index1, index2, index3] = 139;
                                else
                                    this.savedata.chipFolder[index1, index2, index3] = 2;
                            }
                            else if (index2 < 19)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index1, index2, index3] = 148;
                                else
                                    this.savedata.chipFolder[index1, index2, index3] = 0;
                            }
                            else if (index2 < 22)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index1, index2, index3] = 158;
                                else
                                    this.savedata.chipFolder[index1, index2, index3] = 0;
                            }
                            else if (index2 < 25)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index1, index2, index3] = 176;
                                else
                                    this.savedata.chipFolder[index1, index2, index3] = 0;
                            }
                            else if (index2 < 26)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index1, index2, index3] = 177;
                                else
                                    this.savedata.chipFolder[index1, index2, index3] = 0;
                            }
                            else if (index2 < 27)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index1, index2, index3] = 182;
                                else
                                    this.savedata.chipFolder[index1, index2, index3] = 3;
                            }
                            else if (index2 < 29)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index1, index2, index3] = 188;
                                else
                                    this.savedata.chipFolder[index1, index2, index3] = 0;
                            }
                            else if (index2 < 30)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index1, index2, index3] = 190;
                                else
                                    this.savedata.chipFolder[index1, index2, index3] = 0;
                            }
                        }
                    }
                    this.savedata.havefolder[1] = true;
                    this.savedata.datelist[1] = true;
                    this.savedata.datelist[13] = true;
                    this.savedata.datelist[35] = true;
                    this.savedata.datelist[66] = true;
                    this.savedata.datelist[71] = true;
                    this.savedata.datelist[73] = true;
                    this.savedata.datelist[102] = true;
                    this.savedata.datelist[138] = true;
                    this.savedata.datelist[147] = true;
                    this.savedata.datelist[157] = true;
                    this.savedata.datelist[175] = true;
                    this.savedata.datelist[176] = true;
                    this.savedata.datelist[181] = true;
                    this.savedata.datelist[187] = true;
                    this.savedata.datelist[189] = true;
                    this.manager.parent.main.FolderLoad();
                    break;
                case 1:
                    this.manager.parent.main.FolderSave();
                    int index4 = 2;
                    for (int index2 = 0; index2 < this.savedata.chipFolder.GetLength(1); ++index2)
                    {
                        for (int index3 = 0; index3 < this.savedata.chipFolder.GetLength(2); ++index3)
                        {
                            if (index2 <= 1)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index4, index2, index3] = 1;
                                else
                                    this.savedata.chipFolder[index4, index2, index3] = 0;
                            }
                            else if (index2 <= 3)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index4, index2, index3] = 1;
                                else
                                    this.savedata.chipFolder[index4, index2, index3] = 1;
                            }
                            else if (index2 <= 5)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index4, index2, index3] = 59;
                                else
                                    this.savedata.chipFolder[index4, index2, index3] = 0;
                            }
                            else if (index2 <= 7)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index4, index2, index3] = 59;
                                else
                                    this.savedata.chipFolder[index4, index2, index3] = 2;
                            }
                            else if (index2 <= 10)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index4, index2, index3] = 62;
                                else
                                    this.savedata.chipFolder[index4, index2, index3] = 1;
                            }
                            else if (index2 <= 13)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index4, index2, index3] = 71;
                                else
                                    this.savedata.chipFolder[index4, index2, index3] = 2;
                            }
                            else if (index2 <= 15)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index4, index2, index3] = 43;
                                else
                                    this.savedata.chipFolder[index4, index2, index3] = 1;
                            }
                            else if (index2 <= 17)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index4, index2, index3] = 43;
                                else
                                    this.savedata.chipFolder[index4, index2, index3] = 2;
                            }
                            else if (index2 <= 20)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index4, index2, index3] = 121;
                                else
                                    this.savedata.chipFolder[index4, index2, index3] = 1;
                            }
                            else if (index2 <= 22)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index4, index2, index3] = 158;
                                else
                                    this.savedata.chipFolder[index4, index2, index3] = 1;
                            }
                            else if (index2 <= 26)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index4, index2, index3] = 174;
                                else
                                    this.savedata.chipFolder[index4, index2, index3] = 2;
                            }
                            else if (index3 == 0)
                                this.savedata.chipFolder[index4, index2, index3] = 188;
                            else
                                this.savedata.chipFolder[index4, index2, index3] = 0;
                        }
                    }
                    this.savedata.havefolder[2] = true;
                    this.savedata.datelist[0] = true;
                    this.savedata.datelist[58] = true;
                    this.savedata.datelist[61] = true;
                    this.savedata.datelist[70] = true;
                    this.savedata.datelist[42] = true;
                    this.savedata.datelist[120] = true;
                    this.savedata.datelist[157] = true;
                    this.savedata.datelist[173] = true;
                    this.savedata.datelist[187] = true;
                    this.savedata.foldername = ShanghaiEXE.Translate("Special.ExtraFolder");
                    this.manager.parent.main.FolderLoad();
                    this.manager.parent.main.FolderSave();
                    this.manager.parent.main.FolderReset();
                    break;
                case 2:
                    this.manager.parent.main.FolderSave();
                    int index5 = 2;
                    for (int index2 = 0; index2 < this.savedata.chipFolder.GetLength(1); ++index2)
                    {
                        for (int index3 = 0; index3 < this.savedata.chipFolder.GetLength(2); ++index3)
                        {
                            if (index2 <= 3)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index5, index2, index3] = 14;
                                else
                                    this.savedata.chipFolder[index5, index2, index3] = 0;
                            }
                            else if (index2 <= 5)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index5, index2, index3] = 21;
                                else
                                    this.savedata.chipFolder[index5, index2, index3] = 0;
                            }
                            else if (index2 <= 7)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index5, index2, index3] = 41;
                                else
                                    this.savedata.chipFolder[index5, index2, index3] = 0;
                            }
                            else if (index2 <= 10)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index5, index2, index3] = 63;
                                else
                                    this.savedata.chipFolder[index5, index2, index3] = 1;
                            }
                            else if (index2 <= 13)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index5, index2, index3] = 112;
                                else
                                    this.savedata.chipFolder[index5, index2, index3] = 0;
                            }
                            else if (index2 <= 17)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index5, index2, index3] = 122;
                                else
                                    this.savedata.chipFolder[index5, index2, index3] = 1;
                            }
                            else if (index2 <= 20)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index5, index2, index3] = 133;
                                else
                                    this.savedata.chipFolder[index5, index2, index3] = 3;
                            }
                            else if (index2 <= 24)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index5, index2, index3] = 175;
                                else
                                    this.savedata.chipFolder[index5, index2, index3] = 1;
                            }
                            else if (index2 <= 26)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index5, index2, index3] = 183;
                                else
                                    this.savedata.chipFolder[index5, index2, index3] = 0;
                            }
                            else if (index2 <= 28)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index5, index2, index3] = 188;
                                else
                                    this.savedata.chipFolder[index5, index2, index3] = 0;
                            }
                            else if (index3 == 0)
                                this.savedata.chipFolder[index5, index2, index3] = 190;
                            else
                                this.savedata.chipFolder[index5, index2, index3] = 0;
                        }
                    }
                    this.savedata.havefolder[2] = true;
                    this.savedata.datelist[14] = true;
                    this.savedata.datelist[21] = true;
                    this.savedata.datelist[41] = true;
                    this.savedata.datelist[63] = true;
                    this.savedata.datelist[112] = true;
                    this.savedata.datelist[122] = true;
                    this.savedata.datelist[133] = true;
                    this.savedata.datelist[175] = true;
                    this.savedata.datelist[183] = true;
                    this.savedata.datelist[188] = true;
                    this.savedata.datelist[190] = true;
                    this.savedata.foldername = ShanghaiEXE.Translate("Special.N1Folder");
                    this.manager.parent.main.FolderLoad();
                    this.manager.parent.main.FolderSave();
                    this.manager.parent.main.FolderReset();
                    break;
                case 3:
                    this.manager.parent.main.FolderSave();
                    int index6 = 2;
                    for (int index2 = 0; index2 < this.savedata.chipFolder.GetLength(1); ++index2)
                    {
                        for (int index3 = 0; index3 < this.savedata.chipFolder.GetLength(2); ++index3)
                        {
                            if (index2 <= 1)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index6, index2, index3] = 27;
                                else
                                    this.savedata.chipFolder[index6, index2, index3] = 3;
                            }
                            else if (index2 <= 4)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index6, index2, index3] = 46;
                                else
                                    this.savedata.chipFolder[index6, index2, index3] = 0;
                            }
                            else if (index2 <= 6)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index6, index2, index3] = 113;
                                else
                                    this.savedata.chipFolder[index6, index2, index3] = 0;
                            }
                            else if (index2 <= 8)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index6, index2, index3] = 23;
                                else
                                    this.savedata.chipFolder[index6, index2, index3] = 0;
                            }
                            else if (index2 <= 11)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index6, index2, index3] = 33;
                                else
                                    this.savedata.chipFolder[index6, index2, index3] = 3;
                            }
                            else if (index2 <= 12)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index6, index2, index3] = 52;
                                else
                                    this.savedata.chipFolder[index6, index2, index3] = 0;
                            }
                            else if (index2 <= 15)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index6, index2, index3] = 50;
                                else
                                    this.savedata.chipFolder[index6, index2, index3] = 0;
                            }
                            else if (index2 <= 17)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index6, index2, index3] = 163;
                                else
                                    this.savedata.chipFolder[index6, index2, index3] = 0;
                            }
                            else if (index2 <= 20)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index6, index2, index3] = 106;
                                else
                                    this.savedata.chipFolder[index6, index2, index3] = 1;
                            }
                            else if (index2 <= 21)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index6, index2, index3] = 63;
                                else
                                    this.savedata.chipFolder[index6, index2, index3] = 3;
                            }
                            else if (index2 <= 22)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index6, index2, index3] = 64;
                                else
                                    this.savedata.chipFolder[index6, index2, index3] = 2;
                            }
                            else if (index2 <= 23)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index6, index2, index3] = 65;
                                else
                                    this.savedata.chipFolder[index6, index2, index3] = 3;
                            }
                            else if (index2 <= 26)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index6, index2, index3] = 134;
                                else
                                    this.savedata.chipFolder[index6, index2, index3] = 0;
                            }
                            else if (index2 <= 27)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index6, index2, index3] = 178;
                                else
                                    this.savedata.chipFolder[index6, index2, index3] = 3;
                            }
                            else if (index2 <= 28)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index6, index2, index3] = 158;
                                else
                                    this.savedata.chipFolder[index6, index2, index3] = 3;
                            }
                            else if (index3 == 0)
                                this.savedata.chipFolder[index6, index2, index3] = 190;
                            else
                                this.savedata.chipFolder[index6, index2, index3] = 0;
                        }
                        this.savedata.datelist[this.savedata.chipFolder[index6, index2, 0]] = true;
                    }
                    this.savedata.havefolder[2] = true;
                    this.savedata.foldername = ShanghaiEXE.Translate("Special.SeirenFolder");
                    this.manager.parent.main.FolderLoad();
                    this.manager.parent.main.FolderSave();
                    this.manager.parent.main.FolderReset();
                    break;
                case 4:
                    this.manager.parent.main.FolderSave();
                    int index7 = 2;
                    for (int index2 = 0; index2 < this.savedata.chipFolder.GetLength(1); ++index2)
                    {
                        for (int index3 = 0; index3 < this.savedata.chipFolder.GetLength(2); ++index3)
                        {
                            if (index2 <= 2)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 4;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 0;
                            }
                            else if (index2 <= 3)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 9;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 3;
                            }
                            else if (index2 <= 5)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 15;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 0;
                            }
                            else if (index2 <= 7)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 37;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 3;
                            }
                            else if (index2 <= 10)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 44;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 2;
                            }
                            else if (index2 <= 11)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 85;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 0;
                            }
                            else if (index2 <= 12)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 87;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 0;
                            }
                            else if (index2 <= 13)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 89;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 2;
                            }
                            else if (index2 <= 14)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 90;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 3;
                            }
                            else if (index2 <= 15)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 120;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 0;
                            }
                            else if (index2 <= 16)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 135;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 0;
                            }
                            else if (index2 <= 19)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 144;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 2;
                            }
                            else if (index2 <= 21)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 169;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 2;
                            }
                            else if (index2 <= 23)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 171;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 3;
                            }
                            else if (index2 <= 25)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 142;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 0;
                            }
                            else if (index2 <= 26)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 221;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 0;
                            }
                            else if (index2 <= 27)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = 227;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 0;
                            }
                            else if (index2 <= 28)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index7, index2, index3] = byte.MaxValue;
                                else
                                    this.savedata.chipFolder[index7, index2, index3] = 0;
                            }
                            else if (index3 == 0)
                                this.savedata.chipFolder[index7, index2, index3] = 258;
                            else
                                this.savedata.chipFolder[index7, index2, index3] = 0;
                        }
                        this.savedata.datelist[this.savedata.chipFolder[index7, index2, 0]] = true;
                    }
                    this.savedata.havefolder[2] = true;
                    this.savedata.foldername = ShanghaiEXE.Translate("Special.AkinFolder");
                    this.manager.parent.main.FolderLoad();
                    this.manager.parent.main.FolderSave();
                    this.manager.parent.main.FolderReset();
                    break;
                case 5:
                    this.manager.parent.main.FolderSave();
                    int index8 = 2;
                    for (int index2 = 0; index2 < this.savedata.chipFolder.GetLength(1); ++index2)
                    {
                        for (int index3 = 0; index3 < this.savedata.chipFolder.GetLength(2); ++index3)
                        {
                            if (index2 <= 1)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index8, index2, index3] = 58;
                                else
                                    this.savedata.chipFolder[index8, index2, index3] = 3;
                            }
                            else if (index2 <= 3)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index8, index2, index3] = 89;
                                else
                                    this.savedata.chipFolder[index8, index2, index3] = 1;
                            }
                            else if (index2 <= 5)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index8, index2, index3] = 23;
                                else
                                    this.savedata.chipFolder[index8, index2, index3] = 3;
                            }
                            else if (index2 <= 7)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index8, index2, index3] = 97;
                                else
                                    this.savedata.chipFolder[index8, index2, index3] = 2;
                            }
                            else if (index2 <= 8)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index8, index2, index3] = 104;
                                else
                                    this.savedata.chipFolder[index8, index2, index3] = 2;
                            }
                            else if (index2 <= 9)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index8, index2, index3] = 154;
                                else
                                    this.savedata.chipFolder[index8, index2, index3] = 2;
                            }
                            else if (index2 <= 10)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index8, index2, index3] = 155;
                                else
                                    this.savedata.chipFolder[index8, index2, index3] = 2;
                            }
                            else if (index2 <= 12)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index8, index2, index3] = 50;
                                else
                                    this.savedata.chipFolder[index8, index2, index3] = 2;
                            }
                            else if (index2 <= 14)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index8, index2, index3] = 8;
                                else
                                    this.savedata.chipFolder[index8, index2, index3] = 0;
                            }
                            else if (index2 <= 17)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index8, index2, index3] = 101;
                                else
                                    this.savedata.chipFolder[index8, index2, index3] = 0;
                            }
                            else if (index2 <= 19)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index8, index2, index3] = 95;
                                else
                                    this.savedata.chipFolder[index8, index2, index3] = 0;
                            }
                            else if (index2 <= 21)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index8, index2, index3] = 125;
                                else
                                    this.savedata.chipFolder[index8, index2, index3] = 0;
                            }
                            else if (index2 <= 22)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index8, index2, index3] = 150;
                                else
                                    this.savedata.chipFolder[index8, index2, index3] = 0;
                            }
                            else if (index2 <= 24)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index8, index2, index3] = 157;
                                else
                                    this.savedata.chipFolder[index8, index2, index3] = 0;
                            }
                            else if (index2 <= 26)
                            {
                                if (index3 == 0)
                                    this.savedata.chipFolder[index8, index2, index3] = 165;
                                else
                                    this.savedata.chipFolder[index8, index2, index3] = 3;
                            }
                            else if (index3 == 0)
                                this.savedata.chipFolder[index8, index2, index3] = 33;
                            else
                                this.savedata.chipFolder[index8, index2, index3] = 3;
                        }
                        this.savedata.datelist[this.savedata.chipFolder[index8, index2, 0]] = true;
                    }
                    this.savedata.havefolder[2] = true;
                    this.savedata.foldername = ShanghaiEXE.Translate("Special.MeijiFolder");
                    this.manager.parent.main.FolderLoad();
                    this.manager.parent.main.FolderSave();
                    this.manager.parent.main.FolderReset();
                    break;
                case 8:
                    this.savedata.firstchange = false;
                    this.savedata.FlagList[5] = true;
                    this.savedata.manybattle = 100;
                    break;
                case 9:
                    this.savedata.havePeace[0] -= 100;
                    break;
                case 10:
                    this.savedata.AddChip(310, 0, true);
                    this.savedata.haveCaptureBomb = 1;
                    this.savedata.canselectmenu[4] = true;
                    break;
                case 11:
                    this.savedata.FlagList[68] = true;
                    this.savedata.haveCaptureBomb = 2;
                    this.savedata.canselectmenu[4] = true;
                    break;
                case 12:
                    this.savedata.haveCaptureBomb = 3;
                    this.savedata.canselectmenu[4] = true;
                    break;
                case 13:
                    this.savedata.efolder = 2;
                    this.savedata.FlagList[8] = true;
                    break;
                case 14:
                    for (int index2 = 0; index2 < 20; ++index2)
                        this.savedata.FlagList[40 + index2] = false;
                    break;
                case 15:
                    this.manager.parent.eventmanagerParallel.ClearEvent();
                    this.manager.parent.eventmanagerParallel.playevent = false;
                    break;
                case 16:
                    if (this.savedata.haveSubChis[6] > 0)
                    {
                        this.savedata.selectQuestion = 1;
                    }
                    break;
                case 17:
                    if (this.savedata.selectQuestion == 0 && this.savedata.haveSubChis[6] > 0)
                    {
                        this.savedata.haveSubChis[6]--;
                        this.savedata.selectQuestion = 1;
                    }
                    else
                    {
                        this.savedata.selectQuestion = 0;
                    }
                    break;
                case 18:
                    var playerPosition = this.manager.parent.Player.Position;
                    var x = playerPosition.X + 16;
                    var y = playerPosition.Y;
                    var field = this.manager.parent.Field;
                    
                    for (var octave = 3; octave <= 6; octave++)
                    {
                        foreach (var octaveNote in Note.OctaveNotes)
                        {
                            var note = $"{octaveNote}{octave}";
                            var key = field.Events.Find(e => e.ID == note);

                            var yLow = !octaveNote.EndsWith("#", StringComparison.InvariantCulture) ? 22 : 27;
                            var yHigh = !octaveNote.EndsWith("#", StringComparison.InvariantCulture) ? 30 : 34;
                            if (y >= yLow && y <= yHigh && Math.Abs(x - key.Position.X) < 5)
                            {
                                key.eventPages[0].defaultAngle = NSMap.Character.MapCharacterBase.ANGLE.UPLEFT;
                                this.sound.PlayNote(new Note(note), 127, -1);
                            }
                            else if (!this.sound.IsPlayingNote)
                            {
                                key.eventPages[0].defaultAngle = NSMap.Character.MapCharacterBase.ANGLE.DOWNRIGHT;
                                this.sound.PlayNote(new Note(note), 127, 0);
                            }
                        }
                    }
                    break;
                case 19:
                    {
                        var numBusted = 0;
                        for (var i = 1; i <= 41; i++)
                        {
                            if (this.savedata.virusSPbusted[i])
                            {
                                numBusted++;
                            }
                        }

                        this.savedata.ValList[7] = numBusted;
                    }
                    break;
                case 20:
                    {
                        var completionLibrary = new Library(this.sound, null, null, this.savedata);
                        var numStdSeen = completionLibrary.LibraryPages[Library.LibraryPageType.Normal].Chips.Count(c => c.IsSeen);
                        this.savedata.ValList[7] = numStdSeen;
                    }
                    break;
                case 21:
                    {
                        var paCompletionLibrary = new Library(this.sound, null, null, this.savedata);
                        var numPaSeen = paCompletionLibrary.LibraryPages[Library.LibraryPageType.PA].Chips.Count(c => c.IsSeen);
                        this.savedata.ValList[7] = numPaSeen;
                    }
                    break;
                case 22:
                    {
                        var completionLibrary = new Library(this.sound, null, null, this.savedata);
                        var anyIllegalNavi = completionLibrary.LibraryPages[Library.LibraryPageType.Illegal].Chips.Any(c => c.IsSeen && c.Chip.navi);
                        this.savedata.ValList[7] = anyIllegalNavi ? 1 : 0;
                    }
                    break;
                case 23:
                    {
                        var completionLibrary = new Library(this.sound, null, null, this.savedata);
                        // Repair 20,50,100,150,200,300,500, DarkRecov(1000)
                        var recovAmounts = new Dictionary<int, int>
                        {
                            { 174, 20 },
                            { 175, 50 },
                            { 176, 100 },
                            { 177, 150 },
                            { 178, 200 },
                            { 179, 300 },
                            { 180, 500 },
                            { 264, 1000 },
                        };
                        var bagRecov = recovAmounts.Sum(kvp => Enumerable.Range(0, 4).Sum(code => this.savedata.Havechip[kvp.Key, code] * kvp.Value));
                        var folderRecov = Enumerable.Range(0, 3).Sum(fNum => Enumerable.Range(0, 30).Sum(fChip =>
                        {
                            var folderChip = this.savedata.chipFolder[fNum, fChip, 0];
                            return recovAmounts.TryGetValue(folderChip, out int folderRecovChipAmount) ? folderRecovChipAmount : 0;
                        }));
                        var totalHealing = bagRecov + folderRecov;
                        var healingFound = totalHealing >= 5000;
                        //var chips = new[] { 174, 175, 176, 177, 178, 179, 180, 264 };
                        //var chipsSeen = completionLibrary.LibraryPages[Library.LibraryPageType.Normal].Chips.Where(c => chips.Contains(c.Chip.number)).All(c => c.IsSeen)
                        //    && completionLibrary.LibraryPages[Library.LibraryPageType.Dark].Chips.Where(c => chips.Contains(c.Chip.number)).All(c => c.IsSeen);
                        // HP+50,100,200,500, HP+300, ChipCure, CRecov
                        //var addons = new[] { 8, 9, 10, 11, 14, 38, 92 };
                        //var addonsObtained = addons.All(id => this.savedata.haveAddon.Any(a => a.ID == id));
                        //var subChipsObtained = this.savedata.haveSubChis[0] == 9 && this.savedata.haveSubChis[1] == 9;
                        //var healingFound = chipsSeen && addonsObtained && subChipsObtained;
                        this.savedata.ValList[7] = healingFound ? 1 : 0;
                    }
                    break;
                case 24:
                    {
                        var codes = this.savedata.havechips.Select(c => ChipFolder.Chips[c.number].Invoke(this.sound).code[c.code]).Distinct().ToList();

                        for (var folderIndex = 0; folderIndex < this.savedata.chipFolder.GetLength(0); folderIndex++)
                        {
                            for (var chipIndex = 0; chipIndex < this.savedata.chipFolder.GetLength(1); chipIndex++)
                            {
                                var chipNumber = this.savedata.chipFolder[folderIndex, chipIndex, 0];
                                if (!ChipFolder.Chips.ContainsKey(chipNumber))
                                {
                                    continue;
                                }
                                var chip = ChipFolder.Chips[chipNumber].Invoke(this.sound);
                                var code = chip.code[this.savedata.chipFolder[folderIndex, chipIndex, 1]];

                                if (!codes.Contains(code))
                                {
                                    codes.Add(code);
                                }
                            }
                        }

                        var codesFound = codes.Count >= 27;
                        this.savedata.ValList[7] = codesFound ? 1 : 0;
                    }
                    break;
                case 25:
                    {
                        var virusUpgraded = this.savedata.HaveVirus.Where(v => v != null).Any(v => v.eatSum >= 50);
                        this.savedata.ValList[7] = virusUpgraded ? 1 : 0;
                    }
                    break;
            }
            this.EndCommand();
        }

        public override void SkipUpdate()
        {
            this.Update();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
