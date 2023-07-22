using Common.EncodeDecode;
using Common.OpenAL;
using Common.OpenGL;
using Data;
using MapEditor.Core;
using MapEditor.ExtensionMethods;
using MapEditor.Models;
using MapEditor.Models.Elements;
using MapEditor.Models.Elements.Enums;
using MapEditor.ViewModels;
using Moq;
using NSBackground;
using NSGame;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using FontStyle = System.Drawing.FontStyle;
using Point = System.Drawing.Point;

namespace MapEditor
{
    public static class Constants
    {
        public static event EventHandler<ConstantsLoadProgressEventUpdatedEventArgs> ConstantsLoadProgressEventUpdated;

        public static int NormalNaviID = 42;

        public static ITextureLoadStrategy TextureLoadStrategy;
        public static TextMeasurer TextMeasurer;

        public static ISoundLoadStrategy SoundLoadStrategy;
        public static AudioEngine AudioEngine;
        public static ObservableCollection<string> SoundEffects { get; private set; }

        public static string EncounterKey = "Map.RandomItemEncounterName";

        public static string ResourceSuffix = "Resource.tcd";
        public static string PatternSuffix = "Pattern.tcd";

        public static Dictionary<string, string> TranslationCallKeys { get; private set; }
        public static TrackingLanguageTranslationService TranslationService { get; private set; }

        // Used for enemy selection as enemies hold own copy for specific rank, HP, offsets
        public static Dictionary<int, EnemyDefinition> BaseEnemyDefinitions { get; private set; }
        public static Dictionary<int, ChipDefinition> ChipDefinitions { get; private set; }
        public static Dictionary<int, AddOnDefinition> AddOnDefinitions { get; private set; }
        public static Dictionary<int, string> InteriorDefinitions { get; private set; }
        public static Dictionary<int, BackgroundDefinition> BackgroundDefinitions { get; private set; }

        public static bool[,] FloatingCharacters;
        public static bool[,] NoShadowCharacters;
        public static ObservableConcurrentDictionary<int, KeyItemDefinition> KeyItemDefinitions { get; private set; }
        public static ObservableConcurrentDictionary<int, MailDefinition> MailDefinitions { get; private set; }

		public static Rectangle ConveyorSpriteArea;
		public static Rectangle TileConveyorSpriteArea;
		public static Dictionary<FontType, Font> Fonts;
        private static PrivateFontCollection customFontInstance;
        public static Color TextColor = Color.FromArgb(64, 56, 56);

        public static Func<RandomEncounter> BlankEncounterCreator = () => new RandomEncounter { StringValue = "battle:1:1:5:1:0:0:0:0::0:1:5:0:0:0:0:0::0:1:5:2:0:0:0:0::0:0:0:False:True:True:True:VSvirus:0" };
        public static Func<RandomMystery> BlankMysteryCreator = () => new RandomMystery { StringValue = "3,8,100," };
        public static Func<MapEntity> BlankMapEventCreator = () => new MapEntity { StringValue = string.Join("\r\n", new[]
        {
            "ID:Sprite",
            "position:0:0:0",
            "page:1",
            "startterms:Abutton",
            "type:1",
            "terms:none",
            "move:",
            "speed:0",
            "graphic:2:6,0,7,0,0",
            "hitrange:3:0:0:0",
            "hitform:circle",
            "event:",
            "msgopen:",
            "msg:Debug.UnimplementedText",
            "msgclose:",
            "end",
        }) };
        public static Func<MapEventPage> BlankMapEventPageCreator = () => new MapEventPage
        { StringValue = string.Join("\r\n", new[]
        {
            "page:1",
            "startterms:Abutton",
            "type:1",
            "terms:none",
            "move:",
            "speed:0",
            "graphic:-2:0,0,0,0,0",
            "hitrange:3:3:0:0",
            "hitform:square",
            "event:",
            "end"
        }) };
        public static Func<MapMystery> BlankMapMysteryCreator = () => new MapMystery { StringValue = string.Join("\r\n", new[]
        {
            "MysteryID:GMD",
            "position:0:0:0",
            "Mystery:0,0,0,0,,5",
            "page:1",
            "startterms:Abutton",
            "type:1",
            "terms:",
            "move:",
            "speed:3",
            "graphic:-2:0,0,16,40,16",
            "hitrange:2:0:8:8",
            "hitform:circle",
            "event:",
            "end",
            "page:2",
            "startterms:Abutton",
            "type:0",
            "terms:",
            "move:",
            "speed:0",
            "graphic:-1:0,0,0,0,0",
            "hitrange:0:0:0:0",
            "hitform:square",
            "event:",
            "end"
        }) };
        public static Func<Map> BlankMapCreator = () => new Map { Name = "Blank", StringValue = string.Join("\r\n", new[]
        {
            "24,24,-272,-112,784,400,Common.Title,0,1,0,117,1,mobcyber,3",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1",
            "",
            "",
            "",
            "",
            "ID:Exit",
            "position:106:106:0",
            "page:1",
            "startterms:Touch",
            "type:0",
            "terms:none",
            "move:",
            "speed:10",
            "graphic:-2:256,0,80,48,6",
            "hitrange:5:0:0:0",
            "hitform:circle",
            "event:",
            "warpPlugOut:",
            "end",
            "",
            "ID:BGMStart",
            "position:152:88:0",
            "page:1",
            "startterms:Auto",
            "type:1",
            "terms:none",
            "move:jump,32:right,16:left,16",
            "speed:0",
            "graphic:-1:0,0,0,0,0",
            "hitrange:0:0:0:0",
            "hitform:square",
            "event:",
            "bgmon:internet:0",
            "eventDeath:",
            "end",
            "page:2",
            "startterms:Auto",
            "type:0",
            "terms:variable/14/False/0/3",
            "move:",
            "speed:0",
            "graphic:-1:0,0,0,0,0",
            "hitrange:0:0:0:0",
            "hitform:square",
            "event:",
            "bgmoff:",
            "eventDeath:",
            "end",
            ""
        }) };


        public static Func<WalkableTileType, ConveyorColorType, Point, int, MapEntity> ConveyorCreator = (tileType, color, position, level) =>
        {
            var bodyPage = 2;
            var conveyorPoint = new Point(513, 369);
            var positionAdjust = new Point(0, 0);
            switch (color)
			{
				case ConveyorColorType.Red:
					bodyPage = 2;
					conveyorPoint = new Point(513, 369);
					positionAdjust = new Point(0, 0);
					break;
				case ConveyorColorType.Blue:
					bodyPage = 2;
					conveyorPoint = new Point(513, 369 + 128);
					positionAdjust = new Point(0, 0);
					break;
				case ConveyorColorType.GreenTile:
                    bodyPage = 27;
					conveyorPoint = new Point(0, 272);
					switch (tileType)
					{
						case WalkableTileType.ConveyorWest:
						case WalkableTileType.ConveyorEast:
							positionAdjust = new Point(-1, 0);
							break;
						case WalkableTileType.ConveyorSouth:
						case WalkableTileType.ConveyorNorth:
							positionAdjust = new Point(0, -1);
							break;
					}
					break;
				case ConveyorColorType.BlueTile:
					bodyPage = 27;
					conveyorPoint = new Point(0 + 256, 272);
					switch (tileType)
					{
						case WalkableTileType.ConveyorWest:
						case WalkableTileType.ConveyorEast:
							positionAdjust = new Point(-1, 0);
							break;
						case WalkableTileType.ConveyorSouth:
						case WalkableTileType.ConveyorNorth:
							positionAdjust = new Point(0, -1);
							break;
					}
					break;
			}
            switch (tileType)
			{
				case WalkableTileType.ConveyorWest:
					conveyorPoint.Offset(0, 0);
					break;
				case WalkableTileType.ConveyorEast:
					conveyorPoint.Offset(0, 32);
                    break;
                case WalkableTileType.ConveyorSouth:
					conveyorPoint.Offset(0, 32 * 2);
                    break;
                case WalkableTileType.ConveyorNorth:
					conveyorPoint.Offset(0, 32 * 3);
                    break;
            }
            var graphicsLocation = $"{conveyorPoint.X},{conveyorPoint.Y}";

            var adjustedPosition = new Point(position.X + 7 + positionAdjust.X, position.Y + 7 + positionAdjust.Y);
            var positionString = $"{adjustedPosition.X}:{adjustedPosition.Y}";
            return new MapEntity { StringValue = string.Join("\r\n", new[]
            {
                "ID:Conveyor",
                $"position:{positionString}:{level}",
                "page:1",
                "startterms:Touch",
                "type:0",
                "terms:none",
                "move:",
                "speed:6",
                $"graphic:-{bodyPage}:{graphicsLocation},64,32,4",
                $"hitrange:16:16:{-positionAdjust.X}:{-positionAdjust.Y}",
                "hitform:square",
                "event:",
                "end"
            }) };
        };
        public static Func<Move> MoveCreator = () => new Move { StringValue = "wait,0" };
        public static Func<ShopItem> ShopItemCreator = () => new ShopItem { StringValue = "0,0,0,0" };
        public static Func<TermObject> TermCreator = () => TermObject.FromString("none");
        public static Func<EventObject> EventCreator = () => EventObject.FromString("msg:Debug.UnimplementedText");
        public static Func<Wrapper<string>> DialogueCreator = () => "Debug.UnimplementedText".Wrap();
        public static Func<MessageViewModel> MessageCreator = () => new MessageViewModel(0, new[] { DialogueCreator() });
        public static Func<KeyItemViewModel> KeyItemCreator = () => new KeyItemViewModel(0, new KeyItemDefinition { NameKey = "Debug.UnimplementedText", DialogueKeys = new List<string> { "Debug.UnimplementedText" } });
        public static Func<MailItemViewModel> MailCreator = () => new MailItemViewModel(0, new MailDefinition { SenderKey = "Debug.UnimplementedText", SubjectKey = "Debug.UnimplementedText", DialogueKeys = new List<string> { "Debug.UnimplementedText" } });

        public static ICommand MoveItemUpCommand => new RelayCommand(
            (cmdParams) => Constants.CanMoveItem(cmdParams, true),
            (cmdParams) => Constants.MoveItem(cmdParams, true)
        );

        public static ICommand MoveItemDownCommand => new RelayCommand(
            (cmdParams) => Constants.CanMoveItem(cmdParams, false),
            (cmdParams) => Constants.MoveItem(cmdParams, false)
        );

        public static ICommand DuplicateItemCommand => new RelayCommand(
            Constants.CanDuplicateItem,
            Constants.DuplicateItem
        );

        public static ICommand DeleteItemCommand => new RelayCommand(
            Constants.DeleteItem
        );

        public static ICommand DeleteExtraItemCommand => new RelayCommand(
            Constants.CanDeleteItem,
            Constants.DeleteItem
        );

        public static ICommand AddItemCommand => new RelayCommand(
            Constants.AddItem
        );

        static Constants()
		{
			Constants.ConveyorSpriteArea = new Rectangle(512, 368, (64 * 4) + 2, (32 * 4 * 2) + 2);
			Constants.TileConveyorSpriteArea = new Rectangle(0, 272, (64 * 4) + 2, (32 * 4 * 2) + 2);
			Constants.AudioEngine = AudioEngine.Instance;
        }

        public static bool Initialize()
        {
            try
            {
                Constants.InitializeFonts();
                Constants.InitializeLanguage();
                Constants.InitializeResources();
                Constants.InitializeGameData();
                // BGM definitions attached to viewmodel (public static) so standalone does not load everything
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private static void InitializeFonts()
        {
            var installedFont = (new InstalledFontCollection().Families).FirstOrDefault(f => f.Name == "Microsoft Sans Serif");
            if (installedFont != null)
            {
                Constants.Fonts = new Dictionary<FontType, Font>
                {
                    { FontType.Normal, new Font("Microsoft Sans Serif", 15, FontStyle.Regular) },
                    { FontType.Mini, new Font("Microsoft Sans Serif", 12, FontStyle.Regular) },
                    { FontType.Micro, new Font("Microsoft Sans Serif", 11, FontStyle.Regular) }
                };
            }
            else
            {
                Constants.customFontInstance = new PrivateFontCollection();
                var assembly = Assembly.GetExecutingAssembly();
                string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("JF-Dot-jiskan16s.ttf", StringComparison.InvariantCultureIgnoreCase));
                byte[] fontBytes;
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    fontBytes = new byte[stream.Length];
                    stream.Read(fontBytes, 0, fontBytes.Length);
                    stream.Close();
                }
                IntPtr handle = Marshal.AllocCoTaskMem(fontBytes.Length);
                Marshal.Copy(fontBytes, 0, handle, fontBytes.Length);
                Constants.customFontInstance.AddMemoryFont(handle, fontBytes.Length);
                Marshal.FreeCoTaskMem(handle);
                Constants.Fonts = new Dictionary<FontType, Font>
                {
                    { FontType.Normal, new Font(Constants.customFontInstance.Families[0], 15, FontStyle.Regular) },
                    { FontType.Mini, new Font(Constants.customFontInstance.Families[0], 12, FontStyle.Regular) },
                    { FontType.Micro, new Font(Constants.customFontInstance.Families[0], 11, FontStyle.Regular) }
                };
            }

            Constants.ConstantsLoadProgressEventUpdated?.Invoke(null, new ConstantsLoadProgressEventUpdatedEventArgs("Font", 1));
        }

        private static void InitializeLanguage()
        {
            Constants.TranslationCallKeys = new Dictionary<string, string>();
            try
            {
                var locales = Directory.GetDirectories("language").Select(Path.GetFileName).ToArray();
                Constants.TranslationService = new TrackingLanguageTranslationService(locales);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().ToString()}: {e.Message}");
            }

            var mockTranslationService = new Mock<ILanguageTranslationService>();
            mockTranslationService.Setup(lts => lts.Translate(It.IsAny<string>())).Returns((string key) =>
            {
                var translated = Constants.TranslationService?.Translate(key) ?? new Common.Dialogue { Text = $"IN-GAME ERROR: {key}" };
                if (!Constants.TranslationCallKeys.ContainsKey(translated))
                {
                    Constants.TranslationCallKeys[translated] = key;
                }
                return translated;
            });

            ShanghaiEXE.languageTranslationService = mockTranslationService.Object;
            Constants.ConstantsLoadProgressEventUpdated?.Invoke(null, new ConstantsLoadProgressEventUpdatedEventArgs("Language", 1));
        }

        private static void InitializeResources()
        {
            var textureComplete = 0.0;
            var soundComplete = 0.0;

            EventHandler<LoadProgressUpdatedEventArgs> progressTextureUpdateAction = (sender, args) =>
            {
                var updateLabel = args != null ? args.UpdateLabel : "Load Complete";
                textureComplete = args?.UpdateProgress / 2 ?? 0.5;
                Constants.ConstantsLoadProgressEventUpdated?.Invoke(null, new ConstantsLoadProgressEventUpdatedEventArgs("Resources: Textures: " + updateLabel, soundComplete + textureComplete));
            };

            Constants.ReloadTextures(progressTextureUpdateAction);
            Constants.TextMeasurer = new TextMeasurer();

            SpriteRendererPanel.ReloadTextures();

            EventHandler<LoadProgressUpdatedEventArgs> progressSoundUpdateAction = (sender, args) =>
            {
                var updateLabel = args != null ? args.UpdateLabel : "Load Complete";
                soundComplete = args?.UpdateProgress / 2 ?? 0.5;
                Constants.ConstantsLoadProgressEventUpdated?.Invoke(null, new ConstantsLoadProgressEventUpdatedEventArgs("Resources: Sounds: " + updateLabel, soundComplete + textureComplete));
            };

            Constants.SoundEffects = new ObservableCollection<string>();
            Constants.ReloadSound(progressSoundUpdateAction);
        }

        public static void ReloadTextures(EventHandler<LoadProgressUpdatedEventArgs> updateEventHandler)
        {
            ITextureLoadStrategy resourceLoadStrategy;

            if (!LoadingWindowViewModel.Settings.GraphicsIsPackedResource)
            {
                resourceLoadStrategy = new FolderLoadStrategy(LoadingWindowViewModel.Settings.GraphicsFormat);
            }
            else
            {
                resourceLoadStrategy = new TCDLoadStrategy(
                    LoadingWindowViewModel.Settings.GraphicsResourceFile,
                    LoadingWindowViewModel.Settings.GraphicsResourceFilePassword,
                    LoadingWindowViewModel.Settings.GraphicsResourceFileFormat);
            }

            var progressUpdateAction = default(EventHandler<LoadProgressUpdatedEventArgs>);

            progressUpdateAction = (sender, args) =>
            {
                updateEventHandler(sender, args);
                if (args == null)
                {
                    resourceLoadStrategy.ProgressUpdated -= progressUpdateAction;
                }
            };
            resourceLoadStrategy.ProgressUpdated += progressUpdateAction;
            resourceLoadStrategy.Load();

            Constants.TextureLoadStrategy = resourceLoadStrategy;
        }

        public static void ReloadSound(EventHandler<LoadProgressUpdatedEventArgs> updateEventHandler)
        {
            ISoundLoadStrategy resourceLoadStrategy;

            if (!LoadingWindowViewModel.Settings.SoundIsPackedResource)
            {
                resourceLoadStrategy = new FolderLoadStrategy(LoadingWindowViewModel.Settings.SoundFormat);
            }
            else
            {
                resourceLoadStrategy = new TCDLoadStrategy(
                    LoadingWindowViewModel.Settings.SoundResourceFile,
                    LoadingWindowViewModel.Settings.SoundResourceFilePassword,
                    LoadingWindowViewModel.Settings.SoundResourceFileFormat);
            }

            var progressUpdateAction = default(EventHandler<LoadProgressUpdatedEventArgs>);

            progressUpdateAction = (sender, args) =>
            {
                updateEventHandler(sender, args);
                if (args == null)
                {
                    resourceLoadStrategy.ProgressUpdated -= progressUpdateAction;

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Constants.SoundEffects.Clear();
                        var soundEffects = resourceLoadStrategy.GetProvidableFiles();
                        foreach (var se in soundEffects)
                        {
                            Constants.SoundEffects.Add(se);
                        }
                        Constants.SoundEffects.Add("none");
                    });
                }
            };
            resourceLoadStrategy.ProgressUpdated += progressUpdateAction;
            resourceLoadStrategy.Load();

            Constants.SoundLoadStrategy = resourceLoadStrategy;
        }

        private static void InitializeGameData()
        {
            var totalDataToLoad = (double)(LoadingWindowViewModel.Settings.EnemyCount + LoadingWindowViewModel.Settings.ChipCount + LoadingWindowViewModel.Settings.AddOnCount + LoadingWindowViewModel.Settings.InteriorCount + LoadingWindowViewModel.Settings.BackgroundCount);
            var dataLoaded = 0;

            // Prints multiple NullReferenceExceptions as EnemyBase in ShanghaiEXE has undefined ids caught in the game code
            Constants.BaseEnemyDefinitions = new Dictionary<int, EnemyDefinition>();
            for (int enemyID = 0; enemyID < LoadingWindowViewModel.Settings.EnemyCount; enemyID++)
            {
                Constants.BaseEnemyDefinitions[enemyID] = EnemyDefinition.GetEnemyDefinition(enemyID, 0, 0, 1);
                dataLoaded++;
                Constants.ConstantsLoadProgressEventUpdated?.Invoke(null, new ConstantsLoadProgressEventUpdatedEventArgs("Data: Enemy: ", dataLoaded / totalDataToLoad));
            }

            Constants.ChipDefinitions = new Dictionary<int, ChipDefinition>();
            for (int chipID = 0; chipID < LoadingWindowViewModel.Settings.ChipCount; chipID++)
            {
                var chipDefinition = ChipDefinition.GetChipDefinition(chipID);
                if (chipDefinition != null)
                {
                    Constants.ChipDefinitions[chipID] = chipDefinition;
                }
                dataLoaded++;
                Constants.ConstantsLoadProgressEventUpdated?.Invoke(null, new ConstantsLoadProgressEventUpdatedEventArgs("Data: Chip: ", dataLoaded / totalDataToLoad));
            }

            Constants.AddOnDefinitions = new Dictionary<int, AddOnDefinition>();
            for (int addOnID = 0; addOnID < LoadingWindowViewModel.Settings.AddOnCount; addOnID++)
            {
                var addOnDefiniton = AddOnDefinition.GetAddOnDefinition(addOnID);
                if (addOnDefiniton != null)
                {
                    Constants.AddOnDefinitions[addOnID] = addOnDefiniton;
                }
                dataLoaded++;
                Constants.ConstantsLoadProgressEventUpdated?.Invoke(null, new ConstantsLoadProgressEventUpdatedEventArgs("Data: AddOn: ", dataLoaded / totalDataToLoad));
            }

            Constants.InteriorDefinitions = new Dictionary<int, string>();
            for (int interiorID = 0; interiorID < LoadingWindowViewModel.Settings.InteriorCount; interiorID++)
            {
                var interiorKey = $"Interior.Item{interiorID + 1}";
                if (Constants.TranslationService.CanTranslate(interiorKey))
                {
                    Constants.InteriorDefinitions[interiorID] = Constants.TranslationService.Translate(interiorKey).Text;
                }
                dataLoaded++;
                Constants.ConstantsLoadProgressEventUpdated?.Invoke(null, new ConstantsLoadProgressEventUpdatedEventArgs("Data: Interior: ", dataLoaded / totalDataToLoad));
            }

            Constants.BackgroundDefinitions = new Dictionary<int, BackgroundDefinition>();
            for (int i = 0; i < LoadingWindowViewModel.Settings.BackgroundCount; i++)
            {
                Constants.BackgroundDefinitions[i] = new BackgroundDefinition(BackgroundBase.BackMake(i));
                dataLoaded++;
                Constants.ConstantsLoadProgressEventUpdated?.Invoke(null, new ConstantsLoadProgressEventUpdatedEventArgs("Data: Background: ", dataLoaded / totalDataToLoad));
            }

            CharacterInfo.LoadCharacterInfo(out FloatingCharacters, out NoShadowCharacters);

            var keyItemDoc = new XmlDocument();
            keyItemDoc.Load($"data/data/KeyItems.xml");
            var keyItemDefintions = Constants.LoadKeyItems(keyItemDoc);
            Constants.KeyItemDefinitions = new ObservableConcurrentDictionary<int, KeyItemDefinition>();
            foreach (var kvp in keyItemDefintions)
            {
                Constants.KeyItemDefinitions.Add(kvp.Key, kvp.Value);
            }

            var mailDoc = new XmlDocument();
            mailDoc.Load($"data/data/Mail.xml");
            var mailDefinitions = Constants.LoadMail(mailDoc);
            Constants.MailDefinitions = new ObservableConcurrentDictionary<int, MailDefinition>();
            foreach (var kvp in mailDefinitions)
            {
                Constants.MailDefinitions.Add(kvp.Key, kvp.Value);
            }
        }

        public static bool IsFloatingCharacter(int sheet, int index)
        {
            return sheet < Constants.FloatingCharacters.GetLength(0) && index < 8
                && Constants.FloatingCharacters[sheet, index];
        }

        public static bool IsNoShadowCharacter(int sheet, int index)
        {
            return sheet < Constants.NoShadowCharacters.GetLength(0) && index < 8
                && Constants.NoShadowCharacters[sheet, index];
        }

        public static Dictionary<int, KeyItemDefinition> LoadKeyItems(XmlDocument keyItemDoc)
        {
            var keyItemDefinitions = new Dictionary<int, KeyItemDefinition>();

            var keyItemNodes = keyItemDoc.SelectNodes("data/KeyItem");
            foreach (XmlNode keyItemNode in keyItemNodes)
            {
                var index = int.Parse(keyItemNode?.Attributes["Index"]?.Value ?? "-1");

                if (index == -1)
                {
                    throw new InvalidOperationException("Invalid Key Item index.");
                }

                var nameKey = keyItemNode?.Attributes["Name"].Value;

                var info = new List<string>();
                var dialogueNodes = keyItemNode.ChildNodes;
                var dialogueKeys = new List<string>();
                foreach (XmlNode dialogueXml in dialogueNodes)
                {
                    dialogueKeys.Add(dialogueXml.Attributes["Key"].Value);
                }

                keyItemDefinitions[index] = new KeyItemDefinition { NameKey = nameKey, DialogueKeys = dialogueKeys };
            }

            return keyItemDefinitions;
        }

        public static Dictionary<int, MailDefinition> LoadMail(XmlDocument mailDoc)
        {
            var mailDefinitions = new Dictionary<int, MailDefinition>();

            var mailNodes = mailDoc.SelectNodes("data/Mail");
            foreach (XmlNode mailNode in mailNodes)
            {
                var index = int.Parse(mailNode?.Attributes["Index"]?.Value ?? "-1");

                if (index == -1)
                {
                    throw new InvalidOperationException("Invalid Key Item index.");
                }

                var subjectKey = mailNode?.Attributes["Subject"].Value;
                var senderKey = mailNode?.Attributes["Sender"].Value;

                var info = new List<string>();
                var dialogueNodes = mailNode.ChildNodes;
                var dialogueKeys = new List<string>();
                foreach (XmlNode dialogueXml in dialogueNodes)
                {
                    dialogueKeys.Add(dialogueXml.Attributes["Key"].Value);
                }

                mailDefinitions[index] = new MailDefinition { SubjectKey = subjectKey, SenderKey = senderKey, DialogueKeys = dialogueKeys };
            }

            return mailDefinitions;
        }

        private static bool CanMoveItem(object param, bool isUp)
        {
            var paramsArray = param as object[];
            if (param == null || !(paramsArray.Length == 2 || paramsArray.Length == 3))
            {
                return false;
            }

            IEnumerable<object> collection;
            object toMove;
            if (paramsArray.Length == 2)
            {
                collection = paramsArray[0] as IEnumerable<object>;
                toMove = paramsArray[1] as object;
            }
            else
            {
                collection = paramsArray[1] as IEnumerable<object>;
                toMove = paramsArray[2] as object;
            }

            if (collection == null || !collection.Any() || toMove == null)
            {
                return false;
            }

            return (isUp ? collection.First() : collection.Last()) != toMove;
        }

        private static void MoveItem(object param, bool isUp)
        {
            var paramsArray = param as object[];
            if (param == null || !(paramsArray.Length == 2 || paramsArray.Length == 3))
            {
                return;
            }

            object sourceCollection;
            object collection;
            object toMove;
            if (paramsArray.Length == 2)
            {
                sourceCollection = paramsArray[0];
                collection = null;
                toMove = paramsArray[1];
            }
            else
            {
                sourceCollection = paramsArray[0];
                collection = paramsArray[1];
                toMove = paramsArray[2];
            }

            if (sourceCollection == null || (paramsArray.Length == 3 && collection == null) || toMove == null)
            {
                return;
            }

            // Because "as IEnumerable<object>" works but "as ObservableCollection<object>" doesn't
            try
            {
                var bindFlags = BindingFlags.Public | BindingFlags.Instance;
                if (paramsArray.Length == 2)
                {
                    var index = (int)sourceCollection.GetType().GetMethods(bindFlags).First(m => m.Name == "IndexOf").Invoke(sourceCollection, new[] { toMove });
                    sourceCollection.GetType().GetMethod("Move", bindFlags).Invoke(sourceCollection, new object[] { index, isUp ? (index - 1) : (index + 1) });
                }
                else
                {
                    var sourceIndex = (int)sourceCollection.GetType().GetMethod("IndexOf", bindFlags).Invoke(sourceCollection, new[] { toMove });
                    var collectionIndex = (int)collection.GetType().GetMethod("IndexOf", bindFlags).Invoke(collection, new[] { toMove });
                    var swapNeighbor = collection.GetType().GetMethod("get_Item").Invoke(collection, new object[] { isUp ? collectionIndex - 1 : collectionIndex + 1 });
                    var newIndex = (int)sourceCollection.GetType().GetMethod("IndexOf", bindFlags).Invoke(sourceCollection, new[] { swapNeighbor });
                    sourceCollection.GetType().GetMethod("Move", bindFlags).Invoke(sourceCollection, new object[] { sourceIndex, newIndex });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().ToString()}: {e.Message}");
            }

            CommandManager.InvalidateRequerySuggested();
        }

        private static bool CanDeleteItem(object param)
        {
            var paramsArray = param as object[];
            if (param == null || !(paramsArray.Length == 2 || paramsArray.Length == 3))
            {
                return false;
            }

            object collection;
            object toDelete;
            object confirm;
            if (paramsArray.Length == 2)
            {
                collection = paramsArray[0];
                toDelete = paramsArray[1];
                confirm = true;
            }
            else
            {
                collection = paramsArray[0];
                toDelete = paramsArray[1];
                confirm = paramsArray[2];
            }

            if (collection == null || toDelete == null || confirm == null)
            {
                return false;
            }

            try
            {
                var bindFlags = BindingFlags.Public | BindingFlags.Instance;
                var count = (int)collection.GetType().GetProperty("Count", bindFlags).GetGetMethod().Invoke(collection, new object[0]);
                return count != 1;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().ToString()}: {e.Message}");
                return false;
            }
        }

        private static void DeleteItem(object param)
        {
            var paramsArray = param as object[];
            if (param == null)
            {
                return;
            }

            var collection = paramsArray[0];
            var toDelete = paramsArray[1];

            if (collection == null || toDelete == null)
            {
                return;
            }

            var skipConfirm = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

            var deleteConfirm = skipConfirm ? MessageBoxResult.Yes : MessageBox.Show("Are you sure you want to delete this item?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (deleteConfirm == MessageBoxResult.Yes)
            {
                // Because "as IEnumerable<object>" works but "as ObservableCollection<object>" doesn't
                try
                {
                    var bindFlags = BindingFlags.Public | BindingFlags.Instance;
                    var index = (int)collection.GetType().GetMethod("IndexOf", bindFlags).Invoke(collection, new[] { toDelete });
                    collection.GetType().GetMethod("RemoveAt", bindFlags).Invoke(collection, new object[] { index });
                }
                catch { ; }
            }
        }

        private static bool CanDuplicateItem(object param)
        {
            var paramsArray = param as object[];
            if (param == null)
            {
                return false;
            }

            var collection = paramsArray[0];
            var toDuplicate = paramsArray[1];

            if (collection == null || toDuplicate == null)
            {
                return false;
            }

            // Because "as IEnumerable<object>" works but "as ObservableCollection<object>" doesn't
            try
            {
                var bindFlags = BindingFlags.Public | BindingFlags.Instance;
                var index = (int)collection.GetType().GetMethod("IndexOf", bindFlags).Invoke(collection, new[] { toDuplicate });
                return index != -1;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().ToString()}: {e.Message}");
                return false;
            }
        }

        private static void DuplicateItem(object param)
        {
            var paramsArray = param as object[];
            if (param == null)
            {
                return;
            }

            var collection = paramsArray[0];
            var toDuplicate = paramsArray[1];

            if (collection == null || toDuplicate == null || !(toDuplicate is ICloneable toClone))
            {
                return;
            }

            // Because "as IEnumerable<object>" works but "as ObservableCollection<object>" doesn't
            try
            {
                var bindFlags = BindingFlags.Public | BindingFlags.Instance;
                var index = (int)collection.GetType().GetMethod("IndexOf", bindFlags).Invoke(collection, new[] { toClone });
                collection.GetType().GetMethod("Insert", bindFlags).Invoke(collection, new object[] { index, toClone.Clone() });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().ToString()}: {e.Message}");
            }
        }

        private static void AddItem(object param)
        {
            var paramsArray = param as object[];
            if (param == null)
            {
                return;
            }

            var collection = paramsArray[0];
            var toAddCreator = paramsArray[1];
            var baseObject = paramsArray[2];

            if (collection == null || toAddCreator == null)
            {
                return;
            }
            
            // Because "as IEnumerable<object>" works but "as ObservableCollection<object>" doesn't
            try
            {
                var bindFlags = BindingFlags.Public | BindingFlags.Instance;
                var newObject = toAddCreator.GetType().GetMethod("Invoke", bindFlags).Invoke(toAddCreator, new object[] { });
                var toInsertIndex = baseObject == null ? 0 : ((int)collection.GetType().GetMethod("IndexOf", bindFlags).Invoke(collection, new[] { baseObject }) + 1);
                collection.GetType().GetMethod("Insert", bindFlags).Invoke(collection, new object[] { toInsertIndex, newObject });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().ToString()}: {e.Message}");
            }
        }
    }
}
