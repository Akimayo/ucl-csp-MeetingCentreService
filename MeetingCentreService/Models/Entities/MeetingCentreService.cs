using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System;
using System.Linq;
using System.Data.Entity;

namespace MeetingCentreService.Models.Entities
{
    /// <summary>
    /// Represents current session of working with MeetingCentres
    /// </summary>
    [XmlRoot]
    public class MeetingCentreService : INotifyPropertyChanged
    {
        /// <summary cref="MeetingCentreService">
        /// Last created MeetingCentreService
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public static MeetingCentreService Current;

        /// <summary cref="MeetingCentreService">
        /// Asynchronously loads a MeetingCentreService
        /// </summary>
        public static async void LoadService(string loadFrom)
        {
            new MeetingCentreService(await Data.CsvImporter.ReadFromFileAsync(loadFrom));
        }
        /// <summary cref="MeetingCentreService">
        /// Asynchronously loads a MeetingCentreService
        /// </summary>
        public static async Task<MeetingCentreService> LoadServiceAsync(string loadFrom, Data.DocumentFormat format)
        {
            MeetingCentreService service = null;
            try
            {
                switch (format)
                {
                    case Data.DocumentFormat.XML:
                        service = await Data.XmlIO.ParseXmlAsync(loadFrom);
                        service.FilePath = loadFrom;
                        break;
                    case Data.DocumentFormat.JSON:
                        service = await Data.JsonIO.ParseJsonAsync(loadFrom);
                        service.FilePath = loadFrom;
                        break;
                    case Data.DocumentFormat.CSVStyle:
                        service = new MeetingCentreService(await Data.CsvImporter.ReadFromFileAsync(loadFrom));
                        break;
                    default:
                        throw new InvalidEnumArgumentException();
                }
            }
            catch (System.IO.IOException e)
            {
                MessageBox.Show(e.Message, "Failed opening file", MessageBoxButton.OK);
            }
            catch (JsonException e)
            {
                MessageBox.Show(e.Message, "Failed importing JSON file", MessageBoxButton.OK);
            }
            return service;
        }

        /// <summary>
        /// Save path for current session
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public string FilePath { get; set; }
        /// <summary>
        /// Indicates whether any changes have been made during this session
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public bool ServiceChanged { get; private set; }
        /// <summary>
        /// Indicates whether any changes have been made to the Accessories database
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public bool AccessoriesChanged { get; private set; }
        /// <summary cref="MeetingCentre">
        /// Collection of MeetingCentres for current session 
        /// </summary>
        [JsonProperty]
        [XmlArray]
        public ObservableCollection<MeetingCentre> MeetingCentres { get; }
        /// <summary>
        /// Databse context for the current session
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public Data.AccessoryContext AccessoriesContext { get; private set; }

        /// <summary>
        /// Create a new session
        /// </summary>
        public MeetingCentreService()
        {
            this.MeetingCentres = new ObservableCollection<MeetingCentre>();
            this.MeetingCentres.CollectionChanged += CentresCollectionChanged;
            this.ServiceChanged = false;
            MeetingCentreService.Current = this;
            this.LoadFromContext();
        }

        /// <summary>
        /// Create a session from an imported MeetingCentre collection
        /// </summary>
        /// <param name="import"></param>
        public MeetingCentreService(IList<MeetingCentre> import)
        {
            foreach (MeetingCentre centre in import)
                centre.PropertyChanged += CentreChanged;
            this.MeetingCentres = new ObservableCollection<MeetingCentre>(import);
            this.MeetingCentres.CollectionChanged += CentresCollectionChanged;
            this.CentresCollectionChanged(this, new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Add, this.MeetingCentres));
            this.ServiceChanged = false;
            MeetingCentreService.Current = this;
            this.LoadFromContext();
        }

        /// <summary>
        /// Loads Categories and Accessories from database context
        /// and attaches event handlers
        /// </summary>
        private void LoadFromContext()
        {
            this.AccessoriesContext = new Data.AccessoryContext();
            this.AccessoriesContext.CategorySet.Load();
            this.AccessoriesContext.AccessorySet.Include("Category").Load();
            this.AccessoriesContext.AccessorySet.Local.CollectionChanged += AccessoriesCollectionChanged;
            foreach(Accessory accessory in this.AccessoriesContext.AccessorySet.Local)
                accessory.PropertyChanged += this.AccessoryChanged;
        }

        /// <summary>
        /// Event handler for changes made to the AccessoriesFromContext and it's items
        /// </summary>
        private void AccessoryChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged("AccessoriesFromContext");
            this.AccessoriesChanged = true;
        }

        /// <summary>
        /// Event handler for changes made to the individual Accessories in AccessoriesFromContext
        /// </summary>
        private void AccessoriesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("AccessoriesFromContext");
            if (e.NewItems != null)
                foreach (Accessory accessory in e.NewItems)
                    accessory.PropertyChanged += this.AccessoryChanged;
            if (e.OldItems != null)
                foreach (Accessory accessory in e.OldItems)
                    accessory.PropertyChanged -= this.AccessoryChanged;
            this.AccessoriesChanged = true;
        }

        /// <summary>
        /// Event handler for changes made to the MeetingCentres collection and its items
        /// </summary>
        private void CentresCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("MeetingCentres");
            if (e.NewItems != null)
                foreach (MeetingCentre centre in e.NewItems)
                {
                    centre.PropertyChanged += CentreChanged;
                    foreach (MeetingRoom room in centre.MeetingRooms)
                        room.PropertyChanged += RoomChanged;
                }
            if (e.OldItems != null)
                foreach (MeetingCentre centre in e.OldItems)
                {
                    centre.PropertyChanged -= CentreChanged;
                    foreach (MeetingRoom room in centre.MeetingRooms)
                        room.PropertyChanged -= RoomChanged;
                }
            this.ServiceChanged = true;
        }

        /// <summary>
        /// Tells the service that it has been saved.
        /// </summary>
        internal void Saved()
        {
            this.ServiceChanged = false;
        }
        /// <summary>
        /// Tells the service that Accessories database has been saved.
        /// </summary>
        internal void DatabaseSaved()
        {
            this.AccessoriesChanged = false;
        }

        /// <summary>
        /// Event handler for changes made to MeetingRoom items
        /// </summary>
        private void RoomChanged(object sender, PropertyChangedEventArgs e)
        {
            this.ServiceChanged = true;
        }

        /// <summary>
        /// Event handler for changes made to MeetingCentre items
        /// </summary>
        private void CentreChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged("MeetingCentres");
            this.ServiceChanged = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler evt = this.PropertyChanged;
            if (evt != null) evt(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
