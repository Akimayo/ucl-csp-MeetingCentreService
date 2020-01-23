using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

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
            new Models.Entities.MeetingCentreService();
            InitializeComponent();
        }

        private void AppClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Models.Entities.MeetingCentreService.Current.ServiceChanged)
            {
                MessageBoxResult res = MessageBox.Show("You have unsaved changes. Do you want to save these changes before closing?", "Exit Meeting Centre Service", MessageBoxButton.YesNoCancel);
                switch (res)
                {
                    case MessageBoxResult.None:
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.Yes:
                        this.SaveCurrent(sender, e);
                        break;
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

        private void ImportCSV(object sender, RoutedEventArgs e)
        {
            this.ImportFile(Models.Data.DocumentFormat.CSVStyle);
        }

        private void ImportJSON(object sender, RoutedEventArgs e)
        {
            this.ImportFile(Models.Data.DocumentFormat.JSON);
        }

        private void ImportXML(object sender, RoutedEventArgs e)
        {
            this.ImportFile(Models.Data.DocumentFormat.XML);
        }

        private async void ImportFile(Models.Data.DocumentFormat format)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            switch (format)
            {
                case Models.Data.DocumentFormat.XML:
                    fileDialog.Filter = "XML Save Files (*.xml)|*.xml";
                    break;
                case Models.Data.DocumentFormat.JSON:
                    fileDialog.Filter = "JSON Save Files (*.json)|*.json";
                    break;
                case Models.Data.DocumentFormat.CSVStyle:
                    fileDialog.Filter = "CSV-style Legacy Save Files (*.csv)|*.csv";
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }
            if (fileDialog.ShowDialog() == true)
            {
                Views.Popups.FileProcessingPopup popup = new Views.Popups.FileProcessingPopup(fileDialog.FileName);
                popup.Show();
                Models.Entities.MeetingCentreService service = await Models.Entities.MeetingCentreService.LoadServiceAsync(fileDialog.FileName, format);
                popup.Close();
                if (service != null) ViewModels.CentresViewModel.Context.RefreshService();
                else MessageBox.Show("The selected file doesn't exist anymore or is not in the correct format.", "Importing file", MessageBoxButton.OK);
            }
        }

        private async void SaveCurrent(object sender, EventArgs e)
        {
            if (Models.Entities.MeetingCentreService.Current.FilePath != null)
            {
                Views.Popups.FileSavingPopup popup = new Views.Popups.FileSavingPopup(Models.Entities.MeetingCentreService.Current.FilePath);
                popup.Show();
                bool success = false;
                switch (System.IO.Path.GetExtension(Models.Entities.MeetingCentreService.Current.FilePath))
                {
                    case ".json":
                        success = await Models.Data.JsonIO.ExportJsonAsync();
                        break;
                    case ".xml":
                        success = await Models.Data.XmlIO.ExportXmlAsync();
                        break;
                }
                popup.Close();
                if (!success)
                {
                    MessageBox.Show("An error occured while saving the file", "Saving file", MessageBoxButton.OK);
                    if (e is CancelEventArgs) (e as CancelEventArgs).Cancel = true;
                }
            }
            else
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "JSON Save File (*.json)|*.json|XML Save File (*.xml)|*.xml";
                dialog.ShowDialog();
                if (dialog.FileName != null) Models.Entities.MeetingCentreService.Current.FilePath = dialog.FileName;
                this.SaveCurrent(sender, e);
            }
        }
    }
}
