using MapEditor;
using MapEditor.Core;
using MapEditor.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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

            var errors = new List<Tuple<StringRepresentation[], string>>();
            foreach (var file in mapDataDir.GetFiles("*.shd"))
            {
                using (var sr = new StreamReader(file.OpenRead()))
                {
                    var map = new Map { StringValue = sr.ReadToEnd() };
                    foreach (var error in map.Errors)
                    {
                        errors.Add(Tuple.Create(error.Item1, $"{file.FullName} - {error.Item2}"));
                    }
                }
            }

            Assert.AreEqual(0, errors.Count);
        }
    }
}
