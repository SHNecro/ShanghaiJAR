using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap.Character;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace NSEvent
{
    public class Chiptreader : EventBase
    {
        private readonly List<ChipS> setchips = new List<ChipS>();
        private List<ChipS> getchips = new List<ChipS>();
        private List<ChipS> havechips = new List<ChipS>();
        private readonly List<ChipFolder> rendChip = new List<ChipFolder>();
        private readonly Chiptreader.Treader treader;
        private int seetX;
        private bool shopmode;
        private Chiptreader.SCENE nowscene;
        private EventManager eventmanager;
        private byte alpha;
        protected const string texture = "menuwindows";
        private int cursol;
        private byte[,] havechip;
        private int cursolanime;
        private int waittime;
        private const byte waitlong = 10;
        private const byte waitshort = 4;
        private readonly InfoMessage info;
        private int overtop;
        private int topchip;
        private bool startflag;
        private bool lost;
        private bool getend;
        private Thread thread_1;

        private int MaxSet
        {
            get
            {
                switch (this.treader)
                {
                    case Chiptreader.Treader.Normal:
                        return 3;
                    case Chiptreader.Treader.Special:
                        return 10;
                    default:
                        return 0;
                }
            }
        }

        private int Select
        {
            get
            {
                return this.cursol + this.topchip;
            }
        }

        private int Topchip
        {
            get
            {
                return this.topchip;
            }
            set
            {
                this.topchip = value;
                this.overtop = this.havechips.Count < 7 ? 0 : this.havechips.Count - 7;
                if (this.topchip >= this.overtop)
                {
                    this.topchip = this.overtop;
                    this.cursol = 6;
                }
                if (this.topchip >= 0)
                    return;
                this.topchip = 0;
            }
        }

        public Chiptreader(IAudioEngine s, EventManager m, Player player, int type, SaveData save)
          : base(s, m, save)
        {
            this.treader = (Chiptreader.Treader)type;
            this.info = player.info;
            this.NoTimeNext = false;
            this.Init();
        }

        private void Init()
        {
            this.lost = false;
            this.getend = false;
            this.thread_1 = new Thread(new ThreadStart(this.GetChip));
            this.nowscene = Chiptreader.SCENE.fadein;
            this.eventmanager = new EventManager(this.sound);
            this.havechip = new byte[450, 4];
            for (int index1 = 0; index1 < 450; ++index1)
            {
                for (int index2 = 0; index2 < 4; ++index2)
                    this.havechip[index1, index2] = this.savedata.Havechip[index1, index2];
            }
            this.havechips.Clear();
            foreach (ChipS havechip in this.savedata.havechips)
                this.havechips.Add(havechip);
            this.havechips = this.havechips.OrderByDescending<ChipS, int>(n => n.Code).ToList<ChipS>();
            this.havechips = this.havechips.OrderByDescending<ChipS, byte>(n => this.havechip[n.number, n.code]).ToList<ChipS>();
            this.setchips.Clear();
            this.getchips.Clear();
            this.rendChip.Clear();
        }

        public override void Update()
        {
            switch (this.nowscene)
            {
                case Chiptreader.SCENE.fadein:
                    if (!this.shopmode)
                    {
                        if (this.alpha == 0)
                            this.Init();
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
                        this.nowscene = Chiptreader.SCENE.select;
                    }
                    break;
                case Chiptreader.SCENE.select:
                    if (this.eventmanager.playevent)
                    {
                        this.eventmanager.UpDate();
                        break;
                    }
                    if (this.startflag)
                    {
                        if (this.savedata.selectQuestion == 0)
                        {
                            this.thread_1.Start();
                            this.nowscene = Chiptreader.SCENE.fadeout;
                        }
                        else
                        {
                            if (this.setchips.Count / this.MaxSet >= 10)
                            {
                                this.AddChip(this.setchips[this.setchips.Count - 1].number, this.setchips[this.setchips.Count - 1].code, false);
                                this.setchips.RemoveAt(this.setchips.Count - 1);
                            }
                            this.startflag = false;
                        }
                    }
                    else
                        this.Control();
                    break;
                case Chiptreader.SCENE.fadeout:
                    if (this.shopmode)
                    {
                        if (this.alpha >= byte.MaxValue)
                        {
                            this.alpha = byte.MaxValue;
                            if (this.getend || !this.startflag)
                            {
                                this.savedata.ValList[15] = -1;
                                this.shopmode = false;
                                break;
                            }
                            break;
                        }
                        this.alpha += 51;
                        break;
                    }
                    this.alpha -= 51;
                    if (this.alpha <= 0)
                    {
                        this.nowscene = Chiptreader.SCENE.fadein;
                        this.cursol = 0;
                        this.topchip = 0;
                        if (this.startflag)
                        {
                            this.EventMake();
                            this.nowscene = Chiptreader.SCENE.getting;
                        }
                        else
                        {
                            this.Init();
                            this.EndCommand();
                        }
                    }
                    break;
                case Chiptreader.SCENE.getting:
                    if (this.eventmanager.playevent)
                    {
                        this.eventmanager.UpDate();
                        break;
                    }
                    this.Init();
                    this.EndCommand();
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
                this.Decide();
            else if (Input.IsPress(Button._B))
            {
                if (this.setchips.Count > 0)
                {
                    this.AddChip(this.setchips[this.setchips.Count - 1].number, this.setchips[this.setchips.Count - 1].code, false);
                    this.setchips.RemoveAt(this.setchips.Count - 1);
                }
                else
                    this.nowscene = Chiptreader.SCENE.fadeout;
                this.sound.PlaySE(SoundEffect.cancel);
            }
            else if (Input.IsPress(Button._Start))
            {
                if (this.setchips.Count % this.MaxSet == 0 && this.setchips.Count > 0)
                    this.Start();
                else
                    this.sound.PlaySE(SoundEffect.error);
            }
            else if (this.waittime <= 0)
            {
                if (Input.IsPush(Button.Up))
                {
                    if (this.Select > 0)
                    {
                        if (this.cursol > 0)
                            --this.cursol;
                        else
                            --this.Topchip;
                        this.sound.PlaySE(SoundEffect.movecursol);
                    }
                    this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                }
                else if (Input.IsPush(Button.Down))
                {
                    if (this.Select < this.havechips.Count - 1)
                    {
                        if (this.cursol < 6)
                            ++this.cursol;
                        else
                            ++this.Topchip;
                        this.sound.PlaySE(SoundEffect.movecursol);
                    }
                    this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                }
                else if (Input.IsPush(Button._R))
                {
                    this.overtop = this.havechips.Count < 7 ? 0 : this.havechips.Count - 7;
                    int num = this.overtop - this.Topchip;
                    if (num > 7)
                        num = 7;
                    if (num > 0)
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.Topchip += num;
                    }
                    this.waittime = Input.IsPress(Button._R) ? 10 : 4;
                }
                else
                {
                    if (!Input.IsPush(Button._L))
                        return;
                    this.overtop = this.havechips.Count < 7 ? 0 : this.havechips.Count - 7;
                    int num = this.Topchip;
                    if (num > 7)
                        num = 7;
                    if (num > 0)
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.Topchip -= num;
                    }
                    this.waittime = Input.IsPress(Button._L) ? 10 : 4;
                }
            }
            else
                --this.waittime;
        }

        private void Decide()
        {
            if (this.havechips.Count <= 0)
                return;
            if (this.havechips[this.Select].number < byte.MaxValue || this.havechips[this.Select].number >= 350)
            {
                this.setchips.Add(this.havechips[this.Select]);
                this.LosChip(this.havechips[this.Select].number, this.havechips[this.Select].code);
                if (this.Select >= this.havechips.Count && this.Select > 0)
                {
                    if (this.cursol > 0 && this.havechips.Count >= this.cursol && this.Topchip == 0)
                        --this.cursol;
                    --this.Topchip;
                }
                if (this.setchips.Count % this.MaxSet == 0)
                {
                    this.sound.PlaySE(SoundEffect.docking);
                    if (this.setchips.Count / this.MaxSet >= 10)
                        this.Start();
                }
                else
                    this.sound.PlaySE(SoundEffect.decide);
            }
            else
            {
                this.sound.PlaySE(SoundEffect.error);
                this.eventmanager.events.Clear();
                this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
                var dialogue = ShanghaiEXE.Translate("ChipTrader.RareChipStopDialogue1");
                this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
                this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
            }
        }

        private void Start()
        {
            this.startflag = true;
            this.eventmanager.events.Clear();
            this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
            var question = ShanghaiEXE.Translate("ChipTrader.ConfirmQuestion");
            var options = ShanghaiEXE.Translate("ChipTrader.ConfirmOptions");
            this.eventmanager.AddEvent(new Question(this.sound, this.eventmanager, question, options[0], options[1], true, true, true, question.Face, this.savedata, true));
            this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
        }

        private void GetChip()
        {
            if (this.lost)
                return;
            foreach (ChipS setchip in this.setchips)
                this.savedata.LosChip(setchip.number, setchip.code);
            this.getchips = this.GetListMake(this.setchips.Count / this.MaxSet);
            foreach (ChipS getchip in this.getchips)
            {
                this.savedata.AddChip(getchip.number, getchip.code, true);
                ChipFolder chipFolder = new ChipFolder(this.sound);
                chipFolder.SettingChip(getchip.number);
                chipFolder.codeNo = getchip.code;
                this.rendChip.Add(chipFolder);
            }
            this.lost = true;
            this.getend = true;
        }

        private void EventMake()
        {
            this.eventmanager.parent = this.manager.parent;
            this.eventmanager.events.Clear();
            this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
            var dialogue = ShanghaiEXE.Translate("ChipTrader.TradeDialogue1");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, this.savedata));
            this.eventmanager.AddEvent(new SEmon(this.sound, this.eventmanager, "getchip", 0, this.savedata));
            dialogue = ShanghaiEXE.Translate("ChipTrader.TradeDialogue2");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, this.savedata));
            this.eventmanager.AddEvent(new SEmon(this.sound, this.eventmanager, "treader", 0, this.savedata));
            dialogue = ShanghaiEXE.Translate("ChipTrader.TradeDialogue3");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, this.savedata));
            this.eventmanager.AddEvent(new EditValue(this.sound, this.eventmanager, 15, false, 0, 0, "0", this.manager.parent.Player, this.savedata));
            foreach (ChipS getchip in this.getchips)
            {
                ChipFolder chipFolder = new ChipFolder(this.sound);
                chipFolder.SettingChip(getchip.number);
                chipFolder.codeNo = getchip.code;
                string chipCode = chipFolder.chip.code[chipFolder.codeNo] == ChipFolder.CODE.asterisk ? "＊" : chipFolder.chip.code[chipFolder.codeNo].ToString();
                this.eventmanager.AddEvent(new EditValue(this.sound, this.eventmanager, 21, false, 0, 0, "-96", this.manager.parent.Player, this.savedata));
                this.eventmanager.AddEvent(new SEmon(this.sound, this.eventmanager, "get", 0, this.savedata));
                this.eventmanager.AddEvent(new EditValue(this.sound, this.eventmanager, 0, false, 0, 5, "0", this.manager.parent.Player, this.savedata));
                this.eventmanager.AddEvent(new EditValue(this.sound, this.eventmanager, 1, false, 0, 5, "1", this.manager.parent.Player, this.savedata));
                this.eventmanager.AddEvent(new EditValue(this.sound, this.eventmanager, 2, false, 0, 5, "2", this.manager.parent.Player, this.savedata));
                this.eventmanager.AddEvent(new EffectMake(this.sound, this.eventmanager, 4, "get", 0, 1, 2, 1, -1, 0, 1, "none", this.manager.parent, this.manager.parent.Field, null, this.savedata));
                this.eventmanager.AddEvent(new PlayerHide(this.sound, this.eventmanager, true, this.manager.parent.Player, this.savedata));
                string receiver = this.savedata.isJackedIn ? ShanghaiEXE.Translate("ChipTrader.ResultRecipientShanghai") : ShanghaiEXE.Translate("ChipTrader.ResultRecipientAlice");
                dialogue = ShanghaiEXE.Translate("ChipTrader.ResultDialogue1Format").Format(receiver, chipFolder.chip.name, chipCode);
                this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], true, this.savedata));
                this.eventmanager.AddEvent(new PlayerHide(this.sound, this.eventmanager, false, this.manager.parent.Player, this.savedata));
                this.eventmanager.AddEvent(new EffectEnd(this.sound, this.eventmanager, this.manager.parent.Field, this.savedata));
                this.eventmanager.AddEvent(new EditValue(this.sound, this.eventmanager, 15, false, 1, 0, "1", this.manager.parent.Player, this.savedata));
            }
            this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
        }

        public override void Render(IRenderer dg)
        {
            if (this.shopmode && this.nowscene != Chiptreader.SCENE.getting)
            {
                this._rect = new Rectangle(480, 784, 240, 160);
                this._position = new Vector2(0.0f, 0.0f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                AllBase.NAME[] nameArray = this.Nametodata(ShanghaiEXE.Translate("ChipTrader.NameToData"));
                this._position = new Vector2(24, 8f);
                DrawBlockCharacters(dg, nameArray, 88, this._position, Color.White, out this._rect, out this._position);
                string[] strArray = new string[6];
                strArray[0] = (this.setchips.Count % this.MaxSet).ToString();
                strArray[1] = "/";
                strArray[2] = this.MaxSet.ToString();
                strArray[3] = ShanghaiEXE.Translate("ChipTrader.ChipNumberPrefix");
                strArray[4] = (this.setchips.Count / this.MaxSet).ToString();
                strArray[5] = ShanghaiEXE.Translate("ChipTrader.ChipNumberSuffix");
                var name = string.Concat(strArray);
                var nameBlockCharacters = this.Nametodata(name);
                this._position = new Vector2(160, 8f);
                DrawBlockCharacters(dg, nameBlockCharacters, 88, this._position, Color.White, out this._rect, out this._position);
                float num1 = this.overtop != 0 && this.Topchip != 0 ? 104f / overtop * Topchip : 0.0f;
                this._rect = new Rectangle(176, 168, 8, 8);
                this._position = new Vector2(144f, 32f + num1);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                if (this.havechips.Count > 0)
                {
                    for (int index1 = 0; index1 < 7 && this.Topchip + index1 < this.havechips.Count; ++index1)
                    {
                        ChipFolder chipFolder = new ChipFolder(this.sound);
                        chipFolder.SettingChip(this.havechips[this.Topchip + index1].number);
                        chipFolder.codeNo = this.havechips[this.Topchip + index1].code;
                        if (!(chipFolder.chip is DammyChip))
                        {
                            this._position = new Vector2(8f, 32 + 16 * index1);
                            chipFolder.chip.IconRender(dg, this._position, false, false, chipFolder.codeNo, false);
                            var chipNameBlockCharacters = this.Nametodata(chipFolder.chip.name);
                            this._position = new Vector2(24, 32 + 16 * index1);
                            DrawBlockCharacters(dg, chipNameBlockCharacters, 16, this._position, Color.White, out this._rect, out this._position);
                            this._rect = new Rectangle(216 + (int)chipFolder.chip.element * 16, 88, 16, 16);
                            this._position = new Vector2(88f, 32 + index1 * 16);
                            dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                            this._rect = new Rectangle((int)chipFolder.chip.code[chipFolder.codeNo] * 8, 32, 8, 16);
                            this._position = new Vector2(104f, 32 + index1 * 16);
                            dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                            this._rect = new Rectangle(160, 160, 16, 16);
                            this._position = new Vector2(112f, 32 + index1 * 16);
                            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                            int[] numArray1 = this.ChangeCount(chipFolder.chip.regsize);
                            PointF pointF = new PointF(120f, 32 + index1 * 16);
                            for (int index2 = 0; index2 < numArray1.Length; ++index2)
                            {
                                this._rect = new Rectangle(numArray1[index2] * 8, 72, 8, 16);
                                this._position = new Vector2(pointF.X - index2 * 8, pointF.Y);
                                dg.DrawImage(dg, "font", this._rect, true, this._position, Color.Yellow);
                            }
                            int[] numArray2 = this.ChangeCount(this.havechip[chipFolder.chip.number, chipFolder.codeNo]);
                            pointF = new PointF(136f, 32 + index1 * 16);
                            for (int index2 = 0; index2 < numArray2.Length; ++index2)
                            {
                                this._rect = new Rectangle(numArray2[index2] * 8, 0, 8, 16);
                                this._position = new Vector2(pointF.X - index2 * 8, pointF.Y);
                                dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                            }
                        }
                    }
                }
                int num2 = Math.Max(0, this.setchips.Count - 7);
                for (int index = 0; index < Math.Min(this.setchips.Count, 7); ++index)
                {
                    ChipFolder chipFolder = new ChipFolder(this.sound);
                    chipFolder.SettingChip(this.setchips[num2 + index].number);
                    chipFolder.codeNo = this.setchips[num2 + index].code;
                    var chipNameBlockCharacters = this.Nametodata(chipFolder.chip.name);
                    this._position = new Vector2(152, 32 + 16 * index);
                    DrawBlockCharacters(dg, chipNameBlockCharacters, 16, this._position, Color.White, out this._rect, out this._position);
                    this._rect = new Rectangle((int)chipFolder.chip.code[chipFolder.codeNo] * 8, 32, 8, 16);
                    this._position = new Vector2(224f, 32 + index * 16);
                    dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                }
                this._rect = new Rectangle(112 + 16 * this.cursolanime, 160, 16, 16);
                this._position = new Vector2(0.0f, 32 + this.cursol * 16);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            if (this.eventmanager.playevent)
                this.eventmanager.Render(dg);
            if (this.nowscene == Chiptreader.SCENE.getting && this.savedata.ValList[15] >= 0 && this.savedata.ValList[15] < this.rendChip.Count)
            {
                this.seetX = this.savedata.ValList[21];
                if (this.seetX < 0)
                {
                    this.savedata.ValList[21] += 16;
                    if (this.seetX > 0)
                        this.savedata.ValList[21] = 0;
                }
                this._rect = new Rectangle(760, 256, 88, 136);
                this._position = new Vector2(this.seetX + 8, 16f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                ChipFolder chipFolder = this.rendChip[this.savedata.ValList[15]];
                Vector2 p = new Vector2(this.seetX + 24, 32f);
                chipFolder.chip.GraphicsRender(dg, p, chipFolder.codeNo, true, true);
                if (chipFolder.chip.dark)
                {
                    this._rect = new Rectangle(272, 184, 88, 56);
                    this._position = new Vector2(this.seetX + 8, 96f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                }
                else if (chipFolder.chip.navi)
                {
                    this._rect = new Rectangle(272, 128, 88, 56);
                    this._position = new Vector2(this.seetX + 8, 96f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                }
                foreach (var data in ((IEnumerable<string>)chipFolder.chip.information).Select((v, i) => new
                {
                    v,
                    i
                }))
                {
                    string v = data.v;
                    Vector2 point = new Vector2(this.seetX + 10, 102 + 16 * data.i);
                    dg.DrawMiniText(v, point, Color.Black);
                }
            }
            if (this.alpha <= 0)
                return;
            Color color = Color.FromArgb(alpha, Color.Black);
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color);
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

        public void AddChip(int chipno, int chipcode, bool head)
        {
            chipcode = this.CodeCheck(chipno, chipcode);
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

        public int ListCheckNumber(int chipno, int chipcode)
        {
            for (int index = 0; index < this.havechips.Count; ++index)
            {
                if (this.havechips[index].number == chipno && this.havechips[index].code == chipcode)
                    return index;
            }
            return -1;
        }

        private List<ChipS> GetListMake(int many)
        {
            List<ChipS> chipSList1 = new List<ChipS>();
            List<ChipS> chipSList2 = new List<ChipS>();
            List<ChipS> chipSList3 = new List<ChipS>();
            for (int index = 1; index < byte.MaxValue; ++index)
            {
                ChipFolder chipFolder = new ChipFolder(this.sound);
                chipFolder.SettingChip(index);
                if (!(chipFolder.chip is DammyChip))
                {
                    if (this.savedata.datelist[index - 1])
                        chipSList2.Add(new ChipS(index, 0));
                    else
                        chipSList3.Add(new ChipS(index, 0));
                }
            }
            for (int index = 0; index < many; ++index)
            {
                int num1 = this.Random.Next(1000) / 10;
                int num2 = this.treader != Chiptreader.Treader.Normal ? (num1 >= 40 ? (num1 >= 70 ? (num1 >= 90 ? (num1 >= 99 ? 5 : 4) : 3) : 2) : 1) : (num1 >= 60 ? (num1 >= 90 ? 3 : 2) : 1);
                List<ChipS> chipSList4 = new List<ChipS>();
                List<ChipS> chipSList5 = new List<ChipS>();
                foreach (ChipS chipS in chipSList3)
                {
                    if (num2 == chipS.Reality)
                        chipSList4.Add(chipS);
                }
                foreach (ChipS chipS in chipSList2)
                {
                    if (num2 == chipS.Reality)
                        chipSList5.Add(chipS);
                }
                ChipS chipS1 = (this.Random.Next(100) >= 25 || chipSList4.Count <= 0) && chipSList5.Count > 0 ? chipSList5[this.Random.Next(chipSList5.Count)] : chipSList4[this.Random.Next(chipSList4.Count)];
                chipS1.code = this.Random.Next(4);
                chipSList1.Add(chipS1);
            }
            return chipSList1;
        }

        private enum Treader
        {
            Normal,
            Special,
        }

        private enum SCENE
        {
            fadein,
            select,
            fadeout,
            getting,
        }
    }
}
