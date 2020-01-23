﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MeetingCentreService.Views.Popups
{
    /// <summary>
    /// Interakční logika pro FileProcessingPopup.xaml
    /// </summary>
    public partial class FileProcessingPopup : Window
    {
        public string FileBasename { get; }
        public FileProcessingPopup(string filePath)
        {
            this.DataContext = this;
            this.FileBasename = System.IO.Path.GetFileName(filePath);
            InitializeComponent();
        }
    }
}