using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace MapEditor.Core
{
    [Serializable]
    [Obfuscation(Exclude = true)]
    public class Settings : ICloneable
    {
        public string InitialMap { get; set; } = "exOmake.she";
        public string MapDataFolder { get; set; } = "data";
        public string GraphicsFormat { get; set; } = "ShaG/{0}.png";
        public string GraphicsResourceFile { get; set; } = "ShaGResource.tcd";
        public string GraphicsResourceFilePassword { get; set; } = "sasanasi";
        public string GraphicsResourceFileFormat { get; set; } = "{0}.png";

        public int EnemyCount { get; set; } = 85;
        public int ChipCount { get; set; } = 430;
        public int AddOnCount { get; set; } = 95;
        public int InteriorCount { get; set; } = 51;
        public int BackgroundCount { get; set; } = 39;

        public bool UsesPackedResources { get; set; } = true;

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
