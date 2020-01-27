using Common.EncodeDecode;
using MapEditor.Core;
using MapEditor.Core.Converters;
using MapEditor.Models;
using MapEditor.Models.Elements.Events;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MapEditor.ViewModels
{
    public class DebuggerHookWindowViewModel : ViewModelBase
    {
        private List<Map> allMaps = new List<Map>();

        private double mapLoadProgress;
        private string mapLoadProgressLabel;

        public double MapLoadProgress
        {
            get { return this.mapLoadProgress; } 
            set { this.SetValue(ref this.mapLoadProgress, value); }
        }

        public string MapLoadProgressLabel
        {
            get { return this.mapLoadProgressLabel; }
            set { this.SetValue(ref this.mapLoadProgressLabel, value); }
        }

        public string Operation { get; set; }

        public ICommand LoadAllMapsCommand => new RelayCommand(this.LoadAllMaps);
        public ICommand BreakpointCommand => new RelayCommand(this.Breakpoint);

        private void LoadAllMaps()
        {
            Task.Run(() =>
            {
                this.allMaps.Clear();

                var mapFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\data\\", "*.she");

                var mapsLoaded = 0.0;
                foreach (var map in mapFiles)
                {
                    this.MapLoadProgressLabel = Path.GetFileNameWithoutExtension(map);
                    this.allMaps.Add(this.LoadMap(map, true));

                    mapsLoaded++;
                    this.MapLoadProgress = mapsLoaded / mapFiles.Length;
                }
                this.MapLoadProgressLabel = "Complete";
                this.MapLoadProgress = 1.0;
            });
        }

        private void Breakpoint()
        {
            if (this.Operation == "Chips")
            {
                this.DumpChips();
            }
        }

        private void DumpChips()
        {
            var gmdChips = this.allMaps
                .SelectMany(m => m.RandomMysteryData.RandomMysteryData
                    .Select(gmd => gmd.Chip)
                .Select(c => Tuple.Create(c, m.Header.TitleKey, "GMD")))
                .Where(c => c.Item1 != null).ToArray();
            var virusChips = this.allMaps
                .SelectMany(m => m.RandomEncounters.RandomEncounters
                    .SelectMany(re => re.Enemies
                        .SelectMany(e => new[] { e.Chip1, e.Chip2, e.Chip3, e.Chip4, e.Chip5 })
                .Select(c => Tuple.Create(c, m.Header.TitleKey, "Encounter"))))
                .Where(c => c.Item1 != null).ToArray();
            var bmdpmdChips = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .OfType<MapMystery>()
                    .Select(mm => mm.BaseMystery.Chip)
                .Select(c => Tuple.Create(c, m.Header.TitleKey, "BMD/PMD")))
                .Where(c => c.Item1 != null).ToArray();
            var battleChips = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Where(me => me.Category == Models.Elements.Enums.EventCategoryOption.Battle)
                            .Where(me => (me.Instance as BattleEvent).Encounter.IsChipDropped)
                            .SelectMany(me => (me.Instance as BattleEvent).Encounter.Enemies
                                .SelectMany(e => new[] { e.Chip1, e.Chip2, e.Chip3, e.Chip4, e.Chip5 })
                .Select(c => Tuple.Create(c, m.Header.TitleKey, "Scripted Encounter"))))))
                .Where(c => c.Item1 != null).ToArray();
            var givenChipGetChips = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Where(me => me.Category == Models.Elements.Enums.EventCategoryOption.ChipGet)
                            .Where(me => (me.Instance as ChipGetEvent).IsAdding)
                            .Select(me => (me.Instance as ChipGetEvent).Chip)
                .Select(c => Tuple.Create(c, m.Header.TitleKey, "Event (ChipGet)")))))
                .Where(c => c.Item1 != null).ToArray();
            var givenItemGetChips = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Where(me => me.Category == Models.Elements.Enums.EventCategoryOption.ItemGet)
                            .Select(me => (me.Instance as ItemGetEvent).Mystery.Chip)
                .Select(c => Tuple.Create(c, m.Header.TitleKey, "Event (ItemGet)")))))
                .Where(c => c.Item1 != null).ToArray();
            var shopChips = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Where(me => me.Category == Models.Elements.Enums.EventCategoryOption.Shop)
                            .SelectMany(me => (me.Instance as ShopEvent).ShopItems.ShopItems
                                .Select(si => si.Chip))
                .Select(c => Tuple.Create(c, m.Header.TitleKey, "Event (Shop)")))))
                .Where(c => c.Item1 != null).ToArray();

            var allAccessibleChips = new[] { gmdChips, virusChips, bmdpmdChips, battleChips, givenChipGetChips, givenItemGetChips, shopChips }.SelectMany(tup => tup);
            var fullChips = allAccessibleChips.Select(c => Tuple.Create(
                    c.Item1,
                    Constants.ChipDefinitions.ContainsKey(c.Item1.ID) ? Constants.ChipDefinitions[c.Item1.ID] : null,
                    Constants.TranslationService.Translate(c.Item2).Text,
                    c.Item3))
                .Where(c => c.Item2 != null)
                .ToList();

            var chipCodesMultiConverter = new ChipCodesMultiConverter();
            var chipLocations = fullChips.OrderBy(c => c.Item3).ThenBy(c => c.Item4).ThenBy(c => $"{c.Item2.Name} {chipCodesMultiConverter.Convert(new object[] { c.Item2.Codes, c.Item1.CodeNumber }, null, null, null)}")
                .Select(c => $"{c.Item3}\t{c.Item4}\t{c.Item2.Name} {chipCodesMultiConverter.Convert(new object[] { c.Item2.Codes, c.Item1.CodeNumber }, null, null, null)}")
                .ToArray();
            var contents = string.Join("\n", chipLocations);

            var accessibleChipStrings = fullChips.Select(c => $"{c.Item2.Name} {chipCodesMultiConverter.Convert(new object[] { c.Item2.Codes, c.Item1.CodeNumber }, null, null, null)}")
                .Distinct().ToArray();
            var everyChipString = Constants.ChipDefinitions.Values.SelectMany(c => Enumerable.Range(0, 4).Select(i => $"{c.Name} {chipCodesMultiConverter.Convert(new object[] { c.Codes, i }, null, null, null)}"))
                .Distinct().ToArray();
            var inaccessibleChips = everyChipString.Except(accessibleChipStrings).ToArray();

            contents += "\n\nInaccessible chips:\n";
            contents += string.Join("\n", inaccessibleChips);

            var saveFileDialog = new CommonSaveFileDialog
            {
                RestoreDirectory = false,
                InitialDirectory = Directory.GetCurrentDirectory(),
                DefaultExtension = "txt",
                NavigateToShortcut = true,
                OverwritePrompt = true,
                DefaultFileName = "chips.txt",
                Title = "Save chip dump"
            };
            saveFileDialog.Filters.Add(new CommonFileDialogFilter("Text file", "*.txt"));
            saveFileDialog.Filters.Add(new CommonFileDialogFilter("All Files", "*.*"));

            var dialogSuccess = saveFileDialog.ShowDialog();
            if (dialogSuccess == CommonFileDialogResult.Ok)
            {
                File.WriteAllText(saveFileDialog.FileName, contents);
            }
        }

        private Map LoadMap(string fileName, bool decode)
        {
            var mapContents = TCDEncodeDecode.ReadTextFile(fileName, decode);

            if (mapContents == null)
            {
                return null;
            }

            return new Map { StringValue = mapContents, Name = Path.GetFileNameWithoutExtension(fileName) };
        }
    }
}
