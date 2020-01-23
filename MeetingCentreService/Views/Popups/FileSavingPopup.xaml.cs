using System;
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
    /// Interakční logika pro FileSavingPopup.xaml
    /// </summary>
    public partial class FileSavingPopup : Window
    {
        public string FileBasename { get; }
        public FileSavingPopup(string filePath)
        {
            this.DataContext = this;
            this.FileBasename = System.IO.Path.GetFileName(filePath);
            InitializeComponent();
        }
    }
}
