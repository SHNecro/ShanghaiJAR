using NSShanghaiEXE.InputOutput.Audio;
using NSEvent;
using NSGame;
using NSMap.Character.Terms;
using System.Collections.Generic;
using System.Drawing;

namespace NSMap.Character
{
    public class EventPage : AllBase
    {
        public int[] graphicNo = new int[5];
        public bool character;
        private MapEventBase parent;
        private readonly SaveData savedate;
        public MapCharacterBase.ANGLE angle;
        public EventPage.STARTTERMS startterms;
        public None[] pageStartterms;
        public EventMove[] move;
        public int movingOrder;
        public int rendType;
        public byte speed;
        public bool hitform;
        public Point hitrange;
        public Point hitShift;
        public string graphicName;
        public EventManager eventmanager;
        public bool lun;
        public MapCharacterBase.ANGLE defaultAngle;
        public bool rebirth;

        public bool NormalMan
        {
            get
            {
                return !this.hitform && this.hitrange.X <= 3 && !(this.parent is MysteryData);
            }
        }

        public EventPage(IAudioEngine s, MapEventBase p, SaveData save)
          : base(s)
        {
            this.parent = p;
            this.savedate = save;
            this.eventmanager = new EventManager(this.parent.parent, this.sound);
        }

        public void Rebirth()
        {
            this.rebirth = !this.rebirth;
            int x = this.hitrange.X;
            this.hitrange.X = this.hitrange.Y;
            this.hitrange.Y = x;
        }

        public void Update()
        {
            if (this.character)
            {
                if (this.parent.moveOrder.Length == 0)
                {
                    if ((uint)this.move.Length > 0U)
                    {
                        if (!this.parent.stop)
                        {
                            if (this.movingOrder >= this.move.Length)
                                this.movingOrder = 0;
                            this.move[this.movingOrder].Move(speed);
                            if (this.move[this.movingOrder].MoveEnd())
                                ++this.movingOrder;
                        }
                    }
                    else if (!this.parent.parent.eventmanager.playevent && this.parent.Angle != this.defaultAngle)
                        this.parent.Angle = this.defaultAngle;
                }
                else if (this.parent.movingOrder >= this.parent.moveOrder.Length)
                {
                    this.parent.movingOrder = 0;
                    this.parent.moveOrder = new EventMove[0];
                    if (this.parent.CharaAnimeFlame > 0)
                        this.parent.CharaAnimeFlame = 0;
                }
                else
                {
                    this.parent.moveOrder[this.parent.movingOrder].Move(speed);
                    if (this.parent.moveOrder[this.parent.movingOrder].MoveEnd())
                        ++this.parent.movingOrder;
                }
                if (!this.parent.floating)
                    return;
                this.FlameControl(6);
                if (this.moveflame)
                    ++this.parent.CharaAnimeFlame;
            }
            else
            {
                this.FlameControl(speed);
                if (this.moveflame)
                {
                    ++this.parent.AnimeFlame;
                    if (parent.CharaAnimeFlame >= this.graphicNo[4])
                        this.parent.CharaAnimeFlame = 0;
                }
                if (this.parent.moveOrder.Length == 0)
                {
                    if ((this.hitrange.X != 0 || this.hitrange.Y != 0) && this.move != null)
                    {
                        if ((uint)this.move.Length > 0U)
                        {
                            if (this.movingOrder >= this.move.Length)
                                this.movingOrder = 0;
                            this.move[this.movingOrder].Move(speed);
                            if (this.move[this.movingOrder].MoveEnd())
                                ++this.movingOrder;
                        }
                        else if (!this.parent.parent.eventmanager.playevent && this.parent.Angle != this.defaultAngle)
                            this.parent.Angle = this.defaultAngle;
                    }
                }
                else if (this.parent.movingOrder >= this.parent.moveOrder.Length)
                {
                    this.parent.movingOrder = 0;
                    this.parent.moveOrder = new EventMove[0];
                    if (this.parent.CharaAnimeFlame > 0)
                        this.parent.CharaAnimeFlame = 0;
                }
                else
                {
                    this.parent.moveOrder[this.parent.movingOrder].Move(speed);
                    if (this.parent.moveOrder[this.parent.movingOrder].MoveEnd())
                        ++this.parent.movingOrder;
                }
            }
        }

        public EventPage Clone(MapEventBase p)
        {
            EventPage eventPage = new EventPage(this.sound, p, this.savedate)
            {
                character = this.character
            };
            foreach (EventBase eventBase in this.eventmanager.events)
                eventPage.eventmanager.events.Add(eventBase);
            eventPage.graphicName = this.graphicName;
            eventPage.graphicNo = this.graphicNo;
            eventPage.graphicName = this.graphicName;
            eventPage.hitform = this.hitform;
            eventPage.hitrange = this.hitrange;
            eventPage.hitShift = this.hitShift;
            List<EventMove> eventMoveList = new List<EventMove>();
            foreach (EventMove eventMove in this.move)
                eventMoveList.Add(eventMove.Clone(p));
            eventPage.move = eventMoveList.ToArray();
            List<None> noneList = new List<None>();
            foreach (None pageStartterm in this.pageStartterms)
                noneList.Add(pageStartterm);
            eventPage.pageStartterms = noneList.ToArray();
            eventPage.rendType = this.rendType;
            eventPage.speed = this.speed;
            eventPage.startterms = this.startterms;
            eventPage.rebirth = this.rebirth;
            return eventPage;
        }

        public bool LunCheck()
        {
            bool flag = true;
            foreach (None pageStartterm in this.pageStartterms)
            {
                if (!pageStartterm.YesNo(this.savedate))
                    flag = false;
            }
            this.lun = flag;
            return flag;
        }

        public void ParentSet(MapEventBase eb)
        {
            this.parent = eb;
        }

        public void AddEvent(EventBase event_)
        {
            this.eventmanager.events.Add(event_);
        }

        public enum STARTTERMS
        {
            Abutton,
            Rbutton,
            Touch,
            Auto,
            none,
            parallel,
        }
    }
}
