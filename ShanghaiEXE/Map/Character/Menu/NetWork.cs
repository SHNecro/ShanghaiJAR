using Common;
using NSBattle;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEvent;
using NSGame;
using NSNet;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace NSMap.Character.Menu
{
    internal class NetWork : MenuBase
    {
        private readonly string[,] musicListJP = new string[3, 3]
        {
      {
        "ＶＳウィルス",
        "VSvirus",
        "Virus Battle"
      },
      {
        "トーナメント戦",
        "Tournament",
        "Tournament Battle"
      },
      {
        "ＶＳネットナビ",
        "VSnavi",
        "Navi Battle"
      }
        };
        private readonly int[,] faceList = new int[42, 2]
        {
      {
        3,
        0
      },
      {
        3,
        1
      },
      {
        3,
        2
      },
      {
        3,
        3
      },
      {
        3,
        4
      },
      {
        3,
        5
      },
      {
        3,
        6
      },
      {
        3,
        7
      },
      {
        3,
        8
      },
      {
        3,
        9
      },
      {
        3,
        10
      },
      {
        3,
        11
      },
      {
        3,
        12
      },
      {
        3,
        13
      },
      {
        3,
        14
      },
      {
        7,
        0
      },
      {
        7,
        1
      },
      {
        7,
        2
      },
      {
        7,
        3
      },
      {
        9,
        0
      },
      {
        9,
        1
      },
      {
        9,
        2
      },
      {
        9,
        3
      },
      {
        9,
        4
      },
      {
        9,
        5
      },
      {
        9,
        6
      },
      {
        2,
        0
      },
      {
        2,
        1
      },
      {
        2,
        2
      },
      {
        2,
        3
      },
      {
        2,
        4
      },
      {
        2,
        5
      },
      {
        2,
        6
      },
      {
        2,
        7
      },
      {
        2,
        8
      },
      {
        2,
        10
      },
      {
        2,
        11
      },
      {
        2,
        12
      },
      {
        6,
        0
      },
      {
        6,
        1
      },
      {
        4,
        10
      },
      {
        4,
        11
      }
        };
        private NetWork.MENU nextmenu = NetWork.MENU.topMenu;
        private readonly int[] menuXminas = new int[4];
        private readonly List<NSGame.Mail> mails = new List<NSGame.Mail>();
        private int[] enemyName = new int[10];
        private const string defoMusic = "main_center";
        private int connectTime;
        private int setMusic;
        private NameEdit nameedit;
        public NetWork.SCENE nowscene;
        private NetWork.MENU nowmenu;
        private readonly EventManager eventmanager;
        private int cursol;
        private readonly int top;
        private int cursolanime;
        private bool iconanime;
        private readonly IPAddress[] adrList;
        private readonly string playBGM;
        private bool goNameEdit;
        private bool settingEventoNow;
        private int battleStandbyProsess;
        public static Thread connectThread;
        public NetBattle netBattleScene;
        public NetPlayer netPlayer;
        private int enemyFace;
        private int back;

        private int OverTop
        {
            get
            {
                return this.mails.Count - 5;
            }
        }

        private int Select
        {
            get
            {
                return this.cursol + this.top;
            }
        }

        public NetWork(IAudioEngine s, Player p, TopMenu t, SaveData save)
          : base(s, p, t, save)
        {
            this.playBGM = this.sound.CurrentBGM;
            this.sound.StartBGM("main_center");
            this.adrList = Dns.GetHostAddresses(Dns.GetHostName());
            NetParam.myIP = "";
            foreach (IPAddress adr in this.adrList)
            {
                if (adr.ToString().Split('.')[0] == "25")
                {
                    NetParam.myIP = adr.ToString();
                    break;
                }
            }
            if (NetParam.myIP == "")
            {
                foreach (IPAddress adr in this.adrList)
                {
                    if (adr.AddressFamily == AddressFamily.InterNetwork)
                    {
                        NetParam.myIP = adr.ToString();
                        break;
                    }
                }
            }
            this.eventmanager = new EventManager(this.player.parent, this.sound);
            var dialogue = ShanghaiEXE.Translate("NetWork.IntroDialogue1");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
            bool flag = false;
            for (int index = 0; index < this.savedata.netWorkName.Length; ++index)
            {
                if ((uint)this.savedata.netWorkName[index] > 0U)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
                return;
            this.goNameEdit = true;
        }

        public override void UpDate()
        {
            int num1 = -104;
            int num2 = 16;
            if (this.nowmenu != this.nextmenu && this.menuXminas[0] < 0)
            {
                for (int index = 0; index < this.menuXminas.Length; ++index)
                {
                    if (index == 0)
                    {
                        if (this.menuXminas[index] > num1)
                            this.menuXminas[index] -= num2;
                    }
                    else
                    {
                        if (this.menuXminas[index] > num1 && this.menuXminas[index - 1] < -24)
                            this.menuXminas[index] -= num2;
                        if (this.menuXminas[this.menuXminas.Length - 1] <= num1)
                            this.nowmenu = this.nextmenu;
                    }
                }
            }
            else if (this.menuXminas[this.menuXminas.Length - 1] < 0)
            {
                for (int index = 0; index < this.menuXminas.Length; ++index)
                {
                    if (this.menuXminas[index] < 0)
                        this.menuXminas[index] += num2;
                }
            }
            else
            {
                switch (this.nowscene)
                {
                    case NetWork.SCENE.fadein:
                        if (this.Alpha > 0)
                        {
                            this.Alpha -= 51;
                            break;
                        }
                        this.nowscene = NetWork.SCENE.select;
                        this.nowmenu = NetWork.MENU.topMenu;
                        this.nextmenu = NetWork.MENU.topMenu;
                        break;
                    case NetWork.SCENE.select:
                        if (this.eventmanager.playevent)
                        {
                            this.eventmanager.UpDate();
                        }
                        else
                        {
                            switch (this.nowmenu)
                            {
                                case NetWork.MENU.IPInput:
                                    if (!this.settingEventoNow)
                                    {
                                        this.eventmanager.events.Clear();
                                        this.eventmanager.AddEvent(new IPset(this.sound, this.eventmanager, "相手のＩＰアドレスを入力してね", 5, 14, false, this.savedata, -1));
                                        this.settingEventoNow = true;
                                        break;
                                    }
                                    if (NetParam.IP >= 0L)
                                    {
                                        NetParam.ConnectIP = NetParam.IP;
                                        this.nowmenu = NetWork.MENU.ClientWait;
                                        this.nextmenu = NetWork.MENU.ClientWait;
                                    }
                                    else
                                    {
                                        this.nextmenu = NetWork.MENU.topMenu;
                                        this.menuXminas[0] -= 16;
                                    }
                                    break;
                                case NetWork.MENU.ServerWait:
                                    this.ServerWait();
                                    break;
                                case NetWork.MENU.ClientWait:
                                    this.ClientWait();
                                    break;
                                case NetWork.MENU.BattleStandby:
                                    this.BattleStandby();
                                    if (this.battleStandbyProsess <= 5 && !NetParam.battleEnd)
                                    {
                                        this.ConnectOut();
                                        break;
                                    }
                                    break;
                                default:
                                    this.Control();
                                    break;
                            }
                        }
                        this.FlamePlus();
                        if (this.frame % 10 == 0)
                        {
                            ++this.cursolanime;
                            this.iconanime = !this.iconanime;
                        }
                        if (this.nowmenu == NetWork.MENU.ServerWait || this.nowmenu == NetWork.MENU.ClientWait)
                        {
                            if (this.cursolanime >= 8)
                            {
                                this.cursolanime = 0;
                                break;
                            }
                            break;
                        }
                        if (this.cursolanime >= 3)
                            this.cursolanime = 0;
                        break;
                    case NetWork.SCENE.NameEdit:
                        this.nameedit.UpDate();
                        break;
                    case NetWork.SCENE.fadeout:
                        if (this.Alpha < byte.MaxValue)
                        {
                            this.Alpha += 51;
                            break;
                        }
                        if (this.nextmenu == NetWork.MENU.NameEdit)
                            this.nowscene = NetWork.SCENE.NameEdit;
                        else
                            this.topmenu.Return();
                        break;
                }
            }
        }

        private void ConnectOut()
        {
            if (NetParam.connecting)
                return;
            this.sound.PlaySE(SoundEffect.error);
            this.eventmanager.events.Clear();
            var dialogue = ShanghaiEXE.Translate("NetWork.ConnectionFailureDialogue1");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
            this.nowscene = NetWork.SCENE.select;
            this.nowmenu = NetWork.MENU.topMenu;
            this.nextmenu = NetWork.MENU.topMenu;
            this.Reset();
            this.menuXminas[0] = 0;
            this.menuXminas[1] = 0;
            this.menuXminas[2] = 0;
            this.menuXminas[3] = 0;
        }

        private void ClientWait()
        {
            if (!NetParam.connectWait)
            {
                this.sound.StartBGM("main_center");
                NetWork.connectThread = new Thread(new ThreadStart(NetParam.Client));
                NetWork.connectThread.Start();
                NetParam.connectWait = true;
            }
            else if (NetParam.connecting)
            {
                NetWork.connectThread.Abort();
                Task.Run(() => NetParam.SendData());
                Task.Run(() => NetParam.ReadData());
                this.BattleStartUpSend();
                this.battleStandbyProsess = 0;
                NetParam.connectWait = false;
                this.sound.PlaySE(SoundEffect.docking);
                this.nextmenu = NetWork.MENU.BattleStandby;
                this.nowmenu = NetWork.MENU.BattleStandby;
            }
            if (!Input.IsPress(Button._B))
                return;
            NetWork.connectThread.Abort();
            this.sound.PlaySE(SoundEffect.cancel);
            this.nextmenu = NetWork.MENU.topMenu;
            this.menuXminas[0] -= 16;
        }

        private void ServerWait()
        {
            if (!NetParam.connectWait)
            {
                this.sound.StartBGM("main_center");
                NetWork.connectThread = new Thread(new ThreadStart(NetParam.Server));
                NetWork.connectThread.Start();
                NetParam.connectWait = true;
            }
            else if (NetParam.connecting)
            {
                NetWork.connectThread.Abort();
                Task.Run(() => NetParam.SendData());
                Task.Run(() => NetParam.ReadData());
                this.BattleStartUpSend();
                this.battleStandbyProsess = 0;
                NetParam.connectWait = false;
                this.sound.PlaySE(SoundEffect.docking);
                this.nextmenu = NetWork.MENU.BattleStandby;
                this.nowmenu = NetWork.MENU.BattleStandby;
            }
            if (!Input.IsPress(Button._B))
                return;
            NetParam.connecting = false;
            NetParam.Close();
            NetWork.connectThread.Abort();
            this.sound.PlaySE(SoundEffect.cancel);
            this.nextmenu = NetWork.MENU.ModeMenu;
            this.menuXminas[0] -= 16;
        }

        private void BattleStandby()
        {
            switch (this.battleStandbyProsess)
            {
                case 0:
                    string[] strArray1 = NetParam.DataUse("100");
                    if (strArray1 != null)
                    {
                        this.enemyFace = int.Parse(strArray1[3]);
                        this.enemyName = new int[10];
                        string[] strArray2 = strArray1[4].Split(',');
                        for (int index = 0; index < this.enemyName.Length; ++index)
                            this.enemyName[index] = int.Parse(strArray2[index]);
                        NetParam.enemyIP = strArray1[5];
                        this.menuXminas[0] = 120;
                        this.back = this.Random.Next(4, 30);
                        ++this.battleStandbyProsess;
                        break;
                    }
                    this.BattleStartUpSend();
                    break;
                case 1:
                    if (NetParam.Host)
                        NetParam.SendingData(101, "OK@" + this.back.ToString());
                    else
                        NetParam.SendingData(101, "OK");
                    string[] strArray3 = NetParam.DataUse("101");
                    if (strArray3 != null)
                    {
                        if (strArray3[3] == "OK")
                        {
                            NetParam.StartUDP();
                            this.connectTime = 0;
                            ++this.battleStandbyProsess;
                            if (!NetParam.Host)
                                this.back = int.Parse(strArray3[4]);
                            break;
                        }
                        break;
                    }
                    this.BattleStartUpSend();
                    break;
                case 2:
                    this.menuXminas[0] -= 12;
                    if (this.menuXminas[0] <= 0)
                    {
                        this.netBattleScene = null;
                        this.netPlayer = null;
                        ++this.battleStandbyProsess;
                        break;
                    }
                    break;
                case 3:
                    if (this.netBattleScene == null)
                    {
                        int[] numArray1 = new int[3];
                        int[] numArray2 = new int[3];
                        int[] numArray3 = new int[3];
                        int[] numArray4 = new int[3];
                        for (int index = 0; index < numArray1.Length; ++index)
                        {
                            if (this.savedata.HaveVirus[index] != null)
                            {
                                numArray1[index] = this.savedata.HaveVirus[index].type;
                                numArray2[index] = this.savedata.HaveVirus[index].eatBug;
                                numArray3[index] = this.savedata.HaveVirus[index].eatFreeze;
                                numArray4[index] = this.savedata.HaveVirus[index].eatError;
                            }
                            else
                                numArray1[index] = -1;
                        }
                        NetParam.SendingData(102, this.savedata.HPMax.ToString() + "@" + string.Join<byte>(",", savedata.busterspec) + "@" + string.Join<bool>(",", savedata.addonSkill) + "@" + this.savedata.style[this.savedata.setstyle].fileName + "@" + string.Join<int>(",", numArray1) + "@" + string.Join<int>(",", numArray2) + "@" + string.Join<int>(",", numArray3) + "@" + string.Join<int>(",", numArray4));
                        this.netBattleScene = new NetBattle(this.sound, this.player.parent.parent, this.player.parent.main, this.eventmanager, false, 0, false, this.musicListJP[this.setMusic, 1], this.savedata);
                    }
                    if (this.netPlayer == null && this.netBattleScene != null)
                    {
                        string[] strArray2 = NetParam.DataUse("102");
                        if (strArray2 != null)
                        {
                            byte[] array = ((IEnumerable<string>)strArray2[4].Split(',')).Select<string, byte>(new Func<string, byte>(byte.Parse)).ToArray<byte>();
                            this.netPlayer = new NetPlayer(this.sound, netBattleScene, 4, 1, this.player.parent.main, array[0], array[1], array[2], new MindWindow(this.sound, netBattleScene, this.savedata), this.savedata);
                            this.netPlayer.ParamSet(int.Parse(strArray2[3]), ((IEnumerable<string>)strArray2[5].Split(',')).Select<string, bool>(new Func<string, bool>(bool.Parse)).ToArray<bool>(), strArray2[6], ((IEnumerable<string>)strArray2[7].Split(',')).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>(), ((IEnumerable<string>)strArray2[8].Split(',')).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>(), ((IEnumerable<string>)strArray2[9].Split(',')).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>(), ((IEnumerable<string>)strArray2[10].Split(',')).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>());
                            this.netPlayer.enemyName = this.enemyName;
                        }
                    }
                    if (this.netBattleScene != null && this.netPlayer != null)
                    {
                        NetParam.SendingData(103, "OK@" + this.connectTime.ToString());
                        string[] strArray2 = NetParam.DataUse("103");
                        if (strArray2 != null && strArray2[3] == "OK")
                        {
                            NetParam.readDeray = this.connectTime - int.Parse(strArray2[4]);
                            ++this.battleStandbyProsess;
                            break;
                        }
                        break;
                    }
                    this.netBattleScene = null;
                    break;
                case 4:
                    ++this.waittime;
                    NetParam.SendingData(103, "OK@" + this.connectTime.ToString());
                    if (this.waittime >= 120)
                    {
                        ++this.battleStandbyProsess;
                        this.waittime = 0;
                        Panel.PANEL[] panelArray = new Panel.PANEL[2];
                        int type = 0;
                        this.eventmanager.events.Clear();
                        this.eventmanager.AddEvent(new NSEvent.Battle(this.sound, this.eventmanager, this.netBattleScene, this.netPlayer, panelArray[0], panelArray[1], type, false, false, true, this.back, this.savedata));
                        break;
                    }
                    break;
                case 5:
                    ++this.battleStandbyProsess;
                    this.sound.StartBGM("main_center");
                    break;
                case 6:
                    this.battleStandbyProsess = 0;
                    this.eventmanager.events.Clear();
                    this.nextmenu = NetWork.MENU.topMenu;
                    this.menuXminas[0] -= 16;
                    NetParam.Close();
                    this.Reset();
                    var dialogue = new Dialogue();
                    switch (this.savedata.selectQuestion)
                    {
                        case -1:
                            this.ConnectOut();
                            break;
                        case 0:
                            dialogue = ShanghaiEXE.Translate("NetWork.BattleVictoryDialogue1");
                            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
                            break;
                        case 1:
                            dialogue = ShanghaiEXE.Translate("NetWork.BattleDefeatDialogue1");
                            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
                            break;
                        case 2:
                            dialogue = ShanghaiEXE.Translate("NetWork.BattleDrawDialogue1");
                            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
                            break;
                    }
                    break;
            }
            if (this.battleStandbyProsess < 2)
                return;
            ++this.connectTime;
        }

        private void Reset()
        {
            this.netBattleScene = null;
            this.netPlayer = null;
            NetParam.Reset();
        }

        private void BattleStartUpSend()
        {
            string str = "";
            for (int index = 0; index < this.savedata.netWorkName.Length; ++index)
            {
                str += (string)(object)this.savedata.netWorkName[index];
                if (index < this.savedata.netWorkName.Length - 1)
                    str += ",";
            }
            NetParam.SendingData(100, this.savedata.netWorkFace.ToString() + "@" + str + "@" + NetParam.myIP);
        }

        private void Control()
        {
            if (this.goNameEdit)
            {
                this.nowscene = NetWork.SCENE.fadeout;
                this.nameedit = new NameEdit(this.sound, this.player, this.topmenu, this.savedata, this);
                this.nextmenu = NetWork.MENU.NameEdit;
                this.cursol = 3;
                this.goNameEdit = false;
            }
            else
            {
                if (Input.IsPress(Button._A))
                {
                    this.sound.PlaySE(SoundEffect.decide);
                    if (this.nowscene == NetWork.SCENE.select)
                    {
                        switch (this.nowmenu)
                        {
                            case NetWork.MENU.topMenu:
                                switch (this.cursol)
                                {
                                    case 0:
                                        this.nextmenu = NetWork.MENU.ModeMenu;
                                        this.menuXminas[0] -= 16;
                                        break;
                                    case 1:
                                        this.nextmenu = NetWork.MENU.IPInput;
                                        this.settingEventoNow = false;
                                        this.menuXminas[0] -= 16;
                                        break;
                                    case 2:
                                        this.nextmenu = NetWork.MENU.Memory;
                                        this.menuXminas[0] -= 16;
                                        break;
                                    default:
                                        this.nowscene = NetWork.SCENE.fadeout;
                                        this.nameedit = new NameEdit(this.sound, this.player, this.topmenu, this.savedata, this);
                                        this.nextmenu = NetWork.MENU.NameEdit;
                                        break;
                                }
                                break;
                            case NetWork.MENU.ModeMenu:
                                switch (this.cursol)
                                {
                                    case 0:
                                        this.nextmenu = NetWork.MENU.ServerWait;
                                        this.menuXminas[0] -= 16;
                                        break;
                                    case 1:
                                        this.nextmenu = NetWork.MENU.ServerWait;
                                        this.menuXminas[0] -= 16;
                                        break;
                                }
                                break;
                        }
                    }
                }
                if (Input.IsPress(Button._B))
                {
                    this.sound.PlaySE(SoundEffect.cancel);
                    switch (this.nowmenu)
                    {
                        case NetWork.MENU.topMenu:
                            this.sound.StartBGM(this.playBGM);
                            this.nextmenu = NetWork.MENU.topMenu;
                            this.nowscene = NetWork.SCENE.fadeout;
                            break;
                        case NetWork.MENU.ModeMenu:
                            this.nextmenu = NetWork.MENU.topMenu;
                            this.menuXminas[0] -= 16;
                            break;
                    }
                }
                if (this.waittime <= 0)
                {
                    int num = 4;
                    if (this.nowmenu == NetWork.MENU.ModeMenu)
                        num = 2;
                    if (Input.IsPress(Button.Up))
                    {
                        --this.cursol;
                        if (this.cursol < 0)
                            this.cursol = num - 1;
                        this.sound.PlaySE(SoundEffect.movecursol);
                    }
                    if (Input.IsPress(Button.Down))
                    {
                        ++this.cursol;
                        if (this.cursol >= num)
                            this.cursol = 0;
                        this.sound.PlaySE(SoundEffect.movecursol);
                    }
                    else if (Input.IsPress(Button._R))
                    {
                        this.sound.PlaySE(SoundEffect.decide);
                        if (this.nowmenu == NetWork.MENU.topMenu && this.cursol == 3)
                        {
                            ++this.savedata.netWorkFace;
                            if (this.savedata.netWorkFace >= this.faceList.GetLength(0))
                                this.savedata.netWorkFace = 0;
                        }
                        else
                        {
                            ++this.setMusic;
                            if (this.setMusic >= this.musicListJP.GetLength(0))
                                this.setMusic = 0;
                            this.sound.StartBGM(this.musicListJP[this.setMusic, 1]);
                        }
                    }
                    else if (Input.IsPress(Button._L))
                    {
                        this.sound.PlaySE(SoundEffect.decide);
                        if (this.nowmenu == NetWork.MENU.topMenu && this.cursol == 3)
                        {
                            --this.savedata.netWorkFace;
                            if (this.savedata.netWorkFace < 0)
                                this.savedata.netWorkFace = this.faceList.GetLength(0) - 1;
                        }
                        else
                        {
                            --this.setMusic;
                            if (this.setMusic < 0)
                                this.setMusic = this.musicListJP.GetLength(0) - 1;
                            this.sound.StartBGM(this.musicListJP[this.setMusic, 1]);
                        }
                    }
                }
                else
                    --this.waittime;
            }
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(0, 1104, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            if (this.nowmenu != NetWork.MENU.BattleStandby)
            {
                this._rect = new Rectangle(240, 1104, 144, 40);
                this._position = new Vector2(96f, 0.0f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                string[] strArray = NetParam.myIP.Split('.');
                for (int index = 0; index < strArray.Length; ++index)
                {
                    if (strArray[index].Length < 3)
                        strArray[index] = "0" + strArray[index];
                    if (strArray[index].Length < 3)
                        strArray[index] = "0" + strArray[index];
                    this.TextRender(dg, strArray[index], false, new Vector2(104 + 32 * index, 16f), true);
                }
                this._rect = new Rectangle(528, 1104, 104, 40);
                this._position = new Vector2(96f, 32f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                for (int index = 0; index < this.savedata.netWorkName.Length; ++index)
                {
                    this._position = new Vector2(104 + 8 * index, 48f);
                    this._rect = new Rectangle(8 * this.savedata.netWorkName[index], 16, 8, 16);
                    dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                }
                this._position = new Vector2(192f, 32f);
                this._rect = new Rectangle(240, 1144, 48, 40);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                this._position = new Vector2(202f, 40f);
                this._rect = new Rectangle(0, 48 * this.faceList[this.savedata.netWorkFace, 1], 40, 48);
                dg.DrawImage(dg, "Face" + this.faceList[this.savedata.netWorkFace, 0], this._rect, true, this._position, 0.5f, 0.0f, Color.White);
                this._rect = new Rectangle(384, 1104, 144, 40);
                this._position = new Vector2(96f, 64f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                this._position = new Vector2(112f, 80f);
                var musicName = ShanghaiEXE.language == 1
                    ? this.musicListJP[this.setMusic, 2]
                    : this.musicListJP[this.setMusic, 0];
                dg.DrawText(musicName, this._position, true);
                for (int index = 0; index < this.menuXminas.Length; ++index)
                {
                    this._position = new Vector2(8 + this.menuXminas[index], 16 + 16 * index);
                    switch (this.nowmenu)
                    {
                        case NetWork.MENU.topMenu:
                            this._rect = new Rectangle(288, 1144 + 24 * index, 80, 24);
                            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                            break;
                        case NetWork.MENU.ModeMenu:
                            if (index < 2)
                            {
                                this._rect = new Rectangle(288, 1240 + 24 * index, 80, 24);
                                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                                break;
                            }
                            break;
                    }
                }
                if ((uint)this.nowmenu <= 1U && (this.nowmenu == this.nextmenu && this.menuXminas[this.menuXminas.Length - 1] == 0))
                {
                    this._rect = new Rectangle(112 + 16 * this.cursolanime, 160, 16, 16);
                    this._position = new Vector2(0.0f, 20 + this.cursol * 16);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                }
            }
            else if (this.battleStandbyProsess >= 1)
            {
                this._position = new Vector2(44 - this.menuXminas[0], 16f);
                this._rect = new Rectangle(0, 48 * this.faceList[this.savedata.netWorkFace, 1], 40, 48);
                dg.DrawImage(dg, "Face" + this.faceList[this.savedata.netWorkFace, 0], this._rect, true, this._position, Color.White);
                this._rect = new Rectangle(528, 1104, 104, 40);
                this._position = new Vector2(16 - this.menuXminas[0], 64f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                for (int index = 0; index < this.savedata.netWorkName.Length; ++index)
                {
                    this._position = new Vector2(24 + 8 * index - this.menuXminas[0], 80f);
                    this._rect = new Rectangle(8 * this.savedata.netWorkName[index], 16, 8, 16);
                    dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                }
                this._position = new Vector2(156 + this.menuXminas[0], 16f);
                this._rect = new Rectangle(0, 48 * this.faceList[this.enemyFace, 1], 40, 48);
                dg.DrawImage(dg, "Face" + this.faceList[this.enemyFace, 0], this._rect, true, this._position, Color.White);
                this._rect = new Rectangle(528, 1104, 104, 40);
                this._position = new Vector2(128 + this.menuXminas[0], 64f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                for (int index = 0; index < this.enemyName.Length; ++index)
                {
                    this._position = new Vector2(136 + 8 * index + this.menuXminas[0], 80f);
                    this._rect = new Rectangle(8 * this.enemyName[index], 16, 8, 16);
                    dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                }
                this._rect = new Rectangle(240, 1184, 48, 16);
                this._position = new Vector2(96f, 32 - this.menuXminas[0]);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            if (this.eventmanager.playevent)
                this.eventmanager.Render(dg);
            else if (!this.savedata.FlagList[0])
            {
                this._position = new Vector2(5f, 108f);
                this._rect = new Rectangle(0, 672, 40, 48);
                dg.DrawImage(dg, "Face5", this._rect, true, this._position, Color.White);
                var dialogue = new Dialogue();
                var strArray = new string[3];
                switch (this.nowmenu)
                {
                    case NetWork.MENU.topMenu:
                        switch (this.cursol)
                        {
                            case 0:
                                dialogue = ShanghaiEXE.Translate("NetWork.ClientDialogue1");
                                strArray[0] = dialogue[0];
                                strArray[1] = dialogue[1];
                                strArray[2] = ShanghaiEXE.Translate("NetWork.BGMChange");
                                break;
                            case 1:
                                dialogue = ShanghaiEXE.Translate("NetWork.ConnectToHostDialogue1");
                                strArray[0] = dialogue[0];
                                strArray[1] = dialogue[1];
                                strArray[2] = ShanghaiEXE.Translate("NetWork.BGMChange");
                                break;
                            case 2:
                                dialogue = ShanghaiEXE.Translate("NetWork.PreviousConnectionDialogue1");
                                strArray[0] = dialogue[0];
                                strArray[1] = dialogue[1];
                                strArray[2] = ShanghaiEXE.Translate("NetWork.BGMChange");
                                break;
                            case 3:
                                dialogue = ShanghaiEXE.Translate("NetWork.ChangeNameDialogue1");
                                strArray[0] = dialogue[0];
                                strArray[1] = dialogue[1];
                                strArray[2] = ShanghaiEXE.Translate("NetWork.FaceChange");
                                break;
                        }
                        break;
                    case NetWork.MENU.ModeMenu:
                        switch (this.cursol)
                        {
                            case 0:
                                dialogue = ShanghaiEXE.Translate("NetWork.EmptyStageDialogue1");
                                strArray[0] = dialogue[0];
                                strArray[1] = dialogue[1];
                                strArray[2] = ShanghaiEXE.Translate("NetWork.BGMChange");
                                break;
                            case 1:
                                dialogue = ShanghaiEXE.Translate("NetWork.VariedStageDialogue1");
                                strArray[0] = dialogue[0];
                                strArray[1] = dialogue[1];
                                strArray[2] = ShanghaiEXE.Translate("NetWork.BGMChange");
                                break;
                        }
                        break;
                    case NetWork.MENU.ServerWait:
                        dialogue = ShanghaiEXE.Translate("NetWork.WaitForConnectionDialogue1");
                        strArray[0] = dialogue[0];
                        strArray[1] = dialogue[1];
                        strArray[2] = dialogue[2];
                        this._rect = new Rectangle(456, 1144 + 24 * this.cursolanime, 24, 24);
                        this._position = new Vector2(208f, 120f);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        break;
                    case NetWork.MENU.ClientWait:
                        dialogue = ShanghaiEXE.Translate("NetWork.WaitingToConnectDialogue1");
                        strArray[0] = dialogue[0];
                        strArray[1] = dialogue[1];
                        strArray[2] = dialogue[2];
                        this._rect = new Rectangle(456, 1144 + 24 * this.cursolanime, 24, 24);
                        this._position = new Vector2(208f, 120f);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        break;
                    case NetWork.MENU.BattleStandby:
                        if (this.battleStandbyProsess >= 4)
                        {
                            dialogue = ShanghaiEXE.Translate("NetWork.BattleStartDialogue1");
                            strArray[0] = dialogue[0];
                            strArray[1] = dialogue[1];
                            strArray[2] = dialogue[2];
                            break;
                        }
                        break;
                }
                this._position = new Vector2(48f, 108f);
                dg.DrawText(strArray[0], this._position);
                this._position = new Vector2(48f, 124f);
                dg.DrawText(strArray[1], this._position);
                this._position = new Vector2(48f, 140f);
                dg.DrawText(strArray[2], this._position);
            }
            if (this.nowscene == NetWork.SCENE.fadein || this.nowscene == NetWork.SCENE.fadeout)
            {
                Color color = Color.FromArgb(this.Alpha, 0, 0, 0);
                this._rect = new Rectangle(0, 0, 240, 160);
                this._position = new Vector2(0.0f, 0.0f);
                dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color);
            }
            if (this.nowscene == NetWork.SCENE.NameEdit)
                this.nameedit.Render(dg);
            this._position = new Vector2(0.0f, 0.0f);
        }

        public enum SCENE
        {
            fadein,
            select,
            NameEdit,
            fadeout,
        }

        private enum MENU
        {
            topMenu,
            ModeMenu,
            IPInput,
            Memory,
            NameEdit,
            ServerWait,
            ClientWait,
            BattleStandby,
        }
    }
}
