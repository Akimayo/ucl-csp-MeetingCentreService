using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MeetingCentreService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string SaveResourceKey = "MeetingCentreServiceLastSavedItem";
        public MainWindow()
        {
            System.Data.Entity.Database.SetInitializer(new Models.Data.AccessoryContext.AccessoriesInitializer());
            this.Closing += AppClosing;
            new Models.Entities.MeetingCentreService();
            InitializeComponent();
            if (Application.Current.Properties.Contains(SaveResourceKey))
            {
                string filePath = Application.Current.Properties[SaveResourceKey] as string;
                switch (System.IO.Path.GetExtension(filePath))
                {
                    case ".xml":
                        // Operates asynchronously, doesn't really matter, makes for a smoother app experience
                        this._import(filePath, Models.Data.DocumentFormat.XML);
                        break;
                    case ".json":
                        // Operates asynchronously, doesn't really matter, makes for a smoother app experience
                        this._import(filePath, Models.Data.DocumentFormat.JSON);
                        break;
                }
            }
        }

        private void AppClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Models.Entities.MeetingCentreService.Current.ServiceChanged || Models.Entities.MeetingCentreService.Current.AccessoriesChanged)
            {
                MessageBoxResult res = MessageBox.Show("You have unsaved changes. Do you want to save these changes before closing?", "Exit Meeting Centre Service", MessageBoxButton.YesNoCancel);
                switch (res)
                {
                    case MessageBoxResult.None:
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.Yes:
                        if (Models.Entities.MeetingCentreService.Current.ServiceChanged)
                            this.SaveCurrent(sender, e);
                        if (Models.Entities.MeetingCentreService.Current.AccessoriesChanged)
                            Models.Entities.MeetingCentreService.Current.AccessoriesContext.SaveChanges();
                        break;
                    default:
                        e.Cancel = false;
                        break;
                }
            }
            // Close database context properly
            if (!e.Cancel) Models.Entities.MeetingCentreService.Current.AccessoriesContext.Dispose();
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
                await this._import(fileDialog.FileName, format);
            }
        }

        private async Task _import(string fileName, Models.Data.DocumentFormat format)
        {
            Views.Popups.FileProcessingPopup popup = new Views.Popups.FileProcessingPopup(fileName);
            popup.Show();
            Models.Entities.MeetingCentreService service = await Models.Entities.MeetingCentreService.LoadServiceAsync(fileName, format);
            popup.Close();
            if (service != null)
            {
                ViewModels.CentresViewModel.Context.RefreshService();
                ViewModels.MeetingsViewModel.Context.ResetService();
            }
            else MessageBox.Show("The selected file doesn't exist anymore or is not in the correct format.", "Importing file", MessageBoxButton.OK);
        }

        private async void SaveCurrent(object sender, EventArgs e)
        {
            bool ignoreSaveFile = Models.Entities.MeetingCentreService.Current.AccessoriesChanged && !Models.Entities.MeetingCentreService.Current.ServiceChanged;
            if (Models.Entities.MeetingCentreService.Current.AccessoriesChanged)
            {
                await Models.Entities.MeetingCentreService.Current.AccessoriesContext.SaveChangesAsync();
                Models.Entities.MeetingCentreService.Current.DatabaseSaved();
            }
            if (!ignoreSaveFile) await this._save(e);
            if (Application.Current.Properties.Contains(SaveResourceKey)) Application.Current.Properties[SaveResourceKey] = Models.Entities.MeetingCentreService.Current.FilePath;
            else Application.Current.Properties.Add(SaveResourceKey, Models.Entities.MeetingCentreService.Current.FilePath);
        }

        private async Task _save(EventArgs e)
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
                else
                {
                    await Models.Entities.MeetingCentreService.Current.AccessoriesContext.SaveChangesAsync();
                    Models.Entities.MeetingCentreService.Current.DatabaseSaved();
                }
            }
            else
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "JSON Save File (*.json)|*.json|XML Save File (*.xml)|*.xml";
                dialog.ShowDialog();
                if (dialog.FileName != null) Models.Entities.MeetingCentreService.Current.FilePath = dialog.FileName;
                await this._save(e);
            }
        }

        private async void ExportCurrent(object sender, RoutedEventArgs e)
        {
            string savePath = Models.Entities.MeetingCentreService.Current.FilePath;
            Models.Entities.MeetingCentreService.Current.FilePath = null;
            await this._save(e);
            Models.Entities.MeetingCentreService.Current.FilePath = savePath;
        }
    }
}
