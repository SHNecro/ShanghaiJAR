using Common.OpenGL;
using MapEditor.Controls;
using MapEditor.Core;
using System.Windows.Forms.Integration;
using System.Windows.Input;

namespace MapEditor.Rendering
{
    public static partial class AddEditTranslationRenderer
    {
        private const int RenderPassPadding = 5;
        private static bool initialized = false;

        static AddEditTranslationRenderer()
        {
            var dialogueRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy, 2);
            AddEditTranslationRenderer.DialogueRenderer = dialogueRendererPanel;
            AddEditTranslationRenderer.DialogueControl = dialogueRendererPanel;

            SpriteRendererPanel.TexturesReloaded += () =>
            {
                dialogueRendererPanel.TextureLoadStrategy = Constants.TextureLoadStrategy;
            };
        }

        public static WindowsFormsHost DialogueControlHost
        {
            get
            {
                var renderingControl = AddEditTranslationRenderer.DialogueControl.GetControl();
                var scrollFormsHost = new ScrollViewerWindowsFormsHost { Child = renderingControl };
                renderingControl.Paint += (s, e) =>
                {
                    AddEditTranslationRenderer.DrawDialogue();
                    if (!AddEditTranslationRenderer.initialized)
                    {
                        AddEditTranslationRenderer.initialized = true;
                    }
                };
                return scrollFormsHost;
            }
        }

        public static TranslationEntry CurrentEntry { get; set; }

        public static ICommand ZoomOutCommand => new RelayCommand((args) => AddEditTranslationRenderer.DialogueRenderer.RenderScale > 1, (args) => AddEditTranslationRenderer.DialogueRenderer.RenderScale--);
        public static ICommand ZoomInCommand => new RelayCommand((args) => AddEditTranslationRenderer.DialogueRenderer.RenderScale++);

        private static int Frame { get; set; }

        private static IMouseInteractionControl DialogueControl { get; }
        private static ISpriteRenderer DialogueRenderer { get; }
    }
}
