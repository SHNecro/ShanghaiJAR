using MapEditor.Core;
using MapEditor.Models.Elements.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace MapEditor.Models.Elements.Events
{
    public class EffectEvent : EventBase
    {
        private readonly Timer reselectTimer;

        private int effectNumber;
        private string id;
        private int x;
        private int y;
        private int z;
        private string targetName;
        private int locationType;
        private int interval;
        private int randomXY;
        private int rendType;
        private string soundEffect;

        public EffectEvent()
        {
            this.reselectTimer = new Timer { Interval = 100, AutoReset = false, Enabled = false };
            this.reselectTimer.Elapsed += this.ReloadSoundEffects;
        }

        public int EffectNumber
        {
            get { return this.effectNumber; }
            set { this.SetValue(ref this.effectNumber, value); }
        }

        public string ID
        {
            get { return this.id; }
            set { this.SetValue(ref this.id, value); }
        }

        public int X
        {
            get { return this.x; }
            set { this.SetValue(ref this.x, value); }
        }

        public int Y
        {
            get { return this.y; }
            set { this.SetValue(ref this.y, value); }
        }

        public int Z
        {
            get { return this.z; }
            set { this.SetValue(ref this.z, value); }
        }

        public string TargetName
        {
            get { return this.targetName; }
            set { this.SetValue(ref this.targetName, value); }
        }

        public int LocationType
        {
            get { return this.locationType; }
            set { this.SetValue(ref this.locationType, value); }
        }

        public int Interval
        {
            get { return this.interval; }
            set { this.SetValue(ref this.interval, value); }
        }

        public int RandomXY
        {
            get { return this.randomXY; }
            set { this.SetValue(ref this.randomXY, value); }
        }

        public int RendType
        {
            get { return this.rendType; }
            set { this.SetValue(ref this.rendType, value); }
        }

        public string SoundEffect
        {
            get
            {
                return this.soundEffect;
            }
            set
            {
                if (value == null)
                {
                    this.reselectTimer.Stop();
                    this.reselectTimer.Start();
                }
                else
                {
                    this.SetValue(ref this.soundEffect, value);
                }
            }
        }

        public ICommand PlayCommand => new RelayCommand(this.PlaySoundEffectCommand);

        public ICommand StopCommand => new RelayCommand(this.StopSoundEffectCommand);

        public override string Info => "Creates an effect at an object or a set of coordinates.";

        public override string Name
        {
            get
            {
                var idString = string.IsNullOrEmpty(this.ID) ? string.Empty : $" \"{this.ID}\"";
                var effectString = ((EffectTypeNumber)this.EffectNumber).ToString();
                var locationString = default(string);
                switch ((EffectLocationTypeNumber)this.LocationType)
                {
                    case EffectLocationTypeNumber.Position:
                        locationString = $"({this.X},{this.Y},{this.Z})";
                        break;
                    case EffectLocationTypeNumber.Variable:
                        locationString = $"(var[{this.X}],var[{this.Y}],var[{this.Z}])";
                        break;
                    case EffectLocationTypeNumber.ParentObject:
                        locationString = $"Here";
                        break;
                    case EffectLocationTypeNumber.OtherObject:
                        locationString = $"\"{this.TargetName}\"";
                        break;
                }
                return $"Effect{idString}: {effectString} at {locationString}";
            }
        }

        protected override string GetStringValue()
        {
            var entries = new List<string> { "effect", $"{this.EffectNumber}", this.ID };
            if (this.LocationType < 3)
            {
                entries.Add($"{this.X}");
                entries.Add($"{this.Y}");
                entries.Add($"{this.Z}");
            }
            else
            {
                entries.Add($"{this.TargetName}");
                entries.Add($"{string.Empty}");
                entries.Add($"{string.Empty}");
            }
            entries.Add($"{this.LocationType}");
            entries.Add($"{this.Interval}");
            entries.Add($"{this.RandomXY}");
            entries.Add($"{this.RendType}");
            entries.Add($"{this.SoundEffect}");

            return string.Join(":", entries);
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed effect event \"{value}\".", e => e.Length == 11 && e[0] == "effect"))
            {
                return;
            }

            var newEffectNumber = this.ParseIntOrAddError(entries[1]);
            this.ParseEnumOrAddError<EffectTypeNumber>(entries[1]);

            var newID = entries[2];
            var newLocationType = this.ParseIntOrAddError(entries[6]);
            this.ParseEnumOrAddError<EffectLocationTypeNumber>(entries[6]);

            int newX, newY, newZ;
            newX = newY = newZ = default(int);
            var newName = default(string);

            if (newLocationType < 3)
            {
                newX = this.ParseIntOrAddError(entries[3]);
                newY = this.ParseIntOrAddError(entries[4]);
                newZ = this.ParseIntOrAddError(entries[5]);
            }
            else
            {
                newName = entries[3];
            }

            var newInterval = this.ParseIntOrAddError(entries[7]);
            var newRandomXY = this.ParseIntOrAddError(entries[8]);
            var newRendType = this.ParseIntOrAddError(entries[9]);
            this.ParseEnumOrAddError<RenderTypeNumber>(entries[9]);
            var newSoundEffect = entries[10];
            this.Validate(newSoundEffect, () => this.SoundEffect, s => $"Sound ({s}.wav) not found", s => s == "none" || (Constants.SoundLoadStrategy?.CanProvideFile(s) ?? true));

            this.EffectNumber = newEffectNumber;
            this.ID = newID;
            this.X = newX;
            this.Y = newY;
            this.Z = newZ;
            this.TargetName = newName;
            this.LocationType = newLocationType;
            this.Interval = newInterval;
            this.RandomXY = newRandomXY;
            this.RendType = newRendType;
            this.SoundEffect = newSoundEffect;
        }

        private void PlaySoundEffectCommand()
        {
            if (this.SoundEffect == "none")
            {
                return;
            }

            Constants.AudioEngine.WavPlay(Constants.SoundLoadStrategy.ProvideSound(this.SoundEffect));
        }

        private void StopSoundEffectCommand()
        {
            Constants.AudioEngine.WavStop();
        }

        private void ReloadSoundEffects(object sender, ElapsedEventArgs args)
        {
            var originalSoundEffect = this.SoundEffect;
            this.SoundEffect = Constants.SoundEffects.LastOrDefault();
            this.SoundEffect = Constants.SoundEffects[Constants.SoundEffects.IndexOf(originalSoundEffect)];
        }
    }
}
