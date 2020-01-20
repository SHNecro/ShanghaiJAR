using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace NSGame
{
    public enum LoadType
    {
        Data,
        Device,
        Save,
        Graphics,
        Audio
    }

    public class Loading : Form
    {
        public event EventHandler LoadComplete;

        private const int ProgressStages = 6;
        private readonly IContainer components = null;
        private readonly NSGame.ShanghaiEXE form;
        private readonly Dictionary<LoadType, Label> loadingText;
        private readonly Dictionary<LoadType, int> loadingProgress;
        private ProgressBar loadingBar;
        private PictureBox splashPicture;

        public Loading(NSGame.ShanghaiEXE form)
        {
            this.form = form;
            this.loadingText = new Dictionary<LoadType, Label>();
            this.loadingProgress = Enum.GetNames(typeof(LoadType)).ToDictionary(n => (LoadType)Enum.Parse(typeof(LoadType), n), n => 0);
            this.InitializeComponent();
        }

        public void MainLoop()
        {
            try
            {
                this.loadingBar.Value = (int)Math.Ceiling(this.loadingProgress.Average(kvp => kvp.Value));
                if (this.loadingProgress.All(kvp => kvp.Value == 100))
                {
                    this.LoadComplete?.Invoke(this, null);
                }
            }
            catch
            {
            }
            Application.DoEvents();
        }


        public void UpdateLoadingText(LoadType type, int progress)
        {
            this.loadingProgress[type] = progress;
            this.loadingText[type].BackColor = progress != 100 ? Color.LightYellow : Color.LightGreen;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.loadingBar = new ProgressBar();
            var tableLayout = new TableLayoutPanel { ColumnCount = 5, RowCount = 1 };
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            this.splashPicture = new PictureBox();
            this.loadingText[LoadType.Data] = new Label { Text = ShanghaiEXE.Translate("Startup.LoadGameData"), Width = 80, TextAlign = ContentAlignment.MiddleCenter, Anchor = AnchorStyles.None };
            this.loadingText[LoadType.Device] = new Label { Text = ShanghaiEXE.Translate("Startup.LoadDevice"), Width = 80, TextAlign = ContentAlignment.MiddleCenter, Anchor = AnchorStyles.None };
            this.loadingText[LoadType.Save] = new Label { Text = ShanghaiEXE.Translate("Startup.LoadSave"), Width = 80, TextAlign = ContentAlignment.MiddleCenter, Anchor = AnchorStyles.None };
            this.loadingText[LoadType.Audio] = new Label { Text = ShanghaiEXE.Translate("Startup.LoadAudio"), Width = 80, TextAlign = ContentAlignment.MiddleCenter, Anchor = AnchorStyles.None };
            this.loadingText[LoadType.Graphics] = new Label { Text = ShanghaiEXE.Translate("Startup.LoadGraphics"), Width = 80, TextAlign = ContentAlignment.MiddleCenter, Anchor = AnchorStyles.None };
            ((ISupportInitialize)this.splashPicture).BeginInit();
            this.SuspendLayout();
            this.loadingBar.Location = new Point(12, 390);
            this.loadingBar.Maximum = 100;
            this.loadingBar.Name = "_LoadBer";
            this.loadingBar.Size = new Size(480, 23);
            this.loadingBar.TabIndex = 0;
            tableLayout.Location = new Point(12, 360);
            tableLayout.Size = new Size(480, 23);
            tableLayout.Controls.Add(this.loadingText[LoadType.Data], 0, 0);
            tableLayout.Controls.Add(this.loadingText[LoadType.Device], 1, 0);
            tableLayout.Controls.Add(this.loadingText[LoadType.Save], 2, 0);
            tableLayout.Controls.Add(this.loadingText[LoadType.Audio], 3, 0);
            tableLayout.Controls.Add(this.loadingText[LoadType.Graphics], 4, 0);
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith($"load02.jpg", StringComparison.InvariantCultureIgnoreCase));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                this.splashPicture.Image = Bitmap.FromStream(stream);
            }
            this.splashPicture.InitialImage = null;
            this.splashPicture.Location = new Point(12, 12);
            this.splashPicture.Name = "pictureBox1";
            this.splashPicture.Size = new Size(480, 360);
            this.splashPicture.TabIndex = 2;
            this.splashPicture.TabStop = false;
            this.Controls.Add(loadingBar);
            this.Controls.Add(tableLayout);
            this.Controls.Add(splashPicture);
            this.AutoScaleDimensions = new SizeF(6f, 12f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(507, 423);
            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = nameof(Loading);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = nameof(Loading);
            ((ISupportInitialize)this.splashPicture).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
