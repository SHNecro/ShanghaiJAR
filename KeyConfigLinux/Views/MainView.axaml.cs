using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using Common.Config;
using KeyConfigLinux.Converters;
using KeyConfigLinux.ViewModels;
using MessageBox.Avalonia.Enums;

namespace KeyConfigLinux.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }
    }
}
