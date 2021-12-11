using ExtensionMethods;
using NSAddOn;
using NSChip;
using NSShanghaiEXE.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Common.EncodeDecode;
using NSMap.Character.Menu;
using static NSMap.Character.Menu.Library;
using System.Linq;
using Common;
using System.Windows.Forms;
using NSMap;

namespace NSGame
{
    public class SaveData
    {
        const string SavePath = "save.she";
        const string SavePathTemp = "save.she.tmp";
        const string BackupPath = "save.she.bak";

        public static int decCount = 0;
        public static string pass = "sasanasi";
        public static Virus[] HAVEVirus = new Virus[3];
        public static string[] EXmessID = 
        {
            "LHint1",
            "LHint2",
            "QHint1",
            "QHint2",
            "Omake1",
            "Omake2",
            "Omake3",
            "Omake4",
            "Qinfo",
            "QinfoEnd",
            "BBS1",
            "BBS2",
            "BBS3",
            "BBS4",
            "BBS5",
            "BBS6"
        };
        public static int[,] Pad = {{132,118,50,76,33,35,10,28,31,12,53,78},{100,102,101,103,3,2,4,5,7,6,-1,8}};
        public static bool ScreenMode = false;
        public bool saveEnd = true;
        private bool flugEnd = true;
        private bool valEnd = true;
        private bool shopEnd = true;
        private bool mysEnd = true;
        private bool ranEnd = true;
        private bool chipEnd = true;
        public byte[] busterspec = new byte[3];
        public bool[] havefolder = new bool[3];
        public bool[] regularflag = new bool[3];
        public byte[] regularchip = new byte[3];
        public int efolder = 0;
        private byte[,] havechip = new byte[450, 4];
        public List<ChipS> havechips = new List<ChipS>();
        public Style[] style = new Style[5];
        public int[] stylepoint = new int[6];
        public int[] haveSubChis = new int[7];
        public bool[] runSubChips = new bool[4];
        public int haveSubMemory = 2;
        private int naviFolder = 5;
        public int darkFolder = 1;
        public int plusFolder = 0;
        public int custom = 0;
        public List<string> addonNames = new List<string>();
        public List<AddOnBase> haveAddon = new List<AddOnBase>();
        public List<bool> equipAddon = new List<bool>();
        public byte[] time = new byte[4];
        public int manybattle = 0;
        private int money = 0;
        public int moneyover = 10000000;
        public int haveCaptureBomb = 0;
        private Virus[] haveVirus = new Virus[3];
        public List<Virus> stockVirus = new List<Virus>();
        public int[] havePeace = new int[3];
        public bool[,] bbsRead = new bool[6, 100];
        public bool[] questEnd = new bool[50];
        // flags for whether the current bounty target has been defeated
        public bool[] virusSPbustedFlug = new bool[45];
        // flags for whether the bounty targets have ever been defeated
        public bool[] virusSPbusted = new bool[45];
        public bool firstchange = false;
        public int[,,] chipFolder = new int[3, 30, 2];
        public bool[] canselectmenu = new bool[9];
        public bool[] datelist = new bool[450];
        public bool[] addonSkill = new bool[Enum.GetNames(typeof(SaveData.ADDONSKILL)).Length];
        public int[] netWorkName = new int[10];
        public List<int[]> RirekNetWorkName = new List<int[]>();
        public List<int> RirekNetWorkFace = new List<int>();
        public List<int> RirekNetWorkAddress = new List<int>();
        public List<int> mail = new List<int>();
        public List<bool> mailread = new List<bool>();
        public List<int> keyitem = new List<int>();
        private int[,] shopCount = new int[40, 10];
        public List<Interior> interiors = new List<Interior>();
        private bool[] flagList = new bool[2000];
        private VariableArray valList = new VariableArray();
        private bool[] getMystery = new bool[600];
        private bool[] getRandomMystery = new bool[600];
        public string pluginMap = "";
        public bool loadEnd;
        public bool loadSucces;
        public bool saveEndnowsub;
        public bool saveEndnow;
        private bool attemptingBackupLoad;
        private string fluglist;
        private string vallist;
        private string shoplist;
        private string myslist;
        private string ranlist;
        private string chiplist;
        public const int FolderMax = 30;
        public const int ManyChips_normal = 190;
        public const int ManyChips_navi = 64;
        public const int ManyChips_dark = 16;
        public const int ManyChips_PA = 32;
        public const int ManyMystery = 600;
        public const int ManyChips = 450;
        public const int ManyNormalChips = 270;
        public const byte ManyCode = 4;
        public const byte ManyFolder = 3;
        public const byte Folder_chip = 0;
        public const byte Folder_code = 1;
        public const byte Chip_and_code = 2;
        public const byte ManyStyles = 6;
        public const byte TopmenuSelect = 9;
        public const int ManyFlags = 2000;
        public const int ManyVariables = 200;
        public const int BBSmany = 6;
        public const int BBSpages = 100;
        public const int QuestMany = 50;
        public const int SPVirusMany = 45;
        private int hpmax;
        public int HPnow;
        public int HPplus;
        public byte regularlarge;
        private Thread chipThread;
        public string foldername;
        public const byte canhavestyles = 5;
        public int havestyles;
        public int setstyle;
        public const int ManySubChips = 7;
        public Virus v;
        private int maxhz;
        private int maxcore;
        public string plase;
        public int mind;
        public int fukasinArea;
        public const int nameMany = 10;
        public int netWorkFace;
        public int message;
        public bool isJackedIn;
        public int selectQuestion;
        private Thread shopThread;
        private Thread flagThread;
        private Thread valThread;
        private Thread mysThread;
        private Thread ranThread;
        public string nowMap;
        public float nowX;
        public float nowY;
        public float nowZ;
        public int nowFroor;
        public float pluginX;
        public float pluginY;
        public float pluginZ;
        public int pluginFroor;
        public int steptype;
        public int stepoverX;
        public int stepoverY;
        public float stepCounter;
        public bool stepmode;
        public string item;
        public string category;

        public void Load(Control parent = null)
        {
            this.loadEnd = false;
            var loadAttemptedAndFailed = false;

            SaveData.decCount = 0;
            if (!File.Exists(SavePath))
            {
                this.loadSucces = false;
            }
            else
            {
                var streamReader = default(StreamReader);
                string str = "";
                try
                {
                    streamReader = new StreamReader(SavePath, Encoding.GetEncoding("Shift_JIS"));
                    
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray1 = str.Split('@');
                    this.addonNames.Clear();
                    if ((uint)strArray1.Length > 0U)
                    {
                        for (int index = 0; index < strArray1.Length - 1; ++index)
                            this.addonNames.Add(strArray1[index]);
                    }
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray2 = str.Split('@');
                    for (int index = 0; index < strArray2.Length - 1; ++index)
                        this.busterspec[index] = byte.Parse(strArray2[index]);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray3 = str.Split('@');
                    for (int index = 0; index < strArray3.Length - 1; ++index)
                        this.canselectmenu[index] = bool.Parse(strArray3[index]);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray4 = str.Split('/');
                    for (int index1 = 0; index1 < strArray4.Length - 1; ++index1)
                    {
                        string[] strArray5 = strArray4[index1].Split('|');
                        for (int index2 = 0; index2 < strArray5.Length - 1; ++index2)
                        {
                            string[] strArray6 = strArray5[index2].Split('@');
                            this.chipFolder[index1, index2, 0] = int.Parse(strArray6[0]);
                            this.chipFolder[index1, index2, 1] = int.Parse(strArray6[1]);
                        }
                    }
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray7 = str.Split('@');
                    this.havechips.Clear();
                    for (int index = 0; index < strArray7.Length - 1; ++index)
                        this.havechips.Add(new ChipS(int.Parse(strArray7[index].Split('/')[0]), int.Parse(strArray7[index].Split('/')[1])));
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray8 = str.Split('@');
                    for (int index = 0; index < strArray8.Length - 1; ++index)
                        this.datelist[index] = bool.Parse(strArray8[index]);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.efolder = byte.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray9 = str.Split('@');
                    this.equipAddon.Clear();
                    for (int index = 0; index < strArray9.Length - 1; ++index)
                        this.equipAddon.Add(bool.Parse(strArray9[index]));
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.firstchange = bool.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.foldername = str;
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray10 = str.Split('@');
                    this.haveAddon.Clear();
                    for (int index = 0; index < strArray10.Length - 1; ++index)
                    {
                        string[] strArray5 = strArray10[index].Split('/');
                        var color = (AddOnBase.ProgramColor)Enum.Parse(typeof(AddOnBase.ProgramColor), strArray5[1]);
                        string typeName = strArray5[0].Split('.')[1];
                        var addOn = (AddOnBase)Activator.CreateInstance(typeName.ToAddOnType(), color);
                        this.haveAddon.Add(addOn);
                    }
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.haveCaptureBomb = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray11 = str.Split('/');
                    for (int index1 = 0; index1 < strArray11.Length - 1; ++index1)
                    {
                        string[] strArray5 = strArray11[index1].Split('@');
                        for (int index2 = 0; index2 < strArray5.Length - 1; ++index2)
                            this.havechip[index1, index2] = byte.Parse(strArray5[index2]);
                    }
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray12 = str.Split('@');
                    for (int index = 0; index < strArray12.Length - 1; ++index)
                        this.havefolder[index] = bool.Parse(strArray12[index]);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray13 = str.Split('@');
                    for (int index = 0; index < strArray13.Length - 1; ++index)
                        this.havePeace[index] = int.Parse(strArray13[index]);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.havestyles = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray14 = str.Split('@');
                    for (int index = 0; index < strArray14.Length - 1; ++index)
                        this.haveSubChis[index] = int.Parse(strArray14[index]);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.haveSubMemory = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray15 = str.Split('@');
                    for (int index = 0; index < strArray15.Length - 1; ++index)
                    {
                        if (strArray15[index] != "null")
                        {
                            string[] strArray5 = strArray15[index].Split('/');
                            this.HaveVirus[index] = new Virus
                            {
                                type = int.Parse(strArray5[0]),
                                eatBug = int.Parse(strArray5[1]),
                                eatError = int.Parse(strArray5[2]),
                                eatFreeze = int.Parse(strArray5[3]),
                                code = int.Parse(strArray5[4])
                            };
                        }
                        else
                            this.HaveVirus[index] = null;
                    }
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.HPmax = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.HPnow = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.HPplus = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray16 = str.Split('@');
                    this.keyitem.Clear();
                    for (int index = 0; index < strArray16.Length - 1; ++index)
                        this.keyitem.Add(int.Parse(strArray16[index]));
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray17 = str.Split('@');
                    this.mail.Clear();
                    for (int index = 0; index < strArray17.Length - 1; ++index)
                        this.mail.Add(int.Parse(strArray17[index]));
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray18 = str.Split('@');
                    this.mailread.Clear();
                    for (int index = 0; index < strArray18.Length - 1; ++index)
                        this.mailread.Add(bool.Parse(strArray18[index]));
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.manybattle = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.MaxCore = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.MaxHz = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.mind = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.Money = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.plase = str;
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.isJackedIn = bool.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray19 = str.Split('@');
                    for (int index = 0; index < strArray19.Length - 1; ++index)
                        this.regularchip[index] = byte.Parse(strArray19[index]);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray20 = str.Split('@');
                    for (int index = 0; index < strArray20.Length - 1; ++index)
                        this.regularflag[index] = bool.Parse(strArray20[index]);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.Regularlarge = byte.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray21 = str.Split('@');
                    for (int index = 0; index < strArray21.Length - 1; ++index)
                        this.runSubChips[index] = bool.Parse(strArray21[index]);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.selectQuestion = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.setstyle = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray22 = str.Split('@');
                    this.stockVirus.Clear();
                    for (int index = 0; index < strArray22.Length - 1; ++index)
                    {
                        string[] strArray5 = strArray22[index].Split('/');
                        this.stockVirus.Add(new Virus());
                        this.stockVirus[index].type = byte.Parse(strArray5[0]);
                        this.stockVirus[index].eatBug = byte.Parse(strArray5[1]);
                        this.stockVirus[index].eatError = byte.Parse(strArray5[2]);
                        this.stockVirus[index].eatFreeze = byte.Parse(strArray5[3]);
                        this.stockVirus[index].code = byte.Parse(strArray5[4]);
                    }
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray23 = str.Split('@');
                    for (int index = 0; index < strArray23.Length - 1; ++index)
                    {
                        string[] strArray5 = strArray23[index].Split('/');
                        this.style[index].style = int.Parse(strArray5[0]);
                        this.style[index].element = int.Parse(strArray5[1]);
                    }
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray24 = str.Split('@');
                    for (int index = 0; index < strArray24.Length - 1; ++index)
                        this.stylepoint[index] = int.Parse(strArray24[index]);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray25 = str.Split('@');
                    for (int index = 0; index < strArray25.Length - 1; ++index)
                        this.time[index] = byte.Parse(strArray25[index]);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray26 = str.Split('@');
                    for (int index = 0; index < strArray26.Length - 1; ++index)
                        this.virusSPbusted[index] = bool.Parse(strArray26[index]);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray27 = str.Split('@');
                    for (int index = 0; index < strArray27.Length - 1; ++index)
                        this.virusSPbustedFlug[index] = bool.Parse(strArray27[index]);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray28 = str.Split('@');
                    int index3 = -1;
                    for (int index1 = 0; index1 < this.bbsRead.GetLength(0); ++index1)
                    {
                        for (int index2 = 0; index2 < this.bbsRead.GetLength(1); ++index2)
                        {
                            ++index3;
                            this.bbsRead[index1, index2] = bool.Parse(strArray28[index3]);
                        }
                    }
                    try
                    {
                        bool flag = false;
                        str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                        string[] strArray5 = str.Split('@');
                        for (int index1 = 0; index1 < strArray5.Length - 1; ++index1)
                        {
                            flag = true;
                            this.questEnd[index1] = bool.Parse(strArray5[index1]);
                        }
                        if (flag)
                            str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                        string[] strArray6 = str.Split('/');
                        int index2 = -1;
                        for (int index1 = 0; index1 < this.shopCount.GetLength(0); ++index1)
                        {
                            for (int index4 = 0; index4 < this.shopCount.GetLength(1); ++index4)
                            {
                                ++index2;
                                this.shopCount[index1, index4] = int.Parse(strArray6[index2]);
                            }
                        }
                    }
                    catch
                    {
                        string[] strArray5 = str.Split('/');
                        int index1 = -1;
                        for (int index2 = 0; index2 < this.shopCount.GetLength(0); ++index2)
                        {
                            for (int index4 = 0; index4 < this.shopCount.GetLength(1); ++index4)
                            {
                                ++index1;
                                this.shopCount[index2, index4] = int.Parse(strArray5[index1]);
                            }
                        }
                    }
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.message = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.isJackedIn = bool.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.nowMap = str;
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.nowX = FloatParseAnySeparator(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.nowY = FloatParseAnySeparator(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.nowZ = FloatParseAnySeparator(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.nowFroor = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.steptype = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.stepoverX = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.stepoverY = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.stepCounter = FloatParseAnySeparator(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.pluginMap = str;
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.pluginX = FloatParseAnySeparator(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.pluginY = FloatParseAnySeparator(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.pluginZ = FloatParseAnySeparator(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    this.pluginFroor = int.Parse(str);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray29 = str.Split('@');
                    for (int index1 = 0; index1 < strArray29.Length - 1; ++index1)
                        this.FlagList[index1] = bool.Parse(strArray29[index1]);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray30 = str.Split('@');
                    for (int index1 = 0; index1 < strArray30.Length - 1; ++index1)
                        this.ValList[index1] = int.Parse(strArray30[index1]);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray31 = str.Split('@');
                    for (int index1 = 0; index1 < strArray31.Length - 1; ++index1)
                        this.GetMystery[index1] = bool.Parse(strArray31[index1]);
                    str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                    string[] strArray32 = str.Split('@');
                    for (int index1 = 0; index1 < strArray32.Length - 1; ++index1)
                        this.GetRandomMystery[index1] = bool.Parse(strArray32[index1]);
                    try
                    {
                        str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                        string[] strArray5 = str.Split('@');
                        for (int index1 = 0; index1 < strArray5.Length / 5; ++index1)
                            this.interiors.Add(new Interior(int.Parse(strArray5[index1 * 5]), int.Parse(strArray5[index1 * 5 + 1]), int.Parse(strArray5[index1 * 5 + 2]), bool.Parse(strArray5[index1 * 5 + 3]), bool.Parse(strArray5[index1 * 5 + 4])));
                    }
                    catch
                    {
                    }
                    try
                    {
                        str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                        string[] strArray5 = str.Split('@');
                        for (int index1 = 0; index1 < strArray5.Length - 1; ++index1)
                            this.netWorkName[index1] = int.Parse(strArray5[index1]);
                    }
                    catch
                    {
                    }
                    try
                    {
                        str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                        this.netWorkFace = int.Parse(str);
                    }
                    catch
                    {
                    }
                    try
                    {
                        str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                        string[] strArray5 = str.Split('@');
                        for (int index1 = 0; index1 < strArray5.Length - 1; ++index1)
                            this.RirekNetWorkAddress.Add(int.Parse(strArray5[index1]));
                    }
                    catch
                    {
                    }
                    try
                    {
                        str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                        string[] strArray5 = str.Split('@');
                        for (int index1 = 0; index1 < strArray5.Length - 1; ++index1)
                            this.RirekNetWorkFace.Add(int.Parse(strArray5[index1]));
                    }
                    catch
                    {
                    }
                    try
                    {
                        str = TCDEncodeDecode.DecryptString(streamReader.ReadLine(), SaveData.pass);
                        string[] strArray5 = str.Split('@');
                        for (int index1 = 0; index1 < strArray5.Length - 1; ++index1)
                        {
                            string[] strArray6 = strArray5[index1].Split(',');
                            int[] numArray = new int[10];
                            for (int index2 = 0; index2 < numArray.Length; ++index2)
                                numArray[index2] = int.Parse(strArray6[index1]);
                            this.RirekNetWorkName.Add(numArray);
                        }
                    }
                    catch
                    {
                    }
                    this.canselectmenu[7] = false;
                    if (!this.flagList[553])
                    {
                        this.flagList[4] = false;
                        this.flagList[69] = false;
                    }

                    this.loadSucces = true;
                    this.shopThread = new Thread(new ThreadStart(this.ShopSave));
                    this.shopThread.Start();
                    this.flagThread = new Thread(new ThreadStart(this.FlugSave));
                    this.flagThread.Start();
                    this.valThread = new Thread(new ThreadStart(this.ValSave));
                    this.valThread.Start();
                    this.mysThread = new Thread(new ThreadStart(this.MysSave));
                    this.mysThread.Start();
                    this.ranThread = new Thread(new ThreadStart(this.RanSave));
                    this.ranThread.Start();
                    this.chipThread = new Thread(new ThreadStart(this.ChipSave));
                    this.chipThread.Start();
                    this.AddOnRUN();
                }
                catch
                {
                    this.loadSucces = false;
                    loadAttemptedAndFailed = true;
                }
                finally
                {
                    streamReader.Close();
                    streamReader.Dispose();
                }
            }

            if (!this.loadSucces && !this.attemptingBackupLoad)
            {
                var errorText = ShanghaiEXE.Translate("Save.MainSaveCorrupted").Text;

                if (File.Exists(BackupPath))
                {
                    File.Copy(BackupPath, SavePath, true);
                    this.attemptingBackupLoad = true;
                    this.Load(parent);
                    this.attemptingBackupLoad = false;

                    if (!this.loadSucces)
                    {
                        errorText += Environment.NewLine + Environment.NewLine + ShanghaiEXE.Translate("Save.BackupCorrupted").Text;
                    }
                    else
                    {
                        errorText += Environment.NewLine + Environment.NewLine + ShanghaiEXE.Translate("Save.BackupRestored").Text;
                    }

                    parent?.Invoke((Action)(() =>
                    {
                        MessageBox.Show(
                            errorText,
                            ShanghaiEXE.Translate("Save.MainSaveCorruptedTitle").Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }));
                }
                else if (loadAttemptedAndFailed)
                {
                    errorText += Environment.NewLine + Environment.NewLine + ShanghaiEXE.Translate("Save.BackupNotFound").Text;
                    
                    parent?.Invoke((Action)(() =>
                    {
                        MessageBox.Show(
                            errorText,
                            ShanghaiEXE.Translate("Save.MainSaveCorruptedTitle").Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }));
                }
            }

            this.loadEnd = true;
        }

        public ICollection<Dialogue> RetconSave()
        {
            var retconMessages = new List<Dialogue>();

            // 0 : unmodified, fix hospital event incident BGM not cleared.
            if (this.ValList[199] <= 0)
            {
                // If hospital event complete, the postgame hasn't started, and endgame robots aren't out 
                if (this.ValList[14] != 0 && this.FlagList[744] && !this.FlagList[791] && this.ValList[10] != 7)
                {
                    this.ValList[14] = 0;
                    retconMessages.Add(ShanghaiEXE.Translate("Retcon.0503HospitalEmergencyMusic"));
                }
            }

            // 1: 0.550, fix chip ID issues, add illegal chips to library (only recordkeeping), set new hint message
            // Refund duplicate addons
            if (this.ValList[199] <= 1)
            {
                var replacements = new[]
                {
                    new { Original = 416, New = 253 },
                    new { Original = 417, New = 254 },
                    new { Original = 266, New = 313 }
                };
                // Replace in-folder chips
                for (var folderIndex = 0; folderIndex < this.chipFolder.GetLength(0); folderIndex++)
                {
                    for (var chipIndex = 0; chipIndex < this.chipFolder.GetLength(1); chipIndex++)
                    {
                        foreach (var replacement in replacements)
                        {
                            if (this.chipFolder[folderIndex, chipIndex, 0] == replacement.Original)
                            {
                                this.chipFolder[folderIndex, chipIndex, 0] = replacement.New;
                            }
                        }
                    }
                }
                // Replace in-bag chip counts
                for (var codeIndex = 0; codeIndex < 4; codeIndex++)
                {
                    foreach (var replacement in replacements)
                    {
                        this.havechip[replacement.New, codeIndex] = this.havechip[replacement.Original, codeIndex];
                        this.havechip[replacement.Original, codeIndex] = 0;
                    }
                }
                // Replace bag order chips
                for (var chipIndex = 0; chipIndex < this.havechips.Count; chipIndex++)
                {
                    foreach (var replacement in replacements)
                    {
                        if (this.havechips[chipIndex].number == replacement.Original)
                        {
                            this.havechips[chipIndex] = new ChipS(replacement.New, this.havechips[chipIndex].code);
                        }
                    }
                }

                // Add illegal chips to seen list
                for (var chipIndex = 310; chipIndex < this.havechip.GetLength(0); chipIndex++)
                {
                    if (this.havechip[chipIndex, 0] > 0 || this.havechip[chipIndex, 1] > 0 || this.havechip[chipIndex, 2] > 0 || this.havechip[chipIndex, 3] > 0)
                    {
                        this.datelist[chipIndex - 1] = true;
                    }
                }

                // Add/remove chips from seen list
                foreach (var replacement in replacements)
                {
                    this.datelist[replacement.New - 1] = this.datelist[replacement.Original - 1];
                    this.datelist[replacement.Original - 1] = false;
                }

                // Change Humor, EirinCall to grey
                var replacedAddonNames = string.Empty;
                for (int i = 0; i < this.haveAddon.Count; i++)
                {
                    var addOn = this.haveAddon[i];
                    if (addOn.ID == 54 || addOn.ID == 57)
                    {
                        this.haveAddon[i].color = AddOnBase.ProgramColor.glay;
                        if (string.IsNullOrEmpty(replacedAddonNames))
                        {
                            replacedAddonNames = addOn.name;
                        }
                        else
                        {
                            replacedAddonNames = $"{replacedAddonNames} and {addOn.name}";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(replacedAddonNames))
                {
                    retconMessages.Add(ShanghaiEXE.Translate("Retcon.0550HumorEirinCallFormat").Format(replacedAddonNames));
                }
                
                var refundedAddons = new List<Tuple<int, int, string>>();
                var refundedAddonNames = new List<string>();
                var voileCRecovBought = this.shopCount[11, 8] > 0;
                var voileCShotgunBought = this.shopCount[11, 9] > 0;
                var undersquareCLanceBought = this.shopCount[14, 2] > 0;
                var undernetLostLghtBought = this.shopCount[15, 1] > 0;
                var engellesFullOpenOpened = this.getMystery[149];

                // Bought CRecov from Voile for 12000 Z (removed, now only from Engelles 1 BMD 136)
                if (voileCRecovBought)
                {
                    refundedAddons.Add(Tuple.Create(92, 12000, "Z"));
                }

                // Bought CShotgun from Voile for 12000 Z (removed, now only bought from Undersquare 2 for 17000 Z)
                if (voileCShotgunBought)
                {
                    // No refund, free 5000 Z and mark down Undersquare 2
                    this.shopCount[14, 2] = 1;
                }

                // Bought CLance from World Undersquare for 17000 Z (replaced with CShotgun, now only at Voile)
                if (undersquareCLanceBought)
                {
                    refundedAddons.Add(Tuple.Create(91, 17000, "Z"));
                }

                // Bought LostLght from Undernet 3 for 50 BugFrags (removed, now only bought from LordUsa comp for 18000 Z)
                if (undernetLostLghtBought)
                {
                    refundedAddons.Add(Tuple.Create(73, 50, "BugFrag"));
                    for (var i = 1; i <= 4; i++)
                    {
                        this.shopCount[15, i] = this.shopCount[15, i + 1];
                    }
                }

                // Got FullOpen from Engelles 3 PMD (replaced with MedusEye Y, now only from Undernet 10 BMD behind ROM gate)
                if (engellesFullOpenOpened)
                {
                    refundedAddons.Add(Tuple.Create(20, 0, ""));
                    this.AddChip(10, 3, true);
                }

                var removedIndices = new List<Tuple<int,int>>();
                for (var i = 0; i < this.haveAddon.Count; i++)
                {
                    var addOn = this.haveAddon[i];
                    var refund = refundedAddons.FirstOrDefault(tup => tup.Item1 == addOn.ID);
                    if (refund != null)
                    {
                        refundedAddons.Remove(refund);
                        refundedAddonNames.Add(addOn.name);

                        removedIndices.Add(Tuple.Create(i, this.equipAddon[i] ? this.equipAddon.Take(i).Count(b => b) : -1));
                        switch (refund.Item3)
                        {
                            case "Z":
                                this.money += refund.Item2;
                                break;
                            case "BugFrag":
                                this.havePeace[0] += refund.Item2;
                                break;
                            case "":
                                break;
                        }
                    }
                }

                foreach (var removedIndex in removedIndices.OrderByDescending(ri => ri.Item1))
                {
                    this.haveAddon.RemoveAt(removedIndex.Item1);
                    this.equipAddon.RemoveAt(removedIndex.Item1);
                    if (removedIndex.Item2 != -1)
                    {
                        this.addonNames.RemoveAt(removedIndex.Item2);
                    }
                }

                if (refundedAddonNames.Any())
                {
                    var refundedAddonsString = refundedAddonNames.Aggregate((accum, next) =>
                    {
                        var entries = accum.Count(c => c == '，') + 1;
                        var linebreak = entries != 0 && entries % 3 == 0 ? "," : string.Empty;
                        return $"{accum}，{linebreak}{next}";
                    });
                    retconMessages.Add(ShanghaiEXE.Translate("Retcon.0550AddOnRefundFormat").Format(refundedAddonsString));
                    retconMessages.Add(ShanghaiEXE.Translate("Retcon.0550AddOnRefund2"));
                    this.AddOnRUN();
                }

                // Fix old "at end of game" L message with postgame message
                if (this.ValList[3] == 101)
                {
                    this.ValList[3] = 102;
                }

                // Guess V3 defeat flags from V2 flags and
                // V2, chip, code, V3, name
                var v2AndChips = new[]
                {
                    new {V2 = 35, Chip = 220, Code = 0, V3 = 838, Name="Enemy.CirnoName4"}, // Cirno
                    new {V2 = 165, Chip = 229, Code = 0, V3 = 839, Name="Enemy.PyroManName4"}, // PyroMan
                    new {V2 = 170, Chip = 232, Code = 0, V3 = 840, Name="Enemy.MrasaName4"}, // Murasa
                    new {V2 = 171, Chip = 235, Code = 0, V3 = 841, Name="Enemy.ScissorManName4"}, // ScissorMan
                    new {V2 = 173, Chip = 238, Code = 0, V3 = 842, Name="Enemy.ChenName4"}, // Chen
                    new {V2 = 790, Chip = 247, Code = 0, V3 = 843, Name="Enemy.DruidManNameV3"}, // DruidMan
                    new {V2 = 148, Chip = 193, Code = 0, V3 = 844, Name="Enemy.MarisaName3"}, // Marisa
                    new {V2 = 155, Chip = 196, Code = 0, V3 = 845, Name="Enemy.SakuyaName3"}, // Sakuya
                    new {V2 = 163, Chip = 199, Code = 0, V3 = 846, Name="Enemy.TankManName3"}, // TankMan
                    new {V2 = 164, Chip = 226, Code = 0, V3 = 847, Name="Enemy.IkuName3"}, // Iku
                    new {V2 = 166, Chip = 202, Code = 0, V3 = 848, Name="Enemy.SpannerManName3"}, // SpannerMan
                    new {V2 = 161, Chip = 223, Code = 0, V3 = 849, Name="Enemy.MedicineName3"}, // Medicine
                    new {V2 = 169, Chip = 217, Code = 0, V3 = 850, Name="Enemy.YorihimeName3"}, // Yorihime
                    new {V2 = 168, Chip = 208, Code = 0, V3 = 851, Name="Enemy.HakutakuManName3"}, // HakutakuMan
                    new {V2 = 167, Chip = 211, Code = 0, V3 = 852, Name="Enemy.TortoiseManName3"}, // TortoiseMan
                    new {V2 = 172, Chip = 214, Code = 0, V3 = 853, Name="Enemy.BeetleManName3"}, // BeetleMan
                    new {V2 = 174, Chip = 241, Code = 0, V3 = 854, Name="Enemy.RanName3"}, // Ran
                    // No implemented ways to fight Youmu, Utsuho
                };

                var v3Flags = v2AndChips.ToList();
                v3Flags.Clear();

                foreach (var check in v2AndChips)
                {
                    var v2Flag = this.FlagList[check.V2];
                    var hasChip = false;
                    for (var codeIndex = 0; codeIndex < 4; codeIndex++)
                    {
                        hasChip |= this.havechip[check.Chip, check.Code] != 0;
                    }
                    for (var folderIndex = 0; folderIndex < this.chipFolder.GetLength(0); folderIndex++)
                    {
                        for (var chipIndex = 0; chipIndex < this.chipFolder.GetLength(1); chipIndex++)
                        {
                            hasChip |= this.chipFolder[folderIndex, chipIndex, 0] == check.Chip
                                && this.chipFolder[folderIndex, chipIndex, 1] == check.Code;
                        }
                    }

                    if (v2Flag && hasChip)
                    {
                        v3Flags.Add(check);
                    }
                }

                if (v3Flags.Any())
                {
                    retconMessages.Add(ShanghaiEXE.Translate("Retcon.0550V3Tracking"));
                    var nameDialogue = default(Dialogue);
                    var lines = 0;
                    foreach (var v3 in v3Flags)
                    {
                        this.FlagList[v3.V3] = true;

                        if (lines == 0)
                        {
                            nameDialogue = new Dialogue { Face = FACE.Sprite.ToFaceId(), Text = ShanghaiEXE.Translate(v3.Name) + ",," };
                            lines++;
                        }
                        else
                        {
                            var blankCommas = 3 - lines;
                            nameDialogue.Text = nameDialogue.Text.Substring(0, nameDialogue.Text.Length - blankCommas)
                                + "," + ShanghaiEXE.Translate(v3.Name)
                                + new string(Enumerable.Repeat(',', blankCommas - 1).ToArray());
                            lines++;
                            if (lines >= 3)
                            {
                                retconMessages.Add(nameDialogue);
                                nameDialogue = null;
                                lines = 0;
                            }
                        }
                    }

                    if (nameDialogue != null)
                    {
                        retconMessages.Add(nameDialogue);
                    }
                }

                // Lloyd -> Troid retcon
                if (retconMessages.Any())
                {
                    retconMessages.Add(ShanghaiEXE.Translate("Retcon.0550Lloyd"));
                }
            }

            if (this.ValList[199] <= 2)
            {
                // Shift City BBS ids up by 3 for new entries
                var cityBbsEntries = this.bbsRead.GetLength(1);
                var movedEntriesRead = Enumerable.Range(19, cityBbsEntries - 19).Any(i => this.bbsRead[1, i]);
                if (movedEntriesRead)
                {
                    for (var i = cityBbsEntries - 1; i > 21; i--)
                    {
                        this.bbsRead[1, i] = this.bbsRead[1, i - 3];
                    }
                    this.bbsRead[1, 19] = false;
                    this.bbsRead[1, 20] = false;
                    this.bbsRead[1, 21] = false;
                }

                // If HeavenNet already entered, warn that area in progress, battles to be reverted
                // TODO: when no longer needed (battles implemented), unset 900 & revert battles (+give message)
                //if (this.FlagList[793] && !this.FlagList[900])
                //{
                //    retconMessages.Add(ShanghaiEXE.Translate("Retcon.0550HeavenWIP2"));
                //    this.FlagList[900] = true;
                //}
            }

            if (this.ValList[199] <= 3)
            {
                var reAddedAddOns = new List<string>();

                var engellesCRecovOpened = this.getMystery[136];
                if (engellesCRecovOpened && !this.haveAddon.Any(ao => ao is CRepair))
                {
                    this.GetAddon(new CRepair(AddOnBase.ProgramColor.blue));
                    reAddedAddOns.Add(ShanghaiEXE.Translate("AddOn.CRepairName"));
                }

                var undersquareCShotgunBought = this.shopCount[14, 1] > 0;
                if (undersquareCShotgunBought && !this.haveAddon.Any(ao => ao is CShotGun))
                {
                    this.GetAddon(new CShotGun(AddOnBase.ProgramColor.blue));
                    reAddedAddOns.Add(ShanghaiEXE.Translate("AddOn.CShotgunName"));
                }

                if (reAddedAddOns.Any())
                {
                    var reAddDialogue = ShanghaiEXE.Translate("Retcon.0550ReAddedAddOns");
                    reAddDialogue.Text += string.Join("，", reAddedAddOns);
                    retconMessages.Add(reAddDialogue);
                }
            }

            // WIP9
            if (this.ValList[199] <= 4)
            {
                // Replace new addons
                var hasFriendship = this.haveAddon.Any(ao => ao is Sacrifice);
                var hasMammon = this.haveAddon.Any(ao => ao is Mammon);
                var givNTakeIndex = this.haveAddon.FindIndex(ao => ao is Yuzuriai);
                if (givNTakeIndex != -1)
                {
                    var equipIndex = this.equipAddon[givNTakeIndex];
                    var equipNameList = equipIndex ? this.equipAddon.Take(givNTakeIndex).Count(b => b) : -1;

                    this.haveAddon.RemoveAt(givNTakeIndex);
                    this.equipAddon.RemoveAt(givNTakeIndex);

                    if (equipNameList != -1)
                    {
                        this.addonNames.RemoveAt(equipNameList);
                    }

                    this.GetAddon(new Yuzuriai(AddOnBase.ProgramColor.gleen));
                }
                if (hasFriendship || hasMammon)
                {
                    retconMessages.Add(ShanghaiEXE.Translate("Retcon.0550AddOnRebalance"));
                }
                if (givNTakeIndex != -1)
                {
                    retconMessages.Add(ShanghaiEXE.Translate("Retcon.0550AddOnRebalance2"));
                }

                if (this.FlagList[796])
                {
                    this.FlagList[795] = true;
                    retconMessages.Add(ShanghaiEXE.Translate("Retcon.0550SageSkipped"));
                }
                
                if (this.FlagList[880])
                {
                    this.FlagList[800] = true;
                    retconMessages.Add(ShanghaiEXE.Translate("Retcon.0550CrimDexEnabled"));
                }
            }

            // WIP10
            if (this.ValList[199] <= 5)
            {
                // Reset flag accidentally left on after cutscene
                // Flag13 should only be active during automatically-progressing cutscenes, no savegame should have it set legitimately
                this.FlagList[13] = false;

                // Reset WIP endgame flags
                var endgamePlaceholderFlags = new[]
                {
                    803, 804, 805, 806, 807, 808, 809, 824, 825, 826, 827, // Ghost doors opened (always linked to ghosts defeated)
                    814, 815, 816, 817, 818, 819, 820, 828, 829, 830, 831, // Ghosts defeated
                    822, 861, 823, 832, 835, 833, 837 // Barriers destroyed (barrier doors opening unaffected)
                };
                var anyFlagsReset = false;
                foreach (var endgameFlag in endgamePlaceholderFlags)
                {
                    if (this.FlagList[endgameFlag])
                    {
                        this.FlagList[endgameFlag] = false;
                        anyFlagsReset = true;
                    }
                }
                if (anyFlagsReset)
                {
                    retconMessages.Add(ShanghaiEXE.Translate("Retcon.0550WIPEndgameReset"));

                    if (this.nowMap == "heavenNet1" || this.nowMap == "heavenNet2")
                    {
                        retconMessages.Add(ShanghaiEXE.Translate("Retcon.0550HeavenJackOut"));

                        this.nowMap = this.pluginMap;
                        this.nowFroor = this.pluginFroor;
                        this.nowX = this.pluginX;
                        this.nowY = this.pluginY;
                        this.nowZ = this.pluginZ;

                        this.isJackedIn = false;
                        this.FlagList[2] = false;
                        if (!this.FlagList[13])
                        {
                            this.GetRandomMystery = new bool[600];
                            this.runSubChips[0] = false;
                            this.runSubChips[1] = false;
                            this.runSubChips[2] = false;
                            this.runSubChips[3] = false;
                            this.ValList[19] = 0;
                            this.HPNow = this.HPMax;
                            this.steptype = (int)SceneMap.STEPS.normal;
                        }
                    }
                }

                // Unset WIP message shown flag
                this.FlagList[900] = false;
            }

            // WIP14
            if (this.ValList[199] <= 6)
            {
                // Remove duplicate keys (only if door opened)
                if (this.keyitem.Count(ki => ki == 26) >= 2)
                {
                    this.keyitem.RemoveAll(ki => ki == 26);
                }

                // Replace demo room with interior
                if (this.ValList[10] >= 8)
                {
                    this.interiors.Add(new Interior(51, 106, 186, true, false));
                    this.FlagList[465] = true;
                }
            }

            // WIP15
            if (this.ValList[199] <= 7)
            {
                // Return demo and debug rooms if discarded
                if (this.FlagList[465] && !this.interiors.Any(i => i.number == 51))
                {
                    this.interiors.Add(new Interior(51, 106, 186, false, false));
                }
                if (this.FlagList[466] && !this.interiors.Any(i => i.number == 52))
                {
                    this.interiors.Add(new Interior(52, 136, 186, false, false));
                }
            }

            // Set var to "current save version"
            this.ValList[199] = 8;
            return retconMessages;
        }

        public void SaveFile(Form parent = null)
        {
            var streamWriter = default(StreamWriter);
            var saveFailed = false;

            try
            {
                this.shopThread = new Thread(new ThreadStart(this.ShopSave));
                this.shopThread.Start();
                this.flagThread = new Thread(new ThreadStart(this.FlugSave));
                this.flagThread.Start();
                this.valThread = new Thread(new ThreadStart(this.ValSave));
                this.valThread.Start();
                this.mysThread = new Thread(new ThreadStart(this.MysSave));
                this.mysThread.Start();
                this.ranThread = new Thread(new ThreadStart(this.RanSave));
                this.ranThread.Start();
                this.chipThread = new Thread(new ThreadStart(this.ChipSave));
                this.chipThread.Start();
                this.saveEnd = false;
                StringBuilder stringBuilder = new StringBuilder();
                streamWriter = new StreamWriter(SavePathTemp, false, Encoding.GetEncoding("Shift_JIS"));
                StringBuilder sourceString1 = new StringBuilder();
                foreach (string addonName in this.addonNames)
                    sourceString1.Append(addonName + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString1, SaveData.pass));
                StringBuilder sourceString2 = new StringBuilder();
                foreach (byte num in this.busterspec)
                    sourceString2.Append(((int)num).ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString2, SaveData.pass));
                StringBuilder sourceString3 = new StringBuilder();
                foreach (bool flag in this.canselectmenu)
                    sourceString3.Append(flag.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString3, SaveData.pass));
                StringBuilder sourceString4 = new StringBuilder();
                for (int index1 = 0; index1 < this.chipFolder.GetLength(0); ++index1)
                {
                    for (int index2 = 0; index2 < this.chipFolder.GetLength(1); ++index2)
                    {
                        for (int index3 = 0; index3 < this.chipFolder.GetLength(2); ++index3)
                            sourceString4.Append(this.chipFolder[index1, index2, index3].ToString() + "@");
                        sourceString4.Append("|");
                    }
                    sourceString4.Append("/");
                }
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString4, SaveData.pass));
                StringBuilder sourceString5 = new StringBuilder();
                foreach (ChipS havechip in this.havechips)
                    sourceString5.Append(havechip.number.ToString() + "/" + havechip.code + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString5, SaveData.pass));
                StringBuilder sourceString6 = new StringBuilder();
                foreach (bool flag in this.datelist)
                    sourceString6.Append(flag.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString6, SaveData.pass));
                StringBuilder sourceString7 = new StringBuilder();
                sourceString7.Append(this.efolder);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString7, SaveData.pass));
                StringBuilder sourceString8 = new StringBuilder();
                foreach (bool flag in this.equipAddon)
                    sourceString8.Append(flag.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString8, SaveData.pass));
                StringBuilder sourceString9 = new StringBuilder();
                sourceString9.Append(this.firstchange);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString9, SaveData.pass));
                StringBuilder sourceString10 = new StringBuilder();
                sourceString10.Append(this.foldername);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString10, SaveData.pass));
                StringBuilder sourceString11 = new StringBuilder();
                foreach (AddOnBase addOnBase in this.haveAddon)
                {
                    var typeName = addOnBase.GetType().ToAddOnName();
                    sourceString11.Append(typeName + "/" + addOnBase.color + "@");
                }
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString11, SaveData.pass));
                StringBuilder sourceString12 = new StringBuilder();
                sourceString12.Append(this.haveCaptureBomb);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString12, SaveData.pass));
                this.chipThread.Join();
                streamWriter.WriteLine(this.chiplist);
                StringBuilder sourceString13 = new StringBuilder();
                foreach (bool flag in this.havefolder)
                    sourceString13.Append(flag.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString13, SaveData.pass));
                StringBuilder sourceString14 = new StringBuilder();
                foreach (int num in this.havePeace)
                    sourceString14.Append(num.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString14, SaveData.pass));
                StringBuilder sourceString15 = new StringBuilder();
                sourceString15.Append(this.havestyles);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString15, SaveData.pass));
                StringBuilder sourceString16 = new StringBuilder();
                foreach (int haveSubChi in this.haveSubChis)
                    sourceString16.Append(haveSubChi.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString16, SaveData.pass));
                StringBuilder sourceString17 = new StringBuilder();
                sourceString17.Append(this.haveSubMemory);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString17, SaveData.pass));
                StringBuilder sourceString18 = new StringBuilder();
                foreach (Virus haveViru in this.HaveVirus)
                {
                    if (haveViru != null)
                        sourceString18.Append(haveViru.type.ToString() + "/" + haveViru.eatBug + "/" + haveViru.eatError + "/" + haveViru.eatFreeze + "/" + haveViru.code + "@");
                    else
                        sourceString18.Append("null@");
                }
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString18, SaveData.pass));
                StringBuilder sourceString19 = new StringBuilder();
                sourceString19.Append(this.HPmax);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString19, SaveData.pass));
                StringBuilder sourceString20 = new StringBuilder();
                sourceString20.Append(this.HPnow);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString20, SaveData.pass));
                StringBuilder sourceString21 = new StringBuilder();
                sourceString21.Append(this.HPplus);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString21, SaveData.pass));
                StringBuilder sourceString22 = new StringBuilder();
                foreach (int num in this.keyitem)
                    sourceString22.Append(num.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString22, SaveData.pass));
                StringBuilder sourceString23 = new StringBuilder();
                foreach (int num in this.mail)
                    sourceString23.Append(num.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString23, SaveData.pass));
                StringBuilder sourceString24 = new StringBuilder();
                foreach (bool flag in this.mailread)
                    sourceString24.Append(flag.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString24, SaveData.pass));
                StringBuilder sourceString25 = new StringBuilder();
                sourceString25.Append(this.manybattle);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString25, SaveData.pass));
                StringBuilder sourceString26 = new StringBuilder();
                sourceString26.Append(this.MaxCore);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString26, SaveData.pass));
                StringBuilder sourceString27 = new StringBuilder();
                sourceString27.Append(this.MaxHz);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString27, SaveData.pass));
                StringBuilder sourceString28 = new StringBuilder();
                sourceString28.Append(this.mind);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString28, SaveData.pass));
                StringBuilder sourceString29 = new StringBuilder();
                sourceString29.Append(this.Money);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString29, SaveData.pass));
                StringBuilder sourceString30 = new StringBuilder();
                sourceString30.Append(this.plase);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString30, SaveData.pass));
                StringBuilder sourceString31 = new StringBuilder();
                sourceString31.Append(this.isJackedIn);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString31, SaveData.pass));
                StringBuilder sourceString32 = new StringBuilder();
                foreach (byte num in this.regularchip)
                    sourceString32.Append(((int)num).ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString32, SaveData.pass));
                StringBuilder sourceString33 = new StringBuilder();
                foreach (bool flag in this.regularflag)
                    sourceString33.Append(flag.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString33, SaveData.pass));
                StringBuilder sourceString34 = new StringBuilder();
                sourceString34.Append(this.Regularlarge);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString34, SaveData.pass));
                StringBuilder sourceString35 = new StringBuilder();
                foreach (bool runSubChip in this.runSubChips)
                    sourceString35.Append(runSubChip.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString35, SaveData.pass));
                StringBuilder sourceString36 = new StringBuilder();
                sourceString36.Append(this.selectQuestion);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString36, SaveData.pass));
                StringBuilder sourceString37 = new StringBuilder();
                sourceString37.Append(this.setstyle);
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString37, SaveData.pass));
                StringBuilder sourceString38 = new StringBuilder();
                foreach (Virus stockViru in this.stockVirus)
                    sourceString38.Append(stockViru.type.ToString() + "/" + stockViru.eatBug + "/" + stockViru.eatError + "/" + stockViru.eatFreeze + "/" + stockViru.code + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString38, SaveData.pass));
                StringBuilder sourceString39 = new StringBuilder();
                foreach (Style style in this.style)
                    sourceString39.Append(style.style.ToString() + "/" + style.element + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString39, SaveData.pass));
                StringBuilder sourceString40 = new StringBuilder();
                foreach (int num in this.stylepoint)
                    sourceString40.Append(num.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString40, SaveData.pass));
                StringBuilder sourceString41 = new StringBuilder();
                foreach (byte num in this.time)
                    sourceString41.Append(((int)num).ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString41, SaveData.pass));
                StringBuilder sourceString42 = new StringBuilder();
                foreach (bool flag in this.virusSPbusted)
                    sourceString42.Append(flag.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString42, SaveData.pass));
                StringBuilder sourceString43 = new StringBuilder();
                foreach (bool flag in this.virusSPbustedFlug)
                    sourceString43.Append(flag.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString43, SaveData.pass));
                StringBuilder sourceString44 = new StringBuilder();
                bool[,] bbsRead = this.bbsRead;
                int upperBound1 = bbsRead.GetUpperBound(0);
                int upperBound2 = bbsRead.GetUpperBound(1);
                for (int lowerBound1 = bbsRead.GetLowerBound(0); lowerBound1 <= upperBound1; ++lowerBound1)
                {
                    for (int lowerBound2 = bbsRead.GetLowerBound(1); lowerBound2 <= upperBound2; ++lowerBound2)
                    {
                        bool flag = bbsRead[lowerBound1, lowerBound2];
                        sourceString44.Append(flag.ToString() + "@");
                    }
                }
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString44, SaveData.pass));
                StringBuilder sourceString45 = new StringBuilder();
                foreach (bool flag in this.questEnd)
                    sourceString45.Append(flag.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString45, SaveData.pass));
                stringBuilder = new StringBuilder();
                this.shopThread.Join();
                streamWriter.WriteLine(this.shoplist);
                StringBuilder sourceString46 = new StringBuilder();
                sourceString46.Append(this.message.ToString());
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString46, SaveData.pass));
                StringBuilder sourceString47 = new StringBuilder();
                sourceString47.Append(this.isJackedIn.ToString());
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString47, SaveData.pass));
                StringBuilder sourceString48 = new StringBuilder();
                sourceString48.Append(this.nowMap.ToString());
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString48, SaveData.pass));
                StringBuilder sourceString49 = new StringBuilder();
                sourceString49.Append(this.nowX.ToString(System.Globalization.CultureInfo.InvariantCulture));
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString49, SaveData.pass));
                StringBuilder sourceString50 = new StringBuilder();
                sourceString50.Append(this.nowY.ToString(System.Globalization.CultureInfo.InvariantCulture));
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString50, SaveData.pass));
                StringBuilder sourceString51 = new StringBuilder();
                sourceString51.Append(this.nowZ.ToString(System.Globalization.CultureInfo.InvariantCulture));
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString51, SaveData.pass));
                StringBuilder sourceString52 = new StringBuilder();
                sourceString52.Append(this.nowFroor.ToString());
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString52, SaveData.pass));
                StringBuilder sourceString53 = new StringBuilder();
                sourceString53.Append(this.steptype.ToString());
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString53, SaveData.pass));
                StringBuilder sourceString54 = new StringBuilder();
                sourceString54.Append(this.stepoverX.ToString());
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString54, SaveData.pass));
                StringBuilder sourceString55 = new StringBuilder();
                sourceString55.Append(this.stepoverY.ToString());
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString55, SaveData.pass));
                StringBuilder sourceString56 = new StringBuilder();
                sourceString56.Append(this.stepCounter.ToString(System.Globalization.CultureInfo.InvariantCulture));
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString56, SaveData.pass));
                StringBuilder sourceString57 = new StringBuilder();
                sourceString57.Append(this.pluginMap.ToString());
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString57, SaveData.pass));
                StringBuilder sourceString58 = new StringBuilder();
                sourceString58.Append(this.pluginX.ToString(System.Globalization.CultureInfo.InvariantCulture));
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString58, SaveData.pass));
                StringBuilder sourceString59 = new StringBuilder();
                sourceString59.Append(this.pluginY.ToString(System.Globalization.CultureInfo.InvariantCulture));
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString59, SaveData.pass));
                StringBuilder sourceString60 = new StringBuilder();
                sourceString60.Append(this.pluginZ.ToString(System.Globalization.CultureInfo.InvariantCulture));
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString60, SaveData.pass));
                StringBuilder sourceString61 = new StringBuilder();
                sourceString61.Append(this.pluginFroor.ToString());
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString61, SaveData.pass));
                this.flagThread.Join();
                streamWriter.WriteLine(this.fluglist);
                this.valThread.Join();
                streamWriter.WriteLine(this.vallist);
                this.mysThread.Join();
                streamWriter.WriteLine(this.myslist);
                this.ranThread.Join();
                streamWriter.WriteLine(this.ranlist);
                StringBuilder sourceString62 = new StringBuilder();
                for (int index = 0; index < this.interiors.Count; ++index)
                {
                    sourceString62.Append(this.interiors[index].number.ToString() + "@");
                    sourceString62.Append(this.interiors[index].posiX.ToString() + "@");
                    sourceString62.Append(this.interiors[index].posiY.ToString() + "@");
                    sourceString62.Append(this.interiors[index].set.ToString() + "@");
                    sourceString62.Append(this.interiors[index].rebirth.ToString() + "@");
                }
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString62, SaveData.pass));
                StringBuilder sourceString63 = new StringBuilder();
                foreach (int num in this.netWorkName)
                    sourceString63.Append(num.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString63, SaveData.pass));
                StringBuilder sourceString64 = new StringBuilder();
                sourceString64.Append(this.netWorkFace.ToString());
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString64, SaveData.pass));
                StringBuilder sourceString65 = new StringBuilder();
                foreach (int num in this.RirekNetWorkAddress)
                    sourceString65.Append(num.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString65, SaveData.pass));
                StringBuilder sourceString66 = new StringBuilder();
                foreach (int num in this.RirekNetWorkFace)
                    sourceString66.Append(num.ToString() + "@");
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString66, SaveData.pass));
                StringBuilder sourceString67 = new StringBuilder();
                foreach (int[] numArray in this.RirekNetWorkName)
                {
                    string str = "";
                    for (int index = 0; index < numArray.Length; ++index)
                        str = str + numArray[index] + ",";
                    sourceString67.Append(str + "@");
                }
                streamWriter.WriteLine(TCDEncodeDecode.EncryptString(sourceString67, SaveData.pass));
            }
            catch
            {
                saveFailed = true;
            }
            finally
            {
                streamWriter?.Close();
                streamWriter?.Dispose();
            }

            if (saveFailed)
            {
                var errorText = ShanghaiEXE.Translate("Save.SaveFailed").Text;
                if (File.Exists(BackupPath))
                {
                    File.Copy(BackupPath, SavePath, true);
                    errorText += Environment.NewLine + Environment.NewLine + ShanghaiEXE.Translate("Save.SaveRetained").Text;
                }
                else
                {
                    errorText += Environment.NewLine + Environment.NewLine + ShanghaiEXE.Translate("Save.SaveBackupNotFound").Text;
                }

                parent?.Invoke((Action)(() =>
                {
                    MessageBox.Show(
                        errorText,
                        ShanghaiEXE.Translate("Save.SaveFailedTitle").Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }));
            }
            else
            {
                if (File.Exists(SavePath))
                {
                    File.Copy(SavePath, BackupPath, true);
                }
                File.Copy(SavePathTemp, SavePath, true);
                File.Delete(SavePathTemp);
            }

            this.saveEnd = true;
            this.saveEndnowsub = true;
            this.saveEndnow = true;
        }

        public void FlugSave()
        {
            this.flugEnd = false;
            StringBuilder sourceString = new StringBuilder();
            foreach (bool flag in this.flagList)
                sourceString.Append(flag.ToString() + "@");
            this.fluglist = TCDEncodeDecode.EncryptString(sourceString, SaveData.pass);
            this.flugEnd = true;
        }

        public void ValSave()
        {
            this.valEnd = false;
            StringBuilder sourceString = new StringBuilder();
            foreach (int val in this.valList)
                sourceString.Append(val.ToString() + "@");
            this.vallist = TCDEncodeDecode.EncryptString(sourceString, SaveData.pass);
            this.valEnd = true;
        }

        public void ShopSave()
        {
            this.shopEnd = false;
            StringBuilder sourceString = new StringBuilder();
            for (int index1 = 0; index1 < this.shopCount.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.shopCount.GetLength(1); ++index2)
                {
                    sourceString.Append(this.shopCount[index1, index2].ToString());
                    sourceString.Append("/");
                }
            }
            this.shoplist = TCDEncodeDecode.EncryptString(sourceString, SaveData.pass);
            this.shopEnd = true;
        }

        public void MysSave()
        {
            this.mysEnd = false;
            StringBuilder sourceString = new StringBuilder();
            foreach (bool flag in this.getMystery)
                sourceString.Append(flag.ToString() + "@");
            this.myslist = TCDEncodeDecode.EncryptString(sourceString, SaveData.pass);
            this.mysEnd = true;
        }

        public void RanSave()
        {
            this.ranEnd = false;
            StringBuilder sourceString = new StringBuilder();
            foreach (bool flag in this.getRandomMystery)
                sourceString.Append(flag.ToString() + "@");
            this.ranlist = TCDEncodeDecode.EncryptString(sourceString, SaveData.pass);
            this.ranEnd = true;
        }

        public void ChipSave()
        {
            this.chipEnd = false;
            StringBuilder sourceString = new StringBuilder();
            for (int index1 = 0; index1 < this.havechip.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.havechip.GetLength(1); ++index2)
                    sourceString.Append(((int)this.havechip[index1, index2]).ToString() + "@");
                sourceString.Append("/");
            }
            this.chiplist = TCDEncodeDecode.EncryptString(sourceString, SaveData.pass);
            this.chipEnd = true;
        }

        public int HPmax
        {
            get
            {
                return this.hpmax;
            }
            set
            {
                this.hpmax = value;
                if (this.hpmax <= 1000)
                    return;
                this.hpmax = 1000;
            }
        }

        public int HPMax
        {
            get
            {
                if (this.HPmax + this.HPplus <= 0)
                    return 1;
                return this.HPmax + this.HPplus;
            }
        }

        public int HPNow
        {
            get
            {
                return this.HPnow;
            }
            set
            {
                this.HPnow = value;
                if (this.HPnow <= 0)
                    this.HPnow = 1;
                if (this.HPnow <= this.HPMax)
                    return;
                this.HPnow = this.HPMax;
            }
        }

        public byte Regularlarge
        {
            get
            {
                return this.regularlarge;
            }
            set
            {
                this.regularlarge = value;
                if (this.regularlarge <= 50)
                    return;
                this.regularlarge = 50;
            }
        }

        public byte[,] Havechip
        {
            get
            {
                return this.havechip;
            }
            set
            {
                this.havechip = value;
            }
        }

        public int NaviFolder
        {
            set
            {
                this.naviFolder = value;
            }
            get
            {
                if (this.naviFolder > 0)
                    return this.naviFolder;
                return 0;
            }
        }

        public int MaxHz
        {
            get
            {
                return this.maxhz;
            }
            set
            {
                this.maxhz = value;
                if (this.maxhz > 20)
                    this.maxhz = 20;
                if (this.maxhz >= 1)
                    return;
                this.maxhz = 1;
            }
        }

        public int MaxCore
        {
            get
            {
                return this.maxcore;
            }
            set
            {
                this.maxcore = value;
                if (this.maxcore > 5)
                    this.maxcore = 5;
                if (this.maxcore >= 1)
                    return;
                this.maxcore = 1;
            }
        }

        public int Money
        {
            get
            {
                return this.money;
            }
            set
            {
                long num = value;
                if (num > int.MaxValue)
                    num = int.MaxValue;
                this.money = (int)num;
            }
        }

        public Virus[] HaveVirus
        {
            get
            {
                return this.haveVirus;
            }
            set
            {
                this.haveVirus = value;
                SaveData.HAVEVirus = value;
            }
        }

        public void AddonSkillReset()
        {
            this.addonSkill = new bool[Enum.GetNames(typeof(SaveData.ADDONSKILL)).Length];
        }

        public int[,] ShopCount
        {
            get
            {
                return this.shopCount;
            }
            set
            {
                this.shopCount = value;
                this.shopThread = new Thread(new ThreadStart(this.ShopSave));
                this.shopThread.Start();
            }
        }

        public bool[] FlagList
        {
            get
            {
                return this.flagList;
            }
            set
            {
                this.flagList = value;
                this.flagThread = new Thread(new ThreadStart(this.FlugSave));
                this.flagThread.Start();
            }
        }

        public VariableArray ValList
        {
            get
            {
                return this.valList;
            }
            set
            {
                this.valList = value;
                this.valThread = new Thread(new ThreadStart(this.ValSave));
                this.valThread.Start();
            }
        }

        public bool[] GetMystery
        {
            get
            {
                return this.getMystery;
            }
            set
            {
                this.getMystery = value;
            }
        }

        public bool[] GetRandomMystery
        {
            get
            {
                return this.getRandomMystery;
            }
            set
            {
                this.getRandomMystery = value;
            }
        }

        public static void PadNumChange(int PadNum, int Button, int Value)
        {
            SaveData.Pad[PadNum, Button] = Value;
        }

        public SaveData()
        {
            SaveData.pass = "evAJ5h1lGgmYm0EgZbbraA==";
        }

        public void Init()
        {
            this.manybattle = 0;
            this.HPmax = 200;
            this.HPnow = this.HPmax;
            for (int index = 0; index < this.style.Length; ++index)
                this.style[index] = new Style();
            this.style[0].style = 0;
            this.style[0].element = 0;
            this.setstyle = 0;
            this.havestyles = 1;
            this.efolder = 0;
            for (int index = 0; index < this.canselectmenu.Length; ++index)
                this.canselectmenu[index] = true;
            for (int index = 0; index < this.datelist.Length; ++index)
                this.datelist[index] = false;
            for (int index1 = 0; index1 < this.havechip.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.havechip.GetLength(1); ++index2)
                    this.havechip[index1, index2] = 0;
            }
            this.havechips.Clear();
            for (int index1 = 0; index1 < this.shopCount.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.shopCount.GetLength(1); ++index2)
                    this.shopCount[index1, index2] = 0;
            }
            for (int index = 0; index < this.regularflag.Length; ++index)
                this.regularflag[index] = false;
            for (int index = 0; index < this.regularchip.Length; ++index)
                this.regularchip[index] = 0;
            this.HaveVirus = new Virus[3];
            this.stockVirus = new List<Virus>();
            for (int index = 0; index < this.runSubChips.Length; ++index)
                this.runSubChips[index] = false;
            this.interiors.Clear();
            this.Money = 0;
            this.MaxHz = 10;
            this.MaxCore = 2;
            this.NaviFolder = 5;
            this.darkFolder = 1;
            this.plusFolder = 0;
            this.haveAddon.Clear();
            this.equipAddon.Clear();
            for (int index = 0; index < this.busterspec.Length; ++index)
            {
                this.busterspec[index] = 0;
                this.busterspec[index] = 1;
            }
            for (int index1 = 0; index1 < this.chipFolder.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.chipFolder.GetLength(1); ++index2)
                {
                    for (int index3 = 0; index3 < this.chipFolder.GetLength(2); ++index3)
                    {
                        this.chipFolder[index1, index2, index3] = 0;
                        if (index1 == 0)
                            this.chipFolder[index1, index2, index3] = index2 > 1 ? (index2 > 3 ? (index2 > 5 ? (index2 > 7 ? (index2 > 9 ? (index2 > 12 ? (index2 > 14 ? (index2 > 17 ? (index2 > 19 ? (index2 > 20 ? (index2 > 22 ? (index2 > 25 ? (index2 > 28 ? (index3 != 0 ? 0 : 190) : (index3 != 0 ? 2 : 188)) : (index3 != 0 ? 2 : 174)) : (index3 != 0 ? 1 : 158)) : (index3 != 0 ? 0 : 136)) : (index3 != 0 ? 1 : 121)) : (index3 != 0 ? 2 : 100)) : (index3 != 0 ? 1 : 62)) : (index3 != 0 ? 1 : 59)) : (index3 != 0 ? 0 : 59)) : (index3 != 0 ? 2 : 43)) : (index3 != 0 ? 0 : 43)) : (index3 != 0 ? 1 : 1)) : (index3 != 0 ? 0 : 1);
                        if (index1 == 1)
                            this.chipFolder[index1, index2, index3] = index2 > 1 ? (index2 > 3 ? (index2 > 5 ? (index2 > 7 ? (index2 > 10 ? (index2 > 13 ? (index2 > 15 ? (index2 > 17 ? (index2 > 21 ? (index2 > 25 ? (index2 > 28 ? (index3 != 0 ? 0 : 188) : (index3 != 0 ? 2 : 174)) : (index3 != 0 ? 1 : 158)) : (index3 != 0 ? 1 : 121)) : (index3 != 0 ? 2 : 43)) : (index3 != 0 ? 1 : 43)) : (index3 != 0 ? 2 : 100)) : (index3 != 0 ? 1 : 62)) : (index3 != 0 ? 2 : 59)) : (index3 != 0 ? 0 : 59)) : (index3 != 0 ? 1 : 1)) : (index3 != 0 ? 0 : 1);
                        if (index1 == 2)
                            this.chipFolder[index1, index2, index3] = index2 > 1 ? (index2 > 3 ? (index2 > 5 ? (index2 > 7 ? (index2 > 10 ? (index2 > 13 ? (index2 > 15 ? (index2 > 17 ? (index2 > 20 ? (index2 > 22 ? (index2 > 26 ? (index3 != 0 ? 0 : 188) : (index3 != 0 ? 2 : 174)) : (index3 != 0 ? 1 : 158)) : (index3 != 0 ? 1 : 121)) : (index3 != 0 ? 2 : 43)) : (index3 != 0 ? 1 : 43)) : (index3 != 0 ? 2 : 100)) : (index3 != 0 ? 1 : 62)) : (index3 != 0 ? 2 : 59)) : (index3 != 0 ? 0 : 59)) : (index3 != 0 ? 1 : 1)) : (index3 != 0 ? 0 : 1);
                    }
                }
            }
            for (int index = 0; index < this.stylepoint.Length; ++index)
                this.stylepoint[index] = 0;
            this.havefolder[1] = false;
            this.havefolder[2] = false;
            for (int index = 0; index < this.HaveVirus.Length; ++index)
                this.HaveVirus[index] = null;
            this.haveCaptureBomb = 0;
            for (int index = 0; index < this.havePeace.Length; ++index)
                this.havePeace[index] = 0;
            this.mind = 0;
            for (int index = 0; index < this.flagList.Length; ++index)
                this.flagList[index] = false;
            for (int index = 0; index < this.valList.Count; ++index)
                this.valList[index] = 0;
            for (int index = 0; index < this.getMystery.Length; ++index)
                this.getMystery[index] = false;
            for (int index = 0; index < this.getRandomMystery.Length; ++index)
                this.getRandomMystery[index] = false;
            for (int index1 = 0; index1 < this.bbsRead.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.bbsRead.GetLength(1); ++index2)
                    this.bbsRead[index1, index2] = false;
            }
            for (int index = 0; index < this.questEnd.Length; ++index)
                this.questEnd[index] = false;
            for (int index = 0; index < this.virusSPbustedFlug.Length; ++index)
                this.virusSPbustedFlug[index] = false;
            for (int index = 0; index < this.haveSubChis.Length; ++index)
                this.haveSubChis[index] = 0;
            for (int index = 0; index < this.virusSPbusted.Length; ++index)
                this.virusSPbusted[index] = false;
            for (int index = 0; index < this.questEnd.Length; ++index)
                this.questEnd[index] = false;
            this.mailread.Clear();
            this.mail.Clear();
            this.keyitem.Clear();
            this.AddonReset();
            this.fukasinArea = 0;
            this.time = new byte[4];
            this.haveSubMemory = 2;
            this.havefolder[0] = true;
            this.Regularlarge = 4;
            this.datelist[0] = true;
            this.datelist[42] = true;
            this.datelist[58] = true;
            this.datelist[61] = true;
            this.datelist[99] = true;
            this.datelist[120] = true;
            this.datelist[135] = true;
            this.datelist[157] = true;
            this.datelist[173] = true;
            this.datelist[187] = true;
            this.datelist[189] = true;
            this.shopThread = new Thread(new ThreadStart(this.ShopSave));
            this.shopThread.Start();
            this.flagThread = new Thread(new ThreadStart(this.FlugSave));
            this.flagThread.Start();
            this.valThread = new Thread(new ThreadStart(this.ValSave));
            this.valThread.Start();
            this.mysThread = new Thread(new ThreadStart(this.MysSave));
            this.mysThread.Start();
            this.ranThread = new Thread(new ThreadStart(this.RanSave));
            this.ranThread.Start();
            this.chipThread = new Thread(new ThreadStart(this.ChipSave));
            this.chipThread.Start();
        }

        public void TimePlus()
        {

            ++this.time[0];
            if (this.time[0] < 60)
                return;
            this.time[0] = 0;
            ++this.time[1];
            if (this.time[1] >= 60)
            {
                this.time[1] = 0;
                ++this.time[2];
                if (this.time[2] >= 60)
                {
                    this.time[2] = 0;
                    ++this.time[3];
                }
            }
        }

        public string GetTime()
        {
            string str1 = "";
            if (this.time[3] < 10)
                str1 += "0";
            string str2 = str1 + this.time[3] + ":";
            if (this.time[2] < 10)
                str2 += "0";
            return str2 + this.time[2];
        }

        public string GetHaveManyChips()
        {
            var completionLibrary = new Library(null, null, null, this);
            var normalChips = completionLibrary.LibraryPages[LibraryPageType.Normal].Chips.Count(c => c.IsSeen);
            var naviChips = completionLibrary.LibraryPages[LibraryPageType.Navi].Chips.Count(c => c.IsSeen);
            var darkChips = completionLibrary.LibraryPages[LibraryPageType.Dark].Chips.Count(c => c.IsSeen);
            return $"S{normalChips}/ N{naviChips}/ D{darkChips}";
        }

        public string GetHaveChips()
        {
            int num1 = 0;
            foreach (bool flag in this.havefolder)
            {
                if (flag)
                    num1 += 30;
            }
            byte[,] havechip = this.havechip;
            int upperBound1 = havechip.GetUpperBound(0);
            int upperBound2 = havechip.GetUpperBound(1);
            for (int lowerBound1 = havechip.GetLowerBound(0); lowerBound1 <= upperBound1; ++lowerBound1)
            {
                for (int lowerBound2 = havechip.GetLowerBound(1); lowerBound2 <= upperBound2; ++lowerBound2)
                {
                    byte num2 = havechip[lowerBound1, lowerBound2];
                    num1 += num2;
                }
            }
            return num1.ToString();
        }

        public void AddOnRUN()
        {
            this.AddonReset();
            for (int index = 0; index < this.haveAddon.Count; ++index)
            {
                if (this.equipAddon[index])
                {
                    this.addonNames.Add(this.haveAddon[index].name);
                    this.haveAddon[index].Running(this);
                }
            }
            this.AddonSet();
        }

        public void GetAddon(AddOnBase addon)
        {
            this.haveAddon.Add(addon);
            this.equipAddon.Add(false);
        }

        public void AddonReset()
        {
            this.addonNames.Clear();
            for (int index = 0; index < this.busterspec.Length; ++index)
            {
                this.busterspec[index] = 0;
                this.busterspec[index] = 1;
            }
            this.addonSkill = new bool[Enum.GetNames(typeof(SaveData.ADDONSKILL)).Length];
            this.message = 0;
            this.HPplus = 0;
            this.custom = 0;
            this.NaviFolder = 5;
            this.darkFolder = 1;
            this.plusFolder = 0;
        }

        public void AddonSet()
        {
            if (!this.isJackedIn)
                this.HPNow = this.HPMax;
            for (int index = 0; index < this.busterspec.Length; ++index)
            {
                if (this.busterspec[index] > 5)
                    this.busterspec[index] = 5;
            }
        }

        public int CodeCheck(int chipno, int chipcode)
        {
            ChipFolder chipFolder = new ChipFolder(null);
            chipFolder.SettingChip(chipno);
            for (int index = 0; index < 4; ++index)
            {
                if (chipFolder.chip.code[chipcode] == chipFolder.chip.code[index])
                {
                    chipcode = index;
                    break;
                }
            }
            return chipcode;
        }

        public void AddChip(int chipno, int chipcode, bool head)
        {
            try
            {
                if (chipno <= 0)
                    return;
                chipcode = this.CodeCheck(chipno, chipcode);
                if (!this.datelist[chipno - 1])
                    this.datelist[chipno - 1] = true;
                if (this.havechip[chipno, chipcode] < 99)
                    ++this.havechip[chipno, chipcode];
                if (!head)
                {
                    if (this.ListCheck(chipno, chipcode))
                        return;
                    this.havechips.Add(new ChipS(chipno, chipcode));
                }
                else
                {
                    if (this.ListCheck(chipno, chipcode))
                    {
                        int index = this.ListCheckNumber(chipno, chipcode);
                        if (index != -1)
                            this.havechips.RemoveAt(index);
                    }
                    this.havechips.Insert(0, new ChipS(chipno, chipcode));
                }
            }
            catch
            {
            }
        }

        public void LosChip(int chipno, int chipcode)
        {
            if (this.havechip[chipno, chipcode] > 0)
                --this.havechip[chipno, chipcode];
            if (this.havechip[chipno, chipcode] != 0 || !this.ListCheck(chipno, chipcode))
                return;
            int index = this.ListCheckNumber(chipno, chipcode);
            if (index != -1)
                this.havechips.RemoveAt(index);
        }

        public bool ListCheck(int chipno, int chipcode)
        {
            foreach (ChipS havechip in this.havechips)
            {
                if (havechip.number == chipno && havechip.code == chipcode)
                    return true;
            }
            return false;
        }

        public int ListCheckNumber(int chipno, int chipcode)
        {
            for (int index = 0; index < this.havechips.Count; ++index)
            {
                if (this.havechips[index].number == chipno && this.havechips[index].code == chipcode)
                    return index;
            }
            return -1;
        }

        private static float FloatParseAnySeparator(string str)
        {
            var parseCulture = str.Contains(",")
                // known culture using comma as decimal
                ? System.Globalization.CultureInfo.GetCultureInfo("es-ES")
                : System.Globalization.CultureInfo.InvariantCulture;

            return float.Parse(str, parseCulture);
        }

        public enum EXMESSID
        {
            LHint1,
            LHint2,
            QHint1,
            QHint2,
            Omake1,
            Omake2,
            Omake3,
            Omake4,
            Qinfo,
            QinfoEnd,
            BBS1,
            BBS2,
            BBS3,
            BBS4,
            BBS5,
            BBS6,
        }

        public enum ADDONSKILL
        {
            // BltzBstr
            アサルトバスター,
            // RichRich
            確実ゼニー取得,
            // DataFind
            確実チップ取得,
            // (UNUSED? "Suppresses the appearance of weak enemies", Firewall?)
            弱い敵の出現抑制,
            // (UNUSED? "Charge Shot Invincibility", SprArmr?)
            チャージショット無敵,
            // (UNUSED? "Damage invincibility is doubled", Early DmgGhost?)
            ダメージ無敵時間２倍,
            // FullOpen
            最初のターンだけフルオープン,
            // HeatPeac
            炎属性エンカウント,
            // AquaPeac
            水属性エンカウント,
            // LeafPeac
            草属性エンカウント,
            // ElecPeac
            雷属性エンカウント,
            // PoisPeac
            毒属性エンカウント,
            // ErthPeac
            土属性エンカウント,
            // CAuraSrd
            Ｃオーラソード,
            // CDustBom
            Ｃダストボム,
            // CVulcan
            Ｃバルカン,
            // CFalKnif
            Ｃフォールナイフ,
            // CBlstCan
            Ｃブラストカノン,
            // CLance
            Ｃランス,
            // CShotGun
            Ｃショットガン,
            // CRecov
            Ｃリペア,
            // RStrShld
            Ｒボタンシールド,
            // RHoleFix
            Ｒボタン穴塞ぎ,
            // RPnkCrak
            Ｒボタン穴あけ,
            // LCube
            Ｌボタンキューブ,
            // LHeadWnd
            Ｌボタン向かい風,
            // LTailWnd
            Ｌボタン追い風,
            // LLockOn
            Ｌボタンロックオン,
            // Giv&Take
            ユズリアイ,
            // ReStyle
            スタイル再利用,
            // SpdRunner
            倍速移動,
            // ChipCure
            チップ使用回復,
            // (UNUSED? "Prevent push out", Head/Tailwind immunity?)
            押し出し防止,
            // (UNUSED? "Crack floor disabled", FlotShoe?)
            ヒビ床無効,
            // (UNUSED? "No folder shuffling", early UnShuffle?)
            フォルダシャッフル無し,
            // (UNUSED? "Zero version of Shinobi style", unknown)
            ゼロ版シノビスタイル,
            // Ammo
            薬莢,
            // BigAmmo
            薬莢大,
            // BlueBstr
            青バスター,
            // RunSoul
            逃走率１００パー,
            // MeltSelf
            常時メルト,
            // Slippery
            常時スリップ,
            // LostLght
            常時ブラインド,
            // HvyFoot
            常時ヘビィ,
            // AcidBody
            常時ポイズン,
            // CustPain
            カスタム毎にダメージ,
            // AreaHold
            フカシンエリア,
            // MyGarden
            自エリア整理,
            // SBarrier
            開始時バリア,
            // DmgGhost
            無敵時間増加,
            // HoldChrg
            チャージストック,
            // AngrMind
            不安が怒りに変化,
            // CrimNois
            イリーガルゲット,
            // ChipChrg
            チャージで威力１０アップ,
            // JunkBstr
            バスター空打ち,
            // DarkMind
            常にダーク状態,
            // StunDmg
            ダメージでマヒ,
            // HideLife
            敵HP視認不可,
            // LostArea
            ハイスイノジン,
            // (UNUSED? "Sense of Humor", early Humor?)
            ユーモアセンス,
            // (UNUSED? "Aelin Call", early EirnCall?)
            エーリンコール,
            // (UNUSED? "Line Change 1", unknown)
            セリフ変更１,
            // (UNUSED? "Line Change 2", unknown)
            セリフ変更２,
            // Statue
            フドウミョウオウ,
            // NoGuard
            ノーガード,
            // ChipPain
            ユーズドペイン,
            // SlowStrt
            スロウスタート,
            // HrdObjct
            ハードオブジェ,
            // AutoADD
            オートＡＤＤ,
            // AutoChrg
            オートチャージ,
            // UnderSht
            キシカイセイ,
            // Unshuffle
            アンシャッフル,
            // Scavenger
            Scavenger,
            // Sacrifice
            Sacrifice,
            // Mammon
            Mammon,
        }
    }
}
