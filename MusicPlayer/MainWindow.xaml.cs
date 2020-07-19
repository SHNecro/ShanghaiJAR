﻿using MapEditor.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MusicPlayerViewModel();
        }

        private class MusicPlayerViewModel
        {
            public MusicPlayerViewModel()
            {
                // IF IN DEBUG MODE, WILL HAVE ODD INVALIDATE BEHAVIOR
                this.BGMDataViewModel = new BGMDataViewModel();
                this.BGMDataViewModel.LoadFromFile();
            }

            public BGMDataViewModel BGMDataViewModel { get; }

            public void Remove()
            {
                this.BGMDataViewModel.Remove();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            (this.DataContext as MusicPlayerViewModel)?.Remove();
        }
    }
}
