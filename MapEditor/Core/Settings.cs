using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace MapEditor.Core
{
    [Serializable]
    [Obfuscation(Exclude = true)]
    public class Settings : ICloneable
    {
        public string InitialMap { get; set; } = "debugroom1.she";
        public string MapDataFolder { get; set; } = "data";

        public string GraphicsFormat { get; set; } = "ShaG/{0}.png";
        public string GraphicsResourceFile { get; set; } = "ShaGResource.tcd";
        public string GraphicsResourceFilePassword { get; set; } = "sasanasi";
        public string GraphicsResourceFileFormat { get; set; } = "{0}.png";
        public bool GraphicsIsPackedResource { get; set; } = true;

        [OptionalField(VersionAdded = 2)]
        public string soundFormat = "ShaS/{0}.wav";
        [OptionalField(VersionAdded = 2)]
        public string soundResourceFile = "ShaSResource.tcd";
        [OptionalField(VersionAdded = 2)]
        public string soundResourceFilePassword = "sasanasi";
        [OptionalField(VersionAdded = 2)]
        public string soundResourceFileFormat = "{0}.wav";
        [OptionalField(VersionAdded = 2)]
        public bool soundIsPackedResource = true;

        public string SoundFormat { get => this.soundFormat; set => this.soundFormat = value; }
        public string SoundResourceFile { get => this.soundResourceFile; set => this.soundResourceFile = value; }
        public string SoundResourceFilePassword { get => this.soundResourceFilePassword; set => this.soundResourceFilePassword = value; }
        public string SoundResourceFileFormat { get => this.soundResourceFileFormat; set => this.soundResourceFileFormat = value; }
        public bool SoundIsPackedResource { get => this.soundIsPackedResource; set => this.soundIsPackedResource = value; }

        public int EnemyCount { get; set; } = 86;
        public int ChipCount { get; set; } = 431;
        public int AddOnCount { get; set; } = 98;
        public int InteriorCount { get; set; } = 53;
        public int BackgroundCount { get; set; } = 41;

        public void ToXML(string filename)
        {
            var serializer = new XmlSerializer(typeof(Settings));
            using (var writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, this);
            }
        }

        public static Settings FromXML(string filename)
        {
            try
            {
                using (var configReader = new StreamReader(filename))
                {
                    var deserializer = new XmlSerializer(typeof(Settings));
                    var newConfig = (Settings)deserializer.Deserialize(configReader);
                    return newConfig;
                }
            }
            catch
            {
                return null;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
