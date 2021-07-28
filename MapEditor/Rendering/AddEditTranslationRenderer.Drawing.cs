using MapEditor.Core;
using Common.OpenGL;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System;
using NSShanghaiEXE.InputOutput.Audio;

namespace MapEditor.Rendering
{
    public static partial class AddEditTranslationRenderer
    {
        private static readonly List<char> ControlCharacters = new List<char> { 's', 'w', 'b', 'u', 'e', 'p', 'v', 'i', 'c' };

        public static void DrawDialogue()
        {
            if (AddEditTranslationRenderer.CurrentEntry?.Dialogue?.Face != null)
            {
                AddEditTranslationRenderer.DialogueRenderer.Draw(new Sprite
                {
                    Texture = "window",
                    Width = 240,
                    Height = 56,
                    TexX = 0,
                    TexY = 0,
                    Position = new Point(0, 0)
                }.WithTopLeftPosition(), 1);

                var face = AddEditTranslationRenderer.CurrentEntry.Dialogue.Face;
                AddEditTranslationRenderer.DialogueRenderer.Draw(new Sprite
                {
                    Texture = $"Face{face.Sheet}",
                    Width = 40,
                    Height = 48,
                    TexX = face.Mono ? (40 * 5) : 40 * 0,
                    TexY = 48 * face.Index,
                    Position = new Point(5, 4)
                }.WithTopLeftPosition(), 2);

                for (var i = 0; i < 3; i++)
                {
                    AddEditTranslationRenderer.DialogueRenderer.DrawText(new Text
                    {
                        Content = TrimValidateControlSequences(AddEditTranslationRenderer.CurrentEntry.Dialogue[i]),
                        Color = Constants.TextColor,
                        Font = Constants.Fonts[FontType.Normal],
                        Position = new Point(48 - 1, (16 * i))
                    }, 2);
                }

                AddEditTranslationRenderer.DialogueRenderer.Draw(new Sprite
                {
                    Texture = "window",
                    Width = 16,
                    Height = 16,
                    TexX = 240 + (16 * (AddEditTranslationRenderer.Frame % 3)),
                    TexY = 0,
                    Position = new Point(224, 36)
                }.WithTopLeftPosition(), 2);
            }

            AddEditTranslationRenderer.DialogueRenderer.Render();
        }

        // Handled here, CommandMessage:272, OpenGLRenderer/MySlimDG:ControlCharacterChange
        private static string TrimValidateControlSequences(string s)
        {
            var textBuilder = new StringBuilder();
            var paramBuilder = new StringBuilder();
            var controlCharacter = '0';
            var state = ControlSequenceState.Text;
            foreach (var c in s)
            {
                switch (state)
                {
                    case ControlSequenceState.Text:
                        if (c == '#')
                        {
                            state = ControlSequenceState.Delimiter;
                        }
                        else
                        {
                            textBuilder.Append(c);
                        }
                        break;
                    case ControlSequenceState.Delimiter:
                        if (!ControlCharacters.Contains(c))
                        {
                            return $"ERR: Bad control char '{c}'";
                        }
                        controlCharacter = c;
                        state = ControlSequenceState.Type;
                        break;
                    case ControlSequenceState.Type:
                        if (c != '@')
                        {
                            return $"ERR: Missing delimiter '@'";
                        }
                        state = ControlSequenceState.Parameter;
                        break;
                    case ControlSequenceState.Parameter:
                        if (c == '#')
                        {
                            var param = paramBuilder.ToString();
                            paramBuilder.Clear();

                            switch (controlCharacter)
                            {
                                case 's': // Sound effect
                                    if (!Enum.TryParse<SoundEffect>(param, out _))
                                    {
                                        return $"ERR: Bad soundeffect {param}";
                                    }
                                    break;
                                case 'w': // Wait
                                    if (!int.TryParse(param, out _))
                                    {
                                        return $"ERR: Wait time '{param}' not int";
                                    }
                                    break;
                                case 'b': // Fastforward
                                    if (!bool.TryParse(param, out _))
                                    {
                                        return $"ERR: Fastforward '{param}' not true/false";
                                    }
                                    break;
                                case 'u': // Save
                                    break;
                                case 'e': // End
                                    break;
                                case 'p': // Parallel Event Run
                                    break;
                                case 'v': // Print var
                                    if (!int.TryParse(param, out _))
                                    {
                                        return $"ERR: Val '{param}' not int";
                                    }
                                    textBuilder.Append("##");
                                    break;
                                case 'i': // Print recent item
                                    break;
                                case 'c': // Print recent category
                                    break;
                            }

                            state = ControlSequenceState.Text;
                        }
                        else
                        {
                            paramBuilder.Append(c);
                        }
                        break;
                }
            }

            if (state != ControlSequenceState.Text)
            {
                return "ERR: Unclosed '#' sequence";
            }

            return textBuilder.ToString();
        }

        private enum ControlSequenceState
        {
            Text,
            Delimiter,
            Type,
            Parameter
        }
    }
}
