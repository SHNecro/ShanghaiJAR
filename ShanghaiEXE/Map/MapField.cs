using NSBackground;
using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSEvent;
using NSGame;
using NSMap.Character;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Common.EncodeDecode;
using NSShanghaiEXE.Map;

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
        private readonly UnboundedMap map;
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

        public UnboundedMap Map_
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
            this.map = new UnboundedMap(new byte[length3, length1, length2]);
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
            var eventIndex = 0;
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
                    var mapEvent = new MapEventBase(s, this.parent, po, int.Parse(strArray4[3]), MapCharacterBase.ANGLE.UP, this, id, save, reader, this.mapname);
                    mapEvent.index = eventIndex;
                    this.Events.Add(mapEvent);
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
                    var mysteryData = new MysteryData(s, this.parent, po, floor, MapCharacterBase.ANGLE.UP, this, id, save, reader, random);
                    mysteryData.index = eventIndex;
                    this.Events.Add(mysteryData);
                }
                eventIndex++;
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
            this.back.Render(dg);
            var mapCharacters = this.GetSortedLevelObjects();
            for (int index1 = ((IEnumerable<string>)this.GraphicName).Count<string>() / 2 + 1; index1 >= 0; --index1)
            {
                // Draw floors and ramps
                this.FieldRender(dg, index1 * 2);
                this.FieldRender(dg, index1 * 2 + 1);

                var levelMapCharacters = mapCharacters.ContainsKey(index1) ? mapCharacters[index1] : new MapCharacterBase[0];
                foreach (MapCharacterBase mapCharacterBase in levelMapCharacters)
                {
                    mapCharacterBase.Render(dg);
                }
            }
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

        private Dictionary<int, MapCharacterBase[]> GetSortedLevelObjects()
        {
            var unsortedLevels = new IEnumerable<MapCharacterBase>[]
            { this.Events, this.effect, new[] { parent.Player } }
            .SelectMany(em => em).GroupBy(mcb => mcb.floor);

            var sortedLevels = new Dictionary<int, MapCharacterBase[]>();

            foreach (var levelObjects in unsortedLevels)
            {
                var objects = new List<MapCharacterBase>();

                var rendTypes = levelObjects.GroupBy(mcb => mcb.rendType).OrderBy(gr => gr.Key);
                foreach (var rendType in rendTypes)
                {
                    var sortedRendType = TopologicalRenderSort(rendType.ToList(), this.camera, parent.MapsizeX / 2.0);

                    objects.AddRange(sortedRendType);
                }
                sortedLevels[levelObjects.Key] = objects.ToArray();
            }

            return sortedLevels;
        }
        
        private static IList<MapCharacterBase> TopologicalRenderSort(IList<MapCharacterBase> unsorted, Vector2 camera, double mapXAdj)
        {
            // Creates graph of items which are behind each
            var graph = Enumerable.Range(0, unsorted.Count).ToDictionary(i => i, i => new List<int>());
            for (var i = 0; i < unsorted.Count; i++)
            {
                if (IsOrderingSkipped(unsorted[i] as MapEventBase, camera, mapXAdj)) continue;

                for (var ii = i+1; ii < unsorted.Count; ii++)
                {
                    if (IsOrderingSkipped(unsorted[ii] as MapEventBase, camera, mapXAdj)) continue;

                    var sortValue = IsBehind(unsorted[i], unsorted[ii]);
                    // If algorithm supports order-doesn't-matter, null value means no edge
                    if (sortValue == true)
                    {
                        graph[ii].Add(i);
                    }
                    else if (sortValue == false)
                    {
                        graph[i].Add(ii);
                    }
                }
            }

            var sortedNodeIndices = new List<int>();
            
            var lowValues = new Dictionary<int, int>();
            var discoveryDepth = 0;

            Action<int> recursiveTarjan = i => { };
            recursiveTarjan = (i) =>
            {
                lowValues[i] = discoveryDepth;
                foreach (var child in graph[i])
                {
                    if (!lowValues.ContainsKey(child))
                    {
                        discoveryDepth++;
                        recursiveTarjan(child);
                    }
                    lowValues[i] = Math.Min(lowValues[i], lowValues[child]);
                }
                sortedNodeIndices.Add(i);
            };

            var unsortedNode = graph.FirstOrDefault(kvp => !lowValues.ContainsKey(kvp.Key));
            while (unsortedNode.Value != default(List<int>))
            {
                recursiveTarjan(unsortedNode.Key);
                unsortedNode = graph.FirstOrDefault(kvp => !lowValues.ContainsKey(kvp.Key));
            }

            return sortedNodeIndices.Select(i => unsorted[i]).ToList();
        }

        private static bool IsOrderingSkipped(MapEventBase mcb, Vector2 camera, double mapXAdjust)
        {
            var page = mcb?.LunPage;

            if (page == null) return false;
            if (!page.character && (page.graphicNo[2] == 0 || page.graphicNo[3] == 0)) return true;

            Vector2 center = new Vector2(mcb.Position.X, mcb.Position.Y);
            var cameraRect = new RectangleF(0, 0, 240, 160);

            center += new Vector2(page.hitShift.X, page.hitShift.Y);
            if (page.hitform == true)
            {
                center += new Vector2(-8, -8);
            }
            if (mcb is MysteryData)
            {
                center += new Vector2(-10, -10);
            }

            var graphicPosX = (float)(120.0 + mapXAdjust + center.X * 2.0 - center.Y * 2.0);
            var graphicPosY = (float)(32.0 + center.X + center.Y + mcb.Position.Z + 24.0);
            var cameraTLRelativePos = new Vector2(graphicPosX - camera.X, graphicPosY - camera.Y);

            var graphicWidth = (page.character ? 64 : page.graphicNo[2]) / 2;
            var graphicHeight = (page.character ? 96 : page.graphicNo[3]) / 2;

            var cameraTLRelativeRect1 = RectangleF.FromLTRB(
                cameraTLRelativePos.X - graphicWidth,
                cameraTLRelativePos.Y - graphicHeight,
                cameraTLRelativePos.X + graphicWidth,
                cameraTLRelativePos.Y + graphicHeight);
            return !cameraTLRelativeRect1.IntersectsWith(cameraRect);
        }

        private static bool? IsBehind(MapCharacterBase mcb1, MapCharacterBase mcb2)
        {
            if (mcb1 == mcb2) return null;

            // REQUIRES TOPOLOGICAL SORT
            var obj1 = mcb1 as MapEventBase;
            var page1 = obj1?.LunPage;
            var obj2 = mcb2 as MapEventBase;
            var page2 = obj2?.LunPage;

            Vector2 center1 = new Vector2(mcb1.Position.X, mcb1.Position.Y), size1 = Vector2.Zero;
            Vector2 center2 = new Vector2(mcb2.Position.X, mcb2.Position.Y), size2 = Vector2.Zero;

            if (obj1 != null)
            {
                center1 += new Vector2(page1.hitShift.X, page1.hitShift.Y);
                var width1 = (page1.hitform ? page1.hitrange.X : page1.hitrange.X * 2);
                var height1 = (page1.hitform ? page1.hitrange.Y : page1.hitrange.X * 2);
                size1 = new Vector2(width1, height1);

                if (page1.hitform == true)
                {
                    center1 += new Vector2(-8, -8);
                }
                if (obj1 is MysteryData)
                {
                    center1 += new Vector2(-10, -10);
                }
            }
            else if (mcb1 is Player)
            {
                size1 += new Vector2(2, 2);
            }

            if (obj2 != null)
            {
                center2 += new Vector2(page2.hitShift.X, page2.hitShift.Y);
                var width2 = (page2.hitform ? page2.hitrange.X : page2.hitrange.X * 2);
                var height2 = (page2.hitform ? page2.hitrange.Y : page2.hitrange.X * 2);
                size2 = new Vector2(width2, height2);
                if (page2.hitform == true)
                {
                    center2 += new Vector2(-8, -8);
                }
                if (obj2 is MysteryData)
                {
                    center2 += new Vector2(-10, -10);
                }
            }
            else if (mcb2 is Player)
            {
                size2 += new Vector2(2, 2);
            }

            var front1 = center1 + size1 / 2;
            var back1 = center1 - size1 / 2;

            var front2 = center2 + size2 / 2;
            var back2 = center2 - size2 / 2;
            
            var rect1 = RectangleF.FromLTRB(back1.X, back1.Y, front1.X, front1.Y);
            var rect2 = RectangleF.FromLTRB(back2.X, back2.Y, front2.X, front2.Y);

            // Hitboxes do not intersect
            if (!rect1.IntersectsWith(rect2))
            {
                var screenPosition1 = ToScreenPosition(new Vector2(mcb1.Position.X, mcb1.Position.Y));
                var leftmost1 = front1 - new Vector2(size1.X, 0);
                var rightmost1 = front1 - new Vector2(0, size1.Y);
                var screenLeftmost1 = ToScreenPosition(leftmost1);
                var screenRightmost1 = ToScreenPosition(rightmost1);

                var screenPosition2 = ToScreenPosition(new Vector2(mcb2.Position.X, mcb2.Position.Y));
                var leftmost2 = front2 - new Vector2(size2.X, 0);
                var rightmost2 = front2 - new Vector2(0, size2.Y);
                var screenLeftmost2 = ToScreenPosition(leftmost2);
                var screenRightmost2 = ToScreenPosition(rightmost2);

                // If 2 is to the right of 1 entirely
                if (screenLeftmost2.X >= screenRightmost1.X)
                {
                    var graphicRightSideWidth1 = obj1 != null
                        ? (page1.character ? 64 : page1.graphicNo[2]) / 4
                        : (mcb1 is MapEffect ? 24 : 0);

                    var graphicLeftSideWidth2 = obj2 != null
                        ? (page2.character ? 64 : page2.graphicNo[2]) / 4
                        : (mcb2 is MapEffect ? 24 : 0);

                    // Only check if sprite extends past hitbox (fixed length for effects, which have no defined width)
                    // Prevents mis-sorting when Tarjan's algorithm 'cuts' a cycle and an off-screen object is "in front"
                    if (screenPosition1.X + graphicRightSideWidth1 > screenPosition2.X - graphicLeftSideWidth2)
                    {
                        return screenLeftmost2.Y > screenRightmost1.Y;
                    }

                    return null;
                }

                // If 2 is to the left of 1 entirely
                if (screenRightmost2.X <= screenLeftmost1.X)
                {
                    var graphicLeftSideWidth1 = obj1 != null
                        ? (page1.character ? 64 : page1.graphicNo[2]) / 4
                        : (mcb1 is MapEffect ? 24 : 0);

                    var graphicRightSideWidth2 = obj2 != null
                        ? (page2.character ? 64 : page2.graphicNo[2]) / 4
                        : (mcb2 is MapEffect ? 24 : 0);

                    if (screenPosition2.X + graphicRightSideWidth2 > screenPosition1.X - graphicLeftSideWidth1)
                    {
                        return screenLeftmost2.Y > screenRightmost1.Y;
                    }

                    return null;
                }

                // If objects overlap in screen-Y, trivial since boxes do not intersect
                if (back1.X >= front2.X) return false;
                if (back2.X >= front1.X) return true;

                if (back1.Y >= front2.Y) return false;
                if (back2.Y >= front1.Y) return true;

                // Impossible case, does not intersect
                return null;
            }
            else
            {
                // If completely within, compare center to diagonal
                var rect1InRect2 = rect2.Contains(rect1);
                var rect2InRect1 = rect1.Contains(rect2);
                if (rect1InRect2 || rect2InRect1)
                {
                    return ToScreenPosition(center1).Y < ToScreenPosition(center2).Y;
                    //Broken: If 3 objects A contains B contains C, and A is long/B is wide, B can be in front of A, but C is in front of B but behind A
                    //var containerCenter = rect1InRect2 ? center1 : center2;
                    //var containerSize = rect1InRect2 ? size1 : size2;
                    //var containedCenter = rect1InRect2 ? center2 : center1;
                    //// https://stackoverflow.com/questions/1560492/how-to-tell-whether-a-point-is-to-the-right-or-left-side-of-a-line
                    //var ab = (containerCenter + (new Vector2(containerSize.X, -containerSize.Y) / 2)) - containerCenter;
                    //var am = containedCenter - containerCenter;

                    //return (ab.X * am.Y) - (ab.Y * am.X) > 0;
                }

                if (rect1.Contains(new PointF(back2.X, back2.Y)))
                {
                    // If back but not front inside (otherwise would be completely inside), is in front
                    return true;
                }
                else
                {
                    // Only left point inside
                    if (rect1.Contains(new PointF(back2.X, front2.Y)))
                    {
                        var rightmostRect1 = ToScreenPosition(front1 - new Vector2(0, size1.Y));
                        var leftmostRect2 = ToScreenPosition(front2 - new Vector2(size2.X, 0));
                        return rightmostRect1.Y < leftmostRect2.Y;
                    }
                    // Check not needed, all other cases handled (front is outside, back is inside, left not inside, is intersecting)
                    else // if (rect1.Contains(new PointF(front2.X, back2.Y)))
                    {
                        var leftmostRect1 = ToScreenPosition(front1 - new Vector2(size1.X, 0));
                        var rightmostRect2 = ToScreenPosition(front2 - new Vector2(0, size2.Y));
                        return leftmostRect1.Y < rightmostRect2.Y;
                    }
                }
            }
        }

        private static Vector2 ToScreenPosition(Vector2 gamePosition)
        {
            return new Vector2(gamePosition.X - gamePosition.Y, (gamePosition.X + gamePosition.Y) / 2);
        }
    }
}
