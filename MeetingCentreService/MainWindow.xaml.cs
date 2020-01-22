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
            this.Closing += AppClosing;
            InitializeComponent();
        }

        private void AppClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(Models.Entities.MeetingCentreService.Current.ServiceChanged)
            {
                MessageBoxResult res = MessageBox.Show("You have unsaved changes. Do you want to save these changes before closing?", "Exit Meeting Centre Service", MessageBoxButton.YesNoCancel);
                switch (res)
                {
                    case MessageBoxResult.None:
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.Yes:
                        // TODO: Save
                    default:
                        e.Cancel = false;
                        break;
                }
            }
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

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
