using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NSNet
{
    public static class NetParam
    {
        public static int DEFAULT_PORT = 40753;
        public static int UDP_PORTS = 40754;
        public static int UDP_PORTC = 40755;
        public static TcpListener server = null;
        private static List<string> catchLog = new List<string>();
        private static List<string> sendLog = new List<string>();
        private static int[] nextChipRS = new int[1];
        private static int[] ENnextChipRS = new int[1];
        public static List<int> positionX = new List<int>();
        public static List<int> positionY = new List<int>();
        public static int[] ENpositionX = new int[10];
        public static int[] ENpositionY = new int[10];
        public static int[] panelColor = new int[9];
        public static int[] panelElem = new int[9];
        public static int[] ENpanelColor = new int[9];
        public static int[] ENpanelElem = new int[9];
        public const bool LOCAL_TEST = false;
        public const bool TEST_MODE = false;
        public static int BO_Chip_Used;
        public static int readDeray;
        public static long ConnectIP;
        public static long connectTime;
        public static string enemyIP;
        public static bool reConnect;
        public const string HEADER = "SHEX";
        public static long IP;
        public static bool battleEnd;
        public static string myIP;
        private static TcpClient client;
        private static UdpClient udpReceiver;
        private static UdpClient udpSender;
        private static IPEndPoint localEP;
        public static bool connectWait;
        public static bool connecting;
        public static int backFlame;
        public static bool Host;
        public static long sendflame;
        private static int nextChipID;
        private static int nextChipPP;
        private static bool nextChipTS;
        private static bool nextChipPL;
        private static int ENnextChipID;
        private static int ENnextChipPP;
        private static bool ENnextChipTS;
        private static bool ENnextChipPL;
        private const int record = 10;
        public static int logFlame;

        public static int UDP_MY_PORT
        {
            get
            {
                return NetParam.Host ? NetParam.UDP_PORTS : NetParam.UDP_PORTC;
            }
        }

        public static int UDP_YOUR_PORT
        {
            get
            {
                return !NetParam.Host ? NetParam.UDP_PORTS : NetParam.UDP_PORTC;
            }
        }

        public static string ConnectIPAddress
        {
            get
            {
                return NetParam.ConnectIP.ToString();
            }
        }

        public static string GetIPAddress(long ip)
        {
            int[] numArray = new int[4]
            {
        0,
        0,
        0,
        (int) (ip / 1000000000L)
            };
            numArray[2] = (int)(ip % 1000000000L / 1000000L);
            numArray[1] = (int)(ip % 1000000L / 1000L);
            numArray[0] = (int)(ip % 1000L);
            return numArray[3].ToString() + "." + numArray[2].ToString() + "." + numArray[1].ToString() + "." + numArray[0].ToString();
        }

        public static void ReConnect()
        {
            NetParam.reConnect = true;
            NetParam.udpReceiver.Close();
            NetParam.udpSender.Close();
            NetParam.StartUDP();
            NetParam.reConnect = false;
        }

        public static bool UPnP()
        {
            return new UPnP().AddUdpPortMapping(41753, 0, "TCP Connect");
        }

        public static void Reset()
        {
            if (NetParam.server != null)
                NetParam.server.Stop();
            if (NetParam.client != null)
                NetParam.client.Close();
            if (NetParam.udpReceiver != null)
                NetParam.udpReceiver.Close();
            if (NetParam.udpSender != null)
                NetParam.udpSender.Close();
            NetParam.sendLog.Clear();
            NetParam.catchLog.Clear();
            NetParam.server = null;
            NetParam.client = null;
            NetParam.udpReceiver = null;
            NetParam.udpSender = null;
            NetParam.localEP = null;
        }

        private static void Init()
        {
            NetParam.BO_Chip_Used = 0;
            NetParam.connectTime = 0L;
            NetParam.readDeray = 0;
            NetParam.sendflame = 0L;
            NetParam.connecting = true;
        }

        public static void Server()
        {
            try
            {
                int defaultPort = NetParam.DEFAULT_PORT;
                NetParam.server = new TcpListener(IPAddress.Parse(NetParam.myIP), defaultPort);
                NetParam.server.Start();
                Console.WriteLine("接続待機中");
                NetParam.client = NetParam.server.AcceptTcpClient();
                Console.WriteLine("接続されました");
                NetParam.Init();
                NetParam.Host = true;
                NetworkStream stream = NetParam.client.GetStream();
                byte[] numArray = new byte[17];
                int count;
                while ((uint)(count = stream.Read(numArray, 0, numArray.Length)) > 0U)
                    Console.WriteLine(string.Format("受信: {0}", Encoding.UTF8.GetString(numArray, 0, count)));
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (NetParam.server != null)
                    NetParam.server.Stop();
            }
        }

        public static void Client()
        {
            try
            {
                int defaultPort = NetParam.DEFAULT_PORT;
                NetParam.client = new TcpClient(NetParam.GetIPAddress(NetParam.ConnectIP), defaultPort);
                NetParam.Init();
                NetParam.connecting = true;
                NetParam.Host = false;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("ArgumentNullException: {0}", ex);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SocketException: {0}", ex);
            }
        }

        public static void StartUDP()
        {
            IPAddress.Parse(NetParam.enemyIP);
            IPAddress.Parse("239.255.255.255");
            NetParam.localEP = new IPEndPoint(IPAddress.Any, NetParam.UDP_MY_PORT);
            NetParam.udpSender = new UdpClient();
            NetParam.udpReceiver = new UdpClient(NetParam.localEP);
        }

        public static async void SendData()
        {
            NetworkStream stream = NetParam.client.GetStream();
            StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8);
            while (NetParam.connecting)
            {
                if (NetParam.sendLog.Count > 0)
                {
                    try
                    {
                        string send = "";
                        for (int i = 0; i < NetParam.sendLog.Count; ++i)
                        {
                            send += NetParam.sendLog[i];
                            if (i < NetParam.sendLog.Count - 1)
                                send += "^";
                            Debug.DebugMess("送信：" + NetParam.sendLog[i]);
                            NetParam.sendLog[i] = "null";
                        }
                        if (NetParam.udpReceiver != null)
                        {
                            byte[] sendBytes = Encoding.UTF8.GetBytes(send);
                            MemoryStream ms = new MemoryStream();
                            DeflateStream CompressedStream = new DeflateStream(ms, CompressionMode.Compress, true);
                            CompressedStream.Write(sendBytes, 0, sendBytes.Length);
                            CompressedStream.Close();
                            byte[] destination = ms.ToArray();
                            NetParam.udpSender.Send(destination, destination.Length, NetParam.enemyIP, NetParam.UDP_YOUR_PORT);
                            Debug.DebugMess("◆UDP送信◆");
                            sendBytes = null;
                            ms = null;
                            CompressedStream = null;
                            destination = null;
                        }
                        else
                            streamWriter.WriteLine(send);
                        NetParam.sendLog.RemoveAll(s => s == "null");
                        await Task.Delay(1).ConfigureAwait(false);
                        send = null;
                    }
                    catch
                    {
                        NetParam.ConnectOut();
                        break;
                    }
                }
            }
        }

        public static async void ReadData()
        {
            NetworkStream stream = NetParam.client.GetStream();
            StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
            while (NetParam.connecting)
            {
                try
                {
                    ++NetParam.connectTime;
                    byte[] bytes = new byte[1000];
                    string data;
                    if (NetParam.udpReceiver != null)
                    {
                        IPEndPoint remoteEP = null;
                        if (!NetParam.connecting)
                            break;
                        byte[] rcvBytes = NetParam.udpReceiver.Receive(ref remoteEP);
                        MemoryStream ms = new MemoryStream(rcvBytes);
                        MemoryStream ms2 = new MemoryStream();
                        DeflateStream CompressedStream = new DeflateStream(ms, CompressionMode.Decompress);
                        while (true)
                        {
                            int rb = CompressedStream.ReadByte();
                            if (rb != -1)
                                ms2.WriteByte((byte)rb);
                            else
                                break;
                        }
                        data = Encoding.UTF8.GetString(ms2.ToArray());
                        Debug.DebugMess("◆UDP受信◆");
                        remoteEP = null;
                        rcvBytes = null;
                        ms = null;
                        ms2 = null;
                        CompressedStream = null;
                    }
                    else
                        data = streamReader.ReadLine();
                    if (data == null)
                    {
                        Debug.DebugMess("◆◆◆◆◆パケットロス発生◆◆◆◆");
                    }
                    else
                    {
                        string[] datas = data.Split('^');
                        string[] strArray = datas;
                        for (int index = 0; index < strArray.Length; ++index)
                        {
                            string s = strArray[index];
                            string[] ss = s.Split('@');
                            if (ss[ss.Length - 1] == "SHEX")
                            {
                                NetParam.catchLog.Add(s);
                                Debug.DebugMess("受信：" + s);
                            }
                            else
                                Debug.DebugMess("◆◆◆◆◆パケットロス発生◆◆◆◆");
                            ss = null;
                            s = null;
                        }
                        strArray = null;
                        datas = null;
                    }
                    data = null;
                    NetParam.catchLog.RemoveAll(s => s == "null");
                    NetParam.catchLog.RemoveAll(s => !s.Contains("SHEX"));
                    try
                    {
                        NetParam.catchLog.RemoveAll(s => int.Parse(s.Split('@')[2]) < NetParam.connectTime - 200L);
                    }
                    catch
                    {
                    }
                    Debug.DebugMess("-------");
                    await Task.Delay(1).ConfigureAwait(false);
                    bytes = null;
                }
                catch
                {
                    NetParam.ConnectOut();
                    break;
                }
            }
        }

        public static string[] DataUse(string number)
        {
            for (int index = 0; index < NetParam.catchLog.Count; ++index)
            {
                string[] strArray = NetParam.catchLog[index].Split('@');
                if (strArray[0] == "SHEX" && strArray[1] == number)
                {
                    NetParam.catchLog[index] = "null";
                    Debug.DebugMess("☆☆データ利用☆☆" + number.ToString());
                    return strArray;
                }
            }
            return null;
        }

        public static string[] DataUse(int number)
        {
            return NetParam.DataUse(number.ToString());
        }

        public static void SendingData(int type, string data)
        {
            NetParam.sendLog.Add("SHEX@" + type.ToString() + "@" + NetParam.connectTime.ToString() + "@" + data + "@SHEX");
        }

        public static void ConnectOut()
        {
            try
            {
                Debug.DebugMess("◆◆◆◆◆断線◆◆◆◆");
                NetParam.client.Close();
            }
            catch
            {
            }
        }

        public static void Close()
        {
            if (NetParam.server != null)
                NetParam.server.Stop();
            if (NetParam.client != null)
                NetParam.client.Close();
            if (NetParam.udpReceiver != null)
                NetParam.udpReceiver.Close();
            if (NetParam.udpSender != null)
                NetParam.udpSender.Close();
            NetParam.server = null;
            NetParam.client = null;
            NetParam.udpReceiver = null;
            NetParam.udpSender = null;
            NetParam.connecting = false;
            NetParam.connectWait = false;
        }

        public static void NextChipSend(ChipBase chip)
        {
            if (chip is DammyChip || chip == null)
            {
                NetParam.nextChipID = -1;
                NetParam.nextChipPP = 0;
                NetParam.nextChipTS = false;
                NetParam.nextChipPL = false;
                NetParam.nextChipRS = new int[1];
            }
            else
            {
                NetParam.nextChipID = chip.number;
                NetParam.nextChipPP = chip.pluspower;
                NetParam.nextChipTS = chip.timeStopper;
                NetParam.nextChipPL = chip.paralyze;
                NetParam.nextChipRS = chip.randomSeed;
            }
        }

        public static void PosiSave(int x, int y)
        {
            while (NetParam.positionX.Count < 10)
                NetParam.positionX.Add(x);
            while (NetParam.positionY.Count < 10)
                NetParam.positionY.Add(x);
            NetParam.positionX.Insert(0, x);
            NetParam.positionY.Insert(0, y);
            if (NetParam.positionX.Count >= 10)
                NetParam.positionX.RemoveAt(NetParam.positionX.Count - 1);
            if (NetParam.positionY.Count < 10)
                return;
            NetParam.positionY.RemoveAt(NetParam.positionY.Count - 1);
        }

        public static ChipBase NextChipMake(IAudioEngine s)
        {
            ChipFolder chipFolder = new ChipFolder(s);
            chipFolder.SettingChip(NetParam.ENnextChipID);
            chipFolder.chip.pluspower = NetParam.ENnextChipPP;
            chipFolder.chip.timeStopper = NetParam.ENnextChipTS;
            chipFolder.chip.paralyze = NetParam.ENnextChipPL;
            chipFolder.chip.randomSeed = NetParam.ENnextChipRS;
            return chipFolder.chip;
        }

        public static void SendInput(int flame)
        {
            NetParam.logFlame = flame;
            ++NetParam.sendflame;
            string str = "";
            for (int index = 0; index < flame; ++index)
                str = str + string.Join<bool>(",", Input.inputRecord[index]) + "@";
            NetParam.SendingData(400, str.Replace("True", "1").Replace("False", "0") + NetParam.nextChipID.ToString() + "@" + NetParam.nextChipPP.ToString() + "@" + NetParam.nextChipTS.ToString() + "@" + NetParam.nextChipPL.ToString() + "@" + string.Join<int>(",", nextChipRS) + "@" + string.Join<int>(",", NetParam.positionX.ToArray()) + "@" + string.Join<int>(",", NetParam.positionY.ToArray()) + "@" + string.Join<int>(",", ((IEnumerable<int>)NetParam.panelColor).ToArray<int>()) + "@" + string.Join<int>(",", ((IEnumerable<int>)NetParam.panelElem).ToArray<int>()) + "@" + NetParam.sendflame.ToString());
        }

        public static void PanelParamSet(Panel[,] panels)
        {
            NetParam.panelColor = new int[9];
            NetParam.panelElem = new int[9];
            int index1 = 0;
            for (int index2 = 0; index2 < panels.GetLength(0) / 2; ++index2)
            {
                for (int index3 = 0; index3 < panels.GetLength(1); ++index3)
                {
                    Panel panel = panels[index2, index3];
                    NetParam.panelColor[index1] = (int)panel.color;
                    NetParam.panelElem[index1] = (int)panel.state;
                    ++index1;
                }
            }
        }

        public static void ClearData(int num)
        {
            for (int index = 0; index < NetParam.catchLog.Count; ++index)
            {
                string[] strArray = NetParam.catchLog[index].Split('@');
                if (strArray.Length >= 2 && int.Parse(strArray[1]) == num)
                    NetParam.catchLog[index] = "null";
            }
        }

        public static bool GetInput()
        {
            List<bool[]> flagArrayList = new List<bool[]>();
            string[] strArray = NetParam.DataUse("400");
            if (strArray == null)
                return false;
            try
            {
                for (int index = 3; index < 3 + NetParam.logFlame; ++index)
                {
                    strArray[index] = strArray[index].Replace("0", "False");
                    strArray[index] = strArray[index].Replace("1", "True");
                    bool[] array = ((IEnumerable<string>)strArray[index].Split(',')).Select<string, bool>(new Func<string, bool>(bool.Parse)).ToArray<bool>();
                    flagArrayList.Add(array);
                }
                Input.inputRecordEnemy = flagArrayList;
                NetParam.ENnextChipID = int.Parse(strArray[NetParam.logFlame + 3]);
                NetParam.ENnextChipPP = int.Parse(strArray[NetParam.logFlame + 4]);
                NetParam.ENnextChipTS = bool.Parse(strArray[NetParam.logFlame + 5]);
                NetParam.ENnextChipPL = bool.Parse(strArray[NetParam.logFlame + 6]);
                NetParam.ENnextChipRS = ((IEnumerable<string>)strArray[NetParam.logFlame + 7].Split(',')).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>();
                NetParam.ENpositionX = ((IEnumerable<string>)strArray[NetParam.logFlame + 8].Split(',')).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>();
                NetParam.ENpositionY = ((IEnumerable<string>)strArray[NetParam.logFlame + 9].Split(',')).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>();
                NetParam.ENpanelColor = ((IEnumerable<string>)strArray[NetParam.logFlame + 10].Split(',')).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>();
                NetParam.ENpanelElem = ((IEnumerable<string>)strArray[NetParam.logFlame + 11].Split(',')).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>();
                NetParam.readDeray = (int)(NetParam.sendflame - long.Parse(strArray[strArray.Length - 2]));
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
