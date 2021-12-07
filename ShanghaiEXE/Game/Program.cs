using System;
using System.Threading;
using System.Windows.Forms;

namespace NSGame
{
    internal static class Program
    {
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
