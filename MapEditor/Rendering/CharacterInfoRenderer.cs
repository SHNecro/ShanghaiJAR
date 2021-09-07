using MapEditor.Controls;
using Common.OpenGL;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Timers;
using System.Windows;
using System;
using System.Windows.Threading;
using System.Threading.Tasks;
using MapEditor.ViewModels;

namespace MapEditor.Rendering
{
    public static partial class CharacterInfoRenderer
    {
        private const int RenderPassPadding = 5;

        private static readonly Timer PreviewTimer;
        private static readonly Regex CharacterRegex = new Regex(@"charachip(\d+)", RegexOptions.Compiled);

        private static int Frame;

        static CharacterInfoRenderer()
        {
            CharacterInfoRenderer.ViewModel = new CharacterInfoDataViewModel();

            var spriteSheetSpriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            CharacterInfoRenderer.SpriteSheetRenderer = spriteSheetSpriteRendererPanel;
            CharacterInfoRenderer.SpriteSheetControl = spriteSheetSpriteRendererPanel;

            var previewRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            CharacterInfoRenderer.PreviewRenderer = previewRendererPanel;
            CharacterInfoRenderer.PreviewControl = previewRendererPanel;

            SpriteRendererPanel.TexturesReloaded += () =>
            {
                spriteSheetSpriteRendererPanel.TextureLoadStrategy = Constants.TextureLoadStrategy;
                previewRendererPanel.TextureLoadStrategy = Constants.TextureLoadStrategy;
            };

            PreviewTimer = new Timer
            {
                Interval = 1000 / 60,
                AutoReset = true,
                Enabled = true
            };

            PreviewTimer.Elapsed += (sender, args) => { CharacterInfoRenderer.Frame++; };

            CharacterInfoRenderer.ViewModel.CurrentSheetIndex = 2;
            CharacterInfoRenderer.ViewModel.Angle = 1;
        }

        public static WindowsFormsHost SpriteSheetControlHost
        {
            get
            {
                var renderingControl = CharacterInfoRenderer.SpriteSheetControl.GetControl();
                var scrollFormsHost = new ScrollViewerWindowsFormsHost { Child = renderingControl };
                renderingControl.Paint += (s, e) => CharacterInfoRenderer.DrawSpriteSheet();
                renderingControl.Paint += (s, e) =>
                {
                    var textureSize = CharacterInfoRenderer.SpriteSheetRenderer.GetTextureSize(CharacterInfoRenderer.ViewModel.CurrentCharacterSheet).Value;
                    scrollFormsHost.Width = textureSize.Width;
                    scrollFormsHost.Height = textureSize.Height;
                };
                renderingControl.MouseWheel += (sender, args) =>
                {
                    var sv = scrollFormsHost.ParentScrollViewer;
                    if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    {
                        sv.ScrollToHorizontalOffset(sv.HorizontalOffset - args.Delta);
                    }
                    else
                    {
                        sv.ScrollToVerticalOffset(sv.VerticalOffset - args.Delta);
                    }
                };
                return scrollFormsHost;
            }
        }

        public static WindowsFormsHost PreviewControlHost
        {
            get
            {
                var renderingControl = CharacterInfoRenderer.PreviewControl.GetControl();
                var scrollFormsHost = new ScrollViewerWindowsFormsHost { Child = renderingControl };
                renderingControl.Paint += (s, e) => CharacterInfoRenderer.DrawPreview();

                CharacterInfoRenderer.PreviewTimer.Elapsed += (s, e) =>
                {
                    try
                    {
                        Application.Current?.Dispatcher?.BeginInvoke(new Action(() =>
                        {
                            CharacterInfoRenderer.DrawPreview();
                        }), DispatcherPriority.Render);
                    }
                    catch (TaskCanceledException)
                    { }
                };

                return scrollFormsHost;
            }
        }

        public static CharacterInfoDataViewModel ViewModel { get; }

        public static IEnumerable<int> CharacterSheetIndices
        {
            get
            {
                return Constants.TextureLoadStrategy.GetProvidableFiles()
                    .Select(p => CharacterInfoRenderer.CharacterRegex.Match(Path.GetFileNameWithoutExtension(p)))
                    .Where(m => m.Success)
                    .Select(m => int.Parse(m.Groups[1].Value))
                    .OrderBy(x => x)
                    .Skip(1);
            }
        }

        private static IMouseInteractionControl SpriteSheetControl { get; }
        private static ISpriteRenderer SpriteSheetRenderer { get; }

        private static IMouseInteractionControl PreviewControl { get; }
        private static ISpriteRenderer PreviewRenderer { get; }
    }
}
