using Common.EncodeDecode;
using Common.OpenGL;
using MapEditor.Core;
using MapEditor.Models;
using MapEditor.Models.Elements.Enums;
using MapEditor.Rendering;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Application = System.Windows.Application;
using Timer = System.Timers.Timer;

namespace MapEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static Timer renderTimer;
        private static string loadedMapPath;
        private static Map lastloadedMap;
        private static Map currentMap;
        private static string positionStatus;
        private static string progressStatus;
        private static double progress;

        private static MainWindowViewModel instance;

        public static event Action MapLoadProgressEvent;

        public static void Initialize()
        {
            MainWindowViewModel.renderTimer = new Timer(1000 / 60);

            var isDrawing = false;
            MainWindowViewModel.renderTimer.Elapsed += (s, args) =>
            {
                if (isDrawing)
                {
                    return;
                }

                try
                {
                    Application.Current?.Dispatcher?.BeginInvoke(new Action(() =>
                    {
                        isDrawing = true;
                        MapRenderer.Draw();
                        isDrawing = false;
                    }), DispatcherPriority.Render);
                }
                catch
                { }
            };
        }

        public MainWindowViewModel()
        {
            MainWindowViewModel.instance = this;

            MapRenderer.CurrentLevelUpdated += (s, args) =>
            {
                this.OnPropertyChanged(nameof(this.CurrentLevel));
            };

            MapRenderer.CurrentToolUpdated += (s, args) =>
            {
                this.OnPropertyChanged(nameof(this.CurrentTool));
            };

            this.SelectedTile = WalkableTileType.Empty;
            this.CurrentTool = EditToolType.SelectionTool;
            this.IsPlacingConveyors = true;
            this.ConveyorColor = ConveyorColorType.Blue;

            MapRenderer.MapPositionUpdateFunc = (p, z, id) =>
            {
                if (!p.HasValue)
                {
                    this.PositionStatus = null;
                    return;
                }
                var idString = id != null ? $" ({id})" : string.Empty;
                this.PositionStatus = $"{p.Value.X}, {p.Value.Y}, {z}{idString}";
            };

            Constants.TranslationService.KeysReloaded += (sender, args) =>
            {
                // TODO: subscribe translated text to reload on refresh without memory leak
                foreach (var mo in MainWindowViewModel.currentMap.MapObjects.MapObjects)
                {
                    foreach (var p in mo.Pages.MapEventPages)
                    {
                        foreach (var e in p.Events.Events)
                        {
                            if (e.Instance is ITranslatedModel tm)
                            {
                                tm.RefreshTranslation();
                            }
                        }
                    }
                }

                foreach (var re in MainWindowViewModel.currentMap.RandomEncounters.RandomEncounters)
                {
                    re.Enemy1.RefreshTranslation();
                    re.Enemy2.RefreshTranslation();
                    re.Enemy3.RefreshTranslation();
                }
            };
        }

        public void StartRendering()
        {
            MainWindowViewModel.renderTimer.Start();
        }

        public string LoadedMapPath
        {
            get
            {
                return MainWindowViewModel.loadedMapPath;
            }

            set
            {
                this.SetValue(ref MainWindowViewModel.loadedMapPath, value);
                this.OnPropertyChanged(nameof(this.Title));
            }
        }

        public Map LastLoadedMap
        {
            get
            {
                return MainWindowViewModel.lastloadedMap;
            }

            set
            {
                this.SetValue(ref MainWindowViewModel.lastloadedMap, value);
                this.OnPropertyChanged(nameof(this.Title));
            }
        }

        public Map CurrentMap
        {
            get
            {
                return MainWindowViewModel.currentMap;
            }
            set
            {
                if (MainWindowViewModel.currentMap != null)
                {
                    MainWindowViewModel.currentMap.PropertyChanged -= this.OnCurrentMapPropertyUpdated;
                }
                value.PropertyChanged += this.OnCurrentMapPropertyUpdated;
                this.SetValue(ref MainWindowViewModel.currentMap, value);
                MapRenderer.CurrentMap = value;
                this.UpdatePropertiesToMatchLevels();
                MapRenderer.Refresh();
            }
        }

        public string Title
        {
            get
            {
                var dirtyMarker = this.IsDirty ? "*" : string.Empty;
                return $"Map Editor - {dirtyMarker}{this.CurrentMap.Name}";
            }
        }

        public int CurrentLevel
        {
            get
            {
                return MapRenderer.CurrentLevel;
            }
            set
            {
                MapRenderer.CurrentLevel = value;
                OnPropertyChanged(nameof(this.CurrentLevel));
            }
        }

        public EditToolType CurrentTool
        {
            get
            {
                return MapRenderer.CurrentTool;
            }
            set
            {
                MapRenderer.CurrentTool = value;
                OnPropertyChanged(nameof(this.CurrentTool));
            }
        }

        public WalkableTileType SelectedTile
        {
            get
            {
                return MapRenderer.SelectedTile;
            }

            set
            {
                MapRenderer.SelectedTile = value;
                this.OnPropertyChanged(nameof(this.SelectedTile));
                this.CurrentTool = EditToolType.DrawTool;
            }
        }

        public bool IsPlacingConveyors
        {
            get
            {
                return MapRenderer.IsPlacingConveyors;
            }

            set
            {
                MapRenderer.IsPlacingConveyors = value;
                this.OnPropertyChanged(nameof(this.IsPlacingConveyors));
            }
        }

        public ConveyorColorType ConveyorColor
        {
            get
            {
                return MapRenderer.ConveyorColor;
            }

            set
            {
                MapRenderer.ConveyorColor = value;
                this.OnPropertyChanged(nameof(this.ConveyorColor));
            }
        }

        public ObservableCollection<Tuple<int, LevelDisplayOptions>> DisplayOptions
        {
            get { return MapRenderer.DisplayOptions; }
            set { MapRenderer.DisplayOptions = value; }
        }
        
        public MapDisplayOptions MapDisplayOptions
        {
            get { return MapRenderer.MapDisplayOptions; }
            set { MapRenderer.MapDisplayOptions = value; }
        }

        public string PositionStatus
        {
            get { return MainWindowViewModel.positionStatus; }
            set { this.SetValue(ref MainWindowViewModel.positionStatus, value); }
        }
        public string ProgressStatus
        {
            get { return MainWindowViewModel.progressStatus; }
            set { this.SetValue(ref MainWindowViewModel.progressStatus, value); }
        }
        public double Progress
        {
            get { return MainWindowViewModel.progress; }
            set { this.SetValue(ref MainWindowViewModel.progress, value); }
        }

		public bool IsDirty
		{
			get
			{
                var currentMap = this.CurrentMap?.StringValue;
                var lastMap = this.LastLoadedMap?.StringValue;
				return string.Compare(currentMap, lastMap) != 0;
			}
		}

		public ICommand ChooseEncodedFileCommand => new RelayCommand(this.ChooseEncodedFile);
        public ICommand SaveOrCreateEncodedFileCommand => new RelayCommand(() => this.SaveOrCreateEncodedFile());
        public ICommand SaveNewEncodedFileCommand => new RelayCommand(() => this.SaveNewEncodedFile());
        public ICommand ChooseDecodedFileCommand => new RelayCommand(this.ChooseDecodedFile);
        public ICommand SaveDecodedFileCommand => new RelayCommand(this.SaveDecodedFile);
        public ICommand OpenSettingsCommand => new RelayCommand(this.OpenSettings);

        public ICommand ReloadGraphicsCommand => new RelayCommand(this.ReloadGraphics);
        public ICommand ReloadSoundCommand => new RelayCommand(this.ReloadSound);
        public ICommand ReloadTranslationKeysCommand => new RelayCommand(this.ReloadTranslationKeys);
        public ICommand OpenStringBrowserCommand => new RelayCommand(this.OpenStringBrowser);
        public ICommand OpenDataBrowserCommand => new RelayCommand(this.OpenDataBrowser);
        public ICommand UnpackTCDCommand => new RelayCommand(this.UnpackTCD);
        public ICommand PackTCDCommand => new RelayCommand(this.PackTCD);

        public ICommand OpenErrorsCommand => new RelayCommand(this.OpenErrors);
        public ICommand DataDumpCommand => new RelayCommand(this.DataDump);

        public static Map GetCurrentMap() => MainWindowViewModel.currentMap;

        public static MainWindowViewModel GetInstance()
        {
            if (MainWindowViewModel.instance == null)
            {
                try
                {
                    MainWindowViewModel.instance = new MainWindowViewModel();
                }
                catch (Exception e)
                {
                    var exceptionStack = default(string);
                    var currentException = e;
                    while (currentException != null)
                    {
                        exceptionStack = $"{currentException.GetType().ToString()}: {currentException.Message}";
                        currentException = currentException.InnerException;
                    }
                    MessageBox.Show($"Editor failed to initialize.{Environment.NewLine}{exceptionStack.ToString()}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(-1);
                }
            }

            return MainWindowViewModel.instance;
        }

        public void ChooseDecodedFile()
        {
            var selectFileDialog = new CommonOpenFileDialog
            {
                RestoreDirectory = false,
                InitialDirectory = Directory.GetCurrentDirectory() + "\\data\\",
                EnsureFileExists = true,
                Multiselect = false,
                Title = "Import .shd"
            };
            selectFileDialog.Filters.Add(new CommonFileDialogFilter("Decoded Map File", "*.shd"));
            selectFileDialog.Filters.Add(new CommonFileDialogFilter("All Files", "*.*"));

            var dialogSuccess = selectFileDialog.ShowDialog();
            if (dialogSuccess == CommonFileDialogResult.Ok)
            {
                this.LoadMap(selectFileDialog.FileName, false);
            }
        }

        public bool LoadMap(string fileName, bool decode)
        {
            if (!this.ConfirmMapChange())
            {
                return false;
            }

            var mapContents = TCDEncodeDecode.ReadTextFile(fileName, decode);

            if (mapContents == null)
            {
                return false;
            }

            var openedMap = new Map { StringValue = mapContents, Name = Path.GetFileNameWithoutExtension(fileName) };

            if (openedMap.HasErrors)
            {
                MessageBox.Show($"Map contains errors, please find and resolve with View > Errors", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.CurrentMap = openedMap;
            this.LoadedMapPath = fileName;
            this.LastLoadedMap = (Map)this.CurrentMap.Clone();
            //var mapLoadSuccess = !openedMap.HasErrors;
            //if (mapLoadSuccess)
            //{
            //    this.CurrentMap = openedMap;
            //    this.LoadedMapPath = fileName;
            //    this.LastLoadedMap = (Map)this.CurrentMap.Clone();
            //}
            //else
            //{
            //    var filteredErrors = openedMap.Errors.Distinct();
            //    if (filteredErrors.Count() < 25)
            //    {
            //        MessageBox.Show($"Map failed to parse, with the following errors:{Environment.NewLine}{string.Join(Environment.NewLine, filteredErrors)}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //    else
            //    {
            //        MessageBox.Show($"Map failed to parse, with many errors.{Environment.NewLine}Please check that you've chosen the right file:{Environment.NewLine}{string.Join(Environment.NewLine, filteredErrors.Take(25))}{Environment.NewLine}...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }

            //    this.CurrentMap = Constants.BlankMapCreator();
            //    this.LoadedMapPath = null;
            //    this.LastLoadedMap = (Map)this.CurrentMap.Clone();
            //}

            MapRenderer.Refresh();
            MainWindowViewModel.MapLoadProgressEvent?.Invoke();

            return true;
        }

        public bool SaveOrCreateEncodedFile()
        {
            var success = false;
            if (this.LoadedMapPath == null)
            {
                success = this.SaveNewEncodedFile();
            }
            else
            {
                var shdFile = Path.ChangeExtension(this.LoadedMapPath, ".she");
                success = TCDEncodeDecode.SaveFile(shdFile, TCDEncodeDecode.EncodeMap(this.CurrentMap.StringValue));
                this.LastLoadedMap = (Map)this.CurrentMap.Clone();
            }

            return success;
        }

        public bool SaveNewEncodedFile()
        {
            if (this.CurrentMap.HasErrors)
            {
                MessageBox.Show($"Map contains errors, please find and resolve with View > Errors before saving.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var saveFileDialog = new CommonSaveFileDialog
            {
                RestoreDirectory = false,
                InitialDirectory = Directory.GetCurrentDirectory() + "\\data\\",
                DefaultExtension = "she",
                NavigateToShortcut = true,
                OverwritePrompt = true,
                DefaultFileName = Path.GetFileName(this.LoadedMapPath),
                Title = "Save .she"
            };
            saveFileDialog.Filters.Add(new CommonFileDialogFilter("Encoded Map File", "*.she"));
            saveFileDialog.Filters.Add(new CommonFileDialogFilter("All Files", "*.*"));

            var dialogSuccess = saveFileDialog.ShowDialog();
            if (dialogSuccess == CommonFileDialogResult.Ok)
            {
                TCDEncodeDecode.SaveFile(saveFileDialog.FileName, TCDEncodeDecode.EncodeMap(this.CurrentMap.StringValue));
                this.LoadedMapPath = saveFileDialog.FileName;
                this.LastLoadedMap = (Map)this.CurrentMap.Clone();

                return true;
            }

            return false;
        }

        public void ChooseEncodedFile()
        {
            var selectFileDialog = new CommonOpenFileDialog
            {
                RestoreDirectory = false,
                InitialDirectory = Directory.GetCurrentDirectory() + "\\data\\",
                EnsureFileExists = true,
                Multiselect = false,
                Title = "Open .she"
            };
            selectFileDialog.Filters.Add(new CommonFileDialogFilter("Encoded Map File", "*.she"));
            selectFileDialog.Filters.Add(new CommonFileDialogFilter("All Files", "*.*"));

            var dialogSuccess = selectFileDialog.ShowDialog();
            if (dialogSuccess == CommonFileDialogResult.Ok)
            {
                this.LoadMap(selectFileDialog.FileName, true);
            }
        }

        public void SaveDecodedFile()
        {
            var saveFileDialog = new CommonSaveFileDialog
            {
                RestoreDirectory = false,
                InitialDirectory = Directory.GetCurrentDirectory() + "\\data\\",
                DefaultExtension = "shd",
                NavigateToShortcut = true,
                OverwritePrompt = true,
                DefaultFileName = Path.ChangeExtension(Path.GetFileName(this.LoadedMapPath), ".shd"),
                Title = "Export .shd"
            };
            saveFileDialog.Filters.Add(new CommonFileDialogFilter("Decoded Map File", "*.shd"));
            saveFileDialog.Filters.Add(new CommonFileDialogFilter("All Files", "*.*"));

            var dialogSuccess = saveFileDialog.ShowDialog();
            if (dialogSuccess == CommonFileDialogResult.Ok)
            {
                TCDEncodeDecode.SaveFile(saveFileDialog.FileName, this.CurrentMap.StringValue);
            }
        }

        public void OpenSettings()
        {
            SettingsWindow.ShowWindow();
        }

        public void OnCurrentMapPropertyUpdated(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == $"Header.{nameof(Header.Levels)}")
            {
                this.UpdatePropertiesToMatchLevels();
            }

            this.OnPropertyChanged(nameof(this.Title));
        }

        public void UpdatePropertiesToMatchLevels()
        {
            this.CurrentLevel = Math.Min(this.CurrentLevel, this.CurrentMap.Header.Levels - 1);

            var invalidLevels = MapRenderer.DisplayOptions.Where(kvp => kvp.Item1 < 0 || kvp.Item1 >= this.CurrentMap.Header.Levels).ToList();
            foreach (var l in invalidLevels)
            {
                MapRenderer.DisplayOptions.Remove(l);
            }
            for (int i = 0; i < this.CurrentMap.Header.Levels; i++)
            {
                if (!MapRenderer.DisplayOptions.Any(kvp => kvp.Item1 == i))
                {
                    MapRenderer.DisplayOptions.Add(new Tuple<int, LevelDisplayOptions>(i, new LevelDisplayOptions()));
                }
            }
            this.OnPropertyChanged(nameof(this.DisplayOptions));
        }

        public bool ConfirmMapChange()
        {
            if (this.IsDirty)
            {
                var result = MessageBox.Show($"Do you want to save changes to \"{MainWindowViewModel.GetInstance().LoadedMapPath}\"?", "Unsaved Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        return MainWindowViewModel.GetInstance().SaveOrCreateEncodedFile();
                    case MessageBoxResult.No:
                        return true;
                    case MessageBoxResult.Cancel:
                    default:
                        return false;
                }
            }
            return true;
        }

        public static IEnumerable<string> GetAllFiles(string path)
        {
            var children = Directory.GetDirectories(path);
            var childFiles = new List<string>(Directory.GetFiles(path));

            foreach (var child in children)
            {
                childFiles.AddRange(GetAllFiles(child));
            }

            return childFiles;
        }

        private void ReloadGraphics()
        {
            try
            {
                Application.Current?.Dispatcher?.BeginInvoke((Action)(() => LoadProgressWindow.ShowWindow()));

                EventHandler<LoadProgressUpdatedEventArgs> progressUpdateAction = (sender, args) =>
                {
                    Application.Current?.Dispatcher?.BeginInvoke((Action)(() =>
                    {
                        if (args != null)
                        {
                            LoadProgressWindow.SetProgress(args.UpdateProgress, args.UpdateLabel);
                        }
                        else
                        {
                            LoadProgressWindow.HideWindow();
                        }
                    }));
                };

                Constants.ReloadTextures(progressUpdateAction);

                SpriteRendererPanel.ReloadTextures();
            }
            catch (Exception e)
            {
                Application.Current?.Dispatcher?.BeginInvoke((Action)(() => LoadProgressWindow.HideWindow()));
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReloadSound()
        {
            try
            {
                Application.Current?.Dispatcher?.BeginInvoke((Action)(() => LoadProgressWindow.ShowWindow()));

                EventHandler<LoadProgressUpdatedEventArgs> progressUpdateAction = (sender, args) =>
                {
                    Application.Current?.Dispatcher?.BeginInvoke((Action)(() =>
                    {
                        if (args != null)
                        {
                            LoadProgressWindow.SetProgress(args.UpdateProgress, args.UpdateLabel);
                        }
                        else
                        {
                            LoadProgressWindow.HideWindow();
                        }
                    }));
                };

                Constants.ReloadSound(progressUpdateAction);
            }
            catch (Exception e)
            {
                Application.Current?.Dispatcher?.BeginInvoke((Action)(() => LoadProgressWindow.HideWindow()));
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReloadTranslationKeys()
        {
            Constants.TranslationCallKeys.Clear();
            Constants.TranslationService.ReloadTranslationKeys();
        }

        private void OpenStringBrowser()
        {
            TranslationKeySelectionWindow.SetKeySetterAction(key => { });
            TranslationKeySelectionWindow.ShowWindow(null);
        }

        private void OpenDataBrowser()
        {
            DataWindow.ShowWindow();
        }

        public void UnpackTCD()
        {
            var selectFileDialog = new CommonOpenFileDialog
            {
                RestoreDirectory = false,
                InitialDirectory = Directory.GetCurrentDirectory(),
                EnsureFileExists = true,
                Multiselect = false,
                Title = "Unpack .tcd"
            };
            selectFileDialog.Filters.Add(new CommonFileDialogFilter("Packed Resource File", "*.tcd"));

            var dialogSuccess = selectFileDialog.ShowDialog();
            if (dialogSuccess == CommonFileDialogResult.Ok)
            {
                var selectedFile = selectFileDialog.FileName;
                string resourceFile, patternFile, fileBase;
                resourceFile = patternFile = fileBase = string.Empty;
                if (selectedFile.EndsWith(Constants.ResourceSuffix, StringComparison.InvariantCultureIgnoreCase))
                {
                    fileBase = Path.GetFileName(selectedFile.Substring(0, selectedFile.Length - Constants.ResourceSuffix.Length));
                    resourceFile = selectedFile;
                    patternFile = Path.Combine(Path.GetDirectoryName(selectedFile), fileBase + Constants.PatternSuffix);
                    if (!File.Exists(patternFile))
                    {
                        MessageBox.Show($"Missing associated pattern file \"{selectedFile}\".", "Missing pattern file", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else if (selectedFile.EndsWith(Constants.PatternSuffix, StringComparison.InvariantCultureIgnoreCase))
                {
                    fileBase = Path.GetFileName(selectedFile.Substring(0, selectedFile.Length - Constants.PatternSuffix.Length));
                    resourceFile = Path.Combine(Path.GetDirectoryName(selectedFile), fileBase + Constants.ResourceSuffix);
                    patternFile = selectedFile;
                    if (!File.Exists(resourceFile))
                    {
                        MessageBox.Show($"Missing associated resource file \"{resourceFile}\".", "Missing resource file", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show($"Invalid selection \"{selectedFile}\".{Environment.NewLine}Please select a \"<name>Resource\" or \"<name>Pattern\" .tcd file.", "Invalid file", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newDirectory = Path.Combine(Path.GetDirectoryName(selectedFile), fileBase);
                TCDEncodeDecode.UnpackTCDFile(
                    newDirectory,
                    fileBase,
                    (fileName, progress) =>
                    {
                        this.ProgressStatus = fileName;
                        this.Progress = progress;
                });
            }
        }

        public void PackTCD()
        {
            var folderBrowserDialog = new CommonOpenFileDialog
            {
                RestoreDirectory = false,
                InitialDirectory = Directory.GetCurrentDirectory(),
                IsFolderPicker = true,
                EnsureFileExists = true,
                Title = "Pack Folder into .tcd"
            };

            var folderBrowserDialogSuccess = folderBrowserDialog.ShowDialog();
            if (folderBrowserDialogSuccess == CommonFileDialogResult.Ok)
            {
                var fileBase = new DirectoryInfo(Path.GetFullPath(folderBrowserDialog.FileName)).Name;
                TCDEncodeDecode.PackTCDFile(
                    folderBrowserDialog.FileName,
                    fileBase,
                    (fileName, progress) =>
                    {
                        this.ProgressStatus = fileName;
                        this.Progress = progress;
                });
            }
        }

        public void OpenErrors()
        {
            ErrorsWindow.ShowWindow();
        }

        public void DataDump()
        {
            DataDumpWindow.ShowWindow();
        }
    }
}
