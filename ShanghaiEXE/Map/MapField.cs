using NSBackground;
using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSEvent;
using NSGame;
using NSMap.Character;
using SlimDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Common.EncodeDecode;

namespace NSMap
{
    public class MapField : AllBase
    {
        public List<EventManager> encounts = new List<EventManager>();
        public Vector2 camera = new Vector2(100f, 50f);
        public List<MapEventBase> Events = new List<MapEventBase>();
        public List<EffectGenerator> effectgenerator = new List<EffectGenerator>();
        public List<MapEffect> effect = new List<MapEffect>();
        public int[] encountCap = new int[2];
        private readonly List<MapEventBase> masterList = new List<MapEventBase>();
        public string saveBGM;
        private readonly string[] graphicName;
        private readonly byte[,,] map;
        private Rectangle rect;
        private readonly int height;
        private readonly int rendX;
        private readonly int rendY;
        public SceneMap parent;
        private int backNo;
        public BackgroundBase back;
        public string mapname;
        public RandomMystery[] randomMystery;
        public SaveData save;
        private bool threadEnd;
        public Thread threadTexRead;
        public bool masterMake;

        public string[] GraphicName
        {
            get
            {
                return this.graphicName;
            }
        }

        public byte[,,] Map_
        {
            get
            {
                return this.map;
            }
        }

        public int MapsizeX
        {
            get
            {
                return this.rect.Width;
            }
        }

        public int MapsizeY
        {
            get
            {
                return this.rect.Height;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
        }

        public Vector2 CameraPlus
        {
            get
            {
                return this.parent.cameraPlus;
            }
            set
            {
                this.parent.cameraPlus = value;
            }
        }

        public int SlideX
        {
            get
            {
                return this.rendX;
            }
        }

        public int SlideY
        {
            get
            {
                return this.rendY;
            }
        }

        public MapField(IAudioEngine s, string txtname, SaveData save, SceneMap p)
          : base(s)
        {
            this.save = save;
            this.parent = p;
            this.parent.parent.KeepActiveTexList.RemoveAll(e => !e.Contains(txtname));
            this.parent.parent.TexClear(false);
            this.mapname = txtname;
            save.nowMap = txtname;
            string path = NSGame.Debug.MaskMapFile ? "data/" + txtname + ".she" : "map/" + txtname + ".txt";
            if (!File.Exists(path))
                return;
            StreamReader reader = new StreamReader(path, Encoding.GetEncoding("Shift_JIS"));
            string A1 = reader.ReadLine();
            if (NSGame.Debug.MaskMapFile)
                A1 = TCDEncodeDecode.EncMapScript(A1);
            string[] strArray1 = A1.Split(',');
            int length1 = int.Parse(strArray1[0]);
            int length2 = int.Parse(strArray1[1]);
            int length3 = int.Parse(strArray1[8]);
            this.rendX = int.Parse(strArray1[2]);
            this.rendY = int.Parse(strArray1[3]);
            this.rect = new Rectangle(0, 0, int.Parse(strArray1[4]), int.Parse(strArray1[5]));
            save.plase = ShanghaiEXE.Translate(strArray1[6]);
            this.height = int.Parse(strArray1[7]);
            this.map = new byte[length3, length1, length2];
            this.backNo = int.Parse(strArray1[9]);
            this.back = BackgroundBase.BackMake(this.backNo);
            this.encountCap[0] = int.Parse(strArray1[10]);
            this.encountCap[1] = int.Parse(strArray1[11]);
            string str1 = strArray1[12];
            this.graphicName = new string[length3 + (length3 - 1)];
            for (int index = 0; index < ((IEnumerable<string>)this.graphicName).Count<string>(); ++index)
                this.graphicName[index] = str1 + (index + 1).ToString();
            string str2;
            for (int index1 = 0; index1 < this.map.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.map.GetLength(2); ++index2)
                {
                    string A2 = reader.ReadLine();
                    if (NSGame.Debug.MaskMapFile)
                        A2 = TCDEncodeDecode.EncMapScript(A2);
                    string[] strArray2 = A2.Split(',');
                    for (int index3 = 0; index3 < this.map.GetLength(1); ++index3)
                    {
                        this.map[index1, index3, index2] = 0;
                        this.map[index1, index3, index2] = byte.Parse(strArray2[index3]);
                    }
                }
                string A3 = reader.ReadLine();
                if (NSGame.Debug.MaskMapFile)
                    str2 = TCDEncodeDecode.EncMapScript(A3);
            }
            this.encounts.Clear();
            string A4;
            while ((A4 = reader.ReadLine()) != "")
            {
                if (NSGame.Debug.MaskMapFile)
                    A4 = TCDEncodeDecode.EncMapScript(A4);
                if (!(A4 == ""))
                {
                    EventManager m = new EventManager(this.sound);
                    string[] strArray2 = A4.Split(':');
                    m.AddEvent(new BgmSave(this.sound, m, this, save));
                    Battle battle;
                    if (strArray2.Length >= 20)
                    {
                        battle = new NSEvent.Battle(this.sound,
                                                    m,
                                                    int.Parse(strArray2[1]),
                                                    byte.Parse(strArray2[2]),
                                                    int.Parse(strArray2[3]),
                                                    int.Parse(strArray2[4]),
                                                    int.Parse(strArray2[5]),
                                                    int.Parse(strArray2[6]),
                                                    int.Parse(strArray2[7]),
                                                    int.Parse(strArray2[8]),
                                                    ShanghaiEXE.Translate(strArray2[9]),
                                                    int.Parse(strArray2[10]),
                                                    byte.Parse(strArray2[11]),
                                                    int.Parse(strArray2[12]),
                                                    int.Parse(strArray2[13]),
                                                    int.Parse(strArray2[14]),
                                                    int.Parse(strArray2[15]),
                                                    int.Parse(strArray2[16]),
                                                    int.Parse(strArray2[17]),
                                                    ShanghaiEXE.Translate(strArray2[18]),
                                                    int.Parse(strArray2[19]),
                                                    byte.Parse(strArray2[20]),
                                                    int.Parse(strArray2[21]),
                                                    int.Parse(strArray2[22]),
                                                    int.Parse(strArray2[23]),
                                                    int.Parse(strArray2[24]),
                                                    int.Parse(strArray2[25]),
                                                    int.Parse(strArray2[26]),
                                                    ShanghaiEXE.Translate(strArray2[27]),
                                                    (Panel.PANEL)int.Parse(strArray2[28]),
                                                    (Panel.PANEL)int.Parse(strArray2[29]),
                                                    int.Parse(strArray2[30]),
                                                    bool.Parse(strArray2[31]),
                                                    bool.Parse(strArray2[32]),
                                                    bool.Parse(strArray2[33]),
                                                    bool.Parse(strArray2[34]),
                                                    strArray2[35],
                                                    this.backNo,
                                                    save);
                    }
                    else
                    {
                        battle = new NSEvent.Battle(this.sound,
                                                    m,
                                                    (EnemyBase.VIRUS)Enum.Parse(typeof(EnemyBase.VIRUS), strArray2[1]),
                                                    byte.Parse(strArray2[2]),
                                                    int.Parse(strArray2[3]),
                                                    int.Parse(strArray2[4]),
                                                    (EnemyBase.VIRUS)Enum.Parse(typeof(EnemyBase.VIRUS), strArray2[5]),
                                                    byte.Parse(strArray2[6]),
                                                    int.Parse(strArray2[7]),
                                                    int.Parse(strArray2[8]),
                                                    (EnemyBase.VIRUS)Enum.Parse(typeof(EnemyBase.VIRUS), strArray2[9]),
                                                    byte.Parse(strArray2[10]),
                                                    int.Parse(strArray2[11]),
                                                    int.Parse(strArray2[12]),
                                                    (Panel.PANEL)Enum.Parse(typeof(Panel.PANEL), strArray2[13]),
                                                    (Panel.PANEL)Enum.Parse(typeof(Panel.PANEL), strArray2[14]),
                                                    int.Parse(strArray2[15]),
                                                    bool.Parse(strArray2[16]),
                                                    bool.Parse(strArray2[17]),
                                                    bool.Parse(strArray2[18]),
                                                    save);
                    }
                    m.AddEvent(battle);
                    m.AddEvent(new BgmLoad(this.sound, m, this, save));
                    m.AddEvent(new Fade(this.sound, m, 17, 0, 0, 0, 0, false, save));
                    this.encounts.Add(m);
                }
                else
                    break;
            }
            string A5 = reader.ReadLine();
            if (NSGame.Debug.MaskMapFile)
                A5 = TCDEncodeDecode.EncMapScript(A5);
            string[] strArray3 = A5.Split(':');
            List<RandomMystery> randomMysteryList = new List<RandomMystery>();
            foreach (string str3 in strArray3)
            {
                if (str3 == "")
                {
                    break;
                }
                if (str3 == "random")
                {
                    continue;
                }
                string[] strArray2 = str3.Split(',');
                randomMysteryList.Add(new RandomMystery()
                {
                    itemType = int.Parse(strArray2[0]),
                    itemNumber = int.Parse(strArray2[1]),
                    itemSub = int.Parse(strArray2[2]),
                    getInfo = ShanghaiEXE.Translate(strArray2[3])
                });
            }
            this.randomMystery = randomMysteryList.ToArray();
            string A6 = reader.ReadLine();
            if (NSGame.Debug.MaskMapFile)
                str2 = TCDEncodeDecode.EncMapScript(A6);
            string A7;
            while ((A7 = reader.ReadLine()) != null)
            {
                if (NSGame.Debug.MaskMapFile)
                    A7 = TCDEncodeDecode.EncMapScript(A7);
                string[] strArray2 = A7.Split(':');
                if (strArray2[0] == "ID")
                {
                    string id = strArray2[1];
                    string A2 = reader.ReadLine();
                    if (NSGame.Debug.MaskMapFile)
                        A2 = TCDEncodeDecode.EncMapScript(A2);
                    string[] strArray4 = A2.Split(':');
                    Point po = new Point(int.Parse(strArray4[1]), int.Parse(strArray4[2]));
                    this.Events.Add(new MapEventBase(s, this.parent, po, int.Parse(strArray4[3]), MapCharacterBase.ANGLE.UP, this, id, save, reader, this.mapname));
                }
                else
                {
                    string id = strArray2[1];
                    string A2 = reader.ReadLine();
                    if (NSGame.Debug.MaskMapFile)
                        A2 = TCDEncodeDecode.EncMapScript(A2);
                    string[] strArray4 = A2.Split(':');
                    Point po = new Point(int.Parse(strArray4[1]), int.Parse(strArray4[2]));
                    int floor = int.Parse(strArray4[3]);
                    RandomMystery random = new RandomMystery();
                    string A3 = reader.ReadLine();
                    if (NSGame.Debug.MaskMapFile)
                        A3 = TCDEncodeDecode.EncMapScript(A3);
                    string[] strArray5 = A3.Split(':')[1].Split(',');
                    random.type = int.Parse(strArray5[0]);
                    random.itemType = int.Parse(strArray5[1]);
                    random.itemNumber = int.Parse(strArray5[2]);
                    random.itemSub = int.Parse(strArray5[3]);
                    random.getInfo = ShanghaiEXE.Translate(strArray5[4]);
                    random.flugNumber = int.Parse(strArray5[5]);
                    this.Events.Add(new MysteryData(s, this.parent, po, floor, MapCharacterBase.ANGLE.UP, this, id, save, reader, random));
                }
            }
            reader.Close();
            this.threadEnd = false;
            this.MapTexLoad();
            this.threadTexRead = new Thread(new ThreadStart(this.MapTexLoad));
            this.threadTexRead.Start();
            this.parent.eventmanagerParallel.events.Clear();
            this.parent.eventmanagerParallel.playevent = false;
        }

        public void MapTexLoad()
        {
            if (this.threadEnd)
                return;
            for (int index = 0; index < this.parent.parent.KeepActiveTexList.Count; ++index)
                this.parent.parent.dg.LoadTexture(this.parent.parent.KeepActiveTexList[index]);
            this.threadEnd = true;
        }

        public void Update()
        {
            this.back.Update();
            foreach (MapEventBase mapEventBase in this.Events)
                mapEventBase.LunPageCheck();
            if (this.parent.battleflag)
                return;
            int num = -1;
            this.effect.RemoveAll(e => !e.Flag);
            if (!this.parent.Player.openMenu)
            {
                foreach (MapCharacterBase mapCharacterBase in this.effect)
                    mapCharacterBase.Update();
            }
            foreach (MapEventBase mapEventBase in this.Events)
            {
                if (mapEventBase is MysteryData || !this.parent.Player.openMenu)
                {
                    mapEventBase.Update();
                    ++num;
                    if (!this.parent.eventmanager.playevent && !this.parent.Player.openMenu)
                    {
                        if (mapEventBase.LunPage.startterms == EventPage.STARTTERMS.Auto || mapEventBase.LunPage.startterms == EventPage.STARTTERMS.Touch && this.parent.Player.hitEvent == num)
                        {
                            if (mapEventBase.LunPage.startterms != EventPage.STARTTERMS.Touch || !mapEventBase.LunPage.eventmanager.NoWait)
                                this.parent.Player.CharaAnimeFlame = 0;
                            this.parent.eventmanager.EventClone(mapEventBase.LunPage.eventmanager);
                            this.parent.eventmanager.playevent = true;
                        }
                        else if (!this.parent.eventmanagerParallel.playevent && mapEventBase.LunPage.startterms == EventPage.STARTTERMS.parallel)
                        {
                            this.parent.eventmanagerParallel.EventClone(mapEventBase.LunPage.eventmanager);
                            this.parent.eventmanagerParallel.playevent = true;
                        }
                    }
                }
            }
            if (!this.parent.Player.openMenu)
            {
                foreach (MapCharacterBase mapCharacterBase in this.effectgenerator)
                    mapCharacterBase.Update();
            }
        }

        public void Render(IRenderer dg)
        {
            try
            {
                this.back.Render(dg);
            }
            catch
            {
            }
            List<MapCharacterBase> mapCharacterBaseList = this.RendSort();
            for (int index1 = ((IEnumerable<string>)this.GraphicName).Count<string>() / 2 + 1; index1 >= 0; --index1)
            {
                this.FieldRender(dg, index1 * 2);
                this.FieldRender(dg, index1 * 2 + 1);
                for (int index2 = 0; index2 <= 2; ++index2)
                {
                    foreach (MapCharacterBase mapCharacterBase in mapCharacterBaseList)
                    {
                        if (mapCharacterBase.rendType == index2 && mapCharacterBase.floor == index1)
                            mapCharacterBase.Render(dg);
                    }
                }
            }
        }

        private List<MapCharacterBase> RendSort()
        {
            List<MapCharacterBase> source1 = new List<MapCharacterBase>();
            List<MapCharacterBase> source2 = new List<MapCharacterBase>();
            foreach (MapEventBase mapEventBase in this.Events)
            {
                if (mapEventBase.LunPage.hitform)
                    source2.Add(mapEventBase);
                else
                    source1.Add(mapEventBase);
            }
            foreach (MapEffect mapEffect in this.effect)
                source1.Add(mapEffect);
            source1.Add(parent.Player);
            List<MapCharacterBase> list1 = source1.OrderBy<MapCharacterBase, float>(c => c.RendSetter()).ToList<MapCharacterBase>();
            List<MapCharacterBase> list2 = source2.OrderBy<MapCharacterBase, float>(c => c.RendSetter()).ToList<MapCharacterBase>();
            int[] numArray = new int[list2.Count];
            int num = -1;
            foreach (MapEventBase object_ in list2)
            {
                ++num;
                bool flag = false;
                for (int index = 0; index < list1.Count; ++index)
                {
                    if (this.ObjectLine(object_, list1[index]))
                    {
                        object_.index = index;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                    object_.index = list1.Count;
            }
            List<MapCharacterBase> list3 = list2.OrderBy<MapCharacterBase, int>(o => o.index).ToList<MapCharacterBase>();
            for (int index1 = 0; index1 < numArray.Length; ++index1)
            {
                int index2 = numArray.Length - index1 - 1;
                list1.Insert(list3[index2].index, list3[index2]);
            }
            return list1;
        }

        private bool ObjectLine(MapEventBase object_, MapCharacterBase chara_)
        {
            if (object_.floor < chara_.floor)
                return true;
            if (object_.floor > chara_.floor)
                return false;
            EventPage lunPage = object_.LunPage;
            if (lunPage.hitrange.X == 0)
                return true;
            double num1 = -(lunPage.hitrange.Y / (double)lunPage.hitrange.X);
            double num2 = object_.Position.Y + (double)object_.LunPage.hitShift.Y - num1 * (object_.Position.X + (double)object_.LunPage.hitShift.X);
            return chara_.position.X * num1 + num2 < (int)chara_.position.Y;
        }

        public void FieldRender(IRenderer dg, int i)
        {
            if (i >= this.graphicName.Length)
                return;
            Vector2 _point = new Vector2(240 - this.rendX - this.camera.X + Shake.X, 160 - this.rendY - this.camera.Y + Shake.Y);
            Rectangle rectangle = new Rectangle((int)_point.X, (int)_point.Y, 240, 160);
            dg.DrawImage(dg, this.graphicName[i], this.rect, false, _point, Color.White);
        }

        public void InteriorSet()
        {
            if (!this.masterMake)
            {
                this.backNo = this.save.ValList[13];
                this.back = BackgroundBase.BackMake(this.backNo);
                for (int index = 0; index < this.Events.Count; ++index)
                {
                    if (this.Events[index].ID.Contains("InteriorItem"))
                        this.masterList.Add(this.Events[index]);
                }
                this.masterMake = true;
            }
            this.Events.RemoveAll(i => i.ID.Contains("InteriorItem"));
            foreach (Interior interior in this.save.interiors)
            {
                Interior i = interior;
                if (i.set)
                {
                    MapEventBase mapEventBase1 = this.masterList.Find(iv => iv.ID.Contains($"InteriorItem{i.number + 1}"));
                    MapEventBase mapEventBase2 = new MapEventBase(this.sound, this.parent, new Point(i.posiX, i.posiY), 0, MapCharacterBase.ANGLE.DOWN, this, mapEventBase1.ID, this.save, "");
                    List<EventPage> eventPageList = new List<EventPage>();
                    foreach (EventPage eventPage1 in mapEventBase1.eventPages)
                    {
                        EventPage eventPage2 = eventPage1.Clone(mapEventBase2);
                        if (i.rebirth)
                            eventPage2.Rebirth();
                        eventPageList.Add(eventPage2);
                    }
                    EventPage[] array = eventPageList.ToArray();
                    mapEventBase2.eventPages = array;
                    foreach (EventPage eventPage in mapEventBase2.eventPages)
                        eventPage.ParentSet(mapEventBase2);
                    this.Events.Add(mapEventBase2);
                }
            }
        }
    }
}
