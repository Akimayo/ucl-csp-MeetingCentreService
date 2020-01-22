using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MeetingCentreService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadCentres(object sender, RoutedEventArgs e)
        {
            (sender as Frame).Navigate(new Views.CentresView());
            (sender as Frame).Loaded -= LoadCentres;
        }

        private void LoadMeetings(object sender, RoutedEventArgs e)
        {
            (sender as Frame).Navigate(new Views.MeetingsView());
            (sender as Frame).Loaded -= LoadMeetings;
        }

        private void LoadAccessories(object sender, RoutedEventArgs e)
        {
            (sender as Frame).Navigate(new Views.AccessoriesView());
            (sender as Frame).Loaded -= LoadAccessories;
        }
    }
}
