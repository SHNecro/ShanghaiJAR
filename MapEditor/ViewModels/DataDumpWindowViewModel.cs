using Common.EncodeDecode;
using MapEditor.Core;
using MapEditor.Core.Converters;
using MapEditor.Models;
using MapEditor.Models.Elements;
using MapEditor.Models.Elements.Enums;
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
    public class DataDumpWindowViewModel : ViewModelBase
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
        public ICommand DumpCommand => new RelayCommand(this.Dump);
        public ICommand DumpStringsCommand => new RelayCommand(this.DumpStrings);

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

        private void Dump()
        {
            var contents = string.Empty;
            switch (this.Operation)
            {
                case "Chips":
                    contents = this.DumpChips();
                    break;
                case "AddOns":
                    contents = this.DumpAddOns();
                    break;
                default:
                    return;
            }

            var saveFileDialog = new CommonSaveFileDialog
            {
                RestoreDirectory = false,
                InitialDirectory = Directory.GetCurrentDirectory(),
                DefaultExtension = "txt",
                NavigateToShortcut = true,
                OverwritePrompt = true,
                DefaultFileName = "dump.txt",
                Title = "Save dump"
            };
            saveFileDialog.Filters.Add(new CommonFileDialogFilter("Text file", "*.txt"));
            saveFileDialog.Filters.Add(new CommonFileDialogFilter("All Files", "*.*"));

            var dialogSuccess = saveFileDialog.ShowDialog();
            if (dialogSuccess == CommonFileDialogResult.Ok)
            {
                File.WriteAllText(saveFileDialog.FileName, contents);
            }
        }

        private void DumpStrings()
        {
            var folderBrowserDialog = new CommonOpenFileDialog
            {
                RestoreDirectory = false,
                InitialDirectory = Directory.GetCurrentDirectory(),
                IsFolderPicker = true,
                EnsureFileExists = true,
                Title = "Copy all language files"
            };

            var folderBrowserDialogSuccess = folderBrowserDialog.ShowDialog();
            if (folderBrowserDialogSuccess == CommonFileDialogResult.Ok)
            {
                var source = "language";
                var target = Path.GetFullPath(folderBrowserDialog.FileName);

                if (source == target)
                {
                    return;
                }

                this.CopyFolder(source, target);
            }
        }

        private void CopyFolder(string source, string target)
        {
            var files = Directory.GetFiles(source);
            var folders = Directory.GetDirectories(source);

            foreach (var file in files)
            {
                File.Copy(file, Path.Combine(target, Path.GetFileName(file)), true);
            }
            foreach (var folder in folders)
            {
                var newFolder = Path.Combine(target, Path.GetFileName(folder));
                Directory.CreateDirectory(newFolder);
                this.CopyFolder(folder, newFolder);
            }
        }

        private string DumpChips()
        {
            var emptyList = new List<Chip>();

            var gmdChips = this.allMaps
                .SelectMany(m => m.RandomMysteryData.RandomMysteryData
                    .Select(gmd => gmd.Chip)
                .Select(c => Tuple.Create(c, m.Header.TitleKey, "GMD")))
                .Where(c => c.Item1 != null).ToArray();
            var virusChips = this.allMaps
                .SelectMany(m => m.RandomEncounters.RandomEncounters
                    .SelectMany(re => re.Enemies
                        .SelectMany(e =>
                        {
                            return new[]
                            {
                                new[] { e.Chip5 }.Concat(e.Chip5?.RandomAlternatives ?? emptyList).Select(c => Tuple.Create($"{e.Name} Chip 1 (S)", c)),
                                new[] { e.Chip4 }.Concat(e.Chip4?.RandomAlternatives ?? emptyList).Select(c => Tuple.Create($"{e.Name} Chip 2 (10,9)", c)),
                                new[] { e.Chip3 }.Concat(e.Chip3?.RandomAlternatives ?? emptyList).Select(c => Tuple.Create($"{e.Name} Chip 3 (8,7)", c)),
                                new[] { e.Chip2 }.Concat(e.Chip2?.RandomAlternatives ?? emptyList).Select(c => Tuple.Create($"{e.Name} Chip 4 (6,5)", c)),
                                new[] { e.Chip1 }.Concat(e.Chip1?.RandomAlternatives ?? emptyList).Select(c => Tuple.Create($"{e.Name} Chip 5 (4-0)", c)),
                            }.SelectMany(c => c);
                        })
                .Select(tup => Tuple.Create(tup.Item2, m.Header.TitleKey, $"Virus: {tup.Item1}"))))
                .Where(c => c.Item1 != null).ToArray();
            var bmdpmdChips = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .OfType<MapMystery>()
                    .Select(mm => Tuple.Create(mm.Type, mm.BaseMystery.Chip))
                .Select(tup => Tuple.Create(tup.Item1 == 0 ? null : tup.Item2, m.Header.TitleKey, new[] { "GMD", "BMD", "PMD" }[tup.Item1])))
                .Where(c => c.Item1 != null).ToArray();
            var battleChips = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Where(me => me.Category == Models.Elements.Enums.EventCategoryOption.Battle)
                            .Where(me => (me.Instance as BattleEvent).Encounter.IsChipDropped)
                            .SelectMany(me => (me.Instance as BattleEvent).Encounter.Enemies
                                .SelectMany(e =>
                                {
                                    return new[]
                                    {
                                        new[] { e.Chip5 }.Concat(e.Chip5?.RandomAlternatives ?? emptyList).Select(c => Tuple.Create($"{e.Name} Chip 1 (S)", c)),
                                        new[] { e.Chip4 }.Concat(e.Chip4?.RandomAlternatives ?? emptyList).Select(c => Tuple.Create($"{e.Name} Chip 2 (10,9)", c)),
                                        new[] { e.Chip3 }.Concat(e.Chip3?.RandomAlternatives ?? emptyList).Select(c => Tuple.Create($"{e.Name} Chip 3 (8,7)", c)),
                                        new[] { e.Chip2 }.Concat(e.Chip2?.RandomAlternatives ?? emptyList).Select(c => Tuple.Create($"{e.Name} Chip 4 (6,5)", c)),
                                        new[] { e.Chip1 }.Concat(e.Chip1?.RandomAlternatives ?? emptyList).Select(c => Tuple.Create($"{e.Name} Chip 5 (4-0)", c)),
                                    }.SelectMany(c => c);
                                })
                .Select(tup => Tuple.Create(tup.Item2, m.Header.TitleKey, $"Scripted Battle: {tup.Item1}"))))))
                .Where(c => c.Item1 != null).ToArray();
            var givenChipGetChips = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Where(me => me.Category == Models.Elements.Enums.EventCategoryOption.ChipGet)
                            .Where(me => (me.Instance as ChipGetEvent).IsAdding)
                            .Select(me => (me.Instance as ChipGetEvent).Chip)
                .Select(c => Tuple.Create(c, m.Header.TitleKey, "Scripted Event (ChipGet)")))))
                .Where(c => c.Item1 != null).ToArray();
            var givenItemGetChips = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Where(me => me.Category == Models.Elements.Enums.EventCategoryOption.ItemGet)
                            .Select(me => (me.Instance as ItemGetEvent).Mystery.Chip)
                .Select(c => Tuple.Create(c, m.Header.TitleKey, "Scripted Event (ItemGet)")))))
                .Where(c => c.Item1 != null).ToArray();
            var shopChips = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Where(me => me.Category == Models.Elements.Enums.EventCategoryOption.Shop)
                            .SelectMany(me => (me.Instance as ShopEvent).ShopItems.ShopItems
                                .Select(si =>
                                {
                                    var priceTypeString = (new EnumDescriptionTypeConverter(typeof(ShopPriceTypeNumber))).ConvertToString((ShopPriceTypeNumber)si.PriceType);
                                    var shopInfo = $"{si.Price} {priceTypeString}";
                                    var stockString = si.Stock == 0 ? string.Empty : $" (x{si.Stock})";
                                    shopInfo += stockString;
                                    return Tuple.Create(shopInfo, si.Chip);
                                })))
                .Select(tup => Tuple.Create(tup.Item2, m.Header.TitleKey, $"Shop: {tup.Item1}"))))
                .Where(c => c.Item1 != null).ToArray();

            var allAccessibleChips = new[] { gmdChips, virusChips, bmdpmdChips, battleChips, givenChipGetChips, givenItemGetChips, shopChips }.SelectMany(tup => tup)
                .Where(tup => tup.Item2 != "Map.ExOmakeName");
            var fullChips = allAccessibleChips.Select(c => Tuple.Create(
                    c.Item1,
                    Constants.ChipDefinitions.ContainsKey(c.Item1.ID) ? Constants.ChipDefinitions[c.Item1.ID] : null,
                    Constants.TranslationService.Translate(c.Item2).Text,
                    c.Item3))
                .Where(c => c.Item2 != null)
                .ToList();

            var chipCodesMultiConverter = new ChipCodesMultiConverter();
            var chipLocations = fullChips.OrderBy(c => c.Item3).ThenBy(c => c.Item4).ThenBy(c => $"{c.Item2.Name} {chipCodesMultiConverter.Convert(new object[] { c.Item2.Codes, c.Item1.CodeNumber }, null, null, null)}")
                .Select(c => Tuple.Create(c, $"{c.Item3}\t{c.Item4}\t{c.Item2.Name} {chipCodesMultiConverter.Convert(new object[] { c.Item2.Codes, c.Item1.CodeNumber }, null, null, null)}"))
                .GroupBy(tup => tup.Item2).Select(gr => gr.Key + (gr.Any(tup => tup.Item1.Item1.IsRandom) ? $" ({gr.Average(tup => tup.Item1.Item1.RandomChance) * 100:N0}%)" : string.Empty))
                .Distinct().ToArray();
            var contents = string.Join("\n", chipLocations);

            var accessibleChipStrings = fullChips.Select(c => $"{c.Item2.Name} {chipCodesMultiConverter.Convert(new object[] { c.Item2.Codes, c.Item1.CodeNumber }, null, null, null)}")
                .Distinct().ToArray();
            var everyChipString = Constants.ChipDefinitions.Values.SelectMany(c => Enumerable.Range(0, 4).Select(i => $"{c.Name} {chipCodesMultiConverter.Convert(new object[] { c.Codes, i }, null, null, null)}"))
                .Distinct().ToArray();
            var inaccessibleChips = everyChipString.Except(accessibleChipStrings).ToArray();

            contents += "\n\nInaccessible chips w/o rank up, CrimNois, Demo Room:\n";
            contents += string.Join("\n", inaccessibleChips);

            return contents;
        }

        private string DumpAddOns()
        {
            var emptyList = new List<Chip>();

            var bmdpmdAddOns = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .OfType<MapMystery>()
                    .Where(mm => mm.BaseMystery.Category == 2 && mm.Type != 0)
                    .Select(mm => Tuple.Create(mm.Type, Tuple.Create(mm.BaseMystery.ID, mm.BaseMystery.Data)))
                .Select(tup => Tuple.Create(m.Header.TitleKey, new[] { "GMD", "BMD", "PMD" }[tup.Item1], tup.Item2)))
                .ToArray();
            var givenItemGetAddOns = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Where(me => me.Category == Models.Elements.Enums.EventCategoryOption.ItemGet)
                            .Select(me => (me.Instance as ItemGetEvent).Mystery)
                    .Where(rm => rm.Category == 2)
                    .Select(rm => Tuple.Create(rm.ID, rm.Data))
                .Select(tup => Tuple.Create(m.Header.TitleKey, "Scripted Event (ItemGet)", tup)))))
                .ToArray();
            var shopAddOns = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Where(me => me.Category == Models.Elements.Enums.EventCategoryOption.Shop)
                            .Where(me => (me.Instance as ShopEvent).ShopTypeNumber == 2)
                            .SelectMany(me => (me.Instance as ShopEvent).ShopItems.ShopItems
                                .Select(si =>
                                {
                                    var priceTypeString = (new EnumDescriptionTypeConverter(typeof(ShopPriceTypeNumber))).ConvertToString((ShopPriceTypeNumber)si.PriceType);
                                    var shopInfo = $"{si.Price} {priceTypeString}";
                                    var stockString = si.Stock == 0 ? string.Empty : $" (x{si.Stock})";
                                    shopInfo += stockString;
                                    return Tuple.Create(shopInfo, Tuple.Create(si.ID, si.Data));
                                })))
                .Select(tup => Tuple.Create(m.Header.TitleKey, $"Shop: {tup.Item1}", tup.Item2))))
                .ToArray();

            var allAccessibleAddOns = new[] { bmdpmdAddOns, givenItemGetAddOns, shopAddOns }.SelectMany(tup => tup)
                .Where(tup => tup.Item1 != "Map.ExOmakeName");

            Func<int, string> intToAddOnColorAction = (i) => (new EnumDescriptionTypeConverter(typeof(ProgramColorTypeNumber))).ConvertToString((ProgramColorTypeNumber)i);
            var addOnIDToAddOnDefinitionConverter = new AddOnIDToAddOnDefinitionConverter();
            Func<Tuple<int, int>, string> addOnToString = (tup) =>
            {
                var definition = addOnIDToAddOnDefinitionConverter.Convert(tup.Item1, null, null, null) as AddOnDefinition;
                if (definition == null)
                {
                    return null;
                }

                return $"{definition.Name} {intToAddOnColorAction(tup.Item2)}";
            };
            var accessibleAddOnStrings = allAccessibleAddOns.Select(a => $"{Constants.TranslationService.Translate(a.Item1).Text}\t{a.Item2}\t{addOnToString(a.Item3)}");

            var contents = string.Join("\n", accessibleAddOnStrings);

            var everyAddOnName = Constants.AddOnDefinitions.Values.Select(a => a.Name);
            var inaccessibleAddOns = everyAddOnName.Where(a => !accessibleAddOnStrings.Any(aa => aa.Contains(a)));

            var duplicateAddOns = allAccessibleAddOns
                .Where(a => a.Item3.Item2 != 0)
                .GroupBy(a => addOnToString(a.Item3))
                .Where(gr => gr.Count() > 1)
                .Select(gr => string.Join("\n-", gr.Select(a => $"{Constants.TranslationService.Translate(a.Item1).Text}\t{a.Item2}\t{addOnToString(a.Item3)}")));

            contents += "\n\nDuplicate addons:\n";
            contents += string.Join("\n", duplicateAddOns);

            contents += "\n\nInaccessible addons w/o Demo Room:\n";
            contents += string.Join("\n", inaccessibleAddOns);

            return contents;
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
