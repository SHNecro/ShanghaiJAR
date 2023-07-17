using Common;
using ExtensionMethods;
using NSGame;
using Services;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NSShanghaiEXE.Game
{
    public partial class DialogueTester : Form
    {
        private ShanghaiEXE s;
        private Func<SceneBase> sceneGetter;
        private RichTextBox textBox;
        private Button sendButton;
        private Button reloadButton;

        public DialogueTester(ShanghaiEXE s, Func<SceneBase> sceneGetter)
        {
            this.s = s;
            this.sceneGetter = sceneGetter;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 160);
            this.Text = "DialogueTester";
            this.Icon = new Icon("icon.ico");
            this.textBox = new RichTextBox();
            this.sendButton = new Button();
            this.textBox.GotFocus += (sender, args) => {
                if (s.MyKeyBoard == null) return;
                s.MyKeyBoard.disabled = true;
            };
            this.textBox.LostFocus += (sender, args) => {
                if (s.MyKeyBoard == null) return;
                s.MyKeyBoard.disabled = false;
            };
            this.textBox.Anchor = AnchorStyles.Left;
            this.textBox.Dock = DockStyle.Fill;
            this.textBox.Multiline = true;
            this.textBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.sendButton.Anchor = AnchorStyles.Right;
            this.sendButton.Dock = DockStyle.Right;
            this.sendButton.MaximumSize = new Size(64, int.MaxValue);
            this.sendButton.Text = "SEND";
            this.sendButton.Click += (sender, args) => {
                var currScene = (this.sceneGetter() as SceneMain);
                if (currScene == null) return;
                try
                {
                    var eventmanager = currScene.mapscene.eventmanager;
                    eventmanager.events.Clear();
                    eventmanager.AddEvent(new NSEvent.OpenMassageWindow(s.ad, eventmanager));
                    var lines = this.textBox.Text.Split('\n');
                    foreach (var line in lines)
                    {
                        string text;
                        FaceId faceId;
                        var dlgMatch = Regex.Match(line, "<Dialogue Key=\"([^\"]+)\" Value=\"([^\"]+)\" Face=\"([^\"]+)\" Mono=\"([^\"]+)\" />");
                        var textMatch = Regex.Match(line, "<Text Key=\"([^\"]+)\" Value=\"([^\"]+)\" />");
                        if (dlgMatch.Success)
                        {
                            text = dlgMatch.Groups[2].ToString();
                            var mono = bool.Parse(dlgMatch.Groups[4].ToString());
							FACE face;
							if (Enum.TryParse<FACE>(dlgMatch.Groups[3].ToString(), out face))
							{
								faceId = face.ToFaceId(mono);
							}
							else
							{
								var manualFaceTokens = dlgMatch.Groups[3].ToString().Split(',');
								int sheet;
								byte index;
								if (manualFaceTokens.Length == 2
									&& int.TryParse(manualFaceTokens[0], out sheet)
									&& byte.TryParse(manualFaceTokens[1], out index))
								{
									faceId = new FaceId(sheet, index, mono);
								}
								else
								{
									faceId = FACE.None.ToFaceId(mono);
								}
							}
						}
                        else if (textMatch.Success)
                        {
                            text = textMatch.Groups[2].ToString();
                            faceId = FACE.SoundOnly.ToFaceId();
                        }
                        else
                        {
                            continue;
                        }
                        var dialogue = new Dialogue { Text = text, Face = faceId };
                        eventmanager.AddEvent(new NSEvent.CommandMessage(s.ad, eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, s.savedata));
                    }
                    eventmanager.AddEvent(new NSEvent.CloseMassageWindow(s.ad, eventmanager));
                }
                catch { }
            };
            this.reloadButton = new Button();
            this.reloadButton.Anchor = AnchorStyles.Bottom;
            this.reloadButton.Dock = DockStyle.Bottom;
            this.reloadButton.MaximumSize = new Size(int.MaxValue, 20);
            this.reloadButton.Text = "Reload";
            this.reloadButton.Click += (sender, args) =>
            {
                ShanghaiEXE.languageTranslationService = new LanguageTranslationService(ShanghaiEXE.Config.Language);
            };
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.reloadButton);
        }
    }
}
