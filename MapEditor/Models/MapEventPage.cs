using MapEditor.Core;
using MapEditor.ExtensionMethods;
using MapEditor.Models.Elements.Enums;
using MapEditor.Models.Elements.Events;
using NSMap.Character;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace MapEditor.Models
{
    public class MapEventPage : StringRepresentation
    {
        private static readonly Regex GraphicsIndexRegex = new Regex(@"(?:(?:body)|(?:charachip))(\d+)", RegexOptions.Compiled);

        private int pageNumber;
        private int graphicsIndex;
        private int texX;
        private int texY;
        private int texW;
        private int texH;
        private int frames;
        private int angle;
        private int speed;
        private Size hitRange;
        private Point hitShift;
        private int characterIndex;
        private bool isCharacter;
        private HitFormType hitForm;
        private StartTermType startTerms;
        private MoveCollection moves;
        private TermCollection terms;
        private EventCollection events;
        private int rendType;

        //private bool movesExpanded;
        //private bool termsExpanded;
        private bool eventsExpanded;

        public int PageNumber
        {
            get
            {
                return this.pageNumber;
            }

            set
            {
                if (!this.UpdatePageNumberAction?.Invoke(this.pageNumber - 1, value - 1) ?? false)
                {
                    return;
                }
                this.SetValue(ref this.pageNumber, value);
            }
        }

        public int GraphicsIndex
        {
            get { return this.graphicsIndex; }
            set { this.SetValue(ref this.graphicsIndex, value); }
        }
        public int TexX
        {
            get { return this.texX; }
            set { this.SetValue(ref this.texX, value); }
        }
        public int TexY
        {
            get { return this.texY; }
            set { this.SetValue(ref this.texY, value); }
        }
        public int TexW
        {
            get { return this.texW; }
            set { this.SetValue(ref this.texW, value); }
        }
        public int TexH
        {
            get { return this.texH; }
            set { this.SetValue(ref this.texH, value); }
        }
        public int Frames
        {
            get { return this.frames; }
            set { this.SetValue(ref this.frames, value); }
        }

        public bool IsCharacter
        {
            get
            {
                return isCharacter;
            }

            set
            {
                isCharacter = value;
                this.CharacterIndex = 0;
                this.Angle = 1;
            }
        }
        public int CharacterIndex
        {
            get
            {
                return characterIndex;
            }

            set
            {
                bool angleUp = this.Angle == 3 || this.Angle == 5;
                characterIndex = value;
                this.TexX = (this.CharacterIndex < 4 ? 0 : 448) + 0;
                this.TexY = (this.CharacterIndex % 4) * 192 + (angleUp ? 96 : 0);
                this.OnPropertyChanged(nameof(this.CharacterIndex));
            }
        }
        public int Angle
        {
            get
            {
                return angle;
            }

            set
            {
                angle = value;
                bool angleUp = this.Angle == 3 || this.Angle == 5;
                this.TexY = (this.CharacterIndex % 4) * 192 + (angleUp ? 96 : 0);
                this.OnPropertyChanged(nameof(this.Angle));
            }
        }

        public int Speed
        {
            get { return this.speed; }
            set { this.SetValue(ref this.speed, value); }
        }

        public Size HitRange
        {
            get { return this.hitRange; }
            set { this.SetValue(ref this.hitRange, value); }
        }
        public int HitRangeWidth
        {
            get { return this.HitRange.Width; }
            set { this.HitRange = new Size(value, this.HitRange.Height); }
        }
        public int HitRangeHeight
        {
            get { return this.HitRange.Height; }
            set { this.HitRange = new Size(this.HitRange.Width, value); }
        }
        public int HitRangeRadius
        {
            get { return this.HitRange.Width; }
            set
            {
                this.HitRange = new Size(value, value);
            }
        }

        public Point HitShift
        {
            get { return this.hitShift; }
            set { this.SetValue(ref this.hitShift, value); }
        }
        public int HitShiftX
        {
            get { return this.HitShift.X; }
            set { this.HitShift = new Point(value, this.HitShift.Y); }
        }
        public int HitShiftY
        {
            get { return this.HitShift.Y; }
            set { this.HitShift = new Point(this.HitShift.X, value); }
        }

        public HitFormType HitForm
        {
            get { return this.hitForm; }
            set { this.SetValue(ref this.hitForm, value); }
        }

        public StartTermType StartTerms
        {
            get { return this.startTerms; }
            set { this.SetValue(ref this.startTerms, value); }
        }
        public MoveCollection Moves
        {
            get
            {
                return this.moves;
            }
            set
            {
                if (this.moves != null)
                {
                    this.moves.PropertyChanged -= this.OnMovesPropertyChanged;
                }
                value.PropertyChanged += this.OnMovesPropertyChanged;
                this.SetValue(ref this.moves, value);
            }
        }

        public TermCollection Terms
        {
            get
            {
                return this.terms;
            }
            set
            {
                if (this.terms != null)
                {
                    this.terms.PropertyChanged -= this.OnTermsPropertyChanged;
                }
                value.PropertyChanged += this.OnTermsPropertyChanged;
                this.SetValue(ref this.terms, value);
            }
        }

        public EventCollection Events
        {
            get
            {
                return this.events;
            }
            set
            {
                if (this.events != null)
                {
                    this.events.PropertyChanged -= this.OnEventsPropertyChanged;
                }
                value.PropertyChanged += this.OnEventsPropertyChanged;
                this.SetValue(ref this.events, value);
            }
        }

        public int RendType
        {
            get { return this.rendType; }
            set { this.SetValue(ref this.rendType, value); }
        }

        // TODO: Figure out real equation for warp hitboxes
        public bool IsWarp => this.Events.Events.Any(eb =>
        {
            var e = eb.Instance;
            return e is MapChangeEvent || e is MapWarpEvent || e is WarpEvent || e is WarpPlugOutEvent;
        });

        //public bool MovesExpanded
        //{
        //    get { return this.movesExpanded; }
        //    set { this.SetValue(ref this.movesExpanded, value); }
        //}

        //public bool TermsExpanded
        //{
        //    get { return this.termsExpanded; }
        //    set { this.SetValue(ref this.termsExpanded, value); }
        //}

        public bool EventsExpanded
        {
            get { return this.eventsExpanded; }
            set { this.SetValue(ref this.eventsExpanded, value); }
        }

        public Func<int, int, bool> UpdatePageNumberAction { get; set; }

        public string Texture => this.IsCharacter ? $"charachip{this.GraphicsIndex}" : $"body{this.GraphicsIndex}";

        protected override string GetStringValue()
        {
            var gameStartTerm = (EventPage.STARTTERMS)(int)this.StartTerms;

            var termsString = this.Terms.StringValue;
			var movesString = this.Moves.StringValue;
            var graphicString = default(string);
            if (this.IsCharacter)
            {
                graphicString = $"{this.GraphicsIndex}:{this.CharacterIndex},0,{this.Angle},0,0";
            }
            else
            {
                graphicString = $"{-this.GraphicsIndex}:{this.TexX},{this.TexY},{this.TexW},{this.TexH},{this.Frames}";
            }
            var hitRangeByForm = false /*this.HitForm == HitFormType.Circle*/ ? $"{this.HitRange.Width}:{this.HitRange.Width}" : $"{this.HitRange.Width}:{this.HitRange.Height}";
            var hitRangeString = $"{hitRangeByForm}:{this.HitShift.X}:{this.HitShift.Y}";
            var hitFormString = this.HitForm == HitFormType.Circle ? "circle" : "square";
            var eventsString = this.Events.StringValue;
            return string.Join("\r\n", new[]
            {
               $"page:{this.PageNumber}",
               $"startterms:{gameStartTerm}",
               $"type:{this.RendType}",
               $"terms:{termsString}",
               $"move:{movesString}",
               $"speed:{this.Speed}",
               $"graphic:{graphicString}",
               $"hitrange:{hitRangeString}",
               $"hitform:{hitFormString}",
                "event:",
                eventsString,
                "end",
            }.Where(s => !string.IsNullOrEmpty(s)));
        }

        protected override void SetStringValue(string value)
        {
            var eventStrings = new List<string>();
            var pageStrings = new Dictionary<string, string>();
            using (var reader = new StringReader(value))
            {
                string line;
                var eventNesting = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "event:")
                    {
                        eventNesting++;
                    }
                    else if (line == "end")
                    {
                        eventNesting--;
                    }
                    else if (eventNesting == 0)
                    {
                        var lineEntries = line.Split(new[] { ':' }, 2);
                        if (this.Validate(lineEntries, $"Malformed event page line \"{line}\".", le => le.Length == 2))
                        {
                            pageStrings[lineEntries[0]] = lineEntries[1];
                        }
                    }
                    else
                    {
                        eventStrings.Add(line);
                    }
                }
            }

            if (!pageStrings.TryGetValue("page", out string newPageNumberString))
            {
                this.Validate(0, "Missing page number line.", x => false);
            }
            this.PageNumber = this.ParseIntOrAddError(newPageNumberString);

            if (!pageStrings.TryGetValue("graphic", out string graphicsString))
            {
                this.Validate(0, "Missing graphics line.", x => false);
            }
            int newGraphicsIndex = 0;
            string[] spriteEntries = null;
            var graphicEntries = graphicsString.Split(':');
            if (this.Validate(graphicEntries, $"Malformed graphics line \"{graphicsString}\".", ge => ge.Length == 2))
            {
                newGraphicsIndex = ParseIntOrAddError(graphicEntries[0]);
                spriteEntries = graphicEntries[1].Split(',');
            }
            int newCharacterIndex, newTexX, newTexY, newTexW, newTexH, texFrames;
            newCharacterIndex = newTexX = newTexY = newTexW = newTexH = texFrames = 0;
            if (this.Validate(spriteEntries, "Missing graphics positions", se => se.Length == 5))
            {
                newTexX = ParseIntOrAddError(spriteEntries[0]);
                newTexY = ParseIntOrAddError(spriteEntries[1]);
                newTexW = ParseIntOrAddError(spriteEntries[2]);
                newTexH = ParseIntOrAddError(spriteEntries[3]);
                texFrames = ParseIntOrAddError(spriteEntries[4]);
            }

            var newIsCharacter = newGraphicsIndex > 0;
            var newAngle = 0;

            if (newIsCharacter)
            {
                // 0: down-right, 1: up-right, 2: up-left, 3: down-left
                newAngle = this.ParseIntOrAddError(spriteEntries[2]);
                bool angleUp = newAngle == 3 || newAngle == 5;
                newCharacterIndex = newTexX;
                newTexX = (newCharacterIndex < 4 ? 0 : 448) + 0;
                newTexY = (newCharacterIndex % 4) * 192 + (angleUp ? 96 : 0);
            }

            if (!pageStrings.TryGetValue("speed", out string speedString))
            {
                this.Validate(0, "Missing speed line.", x => false);
            }
            var newSpeed = this.ParseIntOrAddError(speedString);

            if (!pageStrings.TryGetValue("move", out string moveString))
            {
                this.Validate(0, "Missing move line.", x => false);
            }
			var newMoves = new MoveCollection { StringValue = moveString };

            if (!pageStrings.TryGetValue("hitrange", out string hitRangeString))
            {
                this.Validate(0, "Missing hitrange line.", x => false);
            }
            var newHitRange = default(Size);
            var newHitShift = default(Point);
            var hitRangeEntries = hitRangeString.Split(':');
            if (this.Validate(hitRangeEntries, "Malformed hitrange line.", hre => hre.Length == 4))
            {
                newHitRange = new Size(
                    ParseIntOrAddError(hitRangeEntries[0]),
                    ParseIntOrAddError(hitRangeEntries[1])
                );
                newHitShift = new Point(
                    ParseIntOrAddError(hitRangeEntries[2]),
                    ParseIntOrAddError(hitRangeEntries[3])
                );
            }

            if (!pageStrings.TryGetValue("hitform", out string hitFormString))
            {
                this.Validate(0, "Missing hitform line.", x => false);
            }
            var newHitForm = hitFormString == "square" ? HitFormType.Square : HitFormType.Circle;

            if (!pageStrings.TryGetValue("type", out string typeString))
            {
                this.Validate(0, "Missing type line.", x => false);
            }
            var type = ParseIntOrAddError(typeString);

            if (!pageStrings.TryGetValue("startterms", out string startTermsString))
            {
                this.Validate(0, "Missing startterms line.", x => false);
            }
            var gameStartTerm = this.ParseEnumOrAddError<EventPage.STARTTERMS>(startTermsString);
            var newStartTerms = (StartTermType)(int)gameStartTerm;

            if (!pageStrings.TryGetValue("terms", out string termsString))
            {
                this.Validate(0, "Missing terms line.", x => false);
            }
            var newTerms = new TermCollection { StringValue = termsString };

            var newEvents = new EventCollection { StringValue = string.Join("\r\n", eventStrings) };

            this.GraphicsIndex = Math.Abs(newGraphicsIndex);

            this.IsCharacter = newIsCharacter;
            if (this.IsCharacter)
            {
                this.CharacterIndex = newCharacterIndex;
                this.Angle = newAngle;
            }
            else
            {
                this.TexX = newTexX;
                this.TexY = newTexY;
                this.TexW = newTexW;
                this.TexH = newTexH;
                this.Frames = texFrames;
            }

            this.Speed = newSpeed;
            this.Moves = newMoves;

            this.HitRange = newHitRange;
            this.HitShift = newHitShift;
            this.HitForm = newHitForm;

            this.RendType = type;

            this.StartTerms = newStartTerms;
            this.Terms = newTerms;

            this.Events = newEvents;
        }

        protected override ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors()
        {
            return new ObservableCollection<Tuple<StringRepresentation[], string>>(new[]
            {
                this.UpdateChildErrorStack(Moves),
                this.UpdateChildErrorStack(Terms),
                this.UpdateChildErrorStack(Events)
            }.SelectMany(oc => oc));
        }

        private void OnMovesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"Moves.{e.PropertyName}");
        }

        private void OnTermsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"Terms.{e.PropertyName}");
        }

        private void OnEventsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"Events.{e.PropertyName}");
        }
    }
}
