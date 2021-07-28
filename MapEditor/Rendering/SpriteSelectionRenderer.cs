using MapEditor.Controls;
using Common.OpenGL;
using MapEditor.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms.Integration;
using System.Windows.Input;

namespace MapEditor.Rendering
{
    public static partial class SpriteSelectionRenderer
    {
        private static readonly Regex ObjectRegex = new Regex(@"body(\d+)", RegexOptions.Compiled);
        private static readonly Regex CharacterRegex = new Regex(@"charachip(\d+)", RegexOptions.Compiled);

        private const int RenderPassPadding = 5;

        static SpriteSelectionRenderer()
        {
            var spriteSheetSpriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            SpriteSelectionRenderer.SpriteSheetRenderer = spriteSheetSpriteRendererPanel;
            SpriteSelectionRenderer.SpriteSheetControl = spriteSheetSpriteRendererPanel;

            SpriteRendererPanel.TexturesReloaded += () =>
            {
                spriteSheetSpriteRendererPanel.TextureLoadStrategy = Constants.TextureLoadStrategy;
            };
        }

        public static WindowsFormsHost SpriteSheetControlHost
        {
            get
            {
                var renderingControl = SpriteSelectionRenderer.SpriteSheetControl.GetControl();
                var scrollFormsHost = new ScrollViewerWindowsFormsHost { Child = renderingControl };
                renderingControl.Paint += (s, e) => SpriteSelectionRenderer.DrawSpriteSheet();
                renderingControl.Paint += (s, e) =>
                {
                    var textureSize = SpriteSelectionRenderer.SpriteSheetRenderer.GetTextureSize(SpriteSelectionRenderer.CurrentPage.Texture).Value;
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

        public static MapEventPage CurrentPage { get; set; }

        public static IEnumerable<int> ObjectSheetIndices
        {
            get
            {
                return Constants.TextureLoadStrategy.GetProvidableFiles()
                    .Select(p => SpriteSelectionRenderer.ObjectRegex.Match(Path.GetFileNameWithoutExtension(p)))
                    .Where(m => m.Success)
                    .Select(m => int.Parse(m.Groups[1].Value))
                    .OrderBy(x => x);
            }
        }

        public static IEnumerable<int> CharacterSheetIndices
        {
            get
            {
                return Constants.TextureLoadStrategy.GetProvidableFiles()
                    .Select(p => SpriteSelectionRenderer.CharacterRegex.Match(Path.GetFileNameWithoutExtension(p)))
                    .Where(m => m.Success)
                    .Select(m => int.Parse(m.Groups[1].Value))
                    .OrderBy(x => x)
                    .Skip(1);
            }
        }

        private static IMouseInteractionControl SpriteSheetControl { get; }
        private static ISpriteRenderer SpriteSheetRenderer { get; }
    }
}
