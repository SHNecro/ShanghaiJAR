using MapEditor.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MapEditor.ViewModels
{
    public class LoadingWindowViewModel : ViewModelBase
    {
        public event Action LoadComplete;

        public static Settings Settings;

        public static MainWindow MainWindow;
        public static MainWindowViewModel MainWindowViewModel;

        private static readonly List<string> CategoriesToLoad = new List<string> { "Font", "Language", "Data", "Map", "Resources" };

        public Dictionary<string, double> CategoriesLoaded = new Dictionary<string, double>();

        private ObservableCollection<string> progressMessages;

        public ObservableCollection<string> ProgressMessages
        {
            get { return this.progressMessages; }
            set { this.SetValue(ref this.progressMessages, value); }
        }

        public bool? FontStatus => this.CategoriesLoaded.ContainsKey("Font") ? (bool?)(Math.Abs(this.CategoriesLoaded["Font"] - 1) < 0.0001) : null;

        public bool? LanguageStatus => this.CategoriesLoaded.ContainsKey("Language") ? (bool?)(Math.Abs(this.CategoriesLoaded["Language"] - 1) < 0.0001) : null;

        public bool? DataStatus => this.CategoriesLoaded.ContainsKey("Data") ? (bool?)(Math.Abs(this.CategoriesLoaded["Data"] - 1) < 0.0001) : null;

        public bool? MapStatus => this.CategoriesLoaded.ContainsKey("Map") ? (bool?)(Math.Abs(this.CategoriesLoaded["Map"] - 1) < 0.0001) : null;

        public bool? ResourcesStatus => this.CategoriesLoaded.ContainsKey("Resources") ? (bool?)(Math.Abs(this.CategoriesLoaded["Resources"] - 1) < 0.0001) : null;

        public double TotalLoadProgress => this.CategoriesLoaded.Values.Sum() / LoadingWindowViewModel.CategoriesToLoad.Count();

        public Action CloseAction { get; set; }

        public void Initialize()
        {
            LoadingWindowViewModel.Settings = Settings.FromXML("option-editor.xml") ?? new Settings();
            this.ProgressMessages = new ObservableCollection<string>();

            Constants.ConstantsLoadProgressEventUpdated += (sender, args) =>
            {
                Application.Current?.Dispatcher?.BeginInvoke((Action)(() =>
                {
                    this.ProgressMessages.Add($"{args.UpdateLabel}: {args.UpdateProgress * 100:N2}%");
                }));

                this.UpdateLoadedCategories(args.UpdateLabel, args.UpdateProgress);
            };

            this.ProgressMessages.Add($"Map: Started loading");
            this.UpdateLoadedCategories("Map", 0);

            MainWindowViewModel.MapLoadProgressEvent += () =>
            {
                Application.Current?.Dispatcher?.BeginInvoke((Action)(() =>
                {
                    this.ProgressMessages.Add($"Map: Finished loading");
                }));

                this.UpdateLoadedCategories("Map", 1);
            };

            MainWindowViewModel.Initialize();

            this.InitializeConstantsAndViewModels();

            this.LoadComplete += this.FinishLoading;
        }

        private void InitializeConstantsAndViewModels()
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Task.Factory.StartNew((Action)(() =>
            {
                var success = false;
                var failedOnce = false;
                while (!success)
                {
                    success = Constants.Initialize();

                    LoadingWindowViewModel.MainWindowViewModel = MainWindowViewModel.GetInstance();
                    var initialMapPath = $"{LoadingWindowViewModel.Settings.MapDataFolder}/{LoadingWindowViewModel.Settings.InitialMap}";
                    success &= LoadingWindowViewModel.MainWindowViewModel.LoadMap(initialMapPath, true);

                    if (failedOnce)
                    {
                        MessageBox.Show("Loading failed again. Check that resource files exist and try again.");
                        Environment.Exit(1);
                    }
                    else if (!success)
                    {
                        failedOnce = true;
                        MessageBox.Show("Loading failed. Check your settings and try again.");
                        Application.Current?.Dispatcher?.Invoke(() => SettingsWindow.ShowWindow());
                    }
                }
            })).ContinueWith((r) =>
            {
                LoadingWindowViewModel.MainWindow = new MainWindow();
            }, scheduler);
        }

        private void UpdateLoadedCategories(string category, double progress)
        {
            var matchingCategory = LoadingWindowViewModel.CategoriesToLoad.FirstOrDefault(remainingCategory => category.StartsWith(remainingCategory, StringComparison.CurrentCulture));

            if (matchingCategory == null)
            {
                return;
            }

            this.CategoriesLoaded[matchingCategory] = progress;

            this.OnPropertyChanged(nameof(this.FontStatus));
            this.OnPropertyChanged(nameof(this.LanguageStatus));
            this.OnPropertyChanged(nameof(this.DataStatus));
            this.OnPropertyChanged(nameof(this.MapStatus));
            this.OnPropertyChanged(nameof(this.ResourcesStatus));

            this.OnPropertyChanged(nameof(this.TotalLoadProgress));

            if (LoadingWindowViewModel.CategoriesToLoad.All(remainingCategory => this.CategoriesLoaded.ContainsKey(remainingCategory) && Math.Abs(this.CategoriesLoaded[remainingCategory] - 1) < 0.0001))
            {
                Application.Current?.Dispatcher?.BeginInvoke((Action)(() =>
                {
                    this.LoadComplete?.Invoke();
                }));
            }
        }

        private void FinishLoading()
        {
            Application.Current?.Dispatcher?.BeginInvoke((Action)(() =>
            {
                LoadingWindowViewModel.MainWindow.DataContext = LoadingWindowViewModel.MainWindowViewModel;
                LoadingWindowViewModel.MainWindow.Show();

                this.CloseAction();
            }));

            LoadingWindowViewModel.MainWindowViewModel.StartRendering();

            this.LoadComplete -= this.FinishLoading;
        }
    }
}
