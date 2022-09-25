using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Config;
using KeyConfig.WinForms.Controls;
using KeyConfigLinux.Converters;
using OpenTK;

namespace KeyConfig.WinForms
{
    public partial class KeyConfig : Form
    {
        private static RegionToTranslatedConverter TranslationConverter = new RegionToTranslatedConverter();

        private Config config;
        private IList<Action<string>> translations = new List<Action<string>>();
        public KeyConfig()
        {
            this.config = Config.FromCFG("option.cfg") ?? Config.FromXML("option.xml") ?? new Config();

            var oldConfig = Config.FromCFG("option.cfg");
            if (oldConfig != null)
            {
                if (Config.FromXML("option.xml") != null)
                {
                    var promptMessage = (string)new RegionToTranslatedConverter().Convert(oldConfig.Language, null, "ImportOverwritePrompt", null);
                    var result = MessageBox.Show(promptMessage, "KeyConfig", MessageBoxButtons.YesNo);
                    switch (result)
                    {
                        case DialogResult.Yes:
                            oldConfig.ToXML("option.xml");
                            File.Move("option.cfg", "option.cfg.OLD");
                            break;
                        case DialogResult.No:
                            File.Move("option.cfg", "option.cfg.OLD");
                            break;
                        case DialogResult.Cancel:
                            this.config = Config.FromXML("option.xml") ?? new Config();
                            break;
                    }
                }
            }
            
            InitializeComponent();

            this.Size = this.MaximumSize = this.MinimumSize = new Size(300, 480);
            this.Padding = new Padding(4);
            this.MaximizeBox = false;
            this.SetStyle(ControlStyles.FixedHeight | ControlStyles.FixedWidth, true);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            var assembly = Assembly.GetExecutingAssembly();
            var iconResourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(s => s.EndsWith("KeyConfig.ico"));
            var iconStream = assembly.GetManifestResourceStream(iconResourceName);
            this.Icon = new Icon(iconStream);
            
            var inputEnabler = new GLControl();
            inputEnabler.Visible = false;
            inputEnabler.Size = Size.Empty;
            this.Controls.Add(inputEnabler);
            
            this.RegisterTranslation(val => this.Text = val, "KeyConfig");

            #region TabControl

            var tabPanel = new TabControl();
            tabPanel.Dock = DockStyle.Fill;
            tabPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            #region Config Tab

            var configPage = new TabPage();
            this.RegisterTranslation(val => configPage.Text = val, "Config");

            #region Screen Size
            
            var screenSizeGroupBox = new GroupBox();
            screenSizeGroupBox.Location = new Point(0, 0);
            screenSizeGroupBox.Size = new Size(150, 120);
            this.RegisterTranslation(val => screenSizeGroupBox.Text = val, "ScreenSize");
            var screenSizesPanel = new FlowLayoutPanel();
            screenSizesPanel.FlowDirection = FlowDirection.TopDown;
            screenSizesPanel.WrapContents = false;
            screenSizesPanel.Dock = DockStyle.Top;
            var x1Option = new RadioButton();
            x1Option.Width = screenSizesPanel.Width;
            x1Option.Margin = new Padding(0, 0, 0, -2);
            x1Option.Height = 20;
            this.RegisterTranslation(val => x1Option.Text = val, "Sizex1");
            var x2Option = new RadioButton();
            x2Option.Width = screenSizesPanel.Width;
            x2Option.Margin = new Padding(0, 0, 0, -2);
            x2Option.Height = 20;
            this.RegisterTranslation(val => x2Option.Text = val, "Sizex2");
            var x3Option = new RadioButton();
            x3Option.Width = screenSizesPanel.Width;
            x3Option.Margin = new Padding(0, 0, 0, -2);
            x3Option.Height = 20;
            this.RegisterTranslation(val => x3Option.Text = val, "Sizex3");
            var x4Option = new RadioButton();
            x4Option.Width = screenSizesPanel.Width;
            x4Option.Margin = new Padding(0, 0, 0, -2);
            x4Option.Height = 20;
            this.RegisterTranslation(val => x4Option.Text = val, "Sizex4");
            var customOptionPanel = new FlowLayoutPanel();
            customOptionPanel.Width = screenSizesPanel.Width;
            customOptionPanel.Height = 30;
            customOptionPanel.WrapContents = false;
            customOptionPanel.FlowDirection = FlowDirection.LeftToRight;
            customOptionPanel.Padding = Padding.Empty;
            customOptionPanel.Margin = Padding.Empty;
            var customOption = new RadioButton();
            customOption.Width = 60;
            customOption.Margin = Padding.Empty;
            customOption.Height = 20;
            this.RegisterTranslation(val => customOption.Text = val, "SizeCustom");
            var customLeftParens = new Label();
            customLeftParens.Margin = Padding.Empty;
            customLeftParens.Width = 15;
            this.RegisterTranslation(val => customLeftParens.Text = val, "SizeCustomLeft");
            var customEntry = new TextBox();
            customEntry.Margin = new Padding(0, 0, 0, 0);
            customEntry.Width = 30;
            customEntry.Dock = DockStyle.Left;
            customEntry.Text = this.config.ScaleFactor.ToString(CultureInfo.InvariantCulture);
            var customRightParens = new Label();
            customRightParens.Margin = Padding.Empty;
            customRightParens.Width = 10;
            this.RegisterTranslation(val => customRightParens.Text = val, "SizeCustomRight");

            var scaleFactorOptions = new []
            {
                Tuple.Create<RadioButton, object>(x1Option, 1.0),
                Tuple.Create<RadioButton, object>(x2Option, 2.0),
                Tuple.Create<RadioButton, object>(x3Option, 3.0),
                Tuple.Create<RadioButton, object>(x4Option, 4.0),
                Tuple.Create<RadioButton, object>(customOption, new Func<object>(() => config.ScaleFactor)),
            };
            var scaleFactorChanging = false;
            var scaleFactorEventHandlerBase = this.CreateRadioButtonSelector(
                () => scaleFactorChanging,
                val => scaleFactorChanging = val,
                scaleFactorOptions,
                () => this.config.ScaleFactor,
                val => this.config.ScaleFactor = (double)val);
            
            var scaleFactorEventHandler = new EventHandler((sender, args) =>
            {
                scaleFactorEventHandlerBase(sender, args);
                customEntry.Text = this.config.ScaleFactor.ToString(CultureInfo.InvariantCulture);
            });
            foreach (var o in scaleFactorOptions)
            {
                if (o.Item1 == customOption)
                {
                    o.Item1.Checked = !scaleFactorOptions.Any(oo => oo.Item1 != customOption
                                                                    && Math.Abs((double)oo.Item2 - this.config.ScaleFactor) < 0.001);
                    continue;
                }

                o.Item1.Checked = Math.Abs(this.config.ScaleFactor - (double)((o.Item2 as Func<object>)?.Invoke() ?? o.Item2)) < 0.01;
                o.Item1.CheckedChanged += scaleFactorEventHandler;
            }
            
            customEntry.TextChanged += (sender, args) =>
            {
                if (scaleFactorChanging)
                {
                    return;
                }

                double proposedValue;
                if (!double.TryParse(customEntry.Text, out proposedValue) || proposedValue <= 0)
                {
                    customEntry.Text = this.config.ScaleFactor.ToString(CultureInfo.InvariantCulture);
                    return;
                }

                this.config.ScaleFactor = proposedValue;

                var anyChecked = false;
                foreach (var option in scaleFactorOptions)
                {
                    if (option.Item1 == customOption)
                    {
                        continue;
                    }
                    
                    option.Item1.Checked = Math.Abs((double)option.Item2 - proposedValue) < 0.001;
                    anyChecked |= option.Item1.Checked;
                }

                customOption.Checked = !anyChecked;
            };
            
            customOptionPanel.Controls.AddRange(new Control[] { customOption, customLeftParens, customEntry, customRightParens });
            screenSizesPanel.Controls.AddRange(new Control[] { x1Option, x2Option, x3Option, x4Option, customOptionPanel });
            screenSizeGroupBox.Controls.Add(screenSizesPanel);

            #endregion

            #region Windowed / Inactive
            
            var windowedGroupBox = new GroupBox();
            windowedGroupBox.Location = new Point(150, 0);
            windowedGroupBox.Size = new Size(120, 60);
            this.RegisterTranslation(val => windowedGroupBox.Text = val, "WindowedHeader");
            var windowedModesPanel = new FlowLayoutPanel();
            windowedModesPanel.FlowDirection = FlowDirection.TopDown;
            windowedModesPanel.WrapContents = false;
            windowedModesPanel.Dock = DockStyle.Top;
            windowedModesPanel.Height = 38;
            var windowedOption = new RadioButton();
            windowedOption.Width = windowedModesPanel.Width;
            windowedOption.Margin = new Padding(0, -2, 0, -1);
            windowedOption.Height = 20;
            this.RegisterTranslation(val => windowedOption.Text = val, "Windowed");
            var fullscreenOption = new RadioButton();
            fullscreenOption.Width = windowedModesPanel.Width;
            fullscreenOption.Margin = new Padding(0, 0, 0, -1);
            fullscreenOption.Height = 20;
            this.RegisterTranslation(val => fullscreenOption.Text = val, "Fullscreen");
            windowedModesPanel.Controls.AddRange(new Control[] { windowedOption, fullscreenOption });
            windowedGroupBox.Controls.Add(windowedModesPanel);
            
            var windowModeOptions = new [] { Tuple.Create<RadioButton, object>(windowedOption, false), Tuple.Create<RadioButton, object>(fullscreenOption, true) };
            var windowModeChanging = false;
            var windowModeEventHandler = this.CreateRadioButtonSelector(
                () => windowModeChanging,
                val => windowModeChanging = val,
                windowModeOptions,
                () => this.config.Fullscreen,
                val => this.config.Fullscreen = (bool)val);
            foreach (var o in windowModeOptions)
            {
                o.Item1.Checked = this.config.Fullscreen == (bool)o.Item2;
                o.Item1.CheckedChanged += windowModeEventHandler;
            }
            
            var inactiveWindowGroupBox = new GroupBox();
            inactiveWindowGroupBox.Location = new Point(150, 60);
            inactiveWindowGroupBox.Size = new Size(120, 60);
            this.RegisterTranslation(val => inactiveWindowGroupBox.Text = val, "InactiveHeader");
            var inactiveWindowPanel = new FlowLayoutPanel();
            inactiveWindowPanel.FlowDirection = FlowDirection.TopDown;
            inactiveWindowPanel.WrapContents = false;
            inactiveWindowPanel.Dock = DockStyle.Top;
            inactiveWindowPanel.Height = 38;
            var runningOption = new RadioButton();
            runningOption.Width = inactiveWindowPanel.Width;
            runningOption.Margin = new Padding(0, -2, 0, -1);
            runningOption.Height = 20;
            runningOption.Checked = !this.config.PausedWhenInactive;
            this.RegisterTranslation(val => runningOption.Text = val, "Running");
            var pausedOption = new RadioButton();
            pausedOption.Width = inactiveWindowPanel.Width;
            pausedOption.Margin = new Padding(0, 0, 0, -1);
            pausedOption.Height = 20;
            pausedOption.Checked = this.config.PausedWhenInactive;
            this.RegisterTranslation(val => pausedOption.Text = val, "Paused");
            
            var inactiveWindowOptions = new [] { Tuple.Create<RadioButton, object>(runningOption, false), Tuple.Create<RadioButton, object>(pausedOption, true) };
            var inactiveWindowChanging = false;
            var inactiveWindowEventHandler = this.CreateRadioButtonSelector(
                    () => inactiveWindowChanging,
                    val => inactiveWindowChanging = val,
                    inactiveWindowOptions,
                    () => this.config.PausedWhenInactive,
                    val => this.config.PausedWhenInactive = (bool)val);
            foreach (var o in inactiveWindowOptions)
            {
                o.Item1.Checked = this.config.PausedWhenInactive == (bool)o.Item2;
                o.Item1.CheckedChanged += inactiveWindowEventHandler;
            }
            
            inactiveWindowPanel.Controls.AddRange(new Control[] { runningOption, pausedOption });
            inactiveWindowGroupBox.Controls.Add(inactiveWindowPanel);

            #endregion

            #region Refresh Rate
            
            var refreshRateGroupBox = new GroupBox();
            refreshRateGroupBox.Location = new Point(0, 120);
            refreshRateGroupBox.Size = new Size(270, 60);
            this.RegisterTranslation(val => refreshRateGroupBox.Text = val, "RefreshRate");
            var refreshTypesPanel = new TableLayoutPanel();
            refreshTypesPanel.ColumnCount = 2;
            refreshTypesPanel.ColumnStyles.Add(new ColumnStyle {SizeType = SizeType.Percent, Width = 0.5f});
            refreshTypesPanel.ColumnStyles.Add(new ColumnStyle {SizeType = SizeType.Percent, Width = 0.5f});
            refreshTypesPanel.RowCount = 1;
            refreshTypesPanel.Dock = DockStyle.Top;
            refreshTypesPanel.Height = 35;
            
            var displayFpsPanel = new FlowLayoutPanel();
            displayFpsPanel.FlowDirection = FlowDirection.LeftToRight;
            displayFpsPanel.WrapContents = false;
            var displayLabel = new Label();
            this.RegisterTranslation(val => displayLabel.Text = val, "DisplayRate");
            displayLabel.Width = 45;
            displayLabel.TextAlign = ContentAlignment.MiddleLeft;
            var displayEntry = new TextBox();
            displayEntry.TextChanged += (sender, args) =>
            {
                int proposedValue;
                if (!int.TryParse(displayEntry.Text, out proposedValue) || proposedValue <= 0)
                {
                    displayEntry.Text = this.config.FPS.ToString();
                    return;
                }

                this.config.FPS = proposedValue;
            };
            displayEntry.Text = this.config.FPS.ToString();
            displayEntry.Width = 40;
            var displayUnitsLabel = new Label();
            displayUnitsLabel.Text = "fps";
            displayUnitsLabel.Width = 20;
            displayUnitsLabel.TextAlign = ContentAlignment.MiddleLeft;
            displayFpsPanel.Controls.AddRange(new Control[] { displayLabel, displayEntry, displayUnitsLabel });
            
            var turboUpsPanel = new FlowLayoutPanel();
            turboUpsPanel.FlowDirection = FlowDirection.LeftToRight;
            turboUpsPanel.WrapContents = false;
            var turboLabel = new Label();
            this.RegisterTranslation(val => turboLabel.Text = val, "TurboRate");
            turboLabel.Width = 45;
            turboLabel.TextAlign = ContentAlignment.MiddleLeft;
            var turboEntry = new TextBox();
            turboEntry.TextChanged += (sender, args) =>
            {
                int proposedValue;
                if (!int.TryParse(turboEntry.Text, out proposedValue) || proposedValue <= 0)
                {
                    turboEntry.Text = this.config.TurboUPS.ToString();
                    return;
                }

                this.config.TurboUPS = proposedValue;
            };
            turboEntry.Text = this.config.TurboUPS.ToString();
            turboEntry.Width = 40;
            var turboUnitsLabel = new Label();
            turboUnitsLabel.Text = "ups";
            turboUnitsLabel.Width = 20;
            turboUnitsLabel.TextAlign = ContentAlignment.MiddleLeft;
            turboUpsPanel.Controls.AddRange(new Control[] { turboLabel, turboEntry, turboUnitsLabel });
            
            refreshTypesPanel.Controls.Add(displayFpsPanel, 0, 0);
            refreshTypesPanel.Controls.Add(turboUpsPanel, 1, 0);
            refreshRateGroupBox.Controls.Add(refreshTypesPanel);

            #endregion

            #region Volume
            
            var bgmVolumeGroupBox = new GroupBox();
            bgmVolumeGroupBox.Location = new Point(0, 180);
            bgmVolumeGroupBox.Size = new Size(270, 50);
            this.RegisterTranslation(val => bgmVolumeGroupBox.Text = val, "BGMVolume");
            var bgmSlider = new TrackBar();
            bgmSlider.Minimum = 0;
            bgmSlider.Maximum = 100;
            bgmSlider.TickStyle = TickStyle.None;
            bgmSlider.Dock = DockStyle.Fill;
            bgmSlider.AutoSize = false;
            bgmSlider.Height = 20;
            var bgmLabel = new Label();
            bgmLabel.Dock = DockStyle.Right;
            bgmLabel.Width = 30;
            bgmSlider.ValueChanged += (sender, args) =>
            {
                this.config.VolumeBGM = bgmSlider.Value;
                bgmLabel.Text = this.config.VolumeBGM.ToString(CultureInfo.InvariantCulture);
            };
            bgmSlider.Value = (int)(this.config.VolumeBGM);
            bgmVolumeGroupBox.Controls.AddRange(new Control[] { bgmSlider, bgmLabel });
            
            var sfxVolumeGroupBox = new GroupBox();
            sfxVolumeGroupBox.Location = new Point(0, 230);
            sfxVolumeGroupBox.Size = new Size(270, 50);
            this.RegisterTranslation(val => sfxVolumeGroupBox.Text = val, "SFXVolume");
            var sfxSlider = new TrackBar();
            sfxSlider.Minimum = 0;
            sfxSlider.Maximum = 100;
            sfxSlider.TickStyle = TickStyle.None;
            sfxSlider.Dock = DockStyle.Fill;
            sfxSlider.AutoSize = false;
            sfxSlider.Height = 20;
            var sfxLabel = new Label();
            sfxLabel.Dock = DockStyle.Right;
            sfxLabel.Width = 30;
            sfxSlider.ValueChanged += (sender, args) =>
            {
                this.config.VolumeSE = sfxSlider.Value;
                sfxLabel.Text = this.config.VolumeSE.ToString(CultureInfo.InvariantCulture);
            };
            sfxSlider.Value = (int)(this.config.VolumeSE);
            sfxVolumeGroupBox.Controls.AddRange(new Control[] { sfxSlider, sfxLabel });

            #endregion
            
            #region Rendering
            
            var renderingGroupBox = new GroupBox();
            renderingGroupBox.Location = new Point(0, 280);
            renderingGroupBox.Size = new Size(135, 50);
            this.RegisterTranslation(val => renderingGroupBox.Text = val, "Rendering");
            var renderingComboBox = new ComboBox();
            renderingComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            renderingComboBox.Dock = DockStyle.Top;
            renderingComboBox.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            renderingComboBox.DisplayMember = "Item1";
            renderingComboBox.ValueMember = "Item2";
            var refreshItems = new Action<ComboBox.ObjectCollection, int, object>((items, maxItems, newItem) =>
            {
                if (items.Count >= maxItems)
                {
                    items.Clear();
                }

                items.Add(newItem);
            });
            this.RegisterTranslation(val => refreshItems(renderingComboBox.Items, 2, Tuple.Create(val, "OpenGL")), "OpenGL");
            this.RegisterTranslation(val => refreshItems(renderingComboBox.Items, 2, Tuple.Create(val, "DirectX9")), "DirectX9");
            var matchingRendering = renderingComboBox.Items.Cast<Tuple<string, string>>()
                                       .FirstOrDefault(tup => tup.Item2 == this.config.RenderEngine)
                                   ?? renderingComboBox.Items[0];
            renderingComboBox.SelectedItem = matchingRendering;
            renderingComboBox.SelectedValueChanged += (sender, args) =>
            {
                this.config.RenderEngine = (renderingComboBox.SelectedItem as Tuple<string, string>).Item2;
            };
            renderingGroupBox.Controls.Add(renderingComboBox);

            #endregion

            #region Audio

            var audioGroupBox = new GroupBox();
            audioGroupBox.Location = new Point(135, 280);
            audioGroupBox.Size = new Size(135, 50);
            this.RegisterTranslation(val => audioGroupBox.Text = val, "Audio");
            var audioComboBox = new ComboBox();
            audioComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            audioComboBox.Dock = DockStyle.Top;
            audioComboBox.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            audioComboBox.DisplayMember = "Item1";
            audioComboBox.ValueMember = "Item2";
            this.RegisterTranslation(val => refreshItems(audioComboBox.Items, 2, Tuple.Create(val, "OpenAL")), "OpenAL");
            this.RegisterTranslation(val => refreshItems(audioComboBox.Items, 2, Tuple.Create(val, "DirectSound")), "DirectSound");
            var matchingAudio = audioComboBox.Items.Cast<Tuple<string, string>>()
                                        .FirstOrDefault(tup => tup.Item2 == this.config.AudioEngine)
                                    ?? audioComboBox.Items[0];
            audioComboBox.SelectedItem = matchingAudio;
            audioComboBox.SelectedValueChanged += (sender, args) =>
            {
                this.config.AudioEngine = (audioComboBox.SelectedItem as Tuple<string, string>).Item2;
            };
            audioGroupBox.Controls.Add(audioComboBox);

            #endregion
            
            configPage.Controls.AddRange(new[] { screenSizeGroupBox, windowedGroupBox, inactiveWindowGroupBox, refreshRateGroupBox, bgmVolumeGroupBox, sfxVolumeGroupBox, renderingGroupBox, audioGroupBox });
            tabPanel.TabPages.Add(configPage);

            #endregion

            #region Keyboard Tab
            
            var keyboardPage = new TabPage();
            this.RegisterTranslation(val => keyboardPage.Text = val, "Keyboard");

            Func<bool, Tuple<string, Action<int>, Func<int>>[], TableLayoutPanel> buttonPanelMaker = (isKeyboard, options) =>
            {
                var keyPanel = new TableLayoutPanel();
                keyPanel.Dock = DockStyle.Fill;
                keyPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left;

                var i = 0;
                var labels = new List<Label>();
                var maxWidth = 0;
                foreach (var key in options)
                {
                    var keyHorizontalPanel = new Panel();
                    keyHorizontalPanel.Dock = DockStyle.Right;
                    keyHorizontalPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
                    keyHorizontalPanel.Height = 30;

                    var keyLabel = new Label();
                    this.RegisterTranslation(val => keyLabel.Text = val, key.Item1);
                    keyLabel.AutoSize = true;
                    var width = keyLabel.Width;
                    keyLabel.AutoSize = false;
                    keyLabel.TextAlign = ContentAlignment.MiddleLeft;
                    keyLabel.Dock = DockStyle.Left;
                    
                    var keyEntry = new EntryField();
                    keyEntry.IsKeyboardEntry = isKeyboard;
                    keyEntry.Dock = DockStyle.Right;
                    keyEntry.TextChanged += (sender, args) =>
                    {
                        key.Item2(keyEntry.KeyCode);
                    };
                    keyEntry.TabStop = true;
                    keyEntry.KeyCode = key.Item3.Invoke();
                    keyEntry.LastEntry = ReferenceEquals(key, options.Last());
                    keyHorizontalPanel.Height = keyEntry.Height;
                    
                    keyHorizontalPanel.Controls.AddRange(new Control[] { keyLabel, keyEntry });
                    keyPanel.Controls.Add(keyHorizontalPanel, 0, i);
                    keyPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    i++;

                    if (width > maxWidth)
                    {
                        maxWidth = keyLabel.Width;
                        foreach (var label in labels)
                        {
                            label.Width = maxWidth;
                        }
                    }
                    labels.Add(keyLabel);
                    keyHorizontalPanel.Width = keyPanel.Width - maxWidth;
                }

                return keyPanel;
            };

            var keyOptions = new[]
            {
                Tuple.Create<string, Action<int>, Func<int>>("KeyUp", val => this.config.KeyboardMapping.Up = val, () => this.config.KeyboardMapping.Up),
                Tuple.Create<string, Action<int>, Func<int>>("KeyRight", val => this.config.KeyboardMapping.Right = val, () => this.config.KeyboardMapping.Right),
                Tuple.Create<string, Action<int>, Func<int>>("KeyDown", val => this.config.KeyboardMapping.Down = val, () => this.config.KeyboardMapping.Down),
                Tuple.Create<string, Action<int>, Func<int>>("KeyLeft", val => this.config.KeyboardMapping.Left = val, () => this.config.KeyboardMapping.Left),
                Tuple.Create<string, Action<int>, Func<int>>("KeyA", val => this.config.KeyboardMapping.A = val, () => this.config.KeyboardMapping.A),
                Tuple.Create<string, Action<int>, Func<int>>("KeyB", val => this.config.KeyboardMapping.B = val, () => this.config.KeyboardMapping.B),
                Tuple.Create<string, Action<int>, Func<int>>("KeyL", val => this.config.KeyboardMapping.L = val, () => this.config.KeyboardMapping.L),
                Tuple.Create<string, Action<int>, Func<int>>("KeyR", val => this.config.KeyboardMapping.R = val, () => this.config.KeyboardMapping.R),
                Tuple.Create<string, Action<int>, Func<int>>("KeyStart", val => this.config.KeyboardMapping.Start = val, () => this.config.KeyboardMapping.Start),
                Tuple.Create<string, Action<int>, Func<int>>("KeySelect", val => this.config.KeyboardMapping.Select = val, () => this.config.KeyboardMapping.Select),
                Tuple.Create<string, Action<int>, Func<int>>("KeyTurbo", val => this.config.KeyboardMapping.Turbo = val, () => this.config.KeyboardMapping.Turbo ?? -4)
            };
            keyboardPage.Controls.Add(buttonPanelMaker(true, keyOptions));
            tabPanel.TabPages.Add(keyboardPage);

            #endregion

            #region GamePad Tab
            
            var gamepadPage = new TabPage();
            this.RegisterTranslation(val => gamepadPage.Text = val, "GamePad");
            
            var gamepadOptions = new[]
            {
                Tuple.Create<string, Action<int>, Func<int>>("KeyUp", val => this.config.ControllerMapping.Up = val, () => this.config.ControllerMapping.Up),
                Tuple.Create<string, Action<int>, Func<int>>("KeyRight", val => this.config.ControllerMapping.Right = val, () => this.config.ControllerMapping.Right),
                Tuple.Create<string, Action<int>, Func<int>>("KeyDown", val => this.config.ControllerMapping.Down = val, () => this.config.ControllerMapping.Down),
                Tuple.Create<string, Action<int>, Func<int>>("KeyLeft", val => this.config.ControllerMapping.Left = val, () => this.config.ControllerMapping.Left),
                Tuple.Create<string, Action<int>, Func<int>>("KeyA", val => this.config.ControllerMapping.A = val, () => this.config.ControllerMapping.A),
                Tuple.Create<string, Action<int>, Func<int>>("KeyB", val => this.config.ControllerMapping.B = val, () => this.config.ControllerMapping.B),
                Tuple.Create<string, Action<int>, Func<int>>("KeyL", val => this.config.ControllerMapping.L = val, () => this.config.ControllerMapping.L),
                Tuple.Create<string, Action<int>, Func<int>>("KeyR", val => this.config.ControllerMapping.R = val, () => this.config.ControllerMapping.R),
                Tuple.Create<string, Action<int>, Func<int>>("KeyStart", val => this.config.ControllerMapping.Start = val, () => this.config.ControllerMapping.Start),
                Tuple.Create<string, Action<int>, Func<int>>("KeySelect", val => this.config.ControllerMapping.Select = val, () => this.config.ControllerMapping.Select),
                Tuple.Create<string, Action<int>, Func<int>>("KeyTurbo", val => this.config.ControllerMapping.Turbo = val, () => this.config.ControllerMapping.Turbo ?? -4)
            };
            gamepadPage.Controls.Add(buttonPanelMaker(false, gamepadOptions));
            tabPanel.TabPages.Add(gamepadPage);

            #endregion

            #region Extra Tab
            
            var extraPage = new TabPage();
            this.RegisterTranslation(val => extraPage.Text = val, "Extra");
            
            tabPanel.TabPages.Add(extraPage);

            #endregion

            #endregion

            #region Language

            var languageGroupBox = new GroupBox();
            this.RegisterTranslation(val => languageGroupBox.Text = val, "Language");
            languageGroupBox.AutoSize = false;
            languageGroupBox.Dock = DockStyle.Bottom;
            languageGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            var languageBox = new ComboBox();
            languageBox.DropDownStyle = ComboBoxStyle.DropDownList;
            languageBox.Items.AddRange(RegionToTranslatedConverter.Locales.ToArray());
            languageBox.DisplayMember = nameof(Tuple<string, string>.Item1);
            languageBox.ValueMember = nameof(Tuple<string, string>.Item2);
            var matchingLanguage = languageBox.Items.Cast<Tuple<string, string>>()
                                       .FirstOrDefault(tup => tup.Item2 == this.config.Language)
                                   ?? languageBox.Items[0];
            languageBox.SelectedItem = matchingLanguage;
            languageBox.SelectedValueChanged += (sender, args) =>
            {
                this.config.Language = (languageBox.SelectedItem as Tuple<string, string>).Item2;
                foreach (var translationSetter in this.translations)
                {
                    translationSetter.Invoke(this.config.Language);
                }
            };
            languageBox.Dock = DockStyle.Top;
            languageBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            languageGroupBox.Height = 50;
            languageGroupBox.Controls.Add(languageBox);

            #endregion

            #region Save Button

            var saveButton = new Button();
            this.RegisterTranslation(val => saveButton.Text = val, "SaveChanges");
            saveButton.Dock = DockStyle.Bottom;
            saveButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            saveButton.Click += (sender, args) => this.SaveChanges();
            saveButton.Height = 30;

            #endregion
            
            this.Controls.Add(tabPanel);
            this.Controls.Add(languageGroupBox);
            this.Controls.Add(saveButton);
        }

        private EventHandler CreateRadioButtonSelector(Func<bool> changeFlagGetter, Action<bool> changeFlagSetter, Tuple<RadioButton, object>[] options, Func<object> optionGetter, Action<object> optionSetter)
        {
            return new EventHandler((sender, args) =>
            {
                if (changeFlagGetter())
                {
                    return;
                }

                changeFlagSetter(true);
                try
                {
                    var configOption = options.FirstOrDefault(o => ((o.Item2 as Func<object>)?.Invoke() ?? o.Item2).Equals(optionGetter()));
                    var checkedOption = options.FirstOrDefault(o => o.Item1.Checked);

                    if (checkedOption == null || configOption == null)
                    {
                        return;
                    }
                
                    if (checkedOption != configOption)
                    {
                        foreach (var o in options)
                        {
                            o.Item1.Checked = false;
                        }

                        checkedOption.Item1.Checked = true;
                        optionSetter(((checkedOption.Item2 as Func<object>)?.Invoke() ?? checkedOption.Item2));
                    }
                }
                finally
                {
                    changeFlagSetter(false);
                }

            });
        }
        
        private void SaveChanges()
        {
            this.config.ToXML("option.xml");
        }

        private void RegisterTranslation(Action<string> propertySetter, string key)
        {
            var setter = new Action<string>(lang => propertySetter(TranslationConverter.Convert(lang, key)));
            this.translations.Add(setter);
            setter(this.config.Language);
        }
    }
}
