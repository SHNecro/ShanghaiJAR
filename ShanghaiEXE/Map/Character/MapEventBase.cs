using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSEvent;
using NSGame;
using NSMap.Character.Terms;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Common.EncodeDecode;
using Data;
using System.Globalization;

namespace NSMap.Character
{
    public class MapEventBase : MapCharacterBase
    {
        private readonly int[,] floatNumber = {{2,6},{7,0},{7,1},{7,2},{7,3},{7,4},{7,7},{8,0},{8,6},{10,0},{11,5},{13,0},{13,1},{13,2},{13,5},{13,7},{17,4}};
        private readonly int[,] NoShadowNumber = {{8,6},{10,1},{10,2},{10,3},{10,4},{10,5},{10,6},{10,7},{13,1},{13,2},{13,4}};
        public EventPage[] eventPages;
        public string ID;
        protected SaveData savedate;
        public int lunPage;
        public bool stop;

        private EventPage defaultEventPage;

        public virtual EventPage LunPage
        {
            get
            {
                if (this.lunPage >= 0)
                    return this.eventPages[this.lunPage];
                return (this.defaultEventPage ?? (this.defaultEventPage = new EventPage(this.sound, this, this.savedate)));
            }
        }

        public override MapCharacterBase.ANGLE Angle
        {
            get
            {
                return this.LunPage.angle;
            }
            set
            {
                this.LunPage.angle = value;
                if (this.LunPage.angle < MapCharacterBase.ANGLE.DOWN)
                    this.LunPage.angle += 8;
                if (this.angle <= MapCharacterBase.ANGLE.DOWNLEFT)
                    return;
                this.LunPage.angle = MapCharacterBase.ANGLE.DOWN;
            }
        }

        public MapEventBase(
          IAudioEngine s,
          SceneMap p,
          Point po,
          int floor,
          MapCharacterBase.ANGLE a,
          MapField fi,
          string id,
          SaveData save,
          StreamReader reader,
          string mapname = "")
          : base(s, p, po, floor, a, fi)
        {
            this.position.Z = floor * (this.field.Height / 2);
            this.ID = id;
            this.savedate = save;
            if (reader != null)
                this.PagedateInput(reader, mapname);
            this.LunPageCheck();
        }

        public MapEventBase(
          IAudioEngine s,
          SceneMap p,
          Point po,
          int floor,
          MapCharacterBase.ANGLE a,
          MapField fi,
          string id,
          SaveData save,
          string mapname = "")
          : base(s, p, po, floor, a, fi)
        {
            this.position.Z = floor * (this.field.Height / 2);
            this.ID = id;
            this.savedate = save;
            this.LunPageCheck();
        }

        public override void Update()
        {
            this.LunPage.Update();
            this.rendType = this.LunPage.rendType;
        }

        public void LunPageCheck()
        {
            this.lunPage = -1;
            if (this.eventPages == null)
                return;
            for (int index = 0; index < this.eventPages.Length; ++index)
            {
                if (this.eventPages[index].LunCheck())
                    this.lunPage = index;
            }
            if (this.LunPage.character)
            {
                var sheet = this.LunPage.graphicNo[1];
                var charIndex = this.LunPage.graphicNo[0];
                this.floating = CharacterInfo.IsFloatingCharacter(sheet, charIndex);
                this.noShadow = CharacterInfo.IsNoShadowCharacter(sheet, charIndex);
            }
        }

        public void PagedateInput(StreamReader reader, string mapname)
        {
            List<EventPage> eventPageList = new List<EventPage>();
            EventPage page = null;
            string A;
            while ((A = reader.ReadLine()) != null)
            {
                if (NSGame.Debug.MaskMapFile)
                    A = TCDEncodeDecode.EncMapScript(A);
                string[] strArray1 = A.Split(':');
                switch (strArray1[0])
                {
                    case "":
                        goto label_36;
                    case "event":
                        this.EventInput(reader, page, mapname);
                        break;
                    case "graphic":
                        string[] strArray2 = strArray1[2].Split(',');
                        page.graphicNo[0] = int.Parse(strArray2[0]);
                        page.graphicNo[1] = int.Parse(strArray2[1]);
                        page.graphicNo[2] = int.Parse(strArray2[2]);
                        page.graphicNo[3] = int.Parse(strArray2[3]);
                        page.graphicNo[4] = int.Parse(strArray2[4]);
                        int num = int.Parse(strArray1[1]);
                        if (num > 0)
                        {
                            page.graphicNo[0] = int.Parse(strArray2[0]);
                            page.graphicNo[1] = num;
                            page.graphicNo[2] = int.Parse(strArray2[2]);
                            page.character = true;
                            page.graphicName = "charachip" + num.ToString();
                            page.angle = (MapCharacterBase.ANGLE)page.graphicNo[2];
                            page.defaultAngle = (MapCharacterBase.ANGLE)page.graphicNo[2];
                            break;
                        }
                        page.graphicNo[0] = int.Parse(strArray2[0]);
                        page.graphicNo[1] = int.Parse(strArray2[1]);
                        page.graphicNo[2] = int.Parse(strArray2[2]);
                        page.graphicNo[3] = int.Parse(strArray2[3]);
                        page.graphicNo[4] = int.Parse(strArray2[4]);
                        page.character = false;
                        num *= -1;
                        page.graphicName = "body" + num.ToString();
                        break;
                    case "hitform":
                        page.hitform = strArray1[1] == "square";
                        break;
                    case "hitrange":
                        page.hitrange = new Point(int.Parse(strArray1[1]), int.Parse(strArray1[2]));
                        page.hitShift = new Point(int.Parse(strArray1[3]), int.Parse(strArray1[4]));
                        break;
                    case "move":
                        List<EventMove> eventMoveList = new List<EventMove>();
                        for (int index = 1; index < strArray1.Length && !(strArray1[index] == ""); ++index)
                        {
                            string[] strArray3 = strArray1[index].Split(',');
                            eventMoveList.Add(new EventMove(this.sound, (EventMove.MOVE)Enum.Parse(typeof(EventMove.MOVE), strArray3[0]), int.Parse(strArray3[1]), this));
                        }
                        page.move = (EventMove[])eventMoveList.ToArray().Clone();
                        break;
                    case "page":
                        if (page != null)
                            eventPageList.Add(page);
                        page = new EventPage(this.sound, this, this.savedate);
                        break;
                    case "speed":
                        page.speed = byte.Parse(strArray1[1]);
                        break;
                    case "startterms":
                        page.startterms = (EventPage.STARTTERMS)Enum.Parse(typeof(EventPage.STARTTERMS), strArray1[1]);
                        break;
                    case "terms":
                        string[] strArray4 = strArray1[1].Split(',');
                        List<None> noneList = new List<None>();
                        foreach (string str in strArray4)
                        {
                            char[] chArray = new char[1] { '/' };
                            string[] strArray3 = str.Split(chArray);
                            if (strArray3[0] == "none")
                                noneList.Add(new None());
                            else if (strArray3[0] == "flag")
                                noneList.Add(new Flag(int.Parse(strArray3[1])));
                            else if (strArray3[0] == "!flag")
                                noneList.Add(new FlagNot(int.Parse(strArray3[1])));
                            else if (strArray3[0] == "variable")
                                noneList.Add(new Variable(int.Parse(strArray3[1]), bool.Parse(strArray3[2]), int.Parse(strArray3[3]), int.Parse(strArray3[4])));
                            else if (strArray3[0] == "havechip")
                                noneList.Add(new Havechip(int.Parse(strArray3[1]), byte.Parse(strArray3[2])));
                        }
                        page.pageStartterms = noneList.ToArray();
                        break;
                    case "type":
                        page.rendType = int.Parse(strArray1[1]);
                        break;
                }
            }
        label_36:
            eventPageList.Add(page);
            this.eventPages = eventPageList.ToArray();
        }

        protected void EventInput(StreamReader reader, EventPage page, string mapname)
        {
            string A;
            while ((A = reader.ReadLine()) != null)
            {
                if (NSGame.Debug.MaskMapFile)
                    A = TCDEncodeDecode.EncMapScript(A);
                string[] strArray1 = A.Split(':');
                switch (strArray1[0])
                {
                    case "BranchEnd":
                        page.AddEvent(new BranchEnd(this.sound, page.eventmanager, this.savedate));
                        break;
                    case "BranchHead":
                        page.AddEvent(new BranchHead(this.sound, page.eventmanager, int.Parse(strArray1[1]), this.savedate));
                        break;
                    case "CameraDefault":
                        page.AddEvent(new DefaultCamera(this.sound, page.eventmanager, int.Parse(strArray1[1]), this.parent, this.savedate));
                        break;
                    case "EditItem":
                        page.AddEvent(new EditItem(this.sound, page.eventmanager, int.Parse(strArray1[1]), bool.Parse(strArray1[2]), this.savedate));
                        break;
                    case "EventLun":
                        page.AddEvent(new RunEvent(this.sound, page.eventmanager, strArray1[1], int.Parse(strArray1[2]), this.parent, this.field, this.savedate));
                        break;
                    case "EventLunPara":
                        page.AddEvent(new RunEventParallel(this.sound, page.eventmanager, strArray1[1], int.Parse(strArray1[2]), this.parent, this.field, this.savedate));
                        break;
                    case "Facehere":
                        page.AddEvent(new Facehere(this.sound, page.eventmanager, this.parent, this.savedate));
                        break;
                    case "Forum":
                        page.AddEvent(new BBS(this.sound, page.eventmanager, this.parent.Player, int.Parse(strArray1[1]), this.savedate));
                        break;
                    case "GetMail":
                        page.AddEvent(new GetMail(this.sound, page.eventmanager, int.Parse(strArray1[1]), bool.Parse(strArray1[2]), this.parent, this.savedate));
                        break;
                    case "Getphone":
                        page.AddEvent(new GetPhone(this.sound, page.eventmanager, this.parent, this.savedate));
                        break;
                    case "HP":
                        page.AddEvent(new HPchange(this.sound, page.eventmanager, int.Parse(strArray1[1]), bool.Parse(strArray1[2]), this.savedate));
                        break;
                    case "Interior":
                        page.AddEvent(new InteriorSet(this.sound, page.eventmanager, this.field, this.parent, this.savedate));
                        break;
                    case "ItemGet":
                        RandomMystery randomMystery = new RandomMystery()
                        {
                            itemType = int.Parse(strArray1[1]),
                            itemNumber = int.Parse(strArray1[2]),
                            itemSub = int.Parse(strArray1[3])
                        };
                        randomMystery.getInfo = MysteryItem.ItemNameGet(randomMystery.itemType, randomMystery.itemNumber, randomMystery.itemSub, ShanghaiEXE.Translate(strArray1[4]));
                        randomMystery.type = 1;
                        page.AddEvent(new MysteryItem(this.sound, page.eventmanager, this.field, randomMystery, false, this.savedate));
                        page.AddEvent(new SEmon(this.sound, page.eventmanager, "get", 0, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 0, false, 0, 5, "0", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 1, false, 0, 5, "1", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 2, false, 0, 5, "2", this.parent.Player, this.savedate));
                        page.AddEvent(new EffectMake(this.sound, page.eventmanager, 4, "get", 0, 1, 2, 1, -1, 0, 1, "none", this.parent, this.field, this, this.savedate));
                        page.AddEvent(new PlayerHide(this.sound, page.eventmanager, true, this.parent.Player, this.savedate));
                        page.AddEvent(new ifFlag(this.sound, page.eventmanager, 2, true, 999, this.savedate));
                        string str = "";
                        switch (randomMystery.itemType)
                        {
                            case 0:
                                str = ShanghaiEXE.Translate("Map.ChipGetItem");
                                break;
                            case 1:
                                str = ShanghaiEXE.Translate("Map.SubChipGetItem");
                                break;
                            case 2:
                                str = ShanghaiEXE.Translate("Map.AddonGetItem");
                                break;
                        }
                        var dialogue = ShanghaiEXE.Translate("Map.GetItemNetFormat").Format(str, randomMystery.getInfo);
                        page.AddEvent(new CommandMessage(
                            this.sound,
                            page.eventmanager,
                            dialogue[0],
                            dialogue[1],
                            dialogue[2],
                            false,
                            this.savedate));
                        Action<int> showGetMessage = (itemNo) =>
                        {
                            switch (randomMystery.itemNumber)
                            {
                                case 0:
                                    dialogue = ShanghaiEXE.Translate("Map.GetHPMemory");
                                    page.AddEvent(new CommandMessage(this.sound, page.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, this.savedate));
                                    break;
                                case 1:
                                    dialogue = ShanghaiEXE.Translate("Map.GetRegUpFormat").Format(randomMystery.itemSub);
                                    page.AddEvent(new CommandMessage(this.sound, page.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, this.savedate));
                                    break;
                                case 2:
                                    dialogue = ShanghaiEXE.Translate("Map.GetSubMemory");
                                    page.AddEvent(new CommandMessage(this.sound, page.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, this.savedate));
                                    break;
                                case 3:
                                    dialogue = ShanghaiEXE.Translate("Map.GetCorePlus");
                                    page.AddEvent(new CommandMessage(this.sound, page.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, this.savedate));
                                    break;
                                case 4:
                                    dialogue = ShanghaiEXE.Translate("Map.GetHertzUpFormat").Format(randomMystery.itemSub);
                                    page.AddEvent(new CommandMessage(this.sound, page.eventmanager, dialogue[0], dialogue[1], dialogue[2], false, this.savedate));
                                    break;
                            }
                        };
                        page.AddEvent(new ifEnd(this.sound, page.eventmanager, 999, this.savedate));
                        page.AddEvent(new ifFlag(this.sound, page.eventmanager, 2, false, 999, this.savedate));
                        dialogue = ShanghaiEXE.Translate("Map.GetItemRealFormat").Format(str, randomMystery.getInfo);
                        page.AddEvent(new CommandMessage(
                            this.sound,
                            page.eventmanager,
                            dialogue[0],
                            dialogue[1],
                            dialogue[2],
                            false,
                            this.savedate));
                        if (randomMystery.itemType == 3)
                        {
                            showGetMessage(randomMystery.itemNumber);
                        }
                        page.AddEvent(new ifEnd(this.sound, page.eventmanager, 999, this.savedate));
                        page.AddEvent(new PlayerHide(this.sound, page.eventmanager, false, this.parent.Player, this.savedate));
                        page.AddEvent(new EffectEnd(this.sound, page.eventmanager, this.field, this.savedate));
                        break;
                    case "PlugOut":
                        page.AddEvent(new PlugOut(this.sound, page.eventmanager, this.parent, this.savedate));
                        break;
                    case "PositionSet":
                        page.AddEvent(new PlayerPosition(this.sound, this.parent.eventmanager, this.savedate));
                        break;
                    case "Quest":
                        page.AddEvent(new QuestSelect(this.sound, page.eventmanager, this.parent.Player, this.savedate));
                        break;
                    case "QuestEnd":
                        page.AddEvent(new QuestEnd(this.sound, page.eventmanager, this.savedate));
                        break;
                    case "Special":
                        page.AddEvent(new Special(this.sound, page.eventmanager, int.Parse(strArray1[1]), this.savedate));
                        break;
                    case "StatusHide":
                        page.AddEvent(new StatusHide(this.sound, page.eventmanager, bool.Parse(strArray1[1]), this.parent, this.savedate));
                        break;
                    case "Virus":
                        page.AddEvent(new VirusManager(this.sound, page.eventmanager, this.parent.Player, this.savedate));
                        break;
                    case "Wanted":
                        page.AddEvent(new Wanted(this.sound, page.eventmanager, this.parent.Player, this.savedate));
                        break;
                    case "battle":
                        if (strArray1.Length < 20)
                        {
                            page.AddEvent(new NSEvent.Battle(this.sound, page.eventmanager,
                                (EnemyBase.VIRUS)Enum.Parse(typeof(EnemyBase.VIRUS), strArray1[1]),
                                byte.Parse(strArray1[2]),
                                int.Parse(strArray1[3]),
                                int.Parse(strArray1[4]),
                                (EnemyBase.VIRUS)Enum.Parse(typeof(EnemyBase.VIRUS), strArray1[5]),
                                byte.Parse(strArray1[6]),
                                int.Parse(strArray1[7]),
                                int.Parse(strArray1[8]),
                                (EnemyBase.VIRUS)Enum.Parse(typeof(EnemyBase.VIRUS), strArray1[9]),
                                byte.Parse(strArray1[10]),
                                int.Parse(strArray1[11]),
                                int.Parse(strArray1[12]),
                                (Panel.PANEL)Enum.Parse(typeof(Panel.PANEL), strArray1[13]),
                                (Panel.PANEL)Enum.Parse(typeof(Panel.PANEL), strArray1[14]),
                                int.Parse(strArray1[15]),
                                bool.Parse(strArray1[16]),
                                bool.Parse(strArray1[17]),
                                bool.Parse(strArray1[18]),
                                this.savedate));
                            break;
                        }
                        page.AddEvent(
                            new NSEvent.Battle(this.sound, page.eventmanager,
                            int.Parse(strArray1[1]),
                            byte.Parse(strArray1[2]),
                            int.Parse(strArray1[3]),
                            int.Parse(strArray1[4]),
                            int.Parse(strArray1[5]),
                            int.Parse(strArray1[6]),
                            int.Parse(strArray1[7]),
                            int.Parse(strArray1[8]),
                            ShanghaiEXE.Translate(strArray1[9]),
                            int.Parse(strArray1[10]),
                            byte.Parse(strArray1[11]),
                            int.Parse(strArray1[12]),
                            int.Parse(strArray1[13]),
                            int.Parse(strArray1[14]),
                            int.Parse(strArray1[15]),
                            int.Parse(strArray1[16]),
                            int.Parse(strArray1[17]),
                            ShanghaiEXE.Translate(strArray1[18]),
                            int.Parse(strArray1[19]),
                            byte.Parse(strArray1[20]),
                            int.Parse(strArray1[21]),
                            int.Parse(strArray1[22]),
                            int.Parse(strArray1[23]),
                            int.Parse(strArray1[24]),
                            int.Parse(strArray1[25]),
                            int.Parse(strArray1[26]),
                            ShanghaiEXE.Translate(strArray1[27]),
                            (Panel.PANEL)int.Parse(strArray1[28]),
                            (Panel.PANEL)int.Parse(strArray1[29]),
                            int.Parse(strArray1[30]),
                            bool.Parse(strArray1[31]),
                            bool.Parse(strArray1[32]),
                            bool.Parse(strArray1[33]),
                            bool.Parse(strArray1[34]),
                            strArray1[35],
                            int.Parse(strArray1[36]),
                            this.savedate));
                        break;
                    case "bgmLoad":
                        page.AddEvent(new BgmLoad(this.sound, page.eventmanager, this.field, this.savedate));
                        break;
                    case "bgmSave":
                        page.AddEvent(new BgmSave(this.sound, page.eventmanager, this.field, this.savedate));
                        break;
                    case "bgmfade":
                        page.AddEvent(new BGMFade(this.sound, page.eventmanager, int.Parse(strArray1[1]), int.Parse(strArray1[2]), bool.Parse(strArray1[3]), this.savedate));
                        break;
                    case "bgmoff":
                        page.AddEvent(new BGMoff(this.sound, page.eventmanager, 0, this.savedate));
                        break;
                    case "bgmon":
                        page.AddEvent(new BGMon(this.sound, page.eventmanager, strArray1[1], 0, this.savedate));
                        break;
                    case "camera":
                        page.AddEvent(new moveCamera(
                            this.sound,
                            page.eventmanager,
                            new Vector2(float.Parse(strArray1[1], CultureInfo.InvariantCulture),
                            float.Parse(strArray1[2], CultureInfo.InvariantCulture)),
                            int.Parse(strArray1[3]),
                            bool.Parse(strArray1[4]),
                            this.parent,
                            this.savedate));
                        break;
                    case "canSkip":
                        page.AddEvent(new CanSkip(this.sound, page.eventmanager, this.savedate));
                        break;
                    case "chipGet":
                        page.AddEvent(new ChipPlus(this.sound, page.eventmanager, int.Parse(strArray1[1]), int.Parse(strArray1[2]), bool.Parse(strArray1[3]), this.savedate));
                        break;
                    case "chiptreader":
                        int type = int.Parse(strArray1[1]);
                        if (type <= 1)
                        {
                            page.AddEvent(new OpenMassageWindow(this.sound, page.eventmanager));
                            if (type == 0)
                            {
                                dialogue = ShanghaiEXE.Translate("Map.ChipTraderQuestion");
                                var options = ShanghaiEXE.Translate("Map.ChipTraderOptions");
                                page.AddEvent(new Question(this.sound, page.eventmanager, dialogue[0], dialogue[1], options[0], options[1], false, false, dialogue.Face, this.savedate, true));
                            }
                            else
                            {
                                dialogue = ShanghaiEXE.Translate("Map.ChipTraderSpecialQuestion");
                                var options = ShanghaiEXE.Translate("Map.ChipTraderSpecialOptions");
                                page.AddEvent(new Question(this.sound, page.eventmanager, dialogue[0], dialogue[1], options[0], options[1], false, false, dialogue.Face, this.savedate, true));
                            }

                            page.AddEvent(new CloseMassageWindow(this.sound, page.eventmanager));
                            page.AddEvent(new BranchHead(this.sound, page.eventmanager, 0, this.savedate));
                            page.AddEvent(new Chiptreader(this.sound, page.eventmanager, this.parent.Player, type, this.savedate));
                            page.AddEvent(new BranchEnd(this.sound, page.eventmanager, this.savedate));
                            break;
                        }
                        break;
					case "credit":
						page.AddEvent(new Credit(this.sound, page.eventmanager, strArray1[1], new Point(int.Parse(strArray1[2]), int.Parse(strArray1[3])), bool.Parse(strArray1[4]), bool.Parse(strArray1[5]), int.Parse(strArray1[6]), int.Parse(strArray1[7]), int.Parse(strArray1[8]), this.parent, this.savedate));
						break;
					case "editFlag":
                        page.AddEvent(new editFlag(this.sound, page.eventmanager, int.Parse(strArray1[1]), bool.Parse(strArray1[2]), this.savedate));
                        break;
                    case "editMenu":
                        page.AddEvent(new editMenu(this.sound, page.eventmanager, int.Parse(strArray1[1]), bool.Parse(strArray1[2]), this.savedate));
                        break;
                    case "editValue":
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, int.Parse(strArray1[1]), bool.Parse(strArray1[2]), byte.Parse(strArray1[3]), byte.Parse(strArray1[4]), strArray1[5], this.parent.Player, this.savedate));
                        break;
                    case "effect":
                        if (int.Parse(strArray1[6]) < 3)
                        {
                            page.AddEvent(new EffectMake(this.sound, page.eventmanager, int.Parse(strArray1[1]), strArray1[2], int.Parse(strArray1[3]), int.Parse(strArray1[4]), int.Parse(strArray1[5]), int.Parse(strArray1[6]), int.Parse(strArray1[7]), int.Parse(strArray1[8]), int.Parse(strArray1[9]), strArray1[10], this.parent, this.field, this, this.savedate));
                            break;
                        }
                        page.AddEvent(new EffectMake(this.sound, page.eventmanager, int.Parse(strArray1[1]), strArray1[2], strArray1[3], int.Parse(strArray1[6]), int.Parse(strArray1[7]), int.Parse(strArray1[8]), int.Parse(strArray1[9]), strArray1[10], this.parent, this.field, this, this.savedate));
                        break;
                    case "effectDelete":
                        page.AddEvent(new EffectDelete(this.sound, page.eventmanager, strArray1[1], this.field, this.savedate));
                        break;
                    case "effectEnd":
                        page.AddEvent(new EffectEnd(this.sound, page.eventmanager, this.field, this.savedate));
                        break;
                    case "emove":
                        List<EventMove> eventMoveList = new List<EventMove>();
                        for (int index = 2; index < strArray1.Length; ++index)
                        {
                            string[] strArray2 = strArray1[index].Split(',');
                            eventMoveList.Add(new EventMove(this.sound, (EventMove.MOVE)Enum.Parse(typeof(EventMove.MOVE), strArray2[0]), int.Parse(strArray2[1]), this));
                        }
                        int result;
                        if (int.TryParse(strArray1[1], out result))
                        {
                            page.AddEvent(new NSEvent.EventMove(this.sound, page.eventmanager, int.Parse(strArray1[1]), (EventMove[])eventMoveList.ToArray().Clone(), this.parent, this.field, this.savedate));
                            break;
                        }
                        page.AddEvent(new NSEvent.EventMove(this.sound, page.eventmanager, strArray1[1], (EventMove[])eventMoveList.ToArray().Clone(), this.parent, this.field, this.savedate));
                        break;
                    case "emoveEnd":
                        page.AddEvent(new EventMoveEnd(this.sound, page.eventmanager, this.field, this.savedate));
                        break;
                    case "end":
                        return;
                    case "eventDeath":
                        page.AddEvent(new EventDelete(this.sound, page.eventmanager, this.ID, this.field, this.savedate));
                        break;
                    case "eventDelete":
                        page.AddEvent(new EventDelete(this.sound, page.eventmanager, strArray1[1], this.field, this.savedate));
                        break;
                    case "fade":
                        page.AddEvent(new Fade(this.sound, page.eventmanager, int.Parse(strArray1[1]), byte.Parse(strArray1[2]), byte.Parse(strArray1[3]), byte.Parse(strArray1[4]), byte.Parse(strArray1[5]), bool.Parse(strArray1[6]), this.savedate));
                        break;
                    case "goto":
                        page.AddEvent(new Goto(this.sound, page.eventmanager, strArray1[1], this.savedate));
                        break;
                    case "ifEnd":
                        page.AddEvent(new ifEnd(this.sound, page.eventmanager, int.Parse(strArray1[1]), this.savedate));
                        break;
                    case "ifFlag":
                        page.AddEvent(new ifFlag(this.sound, page.eventmanager, int.Parse(strArray1[1]), bool.Parse(strArray1[2]), int.Parse(strArray1[3]), this.savedate));
                        break;
                    case "ifValue":
                        page.AddEvent(new ifValue(this.sound, page.eventmanager, int.Parse(strArray1[1]), bool.Parse(strArray1[2]), int.Parse(strArray1[3]), byte.Parse(strArray1[4]), int.Parse(strArray1[5]), this.savedate));
                        break;
                    case "ifchip":
                        page.AddEvent(new ifChip(this.sound, page.eventmanager, int.Parse(strArray1[1]), int.Parse(strArray1[2]), bool.Parse(strArray1[3]), int.Parse(strArray1[4]), this.savedate));
                        break;
                    case "lavel":
                        page.AddEvent(new Lavel(this.sound, page.eventmanager, strArray1[1], this.savedate));
                        break;
                    case "mapChange":
                        page.AddEvent(new MapChange(this.sound, page.eventmanager, strArray1[1], new Point(int.Parse(strArray1[2]), int.Parse(strArray1[3])), int.Parse(strArray1[4]), (MapCharacterBase.ANGLE)int.Parse(strArray1[5]), this.savedate, this.field));
                        page.AddEvent(new RunEvent(this.sound, page.eventmanager, "BGMStart", -1, this.parent, this.field, this.savedate));
                        break;
                    case "mapWarp":
                        page.AddEvent(new PlayerHide(this.sound, page.eventmanager, true, this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 0, false, 0, 5, "0", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 1, false, 0, 5, "1", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 2, false, 0, 5, "2", this.parent.Player, this.savedate));
                        page.AddEvent(new SEmon(this.sound, page.eventmanager, "warp", 0, this.savedate));
                        page.AddEvent(new EffectMake(this.sound, page.eventmanager, 5, "player", 0, 1, 2, 1, -1, 0, 1, "none", this.parent, this.field, this, this.savedate));
                        page.AddEvent(new Wait(this.sound, page.eventmanager, 20, false, this.savedate));
                        page.AddEvent(new mapWarp(this.sound, page.eventmanager, strArray1[1], new Point(int.Parse(strArray1[2]), int.Parse(strArray1[3])), int.Parse(strArray1[4]), MapCharacterBase.ANGLE.DOWN, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 0, false, 0, 5, "0", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 1, false, 0, 5, "1", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 2, false, 0, 5, "2", this.parent.Player, this.savedate));
                        page.AddEvent(new SEmon(this.sound, page.eventmanager, "warp", 0, this.savedate));
                        page.AddEvent(new Facehere(this.sound, page.eventmanager, this.parent, this.savedate));
                        page.AddEvent(new EffectMake(this.sound, page.eventmanager, 2, "player", 0, 1, 2, 1, -1, 0, 1, "none", this.parent, this.field, this, this.savedate));
                        page.AddEvent(new Wait(this.sound, page.eventmanager, 42, false, this.savedate));
                        page.AddEvent(new PlayerHide(this.sound, page.eventmanager, false, this.parent.Player, this.savedate));
                        page.AddEvent(new Wait(this.sound, page.eventmanager, 40, false, this.savedate));
                        EventMove[] mo1 = new EventMove[1];
                        MapCharacterBase.ANGLE angle1 = (MapCharacterBase.ANGLE)Enum.Parse(typeof(MapCharacterBase.ANGLE), strArray1[5]);
                        int l1 = 16;
                        switch (angle1)
                        {
                            case MapCharacterBase.ANGLE.DOWNRIGHT:
                                mo1[0] = new EventMove(this.sound, EventMove.MOVE.right, l1, parent.Player);
                                break;
                            case MapCharacterBase.ANGLE.UPRIGHT:
                                mo1[0] = new EventMove(this.sound, EventMove.MOVE.up, l1, parent.Player);
                                break;
                            case MapCharacterBase.ANGLE.UPLEFT:
                                mo1[0] = new EventMove(this.sound, EventMove.MOVE.left, l1, parent.Player);
                                break;
                            case MapCharacterBase.ANGLE.DOWNLEFT:
                                mo1[0] = new EventMove(this.sound, EventMove.MOVE.down, l1, parent.Player);
                                break;
                        }
                        page.AddEvent(new NSEvent.EventMove(this.sound, page.eventmanager, -1, mo1, this.parent, this.field, this.savedate));
                        break;
                    case "money":
                        page.AddEvent(new MoneyPlus(this.sound, page.eventmanager, int.Parse(strArray1[1]), bool.Parse(strArray1[2]), this.savedate));
                        break;
                    case "msg":
                        var msgKey = strArray1[1];
                        var msgDialogue = ShanghaiEXE.Translate(msgKey);
                        page.AddEvent(new CommandMessage(this.sound, page.eventmanager, msgDialogue[0], msgDialogue[1], msgDialogue[2], msgDialogue.Face, msgDialogue.Face.Mono, msgDialogue.Face.Auto, this.savedate));
                        break;
                    case "msgclose":
                        page.AddEvent(new CloseMassageWindow(this.sound, page.eventmanager));
                        break;
                    case "msgopen":
                        page.AddEvent(new OpenMassageWindow(this.sound, page.eventmanager));
                        break;
                    case "numset":
                        var numsetKey = strArray1[1];
                        var numsetDialogue = ShanghaiEXE.Translate(numsetKey);
                        page.AddEvent(new NumberSet(this.sound, page.eventmanager, numsetDialogue[0], numsetDialogue.Face, numsetDialogue.Face.Mono, int.Parse(strArray1[2]), int.Parse(strArray1[3]), this.savedate));
                        break;
                    case "piano":
                        page.AddEvent(new Piano(this.sound, page.eventmanager, new Note(strArray1[1]), int.Parse(strArray1[2]), int.Parse(strArray1[3]), this.parent, this.savedate));
                        break;
                    case "playerHide":
                        page.AddEvent(new PlayerHide(this.sound, page.eventmanager, bool.Parse(strArray1[1]), this.parent.Player, this.savedate));
                        break;
                    case "plugIn":
                        page.AddEvent(new OpenMassageWindow(this.sound, page.eventmanager));
                        dialogue = ShanghaiEXE.Translate("Map.JackIn");
                        page.AddEvent(new CommandMessage(this.sound, page.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, dialogue.Face.Mono, dialogue.Face.Auto, this.savedate));
                        page.AddEvent(new CloseMassageWindow(this.sound, page.eventmanager));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 0, false, 0, 5, "0", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 1, false, 0, 5, "1", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 2, false, 0, 5, "2", this.parent.Player, this.savedate));
                        page.AddEvent(new EffectMake(this.sound, page.eventmanager, 0, "player", 0, 1, 2, 1, -1, 0, 1, "none", this.parent, this.field, this, this.savedate));
                        page.AddEvent(new PlayerHide(this.sound, page.eventmanager, true, this.parent.Player, this.savedate));
                        page.AddEvent(new Wait(this.sound, page.eventmanager, 48, false, this.savedate));
                        page.AddEvent(new SEmon(this.sound, page.eventmanager, "pikin", 0, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 0, false, 2, 0, "21", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 1, false, 2, 0, "18", this.parent.Player, this.savedate));
                        page.AddEvent(new EffectMake(this.sound, page.eventmanager, 1, "flash", 0, 1, 2, 1, 20, 0, 2, "none", this.parent, this.field, this, this.savedate));
                        page.AddEvent(new Wait(this.sound, page.eventmanager, 48, false, this.savedate));
                        page.AddEvent(new Fade(this.sound, page.eventmanager, 30, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, true, this.savedate));
                        page.AddEvent(new PlugInAnime(this.sound, page.eventmanager, this.parent.Player, this.field, this.savedate));
                        page.AddEvent(new MapChange(this.sound, page.eventmanager, strArray1[1], new Point(int.Parse(strArray1[2]), int.Parse(strArray1[3])), int.Parse(strArray1[4]), MapCharacterBase.ANGLE.DOWN, this.savedate, this.field));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 0, false, 0, 5, "0", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 1, false, 0, 5, "1", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 2, false, 0, 5, "2", this.parent.Player, this.savedate));
                        page.AddEvent(new SEmon(this.sound, page.eventmanager, "warp", 0, this.savedate));
                        page.AddEvent(new EffectMake(this.sound, page.eventmanager, 2, "player", 0, 1, 2, 1, -1, 0, 1, "none", this.parent, this.field, this, this.savedate));
                        page.AddEvent(new Fade(this.sound, page.eventmanager, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, true, this.savedate));
                        page.AddEvent(new RunEvent(this.sound, page.eventmanager, "BGMStart", -1, this.parent, this.field, this.savedate));
                        page.AddEvent(new Fade(this.sound, page.eventmanager, 30, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, true, this.savedate));
                        page.AddEvent(new Wait(this.sound, page.eventmanager, 10, false, this.savedate));
                        page.AddEvent(new PlayerHide(this.sound, page.eventmanager, false, this.parent.Player, this.savedate));
                        page.AddEvent(new Wait(this.sound, page.eventmanager, 40, false, this.savedate));
                        EventMove[] mo2 = new EventMove[2];
                        MapCharacterBase.ANGLE angle2 = (MapCharacterBase.ANGLE)Enum.Parse(typeof(MapCharacterBase.ANGLE), strArray1[5]);
                        int l2 = 16;
                        switch (angle2)
                        {
                            case MapCharacterBase.ANGLE.DOWNRIGHT:
                                mo2[0] = new EventMove(this.sound, EventMove.MOVE.right, l2, parent.Player);
                                break;
                            case MapCharacterBase.ANGLE.UPRIGHT:
                                mo2[0] = new EventMove(this.sound, EventMove.MOVE.up, l2, parent.Player);
                                break;
                            case MapCharacterBase.ANGLE.UPLEFT:
                                mo2[0] = new EventMove(this.sound, EventMove.MOVE.left, l2, parent.Player);
                                break;
                            case MapCharacterBase.ANGLE.DOWNLEFT:
                                mo2[0] = new EventMove(this.sound, EventMove.MOVE.down, l2, parent.Player);
                                break;
                        }
                        mo2[1] = new EventMove(this.sound, EventMove.MOVE.wait, 1, parent.Player);
                        page.AddEvent(new NSEvent.EventMove(this.sound, page.eventmanager, -1, mo2, this.parent, this.field, this.savedate));
                        break;
                    case "plugInNO":
                        page.AddEvent(new PlugIn(this.sound, page.eventmanager, this.parent.Player, this.field, this.savedate));
                        page.AddEvent(new MapChange(this.sound, page.eventmanager, strArray1[1], new Point(int.Parse(strArray1[2]), int.Parse(strArray1[3])), int.Parse(strArray1[4]), MapCharacterBase.ANGLE.DOWN, this.savedate, this.field));
                        break;
                    case "question":
                        var questionKey = strArray1[1];
                        var questionDialogue = ShanghaiEXE.Translate(questionKey);
                        var answerKey = strArray1[2];
                        var answerDialogue = ShanghaiEXE.Translate(answerKey);
                        bool cancel;
                        cancel = strArray1.Length > 5 && bool.TryParse(strArray1[5], out cancel) ? cancel : answerDialogue[2] == "";
                        if (answerDialogue[2] == "")
                        {
                            if (questionDialogue[1] == "")
                            {
                                page.AddEvent(new Question(this.sound, page.eventmanager, questionDialogue[0], answerDialogue[0], answerDialogue[1], questionDialogue.Face.Mono, true, true, questionDialogue.Face, this.savedate, cancel));
                                break;
                            }
                            page.AddEvent(new Question(this.sound, page.eventmanager, questionDialogue[0], questionDialogue[1], answerDialogue[0], answerDialogue[1], questionDialogue.Face.Mono, true, questionDialogue.Face, this.savedate, cancel));
                            break;
                        }
                        if (answerDialogue[3] == "")
                        {
                            page.AddEvent(new Question(this.sound, page.eventmanager, answerDialogue[0], answerDialogue[1], answerDialogue[2], questionDialogue.Face.Mono, true, questionDialogue.Face, this.savedate, cancel));
                            break;
                        }
                        page.AddEvent(new Question(this.sound, page.eventmanager, questionDialogue[0], answerDialogue[0], answerDialogue[1], answerDialogue[2], answerDialogue[3], questionDialogue.Face.Mono, true, questionDialogue.Face, this.savedate, cancel));
                        break;
                    case "seon":
                        page.AddEvent(new SEmon(this.sound, page.eventmanager, strArray1[1], 0, this.savedate));
                        break;
                    case "shake":
                        page.AddEvent(new ShakeStart(this.sound, page.eventmanager, int.Parse(strArray1[1]), int.Parse(strArray1[2]), this.savedate));
                        break;
                    case "shakeStop":
                        page.AddEvent(new ShakeStop(this.sound, page.eventmanager, this.savedate));
                        break;
                    case "shop":
                        List<Goods> goodsList = new List<Goods>();
                        for (int index = 7; index < strArray1.Length; ++index)
                        {
                            string[] strArray2 = strArray1[index].Split(',');
                            goodsList.Add(new Goods()
                            {
                                numberNo = int.Parse(strArray2[0]),
                                numberSub = int.Parse(strArray2[1]),
                                price = int.Parse(strArray2[2]),
                                stock = int.Parse(strArray2[3])
                            });
                        }
						{
                            var faceSheet = 0;
                            var faceIndex = (byte)0;
                            var mono = false;
                            var auto = false;

                            string[] faceSections = null, modifierSections = null;
							if (strArray1[4].Contains(",") && strArray1[5].Contains(",")
								&& int.TryParse((faceSections = faceSections ?? strArray1[4].Split(','))[0], out faceSheet)
								&& byte.TryParse((faceSections = faceSections ?? strArray1[4].Split(','))[1], out faceIndex)
								&& bool.TryParse((modifierSections = modifierSections ?? strArray1[5].Split(','))[0], out mono)
								&& bool.TryParse((modifierSections = modifierSections ?? strArray1[5].Split(','))[1], out auto))
							{
                                ;
							}
                            else if (int.TryParse(strArray1[4], out faceSheet)
                                && byte.TryParse(strArray1[5], out faceIndex))
							{
                                ;
							}
							page.AddEvent(new Shop(this.sound,
								 page.eventmanager,
								 int.Parse(strArray1[1]),
								 int.Parse(strArray1[2]),
								 byte.Parse(strArray1[3]),
                                 faceSheet,
                                 faceIndex,
                                 mono,
                                 auto,
								 goodsList.ToArray(),
								 this.field,
								 this.savedate,
								 byte.Parse(strArray1[6])));
						}
                        break;
                    case "stopSkip":
                        page.AddEvent(new StopSkip(this.sound, page.eventmanager, this.savedate));
                        break;
                    case "wait":
                        page.AddEvent(new Wait(this.sound, page.eventmanager, int.Parse(strArray1[1]), bool.Parse(strArray1[2]), this.savedate));
                        break;
                    case "warp":
                        page.AddEvent(new PlayerHide(this.sound, page.eventmanager, true, this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 0, false, 0, 5, "0", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 1, false, 0, 5, "1", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 2, false, 0, 5, "2", this.parent.Player, this.savedate));
                        page.AddEvent(new SEmon(this.sound, page.eventmanager, "warp", 0, this.savedate));
                        page.AddEvent(new EffectMake(this.sound, page.eventmanager, 5, "player", 0, 1, 2, 1, -1, 0, 1, "none", this.parent, this.field, this, this.savedate));
                        page.AddEvent(new Fade(this.sound, page.eventmanager, 30, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, true, this.savedate));
                        page.AddEvent(new MapChange(this.sound, page.eventmanager, strArray1[1], new Point(int.Parse(strArray1[2]), int.Parse(strArray1[3])), int.Parse(strArray1[4]), MapCharacterBase.ANGLE.DOWN, this.savedate, this.field));
                        page.AddEvent(new RunEvent(this.sound, page.eventmanager, "BGMStart", -1, this.parent, this.field, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 0, false, 0, 5, "0", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 1, false, 0, 5, "1", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 2, false, 0, 5, "2", this.parent.Player, this.savedate));
                        page.AddEvent(new SEmon(this.sound, page.eventmanager, "warp", 0, this.savedate));
                        page.AddEvent(new EffectMake(this.sound, page.eventmanager, 2, "player", 0, 1, 2, 1, -1, 0, 1, "none", this.parent, this.field, this, this.savedate));
                        page.AddEvent(new Fade(this.sound, page.eventmanager, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, true, this.savedate));
                        page.AddEvent(new Fade(this.sound, page.eventmanager, 30, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, true, this.savedate));
                        page.AddEvent(new Wait(this.sound, page.eventmanager, 12, false, this.savedate));
                        page.AddEvent(new PlayerHide(this.sound, page.eventmanager, false, this.parent.Player, this.savedate));
                        page.AddEvent(new Wait(this.sound, page.eventmanager, 40, false, this.savedate));
                        EventMove[] mo3 = new EventMove[1];
                        MapCharacterBase.ANGLE angle3 = (MapCharacterBase.ANGLE)Enum.Parse(typeof(MapCharacterBase.ANGLE), strArray1[5]);
                        int l3 = 16;
                        switch (angle3)
                        {
                            case MapCharacterBase.ANGLE.DOWNRIGHT:
                                mo3[0] = new EventMove(this.sound, EventMove.MOVE.right, l3, parent.Player);
                                break;
                            case MapCharacterBase.ANGLE.UPRIGHT:
                                mo3[0] = new EventMove(this.sound, EventMove.MOVE.up, l3, parent.Player);
                                break;
                            case MapCharacterBase.ANGLE.UPLEFT:
                                mo3[0] = new EventMove(this.sound, EventMove.MOVE.left, l3, parent.Player);
                                break;
                            case MapCharacterBase.ANGLE.DOWNLEFT:
                                mo3[0] = new EventMove(this.sound, EventMove.MOVE.down, l3, parent.Player);
                                break;
                        }
                        page.AddEvent(new NSEvent.EventMove(this.sound, page.eventmanager, -1, mo3, this.parent, this.field, this.savedate));
                        break;
                    case "warpPlugOut":
                        page.AddEvent(new PlayerHide(this.sound, page.eventmanager, true, this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 0, false, 0, 5, "0", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 1, false, 0, 5, "1", this.parent.Player, this.savedate));
                        page.AddEvent(new EditValue(this.sound, page.eventmanager, 2, false, 0, 5, "2", this.parent.Player, this.savedate));
                        page.AddEvent(new SEmon(this.sound, page.eventmanager, "warp", 0, this.savedate));
                        page.AddEvent(new EffectMake(this.sound, page.eventmanager, 5, "player", 0, 1, 2, 1, -1, 0, 1, "none", this.parent, this.field, this, this.savedate));
                        page.AddEvent(new Fade(this.sound, page.eventmanager, 30, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, true, this.savedate));
                        page.AddEvent(new PlugOut(this.sound, page.eventmanager, this.parent, this.savedate));
                        page.AddEvent(new PlayerHide(this.sound, page.eventmanager, false, this.parent.Player, this.savedate));
                        page.AddEvent(new Fade(this.sound, page.eventmanager, 15, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, true, this.savedate));
                        break;
                    default:
                        break;
                }
            }
        }

        public override float RendSetter()
        {
            if (!this.LunPage.hitform)
                return (float)(Position.X + (double)this.LunPage.hitShift.X + (Position.Y + (double)this.LunPage.hitShift.Y));
            return (float)(position.X + (double)this.LunPage.hitShift.X + LunPage.hitrange.X + (position.Y + (double)this.LunPage.hitShift.Y - LunPage.hitrange.Y));
        }

        public override void Render(IRenderer dg)
        {
            if (this.lunPage < 0 || this.NoPrint)
                return;
            this.ChangeQuarter();
            if (this.LunPage.character)
            {
                int num1 = (int)this.positionQ.X - (int)this.field.camera.X + this.Shake.X;
                int num2 = (int)this.positionQ.Y - (int)this.field.camera.Y + this.Shake.Y - 3;
                if (num1 > -32 && num1 < 272 & num2 > -64 && num2 < 304)
                {
                    int num3 = (int)(this.Angle - 1);
                    bool rebirth = num3 / 2 == 1 || num3 / 2 == 3;
                    Point point = new Point(this.eventPages[this.lunPage].graphicNo[0] / 4 * 448, this.eventPages[this.lunPage].graphicNo[0] % 4 * 192 + (num3 / 2 >= 1 && num3 / 2 <= 2 ? 96 : 0));
                    this._rect = new Rectangle(animeflame * 64 + point.X, point.Y, 64, 96);
                    this._position = new Vector2(num1, num2);
                    base.Render(dg);
                    this._position.Y += this.jumpY - 24;
                    dg.DrawImage(dg, this.eventPages[this.lunPage].graphicName, this._rect, false, this._position, rebirth, Color.White);
                }
            }
            else // if (positionQ.X <= (double)(-this.LunPage.graphicNo[0] / 2) || !(positionQ.X < (double)(this.LunPage.graphicNo[0] / 2) & positionQ.Y > (double)(-this.LunPage.graphicNo[1] / 2)) || positionQ.Y >= (double)(this.LunPage.graphicNo[1] / 2))
            {
                this._rect = new Rectangle(this.LunPage.graphicNo[4] <= 1 ? this.LunPage.graphicNo[0] : this.LunPage.graphicNo[0] + this.LunPage.graphicNo[2] * animeflame, this.LunPage.graphicNo[1], this.LunPage.graphicNo[2], this.LunPage.graphicNo[3]);
                this._position = new Vector2((int)this.positionQ.X - (int)this.field.camera.X + this.Shake.X, (int)this.positionQ.Y - (int)this.field.camera.Y + this.Shake.Y - 4);
                dg.DrawImage(dg, this.LunPage.graphicName, this._rect, false, this._position, this.LunPage.rebirth, Color.White);
            }
        }
    }
}
