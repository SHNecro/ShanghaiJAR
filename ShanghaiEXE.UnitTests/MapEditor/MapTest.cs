using MapEditor;
using MapEditor.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Tests.MapEditor
{
    [TestClass]
    public class MapTest
    {
        [TestMethod]
        public void LoadMap_AllExistingMaps_NoErrors()
        {
            typeof(Constants).GetMethod("InitializeLanguage", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);

            var mapDataDir = new DirectoryInfo("MapData");

            var errors = new List<string>();
            foreach (var file in mapDataDir.GetFiles("*.shd"))
            {
                using (var sr = new StreamReader(file.OpenRead()))
                {
                    var map = new Map { StringValue = sr.ReadToEnd() };
                    foreach (var error in map.Errors)
                    {
                        errors.Add($"{file.FullName} - {error}");
                    }
                }
            }

            Assert.AreEqual(0, errors.Count);
        }
    }
}
