using KeyConfigLinux.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Common.Config;
using KeyConfigLinux.Converters;
using MessageBox.Avalonia.Enums;

namespace KeyConfigLinux.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Config config;
        
        public MainViewModel()
        {
            this.Config = Config.FromCFG("option.cfg") ?? Config.FromXML("option.xml") ?? new Config();

            var oldConfig = Config.FromCFG("option.cfg");
            if (oldConfig != null)
            {
                if (Config.FromXML("option.xml") != null)
                {
                    var promptMessage = (string)new RegionToTranslatedConverter().Convert(oldConfig.Language, null, "ImportOverwritePrompt", null);
                    var box = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(promptMessage, "KeyConfig", ButtonEnum.YesNo);
                    box.Show().ContinueWith((task, obj) =>
                    {
                        var result = task.Result;
                        switch (result)
                        {
                            case ButtonResult.Yes:
                                oldConfig.ToXML("option.xml");
                                File.Move("option.cfg", "option.cfg.OLD");
                                break;
                            case ButtonResult.No:
                                File.Move("option.cfg", "option.cfg.OLD");
                                break;
                            case ButtonResult.Cancel:
                                this.Config = Config.FromXML("option.xml");
                                break;
                        }
                    }, null);
                }
            }

            this.SaveChangesCommand = new RelayCommand(this.SaveChanges);
        }

        public Config Config
        {
            get => this.config;
            private set
            {
                this.config = value;
                this.ConfigWrapper = new ConfigWrapperViewModel(this.config);
            }
        }

        public ConfigWrapperViewModel ConfigWrapper { get; private set; }
        
        public ICommand SaveChangesCommand { get; }

        public void SaveChanges(object param)
        {
            this.Config.ToXML("option.xml");
        }
    }
}
