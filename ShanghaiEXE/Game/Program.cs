using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace NSGame
{
    internal static class Program
    {
        public const string restartStopper = "openalrebootattempts";
        
        [STAThread]
        private static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                using (ShanghaiEXE form = new ShanghaiEXE())
                {
                    Loading loading = new Loading(form);
                    loading.Show();
                    new Thread(new ThreadStart(loading.MainLoop)).Start();
                    form.loading = loading;
                    form.InitializeData();
                    while (form.Created || loading.Created)
                        form.MainLoop();
                }
            }
            catch (Exception e) when (!System.Diagnostics.Debugger.IsAttached)
            {
                if (e.ToString().Contains("openal32.dll"))
                {
                    if (!File.Exists(restartStopper) || File.ReadAllText(restartStopper).Length < 5)
                    {
                        File.AppendAllText(restartStopper, "X");
                        System.Diagnostics.Process.Start(Application.ExecutablePath);
                        Application.Exit();
                        return;
                    }
                }
            
                if (Application.OpenForms.Count > 0)
                {
                    var messageText = e.ToString();
                    MessageBox.Show(messageText, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Environment.Exit(1);
            }
        }
    }
}
