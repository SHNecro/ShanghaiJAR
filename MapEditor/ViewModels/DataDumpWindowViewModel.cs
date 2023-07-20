using Common.EncodeDecode;
using MapEditor.Core;
using MapEditor.Core.Converters;
using MapEditor.Models;
using MapEditor.Models.Elements;
using MapEditor.Models.Elements.Enums;
using MapEditor.Models.Elements.Events;
using MapEditor.Models.Elements.Terms;
using Microsoft.WindowsAPICodePack.Dialogs;
using NSEnemy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static NSEnemy.EnemyBase;

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
        public ICommand DumpDataCommand => new RelayCommand(this.DumpData);
        public ICommand DumpMapsCommand => new RelayCommand(this.DumpMaps);

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

        public string[] Options { get; } = new[] { "Chips", "AddOns", "Upgrades", "Flags/Vars" };

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
                case "Upgrades":
                    contents = this.DumpUpgrades();
                    break;
                case "Flags/Vars":
                    contents = this.DumpFlagsVars();
                    break;
                default:
                    return;
            }

            var defaultFileName = new string(this.Operation.Where(char.IsLetter).ToArray()).ToLower();

			var saveFileDialog = new CommonSaveFileDialog
            {
                RestoreDirectory = false,
                InitialDirectory = Directory.GetCurrentDirectory(),
                DefaultExtension = "txt",
                NavigateToShortcut = true,
                OverwritePrompt = true,
                DefaultFileName = $"{defaultFileName}.txt",
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

                if (Path.GetFullPath(source) == target)
                {
                    return;
                }

                this.CopyFolder(source, target);
            }
        }

        private void DumpData()
        {
            var folderBrowserDialog = new CommonOpenFileDialog
            {
                RestoreDirectory = false,
                InitialDirectory = Directory.GetCurrentDirectory(),
                IsFolderPicker = true,
                EnsureFileExists = true,
                Title = "Copy all data files"
            };

            var folderBrowserDialogSuccess = folderBrowserDialog.ShowDialog();
            if (folderBrowserDialogSuccess == CommonFileDialogResult.Ok)
            {
                var source = "data/data";
                var target = Path.GetFullPath(folderBrowserDialog.FileName);

                if (Path.GetFullPath(source) == target)
                {
                    return;
                }

                this.CopyFolder(source, target);
            }
        }

        private void DumpMaps()
        {
            var folderBrowserDialog = new CommonOpenFileDialog
            {
                RestoreDirectory = false,
                InitialDirectory = Directory.GetCurrentDirectory(),
                IsFolderPicker = true,
                EnsureFileExists = true,
                Title = "Export all maps"
            };

            var folderBrowserDialogSuccess = folderBrowserDialog.ShowDialog();
            if (folderBrowserDialogSuccess == CommonFileDialogResult.Ok)
            {
                foreach (var map in this.allMaps)
                {
                    File.WriteAllText($"{Path.Combine(folderBrowserDialog.FileName, map.Name)}.shd", map.StringValue);
                }
            }
        }

        private void CopyFolder(string source, string target)
        {
            var files = Directory.GetFiles(source);
            var folders = Directory.GetDirectories(source);

            foreach (var file in files)
            {
                if (file.Contains("Config.xml"))
                {
                    continue;
                }
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
			var virusChipsSolo = this.allMaps
				.SelectMany(m => m.RandomEncounters.RandomEncounters
					.Where(re => re.IsChipDropped && re.Enemies.All(e => !e.EnemyDefinition?.IsNavi ?? true) && re.Enemies.Count(e => e.ID != 0) == 1)
					.SelectMany(re => re.Enemies
						.SelectMany(e =>
						{
							return new[]
							{
								new[] { e.Chip4 }.Concat(e.Chip4?.RandomAlternatives ?? emptyList).Select(c => Tuple.Create($"{e.Name} Chip 2 (10,9)", c)),
								new[] { e.Chip3 }.Concat(e.Chip3?.RandomAlternatives ?? emptyList).Select(c => Tuple.Create($"{e.Name} Chip 3 (8,7)", c)),
								new[] { e.Chip2 }.Concat(e.Chip2?.RandomAlternatives ?? emptyList).Select(c => Tuple.Create($"{e.Name} Chip 4 (6,5)", c)),
								new[] { e.Chip1 }.Concat(e.Chip1?.RandomAlternatives ?? emptyList).Select(c => Tuple.Create($"{e.Name} Chip 5 (4-0)", c)),
							}.SelectMany(c => c);
						})
				.Select(tup => Tuple.Create(tup.Item2, m.Header.TitleKey, $"Virus (Solo): {tup.Item1}"))))
				.Where(c => c.Item1 != null).ToArray();
			var virusChipsSRank = this.allMaps
				.SelectMany(m => m.RandomEncounters.RandomEncounters
					.Where(re => re.IsChipDropped && re.Enemies.Any(e => e.EnemyDefinition?.IsNavi ?? false) || re.Enemies.Count(ee => ee.ID != 0) > 1)
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
                            .Where(me => me.Category == EventCategoryOption.Battle)
                            .Where(me => (me.Instance as BattleEvent).Encounter.IsChipDropped)
						    .Where(me => (me.Instance as BattleEvent).Encounter.Enemies.Any(e => e.EnemyDefinition != null && (e.EnemyDefinition.IsNavi) || (me.Instance as BattleEvent).Encounter.Enemies.Count(ee => ee.ID != 0) > 1))
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
                            .Where(me => me.Category == EventCategoryOption.ChipGet)
                            .Where(me => (me.Instance as ChipGetEvent).IsAdding)
                            .Select(me => (me.Instance as ChipGetEvent).Chip)
                .Select(c => Tuple.Create(c, m.Header.TitleKey, "Scripted Event (ChipGet)")))))
                .Where(c => c.Item1 != null).ToArray();
            var givenItemGetChips = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Where(me => me.Category == EventCategoryOption.ItemGet)
                            .Select(me => (me.Instance as ItemGetEvent).Mystery.Chip)
                .Select(c => Tuple.Create(c, m.Header.TitleKey, "Scripted Event (ItemGet)")))))
                .Where(c => c.Item1 != null).ToArray();
            var shopChips = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Where(me => me.Category == EventCategoryOption.Shop)
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

            var allAccessibleChips = new[] { gmdChips, virusChipsSRank, virusChipsSolo, bmdpmdChips, battleChips, givenChipGetChips, givenItemGetChips, shopChips }.SelectMany(tup => tup)
                .Where(tup => tup.Item2 != "Map.ExOmakeName" && tup.Item2 != "Map.DebugRoom1Name");
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
			var contents = "Solo virus S-rank rewards omitted, would require <5s nohit 2step 2counter:\n\n";
			contents += string.Join("\n", chipLocations);

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
                            .Where(me => me.Category == EventCategoryOption.ItemGet)
                            .Select(me => (me.Instance as ItemGetEvent).Mystery)
                    .Where(rm => rm.Category == 2)
                    .Select(rm => Tuple.Create(rm.ID, rm.Data))
                .Select(tup => Tuple.Create(m.Header.TitleKey, "Scripted Event (ItemGet)", tup)))))
                .ToArray();
            var shopAddOns = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Where(me => me.Category == EventCategoryOption.Shop)
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

        private string DumpUpgrades()
        {
            var bmdpmdUpgrades = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .OfType<MapMystery>()
                    .Where(mm => mm.BaseMystery.Category == 3 && mm.Type != 0 && mm.BaseMystery.ID <= 4)
                    .Select(mm => Tuple.Create(mm.Type, Tuple.Create(mm.BaseMystery.ID, mm.BaseMystery.Data)))
                .Select(tup => Tuple.Create(m.Header.TitleKey, new[] { "GMD", "BMD", "PMD" }[tup.Item1], tup.Item2)))
                .ToArray();
            var givenItemGetUpgrades = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Where(me => me.Category == EventCategoryOption.ItemGet)
                            .Select(me => (me.Instance as ItemGetEvent).Mystery)
                    .Where(rm => rm.Category == 3 && rm.ID <= 4)
                    .Select(rm => Tuple.Create(rm.ID, rm.Data))
                .Select(tup => Tuple.Create(m.Header.TitleKey, "Scripted Event (ItemGet)", tup)))))
                .ToArray();
            var shopUpgrades = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Where(me => me.Category == EventCategoryOption.Shop)
                            .Where(me => (me.Instance as ShopEvent).ShopTypeNumber == 0)
                            .SelectMany(me => (me.Instance as ShopEvent).ShopItems.ShopItems
                                .Where(si => si.ID == 0)
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
            
            var mysteryUpgrades = new[] { bmdpmdUpgrades, givenItemGetUpgrades }.SelectMany(tup => tup);

            var hpMemoryStrings = shopUpgrades.Select(tup => $"{Constants.TranslationService.Translate(tup.Item1).Text}\t{tup.Item2} + {tup.Item3.Item2} per")
                .Concat(mysteryUpgrades.Where(tup => tup.Item3.Item1 == 0).Select(tup => $"{Constants.TranslationService.Translate(tup.Item1).Text}\t{tup.Item2}"));

            var regUpStrings = mysteryUpgrades.Where(tup => tup.Item3.Item1 == 1).Select(tup => $"{Constants.TranslationService.Translate(tup.Item1).Text}\t{tup.Item2}\t+{tup.Item3.Item2}");
            var subMemoryStrings = mysteryUpgrades.Where(tup => tup.Item3.Item1 == 2).Select(tup => $"{Constants.TranslationService.Translate(tup.Item1).Text}\t{tup.Item2}");
            var corePlusStrings = mysteryUpgrades.Where(tup => tup.Item3.Item1 == 3).Select(tup => $"{Constants.TranslationService.Translate(tup.Item1).Text}\t{tup.Item2}");
            var hertzUpStrings = mysteryUpgrades.Where(tup => tup.Item3.Item1 == 4).Select(tup => $"{Constants.TranslationService.Translate(tup.Item1).Text}\t{tup.Item2}\t+{tup.Item3.Item2}");

            var contents = new StringBuilder();

            contents.AppendLine("HP Memory");
            contents.Append(string.Join("\n", hpMemoryStrings.Select(s => $"\t{s}")));
            contents.AppendLine();

            contents.AppendLine("RegUp");
            contents.Append(string.Join("\n", regUpStrings.Select(s => $"\t{s}")));
            contents.AppendLine();

            contents.AppendLine("SubMemory");
            contents.Append(string.Join("\n", subMemoryStrings.Select(s => $"\t{s}")));
            contents.AppendLine();

            contents.AppendLine("CorePlus");
            contents.Append(string.Join("\n", corePlusStrings.Select(s => $"\t{s}")));
            contents.AppendLine();

            contents.AppendLine("HertzUp");
            contents.Append(string.Join("\n", hertzUpStrings.Select(s => $"\t{s}")));

            return contents.ToString();
        }

        private string DumpFlagsVars()
        {
            //flags
            var specialEncounterFlags = this.allMaps
                .Where(m => m.Header.SpecialEncounterCount > 0)
                .Select(m => Tuple.Create(m.Header.SpecialEncounterFlag, "Special Encounter Flag", Constants.TranslationService.Translate(m.Header.TitleKey).Text));
            var termFlags = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Terms.Terms
                            .Select(to => to.Instance)
                            .OfType<FlagTerm>()
                            .Select(ft =>
                            {
                                var mapTitle = Constants.TranslationService.Translate(m.Header.TitleKey).Text;
                                var mapObject = mo.ID;
                                var page = mep.PageNumber;
                                return Tuple.Create(ft.Flag, "Term", $"{mapTitle}: {mapObject} pg.{page}: {ft.Name}");
                            }))));
            var ifFlagEvents = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Select(eo => eo.Instance)
                            .OfType<IfFlagEvent>()
                            .Select(ife =>
                            {
                                var mapTitle = Constants.TranslationService.Translate(m.Header.TitleKey).Text;
                                var mapObject = mo.ID;
                                var page = mep.PageNumber;
                                return Tuple.Create(ife.FlagNumber, "Event (ifFlag)", $"{mapTitle}: {mapObject} pg.{page}: {ife.Name}");
                            }))));
            var setFlagEvents = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Select(eo => eo.Instance)
                            .OfType<EditFlagEvent>()
                            .Select(efe =>
                            {
                                var mapTitle = Constants.TranslationService.Translate(m.Header.TitleKey).Text;
                                var mapObject = mo.ID;
                                var page = mep.PageNumber;
                                return Tuple.Create(efe.FlagNumber, "Event (editFlag)", $"{mapTitle}: {mapObject} pg.{page}: {efe.Name}");
                            }))));

            //variables
            var termVariables = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Terms.Terms
                            .Select(to => to.Instance)
                            .OfType<VariableTerm>()
                            .SelectMany(vt =>
                            {
                                var mapTitle = Constants.TranslationService.Translate(m.Header.TitleKey).Text;
                                var mapObject = mo.ID;
                                var page = mep.PageNumber;
                                var variablesUsed = new List<Tuple<int, string, string>> { Tuple.Create(vt.VariableLeft, "Term, lhs", $"{mapTitle}: {mapObject} pg.{page}: {vt.Name}") };
                                if (vt.IsVariable)
                                {
                                    variablesUsed.Add(Tuple.Create(vt.VariableOrConstantRight, "Term, rhs", $"{mapTitle}: {mapObject} pg.{page}: {vt.Name}"));
                                }

                                return variablesUsed;
                            }))));
            var ifVariableEvents = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Select(eo => eo.Instance)
                            .OfType<IfValueEvent>()
                            .SelectMany(ive =>
                            {
                                var mapTitle = Constants.TranslationService.Translate(m.Header.TitleKey).Text;
                                var mapObject = mo.ID;
                                var page = mep.PageNumber;
                                var variablesUsed = new List<Tuple<int, string, string>> { Tuple.Create(ive.VariableLeft, "Event (ifValue, lhs)", $"{mapTitle}: {mapObject} pg.{page}: {ive.Name}") };
                                if (ive.IsVariable)
                                {
                                    variablesUsed.Add(Tuple.Create(ive.VariableOrConstantRight, "Event (ifValue, rhs)", $"{mapTitle}: {mapObject} pg.{page}: {ive.Name}"));
                                }

                                return variablesUsed;
                            }))));
            var setVariableEvents = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Select(eo => eo.Instance)
                            .OfType<EditValueEvent>()
                            .SelectMany(eve =>
                            {
                                var mapTitle = Constants.TranslationService.Translate(m.Header.TitleKey).Text;
                                var mapObject = mo.ID;
                                var page = mep.PageNumber;

                                var variable = eve.IsVariable ? -1 : eve.Index;
                                var variablesUsed = new List<Tuple<int, string, string>> { Tuple.Create(variable, "Event (setValue, lhs)", $"{mapTitle}: {mapObject} pg.{page}: {eve.Name}") };
                                if (eve.ReferenceType == 1 || eve.ReferenceType == 2)
                                {
                                    var setValue = eve.ReferenceType == 2 ? -1 : int.Parse(eve.ReferenceData);
                                    variablesUsed.Add(Tuple.Create(setValue, "Event (setValue, rhs)", $"{mapTitle}: {mapObject} pg.{page}: {eve.Name}"));
                                }

                                return variablesUsed;
                            }))));
            var effectPositionVariables = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Select(eo => eo.Instance)
                            .OfType<EffectEvent>()
                            .Where(ee => ee.LocationType == (int)EffectLocationTypeNumber.Variable)
                            .SelectMany(ee =>
                            {
                                var mapTitle = Constants.TranslationService.Translate(m.Header.TitleKey).Text;
                                var mapObject = mo.ID;
                                var page = mep.PageNumber;

                                var variablesUsed = new List<Tuple<int, string, string>>
                                {
                                    Tuple.Create(ee.X, "Event (effect, X position)", $"{mapTitle}: {mapObject} pg.{page}: {ee.Name}"),
                                    Tuple.Create(ee.Y, "Event (effect, Y position)", $"{mapTitle}: {mapObject} pg.{page}: {ee.Name}"),
                                    Tuple.Create(ee.Z, "Event (effect, Z position)", $"{mapTitle}: {mapObject} pg.{page}: {ee.Name}")
                                };

                                return variablesUsed;
                            }))));
            var numSetEvents = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Select(eo => eo.Instance)
                            .OfType<NumSetEvent>()
                            .Select(nse =>
                            {
                                var mapTitle = Constants.TranslationService.Translate(m.Header.TitleKey).Text;
                                var mapObject = mo.ID;
                                var page = mep.PageNumber;
                                return Tuple.Create(nse.TargetVariable, "Event (numSet)", $"{mapTitle}: {mapObject} pg.{page}: {nse.Name}");
                            }))));

            // bmd/pmd opened "flags"
            var bmdPmdObjects = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .OfType<MapMystery>()
                    .Where(mm => mm.Type != 0)
                    .Select(mm =>
                    {
                        var mapTitle = Constants.TranslationService.Translate(m.Header.TitleKey).Text;

                        var mysteryDescription = mm.BaseMystery.Name;
                        return Tuple.Create(mm.Flag, mm.Type == 1 ? "BMD" : "PMD", $"{mapTitle}: {mysteryDescription}");
                    }));

            // gmd opened "flags"
            var gmdObjects = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .OfType<MapMystery>()
                    .Where(mm => mm.Type == 0)
                    .Select(mm =>
                    {
                        var mapTitle = Constants.TranslationService.Translate(m.Header.TitleKey).Text;
                        return Tuple.Create(mm.Flag, "GMD", mapTitle);
                    }));

            // shop stock
            var shopEvents = this.allMaps
                .SelectMany(m => m.MapObjects.MapObjects
                    .SelectMany(mo => mo.Pages.MapEventPages
                        .SelectMany(mep => mep.Events.Events
                            .Select(eo => eo.Instance)
                            .OfType<ShopEvent>()
                            .Where(se => se.ShopStockIndex != 0)
                            .Select(se =>
                            {
                                var mapTitle = Constants.TranslationService.Translate(m.Header.TitleKey).Text;

                                return Tuple.Create(se.ShopStockIndex, "Store", $"{mapTitle}: {se.Name}");
                            }))));

            var contents = new StringBuilder();

            contents.AppendLine("FLAGS (Does not include code-defined/used)");
            var flags = new[] { specialEncounterFlags, termFlags, ifFlagEvents, setFlagEvents };
            var flagEntries = flags.SelectMany(l => l).OrderBy(tup => tup.Item1).Select(tup => $"{tup.Item1}\t{tup.Item2}\t{tup.Item3}");
            contents.Append(string.Join("\n", flagEntries));
            contents.AppendLine();

            contents.AppendLine();

            contents.AppendLine("VARS (Does not include code-defined/used)");
            var vars = new[] { termVariables, ifVariableEvents, setVariableEvents, effectPositionVariables, numSetEvents };
            var varEntries = vars.SelectMany(l => l).OrderBy(tup => tup.Item1).Select(tup => $"{tup.Item1}\t{tup.Item2}\t{tup.Item3}");
            contents.Append(string.Join("\n", varEntries));
            contents.AppendLine();

            contents.AppendLine();

            contents.AppendLine("BMD/PMD");
            var bmdPmdEntries = bmdPmdObjects.OrderBy(tup => tup.Item1).Select(tup => $"{tup.Item1}\t{tup.Item2}\t{tup.Item3}");
            contents.Append(string.Join("\n", bmdPmdEntries));
            contents.AppendLine();

            contents.AppendLine();

            contents.AppendLine("GMD (Reset on jackin/out)");
            var gmdEntries = gmdObjects.OrderBy(tup => tup.Item1).Select(tup => $"{tup.Item1}\t{tup.Item2}\t{tup.Item3}");
            contents.Append(string.Join("\n", gmdEntries));
            contents.AppendLine();

            contents.AppendLine();

            contents.AppendLine("Stores Stocks");
            var shopEntries = shopEvents.OrderBy(tup => tup.Item1).Select(tup => $"{tup.Item1}\t{tup.Item2}\t{tup.Item3}");
            contents.Append(string.Join("\n", shopEntries));
            contents.AppendLine();

            return contents.ToString();
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
