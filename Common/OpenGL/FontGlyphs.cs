using SharpFont;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Common.OpenGL
{
    public static class FontGlyphs
    {
        private static Library TextLibrary;
        private static string FallbackFont = "MSGOTHIC.TTC";
        private static byte[] FallbackFontBytes;
        private static Dictionary<Font, Face> FallbackTextFaces;

        public static Dictionary<string, GlyphMetrics> TextMetrics;
        public static Dictionary<Font, Face> TextFaces;
        // Face heights caches due to memory leak accessing Face.Size
        public static Dictionary<Font, Fixed26Dot6> TextFaceHeights;
        public static string FontOverride;

        static FontGlyphs()
        {
            FontGlyphs.TextMetrics = new Dictionary<string, GlyphMetrics>();
            FontGlyphs.TextLibrary = new Library();
            FontGlyphs.TextFaces = new Dictionary<Font, Face>();
            FontGlyphs.TextFaceHeights = new Dictionary<Font, Fixed26Dot6>();
            FontGlyphs.FallbackTextFaces = new Dictionary<Font, Face>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fallbackFontResourceName = assembly.GetManifestResourceNames().First(s => s.EndsWith(FontGlyphs.FallbackFont, StringComparison.Ordinal));
            using (var stream = assembly.GetManifestResourceStream(fallbackFontResourceName))
            {
                FontGlyphs.FallbackFontBytes = new byte[stream.Length];
                stream.Read(FontGlyphs.FallbackFontBytes, 0, FontGlyphs.FallbackFontBytes.Length);
            }
        }

        public static GlyphSlot GetOrSetGlyphMetrics(Text text, int i)
        {
            if (!FontGlyphs.TextFaces.TryGetValue(text.Font, out var face))
            {
                var windowFontFamily = new System.Windows.Media.FontFamily(text.Font.FontFamily.Name);
                var typeface = new Typeface(windowFontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
                if (typeface.TryGetGlyphTypeface(out var glyphTypeface))
                {
                    face = new Face(FontGlyphs.TextLibrary, glyphTypeface.FontUri.AbsolutePath);
                    face.SetCharSize(0, text.Font.SizeInPoints * 64, 1, 1);
                    FontGlyphs.TextFaces[text.Font] = face;
                    FontGlyphs.TextFaceHeights[text.Font] = face.Size.Metrics.Height;
                }
                else if (FontGlyphs.FontOverride != null)
                {
                    face = new Face(FontGlyphs.TextLibrary, FontGlyphs.FontOverride);
                    face.SetCharSize(0, text.Font.SizeInPoints * 64, 1, 1);
                    FontGlyphs.TextFaces[text.Font] = face;
                    FontGlyphs.TextFaceHeights[text.Font] = face.Size.Metrics.Height;
                }
            }

            if (/*face == null || */face.GetCharIndex(text.Content[i]) == 0)
            {
                if (!FontGlyphs.FallbackTextFaces.TryGetValue(text.Font, out face))
                {
                    face = new Face(FontGlyphs.TextLibrary, FontGlyphs.FallbackFontBytes, 1);
                    face.SetCharSize(0, text.Font.SizeInPoints * 64, 1, 1);
                    FontGlyphs.FallbackTextFaces[text.Font] = face;
                    //FontGlyphs.TextFaces[text.Font] = face;
                    //FontGlyphs.TextFaceHeights[text.Font] = face.Size.Metrics.Height;
                }
            }

            face.LoadChar(text.Content[i], LoadFlags.Default, LoadTarget.Mono);

            var glyphSlot = face.Glyph;
            glyphSlot.RenderGlyph(RenderMode.Mono);
            FontGlyphs.TextMetrics[text.GetCharKey(i)] = glyphSlot.Metrics;
            return glyphSlot;
        }
    }
}
