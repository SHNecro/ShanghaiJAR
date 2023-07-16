using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using NSMap.Character;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSEvent
{
    public class EventManager : AllBase
    {
        private int playeventnumber = 0;
        public List<EventBase> events = new List<EventBase>();
        public bool playevent = false;
        public int alpha = 0;
        private bool endflag;
        public SceneMap parent;
        private bool noTimeNext;
        public bool skip;
        public bool canSkip;
        private bool endSkip;
        private bool resetFlag;
        public Color skipColor;
        public MapEventBase eventCharacter;
        public bool skipnow;

        public int Playeventnumber
        {
            get
            {
                return this.playeventnumber;
            }
            set
            {
                this.playeventnumber = value;
            }
        }

        public bool NoWait
        {
            get
            {
                bool flag = true;
                foreach (EventBase eventBase in this.events)
                {
                    if (!eventBase.NoTimeNext)
                    {
                        flag = false;
                        break;
                    }
                }
                return flag;
            }
        }

        public EventManager(IAudioEngine s)
          : base(s)
        {
        }

        public EventManager(SceneMap m, IAudioEngine s)
          : base(s)
        {
            this.parent = m;
        }

        public void UpDate()
        {
            if (this.playeventnumber <= this.events.Count - 1)
            {
                if (this.endflag && !this.skipnow)
                    this.EndEvent();
            }
            if (this.endSkip)
            {
                if (this.alpha > 0)
                {
                    this.alpha -= 4;
                }
                else
                {
                    this.alpha = 0;
                    this.endSkip = false;
                }
            }
            else
            {
                if (this.playeventnumber > this.events.Count - 1)
                {
                    this.playevent = false;
                    if (this.eventCharacter != null)
                    {
                        this.eventCharacter.stop = false;
                        this.skipnow = false;
                    }
                    this.Init();
                }
                else
                {
                    do
                    {
                        if (this.alpha < byte.MaxValue || !this.skipnow)
                        {
                            if (this.noTimeNext)
                            {
                                this.noTimeNext = false;
                                ++this.playeventnumber;
                                if (this.playeventnumber >= this.events.Count)
                                {
                                    if (this.eventCharacter != null)
                                    {
                                        this.eventCharacter.stop = false;
                                        this.skipnow = false;
                                        break;
                                    }
                                    break;
                                }
                            }
                            if (!this.skip || !this.endflag)
                                this.events[this.playeventnumber].Update();
                            if (this.canSkip && !this.skip && Input.IsPush(Button._Select))
                                this.SkipMode();
                        }
                        else
                        {
                            this.noTimeNext = false;

                            // Allow cleanup of events after skipping blackout
                            var effects = this.parent?.Field?.effect;
                            foreach (var effect in effects)
                            {
                                effect?.effect?.SkipUpdate();
                            }

                            if (this.playeventnumber >= this.events.Count || this.events[this.playeventnumber] is Battle || this.events[this.playeventnumber] is StopSkip)
                            {
                                if (this.eventCharacter != null)
                                {
                                    this.eventCharacter.stop = false;
                                    this.skipnow = false;
                                }
                                this.skipnow = false;
                                this.endSkip = true;
                                if (this.events[this.playeventnumber] is Battle)
                                {
                                    this.resetFlag = true;
                                    break;
                                }
                                break;
                            }
                            ++this.playeventnumber;
                            if (this.playeventnumber >= this.events.Count)
                            {
                                if (this.eventCharacter != null)
                                {
                                    this.eventCharacter.stop = false;
                                    this.skipnow = false;
                                    break;
                                }
                                break;
                            }
                            this.events[this.playeventnumber].SkipUpdate();
							if (this.events[this.playeventnumber] is Battle)
							{
								this.skipColor = Color.Black;
							}
						}
                    }
                    while (this.noTimeNext);
                }

                // Skip started
                if (this.skip)
                {
                    this.alpha += 5;
                    if (this.alpha > byte.MaxValue)
                    {
                        this.alpha = byte.MaxValue;
                        this.skip = false;
                        this.skipnow = true;
                        this.events[this.playeventnumber - 1].SkipUpdate();
                        this.events[this.playeventnumber].FlameReset();
                    }
                }
            }

            if (this.resetFlag)
            {
                this.alpha = 0;
                this.canSkip = false;
                this.skip = false;
                this.resetFlag = false;
                this.skipColor = Color.Black;
            }
        }

        public void Render(IRenderer dg)
        {
            if (this.playevent && this.playeventnumber < this.events.Count)
            {
                this.events[this.playeventnumber].Render(dg);
            }
            if (this.alpha <= 0)
                return;
            Color color = Color.FromArgb(this.alpha, this.skipColor);
            Rectangle _rect = new Rectangle(0, 0, 240, 160);
            Vector2 _point = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", _rect, true, _point, color);
        }

        // Skip pressed
        public void SkipMode()
        {
            this.skip = true;

            // If skipping to battle
            if (this.skipColor != Color.Black)
            {
                this.sound.PlaySE(SoundEffect.encount);
            }
        }

        public void Init()
        {
            this.playeventnumber = 0;
        }

        public void NextEvent()
        {
            this.endflag = true;
        }

        public void NoTimeNext()
        {
            this.noTimeNext = true;
        }

        public void EndEvent()
        {
            this.events[this.playeventnumber].FlameReset();
            if (!this.skip)
            {
                ++this.playeventnumber;
            }
            if (this.playeventnumber > this.events.Count)
                this.playevent = false;
            this.endflag = false;
        }

        public void NextEventNoplus()
        {
            if (this.playeventnumber <= this.events.Count)
                return;
            this.playevent = false;
        }

        public void ClearEvent()
        {
            this.events.Clear();
            this.playevent = false;
            this.playeventnumber = 0;
        }

        public void AddEvent(EventBase e)
        {
            this.events.Add(e);
            if (this.playevent)
                return;
            this.playevent = true;
        }

        public void EventClone(EventManager host)
        {
            this.events = new List<EventBase>();
            foreach (EventBase eventBase in host.events)
                this.events.Add(eventBase);
            foreach (EventBase eventBase in this.events)
                eventBase.ManagerChange(this);
        }

        public void EventPass(EventManager host)
        {
            foreach (EventBase eventBase in host.events)
                this.events.Add(eventBase);
            foreach (EventBase eventBase in this.events)
                eventBase.ManagerChange(this);
        }
    }
}
