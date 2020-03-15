using Common.Config;
using KeyConfig.Converters;
using System.IO;
using System.Windows;

namespace KeyConfig
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.Config = Config.FromCFG("option.cfg") ?? Config.FromXML("option.xml") ?? new Config();

            var oldConfig = Config.FromCFG("option.cfg");
            if (oldConfig != null)
            {
                if (Config.FromXML("option.xml") != null)
                {
                    var promptMessage = (string)new RegionToTranslatedConverter().Convert(oldConfig.Language, null, "ImportOverwritePrompt", null);
                    var result = MessageBox.Show(promptMessage, "KeyConfig", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            oldConfig.ToXML("option.xml");
                            File.Move("option.cfg", "option.cfg.OLD");
                            break;
                        case MessageBoxResult.No:
                            File.Move("option.cfg", "option.cfg.OLD");
                            break;
                        case MessageBoxResult.Cancel:
                            this.Config = Config.FromXML("option.xml");
                            break;
                    }
                }
            }

            InitializeComponent();
        }

        public Config Config { get; private set; }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            this.Config.ToXML("option.xml");
        }
    }
}
